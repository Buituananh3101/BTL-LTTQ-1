using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Data;
namespace Quanlilaptop
{
    public partial class FormDangNhap : Form
    {
        public FormDangNhap()
        {
            InitializeComponent();
        }

        private static MySqlConnection connection = new MySqlConnection("Server=localhost;Database=quanlimaytinh;Port=3306;User ID=root;Password=");

        private void label4_Click(object sender, EventArgs e)
        {
            //none
        }
        private void label3_Click(object sender, EventArgs e)
        {
            //none
        }
        private void txtUsername_TextChanged(object sender, EventArgs e)
        {
            //none
        }
        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            //none
        }

        private void btnDangnhap_Click(object sender, EventArgs e)
        {
            string username, password;
            username = txtUsername.Text;
            password = txtPassword.Text;

            try
            {
                string query = "select * from account where userName = N'" + txtUsername.Text + "' and password = N'" + txtPassword.Text + "'";
                MySqlDataAdapter sda = new MySqlDataAdapter(query, connection);
                DataTable table = new DataTable();
                sda.Fill(table);
                if (table.Rows.Count > 0)
                {
                    username = txtUsername.Text;
                    password = txtPassword.Text;
                    MessageBox.Show("CHÀO MỪNG QUAY TRỞ LẠI!");
                    Form1 f = new Form1(table.Rows[0][0].ToString(), table.Rows[0][1].ToString(), table.Rows[0][2].ToString(), table.Rows[0][3].ToString());
                    f.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Đăng nhập thất bại, xin vui lòng thử lại !", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPassword.Clear();
                    txtUsername.Clear();
                    txtUsername.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            finally
            {
                connection.Close();
            }
        }
        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult res;
            res = MessageBox.Show("Bạn có muốn thoát không ?", "Thoát", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
            {
                Application.Exit();
            }
            else
            {
                this.Show();
            }
        }
    }
}
