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
        private readonly string strKetNoi = ConfigurationManager.ConnectionStrings["QuanLyChuoiNhaHangKhachSan"].ConnectionString;

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

            // Hiển thị số bàn (loại bỏ khoảng trắng)
            txtTableNumber.Text = (btn.Text ?? string.Empty).Trim();

            int tableNumber;
            if (!int.TryParse(txtTableNumber.Text, out tableNumber))
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

            // Lưu khách hàng trước khi gọi món (cho phép thiếu Phone/Email, sẽ để NULL)
            try
            {
                using (var conn = new SqlConnection(strKetNoi))
                {
                    conn.Open();
                    currentCustomerId = FindOrCreateCustomer(conn, txtClient.Text.Trim(), txtNumberPhone.Text.Trim(), txtEmail.Text.Trim());
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
            using (var billForm = new frmBill())
            {
                billForm.StartPosition = FormStartPosition.CenterParent;
                billForm.ShowDialog(this);
            }

            // Sau khi thanh toán xong, cập nhật bàn về trạng thái trống
            string tableNumberText = (txtTableNumber.Text ?? string.Empty).Trim();
            if (string.IsNullOrEmpty(tableNumberText)) return;

            try
            {
                using (var conn = new SqlConnection(strKetNoi))
                {
                    conn.Open();

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

                    if (tableId != 0)
                    {
                        // Cập nhật trạng thái OrderTicket thành Completed
                        using (var cmdUpdateOrder = new SqlCommand(
                            "UPDATE OrderTicket SET Status = N'Completed', DateCheckOut = GETDATE() WHERE TableID = @TableID AND Status = N'Pending'", conn))
                        {
                            cmdUpdateOrder.Parameters.AddWithValue("@TableID", tableId);
                            cmdUpdateOrder.ExecuteNonQuery();
                        }

                        // Cập nhật trạng thái bàn về trống
                        using (var cmdUpdate = new SqlCommand("UPDATE RestaurantTable SET StatusID = 1 WHERE TableID = @TableID", conn))
                        {
                            cmdUpdate.Parameters.AddWithValue("@TableID", tableId);
                            cmdUpdate.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật trạng thái bàn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Luôn xóa danh sách món và thông tin khách sau thanh toán (nằm ngoài try-catch)
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
                    currentCustomerId = FindOrCreateCustomer(conn, txtClient.Text.Trim(), txtNumberPhone.Text.Trim(), txtEmail.Text.Trim());

                    // INSERT vào bảng BookingsTable mới
                    using (var cmd = new SqlCommand(
                       @"INSERT INTO BookingsTable (TableID, CustomerID, BookingDate, BookingTime, GuestCount, Status)
                         VALUES (@TableID, @CustomerID, @BookingDate, @BookingTime, @GuestCount, N'Đã đặt');", conn))
                    {
                        cmd.Parameters.AddWithValue("@TableID", tableId);
                        cmd.Parameters.AddWithValue("@CustomerID", (object)currentCustomerId ?? DBNull.Value);
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

            if (MessageBox.Show("Bạn có chắc chắn muốn hủy đặt bàn này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            try
            {
                using (var conn = new SqlConnection(strKetNoi))
                {
                    conn.Open();

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

                    // Đưa bàn về trạng thái trống (StatusID = 1)
                    using (var cmdUpdate = new SqlCommand(
                        "UPDATE RestaurantTable SET StatusID = 1 WHERE TableID = @TableID", conn))
                    {
                        cmdUpdate.Parameters.AddWithValue("@TableID", tableId);
                        cmdUpdate.ExecuteNonQuery();
                    }
                }

                if (currentSelectedTable != null)
                {
                    currentSelectedTable.Tag = TableStatus.Trong;
                    currentSelectedTable.BackColor = Color.Transparent;
                    UpdateStatusText(TableStatus.Trong);
                    UpdateButtonState(TableStatus.Trong);
                }

                MessageBox.Show("Bàn đã được cập nhật về trạng thái trống.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi hủy đặt bàn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                    txtStatus.Text = "Không có khách";
                    break;
            }
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        /// <summary>
        /// Tìm hoặc tạo khách hàng theo SĐT, trả về CustomerID
        /// </summary>
        private int FindOrCreateCustomer(SqlConnection conn, string fullName, string phone, string email)
        {
            string phoneVal = string.IsNullOrWhiteSpace(phone) ? null : phone.Trim();
            string emailVal = string.IsNullOrWhiteSpace(email) ? null : email.Trim();

            // 1. Ưu tiên tìm theo SĐT nếu có
            if (!string.IsNullOrEmpty(phoneVal))
            {
                using (var cmdFind = new SqlCommand("SELECT CustomerId FROM Customers WHERE PhoneNumber = @Phone", conn))
                {
                    cmdFind.Parameters.AddWithValue("@Phone", phoneVal);
                    var result = cmdFind.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        return Convert.ToInt32(result);
                    }
                }
            }

            // 2. Nếu không có SĐT hoặc không tìm thấy, thử tìm theo Email
            if (!string.IsNullOrEmpty(emailVal))
            {
                using (var cmdFind = new SqlCommand("SELECT CustomerId FROM Customers WHERE Email = @Email", conn))
                {
                    cmdFind.Parameters.AddWithValue("@Email", emailVal);
                    var result = cmdFind.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        return Convert.ToInt32(result);
                    }
                }
            }

            // 3. Không tìm thấy => tạo mới (các trường thiếu để NULL)
            using (var cmdInsert = new SqlCommand(
                @"INSERT INTO Customers (FullName, PhoneNumber, Email, CreatedAt)
                  VALUES (@FullName, @Phone, @Email, GETDATE());
                  SELECT SCOPE_IDENTITY();", conn))
            {
                cmdInsert.Parameters.AddWithValue("@FullName", string.IsNullOrWhiteSpace(fullName) ? (object)DBNull.Value : fullName);
                cmdInsert.Parameters.AddWithValue("@Phone", (object)phoneVal ?? DBNull.Value);
                cmdInsert.Parameters.AddWithValue("@Email", (object)emailVal ?? DBNull.Value);
                return Convert.ToInt32(cmdInsert.ExecuteScalar());
            }
        }

        /// <summary>
        /// Tải thông tin khách hàng theo bàn (từ booking hoặc order gần nhất)
        /// </summary>
        private void LoadCustomerInfoByTable(int tableId)
        {
            try
            {
                using (var conn = new SqlConnection(strKetNoi))
                {
                    conn.Open();

                    // Ưu tiên lấy từ BookingsTable (đặt bàn)
                    using (var cmd = new SqlCommand(
                        @"SELECT TOP 1 c.FullName, c.PhoneNumber, c.Email, b.GuestCount
                          FROM BookingsTable b
                          INNER JOIN Customers c ON b.CustomerID = c.CustomerId
                          WHERE b.TableID = @TableID AND b.Status = N'Đã đặt'
                          ORDER BY b.BookingID DESC", conn))
                    {
                        cmd.Parameters.AddWithValue("@TableID", tableId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtClient.Text = reader["FullName"].ToString();
                                txtNumberPhone.Text = reader["PhoneNumber"].ToString();
                                txtEmail.Text = reader["Email"] == DBNull.Value ? string.Empty : reader["Email"].ToString();
                                txtQuantity.Text = reader["GuestCount"].ToString();
                                return;
                            }
                        }
                    }

                    // Nếu không có booking, thử lấy từ OrderTicket (gọi món)
                    using (var cmd = new SqlCommand(
                        @"SELECT TOP 1 c.FullName, c.PhoneNumber, c.Email
                          FROM OrderTicket o
                          INNER JOIN Customers c ON o.CustomerID = c.CustomerId
                          WHERE o.TableID = @TableID AND o.Status = N'Pending'
                          ORDER BY o.OrderId DESC", conn))
                    {
                        cmd.Parameters.AddWithValue("@TableID", tableId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtClient.Text = reader["FullName"].ToString();
                                txtNumberPhone.Text = reader["PhoneNumber"].ToString();
                                txtEmail.Text = reader["Email"] == DBNull.Value ? string.Empty : reader["Email"].ToString();
                                return;
                            }
                        }
                    }
                }
            }
            catch
            {
                // Nếu lỗi, không hiển thị thông tin
            }
        }

        private void ClearCustomerFields()
        {
            txtClient.Text = string.Empty;
            txtNumberPhone.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtQuantity.Text = string.Empty;
        }
    }
}
