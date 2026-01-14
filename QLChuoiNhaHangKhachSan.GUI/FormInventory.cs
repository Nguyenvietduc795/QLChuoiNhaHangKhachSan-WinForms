using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Configuration;
using QLChuoiNhaHangKhachSan.BLL;
using System.Windows.Forms;

namespace QLChuoiNhaHangKhachSan.GUI
{
    public partial class FormInventory : Form
    {
        private readonly StockBLL _stockBll;
        private List<StockRow> _stockData;
        private string[] _activeStatusFilter;

        public FormInventory()
        {
            InitializeComponent();
            this.Load += FormInventory_Load;
            _stockBll = new StockBLL(ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString);
            txTimkiem.TextChanged += txTimkiem_TextChanged;
            btnfixitems.Click += btnupdateitems_Click;
            btnstopItems.Click += btnstopItems_Click;
            WireCardInteractions();

        }

        private void iconPictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void chipTotal_Click(object sender, EventArgs e)
        {

        }

        private void guna2HtmlLabel3_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tlpFilter_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cboDonVi_SelectedIndexChanged(object sender, EventArgs e)
        {

        }




        private void FormInventory_Load(object sender, EventArgs e)
        {
            // ===== Loại kho =====
            cboLoaiKho.Items.Clear();
            cboLoaiKho.Items.AddRange(new object[]
            {
                        "Kho Nguyên Liệu",
                        "Kho Thiết Bị",
                        "Tất cả"
            });
            cboLoaiKho.SelectedIndex = 2; // mặc định Tất cả

            // ===== Đơn vị (NH / KS) =====
            cboDonVi.Items.Clear();
            cboDonVi.Items.AddRange(new object[]
            {
                        "NH01",
                        "KS01",
                        "Tất cả"
            });
            cboDonVi.SelectedIndex = 2; // mặc định Tất cả

            DGdanhsachtonkho.RowTemplate.Height = 60;
            DGdanhsachtonkho.ColumnHeadersHeight = 48;
            DGCanhbaotonkho.RowTemplate.Height = 44;
            DGCanhbaotonkho.ColumnHeadersHeight = 48;
            LoadStockFromDb();  
            LoadFakeCanhBao();
        


        }


        private void cboLoaiKho_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadStockFromDb();
        }

        private void lbDanhSachTonKhotext_Click(object sender, EventArgs e)
        {

        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void LoadStockFromDb()
        {
            try
            {
                var warehouseType = GetSelectedWarehouseType();
                var keyword = GetSearchKeyword();
                var dt = _stockBll.GetStockList(warehouseType, keyword);

                UpdateSummaryLabels();

                _stockData = dt.AsEnumerable().Select(r =>
                {
                    var rowType = GetEffectiveWarehouseType(warehouseType, r.GetColumnValue("ItemType"));
                    return new StockRow
                    {
                        Ma = r.GetColumnValue("ItemCode"),
                        Ten = FormatItemName(r.GetColumnValue("ItemCode"), r.GetColumnValue("ItemName")),
                        DonVi = r.GetColumnValue("Unit"),
                        Ton = GetRowQuantity(r),
                        MiniStock = GetMiniStockLabel(r),
                        GiaNhap = r.GetColumnValueDecimal("DefaultPrice"),
                        NgayNhap = r.GetColumnValueDate("LastUpdated"),
                        DonViQuanLy = ResolveUnitCode(rowType, r.GetColumnValue("ItemType")),
                        TrangThai = ResolveStatus(r),
                        LoaiKho = rowType
                    };
                })
                .OrderBy(x => x.Ma)
                .ToList();

                EnsureStockColumns();
                ApplyStatusFilter();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Tải danh sách tồn kho thất bại: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EnsureStockColumns()
        {
            if (DGdanhsachtonkho.Columns.Count > 0) return;

            DGdanhsachtonkho.Columns.Clear();

            DGdanhsachtonkho.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Ma",
                HeaderText = "Mã",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 12
            });

            DGdanhsachtonkho.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Ten",
                HeaderText = "Tên",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 18
            });

            DGdanhsachtonkho.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "DonVi",
                HeaderText = "Đơn vị",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 10
            });

            DGdanhsachtonkho.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Ton",
                HeaderText = "Tồn",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 10,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            DGdanhsachtonkho.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "MiniStock",
                HeaderText = "Ngưỡng",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 12,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            DGdanhsachtonkho.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "GiaNhap",
                HeaderText = "Giá nhập",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 12,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" }
            });

            DGdanhsachtonkho.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "NgayNhap",
                HeaderText = "Ngày nhập",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 14,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" }
            });

            DGdanhsachtonkho.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "DonViQuanLy",
                HeaderText = "NH/KS",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 12
            });

            DGdanhsachtonkho.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TrangThai",
                HeaderText = "Trạng thái",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 12,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            DGdanhsachtonkho.DefaultCellStyle.SelectionBackColor = Color.White;
            DGdanhsachtonkho.DefaultCellStyle.SelectionForeColor = Color.Black;
            DGdanhsachtonkho.AutoGenerateColumns = false;
            DGdanhsachtonkho.AllowUserToAddRows = false;
            DGdanhsachtonkho.RowHeadersVisible = false;
        }

        private string GetSelectedWarehouseType()
        {
            var selected = cboLoaiKho.SelectedItem as string;
            if (string.Equals(selected, "Kho Thiết Bị", StringComparison.OrdinalIgnoreCase)) return "EQUIPMENT";
            if (string.Equals(selected, "Kho Nguyên Liệu", StringComparison.OrdinalIgnoreCase)) return "INGREDIENT";
            return null; // tất cả
        }

        private string ResolveUnitCode(string selectedWarehouseType, string itemType)
        {
            var type = selectedWarehouseType ?? itemType;
            if (string.Equals(type, "EQUIPMENT", StringComparison.OrdinalIgnoreCase)) return "KS01";
            if (string.Equals(type, "INGREDIENT", StringComparison.OrdinalIgnoreCase)) return "NH01";
            return string.Empty;
        }

        private string GetEffectiveWarehouseType(string selectedType, string rowType)
        {
            if (!string.IsNullOrWhiteSpace(selectedType)) return selectedType;
            if (!string.IsNullOrWhiteSpace(rowType)) return rowType;
            return null;
        }

        private string GetMiniStockLabel(DataRow row)
        {
            var label = row.GetColumnValue("MiniStock")
                        ?? row.GetColumnValue("MiniStockName")
                        ?? row.GetColumnValue("MinStockName")
                        ?? row.GetColumnValue("MinStock");

            if (string.IsNullOrWhiteSpace(label)) return string.Empty;
            return label.Trim();
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

        private string NormalizeStatus(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return string.Empty;

            var trimmed = raw.Trim();
            var lower = trimmed.ToLowerInvariant();
            var normalized = RemoveDiacritics(trimmed).ToLowerInvariant().Replace("?", string.Empty);
            var collapsed = new string(normalized.Where(char.IsLetterOrDigit).ToArray());

            if (ContainsToken(lower, "ng?ng", "ngừng", "ngưng", "ngung") ||
                ContainsToken(normalized, "ngung", "ngungsudung") ||
                collapsed.Contains("ngungsudung") || lower.Contains("inactive") || lower.Contains("stop"))
            {
                return "Ngừng sử dụng";
            }

            if (ContainsToken(lower, "h?t", "hết", "het", "out", "empty") ||
                ContainsToken(normalized, "hethang") ||
                collapsed.Contains("hethang"))
            {
                return "Hết hàng";
            }

            if (ContainsToken(lower, "thi?u", "thiếu", "thieu", "low", "warning") ||
                ContainsToken(normalized, "thieu") ||
                collapsed.Contains("thieu"))
            {
                return "Thiếu";
            }

            if (ContainsToken(lower, "?n ??nh", "ổn", "on dinh", "ondinh", "tot") ||
                ContainsToken(normalized, "ondinh", "tot") ||
                collapsed.Contains("ondinh") || lower.Contains("stable") || lower.Contains("healthy"))
            {
                return "Ổn định";
            }

            return string.Empty;
        }

        private string RemoveDiacritics(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;

            var normalized = text.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder(normalized.Length);
            foreach (var c in normalized)
            {
                var category = CharUnicodeInfo.GetUnicodeCategory(c);
                if (category != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(c);
                }
            }

            return sb.ToString().Normalize(NormalizationForm.FormC);
        }

        private bool ContainsToken(string source, params string[] tokens)
        {
            if (string.IsNullOrEmpty(source) || tokens == null) return false;
            foreach (var token in tokens)
            {
                if (string.IsNullOrEmpty(token)) continue;
                if (source.Contains(token)) return true;
            }
            return false;
        }

        private string ResolveStatus(DataRow row)
        {
            bool isActive = true;
            if (row.Table.Columns.Contains("IsActive") && row["IsActive"] != DBNull.Value)
            {
                bool.TryParse(row["IsActive"].ToString(), out isActive);
            }

            if (!isActive)
                return "Ngừng sử dụng";

            var normalized = NormalizeStatus(row.GetColumnValue("StatusText"));
            if (!string.IsNullOrEmpty(normalized))
                return normalized;

            var quantity = GetRowQuantity(row);
            var threshold = GetMiniStockThreshold(row);

            if (quantity <= 0)
                return "Hết hàng";

            if (threshold.HasValue && quantity <= threshold.Value)
                return "Thiếu";

            return "Ổn định";
        }

        private void UpdateSummaryLabels()
        {
            try
            {
                var total = _stockBll.GetTotalStockQuantity();
                var low = _stockBll.GetLowStockCount();
                var stable = _stockBll.GetStableStockCount();

                lblTotal.Text = total.ToString("N0", CultureInfo.InvariantCulture);
                lbStockLow.Text = low.ToString("N0", CultureInfo.InvariantCulture);
                lbHealthyStock.Text = stable.ToString("N0", CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                lblTotal.Text = "0";
                lbStockLow.Text = "0";
                lbHealthyStock.Text = "0";
                MessageBox.Show("Không thể tải KPI tồn kho: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int GetRowQuantity(DataRow row)
        {
            var candidates = new[] { "StockQuantity", "Quantity", "TonKho", "CurrentStock", "OnHandQuantity" };
            foreach (var column in candidates)
            {
                if (row.Table.Columns.Contains(column) && row[column] != DBNull.Value)
                {
                    try
                    {
                        return Convert.ToInt32(row[column]);
                    }
                    catch
                    {
                        // ignore and try next candidate
                    }
                }
            }

            return 0;
        }

        private int? GetMiniStockThreshold(DataRow row)
        {
            var candidates = new[] { "MiniStock", "MiniStockValue", "MinStock", "MinStockQty" };
            foreach (var column in candidates)
            {
                if (!row.Table.Columns.Contains(column)) continue;
                var raw = row[column];
                if (raw == DBNull.Value) continue;
                if (raw is int i) return i;
                if (raw is decimal dec) return (int)Math.Round(dec, MidpointRounding.AwayFromZero);
                var text = raw.ToString();
                if (string.IsNullOrWhiteSpace(text)) continue;
                var digits = Regex.Match(text, "-?\\d+");
                if (digits.Success && int.TryParse(digits.Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsed))
                {
                    return parsed;
                }
            }

            return null;
        }

        private string GetSearchKeyword()
        {
            return string.IsNullOrWhiteSpace(txTimkiem.Text) ? null : txTimkiem.Text.Trim();
        }

        private void txTimkiem_TextChanged(object sender, EventArgs e)
        {
            LoadStockFromDb();
        }

        private void TbDanhsachtonkho_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2DataGridView4_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void lbdanhsachtonkho_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {

        }

        private void LoadFakeCanhBao()
        {
            DataTable dt = new DataTable();

            // 2 cột thôi: trái (2 dòng text), phải (badge số)
            dt.Columns.Add("Mặt hàng");          // "NL001 · Cà phê hạt\nNH01 · Ngưỡng: 10"
            dt.Columns.Add("Lượng Tồn", typeof(int));

            dt.Rows.Add("NL001 · Hạt cà phê \nNH01 · Ngưỡng: 10", 8);
            dt.Rows.Add("TB011 · Máy hút bụi\nKS01 · Ngưỡng: 5", 3);
            dt.Rows.Add("NL009 · Sữa tươi\nNH01 · Ngưỡng: 20", 12);

            DGCanhbaotonkho.AutoGenerateColumns = true;
            DGCanhbaotonkho.DataSource = dt;



            // Chỉ còn 2 cột, set tỷ lệ giống UI
           // DGCanhbaotonkho.Columns["Info"].FillWeight = 85;
            //DGCanhbaotonkho.Columns["Badge"].FillWeight = 15;

            // Format cột Info: cho xuống dòng
            DGCanhbaotonkho.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            DGCanhbaotonkho.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            DGCanhbaotonkho.RowTemplate.MinimumHeight = 52;

            // Canh badge giữa
            //DGCanhbaotonkho.Columns["Badge"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Font đẹp hơn
            DGCanhbaotonkho.DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            DGCanhbaotonkho.DefaultCellStyle.SelectionBackColor = Color.White;
            DGCanhbaotonkho.DefaultCellStyle.SelectionForeColor = Color.Black;

        }

        private void guna2Panel3_Paint(object sender, PaintEventArgs e)
        {

        }
        private Panel overlay;

        private void ShowOverlayOnLbDSTonkho()
        {
            if (overlay != null) return;

            overlay = new Panel();
            overlay.Dock = DockStyle.Fill;
            overlay.BackColor = Color.FromArgb(60, 17, 24, 39);

            lbDSTonkho.Controls.Add(overlay);
            overlay.BringToFront();
        }

        private void HideOverlay()
        {
            if (overlay == null) return;

            lbDSTonkho.Controls.Remove(overlay);
            overlay.Dispose();
            overlay = null;
        }



        private void btnadditems_Click(object sender, EventArgs e)
        {
            ShowOverlayOnLbDSTonkho();

            using (var f = new FormAddMatHang())
            {
                f.StartPosition = FormStartPosition.CenterParent;
                var result = f.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    LoadStockFromDb();
                }
            }

            HideOverlay();
        }

        private void btnupdateitems_Click(object sender, EventArgs e)
        {
            ShowOverlayOnLbDSTonkho();

            var selectedRow = DGdanhsachtonkho.CurrentRow;
            var selectedCode = (selectedRow?.DataBoundItem as StockRow)?.Ma;

            if (string.IsNullOrWhiteSpace(selectedCode))
            {
                MessageBox.Show("Vui lòng chọn một mặt hàng trước khi sửa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                HideOverlay();
                return;
            }

            using (var f = new FormUpdateItems())
            {
                f.StartPosition = FormStartPosition.CenterParent;
                f.PreselectWarehouse(GetSelectedWarehouseType());
                f.PreselectItemCode(selectedCode);

                if (f.ShowDialog(this) == DialogResult.OK)
                {
                    LoadStockFromDb();
                }
            }

            HideOverlay();
        }

        private void btnstopItems_Click(object sender, EventArgs e)
        {
            ShowOverlayOnLbDSTonkho();
            var selectedRow = DGdanhsachtonkho.CurrentRow;
            var selectedData = selectedRow?.DataBoundItem as StockRow;
            var selectedCode = selectedData?.Ma;
            var selectedType = selectedData?.LoaiKho ?? GetSelectedWarehouseType();

            if (string.IsNullOrWhiteSpace(selectedCode))
            {
                MessageBox.Show("Vui lòng chọn một mặt hàng trước khi ngừng sử dụng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                HideOverlay();
                return;
            }

            using (var f = new FormStopItems())
            {
                f.StartPosition = FormStartPosition.CenterParent;
                if (!string.IsNullOrWhiteSpace(selectedType))
                {
                    f.PreselectWarehouse(selectedType);
                }
                f.PreselectItemCode(selectedCode);

                if (f.ShowDialog(this) == DialogResult.OK)
                {
                    LoadStockFromDb();
                }
            }
            HideOverlay();
        }

        private void WireCardInteractions()
        {
            AttachCardHandler(CardSlowTotal, CardSlowTotal_Click);
            AttachCardHandler(CardHealthyTotal, CardHealthyTotal_Click);
            AttachCardHandler(cardTotal, CardTotal_Click);
        }

        private void AttachCardHandler(Control control, EventHandler handler)
        {
            if (control == null || handler == null) return;
            control.Click -= handler; // avoid duplicate hooks
            control.Click += handler;
            foreach (Control child in control.Controls)
            {
                AttachCardHandler(child, handler);
            }
        }

        private void CardSlowTotal_Click(object sender, EventArgs e)
        {
            SetStatusFilter("Thiếu", "Hết hàng");
        }

        private void CardHealthyTotal_Click(object sender, EventArgs e)
        {
            SetStatusFilter("Ổn định");
        }

        private void CardTotal_Click(object sender, EventArgs e)
        {
            SetStatusFilter();
        }

        private void SetStatusFilter(params string[] statuses)
        {
            _activeStatusFilter = statuses != null && statuses.Length > 0 ? statuses : null;
            ApplyStatusFilter();
        }

        private void ApplyStatusFilter()
        {
            if (_stockData == null)
            {
                DGdanhsachtonkho.DataSource = null;
                return;
            }

            IEnumerable<StockRow> data = _stockData;
            if (_activeStatusFilter != null && _activeStatusFilter.Length > 0)
            {
                data = data.Where(row => !string.IsNullOrWhiteSpace(row.TrangThai)
                    && _activeStatusFilter.Any(status => string.Equals(row.TrangThai, status, StringComparison.OrdinalIgnoreCase)));
            }

            BindStockGrid(data);
        }

        private void BindStockGrid(IEnumerable<StockRow> rows)
        {
            DGdanhsachtonkho.DataSource = rows?.ToList();
        }

        private void lbDSTonkho_Paint(object sender, PaintEventArgs e)
        {

        }

        private void CardSlowTotal_Click(object sender, PaintEventArgs e)
        {

        }

        private void CardHealthyTotal_Click(object sender, PaintEventArgs e)
        {

        }
    }

    internal class StockRow
    {
        public string Ma { get; set; }
        public string Ten { get; set; }
        public string DonVi { get; set; }
        public int Ton { get; set; }
        public string MiniStock { get; set; }
        public decimal GiaNhap { get; set; }
        public DateTime? NgayNhap { get; set; }
        public string DonViQuanLy { get; set; }
        public string TrangThai { get; set; }
        public string LoaiKho { get; set; }
    }

    internal static class DataRowExtensions
    {
        public static string GetColumnValue(this DataRow row, string columnName)
        {
            return row.Table.Columns.Contains(columnName) && row[columnName] != DBNull.Value
                ? row[columnName].ToString()
                : null;
        }

        public static int GetColumnValueInt(this DataRow row, string columnName)
        {
            return row.Table.Columns.Contains(columnName) && row[columnName] != DBNull.Value
                ? Convert.ToInt32(row[columnName])
                : 0;
        }

        public static decimal GetColumnValueDecimal(this DataRow row, string columnName)
        {
            return row.Table.Columns.Contains(columnName) && row[columnName] != DBNull.Value
                ? Convert.ToDecimal(row[columnName])
                : 0m;
        }

        public static DateTime? GetColumnValueDate(this DataRow row, string columnName)
        {
            return row.Table.Columns.Contains(columnName) && row[columnName] != DBNull.Value
                ? (DateTime?)Convert.ToDateTime(row[columnName])
                : null;
        }
    }
}
