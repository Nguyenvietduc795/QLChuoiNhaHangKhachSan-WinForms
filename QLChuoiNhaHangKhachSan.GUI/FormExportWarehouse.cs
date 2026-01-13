using QLChuoiNhaHangKhachSan.BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QLChuoiNhaHangKhachSan.DTO;

namespace QLChuoiNhaHangKhachSan.GUI
{
    public partial class FormExportWarehouse : Form
    {
        private BindingList<SelectItemDTO> _items;
        private readonly UnitBLL _unitBll;
        private readonly WarehouseVoucherBLL _voucherBll;
        private readonly IngredientBLL _ingredientBll;
        private readonly EquipmentBLL _equipmentBll;
        private WarehouseVoucherDTO _editingVoucher;
        private bool IsEditMode => _editingVoucher != null;

        public FormExportWarehouse(WarehouseVoucherDTO editingVoucher = null)
        {
            InitializeComponent();
            this.Load += FormExportWarehouse_Load;

            var connStr = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            _unitBll = new UnitBLL(connStr);
            _voucherBll = new WarehouseVoucherBLL(connStr);
            _ingredientBll = new IngredientBLL(connStr);
            _equipmentBll = new EquipmentBLL(connStr);
            _editingVoucher = editingVoucher;
            bnupdaterowexportKho.Click += bnupdaterowexportKho_Click;
            bndeleterowexportkho.Click += bndeleterowexportkho_Click;
            btnCancelExportKho.Click += btnCancelExportKho_Click;
            bnCreateExportPhieu.Click += bnCreateExportPhieu_Click;
            cbtypekhoexport.SelectedIndexChanged += Cbtypekhoexport_SelectedIndexChanged;
        }

        private void FormExportWarehouse_Load(object sender, EventArgs e)
        {
            InitCombos();
            InitGrid();
            txtMaPhieuexport.ReadOnly = true;
            txtMaPhieuexport.TabStop = false;
            if (IsEditMode)
            {
                LoadVoucherDraftForEdit();
            }
            else
            {
                try
                {
                    txtMaPhieuexport.Text = _voucherBll.GetNextVoucherCode(false);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Không lấy được mã phiếu tiếp theo: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void InitCombos()
        {
            cbStatusExport.Items.Clear();
            cbStatusExport.Items.AddRange(new object[] { "Nháp", "Hoàn thành" });
            if (cbStatusExport.Items.Count > 0) cbStatusExport.SelectedIndex = 0;

            cbtypekhoexport.DisplayMember = "Text";
            cbtypekhoexport.ValueMember = "Value";
            cbtypekhoexport.DataSource = new[]
            {
                new { Text = "Kho Thiết Bị",   Value = WarehouseType.Equipment },
                new { Text = "Kho Nguyên Liệu", Value = WarehouseType.Ingredient }
            };
            if (cbtypekhoexport.Items.Count > 0) cbtypekhoexport.SelectedIndex = 0;

            LoadDonViFromDb();
            cbdonviexportkho.SelectedIndexChanged += Cbdonviexportkho_SelectedIndexChanged;
        }

        private void LoadDonViFromDb()
        {
            try
            {
                int areaId = 1; // Branch 1
                var type = cbtypekhoexport.SelectedValue is WarehouseType wt ? wt : WarehouseType.Ingredient;
                string unitType = type == WarehouseType.Ingredient ? "RESTAURANT" : "HOTEL";
                var dt = _unitBll.GetUnitsByAreaAndType(areaId, unitType);

                DataTable filtered = null;
                if (dt != null && dt.Rows.Count > 0)
                {
                    var rows = dt.AsEnumerable().Where(r => r.Field<string>("UnitType") == unitType);
                    if (rows.Any()) filtered = rows.CopyToDataTable();
                }

                cbdonviexportkho.DataSource = null;
                if (filtered != null && filtered.Rows.Count > 0)
                {
                    cbdonviexportkho.DisplayMember = "UnitCode";
                    cbdonviexportkho.ValueMember = "UnitCode";
                    cbdonviexportkho.DataSource = filtered;
                    if (cbdonviexportkho.Items.Count > 0) cbdonviexportkho.SelectedIndex = 0;
                }
                else
                {
                    cbdonviexportkho.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Load đơn vị thất bại: " + ex.Message,
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitGrid()
        {
            _items = new BindingList<SelectItemDTO>();
            DGVexportkho.AutoGenerateColumns = true;
            DGVexportkho.DataSource = _items;
            DGVexportkho.AllowUserToAddRows = false;
            DGVexportkho.RowHeadersVisible = false;
            DGVexportkho.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DGVexportkho.MultiSelect = false;
            DGVexportkho.DefaultCellStyle.SelectionBackColor = Color.White;
            DGVexportkho.DefaultCellStyle.SelectionForeColor = Color.Black;
            DGVexportkho.ReadOnly = true;
            HideGridColumns();
            LocalizeGridHeaders();
        }

        private void LoadVoucherDraftForEdit()
        {
            var latest = _voucherBll.GetVoucherById(_editingVoucher.VoucherID);
            if (latest == null)
            {
                MessageBox.Show("Không tìm thấy dữ liệu phiếu cần sửa.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.Cancel;
                Close();
                return;
            }

            _editingVoucher = latest;
            txtMaPhieuexport.Text = latest.VoucherCode;

            var type = string.Equals(latest.WarehouseType, "EQUIPMENT", StringComparison.OrdinalIgnoreCase)
                ? WarehouseType.Equipment
                : WarehouseType.Ingredient;
            var isIngredientWarehouse = type == WarehouseType.Ingredient;

            cbtypekhoexport.SelectedValue = type;
            cbtypekhoexport.Enabled = false;
            LoadDonViFromDb();
            if (!string.IsNullOrWhiteSpace(latest.UnitCode))
            {
                try
                {
                    cbdonviexportkho.SelectedValue = latest.UnitCode;
                }
                catch
                {
                }
            }

            cbStatusExport.SelectedItem = string.IsNullOrWhiteSpace(latest.Status) ? "Nháp" : latest.Status;
            bnCreateExportPhieu.Text = "Lưu phiếu";

            var detailDtos = _voucherBll.GetVoucherDetails(latest.VoucherID) ?? new List<WarehouseVoucherDetailDTO>();
            _items.Clear();
            foreach (var detail in detailDtos)
            {
                var item = new SelectItemDTO
                {
                    IngredientID = detail.IngredientID ?? 0,
                    IngredientCode = detail.IngredientID.HasValue ? detail.ItemCode : null,
                    IngredientName = detail.IngredientID.HasValue ? detail.ItemName : null,
                    ItemID = detail.EquipmentID ?? 0,
                    ItemCode = detail.EquipmentID.HasValue ? detail.ItemCode : null,
                    ItemName = detail.EquipmentID.HasValue ? detail.ItemName : null,
                    Unit = detail.Unit,
                    Quantity = detail.Quantity,
                    UnitPrice = detail.UnitPrice
                };
                PopulateStockSnapshot(item, isIngredientWarehouse);
                _items.Add(item);
            }

            DGVexportkho.Refresh();
        }

        private void Cbtypekhoexport_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDonViFromDb();
            _items?.Clear();
        }

        private void Cbdonviexportkho_SelectedIndexChanged(object sender, EventArgs e)
        {
            var type = cbtypekhoexport.SelectedValue is WarehouseType wt ? wt : WarehouseType.Ingredient;
            var code = cbdonviexportkho.SelectedValue as string;
            if (string.IsNullOrEmpty(code)) return;

            bool ok = (type == WarehouseType.Ingredient && code.StartsWith("NH")) ||
                      (type == WarehouseType.Equipment && code.StartsWith("KS"));
            if (!ok)
            {
                MessageBox.Show("Vui lòng chọn lại đơn vị đúng với loại kho", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                for (int i = 0; i < cbdonviexportkho.Items.Count; i++)
                {
                    var drv = cbdonviexportkho.Items[i] as DataRowView;
                    var c = drv?["UnitCode"] as string;
                    if (!string.IsNullOrEmpty(c) && ((type == WarehouseType.Ingredient && c.StartsWith("NH")) || (type == WarehouseType.Equipment && c.StartsWith("KS"))))
                    {
                        cbdonviexportkho.SelectedIndex = i;
                        return;
                    }
                }
            }
        }

        private void bnaddrowexportKho_Click(object sender, EventArgs e)
        {
            var selectedType = cbtypekhoexport.SelectedValue is WarehouseType wt ? wt : WarehouseType.Ingredient;
            if (cbdonviexportkho.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn đơn vị trước khi thêm dòng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            using (var f = new FormSelectItems(SelectMode.Export, selectedType))
            {
                f.StartPosition = FormStartPosition.CenterParent;
                if (f.ShowDialog(this) == DialogResult.OK && f.SelectedItems != null)
                {
                    foreach (var item in f.SelectedItems) AddOrMergeItem(item);
                }
            }
        }

        private void bnupdaterowexportKho_Click(object sender, EventArgs e)
        {
            var selectedType = cbtypekhoexport.SelectedValue is WarehouseType wt ? wt : WarehouseType.Ingredient;
            if (_items == null || DGVexportkho.CurrentRow == null) return;

            var oldItem = DGVexportkho.CurrentRow.DataBoundItem as SelectItemDTO;
            if (oldItem == null) return;

            using (var f = new FormSelectItems(oldItem, selectedType))
            {
                f.StartPosition = FormStartPosition.CenterParent;
                if (f.ShowDialog(this) == DialogResult.OK && f.SelectedItems != null && f.SelectedItems.Count > 0)
                {
                    var newItem = f.SelectedItems[0];
                    oldItem.ItemID = newItem.ItemID;
                    oldItem.ItemCode = newItem.ItemCode;
                    oldItem.ItemName = newItem.ItemName;
                    oldItem.IngredientID = newItem.IngredientID;
                    oldItem.IngredientCode = newItem.IngredientCode;
                    oldItem.IngredientName = newItem.IngredientName;
                    oldItem.Unit = newItem.Unit;
                    oldItem.StockQuantity = newItem.StockQuantity;
                    oldItem.Quantity = newItem.Quantity;
                    oldItem.UnitPrice = newItem.UnitPrice;
                    DGVexportkho.Refresh();
                }
            }
        }

        private void bndeleterowexportkho_Click(object sender, EventArgs e)
        {
            if (_items == null || DGVexportkho.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn dòng cần xóa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var row = DGVexportkho.SelectedRows[0];
            var item = row.DataBoundItem as SelectItemDTO;
            if (item == null) return;
            var confirm = MessageBox.Show($"Bạn có chắc muốn xóa mặt hàng {item.ItemName ?? item.IngredientName}?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                _items.Remove(item);
            }
        }

        private void btnCancelExportKho_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bnCreateExportPhieu_Click(object sender, EventArgs e)
        {
            var selectedType = cbtypekhoexport.SelectedValue is WarehouseType wt ? wt : WarehouseType.Ingredient;
            if (_items == null || _items.Count == 0)
            {
                MessageBox.Show("Vui lòng thêm ít nhất một dòng hàng hóa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var unitCode = cbdonviexportkho.SelectedValue as string;
            if (string.IsNullOrWhiteSpace(unitCode))
            {
                MessageBox.Show("Vui lòng chọn đơn vị", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateExportLines(out var validationMessage, out var warnings))
            {
                MessageBox.Show(validationMessage, "Không thể tạo phiếu xuất", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (warnings != null && warnings.Count > 0)
            {
                var warningText = string.Join(Environment.NewLine, warnings.Select(w => "- " + w));
                MessageBox.Show("Tồn kho đã chạm/ngang ngưỡng cảnh báo cho các mặt hàng:\n" + warningText,
                    "Cảnh báo tồn kho", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            var voucher = new WarehouseVoucherDTO
            {
                VoucherType = "EXPORT",
                WarehouseType = selectedType == WarehouseType.Ingredient ? "INGREDIENT" : "EQUIPMENT",
                UnitCode = unitCode,
                Status = cbStatusExport.SelectedItem?.ToString(),
                Note = string.Empty
            };

            var details = BuildExportDetails(selectedType);
            if (details.Count == 0)
            {
                MessageBox.Show("Không có dòng dữ liệu hợp lệ để lưu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (IsEditMode)
                {
                    voucher.VoucherID = _editingVoucher.VoucherID;
                    _voucherBll.UpdateDraftVoucher(voucher, details);

                    var isDraft = string.Equals(voucher.Status, "Nháp", StringComparison.OrdinalIgnoreCase);
                    var msg = isDraft
                        ? "Cập nhật phiếu nháp thành công"
                        : "Cập nhật phiếu và đã cập nhật tồn kho";

                    MessageBox.Show(msg, "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    var created = _voucherBll.CreateVoucherAndApplyStock(voucher, details);
                    txtMaPhieuexport.Text = created.VoucherCode;

                    var isDraft = string.Equals(voucher.Status, "Nháp", StringComparison.OrdinalIgnoreCase);
                    var msg = isDraft
                        ? "Tạo phiếu nháp thành công"
                        : "Tạo phiếu xuất thành công và đã cập nhật tồn kho";

                    MessageBox.Show(msg, "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    DialogResult = DialogResult.OK;
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Tạo phiếu xuất thất bại: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnExistExport_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void HideGridColumns()
        {
            if (DGVexportkho.Columns["ItemID"] != null)
                DGVexportkho.Columns["ItemID"].Visible = false;

            if (DGVexportkho.Columns["ItemCode"] != null)
                DGVexportkho.Columns["ItemCode"].Visible = false;

            if (DGVexportkho.Columns["ItemName"] != null)
                DGVexportkho.Columns["ItemName"].Visible = false;
            if (DGVexportkho.Columns["IngredientID"] != null)
                DGVexportkho.Columns["IngredientID"].Visible = false;
            if (DGVexportkho.Columns["DisplayItem"] != null)
                DGVexportkho.Columns["DisplayItem"].Visible = false;
        }

        private void LocalizeGridHeaders()
        {
            if (DGVexportkho.Columns["IngredientCode"] != null)
                DGVexportkho.Columns["IngredientCode"].HeaderText = "Mã hàng";
            if (DGVexportkho.Columns["IngredientName"] != null)
                DGVexportkho.Columns["IngredientName"].HeaderText = "Tên hàng";
            if (DGVexportkho.Columns["Unit"] != null)
                DGVexportkho.Columns["Unit"].HeaderText = "Đơn vị";
            if (DGVexportkho.Columns["Quantity"] != null)
                DGVexportkho.Columns["Quantity"].HeaderText = "Số lượng";
            if (DGVexportkho.Columns["UnitPrice"] != null)
                DGVexportkho.Columns["UnitPrice"].HeaderText = "Đơn giá";
            if (DGVexportkho.Columns["Total"] != null)
                DGVexportkho.Columns["Total"].HeaderText = "Thành tiền";
            if (DGVexportkho.Columns["StockQuantity"] != null)
                DGVexportkho.Columns["StockQuantity"].HeaderText = "Số lượng hàng tồn kho";
        }

        private void AddOrMergeItem(SelectItemDTO newItem)
        {
            if (newItem == null) return;
            var exist = _items.FirstOrDefault(x => x.ItemID == newItem.ItemID);
            if (exist == null)
            {
                _items.Add(newItem);
                return;
            }
            exist.Quantity += newItem.Quantity;
            if (newItem.StockQuantity > 0)
            {
                exist.StockQuantity = Math.Max(exist.StockQuantity, newItem.StockQuantity);
            }
            if (newItem.MinStock > 0)
            {
                exist.MinStock = newItem.MinStock;
            }
            DGVexportkho.Refresh();
        }

        private List<WarehouseVoucherDetailDTO> BuildExportDetails(WarehouseType selectedType)
        {
            var details = new List<WarehouseVoucherDetailDTO>();
            bool isIngredientWarehouse = selectedType == WarehouseType.Ingredient;

            foreach (DataGridViewRow row in DGVexportkho.Rows)
            {
                if (row.IsNewRow) continue;
                if (!(row.DataBoundItem is SelectItemDTO item)) continue;

                var detail = CreateDetailFromExportItem(item, isIngredientWarehouse);
                if (detail != null)
                {
                    details.Add(detail);
                }
            }

            return details;
        }

        private bool ValidateExportLines(out string message, out List<string> warnings)
        {
            message = null;
            warnings = new List<string>();
            if (_items == null || _items.Count == 0) return true;

            foreach (var item in _items)
            {
                var displayName = item.ItemName ?? item.IngredientName ?? item.ItemCode ?? item.IngredientCode ?? "(Không rõ)";
                var stock = item.StockQuantity;
                var minStock = item.MinStock;
                var remaining = stock - item.Quantity;

                if (stock <= 0)
                {
                    message = $"Mặt hàng {displayName} không còn tồn kho để xuất.";
                    return false;
                }

                if (remaining < 0)
                {
                    message = $"Mặt hàng {displayName} không đủ số lượng để xuất.";
                    return false;
                }

                if (minStock > 0 && remaining <= minStock)
                {
                    warnings.Add($"{displayName}: còn lại {remaining:N0}, ngưỡng cảnh báo {minStock:N0}");
                }
            }

            return true;
        }

        private void PopulateStockSnapshot(SelectItemDTO item, bool isIngredientWarehouse)
        {
            if (item == null) return;

            DataTable dt = null;
            try
            {
                if (isIngredientWarehouse && item.IngredientID > 0)
                {
                    dt = _ingredientBll.GetIngredientById(item.IngredientID);
                }
                else if (!isIngredientWarehouse && item.ItemID > 0)
                {
                    dt = _equipmentBll.GetEquipmentById(item.ItemID);
                }
            }
            catch
            {
                dt = null;
            }

            if (dt == null || dt.Rows.Count == 0) return;

            var row = dt.Rows[0];
            if (row.Table.Columns.Contains("StockQuantity") && row["StockQuantity"] != DBNull.Value)
            {
                item.StockQuantity = Convert.ToDecimal(row["StockQuantity"]);
            }
            if (row.Table.Columns.Contains("MinStock") && row["MinStock"] != DBNull.Value)
            {
                item.MinStock = Convert.ToDecimal(row["MinStock"]);
            }
        }

        private WarehouseVoucherDetailDTO CreateDetailFromExportItem(SelectItemDTO item, bool isIngredientWarehouse)
        {
            if (item == null || item.Quantity <= 0) return null;

            int? ingredientId = null;
            int? equipmentId = null;

            if (isIngredientWarehouse)
            {
                if (item.IngredientID > 0)
                {
                    ingredientId = item.IngredientID;
                }
                else
                {
                    ingredientId = _voucherBll.ResolveIngredientIdByCode(item.IngredientCode);
                }

                if (!ingredientId.HasValue)
                {
                    return null;
                }
                equipmentId = null;
            }
            else
            {
                if (item.ItemID > 0)
                {
                    equipmentId = item.ItemID;
                }
                else
                {
                    equipmentId = _voucherBll.ResolveEquipmentIdByCode(item.ItemCode);
                }

                if (!equipmentId.HasValue)
                {
                    return null;
                }
                ingredientId = null;
            }

            return new WarehouseVoucherDetailDTO
            {
                IngredientID = ingredientId,
                EquipmentID = equipmentId,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                LineTotal = item.Quantity * item.UnitPrice
            };
        }
    }
}
