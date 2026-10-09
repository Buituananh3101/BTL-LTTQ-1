using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Quanlilaptop
{
    public partial class ThemTaiKhoan : Form
    {
        private FormAccount formAccount;

        public ThemTaiKhoan(FormAccount form)
        {
            InitializeComponent();
            formAccount = form;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sql = "CALL spThemTaiKhoan(@userName, @password, @email, @chucVu)";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@userName", txtUsername.Text);
            parameters.Add("@password", txtPassword.Text);
            parameters.Add("@email", txtEmail.Text);
            parameters.Add("@chucVu", txtChucVu.Text);

            try
            {
                using (MySqlConnection connection = new MySqlConnection("Server=localhost;Database=quanlimaytinh;Port=3306;User ID=root;Password="))
                {
                    connection.Open();
                    Database.Execute(sql, parameters); 
                }

                formAccount.napdgvtaikhoan();

                MessageBox.Show("Tài khoản đã được thêm thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            this.Close();
        }

        private void ThemTaiKhoan_Load(object sender, EventArgs e)
        {
            
        }
    }
}
