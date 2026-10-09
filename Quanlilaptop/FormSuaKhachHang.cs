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
    public partial class FormSuaKhachHang : Form
    {
        private FormKhachHang formKH;
        public FormSuaKhachHang(FormKhachHang form)
        {
            InitializeComponent();
            LoadmakhachhangToComboBox();
            formKH = form;
        }
        private void LoadmakhachhangToComboBox()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection("Server=localhost;Database=quanlimaytinh;Port=3306;User ID=root;Password="))
                {
                    conn.Open();
                    string query = "SELECT id_khach_hang FROM khachhang";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        cbbMaKH.Items.Clear();

                        while (reader.Read())
                        {
                            cbbMaKH.Items.Add(reader["id_khach_hang"].ToString());
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

        private void btnThem_Click(object sender, EventArgs e)
        {
            string idKH = cbbMaKH.Text.Trim();
            string hoTen = txtHoten.Text.Trim();
            string sdt = txtSdt.Text.Trim();
            string diaChi = txtDiachi.Text.Trim();

            if (string.IsNullOrEmpty(idKH))
            {
                MessageBox.Show("Vui lòng chọn mã khách hàng cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection("Server=localhost;Database=quanlimaytinh;Port=3306;User ID=root;Password="))
                {
                    conn.Open();
                    string sql = @"UPDATE khachhang 
                           SET ho_ten = @hoTen,
                               sdt = @sdt,
                               dia_chi = @diaChi
                           WHERE id_khach_hang = @idKH";

                    MySqlCommand command = new MySqlCommand(sql, conn);
                    command.Parameters.AddWithValue("@hoTen", hoTen);
                    command.Parameters.AddWithValue("@sdt", sdt);
                    command.Parameters.AddWithValue("@diaChi", diaChi);
                    command.Parameters.AddWithValue("@idKH", idKH);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Cập nhật thông tin khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        formKH.napdgvkhachhang();
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy khách hàng để cập nhật!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.Close();
        }

        private void cbbMaKH_SelectedIndexChanged(object sender, EventArgs e)
        {
            string idKH = cbbMaKH.SelectedItem.ToString();
            using (MySqlConnection conn = new MySqlConnection("Server=localhost;Database=quanlimaytinh;Port=3306;User ID=root;Password="))
            {
                conn.Open();
                string sql = "SELECT ho_ten, sdt, dia_chi FROM khachhang WHERE id_khach_hang = @id";
                MySqlCommand command = new MySqlCommand(sql, conn);
                command.Parameters.AddWithValue("@id", idKH);
                MySqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    txtHoten.Text = reader["ho_ten"].ToString();
                    txtSdt.Text = reader["sdt"].ToString();
                    txtDiachi.Text = reader["dia_chi"].ToString();
                }
                reader.Close();
            }
        }
    }
}
