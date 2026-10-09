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
    public partial class Thongke_tonkho : Form
    {
        private Panel panel1;
        private Button btn_reset;
        private Button btn_apdung;
        private Label label3;
        private TextBox txt_search;
        private Label label2;
        private Label label1;
        private ComboBox cbb_loaisanpham;
        private DataGridView dataGridView1;
        private Label label4;
        private ComboBox cbb_sort;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private BackgroundWorker backgroundWorker1;
        private ComboBox cbb_kho;

        public Thongke_tonkho()
        {
            InitializeComponent();

            LoadPieChartData();
            LoadKhoComboBox();
            LoadLoaiSanPhamComboBox();
            LoadProductInventoryData();
            LoadPieChartData();
        }

        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.cbb_sort = new System.Windows.Forms.ComboBox();
            this.btn_reset = new System.Windows.Forms.Button();
            this.btn_apdung = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_search = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cbb_loaisanpham = new System.Windows.Forms.ComboBox();
            this.cbb_kho = new System.Windows.Forms.ComboBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.cbb_sort);
            this.panel1.Controls.Add(this.btn_reset);
            this.panel1.Controls.Add(this.btn_apdung);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.txt_search);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.cbb_loaisanpham);
            this.panel1.Controls.Add(this.cbb_kho);
            this.panel1.Location = new System.Drawing.Point(1, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(799, 66);
            this.panel1.TabIndex = 1;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(304, 40);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Sắp xếp";
            // 
            // cbb_sort
            // 
            this.cbb_sort.FormattingEnabled = true;
            this.cbb_sort.Items.AddRange(new object[] {
            "thấp đến cao",
            "cao đến thấp"});
            this.cbb_sort.Location = new System.Drawing.Point(373, 36);
            this.cbb_sort.Name = "cbb_sort";
            this.cbb_sort.Size = new System.Drawing.Size(182, 21);
            this.cbb_sort.TabIndex = 8;
            // 
            // btn_reset
            // 
            this.btn_reset.Location = new System.Drawing.Point(673, 37);
            this.btn_reset.Name = "btn_reset";
            this.btn_reset.Size = new System.Drawing.Size(75, 23);
            this.btn_reset.TabIndex = 7;
            this.btn_reset.Text = "Làm mới";
            this.btn_reset.UseVisualStyleBackColor = true;
            this.btn_reset.Click += new System.EventHandler(this.btn_reset_Click);
            // 
            // btn_apdung
            // 
            this.btn_apdung.Location = new System.Drawing.Point(673, 3);
            this.btn_apdung.Name = "btn_apdung";
            this.btn_apdung.Size = new System.Drawing.Size(75, 23);
            this.btn_apdung.TabIndex = 6;
            this.btn_apdung.Text = "Áp dụng ";
            this.btn_apdung.UseVisualStyleBackColor = true;
            this.btn_apdung.Click += new System.EventHandler(this.btn_apdung_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(301, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Tìm kiếm";
            // 
            // txt_search
            // 
            this.txt_search.Location = new System.Drawing.Point(373, 10);
            this.txt_search.Name = "txt_search";
            this.txt_search.Size = new System.Drawing.Size(182, 20);
            this.txt_search.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(42, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Loại sản phẩm";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(42, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Kho";
            // 
            // cbb_loaisanpham
            // 
            this.cbb_loaisanpham.FormattingEnabled = true;
            this.cbb_loaisanpham.Location = new System.Drawing.Point(127, 37);
            this.cbb_loaisanpham.Name = "cbb_loaisanpham";
            this.cbb_loaisanpham.Size = new System.Drawing.Size(121, 21);
            this.cbb_loaisanpham.TabIndex = 1;
            // 
            // cbb_kho
            // 
            this.cbb_kho.FormattingEnabled = true;
            this.cbb_kho.Location = new System.Drawing.Point(127, 10);
            this.cbb_kho.Name = "cbb_kho";
            this.cbb_kho.Size = new System.Drawing.Size(121, 21);
            this.cbb_kho.TabIndex = 0;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 75);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(534, 475);
            this.dataGridView1.TabIndex = 2;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(578, 148);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
            series1.Label = "#PERCENT";
            series1.Legend = "Legend1";
            series1.Name = "Tồn kho theo loại";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(430, 325);
            this.chart1.TabIndex = 3;
            this.chart1.Text = "chart1";
            this.chart1.Click += new System.EventHandler(this.chart1_Click);
            // 
            // Thongke_tonkho
            // 
            this.ClientSize = new System.Drawing.Size(1032, 551);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.panel1);
            this.Name = "Thongke_tonkho";
            this.Load += new System.EventHandler(this.Thongke_tonkho_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);

        }

        private void LoadKhoComboBox()
        {
            cbb_kho.Items.Clear();
            cbb_kho.DisplayMember = "Text";
            cbb_kho.ValueMember = "Value";

            cbb_kho.Items.Add(new ComboboxItem("Tổng hợp", "all"));

            string query = "SELECT maKho, ten_kho FROM kho";

            using (MySqlConnection conn = new MySqlConnection("Server=localhost;Database=quanlimaytinh;Port=3306;User ID=root;Password="))
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                conn.Open();

                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cbb_kho.Items.Add(new ComboboxItem(
                        reader["ten_kho"].ToString(),
                        reader["maKho"].ToString()
                    ));
                }
            }
            cbb_kho.SelectedIndex = 0;
        }

        public class ComboboxItem
        {
            public string Text { get; set; }
            public object Value { get; set; }

            public ComboboxItem(string text, object value)
            {
                Text = text;
                Value = value;
            }

            public override string ToString()
            {
                return Text;
            }
        }

        private void LoadLoaiSanPhamComboBox()
        {
            cbb_loaisanpham.Items.Clear();

            string query = "SELECT DISTINCT loai FROM sanpham WHERE loai IS NOT NULL";

            using (MySqlConnection conn = new MySqlConnection("Server=localhost;Database=quanlimaytinh;Port=3306;User ID=root;Password="))
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                conn.Open();

                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cbb_loaisanpham.Items.Add(reader["loai"].ToString());
                }
            }
            cbb_loaisanpham.Items.Insert(0, "Tất cả");
            cbb_loaisanpham.SelectedIndex = 0;
        }

        private void LoadProductInventoryData()
        {
            if (cbb_kho.SelectedIndex == 0)
            {
                string query = "SELECT ten_sanpham AS 'Tên sản phẩm', ton_kho AS 'Tồn kho' FROM sanpham";

                using (MySqlConnection conn = new MySqlConnection("Server=localhost;Database=quanlimaytinh;Port=3306;User ID=root;Password="))
                {
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dataGridView1.DataSource = dt;
                    FormatDataGridView();
                }
            }
            else
            {
                string selectedKho = (cbb_kho.SelectedItem as ComboboxItem)?.Value?.ToString();
                string query = BuildFilterQuery(selectedKho, "", "", "");
                LoadFilteredData(query, selectedKho, cbb_loaisanpham.Text,txt_search.Text,cbb_sort.Text);
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Thongke_tonkho_Load(object sender, EventArgs e)
        {

            LoadPieChartData();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btn_apdung_Click(object sender, EventArgs e)
        {
            try
            {
                string selectedKho = (cbb_kho.SelectedItem as ComboboxItem)?.Value?.ToString();
                string selectedLoai = cbb_loaisanpham.SelectedItem?.ToString();
                string searchText = txt_search.Text.Trim();
                string sortOrder = cbb_sort.SelectedItem?.ToString();

                string query = BuildFilterQuery(selectedKho, selectedLoai, searchText, sortOrder);
                LoadFilteredData(query, selectedKho, selectedLoai, searchText, sortOrder);

                LoadPieChartData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi áp dụng bộ lọc: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string BuildFilterQuery(string maKho, string loaiSP, string searchText, string sortOrder)
        {
            StringBuilder query = new StringBuilder();

            query.Append("SELECT s.ten_sanpham AS 'Tên sản phẩm', s.ton_kho AS 'Tồn kho'");

            if (!string.IsNullOrEmpty(maKho) && maKho != "all")
            {
                query.Append(", k.ten_kho AS 'Kho'");
            }

            query.Append(" FROM sanpham s");

            if (!string.IsNullOrEmpty(maKho) && maKho != "all")
            {
                query.Append(" INNER JOIN chitietphieunhap ctn ON s.id = ctn.maMay");
                query.Append(" INNER JOIN kho k ON ctn.maKho = k.maKho");
            }

            query.Append(" WHERE 1=1");

            if (!string.IsNullOrEmpty(maKho) && maKho != "all")
            {
                query.Append(" AND ctn.maKho = @MaKho");
            }

            if (!string.IsNullOrEmpty(loaiSP) && loaiSP != "Tất cả")
            {
                query.Append(" AND s.loai = @LoaiSP");
            }

            if (!string.IsNullOrEmpty(searchText))
            {
                query.Append(" AND (s.ten_sanpham LIKE @SearchText OR s.loai LIKE @SearchText)");
            }

            if (!string.IsNullOrEmpty(sortOrder))
            {
                if (sortOrder == "thấp đến cao")
                {
                    query.Append(" ORDER BY s.ton_kho ASC");
                }
                else if (sortOrder == "cao đến thấp")
                {
                    query.Append(" ORDER BY s.ton_kho DESC");
                }
            }

            return query.ToString();
        }

        private void LoadFilteredData(string query, string maKho, string loaiSP, string searchText, string sortOrder)
        {
            using (MySqlConnection conn = new MySqlConnection("Server=localhost;Database=quanlimaytinh;Port=3306;User ID=root;Password="))
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);

                if (!string.IsNullOrEmpty(maKho) && maKho != "all")
                {
                    cmd.Parameters.AddWithValue("@MaKho", maKho);
                }

                if (!string.IsNullOrEmpty(loaiSP) && loaiSP != "Tất cả")
                {
                    cmd.Parameters.AddWithValue("@LoaiSP", loaiSP);
                }

                if (!string.IsNullOrEmpty(searchText))
                {
                    cmd.Parameters.AddWithValue("@SearchText", "%" + searchText + "%");
                }

                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dataGridView1.DataSource = dt;
                FormatDataGridView();
            }
        }
        private void FormatDataGridView()
        {
            if (dataGridView1.Columns.Contains("Tồn kho"))
            {
                dataGridView1.Columns["Tồn kho"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells["Tồn kho"].Value != null &&
                        int.TryParse(row.Cells["Tồn kho"].Value.ToString(), out int tonKho))
                    {
                        if (tonKho < 5) 
                        {
                            row.DefaultCellStyle.BackColor = Color.LightPink;
                            row.DefaultCellStyle.ForeColor = Color.Red;
                        }
                    }
                }
            }
        }
        private void btn_reset_Click(object sender, EventArgs e)
        {
            cbb_kho.SelectedIndex = 0; 
            cbb_loaisanpham.SelectedIndex = 0; 
            txt_search.Text = "";
            cbb_sort.SelectedIndex = -1;
            LoadProductInventoryData();
            LoadPieChartData();
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }
        private void LoadPieChartData()
        {
            if (cbb_loaisanpham.SelectedIndex > 0 && cbb_loaisanpham.SelectedItem?.ToString() != "Tất cả")
            {
                chart1.Series["Tồn kho theo loại"].Points.Clear();
                chart1.Titles.Clear();
                chart1.Titles.Add(new Title("Biểu đồ chỉ hiển thị khi chọn 'Tất cả' loại sản phẩm"));
                return;
            }
            string selectedKho = (cbb_kho.SelectedItem as ComboboxItem)?.Value?.ToString();

            StringBuilder query = new StringBuilder();
            query.Append(@"SELECT 
                s.loai AS 'Loại sản phẩm', 
                SUM(s.ton_kho) AS 'Tổng tồn kho'
             FROM sanpham s");

            if (!string.IsNullOrEmpty(selectedKho) && selectedKho != "all")
            {
                query.Append(" INNER JOIN chitietphieunhap ctn ON s.id = ctn.maMay");
            }

            query.Append(" WHERE s.loai IS NOT NULL");

            if (!string.IsNullOrEmpty(selectedKho) && selectedKho != "all")
            {
                query.Append(" AND ctn.maKho = @MaKho");
            }
            query.Append(" GROUP BY s.loai");

            using (MySqlConnection conn = new MySqlConnection("Server=localhost;Database=quanlimaytinh;Port=3306;User ID=root;Password="))
            {
                MySqlCommand cmd = new MySqlCommand(query.ToString(), conn);

                if (!string.IsNullOrEmpty(selectedKho) && selectedKho != "all")
                {
                    cmd.Parameters.AddWithValue("@MaKho", selectedKho);
                }

                conn.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                chart1.Series["Tồn kho theo loại"].Points.Clear();
                chart1.Titles.Clear();

                int totalInventory = 0;
                var inventoryData = new Dictionary<string, int>();

                while (reader.Read())
                {
                    string productType = reader["Loại sản phẩm"].ToString();
                    int inventory = Convert.ToInt32(reader["Tổng tồn kho"]);
                    inventoryData.Add(productType, inventory);
                    totalInventory += inventory;
                }

                foreach (var item in inventoryData)
                {
                    double percentage = totalInventory > 0 ? (double)item.Value / totalInventory * 100 : 0;
                    var dataPoint = chart1.Series["Tồn kho theo loại"].Points.Add(item.Value);
                    dataPoint.AxisLabel = item.Key;
                    dataPoint.LegendText = $"{item.Key}: {item.Value} ({percentage:F1}%)";
                    dataPoint.Label = $"{percentage:F1}%";
                    dataPoint.LabelFormat = "#.#'%'";
                    dataPoint.Font = new Font("Arial", 8, FontStyle.Bold);
                }

                string chartTitle = "TỶ LỆ TỒN KHO THEO LOẠI SẢN PHẨM";
                if (!string.IsNullOrEmpty(selectedKho) && selectedKho != "all")
                {
                    chartTitle += $" - KHO: {cbb_kho.Text.ToUpper()}";
                }

                Title title = new Title(chartTitle, Docking.Top, new Font("Arial", 10, FontStyle.Bold), Color.Black);
                chart1.Titles.Add(title);

                chart1.Series["Tồn kho theo loại"].IsValueShownAsLabel = true;
                chart1.Series["Tồn kho theo loại"]["PieLabelStyle"] = "Outside";
                chart1.Series["Tồn kho theo loại"].LabelForeColor = Color.Black;
                chart1.Series["Tồn kho theo loại"].Font = new Font("Arial", 8, FontStyle.Bold);
                chart1.Legends[0].Font = new Font("Arial", 8);
                chart1.Legends[0].Docking = Docking.Bottom;

                Random rnd = new Random();
                foreach (DataPoint point in chart1.Series["Tồn kho theo loại"].Points)
                {
                    point.Color = Color.FromArgb(rnd.Next(150, 255), rnd.Next(150, 255), rnd.Next(150, 255));
                }
            }
        }

        private Color GetRandomColor()
        {
            Random random = new Random();
            return Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
        }
    }
}