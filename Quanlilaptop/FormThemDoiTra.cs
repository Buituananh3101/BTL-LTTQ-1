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
    public partial class FormThemDoiTra : Form
    {
        private FormDoiTra formDT;
        public FormThemDoiTra(FormDoiTra form)
        {
            InitializeComponent();
            LoadmaphieuxuatToComboBox();
            LoadmamayToComboBox();
            formDT = form;
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void LoadmaphieuxuatToComboBox()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection("Server=localhost;Database=quanlimaytinh;Port=3306;User ID=root;Password="))
                {
                    conn.Open();
                    string query = "SELECT DISTINCT maPhieu, maPhieuNhap FROM chitietphieuxuat";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        cbbMaphieuxuat.Items.Clear();

                        while (reader.Read())
                        {
                            string maPhieuXuat = reader["maPhieu"].ToString();
                            string maPhieuNhap = reader["maPhieuNhap"].ToString();

                            string displayText = $"{maPhieuXuat} (PN:{maPhieuNhap})";
                            cbbMaphieuxuat.Items.Add(displayText);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load ComboBox: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadmamayToComboBox()
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
                        cbbMamay.Items.Clear();

                        while (reader.Read())
                        {
                            cbbMamay.Items.Add(reader["id"].ToString());
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
            string maDoiTra = txtMadoitra.Text.Trim();
            string selectedText = cbbMaphieuxuat.SelectedItem?.ToString();
            string maPhieu = selectedText?.Split(' ')[0];  

            string maMay = cbbMamay.SelectedItem?.ToString();
            int soLuong;
            bool parsed = int.TryParse(txtSoluong.Text.Trim(), out soLuong);
            DateTime ngayDoiTra = dtpDT.Value;
            string lyDo = txtLydo.Text.Trim();
            string trangThai = txtTrangthai.Text.Trim();

            if (string.IsNullOrEmpty(maDoiTra) || string.IsNullOrEmpty(maPhieu) || string.IsNullOrEmpty(maMay)
                || !parsed || string.IsNullOrEmpty(lyDo) || string.IsNullOrEmpty(trangThai))
            {
                MessageBox.Show("Vui lòng điền đầy đủ và đúng định dạng các trường!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string connStr = "Server=localhost;Database=quanlimaytinh;Port=3306;User ID=root;Password=";
            string insertQuery = @"
        INSERT INTO doitra (ma_doi_tra, ma_phieu, ma_may, so_luong, ngay_doi_tra, ly_do, trang_thai)
        VALUES (@maDoiTra, @maPhieu, @maMay, @soLuong, @ngayDoiTra, @lyDo, @trangThai)";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    
                    DateTime ngayXuat;
                    string queryNgayXuat = "SELECT thoi_gian_tao FROM phieuxuat WHERE maPhieu = @maPhieu";
                    using (MySqlCommand cmd = new MySqlCommand(queryNgayXuat, conn))
                    {
                        cmd.Parameters.AddWithValue("@maPhieu", maPhieu);
                        object result = cmd.ExecuteScalar();
                        if (result == null || !DateTime.TryParse(result.ToString(), out ngayXuat))
                        {
                            MessageBox.Show("Không tìm thấy ngày xuất cho phiếu này!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    
                    int soThangBaoHanh = 0;
                    string queryBH = "SELECT baohanhcuahang FROM sanpham WHERE id = @maMay";
                    using (MySqlCommand cmd = new MySqlCommand(queryBH, conn))
                    {
                        cmd.Parameters.AddWithValue("@maMay", maMay);
                        object result = cmd.ExecuteScalar();
                        if (result == null || !int.TryParse(result.ToString(), out soThangBaoHanh))
                        {
                            MessageBox.Show("Không tìm thấy thời gian bảo hành cho sản phẩm!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                   
                    DateTime hanBaoHanh = ngayXuat.AddMonths(soThangBaoHanh);

                    if (ngayDoiTra > hanBaoHanh)
                    {
                        MessageBox.Show("Sản phẩm đã hết hạn bảo hành cửa hàng, không thể tạo phiếu đổi trả!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    
                    MySqlCommand insertCmd = new MySqlCommand(insertQuery, conn);
                    insertCmd.Parameters.AddWithValue("@maDoiTra", maDoiTra);
                    insertCmd.Parameters.AddWithValue("@maPhieu", maPhieu);
                    insertCmd.Parameters.AddWithValue("@maMay", maMay);
                    insertCmd.Parameters.AddWithValue("@soLuong", soLuong);
                    insertCmd.Parameters.AddWithValue("@ngayDoiTra", ngayDoiTra);
                    insertCmd.Parameters.AddWithValue("@lyDo", lyDo);
                    insertCmd.Parameters.AddWithValue("@trangThai", trangThai);

                    int resultInsert = insertCmd.ExecuteNonQuery();

                    if (resultInsert > 0)
                    {
                        MessageBox.Show("Thêm phiếu đổi trả thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        formDT.napdgvdoitra();
                    }
                    else
                    {
                        MessageBox.Show("Thêm phiếu đổi trả thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm phiếu đổi trả: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void FormThemDoiTra_Load(object sender, EventArgs e)
        {

        }
    }
}
