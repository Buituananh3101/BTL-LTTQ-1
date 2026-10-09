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
using System.Windows.Forms.DataVisualization.Charting;

namespace Quanlilaptop
{
    public partial class FormThongKe : Form
    {


        public FormThongKe()
        {
            InitializeComponent();
        }
        private void OpenFormInPanel<T>() where T : Form, new()
        {
            foreach (Form form in Application.OpenForms.OfType<T>().ToList())
            {
                form.Close();
            }
            T frm = new T();
            frm.TopLevel = false;
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.Dock = DockStyle.Fill;

            panel.Controls.Clear();
            panel.Controls.Add(frm);
            frm.Show();
        }


        private void LoadFormToPanel(Form form)
        {

        }
        private void FormThongKe_Load(object sender, EventArgs e)
        {

        }



        private void tke_tonkho_Click(object sender, EventArgs e)
        {
            OpenFormInPanel<Thongke_tonkho>();

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void tke_nhaphang_Click(object sender, EventArgs e)
        {
            OpenFormInPanel<Thongke_nhaphang>();
        }

        private void tke_xuathang_Click(object sender, EventArgs e)
        {
            OpenFormInPanel<Thongke_xuathang>();
        }

        private void tke_doanhthu_Click(object sender, EventArgs e)
        {
            OpenFormInPanel<Thongke_doanhthu>();
        }

        private void panel_Paint(object sender, PaintEventArgs e)
        {

        }

        

        private void FormThongKe_Load_1(object sender, EventArgs e)
        {

        }

        private void FormThongKe_Load_2(object sender, EventArgs e)
        {

        }
    }
}
