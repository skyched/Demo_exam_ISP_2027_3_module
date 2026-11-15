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
        List<UC_Card> cardList = new List<UC_Card>();
        List<int> sales = new List<int>();
        public List<AbstractClassSales> abstractClassSales = new List<AbstractClassSales>();
        private string Sort;
        private string Cat;

        public MainForm()
        {
            InitializeComponent();
            labelShowCount.Visible = false;
            refresh();
            LoadDateCBCat();
        }
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
        private void buttonKorzinkaManuDescriptionSubcatOptimusPrime_Click(object sender, EventArgs e)
        {
            sales.Clear();
            cardList.Clear();
            OutSales();
        }




        public void OutSales()
        {
            flowLayoutPanel3.Controls.Clear();
            List<UC_Card> cards = Prod();
            for (int i = 0; i < abstractClassSales.Count; i++)
            {
                foreach (UC_Card product in cards)
                {
                    if (product.id == abstractClassSales[i].idProd)
                    { 
                        UC_Card salescard = new UC_Card()
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
                        };
                        salescard.SetMode(abstractClassSales[i].Sizes, abstractClassSales[i].Quantity);
                        salescard.SetLabel();
                        flowLayoutPanel3.Controls.Add(salescard);
                        break;
                    }
                }
            }
        }
        public void SetSizeAndQuant(AbstractClassSales sales)
        { 
            abstractClassSales.Add(sales);
        }


        private void refresh()
        {
            GetSales();
            OutProd();
            LoadDateCBCat();
        }



        //получение скидок
        private void GetSales()
        {
            sales.Clear();

            using (var conn = new NpgsqlConnection(Resources.conn))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand(@"
                    SELECT prod_id FROM public.orders
                    WHERE date_trunc('month', start_date) =
                          (SELECT date_trunc('month', max(start_date)) FROM orders)
                    EXCEPT
                    SELECT prod_id FROM public.orders
                    WHERE date_trunc('month', start_date) =
                          (SELECT date_trunc('month', max(start_date)) - interval '1 month' FROM orders)", conn))
                using (var rez = cmd.ExecuteReader())
                {
                    while (rez.Read())
                        sales.Add(rez.GetInt32(0));
                }
            }
        }



        //метод вывода карточк
        public void OutProd()
        {

            flowLayoutPanel3.Controls.Clear();
            List<UC_Card> cards = Prod();
            foreach (UC_Card product in cards)
            {
                flowLayoutPanel3.Controls.Add(product);
            }

        }




        //метод, который получает карты из бд
        private List<UC_Card> Prod()
        {

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
                    GROUP BY id_dubl, d_cat.title, image, prod.title, d_manu.title, consist, price, prod.description, d_scat.title", conn))
                using (var rez = cmd.ExecuteReader())
                {
                    while (rez.Read())
                    {
                        var id = rez.GetInt32(0);
                        var issales = sales.Contains(id);
                        var price = rez.GetDecimal(6);

                        var card1 = new UC_Card
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



        //получение товара по кнопке
        private void button1_Click(object sender, System.EventArgs e)
        {
            NpgsqlConnection.ClearAllPools();
            refresh();
        }


        //подгрузка данных в комбобокс
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

        //поисковик
        private void SearchFiltersAndSorting()
        {
            string searchText = textBox1.Text.Trim().ToLower();
            flowLayoutPanel3.SuspendLayout();
            foreach (var card in flowLayoutPanel3.Controls.OfType<UC_Card>())
            {

                bool matchesSearch = string.IsNullOrEmpty(searchText) ||
                                     card.name.ToLower().Contains(searchText);
                bool matchesCategory = string.IsNullOrEmpty(Cat) ||
                                       card.cat.Equals(Cat, StringComparison.OrdinalIgnoreCase);
                card.Visible = matchesSearch && matchesCategory;

            }
            if (comboBox1.SelectedIndex >= 0)
            {
                var allCards = flowLayoutPanel3.Controls.Cast<UC_Card>().ToList();

                if (comboBox1.SelectedIndex == 1)
                {
                    allCards = allCards.OrderByDescending(c => c.price2).ToList();
                }
                else
                {
                    allCards = allCards.OrderBy(c => c.price2).ToList();
                }

                for (int i = 0; i < allCards.Count; i++)
                {
                    flowLayoutPanel3.Controls.SetChildIndex(allCards[i], i);
                }
            }
            flowLayoutPanel3.ResumeLayout();
        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            Sort = comboBox1.SelectedIndex == -1 ? "" : comboBox1.Text;
            SearchFiltersAndSorting();
        }

        private void comboBox2_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            Cat = comboBox2.SelectedIndex == 0 ? "" : comboBox2.Text;
            SearchFiltersAndSorting();
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e) => SearchFiltersAndSorting();

       
    }
}