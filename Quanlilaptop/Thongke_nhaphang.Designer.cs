namespace Quanlilaptop
{
    partial class Thongke_nhaphang
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.dateTime_end = new System.Windows.Forms.DateTimePicker();
            this.dateTime_begin = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btn_reset = new System.Windows.Forms.Button();
            this.btn_apdung = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_search = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cbb_nhacungcap = new System.Windows.Forms.ComboBox();
            this.cbb_loaisanpham = new System.Windows.Forms.ComboBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.chart_loaisp = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_loaisp)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dateTime_end);
            this.panel1.Controls.Add(this.dateTime_begin);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.btn_reset);
            this.panel1.Controls.Add(this.btn_apdung);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.txt_search);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.cbb_nhacungcap);
            this.panel1.Controls.Add(this.cbb_loaisanpham);
            this.panel1.Location = new System.Drawing.Point(1, 8);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(794, 66);
            this.panel1.TabIndex = 3;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // dateTime_end
            // 
            this.dateTime_end.Location = new System.Drawing.Point(467, 37);
            this.dateTime_end.Name = "dateTime_end";
            this.dateTime_end.Size = new System.Drawing.Size(203, 20);
            this.dateTime_end.TabIndex = 15;
            // 
            // dateTime_begin
            // 
            this.dateTime_begin.Location = new System.Drawing.Point(467, 11);
            this.dateTime_begin.Name = "dateTime_begin";
            this.dateTime_begin.Size = new System.Drawing.Size(203, 20);
            this.dateTime_begin.TabIndex = 14;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(410, 42);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Đến ngày";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(413, 15);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(46, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Từ ngày";
            // 
            // btn_reset
            // 
            this.btn_reset.Location = new System.Drawing.Point(676, 36);
            this.btn_reset.Name = "btn_reset";
            this.btn_reset.Size = new System.Drawing.Size(75, 23);
            this.btn_reset.TabIndex = 7;
            this.btn_reset.Text = "Làm mới";
            this.btn_reset.UseVisualStyleBackColor = true;
            this.btn_reset.Click += new System.EventHandler(this.btn_reset_Click);
            // 
            // btn_apdung
            // 
            this.btn_apdung.Location = new System.Drawing.Point(676, 11);
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
            this.label3.Location = new System.Drawing.Point(218, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Tìm kiếm";
            // 
            // txt_search
            // 
            this.txt_search.Location = new System.Drawing.Point(273, 11);
            this.txt_search.Name = "txt_search";
            this.txt_search.Size = new System.Drawing.Size(101, 20);
            this.txt_search.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Nhà cung cấp";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Loại sản phẩm";
            // 
            // cbb_nhacungcap
            // 
            this.cbb_nhacungcap.FormattingEnabled = true;
            this.cbb_nhacungcap.Location = new System.Drawing.Point(96, 36);
            this.cbb_nhacungcap.Name = "cbb_nhacungcap";
            this.cbb_nhacungcap.Size = new System.Drawing.Size(104, 21);
            this.cbb_nhacungcap.TabIndex = 1;
            // 
            // cbb_loaisanpham
            // 
            this.cbb_loaisanpham.FormattingEnabled = true;
            this.cbb_loaisanpham.Location = new System.Drawing.Point(96, 9);
            this.cbb_loaisanpham.Name = "cbb_loaisanpham";
            this.cbb_loaisanpham.Size = new System.Drawing.Size(104, 21);
            this.cbb_loaisanpham.TabIndex = 0;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 80);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(492, 485);
            this.dataGridView1.TabIndex = 4;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // chart_loaisp
            // 
            chartArea1.Name = "ChartArea1";
            this.chart_loaisp.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chart_loaisp.Legends.Add(legend1);
            this.chart_loaisp.Location = new System.Drawing.Point(538, 123);
            this.chart_loaisp.Name = "chart_loaisp";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
            series1.Label = "#PERCENT";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart_loaisp.Series.Add(series1);
            this.chart_loaisp.Size = new System.Drawing.Size(378, 371);
            this.chart_loaisp.TabIndex = 16;
            this.chart_loaisp.Text = "chart1";
            // 
            // Thongke_nhaphang
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(951, 573);
            this.Controls.Add(this.chart_loaisp);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.dataGridView1);
            this.Name = "Thongke_nhaphang";
            this.Text = "Thongke_nhaphang";
            this.Load += new System.EventHandler(this.Thongke_nhaphang_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_loaisp)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btn_reset;
        private System.Windows.Forms.Button btn_apdung;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt_search;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbb_nhacungcap;
        private System.Windows.Forms.ComboBox cbb_loaisanpham;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DateTimePicker dateTime_end;
        private System.Windows.Forms.DateTimePicker dateTime_begin;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart_loaisp;
    }
}