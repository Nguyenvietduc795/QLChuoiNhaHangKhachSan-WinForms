using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Windows.Forms;
using QLChuoiNhaHangKhachSan.BLL;
using QLChuoiNhaHangKhachSan.DTO;

namespace QLChuoiNhaHangKhachSan.GUI
{
    public partial class FormStopItems : Form
    {
        private readonly InventoryItemBLL _inventoryItemBll =
            new InventoryItemBLL(ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString);

        private WarehouseType _currentType = WarehouseType.Ingredient;
        private List<InventoryItemDetailDTO> _listCache = new List<InventoryItemDetailDTO>();
        private bool _isBindingNames;
        private string _pendingWarehouseType;
        private string _pendingItemCode;

        public FormStopItems()
        {
            InitializeComponent();
        }

        private void FormStopItems_Load(object sender, EventArgs e)
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

            cbTypeStopItems.SelectedIndexChanged -= cbTypeStopItems_SelectedIndexChanged;
            cbTypeStopItems.SelectedIndex = _currentType == WarehouseType.Equipment ? 0 : 1;
            cbTypeStopItems.SelectedIndexChanged += cbTypeStopItems_SelectedIndexChanged;
            LoadItemsForCurrentType();
            TryApplyPendingItemSelection();
        }

        private void BtnExistUpdateItems_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnCancelStopItems_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSaveStopItems_Click(object sender, EventArgs e)
        {
            if (!(cbNameStopItems.SelectedValue is int id))
            {
                MessageBox.Show("Vui lòng chọn mặt hàng cần ngừng sử dụng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirm = MessageBox.Show("Bạn có chắc chắn muốn ngừng sử dụng mặt hàng này không?",
                "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            try
            {
                _inventoryItemBll.StopItem(_currentType, id);
                MessageBox.Show("Ngừng sử dụng mặt hàng thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ngừng sử dụng mặt hàng chưa thành công: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pnlCardstopItems_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cbTypeStopItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            _currentType = cbTypeStopItems.SelectedIndex == 0
                ? WarehouseType.Equipment
                : WarehouseType.Ingredient;

            LoadItemsForCurrentType();
            ClearDetails();
        }

        private void cbNameStopItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isBindingNames) return;
            if (cbNameStopItems.SelectedValue is int id)
            {
                LoadItemDetails(id);
            }
        }

        private void txUnitstopItems_TextChanged(object sender, EventArgs e)
        {

        }

        private void txDefaultPricestopItems_TextChanged(object sender, EventArgs e)
        {

        }

        private void txStockQuantitystopItems_TextChanged(object sender, EventArgs e)
        {

        }

        private void txMinStockstopItems_TextChanged(object sender, EventArgs e)
        {

        }

        public void PreselectWarehouse(string warehouseType)
        {
            _pendingWarehouseType = warehouseType;
        }

        public void PreselectItemCode(string itemCode)
        {
            _pendingItemCode = itemCode;
        }

        private void InitializeTypeCombo()
        {
            if (cbTypeStopItems.Items.Count > 0) return;
            cbTypeStopItems.Items.AddRange(new object[]
            {
                "Kho Thiết Bị",
                "Kho Nguyên Liệu"
            });
        }

        private void LoadItemsForCurrentType()
        {
            _listCache = _currentType == WarehouseType.Equipment
                ? _inventoryItemBll.GetEquipmentList().ToList()
                : _inventoryItemBll.GetIngredientList().ToList();

            BindCombo(_listCache);
            TryApplyPendingItemSelection();
        }

        private void BindCombo(IEnumerable<InventoryItemDetailDTO> data)
        {
            var list = data?.ToList() ?? new List<InventoryItemDetailDTO>();

            _isBindingNames = true;
            cbNameStopItems.DataSource = null;
            cbNameStopItems.DisplayMember = nameof(InventoryItemDetailDTO.DisplayLabel);
            cbNameStopItems.ValueMember = nameof(InventoryItemDetailDTO.Id);
            cbNameStopItems.DataSource = list;
            cbNameStopItems.SelectedIndex = -1;
            _isBindingNames = false;
        }

        private void ClearDetails()
        {
            _isBindingNames = true;
            cbNameStopItems.SelectedIndex = -1;
            _isBindingNames = false;
            txCodeStopItems.Text = string.Empty;
            txUnitstopItems.Text = string.Empty;
            txDefaultPricestopItems.Text = string.Empty;
            txStockQuantitystopItems.Text = string.Empty;
            txMinStockstopItems.Text = string.Empty;
            txLastUpdatedstopItems.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }

        private void LoadItemDetails(int id)
        {
            var dto = _currentType == WarehouseType.Equipment
                ? _inventoryItemBll.GetEquipmentById(id)
                : _inventoryItemBll.GetIngredientById(id);

            if (dto == null)
            {
                ClearDetails();
                return;
            }

            txCodeStopItems.Text = dto.Code;
            txUnitstopItems.Text = dto.Unit;
            txDefaultPricestopItems.Text = dto.DefaultPrice.ToString("0.##");
            txStockQuantitystopItems.Text = dto.StockQuantity.ToString("0.##");
            txMinStockstopItems.Text = dto.MinStock.ToString("0.##");
            txLastUpdatedstopItems.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }

        private void TryApplyPendingItemSelection()
        {
            var list = cbNameStopItems.DataSource as List<InventoryItemDetailDTO>;
            if (list == null || list.Count == 0) return;

            if (!string.IsNullOrWhiteSpace(_pendingItemCode))
            {
                var index = list.FindIndex(x => string.Equals(x.Code, _pendingItemCode, StringComparison.OrdinalIgnoreCase));
                if (index >= 0)
                {
                    cbNameStopItems.SelectedIndex = index;
                    _pendingItemCode = null;
                    return;
                }
            }
        }

        private void SepBottomstopItems_Click(object sender, EventArgs e)
        {

        }
    }
}
