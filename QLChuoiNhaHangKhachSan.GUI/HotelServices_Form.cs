using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLChuoiNhaHangKhachSan.GUI
{
    public partial class HotelServices_Form : Form
    {
        private readonly List<ServiceItem> _allServices = new List<ServiceItem>();
        private readonly BindingList<ServiceSelection> _selected = new BindingList<ServiceSelection>();

        public IReadOnlyList<ServiceSelection> SelectedServices => _selected.ToList();

        public class ServiceSelection
        {
            public string Category { get; set; }
            public string Name { get; set; }
            public decimal UnitPrice { get; set; }
            public int Quantity { get; set; }
            public decimal Total { get; set; }
        }

        public HotelServices_Form()
        {
            InitializeComponent();
            ConfigureGrids();
            BindEvents();
        }

        private void HotelServices_Form_Load(object sender, EventArgs e)
        {
            LoadServices();
            RefreshSelectedGrid();
        }

        private void ConfigureGrids()
        {
            if (guna2DataGridView1.Columns.Contains("them"))
            {
                guna2DataGridView1.Columns["them"].Width = 60;
                guna2DataGridView1.Columns["them"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            if (guna2DataGridView2.Columns.Contains("xoa"))
            {
                guna2DataGridView2.Columns["xoa"].Width = 60;
                guna2DataGridView2.Columns["xoa"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            guna2DataGridView1.AllowUserToAddRows = false;
            guna2DataGridView2.AllowUserToAddRows = false;
        }

        private void BindEvents()
        {
            this.Load += HotelServices_Form_Load;
            guna2DataGridView1.CellContentClick += guna2DataGridView1_CellContentClick;
            guna2DataGridView2.CellContentClick += guna2DataGridView2_CellContentClick;
            txtTim.TextChanged += TxtTim_TextChanged;
            cbbLoaidichvu.SelectedIndexChanged += CbbLoaidichvu_SelectedIndexChanged;
            btnThoat.Click += BtnThoat_Click;
            btnLuu.Click += BtnLuu_Click;
            pictureBox1.Click += PictureBox1_Click;
        }

        private void LoadServices()
        {
            _allServices.Clear();

            var connStr = ConfigurationManager.ConnectionStrings["ConnStr"]?.ConnectionString;
            if (string.IsNullOrWhiteSpace(connStr))
            {
                MessageBox.Show("Không tìm thấy chuỗi kết nối 'ConnStr' trong cấu hình.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            const string query = "SELECT ServiceCategory, ServiceName, Price FROM dbo.Service";
            try
            {
                using (var conn = new SqlConnection(connStr))
                using (var cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var category = reader["ServiceCategory"]?.ToString()?.Trim();
                            var name = reader["ServiceName"]?.ToString()?.Trim();
                            decimal price = 0;
                            if (reader["Price"] != DBNull.Value)
                            {
                                decimal.TryParse(reader["Price"].ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out price);
                            }

                            if (!string.IsNullOrWhiteSpace(name))
                            {
                                _allServices.Add(new ServiceItem(category ?? string.Empty, name, price));
                            }
                        }
                    }
                }

                UpdateCategoryFilterItems();
                ApplyFilter();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải danh sách dịch vụ từ database.\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateCategoryFilterItems()
        {
            var categories = _allServices
                .Select(s => s.Category)
                .Where(c => !string.IsNullOrWhiteSpace(c))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(c => c)
                .ToList();

            cbbLoaidichvu.Items.Clear();
            cbbLoaidichvu.Items.Add("Tất cả");
            foreach (var c in categories)
            {
                cbbLoaidichvu.Items.Add(c);
            }
            cbbLoaidichvu.SelectedIndex = 0;
        }

        private void ApplyFilter()
        {
            string keyword = txtTim.Text?.Trim() ?? string.Empty;
            string selectedCategory = cbbLoaidichvu.SelectedItem as string;
            if (string.Equals(selectedCategory, "Tất cả", StringComparison.OrdinalIgnoreCase))
            {
                selectedCategory = string.Empty;
            }

            var filtered = _allServices.Where(s =>
                (string.IsNullOrEmpty(selectedCategory) || s.Category.Equals(selectedCategory, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrEmpty(keyword) || ContainsInsensitive(s.Name, keyword) || ContainsInsensitive(s.Category, keyword)))
                .ToList();

            guna2DataGridView1.Rows.Clear();
            foreach (var item in filtered)
            {
                int rowIndex = guna2DataGridView1.Rows.Add(item.Category, item.Name, item.Price.ToString("N0"), "+");
                guna2DataGridView1.Rows[rowIndex].Tag = item;
            }
        }

        private void RefreshSelectedGrid()
        {
            guna2DataGridView2.Rows.Clear();
            foreach (var s in _selected)
            {
                int rowIndex = guna2DataGridView2.Rows.Add(s.Name, s.Quantity, s.Total.ToString("N0"), "X");
                guna2DataGridView2.Rows[rowIndex].Tag = s;
            }
        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (guna2DataGridView1.Columns[e.ColumnIndex].Name != "them") return;

            var item = guna2DataGridView1.Rows[e.RowIndex].Tag as ServiceItem;
            if (item == null) return;

            var existing = _selected.FirstOrDefault(x => x.Name.Equals(item.Name, StringComparison.OrdinalIgnoreCase));
            if (existing != null)
            {
                existing.Quantity += 1;
                existing.Total = existing.Quantity * item.Price;
            }
            else
            {
                _selected.Add(new ServiceSelection { Category = item.Category, Name = item.Name, UnitPrice = item.Price, Quantity = 1, Total = item.Price });
            }
            RefreshSelectedGrid();
        }

        private void guna2DataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (guna2DataGridView2.Columns[e.ColumnIndex].Name != "xoa") return;

            var selected = guna2DataGridView2.Rows[e.RowIndex].Tag as ServiceSelection;
            if (selected == null) return;
            _selected.Remove(selected);
            RefreshSelectedGrid();
        }

        private void TxtTim_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void CbbLoaidichvu_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void BtnLuu_Click(object sender, EventArgs e)
        {
            if (_selected.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn ít nhất một dịch vụ.");
                return;
            }
            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void BtnThoat_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private bool ContainsInsensitive(string source, string keyword)
        {
            if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(keyword)) return false;
            return RemoveDiacritics(source).IndexOf(RemoveDiacritics(keyword), StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private string RemoveDiacritics(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            var normalized = text.Normalize(NormalizationForm.FormD);
            var builder = new StringBuilder();
            foreach (var c in normalized)
            {
                var uc = CharUnicodeInfo.GetUnicodeCategory(c);
                if (uc != UnicodeCategory.NonSpacingMark)
                    builder.Append(c);
            }
            return builder.ToString().Normalize(NormalizationForm.FormC);
        }

        private class ServiceItem
        {
            public ServiceItem(string category, string name, decimal price)
            {
                Category = category;
                Name = name;
                Price = price;
            }
            public string Category { get; }
            public string Name { get; }
            public decimal Price { get; }
        }
    }
}
