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
    public partial class FormInventory : Form
    {



        public FormInventory()
        {
            InitializeComponent();
            this.Load += FormInventory_Load;
            // Wire update button to open update form
            this.btnupdateitems.Click += new EventHandler(this.btnupdateitems_Click);

            // Ensure when user clicks the close (X) button we notify the host dashboard
            this.FormClosed += FormInventory_FormClosed;
        }

        private void FormInventory_FormClosed(object sender, FormClosedEventArgs e)
        {
            // If this form is hosted inside the dashboard, notify it so it can hide the child container
            try
            {
                var host = this.FindForm() as FormDashBoard;
                if (host != null)
                {
                    host.NotifyChildClosed();
                    return;
                }

                // If ParentForm returns dashboard (safer fallback)
                host = this.ParentForm as FormDashBoard;
                if (host != null)
                {
                    host.NotifyChildClosed();
                }
            }
            catch { }
        }

        private void btnupdateitems_Click(object sender, EventArgs e)
        {
            ShowOverlayOnLbDSTonkho();

            using (var f = new FormUpdateItems())
            {
                f.StartPosition = FormStartPosition.CenterParent;
                f.ShowDialog(this);
            }

            HideOverlay();
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

            DGdanhsachtonkho.RowTemplate.Height = 60;
            DGdanhsachtonkho.ColumnHeadersHeight = 48;
            DGCanhbaotonkho.RowTemplate.Height = 44;
            DGCanhbaotonkho.ColumnHeadersHeight = 48;
            LoadFakeTonKho();
            LoadFakeCanhBao();

            // Ensure stop button event wired (fallback)
            try
            {
                this.btnstopItems.Click -= this.btnstopItems_Click;
            }
            catch { }
            this.btnstopItems.Click += new EventHandler(this.btnstopItems_Click);

            // optional double-click handler can be added in designer if desired
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
        private void LoadFakeTonKho()
        {
            // 1) Tạo bảng dữ liệu fake
            DataTable dt = new DataTable();
            dt.Columns.Add("Mã");
            dt.Columns.Add("Tên");
            dt.Columns.Add("Đơn vị");
            dt.Columns.Add("Tồn", typeof(int));
            dt.Columns.Add("Giá Nhập", typeof(decimal));
            dt.Columns.Add("Ngày Nhập", typeof(DateTime));
            dt.Columns.Add("NH/KS");
            dt.Columns.Add("Trạng Thái");

            // 2) Thêm 3 dòng fake giống ảnh web
            dt.Rows.Add("NL001", "Hạt cà phê", "Kg", 8, 120000, new DateTime(2025, 12, 12), "NH01", "Sắp hết");
            dt.Rows.Add("TB011", "Máy hút bụi", "Housekeeping", 3, 1200000, new DateTime(2025, 12, 10), "KS01", "Thiếu");
            dt.Rows.Add("NL002", "Đường", "Kg", 55, 18000, new DateTime(2025, 12, 9), "NH01", "Ổn định");

            // 3) Bind vào Guna2DataGridView
            DGdanhsachtonkho.AutoGenerateColumns = true; // cho nó tự sinh cột theo DataTable
            DGdanhsachtonkho.DataSource = dt;
            //
           

            // 4) Format cơ bản cho giống web
            DGdanhsachtonkho.Columns["Giá Nhập"].DefaultCellStyle.Format = "N0";
            DGdanhsachtonkho.Columns["Ngày Nhập"].DefaultCellStyle.Format = "dd/MM/yyyy";
            DGdanhsachtonkho.Columns["Tồn"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DGdanhsachtonkho.Columns["Trạng Thái"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // (tuỳ chọn) chỉnh chiều cao dòng cho đẹp
            
            DGdanhsachtonkho.DefaultCellStyle.SelectionBackColor = Color.White;
            DGdanhsachtonkho.DefaultCellStyle.SelectionForeColor = Color.Black;
        }

        private void TbDanhsachtonkho_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2DataGridView4_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void lbdanhsachtonkho_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {

        }

        private void LoadFakeCanhBao()
        {
            DataTable dt = new DataTable();

            // 2 cột thôi: trái (2 dòng text), phải (badge số)
            dt.Columns.Add("Mặt hàng");          // "NL001 · Cà phê hạt\nNH01 · Ngưỡng: 10"
            dt.Columns.Add("Lượng Tồn", typeof(int));

            dt.Rows.Add("NL001 · Hạt cà phê \nNH01 · Ngưỡng: 10", 8);
            dt.Rows.Add("TB011 · Máy hút bụi\nKS01 · Ngưỡng: 5", 3);
            dt.Rows.Add("NL009 · Sữa tươi\nNH01 · Ngưỡng: 20", 12);

            DGCanhbaotonkho.AutoGenerateColumns = true;
            DGCanhbaotonkho.DataSource = dt;



            // Chỉ còn 2 cột, set tỷ lệ giống UI
           // DGCanhbaotonkho.Columns["Info"].FillWeight = 85;
            //DGCanhbaotonkho.Columns["Badge"].FillWeight = 15;

            // Format cột Info: cho xuống dòng
            DGCanhbaotonkho.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            DGCanhbaotonkho.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            DGCanhbaotonkho.RowTemplate.MinimumHeight = 52;

            // Canh badge giữa
            //DGCanhbaotonkho.Columns["Badge"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Font đẹp hơn
            DGCanhbaotonkho.DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            DGCanhbaotonkho.DefaultCellStyle.SelectionBackColor = Color.White;
            DGCanhbaotonkho.DefaultCellStyle.SelectionForeColor = Color.Black;

        }

        private void guna2Panel3_Paint(object sender, PaintEventArgs e)
        {

        }
        private Panel overlay;

        private void ShowOverlayOnLbDSTonkho()
        {
            if (overlay != null) return;

            overlay = new Panel();
            overlay.Dock = DockStyle.Fill;
            overlay.BackColor = Color.FromArgb(60, 17, 24, 39);

            lbDSTonkho.Controls.Add(overlay);
            overlay.BringToFront();
        }

        private void HideOverlay()
        {
            if (overlay == null) return;

            lbDSTonkho.Controls.Remove(overlay);
            overlay.Dispose();
            overlay = null;
        }



        private void btnadditems_Click(object sender, EventArgs e)
        {
            ShowOverlayOnLbDSTonkho();

            using (var f = new FormAddMatHang())
            {
                f.StartPosition = FormStartPosition.CenterParent;
                f.ShowDialog(this);
            }

            HideOverlay();
        }

        private void btnstopItems_Click(object sender, EventArgs e)
        {
            ShowOverlayOnLbDSTonkho();

            string selectedCode = null;
            try
            {
                if (DGdanhsachtonkho.CurrentRow != null && DGdanhsachtonkho.CurrentRow.Cells.Count > 0)
                {
                    selectedCode = DGdanhsachtonkho.CurrentRow.Cells[0].Value?.ToString();
                }
            }
            catch { }

            FormStopItems f = null;
            if (!string.IsNullOrEmpty(selectedCode)) f = new FormStopItems(selectedCode);
            else f = new FormStopItems();

            using (f)
            {
                f.StartPosition = FormStartPosition.CenterParent;
                f.ShowDialog(this);
            }

            HideOverlay();
        }
    }



}
