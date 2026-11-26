using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.Properties;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp1
{
    public partial class AuthorizationForm : Form
    {
        public AuthorizationForm()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (var conn = new NpgsqlConnection(Resources.conn))
            {
                conn.Open();

                var sql = @"SELECT ""user"".id, CONCAT(last_name, ' ', f_name, ' ',get_name) AS full_name,
                            role_id
	                            FROM public.""user""
	                            join public.d_role ON d_role.id = ""user"".role_id
	                        where login = @login";

                using (var cmd = new NpgsqlCommand(sql, conn))
                {

                    cmd.Parameters.AddWithValue("@login", textBox1.Text);

                    using (var rez = cmd.ExecuteReader())
                    {
                        if (rez.Read())
                        {
                            var mainform = new MainForm(rez.GetInt32(0), rez.GetString(1)); //передаем параметры
                            this.Hide();
                            mainform.ShowDialog();
                        }
                        else
                        {
                            MessageBox.Show("Пользователь не найден.\n\nЛибо Вы не заполнили поле.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }
    }
}