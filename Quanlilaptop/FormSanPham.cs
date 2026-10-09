using System;
using System.Data;
using System.Windows.Forms;
using Microsoft.Data.SqlClient; // Đổi sang thư viện SQL Server
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace Quanlilaptop
{
    public partial class FormSanPham : Form
    {
        // Chuỗi kết nối SQL Server chung cho form
        private string connString = "Server=localhost\\SQLEXPRESS;Database=quanlimaytinh;Integrated Security=true;TrustServerCertificate=True;";

        public FormSanPham()
        {
            InitializeComponent();
            napdgvsanpham();
            dgvSanpham.CellEndEdit += dgvSanpham_CellEndEdit;
        }

        public void napdgvsanpham()
        {
            string sql = "SELECT id AS 'ID', ten_sanpham AS 'Tên sản phẩm', loai AS 'Loại', co_the_ban AS 'Có thể bán', ton_kho AS 'Tồn kho', ngay_khoi_tao AS 'Ngày khởi tạo', donvitinh AS 'Đơn vị tính', chitietsanpham AS 'Chi tiết sản phẩm', don_gia AS 'Đơn giá', baohanhcuahang AS 'Bảo hành cửa hàng' FROM sanpham;";

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(sql, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable table = new DataTable();
                adapter.Fill(table);
                dgvSanpham.DataSource = table;

                foreach (DataGridViewColumn col in dgvSanpham.Columns)
                {
                    if (col.Name == "ID")
                    {
                        col.ReadOnly = true;
                    }
                }
            }
        }

        private void btnXuatfile_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Word Document (*.docx)|*.docx",
                Title = "Chọn nơi lưu file Word",
                FileName = "DanhSachSanPham.docx"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string filePath = saveFileDialog.FileName;

                    DataTable tableSanPham = new DataTable();
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        conn.Open();
                        string sql = "SELECT id, ten_sanpham, loai, co_the_ban, ton_kho, ngay_khoi_tao, donvitinh, chitietsanpham, don_gia, baohanhcuahang FROM sanpham";
                        SqlCommand command = new SqlCommand(sql, conn);
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        adapter.Fill(tableSanPham);
                    }

                    if (tableSanPham.Rows.Count == 0)
                    {
                        MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    using (DocX document = DocX.Create(filePath))
                    {
                        var title = document.InsertParagraph("DANH SÁCH SẢN PHẨM")
                                           .Bold()
                                           .FontSize(16)
                                           .Alignment = Alignment.center;

                        document.InsertParagraph("\n");

                        var table = document.AddTable(tableSanPham.Rows.Count + 1, 10);
                        table.Design = TableDesign.TableGrid;

                        string[] headers = { "ID", "Tên sản phẩm", "Loại", "Có thể bán", "Tồn kho", "Ngày khởi tạo", "Đơn vị tính", "Chi tiết", "Đơn giá", "Bảo hành" };
                        for (int i = 0; i < headers.Length; i++)
                        {
                            table.Rows[0].Cells[i].Paragraphs[0].Append(headers[i]).Bold();
                        }

                        for (int rowIndex = 0; rowIndex < tableSanPham.Rows.Count; rowIndex++)
                        {
                            for (int colIndex = 0; colIndex < tableSanPham.Columns.Count; colIndex++)
                            {
                                table.Rows[rowIndex + 1].Cells[colIndex].Paragraphs[0].Append(tableSanPham.Rows[rowIndex][colIndex]?.ToString() ?? "");
                            }
                        }
                        document.InsertTable(table);
                        document.Save();
                    }

                    MessageBox.Show("Xuất file Word thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xuất Word: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnTimkiem_Click(object sender, EventArgs e)
        {
            try
            {
                string keyword = txtTimkiem.Text.Trim();

                // Đã chuyển đổi các cột số/ngày sang VARCHAR để dùng được toán tử LIKE trong SQL Server
                string sql = @"SELECT id, ten_sanpham, loai, co_the_ban, ton_kho, ngay_khoi_tao, donvitinh, chitietsanpham 
                       FROM sanpham 
                       WHERE id LIKE @keyword
                          OR ten_sanpham LIKE @keyword
                          OR loai LIKE @keyword
                          OR CAST(co_the_ban AS VARCHAR(50)) LIKE @keyword
                          OR CAST(ton_kho AS VARCHAR(50)) LIKE @keyword
                          OR CAST(ngay_khoi_tao AS VARCHAR(50)) LIKE @keyword
                          OR donvitinh LIKE @keyword
                          OR chitietsanpham LIKE @keyword";

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand(sql, conn);
                    command.Parameters.AddWithValue("@keyword", "%" + keyword + "%");

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    dgvSanpham.DataSource = table;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            FormThongTinSP form = new FormThongTinSP(this);
            form.Show();
        }

        private void btnSuaSL_Click(object sender, EventArgs e)
        {
            FormSuaSL form = new FormSuaSL(this);
            form.Show();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            FormSuaSanPham form = new FormSuaSanPham(this);
            form.Show();
        }

        private void btnKho_Click(object sender, EventArgs e)
        {
            FormKho form = new FormKho();
            form.Show();
        }

        private void dgvSanpham_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            try
            {
                string id = dgvSanpham.Rows[e.RowIndex].Cells["ID"].Value?.ToString();
                if (string.IsNullOrEmpty(id)) return;

                string tenSP = dgvSanpham.Rows[e.RowIndex].Cells["Tên sản phẩm"].Value?.ToString();
                string loai = dgvSanpham.Rows[e.RowIndex].Cells["Loại"].Value?.ToString();
                string donViTinh = dgvSanpham.Rows[e.RowIndex].Cells["Đơn vị tính"].Value?.ToString();
                string chiTiet = dgvSanpham.Rows[e.RowIndex].Cells["Chi tiết sản phẩm"].Value?.ToString();

                string ngayKhoiTao = dgvSanpham.Rows[e.RowIndex].Cells["Ngày khởi tạo"].Value?.ToString();
                string baoHanh = dgvSanpham.Rows[e.RowIndex].Cells["Bảo hành cửa hàng"].Value?.ToString();

                int coTheBan = 0, tonKho = 0, donGia = 0;
                int.TryParse(dgvSanpham.Rows[e.RowIndex].Cells["Có thể bán"].Value?.ToString(), out coTheBan);
                int.TryParse(dgvSanpham.Rows[e.RowIndex].Cells["Tồn kho"].Value?.ToString(), out tonKho);
                int.TryParse(dgvSanpham.Rows[e.RowIndex].Cells["Đơn giá"].Value?.ToString(), out donGia);

                string sql = @"UPDATE sanpham SET 
                        ten_sanpham = @ten,
                        loai = @loai,
                        donvitinh = @dvt,
                        chitietsanpham = @chitiet,
                        ngay_khoi_tao = @ngay,
                        co_the_ban = @coTheBan,
                        ton_kho = @tonKho,
                        don_gia = @donGia,
                        baohanhcuahang = @baoHanh
                       WHERE id = @id";

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@ten", tenSP ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@loai", loai ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@dvt", donViTinh ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@chitiet", chiTiet ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@ngay", DateTime.TryParse(ngayKhoiTao, out DateTime date) ? date : (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@coTheBan", coTheBan);
                        cmd.Parameters.AddWithValue("@tonKho", tonKho);
                        cmd.Parameters.AddWithValue("@donGia", donGia);
                        cmd.Parameters.AddWithValue("@baoHanh", baoHanh ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                napdgvsanpham();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}