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
    public partial class FormDoiTra : Form
    {
        public FormDoiTra()
        {
            InitializeComponent();
            napdgvdoitra();
        }
    
    public void napdgvdoitra()

        {
            string sql = "SELECT ma_doi_tra AS 'Mã Đổi Trả',ma_phieu AS 'Mã Phiếu',ma_may AS 'Mã Máy',ma_kho AS 'Mã Kho'," +
                "so_luong AS 'Số Lượng đổi trả',ngay_doi_tra AS 'Ngày Đổi Trả',ly_do AS 'Lý Do', trang_thai AS 'Trạng Thái' FROM doitra";
            using (MySqlConnection conn = new MySqlConnection("Server=localhost;Database=quanlimaytinh;Port=3306;User ID=root;Password="))
            {
                conn.Open();
                MySqlCommand command = new MySqlCommand(sql, conn);
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable table = new DataTable();
                adapter.Fill(table);
                dgvDoitra.DataSource = table;
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
            string connectionString = "Server=localhost;Database=quanlimaytinh;Port=3306;User ID=root;Password=";
            string query = @"
                                SELECT 
                                    ma_doi_tra AS 'Mã Đổi Trả', 
                                    ma_phieu AS 'Mã Phiếu', 
                                    ma_may AS 'Mã Máy', 
                                    ma_kho AS 'Mã Kho', 
                                    so_luong AS 'Số Lượng Đổi Trả', 
                                    ngay_doi_tra AS 'Ngày Đổi Trả', 
                                    ly_do AS 'Lý Do', 
                                    trang_thai AS 'Trạng Thái Đổi Trả'
                                FROM doitra
                                WHERE 
                                    ma_phieu LIKE @keyword OR
                                    ma_may LIKE @keyword OR
                                    ma_kho LIKE @keyword OR
                                    ly_do LIKE @keyword OR
                                    trang_thai LIKE @keyword;";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    adapter.SelectCommand.Parameters.AddWithValue("@keyword", "%" + keyword + "%");
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvDoitra.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvDoitra.SelectedRows.Count > 0)
            {
                string maDoiTra = dgvDoitra.SelectedRows[0].Cells["Mã Đổi Trả"].Value.ToString();

                DialogResult result = MessageBox.Show($"Bạn có chắc chắn muốn xóa phiếu đổi trả có mã {maDoiTra}?",
                                                      "Xác nhận xóa",
                                                      MessageBoxButtons.YesNo,
                                                      MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        string connectionString = "Server=localhost;Database=quanlimaytinh;Port=3306;User ID=root;Password=";
                        string query = "DELETE FROM doitra WHERE ma_doi_tra = @maDoiTra";

                        using (MySqlConnection conn = new MySqlConnection(connectionString))
                        {
                            conn.Open();
                            MySqlCommand cmd = new MySqlCommand(query, conn);
                            cmd.Parameters.AddWithValue("@maDoiTra", maDoiTra);

                            int rowsAffected = cmd.ExecuteNonQuery(); 

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Xóa phiếu đổi trả thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                napdgvdoitra();
                            }
                            else
                            {
                                MessageBox.Show("Không tìm thấy phiếu đổi trả để xóa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi xóa phiếu đổi trả: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn phiếu đổi trả cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnXuat_Click(object sender, EventArgs e)
        {
            if (dgvDoitra.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một phiếu đổi trả để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string maDoiTra = dgvDoitra.SelectedRows[0].Cells["Mã Đổi Trả"].Value.ToString();

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Word Document (*.docx)|*.docx",
                Title = "Chọn nơi lưu phiếu đổi trả",
                FileName = $"PhieuDoiTra_{maDoiTra}.docx" 
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                XuatPhieuDoiTraWord(maDoiTra, saveFileDialog.FileName);
            }
        }

        private void XuatPhieuDoiTraWord(string maDoiTra, string filePath)
        {
            try
            {
                string sqlPhieu = "SELECT * FROM doitra WHERE ma_doi_tra = @maDoiTra";

                string sqlChiTiet = @"
            SELECT 
                ctpx.maMay, sp.ten_sanpham, ctpx.maKho, doitra.so_luong, phieu.thoi_gian_tao AS thoi_gian_xuat,
                kh.ho_ten, kh.Sdt, kh.dia_chi
            FROM chitietphieuxuat ctpx
            JOIN sanpham sp ON ctpx.maMay = sp.id
            JOIN doitra ON doitra.ma_phieu = ctpx.maPhieu
            JOIN phieuxuat phieu ON ctpx.maPhieu = phieu.maPhieu
            JOIN khachhang kh ON phieu.id_khach_hang = kh.id_khach_hang
            WHERE ctpx.maPhieu = @maPhieu";

                DataTable tblPhieu = new DataTable();
                DataTable tblChiTiet = new DataTable();

                using (MySqlConnection conn = new MySqlConnection("Server=localhost;Database=quanlimaytinh;Port=3306;User ID=root;Password="))
                {
                    conn.Open();

                    using (MySqlCommand cmd = new MySqlCommand(sqlPhieu, conn))
                    {
                        cmd.Parameters.AddWithValue("@maDoiTra", maDoiTra);
                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        adapter.Fill(tblPhieu);
                    }

                    if (tblPhieu.Rows.Count == 0)
                    {
                        MessageBox.Show("Không tìm thấy phiếu đổi trả!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string maPhieu = tblPhieu.Rows[0]["ma_phieu"].ToString();

                    using (MySqlCommand cmd = new MySqlCommand(sqlChiTiet, conn))
                    {
                        cmd.Parameters.AddWithValue("@maPhieu", maPhieu);
                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        adapter.Fill(tblChiTiet);
                    }
                }

                using (var doc = DocX.Create(filePath))
                {
                    doc.InsertParagraph("CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM")
                        .Font("Times New Roman").Bold().FontSize(12).Alignment = Alignment.center;
                    doc.InsertParagraph("Độc lập - Tự do - Hạnh phúc\n")
                        .Font("Times New Roman").Bold().FontSize(11).Alignment = Alignment.center;

                    doc.InsertParagraph("PHIẾU ĐỔI TRẢ")
                        .Font("Times New Roman").Bold().FontSize(16).SpacingBefore(20).SpacingAfter(20).Alignment = Alignment.center;

                    DataRow row = tblPhieu.Rows[0];
                    string info = $"Mã phiếu đổi trả: {row["ma_doi_tra"]}\n" +
                                  $"Mã phiếu xuất hàng: {row["ma_phieu"]}\n" +
                                  $"Ngày đổi trả: {Convert.ToDateTime(row["ngay_doi_tra"]).ToString("dd/MM/yyyy")}\n" +
                                  $"Lý do: {row["ly_do"]}\n" +
                                  $"Trạng thái: {row["trang_thai"]}";

                    doc.InsertParagraph(info).Font("Times New Roman").FontSize(12).SpacingAfter(15);

                    if (tblChiTiet.Rows.Count > 0)
                    {
                        string hoTenKH = tblChiTiet.Rows[0]["ho_ten"].ToString();
                        string sdtKH = tblChiTiet.Rows[0]["Sdt"].ToString();
                        string diaChiKH = tblChiTiet.Rows[0]["dia_chi"].ToString();

                        string khInfo = $"Họ và tên khách hàng: {hoTenKH}\nSố điện thoại: {sdtKH}\nĐịa chỉ: {diaChiKH}";
                        doc.InsertParagraph(khInfo)
                            .Font("Times New Roman").FontSize(12).SpacingAfter(20);

                        doc.InsertParagraph("Chi tiết phiếu đổi trả")
                            .Font("Times New Roman").FontSize(14).Bold().Alignment = Alignment.center;

                        var table = doc.AddTable(tblChiTiet.Rows.Count + 1, 5);
                        table.Design = TableDesign.TableGrid;

                        string[] headers = { "Mã máy", "Tên sản phẩm", "Kho", "Số lượng đổi trả", "Thời gian lúc xuất hàng" };
                        for (int i = 0; i < headers.Length; i++)
                            table.Rows[0].Cells[i].Paragraphs[0].Append(headers[i]).Bold();

                        for (int i = 0; i < tblChiTiet.Rows.Count; i++)
                        {
                            table.Rows[i + 1].Cells[0].Paragraphs[0].Append(tblChiTiet.Rows[i]["maMay"].ToString());
                            table.Rows[i + 1].Cells[1].Paragraphs[0].Append(tblChiTiet.Rows[i]["ten_sanpham"].ToString());
                            table.Rows[i + 1].Cells[2].Paragraphs[0].Append(tblChiTiet.Rows[i]["maKho"].ToString());
                            table.Rows[i + 1].Cells[3].Paragraphs[0].Append(tblChiTiet.Rows[i]["so_luong"].ToString());

                            var tgXuat = tblChiTiet.Rows[i]["thoi_gian_xuat"];
                            string tgXuatStr = tgXuat == DBNull.Value ? "" : Convert.ToDateTime(tgXuat).ToString("dd/MM/yyyy HH:mm");
                            table.Rows[i + 1].Cells[4].Paragraphs[0].Append(tgXuatStr);
                        }

                        doc.InsertTable(table);
                    }

                    doc.InsertParagraph("\n\nNgười lập phiếu")
                        .Font("Times New Roman").FontSize(12).Italic().Alignment = Alignment.right;

                    doc.Save();
                }

                MessageBox.Show("Xuất phiếu đổi trả thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xuất phiếu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            FormSuaDoiTra form = new FormSuaDoiTra(this);
            form.Show();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            FormThemDoiTra form = new FormThemDoiTra(this);
            form.Show();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dgvDoitra_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}

