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
    public partial class FormKhachHang : Form
    {
        // Chuỗi kết nối SQL Server chung cho form
        private string connString = "Server=localhost\\SQLEXPRESS;Database=quanlimaytinh;Integrated Security=true;TrustServerCertificate=True;";

        public FormKhachHang()
        {
            InitializeComponent();
            napdgvkhachhang();
        }

        public void napdgvkhachhang()
        {
            string sql = "SELECT id_khach_hang AS 'ID Khách Hàng', ho_ten AS 'Họ Tên', Sdt AS 'Số Điện Thoại', dia_chi AS 'Địa Chỉ' FROM khachhang;";
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(sql, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable table = new DataTable();
                adapter.Fill(table);
                dgvCustomer.DataSource = table;
            }
        }

        private void btnThemKH_Click(object sender, EventArgs e)
        {
            FormThemKhachHang form = new FormThemKhachHang(this);
            form.Show();
        }

        private void btnSuaKH_Click(object sender, EventArgs e)
        {
            FormSuaKhachHang form = new FormSuaKhachHang(this);
            form.Show();
        }

        private void btnXoaKH_Click(object sender, EventArgs e)
        {
            if (dgvCustomer.SelectedRows.Count > 0)
            {
                string idKhachHang = dgvCustomer.SelectedRows[0].Cells["ID Khách Hàng"].Value.ToString();

                DialogResult result = MessageBox.Show($"Bạn có chắc chắn muốn xóa khách hàng có ID {idKhachHang} không?",
                                                      "Xác nhận xóa",
                                                      MessageBoxButtons.YesNo,
                                                      MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        try
                        {
                            conn.Open();
                            string sql = "DELETE FROM khachhang WHERE id_khach_hang = @id";
                            SqlCommand command = new SqlCommand(sql, conn);
                            command.Parameters.AddWithValue("@id", idKhachHang);

                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Xóa khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                napdgvkhachhang();
                            }
                            else
                            {
                                MessageBox.Show("Không tìm thấy khách hàng để xóa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Lỗi khi xóa khách hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnWordKH_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Word Document (*.docx)|*.docx",
                Title = "Chọn nơi lưu file Word",
                FileName = "DanhSachKhachHang.docx"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string filePath = saveFileDialog.FileName;

                    DataTable tableKH = new DataTable();
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        conn.Open();
                        string sql = "SELECT id_khach_hang, ho_ten, sdt, dia_chi FROM khachhang";
                        SqlCommand command = new SqlCommand(sql, conn);
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        adapter.Fill(tableKH);
                    }

                    if (tableKH.Rows.Count == 0)
                    {
                        MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    using (DocX document = DocX.Create(filePath))
                    {
                        var title = document.InsertParagraph("DANH SÁCH KHÁCH HÀNG")
                                            .Bold()
                                            .FontSize(16)
                                            .Alignment = Alignment.center;

                        document.InsertParagraph("\n");

                        var table = document.AddTable(tableKH.Rows.Count + 1, 4);
                        table.Design = TableDesign.TableGrid;

                        string[] headers = { "Mã KH", "Họ Tên", "SĐT", "Địa Chỉ" };
                        for (int i = 0; i < headers.Length; i++)
                        {
                            table.Rows[0].Cells[i].Paragraphs[0].Append(headers[i]).Bold();
                        }

                        for (int rowIndex = 0; rowIndex < tableKH.Rows.Count; rowIndex++)
                        {
                            for (int colIndex = 0; colIndex < tableKH.Columns.Count; colIndex++)
                            {
                                table.Rows[rowIndex + 1].Cells[colIndex].Paragraphs[0].Append(tableKH.Rows[rowIndex][colIndex]?.ToString() ?? "");
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

        private void btnTK_Click(object sender, EventArgs e)
        {
            try
            {
                string keyword = txtTKKH.Text.Trim();

                string sql = @"SELECT id_khach_hang, ho_ten, Sdt, dia_chi 
                   FROM khachhang 
                   WHERE id_khach_hang LIKE @keyword
                      OR ho_ten LIKE @keyword
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

                    dgvCustomer.DataSource = table;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}