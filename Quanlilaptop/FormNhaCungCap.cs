using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.SqlClient; // Đổi sang thư viện SQL Server
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace Quanlilaptop
{
    public partial class FormNhaCungCap : Form
    {
        // Chuỗi kết nối SQL Server chung cho form
        private string connString = "Server=localhost\\SQLEXPRESS;Database=quanlimaytinh;Integrated Security=true;TrustServerCertificate=True;";

        public FormNhaCungCap()
        {
            InitializeComponent();
            napdgvNhaCungCap();
        }

        public void napdgvNhaCungCap()
        {
            string sql = "SELECT ma_nha_cung_cap AS 'Mã Nhà Cung Cấp', ten_nha_cung_cap AS 'Tên Nhà Cung Cấp', Sdt AS 'Số Điện Thoại', dia_chi AS 'Địa Chỉ' FROM nhacungcap;";
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(sql, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable table = new DataTable();
                adapter.Fill(table);
                dgvNhacungcap.DataSource = table;
            }
        }

        private void btnTimkiem_Click(object sender, EventArgs e)
        {
            try
            {
                string keyword = txtTimkiem.Text.Trim();

                string sql = @"SELECT ma_nha_cung_cap, ten_nha_cung_cap, Sdt, dia_chi 
                       FROM nhacungcap 
                       WHERE ma_nha_cung_cap LIKE @keyword
                          OR ten_nha_cung_cap LIKE @keyword
                          OR Sdt LIKE @keyword
                          OR dia_chi LIKE @keyword";

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand(sql, conn);
                    command.Parameters.AddWithValue("@keyword", "%" + keyword + "%");

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    dgvNhacungcap.DataSource = table;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXuatfile_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Word Document (*.docx)|*.docx",
                Title = "Chọn nơi lưu file Word",
                FileName = "DanhSachNhaCungCap.docx"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string filePath = saveFileDialog.FileName;

                    DataTable tableNCC = new DataTable();
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        conn.Open();
                        string sql = "SELECT ma_nha_cung_cap, ten_nha_cung_cap, Sdt, dia_chi FROM nhacungcap";
                        SqlCommand command = new SqlCommand(sql, conn);
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        adapter.Fill(tableNCC);
                    }

                    if (tableNCC.Rows.Count == 0)
                    {
                        MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    using (DocX document = DocX.Create(filePath))
                    {
                        var title = document.InsertParagraph("DANH SÁCH NHÀ CUNG CẤP")
                                            .Bold()
                                            .FontSize(16)
                                            .Alignment = Alignment.center;

                        document.InsertParagraph("\n");

                        var table = document.AddTable(tableNCC.Rows.Count + 1, 4);
                        table.Design = TableDesign.TableGrid;

                        string[] headers = { "Mã NCC", "Tên Nhà Cung Cấp", "SĐT", "Địa Chỉ" };
                        for (int i = 0; i < headers.Length; i++)
                        {
                            table.Rows[0].Cells[i].Paragraphs[0].Append(headers[i]).Bold();
                        }

                        for (int rowIndex = 0; rowIndex < tableNCC.Rows.Count; rowIndex++)
                        {
                            for (int colIndex = 0; colIndex < tableNCC.Columns.Count; colIndex++)
                            {
                                table.Rows[rowIndex + 1].Cells[colIndex].Paragraphs[0].Append(tableNCC.Rows[rowIndex][colIndex]?.ToString() ?? "");
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

        private void btnThem_Click(object sender, EventArgs e)
        {
            FormThemNCC form = new FormThemNCC(this);
            form.Show();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvNhacungcap.SelectedRows.Count > 0)
            {
                string maNCC = dgvNhacungcap.SelectedRows[0].Cells["Mã Nhà Cung Cấp"].Value.ToString();

                DialogResult result = MessageBox.Show($"Bạn có chắc chắn muốn xóa nhà cung cấp {maNCC} không?",
                                                      "Xác nhận",
                                                      MessageBoxButtons.YesNo,
                                                      MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        try
                        {
                            conn.Open();
                            string sql = "DELETE FROM NhaCungCap WHERE ma_nha_cung_cap = @maNCC";
                            SqlCommand command = new SqlCommand(sql, conn);
                            command.Parameters.AddWithValue("@maNCC", maNCC);

                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Xóa nhà cung cấp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                napdgvNhaCungCap();
                            }
                            else
                            {
                                MessageBox.Show("Không tìm thấy nhà cung cấp để xóa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Lỗi khi xóa nhà cung cấp: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn nhà cung cấp cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            FormSuaNCC form = new FormSuaNCC(this);
            form.Show();
        }
    }
}