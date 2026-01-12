using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient; // 1. Thư viện SQL
using System.Configuration;  // 2. Thư viện đọc file Config
using System.Globalization;

namespace QLChuoiNhaHangKhachSan.GUI
{
    public partial class frmMenu : Form
    {
        // Định nghĩa event để thông báo khi một món ăn được thêm
        public event EventHandler<DishAddedEventArgs> DishAdded;

        public event Action<string> OrderConfirmed;

        private int? _customerId;
        private int? _guestCount;

        // --- 3. LẤY CHUỖI KẾT NỐI TỪ APP.CONFIG ---
        // Đảm bảo tên "QuanLyChuoiNhaHangKhachSan" khớp với file App.config của bạn
        string strKetNoi = ConfigurationManager.ConnectionStrings["QuanLyChuoiNhaHangKhachSan"].ConnectionString;
        private readonly List<FoodItem> _allFoods = new List<FoodItem>();

        public frmMenu()
        {
            InitializeComponent();

            this.Load += new System.EventHandler(this.Menu_Load);
            dgvDsMon.SelectionChanged += new System.EventHandler(this.UpdateAddButtonState);
            dgvDsMon.SelectionChanged += new System.EventHandler(this.dgvDsMon_SelectionChanged);
            btnRepair.Click += btnRepair_Click;
        }

        private void Menu_Load(object sender, EventArgs e)
        {
            // --- THIẾT LẬP DANH SÁCH LỌC LOẠI MÓN ĂN ---
            cboTilter.Items.Clear();
            cboTilter.Items.AddRange(new object[]
            {
                "Tất cả",
                "Khai vị",
                "Món chính",
                "Tráng miệng",
                "Đồ uống"
            });
            cboTilter.SelectedIndexChanged += cboTilter_SelectedIndexChanged;

            // --- THIẾT LẬP BẢNG DANH SÁCH MÓN ĂN (dgvDsMon) ---
            dgvDsMon.Columns.Clear();
            dgvDsMon.Rows.Clear();
            dgvDsMon.Columns.Add("MaMon", "Mã món");
            dgvDsMon.Columns.Add("TenMon", "Tên món");
            dgvDsMon.Columns.Add("Loai", "Loại");
            dgvDsMon.Columns.Add("DonGia", "Đơn giá");
            dgvDsMon.Columns.Add("TrangThai", "Trạng thái");
            dgvDsMon.Columns.Add("StatusID", "StatusID");

            dgvDsMon.Columns["StatusID"].Visible = false;
            dgvDsMon.Columns["TenMon"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvDsMon.Columns["DonGia"].DefaultCellStyle.Format = "N0";

            // --- THIẾT LẬP BẢNG MÓN ĐÃ GỌI (dgvDishMenu) ---
            dgvDishMenu.Columns.Clear();
            dgvDishMenu.Rows.Clear();
            dgvDishMenu.Columns.Add("MaMon", "Mã món");
            dgvDishMenu.Columns.Add("TenMon", "Tên món");
            dgvDishMenu.Columns.Add("SoLuong", "Số lượng");
            dgvDishMenu.Columns.Add("DonGia", "Đơn giá");
            dgvDishMenu.Columns.Add("ThanhTien", "Thành tiền");
            dgvDishMenu.Columns.Add("GhiChu", "Ghi chú");

            dgvDishMenu.Columns["TenMon"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvDishMenu.Columns["DonGia"].DefaultCellStyle.Format = "N0";
            dgvDishMenu.Columns["ThanhTien"].DefaultCellStyle.Format = "N0";

            bntAdd.Enabled = false;
            btnRepair.Enabled = false;
            btnComfirm.Enabled = false; // Xác nhận bị khóa lúc đầu

            txtNumber.Minimum = 1;
            txtNumber.Value = 1;
            txtTotal.Text = "0 VND";

            // Đặt giá trị mặc định cho combo lọc nếu chưa chọn
            if (cboTilter.SelectedIndex < 0 && cboTilter.Items.Count > 0)
            {
                cboTilter.SelectedIndex = 0; // Tất cả
            }

            LoadDataFromSQL();
        }

        private void LoadDataFromSQL()
        {
            try
            {
                _allFoods.Clear();
                using (SqlConnection conn = new SqlConnection(strKetNoi))
                {
                    conn.Open();
                    string query = @"
                        SELECT 
                            f.FoodID,
                            f.FoodName, 
                            ISNULL(c.CategoryName, N'Chưa phân loại') AS CategoryName, 
                            f.Price, 
                            f.StatusID,
                            ISNULL(fs.StatusName, N'Không xác định') AS StatusName
                        FROM Food f
                        LEFT JOIN FoodCategory c ON f.CategoryID = c.CategoryID
                        LEFT JOIN FoodStatus fs ON f.StatusID = fs.StatusID";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        _allFoods.Add(new FoodItem
                        {
                            MaMon = reader["FoodID"].ToString(),
                            TenMon = reader["FoodName"].ToString(),
                            Loai = reader["CategoryName"] != DBNull.Value ? reader["CategoryName"].ToString() : "Chưa phân loại",
                            DonGia = Convert.ToDecimal(reader["Price"]),
                            StatusId = reader["StatusID"] != DBNull.Value ? Convert.ToInt32(reader["StatusID"]) : 0,
                            TrangThai = reader["StatusName"].ToString()
                        });
                    }
                    reader.Close();
                }

                ApplyCategoryFilter(cboTilter.SelectedItem?.ToString());
                UpdateTotalDishTypes();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối CSDL: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ApplyCategoryFilter(string selectedCategory)
        {
            dgvDsMon.Rows.Clear();

            string targetCategory = Normalize(selectedCategory?.Trim() ?? string.Empty);
            string searchTerm = Normalize((txtSearch.Text ?? string.Empty).Trim());
            bool hasSearch = !string.IsNullOrEmpty(searchTerm);

            foreach (var food in _allFoods)
            {
                string loaiNorm = Normalize((food.Loai ?? string.Empty).Trim());

                // Lọc theo loại
                bool categoryMatched =
                    string.IsNullOrWhiteSpace(targetCategory) ||
                    targetCategory.Equals("Tat ca", StringComparison.OrdinalIgnoreCase) ||
                    loaiNorm.Equals(targetCategory, StringComparison.OrdinalIgnoreCase) ||
                    loaiNorm.IndexOf(targetCategory, StringComparison.OrdinalIgnoreCase) >= 0;

                if (!categoryMatched)
                {
                    continue;
                }

                // Lọc theo nội dung tìm kiếm (tên món hoặc mã món)
                if (hasSearch)
                {
                    string tenNorm = Normalize(food.TenMon ?? string.Empty);
                    string maNorm = Normalize(food.MaMon ?? string.Empty);

                    if (tenNorm.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) < 0 &&
                        maNorm.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) < 0)
                    {
                        continue;
                    }
                }

                dgvDsMon.Rows.Add(food.MaMon, food.TenMon, food.Loai, food.DonGia, food.TrangThai, food.StatusId);
            }

            dgvDsMon.ClearSelection();
            UpdateAddButtonState(null, null);
            UpdateTotalDishTypes();
        }

        private static string Normalize(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;
            string formD = input.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            foreach (var ch in formD)
            {
                var cat = CharUnicodeInfo.GetUnicodeCategory(ch);
                if (cat != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(ch);
                }
            }
            return sb.ToString().Normalize(NormalizationForm.FormC);
        }

        private void cboTilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyCategoryFilter(cboTilter.SelectedItem?.ToString());
        }

        private void dgvDsMon_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDsMon.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgvDsMon.SelectedRows[0];

                string tenMon = selectedRow.Cells["TenMon"].Value?.ToString() ?? string.Empty;
                string maMon = selectedRow.Cells["MaMon"].Value?.ToString() ?? string.Empty;

                txtDishName.Text = tenMon;
                txtItemCode.Text = maMon;

                // Reset số lượng và ghi chú khi chọn món khác
                txtNumber.Value = 1;
                txtNote.Text = string.Empty;
            }
        }

        private void UpdateAddButtonState(object sender, EventArgs e)
        {
            bool hasSelection = dgvDsMon.SelectedRows.Count > 0;
            bntAdd.Enabled = hasSelection;
            btnRepair.Enabled = hasSelection;
        }

        private void bntAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvDsMon.SelectedRows.Count == 0) return;

                var selectedRow = dgvDsMon.SelectedRows[0];

                // 1. Kiểm tra trạng thái (ưu tiên StatusID == 1)
                int statusId = 0;
                if (selectedRow.Cells["StatusID"].Value != null)
                {
                    int.TryParse(selectedRow.Cells["StatusID"].Value.ToString(), out statusId);
                }
                string statusName = Convert.ToString(selectedRow.Cells["TrangThai"].Value ?? string.Empty).Trim();
                string[] allowedNames = { "Đang bán", "Đang phục vụ", "Dang ban", "Dang phuc vu" };
                bool statusOk = statusId == 1 || allowedNames.Any(s => statusName.Equals(s, StringComparison.OrdinalIgnoreCase));

                if (!statusOk)
                {
                    MessageBox.Show("Món này hiện không có sẵn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 2. Lấy thông tin
                string maMon = selectedRow.Cells["MaMon"].Value.ToString();
                string tenMon = selectedRow.Cells["TenMon"].Value.ToString();
                decimal donGia = Convert.ToDecimal(selectedRow.Cells["DonGia"].Value);

                // Lấy số lượng từ txtNumber, mặc định 1 nếu trống/0
                int soLuong = (int)txtNumber.Value;
                if (soLuong <= 0) soLuong = 1;

                // 3. Kiểm tra trùng theo Mã món
                foreach (DataGridViewRow existingRow in dgvDishMenu.Rows)
                {
                    if (existingRow.Cells["MaMon"].Value != null && existingRow.Cells["MaMon"].Value.ToString() == maMon)
                    {
                        int currentQuantity = Convert.ToInt32(existingRow.Cells["SoLuong"].Value);
                        int newQuantity = currentQuantity + soLuong;
                        existingRow.Cells["SoLuong"].Value = newQuantity;
                        existingRow.Cells["ThanhTien"].Value = newQuantity * donGia;

                        UpdateTotalAmount();
                        return;
                    }
                }

                // 4. Thêm mới
                string ghiChu = txtNote.Text;
                decimal thanhTien = soLuong * donGia;
                dgvDishMenu.Rows.Add(maMon, tenMon, soLuong, donGia, thanhTien, ghiChu);

                // 5. Cập nhật tổng
                UpdateTotalAmount();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateTotalAmount()
        {
            decimal total = 0;
            foreach (DataGridViewRow row in dgvDishMenu.Rows)
            {
                if (row.Cells["ThanhTien"].Value != null)
                {
                    total += Convert.ToDecimal(row.Cells["ThanhTien"].Value);
                }
            }
            txtTotal.Text = total.ToString("N0");
            UpdateConfirmButtonState();
        }

        private void UpdateConfirmButtonState()
        {
            btnComfirm.Enabled = dgvDishMenu.Rows.Count > 0;
        }

        private void UpdateTotalDishTypes()
        {
            HashSet<string> uniqueDishes = new HashSet<string>();
            foreach (DataGridViewRow row in dgvDsMon.Rows)
            {
                if (row.Cells["TenMon"].Value != null)
                {
                    uniqueDishes.Add(row.Cells["TenMon"].Value.ToString());
                }
            }
            txtSum.Text = uniqueDishes.Count.ToString();
        }

        protected virtual void OnDishAdded(DishAddedEventArgs e)
        {
            DishAdded?.Invoke(this, e);
        }

        // Các event trống giữ nguyên
        private void dgvDsMon_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void guna2PictureBox1_Click(object sender, EventArgs e) { }
        private void guna2Button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvDishMenu.Rows.Count == 0)
                {
                    MessageBox.Show("No items to confirm.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Lấy TableID từ RestaurantTable dựa trên TableName
                int tableId = 0;
                string tableNumberText = txtTableNumber.Text?.Trim() ?? string.Empty;

                // Tính tổng tiền từ grid (phần bổ sung thêm)
                decimal addedSubTotal = 0m;
                foreach (DataGridViewRow row in dgvDishMenu.Rows)
                {
                    if (row.Cells["ThanhTien"].Value != null)
                    {
                        decimal line;
                        if (decimal.TryParse(row.Cells["ThanhTien"].Value.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out line))
                        {
                            addedSubTotal += line;
                        }
                    }
                }

                if (!_customerId.HasValue)
                {
                    MessageBox.Show("Không có thông tin khách hàng, vui lòng quay lại màn hình trước để nhập khách.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (SqlConnection conn = new SqlConnection(strKetNoi))
                {
                    conn.Open();

                    // Tìm TableID từ RestaurantTable theo TableName hoặc số bàn
                    using (var cmdTable = new SqlCommand(
                        @"SELECT TableID FROM RestaurantTable 
                          WHERE TableName = @TableName OR TableName = N'Bàn ' + @TableNumber", conn))
                    {
                        cmdTable.Parameters.AddWithValue("@TableName", tableNumberText);
                        cmdTable.Parameters.AddWithValue("@TableNumber", tableNumberText);
                        var result = cmdTable.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            tableId = Convert.ToInt32(result);
                        }
                    }

                    if (tableId == 0)
                    {
                        MessageBox.Show("Không tìm thấy bàn trong hệ thống.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    SqlTransaction tran = conn.BeginTransaction();

                    try
                    {
                        // 1) Tìm order Pending hiện có của bàn
                        int orderId = 0;
                        decimal currentSubTotal = 0m;
                        using (var cmdFindOrder = new SqlCommand(
                            @"SELECT TOP 1 OrderId, SubTotal FROM OrderTicket 
                              WHERE TableID = @TableID AND Status = N'Pending'
                              ORDER BY OrderId DESC", conn, tran))
                        {
                            cmdFindOrder.Parameters.AddWithValue("@TableID", tableId);
                            using (var reader = cmdFindOrder.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    orderId = Convert.ToInt32(reader["OrderId"]);
                                    currentSubTotal = reader["SubTotal"] == DBNull.Value ? 0m : Convert.ToDecimal(reader["SubTotal"]);
                                }
                            }
                        }

                        // 2) Nếu chưa có, tạo mới
                        if (orderId == 0)
                        {
                            var insertOrder = new SqlCommand(
                                @"INSERT INTO OrderTicket (TableID, CustomerID, GuestCount, DateCheckIn, Status, SubTotal, TotalAmount)
                                  VALUES (@TableID, @CustomerID, @GuestCount, GETDATE(), N'Pending', @SubTotal, @SubTotal);
                                  SELECT SCOPE_IDENTITY();", conn, tran);
                            insertOrder.Parameters.AddWithValue("@TableID", tableId);
                            insertOrder.Parameters.AddWithValue("@CustomerID", _customerId.Value);
                            insertOrder.Parameters.AddWithValue("@GuestCount", _guestCount ?? 1);
                            insertOrder.Parameters.AddWithValue("@SubTotal", addedSubTotal);

                            orderId = Convert.ToInt32(insertOrder.ExecuteScalar());
                            currentSubTotal = 0m; // vừa tạo, subtotal cũ = 0
                        }

                        // 3) Thêm món mới vào order (cộng dồn)
                        var insertItem = new SqlCommand(
                            @"INSERT INTO OrderTicketItem (OrderId, FoodID, Quantity, UnitPrice, Note)
                              VALUES (@OrderId, @FoodID, @Quantity, @UnitPrice, @Note);", conn, tran);

                        insertItem.Parameters.Add("@OrderId", SqlDbType.Int);
                        insertItem.Parameters.Add("@FoodID", SqlDbType.Int);
                        insertItem.Parameters.Add("@Quantity", SqlDbType.Int);
                        insertItem.Parameters.Add("@UnitPrice", SqlDbType.Decimal).Precision = 18;
                        insertItem.Parameters["@UnitPrice"].Scale = 0;
                        insertItem.Parameters.Add("@Note", SqlDbType.NVarChar, 200);

                        foreach (DataGridViewRow row in dgvDishMenu.Rows)
                        {
                            if (row.IsNewRow) continue;

                            int foodId = 0;
                            int.TryParse(Convert.ToString(row.Cells["MaMon"].Value), out foodId);

                            int quantity = Convert.ToInt32(row.Cells["SoLuong"].Value);
                            decimal unitPrice = Convert.ToDecimal(row.Cells["DonGia"].Value);
                            string note = Convert.ToString(row.Cells["GhiChu"].Value);

                            insertItem.Parameters["@OrderId"].Value = orderId;
                            insertItem.Parameters["@FoodID"].Value = foodId;
                            insertItem.Parameters["@Quantity"].Value = quantity;
                            insertItem.Parameters["@UnitPrice"].Value = unitPrice;
                            insertItem.Parameters["@Note"].Value = string.IsNullOrEmpty(note) ? (object)DBNull.Value : note;

                            insertItem.ExecuteNonQuery();
                        }

                        // 4) Cập nhật tổng tiền cộng dồn
                        decimal newSubTotal = currentSubTotal + addedSubTotal;
                        using (var cmdUpdateTotal = new SqlCommand(
                            "UPDATE OrderTicket SET SubTotal = @SubTotal, TotalAmount = @SubTotal WHERE OrderId = @OrderId", conn, tran))
                        {
                            cmdUpdateTotal.Parameters.AddWithValue("@SubTotal", newSubTotal);
                            cmdUpdateTotal.Parameters.AddWithValue("@OrderId", orderId);
                            cmdUpdateTotal.ExecuteNonQuery();
                        }

                        // 5) Cập nhật trạng thái bàn thành "Có khách" (StatusID = 2)
                        var updateTableStatus = new SqlCommand(
                            @"UPDATE RestaurantTable SET StatusID = 2 WHERE TableID = @TableID", conn, tran);
                        updateTableStatus.Parameters.AddWithValue("@TableID", tableId);
                        updateTableStatus.ExecuteNonQuery();

                        tran.Commit();
                        RaiseOrderConfirmed(); // thông báo bàn vừa gọi món
                        var res = MessageBox.Show("Order saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        // Khi người dùng bấm OK thì đóng form Menu
                        dgvDishMenu.Rows.Clear();
                        UpdateTotalAmount();
                        UpdateConfirmButtonState();
                        if (res == DialogResult.OK)
                        {
                            this.Close();
                        }
                    }
                    catch
                    {
                        tran.Rollback();
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while saving: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRepair_Click(object sender, EventArgs e)
        {
            if (dgvDishMenu.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn món cần xóa khỏi danh sách.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            foreach (DataGridViewRow row in dgvDishMenu.SelectedRows)
            {
                if (!row.IsNewRow)
                {
                    dgvDishMenu.Rows.Remove(row);
                }
            }

            UpdateTotalAmount();
            UpdateConfirmButtonState();
        }
        private void guna2Button1_Click(object sender, EventArgs e) { }
        private void guna2HtmlLabel3_Click(object sender, EventArgs e) { }
        private void txtItemCode_TextChanged(object sender, EventArgs e) { }
        private void txtDishName_TextChanged(object sender, EventArgs e) { }
        private void cboNumber_SelectedIndexChanged(object sender, EventArgs e) { }
        private void txtTotal_TextChanged(object sender, EventArgs e) { }
        private void pnlList_Paint(object sender, PaintEventArgs e) { }
        private void txtSearch_TextChanged(object sender, EventArgs e) { }
        private void dgvDishMenu_CellContentClick(object sender, DataGridViewCellEventArgs e) { }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            ApplyCategoryFilter(cboTilter.SelectedItem?.ToString());
        }

        public void SetTableNumber(string tableNumber)
        {
            txtTableNumber.Text = tableNumber ?? string.Empty;
        }

        public void SetCustomer(int customerId)
        {
            _customerId = customerId;
        }

        public void SetGuestCount(int? guestCount)
        {
            _guestCount = guestCount;
        }

        private void RaiseOrderConfirmed()
        {
            var tableNumber = txtTableNumber.Text?.Trim();
            if (string.IsNullOrEmpty(tableNumber))
            {
                return;
            }

            OrderConfirmed?.Invoke(tableNumber);
        }
    }

    public class DishAddedEventArgs : EventArgs
    {
        public string TenMon { get; }
        public decimal DonGia { get; }
        public int SoLuong { get; }

        public DishAddedEventArgs(string tenMon, decimal donGia, int soLuong)
        {
            TenMon = tenMon;
            DonGia = donGia;
            SoLuong = soLuong;
        }
    }

    // Thêm lớp FoodItem để lọc theo loại
    internal class FoodItem
    {
        public string MaMon { get; set; }
        public string TenMon { get; set; }
        public string Loai { get; set; }
        public decimal DonGia { get; set; }
        public int StatusId { get; set; }
        public string TrangThai { get; set; }
    }
}