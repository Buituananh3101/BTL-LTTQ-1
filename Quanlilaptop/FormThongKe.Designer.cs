namespace Quanlilaptop
{
    partial class FormThongKe
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.tke_tonkho = new System.Windows.Forms.ToolStripMenuItem();
            this.tke_nhaphang = new System.Windows.Forms.ToolStripMenuItem();
            this.tke_xuathang = new System.Windows.Forms.ToolStripMenuItem();
            this.tke_doanhthu = new System.Windows.Forms.ToolStripMenuItem();
            this.panel = new System.Windows.Forms.Panel();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tke_tonkho,
            this.tke_nhaphang,
            this.tke_xuathang,
            this.tke_doanhthu});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1038, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip1_ItemClicked);
            // 
            // tke_tonkho
            // 
            this.tke_tonkho.Name = "tke_tonkho";
            this.tke_tonkho.Size = new System.Drawing.Size(63, 20);
            this.tke_tonkho.Text = "Tồn kho";
            this.tke_tonkho.Click += new System.EventHandler(this.tke_tonkho_Click);
            // 
            // tke_nhaphang
            // 
            this.tke_nhaphang.Name = "tke_nhaphang";
            this.tke_nhaphang.Size = new System.Drawing.Size(78, 20);
            this.tke_nhaphang.Text = "Nhập hàng";
            this.tke_nhaphang.Click += new System.EventHandler(this.tke_nhaphang_Click);
            // 
            // tke_xuathang
            // 
            this.tke_xuathang.Name = "tke_xuathang";
            this.tke_xuathang.Size = new System.Drawing.Size(73, 20);
            this.tke_xuathang.Text = "Xuất hàng";
            this.tke_xuathang.Click += new System.EventHandler(this.tke_xuathang_Click);
            // 
            // tke_doanhthu
            // 
            this.tke_doanhthu.Name = "tke_doanhthu";
            this.tke_doanhthu.Size = new System.Drawing.Size(75, 20);
            this.tke_doanhthu.Text = "Doanh thu";
            this.tke_doanhthu.Click += new System.EventHandler(this.tke_doanhthu_Click);
            // 
            // panel
            // 
            this.panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel.Location = new System.Drawing.Point(0, 24);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(1038, 477);
            this.panel.TabIndex = 1;
            this.panel.Paint += new System.Windows.Forms.PaintEventHandler(this.panel_Paint);
            // 
            // FormThongKe
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1038, 501);
            this.Controls.Add(this.panel);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormThongKe";
            this.Text = "Thống kê";
            this.Load += new System.EventHandler(this.FormThongKe_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tke_tonkho;
        private System.Windows.Forms.ToolStripMenuItem tke_nhaphang;
        private System.Windows.Forms.ToolStripMenuItem tke_xuathang;
        private System.Windows.Forms.ToolStripMenuItem tke_doanhthu;
    }
}
