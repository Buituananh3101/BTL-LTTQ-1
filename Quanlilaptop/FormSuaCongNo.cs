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

namespace Quanlilaptop
{
    public partial class FormSuaCongNo : Form
    {
        private FormCongNo formCN;

        // Khai báo chuỗi kết nối chung cho SQL Server
        private string connString = "Server=localhost\\SQLEXPRESS;Database=quanlimaytinh;Integrated Security=true;TrustServerCertificate=True;";

        public FormSuaCongNo(FormCongNo form)
        {
            InitializeComponent();
            LoadmacongnoToComboBox();
            formCN = form;
        }

        private void LoadmacongnoToComboBox()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    string query = "SELECT ma_cong_no FROM congnonhacungcap"; //[cite: 1]

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        cbbMacongno.Items.Clear();

                        while (reader.Read())
                        {
                            cbbMacongno.Items.Add(reader["ma_cong_no"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load ComboBox: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbbMacongno_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbbMacongno.SelectedItem == null) return;

            string maCongNo = cbbMacongno.SelectedItem.ToString();

            string query = @"SELECT ma_phieu_nhap, ngay_phat_sinh, trang_thai, ghi_chu
                             FROM congnonhacungcap
                             WHERE ma_cong_no = @maCongNo"; //[cite: 1]

            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@maCongNo", maCongNo);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string maPhieuNhap = reader["ma_phieu_nhap"].ToString();
                            DateTime ngayPhatSinh = Convert.ToDateTime(reader["ngay_phat_sinh"]);
                            string trangThai = reader["trang_thai"].ToString();
                            string ghiChu = reader["ghi_chu"].ToString();
                            txtMaphieunhap.Text = maPhieuNhap;
                            dtpCN.Value = ngayPhatSinh;
                            txtTrangthai.Text = trangThai;
                            txtGhichu.Text = ghiChu;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi load thông tin công nợ: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (cbbMacongno.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn mã công nợ để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maCongNo = cbbMacongno.SelectedItem.ToString();
            if (string.IsNullOrEmpty(maCongNo))
            {
                MessageBox.Show("Mã công nợ không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string maPhieuNhap = txtMaphieunhap.Text.Trim();
            DateTime ngayPhatSinh = dtpCN.Value;
            string trangThai = txtTrangthai.Text.Trim();
            string ghiChu = txtGhichu.Text.Trim();

            if (string.IsNullOrEmpty(trangThai))
            {
                MessageBox.Show("Vui lòng nhập trạng thái.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    string updateQuery = @"
                        UPDATE congnonhacungcap
                        SET 
                            ma_phieu_nhap = @maPhieuNhap,
                            ngay_phat_sinh = @ngayPhatSinh,
                            trang_thai = @trangThai,
                            ghi_chu = @ghiChu
                        WHERE ma_cong_no = @maCongNo"; //[cite: 1]

                    SqlCommand cmdUpdate = new SqlCommand(updateQuery, conn);
                    cmdUpdate.Parameters.AddWithValue("@maCongNo", maCongNo);
                    cmdUpdate.Parameters.AddWithValue("@maPhieuNhap", maPhieuNhap);
                    cmdUpdate.Parameters.AddWithValue("@ngayPhatSinh", ngayPhatSinh);
                    cmdUpdate.Parameters.AddWithValue("@trangThai", trangThai);
                    cmdUpdate.Parameters.AddWithValue("@ghiChu", ghiChu);

                    int rowsAffected = cmdUpdate.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Cập nhật công nợ thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBox.Show("Lỗi khi cập nhật công nợ: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}