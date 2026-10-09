using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Microsoft.Data.SqlClient; // Sử dụng thư viện SQL Server

namespace Quanlilaptop
{
    public partial class FormThongTinSP : Form
    {
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HTCAPTION = 0x2;

        private FormSanPham formSanPham;

        // Khai báo chuỗi kết nối SQL Server chung
        private string connStr = "Server=localhost\\SQLEXPRESS;Database=quanlimaytinh;Integrated Security=true;TrustServerCertificate=True;";

        public FormThongTinSP(FormSanPham form)
        {
            InitializeComponent();
            formSanPham = form;
            this.MouseDown += new MouseEventHandler(FormThongTinSP_MouseDown);
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            string id = txtID.Text.Trim();
            string tenSP = txtTenSP.Text.Trim();
            string loai = txtLoai.Text.Trim();
            string ngayTao = dtpNKT.Value.ToString("yyyy-MM-dd");
            string donViTinh = txtDonvitinh.Text.Trim();
            string chiTiet = txtThongtin.Text.Trim();
            string baohanhch = txtBaohanhCH.Text.Trim();

            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(tenSP) || string.IsNullOrEmpty(loai) || string.IsNullOrEmpty(donViTinh) || string.IsNullOrEmpty(baohanhch))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ các trường bắt buộc!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    string insertQuery = @"
                    INSERT INTO sanpham 
                    (id, ten_sanpham, loai, co_the_ban, ton_kho, ngay_khoi_tao, donvitinh, chitietsanpham, don_gia, baohanhcuahang)
                    VALUES 
                    (@id, @tenSP, @loai, 0, 0, @ngayTao, @donViTinh, @chiTiet, 0, @baohanhch)";

                    using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@tenSP", tenSP);
                        cmd.Parameters.AddWithValue("@loai", loai);
                        cmd.Parameters.AddWithValue("@ngayTao", ngayTao);
                        cmd.Parameters.AddWithValue("@donViTinh", donViTinh);
                        cmd.Parameters.AddWithValue("@chiTiet", chiTiet);
                        cmd.Parameters.AddWithValue("@baohanhch", baohanhch);

                        int result = cmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            MessageBox.Show("Thêm sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            if (formSanPham != null)
                            {
                                formSanPham.napdgvsanpham();
                            }

                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Không thể thêm sản phẩm.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormThongTinSP_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }

        private void txtBaohanhCH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}