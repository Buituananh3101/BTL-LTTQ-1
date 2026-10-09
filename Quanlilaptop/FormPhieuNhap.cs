using System;
using System.Data;
using System.Windows.Forms;
using Microsoft.Data.SqlClient; // Đổi sang thư viện SQL Server
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace Quanlilaptop
{
    public partial class FormPhieuNhap : Form
    {
        // Chuỗi kết nối SQL Server chung cho form
        private string connString = "Server=localhost\\SQLEXPRESS;Database=quanlimaytinh;Integrated Security=true;TrustServerCertificate=True;";

        public FormPhieuNhap()
        {
            InitializeComponent();
            napdgvphieunhap();
            dgvPhieunhap.CellEndEdit += dgvPhieunhap_CellEndEdit;
        }

        public void napdgvphieunhap()
        {
            string sql = @"SELECT pn.maPhieu AS 'Mã Phiếu', pn.thoi_gian_tao AS 'Thời Gian Tạo', pn.nguoi_tao AS 'Người Tạo', pn.ma_nha_cung_cap AS 'Mã Nhà Cung Cấp', pn.tong_tien AS 'Tổng Tiền phiếu nhập', sp.id AS 'Mã Máy', ctpn.maKho AS 'Mã Kho', ctpn.so_luong AS 'Số Lượng', ctpn.don_gia AS 'Đơn Giá', ctpn.baohanhnhacungcap AS 'Bảo hành nhà cung cấp' FROM phieunhap pn JOIN chitietphieunhap ctpn ON pn.maPhieu = ctpn.maPhieu JOIN sanpham sp ON sp.id = ctpn.maMay";

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(sql, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable table = new DataTable();
                adapter.Fill(table);
                dgvPhieunhap.DataSource = table;

                foreach (DataGridViewColumn col in dgvPhieunhap.Columns)
                {
                    if (col.Name == "Mã Phiếu" || col.Name == "Mã Máy" || col.Name == "Thời Gian Tạo")
                    {
                        col.ReadOnly = true;
                    }
                }
            }
            dgvPhieunhap.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void btnXuatfile_Click(object sender, EventArgs e)
        {
            if (dgvPhieunhap.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một dòng trong bảng phiếu nhập.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maPhieu = dgvPhieunhap.SelectedRows[0].Cells["Mã Phiếu"].Value.ToString();

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Word Document (*.docx)|*.docx",
                Title = "Chọn nơi lưu phiếu nhập",
                FileName = $"PhieuNhap_{maPhieu}.docx"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                XuatPhieuNhapWord(maPhieu, saveFileDialog.FileName);
            }
        }

        private void XuatPhieuNhapWord(string maPhieu, string filePath)
        {
            try
            {
                string sqlPhieu = "SELECT * FROM phieunhap WHERE maPhieu = @maPhieu";
                string sqlChiTiet = @" SELECT ctpn.maMay, sp.ten_sanpham, ctpn.maKho, ctpn.so_luong, ctpn.don_gia 
                                        FROM chitietphieunhap ctpn
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
                    MessageBox.Show("Không tìm thấy phiếu nhập!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using (var doc = DocX.Create(filePath))
                {
                    doc.InsertParagraph("CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM")
                        .Font("Times New Roman").Bold().FontSize(12).Alignment = Alignment.center;
                    doc.InsertParagraph("Độc lập - Tự do - Hạnh phúc\n")
                        .Font("Times New Roman").Bold().FontSize(11).Alignment = Alignment.center;

                    doc.InsertParagraph("PHIẾU NHẬP")
                        .Font("Times New Roman").Bold().FontSize(16).SpacingBefore(20).SpacingAfter(20).Alignment = Alignment.center;

                    var row = tblPhieu.Rows[0];
                    string info = $"Mã phiếu: {row["maPhieu"]}\n"
                                + $"Ngày tạo: {Convert.ToDateTime(row["thoi_gian_tao"]).ToString("dd/MM/yyyy HH:mm")}\n"
                                + $"Người tạo: {row["nguoi_tao"]}\n"
                                + $"Nhà cung cấp: {row["ma_nha_cung_cap"]}\n"
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

                MessageBox.Show("Xuất phiếu nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xuất phiếu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            FormTaoPhieuNhap form = new FormTaoPhieuNhap();
            form.Show();
        }

        private void btnTimkiem_Click(object sender, EventArgs e)
        {
            string keyword = txtTimkiem.Text.Trim();

            string sql = @" SELECT 
                                    pn.maPhieu, 
                                    pn.thoi_gian_tao, 
                                    pn.nguoi_tao, 
                                    pn.ma_nha_cung_cap, 
                                    pn.tong_tien,
                                    sp.id AS maMay,
                                    ctpn.maKho,
                                    ctpn.so_luong,
                                    ctpn.don_gia
                                FROM phieunhap pn
                                JOIN chitietphieunhap ctpn ON pn.maPhieu = ctpn.maPhieu
                                JOIN sanpham sp ON sp.id = ctpn.maMay
                                WHERE pn.maPhieu LIKE @kw
                                   OR pn.thoi_gian_tao LIKE @kw
                                   OR pn.nguoi_tao LIKE @kw
                                   OR pn.ma_nha_cung_cap LIKE @kw
                                   OR pn.tong_tien LIKE @kw
                                   OR sp.id LIKE @kw
                                   OR ctpn.maKho LIKE @kw
                                   OR ctpn.so_luong LIKE @kw
                                   OR ctpn.don_gia LIKE @kw";

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(sql, conn);
                command.Parameters.AddWithValue("@kw", $"%{keyword}%");

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable table = new DataTable();
                adapter.Fill(table);

                dgvPhieunhap.DataSource = table;
                dgvPhieunhap.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvPhieunhap.CurrentRow != null)
            {
                string maPhieu = dgvPhieunhap.CurrentRow.Cells["Mã Phiếu"].Value.ToString();
                string maMay = dgvPhieunhap.CurrentRow.Cells["Mã Máy"].Value.ToString();

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    try
                    {
                        conn.Open();

                        string checkConstraintSql = "SELECT COUNT(*) FROM phieubaohanh pbh join chitietbaohanh ctbh on pbh.ma_phieu_bao_hanh=ctbh.ma_phieu_bao_hanh WHERE pbh.ma_phieu_nhap = @maPhieu AND ctbh.ma_san_pham = @maMay";
                        SqlCommand checkCmd = new SqlCommand(checkConstraintSql, conn);
                        checkCmd.Parameters.AddWithValue("@maPhieu", maPhieu);
                        checkCmd.Parameters.AddWithValue("@maMay", maMay);

                        int constraintCount = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (constraintCount > 0)
                        {
                            MessageBox.Show("Không thể xóa vì sản phẩm này đã có trong phiếu bảo hành!",
                                            "Lỗi ràng buộc", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        DialogResult result = MessageBox.Show(
                            $"Bạn có chắc muốn xoá sản phẩm {maMay} trong phiếu {maPhieu}?",
                            "Xác nhận xoá", MessageBoxButtons.YesNo, MessageBoxIcon.Warning
                        );

                        if (result == DialogResult.Yes)
                        {
                            string query = "DELETE FROM chitietphieunhap WHERE maPhieu = @maPhieu AND maMay = @maMay";
                            SqlCommand cmd = new SqlCommand(query, conn);
                            cmd.Parameters.AddWithValue("@maPhieu", maPhieu);
                            cmd.Parameters.AddWithValue("@maMay", maMay);

                            int rowsAffected = cmd.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Xoá thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                string checkRemainingSql = "SELECT COUNT(*) FROM chitietphieunhap WHERE maPhieu = @maPhieu";
                                SqlCommand remainingCmd = new SqlCommand(checkRemainingSql, conn);
                                remainingCmd.Parameters.AddWithValue("@maPhieu", maPhieu);
                                int remaining = Convert.ToInt32(remainingCmd.ExecuteScalar());

                                if (remaining == 0)
                                {
                                    string sqlDeletePhieu = "DELETE FROM phieunhap WHERE maPhieu = @maPhieu";
                                    using (SqlCommand cmdDel = new SqlCommand(sqlDeletePhieu, conn))
                                    {
                                        cmdDel.Parameters.AddWithValue("@maPhieu", maPhieu);
                                        cmdDel.ExecuteNonQuery();
                                    }
                                }

                                napdgvphieunhap();
                            }
                            else
                            {
                                MessageBox.Show("Không tìm thấy dòng cần xoá!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn dòng cần xoá!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dgvPhieunhap_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string maPhieu = dgvPhieunhap.Rows[e.RowIndex].Cells["Mã Phiếu"].Value.ToString();
            string maMay = dgvPhieunhap.Rows[e.RowIndex].Cells["Mã Máy"].Value.ToString();

            string maKho = dgvPhieunhap.Rows[e.RowIndex].Cells["Mã Kho"].Value.ToString();
            int soLuong = Convert.ToInt32(dgvPhieunhap.Rows[e.RowIndex].Cells["Số Lượng"].Value);
            int donGia = Convert.ToInt32(dgvPhieunhap.Rows[e.RowIndex].Cells["Đơn Giá"].Value);
            string baoHanh = dgvPhieunhap.Rows[e.RowIndex].Cells["Bảo hành nhà cung cấp"].Value.ToString();

            string sql = @"UPDATE chitietphieunhap 
                   SET maKho = @maKho,
                       so_luong = @soLuong,
                       don_gia = @donGia,
                       baohanhnhacungcap = @baoHanh
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
                    cmd.Parameters.AddWithValue("@baoHanh", baoHanh);
                    cmd.Parameters.AddWithValue("@maPhieu", maPhieu);
                    cmd.Parameters.AddWithValue("@maMay", maMay);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi cập nhật: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            napdgvphieunhap();
        }

        private void dgvPhieunhap_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}