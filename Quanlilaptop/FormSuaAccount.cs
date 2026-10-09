using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quanlilaptop
{
    public partial class FormSuaAccount : Form
    {
        private FormAccount formAcc;
        public FormSuaAccount(FormAccount form)
        {
            InitializeComponent();
            LoadtaikhoanToComboBox();
            formAcc = form;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void LoadtaikhoanToComboBox()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection("Server=localhost;Database=quanlimaytinh;Port=3306;User ID=root;Password="))
                {
                    conn.Open();
                    string query = "SELECT userName FROM account";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        cbbAcc.Items.Clear();

                        while (reader.Read())
                        {
                            cbbAcc.Items.Add(reader["userName"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load ComboBox: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cbbAcc_SelectedIndexChanged(object sender, EventArgs e)
        {
            string userName = cbbAcc.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(userName)) return;

            using (MySqlConnection conn = new MySqlConnection("Server=localhost;Database=quanlimaytinh;Port=3306;User ID=root;Password="))
            {
                conn.Open();
                string sql = "SELECT password, email, chuc_vu FROM account WHERE userName = @userName";
                MySqlCommand command = new MySqlCommand(sql, conn);
                command.Parameters.AddWithValue("@userName", userName);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        txtPassword.Text = reader["password"].ToString();
                        txtEmail.Text = reader["email"].ToString();
                        txtChucVu.Text = reader["chuc_vu"].ToString();
                    }
                }
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            string userName = cbbAcc.Text.Trim();
            string password = txtPassword.Text.Trim();
            string email = txtEmail.Text.Trim();
            string chucVu = txtChucVu.Text.Trim();

            if (string.IsNullOrEmpty(userName))
            {
                MessageBox.Show("Vui lòng chọn tài khoản người dùng cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection("Server=localhost;Database=quanlimaytinh;Port=3306;User ID=root;Password="))
                {
                    conn.Open();
                    string sql = @"UPDATE account 
                           SET password = @password,
                               email = @email,
                               chuc_vu = @chucVu
                           WHERE userName = @userName";

                    MySqlCommand command = new MySqlCommand(sql, conn);
                    command.Parameters.AddWithValue("@password", password);
                    command.Parameters.AddWithValue("@email", email);
                    command.Parameters.AddWithValue("@chucVu", chucVu);
                    command.Parameters.AddWithValue("@userName", userName);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Cập nhật thông tin tài khoản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        formAcc.napdgvtaikhoan();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy tài khoản để cập nhật!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật tài khoản: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
