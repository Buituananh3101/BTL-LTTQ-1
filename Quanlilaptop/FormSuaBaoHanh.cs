using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Quanlilaptop
{
    public partial class FormSuaBaoHanh : Form
    {
        public FormSuaBaoHanh()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void FormSuaBaoHanh_Load(object sender, EventArgs e)
        {
            string query = "SELECT ma_phieu_bao_hanh FROM chitietbaohanh";
            using (MySqlConnection conn = new MySqlConnection("server=localhost;database=quanlimaytinh;uid=root;pwd=;"))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cbbMabaohanh.Items.Add(reader["ma_phieu_bao_hanh"].ToString());
                }
            }

        }
        
        public event EventHandler BaoHanhUpdated;


        private void btn__Click(object sender, EventArgs e)
        {
            if (cbbMabaohanh.SelectedItem == null || cbb_Ketqua.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maPhieuBaoHanh = cbbMabaohanh.SelectedItem.ToString();
            string ketQua = cbb_Ketqua.SelectedItem.ToString();

            using (MySqlConnection conn = new MySqlConnection("server=localhost;database=quanlimaytinh;uid=root;pwd=;"))
            {
                conn.Open();

                string checkQuery = "SELECT ket_qua_bao_hanh FROM chitietbaohanh WHERE ma_phieu_bao_hanh = @maPhieu";
                MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("@maPhieu", maPhieuBaoHanh);
                object currentResult = checkCmd.ExecuteScalar();

                if (currentResult != null)
                {
                    string currentKetQua = currentResult.ToString();
                    if (currentKetQua == "Đã sửa chữa xong" || currentKetQua == "Từ chối bảo hành")
                    {
                        MessageBox.Show("Phiếu bảo hành này đã hoàn tất. Không thể cập nhật thêm!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                
                string query = "UPDATE chitietbaohanh SET ket_qua_bao_hanh = @ketQua WHERE ma_phieu_bao_hanh = @maPhieu";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ketQua", ketQua);
                cmd.Parameters.AddWithValue("@maPhieu", maPhieuBaoHanh);
                int rows = cmd.ExecuteNonQuery();

                if (rows > 0)
                {
                    MessageBox.Show("Cập nhật thành công!", "Thông báo");
                    BaoHanhUpdated?.Invoke(this, EventArgs.Empty); 
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Không có dữ liệu nào được cập nhật.", "Thông báo");
                }
            }
        }



        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
