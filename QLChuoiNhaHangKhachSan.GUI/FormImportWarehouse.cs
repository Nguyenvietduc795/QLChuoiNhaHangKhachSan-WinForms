using QLChuoiNhaHangKhachSan.BLL;
using QLChuoiNhaHangKhachSan.DTO;
using System;
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

        public FormImportWarehouse()
        {
            InitializeComponent();
            this.Load += FormImportWarehouse_Load;
            _unitBll = new UnitBLL(ConfigurationManager.ConnectionStrings["RHGROUP"].ConnectionString);
            _voucherBll = new WarehouseVoucherBLL(ConfigurationManager.ConnectionStrings["RHGROUP"].ConnectionString);
        }

        private void FormImportWarehouse_Load(object sender, EventArgs e)
        {
            InitCombos();
            InitGrid();
            txtMaPhieuimport.ReadOnly = true;
            txtMaPhieuimport.TabStop = false;
            txtMaPhieuimport.Text = _voucherBll.GetNextVoucherCode(true);   
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
                    item.Unit = f.SelectedItem.Unit;
                    item.Quantity = f.SelectedItem.Quantity;
                    item.UnitPrice = f.SelectedItem.UnitPrice;

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
                var created = _voucherBll.CreateVoucherWithDetails(voucher, details);
                txtMaPhieuimport.Text = created.VoucherCode;

                MessageBox.Show("Tạo phiếu nhập thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
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

            // tiêu chí trùng: IngredientID (hoặc IngredientCode)
            var exist = _items.FirstOrDefault(x => x.IngredientID == newItem.IngredientID);

            if (exist == null)
            {
                _items.Add(newItem);
                return;
            }

            // ✅ Nếu đã có -> cộng dồn số lượng
            exist.Quantity += newItem.Quantity;

            // RULE đơn giá: chọn 1 trong 2 cách dưới đây

            // Cách A: giữ nguyên đơn giá cũ (khuyên dùng để ổn định)
            // exist.UnitPrice = exist.UnitPrice;

            // Cách B: nếu lần mới khác giá -> cập nhật theo giá mới
            // exist.UnitPrice = newItem.UnitPrice;

            // Cách C: tính giá trung bình theo số lượng (chuẩn nghiệp vụ)
            // var totalOld = exist.Quantity * exist.UnitPrice;
            // var totalNew = newItem.Quantity * newItem.UnitPrice;
            // var qtySum = exist.Quantity + newItem.Quantity;
            // exist.UnitPrice = qtySum == 0 ? 0 : (totalOld + totalNew) / qtySum;

            DGVimportkho.Refresh();
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
