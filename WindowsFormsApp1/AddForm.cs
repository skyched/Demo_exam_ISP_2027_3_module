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
            labelWarning.Visible = false;
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

                if (available == 0)
                {
                    button2.Enabled = false;
                    numericUpDown1.Enabled = false;
                    
                    labelWarning.Visible = true;
                    labelWarning.Text = $"Выбранный размер закончился.";
                }
                else
                {
                    button2.Enabled = true;
                    numericUpDown1.Enabled = true;
                    numericUpDown1.Value = 1;

                    labelWarning.Visible = false;
                }
            }
        }

        // логика с максимальным значением numericUpDown 
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

            if (numericUpDown1.Value == numericUpDown1.Maximum)
            {
                labelWarning.Visible = true;
                labelWarning.Text = $"Количество товаров на складе: {numericUpDown1.Maximum}.";
            }
            else if (numericUpDown1.Value < numericUpDown1.Maximum)
            {
                labelWarning.Visible = false;
            }
        }

        // метод добавления товара в корзину с определенным размеров и кол-вом
        private void button2_Click(object sender, EventArgs e)
        {
            AbstractClassSales abstractClassSales = new AbstractClassSales(idProd, Convert.ToInt32(numericUpDown1.Value), Convert.ToString(comboBox1.SelectedItem));
            MainForm mainForm = (MainForm)Application.OpenForms["MainForm"];
            mainForm.ItemsInCart(abstractClassSales);
            mainForm.CountSales();
            MessageBox.Show($"Товар успешно добавлен в размере {comboBox1.SelectedItem} в количестве {numericUpDown1.Value}.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        // метод закрытия формы
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
