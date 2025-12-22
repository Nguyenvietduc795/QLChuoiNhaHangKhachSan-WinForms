using Guna.UI2.WinForms;
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
    public partial class FormInventory2 : Form
    {
        public FormInventory2()
        {
            InitializeComponent();
            btnTaoPhieuInventory2.Click += btnTaoPhieuInventory2_Click;
        }

        

        private void lblTitle_Click(object sender, EventArgs e)
        {

        }




        private void FormInventory2_Load(object sender, EventArgs e)
        {
           
            DGdgvPhieu.RowTemplate.Height = 60;
            DGdgvPhieu.ColumnHeadersHeight = 48;
            DGLSPhieuNhap.RowTemplate.Height =60 ;
            DGLSPhieuNhap.ColumnHeadersHeight = 48;
            LoadDataDemo();
            LoadDGLSPhieuNhap_Demo();


        }

        private void btntheminventory2_Click(object sender, EventArgs e)
        {

        }

        private void btnsuainventory2_Click(object sender, EventArgs e)
        {

        }

        private void btnxoaInventory2_Click(object sender, EventArgs e)
        {

        }

        private void btnNhapKhoInventory2_Click(object sender, EventArgs e)
        {

        }

        private void btnXuatKhoInventory2_Click(object sender, EventArgs e)
        {

        }

        private void btnTaoPhieuInventory2_Click(object sender, EventArgs e)
        {
           MessageBox.Show("OK - Tạo phiếu");
        }


        // ================== DATA DEMO ==================
        private void LoadDataDemo()
        {
            // Ngắt bind cũ nếu có
            DGdgvPhieu.DataSource = null;

            var dt = new DataTable();
            dt.Columns.Add("Mã Phiếu");
            dt.Columns.Add("Ngày");
            dt.Columns.Add("Lọai Kho");
            dt.Columns.Add("Đơn Vị");
            dt.Columns.Add("Trạng Thái");

            dt.Rows.Add("PN001", "10/12/2025", "Nguyên liệu", "NH01", "Nhập");
            dt.Rows.Add("PN002", "11/12/2025", "Nguyên liệu", "NH01", "Hoàn tất");
            dt.Rows.Add("PX001", "12/12/2025", "Thiết bị", "KS01", "Xuất");
            dt.Rows.Add("PX002", "13/12/2025", "Thiết bị", "KS01", "Hủy");

            DGdgvPhieu.AutoGenerateColumns = true;   // tự sinh cột theo DataTable
            DGdgvPhieu.AllowUserToAddRows = false;
            DGdgvPhieu.RowHeadersVisible = false;
            
            DGdgvPhieu.DataSource = dt;
           
        }


        private void TbDanhsachtonkho_Paint(object sender, PaintEventArgs e)
        {

        }

        private void LoadDGLSPhieuNhap_Demo()
        {
            var dt = new DataTable();
            dt.Columns.Add("Mã Phiếu");        // cột hiển thị 2 dòng
            dt.Columns.Add("Trạng Thái");   // cột badge

            // giống ảnh bạn
            dt.Rows.Add("PN001 - Nhập kho\nNH01 - 10/12/2025", "Hoàn tất");
            dt.Rows.Add("PX002 - Xuất kho\nKS01 - 11/12/2025", "Nháp");
            dt.Rows.Add("PN010 - Nhập kho\nNH01 - 12/12/2025", "Hoàn tất");
            DGLSPhieuNhap.AutoGenerateColumns = true;   // tự sinh cột theo DataTable
            DGLSPhieuNhap.AllowUserToAddRows = false;
            DGLSPhieuNhap.RowHeadersVisible = false;


            DGLSPhieuNhap.DataSource = dt;
        }



    }

}
