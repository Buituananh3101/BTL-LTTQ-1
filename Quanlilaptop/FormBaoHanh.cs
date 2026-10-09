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
    public partial class FormBaoHanh : Form
    {
        // Khai báo chuỗi kết nối chung cho SQL Server
        private string connString = "Server=localhost\\SQLEXPRESS;Database=quanlimaytinh;Integrated Security=true;TrustServerCertificate=True;";

        public FormBaoHanh()
        {
            InitializeComponent();
            napdgvbaohanh();
            dgvBaohanh.CellEndEdit += dgvBaohanh_CellEndEdit;
        }

        public void napdgvbaohanh()
        {
            string sql = @"SELECT 
                pb.ma_phieu_bao_hanh AS 'Mã Phiếu Bảo Hành',
                pb.ma_phieu_nhap AS 'Mã Phiếu Nhập',
                pb.thoi_gian_nhan AS 'Thời Gian Nhận',
                nv.ho_ten AS 'Người Gửi',
                nv.user_name AS 'Tài Khoản Nhân Viên',
                pb.ghi_chu AS 'Ghi Chú',
                ct.ma_san_pham AS 'Mã Sản Phẩm',
                sp.ten_sanpham AS 'Tên Sản Phẩm',
                ct.so_luong AS 'Số Lượng',
                ct.mo_ta_loi AS 'Mô Tả Lỗi',
                ct.ket_qua_bao_hanh AS 'Kết Quả Bảo Hành'
            FROM 
                phieubaohanh pb
            JOIN 
                nhanvien nv ON pb.nguoi_gui = nv.ma_nhan_vien
            JOIN 
                chitietbaohanh ct ON pb.ma_phieu_bao_hanh = ct.ma_phieu_bao_hanh
            JOIN 
                sanpham sp ON ct.ma_san_pham = sp.id;";

            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand(sql, conn);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    dgvBaohanh.DataSource = table;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi tải dữ liệu bảo hành: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            dgvBaohanh.Columns["Mã Phiếu Bảo Hành"].ReadOnly = true;
            dgvBaohanh.Columns["Mã Phiếu Nhập"].ReadOnly = true;
            dgvBaohanh.Columns["Tài Khoản Nhân Viên"].ReadOnly = true;
            dgvBaohanh.Columns["Tên Sản Phẩm"].ReadOnly = true;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvBaohanh.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một phiếu bảo hành để xoá!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string maPhieuBaoHanh = dgvBaohanh.SelectedRows[0].Cells["Mã Phiếu Bảo Hành"].Value?.ToString();

            if (string.IsNullOrEmpty(maPhieuBaoHanh))
            {
                MessageBox.Show("Mã phiếu bảo hành không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult result = MessageBox.Show(
                $"Bạn có chắc chắn muốn xoá phiếu bảo hành '{maPhieuBaoHanh}' không?\nTất cả chi tiết liên quan cũng sẽ bị xoá!",
                "Xác nhận xoá",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        conn.Open();

                        string sql = "DELETE FROM phieubaohanh WHERE ma_phieu_bao_hanh = @maPhieuBaoHanh";
                        SqlCommand cmd = new SqlCommand(sql, conn);
                        cmd.Parameters.AddWithValue("@maPhieuBaoHanh", maPhieuBaoHanh);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Xoá thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            napdgvbaohanh();
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy phiếu bảo hành để xoá!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xoá: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnWord_Click(object sender, EventArgs e)
        {
            if (dgvBaohanh.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một phiếu bảo hành để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string maPhieuBaoHanh = dgvBaohanh.SelectedRows[0].Cells["Mã Phiếu Bảo Hành"].Value?.ToString();

            if (string.IsNullOrEmpty(maPhieuBaoHanh))
            {
                MessageBox.Show("Mã phiếu bảo hành không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Word Document (*.docx)|*.docx",
                Title = "Chọn nơi lưu file Word",
                FileName = $"PhieuBaoHanh_{maPhieuBaoHanh}.docx"
            };

            if (saveFileDialog.ShowDialog() != DialogResult.OK) return;

            try
            {
                string sqlPhieu = @"SELECT pb.ma_phieu_bao_hanh, pb.ma_phieu_nhap, pb.thoi_gian_nhan, nv.ho_ten AS nguoi_gui, pb.ghi_chu 
                                    FROM phieubaohanh pb 
                                    JOIN nhanvien nv ON pb.nguoi_gui = nv.ma_nhan_vien
                                    WHERE pb.ma_phieu_bao_hanh = @maPhieuBaoHanh";
                DataTable dtPhieu = new DataTable();
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sqlPhieu, conn);
                    cmd.Parameters.AddWithValue("@maPhieuBaoHanh", maPhieuBaoHanh);
                    SqlDataAdapter adt = new SqlDataAdapter(cmd);
                    adt.Fill(dtPhieu);
                }

                if (dtPhieu.Rows.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy thông tin phiếu bảo hành!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                DataRow phieu = dtPhieu.Rows[0];
                string sqlChiTiet = @"SELECT ct.ma_san_pham AS 'Mã sản phẩm', sp.ten_sanpham AS 'Tên sản phẩm', ct.so_luong AS 'Số lượng', 
                                        ct.mo_ta_loi AS 'Mô tả lỗi', ct.ket_qua_bao_hanh AS 'Kết quả bảo hành'
                                    FROM chitietbaohanh ct
                                    JOIN sanpham sp ON ct.ma_san_pham = sp.id
                                    WHERE ct.ma_phieu_bao_hanh = @maPhieuBaoHanh";
                DataTable dtChiTiet = new DataTable();
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sqlChiTiet, conn);
                    cmd.Parameters.AddWithValue("@maPhieuBaoHanh", maPhieuBaoHanh);
                    SqlDataAdapter adt = new SqlDataAdapter(cmd);
                    adt.Fill(dtChiTiet);
                }

                using (DocX doc = DocX.Create(saveFileDialog.FileName))
                {
                    doc.InsertParagraph("CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM")
                        .Font("Times New Roman").Bold().FontSize(12).Alignment = Alignment.center;
                    doc.InsertParagraph("Độc lập - Tự do - Hạnh phúc\n")
                        .Font("Times New Roman").Bold().FontSize(11).Alignment = Alignment.center;

                    doc.InsertParagraph("PHIẾU BẢO HÀNH")
                        .Font("Times New Roman").Bold().FontSize(16).SpacingBefore(20).SpacingAfter(20).Alignment = Alignment.center;

                    doc.InsertParagraph($"Mã Phiếu Bảo Hành: {phieu["ma_phieu_bao_hanh"]}").FontSize(12);
                    doc.InsertParagraph($"Mã Phiếu Nhập: {phieu["ma_phieu_nhap"]}").FontSize(12);
                    doc.InsertParagraph($"Thời Gian Nhận: {phieu["thoi_gian_nhan"]}").FontSize(12);
                    doc.InsertParagraph($"Người Gửi: {phieu["nguoi_gui"]}").FontSize(12);
                    doc.InsertParagraph($"Ghi Chú: {phieu["ghi_chu"]}").FontSize(12);

                    doc.InsertParagraph("\nDanh sách chi tiết bảo hành:\n").FontSize(12).Bold();

                    if (dtChiTiet.Rows.Count > 0)
                    {
                        var table = doc.AddTable(dtChiTiet.Rows.Count + 1, dtChiTiet.Columns.Count);
                        table.Design = TableDesign.TableGrid;

                        for (int i = 0; i < dtChiTiet.Columns.Count; i++)
                            table.Rows[0].Cells[i].Paragraphs[0].Append(dtChiTiet.Columns[i].ColumnName).Bold();

                        for (int i = 0; i < dtChiTiet.Rows.Count; i++)
                        {
                            for (int j = 0; j < dtChiTiet.Columns.Count; j++)
                            {
                                table.Rows[i + 1].Cells[j].Paragraphs[0].Append(dtChiTiet.Rows[i][j]?.ToString() ?? "");
                            }
                        }
                        doc.InsertTable(table);
                    }
                    else
                    {
                        doc.InsertParagraph("Không có chi tiết bảo hành nào.").FontSize(12);
                    }

                    doc.InsertParagraph("\n\nNgười lập biểu")
                        .Font("Times New Roman")
                        .FontSize(12)
                        .Italic()
                        .Alignment = Alignment.right;

                    doc.Save();
                }

                MessageBox.Show("Xuất file Word thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xuất file Word: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            string sql = @"
                SELECT 
                    pb.ma_phieu_bao_hanh AS 'Mã Phiếu Bảo Hành',
                    pb.ma_phieu_nhap AS 'Mã Phiếu Nhập',
                    pb.thoi_gian_nhan AS 'Thời Gian Nhận',
                    nv.ho_ten AS 'Người Gửi',
                    pb.ghi_chu AS 'Ghi Chú',
                    ct.ma_san_pham AS 'Mã Sản Phẩm',
                    sp.ten_sanpham AS 'Tên Sản Phẩm',
                    ct.so_luong AS 'Số Lượng',
                    ct.mo_ta_loi AS 'Mô Tả Lỗi',
                    ct.ket_qua_bao_hanh AS 'Kết Quả Bảo Hành'
                FROM phieubaohanh pb
                JOIN nhanvien nv ON pb.nguoi_gui = nv.ma_nhan_vien
                JOIN chitietbaohanh ct ON pb.ma_phieu_bao_hanh = ct.ma_phieu_bao_hanh
                JOIN sanpham sp ON ct.ma_san_pham = sp.id
                WHERE 
                    CAST(pb.ma_phieu_bao_hanh AS VARCHAR) LIKE @keyword OR
                    pb.ma_phieu_nhap LIKE @keyword OR
                    CAST(pb.thoi_gian_nhan AS VARCHAR) LIKE @keyword OR
                    nv.ho_ten LIKE @keyword OR
                    pb.ghi_chu LIKE @keyword OR
                    ct.ma_san_pham LIKE @keyword OR
                    sp.ten_sanpham LIKE @keyword OR
                    CAST(ct.so_luong AS VARCHAR) LIKE @keyword OR
                    ct.mo_ta_loi LIKE @keyword OR
                    ct.ket_qua_bao_hanh LIKE @keyword
            ";

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@keyword", "%" + keyword + "%");

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dgvBaohanh.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            // none
        }

        private void dgvBaohanh_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // none
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            FormThemBaoHanh form = new FormThemBaoHanh(this);
            form.Show();
        }

        private void btnCapnhat_Click(object sender, EventArgs e)
        {
            FormSuaBaoHanh suaForm = new FormSuaBaoHanh();

            suaForm.BaoHanhUpdated += (s, ev) =>
            {
                string query = "SELECT * FROM phieubaohanh";
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvBaohanh.DataSource = dt;
                }
            };

            suaForm.ShowDialog();
            napdgvbaohanh();
        }

        private void dgvBaohanh_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // none
        }
    }
}