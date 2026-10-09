using System;
using System.Data;
using System.Windows.Forms;
using Microsoft.Data.SqlClient; // Sử dụng thư viện SQL Server

namespace Quanlilaptop
{
    public partial class FormThemNCC : Form
    {
        private FormNhaCungCap formNCC;

        // Khai báo chuỗi kết nối SQL Server
        private string connStr = "Server=localhost\\SQLEXPRESS;Database=quanlimaytinh;Integrated Security=true;TrustServerCertificate=True;";

        public FormThemNCC(FormNhaCungCap form)
        {
            InitializeComponent();
            formNCC = form;
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                string maNCC = txtMaNCC.Text.Trim();
                string tenNCC = txtTenNCC.Text.Trim();
                string sdt = txtSdtNCC.Text.Trim();
                string dia_chi = txtdia_chiNCC.Text.Trim();

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand("spThemNCC", conn);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@p_ma_nha_cung_cap", maNCC);
                    command.Parameters.AddWithValue("@p_ten_nha_cung_cap", tenNCC);
                    command.Parameters.AddWithValue("@p_Sdt", sdt);
                    command.Parameters.AddWithValue("@p_dia_chi", dia_chi);

                    command.ExecuteNonQuery();
                }

                MessageBox.Show("Thêm nhà cung cấp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (formNCC != null)
                {
                    formNCC.napdgvNhaCungCap();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Lỗi SQL khi thêm nhà cung cấp: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.Close();
        }
    }
}