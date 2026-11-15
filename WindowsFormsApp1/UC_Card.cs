using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class UC_Card : UserControl
    {
        public string Sizes;
        public int Quant;
        public UC_Card()
        {
            InitializeComponent();
            labelColvo.Visible = false;
            labelRazmer.Visible = false;
            button2.Visible = false;
        }
        public void SetMode(string size, int quant)
        {
            labelColvo.Visible = true;
            labelRazmer.Visible = true;
            button2.Visible = true;
            labelRazmer.Text = "Размеры: " + size;
            labelColvo.Text = "Количество: "+quant.ToString();
        }
        public int id;
        public string cat;
        public string image;
        public string name;
        public string manu;
        public string consist;
        public decimal price1;
        public decimal price2;
        public int totalquan;
        public string description;
        public string subcat;
        public void SetLabel()
        {

            label1.Text = $"{manu} | {name}";

            label2.Text = cat;

            label3.Text = totalquan >= 5 ? "Товаров на складе: много" : "Товаров на складе: мало";

            label4.Text = consist;

            label5.Text = $"{price1} руб.";

            label6.Text = $"{price2} руб.";

            if (price1 == 0)
            {

                label5.Visible = false;

            }
            else
            {
                label5.Font = new Font(label5.Font, FontStyle.Strikeout);

                label6.ForeColor = Color.Red;

                label6.Text = $"Скидка 25%: {price2} руб.";
            }

            if (totalquan <= 3)
            {
                this.BackColor = Color.LightPink;
            }

            pictureBox1.ImageLocation = Path.Combine(Application.StartupPath, "../../Properties/Images", image);
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            AddForm addForm = new AddForm(this);
            addForm.ShowDialog();
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            MainForm mainForm = (MainForm)Application.OpenForms["MainForm"];
            mainForm.abstractClassSales.RemoveAt(this.Parent.Controls.IndexOf(this));
            mainForm.CountSales();
            this.Parent.Controls.Remove(this);

        }
    }
}
