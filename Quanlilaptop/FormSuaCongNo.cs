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

namespace Quanlilaptop
{
    public partial class FormSuaCongNo : Form
    {
        private FormCongNo formCN;
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
                using (MySqlConnection conn = new MySqlConnection("Server=localhost;Database=quanlimaytinh;Port=3306;User ID=root;Password="))
                {
                    conn.Open();
                    string query = "SELECT ma_cong_no FROM congnonhacungcap";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
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
            string connStr = "Server=localhost;Database=quanlimaytinh;Port=3306;User ID=root;Password=";

            string query = @"SELECT ma_phieu_nhap, ngay_phat_sinh, trang_thai, ghi_chu
                     FROM congnonhacungcap
                     WHERE ma_cong_no = @maCongNo";

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@maCongNo", maCongNo);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
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

            string connectionString = "Server=localhost;Database=quanlimaytinh;Port=3306;User ID=root;Password=";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string updateQuery = @"
                UPDATE congnonhacungcap
                SET 
                    ma_phieu_nhap = @maPhieuNhap,
                    ngay_phat_sinh = @ngayPhatSinh,
                    trang_thai = @trangThai,
                    ghi_chu = @ghiChu
                WHERE ma_cong_no = @maCongNo";

                    MySqlCommand cmdUpdate = new MySqlCommand(updateQuery, conn);
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
