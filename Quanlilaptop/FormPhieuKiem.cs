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
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace Quanlilaptop
{
    public partial class FormPhieuKiem : Form
    {
        // Khai báo chuỗi kết nối chung cho SQL Server
        private string connectionString = "Server=localhost\\SQLEXPRESS;Database=quanlimaytinh;Integrated Security=true;TrustServerCertificate=True;";

        public FormPhieuKiem()
        {
            InitializeComponent();
            napdgvphieukiem();
        }

        public void napdgvphieukiem()
        {
            string sql = "SELECT ma_phieu AS 'Mã Phiếu', ma_kho AS 'Mã Kho', nguoi_kiem AS 'Người Kiểm', ngay_kiem AS 'Ngày Kiểm', trang_thai AS 'Trạng Thái', ghi_chu AS 'Ghi Chú' FROM phieukiem;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(sql, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable table = new DataTable();
                adapter.Fill(table);
                dgvPhieuKiem.DataSource = table;
            }
        }

        private void FormPhieuKiem_Load(object sender, EventArgs e)
        {
            // none
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvPhieuKiem.SelectedRows.Count > 0)
            {
                string maPhieu = dgvPhieuKiem.SelectedRows[0].Cells["Mã Phiếu"].Value.ToString();

                DialogResult result = MessageBox.Show($"Bạn có chắc chắn muốn xóa phiếu kiểm có mã {maPhieu}?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        using (SqlConnection conn = new SqlConnection(connectionString))
                        {
                            conn.Open();
                            string query = "DELETE FROM phieukiem WHERE ma_phieu = @maPhieu";
                            SqlCommand cmd = new SqlCommand(query, conn);
                            cmd.Parameters.AddWithValue("@maPhieu", maPhieu);

                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Xóa phiếu kiểm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                napdgvphieukiem();
                            }
                            else
                            {
                                MessageBox.Show("Không tìm thấy phiếu kiểm để xóa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi xóa phiếu kiểm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn phiếu kiểm cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnTimkiem_Click(object sender, EventArgs e)
        {
            string keyword = txtTimkiem.Text.Trim();

            if (string.IsNullOrEmpty(keyword))
            {
                MessageBox.Show("Vui lòng nhập từ khóa tìm kiếm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Dùng CAST cho cột ngày tháng để tránh lỗi kiểu dữ liệu trong SQL Server khi dùng LIKE
            string query = @" 
                SELECT ma_phieu AS 'Mã Phiếu', ma_kho AS 'Mã Kho', nguoi_kiem AS 'Người Kiểm', ngay_kiem AS 'Ngày Kiểm', trang_thai AS 'Trạng Thái', ghi_chu AS 'Ghi Chú'
                FROM phieukiem 
                WHERE ma_phieu LIKE @keyword 
                   OR ma_kho LIKE @keyword 
                   OR nguoi_kiem LIKE @keyword 
                   OR CAST(ngay_kiem AS VARCHAR) LIKE @keyword 
                   OR trang_thai LIKE @keyword 
                   OR ghi_chu LIKE @keyword;";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    adapter.SelectCommand.Parameters.AddWithValue("@keyword", "%" + keyword + "%");

                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dgvPhieuKiem.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCapnhat_Click(object sender, EventArgs e)
        {
            if (dgvPhieuKiem.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một phiếu kiểm để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string maPhieu = dgvPhieuKiem.SelectedRows[0].Cells["Mã Phiếu"].Value.ToString();

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Word Document (*.docx)|*.docx",
                Title = "Chọn nơi lưu phiếu kiểm",
                FileName = $"PhieuKiem_{maPhieu}.docx"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                XuatPhieuKiemWord(maPhieu, saveFileDialog.FileName);
            }
        }

        private void XuatPhieuKiemWord(string maPhieu, string filePath)
        {
            try
            {
                string sqlPhieu = "SELECT * FROM phieukiem WHERE ma_phieu = @maPhieu";
                string sqlChiTietNhap = @"
                    SELECT ctpn.maMay, sp.ten_sanpham, ctpn.maKho, ctpn.so_luong, ctpn.don_gia, phieu.thoi_gian_tao AS thoi_gian_nhap
                    FROM chitietphieunhap ctpn
                    JOIN sanpham sp ON ctpn.maMay = sp.id
                    JOIN phieunhap phieu ON ctpn.maPhieu = phieu.maPhieu
                    WHERE ctpn.maKho = @maKho";

                string sqlChiTietXuat = @"
                    SELECT ctpx.maMay, sp.ten_sanpham, ctpx.maKho, ctpx.so_luong, ctpx.don_gia, phieu.thoi_gian_tao AS thoi_gian_xuat
                    FROM chitietphieuxuat ctpx
                    JOIN sanpham sp ON ctpx.maMay = sp.id
                    JOIN phieuxuat phieu ON ctpx.maPhieu = phieu.maPhieu
                    WHERE ctpx.maKho = @maKho";

                DataTable tblPhieu = new DataTable();
                DataTable tblChiTietNhap = new DataTable();
                DataTable tblChiTietXuat = new DataTable();

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(sqlPhieu, conn))
                    {
                        cmd.Parameters.AddWithValue("@maPhieu", maPhieu);
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        adapter.Fill(tblPhieu);
                    }

                    if (tblPhieu.Rows.Count == 0)
                    {
                        MessageBox.Show("Không tìm thấy phiếu kiểm!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string maKho = tblPhieu.Rows[0]["ma_kho"].ToString();

                    using (SqlCommand cmd = new SqlCommand(sqlChiTietNhap, conn))
                    {
                        cmd.Parameters.AddWithValue("@maKho", maKho);
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        adapter.Fill(tblChiTietNhap);
                    }

                    using (SqlCommand cmd = new SqlCommand(sqlChiTietXuat, conn))
                    {
                        cmd.Parameters.AddWithValue("@maKho", maKho);
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        adapter.Fill(tblChiTietXuat);
                    }
                }

                using (var doc = DocX.Create(filePath))
                {
                    doc.InsertParagraph("CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM")
                        .Font("Times New Roman").Bold().FontSize(12).Alignment = Alignment.center;
                    doc.InsertParagraph("Độc lập - Tự do - Hạnh phúc\n")
                        .Font("Times New Roman").Bold().FontSize(11).Alignment = Alignment.center;

                    doc.InsertParagraph("PHIẾU KIỂM")
                        .Font("Times New Roman").Bold().FontSize(16).SpacingBefore(20).SpacingAfter(20).Alignment = Alignment.center;

                    DataRow row = tblPhieu.Rows[0];
                    string info = $"Mã phiếu: {row["ma_phieu"]}\n"
                                + $"Ngày kiểm: {Convert.ToDateTime(row["ngay_kiem"]).ToString("dd/MM/yyyy")}\n"
                                + $"Người kiểm: {row["nguoi_kiem"]}\n"
                                + $"Trạng thái: {row["trang_thai"]}\n"
                                + $"Ghi chú: {row["ghi_chu"]}";

                    doc.InsertParagraph(info).Font("Times New Roman").FontSize(12).SpacingAfter(20);

                    if (tblChiTietNhap.Rows.Count > 0)
                    {
                        doc.InsertParagraph("Chi tiết nhập")
                            .Font("Times New Roman")
                            .FontSize(14)
                            .Bold()
                            .Alignment = Alignment.center;

                        var tableNhap = doc.AddTable(tblChiTietNhap.Rows.Count + 1, 6);
                        tableNhap.Design = TableDesign.TableGrid;

                        string[] headers = { "Mã máy", "Tên sản phẩm", "Kho", "Số lượng", "Đơn giá", "Thời gian nhập" };
                        for (int i = 0; i < headers.Length; i++)
                        {
                            tableNhap.Rows[0].Cells[i].Paragraphs[0].Append(headers[i]).Bold();
                        }

                        for (int i = 0; i < tblChiTietNhap.Rows.Count; i++)
                        {
                            for (int j = 0; j < 5; j++)
                            {
                                tableNhap.Rows[i + 1].Cells[j].Paragraphs[0].Append(tblChiTietNhap.Rows[i][j].ToString());
                            }
                            var tgNhap = tblChiTietNhap.Rows[i]["thoi_gian_nhap"];
                            string tgNhapStr = tgNhap == DBNull.Value ? "" : Convert.ToDateTime(tgNhap).ToString("dd/MM/yyyy HH:mm");
                            tableNhap.Rows[i + 1].Cells[5].Paragraphs[0].Append(tgNhapStr);
                        }

                        doc.InsertTable(tableNhap);
                    }

                    if (tblChiTietXuat.Rows.Count > 0)
                    {
                        doc.InsertParagraph("\nChi tiết xuất")
                            .Font("Times New Roman")
                            .FontSize(14)
                            .Bold()
                            .Alignment = Alignment.center;

                        var tableXuat = doc.AddTable(tblChiTietXuat.Rows.Count + 1, 6);
                        tableXuat.Design = TableDesign.TableGrid;

                        string[] headers = { "Mã máy", "Tên sản phẩm", "Kho", "Số lượng", "Đơn giá", "Thời gian xuất" };
                        for (int i = 0; i < headers.Length; i++)
                        {
                            tableXuat.Rows[0].Cells[i].Paragraphs[0].Append(headers[i]).Bold();
                        }

                        for (int i = 0; i < tblChiTietXuat.Rows.Count; i++)
                        {
                            for (int j = 0; j < 5; j++)
                            {
                                tableXuat.Rows[i + 1].Cells[j].Paragraphs[0].Append(tblChiTietXuat.Rows[i][j].ToString());
                            }
                            var tgXuat = tblChiTietXuat.Rows[i]["thoi_gian_xuat"];
                            string tgXuatStr = tgXuat == DBNull.Value ? "" : Convert.ToDateTime(tgXuat).ToString("dd/MM/yyyy HH:mm");
                            tableXuat.Rows[i + 1].Cells[5].Paragraphs[0].Append(tgXuatStr);
                        }

                        doc.InsertTable(tableXuat);
                    }

                    doc.InsertParagraph("\n\nNgười lập phiếu")
                        .Font("Times New Roman")
                        .FontSize(12)
                        .Italic()
                        .Alignment = Alignment.right;
                    doc.Save();
                }
                MessageBox.Show("Xuất phiếu kiểm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xuất phiếu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            FormSuaPhieuKiem form = new FormSuaPhieuKiem(this);
            form.Show();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            FormThemPhieuKiem form = new FormThemPhieuKiem(this);
            form.Show();
        }
    }
}