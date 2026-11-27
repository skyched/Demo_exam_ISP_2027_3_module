using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using WindowsFormsApp1.Properties;

namespace WindowsFormsApp1
{
    public partial class MainForm : Form
    {
        List<CardUserControl> cardList = new List<CardUserControl>(); // тут будут храниться карточки из БД

        List<int> sales = new List<int>(); // тут будут храниться айдишники товаров, на которые будут скидки

        public List<AbstractClassSales> abstractClassSales = new List<AbstractClassSales>(); // тут будут храниться карточки, которые юзер добавил в корзину

        private string Cat; // тут будет хранить значение категории

        public int id_user;


        //----------------------------------------------------------------------------------------------------

        public MainForm(int id, string user)
        {
            InitializeComponent();
            refresh();
            this.id_user = id;
            label4.Text = "Здравствуйте, " + user + "!";
            labelShowCount.Visible = false;
            buttonOrderConfirm.Visible = false;
        }

        //----------------------------------------------------------------------------------------------------


        //  ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ КОРЗИНА И ОФОРМЛЕНИЕ ЗАКАЗА ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓



        // логика циферки рядом с корзиной
        public void CountSales()
        {
            if (abstractClassSales.Count > 0)
            {
                labelShowCount.Visible = true;
                labelShowCount.Text = $"({abstractClassSales.Count})";
            }
            else if (abstractClassSales.Count == 0)
            {
                labelShowCount.Visible = false;
            }
        }

        // метод проверки события
        public void CheckSales()
        {
            if (abstractClassSales.Count == 0)
            {
                buttonOrderConfirm.Visible = false;
                OutSales();
                return;
            }
            else
            {
                buttonOrderConfirm.Visible = true;
                sales.Clear();
                cardList.Clear();
                OutSales();
            }
        }

        // метод-обрабочик события при нажатии на кнопку "Открыть корзину"
        private void buttonBasket_Click(object sender, EventArgs e)
        {
            CheckSales();
        }

        // метод-обрабочик события при нажатии на кнопку "Оформить заказ"
        private void buttonOrderConfirm_Click(object sender, EventArgs e)
        {
            InsertInfoOrderInDB();
            abstractClassSales.Clear();
            sales.Clear();
            cardList.Clear();
            CountSales();
            OutProd();
        }

        // метод, который записывает информацию о заказе в БД
        public void InsertInfoOrderInDB()
        {
            try
            {
                int nomerZakaza = 0;

                using (var con = new NpgsqlConnection(Resources.conn))
                {
                    con.Open();
                    var sql = @"SELECT MAX(num_of_order)
                            FROM public.orders;";
                    using (var cmd = new NpgsqlCommand(sql, con))
                    {
                        nomerZakaza = (int)cmd.ExecuteScalar() + 1; //ну здесь подумайте сами
                    }

                    using (var trans = con.BeginTransaction()) // поясню на парах
                    {
                        var sql1 = @"INSERT INTO public.orders(
                                num_of_order, start_date, user_id, cat_id, prod_id, manu_id, size_id, quantity, price_unit)
                                VALUES (
                                @nomerzak,
                                @date, 
                                @user, 
                                (select d_cat.id from d_cat where d_cat.title = @cat), 
                                @prodid, 
                                (select d_manu.id from d_manu where d_manu.title = @manu), 
                                (select d_size.id from d_size where d_size.title = @size),
                                @quan,
                                @price);";
                        using (var cmd1 = new NpgsqlCommand(sql1, con, trans))
                        {
                            foreach (CardUserControl prodsales in flowLayoutPanel3.Controls) //вытягиваем данные из динамического контейнера(потому что он тоже массив)
                            {
                                cmd1.Parameters.Clear();
                                cmd1.Parameters.AddWithValue("nomerzak", nomerZakaza);
                                cmd1.Parameters.AddWithValue("date", DateTime.UtcNow); // время сейчас с опереционной системы
                                cmd1.Parameters.AddWithValue("user", id_user);
                                cmd1.Parameters.AddWithValue("cat", prodsales.cat);
                                cmd1.Parameters.AddWithValue("prodid", prodsales.id);
                                cmd1.Parameters.AddWithValue("manu", prodsales.manu);
                                cmd1.Parameters.AddWithValue("size", prodsales.Sizes);
                                cmd1.Parameters.AddWithValue("quan", prodsales.Quant);
                                cmd1.Parameters.AddWithValue("price", prodsales.price2 * prodsales.Quant); // ВАЖНО! В БД УКАЗЫВАЕСЯ ЦЕНА ЗА ЕДИНИЦУ, НО ТУТ МЫ ПЕРЕМНОЖАЕМ
                                                                                                           // В ЦЕЛОМ, ЭТО ЛОГИЧНО, НО СТОИТ УТОЧНИТЬ.
                                cmd1.ExecuteNonQuery();
                            }
                        }
                        trans.Commit(); // поясню на парах
                        MessageBox.Show("Заказ успешно оформлен.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (PostgresException ex)
            {
                MessageBox.Show($"Внимание! Вы превысили лимит доступного количества товаров на складе, пожалуйста, проверьте заказ.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключение к БД: {ex.Message}", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        // Сохраняем в List товары, которые пользователь отметил для добавления в корзину 
        public void ItemsInCart(AbstractClassSales sales)
        {
            abstractClassSales.Add(sales);
        }

        // метод, который выводит данные во flowLayoutPanel о товарах, которые пользователь добавил в корзину
        public void OutSales()
        {
            flowLayoutPanel3.Controls.Clear();
            List<CardUserControl> cards = Prod();
            for (int i = 0; i < abstractClassSales.Count; i++)
            {
                foreach (CardUserControl product in cards)
                {
                    if (product.id == abstractClassSales[i].idProd)
                    {
                        CardUserControl salescard = new CardUserControl()
                        {
                            id = product.id,
                            name = product.name,
                            manu = product.manu,
                            cat = product.cat,
                            consist = product.consist,
                            description = product.description,
                            subcat = product.subcat,
                            price2 = product.price2,
                            image = product.image,
                            totalquan = product.totalquan,
                        };
                        salescard.SetMode(abstractClassSales[i].Sizes, abstractClassSales[i].Quantity);
                        salescard.SetLabel();
                        flowLayoutPanel3.Controls.Add(salescard);
                        break;
                    }
                }
            }
        }



        //  ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ВЫВОД КАРТОЧЕК ИЗ БД ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓



        //метод, который будет вызываться при нажатии кнопки "Вывести товары". Нужен для удобства, как по мне) если вам удобно без него, можете убрать)
        private void refresh()
        {
            GetSales();
            OutProd();
            LoadDateCBCat();

            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
        }

        // получение скидок
        private void GetSales()
        {
            sales.Clear();

            using (var conn = new NpgsqlConnection(Resources.conn))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand(@"
                    select id_dubl 
                    from prod
                    except
                    SELECT orders.prod_id
                     FROM public.orders
                     where date_trunc('month', start_date) =
                     (select date_trunc('month', max(start_date)) - interval '1 month' from orders)", conn))
                using (var rez = cmd.ExecuteReader())
                {
                    while (rez.Read())
                        sales.Add(rez.GetInt32(0));
                }
            }
        }

        // метод вывода карточек
        public void OutProd()
        {
            buttonOrderConfirm.Visible = false;

            List<CardUserControl> cards = Prod();

            flowLayoutPanel3.SuspendLayout();

            flowLayoutPanel3.Controls.Clear();

            foreach (CardUserControl product in cards)
            {
                flowLayoutPanel3.Controls.Add(product);
            }

            flowLayoutPanel3.ResumeLayout();
        }

        // метод, который получает карты из бд
        private List<CardUserControl> Prod()
        {

            cardList.Clear();

            using (var conn = new NpgsqlConnection(Resources.conn))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand(@"
                    SELECT id_dubl, d_cat.title, image, prod.title, d_manu.title,
                           consist, price, sum(stock.quantity_availabe), prod.description, d_scat.title
                    FROM public.prod
                    JOIN public.d_cat  ON d_cat.id  = prod.cat_id
                    JOIN public.d_manu ON d_manu.id = prod.manu_id
                    JOIN public.stock  ON stock.prod_id = prod.id_dubl
					join public.d_scat ON d_scat.id = prod.scat_id
                    GROUP BY id_dubl, d_cat.title, image, prod.title, d_manu.title, consist, price, prod.description, d_scat.title
order by prod.id_dubl", conn))
                using (var rez = cmd.ExecuteReader())
                {
                    while (rez.Read())
                    {
                        var id = rez.GetInt32(0);
                        var issales = sales.Contains(id);
                        var price = rez.GetDecimal(6);

                        var card1 = new CardUserControl
                        {
                            id = id,
                            cat = rez.GetString(1),
                            image = rez.IsDBNull(2) ? "picture.png" : rez.GetString(2),
                            name = rez.GetString(3),
                            manu = rez.GetString(4),
                            consist = rez.GetString(5),
                            price1 = issales ? price : 0,
                            price2 = issales ? price * 0.75m : price,
                            totalquan = rez.GetInt32(7),
                            description = rez.GetString(8),
                            subcat = rez.GetString(9)
                        };
                        cardList.Add(card1);
                        card1.SetLabel();
                    }
                }
            }
            return cardList;
        }

        // получение товара по кнопке
        private void button1_Click(object sender, System.EventArgs e)
        {
            NpgsqlConnection.ClearAllPools();
            refresh();
        }



        //  ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ПОИСКОВИК ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ ↓ 



        // подгрузка данных в комбобокс - категории
        private void LoadDateCBCat()
        {

            comboBox2.Items.Clear();

            comboBox2.Items.Add("Все категории");

            using (var conn = new NpgsqlConnection(Resources.conn))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand(@"
                            SELECT title
	                            FROM public.d_cat
                    ", conn))
                using (var rez = cmd.ExecuteReader())
                {
                    while (rez.Read())
                    {
                        comboBox2.Items.Add(rez.GetString(0));
                    }
                }
            }
        }

        // поисковик
        private void SearchFiltersAndSorting()
        {
            string searchText = textBox1.Text.Trim().ToLower();
            flowLayoutPanel3.SuspendLayout();
            foreach (var card in flowLayoutPanel3.Controls.OfType<CardUserControl>())
            {

                bool matchesSearch = string.IsNullOrEmpty(searchText) ||
                                     card.name.ToLower().Contains(searchText);
                bool matchesCategory = string.IsNullOrEmpty(Cat) ||
                                       card.cat.Equals(Cat, StringComparison.OrdinalIgnoreCase);
                card.Visible = matchesSearch && matchesCategory;

            }
            if (comboBox1.SelectedIndex >= 0)
            {
                var allCards = flowLayoutPanel3.Controls.Cast<CardUserControl>().ToList();

                if (comboBox1.SelectedIndex == 2)
                {
                    allCards = allCards.OrderByDescending(c => c.price2).ToList();
                }
                else if (comboBox1.SelectedIndex == 1)
                {
                    allCards = allCards.OrderBy(c => c.price2).ToList();
                }
                else
                {
                    allCards = allCards.OrderBy(c => c.id).ToList();
                }

                for (int i = 0; i < allCards.Count; i++)
                {
                    flowLayoutPanel3.Controls.SetChildIndex(allCards[i], i);
                }
            }
            flowLayoutPanel3.ResumeLayout();
        }

        // событие, когда вы выбираем что-то из категории
        private void comboBox2_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            Cat = comboBox2.SelectedIndex == 0 ? "" : comboBox2.Text;
            SearchFiltersAndSorting();
        }

        // событие, когда вы изменяем (набираем) текст в текстовом поле
        private void textBox1_TextChanged_1(object sender, EventArgs e) => SearchFiltersAndSorting();

        // событие, когда вы выбираем что-то из сортивки по цене
        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e) => SearchFiltersAndSorting();


        //----------------------------------------------------------------------------------------------------


        // выход на авторизацию
        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}