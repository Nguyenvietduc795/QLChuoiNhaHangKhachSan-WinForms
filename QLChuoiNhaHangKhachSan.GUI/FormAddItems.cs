using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QLChuoiNhaHangKhachSan.BLL;
using QLChuoiNhaHangKhachSan.DTO;

namespace QLChuoiNhaHangKhachSan.GUI
{
    public partial class FormAddMatHang : Form
    {
        private readonly InventoryItemBLL _inventoryItemBll;

        public FormAddMatHang()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterParent;
            var connStr = ConfigurationManager.ConnectionStrings["QuanLyChuoiNhaHangKhachSan"].ConnectionString;
            _inventoryItemBll = new InventoryItemBLL(connStr);
        }

        private void lblTotal_Click(object sender, EventArgs e)
        {

        }

        private void InitializeTypeCombo()
        {
            if (cbTypeaddItems.Items.Count == 0)
            {
                cbTypeaddItems.Items.AddRange(new object[]
                {
                    "Kho Thiết Bị",
                    "Kho Nguyên Liệu"
                });
            }

            if (cbTypeaddItems.SelectedIndex < 0 && cbTypeaddItems.Items.Count > 0)
            {
                cbTypeaddItems.SelectedIndex = 0;
            }
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2TextBox3_TextChanged(object sender, EventArgs e)
        {

        }

       

        private void btnCancelAddItems_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnSaveAddItems_Click(object sender, EventArgs e)
        {
            if (!TryGetFormData(out var type, out var name, out var unit,
                out var defaultPrice, out var stockQuantity, out var minStock))
            {
                return;
            }

            try
            {
                var result = _inventoryItemBll.CreateItem(type, name, unit, defaultPrice, stockQuantity, minStock);
                MessageBox.Show($"Thêm mặt hàng thành công: {result.NewCode}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Thêm mặt hàng thất bại: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormAddMatHang_Load(object sender, EventArgs e)
        {
            InitializeTypeCombo();
        }

        private void bnExistaddItems_Click(object sender, EventArgs e)
        {
            Close();
        }

        private bool TryGetFormData(out WarehouseType type, out string name, out string unit,
            out decimal defaultPrice, out decimal stockQuantity, out decimal minStock)
        {
            type = WarehouseType.Equipment;
            name = txNameaddItems.Text?.Trim();
            unit = txUnitaddItems.Text?.Trim();
            defaultPrice = stockQuantity = minStock = 0m;

            if (cbTypeaddItems.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn loại kho", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbTypeaddItems.Focus();
                return false;
            }

            type = string.Equals(cbTypeaddItems.SelectedItem.ToString(), "Kho Nguyên Liệu", StringComparison.OrdinalIgnoreCase)
                ? WarehouseType.Ingredient
                : WarehouseType.Equipment;

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Tên hàng không được để trống", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txNameaddItems.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(unit))
            {
                MessageBox.Show("Đơn vị không được để trống", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txUnitaddItems.Focus();
                return false;
            }

            if (!decimal.TryParse(txDefaultPriceaddItems.Text, out defaultPrice) || defaultPrice < 0)
            {
                MessageBox.Show("Giá nhập không hợp lệ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txDefaultPriceaddItems.Focus();
                return false;
            }

            if (!decimal.TryParse(txStockQuantityaddItems.Text, out stockQuantity) || stockQuantity < 0)
            {
                MessageBox.Show("Tồn kho không hợp lệ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txStockQuantityaddItems.Focus();
                return false;
            }

            if (!decimal.TryParse(txMinStockaddItems.Text, out minStock) || minStock < 0)
            {
                MessageBox.Show("Ngưỡng cảnh báo không hợp lệ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txMinStockaddItems.Focus();
                return false;
            }

            return true;
        }
    }
}
