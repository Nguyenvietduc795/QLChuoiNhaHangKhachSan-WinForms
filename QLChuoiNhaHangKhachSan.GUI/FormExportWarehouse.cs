using QLChuoiNhaHangKhachSan.BLL;
using System;
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

        public FormExportWarehouse()
        {
            InitializeComponent();
            this.Load += FormExportWarehouse_Load;
            _unitBll = new UnitBLL(ConfigurationManager.ConnectionStrings["RHGROUP"].ConnectionString);
            _voucherBll = new WarehouseVoucherBLL(ConfigurationManager.ConnectionStrings["RHGROUP"].ConnectionString);
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
            try
            {
                txtMaPhieuexport.Text = _voucherBll.GetNextVoucherCode(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không lấy được mã phiếu tiếp theo: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            var voucher = new WarehouseVoucherDTO
            {
                VoucherType = "EXPORT",
                WarehouseType = selectedType == WarehouseType.Ingredient ? "INGREDIENT" : "EQUIPMENT",
                UnitCode = unitCode,
                Status = cbStatusExport.SelectedItem?.ToString(),
                Note = string.Empty
            };

            var details = _items.Select(i => new WarehouseVoucherDetailDTO
            {
                IngredientID = selectedType == WarehouseType.Ingredient && i.IngredientID > 0 ? (int?)i.IngredientID : null,
                EquipmentID = selectedType == WarehouseType.Equipment && i.ItemID > 0 ? (int?)i.ItemID : null,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                LineTotal = i.Quantity * i.UnitPrice
            }).ToList();

            try
            {
                // ... build voucher + details, gọi BLL lưu ...
                var created = _voucherBll.CreateVoucherWithDetails(voucher, details);
                txtMaPhieuexport.Text = created.VoucherCode;

                MessageBox.Show("Tạo phiếu xuất thành công", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
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
            DGVexportkho.Refresh();
        }
    }
}
