using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quanlilaptop
{
    public partial class FormThemKhachHang : Form
    {
        private FormKhachHang formKH;
        public FormThemKhachHang(FormKhachHang form)
        {
            InitializeComponent();
            formKH = form;
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                string idKH = txtID.Text.Trim();       
                string hoTen = txtHoten.Text.Trim();       
                string sdt = txtSdt.Text.Trim(); 
                string diaChi = txtDiachi.Text.Trim();

                using (MySqlConnection conn = new MySqlConnection("Server=localhost;Database=quanlimaytinh;Port=3306;User ID=root;Password="))
                {
                    conn.Open();
                    MySqlCommand command = new MySqlCommand("spThemKhachHang", conn);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@p_id_khach_hang", idKH);
                    command.Parameters.AddWithValue("@p_ho_ten", hoTen);
                    command.Parameters.AddWithValue("@p_sdt", sdt);
                    command.Parameters.AddWithValue("@p_dia_chi", diaChi);

                    command.ExecuteNonQuery();
                }

                MessageBox.Show("Thêm khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                formKH.napdgvkhachhang(); 
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Lỗi khi thêm khách hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.Close();

        }
    }
}
