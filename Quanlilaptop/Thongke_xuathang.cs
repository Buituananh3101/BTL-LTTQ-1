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
    public partial class Thongke_xuathang : Form
    {
        public Thongke_xuathang()
        {
            InitializeComponent();
            dateTime_begin.Value = DateTime.Now.AddMonths(-1);
            dateTime_end.Value = DateTime.Now;
            LoadComboBoxData();
            LoadData();
        }
        private void LoadComboBoxData()
        {
            string connectionString = "Server=localhost;Database=quanlimaytinh;Uid=root;Pwd=;";

            cbb_loaisanpham.Items.Clear();
            
            cbb_loaisanpham.Items.Add("Tất cả");
           

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
                
                DATE(px.thoi_gian_tao) AS 'Ngày xuất',
                ctx.so_luong AS 'Số lượng',
                ctx.don_gia AS 'Đơn giá',
                (ctx.so_luong * ctx.don_gia) AS 'Thành tiền'
            FROM 
                sanpham sp
            JOIN 
                chitietphieuxuat ctx ON sp.id = ctx.maMay
            JOIN 
                phieuxuat px ON ctx.maPhieu = px.maPhieu
            JOIN
                khachhang kh ON px.id_khach_hang = kh.id_khach_hang
            WHERE 
                1=1";

            if (cbb_loaisanpham.SelectedIndex > 0)
            {
                query += $" AND sp.loai = '{cbb_loaisanpham.SelectedItem}'";
            }

            if (!string.IsNullOrWhiteSpace(txt_search.Text))
            {
                query += $" AND sp.ten_sanpham LIKE '%{txt_search.Text}%'";
            }

            query += $" AND px.thoi_gian_tao BETWEEN '{dateTime_begin.Value:yyyy-MM-dd}' AND '{dateTime_end.Value.AddDays(1):yyyy-MM-dd}'";

            query += " ORDER BY px.thoi_gian_tao DESC";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dataGridView1.DataSource = dt;

                if (dataGridView1.Columns.Count > 0)
                {
                    dataGridView1.Columns["Ngày xuất"].DefaultCellStyle.Format = "dd/MM/yyyy";
                    dataGridView1.Columns["Đơn giá"].DefaultCellStyle.Format = "N0";
                    dataGridView1.Columns["Thành tiền"].DefaultCellStyle.Format = "N0";
                }
            }
            VeBieuDoLoaiSanPham_Xuat();

        }
        private void Thongke_xuathang_Load(object sender, EventArgs e)
        {

        }

        private void btn_reset_Click(object sender, EventArgs e)
        {
            cbb_loaisanpham.SelectedIndex = 0;
            txt_search.Text = "";
            dateTime_begin.Value = DateTime.Now.AddMonths(-1);
            dateTime_end.Value = DateTime.Now;
            LoadData();
            VeBieuDoLoaiSanPham_Xuat();

        }

        private void btn_apdung_Click(object sender, EventArgs e)
        {
            LoadData();
            VeBieuDoLoaiSanPham_Xuat();

        }
        private void VeBieuDoLoaiSanPham_Xuat()
        {
            string connectionString = "Server=localhost;Database=quanlimaytinh;Uid=root;Pwd=;";

            string query = @"
        SELECT 
            sp.loai AS LoaiSanPham,
            SUM(ctx.so_luong) AS TongSoLuong
        FROM 
            sanpham sp
        JOIN 
            chitietphieuxuat ctx ON sp.id = ctx.maMay
        JOIN 
            phieuxuat px ON ctx.maPhieu = px.maPhieu
        WHERE 
            px.thoi_gian_tao BETWEEN @begin AND @end
            AND sp.loai IS NOT NULL
        GROUP BY 
            sp.loai";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@begin", dateTime_begin.Value.Date);
                cmd.Parameters.AddWithValue("@end", dateTime_end.Value.Date.AddDays(1).AddSeconds(-1));

                conn.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                chart_loaisp.Series.Clear();
                chart_loaisp.ChartAreas.Clear();
                chart_loaisp.Titles.Clear();

                ChartArea area = new ChartArea("Area1");
                chart_loaisp.ChartAreas.Add(area);

                Series series = new Series("Loại sản phẩm xuất")
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
                chart_loaisp.Titles.Add("Tỷ lệ các loại sản phẩm được xuất");
            }
        }

        private void chart_loaisp_Click(object sender, EventArgs e)
        {

        }
    }
}
