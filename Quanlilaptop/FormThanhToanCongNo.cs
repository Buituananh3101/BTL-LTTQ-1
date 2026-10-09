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
    public partial class FormThanhToanCongNo : Form
    {
        private FormCongNo formCN;
        public FormThanhToanCongNo(FormCongNo form)
        {
            InitializeComponent();
            LoadmacongnoToComboBox();
            napdgvThanhtoan();
            formCN = form;
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public void napdgvThanhtoan()
        {
            string sql = "SELECT ma_thanh_toan AS 'Mã Thanh Toán',  ma_cong_no AS 'Mã Công Nợ'," +
                "  ngay_thanh_toan AS 'Ngày Thanh Toán', so_tien_thanh_toan AS 'Số Tiền Thanh Toán', " +
                " phuong_thuc AS 'Phương Thức Thanh Toán',  ghi_chu AS 'Ghi Chú' FROM thanhtoancongno;";
            using (MySqlConnection conn = new MySqlConnection("Server=localhost;Database=quanlimaytinh;Port=3306;User ID=root;Password="))
            {
                conn.Open();
                MySqlCommand command = new MySqlCommand(sql, conn);
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable table = new DataTable();
                adapter.Fill(table);
                dgvThanhtoan.DataSource = table;
            }
        }
        private void LoadmacongnoToComboBox()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection("Server=localhost;Database=quanlimaytinh;Port=3306;User ID=root;Password="))
                {
                    conn.Open();
                    string query = "SELECT ma_cong_no FROM congnonhacungcap";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        cbbMaCN.Items.Clear();

                        while (reader.Read())
                        {
                            cbbMaCN.Items.Add(reader["ma_cong_no"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load ComboBox: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (cbbMaCN.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn Mã Công Nợ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string mathanhToan = txtMaTT.Text.ToString();
            string maCongNo = cbbMaCN.SelectedItem?.ToString();
            DateTime ngayThanhToan = dtpTT.Value;
            decimal soTienThanhToan = 0;
            bool isValid = decimal.TryParse(txtSotien.Text.Trim(), out soTienThanhToan);
            string phuongThuc = txtPhuongthuc.Text.Trim();
            string ghiChu = txtGhichu.Text.Trim();
            if (string.IsNullOrEmpty(maCongNo) || !isValid || soTienThanhToan <= 0 || string.IsNullOrEmpty(phuongThuc))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ và chính xác thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string connStr = "Server=localhost;Database=quanlimaytinh;Port=3306;User ID=root;Password=";
            string insertQuery = @"
        INSERT INTO thanhtoancongno (ma_thanh_toan,ma_cong_no, ngay_thanh_toan, so_tien_thanh_toan, phuong_thuc, ghi_chu)
        VALUES (@mathanhToan,@maCongNo, @ngayThanhToan, @soTienThanhToan, @phuongThuc, @ghiChu)";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(insertQuery, conn);
                    cmd.Parameters.AddWithValue("@mathanhToan", mathanhToan);
                    cmd.Parameters.AddWithValue("@maCongNo", maCongNo);
                    cmd.Parameters.AddWithValue("@ngayThanhToan", ngayThanhToan);
                    cmd.Parameters.AddWithValue("@soTienThanhToan", soTienThanhToan);
                    cmd.Parameters.AddWithValue("@phuongThuc", phuongThuc);
                    cmd.Parameters.AddWithValue("@ghiChu", ghiChu);

                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        MessageBox.Show("Thêm thanh toán công nợ thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        napdgvThanhtoan();
                        formCN.napdgvcongno();
                    }
                    else
                    {
                        MessageBox.Show("Không thể thêm thanh toán công nợ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm thanh toán công nợ: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvThanhtoan.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn phiếu thanh toán cần sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string maThanhToan = dgvThanhtoan.SelectedRows[0].Cells["Mã Thanh Toán"].Value.ToString();
            string maCongNo = cbbMaCN.SelectedItem.ToString();
            DateTime ngayThanhToan = dtpTT.Value;
            decimal soTienThanhToan = Convert.ToDecimal(txtSotien.Text.Trim());
            string phuongThucThanhToan = txtPhuongthuc.Text.Trim();
            string ghiChu = txtGhichu.Text.Trim();

            if (string.IsNullOrEmpty(phuongThucThanhToan) || soTienThanhToan <= 0)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ và chính xác thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string connStr = "Server=localhost;Database=quanlimaytinh;Port=3306;User ID=root;Password=";
            string updateQuery = @"
        UPDATE thanhtoancongno
        SET 
            ma_cong_no = @maCongNo,
            ngay_thanh_toan = @ngayThanhToan,
            so_tien_thanh_toan = @soTienThanhToan,
            phuong_thuc = @phuongThucThanhToan,
            ghi_chu = @ghiChu
        WHERE ma_thanh_toan = @maThanhToan";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(updateQuery, conn);
                    cmd.Parameters.AddWithValue("@maThanhToan", maThanhToan);
                    cmd.Parameters.AddWithValue("@maCongNo", maCongNo);
                    cmd.Parameters.AddWithValue("@ngayThanhToan", ngayThanhToan);
                    cmd.Parameters.AddWithValue("@soTienThanhToan", soTienThanhToan);
                    cmd.Parameters.AddWithValue("@phuongThucThanhToan", phuongThucThanhToan);
                    cmd.Parameters.AddWithValue("@ghiChu", ghiChu);

                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        MessageBox.Show("Cập nhật phiếu thanh toán công nợ thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        napdgvThanhtoan();
                        formCN.napdgvcongno();
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy bản ghi để cập nhật!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật phiếu thanh toán công nợ: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvThanhtoan.SelectedRows.Count > 0)
            {
                string maThanhToan = dgvThanhtoan.SelectedRows[0].Cells["Mã Thanh Toán"].Value.ToString();
                DialogResult result = MessageBox.Show($"Bạn có chắc chắn muốn xóa phiếu thanh toán có mã {maThanhToan}?",
                                                      "Xác nhận xóa",
                                                      MessageBoxButtons.YesNo,
                                                      MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        string connStr = "Server=localhost;Database=quanlimaytinh;Port=3306;User ID=root;Password=";
                        string query = "DELETE FROM thanhtoancongno WHERE ma_thanh_toan = @maThanhToan";

                        using (MySqlConnection conn = new MySqlConnection(connStr))
                        {
                            conn.Open();
                            MySqlCommand cmd = new MySqlCommand(query, conn);
                            cmd.Parameters.AddWithValue("@maThanhToan", maThanhToan);

                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Xóa phiếu thanh toán thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                napdgvThanhtoan();
                                formCN.napdgvcongno();
                            }
                            else
                            {
                                MessageBox.Show("Không tìm thấy phiếu thanh toán để xóa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi xóa phiếu thanh toán: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn phiếu thanh toán cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dgvThanhtoan_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string maThanhToan = dgvThanhtoan.Rows[e.RowIndex].Cells["Mã Thanh Toán"].Value.ToString();
                string maCongNo = dgvThanhtoan.Rows[e.RowIndex].Cells["Mã Công Nợ"].Value.ToString();
                DateTime ngayThanhToan = Convert.ToDateTime(dgvThanhtoan.Rows[e.RowIndex].Cells["Ngày Thanh Toán"].Value);
                decimal soTienThanhToan = Convert.ToDecimal(dgvThanhtoan.Rows[e.RowIndex].Cells["Số Tiền Thanh Toán"].Value);
                string phuongThucThanhToan = dgvThanhtoan.Rows[e.RowIndex].Cells["Phương Thức Thanh Toán"].Value.ToString();
                string ghiChu = dgvThanhtoan.Rows[e.RowIndex].Cells["Ghi Chú"].Value.ToString();

                txtPhuongthuc.Text = phuongThucThanhToan;  
                cbbMaCN.SelectedItem = maCongNo;
                dtpTT.Value = ngayThanhToan;
                txtSotien.Text = soTienThanhToan.ToString();
                txtGhichu.Text = ghiChu;
            }
        }

        private void btnXuat_Click(object sender, EventArgs e)
        {
            if (dgvThanhtoan.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một phiếu thanh toán để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string maThanhToan = dgvThanhtoan.SelectedRows[0].Cells["Mã Thanh Toán"].Value.ToString();
            string maCongNo = dgvThanhtoan.SelectedRows[0].Cells["Mã Công Nợ"].Value.ToString();
            DateTime ngayThanhToan = Convert.ToDateTime(dgvThanhtoan.SelectedRows[0].Cells["Ngày Thanh Toán"].Value);
            decimal soTienThanhToan = Convert.ToDecimal(dgvThanhtoan.SelectedRows[0].Cells["Số Tiền Thanh Toán"].Value);
            string phuongThucThanhToan = dgvThanhtoan.SelectedRows[0].Cells["Phương Thức Thanh Toán"].Value.ToString();
            string ghiChu = dgvThanhtoan.SelectedRows[0].Cells["Ghi Chú"].Value.ToString();
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Word Document (*.docx)|*.docx",
                Title = "Chọn nơi lưu phiếu thanh toán",
                FileName = $"PhieuThanhToan_{maThanhToan}.docx"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                XuatPhieuThanhToanWord(maThanhToan, maCongNo, ngayThanhToan, soTienThanhToan, phuongThucThanhToan, ghiChu, saveFileDialog.FileName);
            }
        }
        private void XuatPhieuThanhToanWord(string maThanhToan, string maCongNo, DateTime ngayThanhToan, decimal soTienThanhToan, string phuongThucThanhToan, string ghiChu, string filePath)
        {
            try
            {
                string query = @"
            SELECT ma_thanh_toan, ma_cong_no, ngay_thanh_toan, so_tien_thanh_toan, phuong_thuc, ghi_chu 
            FROM thanhtoancongno 
            WHERE ma_thanh_toan = @maThanhToan";

                DataTable dt = new DataTable();
                using (MySqlConnection conn = new MySqlConnection("Server=localhost;Database=quanlimaytinh;Port=3306;User ID=root;Password="))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@maThanhToan", maThanhToan);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    adapter.Fill(dt);
                }

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy phiếu thanh toán!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                using (var doc = DocX.Create(filePath))
                {
                    doc.InsertParagraph("CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM")
                        .Font("Times New Roman").Bold().FontSize(12).Alignment = Alignment.center;
                    doc.InsertParagraph("Độc lập - Tự do - Hạnh phúc\n")
                        .Font("Times New Roman").Bold().FontSize(11).Alignment = Alignment.center;

                    doc.InsertParagraph("PHIẾU THANH TOÁN CÔNG NỢ")
                        .Font("Times New Roman").Bold().FontSize(16).SpacingBefore(20).SpacingAfter(20).Alignment = Alignment.center;
                    DataRow row = dt.Rows[0];
                    string info = $"Mã thanh toán: {row["ma_thanh_toan"]}\n"
                                + $"Mã công nợ: {row["ma_cong_no"]}\n"
                                + $"Ngày thanh toán: {Convert.ToDateTime(row["ngay_thanh_toan"]).ToString("dd/MM/yyyy")}\n"
                                + $"Số tiền thanh toán: {string.Format("{0:N0} VNĐ", soTienThanhToan)}\n"
                                + $"Phương thức thanh toán: {row["phuong_thuc"]}\n"
                                + $"Ghi chú: {row["ghi_chu"]}";

                    doc.InsertParagraph(info).Font("Times New Roman").FontSize(12).SpacingAfter(20);

                    var signTable = doc.AddTable(1, 2);
                    signTable.Design = TableDesign.None;

                    signTable.Rows[0].Cells[0].Paragraphs[0]
                        .Append("Nhà Cung Cấp")
                        .Font("Times New Roman")
                        .FontSize(12)
                        .Italic()
                        .Alignment = Alignment.left;

                    signTable.Rows[0].Cells[1].Paragraphs[0]
                        .Append("Người lập phiếu")
                        .Font("Times New Roman")
                        .FontSize(12)
                        .Italic()
                        .Alignment = Alignment.right;

                    doc.InsertParagraph("\n");
                    doc.InsertTable(signTable);

                    doc.Save();
                }

                MessageBox.Show("Xuất phiếu thanh toán thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xuất phiếu thanh toán: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormThanhToanCongNo_Load(object sender, EventArgs e)
        {

        }
    }
}
