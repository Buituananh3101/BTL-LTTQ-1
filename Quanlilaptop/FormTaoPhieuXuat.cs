using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Microsoft.Data.SqlClient; // Thay thế thư viện MySQL bằng SQL Server

namespace Quanlilaptop
{
    public partial class FormTaoPhieuXuat : Form
    {
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HTCAPTION = 0x2;

        // Khai báo chuỗi kết nối SQL Server chung
        private string connStr = "Server=localhost\\SQLEXPRESS;Database=quanlimaytinh;Integrated Security=true;TrustServerCertificate=True;";

        public FormTaoPhieuXuat()
        {
            InitializeComponent();
            Loadnguoi_taoToComboBox();
            LoadKhachHangToComboBox();
            this.MouseDown += new MouseEventHandler(FormTaoPhieuXuat_MouseDown);
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSkip_Click(object sender, EventArgs e)
        {
            FormChiTietPhieuXuat form = new FormChiTietPhieuXuat();
            this.Close();
            form.Show();
        }

        private void Loadnguoi_taoToComboBox()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    string query = "SELECT userName FROM account";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        cbbnguoi_tao.Items.Clear();

                        while (reader.Read())
                        {
                            cbbnguoi_tao.Items.Add(reader["userName"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load ComboBox: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadKhachHangToComboBox()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    string query = "SELECT id_khach_hang FROM khachhang";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        cbbMakh.Items.Clear();

                        while (reader.Read())
                        {
                            cbbMakh.Items.Add(reader["id_khach_hang"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load ComboBox: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormTaoPhieuXuat_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            string maPhieu = txtMaphieu.Text.Trim();
            string nguoi_tao = cbbnguoi_tao.SelectedItem?.ToString();
            DateTime thoi_gian_tao = dtpThoigian.Value;
            string idKhachHang = cbbMakh.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(maPhieu) || string.IsNullOrEmpty(nguoi_tao) || string.IsNullOrEmpty(idKhachHang))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    string query = @"INSERT INTO phieuxuat (maPhieu, thoi_gian_tao, nguoi_tao, id_khach_hang, tong_tien)
                             VALUES (@maPhieu, @thoi_gian_tao, @nguoi_tao, @idKhachHang, 0)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@maPhieu", maPhieu);
                        cmd.Parameters.AddWithValue("@thoi_gian_tao", thoi_gian_tao);
                        cmd.Parameters.AddWithValue("@nguoi_tao", nguoi_tao);
                        cmd.Parameters.AddWithValue("@idKhachHang", idKhachHang);

                        int result = cmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            MessageBox.Show("Thêm phiếu xuất thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Không thể thêm phiếu xuất.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            this.Close();

            FormChiTietPhieuXuat form = new FormChiTietPhieuXuat();
            form.Show();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
}