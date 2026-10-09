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
    public partial class FormThemKhachHang : Form
    {
        private FormKhachHang formKH;

        // Khai báo chuỗi kết nối SQL Server chung
        private string connectionString = "Server=localhost\\SQLEXPRESS;Database=quanlimaytinh;Integrated Security=true;TrustServerCertificate=True;";

        public FormThemKhachHang(FormKhachHang form)
        {
            InitializeComponent();
            formKH = form;
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                string idKH = txtID.Text.Trim();
                string hoTen = txtHoten.Text.Trim();
                string sdt = txtSdt.Text.Trim();
                string diaChi = txtDiachi.Text.Trim();

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand("spThemKhachHang", conn);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@p_id_khach_hang", idKH);
                    command.Parameters.AddWithValue("@p_ho_ten", hoTen);
                    command.Parameters.AddWithValue("@p_sdt", sdt);
                    command.Parameters.AddWithValue("@p_dia_chi", diaChi);

                    command.ExecuteNonQuery();
                }

                MessageBox.Show("Thêm khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                formKH.napdgvkhachhang();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Lỗi SQL khi thêm khách hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.Close();
        }
    }
}