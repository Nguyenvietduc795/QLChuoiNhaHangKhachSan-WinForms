using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using QLChuoiNhaHangKhachSan.BLL;
using QLChuoiNhaHangKhachSan.DTO;

namespace QLChuoiNhaHangKhachSan.GUI
{
    public partial class FormInventory2 : Form
    {
        private readonly WarehouseVoucherBLL _voucherBll;
        private readonly StockBLL _stockBll;
        private List<WarehouseVoucherDTO> _voucherCache;
        private bool _filtersInitialized;
        private bool _suppressFilterEvents;

        public FormInventory2()
        {
            InitializeComponent();
            var connStr = ConfigurationManager.ConnectionStrings["RHGROUP"].ConnectionString;
            _voucherBll = new WarehouseVoucherBLL(connStr);
            _stockBll = new StockBLL(connStr);
        }

        private void FormInventory2_Load(object sender, EventArgs e)
        {
            DGdgvPhieu.RowTemplate.Height = 60;
            DGdgvPhieu.ColumnHeadersHeight = 48;
            DGLSPhieuNhap.RowTemplate.Height = 60;
            DGLSPhieuNhap.ColumnHeadersHeight = 48;
            lblTitle2.Text = "Quản lý Phiếu Nhập – Xuất Kho";
            lblSubtitle2.Text = "Tạo và quản lý phiếu nhập – xuất kho, đồng bộ báo cáo kế toán";

            InitializeFilterControls();
            LoadVouchersFromDb();
            DGLSPhieuNhap.DataSource = null; // không dùng demo

            RelayoutHeaderButtons();
            RefreshDashboardKpis();
        }

        private void RelayoutHeaderButtons()
        {
            // hiển thị lại nhóm nút hành động
            pnlTopActions.Visible = true;

            // giữ nguyên layout mặc định trong Designer, chỉ chuẩn hóa màu sắc
            btnNhapKhoInventory2.FillColor = Color.FromArgb(22, 163, 74);
            btnNhapKhoInventory2.BorderColor = Color.FromArgb(22, 163, 74);
            btnNhapKhoInventory2.ForeColor = Color.White;
            btnNhapKhoInventory2.Text = "+ Nhập kho";

            btnXuatKhoInventory2.FillColor = Color.FromArgb(239, 68, 68);
            btnXuatKhoInventory2.BorderColor = Color.FromArgb(239, 68, 68);
            btnXuatKhoInventory2.ForeColor = Color.White;
            btnXuatKhoInventory2.Text = "- Xuất kho";

            btnSuaPhieu.FillColor = Color.White;
            btnSuaPhieu.BorderColor = Color.FromArgb(203, 213, 225);
            btnSuaPhieu.ForeColor = Color.FromArgb(15, 23, 42);

            btnviewPhieu.FillColor = Color.FromArgb(251, 191, 36); // vàng đồng bộ với palette nóng
            btnviewPhieu.BorderColor = Color.FromArgb(245, 158, 11);
            btnviewPhieu.ForeColor = Color.FromArgb(55, 29, 6);
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
                _voucherCache = _voucherBll.GetVouchersList();

                EnsureVoucherGridColumns(); 
                RefreshUnitComboOptions();
                ApplyFilters();
                RefreshDashboardKpis();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Tải danh sách phiếu thất bại: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshDashboardKpis()
        {
            try
            {
                var total = _stockBll.GetTotalStockQuantity();
                var low = _stockBll.GetLowStockCount();
                var stable = _stockBll.GetStableStockCount();

                lblTotal2.Text = total.ToString("N0", CultureInfo.InvariantCulture);
                lbStockLow2.Text = low.ToString("N0", CultureInfo.InvariantCulture);
                lbHealthyStock2.Text = stable.ToString("N0", CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                lblTotal2.Text = "0";
                lbStockLow2.Text = "0";
                lbHealthyStock2.Text = "0";
                MessageBox.Show("Không thể tải KPI tồn kho: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EnsureVoucherGridColumns()
        {
            if (DGdgvPhieu.Columns.Count > 0) return;

            DGdgvPhieu.Columns.Clear();

            DGdgvPhieu.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "VoucherID",
                HeaderText = "VoucherID",
                Name = "colVoucherId",
                Visible = false
            });

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

            // Ẩn loại phiếu để xác định mở form sửa/view
            DGdgvPhieu.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "VoucherType",
                HeaderText = "VoucherType",
                Name = "colVoucherType",
                Visible = false
            });
        }

        private int? GetSelectedVoucherId()
        {
            var row = DGdgvPhieu.CurrentRow;
            if (row == null) return null;
            var cell = row.Cells["colVoucherId"];
            if (cell == null) return null;
            var value = cell.Value;
            if (value == null || value == DBNull.Value) return null;
            return Convert.ToInt32(value);
        }

        private WarehouseVoucherDTO GetSelectedVoucher()
        {
            var id = GetSelectedVoucherId();
            if (!id.HasValue || _voucherCache == null) return null;
            return _voucherCache.FirstOrDefault(v => v.VoucherID == id.Value);
        }

        private string GetSelectedVoucherCode()
        {
            var row = DGdgvPhieu.CurrentRow;
            if (row == null) return null;
            var cell = row.Cells["colMaPhieu"];
            return cell?.Value as string;
        }

        private void ShowVoucherDetails(string voucherCode)
        {
            if (string.IsNullOrWhiteSpace(voucherCode)) return;

            try
            {
                var dt = _voucherBll.GetDetailsByCode(voucherCode);
                if (dt == null || dt.Rows.Count == 0)
                {
                    MessageBox.Show($"Không tìm thấy chi tiết cho phiếu {voucherCode}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DGLSPhieuNhap.DataSource = null;
                    return;
                }

                NormalizeDetailNames(dt);

                DGLSPhieuNhap.AutoGenerateColumns = true;
                DGLSPhieuNhap.DataSource = dt;
                lbPhieunhapganday.Text = $"Chi tiết phiếu {voucherCode}";
                FormatDetailGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không tải được chi tiết phiếu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormatDetailGrid()
        {
            var grid = DGLSPhieuNhap;
            if (grid.Columns.Count == 0) return;

            void HideColumn(string name)
            {
                if (grid.Columns.Contains(name)) grid.Columns[name].Visible = false;
            }

            HideColumn("VoucherDetailID");
            HideColumn("VoucherID");
            HideColumn("IngredientID");
            HideColumn("EquipmentID");

            if (grid.Columns.Contains("ItemCode"))
            {
                var col = grid.Columns["ItemCode"];
                col.HeaderText = "Mã hàng";
                col.DisplayIndex = 0;
            }
            if (grid.Columns.Contains("ItemName"))
            {
                var col = grid.Columns["ItemName"];
                col.HeaderText = "Tên hàng";
                col.DisplayIndex = 1;
            }
            if (grid.Columns.Contains("Unit")) grid.Columns["Unit"].HeaderText = "Đơn vị";
            if (grid.Columns.Contains("Quantity")) grid.Columns["Quantity"].HeaderText = "Số lượng";
            if (grid.Columns.Contains("UnitPrice"))
            {
                grid.Columns["UnitPrice"].HeaderText = "Đơn giá";
                grid.Columns["UnitPrice"].DefaultCellStyle.Format = "N0";
            }
            if (grid.Columns.Contains("LineTotal"))
            {
                grid.Columns["LineTotal"].HeaderText = "Thành tiền";
                grid.Columns["LineTotal"].DefaultCellStyle.Format = "N0";
            }
        }

        private void NormalizeDetailNames(DataTable dt)
        {
            if (!dt.Columns.Contains("ItemName")) return;

            var codeColumnExists = dt.Columns.Contains("ItemCode");
            foreach (DataRow row in dt.Rows)
            {
                var code = codeColumnExists ? row["ItemCode"]?.ToString() : null;
                var name = row["ItemName"]?.ToString();
                row["ItemName"] = FormatItemName(code, name);
            }
        }

        private string FormatItemName(string code, string rawName)
        {
            if (string.IsNullOrWhiteSpace(rawName)) return string.Empty;
            var cleaned = rawName.Trim();
            if (string.IsNullOrWhiteSpace(code)) return cleaned;

            var parts = cleaned.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries)
                               .Select(p => p.Trim())
                               .Where(p => !string.IsNullOrWhiteSpace(p))
                               .ToList();

            if (parts.Count > 1)
            {
                var filtered = parts.Where(p => !string.Equals(p, code, StringComparison.OrdinalIgnoreCase)).ToList();
                if (filtered.Count > 0)
                {
                    return string.Join(" - ", filtered);
                }
            }

            if (cleaned.StartsWith(code, StringComparison.OrdinalIgnoreCase))
            {
                var withoutCode = cleaned.Substring(code.Length).TrimStart(' ', '-', '_');
                if (!string.IsNullOrWhiteSpace(withoutCode))
                    return withoutCode;
            }

            return cleaned;
        }

        private string MapWarehouseType(string warehouseType)
        {
            if (string.Equals(warehouseType, "INGREDIENT", StringComparison.OrdinalIgnoreCase)) return "Nguyên liệu";
            if (string.Equals(warehouseType, "EQUIPMENT", StringComparison.OrdinalIgnoreCase)) return "Thiết bị";
            return warehouseType;
        }

        private void btnSuaPhieuInventory2_Click(object sender, EventArgs e)
        {
            var voucher = GetSelectedVoucher();
            if (voucher == null)
            {
                MessageBox.Show("Vui lòng chọn một phiếu cần sửa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!string.Equals(voucher.Status, "Nháp", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Chỉ có thể chỉnh sửa phiếu đang ở trạng thái Nháp", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var voucherType = voucher.VoucherType;
            var voucherId = voucher.VoucherID;

            if (voucherType.Equals("IMPORT", StringComparison.OrdinalIgnoreCase))
            {
                using (var f = new FormImportWarehouse(voucher))
                {
                    f.StartPosition = FormStartPosition.CenterParent;
                    if (f.ShowDialog(this) == DialogResult.OK)
                    {
                        LoadVouchersFromDb();
                    }
                }
            }
            else if (voucherType.Equals("EXPORT", StringComparison.OrdinalIgnoreCase))
            {
                using (var f = new FormExportWarehouse(voucher))
                {
                    f.StartPosition = FormStartPosition.CenterParent;
                    if (f.ShowDialog(this) == DialogResult.OK)
                    {
                        LoadVouchersFromDb();
                    }
                }
            }
            else
            {
                MessageBox.Show("Không xác định được loại phiếu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnviewPhieuInventory2_Click(object sender, EventArgs e)
        {
            var voucherCode = GetSelectedVoucherCode();
            if (string.IsNullOrWhiteSpace(voucherCode))
            {
                MessageBox.Show("Vui lòng chọn một phiếu để xem", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            ShowVoucherDetails(voucherCode);
        }

        private void lbPhieunhapganday_Click(object sender, EventArgs e)
        {

        }

        private void InitializeFilterControls()
        {
            if (_filtersInitialized) return;

            var warehouseOptions = new List<FilterOption>
            {
                new FilterOption { Text = "Tất cả", Value = null },
                new FilterOption { Text = "Kho Thiết Bị", Value = "EQUIPMENT" },
                new FilterOption { Text = "Kho Nguyên Liệu", Value = "INGREDIENT" }
            };

            cboLoaiKho2.DisplayMember = "Text";
            cboLoaiKho2.ValueMember = "Value";
            cboLoaiKho2.DataSource = warehouseOptions;
            cboLoaiKho2.SelectedIndex = 0;

            cboDonVi2.Items.Clear();
            cboDonVi2.Items.Add("Tất cả");
            cboDonVi2.SelectedIndex = 0;

            cboLoaiKho2.SelectedIndexChanged += FilterControlChanged;
            cboDonVi2.SelectedIndexChanged += FilterControlChanged;
            txTimkiem2.TextChanged += TxTimkiem2_TextChanged;
            btnLoc2.Click += BtnLoc2_Click;
            btnReset2.Click += BtnReset2_Click;

            _filtersInitialized = true;
        }

        private void RefreshUnitComboOptions()
        {
            if (!_filtersInitialized) return;

            var selectedUnit = cboDonVi2.SelectedItem as string;
            var unitOptions = (_voucherCache ?? new List<WarehouseVoucherDTO>())
                .Select(v => v.UnitCode)
                .Where(u => !string.IsNullOrWhiteSpace(u))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(u => u)
                .ToList();

            unitOptions.Insert(0, "Tất cả");

            _suppressFilterEvents = true;
            cboDonVi2.DataSource = unitOptions;

            if (!string.IsNullOrEmpty(selectedUnit))
            {
                var index = unitOptions.FindIndex(u => string.Equals(u, selectedUnit, StringComparison.OrdinalIgnoreCase));
                cboDonVi2.SelectedIndex = index >= 0 ? index : 0;
            }
            else
            {
                cboDonVi2.SelectedIndex = 0;
            }

            _suppressFilterEvents = false;
        }

        private void ApplyFilters()
        {
            var source = _voucherCache ?? new List<WarehouseVoucherDTO>();
            IEnumerable<WarehouseVoucherDTO> filtered = source;

            if (_filtersInitialized)
            {
                var warehouseValue = cboLoaiKho2.SelectedValue as string;
                if (!string.IsNullOrEmpty(warehouseValue))
                {
                    filtered = filtered.Where(v => string.Equals(v.WarehouseType, warehouseValue, StringComparison.OrdinalIgnoreCase));
                }

                var unitValue = cboDonVi2.SelectedItem as string;
                if (!string.IsNullOrEmpty(unitValue) && !string.Equals(unitValue, "Tất cả", StringComparison.OrdinalIgnoreCase))
                {
                    filtered = filtered.Where(v => string.Equals(v.UnitCode, unitValue, StringComparison.OrdinalIgnoreCase));
                }

                var keyword = txTimkiem2.Text?.Trim();
                if (!string.IsNullOrEmpty(keyword))
                {
                    filtered = filtered.Where(v =>
                        (!string.IsNullOrEmpty(v.VoucherCode) && v.VoucherCode.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0) ||
                        (!string.IsNullOrEmpty(v.UnitCode) && v.UnitCode.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0) ||
                        (!string.IsNullOrEmpty(v.Note) && v.Note.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0));
                }
            }

            var data = filtered
                .Select(v => new
                {
                    VoucherID = v.VoucherID,
                    MaPhieu = v.VoucherCode,
                    NgayLapPhieu = v.CreatedAt.ToString("dd/MM/yyyy"),
                    LoaiKho = MapWarehouseType(v.WarehouseType),
                    DonViQuanLy = v.UnitCode,
                    TrangThaiPhieu = v.Status,
                    VoucherType = v.VoucherType
                })
                .ToList();

            DGdgvPhieu.AllowUserToAddRows = false;
            DGdgvPhieu.RowHeadersVisible = false;
            DGdgvPhieu.AutoGenerateColumns = false;
            DGdgvPhieu.DataSource = data;
        }

        private void ResetFilters()
        {
            if (!_filtersInitialized) return;

            _suppressFilterEvents = true;
            cboLoaiKho2.SelectedIndex = 0;
            cboDonVi2.SelectedIndex = 0;
            txTimkiem2.Clear();
            _suppressFilterEvents = false;

            ApplyFilters();
        }

        private void FilterControlChanged(object sender, EventArgs e)
        {
            if (_suppressFilterEvents) return;
            ApplyFilters();
        }

        private void TxTimkiem2_TextChanged(object sender, EventArgs e)
        {
            if (_suppressFilterEvents) return;
            ApplyFilters();
        }

        private void BtnLoc2_Click(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void BtnReset2_Click(object sender, EventArgs e)
        {
            ResetFilters();
        }

        private class FilterOption
        {
            public string Text { get; set; }
            public string Value { get; set; }
        }
    }

}
