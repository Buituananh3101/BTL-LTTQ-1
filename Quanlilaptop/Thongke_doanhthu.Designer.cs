namespace Quanlilaptop
{
    partial class Thongke_doanhthu
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label4 = new System.Windows.Forms.Label();
            this.btn_reset = new System.Windows.Forms.Button();
            this.btn_apdung = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dateTime_end = new System.Windows.Forms.DateTimePicker();
            this.dateTime_begin = new System.Windows.Forms.DateTimePicker();
            this.chart_loinhuan = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.loi_nhuan_theo_ngay = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.cbbThoiGianThongKe = new System.Windows.Forms.ComboBox();
            this.cboKho = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart_loinhuan)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.loi_nhuan_theo_ngay)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(16, 98);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.Size = new System.Drawing.Size(571, 269);
            this.dataGridView1.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 50);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 16);
            this.label4.TabIndex = 9;
            this.label4.Text = "Đến ngày";
            // 
            // btn_reset
            // 
            this.btn_reset.Location = new System.Drawing.Point(516, 47);
            this.btn_reset.Margin = new System.Windows.Forms.Padding(4);
            this.btn_reset.Name = "btn_reset";
            this.btn_reset.Size = new System.Drawing.Size(100, 28);
            this.btn_reset.TabIndex = 7;
            this.btn_reset.Text = "Làm mới";
            this.btn_reset.UseVisualStyleBackColor = true;
            this.btn_reset.Click += new System.EventHandler(this.btn_reset_Click);
            // 
            // btn_apdung
            // 
            this.btn_apdung.Location = new System.Drawing.Point(516, 5);
            this.btn_apdung.Margin = new System.Windows.Forms.Padding(4);
            this.btn_apdung.Name = "btn_apdung";
            this.btn_apdung.Size = new System.Drawing.Size(100, 28);
            this.btn_apdung.TabIndex = 6;
            this.btn_apdung.Text = "Áp dụng ";
            this.btn_apdung.UseVisualStyleBackColor = true;
            this.btn_apdung.Click += new System.EventHandler(this.btn_apdung_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Location = new System.Drawing.Point(28, 17);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 18);
            this.label3.TabIndex = 5;
            this.label3.Text = "Từ ngày";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dateTime_end);
            this.panel1.Controls.Add(this.dateTime_begin);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.btn_reset);
            this.panel1.Controls.Add(this.btn_apdung);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Location = new System.Drawing.Point(1, 10);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(623, 81);
            this.panel1.TabIndex = 3;
            // 
            // dateTime_end
            // 
            this.dateTime_end.Location = new System.Drawing.Point(151, 47);
            this.dateTime_end.Margin = new System.Windows.Forms.Padding(4);
            this.dateTime_end.Name = "dateTime_end";
            this.dateTime_end.Size = new System.Drawing.Size(265, 22);
            this.dateTime_end.TabIndex = 11;
            // 
            // dateTime_begin
            // 
            this.dateTime_begin.Location = new System.Drawing.Point(151, 15);
            this.dateTime_begin.Margin = new System.Windows.Forms.Padding(4);
            this.dateTime_begin.Name = "dateTime_begin";
            this.dateTime_begin.Size = new System.Drawing.Size(265, 22);
            this.dateTime_begin.TabIndex = 10;
            // 
            // chart_loinhuan
            // 
            chartArea1.Name = "ChartArea1";
            this.chart_loinhuan.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chart_loinhuan.Legends.Add(legend1);
            this.chart_loinhuan.Location = new System.Drawing.Point(680, 27);
            this.chart_loinhuan.Margin = new System.Windows.Forms.Padding(4);
            this.chart_loinhuan.Name = "chart_loinhuan";
            this.chart_loinhuan.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Grayscale;
            series1.ChartArea = "ChartArea1";
            series1.CustomProperties = "PointWidth=0.4";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart_loinhuan.Series.Add(series1);
            this.chart_loinhuan.Size = new System.Drawing.Size(561, 369);
            this.chart_loinhuan.TabIndex = 5;
            this.chart_loinhuan.Text = "chart1";
            // 
            // loi_nhuan_theo_ngay
            // 
            chartArea2.Name = "ChartArea1";
            this.loi_nhuan_theo_ngay.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.loi_nhuan_theo_ngay.Legends.Add(legend2);
            this.loi_nhuan_theo_ngay.Location = new System.Drawing.Point(244, 404);
            this.loi_nhuan_theo_ngay.Margin = new System.Windows.Forms.Padding(4);
            this.loi_nhuan_theo_ngay.Name = "loi_nhuan_theo_ngay";
            this.loi_nhuan_theo_ngay.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Excel;
            series2.ChartArea = "ChartArea1";
            series2.CustomProperties = "PointWidth=0.4";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.loi_nhuan_theo_ngay.Series.Add(series2);
            this.loi_nhuan_theo_ngay.Size = new System.Drawing.Size(997, 369);
            this.loi_nhuan_theo_ngay.TabIndex = 6;
            this.loi_nhuan_theo_ngay.Text = "chart1";
            // 
            // cbbThoiGianThongKe
            // 
            this.cbbThoiGianThongKe.FormattingEnabled = true;
            this.cbbThoiGianThongKe.Location = new System.Drawing.Point(28, 452);
            this.cbbThoiGianThongKe.Name = "cbbThoiGianThongKe";
            this.cbbThoiGianThongKe.Size = new System.Drawing.Size(137, 24);
            this.cbbThoiGianThongKe.TabIndex = 7;
            this.cbbThoiGianThongKe.SelectedIndexChanged += new System.EventHandler(this.cbbThoiGianThongKe_SelectedIndexChanged);
            // 
            // cboKho
            // 
            this.cboKho.FormattingEnabled = true;
            this.cboKho.Location = new System.Drawing.Point(28, 513);
            this.cboKho.Name = "cboKho";
            this.cboKho.Size = new System.Drawing.Size(137, 24);
            this.cboKho.TabIndex = 8;
            this.cboKho.SelectedIndexChanged += new System.EventHandler(this.cboKho_SelectedIndexChanged);
            // 
            // Thongke_doanhthu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1311, 768);
            this.Controls.Add(this.cboKho);
            this.Controls.Add(this.cbbThoiGianThongKe);
            this.Controls.Add(this.loi_nhuan_theo_ngay);
            this.Controls.Add(this.chart_loinhuan);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Thongke_doanhthu";
            this.Text = "Thongke_doanhthu";
            this.Load += new System.EventHandler(this.Thongke_doanhthu_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart_loinhuan)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.loi_nhuan_theo_ngay)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btn_reset;
        private System.Windows.Forms.Button btn_apdung;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DateTimePicker dateTime_end;
        private System.Windows.Forms.DateTimePicker dateTime_begin;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart_loinhuan;
        private System.Windows.Forms.DataVisualization.Charting.Chart loi_nhuan_theo_ngay;
        private System.Windows.Forms.ComboBox cbbThoiGianThongKe;
        private System.Windows.Forms.ComboBox cboKho;
    }
}