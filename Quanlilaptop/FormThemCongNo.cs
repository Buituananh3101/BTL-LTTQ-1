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
    public partial class FormThemCongNo : Form
    {
        private FormCongNo formCN;
        public FormThemCongNo(FormCongNo form)
        {
            InitializeComponent();
            LoadmaphieunhapToComboBox();
            formCN = form;
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void LoadmaphieunhapToComboBox()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection("Server=localhost;Database=quanlimaytinh;Port=3306;User ID=root;Password="))
                {
                    conn.Open();
                    string query = "SELECT maPhieu FROM phieunhap";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        cbbMaphieunhap.Items.Clear();

                        while (reader.Read())
                        {
                            cbbMaphieunhap.Items.Add(reader["maPhieu"].ToString());
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
            string maCongNo = txtMaCN.Text.Trim();

            if (string.IsNullOrEmpty(maCongNo))
            {
                MessageBox.Show("Vui lòng nhập mã công nợ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cbbMaphieunhap.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn Mã Phiếu Nhập.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maPhieuNhap = cbbMaphieunhap.SelectedItem.ToString();
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

                    string insertQuery = @"
                INSERT INTO congnonhacungcap
                (ma_cong_no, ma_phieu_nhap, ngay_phat_sinh, trang_thai, ghi_chu)
                VALUES
                (@maCongNo, @maPhieuNhap, @ngayPhatSinh, @trangThai, @ghiChu)";

                    MySqlCommand cmdInsert = new MySqlCommand(insertQuery, conn);
                    cmdInsert.Parameters.AddWithValue("@maCongNo", maCongNo);
                    cmdInsert.Parameters.AddWithValue("@maPhieuNhap", maPhieuNhap);
                    cmdInsert.Parameters.AddWithValue("@ngayPhatSinh", ngayPhatSinh);
                    cmdInsert.Parameters.AddWithValue("@trangThai", trangThai);
                    cmdInsert.Parameters.AddWithValue("@ghiChu", ghiChu);

                    int rowsAffected = cmdInsert.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Thêm công nợ nhà cung cấp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        formCN.napdgvcongno();
                    }
                    else
                    {
                        MessageBox.Show("Thêm công nợ thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm công nợ: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormThemCongNo_Load(object sender, EventArgs e)
        {

        }
    }
}
