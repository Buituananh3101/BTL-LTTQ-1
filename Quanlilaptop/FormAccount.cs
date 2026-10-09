using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Microsoft.Data.SqlClient; // Đổi sang thư viện SQL Server
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace Quanlilaptop
{
    public partial class FormAccount : Form
    {
        // Chuỗi kết nối SQL Server chung
        private string connString = "Server=localhost\\SQLEXPRESS;Database=quanlimaytinh;Integrated Security=true;TrustServerCertificate=True;";

        public FormAccount()
        {
            InitializeComponent();
            napdgvtaikhoan();
        }

        public void napdgvtaikhoan()
        {
            string sql = "SELECT userName AS 'Tên Đăng Nhập', password AS 'Mật Khẩu', email AS 'Email', chuc_vu AS 'Chức Vụ' FROM account;";
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(sql, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable table = new DataTable();
                adapter.Fill(table);
                dgvAccount.DataSource = table;
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            ThemTaiKhoan form = new ThemTaiKhoan(this);
            form.Show();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            FormSuaAccount form = new FormSuaAccount(this);
            form.Show();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvAccount.SelectedRows.Count > 0)
            {
                string userName = dgvAccount.SelectedRows[0].Cells["Tên Đăng Nhập"].Value.ToString();

                DialogResult result = MessageBox.Show($"Bạn có chắc chắn muốn xóa tài khoản {userName} không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        try
                        {
                            conn.Open();
                            string sql = "DELETE FROM account WHERE userName = @userName";
                            SqlCommand command = new SqlCommand(sql, conn);
                            command.Parameters.AddWithValue("@userName", userName);

                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Xóa tài khoản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                napdgvtaikhoan();
                            }
                            else
                            {
                                MessageBox.Show("Không tìm thấy tài khoản để xóa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Lỗi khi xóa tài khoản: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn tài khoản cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnTimkiem_Click(object sender, EventArgs e)
        {
            try
            {
                // Dùng toán tử cộng chuỗi '+' của SQL Server cho phần tìm kiếm LIKE
                string sql = "SELECT userName AS 'username', password AS 'password', email AS 'email', chuc_vu AS 'role' FROM account WHERE userName LIKE '%' + @userName + '%'";

                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("@userName", txtTimkiem.Text);

                dgvAccount.DataSource = Database.Query(sql, parameters);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void FormAccount_Load(object sender, EventArgs e)
        {

        }

        private void btnCapnhat_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Word Document (*.docx)|*.docx",
                Title = "Chọn nơi lưu file Word",
                FileName = "DanhSachTaiKhoan.docx"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string filePath = saveFileDialog.FileName;

                    if (dgvAccount.Rows.Count == 0)
                    {
                        MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    using (DocX document = DocX.Create(filePath))
                    {
                        var title = document.InsertParagraph("DANH SÁCH TÀI KHOẢN")
                                            .Bold()
                                            .FontSize(16)
                                            .Alignment = Alignment.center;

                        document.InsertParagraph("\n");

                        var table = document.AddTable(dgvAccount.Rows.Count + 1, 4);
                        table.Design = TableDesign.TableGrid;

                        table.Rows[0].Cells[0].Paragraphs[0].Append("Username").Bold();
                        table.Rows[0].Cells[1].Paragraphs[0].Append("Password").Bold();
                        table.Rows[0].Cells[2].Paragraphs[0].Append("Email").Bold();
                        table.Rows[0].Cells[3].Paragraphs[0].Append("Chức vụ").Bold();

                        int rowIndex = 1;
                        foreach (DataGridViewRow row in dgvAccount.Rows)
                        {
                            if (!row.IsNewRow)
                            {
                                table.Rows[rowIndex].Cells[0].Paragraphs[0].Append(row.Cells["Tên Đăng Nhập"].Value?.ToString() ?? "");
                                table.Rows[rowIndex].Cells[1].Paragraphs[0].Append(row.Cells["Mật Khẩu"].Value?.ToString() ?? "");
                                table.Rows[rowIndex].Cells[2].Paragraphs[0].Append(row.Cells["Email"].Value?.ToString() ?? "");
                                table.Rows[rowIndex].Cells[3].Paragraphs[0].Append(row.Cells["Chức Vụ"].Value?.ToString() ?? "");
                                rowIndex++;
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

        private void dgvAccount_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}