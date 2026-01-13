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

        public int CurrentBookingID { get; set; }
        public string CurrentRoomID { get; set; } // Dùng cái này nếu chưa có BookingID
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
            // Nếu có mã phòng, hiển thị lên tiêu đề cho đẹp

            if (!string.IsNullOrEmpty(CurrentRoomID))
            {
                // 1. Đổi tên trên thanh tiêu đề cửa sổ
                this.Text = "Dịch Vụ - " + CurrentRoomID;

                // 2. Đổi tên cái Label to màu xanh (Thay 'label1' bằng tên thực tế trong Design của bạn)
                // Ví dụ: lblTieuDe.Text = ...
                // Nếu bạn dùng Label thường:
                if (Controls.Find("label1", true).FirstOrDefault() is Label lbl)
                {
                    lbl.Text = "DỊCH VỤ PHÒNG " + CurrentRoomID;
                }
                // Hoặc nếu bạn biết chắc tên biến (ví dụ lblHeader):
                // lblHeader.Text = "DỊCH VỤ PHÒNG " + CurrentRoomID;
            }


            LoadServices();
            RefreshSelectedGrid();
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

                            // Clamp inflated drink prices to 25,000 per can
                            if (!string.IsNullOrWhiteSpace(category) && category.IndexOf("drink", StringComparison.OrdinalIgnoreCase) >= 0 && price > 100000)
                            {
                                price = 25000m;
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
            guna2DataGridView2.CellEndEdit += guna2DataGridView2_CellEndEdit;
        }

        private void BindEvents()
        {
            this.Load += HotelServices_Form_Load;
            guna2DataGridView1.CellContentClick += guna2DataGridView1_CellContentClick;
            guna2DataGridView2.CellContentClick += guna2DataGridView2_CellContentClick;
            txtTim.TextChanged += TxtTim_TextChanged;
            cbbLoaidichvu.SelectedIndexChanged += CbbLoaidichvu_SelectedIndexChanged;
            btnThoat.Click += BtnThoat_Click;
            btnLuu.Click += btnLuu_Click;
            pictureBox1.Click += PictureBox1_Click;
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

        private void RefreshSelectedGrid()
        {
            guna2DataGridView2.Rows.Clear();
            foreach (var s in _selected)
            {
                int rowIndex = guna2DataGridView2.Rows.Add(s.Name, s.Quantity, s.UnitPrice.ToString("N0"), "X");
                guna2DataGridView2.Rows[rowIndex].Cells[3].Value = s.Total.ToString("N0");
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

        private void guna2DataGridView2_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var selected = guna2DataGridView2.Rows[e.RowIndex].Tag as ServiceSelection;
            if (selected == null) return;
            if (e.ColumnIndex == 1) // quantity column
            {
                int qty;
                if (!int.TryParse(Convert.ToString(guna2DataGridView2.Rows[e.RowIndex].Cells[1].Value), out qty) || qty <= 0)
                    qty = 1;
                selected.Quantity = qty;
                selected.Total = selected.Quantity * selected.UnitPrice;
                guna2DataGridView2.Rows[e.RowIndex].Cells[1].Value = selected.Quantity;
                guna2DataGridView2.Rows[e.RowIndex].Cells[2].Value = selected.UnitPrice.ToString("N0");
                guna2DataGridView2.Rows[e.RowIndex].Cells[3].Value = selected.Total.ToString("N0");
            }
        }

        private void SyncSelectionsFromGrid()
        {
            foreach (DataGridViewRow row in guna2DataGridView2.Rows)
            {
                var sel = row.Tag as ServiceSelection;
                if (sel == null) continue;
                int qty;
                if (!int.TryParse(Convert.ToString(row.Cells[1].Value), out qty) || qty <= 0) qty = 1;
                sel.Quantity = qty;
                sel.Total = sel.Quantity * sel.UnitPrice;
                row.Cells[1].Value = sel.Quantity;
                row.Cells[2].Value = sel.UnitPrice.ToString("N0");
                row.Cells[3].Value = sel.Total.ToString("N0");
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CurrentRoomID))
            {
                MessageBox.Show("Chưa nhận được Mã phòng! Vui lòng mở lại từ form chi tiết phòng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (_selected.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn ít nhất một dịch vụ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // sync any edited quantities before saving
            SyncSelectionsFromGrid();

            var connStr = ConfigurationManager.ConnectionStrings["ConnStr"]?.ConnectionString;
            if (string.IsNullOrWhiteSpace(connStr) || string.IsNullOrWhiteSpace(CurrentRoomID))
            {
                MessageBox.Show("Lỗi kết nối hoặc thiếu mã phòng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (var conn = new SqlConnection(connStr))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        // 1. Tìm BookingID đang hoạt động của phòng này (chính xác theo RoomID)
                        int bookingId = 0;
                        string findSql = @"SELECT TOP 1 b.BookingId 
                                   FROM dbo.HotelBookings b
                                   JOIN dbo.BookingDetails d ON b.BookingId = d.BookingId
                                   WHERE d.RoomId = @RoomID 
                                     AND b.Status NOT IN (N'Paid', N'Cancelled')
                                   ORDER BY b.CreatedDate DESC";

                        using (var cmd = new SqlCommand(findSql, conn, tran))
                        {
                            cmd.Parameters.AddWithValue("@RoomID", CurrentRoomID.Trim());
                            var res = cmd.ExecuteScalar();
                            if (res != null && res != DBNull.Value) bookingId = Convert.ToInt32(res);
                        }

                        if (bookingId == 0)
                        {
                            MessageBox.Show("Phòng này chưa có khách (chưa tạo Booking). Vui lòng Nhận phòng trước khi thêm dịch vụ!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            tran.Rollback();
                            return;
                        }

                        // 2. Tạo/Cập nhật bảng BookingServices với RoomID
                        using (var cmdTable = new SqlCommand(@"
                            IF OBJECT_ID('dbo.BookingServices','U') IS NULL
                            BEGIN
                                CREATE TABLE dbo.BookingServices (
                                    ID INT IDENTITY(1,1) PRIMARY KEY,
                                    BookingID INT,
                                    RoomID NVARCHAR(50),
                                    ServiceName NVARCHAR(100),
                                    Quantity INT,
                                    UnitPrice DECIMAL(18,2),
                                    TotalAmount DECIMAL(18,2)
                                );
                            END
                            ELSE IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.BookingServices') AND name = 'RoomID')
                            BEGIN
                                ALTER TABLE dbo.BookingServices ADD RoomID NVARCHAR(50);
                            END", conn, tran))
                        {
                            cmdTable.ExecuteNonQuery();
                        }

                        // 3. Lưu từng món vào Database (kèm RoomID)
                        var serviceDetails = new StringBuilder();
                        foreach (var item in _selected)
                        {
                            string sqlInsert = @"INSERT INTO dbo.BookingServices(BookingID, RoomID, ServiceName, Quantity, UnitPrice, TotalAmount)
                                         VALUES(@BID, @RoomID, @Name, @Qty, @Price, @Total)";
                            using (var cmdIns = new SqlCommand(sqlInsert, conn, tran))
                            {
                                cmdIns.Parameters.AddWithValue("@BID", bookingId);
                                cmdIns.Parameters.AddWithValue("@RoomID", CurrentRoomID.Trim());
                                cmdIns.Parameters.AddWithValue("@Name", item.Name);
                                cmdIns.Parameters.AddWithValue("@Qty", item.Quantity);
                                cmdIns.Parameters.AddWithValue("@Price", item.UnitPrice);
                                cmdIns.Parameters.AddWithValue("@Total", item.Total);
                                cmdIns.ExecuteNonQuery();
                            }
                            
                            // Tạo chi tiết thông báo
                            serviceDetails.AppendLine($"  • {item.Name} x{item.Quantity} = {item.Total:N0} VNĐ");
                        }

                        tran.Commit();
                        
                        // Hiển thị thông báo thành công với chi tiết
                        //
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        MessageBox.Show("Lỗi Database: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
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
