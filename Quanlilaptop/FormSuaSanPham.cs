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
    public partial class FormSuaSanPham : Form
    {
        private FormSanPham formSP;
        public FormSuaSanPham(FormSanPham form)
        {
            InitializeComponent();
            LoadidsanphamToComboBox();
            formSP = form;
        }
        private void LoadidsanphamToComboBox()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection("Server=localhost;Database=quanlimaytinh;Port=3306;User ID=root;Password="))
                {
                    conn.Open();
                    string query = "SELECT id FROM sanpham";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        cbbIDSP.Items.Clear();

                        while (reader.Read())
                        {
                            cbbIDSP.Items.Add(reader["id"].ToString());
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

        private void btnSua_Click(object sender, EventArgs e)
        {
            string idSP = cbbIDSP.Text.Trim();
            string tenSP = txtTenSP.Text.Trim();
            string loai = txtLoai.Text.Trim();
            string donViTinh = txtDonvitinh.Text.Trim();
            string chiTietSP = txtThongtin.Text.Trim();
            string baoHanhCH = txtBaohanhCH.Text.Trim();

            if (string.IsNullOrEmpty(idSP))
            {
                MessageBox.Show("Vui lòng chọn ID sản phẩm cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection("Server=localhost;Database=quanlimaytinh;Port=3306;User ID=root;Password="))
                {
                    conn.Open();
                    string sql = @"UPDATE sanpham 
                           SET ten_sanpham = @ten,
                               loai = @loai,
                               donvitinh = @donvi,
                               chitietsanpham = @chitiet,
                               baohanhcuahang = @baohanh
                           WHERE id = @id";

                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@ten", tenSP);
                    cmd.Parameters.AddWithValue("@loai", loai);
                    cmd.Parameters.AddWithValue("@donvi", donViTinh);
                    cmd.Parameters.AddWithValue("@chitiet", chiTietSP);
                    cmd.Parameters.AddWithValue("@baohanh", baoHanhCH);
                    cmd.Parameters.AddWithValue("@id", idSP);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Cập nhật sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        formSP.napdgvsanpham();
                        this.Close(); 
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy sản phẩm để cập nhật!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cbbIDSP_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            string idSP = cbbIDSP.SelectedItem.ToString();

            using (MySqlConnection conn = new MySqlConnection("Server=localhost;Database=quanlimaytinh;Port=3306;User ID=root;Password="))
            {
                conn.Open();
                string sql = "SELECT ten_sanpham, loai, donvitinh, chitietsanpham, baohanhcuahang FROM sanpham WHERE id = @id";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", idSP);
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtTenSP.Text = reader["ten_sanpham"].ToString();
                    txtLoai.Text = reader["loai"].ToString();
                    txtDonvitinh.Text = reader["donvitinh"].ToString();
                    txtThongtin.Text = reader["chitietsanpham"].ToString();
                    txtBaohanhCH.Text = reader["baohanhcuahang"].ToString();
                }
                reader.Close();
            }
        }
    }
}
