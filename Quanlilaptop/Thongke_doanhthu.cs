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
    public partial class Thongke_doanhthu : Form
    {
        private DateTime _beginDate;
        private DateTime _endDate;
        
     
        public class ThongKeKho
        {
            public string TenKho { get; set; }
            public decimal DoanhThu { get; set; }
            public decimal ThanhToan { get; set; }
            public decimal LoiNhuan { get; set; }
        }
        private void LoadDanhSachKho()
        {
            cboKho.Items.Clear();
            cboKho.Items.Add("Tất cả");

            using (var connection = new MySqlConnection("Server=localhost;Database=quanlimaytinh;Port=3306;User ID=root;Password="))
            {
                connection.Open();

                var query = "SELECT ten_kho FROM kho";
                using (var cmd = new MySqlCommand(query, connection))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string tenKho = reader["ten_kho"].ToString();
                        cboKho.Items.Add(tenKho);
                    }
                }
            }

            cboKho.SelectedIndex = 0;
        }




        public List<ThongKeKho> LayThongKeKho(DateTime begin, DateTime end, string tenKho)

        {
            List<ThongKeKho> result = new List<ThongKeKho>();

            using (var connection = new MySqlConnection("Server=localhost;Database=quanlimaytinh;Port=3306;User ID=root;Password="))
            {
                connection.Open();

                List<(string maKho, string tenKho)> khoList = new List<(string, string)>();
                var khoQuery = "SELECT maKho, ten_kho FROM kho";

                if (tenKho != "Tất cả")
                {
                    khoQuery += " WHERE ten_kho = @tenKho";
                }

                using (var khoCmd = new MySqlCommand(khoQuery, connection))
                {
                    if (tenKho != "Tất cả")
                    {
                        khoCmd.Parameters.AddWithValue("@tenKho", tenKho);
                    }

                    using (var reader = khoCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            khoList.Add((reader["maKho"].ToString(), reader["ten_kho"].ToString()));
                        }
                    }
                }

                foreach (var kho in khoList)
                {
                    decimal doanhThu = 0;
                    var doanhThuQuery = @"SELECT IFNULL(SUM(ct.so_luong * ct.don_gia), 0) AS TongDoanhThu
                    FROM chitietphieuxuat ct
                    JOIN phieuxuat px ON ct.maPhieu = px.maPhieu
                    WHERE ct.maKho = @maKho AND px.thoi_gian_tao BETWEEN @begin AND @end";

                    using (var doanhThuCmd = new MySqlCommand(doanhThuQuery, connection))
                    {
                        doanhThuCmd.Parameters.AddWithValue("@maKho", kho.maKho);
                        doanhThuCmd.Parameters.AddWithValue("@begin", begin);
                        doanhThuCmd.Parameters.AddWithValue("@end", end);
                        doanhThu = Convert.ToDecimal(doanhThuCmd.ExecuteScalar());
                    }

                    decimal thanhToan = 0;
                    var thanhToanQuery = @"SELECT IFNULL(SUM(ct.so_luong * ct.don_gia), 0) AS TongThanhToan
                    FROM chitietphieunhap ct
                    JOIN phieunhap pn ON ct.maPhieu = pn.maPhieu
                    WHERE ct.maKho = @maKho AND pn.thoi_gian_tao BETWEEN @begin AND @end";

                    using (var thanhToanCmd = new MySqlCommand(thanhToanQuery, connection))
                    {
                        thanhToanCmd.Parameters.AddWithValue("@maKho", kho.maKho);
                        thanhToanCmd.Parameters.AddWithValue("@begin", begin);
                        thanhToanCmd.Parameters.AddWithValue("@end", end);
                        thanhToan = Convert.ToDecimal(thanhToanCmd.ExecuteScalar());
                    }

                    result.Add(new ThongKeKho
                    {
                        TenKho = kho.tenKho,
                        DoanhThu = doanhThu,
                        ThanhToan = thanhToan,
                        LoiNhuan = doanhThu - thanhToan
                    });
                }
            }

            return result;
        }

        public Thongke_doanhthu()
        {
            

            InitializeComponent();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
        private void LoadThongKeKho()
        {
            string tenKho = cboKho.SelectedItem?.ToString() ?? "Tất cả";
            var data = LayThongKeKho(_beginDate, _endDate, tenKho);

            var tongHop = new ThongKeKho
            {
                TenKho = "TỔNG HỢP",
                DoanhThu = data.Sum(x => x.DoanhThu),
                ThanhToan = data.Sum(x => x.ThanhToan),
                LoiNhuan = data.Sum(x => x.LoiNhuan)
            };
            data.Add(tongHop);

            dataGridView1.DataSource = data;

            dataGridView1.Columns["TenKho"].HeaderText = "Kho";
            dataGridView1.Columns["DoanhThu"].HeaderText = "Doanh Thu";
            dataGridView1.Columns["ThanhToan"].HeaderText = "Thanh Toán";
            dataGridView1.Columns["LoiNhuan"].HeaderText = "Lợi Nhuận";


            dataGridView1.Columns["DoanhThu"].DefaultCellStyle.Format = "N0";
            dataGridView1.Columns["ThanhToan"].DefaultCellStyle.Format = "N0";
            dataGridView1.Columns["LoiNhuan"].DefaultCellStyle.Format = "N0";


            dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.LightBlue;
            dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.Font = new System.Drawing.Font(dataGridView1.Font, FontStyle.Bold);
            VeBieuDoLoiNhuan(data);
        }
        public class ThongKeTheoNgayTuan
        {
            public string TenKho { get; set; }
            public DateTime NgayHoacThang { get; set; }
            public decimal DoanhThu { get; set; }
        }
        private void Thongke_doanhthu_Load(object sender, EventArgs e)
        {
            _endDate = DateTime.Now;
            _beginDate = _endDate.AddMonths(-1);

            dateTime_begin.Value = _beginDate;
            dateTime_end.Value = _endDate;
            cbbThoiGianThongKe.Items.Add("Theo ngày");
            cbbThoiGianThongKe.Items.Add("Theo tháng");
            cbbThoiGianThongKe.SelectedIndex = 0;
            LoadThongKeKho();
            LoadDanhSachKho();
        }
        public List<ThongKeTheoNgayTuan> LayThongKeTheoNgayHoacThang(DateTime begin, DateTime end, string kieuThongKe, string tenKho)
        {
            var result = new List<ThongKeTheoNgayTuan>();
            using (var connection = new MySqlConnection("Server=localhost;Database=quanlimaytinh;Port=3306;User ID=root;Password="))
            {
                connection.Open();

                var khoQuery = "SELECT maKho, ten_kho FROM kho";
                if (tenKho != "Tất cả")
                {
                    khoQuery += " WHERE ten_kho = @tenKho";
                }
                var khoList = new List<(string, string)>();
                using (var cmd = new MySqlCommand(khoQuery, connection))
                {
                    if (tenKho != "Tất cả")
                        cmd.Parameters.AddWithValue("@tenKho", tenKho);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            khoList.Add((reader["maKho"].ToString(), reader["ten_kho"].ToString()));
                        }
                    }
                }

                foreach (var kho in khoList)
                {
                    string groupFormat = "%Y-%m-%d"; 
                    if (kieuThongKe == "thang") groupFormat = "%Y-%m";

                    var query = $@"
                SELECT DATE_FORMAT(px.thoi_gian_tao, '{groupFormat}') AS ThoiGian,
                       SUM(ct.so_luong * ct.don_gia) AS DoanhThu
                FROM chitietphieuxuat ct
                JOIN phieuxuat px ON ct.maPhieu = px.maPhieu
                WHERE ct.maKho = @maKho AND px.thoi_gian_tao BETWEEN @begin AND @end
                GROUP BY ThoiGian
                ORDER BY ThoiGian";

                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@maKho", kho.Item1);
                        cmd.Parameters.AddWithValue("@begin", begin);
                        cmd.Parameters.AddWithValue("@end", end);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                result.Add(new ThongKeTheoNgayTuan
                                {
                                    TenKho = kho.Item2,
                                    NgayHoacThang = DateTime.Parse(reader["ThoiGian"].ToString()),
                                    DoanhThu = Convert.ToDecimal(reader["DoanhThu"])
                                });
                            }
                        }
                    }
                }
            }

            return result;
        }

        private void btn_apdung_Click(object sender, EventArgs e)
        {

            _beginDate = dateTime_begin.Value.Date;
            _endDate = dateTime_end.Value.Date.AddDays(1).AddSeconds(-1);

            string kieuThongKe = cbbThoiGianThongKe.SelectedItem.ToString().ToLower(); 
            string tenKho = cboKho.SelectedItem?.ToString() ?? "Tất cả";

            LoadThongKeKho();

            // Lấy dữ liệu doanh thu theo từng mốc thời gian
            string loaiThongKe = "ngay";
            if (kieuThongKe.Contains("tuần")) loaiThongKe = "tuan";
            else if (kieuThongKe.Contains("tháng")) loaiThongKe = "thang";

            var data = LayThongKeTheoNgayHoacThang(_beginDate, _endDate, loaiThongKe, tenKho);
            VeBieuDoThongKeTheoNgayTuan(data);


        }

        private void btn_reset_Click(object sender, EventArgs e)
        {
            _endDate = DateTime.Now;
            _beginDate = _endDate.AddMonths(-1);

            dateTime_begin.Value = _beginDate;
            dateTime_end.Value = _endDate;

            LoadThongKeKho();

        }
        private void VeBieuDoLoiNhuan(List<ThongKeKho> data)
        {
            chart_loinhuan.Series.Clear();
            chart_loinhuan.ChartAreas.Clear();
            chart_loinhuan.Legends.Clear(); // <- Ẩn hoặc xoá chú thích

            ChartArea chartArea = new ChartArea("ChartArea1");
            chart_loinhuan.ChartAreas.Add(chartArea);
            chart_loinhuan.ChartAreas[0].AxisY.Title = "Lợi nhuận";
            chart_loinhuan.ChartAreas[0].AxisX.Title = "Kho";

            Series series = new Series("Lợi nhuận");
            series.ChartType = SeriesChartType.Column;
            series.IsValueShownAsLabel = true;

            foreach (var item in data)
            {
                if (item.TenKho == "TỔNG HỢP") continue;

                int pointIndex = series.Points.AddXY(item.TenKho, item.LoiNhuan);
                series.Points[pointIndex].Color = item.LoiNhuan >= 0 ? Color.Green : Color.Red;
            }

            chart_loinhuan.Series.Add(series);
            chart_loinhuan.ChartAreas[0].AxisY.IsReversed = false;
            chart_loinhuan.ChartAreas[0].AxisX.LabelStyle.Angle = 0;
            chart_loinhuan.ChartAreas[0].AxisX.LabelStyle.Interval = 1;
            chart_loinhuan.ChartAreas[0].AxisX.LabelStyle.IsStaggered = false;
        }



        private void chart1_Click(object sender, EventArgs e)
        {

        }
        private void VeBieuDoThongKeTheoNgayTuan(List<ThongKeTheoNgayTuan> data)
        {
            loi_nhuan_theo_ngay.Series.Clear();
            loi_nhuan_theo_ngay.ChartAreas.Clear();

            ChartArea area = new ChartArea("MainArea");
            loi_nhuan_theo_ngay.ChartAreas.Add(area);
            loi_nhuan_theo_ngay.ChartAreas[0].AxisX.Title = "Thời gian";
            loi_nhuan_theo_ngay.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            loi_nhuan_theo_ngay.ChartAreas[0].AxisY.Title = "Doanh thu";

            if (data == null || data.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để hiển thị biểu đồ.");
                return;
            }

            // Group data by TenKho
            var groupedByKho = data
                .GroupBy(x => x.TenKho)
                .ToList();

            foreach (var khoGroup in groupedByKho)
            {
                var series = new Series(khoGroup.Key)
                {
                    ChartType = SeriesChartType.Column,
                    XValueType = ChartValueType.DateTime,
                    IsValueShownAsLabel = true
                };

                // Filter dates for this specific warehouse where DoanhThu > 0
                var datesWithRevenue = khoGroup
                    .Where(x => x.DoanhThu > 0)
                    .Select(x => x.NgayHoacThang)
                    .Distinct()
                    .OrderBy(x => x)
                    .ToList();

                // Only add points for dates with revenue
                foreach (var date in datesWithRevenue)
                {
                    var doanhThu = khoGroup
                        .FirstOrDefault(x => x.NgayHoacThang == date)?.DoanhThu ?? 0;

                    if (doanhThu > 0) // This check is redundant now but kept for clarity
                    {
                        int pointIndex = series.Points.AddXY(date, doanhThu);
                        series.Points[pointIndex].Label = doanhThu.ToString("N0");
                    }
                }

                loi_nhuan_theo_ngay.Series.Add(series);
            }

            // Adjust X-axis settings based on the type of statistics
            string kieuThongKe = cbbThoiGianThongKe.SelectedItem.ToString().ToLower();
            if (kieuThongKe.Contains("tháng"))
            {
                loi_nhuan_theo_ngay.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Months;
                loi_nhuan_theo_ngay.ChartAreas[0].AxisX.Interval = 1;
                loi_nhuan_theo_ngay.ChartAreas[0].AxisX.LabelStyle.Format = "yyyy-MM";
            }
            else // Theo ngày
            {
                loi_nhuan_theo_ngay.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Days;
                loi_nhuan_theo_ngay.ChartAreas[0].AxisX.Interval = 1; // Changed to 1 since we're only showing dates with data
                loi_nhuan_theo_ngay.ChartAreas[0].AxisX.LabelStyle.Format = "yyyy-MM-dd";
            }

            // Ensure the X-axis only shows labels for the dates that have data
            loi_nhuan_theo_ngay.ChartAreas[0].AxisX.LabelStyle.Interval = 1;
        }

        private void cbbThoiGianThongKe_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cboKho_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
