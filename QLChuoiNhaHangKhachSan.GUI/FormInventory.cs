using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLChuoiNhaHangKhachSan.GUI
{
    public partial class FormInventory : Form
    {
        

        public FormInventory()
        {
            InitializeComponent();
        }

        private void pnlMainWrapper_Paint(object sender, PaintEventArgs e)
        {

        }

        private void iconPictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void chipTotal_Click(object sender, EventArgs e)
        {

        }

        private void guna2HtmlLabel3_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tlpFilter_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cboDonVi_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void FormInventory_Load(object sender, EventArgs e)
        {
            // ===== Loại kho =====
            cboLoaiKho.Items.Clear();
            cboLoaiKho.Items.AddRange(new object[]
            {
                        "Kho Nguyên Liệu",
                        "Kho Thiết Bị",
                        "Tất cả"
            });
            cboLoaiKho.SelectedIndex = 2; // mặc định Tất cả

            // ===== Đơn vị (NH / KS) =====
            cboDonVi.Items.Clear();
            cboDonVi.Items.AddRange(new object[]
            {
                        "NH01",
                        "KS01",
                        "Tất cả"
            });
            cboDonVi.SelectedIndex = 2; // mặc định Tất cả
        }

        private void cboLoaiKho_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void lbDanhSachTonKhotext_Click(object sender, EventArgs e)
        {

        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
    
}
