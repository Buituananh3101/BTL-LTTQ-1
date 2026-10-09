using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace Quanlilaptop
{
    public partial class FormCongNo : Form
    {
        public FormCongNo()
        {
            InitializeComponent();
            napdgvcongno();
        }
        public void napdgvcongno()
        {
            string sql = "SELECT c.ma_cong_no AS 'Mã Công Nợ', c.ma_phieu_nhap AS 'Mã Phiếu Nhập', c.ngay_phat_sinh AS 'Ngày Phát Sinh'," +
                "  c.so_tien_no AS 'Số Tiền Nợ', c.trang_thai AS 'Trạng Thái',  c.ghi_chu AS 'Ghi Chú', " +
                " p.ma_nha_cung_cap AS 'Mã Nhà Cung Cấp', ncc.ten_nha_cung_cap AS 'Tên nhà cung cấp'" +
                "FROM congnonhacungcap c LEFT JOIN phieunhap p ON c.ma_phieu_nhap = p.maPhieu LEFT JOIN nhacungcap ncc ON p.ma_nha_cung_cap = ncc.ma_nha_cung_cap;";
            using (MySqlConnection conn = new MySqlConnection("Server=localhost;Database=quanlimaytinh;Port=3306;User ID=root;Password="))
            {
                conn.Open();
                MySqlCommand command = new MySqlCommand(sql, conn);
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable table = new DataTable();
                adapter.Fill(table);
                dgvCongno.DataSource = table;
            }
        }

        private void btnCapnhat_Click(object sender, EventArgs e)
        {
            if (dgvCongno.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một dòng để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var row = dgvCongno.SelectedRows[0];
            string maCongNo = row.Cells["Mã Công Nợ"].Value?.ToString() ?? "";
            string maPhieuNhap = row.Cells["Mã Phiếu Nhập"].Value?.ToString() ?? "";
            string ngayPhatSinh = row.Cells["Ngày Phát Sinh"].Value?.ToString() ?? "";
            string soTienNo = row.Cells["Số Tiền Nợ"].Value?.ToString() ?? "";
            string trangThai = row.Cells["Trạng Thái"].Value?.ToString() ?? "";
            string ghiChu = row.Cells["Ghi Chú"].Value?.ToString() ?? "";
            string maNhaCungCap = row.Cells["Mã Nhà Cung Cấp"].Value?.ToString() ?? "";
            string tenNhaCungCap = row.Cells["Tên nhà cung cấp"].Value?.ToString() ?? "";

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Word Document (*.docx)|*.docx",
                Title = "Chọn nơi lưu file Word",
                FileName = $"CongNo_{maCongNo}.docx"
            };

            if (saveFileDialog.ShowDialog() != DialogResult.OK) return;

            try
            {
                using (DocX document = DocX.Create(saveFileDialog.FileName))
                {
                    document.InsertParagraph("CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM")
                        .Font("Times New Roman").Bold().FontSize(12).Alignment = Alignment.center;

                    document.InsertParagraph("Độc lập - Tự do - Hạnh phúc\n")
                        .Font("Times New Roman").Bold().FontSize(11).Alignment = Alignment.center;

                    document.InsertParagraph("CHI TIẾT CÔNG NỢ NHÀ CUNG CẤP")
                        .Font("Times New Roman").Bold().FontSize(16).SpacingBefore(20).SpacingAfter(20).Alignment = Alignment.center;
                    document.InsertParagraph($"Mã Công Nợ: {maCongNo}").FontSize(12);
                    document.InsertParagraph($"Mã Phiếu Nhập: {maPhieuNhap}").FontSize(12);
                    document.InsertParagraph($"Ngày Phát Sinh: {ngayPhatSinh}").FontSize(12);
                    document.InsertParagraph($"Số Tiền Nợ: {soTienNo}").FontSize(12);
                    document.InsertParagraph($"Trạng Thái: {trangThai}").FontSize(12);
                    document.InsertParagraph($"Ghi Chú: {ghiChu}").FontSize(12);
                    document.InsertParagraph($"Mã Nhà Cung Cấp: {maNhaCungCap}").FontSize(12);
                    document.InsertParagraph($"Tên Nhà Cung Cấp: {tenNhaCungCap}").FontSize(12);

                    document.InsertParagraph("\n\nNgười lập biểu")
                        .Font("Times New Roman")
                        .FontSize(12)
                        .Italic()
                        .Alignment = Alignment.right;

                    document.Save();
                }

                MessageBox.Show("Xuất file Word thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xuất file Word: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTK_Click(object sender, EventArgs e)
        {
            string keyword = txtTimkiem.Text.Trim();

            if (string.IsNullOrEmpty(keyword))
            {
                MessageBox.Show("Vui lòng nhập từ khóa tìm kiếm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string connectionString = "Server=localhost;Database=quanlimaytinh;Port=3306;User ID=root;Password=";

            string query = @"
                            SELECT
                                c.ma_cong_no AS 'Mã Công Nợ',
                                c.ma_phieu_nhap AS 'Mã Phiếu Nhập',
                                c.ngay_phat_sinh AS 'Ngày Phát Sinh',
                                c.so_tien_no AS 'Số Tiền Nợ',
                                c.trang_thai AS 'Trạng Thái',
                                c.ghi_chu AS 'Ghi Chú',
                                p.ma_nha_cung_cap AS 'Mã Nhà Cung Cấp',
                                n.ten_nha_cung_cap AS 'Tên Nhà Cung Cấp'
                            FROM congnonhacungcap c
                            LEFT JOIN phieunhap p ON c.ma_phieu_nhap = p.maPhieu
                            LEFT JOIN nhacungcap n ON p.ma_nha_cung_cap = n.ma_nha_cung_cap
                            WHERE 
                                c.ma_cong_no LIKE @keyword OR
                                c.ma_phieu_nhap LIKE @keyword OR
                                c.ngay_phat_sinh LIKE @keyword OR
                                c.trang_thai LIKE @keyword OR
                                c.ghi_chu LIKE @keyword OR
                                p.ma_nha_cung_cap LIKE @keyword OR
                                n.ten_nha_cung_cap LIKE @keyword";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@keyword", "%" + keyword + "%");

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dgvCongno.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            FormThemCongNo form = new FormThemCongNo(this);
            form.Show();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvCongno.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một dòng để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string maCongNo = dgvCongno.SelectedRows[0].Cells["Mã Công Nợ"].Value?.ToString();

            if (string.IsNullOrEmpty(maCongNo))
            {
                MessageBox.Show("Mã công nợ không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult result = MessageBox.Show($"Bạn có chắc chắn muốn xóa công nợ có mã '{maCongNo}' không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    string connectionString = "Server=localhost;Database=quanlimaytinh;Port=3306;User ID=root;Password=";

                    using (MySqlConnection conn = new MySqlConnection(connectionString))
                    {
                        conn.Open();

                        string query = "DELETE FROM congnonhacungcap WHERE ma_cong_no = @maCongNo";

                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@maCongNo", maCongNo);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            napdgvcongno();
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy bản ghi để xóa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            FormSuaCongNo form = new FormSuaCongNo(this);
            form.Show();
        }

        private void btnWordfull_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Word Document (*.docx)|*.docx",
                Title = "Chọn nơi lưu file Word",
                FileName = "CongNoNhaCungCap.docx"
            };

            if (saveFileDialog.ShowDialog() != DialogResult.OK) return;

            string filePath = saveFileDialog.FileName;
            string sql = @"
        SELECT
            c.ma_cong_no AS 'Mã Công Nợ',
            c.ma_phieu_nhap AS 'Mã Phiếu Nhập',
            c.ngay_phat_sinh AS 'Ngày Phát Sinh',
            c.so_tien_no AS 'Số Tiền Nợ',
            c.trang_thai AS 'Trạng Thái',
            c.ghi_chu AS 'Ghi Chú',
            p.ma_nha_cung_cap AS 'Mã Nhà Cung Cấp',
            n.ten_nha_cung_cap AS 'Tên Nhà Cung Cấp'
        FROM congnonhacungcap c
        LEFT JOIN phieunhap p ON c.ma_phieu_nhap = p.maPhieu
        LEFT JOIN nhacungcap n ON p.ma_nha_cung_cap = n.ma_nha_cung_cap;
    ";

            DataTable dt = new DataTable();
            try
            {
                using (MySqlConnection conn = new MySqlConnection("Server=localhost;Database=quanlimaytinh;Port=3306;User ID=root;Password="))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    adapter.Fill(dt);
                }

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                using (DocX document = DocX.Create(filePath))
                {
                    document.InsertParagraph("CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM")
                        .Font("Times New Roman").Bold().FontSize(12).Alignment = Alignment.center;

                    document.InsertParagraph("Độc lập - Tự do - Hạnh phúc\n")
                        .Font("Times New Roman").Bold().FontSize(11).Alignment = Alignment.center;

                    document.InsertParagraph("DANH SÁCH CÔNG NỢ NHÀ CUNG CẤP")
                        .Font("Times New Roman").Bold().FontSize(16).SpacingBefore(20).SpacingAfter(20).Alignment = Alignment.center;

                    var table = document.AddTable(dt.Rows.Count + 1, dt.Columns.Count);
                    table.Design = TableDesign.TableGrid;

                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        table.Rows[0].Cells[i].Paragraphs[0].Append(dt.Columns[i].ColumnName).Bold();
                    }

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            table.Rows[i + 1].Cells[j].Paragraphs[0].Append(dt.Rows[i][j]?.ToString() ?? "");
                        }
                    }

                    document.InsertTable(table);

                    document.InsertParagraph("\n\nNgười lập biểu")
                        .Font("Times New Roman")
                        .FontSize(12)
                        .Italic()
                        .Alignment = Alignment.right;

                    document.Save();
                }

                MessageBox.Show("Xuất file Word thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xuất Word: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTimkiem_Click(object sender, EventArgs e)
        {
            FormThanhToanCongNo form = new FormThanhToanCongNo(this);
            form.Show();
        }
    }
}
