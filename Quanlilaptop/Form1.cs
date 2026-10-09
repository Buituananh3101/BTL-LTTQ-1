using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Quanlilaptop
{
    public partial class Form1 : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect,
            int nTopRect,
            int nRightRect,
            int nBottomRect,
            int nWidthEllipse,
            int nHeightEllipse
        );

        string tenTaikhoan = "", matKhau = "", Gmail = "", chucVu = "";

        public Form1(string tenTaikhoan, string matKhau, string Gmail, string chucVu)
        {
            InitializeComponent();
            this.tenTaikhoan = tenTaikhoan;
            this.matKhau = matKhau;
            this.chucVu = chucVu;
            this.Gmail = Gmail;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 25, 25));
            PnLNaV.Height = btnSanPham.Height;
            PnLNaV.Top = btnSanPham.Top;
            PnLNaV.Left = btnSanPham.Left;
            btnSanPham.BackColor = Color.FromArgb(46, 51, 73);

            if (chucVu == "nhanvien")
            {
                btnSaoLuu.Enabled = false;
                btnCongno.Enabled = false;
                btnTaiKhoan.Enabled = false;

            }
            else if(chucVu == "ketoan")
            {
                btnSaoLuu.Enabled = false;
                btnDoitra.Enabled = false;
                btnTaiKhoan.Enabled = false;
                btnBaoHanh.Enabled = false;
            }

            lblName.Text = "Xin chào, " + tenTaikhoan;
            FormSanPham frmSanPham_Vrb = new FormSanPham()
            {
                Dock = DockStyle.Fill,
                TopLevel = false,
                TopMost = true
            };
            frmSanPham_Vrb.FormBorderStyle = FormBorderStyle.None;
            this.PnLFormLoader.Controls.Add(frmSanPham_Vrb);
            frmSanPham_Vrb.Show();
        }

        private void Form1_Load(object sender, EventArgs e) { }
        private void Form1_MouseDown(object sender, MouseEventArgs e) { }
        private void panel1_Paint(object sender, PaintEventArgs e) { }
        private void PnLFormLoader_Paint(object sender, PaintEventArgs e) { }

        private void btnSanPham_Click(object sender, EventArgs e) 
        { 
            LoadForm(new FormSanPham(), btnSanPham); 
        }
        private void btnSanPham_Leave(object sender, EventArgs e) 
        { 
            btnSanPham.BackColor = Color.FromArgb(24, 30, 54); 
        }
        private void btnNhaCungCap_Click(object sender, EventArgs e) 
        {
            LoadForm(new FormNhaCungCap(), btnNhaCungCap); 
        }
        private void btnNhaCungCap_Leave(object sender, EventArgs e)
        { 
            btnNhaCungCap.BackColor = Color.FromArgb(24, 30, 54); 
        }

        private void btnPhieuNhap_Click(object sender, EventArgs e) 
        { 
            LoadForm(new FormPhieuNhap(), btnPhieuNhap); 
        }
        private void btnPhieuNhap_Leave(object sender, EventArgs e) 
        { 
            btnPhieuNhap.BackColor = Color.FromArgb(24, 30, 54); 
        }

        private void btnPhieuXuat_Click(object sender, EventArgs e) 
        { 
            LoadForm(new FormPhieuXuat(), btnPhieuXuat); 
        }
        private void btnPhieuXuat_Leave(object sender, EventArgs e)
        { 
            btnPhieuXuat.BackColor = Color.FromArgb(24, 30, 54); 
        }

        private void btnPhieuKiem_Click(object sender, EventArgs e)
        { 
            LoadForm(new FormPhieuKiem(), btnPhieuKiem); 
        }
        private void btnPhieuKiem_Leave(object sender, EventArgs e)
        { 
            btnPhieuKiem.BackColor = Color.FromArgb(24, 30, 54); 
        }

        private void btnXuatHang_Click(object sender, EventArgs e) 
        { 
            LoadForm(new FormKhachHang(), btnThongtinkhachhang); 
        }
        private void btnXuatHang_Leave(object sender, EventArgs e) 
        { 
            btnThongtinkhachhang.BackColor = Color.FromArgb(24, 30, 54); 
        }

        private void btnNhapHang_Click(object sender, EventArgs e) 
        { 
            LoadForm(new FormCongNo(), btnCongno); 
        }
        private void btnNhapHang_Leave(object sender, EventArgs e)
        { 
            btnCongno.BackColor = Color.FromArgb(24, 30, 54); 
        }

        private void btnTonKho_Click(object sender, EventArgs e) 
        {
            LoadForm(new FormBaoHanh(), btnBaoHanh); 
        }
        private void btnTonKho_Leave(object sender, EventArgs e) 
        { 
            btnBaoHanh.BackColor = Color.FromArgb(24, 30, 54); 
        }

        private void btnDoithongtin_Click(object sender, EventArgs e)
        { 
            LoadForm(new FormDoiTra(), btnDoitra); 
        }
        private void btnDoithongtin_Leave(object sender, EventArgs e) 
        {
            btnDoitra.BackColor = Color.FromArgb(24, 30, 54); 
        }

        private void btnThongKe_Click(object sender, EventArgs e) 
        { 
            LoadForm(new FormThongKe(), btnThongKe); 
        }
        private void btnThongKe_Leave(object sender, EventArgs e) 
        {
            btnThongKe.BackColor = Color.FromArgb(24, 30, 54);
        }

        private void button1_Click(object sender, EventArgs e) 
        { 
            LoadForm(new SaoLuuVaKhoiPhuc(), btnSaoLuu); 
        }
        private void button1_Leave(object sender, EventArgs e) 
        { 
            btnSaoLuu.BackColor = Color.FromArgb(24, 30, 54); 
        }

        private void btnTaiKhoan_Click(object sender, EventArgs e) 
        { 
            LoadForm(new FormAccount(), btnTaiKhoan);
        }
        private void btnTaiKhoan_Leave(object sender, EventArgs e) 
        {
            btnTaiKhoan.BackColor = Color.FromArgb(24, 30, 54); 
        }

        private void button10_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("Bạn có muốn đăng xuất không ?", "Đăng xuất", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
            {
                FormDangNhap f = new FormDangNhap();
                f.Show();
                this.Close();
            }
        }

        private void LoadForm(Form frm, Button btn)
        {
            PnLNaV.Height = btn.Height;
            PnLNaV.Top = btn.Top;
            btn.BackColor = Color.FromArgb(46, 51, 73);
            this.PnLFormLoader.Controls.Clear();
            frm.Dock = DockStyle.Fill;
            frm.TopLevel = false;
            frm.TopMost = true;
            frm.FormBorderStyle = FormBorderStyle.None;
            this.PnLFormLoader.Controls.Add(frm);
            frm.Show();
        }
    }
}
