using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.SqlClient; // Sử dụng thư viện SQL Server

namespace Quanlilaptop
{
    public partial class FormThemBaoHanh : Form
    {
        private FormBaoHanh formBH;

        // Chuỗi kết nối SQL Server
        private string connString = "Server=localhost\\SQLEXPRESS;Database=quanlimaytinh;Integrated Security=true;TrustServerCertificate=True;";

        public FormThemBaoHanh(FormBaoHanh form)
        {
            InitializeComponent();
            LoadMaPhieuNhapToComboBox();
            LoadMaSanPhamToComboBox();
            LoadNguoiGuiToComboBox();
            formBH = form;
        }

        public FormThemBaoHanh(string maPhieuXuat, string maMay, int soLuong, DateTime ngayDoiTra)
        {
            InitializeComponent();

            cbbMaphieunhap.Items.Add(maPhieuXuat);
            cbbMaphieunhap.SelectedIndex = 0;

            cbbMaSP.Items.Add(maMay);
            cbbMaSP.SelectedIndex = 0;

            txtSoluong.Text = soLuong.ToString();
            dtpBH.Value = ngayDoiTra;

            LoadNguoiGuiToComboBox();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (cbbMaphieunhap.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn Mã Phiếu Nhập!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cbbNguoigui.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn Người Gửi!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cbbMaSP.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn Mã Sản Phẩm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string maPhieuNhap = cbbMaphieunhap.SelectedItem.ToString();
            string nguoiGui = cbbNguoigui.SelectedValue.ToString();
            DateTime thoiGianNhan = dtpBH.Value;
            string ghiChu = txtGhichu.Text.Trim();

            string maSanPham = cbbMaSP.SelectedItem.ToString();
            if (!int.TryParse(txtSoluong.Text.Trim(), out int soLuong) || soLuong <= 0)
            {
                MessageBox.Show("Vui lòng nhập số lượng hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoluong.Focus();
                return;
            }

            string moTaLoi = txtMotaloi.Text.Trim();
            string ketQuaBaoHanh = txtKetqua.Text.Trim();

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    // SQL Server dùng SCOPE_IDENTITY() thay cho LAST_INSERT_ID() của MySQL
                    string insertPhieu = @"INSERT INTO phieubaohanh 
                    (ma_phieu_nhap, thoi_gian_nhan, nguoi_gui, ghi_chu)
                    VALUES (@maPhieuNhap, @thoiGianNhan, @nguoiGui, @ghiChu);
                    SELECT SCOPE_IDENTITY();";

                    SqlCommand cmdPhieu = new SqlCommand(insertPhieu, conn);
                    cmdPhieu.Parameters.AddWithValue("@maPhieuNhap", maPhieuNhap);
                    cmdPhieu.Parameters.AddWithValue("@thoiGianNhan", thoiGianNhan);
                    cmdPhieu.Parameters.AddWithValue("@nguoiGui", nguoiGui);
                    cmdPhieu.Parameters.AddWithValue("@ghiChu", ghiChu);

                    int maPhieuBaoHanhMoi = Convert.ToInt32(cmdPhieu.ExecuteScalar());

                    string insertCT = @"INSERT INTO chitietbaohanh
                    (ma_phieu_bao_hanh, ma_san_pham, so_luong, mo_ta_loi, ket_qua_bao_hanh)
                    VALUES (@maPhieuBaoHanh, @maSanPham, @soLuong, @moTaLoi, @ketQuaBaoHanh)";

                    SqlCommand cmdCT = new SqlCommand(insertCT, conn);
                    cmdCT.Parameters.AddWithValue("@maPhieuBaoHanh", maPhieuBaoHanhMoi);
                    cmdCT.Parameters.AddWithValue("@maSanPham", maSanPham);
                    cmdCT.Parameters.AddWithValue("@soLuong", soLuong);
                    cmdCT.Parameters.AddWithValue("@moTaLoi", moTaLoi);
                    cmdCT.Parameters.AddWithValue("@ketQuaBaoHanh", ketQuaBaoHanh);

                    int rows = cmdCT.ExecuteNonQuery();

                    if (rows > 0)
                    {
                        MessageBox.Show("Thêm phiếu bảo hành và chi tiết thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        formBH.napdgvbaohanh();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Thêm chi tiết bảo hành thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadMaPhieuNhapToComboBox()
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
                        cbbMaphieunhap.Items.Clear();
                        while (reader.Read())
                        {
                            cbbMaphieunhap.Items.Add(reader["maPhieu"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load mã phiếu nhập: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadNguoiGuiToComboBox()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    string query = "SELECT ma_nhan_vien, user_name FROM nhanvien";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        cbbNguoigui.DataSource = dt;
                        cbbNguoigui.DisplayMember = "user_name";
                        cbbNguoigui.ValueMember = "ma_nhan_vien";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load ComboBox nhân viên: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadMaSanPhamToComboBox()
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
                MessageBox.Show("Lỗi khi load ComboBox sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormThemBaoHanh_Load(object sender, EventArgs e) { }
        private void cbbKetqua_SelectedIndexChanged(object sender, EventArgs e) { }
        private void txtKetqua_TextChanged(object sender, EventArgs e) { }
    }
}