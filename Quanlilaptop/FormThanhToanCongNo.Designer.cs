namespace Quanlilaptop
{
    partial class FormThanhToanCongNo
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
            this.dgvThanhtoan = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btnThem = new System.Windows.Forms.Button();
            this.btnSua = new System.Windows.Forms.Button();
            this.btnXoa = new System.Windows.Forms.Button();
            this.btnXuat = new System.Windows.Forms.Button();
            this.cbbMaCN = new System.Windows.Forms.ComboBox();
            this.txtMaTT = new System.Windows.Forms.TextBox();
            this.txtSotien = new System.Windows.Forms.TextBox();
            this.txtPhuongthuc = new System.Windows.Forms.TextBox();
            this.txtGhichu = new System.Windows.Forms.TextBox();
            this.dtpTT = new System.Windows.Forms.DateTimePicker();
            this.btnThoat = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvThanhtoan)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvThanhtoan
            // 
            this.dgvThanhtoan.AllowUserToAddRows = false;
            this.dgvThanhtoan.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvThanhtoan.Dock = System.Windows.Forms.DockStyle.Right;
            this.dgvThanhtoan.Location = new System.Drawing.Point(541, 0);
            this.dgvThanhtoan.Name = "dgvThanhtoan";
            this.dgvThanhtoan.Size = new System.Drawing.Size(480, 566);
            this.dgvThanhtoan.TabIndex = 12;
            this.dgvThanhtoan.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvThanhtoan_CellClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(31, 63);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 20);
            this.label1.TabIndex = 13;
            this.label1.Text = "Mã thanh toán";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(33, 123);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 20);
            this.label2.TabIndex = 14;
            this.label2.Text = "Mã công nợ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(31, 186);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(141, 20);
            this.label3.TabIndex = 15;
            this.label3.Text = "Ngày thanh toán";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(31, 249);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(158, 20);
            this.label4.TabIndex = 16;
            this.label4.Text = "Số tiền thanh toán";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(31, 315);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(202, 20);
            this.label5.TabIndex = 17;
            this.label5.Text = "Phương thức thanh toán";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(33, 386);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 20);
            this.label6.TabIndex = 18;
            this.label6.Text = "Ghi chú";
            // 
            // btnThem
            // 
            this.btnThem.Location = new System.Drawing.Point(29, 456);
            this.btnThem.Name = "btnThem";
            this.btnThem.Size = new System.Drawing.Size(75, 50);
            this.btnThem.TabIndex = 19;
            this.btnThem.Text = "Thêm";
            this.btnThem.UseVisualStyleBackColor = true;
            this.btnThem.Click += new System.EventHandler(this.btnThem_Click);
            // 
            // btnSua
            // 
            this.btnSua.Location = new System.Drawing.Point(127, 456);
            this.btnSua.Name = "btnSua";
            this.btnSua.Size = new System.Drawing.Size(75, 50);
            this.btnSua.TabIndex = 20;
            this.btnSua.Text = "Sửa";
            this.btnSua.UseVisualStyleBackColor = true;
            this.btnSua.Click += new System.EventHandler(this.btnSua_Click);
            // 
            // btnXoa
            // 
            this.btnXoa.Location = new System.Drawing.Point(228, 456);
            this.btnXoa.Name = "btnXoa";
            this.btnXoa.Size = new System.Drawing.Size(75, 50);
            this.btnXoa.TabIndex = 21;
            this.btnXoa.Text = "Xoá";
            this.btnXoa.UseVisualStyleBackColor = true;
            this.btnXoa.Click += new System.EventHandler(this.btnXoa_Click);
            // 
            // btnXuat
            // 
            this.btnXuat.Location = new System.Drawing.Point(330, 456);
            this.btnXuat.Name = "btnXuat";
            this.btnXuat.Size = new System.Drawing.Size(75, 50);
            this.btnXuat.TabIndex = 22;
            this.btnXuat.Text = "Xuất Word";
            this.btnXuat.UseVisualStyleBackColor = true;
            this.btnXuat.Click += new System.EventHandler(this.btnXuat_Click);
            // 
            // cbbMaCN
            // 
            this.cbbMaCN.FormattingEnabled = true;
            this.cbbMaCN.Location = new System.Drawing.Point(277, 122);
            this.cbbMaCN.Name = "cbbMaCN";
            this.cbbMaCN.Size = new System.Drawing.Size(202, 21);
            this.cbbMaCN.TabIndex = 24;
            // 
            // txtMaTT
            // 
            this.txtMaTT.Location = new System.Drawing.Point(277, 63);
            this.txtMaTT.Name = "txtMaTT";
            this.txtMaTT.Size = new System.Drawing.Size(202, 20);
            this.txtMaTT.TabIndex = 25;
            // 
            // txtSotien
            // 
            this.txtSotien.Location = new System.Drawing.Point(277, 249);
            this.txtSotien.Name = "txtSotien";
            this.txtSotien.Size = new System.Drawing.Size(202, 20);
            this.txtSotien.TabIndex = 26;
            // 
            // txtPhuongthuc
            // 
            this.txtPhuongthuc.Location = new System.Drawing.Point(277, 315);
            this.txtPhuongthuc.Name = "txtPhuongthuc";
            this.txtPhuongthuc.Size = new System.Drawing.Size(202, 20);
            this.txtPhuongthuc.TabIndex = 27;
            // 
            // txtGhichu
            // 
            this.txtGhichu.Location = new System.Drawing.Point(277, 386);
            this.txtGhichu.Name = "txtGhichu";
            this.txtGhichu.Size = new System.Drawing.Size(202, 20);
            this.txtGhichu.TabIndex = 28;
            // 
            // dtpTT
            // 
            this.dtpTT.Location = new System.Drawing.Point(277, 185);
            this.dtpTT.Name = "dtpTT";
            this.dtpTT.Size = new System.Drawing.Size(200, 20);
            this.dtpTT.TabIndex = 29;
            // 
            // btnThoat
            // 
            this.btnThoat.Location = new System.Drawing.Point(434, 456);
            this.btnThoat.Name = "btnThoat";
            this.btnThoat.Size = new System.Drawing.Size(75, 50);
            this.btnThoat.TabIndex = 30;
            this.btnThoat.Text = "Thoát";
            this.btnThoat.UseVisualStyleBackColor = true;
            this.btnThoat.Click += new System.EventHandler(this.btnThoat_Click);
            // 
            // FormThanhToanCongNo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(56)))), ((int)(((byte)(73)))));
            this.ClientSize = new System.Drawing.Size(1021, 566);
            this.Controls.Add(this.btnThoat);
            this.Controls.Add(this.dtpTT);
            this.Controls.Add(this.txtGhichu);
            this.Controls.Add(this.txtPhuongthuc);
            this.Controls.Add(this.txtSotien);
            this.Controls.Add(this.txtMaTT);
            this.Controls.Add(this.cbbMaCN);
            this.Controls.Add(this.btnXuat);
            this.Controls.Add(this.btnXoa);
            this.Controls.Add(this.btnSua);
            this.Controls.Add(this.btnThem);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvThanhtoan);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormThanhToanCongNo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormThanhToanCongNo";
            this.Load += new System.EventHandler(this.FormThanhToanCongNo_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvThanhtoan)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvThanhtoan;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnThem;
        private System.Windows.Forms.Button btnSua;
        private System.Windows.Forms.Button btnXoa;
        private System.Windows.Forms.Button btnXuat;
        private System.Windows.Forms.ComboBox cbbMaCN;
        private System.Windows.Forms.TextBox txtMaTT;
        private System.Windows.Forms.TextBox txtSotien;
        private System.Windows.Forms.TextBox txtPhuongthuc;
        private System.Windows.Forms.TextBox txtGhichu;
        private System.Windows.Forms.DateTimePicker dtpTT;
        private System.Windows.Forms.Button btnThoat;
    }
}