using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Data.SqlClient; // Đổi sang thư viện SQL Server

namespace Quanlilaptop
{
    public partial class FormChiTietPhieuNhap : Form
    {
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HTCAPTION = 0x2;

        // Khai báo chuỗi kết nối SQL Server
        private string connString = "Server=localhost\\SQLEXPRESS;Database=quanlimaytinh;Integrated Security=true;TrustServerCertificate=True;";

        public FormChiTietPhieuNhap()
        {
            InitializeComponent();
            LoadmakhoToComboBox();
            LoadMamayToComboBox();
            LoadMaPhieuToComboBox();
            this.MouseDown += new MouseEventHandler(FormChiTietPhieuNhap_MouseDown);
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {

        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            string maPhieu = cbbMaphieu.SelectedItem?.ToString();
            string maMay = cbbMamay.SelectedItem?.ToString();
            string maKho = cbbMakho.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(maPhieu) || string.IsNullOrEmpty(maMay) || string.IsNullOrEmpty(maKho))
            {
                MessageBox.Show("Vui lòng chọn đầy đủ mã phiếu, mã máy và mã kho.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtso_luong.Text.Trim(), out int so_luong) || so_luong <= 0)
            {
                MessageBox.Show("Số lượng phải là số nguyên dương!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!double.TryParse(txtdon_gia.Text.Trim(), out double don_gia) || don_gia <= 0)
            {
                MessageBox.Show("Đơn giá phải là số thực dương!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string baohanhnhacungcap = txtBH.Text.Trim();

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    string query = @"INSERT INTO chitietphieunhap (maPhieu, maMay, maKho, so_luong, don_gia, baohanhnhacungcap)
                             VALUES (@maPhieu, @maMay, @maKho, @so_luong, @don_gia, @baohanhnhacungcap)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@maPhieu", maPhieu);
                        cmd.Parameters.AddWithValue("@maMay", maMay);
                        cmd.Parameters.AddWithValue("@maKho", maKho);
                        cmd.Parameters.AddWithValue("@so_luong", so_luong);
                        cmd.Parameters.AddWithValue("@don_gia", don_gia);
                        cmd.Parameters.AddWithValue("@baohanhnhacungcap", baohanhnhacungcap);
                        int result = cmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            MessageBox.Show("Thêm chi tiết phiếu nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            var formPhieuNhap = Application.OpenForms.OfType<FormPhieuNhap>().FirstOrDefault();
                            if (formPhieuNhap != null)
                            {
                                formPhieuNhap.napdgvphieunhap();
                            }

                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Không thể thêm chi tiết phiếu nhập.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadMaPhieuToComboBox()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    string query = "SELECT maPhieu FROM phieunhap";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        cbbMaphieu.Items.Clear();

                        while (reader.Read())
                        {
                            cbbMaphieu.Items.Add(reader["maPhieu"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load ComboBox: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadMamayToComboBox()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    string query = "SELECT id FROM sanpham";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        cbbMamay.Items.Clear();

                        while (reader.Read())
                        {
                            cbbMamay.Items.Add(reader["id"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load ComboBox: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        private void FormChiTietPhieuNhap_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }

        private void txtBH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void FormChiTietPhieuNhap_Load(object sender, EventArgs e)
        {
            txtBH.ImeMode = ImeMode.Disable;
        }
    }
}