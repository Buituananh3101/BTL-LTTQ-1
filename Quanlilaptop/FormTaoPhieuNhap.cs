using MySql.Data.MySqlClient;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Quanlilaptop
{
    public partial class FormTaoPhieuNhap : Form
    {
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HTCAPTION = 0x2;
        public FormTaoPhieuNhap()
        {
            InitializeComponent();
            Loadnguoi_taoToComboBox();
            LoadNhaCCToComboBox();
            this.MouseDown += new MouseEventHandler(FormTaoPhieuNhap_MouseDown);
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSkip_Click(object sender, EventArgs e)
        {
            FormChiTietPhieuNhap form = new FormChiTietPhieuNhap();
            this.Close();
            form.Show();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            string maPhieu = txtMaphieu.Text.Trim();
            string nguoi_tao = cbbnguoi_tao.SelectedItem?.ToString();
            string nhaCC = cbbNhaCC.SelectedItem?.ToString();
            DateTime thoi_gian_tao = dtpThoigian.Value;

            if (string.IsNullOrEmpty(maPhieu) || string.IsNullOrEmpty(nguoi_tao) || string.IsNullOrEmpty(nhaCC))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection("Server=localhost;Database=quanlimaytinh;Port=3306;User ID=root;Password="))
                {
                    conn.Open();
                    string query = @"INSERT INTO phieunhap (maPhieu, thoi_gian_tao, nguoi_tao, ma_nha_cung_cap, tong_tien)
                             VALUES (@maPhieu, @thoi_gian_tao, @nguoi_tao, @ma_nha_cung_cap, 0)";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@maPhieu", maPhieu);
                        cmd.Parameters.AddWithValue("@thoi_gian_tao", thoi_gian_tao);
                        cmd.Parameters.AddWithValue("@nguoi_tao", nguoi_tao);
                        cmd.Parameters.AddWithValue("@ma_nha_cung_cap", nhaCC);

                        int result = cmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            MessageBox.Show("Thêm phiếu nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Không thể thêm phiếu nhập.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.Close();
            FormChiTietPhieuNhap form = new FormChiTietPhieuNhap();
            form.Show();
        }

        private void FormTaoPhieuNhap_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }
        private void Loadnguoi_taoToComboBox()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection("Server=localhost;Database=quanlimaytinh;Port=3306;User ID=root;Password="))
                {
                    conn.Open();
                    string query = "SELECT userName FROM account";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
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
        private void LoadNhaCCToComboBox()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection("Server=localhost;Database=quanlimaytinh;Port=3306;User ID=root;Password="))
                {
                    conn.Open();
                    string query = "SELECT ma_nha_cung_cap FROM nhacungcap";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        cbbNhaCC.Items.Clear(); 

                        while (reader.Read())
                        {
                            cbbNhaCC.Items.Add(reader["ma_nha_cung_cap"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load ComboBox: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormTaoPhieuNhap_Load(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
}
