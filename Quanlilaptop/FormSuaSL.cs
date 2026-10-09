using Microsoft.Data.SqlClient; // Thay thế thư viện MySQL bằng SQL Server
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quanlilaptop
{
    public partial class FormSuaSL : Form
    {
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HTCAPTION = 0x2;

        private FormSanPham formSanPham;

        // Khai báo chuỗi kết nối chung cho SQL Server
        private string connString = "Server=localhost\\SQLEXPRESS;Database=quanlimaytinh;Integrated Security=true;TrustServerCertificate=True;";

        public void napdgvchitiet()
        {
            string sql = "SELECT * FROM chitietsoluongsanpham";
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(sql, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable table = new DataTable();
                adapter.Fill(table);
                dgvChitiet.DataSource = table;
            }
        }

        public FormSuaSL(FormSanPham form)
        {
            InitializeComponent();
            LoadSanPhamToComboBox();
            LoadphieunhapToComboBox();
            napdgvchitiet();
            this.MouseDown += new MouseEventHandler(FormSuaSL_MouseDown);
            formSanPham = form;
        }

        private void FormSuaSL_Load(object sender, EventArgs e)
        {
        }

        private void LoadSanPhamToComboBox()
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
                        cbbMaSP.Items.Clear();
                        while (reader.Read())
                        {
                            cbbMaSP.Items.Add(reader["id"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load ComboBox: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadphieunhapToComboBox()
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

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbbMaSP_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            string maSanPham = cbbMaSP.SelectedItem?.ToString();
            string maPhieu = cbbMaphieu.SelectedItem?.ToString();
            string sanPhamChuaKiemtraText = txtSLCKT.Text.Trim();
            string sanPhamLoiText = txtSLBL.Text.Trim();
            string sanPhamKTVText = txtSLDKT.Text.Trim();

            if (string.IsNullOrEmpty(maSanPham) ||
                string.IsNullOrEmpty(maPhieu) ||
                string.IsNullOrEmpty(sanPhamChuaKiemtraText) ||
                string.IsNullOrEmpty(sanPhamLoiText) ||
                string.IsNullOrEmpty(sanPhamKTVText))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(sanPhamChuaKiemtraText, out int sanPhamChuaKiemtra) ||
                !int.TryParse(sanPhamLoiText, out int sanPhamLoi) ||
                !int.TryParse(sanPhamKTVText, out int sanPhamKTV))
            {
                MessageBox.Show("Số lượng phải là số nguyên dương!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    string checkQuery = @"SELECT COUNT(*) FROM chitietsoluongsanpham 
                                    WHERE ma_san_pham = @maSanPham AND maPhieu = @maPhieu";

                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@maSanPham", maSanPham);
                        checkCmd.Parameters.AddWithValue("@maPhieu", maPhieu);
                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (count == 0)
                        {
                            MessageBox.Show("Dữ liệu chưa tồn tại để cập nhật. Vui lòng thêm trước!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    string updateQuery = @"UPDATE chitietsoluongsanpham
                                   SET san_pham_chua_kiem_tra = @sanPhamChuaKiemtra,
                                       san_pham_loi = @sanPhamLoi,
                                       san_pham_ktv = @sanPhamKTV
                                   WHERE ma_san_pham = @maSanPham AND maPhieu = @maPhieu";

                    using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@maSanPham", maSanPham);
                        cmd.Parameters.AddWithValue("@maPhieu", maPhieu);
                        cmd.Parameters.AddWithValue("@sanPhamChuaKiemtra", sanPhamChuaKiemtra);
                        cmd.Parameters.AddWithValue("@sanPhamLoi", sanPhamLoi);
                        cmd.Parameters.AddWithValue("@sanPhamKTV", sanPhamKTV);

                        int result = cmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            MessageBox.Show("Cập nhật chi tiết sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            UpdateCoTheBan(maSanPham);
                            napdgvchitiet();
                            formSanPham.napdgvsanpham();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Không thể cập nhật chi tiết sản phẩm.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateCoTheBan(string maSanPham)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    // Dùng ISNULL thay vì hàm kiểm tra null của MySQL
                    string query = @"UPDATE sanpham
                                    SET co_the_ban = ton_kho - ISNULL((
                                        SELECT SUM(san_pham_chua_kiem_tra + san_pham_loi + san_pham_ktv)
                                        FROM chitietsoluongsanpham
                                        WHERE ma_san_pham = @maSanPham
                                    ), 0)
                                    WHERE id = @maSanPham";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@maSanPham", maSanPham);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật lại co_the_ban: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormSuaSL_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            string maSanPham = cbbMaSP.SelectedItem?.ToString();
            string maPhieu = cbbMaphieu.SelectedItem?.ToString();
            string sanPhamChuaKiemtraText = txtSLCKT.Text.Trim();
            string sanPhamLoiText = txtSLBL.Text.Trim();
            string sanPhamKTVText = txtSLDKT.Text.Trim();

            if (string.IsNullOrEmpty(maSanPham) ||
                string.IsNullOrEmpty(maPhieu) ||
                string.IsNullOrEmpty(sanPhamChuaKiemtraText) ||
                string.IsNullOrEmpty(sanPhamLoiText) ||
                string.IsNullOrEmpty(sanPhamKTVText))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(sanPhamChuaKiemtraText, out int sanPhamChuaKiemtra) ||
                !int.TryParse(sanPhamLoiText, out int sanPhamLoi) ||
                !int.TryParse(sanPhamKTVText, out int sanPhamKTV))
            {
                MessageBox.Show("Số lượng phải là số nguyên dương!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    string query = @"INSERT INTO chitietsoluongsanpham 
                                   (ma_san_pham, san_pham_chua_kiem_tra, san_pham_loi, san_pham_ktv, maPhieu)
                                   VALUES (@maSanPham, @sanPhamChuaKiemtra, @sanPhamLoi, @sanPhamKTV, @maPhieu)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@maSanPham", maSanPham);
                        cmd.Parameters.AddWithValue("@maPhieu", maPhieu);
                        cmd.Parameters.AddWithValue("@sanPhamChuaKiemtra", sanPhamChuaKiemtra);
                        cmd.Parameters.AddWithValue("@sanPhamLoi", sanPhamLoi);
                        cmd.Parameters.AddWithValue("@sanPhamKTV", sanPhamKTV);

                        int result = cmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            MessageBox.Show("Thêm chi tiết sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            formSanPham.napdgvsanpham();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Không thể thêm chi tiết sản phẩm.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}