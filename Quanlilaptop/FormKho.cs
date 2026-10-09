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
    public partial class FormKho : Form
    {
        // Khai báo chuỗi kết nối chung cho SQL Server
        private string connString = "Server=localhost\\SQLEXPRESS;Database=quanlimaytinh;Integrated Security=true;TrustServerCertificate=True;";

        public FormKho()
        {
            InitializeComponent();
            LoadnhanvienToComboBox();
            napdgvkho();
        }

        public void napdgvkho()
        {
            string sql = "SELECT     " +
                "maKho AS 'Mã kho',    " +
                "ten_kho AS 'Tên kho',     " +
                "dia_chi AS 'Địa chỉ',     " +
                "so_dien_thoai AS 'Số điện thoại',     " +
                "nguoi_quan_ly AS 'Người quản lý',     " +
                "trang_thai AS 'Trạng thái',    " +
                "ngay_cap_nhat AS 'Ngày cập nhật' FROM kho;";

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(sql, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable table = new DataTable();
                adapter.Fill(table);
                dgvKho.DataSource = table;
                dgvKho.Columns["Mã kho"].ReadOnly = true;
            }
        }

        private void LoadnhanvienToComboBox()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    string query = "SELECT user_name FROM nhanvien";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        cbbQuanly.Items.Clear();
                        while (reader.Read())
                        {
                            cbbQuanly.Items.Add(reader["user_name"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load ComboBox nhân viên quản lý: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            // none
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            string maKho = txtMakho.Text.Trim();
            string tenKho = txtTenkho.Text.Trim();
            string diaChi = txtDiachi.Text.Trim();
            string soDienThoai = txtSdt.Text.Trim();
            string nguoiQuanLy = cbbQuanly.SelectedItem?.ToString();
            string trangThai = txtTT.Text.Trim();
            DateTime ngayCapNhat = dtpKho.Value;

            if (string.IsNullOrEmpty(maKho) || string.IsNullOrEmpty(tenKho) ||
                string.IsNullOrEmpty(diaChi) || string.IsNullOrEmpty(soDienThoai) ||
                string.IsNullOrEmpty(nguoiQuanLy) || string.IsNullOrEmpty(trangThai))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ các trường!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string userName = nguoiQuanLy.Split('-')[0].Trim();

            string sql = @"
                INSERT INTO kho (maKho, ten_kho, dia_chi, so_dien_thoai, nguoi_quan_ly, trang_thai, ngay_cap_nhat)
                VALUES (@maKho, @tenKho, @diaChi, @soDienThoai, @nguoiQuanLy, @trangThai, @ngayCapNhat);
            ";

            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@maKho", maKho);
                    cmd.Parameters.AddWithValue("@tenKho", tenKho);
                    cmd.Parameters.AddWithValue("@diaChi", diaChi);
                    cmd.Parameters.AddWithValue("@soDienThoai", soDienThoai);
                    cmd.Parameters.AddWithValue("@nguoiQuanLy", userName);
                    cmd.Parameters.AddWithValue("@trangThai", trangThai);
                    cmd.Parameters.AddWithValue("@ngayCapNhat", ngayCapNhat);

                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("Thêm kho thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        napdgvkho();
                    }
                    else
                    {
                        MessageBox.Show("Thêm kho thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi thêm kho: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dgvKho_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                var row = dgvKho.Rows[e.RowIndex];
                if (row.IsNewRow) return;

                string maKho = row.Cells["Mã kho"].Value?.ToString();
                string tenKho = row.Cells["Tên kho"].Value?.ToString();
                string diaChi = row.Cells["Địa chỉ"].Value?.ToString();
                string soDienThoai = row.Cells["Số điện thoại"].Value?.ToString();
                string nguoiQuanLy = row.Cells["Người quản lý"].Value?.ToString();
                string trangThai = row.Cells["Trạng thái"].Value?.ToString();
                string ngayCapNhat = row.Cells["Ngày cập nhật"].Value?.ToString();
                DateTime ngayCapNhatDT = DateTime.TryParse(ngayCapNhat, out DateTime d) ? d : DateTime.Now;

                string sql = @"
                    UPDATE kho SET 
                        ten_kho = @tenKho,
                        dia_chi = @diaChi,
                        so_dien_thoai = @soDienThoai,
                        nguoi_quan_ly = @nguoiQuanLy,
                        trang_thai = @trangThai,
                        ngay_cap_nhat = @ngayCapNhat
                    WHERE maKho = @maKho
                ";

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@tenKho", tenKho ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@diaChi", diaChi ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@so_dien_thoai", soDienThoai ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@nguoiQuanLy", nguoiQuanLy ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@trangThai", trangThai ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@ngayCapNhat", ngayCapNhatDT);
                    cmd.Parameters.AddWithValue("@maKho", maKho ?? (object)DBNull.Value);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật kho: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}