using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace Quanlilaptop
{
    public partial class FormThemNCC : Form
    {
        private FormNhaCungCap formNCC;
        public FormThemNCC(FormNhaCungCap form)
        {
            InitializeComponent();
            formNCC = form;
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                string maNCC = txtMaNCC.Text.Trim();
                string tenNCC = txtTenNCC.Text.Trim();
                string sdt = txtSdtNCC.Text.Trim();
                string dia_chi = txtdia_chiNCC.Text.Trim();

                using (MySqlConnection conn = new MySqlConnection("Server=localhost;Database=quanlimaytinh;Port=3306;User ID=root;Password="))
                {
                    conn.Open();
                    MySqlCommand command = new MySqlCommand("spThemNCC", conn);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@p_ma_nha_cung_cap", maNCC);
                    command.Parameters.AddWithValue("@p_ten_nha_cung_cap", tenNCC);
                    command.Parameters.AddWithValue("@p_Sdt", sdt);
                    command.Parameters.AddWithValue("@p_dia_chi", dia_chi);

                    command.ExecuteNonQuery();
                }

                MessageBox.Show("Thêm nhà cung cấp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                formNCC.napdgvNhaCungCap();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Lỗi khi thêm nhà cung cấp: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.Close();
        }
    }
}
