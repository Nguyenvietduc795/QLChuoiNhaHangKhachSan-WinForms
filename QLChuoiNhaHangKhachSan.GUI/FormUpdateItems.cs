using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using QLChuoiNhaHangKhachSan.BLL;
using QLChuoiNhaHangKhachSan.DTO;

namespace QLChuoiNhaHangKhachSan.GUI
{
    public partial class FormUpdateItems : Form
    {
        private readonly InventoryItemBLL _inventoryItemBll =
            new InventoryItemBLL(ConfigurationManager.ConnectionStrings["QuanLyChuoiNhaHangKhachSan"].ConnectionString);
        private WarehouseType _currentType = WarehouseType.Ingredient;
        private List<InventoryItemDetailDTO> _listCache = new List<InventoryItemDetailDTO>();
        private bool _isBindingNames;
        private string _pendingWarehouseType;
        private string _pendingItemCode;

        public FormUpdateItems()
        {
            InitializeComponent();
        }

        private void FormUpdateItems_Load(object sender, EventArgs e)
        {
            InitializeTypeCombo();

            if (!string.IsNullOrWhiteSpace(_pendingWarehouseType))
            {
                if (string.Equals(_pendingWarehouseType, "EQUIPMENT", StringComparison.OrdinalIgnoreCase))
                {
                    _currentType = WarehouseType.Equipment;
                }
                else if (string.Equals(_pendingWarehouseType, "INGREDIENT", StringComparison.OrdinalIgnoreCase))
                {
                    _currentType = WarehouseType.Ingredient;
                }
            }

            cbTypeupdateItems.SelectedIndex =
                _currentType == WarehouseType.Equipment ? 0 : 1;
            LoadItemsForCurrentType();
            TryApplyPendingItemSelection();
        }

        private void btnCancelUpdateItems_click(object sender, EventArgs e)
        {
            Close();
        }

        private void cbTypeupdateItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            _currentType = cbTypeupdateItems.SelectedIndex == 0
                ? WarehouseType.Equipment
                : WarehouseType.Ingredient;
            LoadItemsForCurrentType();
            ClearDetails();
        }

        private void txTimupdateItems_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter(txFindupdateItems.Text);
        }

        private void cbNameupdateItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isBindingNames) return;
            if (cbNameupdateItems.SelectedValue is int id)
                LoadItemDetails(id);
        }

        private void btnSaveUpdateItems_Click(object sender, EventArgs e)
        {
            if (!TryCollectDetail(out var dto)) return;

            try
            {
                if (_currentType == WarehouseType.Equipment)
                    _inventoryItemBll.UpdateEquipment(dto);
                else
                    _inventoryItemBll.UpdateIngredient(dto);

                MessageBox.Show("Cập nhật mặt hàng thành công",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chưa cập nhật mặt hàng thành công: "
                    + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadItemsForCurrentType()
        {
            _listCache = _currentType == WarehouseType.Equipment
                ? _inventoryItemBll.GetEquipmentList().ToList()
                : _inventoryItemBll.GetIngredientList().ToList();
            ApplyFilter(txFindupdateItems.Text);
        }

        private void BindCombo(IEnumerable<InventoryItemDetailDTO> data)
        {
            var list = data?.ToList() ?? new List<InventoryItemDetailDTO>();

            _isBindingNames = true;
            cbNameupdateItems.DataSource = null;
            cbNameupdateItems.DisplayMember = nameof(InventoryItemDetailDTO.DisplayLabel);
            cbNameupdateItems.ValueMember = nameof(InventoryItemDetailDTO.Id);
            cbNameupdateItems.DataSource = list;
            cbNameupdateItems.SelectedIndex = -1;
            _isBindingNames = false;
        }

        private void LoadItemDetails(int id)
        {
            var dto = _currentType == WarehouseType.Equipment
                ? _inventoryItemBll.GetEquipmentById(id)
                : _inventoryItemBll.GetIngredientById(id);

            if (dto == null) return;

            txCodeupdateItems.Text = dto.Code;
            cbNameupdateItems.Text = dto.Name;
            txUnitupdateItems.Text = dto.Unit;
            txDefaultPriceupdateItems.Text = dto.DefaultPrice.ToString("0.##");
            txStockQuantityupdateItems.Text = dto.StockQuantity.ToString("0.##");
            txMinStockupdateItems.Text = dto.MinStock.ToString("0.##");
        }

        private void InitializeTypeCombo()
        {
            if (cbTypeupdateItems.Items.Count > 0) return;
            cbTypeupdateItems.Items.AddRange(new object[]
            {
                "Kho Thiết Bị",
                "Kho Nguyên Liệu"
            });
        }

        private void ClearDetails()
        {
            _isBindingNames = true;
            cbNameupdateItems.SelectedIndex = -1;
            _isBindingNames = false;
            txCodeupdateItems.Text = string.Empty;
            cbNameupdateItems.Text = string.Empty;
            txUnitupdateItems.Text = string.Empty;
            txDefaultPriceupdateItems.Text = string.Empty;
            txStockQuantityupdateItems.Text = string.Empty;
            txMinStockupdateItems.Text = string.Empty;
        }

        private void ApplyFilter(string keyword)
        {
            IEnumerable<InventoryItemDetailDTO> data = _listCache ?? new List<InventoryItemDetailDTO>();
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var normalized = keyword.Trim().ToLowerInvariant();
                data = data.Where(x =>
                    (!string.IsNullOrWhiteSpace(x.Name) && x.Name.ToLowerInvariant().Contains(normalized)) ||
                    (!string.IsNullOrWhiteSpace(x.Code) && x.Code.ToLowerInvariant().Contains(normalized)));
            }

            BindCombo(data);
            TryApplyPendingItemSelection();
        }

        private bool TryCollectDetail(out InventoryItemDetailDTO dto)
        {
            dto = null;

            var selectedItem = cbNameupdateItems.SelectedItem as InventoryItemDetailDTO;
            if (selectedItem == null && cbNameupdateItems.SelectedValue is int fallbackId)
            {
                selectedItem = _listCache?.FirstOrDefault(x => x.Id == fallbackId);
            }

            if (selectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn mặt hàng cần sửa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            var name = selectedItem.Name?.Trim();
            var unit = txUnitupdateItems.Text?.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Tên mặt hàng không được để trống", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbNameupdateItems.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(unit))
            {
                MessageBox.Show("Đơn vị không được để trống", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txUnitupdateItems.Focus();
                return false;
            }

            if (!TryParseDecimal(txDefaultPriceupdateItems.Text, out var defaultPrice))
            {
                MessageBox.Show("Giá nhập không hợp lệ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txDefaultPriceupdateItems.Focus();
                return false;
            }

            if (!TryParseDecimal(txStockQuantityupdateItems.Text, out var stockQty))
            {
                MessageBox.Show("Số lượng tồn không hợp lệ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txStockQuantityupdateItems.Focus();
                return false;
            }

            if (!TryParseDecimal(txMinStockupdateItems.Text, out var minStock))
            {
                MessageBox.Show("Ngưỡng cảnh báo không hợp lệ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txMinStockupdateItems.Focus();
                return false;
            }

            dto = new InventoryItemDetailDTO
            {
                Id = selectedItem.Id,
                Code = txCodeupdateItems.Text,
                Name = name,
                Unit = unit,
                DefaultPrice = defaultPrice,
                StockQuantity = stockQty,
                MinStock = minStock,
                Type = _currentType
            };
            return true;
        }

        private static bool TryParseDecimal(string input, out decimal value)
        {
            return decimal.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out value)
                   && value >= 0;
        }

        private void TryApplyPendingItemSelection()
        {
            var list = cbNameupdateItems.DataSource as List<InventoryItemDetailDTO>;
            if (list == null || list.Count == 0) return;

            if (!string.IsNullOrWhiteSpace(_pendingItemCode))
            {
                var index = list.FindIndex(x => string.Equals(x.Code, _pendingItemCode, StringComparison.OrdinalIgnoreCase));
                if (index >= 0)
                {
                    cbNameupdateItems.SelectedIndex = index;
                    _pendingItemCode = null;
                    return;
                }
            }
        }

        public void PreselectWarehouse(string warehouseType)
        {
            _pendingWarehouseType = warehouseType;
        }

        public void PreselectItemCode(string itemCode)
        {
            _pendingItemCode = itemCode;
        }

        private void BtnExistUpdateItems_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
