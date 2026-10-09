using MySql.Data.MySqlClient;
using System;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Data;
namespace Quanlilaptop
{
    public partial class FormChiTietPhieuXuat : Form
    {
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HTCAPTION = 0x2;
        private string connectionString = "server=localhost;user id=root;password=;database=quanlimaytinh;";
        public FormChiTietPhieuXuat()
        {
            InitializeComponent();
            LoadmakhoToComboBox();
            LoadMaPhieuToComboBox();
            LoadMamayToComboBox();
            this.MouseDown += new MouseEventHandler(FormChiTietPhieuXuat_MouseDown);
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            string maPhieu = cbbMaphieu.SelectedItem?.ToString();
            string maMay = cbbMamay.SelectedItem?.ToString();
            string maKho = cbbMakho.SelectedItem?.ToString();
            var selectedLohang = cbb_Lohang.SelectedItem as ComboBoxItem;

            if (string.IsNullOrEmpty(maPhieu) || string.IsNullOrEmpty(maMay) || string.IsNullOrEmpty(maKho) || selectedLohang == null)
            {
                MessageBox.Show("Vui lòng chọn đầy đủ mã phiếu, mã máy, mã kho và lô hàng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    // Truy vấn tính số lượng khả dụng để xuất
                    string query = @"
                SELECT 
                    sp.ton_kho - IFNULL(ct.san_pham_chua_kiem_tra, 0) 
                               - IFNULL(ct.san_pham_loi, 0) 
                               - IFNULL(ct.san_pham_ktv, 0) AS soluong_conlai
                FROM sanpham sp
                LEFT JOIN chitietsoluongsanpham ct ON sp.id = ct.ma_san_pham AND ct.maPhieu = @maPhieuNhap
                WHERE sp.id = @maMay";

                    using (MySqlCommand checkCmd = new MySqlCommand(query, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@maMay", maMay);
                        checkCmd.Parameters.AddWithValue("@maPhieuNhap", selectedLohang.Value);

                        object result = checkCmd.ExecuteScalar();

                        if (result == null || Convert.ToInt32(result) < so_luong)
                        {
                            MessageBox.Show("Số lượng tồn kho khả dụng không đủ để xuất!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    // Thêm vào chi tiết phiếu xuất
                    string insertQuery = @"INSERT INTO chitietphieuxuat (maPhieu, maMay, maKho, so_luong, don_gia, maPhieuNhap)
                       VALUES (@maPhieu, @maMay, @maKho, @so_luong, @don_gia, @maPhieuNhap)";
                    

                    using (MySqlCommand insertCmd = new MySqlCommand(insertQuery, conn))
                    {
                        insertCmd.Parameters.AddWithValue("@maPhieu", maPhieu);
                        insertCmd.Parameters.AddWithValue("@maMay", maMay);
                        insertCmd.Parameters.AddWithValue("@maKho", maKho);
                        insertCmd.Parameters.AddWithValue("@so_luong", so_luong);
                        insertCmd.Parameters.AddWithValue("@don_gia", don_gia);
                        insertCmd.Parameters.AddWithValue("@maPhieuNhap", selectedLohang.Value);
                        int result = insertCmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            MessageBox.Show("Thêm chi tiết phiếu xuất thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            var formPhieuXuat = Application.OpenForms.OfType<FormPhieuXuat>().FirstOrDefault();
                            if (formPhieuXuat != null)
                            {
                                formPhieuXuat.napdgvphieuxuat();
                            }

                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Không thể thêm chi tiết phiếu xuất.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                using (MySqlConnection conn = new MySqlConnection("Server=localhost;Database=quanlimaytinh;Port=3306;User ID=root;Password="))
                {
                    conn.Open();
                    string query = "SELECT maPhieu FROM phieuxuat";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
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
                using (MySqlConnection conn = new MySqlConnection("Server=localhost;Database=quanlimaytinh;Port=3306;User ID=root;Password="))
                {
                    conn.Open();
                    string query = "SELECT id FROM sanpham";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
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
                using (MySqlConnection conn = new MySqlConnection("Server=localhost;Database=quanlimaytinh;Port=3306;User ID=root;Password="))
                {
                    conn.Open();
                    string query = "SELECT maKho FROM kho";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
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

        private void FormChiTietPhieuXuat_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }

        private void FormChiTietPhieuXuat_Load(object sender, EventArgs e)
        {

        }

        private void cbbMamay_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbbMamay.SelectedItem == null || cbbMakho.SelectedItem == null)
                return;

            string maMay = cbbMamay.SelectedItem.ToString();
            string maKho = cbbMakho.SelectedItem.ToString();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = @"
        SELECT DISTINCT 
            pn.maPhieu, 
            ncc.ten_nha_cung_cap, 
            pn.thoi_gian_tao
        FROM phieunhap pn
        JOIN chitietphieunhap ctpn on pn.maPhieu = ctpn.maPhieu
        JOIN chitietsoluongsanpham ct ON pn.maPhieu = ct.maPhieu
        JOIN nhacungcap ncc ON pn.ma_nha_cung_cap = ncc.ma_nha_cung_cap
        WHERE ctpn.maMay = @maMay AND ctpn.maKho = @maKho";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@maMay", maMay);
                    cmd.Parameters.AddWithValue("@maKho", maKho);
                    conn.Open();

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        cbb_Lohang.Items.Clear();

                        while (reader.Read())
                        {
                            string maPhieuNhap = reader["maPhieu"].ToString();
                            string tenNCC = reader["ten_nha_cung_cap"].ToString();
                            DateTime ngayNhap = Convert.ToDateTime(reader["thoi_gian_tao"]);

                            string displayText = $"{tenNCC} - {ngayNhap:dd/MM/yyyy}";
                            cbb_Lohang.Items.Add(new ComboBoxItem(displayText, maPhieuNhap));
                        }
                    }
                }
            }
        }



        public class ComboBoxItem
        {
            public string Text { get; set; }
            public string Value { get; set; }

            public ComboBoxItem(string text, string value)
            {
                Text = text;
                Value = value;
            }

            public override string ToString()
            {
                return Text; // Hiển thị lên ComboBox
            }
        }

        private void cbbMakho_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbbMamay.SelectedItem != null)
            {
                cbbMamay_SelectedIndexChanged(sender, e);
            }
        }
    }
}
