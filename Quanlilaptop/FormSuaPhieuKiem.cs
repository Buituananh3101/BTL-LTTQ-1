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
    public partial class FormSuaPhieuKiem : Form
    {
        private FormPhieuKiem formSPK;

        // Khai báo chuỗi kết nối chung cho SQL Server
        private string connString = "Server=localhost\\SQLEXPRESS;Database=quanlimaytinh;Integrated Security=true;TrustServerCertificate=True;";

        public FormSuaPhieuKiem(FormPhieuKiem form)
        {
            InitializeComponent();
            LoadmakhoToComboBox();
            LoadtennhanvienToComboBox();
            LoadmaphieukiemToComboBox();
            formSPK = form;
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LoadmakhoToComboBox()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
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
                using (SqlConnection conn = new SqlConnection(connString))
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

        private void LoadmaphieukiemToComboBox()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    string query = "SELECT ma_phieu FROM phieukiem";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        cbbMaphieu.Items.Clear();

                        while (reader.Read())
                        {
                            cbbMaphieu.Items.Add(reader["ma_phieu"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load ComboBox: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (cbbMaphieu.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn mã phiếu cần sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maPhieu = cbbMaphieu.SelectedItem.ToString();
            string maKho = cbbMakho.SelectedItem != null ? cbbMakho.SelectedItem.ToString() : string.Empty;
            string nguoiKiem = cbbTen.SelectedItem != null ? cbbTen.SelectedItem.ToString() : string.Empty;
            DateTime ngayKiem = dtpPK.Value;
            string trangThai = txtTrangthai.Text.Trim();
            string ghiChu = txtGhichu.Text.Trim();

            if (string.IsNullOrEmpty(maKho) || string.IsNullOrEmpty(nguoiKiem) || string.IsNullOrEmpty(trangThai))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = @"
                UPDATE phieukiem
                SET ma_kho = @maKho, nguoi_kiem = @nguoiKiem, ngay_kiem = @ngayKiem, trang_thai = @trangThai, ghi_chu = @ghiChu
                WHERE ma_phieu = @maPhieu";

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@maPhieu", maPhieu);
                    cmd.Parameters.AddWithValue("@maKho", maKho);
                    cmd.Parameters.AddWithValue("@nguoiKiem", nguoiKiem);
                    cmd.Parameters.AddWithValue("@ngayKiem", ngayKiem);
                    cmd.Parameters.AddWithValue("@trangThai", trangThai);
                    cmd.Parameters.AddWithValue("@ghiChu", ghiChu);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Cập nhật phiếu kiểm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        formSPK.napdgvphieukiem();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Không thể cập nhật phiếu kiểm!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi sửa phiếu kiểm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cbbMaphieu_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbbMaphieu.SelectedItem != null)
            {
                string maPhieu = cbbMaphieu.SelectedItem.ToString();
                string query = @"
                    SELECT pk.ma_phieu, pk.ma_kho, pk.nguoi_kiem, pk.ngay_kiem, pk.trang_thai, pk.ghi_chu
                    FROM phieukiem pk
                    WHERE pk.ma_phieu = @maPhieu";

                try
                {
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@maPhieu", maPhieu);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                cbbMakho.SelectedItem = reader["ma_kho"].ToString();
                                cbbTen.SelectedItem = reader["nguoi_kiem"].ToString();
                                dtpPK.Value = Convert.ToDateTime(reader["ngay_kiem"]);
                                txtTrangthai.Text = reader["trang_thai"].ToString();
                                txtGhichu.Text = reader["ghi_chu"].ToString();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tải thông tin phiếu kiểm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}