using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.SqlClient; // Đổi sang thư viện SQL Server

namespace Quanlilaptop
{
    public partial class FormSuaNCC : Form
    {
        private FormNhaCungCap formNCC;
        private string connString = "Server=localhost\\SQLEXPRESS;Database=quanlimaytinh;Integrated Security=true;TrustServerCertificate=True;";

        public FormSuaNCC(FormNhaCungCap form)
        {
            InitializeComponent();
            LoadmanhacungcapToComboBox();
            formNCC = form;
        }

        private void LoadmanhacungcapToComboBox()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    string query = "SELECT ma_nha_cung_cap FROM nhacungcap";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        cbbNCC.Items.Clear();

                        while (reader.Read())
                        {
                            cbbNCC.Items.Add(reader["ma_nha_cung_cap"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load ComboBox: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbbNCC_SelectedIndexChanged(object sender, EventArgs e)
        {
            string maNCC = cbbNCC.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(maNCC)) return;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                string sql = "SELECT ten_nha_cung_cap, Sdt, dia_chi FROM nhacungcap WHERE ma_nha_cung_cap = @maNCC";
                SqlCommand command = new SqlCommand(sql, conn);
                command.Parameters.AddWithValue("@maNCC", maNCC);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    txtTenNCC.Text = reader["ten_nha_cung_cap"].ToString();
                    txtSdtNCC.Text = reader["Sdt"].ToString();
                    txtdia_chiNCC.Text = reader["dia_chi"].ToString();
                }
                reader.Close();
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            string maNCC = cbbNCC.Text.Trim();
            string tenNCC = txtTenNCC.Text.Trim();
            string sdt = txtSdtNCC.Text.Trim();
            string diaChi = txtdia_chiNCC.Text.Trim();

            if (string.IsNullOrEmpty(maNCC))
            {
                MessageBox.Show("Vui lòng chọn mã nhà cung cấp cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    string sql = @"UPDATE nhacungcap 
                           SET ten_nha_cung_cap = @tenNCC,
                               Sdt = @sdt,
                               dia_chi = @diaChi
                           WHERE ma_nha_cung_cap = @maNCC";

                    SqlCommand command = new SqlCommand(sql, conn);
                    command.Parameters.AddWithValue("@tenNCC", tenNCC);
                    command.Parameters.AddWithValue("@sdt", sdt);
                    command.Parameters.AddWithValue("@diaChi", diaChi);
                    command.Parameters.AddWithValue("@maNCC", maNCC);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Cập nhật nhà cung cấp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        formNCC.napdgvNhaCungCap();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy nhà cung cấp để cập nhật!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật nhà cung cấp: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}