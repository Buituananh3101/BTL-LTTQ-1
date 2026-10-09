using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.SqlClient; // Thay thế thư viện MySQL bằng SQL Server

namespace Quanlilaptop
{
    public partial class FormSuaAccount : Form
    {
        private FormAccount formAcc;

        // Khai báo chuỗi kết nối chung cho SQL Server
        private string connString = "Server=localhost\\SQLEXPRESS;Database=quanlimaytinh;Integrated Security=true;TrustServerCertificate=True;";

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
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    string query = "SELECT userName FROM account"; //[cite: 1]

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
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

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                string sql = "SELECT password, email, chuc_vu FROM account WHERE userName = @userName"; //[cite: 1]
                SqlCommand command = new SqlCommand(sql, conn);
                command.Parameters.AddWithValue("@userName", userName);

                using (SqlDataReader reader = command.ExecuteReader())
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
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    string sql = @"UPDATE account 
                                   SET password = @password,
                                       email = @email,
                                       chuc_vu = @chucVu
                                   WHERE userName = @userName"; //[cite: 1]

                    SqlCommand command = new SqlCommand(sql, conn);
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