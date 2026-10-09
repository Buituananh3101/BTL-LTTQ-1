using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Quanlilaptop
{
    public partial class FormSuaDoiTra : Form
    {
        private FormDoiTra formDT;

        public FormSuaDoiTra(FormDoiTra form)
        {
            InitializeComponent();
            formDT = form;
            LoadmaphieudoitraToComboBox();
        }

        List<string> trangThaiCamSua = new List<string>
        {
            "Đã sửa chữa xong",
            "Đã đổi trả xong",
            "Đã hoàn tiền",
            "Lỗi từ người dùng từ chối bảo hành"
        };

        private void LoadmaphieudoitraToComboBox()
        {
            using (MySqlConnection conn = new MySqlConnection("Server=localhost;Database=quanlimaytinh;Port=3306;User ID=root;Password="))
            {
                conn.Open();
                string query = "SELECT ma_doi_tra FROM doitra";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    cbbMadoitra.Items.Clear();
                    while (reader.Read())
                    {
                        cbbMadoitra.Items.Add(reader["ma_doi_tra"].ToString());
                    }
                }
            }
        }

        private void cbbMadoitra_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbbMadoitra.SelectedItem == null) return;

            string maDoiTra = cbbMadoitra.SelectedItem.ToString();
            string connStr = "Server=localhost;Database=quanlimaytinh;Port=3306;User ID=root;Password=";

            string query = "SELECT * FROM doitra WHERE ma_doi_tra = @maDoiTra";

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@maDoiTra", maDoiTra);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        txtMaphieu.Text = reader["ma_phieu"].ToString();
                        txtMamay.Text = reader["ma_may"].ToString();
                        txtSoluong.Text = reader["so_luong"].ToString();
                        dtpDT.Value = Convert.ToDateTime(reader["ngay_doi_tra"]);
                        txtLydo.Text = reader["ly_do"].ToString();
                        cbbTrangthai.Text = reader["trang_thai"].ToString();
                    }
                }
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            string maDoiTra = cbbMadoitra.SelectedItem?.ToString();
            string maPhieu = txtMaphieu.Text.Trim();
            string maMay = txtMamay.Text.Trim();
            string lyDo = txtLydo.Text.Trim();
            string trangThaiMoi = cbbTrangthai.Text.Trim();
            DateTime ngayDoiTra = dtpDT.Value;

            if (!int.TryParse(txtSoluong.Text.Trim(), out int soLuong) || string.IsNullOrEmpty(maPhieu) || string.IsNullOrEmpty(maMay) || string.IsNullOrEmpty(lyDo) || string.IsNullOrEmpty(trangThaiMoi))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (MySqlConnection conn = new MySqlConnection("Server=localhost;Database=quanlimaytinh;Port=3306;User ID=root;Password="))
            {
                conn.Open();

                
                var cmd = new MySqlCommand("SELECT trang_thai FROM doitra WHERE ma_doi_tra = @maDoiTra", conn);
                cmd.Parameters.AddWithValue("@maDoiTra", maDoiTra);
                string trangThaiHienTai = cmd.ExecuteScalar()?.ToString();

                if (trangThaiCamSua.Contains(trangThaiHienTai))
                {
                    MessageBox.Show("Phiếu đổi trả này đã hoàn tất, không thể chỉnh sửa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                
                string getSLXuat = "SELECT so_luong FROM chitietphieuxuat WHERE maPhieu = @maPhieu AND maMay = @maMay";
                var cmdXuat = new MySqlCommand(getSLXuat, conn);
                cmdXuat.Parameters.AddWithValue("@maPhieu", maPhieu);
                cmdXuat.Parameters.AddWithValue("@maMay", maMay);
                object objSoLuongXuat = cmdXuat.ExecuteScalar();

                if (objSoLuongXuat == null)
                {
                    MessageBox.Show("Không tìm thấy phiếu xuất tương ứng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int soLuongXuat = Convert.ToInt32(objSoLuongXuat);

                
                string queryDaDoi = @"
            SELECT COALESCE(SUM(so_luong), 0)
            FROM doitra
            WHERE ma_phieu = @maPhieu AND ma_may = @maMay 
              AND trang_thai IN ('Đã hoàn tiền', 'Đã đổi trả xong')
              AND ma_doi_tra != @maDoiTra";

                var cmdDaDoi = new MySqlCommand(queryDaDoi, conn);
                cmdDaDoi.Parameters.AddWithValue("@maPhieu", maPhieu);
                cmdDaDoi.Parameters.AddWithValue("@maMay", maMay);
                cmdDaDoi.Parameters.AddWithValue("@maDoiTra", maDoiTra);

                int soLuongDaDoi = Convert.ToInt32(cmdDaDoi.ExecuteScalar());

                
                if (soLuong + soLuongDaDoi > soLuongXuat)
                {
                    MessageBox.Show($"Tổng số lượng đổi trả ({soLuongDaDoi + soLuong}) vượt quá số lượng xuất ({soLuongXuat})!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                
                if (trangThaiMoi == "Đã gửi nhà cung cấp")
                {
                    string query = @"
                SELECT pn.thoi_gian_tao, ctpn.baohanhnhacungcap
                FROM chitietphieuxuat ctx
                JOIN chitietphieunhap ctpn ON ctx.maPhieuNhap = ctpn.maPhieu AND ctx.maMay = ctpn.maMay
                JOIN phieunhap pn ON pn.maPhieu = ctpn.maPhieu
                WHERE ctx.maPhieu = @maPhieu AND ctx.maMay = @maMay LIMIT 1";

                    var cmdCheck = new MySqlCommand(query, conn);
                    cmdCheck.Parameters.AddWithValue("@maPhieu", maPhieu);
                    cmdCheck.Parameters.AddWithValue("@maMay", maMay);

                    using (var reader = cmdCheck.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            DateTime ngayNhap = Convert.ToDateTime(reader["thoi_gian_tao"]);
                            int thangBH = Convert.ToInt32(reader["baohanhnhacungcap"]);
                            if (ngayDoiTra > ngayNhap.AddMonths(thangBH))
                            {
                                MessageBox.Show("Sản phẩm đã hết bảo hành nhà cung cấp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy thông tin phiếu nhập phù hợp.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }

                string updateQuery = @"UPDATE doitra SET ma_phieu = @maPhieu, ma_may = @maMay, so_luong = @soLuong,
                ngay_doi_tra = @ngayDoiTra, ly_do = @lyDo, trang_thai = @trangThai WHERE ma_doi_tra = @maDoiTra";

                var cmdUpdate = new MySqlCommand(updateQuery, conn);
                cmdUpdate.Parameters.AddWithValue("@maPhieu", maPhieu);
                cmdUpdate.Parameters.AddWithValue("@maMay", maMay);
                cmdUpdate.Parameters.AddWithValue("@soLuong", soLuong);
                cmdUpdate.Parameters.AddWithValue("@ngayDoiTra", ngayDoiTra);
                cmdUpdate.Parameters.AddWithValue("@lyDo", lyDo);
                cmdUpdate.Parameters.AddWithValue("@trangThai", trangThaiMoi);
                cmdUpdate.Parameters.AddWithValue("@maDoiTra", maDoiTra);

                if (cmdUpdate.ExecuteNonQuery() > 0)
                {
                    if (trangThaiMoi == "Đã hoàn tiền" || trangThaiMoi=="Đã đổi trả xong")
                    {
                        CapNhatTonKhoSauHoanTien(conn, maMay, soLuong);
                    }

                    MessageBox.Show("Cập nhật phiếu đổi trả thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    formDT.napdgvdoitra();
                }
                else
                {
                    MessageBox.Show("Không tìm thấy phiếu đổi trả!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }




        private void CapNhatTonKhoSauHoanTien(MySqlConnection conn, string maMay, int soLuong)
        {
            // Cập nhật tồn kho
            var updateTonKho = new MySqlCommand("UPDATE sanpham SET ton_kho = ton_kho + @sl WHERE id = @maMay", conn);
            updateTonKho.Parameters.AddWithValue("@sl", soLuong);
            updateTonKho.Parameters.AddWithValue("@maMay", maMay);
            updateTonKho.ExecuteNonQuery();

            // Cập nhật chi tiết tồn
            var checkCT = new MySqlCommand("SELECT COUNT(*) FROM chitietsoluongsanpham WHERE ma_san_pham = @maMay", conn);
            checkCT.Parameters.AddWithValue("@maMay", maMay);
            bool exists = Convert.ToInt32(checkCT.ExecuteScalar()) > 0;

            if (exists)
            {
                var update = new MySqlCommand("UPDATE chitietsoluongsanpham SET san_pham_chua_kiem_tra = san_pham_chua_kiem_tra + @sl WHERE ma_san_pham = @maMay", conn);
                update.Parameters.AddWithValue("@sl", soLuong);
                update.Parameters.AddWithValue("@maMay", maMay);
                update.ExecuteNonQuery();
            }
            else
            {
                var insert = new MySqlCommand("INSERT INTO chitietsoluongsanpham(ma_san_pham, chua_kiem_tra, loi, ktv) VALUES (@maMay, @sl, 0, 0)", conn);
                insert.Parameters.AddWithValue("@maMay", maMay);
                insert.Parameters.AddWithValue("@sl", soLuong);
                insert.ExecuteNonQuery();
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormSuaDoiTra_Load(object sender, EventArgs e) { }
        private void cbbTrangthai_SelectedIndexChanged(object sender, EventArgs e) { }
    }
}
