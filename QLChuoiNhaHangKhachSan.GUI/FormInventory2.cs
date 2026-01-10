using Guna.UI2.WinForms;
using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QLChuoiNhaHangKhachSan.BLL;
using QLChuoiNhaHangKhachSan.DTO;

namespace QLChuoiNhaHangKhachSan.GUI
{
    public partial class FormInventory2 : Form
    {
        private readonly WarehouseVoucherBLL _voucherBll;

        public FormInventory2()
        {
            InitializeComponent();
            _voucherBll = new WarehouseVoucherBLL(ConfigurationManager.ConnectionStrings["RHGROUP"].ConnectionString);
          
            btnNhapKhoInventory2.Click += btnNhapKhoInventory2_Click;
        }

        private void FormInventory2_Load(object sender, EventArgs e)
        {
            DGdgvPhieu.RowTemplate.Height = 60;
            DGdgvPhieu.ColumnHeadersHeight = 48;
            DGLSPhieuNhap.RowTemplate.Height = 60;
            DGLSPhieuNhap.ColumnHeadersHeight = 48;
            lblTitle2.Text = "Quản lý Phiếu Nhập – Xuất Kho";
            lblSubtitle2.Text = "Tạo và quản lý phiếu nhập – xuất kho, đồng bộ báo cáo kế toán";

            LoadVouchersFromDb();
            DGLSPhieuNhap.DataSource = null; // không dùng demo

            RelayoutHeaderButtons();
        }

        private void RelayoutHeaderButtons()
        {
            pnlTopActions.Visible = false;
          

            btnNhapKhoInventory2.Parent = pntaophieumau;
            btnXuatKhoInventory2.Parent = pntaophieumau;

            btnNhapKhoInventory2.Size = new Size(118, 45);
            btnNhapKhoInventory2.Location = new Point(0, 3);
            btnNhapKhoInventory2.FillColor = Color.FromArgb(22, 163, 74);
            btnNhapKhoInventory2.BorderColor = Color.FromArgb(22, 163, 74);
            btnNhapKhoInventory2.ForeColor = Color.White;
            btnNhapKhoInventory2.Text = "+ Nhập kho";

            btnXuatKhoInventory2.Size = new Size(118, 45);
            btnXuatKhoInventory2.Location = new Point(btnNhapKhoInventory2.Right + 8, 3);
            btnXuatKhoInventory2.FillColor = Color.FromArgb(239, 68, 68);
            btnXuatKhoInventory2.BorderColor = Color.FromArgb(239, 68, 68);
            btnXuatKhoInventory2.ForeColor = Color.White;
            btnXuatKhoInventory2.Text = "- Xuất kho";

            pntaophieumau.Size = new Size(btnXuatKhoInventory2.Right, pntaophieumau.Height);
            pntaophieumau.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pntaophieumau.Location = new Point(pnbottom1.Width - pntaophieumau.Width - 16, pntaophieumau.Location.Y);
        }

        private void lblTitle_Click(object sender, EventArgs e)
        {

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
            using (var f = new FormImportWarehouse())
            {
                f.StartPosition = FormStartPosition.CenterParent;
                if (f.ShowDialog(this) == DialogResult.OK)
                {
                    LoadVouchersFromDb();
                }
            }
        }

        private void btnXuatKhoInventory2_Click(object sender, EventArgs e)
        {
            using (var f = new FormExportWarehouse())
            {
                f.StartPosition = FormStartPosition.CenterParent;
                if (f.ShowDialog(this) == DialogResult.OK)
                {
                    LoadVouchersFromDb();
                }
            }
        }

        private void btnTaoPhieuInventory2_Click(object sender, EventArgs e)
        {
           MessageBox.Show("OK - Tạo phiếu");
        }


        private void TbDanhsachtonkho_Paint(object sender, PaintEventArgs e)
        {

        }

        private void LoadVouchersFromDb()
        {
            try
            {
                var data = _voucherBll.GetVouchersList()
                    .Select(v => new
                    {
                        MaPhieu = v.VoucherCode,
                        NgayLapPhieu = v.CreatedAt.ToString("dd/MM/yyyy"),
                        LoaiKho = MapWarehouseType(v.WarehouseType),
                        DonViQuanLy = v.UnitCode,
                        TrangThaiPhieu = v.Status
                    })
                    .ToList();

                EnsureVoucherGridColumns();

                DGdgvPhieu.AllowUserToAddRows = false;
                DGdgvPhieu.RowHeadersVisible = false;
                DGdgvPhieu.AutoGenerateColumns = false;
                DGdgvPhieu.DataSource = data;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Tải danh sách phiếu thất bại: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EnsureVoucherGridColumns()
        {
            if (DGdgvPhieu.Columns.Count > 0) return;

            DGdgvPhieu.Columns.Clear();

            DGdgvPhieu.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "MaPhieu",
                HeaderText = "Mã phiếu",
                Name = "colMaPhieu",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 18
            });

            DGdgvPhieu.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "NgayLapPhieu",
                HeaderText = "Ngày lập phiếu",
                Name = "colNgayLap",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 18
            });

            DGdgvPhieu.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "LoaiKho",
                HeaderText = "Loại kho",
                Name = "colLoaiKho",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 18
            });

            DGdgvPhieu.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "DonViQuanLy",
                HeaderText = "Đơn vị quản lý (NH/KS)",
                Name = "colDonVi",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 22
            });

            DGdgvPhieu.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TrangThaiPhieu",
                HeaderText = "Trạng thái phiếu",
                Name = "colTrangThai",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 24
            });
        }

        private string MapWarehouseType(string warehouseType)
        {
            if (string.Equals(warehouseType, "INGREDIENT", StringComparison.OrdinalIgnoreCase)) return "Nguyên liệu";
            if (string.Equals(warehouseType, "EQUIPMENT", StringComparison.OrdinalIgnoreCase)) return "Thiết bị";
            return warehouseType;
        }



    }

}
