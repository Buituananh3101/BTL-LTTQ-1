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
    public partial class FormThemPhieuKiem : Form
    {
        private FormPhieuKiem formPK;

        // Khai báo chuỗi kết nối SQL Server chung
        private string connectionString = "Server=localhost\\SQLEXPRESS;Database=quanlimaytinh;Integrated Security=true;TrustServerCertificate=True;";

        public FormThemPhieuKiem(FormPhieuKiem form)
        {
            InitializeComponent();
            LoadmakhoToComboBox();
            LoadtennhanvienToComboBox();
            formPK = form;
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LoadmakhoToComboBox()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT maKho FROM kho";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        cbbMakho.Items.Clear();

                        while (reader.Read())
                        {
                            cbbMakho.Items.Add(reader["maKho"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load ComboBox: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadtennhanvienToComboBox()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT userName FROM account";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        cbbTen.Items.Clear();

                        while (reader.Read())
                        {
                            cbbTen.Items.Add(reader["userName"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load ComboBox: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            string maPhieu = txtMaphieu.Text.Trim();
            string maKho = cbbMakho.SelectedItem != null ? cbbMakho.SelectedItem.ToString() : string.Empty;
            string nguoiKiem = cbbTen.SelectedItem != null ? cbbTen.SelectedItem.ToString() : string.Empty;
            DateTime ngayKiem = dtpPK.Value;
            string trangThai = txtTrangthai.Text.Trim();
            string ghiChu = txtGhichu.Text.Trim();

            if (string.IsNullOrEmpty(maPhieu) || string.IsNullOrEmpty(maKho) || string.IsNullOrEmpty(nguoiKiem))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = @"
                INSERT INTO phieukiem (ma_phieu, ma_kho, nguoi_kiem, ngay_kiem, trang_thai, ghi_chu)
                VALUES (@maPhieu, @maKho, @nguoiKiem, @ngayKiem, @trangThai, @ghiChu);";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@maPhieu", maPhieu);
                    cmd.Parameters.AddWithValue("@maKho", maKho);
                    cmd.Parameters.AddWithValue("@nguoiKiem", nguoiKiem);
                    cmd.Parameters.AddWithValue("@ngayKiem", ngayKiem);
                    cmd.Parameters.AddWithValue("@trangThai", string.IsNullOrEmpty(trangThai) ? (object)DBNull.Value : trangThai);
                    cmd.Parameters.AddWithValue("@ghiChu", string.IsNullOrEmpty(ghiChu) ? (object)DBNull.Value : ghiChu);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Thêm phiếu kiểm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        formPK.napdgvphieukiem();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Không thể thêm phiếu kiểm!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Lỗi SQL khi thêm phiếu kiểm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormThemPhieuKiem_Load(object sender, EventArgs e)
        {

        }
    }
}