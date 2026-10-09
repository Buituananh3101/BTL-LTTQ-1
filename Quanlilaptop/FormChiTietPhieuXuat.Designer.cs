namespace Quanlilaptop
{
    partial class FormChiTietPhieuXuat
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
            this.btnThoat = new System.Windows.Forms.Button();
            this.btnThem = new System.Windows.Forms.Button();
            this.cbbMakho = new System.Windows.Forms.ComboBox();
            this.cbbMamay = new System.Windows.Forms.ComboBox();
            this.cbbMaphieu = new System.Windows.Forms.ComboBox();
            this.txtdon_gia = new System.Windows.Forms.TextBox();
            this.txtso_luong = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.cbb_Lohang = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // btnThoat
            // 
            this.btnThoat.Location = new System.Drawing.Point(302, 226);
            this.btnThoat.Name = "btnThoat";
            this.btnThoat.Size = new System.Drawing.Size(75, 23);
            this.btnThoat.TabIndex = 26;
            this.btnThoat.Text = "Thoát";
            this.btnThoat.UseVisualStyleBackColor = true;
            this.btnThoat.Click += new System.EventHandler(this.btnThoat_Click);
            // 
            // btnThem
            // 
            this.btnThem.Location = new System.Drawing.Point(111, 226);
            this.btnThem.Name = "btnThem";
            this.btnThem.Size = new System.Drawing.Size(124, 23);
            this.btnThem.TabIndex = 25;
            this.btnThem.Text = "Thêm chi tiết phiếu";
            this.btnThem.UseVisualStyleBackColor = true;
            this.btnThem.Click += new System.EventHandler(this.btnThem_Click);
            // 
            // cbbMakho
            // 
            this.cbbMakho.FormattingEnabled = true;
            this.cbbMakho.Location = new System.Drawing.Point(129, 170);
            this.cbbMakho.Name = "cbbMakho";
            this.cbbMakho.Size = new System.Drawing.Size(121, 21);
            this.cbbMakho.TabIndex = 24;
            this.cbbMakho.SelectedIndexChanged += new System.EventHandler(this.cbbMakho_SelectedIndexChanged);
            // 
            // cbbMamay
            // 
            this.cbbMamay.FormattingEnabled = true;
            this.cbbMamay.Location = new System.Drawing.Point(129, 128);
            this.cbbMamay.Name = "cbbMamay";
            this.cbbMamay.Size = new System.Drawing.Size(121, 21);
            this.cbbMamay.TabIndex = 23;
            this.cbbMamay.SelectedIndexChanged += new System.EventHandler(this.cbbMamay_SelectedIndexChanged);
            // 
            // cbbMaphieu
            // 
            this.cbbMaphieu.FormattingEnabled = true;
            this.cbbMaphieu.Location = new System.Drawing.Point(129, 88);
            this.cbbMaphieu.Name = "cbbMaphieu";
            this.cbbMaphieu.Size = new System.Drawing.Size(121, 21);
            this.cbbMaphieu.TabIndex = 22;
            // 
            // txtdon_gia
            // 
            this.txtdon_gia.Location = new System.Drawing.Point(416, 128);
            this.txtdon_gia.Name = "txtdon_gia";
            this.txtdon_gia.Size = new System.Drawing.Size(122, 20);
            this.txtdon_gia.TabIndex = 21;
            // 
            // txtso_luong
            // 
            this.txtso_luong.Location = new System.Drawing.Point(416, 89);
            this.txtso_luong.Name = "txtso_luong";
            this.txtso_luong.Size = new System.Drawing.Size(122, 20);
            this.txtso_luong.TabIndex = 20;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label6.Location = new System.Drawing.Point(299, 93);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(72, 16);
            this.label6.TabIndex = 19;
            this.label6.Text = "Số lượng:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label5.Location = new System.Drawing.Point(299, 133);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(66, 16);
            this.label5.TabIndex = 18;
            this.label5.Text = "Đơn giá:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label4.Location = new System.Drawing.Point(25, 175);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 16);
            this.label4.TabIndex = 17;
            this.label4.Text = "Mã kho:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label3.Location = new System.Drawing.Point(25, 133);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 16);
            this.label3.TabIndex = 16;
            this.label3.Text = "Mã máy:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label2.Location = new System.Drawing.Point(25, 93);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 16);
            this.label2.TabIndex = 15;
            this.label2.Text = "Mã phiếu:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label1.Location = new System.Drawing.Point(154, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(204, 25);
            this.label1.TabIndex = 14;
            this.label1.Text = "Chi tiết phiếu xuất";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label7.Location = new System.Drawing.Point(299, 171);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(70, 16);
            this.label7.TabIndex = 27;
            this.label7.Text = "Lô hàng :";
            // 
            // cbb_Lohang
            // 
            this.cbb_Lohang.FormattingEnabled = true;
            this.cbb_Lohang.Location = new System.Drawing.Point(417, 170);
            this.cbb_Lohang.Name = "cbb_Lohang";
            this.cbb_Lohang.Size = new System.Drawing.Size(121, 21);
            this.cbb_Lohang.TabIndex = 28;
            // 
            // FormChiTietPhieuXuat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(56)))), ((int)(((byte)(73)))));
            this.ClientSize = new System.Drawing.Size(550, 277);
            this.Controls.Add(this.cbb_Lohang);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.btnThoat);
            this.Controls.Add(this.btnThem);
            this.Controls.Add(this.cbbMakho);
            this.Controls.Add(this.cbbMamay);
            this.Controls.Add(this.cbbMaphieu);
            this.Controls.Add(this.txtdon_gia);
            this.Controls.Add(this.txtso_luong);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormChiTietPhieuXuat";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormChiTietPhieuXuat";
            this.Load += new System.EventHandler(this.FormChiTietPhieuXuat_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FormChiTietPhieuXuat_MouseDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnThoat;
        private System.Windows.Forms.Button btnThem;
        private System.Windows.Forms.ComboBox cbbMakho;
        private System.Windows.Forms.ComboBox cbbMamay;
        private System.Windows.Forms.ComboBox cbbMaphieu;
        private System.Windows.Forms.TextBox txtdon_gia;
        private System.Windows.Forms.TextBox txtso_luong;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cbb_Lohang;
    }
}