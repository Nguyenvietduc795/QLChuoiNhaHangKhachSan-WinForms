using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using QLChuoiNhaHangKhachSan.BLL;
using QLChuoiNhaHangKhachSan.DTO;
using System.Configuration;

namespace QLChuoiNhaHangKhachSan.GUI
{
    public partial class FormSelectItems : Form
    {
        private bool _isLoading;
        private DataTable _dtAll;
        private bool _isBinding;
        private readonly IngredientBLL _ingredientBll;
        private readonly EquipmentBLL _equipmentBll;
        private readonly SelectMode _mode;
        private readonly WarehouseType _warehouseType;
        private readonly ImportWarehouseItemDTO _editingItem;
        private readonly SelectItemDTO _editingExportItem;
        private bool _suppressPriceUpdate;

        public ImportWarehouseItemDTO SelectedItem { get; private set; }
        public List<SelectItemDTO> SelectedItems { get; private set; }

        public FormSelectItems() : this(SelectMode.Import, WarehouseType.Ingredient) { }

        public FormSelectItems(ImportWarehouseItemDTO editingItem) : this(SelectMode.Import, WarehouseType.Ingredient)
        {
            _editingItem = editingItem;
        }

        public FormSelectItems(SelectItemDTO editingItem, WarehouseType warehouseType) : this(SelectMode.Export, warehouseType)
        {
            _editingExportItem = editingItem;
        }

        public FormSelectItems(SelectMode mode, WarehouseType warehouseType)
        {
            InitializeComponent();

            _mode = mode;
            _warehouseType = warehouseType;

            string conn = ConfigurationManager.ConnectionStrings["RHGROUP"].ConnectionString;
            _ingredientBll = new IngredientBLL(conn);
            _equipmentBll = new EquipmentBLL(conn);

            this.Load += FormSelectItems_Load;
            cboDonViSelectItems.SelectedIndexChanged += cboDonViSelectItems_SelectedIndexChanged;
            txMaHangSelectItems.TextChanged += txMaHangSelectItems_TextChanged;
        }

        private void FormSelectItems_Load(object sender, EventArgs e)
        {
            // 1) Tắt autocomplete trước để tránh exception lúc control chưa sẵn
            cboDonViSelectItems.AutoCompleteMode = AutoCompleteMode.None;
            cboDonViSelectItems.AutoCompleteSource = AutoCompleteSource.None;

            // 2) BẮT BUỘC set DropDownStyle trước khi bind DataSource
            cboDonViSelectItems.DropDownStyle = ComboBoxStyle.DropDown;

            _isLoading = true;
            _suppressPriceUpdate = true;

            InitPriceControl();
            LoadItemsFromDb();
            ApplyModeUi();

            if (_editingExportItem != null)
            {
                cboDonViSelectItems.SelectedValue = _editingExportItem.ItemID;
                nudSoluong.Value = _editingExportItem.Quantity;
                nudDonGia.Value = _editingExportItem.UnitPrice;
                btnThemSelectItems.Text = "Lưu";
            }
            else if (_editingItem != null)
            {
                cboDonViSelectItems.SelectedValue = _editingItem.ItemID;
                nudSoluong.Value = _editingItem.Quantity;
                nudDonGia.Value = _editingItem.UnitPrice;
                btnThemSelectItems.Text = "Lưu";
            }

            _suppressPriceUpdate = false;
            _isLoading = false;
        }

        private void LoadItemsFromDb()
        {
            if (_warehouseType == WarehouseType.Ingredient)
            {
                _dtAll = _mode == SelectMode.Export ? _ingredientBll.GetIngredientsForExport() : _ingredientBll.GetIngredientsForCombo();
            }
            else
            {
                _dtAll = _mode == SelectMode.Export ? _equipmentBll.GetEquipmentForExport() : _equipmentBll.GetEquipmentForImport();
            }

            if (!_dtAll.Columns.Contains("DisplayText"))
            {
                _dtAll.Columns.Add("DisplayText", typeof(string));
            }

            foreach (DataRow row in _dtAll.Rows)
            {
                var code = row[CodeColumn]?.ToString();
                var name = row[NameColumn]?.ToString();
                var unit = row["Unit"]?.ToString();
                row["DisplayText"] = $"{code} : {name} ({unit})";
            }

            BindCombo(_dtAll, IdColumn);
        }

        private void InitPriceControl()
        {
            nudDonGia.Minimum = 0;
            nudDonGia.Maximum = 1000000000;
            nudDonGia.DecimalPlaces = 0;
            nudDonGia.ThousandsSeparator = true;
        }

        private bool IsEditing => _editingItem != null || _editingExportItem != null;

        private void UpdatePriceFromSelection()
        {
            var drv = cboDonViSelectItems.SelectedItem as DataRowView;
            if (drv == null) return;

            // ✅ chỉ chặn khi đang load/bind/edit set giá trị ban đầu
            if (_suppressPriceUpdate)
            {
                // vẫn cập nhật đơn vị nếu muốn
                var lblUnit = this.Controls.Find("lblDonVi", true).FirstOrDefault() as Label;
                if (lblUnit != null)
                    lblUnit.Text = drv.Row.Field<string>("Unit") ?? string.Empty;
                return;
            }

            var price = drv.Row.Field<decimal?>("DefaultPrice") ?? 0;

            if (price > nudDonGia.Maximum) nudDonGia.Maximum = price;
            if (price < nudDonGia.Minimum) nudDonGia.Minimum = price;

            nudDonGia.Value = price;

            var lblUnit2 = this.Controls.Find("lblDonVi", true).FirstOrDefault() as Label;
            if (lblUnit2 != null)
                lblUnit2.Text = drv.Row.Field<string>("Unit") ?? string.Empty;
        }

        private void ApplyModeUi()
        {
            if (_mode == SelectMode.Export)
            {
                lbSelectItems.Text = "Chọn hàng để XUẤT";
            }
        }

        private void cboDonViSelectItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isBinding) return;
            if (_isLoading) return;

            UpdatePriceFromSelection();
        }

        private void btnThemSelectItems_Click(object sender, EventArgs e)
        {
            var drv = cboDonViSelectItems.SelectedItem as DataRowView;
            if (drv == null)
            {
                MessageBox.Show("Vui lòng chọn mặt hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (nudSoluong.Value <= 0)
            {
                MessageBox.Show("Số lượng phải lớn hơn 0", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var idCol = IdColumn;
            var codeCol = CodeColumn;
            var nameCol = NameColumn;

            if (_mode == SelectMode.Import)
            {
                SelectedItem = new ImportWarehouseItemDTO
                {
                    ItemID = drv.Row.Field<int>(idCol),
                    ItemCode = drv.Row.Field<string>(codeCol),
                    ItemName = drv.Row.Field<string>(nameCol),
                    IngredientID = drv.Row.Field<int>(idCol),
                    IngredientCode = drv.Row.Field<string>(codeCol),
                    IngredientName = drv.Row.Field<string>(nameCol),
                    Unit = drv.Row.Field<string>("Unit"),
                    Quantity = nudSoluong.Value,
                    UnitPrice = nudDonGia.Value
                };
            }
            else
            {
                var stock = drv.Row.Field<decimal?>("StockQuantity") ?? 0;
                if (nudSoluong.Value > stock)
                {
                    MessageBox.Show("Số lượng xuất không được vượt quá tồn kho", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var item = new SelectItemDTO
                {
                    ItemID = drv.Row.Field<int>(idCol),
                    ItemCode = drv.Row.Field<string>(codeCol),
                    ItemName = drv.Row.Field<string>(nameCol),
                    IngredientID = drv.Row.Field<int>(idCol),
                    IngredientCode = drv.Row.Field<string>(codeCol),
                    IngredientName = drv.Row.Field<string>(nameCol),
                    Unit = drv.Row.Field<string>("Unit"),
                    StockQuantity = stock,
                    Quantity = nudSoluong.Value,
                    UnitPrice = nudDonGia.Value
                };
                SelectedItems = new List<SelectItemDTO> { item };
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancelSelecItems_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnExistSelectItems_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BindCombo(DataTable dt, string valueMember)
        {
            _isBinding = true;

            cboDonViSelectItems.DataSource = null;
            cboDonViSelectItems.DisplayMember = "DisplayText";
            cboDonViSelectItems.ValueMember = valueMember;
            cboDonViSelectItems.DataSource = dt;

            cboDonViSelectItems.DropDownStyle = ComboBoxStyle.DropDown;
            cboDonViSelectItems.AutoCompleteSource = AutoCompleteSource.ListItems;
            cboDonViSelectItems.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            _isBinding = false;
        }

        private void txMaHangSelectItems_TextChanged(object sender, EventArgs e)
        {
            if (_dtAll == null) return;

            string keyword = txMaHangSelectItems.Text.Trim().Replace("'", "''");
            var codeCol = CodeColumn;
            var nameCol = NameColumn;

            _isLoading = true; // chặn event

            try
            {
                if (string.IsNullOrWhiteSpace(keyword))
                {
                    BindCombo(_dtAll, IdColumn);
                    return;
                }

                var dv = new DataView(_dtAll);
                dv.RowFilter = string.Format("{0} LIKE '%{1}%' OR {2} LIKE '%{1}%'", codeCol, keyword, nameCol);

                BindCombo(dv.ToTable(), IdColumn);

                if (cboDonViSelectItems.Items.Count > 0)
                {
                    if (IsEditing)
                    {
                        int id = _editingExportItem != null ? _editingExportItem.ItemID : _editingItem.ItemID;
                        cboDonViSelectItems.SelectedValue = id; // giữ đúng dòng đang edit
                    }
                    else
                    {
                        cboDonViSelectItems.SelectedIndex = 0;
                    }
                }
            }
            finally
            {
                _isLoading = false; // ✅ luôn luôn trả về false
            }
        }

        private string IdColumn => _warehouseType == WarehouseType.Ingredient ? "IngredientID" : "EquipmentID";
        private string CodeColumn => _warehouseType == WarehouseType.Ingredient ? "IngredientCode" : "EquipmentCode";
        private string NameColumn => _warehouseType == WarehouseType.Ingredient ? "IngredientName" : "EquipmentName";
    }
}
