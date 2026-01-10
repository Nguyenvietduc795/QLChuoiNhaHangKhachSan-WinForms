using System;
using System.Configuration;
using System.Linq;
using System.Windows.Forms;
using QLChuoiNhaHangKhachSan.BLL;

public partial class FormInventory2 : Form
{
    private readonly WarehouseVoucherBLL _voucherBll;

    public FormInventory2()
    {
        InitializeComponent();
        _voucherBll = new WarehouseVoucherBLL(ConfigurationManager.ConnectionStrings["RHGROUP"].ConnectionString);

        btnNhapKhoInventory2.Click += btnNhapKhoInventory2_Click;
        btnXuatKhoInventory2.Click += btnXuatKhoInventory2_Click;
    }

    private void FormInventory2_Load(object sender, EventArgs e)
    {
        LoadVouchersFromDb();
    }

    private void btnNhapKhoInventory2_Click(object sender, EventArgs e)
    {
        using (var f = new FormImportWarehouse())
        {
            f.StartPosition = FormStartPosition.CenterParent;
            if (f.ShowDialog(this) == DialogResult.OK)
                LoadVouchersFromDb();
        }
    }

    private void btnXuatKhoInventory2_Click(object sender, EventArgs e)
    {
        using (var f = new FormExportWarehouse())
        {
            f.StartPosition = FormStartPosition.CenterParent;
            if (f.ShowDialog(this) == DialogResult.OK)
                LoadVouchersFromDb();
        }
    }

    private void LoadVouchersFromDb()
    {
        try
        {
            var data = _voucherBll.GetVouchersList()
                .Select(v => new
                {
                    MaPhieu = v.VoucherCode,
                    Ngay = v.CreatedAt.ToString("dd/MM/yyyy"),
                    LoaiKho = MapWarehouseType(v.WarehouseType),
                    DonVi = v.UnitCode,
                    TrangThai = v.Status
                })
                .ToList();

            DGdgvPhieu.AutoGenerateColumns = true;
            DGdgvPhieu.AllowUserToAddRows = false;
            DGdgvPhieu.RowHeadersVisible = false;
            DGdgvPhieu.DataSource = data;
        }
        catch (Exception ex)
        {
            MessageBox.Show("T?i danh sách phieu thiết bị: " + ex.Message,
                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private string MapWarehouseType(string warehouseType)
    {
        if (string.Equals(warehouseType, "INGREDIENT", StringComparison.OrdinalIgnoreCase)) return "Nguyên liệu";
        if (string.Equals(warehouseType, "EQUIPMENT", StringComparison.OrdinalIgnoreCase)) return "Thiết b?";
        return warehouseType;
    }
}