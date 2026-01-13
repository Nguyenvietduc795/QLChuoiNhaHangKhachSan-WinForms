using QLChuoiNhaHangKhachSan.BLL;
using QLChuoiNhaHangKhachSan.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace QLChuoiNhaHangKhachSan.GUI
{

    public partial class FormImportWarehouse : Form
    {
        private BindingList<ImportWarehouseItemDTO> _items;
        private readonly UnitBLL _unitBll;
        private readonly WarehouseVoucherBLL _voucherBll;
        private WarehouseVoucherDTO _editingVoucher;
        private bool IsEditMode => _editingVoucher != null;

        public FormImportWarehouse(WarehouseVoucherDTO editingVoucher = null)
        {
            InitializeComponent();
            this.Load += FormImportWarehouse_Load;
            _unitBll = new UnitBLL(ConfigurationManager.ConnectionStrings["QuanLyChuoiNhaHangKhachSan"].ConnectionString);
            _voucherBll = new WarehouseVoucherBLL(ConfigurationManager.ConnectionStrings["QuanLyChuoiNhaHangKhachSan"].ConnectionString);
            _editingVoucher = editingVoucher;
        }

        private void FormImportWarehouse_Load(object sender, EventArgs e)
        {
            InitCombos();
            InitGrid();
            txtMaPhieuimport.ReadOnly = true;
            txtMaPhieuimport.TabStop = false;
            if (IsEditMode)
            {
                LoadVoucherDraftForEdit();
            }
            else
            {
                txtMaPhieuimport.Text = _voucherBll.GetNextVoucherCode(true);   
            }
        }

        private void InitCombos()
        {
            cbStatusImport.Items.Clear();
            cbStatusImport.Items.AddRange(new object[] { "Nháp", "Hoàn thành" });
            if (cbStatusImport.Items.Count > 0) cbStatusImport.SelectedIndex = 0;

            // Bind loại kho riêng, không dùng chung DataSource với đơn vị
            cbtypekhoimport.DisplayMember = "Text";
            cbtypekhoimport.ValueMember = "Value";
            cbtypekhoimport.DataSource = new[]
            {
                new { Text = "Kho Thiết Bị",   Value = WarehouseType.Equipment },
                new { Text = "Kho Nguyên Liệu", Value = WarehouseType.Ingredient }
            };
            if (cbtypekhoimport.Items.Count > 0) cbtypekhoimport.SelectedIndex = 0;
            cbtypekhoimport.SelectedIndexChanged += Cbtypekhoimport_SelectedIndexChanged;

            LoadDonViFromDb();
            cbdonviimportkho.SelectedIndexChanged += Cbdonviimportkho_SelectedIndexChanged;
        }

        private void Cbtypekhoimport_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDonViFromDb();
            _items?.Clear();
        }

        private void LoadDonViFromDb()
        {
            int areaId = 1; // Branch 1 (chưa có login)
            var type = cbtypekhoimport.SelectedValue is WarehouseType wt ? wt : WarehouseType.Ingredient;
            string unitType = type == WarehouseType.Ingredient ? "RESTAURANT" : "HOTEL";
            var dt = _unitBll.GetUnitsByAreaAndType(areaId, unitType);

            // Lọc đúng loại kho và chỉ lấy theo UnitType
            DataTable filtered = null;
            if (dt != null && dt.Rows.Count > 0)
            {
                var rows = dt.AsEnumerable().Where(r => r.Field<string>("UnitType") == unitType);
                if (rows.Any()) filtered = rows.CopyToDataTable();
            }

            cbdonviimportkho.DataSource = null;
            if (filtered != null && filtered.Rows.Count > 0)
            {
                cbdonviimportkho.DisplayMember = "UnitCode";
                cbdonviimportkho.ValueMember = "UnitCode";
                cbdonviimportkho.DataSource = filtered;
                if (cbdonviimportkho.Items.Count > 0) cbdonviimportkho.SelectedIndex = 0;
            }
            else
            {
                cbdonviimportkho.Items.Clear();
            }
        }

        private void Cbdonviimportkho_SelectedIndexChanged(object sender, EventArgs e)
        {
            var type = cbtypekhoimport.SelectedValue is WarehouseType wt ? wt : WarehouseType.Ingredient;
            var code = cbdonviimportkho.SelectedValue as string;
            if (string.IsNullOrEmpty(code)) return;

            bool ok = (type == WarehouseType.Ingredient && code.StartsWith("NH")) ||
                      (type == WarehouseType.Equipment  && code.StartsWith("KS"));
            if (!ok)
            {
                MessageBox.Show("Vui lòng chọn lại đơn vị đúng với loại kho", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                // tìm lại lựa chọn hợp lệ
                for (int i = 0; i < cbdonviimportkho.Items.Count; i++)
                {
                    var drv = cbdonviimportkho.Items[i] as DataRowView;
                    var c = drv?["UnitCode"] as string;
                    if (!string.IsNullOrEmpty(c) && ((type == WarehouseType.Ingredient && c.StartsWith("NH")) || (type == WarehouseType.Equipment && c.StartsWith("KS"))))
                    {
                        cbdonviimportkho.SelectedIndex = i;
                        return;
                    }
                }
            }
        }

        private void InitGrid()
        {
            _items = new BindingList<ImportWarehouseItemDTO>();

            DGVimportkho.AutoGenerateColumns = true;
            DGVimportkho.DataSource = _items;
            HideGridColumns();
            LocalizeGridHeaders();
            DGVimportkho.AllowUserToAddRows = false;
            DGVimportkho.RowHeadersVisible = false;
            DGVimportkho.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DGVimportkho.MultiSelect = false;
            DGVimportkho.DefaultCellStyle.SelectionBackColor = Color.White;
            DGVimportkho.DefaultCellStyle.SelectionForeColor = Color.Black;
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
            txtMaPhieuimport.Text = latest.VoucherCode;

            var type = string.Equals(latest.WarehouseType, "EQUIPMENT", StringComparison.OrdinalIgnoreCase)
                ? WarehouseType.Equipment
                : WarehouseType.Ingredient;

            cbtypekhoimport.SelectedValue = type;
            cbtypekhoimport.Enabled = false;
            LoadDonViFromDb();
            if (!string.IsNullOrWhiteSpace(latest.UnitCode))
            {
                try
                {
                    cbdonviimportkho.SelectedValue = latest.UnitCode;
                }
                catch
                {
                    // ignore if not found
                }
            }

            cbStatusImport.SelectedItem = string.IsNullOrWhiteSpace(latest.Status) ? "Nháp" : latest.Status;
            bnCreateImportPhieu.Text = "Lưu phiếu";

            var detailDtos = _voucherBll.GetVoucherDetails(latest.VoucherID) ?? new List<WarehouseVoucherDetailDTO>();
            _items.Clear();
            foreach (var detail in detailDtos)
            {
                var item = new ImportWarehouseItemDTO
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
                _items.Add(item);
            }

            DGVimportkho.Refresh();
        }

        private void bnaddrowimportKho_Click(object sender, EventArgs e)
        {
            var selectedType = cbtypekhoimport.SelectedValue is WarehouseType wt ? wt : WarehouseType.Ingredient;
            if (cbdonviimportkho.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn đơn vị trước khi thêm dòng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            using (var f = new FormSelectItems(SelectMode.Import, selectedType))
            {
                f.StartPosition = FormStartPosition.CenterParent;
                if (f.ShowDialog(this) == DialogResult.OK && f.SelectedItem != null)
                    AddOrMergeItem(f.SelectedItem);
            }
        }

        private void bnupdaterowimportKho_Click(object sender, EventArgs e)
        {
            var selectedType = cbtypekhoimport.SelectedValue is WarehouseType wt ? wt : WarehouseType.Ingredient;
            if (_items == null || DGVimportkho.CurrentRow == null) return;

            var item = DGVimportkho.CurrentRow.DataBoundItem as ImportWarehouseItemDTO;
            if (item == null) return;

            using (var f = new FormSelectItems(item))
            {
                f.StartPosition = FormStartPosition.CenterParent;

                if (f.ShowDialog(this) == DialogResult.OK && f.SelectedItem != null)
                {
                    // cập nhật lại item đang chọn
                    item.IngredientID = f.SelectedItem.IngredientID;
                    item.IngredientCode = f.SelectedItem.IngredientCode;
                    item.IngredientName = f.SelectedItem.IngredientName;
                    item.ItemID = f.SelectedItem.ItemID;
                    item.ItemCode = f.SelectedItem.ItemCode;
                    item.ItemName = f.SelectedItem.ItemName;
                    item.Unit = f.SelectedItem.Unit;
                    item.Quantity = f.SelectedItem.Quantity;
                    item.UnitPrice = f.SelectedItem.UnitPrice;

                    MergeDuplicateWithCurrent(item);
                    DGVimportkho.Refresh();
                }
            }
        }

        private void bndeleterowimportkho_Click(object sender, EventArgs e)
        {
            if (_items == null || DGVimportkho.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn dòng cần xóa",
                                "Thông báo",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            var row = DGVimportkho.SelectedRows[0];
            var item = row.DataBoundItem as ImportWarehouseItemDTO;

            if (item == null) return;

            var confirm = MessageBox.Show(
                $"Bạn có chắc muốn xóa mặt hàng {item.IngredientName}?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                _items.Remove(item);
            }
        }

        private void btnCancelImportKho_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bnCreateImportPhieu_Click(object sender, EventArgs e)
        {
            var selectedType = cbtypekhoimport.SelectedValue is WarehouseType wt ? wt : WarehouseType.Ingredient;
            if (_items == null || _items.Count == 0)
            {
                MessageBox.Show("Vui lòng thêm ít nhất một dòng hàng hóa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var unitCode = cbdonviimportkho.SelectedValue as string;
            if (string.IsNullOrWhiteSpace(unitCode))
            {
                MessageBox.Show("Vui lòng chọn đơn vị", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var voucher = new WarehouseVoucherDTO
            {
                VoucherType = "IMPORT",
                WarehouseType = selectedType == WarehouseType.Ingredient ? "INGREDIENT" : "EQUIPMENT",
                UnitCode = unitCode,
                Status = cbStatusImport.SelectedItem?.ToString(),
                Note = string.Empty
            };

            var details = BuildImportDetails(selectedType);

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

                    MessageBox.Show(msg, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    var created = _voucherBll.CreateVoucherAndApplyStock(voucher, details);
                    txtMaPhieuimport.Text = created.VoucherCode;

                    var isDraft = string.Equals(voucher.Status, "Nháp", StringComparison.OrdinalIgnoreCase);
                    var msg = isDraft
                        ? "Tạo phiếu nháp thành công"
                        : "Tạo phiếu nhập thành công và đã cập nhật tồn kho";

                    MessageBox.Show(msg, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Tạo phiếu nhập thất bại: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnExistImport_Click(object sender, EventArgs e)
        {
            this.Close();
        }
            // Ẩn các cột không hiển thị 
        private void HideGridColumns()
        {
            if (DGVimportkho.Columns["ItemID"] != null)
                DGVimportkho.Columns["ItemID"].Visible = false;

            if (DGVimportkho.Columns["ItemCode"] != null)
                DGVimportkho.Columns["ItemCode"].Visible = false;

            if (DGVimportkho.Columns["ItemName"] != null)
                DGVimportkho.Columns["ItemName"].Visible = false;

            if (DGVimportkho.Columns["IngredientID"] != null)
                DGVimportkho.Columns["IngredientID"].Visible = false;

            if (DGVimportkho.Columns["DisplayItem"] != null)
                DGVimportkho.Columns["DisplayItem"].Visible = false;
        }

        private void LocalizeGridHeaders()
        {
            if (DGVimportkho.Columns["IngredientCode"] != null)
                DGVimportkho.Columns["IngredientCode"].HeaderText = "Mã hàng";

            if (DGVimportkho.Columns["IngredientName"] != null)
                DGVimportkho.Columns["IngredientName"].HeaderText = "Tên hàng";

            if (DGVimportkho.Columns["Unit"] != null)
                DGVimportkho.Columns["Unit"].HeaderText = "Đơn vị";

            if (DGVimportkho.Columns["Quantity"] != null)
                DGVimportkho.Columns["Quantity"].HeaderText = "Số lượng";

            if (DGVimportkho.Columns["UnitPrice"] != null)
                DGVimportkho.Columns["UnitPrice"].HeaderText = "Đơn giá";

            if (DGVimportkho.Columns["Total"] != null)
                DGVimportkho.Columns["Total"].HeaderText = "Thành tiền";
        }
        private void AddOrMergeItem(QLChuoiNhaHangKhachSan.DTO.ImportWarehouseItemDTO newItem)
        {
            if (newItem == null) return;

            var exist = _items.FirstOrDefault(x => IsSameItem(x, newItem));

            if (exist == null)
            {
                _items.Add(newItem);
                return;
            }

            exist.Quantity += newItem.Quantity;

            if (newItem.UnitPrice > 0)
            {
                exist.UnitPrice = newItem.UnitPrice;
            }

            DGVimportkho.Refresh();
        }

        private void MergeDuplicateWithCurrent(ImportWarehouseItemDTO target)
        {
            if (target == null || _items == null) return;

            var duplicates = _items.Where(x => !ReferenceEquals(x, target) && IsSameItem(x, target)).ToList();
            foreach (var dup in duplicates)
            {
                target.Quantity += dup.Quantity;
                if (dup.UnitPrice > 0)
                {
                    target.UnitPrice = dup.UnitPrice;
                }
                _items.Remove(dup);
            }
        }

        private bool IsSameItem(ImportWarehouseItemDTO a, ImportWarehouseItemDTO b)
        {
            if (a == null || b == null) return false;

            if (a.IngredientID > 0 && b.IngredientID > 0)
                return a.IngredientID == b.IngredientID;

            if (a.ItemID > 0 && b.ItemID > 0)
                return a.ItemID == b.ItemID;

            var codeA = !string.IsNullOrWhiteSpace(a.IngredientCode) ? a.IngredientCode : a.ItemCode;
            var codeB = !string.IsNullOrWhiteSpace(b.IngredientCode) ? b.IngredientCode : b.ItemCode;
            if (!string.IsNullOrWhiteSpace(codeA) && !string.IsNullOrWhiteSpace(codeB))
                return string.Equals(codeA, codeB, StringComparison.OrdinalIgnoreCase);

            return false;
        }

        private List<WarehouseVoucherDetailDTO> BuildImportDetails(WarehouseType selectedType)
        {
            var details = new List<WarehouseVoucherDetailDTO>();
            bool isIngredientWarehouse = selectedType == WarehouseType.Ingredient;

            foreach (DataGridViewRow row in DGVimportkho.Rows)
            {
                if (row.IsNewRow) continue;
                if (!(row.DataBoundItem is ImportWarehouseItemDTO item)) continue;

                var detail = CreateDetailFromImportItem(item, isIngredientWarehouse);
                if (detail != null)
                {
                    details.Add(detail);
                }
            }

            return details;
        }

        private WarehouseVoucherDetailDTO CreateDetailFromImportItem(ImportWarehouseItemDTO item, bool isIngredientWarehouse)
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
        private void InitLoaiKhoCombo_Import()
        {
            cbtypekhoimport.DataSource = null;
            cbtypekhoimport.DisplayMember = "Text";
            cbtypekhoimport.ValueMember = "Value";

            cbtypekhoimport.DataSource = new[]
            {
        new { Text = "Kho Thiết Bị",   Value = WarehouseType.Equipment },
        new { Text = "Kho Nguyên Liệu", Value = WarehouseType.Ingredient }
    };

            cbtypekhoimport.SelectedIndex = 0;
        }

    }
}
