using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace Quanlilaptop
{
    public partial class FormSanPham : Form
    {
        public FormSanPham()
        {
            InitializeComponent();
            napdgvsanpham();
            dgvSanpham.CellEndEdit += dgvSanpham_CellEndEdit;
        }
        public void napdgvsanpham()
        {
            string sql = "SELECT id AS 'ID',ten_sanpham AS 'Tên sản phẩm',loai AS 'Loại',co_the_ban AS 'Có thể bán',ton_kho AS 'Tồn kho',ngay_khoi_tao AS 'Ngày khởi tạo',donvitinh AS 'Đơn vị tính',chitietsanpham AS 'Chi tiết sản phẩm',don_gia AS 'Đơn giá',baohanhcuahang AS 'Bảo hành cửa hàng'FROM sanpham;";
            using (MySqlConnection conn = new MySqlConnection("Server=localhost;Database=quanlimaytinh;Port=3306;User ID=root;Password="))
            {
                conn.Open();
                MySqlCommand command = new MySqlCommand(sql, conn);
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
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
                    using (MySqlConnection conn = new MySqlConnection("Server=localhost;Database=quanlimaytinh;Port=3306;User ID=root;Password="))
                    {
                        conn.Open();
                        string sql = "SELECT id, ten_sanpham, loai, co_the_ban, ton_kho, ngay_khoi_tao, donvitinh, chitietsanpham, don_gia, baohanhcuahang FROM sanpham";
                        MySqlCommand command = new MySqlCommand(sql, conn);
                        MySqlDataAdapter adapter = new MySqlDataAdapter(command);
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

                string sql = @"SELECT id, ten_sanpham, loai, co_the_ban, ton_kho, ngay_khoi_tao, donvitinh, chitietsanpham 
                       FROM sanpham 
                       WHERE id LIKE @keyword
                          OR ten_sanpham LIKE @keyword
                          OR loai LIKE @keyword
                          OR co_the_ban LIKE @keyword
                          OR ton_kho LIKE @keyword
                          OR ngay_khoi_tao LIKE @keyword
                          OR donvitinh LIKE @keyword
                          OR chitietsanpham LIKE @keyword";

                using (MySqlConnection conn = new MySqlConnection("Server=localhost;Database=quanlimaytinh;Port=3306;User ID=root;Password="))
                {
                    conn.Open();
                    MySqlCommand command = new MySqlCommand(sql, conn);
                    command.Parameters.AddWithValue("@keyword", "%" + keyword + "%");

                    MySqlDataAdapter adapter = new MySqlDataAdapter(command);
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

                using (MySqlConnection conn = new MySqlConnection("Server=localhost;Database=quanlimaytinh;Port=3306;User ID=root;Password=;Allow Zero Datetime=True;Convert Zero Datetime=True"))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@ten", tenSP);
                        cmd.Parameters.AddWithValue("@loai", loai);
                        cmd.Parameters.AddWithValue("@dvt", donViTinh);
                        cmd.Parameters.AddWithValue("@chitiet", chiTiet);
                        cmd.Parameters.AddWithValue("@ngay", DateTime.TryParse(ngayKhoiTao, out DateTime date) ? date : (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@coTheBan", coTheBan);
                        cmd.Parameters.AddWithValue("@tonKho", tonKho);
                        cmd.Parameters.AddWithValue("@donGia", donGia);
                        cmd.Parameters.AddWithValue("@baoHanh", baoHanh);
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

