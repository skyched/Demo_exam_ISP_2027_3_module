using Npgsql;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using WindowsFormsApp1.Properties;

namespace WindowsFormsApp1
{
    public partial class AddForm : Form
    {
        public int idProd;

        List<int> quantity = new List<int>(); // тут будут храниться размеры (метод GetSizeANDQuan())

        public AddForm(CardUserControl cards)
        {
            InitializeComponent();
            this.idProd = cards.id;
            labelNameProd.Text = "Наименование товара: " + cards.name;
            labelProiz.Text = "Производитель: " + cards.manu;
            labelSostav.Text = "Состав: " + cards.consist;
            labelCat.Text = "Категория: " + cards.cat;
            labelOpis.Text = "Описание: " + cards.description;
            labelPodcat.Text = "Подкатегория: " + cards.subcat;
            labelPrice.Text = "Цена: " + Decimal.Round(cards.price2, 2).ToString() + " руб.";
            pictureBox1.ImageLocation = Path.Combine(Application.StartupPath, @"../../Properties/Images", cards.image);
            GetSizeANDQuan();
        }

        // получение размеров и количество доступных размеров
        public void GetSizeANDQuan()
        {
            var con = new NpgsqlConnection(Resources.conn);
            con.Open();
            string sql = $@"SELECT d_size.title, quantity_availabe
	                            FROM public.stock
	                            join public.d_size ON d_size.id = stock.size_id
	                            where prod_id = {idProd}";
            var cmd = new NpgsqlCommand(sql, con);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                comboBox1.Items.Add(reader.GetString(0));
                quantity.Add(reader.GetInt32(1));
            }
        }

        // логика с максимальным значением numericUpDown
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex >= 0)
            {
                int available = quantity[comboBox1.SelectedIndex];

                numericUpDown1.Maximum = available;
                numericUpDown1.Value = 0;
                button2.Enabled = true;
                labelWarning.Text = $"Количество товаров на складе: {numericUpDown1.Maximum}.";
            }
        }

        // проверка и добавление товара в корзину
        public void CheckTovar()
        {
            if (comboBox1.SelectedIndex == -1 && numericUpDown1.Value == 0)
            {
                MessageBox.Show("Пожалуйста, выберите размер и количество, доступное для заказа.",
                    "Информация", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (quantity[comboBox1.SelectedIndex] == 0)
            {
                MessageBox.Show("Выбранный размер данного товара закончился, пожалуйстa, выберите другой размер.",
                    "Информация", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (quantity[comboBox1.SelectedIndex] > 0 && numericUpDown1.Value == 0)
            {
                MessageBox.Show("Пожалуйста, выберите размер и количество, доступное для заказа",
                   "Информация", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                AbstractClassSales abstractClassSales = new AbstractClassSales(idProd, Convert.ToInt32(numericUpDown1.Value), Convert.ToString(comboBox1.SelectedItem));
                foreach (var item in Application.OpenForms)
                {
                    if (item is MainForm main)
                    {
                        main.ItemsInCart(abstractClassSales);
                        main.CountSales();
                        MessageBox.Show($"Товар добавлен в корзину: размер {comboBox1.SelectedItem}, количество — {numericUpDown1.Value} шт.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                        break;
                    }
                }
            }
        }

        // метод добавления товара в корзину с определенным размеров и кол-вом
        private void button2_Click(object sender, EventArgs e)
        {
            CheckTovar();
        }

        // метод закрытия формы
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}