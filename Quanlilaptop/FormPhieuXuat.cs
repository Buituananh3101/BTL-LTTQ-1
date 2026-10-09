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
    public partial class FormPhieuXuat : Form
    {
        // Khai báo chuỗi kết nối chung cho SQL Server
        private string connString = "Server=localhost\\SQLEXPRESS;Database=quanlimaytinh;Integrated Security=true;TrustServerCertificate=True;";

        public FormPhieuXuat()
        {
            InitializeComponent();
            napdgvphieuxuat();
            dgvPhieuxuat.CellEndEdit += dgvPhieuxuat_CellEndEdit;
        }

        public void napdgvphieuxuat()
        {
            string sql = @"SELECT px.maPhieu AS 'Mã Phiếu', px.thoi_gian_tao AS 'Thời Gian Tạo', px.nguoi_tao AS 'Người Tạo', px.tong_tien AS 'Tổng Tiền phiếu xuất', sp.id AS 'Mã Máy', ctpx.maKho AS 'Mã Kho', ctpx.so_luong AS 'Số Lượng', ctpx.don_gia AS 'Đơn Giá', px.id_khach_hang AS 'Mã khách hàng' FROM phieuxuat px JOIN chitietphieuxuat ctpx ON px.maPhieu = ctpx.maPhieu JOIN sanpham sp ON sp.id = ctpx.maMay";

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(sql, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable table = new DataTable();
                adapter.Fill(table);
                dgvPhieuxuat.DataSource = table;
                foreach (DataGridViewColumn col in dgvPhieuxuat.Columns)
                {
                    if (col.Name == "Mã Phiếu" || col.Name == "Mã Máy" || col.Name == "Thời Gian Tạo")
                    {
                        col.ReadOnly = true;
                    }
                }
            }
            dgvPhieuxuat.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void btnTimkiem_Click(object sender, EventArgs e)
        {
            string keyword = txtTimkiem.Text.Trim();

            // Sử dụng CAST để chuyển đổi an toàn các trường số và ngày tháng sang chuỗi khi dùng LIKE trên SQL Server
            string sql = @"SELECT 
                                px.maPhieu, 
                                px.thoi_gian_tao, 
                                px.nguoi_tao, 
                                px.tong_tien,
                                sp.id AS maMay,
                                ctpx.maKho,
                                ctpx.so_luong,
                                ctpx.don_gia
                            FROM phieuxuat px
                            JOIN chitietphieuxuat ctpx ON px.maPhieu = ctpx.maPhieu
                            JOIN sanpham sp ON sp.id = ctpx.maMay
                            WHERE px.maPhieu LIKE @kw
                               OR CAST(px.thoi_gian_tao AS VARCHAR) LIKE @kw
                               OR px.nguoi_tao LIKE @kw
                               OR CAST(px.tong_tien AS VARCHAR) LIKE @kw
                               OR sp.id LIKE @kw
                               OR ctpx.maKho LIKE @kw
                               OR CAST(ctpx.so_luong AS VARCHAR) LIKE @kw
                               OR CAST(ctpx.don_gia AS VARCHAR) LIKE @kw";

            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand(sql, conn);
                    command.Parameters.AddWithValue("@kw", $"%{keyword}%");

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    dgvPhieuxuat.DataSource = table;
                    dgvPhieuxuat.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            FormTaoPhieuXuat form = new FormTaoPhieuXuat();
            form.Show();
        }

        private void btnXuatfile_Click(object sender, EventArgs e)
        {
            if (dgvPhieuxuat.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một dòng trong bảng phiếu xuất.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maPhieu = dgvPhieuxuat.SelectedRows[0].Cells["Mã Phiếu"].Value.ToString();

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Word Document (*.docx)|*.docx",
                Title = "Chọn nơi lưu phiếu xuất",
                FileName = $"PhieuXuat_{maPhieu}.docx"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                XuatPhieuXuatWord(maPhieu, saveFileDialog.FileName);
            }
        }

        private void XuatPhieuXuatWord(string maPhieu, string filePath)
        {
            try
            {
                string sqlPhieu = "SELECT * FROM phieuxuat WHERE maPhieu = @maPhieu";
                string sqlChiTiet = @" 
                    SELECT ctpn.maMay, sp.ten_sanpham, ctpn.maKho, ctpn.so_luong, ctpn.don_gia 
                    FROM chitietphieuxuat ctpn
                    JOIN sanpham sp ON ctpn.maMay = sp.id
                    WHERE ctpn.maPhieu = @maPhieu";

                DataTable tblPhieu = new DataTable();
                DataTable tblChiTiet = new DataTable();

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(sqlPhieu, conn))
                    {
                        cmd.Parameters.AddWithValue("@maPhieu", maPhieu);
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        adapter.Fill(tblPhieu);
                    }

                    using (SqlCommand cmd = new SqlCommand(sqlChiTiet, conn))
                    {
                        cmd.Parameters.AddWithValue("@maPhieu", maPhieu);
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        adapter.Fill(tblChiTiet);
                    }
                }

                if (tblPhieu.Rows.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy phiếu xuất!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using (var doc = DocX.Create(filePath))
                {
                    doc.InsertParagraph("CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM")
                        .Font("Times New Roman").Bold().FontSize(12).Alignment = Alignment.center;
                    doc.InsertParagraph("Độc lập - Tự do - Hạnh phúc\n")
                        .Font("Times New Roman").Bold().FontSize(11).Alignment = Alignment.center;

                    doc.InsertParagraph("PHIẾU XUẤT")
                        .Font("Times New Roman").Bold().FontSize(16).SpacingBefore(20).SpacingAfter(20).Alignment = Alignment.center;

                    var row = tblPhieu.Rows[0];
                    string info = $"Mã phiếu: {row["maPhieu"]}\n"
                                + $"Ngày tạo: {Convert.ToDateTime(row["thoi_gian_tao"]).ToString("dd/MM/yyyy HH:mm")}\n"
                                + $"Người tạo: {row["nguoi_tao"]}\n"
                                + $"Tổng tiền: {string.Format("{0:N0} VNĐ", row["tong_tien"])}";

                    doc.InsertParagraph(info).Font("Times New Roman").FontSize(12).SpacingAfter(20);

                    var table = doc.AddTable(tblChiTiet.Rows.Count + 1, 5);
                    table.Design = TableDesign.TableGrid;

                    string[] headers = { "Mã máy", "Tên sản phẩm", "Kho", "Số lượng", "Đơn giá" };
                    for (int i = 0; i < headers.Length; i++)
                    {
                        table.Rows[0].Cells[i].Paragraphs[0].Append(headers[i]).Bold();
                    }

                    for (int i = 0; i < tblChiTiet.Rows.Count; i++)
                    {
                        for (int j = 0; j < 5; j++)
                        {
                            table.Rows[i + 1].Cells[j].Paragraphs[0].Append(tblChiTiet.Rows[i][j].ToString());
                        }
                    }

                    doc.InsertTable(table);

                    doc.InsertParagraph("\n\nNgười lập phiếu")
                        .Font("Times New Roman")
                        .FontSize(12)
                        .Italic()
                        .Alignment = Alignment.right;

                    doc.Save();
                }

                MessageBox.Show("Xuất phiếu xuất thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xuất phiếu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvPhieuxuat.CurrentRow != null)
            {
                string maPhieu = dgvPhieuxuat.CurrentRow.Cells["Mã Phiếu"].Value.ToString();
                string maMay = dgvPhieuxuat.CurrentRow.Cells["Mã Máy"].Value.ToString();
                string maKho = dgvPhieuxuat.CurrentRow.Cells["Mã Kho"].Value.ToString();

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    try
                    {
                        conn.Open();

                        DialogResult result = MessageBox.Show(
                            $"Bạn có chắc muốn xoá sản phẩm {maMay} trong phiếu {maPhieu} tại kho {maKho}?",
                            "Xác nhận xoá", MessageBoxButtons.YesNo, MessageBoxIcon.Warning
                        );

                        if (result == DialogResult.Yes)
                        {
                            // Xóa dòng liên quan trong bảng đổi trả (nếu có)
                            string deleteDoiTraSql = @"
                                DELETE FROM doitra 
                                WHERE ma_phieu = @maPhieu 
                                  AND ma_may = @maMay 
                                  AND ma_kho = @maKho";
                            SqlCommand cmdDoiTra = new SqlCommand(deleteDoiTraSql, conn);
                            cmdDoiTra.Parameters.AddWithValue("@maPhieu", maPhieu);
                            cmdDoiTra.Parameters.AddWithValue("@maMay", maMay);
                            cmdDoiTra.Parameters.AddWithValue("@maKho", maKho);
                            cmdDoiTra.ExecuteNonQuery();

                            // Xoá chi tiết phiếu xuất
                            string deleteChiTietSql = @"DELETE FROM chitietphieuxuat 
                                                        WHERE maPhieu = @maPhieu 
                                                          AND maMay = @maMay 
                                                          AND maKho = @maKho";
                            SqlCommand cmdChiTiet = new SqlCommand(deleteChiTietSql, conn);
                            cmdChiTiet.Parameters.AddWithValue("@maPhieu", maPhieu);
                            cmdChiTiet.Parameters.AddWithValue("@maMay", maMay);
                            cmdChiTiet.Parameters.AddWithValue("@maKho", maKho);
                            cmdChiTiet.ExecuteNonQuery();

                            // Cập nhật lại tổng tiền (Thay IFNULL thành ISNULL chuẩn SQL Server)
                            string updateTongTienSql = @"
                                UPDATE phieuxuat 
                                SET tong_tien = (
                                    SELECT ISNULL(SUM(so_luong * don_gia), 0) 
                                    FROM chitietphieuxuat 
                                    WHERE maPhieu = @maPhieu
                                )
                                WHERE maPhieu = @maPhieu";
                            SqlCommand cmdUpdate = new SqlCommand(updateTongTienSql, conn);
                            cmdUpdate.Parameters.AddWithValue("@maPhieu", maPhieu);
                            cmdUpdate.ExecuteNonQuery();

                            // Nếu không còn dòng nào trong phiếu, xóa luôn phiếu
                            string checkRemainingSql = "SELECT COUNT(*) FROM chitietphieuxuat WHERE maPhieu = @maPhieu";
                            SqlCommand remainingCmd = new SqlCommand(checkRemainingSql, conn);
                            remainingCmd.Parameters.AddWithValue("@maPhieu", maPhieu);
                            int remaining = Convert.ToInt32(remainingCmd.ExecuteScalar());
                            if (remaining == 0)
                            {
                                string sqlDeletePhieu = "DELETE FROM phieuxuat WHERE maPhieu = @maPhieu";
                                SqlCommand cmdDel = new SqlCommand(sqlDeletePhieu, conn);
                                cmdDel.Parameters.AddWithValue("@maPhieu", maPhieu);
                                cmdDel.ExecuteNonQuery();
                            }

                            MessageBox.Show("Xoá thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            napdgvphieuxuat();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi xoá: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn dòng cần xoá!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dgvPhieuxuat_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            string maPhieu = dgvPhieuxuat.Rows[e.RowIndex].Cells["Mã Phiếu"].Value.ToString();
            string maMay = dgvPhieuxuat.Rows[e.RowIndex].Cells["Mã Máy"].Value.ToString();
            string maKho = dgvPhieuxuat.Rows[e.RowIndex].Cells["Mã Kho"].Value.ToString();
            int soLuong = Convert.ToInt32(dgvPhieuxuat.Rows[e.RowIndex].Cells["Số Lượng"].Value);
            int donGia = Convert.ToInt32(dgvPhieuxuat.Rows[e.RowIndex].Cells["Đơn Giá"].Value);

            string sql = @"UPDATE chitietphieuxuat 
                           SET maKho = @maKho,
                               so_luong = @soLuong,
                               don_gia = @donGia
                           WHERE maPhieu = @maPhieu AND maMay = @maMay";

            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@maKho", maKho);
                    cmd.Parameters.AddWithValue("@soLuong", soLuong);
                    cmd.Parameters.AddWithValue("@donGia", donGia);
                    cmd.Parameters.AddWithValue("@maPhieu", maPhieu);
                    cmd.Parameters.AddWithValue("@maMay", maMay);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi cập nhật: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            napdgvphieuxuat();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            // none
        }
    }
}