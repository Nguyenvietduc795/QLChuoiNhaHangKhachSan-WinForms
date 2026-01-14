using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace QLChuoiNhaHangKhachSan.GUI
{
    public partial class frmRestaurant : Form
    {
        private Button currentSelectedTable;
        private int? currentCustomerId;
        private readonly string strKetNoi = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;

        public frmRestaurant()
        {
            InitializeComponent();

            // Gắn sự kiện kiểm tra thông tin đặt bàn
            txtClient.TextChanged += BookingInfoChanged;
            txtEmail.TextChanged += BookingInfoChanged;
            txtQuantity.TextChanged += BookingInfoChanged;
            txtNumberPhone.TextChanged += BookingInfoChanged;
            dtpDay.ValueChanged += BookingInfoChanged;
            dtpTime.ValueChanged += BookingInfoChanged;
            dtpTime.MouseDown += dtpTime_Click;

            btnConfirm.Click += btnConfirmLookup_Click;
        }

        // Trạng thái bàn
        public enum TableStatus
        {
            Trong,      // Trống
            CoKhach,    // Có khách
            DaDat       // Đã đặt
        }

        // Cập nhật trạng thái nút
        private void UpdateButtonState(TableStatus status)
        {
            if (currentSelectedTable == null)
            {
                ResetButtonState();
                return;
            }

            switch (status)
            {
                case TableStatus.Trong:
                    SetButtonState(btnTableChoose, true);
                    SetButtonState(btnOrder, true);
                    SetButtonState(btnPay, false);
                    SetButtonState(btnCancel, false);
                    break;
                case TableStatus.DaDat:
                    SetButtonState(btnTableChoose, false);
                    SetButtonState(btnOrder, true);
                    SetButtonState(btnPay, false);
                    SetButtonState(btnCancel, true);
                    break;
                case TableStatus.CoKhach:
                    SetButtonState(btnTableChoose, false);
                    SetButtonState(btnOrder, true);
                    SetButtonState(btnPay, true);
                    SetButtonState(btnCancel, false);
                    break;
            }

            ApplyBookingGuard();
        }

        private void ResetButtonState()
        {
            SetButtonState(btnTableChoose, false);
            SetButtonState(btnCancel, false);
            SetButtonState(btnOrder, false);
            SetButtonState(btnPay, false);
            txtTableNumber.Text = string.Empty;
        }

        private void SetButtonState(Control btn, bool enable)
        {
            btn.Enabled = enable;
            btn.BackColor = enable
                ? (btn.Name == "btnTableChoose" ? Color.Salmon
                  : btn.Name == "btnOrder" ? Color.ForestGreen
                  : btn.Name == "btnPay" ? Color.Blue
                  : btn.Name == "btnCancel" ? Color.Red
                  : btn.BackColor)
                : Color.Gray;
        }

        private bool HasBookingInfo()
        {
            return !string.IsNullOrWhiteSpace(txtClient.Text)
                   && !string.IsNullOrWhiteSpace(txtEmail.Text)
                   && !string.IsNullOrWhiteSpace(txtNumberPhone.Text)
                   && !string.IsNullOrWhiteSpace(txtQuantity.Text)
                   && !string.IsNullOrWhiteSpace(txtTableNumber.Text)
                   && IsValidPhone(txtNumberPhone.Text)
                   && IsValidGmail(txtEmail.Text);
        }

        private void ApplyBookingGuard()
        {
            if (currentSelectedTable == null)
            {
                SetButtonState(btnTableChoose, false);
            }
        }

        private void btnConfirmLookup_Click(object sender, EventArgs e)
        {
            using (var confirm = new ConfirmTable())
            {
                confirm.StartPosition = FormStartPosition.CenterParent;
                if (confirm.ShowDialog(this) == DialogResult.OK && confirm.IsConfirmed)
                {
                    int tableId = confirm.FoundTableId;
                    string tableName = confirm.FoundTableName;

                    if (string.IsNullOrWhiteSpace(tableName) && tableId != 0)
                    {
                        // Lấy tên bàn từ DB nếu cần
                        try
                        {
                            using (var conn = new SqlConnection(strKetNoi))
                            using (var cmd = new SqlCommand("SELECT TableName FROM RestaurantTable WHERE TableID = @TableID", conn))
                            {
                                cmd.Parameters.AddWithValue("@TableID", tableId);
                                conn.Open();
                                var result = cmd.ExecuteScalar();
                                if (result != null && result != DBNull.Value)
                                {
                                    tableName = Convert.ToString(result);
                                }
                            }
                        }
                        catch { }
                    }

                    if (string.IsNullOrWhiteSpace(tableName))
                    {
                        MessageBox.Show("Không tìm thấy tên bàn để chọn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    var btn = FindTableButton(tableName);
                    if (btn == null)
                    {
                        // Thử với dạng "Bàn X"
                        btn = FindTableButton("Bàn " + tableName);
                    }

                    if (btn != null)
                    {
                        Table_Click(btn, EventArgs.Empty);
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy bàn trên giao diện.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private void BookingInfoChanged(object sender, EventArgs e)
        {
            if (currentSelectedTable == null)
            {
                SetButtonState(btnTableChoose, false);
                return;
            }

            TableStatus status = TableStatus.Trong;
            if (currentSelectedTable.Tag != null)
            {
                status = (TableStatus)currentSelectedTable.Tag;
            }

            UpdateButtonState(status);
        }

        private void Table_Click(object sender, EventArgs e)
        {
            if (currentSelectedTable != null)
            {
                currentSelectedTable.FlatAppearance.BorderSize = 0;
            }

            Button btn = (Button)sender;
            currentSelectedTable = btn;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderColor = Color.Black;
            btn.FlatAppearance.BorderSize = 3;

            // Hiển thị số bàn (loại bỏ khoảng trắng) và trích xuất số bàn dù button có chữ "Bàn"
            string rawTableText = (btn.Text ?? string.Empty).Trim();
            txtTableNumber.Text = rawTableText;

            int tableNumber;
            // Cho phép dạng "Bàn 14" hoặc "14"
            var digits = System.Text.RegularExpressions.Regex.Match(rawTableText, "\\d+");
            if (digits.Success)
            {
                int.TryParse(digits.Value, out tableNumber);
            }
            else
            {
                tableNumber = 0;
            }

            if (tableNumber == 0)
            {
                dgvDishList.Rows.Clear();
                ClearCustomerFields();
                UpdateStatusText(TableStatus.Trong);
                UpdateButtonState(TableStatus.Trong);
                return;
            }

            // Lấy trạng thái và TableID từ DB (ưu tiên trạng thái thật)
            int tableId = 0;
            TableStatus status = TableStatus.Trong;
            try
            {
                using (var conn = new SqlConnection(strKetNoi))
                {
                    conn.Open();
                    using (var cmdTable = new SqlCommand(
                        @"SELECT rt.TableID, ts.StatusName
                          FROM RestaurantTable rt
                          INNER JOIN TableStatus ts ON rt.StatusID = ts.StatusID
                          WHERE rt.TableName = @TableName OR rt.TableName = N'Bàn ' + @TableNumber", conn))
                    {
                        cmdTable.Parameters.AddWithValue("@TableName", tableNumber.ToString());
                        cmdTable.Parameters.AddWithValue("@TableNumber", tableNumber.ToString());
                        using (var reader = cmdTable.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                tableId = Convert.ToInt32(reader[0]);
                                status = MapStatus(reader.GetString(1));
                            }
                        }
                    }
                }
            }
            catch
            {
                // fallback: dùng Tag nếu có
                if (btn.Tag != null) status = (TableStatus)btn.Tag;
            }

            // Cập nhật Tag để lần sau dùng luôn
            btn.Tag = status;

            UpdateStatusText(status);
            UpdateButtonState(status);

            if (status == TableStatus.Trong || tableId == 0)
            {
                dgvDishList.Rows.Clear();
                txtTotalAmount.Text = "0";
                ClearCustomerFields();
                return;
            }

            // Bàn có khách / đã đặt: hiển thị món và thông tin khách
            LoadDishListByTableId(tableId);
            LoadCustomerInfoByTable(tableId);
        }

        private void LoadDishListByTableId(int tableId)
        {
            dgvDishList.Rows.Clear();
            txtTotalAmount.Text = "0";

            try
            {
                using (SqlConnection conn = new SqlConnection(strKetNoi))
                {
                    conn.Open();

                    // Lấy OrderId mới nhất của bàn (chỉ lấy đơn đang Pending)
                    int? orderId = null;
                    using (var cmdOrder = new SqlCommand(
                        @"SELECT TOP 1 OrderId 
                          FROM OrderTicket 
                          WHERE TableID = @TableID AND Status = N'Pending'
                          ORDER BY OrderId DESC", conn))
                    {
                        cmdOrder.Parameters.AddWithValue("@TableID", tableId);
                        var result = cmdOrder.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            orderId = Convert.ToInt32(result, CultureInfo.InvariantCulture);
                        }
                    }

                    if (!orderId.HasValue)
                    {
                        return; // không có phiếu nào cho bàn này
                    }

                    decimal total = 0m;

                    // Join với Food để lấy FoodName vì OrderTicketItem không còn lưu FoodName
                    using (var cmdItems = new SqlCommand(
                        @"SELECT f.FoodName, oti.Quantity, oti.UnitPrice, oti.LineTotal
                          FROM OrderTicketItem oti
                          INNER JOIN Food f ON oti.FoodID = f.FoodID
                          WHERE oti.OrderId = @OrderId", conn))
                    {
                        cmdItems.Parameters.AddWithValue("@OrderId", orderId.Value);
                        using (var reader = cmdItems.ExecuteReader())
                        {
                            int stt = 1;
                            while (reader.Read())
                            {
                                string foodName = reader["FoodName"].ToString();
                                int quantity = Convert.ToInt32(reader["Quantity"], CultureInfo.InvariantCulture);
                                decimal unitPrice = Convert.ToDecimal(reader["UnitPrice"], CultureInfo.InvariantCulture);
                                decimal lineTotal = Convert.ToDecimal(reader["LineTotal"], CultureInfo.InvariantCulture);

                                total += lineTotal;

                                dgvDishList.Rows.Add(
                                    stt,
                                    foodName,
                                    unitPrice.ToString("N0"),
                                    quantity,
                                    lineTotal.ToString("N0"));
                                stt++;
                            }
                        }
                    }

                    txtTotalAmount.Text = total.ToString("N0");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách món: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGoiMon_Click(object sender, EventArgs e)
        {
            if (!HasBookingInfo())
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin (tên, email, số điện thoại, số người và số bàn).", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!IsValidName(txtClient.Text))
            {
                MessageBox.Show("Tên khách hàng không hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!IsValidPhone(txtNumberPhone.Text))
            {
                MessageBox.Show("Số điện thoại không hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!IsValidGmail(txtEmail.Text))
            {
                MessageBox.Show("Email phải có định dạng @gmail.com.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int guestCount;
            if (!int.TryParse(txtQuantity.Text, NumberStyles.Integer, CultureInfo.InvariantCulture, out guestCount) || guestCount <= 0)
            {
                MessageBox.Show("Số người phải là số nguyên dương.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Lưu khách hàng trước khi gọi món (cho phép thiếu Phone/Email, sẽ để NULL)
            try
            {
                using (var conn = new SqlConnection(strKetNoi))
                {
                    conn.Open();
                    currentCustomerId = FindOrCreateCustomer(conn,
                        txtClient.Text.Trim(),
                        txtNumberPhone.Text.Trim(),
                        txtEmail.Text.Trim(),
                        "NH");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lưu khách hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            frmMenu menu = new frmMenu();
            menu.SetTableNumber(txtTableNumber.Text);
            if (currentCustomerId.HasValue)
            {
                menu.SetCustomer(currentCustomerId.Value);
            }
            menu.SetGuestCount(guestCount);
            menu.OrderConfirmed += OnMenuOrderConfirmed;
            menu.ShowDialog();
        }

        private void OnMenuOrderConfirmed(string tableNumber)
        {
            var btn = FindTableButton(tableNumber);
            if (btn == null) return;

            btn.Tag = TableStatus.CoKhach;
            btn.BackColor = Color.MediumSeaGreen;
            if (currentSelectedTable != btn)
            {
                currentSelectedTable = btn;
                txtTableNumber.Text = (btn.Text ?? string.Empty).Trim();

                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderColor = Color.Black;
                btn.FlatAppearance.BorderSize = 3;
            }

            UpdateStatusText(TableStatus.CoKhach);
            UpdateButtonState(TableStatus.CoKhach);
        }

        private Button FindTableButton(string tableNumber)
        {
            if (string.IsNullOrWhiteSpace(tableNumber)) return null;
            string target = tableNumber.Trim();

            Button result = null;
            void Search(Control parent)
            {
                foreach (Control c in parent.Controls)
                {
                    if (result != null) return;
                    if (c is Button b && string.Equals(b.Text?.Trim(), target, StringComparison.OrdinalIgnoreCase))
                    {
                        result = b;
                        return;
                    }
                    if (c.HasChildren)
                    {
                        Search(c);
                    }
                }
            }

            Search(this);
            return result;
        }

        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            string tableNumberText = (txtTableNumber.Text ?? string.Empty).Trim();
            if (string.IsNullOrEmpty(tableNumberText))
            {
                MessageBox.Show("Vui lòng chọn bàn để thanh toán.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int tableId = 0;
            int? orderId = null;
            decimal totalAmount = 0m;
            int? customerId = null;
            string customerType = null;

            try
            {
                using (var conn = new SqlConnection(strKetNoi))
                {
                    conn.Open();

                    // Lấy TableID
                    using (var cmdTable = new SqlCommand(
                        @"SELECT TableID, TableName FROM RestaurantTable 
                          WHERE TableName = @TableName OR TableName = N'Bàn ' + @TableNumber", conn))
                    {
                        cmdTable.Parameters.AddWithValue("@TableName", tableNumberText);
                        cmdTable.Parameters.AddWithValue("@TableNumber", tableNumberText);
                        using (var reader = cmdTable.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                tableId = Convert.ToInt32(reader["TableID"]);
                                if (reader["TableName"] != DBNull.Value)
                                {
                                    tableNumberText = reader["TableName"].ToString();
                                }
                            }
                        }
                    }

                    if (tableId == 0)
                    {
                        MessageBox.Show("Không tìm thấy bàn trong hệ thống.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Lấy order đang chờ thanh toán
                    using (var cmdOrder = new SqlCommand(
                        @"SELECT TOP 1 OrderId, TotalAmount, CustomerID 
                          FROM OrderTicket 
                          WHERE TableID = @TableID AND Status = N'Pending'
                          ORDER BY OrderId DESC", conn))
                    {
                        cmdOrder.Parameters.AddWithValue("@TableID", tableId);
                        using (var reader = cmdOrder.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                orderId = Convert.ToInt32(reader["OrderId"]);
                                totalAmount = reader["TotalAmount"] == DBNull.Value
                                    ? 0m
                                    : Convert.ToDecimal(reader["TotalAmount"], CultureInfo.InvariantCulture);
                                if (reader["CustomerID"] != DBNull.Value)
                                {
                                    customerId = Convert.ToInt32(reader["CustomerID"]);
                                }
                            }
                        }
                    }

                    if (!orderId.HasValue)
                    {
                        MessageBox.Show("Không tìm thấy hóa đơn đang chờ cho bàn này.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    // Nếu chưa có tổng tiền, tính từ chi tiết món
                    if (totalAmount <= 0)
                    {
                        using (var cmdTotal = new SqlCommand("SELECT SUM(LineTotal) FROM OrderTicketItem WHERE OrderId = @OrderId", conn))
                        {
                            cmdTotal.Parameters.AddWithValue("@OrderId", orderId.Value);
                            var result = cmdTotal.ExecuteScalar();
                            if (result != null && result != DBNull.Value)
                            {
                                totalAmount = Convert.ToDecimal(result, CultureInfo.InvariantCulture);
                            }
                        }
                    }

                    // Lấy CustomerType để kiểm tra đối tượng áp dụng mã giảm giá
                    if (customerId.HasValue)
                    {
                        using (var cmdCustType = new SqlCommand("SELECT CustomerType FROM Customers WHERE CustomerId = @Id", conn))
                        {
                            cmdCustType.Parameters.AddWithValue("@Id", customerId.Value);
                            var obj = cmdCustType.ExecuteScalar();
                            if (obj != null && obj != DBNull.Value)
                            {
                                customerType = obj.ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tra cứu hóa đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Mở form áp dụng mã giảm giá
            decimal finalAmount = totalAmount;
            decimal discountAmount = 0m;
            decimal discountPercent = 0m;
            string appliedPromoCode = null;
            string promoName = null;

            using (var promoForm = new FormApplyPromotion(totalAmount, customerType))
            {
                promoForm.StartPosition = FormStartPosition.CenterParent;
                if (promoForm.ShowDialog(this) != DialogResult.OK)
                {
                    // Người dùng hủy, không thanh toán
                    return;
                }

                finalAmount = promoForm.FinalAmount;
                discountAmount = promoForm.DiscountAmount;
                discountPercent = promoForm.DiscountPercent;
                appliedPromoCode = promoForm.AppliedPromotionCode;
                promoName = promoForm.PromotionName;
            }

            using (var billForm = new frmBill(orderId.Value, tableNumberText))
            {
                // Cập nhật thông tin giảm giá vào bill form nếu có
                if (!string.IsNullOrEmpty(appliedPromoCode))
                {
                    billForm.SetDiscountInfo(discountPercent, discountAmount, finalAmount, appliedPromoCode);
                }
                billForm.StartPosition = FormStartPosition.CenterParent;
                billForm.ShowDialog(this);
            }

            try
            {
                using (var conn = new SqlConnection(strKetNoi))
                {
                    conn.Open();
                    using (var tran = conn.BeginTransaction())
                    {
                        try
                        {
                            using (var cmdUpdateOrder = new SqlCommand(
                                "UPDATE OrderTicket SET Status = N'Completed', DateCheckOut = GETDATE(), TotalAmount = @TotalAmount, Discount = @Discount WHERE OrderId = @OrderId", conn, tran))
                            {
                                cmdUpdateOrder.Parameters.AddWithValue("@OrderId", orderId.Value);
                                cmdUpdateOrder.Parameters.AddWithValue("@TotalAmount", finalAmount);
                                cmdUpdateOrder.Parameters.AddWithValue("@Discount", discountAmount);
                                cmdUpdateOrder.ExecuteNonQuery();
                            }

                            using (var cmdUpdate = new SqlCommand("UPDATE RestaurantTable SET StatusID = 1 WHERE TableID = @TableID", conn, tran))
                            {
                                cmdUpdate.Parameters.AddWithValue("@TableID", tableId);
                                cmdUpdate.ExecuteNonQuery();
                            }

                            string customerName = (txtClient.Text ?? string.Empty).Trim();
                            if (string.IsNullOrEmpty(customerName) && customerId.HasValue)
                            {
                                using (var cmdCust = new SqlCommand("SELECT FullName FROM Customers WHERE CustomerId = @Id", conn, tran))
                                {
                                    cmdCust.Parameters.AddWithValue("@Id", customerId.Value);
                                    var obj = cmdCust.ExecuteScalar();
                                    if (obj != null && obj != DBNull.Value)
                                    {
                                        customerName = obj.ToString();
                                    }
                                }
                            }

                            SaveInvoiceAndTransaction(conn, tran, finalAmount, tableNumberText, orderId.Value, customerId, customerName);

                            // Cộng dồn tổng chi tiêu cho khách hàng và tự động nâng hạng VIP nếu >= 30 triệu
                            UpdateCustomerSpending(conn, tran, orderId.Value, finalAmount);

                            tran.Commit();
                        }
                        catch
                        {
                            tran.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật thanh toán: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Luôn xóa danh sách món và thông tin khách sau thanh toán
            if (currentSelectedTable != null)
            {
                currentSelectedTable.Tag = TableStatus.Trong;
                currentSelectedTable.BackColor = Color.Transparent;
                UpdateStatusText(TableStatus.Trong);
                UpdateButtonState(TableStatus.Trong);
            }

            dgvDishList.Rows.Clear();
            txtTotalAmount.Text = "0";
            ClearCustomerFields();

            try
            {
                NotificationCenter.RaiseInvoiceChanged();
            }
            catch { }
        }

        private void btnTableChoose_Click(object sender, EventArgs e)
        {
            if (!HasBookingInfo())
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin (tên, email, số điện thoại, số người và số bàn).", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validate đầu vào
            if (!IsValidPhone(txtNumberPhone.Text))
            {
                MessageBox.Show("Số điện thoại chỉ gồm chữ số và tối đa 10 ký tự.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!IsValidGmail(txtEmail.Text))
            {
                MessageBox.Show("Email phải có định dạng @gmail.com.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int guestCount;
            if (!int.TryParse(txtQuantity.Text, NumberStyles.Integer, CultureInfo.InvariantCulture, out guestCount) || guestCount <= 0)
            {
                MessageBox.Show("Số người phải là số nguyên dương.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string tableNumberText = txtTableNumber.Text.Trim();
            if (string.IsNullOrEmpty(tableNumberText))
            {
                MessageBox.Show("Vui lòng chọn bàn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DateTime bookingDate = dtpDay.Value.Date;
            TimeSpan bookingTime = dtpTime.Value.TimeOfDay;

            try
            {
                using (SqlConnection conn = new SqlConnection(strKetNoi))
                {
                    conn.Open();

                    // Tìm TableID từ RestaurantTable
                    int tableId = 0;
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

                    // Tìm hoặc tạo Customer (cho phép thiếu Phone/Email)
                    currentCustomerId = FindOrCreateCustomer(conn,
                        txtClient.Text.Trim(),
                        txtNumberPhone.Text.Trim(),
                        txtEmail.Text.Trim(),
                        "NH");
                    if (!currentCustomerId.HasValue)
                    {
                        MessageBox.Show("Không thể xác định khách hàng để đặt bàn.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // INSERT vào bảng BookingsTable mới
                    using (var cmd = new SqlCommand(
                       @"INSERT INTO BookingsTable (TableID, CustomerID, BookingDate, BookingTime, GuestCount, Status)
                         VALUES (@TableID, @CustomerID, @BookingDate, @BookingTime, @GuestCount, N'Đã đặt');", conn))
                    {
                        cmd.Parameters.AddWithValue("@TableID", tableId);
                        cmd.Parameters.AddWithValue("@CustomerID", currentCustomerId.Value);
                        cmd.Parameters.Add("@BookingDate", SqlDbType.Date).Value = bookingDate;
                        cmd.Parameters.Add("@BookingTime", SqlDbType.Time).Value = bookingTime;
                        cmd.Parameters.AddWithValue("@GuestCount", guestCount);

                        cmd.ExecuteNonQuery();
                    }

                    // Cập nhật trạng thái bàn thành "Đã đặt" (StatusID = 3)
                    using (var cmdUpdateTable = new SqlCommand(
                        @"UPDATE RestaurantTable SET StatusID = 3 WHERE TableID = @TableID", conn))
                    {
                        cmdUpdateTable.Parameters.AddWithValue("@TableID", tableId);
                        cmdUpdateTable.ExecuteNonQuery();
                    }
                }

                if (currentSelectedTable != null)
                {
                    currentSelectedTable.Tag = TableStatus.DaDat;
                    currentSelectedTable.BackColor = Color.FromArgb(255, 128, 128);
                    UpdateButtonState(TableStatus.DaDat);
                }

                using (var reservationForm = new frmReservationForm())
                {
                    reservationForm.StartPosition = FormStartPosition.CenterParent;
                    reservationForm.ShowDialog(this);
                }

                MessageBox.Show("Đặt bàn thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu đặt bàn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            string tableNumberText = (txtTableNumber.Text ?? string.Empty).Trim();
            if (string.IsNullOrEmpty(tableNumberText))
            {
                MessageBox.Show("Vui lòng chọn bàn để hủy trạng thái.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Mở form frmCancelTable để xử lý hủy đặt bàn và gửi email
            using (var cancelForm = new frmCancelTable())
            {
                cancelForm.StartPosition = FormStartPosition.CenterParent;
                cancelForm.ShowDialog(this);
            }

            // Sau khi đóng form, cập nhật lại giao diện
            if (currentSelectedTable != null)
            {
                // Reload trạng thái bàn từ DB
                LoadTableStatusesFromDb();
                
                // Cập nhật trạng thái button hiện tại
                currentSelectedTable.Tag = TableStatus.Trong;
                currentSelectedTable.BackColor = Color.Transparent;
                UpdateStatusText(TableStatus.Trong);
                UpdateButtonState(TableStatus.Trong);
            }

            dgvDishList.Rows.Clear();
            txtTotalAmount.Text = "0";
            ClearCustomerFields();
        }

        // Handler gắn từ Designer
        private void Form3_Load(object sender, EventArgs e)
        {
            ResetButtonState();

            // Chọn giờ cho dtpTime
            dtpTime.Format = DateTimePickerFormat.Custom;
            dtpTime.CustomFormat = "HH:mm";
            dtpTime.ShowUpDown = true;

            ApplyBookingGuard();

            // Tải trạng thái bàn từ CSDL và tô màu theo trạng thái
            LoadTableStatusesFromDb();
        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtTableNumber_TextChanged(object sender, EventArgs e)
        {
        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {
        }

        private void frmRestaurant_Click(object sender, EventArgs e)
        {
            if (currentSelectedTable != null)
            {
                currentSelectedTable.FlatAppearance.BorderSize = 0;
            }

            currentSelectedTable = null;
            ResetButtonState();
        }

        private void dtpTime_Click(object sender, EventArgs e)
        {
            using (var picker = new frmTime(dtpTime.Value))
            {
                picker.StartPosition = FormStartPosition.CenterParent;
                if (picker.ShowDialog(this) == DialogResult.OK)
                {
                    dtpTime.Value = picker.SelectedDateTime;
                }
            }
        }

        private void LoadTableStatusesFromDb()
        {
            var tableStatuses = new System.Collections.Generic.Dictionary<string, TableStatus>(System.StringComparer.OrdinalIgnoreCase);

            try
            {
                using (var conn = new SqlConnection(strKetNoi))
                using (var cmd = new SqlCommand(@"SELECT rt.TableName, ts.StatusName
                                                  FROM RestaurantTable rt
                                                  INNER JOIN TableStatus ts ON rt.StatusID = ts.StatusID", conn))
                {
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string tableName = reader.GetString(0).Trim();
                            string statusName = reader.GetString(1).Trim();
                            tableStatuses[tableName] = MapStatus(statusName);
                        }
                    }
                }

                foreach (var button in FindAllButtons(this))
                {
                    string text = button.Text != null ? button.Text.Trim() : string.Empty;
                    if (tableStatuses.TryGetValue(text, out var status)
                        || tableStatuses.TryGetValue("Bàn " + text, out status))
                    {
                        ApplyStatusToButton(button, status);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải trạng thái bàn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static System.Collections.Generic.IEnumerable<Button> FindAllButtons(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (c is Button b)
                {
                    yield return b;
                }

                if (c.HasChildren)
                {
                    foreach (var child in FindAllButtons(c))
                    {
                        yield return child;
                    }
                }
            }
        }

        private TableStatus MapStatus(string statusName)
        {
            if (statusName.Equals("Có khách", StringComparison.OrdinalIgnoreCase))
            {
                return TableStatus.CoKhach;
            }

            if (statusName.Equals("Đã đặt", StringComparison.OrdinalIgnoreCase))
            {
                return TableStatus.DaDat;
            }

            return TableStatus.Trong;
        }

        private void ApplyStatusToButton(Button button, TableStatus status)
        {
            button.Tag = status;
            switch (status)
            {
                case TableStatus.CoKhach:
                    button.BackColor = Color.MediumSeaGreen;
                    break;
                case TableStatus.DaDat:
                    button.BackColor = Color.FromArgb(255, 128, 128);
                    break;
                default:
                    // giữ nguyên màu mặc định khi bàn trống
                    break;
            }
        }

        private bool IsValidPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone)) return false;
            // Chỉ chữ số, tối đa 10
            return Regex.IsMatch(phone.Trim(), @"^\d{1,10}$");
        }

        private bool IsValidName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return false;
            // Không cho phép ký tự số trong tên
            return !Regex.IsMatch(name, @"\d");
        }

        private bool IsValidGmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            return Regex.IsMatch(email.Trim(), @"^[^@\s]+@gmail\.com$", RegexOptions.IgnoreCase);
        }

        private void UpdateStatusText(TableStatus status)
        {
            switch (status)
            {
                case TableStatus.CoKhach:
                    txtStatus.Text = "Có khách";
                    break;
                case TableStatus.DaDat:
                    txtStatus.Text = "Đã đặt";
                    break;
                default:
                    txtStatus.Text = "Bàn trống";
                    break;
            }
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        /// <summary>
        /// Tìm hoặc tạo khách hàng theo SĐT/Email, trả về CustomerID và đảm bảo CustomerCode/CustomerType không bị NULL.
        /// </summary>
        private int FindOrCreateCustomer(SqlConnection conn, string fullName, string phone, string email, string sourcePrefix)
        {
            string phoneVal = string.IsNullOrWhiteSpace(phone) ? null : phone.Trim();
            string emailVal = string.IsNullOrWhiteSpace(email) ? null : email.Trim();

            // Chuẩn hóa prefix: KS hoặc NH
            string prefix;
            if (string.IsNullOrWhiteSpace(sourcePrefix))
            {
                prefix = "NH";
            }
            else
            {
                var up = sourcePrefix.ToUpperInvariant();
                prefix = up.StartsWith("KS") ? "KS" : "NH";
            }

            string defaultType = prefix + "_Thường";
            string defaultSource = prefix == "KS" ? "Khách sạn" : "Nhà hàng";

            // 1. Ưu tiên tìm theo SĐT nếu có
            if (!string.IsNullOrEmpty(phoneVal))
            {
                using (var cmdFind = new SqlCommand("SELECT CustomerId, CustomerCode, CustomerType, Source FROM Customers WHERE PhoneNumber = @Phone", conn))
                {
                    cmdFind.Parameters.AddWithValue("@Phone", phoneVal);
                    using (var reader = cmdFind.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int id = Convert.ToInt32(reader["CustomerId"]);
                            string code = reader["CustomerCode"] == DBNull.Value ? null : reader["CustomerCode"].ToString();
                            string type = reader["CustomerType"] == DBNull.Value ? null : reader["CustomerType"].ToString();
                            string source = reader["Source"] == DBNull.Value ? null : reader["Source"].ToString();
                            reader.Close();
                            EnsureCustomerCodeAndType(conn, id, code, type, source, prefix, defaultType, defaultSource);
                            return id;
                        }
                    }
                }
            }

            // 2. Nếu không có SĐT hoặc không tìm thấy, thử tìm theo Email
            if (!string.IsNullOrEmpty(emailVal))
            {
                using (var cmdFind = new SqlCommand("SELECT CustomerId, CustomerCode, CustomerType, Source FROM Customers WHERE Email = @Email", conn))
                {
                    cmdFind.Parameters.AddWithValue("@Email", emailVal);
                    using (var reader = cmdFind.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int id = Convert.ToInt32(reader["CustomerId"]);
                            string code = reader["CustomerCode"] == DBNull.Value ? null : reader["CustomerCode"].ToString();
                            string type = reader["CustomerType"] == DBNull.Value ? null : reader["CustomerType"].ToString();
                            string source = reader["Source"] == DBNull.Value ? null : reader["Source"].ToString();
                            reader.Close();
                            EnsureCustomerCodeAndType(conn, id, code, type, source, prefix, defaultType, defaultSource);
                            return id;
                        }
                    }
                }
            }

            // 3. Không tìm thấy => tạo mới (tránh CustomerCode NULL gây lỗi unique)
            // Mã tạm ngắn gọn: prefix + timestamp (tối đa 18 ký tự, vừa với NVARCHAR(20))
            string tempCode = prefix + DateTime.Now.ToString("yyMMddHHmmssfff");

            int newId;
            using (var cmdInsert = new SqlCommand(
                @"INSERT INTO Customers (FullName, PhoneNumber, Email, CustomerType, CustomerCode, Source, CreatedAt, CCCD)
                  OUTPUT INSERTED.CustomerId
                  VALUES (@FullName, @Phone, @Email, @CustomerType, @CustomerCode, @Source, GETDATE(), @CCCD);", conn))
            {
                cmdInsert.Parameters.AddWithValue("@FullName", string.IsNullOrWhiteSpace(fullName) ? (object)DBNull.Value : fullName);
                cmdInsert.Parameters.AddWithValue("@Phone", (object)phoneVal ?? DBNull.Value);
                cmdInsert.Parameters.AddWithValue("@Email", (object)emailVal ?? DBNull.Value);
                cmdInsert.Parameters.AddWithValue("@CustomerType", (object)defaultType ?? DBNull.Value);
                cmdInsert.Parameters.AddWithValue("@CustomerCode", tempCode);
                cmdInsert.Parameters.AddWithValue("@Source", defaultSource);
                // Sinh CCCD tạm nhưng duy nhất để không vi phạm ràng buộc UNIQUE
                string cccdTemp = "TMP" + Guid.NewGuid().ToString("N").Substring(0, 9);
                cmdInsert.Parameters.AddWithValue("@CCCD", cccdTemp);
                newId = Convert.ToInt32(cmdInsert.ExecuteScalar());
            }

            // Cập nhật mã chuẩn theo ID
            EnsureCustomerCodeAndType(conn, newId, tempCode, defaultType, defaultSource, prefix, defaultType, defaultSource);
            return newId;
        }

        private void EnsureCustomerCodeAndType(SqlConnection conn, int customerId, string existingCode, string existingType, string existingSource,
            string prefix, string defaultType, string defaultSource)
        {
            string expectedCode = prefix + customerId.ToString("D4");
            bool needUpdateCode = string.IsNullOrWhiteSpace(existingCode) || !existingCode.Equals(expectedCode, StringComparison.OrdinalIgnoreCase);
            bool needUpdateType = string.IsNullOrWhiteSpace(existingType);
            bool needUpdateSource = string.IsNullOrWhiteSpace(existingSource);

            if (!needUpdateCode && !needUpdateType && !needUpdateSource)
                return;

            using (var cmd = new SqlCommand(
                "UPDATE Customers SET CustomerCode = ISNULL(@Code, CustomerCode), CustomerType = ISNULL(@Type, CustomerType), Source = ISNULL(@Source, Source) WHERE CustomerId = @Id",
                conn))
            {
                cmd.Parameters.AddWithValue("@Code", needUpdateCode ? (object)expectedCode : DBNull.Value);
                cmd.Parameters.AddWithValue("@Type", needUpdateType ? (object)defaultType : DBNull.Value);
                cmd.Parameters.AddWithValue("@Source", needUpdateSource ? (object)defaultSource : DBNull.Value);
                cmd.Parameters.AddWithValue("@Id", customerId);
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Tạo hóa đơn và giao dịch từ đơn nhà hàng, gắn mã hóa đơn tự sinh tuần tự.
        /// </summary>
        private void SaveInvoiceAndTransaction(SqlConnection conn, SqlTransaction tran, decimal totalAmount, string tableName, int orderId, int? customerId, string customerName)
        {
            DateTime now = DateTime.Now;
            bool hasInvoiceSource = ColumnExists(conn, tran, "Invoices", "Source");
            bool hasRestaurantOrderId = ColumnExists(conn, tran, "Invoices", "RestaurantOrderId");
            bool hasHotelBookingId = ColumnExists(conn, tran, "Invoices", "HotelBookingId");
            bool hasCustomerIdCol = ColumnExists(conn, tran, "Invoices", "CustomerId");
            // Không set InvoiceCode để tránh xung đột với computed column/view; sẽ đọc lại sau khi INSERT
            string generatedInvoiceCode = null;

            // Đảm bảo luôn có CustomerId (bảng mới bắt buộc NOT NULL)
            if (!customerId.HasValue)
            {
                using (var cmdNewCust = new SqlCommand(@"INSERT INTO Customers (FullName, CustomerType, CustomerCode, Source, CreatedAt)
OUTPUT INSERTED.CustomerId
VALUES (@FullName, @CustomerType, @CustomerCode, @Source, GETDATE());", conn, tran))
                {
                    string tempCode = "NH" + (DateTime.UtcNow.Ticks % 1_000_000_000).ToString("D9");
                    cmdNewCust.Parameters.AddWithValue("@FullName", string.IsNullOrWhiteSpace(customerName) ? (object)"Khách lẻ" : customerName);
                    cmdNewCust.Parameters.AddWithValue("@CustomerType", "NH_Thường");
                    cmdNewCust.Parameters.AddWithValue("@CustomerCode", tempCode);
                    cmdNewCust.Parameters.AddWithValue("@Source", "Nhà hàng");
                    var objCust = cmdNewCust.ExecuteScalar();
                    if (objCust != null && objCust != DBNull.Value)
                    {
                        customerId = Convert.ToInt32(objCust);
                    }
                }
            }

            int invoiceId = 0;
            // Xây dựng câu lệnh INSERT động để phù hợp các cột hiện có (RestaurantOrderId để thỏa CK_Invoice_Source)
            var colList = new System.Collections.Generic.List<string> { "InvoiceType", "InvoiceDate", "TotalAmount", "InvoiceStatus" };
            var valList = new System.Collections.Generic.List<string> { "@type", "@date", "@amount", "@status" };

            if (hasInvoiceSource)
            {
                colList.Add("Source");
                valList.Add("@source");
            }
            if (hasRestaurantOrderId)
            {
                colList.Add("RestaurantOrderId");
                valList.Add("@restaurantOrderId");
            }
            if (hasHotelBookingId)
            {
                // bảo đảm HotelBookingId NULL khi là hóa đơn nhà hàng
                colList.Add("HotelBookingId");
                valList.Add("@hotelBookingId");
            }
            // Luôn thêm CustomerId nếu cột tồn tại (bảng mới yêu cầu NOT NULL)
            if (hasCustomerIdCol)
            {
                colList.Add("CustomerId");
                valList.Add("@customerId");
            }

            string insertInvoiceSql = $"INSERT INTO Invoices ({string.Join(",", colList)}) VALUES ({string.Join(",", valList)}); SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using (var cmdInv = new SqlCommand(insertInvoiceSql, conn, tran))
            {
                cmdInv.Parameters.AddWithValue("@type", "Restaurant");
                cmdInv.Parameters.AddWithValue("@date", now);
                cmdInv.Parameters.AddWithValue("@amount", totalAmount);
                cmdInv.Parameters.AddWithValue("@status", "Hoàn thành");
                if (hasInvoiceSource)
                {
                    cmdInv.Parameters.AddWithValue("@source", "Restaurant");
                }
                if (hasRestaurantOrderId)
                {
                    cmdInv.Parameters.AddWithValue("@restaurantOrderId", orderId);
                }
                if (hasHotelBookingId)
                {
                    cmdInv.Parameters.AddWithValue("@hotelBookingId", DBNull.Value);
                }
                if (hasCustomerIdCol)
                {
                    cmdInv.Parameters.AddWithValue("@customerId", customerId.HasValue ? (object)customerId.Value : DBNull.Value);
                }

                var obj = cmdInv.ExecuteScalar();
                if (obj != null)
                {
                    int.TryParse(obj.ToString(), out invoiceId);
                }
            }

            if (invoiceId == 0)
            {
                throw new InvalidOperationException("Không tạo được hóa đơn cho thanh toán nhà hàng.");
            }

            // Lấy mã InvoiceCode nếu DB có cột này
            if (ColumnExists(conn, tran, "Invoices", "InvoiceCode"))
            {
                using (var cmdCode = new SqlCommand("SELECT InvoiceCode FROM Invoices WHERE InvoiceId = @Id", conn, tran))
                {
                    cmdCode.Parameters.AddWithValue("@Id", invoiceId);
                    var obj = cmdCode.ExecuteScalar();
                    if (obj != null && obj != DBNull.Value)
                    {
                        generatedInvoiceCode = obj.ToString();
                    }
                }
            }

            bool hasCustomerNameCol = ColumnExists(conn, tran, "Transactions", "CustomerName");
            string insertTranSql = hasCustomerNameCol
                ? @"INSERT INTO Transactions (InvoiceId, PaymentDate, Amount, MethodID, StatusID, Note, CustomerName)
                  VALUES (@InvoiceId, @PaymentDate, @Amount, @MethodID, @StatusID, @Note, @CustomerName);"
                : @"INSERT INTO Transactions (InvoiceId, PaymentDate, Amount, MethodID, StatusID, Note)
                  VALUES (@InvoiceId, @PaymentDate, @Amount, @MethodID, @StatusID, @Note);";

            using (var cmdTran = new SqlCommand(insertTranSql, conn, tran))
            {
                cmdTran.Parameters.AddWithValue("@InvoiceId", invoiceId);
                cmdTran.Parameters.AddWithValue("@PaymentDate", now);
                cmdTran.Parameters.AddWithValue("@Amount", totalAmount);
                cmdTran.Parameters.AddWithValue("@MethodID", 1); // 1 = Tiền mặt
                cmdTran.Parameters.AddWithValue("@StatusID", 1); // 1 = Hoàn thành
                cmdTran.Parameters.AddWithValue("@Note", string.IsNullOrWhiteSpace(tableName)
                    ? (object)DBNull.Value
                    : (object)$"Thanh toán bàn {tableName} - Order {orderId}");
                if (hasCustomerNameCol)
                {
                    cmdTran.Parameters.AddWithValue("@CustomerName", string.IsNullOrWhiteSpace(customerName)
                        ? (object)DBNull.Value
                        : (object)customerName);
                }

                cmdTran.ExecuteNonQuery();
            }

            // Cập nhật CustomerID cho OrderTicket nếu chưa có (để liên kết về sau)
            if (customerId.HasValue)
            {
                using (var cmdUpdateOrder = new SqlCommand("UPDATE OrderTicket SET CustomerID = ISNULL(CustomerID, @CustomerID) WHERE OrderId = @OrderId", conn, tran))
                {
                    cmdUpdateOrder.Parameters.AddWithValue("@CustomerID", customerId.Value);
                    cmdUpdateOrder.Parameters.AddWithValue("@OrderId", orderId);
                    cmdUpdateOrder.ExecuteNonQuery();
                }
            }
        }

        private string GenerateNextInvoiceCode(SqlConnection conn, SqlTransaction tran)
        {
            int next = 1;
            try
            {
                using (var cmd = new SqlCommand(
                    "SELECT MAX(CAST(REPLACE(InvoiceCode,'INV-','') AS INT)) FROM Invoices WHERE InvoiceCode LIKE 'INV-%'", conn, tran))
                {
                    var obj = cmd.ExecuteScalar();
                    if (obj != null && obj != DBNull.Value)
                    {
                        int.TryParse(obj.ToString(), out next);
                        next++;
                    }
                }
            }
            catch
            {
                next = (int)(DateTime.Now.Ticks % 1000000);
            }

            return $"INV-{next:D4}";
        }

        private bool ColumnExists(SqlConnection conn, SqlTransaction tran, string tableName, string columnName)
        {
            try
            {
                using (var cmd = new SqlCommand(
                    "SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @Table AND COLUMN_NAME = @Col", conn, tran))
                {
                    cmd.Parameters.AddWithValue("@Table", tableName);
                    cmd.Parameters.AddWithValue("@Col", columnName);
                    var obj = cmd.ExecuteScalar();
                    return obj != null;
                }
            }
            catch
            {
                return false;
            }
        }

        private bool IsComputedColumn(SqlConnection conn, SqlTransaction tran, string tableName, string columnName)
        {
            try
            {
                using (var cmd = new SqlCommand(
                    "SELECT COLUMNPROPERTY(object_id(@tbl), @col, 'IsComputed')", conn, tran))
                {
                    cmd.Parameters.AddWithValue("@tbl", tableName);
                    cmd.Parameters.AddWithValue("@col", columnName);
                    var obj = cmd.ExecuteScalar();
                    if (obj != null && obj != DBNull.Value)
                    {
                        return Convert.ToInt32(obj) == 1;
                    }
                }
            }
            catch { }
            return false;
        }

        /// <summary>
        /// Cộng dồn tổng chi tiêu cho khách hàng và tự động nâng hạng VIP nếu >= 30 triệu.
        /// </summary>
        private void UpdateCustomerSpending(SqlConnection conn, SqlTransaction tran, int orderId, decimal orderTotal)
        {
            // Lấy CustomerID từ OrderTicket
            int? customerId = null;
            using (var cmdGetCust = new SqlCommand("SELECT CustomerID FROM OrderTicket WHERE OrderId = @OrderId", conn, tran))
            {
                cmdGetCust.Parameters.AddWithValue("@OrderId", orderId);
                var result = cmdGetCust.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    customerId = Convert.ToInt32(result);
                }
            }

            if (!customerId.HasValue) return;

            // Cộng dồn TotalSpending
            using (var cmdUpdate = new SqlCommand(
                "UPDATE Customers SET TotalSpending = ISNULL(TotalSpending, 0) + @Amount WHERE CustomerId = @Id", conn, tran))
            {
                cmdUpdate.Parameters.AddWithValue("@Amount", orderTotal);
                cmdUpdate.Parameters.AddWithValue("@Id", customerId.Value);
                cmdUpdate.ExecuteNonQuery();
            }

            // Lấy TotalSpending mới và CustomerType hiện tại để xét nâng hạng
            decimal newTotal = 0m;
            string currentType = null;
            using (var cmdGet = new SqlCommand("SELECT TotalSpending, CustomerType FROM Customers WHERE CustomerId = @Id", conn, tran))
            {
                cmdGet.Parameters.AddWithValue("@Id", customerId.Value);
                using (var reader = cmdGet.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        newTotal = reader["TotalSpending"] == DBNull.Value ? 0m : Convert.ToDecimal(reader["TotalSpending"]);
                        currentType = reader["CustomerType"] == DBNull.Value ? null : reader["CustomerType"].ToString();
                    }
                }
            }

            // Nếu >= 30 triệu và chưa phải VIP thì nâng hạng
            const decimal VIP_THRESHOLD = 30_000_000m;
            if (newTotal >= VIP_THRESHOLD && !string.IsNullOrEmpty(currentType))
            {
                string upper = currentType.ToUpperInvariant();
                string newType = null;

                if (upper.StartsWith("NH") && !upper.Contains("VIP"))
                {
                    newType = "NH_Vip";
                }
                else if (upper.StartsWith("KS") && !upper.Contains("VIP"))
                {
                    newType = "KS_Vip";
                }

                if (!string.IsNullOrEmpty(newType))
                {
                    using (var cmdUpgrade = new SqlCommand("UPDATE Customers SET CustomerType = @Type WHERE CustomerId = @Id", conn, tran))
                    {
                        cmdUpgrade.Parameters.AddWithValue("@Type", newType);
                        cmdUpgrade.Parameters.AddWithValue("@Id", customerId.Value);
                        cmdUpgrade.ExecuteNonQuery();
                    }
                }
            }
        }

        /// <summary>
        /// Tải thông tin khách hàng theo bàn (ưu tiên OrderTicket mới nhất có CustomerID, sau đó BookingsTable mới nhất)
        /// </summary>
        private void LoadCustomerInfoByTable(int tableId)
        {
            ClearCustomerFields();

            try
            {
                using (var conn = new SqlConnection(strKetNoi))
                {
                    conn.Open();

                    // 1) Ưu tiên OrderTicket mới nhất (mọi trạng thái) có CustomerID
                    using (var cmdOrder = new SqlCommand(
                        @"SELECT TOP 1 c.FullName, c.PhoneNumber, c.Email, o.GuestCount
                          FROM OrderTicket o
                          INNER JOIN Customers c ON o.CustomerID = c.CustomerId
                          WHERE o.TableID = @TableID AND o.CustomerID IS NOT NULL
                          ORDER BY o.OrderId DESC", conn))
                    {
                        cmdOrder.Parameters.AddWithValue("@TableID", tableId);
                        using (var reader = cmdOrder.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtClient.Text = reader["FullName"] == DBNull.Value ? string.Empty : reader["FullName"].ToString();
                                txtNumberPhone.Text = reader["PhoneNumber"] == DBNull.Value ? string.Empty : reader["PhoneNumber"].ToString();
                                txtEmail.Text = reader["Email"] == DBNull.Value ? string.Empty : reader["Email"].ToString();
                                txtQuantity.Text = reader["GuestCount"] == DBNull.Value ? string.Empty : reader["GuestCount"].ToString();
                                return;
                            }
                        }
                    }

                    // 2) Nếu không có OrderTicket có khách, lấy booking mới nhất có CustomerID
                    using (var cmdBook = new SqlCommand(
                        @"SELECT TOP 1 c.FullName, c.PhoneNumber, c.Email, b.GuestCount
                          FROM BookingsTable b
                          INNER JOIN Customers c ON b.CustomerID = c.CustomerId
                          WHERE b.TableID = @TableID AND b.CustomerID IS NOT NULL
                          ORDER BY b.BookingID DESC", conn))
                    {
                        cmdBook.Parameters.AddWithValue("@TableID", tableId);
                        using (var reader = cmdBook.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtClient.Text = reader["FullName"] == DBNull.Value ? string.Empty : reader["FullName"].ToString();
                                txtNumberPhone.Text = reader["PhoneNumber"] == DBNull.Value ? string.Empty : reader["PhoneNumber"].ToString();
                                txtEmail.Text = reader["Email"] == DBNull.Value ? string.Empty : reader["Email"].ToString();
                                txtQuantity.Text = reader["GuestCount"] == DBNull.Value ? string.Empty : reader["GuestCount"].ToString();
                                return;
                            }
                        }
                    }

                    // 3) Không tìm thấy thông tin khách hàng nào có liên kết
                    // Có thể do OrderTicket/BookingsTable có CustomerID = NULL
                    // Debug: kiểm tra xem có bản ghi nào không
#if DEBUG
                    using (var cmdDebug = new SqlCommand(
                        @"SELECT 
                            (SELECT COUNT(*) FROM OrderTicket WHERE TableID = @TableID) AS OrderCount,
                            (SELECT COUNT(*) FROM OrderTicket WHERE TableID = @TableID AND CustomerID IS NOT NULL) AS OrderWithCustomer,
                            (SELECT COUNT(*) FROM BookingsTable WHERE TableID = @TableID) AS BookingCount,
                            (SELECT COUNT(*) FROM BookingsTable WHERE TableID = @TableID AND CustomerID IS NOT NULL) AS BookingWithCustomer", conn))
                    {
                        cmdDebug.Parameters.AddWithValue("@TableID", tableId);
                        using (var reader = cmdDebug.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int orderCount = Convert.ToInt32(reader["OrderCount"]);
                                int orderWithCust = Convert.ToInt32(reader["OrderWithCustomer"]);
                                int bookCount = Convert.ToInt32(reader["BookingCount"]);
                                int bookWithCust = Convert.ToInt32(reader["BookingWithCustomer"]);

                                if (orderCount > 0 && orderWithCust == 0)
                                {
                                    System.Diagnostics.Debug.WriteLine($"Table {tableId}: {orderCount} orders but none have CustomerID");
                                }
                                if (bookCount > 0 && bookWithCust == 0)
                                {
                                    System.Diagnostics.Debug.WriteLine($"Table {tableId}: {bookCount} bookings but none have CustomerID");
                                }
                            }
                        }
                    }
#endif
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("LoadCustomerInfoByTable error: " + ex.Message);
            }
        }

        private void ClearCustomerFields()
        {
            txtClient.Text = string.Empty;
            txtNumberPhone.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtQuantity.Text = string.Empty;
        }

        private void dtpTime_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
