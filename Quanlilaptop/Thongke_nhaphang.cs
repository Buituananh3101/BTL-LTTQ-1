using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using MySql.Data.MySqlClient;

namespace Quanlilaptop
{
    public partial class Thongke_nhaphang : Form
    {
        public Thongke_nhaphang()
        {
            InitializeComponent();
            dateTime_begin.Value = DateTime.Now.AddMonths(-1);
            dateTime_end.Value = DateTime.Now;
            LoadComboBoxData();
            LoadData();
        }

        private void btn_reset_Click(object sender, EventArgs e)
        {
            cbb_loaisanpham.SelectedIndex = 0;
            cbb_nhacungcap.SelectedIndex = 0;
            txt_search.Text = "";
            dateTime_begin.Value = DateTime.Now.AddMonths(-1);
            dateTime_end.Value = DateTime.Now;
            LoadData();
            VeBieuDoLoaiSanPham();

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        private void LoadComboBoxData()
        {
            string connectionString = "Server=localhost;Database=quanlimaytinh;Uid=root;Pwd=;";

            cbb_loaisanpham.Items.Clear();
            cbb_nhacungcap.Items.Clear();

            cbb_loaisanpham.Items.Add("Tất cả");
            cbb_nhacungcap.Items.Add("Tất cả");

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT DISTINCT loai FROM sanpham WHERE loai IS NOT NULL";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    cbb_loaisanpham.Items.Add(reader["loai"].ToString());
                }
                cbb_loaisanpham.SelectedIndex = 0;
                reader.Close();

                query = "SELECT ma_nha_cung_cap, ten_nha_cung_cap FROM nhacungcap";
                cmd = new MySqlCommand(query, conn);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    cbb_nhacungcap.Items.Add(new KeyValuePair<string, string>(
                        reader["ma_nha_cung_cap"].ToString(),
                        reader["ten_nha_cung_cap"].ToString()));
                }
                cbb_nhacungcap.SelectedIndex = 0;
                cbb_nhacungcap.DisplayMember = "Value";
                cbb_nhacungcap.ValueMember = "Key";
            }
        }
        private void LoadData()
        {
            string connectionString = "Server=localhost;Database=quanlimaytinh;Uid=root;Pwd=;";

            string query = @"
            SELECT 
                sp.id AS 'Mã SP',
                sp.ten_sanpham AS 'Tên sản phẩm',
                sp.loai AS 'Loại',
                ncc.ten_nha_cung_cap AS 'Nhà cung cấp',
                DATE(pn.thoi_gian_tao) AS 'Ngày nhập',
                ctn.so_luong AS 'Số lượng',
                ctn.don_gia AS 'Đơn giá',
                (ctn.so_luong * ctn.don_gia) AS 'Thành tiền'
            FROM 
                sanpham sp
            JOIN 
                chitietphieunhap ctn ON sp.id = ctn.maMay
            JOIN 
                phieunhap pn ON ctn.maPhieu = pn.maPhieu
            JOIN
                nhacungcap ncc ON pn.ma_nha_cung_cap = ncc.ma_nha_cung_cap
            WHERE 
                1=1";

            if (cbb_loaisanpham.SelectedIndex > 0)
            {
                query += $" AND sp.loai = '{cbb_loaisanpham.SelectedItem}'";
            }

            if (cbb_nhacungcap.SelectedIndex > 0)
            {
                var selectedSupplier = (KeyValuePair<string, string>)cbb_nhacungcap.SelectedItem;
                query += $" AND pn.ma_nha_cung_cap = '{selectedSupplier.Key}'";
            }

            if (!string.IsNullOrWhiteSpace(txt_search.Text))
            {
                query += $" AND sp.ten_sanpham LIKE '%{txt_search.Text}%'";
            }

            query += $" AND pn.thoi_gian_tao BETWEEN '{dateTime_begin.Value:yyyy-MM-dd}' AND '{dateTime_end.Value.AddDays(1):yyyy-MM-dd}'";

            query += " ORDER BY pn.thoi_gian_tao DESC";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dataGridView1.DataSource = dt;

                if (dataGridView1.Columns.Count > 0)
                {
                    dataGridView1.Columns["Ngày nhập"].DefaultCellStyle.Format = "dd/MM/yyyy";
                    dataGridView1.Columns["Đơn giá"].DefaultCellStyle.Format = "N0";
                    dataGridView1.Columns["Thành tiền"].DefaultCellStyle.Format = "N0";
                }
            }
            VeBieuDoLoaiSanPham();

        }
        private void Thongke_nhaphang_Load(object sender, EventArgs e) { 
        
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btn_apdung_Click(object sender, EventArgs e)
        {
            LoadData();
            VeBieuDoLoaiSanPham();

        }
        private void VeBieuDoLoaiSanPham()
        {
            string connectionString = "Server=localhost;Database=quanlimaytinh;Uid=root;Pwd=;";

            string query = @"
        SELECT 
            sp.loai AS LoaiSanPham,
            SUM(ctn.so_luong) AS TongSoLuong
        FROM 
            sanpham sp
        JOIN 
            chitietphieunhap ctn ON sp.id = ctn.maMay
        JOIN 
            phieunhap pn ON ctn.maPhieu = pn.maPhieu
        WHERE 
            pn.thoi_gian_tao BETWEEN @begin AND @end
            AND sp.loai IS NOT NULL";

            // Nếu đã chọn một nhà cung cấp cụ thể (không phải "Tất cả"), thêm điều kiện lọc
            if (cbb_nhacungcap.SelectedIndex > 0)
            {
                query += " AND pn.ma_nha_cung_cap = @ma_nha_cung_cap";
            }

            query += " GROUP BY sp.loai";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@begin", dateTime_begin.Value.Date);
                cmd.Parameters.AddWithValue("@end", dateTime_end.Value.Date.AddDays(1).AddSeconds(-1));

                if (cbb_nhacungcap.SelectedIndex > 0)
                {
                    var selectedSupplier = (KeyValuePair<string, string>)cbb_nhacungcap.SelectedItem;
                    cmd.Parameters.AddWithValue("@ma_nha_cung_cap", selectedSupplier.Key);
                }

                conn.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                chart_loaisp.Series.Clear();
                chart_loaisp.ChartAreas.Clear();
                chart_loaisp.Titles.Clear();

                ChartArea area = new ChartArea("Area1");
                chart_loaisp.ChartAreas.Add(area);

                Series series = new Series("Loại sản phẩm")
                {
                    ChartType = SeriesChartType.Pie,
                    IsValueShownAsLabel = true,
                    LabelForeColor = Color.Black
                };

                while (reader.Read())
                {
                    string loai = reader["LoaiSanPham"].ToString();
                    int tongSoLuong = Convert.ToInt32(reader["TongSoLuong"]);
                    series.Points.AddXY(loai, tongSoLuong);
                }

                chart_loaisp.Series.Add(series);
                chart_loaisp.Titles.Add("Tỷ lệ các loại sản phẩm được nhập");
            }
        }


    }
}
