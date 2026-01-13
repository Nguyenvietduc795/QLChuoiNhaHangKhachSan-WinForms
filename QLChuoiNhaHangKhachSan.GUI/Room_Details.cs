using Guna.UI2.WinForms;
using QLChuoiNhaHangKhachSan.GUI;
using QLChuoiNhaHangKhachSan.GUI.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QLChuoiNhaHangKhachSan.GUI
{
    public partial class Room_Details : Form
    {
        public string TenKhach { get; set; }
        public int SoNgay { get; set; }
        public string SelectedStatus { get; private set; }
        public string SelectedRoomType { get; private set; }

        private readonly DateTime? _lockedViewTime;
        private readonly bool _lockDateTimePickers;
        private readonly string _providedRoomType;

        private Label _lblCheckout;
        private Guna2Button _btnThanhToan;
        private Button _btnAddService;
        private Button _btnRemoveService;
        private BookingInfo _existing;
        private ContextMenuStrip _menuService;

        // --- ĐÃ SỬA: Bỏ giá cứng, sẽ lấy từ Database ---
        private const string ROOM_SERVICE_NAME = "Tiền phòng";

        private Label _toastLabel;
        private Timer _toastTimer;

        public Room_Details(string maPhong, string trangThai, string loaiPhong = null, DateTime? lockedViewTime = null, bool lockDateTimePickers = true)
        {

            InitializeComponent();

            _lockedViewTime = lockedViewTime;
            _lockDateTimePickers = lockDateTimePickers && lockedViewTime.HasValue;
            _providedRoomType = loaiPhong;

            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                labMaphong.Text = maPhong;
                return;
            }

            labMaphong.Text = maPhong;
            if (!string.IsNullOrWhiteSpace(_providedRoomType))
                SetRoomTypeByName(_providedRoomType);
            else
                SetRoomTypeByCode(maPhong);
            SetStatusSelection(trangThai);
            UpdateVipBadge();

            if (guna2DataGridView2.Columns.Count >= 4)
            {
                guna2DataGridView2.Columns[0].HeaderText = "Dịch vụ";
                guna2DataGridView2.Columns[1].HeaderText = "Số lượng";
                guna2DataGridView2.Columns[2].HeaderText = "Đơn giá";
                guna2DataGridView2.Columns[3].HeaderText = "Thành tiền";
            }
            InitializeServiceMenu();
            InitToast();

            _lblCheckout = new Label
            {
                AutoSize = true,
                Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic),
                ForeColor = System.Drawing.Color.DimGray,
                Location = new System.Drawing.Point(dtNgay.Left, dtNgay.Bottom + 5)
            };
            this.gunaMaphong.Controls.Add(_lblCheckout);

            _btnThanhToan = new Guna2Button
            {
                Text = "Thanh toán",
                AutoSize = false,
                Visible = false,
                BorderRadius = 8,
                FillColor = Color.FromArgb(255, 159, 67),
                ForeColor = Color.White,
                Padding = new Padding(8, 4, 8, 4),
                Font = btnNhanphong.Font,
                Size = new Size(btnThoat.Width, btnThoat.Height),
                Location = new System.Drawing.Point(btnThoat.Right + 10, btnThoat.Top),
                Anchor = AnchorStyles.Bottom
            };
            _btnThanhToan.Click += BtnThanhToan_Click;
            this.gunaMaphong.Controls.Add(_btnThanhToan);

            // If incoming status indicates a reservation, show cancel button immediately
            try
            {
                if (IsBookedNotCheckedIn())
                {
                    _btnThanhToan.Visible = true;
                    var startPreview = dtNgay.Value.Date + dtGio.Value.TimeOfDay;
                    var endPreview = startPreview.AddDays((int)nNgay.Value);
                    UpdatePaymentButtonByState(startPreview, endPreview);
                }
            }
            catch { }

            _btnAddService = new Button
            {
                Text = "+ Thêm DV",
                AutoSize = true,
                BackColor = System.Drawing.Color.SeaGreen,
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new System.Drawing.Point(guna2DataGridView2.Left, guna2DataGridView2.Top - 30)
            };
            _btnAddService.FlatAppearance.BorderSize = 0;
            _btnAddService.Click += (s, e) => AddService();
            this.gunaMaphong.Controls.Add(_btnAddService);

            _btnRemoveService = new Button
            {
                Text = "Xóa dòng",
                AutoSize = true,
                BackColor = System.Drawing.Color.IndianRed,
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new System.Drawing.Point(guna2DataGridView2.Left + 90, guna2DataGridView2.Top - 30)
            };
            _btnRemoveService.FlatAppearance.BorderSize = 0;
            _btnRemoveService.Click += (s, e) => RemoveService();
            this.gunaMaphong.Controls.Add(_btnRemoveService);

            dtNgay.ValueChanged += ScheduleChanged;
            dtGio.ValueChanged += ScheduleChanged;
            dtGio.MouseDown += DtGio_MouseDown;
            dtGio.Click += DtGio_Click;
            nNgay.ValueChanged += ScheduleChanged;

            // --- SỬA ĐOẠN NÀY ---
            // Chuẩn hóa chuỗi để so sánh không phân biệt hoa thường và khoảng trắng
            // Chuẩn hóa chuỗi để so sánh chính xác
            string statusNorm = (trangThai ?? "").Trim().ToLower();

            if (statusNorm.Contains("trống") || statusNorm == "vacant")
            {
                // Phòng trống -> Nút là Đặt phòng
                btnNhanphong.Text = "Đặt phòng";
            }
            else if (statusNorm.Contains("đặt") || statusNorm == "booked")
            {
                // ĐÃ SỬA: Phòng đã đặt -> Nút là NHẬN PHÒNG
                btnNhanphong.Text = "Nhận phòng";
            }
            else if (statusNorm.Contains("thuê") || statusNorm.Contains("occupied") || statusNorm.Contains("nhận"))
            {
                // Phòng đang thuê -> Nút là Lưu thay đổi (hoặc Thanh toán tùy logic bạn muốn)
                btnNhanphong.Text = "Lưu thay đổi";
            }
            else
            {
                btnNhanphong.Text = "Lưu thay đổi";
            }
            // --------------------

            // Khóa ngày/giờ theo thời gian đang xem nếu được truyền vào
            ApplyLockedViewTime();

            // Thử lấy từ BookingManager trước, nếu không có thì lấy từ DB
            if (_existing == null && _lockedViewTime.HasValue)
            {
                // Lấy booking từ DB theo viewTime
                _existing = GetBookingFromDb(maPhong, _lockedViewTime.Value);
            }

            // Nếu vẫn chưa có booking và trạng thái cho thấy phòng đang có khách (đã đặt hoặc đang thuê) -> lấy từ DB theo thời điểm hiện tại
            if (_existing == null && !string.IsNullOrWhiteSpace(trangThai))
            {
                string statusLower = trangThai.ToLower();
                if (statusLower.Contains("đặt") || statusLower.Contains("thuê") || statusLower.Contains("occupied") || statusLower.Contains("booked"))
                {
                    // Lấy booking đang hoạt động của phòng này (bất kể thời gian)
                    _existing = GetActiveBookingFromDb(maPhong);
                }
            }

            if (_existing != null)
            {
                txtName.Text = _existing.Customer;
                DateTime start = _existing.Start;
                DateTime end = _existing.End;
                dtNgay.Value = start.Date;
                // Ensure dtGio uses the same date component as dtNgay to avoid cross-form inconsistency
                dtGio.Value = new DateTime(dtNgay.Value.Year, dtNgay.Value.Month, dtNgay.Value.Day, start.Hour, start.Minute, start.Second);
                int days = Math.Max(1, (int)Math.Ceiling((end - start).TotalDays));
                nNgay.Value = days;
                UpdateCheckoutLabel(start, end, days);
                LoadServicesForBooking(labMaphong.Text);
                EnsureRoomCharge(days);
                EnsureVipBreakfast();
                _btnThanhToan.Visible = true;
                // giữ trạng thái theo tham số truyền vào (trangThai) thay vì tự suy diễn theo thời gian
                SetStatusSelection(trangThai);
                UpdatePaymentButtonByState(start, end);
            }
            else
            {
                EnsureRoomCharge((int)nNgay.Value);
                EnsureVipBreakfast();
                UpdateCheckoutLabel(dtNgay.Value, dtNgay.Value.AddDays((int)nNgay.Value), (int)nNgay.Value);
            }
        }

        // --- HÀM MỚI: Lấy giá từ Database ---
        private decimal GetCurrentPriceFromDB(string roomId)
        {
            decimal price = 0;
            var connStr = ConfigurationManager.ConnectionStrings["ConnStr"]?.ConnectionString;

            if (string.IsNullOrWhiteSpace(connStr)) return 0;

            try
            {
                using (var conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    // Truy vấn vào View v_RoomPrice mà bạn đã tạo trong SQL
                    string sql = "SELECT TOP 1 AppliedPrice FROM dbo.v_RoomPrice WHERE RoomID = @RoomID";

                    using (var cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@RoomID", roomId);
                        var result = cmd.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            price = Convert.ToDecimal(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Có thể bỏ qua lỗi hoặc log lại
                MessageBox.Show("Lỗi lấy giá: " + ex.Message);
            }
            return price;
        }

        /// <summary>
        /// Lấy thông tin booking từ DB theo mã phòng và thời điểm tham chiếu
        /// </summary>
        private BookingInfo GetBookingFromDb(string roomCode, DateTime referenceTime)
        {
            var connStr = ConfigurationManager.ConnectionStrings["ConnStr"]?.ConnectionString;
            if (string.IsNullOrWhiteSpace(connStr)) return null;

            try
            {
                using (var conn = new SqlConnection(connStr))
                using (var cmd = new SqlCommand(@"SELECT c.FullName, d.CheckIn, d.CheckOut
    FROM dbo.BookingDetails d
    JOIN dbo.HotelBookings b ON d.BookingId = b.BookingId
    JOIN dbo.Customers c ON b.CustomerId = c.CustomerId
    WHERE d.RoomId = @RoomID AND b.Status NOT IN (N'Paid', N'Cancelled') AND d.CheckIn <= @RefTime AND d.CheckOut > @RefTime", conn))
                {
                    cmd.Parameters.AddWithValue("@RoomID", roomCode);
                    cmd.Parameters.AddWithValue("@RefTime", referenceTime);
                    conn.Open();
                    using (var rd = cmd.ExecuteReader())
                    {
                        if (rd.Read())
                        {
                            var customer = rd["FullName"]?.ToString();
                            DateTime? start = rd["CheckIn"] != DBNull.Value ? (DateTime?)rd["CheckIn"] : null;
                            DateTime? end = rd["CheckOut"] != DBNull.Value ? (DateTime?)rd["CheckOut"] : null;
                            if (start.HasValue && end.HasValue)
                            {
                                // defensive: if DB contains invalid interval (end <= start), adjust to at least +1 day
                                if (end.Value <= start.Value)
                                {
                                    Debug.WriteLine($"[GetBookingFromDb][WARN] Room={roomCode} DB end <= start. Adjusting end. Start={start:yyyy-MM-dd HH:mm:ss} DBEnd={end:yyyy-MM-dd HH:mm:ss}");
                                    end = start.Value.AddDays(1);
                                }
                                Debug.WriteLine($"[GetBookingFromDb] Room={roomCode} Read Start={start:yyyy-MM-dd HH:mm:ss} End={end:yyyy-MM-dd HH:mm:ss}");
                                return new BookingInfo
                                {
                                    Customer = customer,
                                    Start = start.Value,
                                    End = end.Value,
                                    Services = new List<ServiceItem>()
                                };
                            }
                        }
                    }
                }
            }
            catch { }

            return null;
        }

        /// <summary>
        /// Lấy booking đang hoạt động của phòng (không quan tâm thời gian)
        /// </summary>
        private BookingInfo GetActiveBookingFromDb(string roomCode)
        {
            var connStr = ConfigurationManager.ConnectionStrings["ConnStr"]?.ConnectionString;
            if (string.IsNullOrWhiteSpace(connStr)) return null;

            try
            {
                using (var conn = new SqlConnection(connStr))
                using (var cmd = new SqlCommand(@"SELECT TOP 1 c.FullName, d.CheckIn, d.CheckOut
    FROM dbo.BookingDetails d
    JOIN dbo.HotelBookings b ON d.BookingId = b.BookingId
    LEFT JOIN dbo.Customers c ON b.CustomerId = c.CustomerId
    WHERE d.RoomId = @RoomID AND b.Status NOT IN (N'Paid', N'Cancelled')
    ORDER BY b.CreatedDate DESC", conn))
                {
                    cmd.Parameters.AddWithValue("@RoomID", roomCode);
                    conn.Open();
                    using (var rd = cmd.ExecuteReader())
                    {
                        if (rd.Read())
                        {
                            var customer = rd["FullName"]?.ToString();
                            DateTime? start = rd["CheckIn"] != DBNull.Value ? (DateTime?)rd["CheckIn"] : null;
                            DateTime? end = rd["CheckOut"] != DBNull.Value ? (DateTime?)rd["CheckOut"] : null;
                            
                            if (start.HasValue && end.HasValue)
                            {
                                // defensive: if DB contains invalid interval (end <= start), adjust to at least +1 day
                                if (end.Value <= start.Value)
                                {
                                    end = start.Value.AddDays(1);
                                }
                                Debug.WriteLine($"[GetActiveBookingFromDb] Room={roomCode} Customer={customer} Start={start:yyyy-MM-dd HH:mm:ss} End={end:yyyy-MM-dd HH:mm:ss}");
                                return new BookingInfo
                                {
                                    Customer = customer ?? string.Empty,
                                    Start = start.Value,
                                    End = end.Value,
                                    Services = new List<ServiceItem>()
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[GetActiveBookingFromDb] Error: {ex.Message}");
            }

            return null;
        }

        private void ApplyLockedViewTime()
        {
            if (!_lockedViewTime.HasValue) return;
            var view = _lockedViewTime.Value;

            // Chỉ set ngày/giờ khi chưa có booking (phòng trống)
            // Nếu có booking thì sẽ được set từ thông tin booking
            if (_existing == null)
            {
                dtNgay.Value = view.Date;
                // combine date and time explicitly
                dtGio.Value = new DateTime(dtNgay.Value.Year, dtNgay.Value.Month, dtNgay.Value.Day, view.Hour, view.Minute, view.Second);

                var days = (int)Math.Max(1, nNgay.Value);
                var end = view.AddDays(days);
                UpdateCheckoutLabel(view, end, days);
            }

            if (_lockDateTimePickers)
            {
                // Giữ nguyên enable để người dùng có thể chỉnh giờ/ngày nếu cần
            }
        }

        private decimal ParsePrice(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return 0;
            input = input.Trim();
            bool hasK = input.EndsWith("k", StringComparison.OrdinalIgnoreCase);
            var digits = new StringBuilder();
            foreach (char c in input)
            {
                if (char.IsDigit(c)) digits.Append(c);
            }
            if (digits.Length == 0) return 0;
            decimal val = 0;
            decimal.TryParse(digits.ToString(), out val);
            if (hasK) val *= 1000;
            return val;
        }

        private void EnsureRoomCharge(int days)
        {
            decimal basePrice = GetRoomPrice();

            // 2. Xác định khoảng thời gian khách ở
            DateTime startDate = dtNgay.Value.Date; // Ngày bắt đầu từ DatePicker
            DateTime endDate = startDate.AddDays(days); // Ngày kết thúc

            // 3. [QUAN TRỌNG] Tính tổng tiền có áp dụng tăng giá Lễ/Tết/Cuối tuần
            // Hàm này nằm trong class HolidayPriceConfig bạn vừa gửi
            decimal totalAmount = HolidayPriceConfig.CalculateTotalPrice(basePrice, startDate, endDate);

            // 4. Cập nhật hoặc Thêm dòng "Tiền phòng" vào lưới (GridView)
            bool found = false;
            foreach (DataGridViewRow r in guna2DataGridView2.Rows)
            {
                if (r.IsNewRow) continue;

                // Tìm dòng có tên "Tiền phòng"
                if (string.Equals(Convert.ToString(r.Cells[0].Value), ROOM_SERVICE_NAME, StringComparison.OrdinalIgnoreCase))
                {
                    found = true;
                    r.Cells[1].Value = days; // Cập nhật số ngày
                    r.Cells[2].Value = basePrice.ToString("N0"); // Cột Đơn giá: Hiển thị giá gốc
                    r.Cells[3].Value = totalAmount.ToString("N0"); // Cột Thành tiền: Hiển thị giá đã tính Lễ/Tết
                    break;
                }
            }

            // Nếu chưa có dòng tiền phòng thì thêm mới
            if (!found)
            {
                int idx = guna2DataGridView2.Rows.Add();
                var row = guna2DataGridView2.Rows[idx];
                row.Cells[0].Value = ROOM_SERVICE_NAME;
                row.Cells[1].Value = days;
                row.Cells[2].Value = basePrice.ToString("N0");
                row.Cells[3].Value = totalAmount.ToString("N0"); // Giá cuối cùng
            }
        }

        private void InitializeServiceMenu()
        {
            _menuService = new ContextMenuStrip();
            var miAddService = new ToolStripMenuItem("Thêm dịch vụ", null, (s, e) => AddService());
            var miRemoveService = new ToolStripMenuItem("Xóa dòng", null, (s, e) => RemoveService());
            _menuService.Items.Add(miAddService);
            _menuService.Items.Add(miRemoveService);
            guna2DataGridView2.ContextMenuStrip = _menuService;
        }

        private void InitToast()
        {
            _toastLabel = new Label
            {
                AutoSize = true,
                Visible = false,
                BackColor = Color.FromArgb(44, 62, 80),
                ForeColor = Color.White,
                Padding = new Padding(10, 6, 10, 6),
                Font = new Font("Segoe UI", 9F, FontStyle.Regular),
            };
            this.Controls.Add(_toastLabel);
            _toastTimer = new Timer { Interval = 1800 };
            _toastTimer.Tick += (s, e) => { _toastTimer.Stop(); _toastLabel.Visible = false; };
        }

        private void ShowToast(string message)
        {
            if (_toastLabel == null) return;
            _toastLabel.Text = message;
            _toastLabel.BringToFront();
            _toastLabel.Visible = true;
            // position bottom-right inside form
            _toastLabel.Left = this.ClientSize.Width - _toastLabel.PreferredWidth - 20;
            _toastLabel.Top = this.ClientSize.Height - _toastLabel.PreferredHeight - 20;
            _toastTimer?.Stop();
            _toastTimer?.Start();
        }

        private int GetActiveBookingId(SqlConnection conn, SqlTransaction tran, string roomId)
        {
            string findSql = @"SELECT TOP 1 b.BookingId
                           FROM dbo.HotelBookings b
                           JOIN dbo.BookingDetails d ON b.BookingId = d.BookingId
                           WHERE d.RoomId = @RoomID AND b.Status NOT IN (N'Paid', N'Cancelled')
                           ORDER BY b.CreatedDate DESC";
            using (var cmd = new SqlCommand(findSql, conn, tran))
            {
                cmd.Parameters.AddWithValue("@RoomID", roomId);
                var obj = cmd.ExecuteScalar();
                if (obj != null && obj != DBNull.Value) return Convert.ToInt32(obj);
            }
            return 0;
        }

        private void UpsertBookingServices(SqlConnection conn, SqlTransaction tran, int bookingId, List<ServiceItem> services)
        {
            if (bookingId <= 0) return;
            string roomId = labMaphong.Text.Trim();
            
            // Xóa dịch vụ cũ của booking này CHỈ CHO PHÒNG NÀY (không ảnh hưởng phòng khác)
            using (var del = new SqlCommand(@"DELETE FROM dbo.BookingServices 
                WHERE BookingID = @BID AND (RoomID = @RoomID OR RoomID IS NULL)", conn, tran))
            {
                del.Parameters.AddWithValue("@BID", bookingId);
                del.Parameters.AddWithValue("@RoomID", roomId);
                del.ExecuteNonQuery();
            }
            
            if (services == null || services.Count == 0) return;
            
            // Đảm bảo cột RoomID tồn tại
            try
            {
                using (var cmdAlter = new SqlCommand(@"
                    IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.BookingServices') AND name = 'RoomID')
                    BEGIN
                        ALTER TABLE dbo.BookingServices ADD RoomID NVARCHAR(50);
                    END", conn, tran))
                {
                    cmdAlter.ExecuteNonQuery();
                }
            }
            catch { }
            
            foreach (var s in services)
            {
                using (var ins = new SqlCommand(@"INSERT INTO dbo.BookingServices(BookingID, RoomID, ServiceName, Quantity, UnitPrice, TotalAmount)
VALUES(@BID, @RoomID, @Name, @Qty, @Price, @Total)", conn, tran))
                {
                    ins.Parameters.AddWithValue("@BID", bookingId);
                    ins.Parameters.AddWithValue("@RoomID", roomId);
                    ins.Parameters.AddWithValue("@Name", s.Name);
                    ins.Parameters.AddWithValue("@Qty", s.Quantity);
                    ins.Parameters.AddWithValue("@Price", s.UnitPrice);
                    ins.Parameters.AddWithValue("@Total", s.Amount);
                    ins.ExecuteNonQuery();
                }
            }
        }

        private void BtnThanhToan_Click(object sender, EventArgs e)
        {
            // If no in-memory booking object, but status indicates a reservation, attempt DB cancel fallback
            if (_existing == null && IsBookedNotCheckedIn())
            {
                var confirm = MessageBox.Show("Hủy đặt phòng này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm != DialogResult.Yes) return;

                DateTime start = dtNgay.Value.Date + dtGio.Value.TimeOfDay;
                DateTime end = start.AddDays((int)nNgay.Value);

                // Get booking info before canceling for email
                var bookingInfoForEmail = GetBookingInfoForEmail(labMaphong.Text);

                bool ok = CancelBookingInDb(labMaphong.Text, start, end);
                if (ok)
                {
                    // Send cancellation email
                    try
                    {
                        if (bookingInfoForEmail != null && !string.IsNullOrWhiteSpace(bookingInfoForEmail.Email))
                        {
                            bookingInfoForEmail.IsCancelled = true;
                            bookingInfoForEmail.CancellationReason = "Khách hàng yêu cầu hủy";
                            EmailHelper.SendCancellationEmail(bookingInfoForEmail);
                        }
                    }
                    catch { }

                    BookingManager.RemoveBooking(labMaphong.Text);
                    _existing = null;
                    _btnThanhToan.Visible = false;
                    txtName.Text = string.Empty;
                    guna2DataGridView2.Rows.Clear();
                    EnsureRoomCharge((int)nNgay.Value);

                    try
                    {
                        var listForm = Application.OpenForms.OfType<ListRoom>().FirstOrDefault();
                        if (listForm != null)
                        {
                            listForm.RefreshFromBookings(new[] { labMaphong.Text });
                        }
                    }
                    catch { }

                    MessageBox.Show("Đã hủy phòng thành công!", "Thông báo");
                    this.Close();
                    return;
                }
                else
                {
                    MessageBox.Show("Không tìm thấy hoặc không thể hủy booking trên cơ sở dữ liệu.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            if (_existing == null)
            {
                MessageBox.Show("Chưa có thông tin đặt/nhận phòng để thanh toán.", "Thông báo");
                return;
            }

            // Nếu đang ở trạng thái ĐÃ ĐẶT: nút này là HỦY PHÒNG, không in hóa đơn
            if (IsBookedNotCheckedIn())
            {
                var confirm = MessageBox.Show("Hủy đặt phòng này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm == DialogResult.Yes)
                {
                    // 1. Lấy ngày giờ chuẩn từ booking đang có (không lấy từ DatePicker vì có thể bị sai lệch)
                    DateTime start = _existing.Start;
                    DateTime end = _existing.End;

                    // Get booking info before canceling for email
                    var bookingInfoForEmail = GetBookingInfoForEmail(labMaphong.Text);

                    // 2. Gọi hàm xóa dưới Database
                    bool dbSuccess = CancelBookingInDb(labMaphong.Text, start, end);

                    if (dbSuccess)
                    {
                        // Send cancellation email
                        try
                        {
                            if (bookingInfoForEmail != null && !string.IsNullOrWhiteSpace(bookingInfoForEmail.Email))
                            {
                                bookingInfoForEmail.IsCancelled = true;
                                bookingInfoForEmail.CancellationReason = "Khách hàng yêu cầu hủy";
                                EmailHelper.SendCancellationEmail(bookingInfoForEmail);
                            }
                        }
                        catch { }

                        // 3. Nếu xóa DB thành công thì mới xóa trên RAM và cập nhật giao diện
                        BookingManager.RemoveBooking(labMaphong.Text);
                        _existing = null;
                        _btnThanhToan.Visible = false;
                        txtName.Text = string.Empty;
                        guna2DataGridView2.Rows.Clear();
                        EnsureRoomCharge((int)nNgay.Value);

                        try
                        {
                            var listForm = Application.OpenForms.OfType<ListRoom>().FirstOrDefault();
                            if (listForm != null)
                            {
                                listForm.RefreshFromBookings(new[] { labMaphong.Text });
                            }
                        }
                        catch { }

                        MessageBox.Show("Đã hủy phòng thành công!", "Thông báo");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Lỗi: Không thể xóa đặt phòng dưới cơ sở dữ liệu. Vui lòng kiểm tra lại ngày giờ.", "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                return;
            }

            // ...existing code for payment flow...
            var sfd = new SaveFileDialog
            {
                Title = "Lưu hóa đơn",
                Filter = "Text Files (*.txt)|*.txt|All files (*.*)|*.*",
                FileName = $"HoaDon_{labMaphong.Text}_{DateTime.Now:yyyyMMddHHmm}.txt"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                EnsureRoomCharge((int)nNgay.Value);
                SyncServicesToBooking();
                WriteInvoiceText(sfd.FileName);
                MessageBox.Show("Đã lưu hóa đơn (TXT).", "Thông báo");

                decimal totalAmount = 0m;
                var services = CollectServicesFromGrid();
                foreach (var s in services) totalAmount += s.Amount;

                try
                {
                    SaveInvoiceToDatabase(sfd.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lưu hóa đơn vào DB thất bại: " + ex.Message + "\nHủy thao tác hoàn tất phòng. Vui lòng kiểm tra kết nối CSDL.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                BookingManager.RemoveBooking(labMaphong.Text);
                _existing = null;
                _btnThanhToan.Visible = false;
                txtName.Text = string.Empty;
                guna2DataGridView2.Rows.Clear();
                EnsureRoomCharge((int)nNgay.Value);

                try
                {
                    var listForm = Application.OpenForms.OfType<ListRoom>().FirstOrDefault();
                    if (listForm != null)
                    {
                        listForm.RefreshFromBookings(new[] { labMaphong.Text });
                    }
                }
                catch { }
            }
        }

        /// <summary>
        /// Lưu thông tin thanh toán và trả phòng.
        /// Logic: Tìm Booking đang hoạt động của phòng này -> Set thành Paid -> Set phòng thành Trống.
        /// </summary>
        private void SaveInvoiceToDatabase(string invoiceFilePath)
        {
            if (_existing == null) return;

            // 1. Tính tổng tiền từ Grid
            decimal totalAmount = 0m;
            foreach (DataGridViewRow r in guna2DataGridView2.Rows)
            {
                if (r.IsNewRow) continue;
                decimal cell;
                if (decimal.TryParse(Convert.ToString(r.Cells[3].Value), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.CurrentCulture, out cell))
                    totalAmount += cell;
            }

            var connStr = ConfigurationManager.ConnectionStrings["ConnStr"]?.ConnectionString;
            if (string.IsNullOrWhiteSpace(connStr)) return;

            using (var conn = new SqlConnection(connStr))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        // 2. Tìm BookingID đang "sống" của phòng này
                        int foundBookingId = 0;
                        string findSql = @"
                            SELECT TOP 1 b.BookingId 
                            FROM dbo.HotelBookings b
                            JOIN dbo.BookingDetails d ON b.BookingId = d.BookingId
                            WHERE d.RoomId = @RoomID 
                              AND b.Status NOT IN (N'Paid', N'Cancelled') 
                            ORDER BY b.CreatedDate DESC";

                        using (var cmd = new SqlCommand(findSql, conn, tran))
                        {
                            cmd.Parameters.AddWithValue("@RoomID", labMaphong.Text.Trim());
                            var obj = cmd.ExecuteScalar();
                            if (obj != null && obj != DBNull.Value) foundBookingId = Convert.ToInt32(obj);
                        }

                        // 3. Cập nhật Booking thành Paid
                        if (foundBookingId > 0)
                        {
                            using (var upd = new SqlCommand("UPDATE dbo.HotelBookings SET Status = N'Paid' WHERE BookingId = @BID", conn, tran))
                            {
                                upd.Parameters.AddWithValue("@BID", foundBookingId);
                                upd.ExecuteNonQuery();
                            }
                        }

                        // 4. Lưu lịch sử thanh toán vào bảng Payment
                        using (var cmdTable = new SqlCommand(@"
                            IF OBJECT_ID('dbo.Payment','U') IS NULL
                            CREATE TABLE dbo.Payment(
                                PaymentID INT IDENTITY(1,1) PRIMARY KEY,
                                RoomID NVARCHAR(50), Amount DECIMAL(18,2), PaidDate DATETIME, Note NVARCHAR(200)
                            );", conn, tran))
                        {
                            cmdTable.ExecuteNonQuery();
                        }

                        using (var ins = new SqlCommand("INSERT INTO dbo.Payment(RoomID, Amount, PaidDate, Note) VALUES(@RoomID, @Amount, @PaidDate, @Note)", conn, tran))
                        {
                            ins.Parameters.AddWithValue("@RoomID", labMaphong.Text.Trim());
                            ins.Parameters.AddWithValue("@Amount", totalAmount);
                            ins.Parameters.AddWithValue("@PaidDate", DateTime.Now);
                            ins.Parameters.AddWithValue("@Note", Path.GetFileName(invoiceFilePath) ?? (object)DBNull.Value);
                            ins.ExecuteNonQuery();
                        }

                        // 5. Trả phòng về trạng thái "Phòng Trống"
                        using (var cmd = new SqlCommand("UPDATE dbo.Rooms SET Status = N'Phòng Trống' WHERE RoomId = @RoomID", conn, tran))
                        {
                            cmd.Parameters.AddWithValue("@RoomID", labMaphong.Text.Trim());
                            cmd.ExecuteNonQuery();
                        }

                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        throw new Exception("Lỗi Database: " + ex.Message);
                    }
                }
            }
        }

        private void WriteInvoiceText(string path)
        {
            if (_existing == null) return;
            string room = labMaphong.Text;
            DateTime start = _existing.Start;
            DateTime end = _existing.End;
            int days = Math.Max(1, (int)Math.Ceiling((end - start).TotalDays));

            SyncServicesToBooking();
            decimal serviceTotal = 0;
            var sb = new StringBuilder();
            sb.AppendLine("HOA DON THANH TOAN");
            sb.AppendLine($"Phong: {room}");
            sb.AppendLine($"Khach hang: {_existing.Customer}");
            sb.AppendLine($"Check-in: {start:dd/MM/yyyy HH:mm}");
            sb.AppendLine($"Check-out: {end:dd/MM/yyyy HH:mm}");
            sb.AppendLine($"So ngay o: {days}");
            sb.AppendLine(new string('-', 40));
            sb.AppendLine("Dich vu:");
            foreach (var s in _existing.Services)
            {
                var amt = s.UnitPrice * s.Quantity;
                serviceTotal += amt;
                sb.AppendLine($" - {s.Name}: {s.Quantity} x {s.UnitPrice:N0} = {amt:N0}");
            }
            sb.AppendLine($"Tong dich vu: {serviceTotal:N0}");
            sb.AppendLine($"Ngay lap: {DateTime.Now:dd/MM/yyyy HH:mm}");
            sb.AppendLine(new string('-', 40));
            sb.AppendLine("Cam on quy khach!");

            File.WriteAllText(path, sb.ToString());
        }

        /// <summary>
        /// Lấy thông tin booking từ DB để gửi email (bao gồm email khách hàng)
        /// </summary>
        private BookingRowInfo GetBookingInfoForEmail(string roomId)
        {
            var connStr = ConfigurationManager.ConnectionStrings["ConnStr"]?.ConnectionString;
            if (string.IsNullOrWhiteSpace(connStr)) return null;

            try
            {
                using (var conn = new SqlConnection(connStr))
                using (var cmd = new SqlCommand(@"
                    SELECT TOP 1 
                        b.BookingCode, c.FullName, c.CCCD, c.PhoneNumber, c.Email, c.Sex, c.Nationality,
                        d.CheckIn, d.CheckOut, b.CreatedDate
                    FROM dbo.BookingDetails d
                    JOIN dbo.HotelBookings b ON d.BookingId = b.BookingId
                    JOIN dbo.Customers c ON b.CustomerId = c.CustomerId
                    WHERE d.RoomId = @RoomID 
                      AND b.Status NOT IN (N'Paid', N'Cancelled')
                    ORDER BY b.CreatedDate DESC", conn))
                {
                    cmd.Parameters.AddWithValue("@RoomID", roomId.Trim());
                    conn.Open();
                    using (var rd = cmd.ExecuteReader())
                    {
                        if (rd.Read())
                        {
                            DateTime? checkIn = rd["CheckIn"] != DBNull.Value ? (DateTime?)rd["CheckIn"] : null;
                            DateTime? checkOut = rd["CheckOut"] != DBNull.Value ? (DateTime?)rd["CheckOut"] : null;
                            DateTime? created = rd["CreatedDate"] != DBNull.Value ? (DateTime?)rd["CreatedDate"] : null;

                            string detail = roomId;
                            if (checkIn.HasValue && checkOut.HasValue)
                            {
                                detail = $"{roomId} ({checkIn.Value:dd/MM/yyyy HH:mm} - {checkOut.Value:dd/MM/yyyy HH:mm})";
                            }

                            return new BookingRowInfo
                            {
                                Id = rd["BookingCode"]?.ToString() ?? roomId,
                                Customer = rd["FullName"]?.ToString(),
                                Date = created.HasValue ? created.Value.ToString("dd/MM/yyyy") : DateTime.Now.ToString("dd/MM/yyyy"),
                                CCCD = rd["CCCD"]?.ToString(),
                                SDT = rd["PhoneNumber"]?.ToString(),
                                Email = rd["Email"]?.ToString(),
                                Gender = rd["Sex"]?.ToString(),
                                Nationality = rd["Nationality"]?.ToString(),
                                Detail = detail,
                                CreatedDate = created
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"GetBookingInfoForEmail error: {ex.Message}");
            }
            return null;
        }

        private void ScheduleChanged(object sender, EventArgs e)
        {
            DateTime start = dtNgay.Value.Date + dtGio.Value.TimeOfDay;
            int days = (int)nNgay.Value;
            DateTime end = start.AddDays(days);
            UpdateCheckoutLabel(start, end, days);
            EnsureRoomCharge(days);
        }

        private void UpdateCheckoutLabel(DateTime start, DateTime end, int days)
        {
            if (_lblCheckout == null) return;
            _lblCheckout.Text = $"Check-out: {end:dd/MM/yyyy HH:mm} | Số ngày: {days}";
        }

        // Đây là file Room_Details.cs
        private void btnNhanphong_Click(object sender, EventArgs e)
        {
            // Decide whether to open Rental_information or to save directly.
            // Only open Rental_information when this is a check-in action (booking -> renting), NOT when already renting.
            bool shouldOpenRentalForm = false;

            // 1) Button text contains "nhận" (Nhận phòng) => check-in flow
            if (!string.IsNullOrWhiteSpace(btnNhanphong.Text) && btnNhanphong.Text.IndexOf("nhận", StringComparison.OrdinalIgnoreCase) >= 0)
                shouldOpenRentalForm = true;

            // 2) SelectedStatus indicates a booking (đặt/open) -> needs check-in
            if (!shouldOpenRentalForm && !string.IsNullOrWhiteSpace(SelectedStatus))
            {
                var s = SelectedStatus.ToLowerInvariant();
                if (s.Contains("đặt") || s.Contains("book"))
                    shouldOpenRentalForm = true;
            }

            // Already renting -> skip opening rental form, just save changes
            if (shouldOpenRentalForm)
            {
                using (var frm = new Rental_information())
                {
                    frm.CurrentRoomID = labMaphong.Text; // pass current room
                    frm.TenKhachDaCo = txtName.Text.Trim();
                    // pass check-in / check-out and price so Rental_information saves correct interval
                    frm.SelectedCheckIn = dtNgay.Value.Date + dtGio.Value.TimeOfDay;
                    frm.SelectedCheckOut = (dtNgay.Value.Date + dtGio.Value.TimeOfDay).AddDays((int)nNgay.Value);
                    frm.CurrentPrice = GetRoomPrice();
                    var res = frm.ShowDialog(this);
                    if (res == DialogResult.OK)
                    {
                        try
                        {
                            var listForm = Application.OpenForms.OfType<ListRoom>().FirstOrDefault();
                            if (listForm != null)
                                listForm.RefreshFromBookings(new[] { labMaphong.Text });
                        }
                        catch { }

                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
                return;
            }

            // Fallback: save booking (Đặt phòng / Lưu thay đổi)
            PerformSave();
        }
        private void BtnLuu_Click(object sender, EventArgs e)
        {
            PerformSave();
        }

        private void PerformSave()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Vui lòng nhập Full Name khách hàng!", "Thông báo");
                return;
            }

            this.TenKhach = txtName.Text.Trim();
            this.SoNgay = (int)nNgay.Value;
            
            // Xác định trạng thái dựa vào nút bấm
            string targetStatus;
            if (btnNhanphong.Text.IndexOf("Đặt", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                // Nút "Đặt phòng" -> Trạng thái là "Phòng đã đặt" (màu vàng)
                targetStatus = "Phòng đã đặt";
            }
            else if (btnNhanphong.Text.IndexOf("Nhận", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                // Nút "Nhận phòng" -> Trạng thái là "Phòng đang thuê" (màu xanh)
                targetStatus = "Phòng đang thuê";
            }
            else
            {
                // Nút "Lưu thay đổi" -> Giữ nguyên trạng thái từ combobox
                targetStatus = guna2ComboBox1.SelectedItem?.ToString() ?? "Phòng đang thuê";
            }
            
            this.SelectedStatus = targetStatus;
            this.SelectedRoomType = guna2ComboBox2.SelectedItem != null ? guna2ComboBox2.SelectedItem.ToString() : null;

            DateTime start = dtNgay.Value.Date + dtGio.Value.TimeOfDay;

            // QUAN TRỌNG: Đảm bảo số ngày ít nhất là 1
            if (this.SoNgay <= 0)
            {
                this.SoNgay = 1;
                nNgay.Value = 1;
            }

            DateTime end = start.AddDays(this.SoNgay);

            // KIỂM TRA: Nếu end <= start thì tự động set end = start + 1 ngày
            if (end <= start)
            {
                this.SoNgay = 1;
                end = start.AddDays(1);
                nNgay.Value = 1;
            }

            Debug.WriteLine($"[PerformSave] Room={labMaphong.Text} Start={start:yyyy-MM-dd HH:mm:ss} End={end:yyyy-MM-dd HH:mm:ss} Days={this.SoNgay} Status={targetStatus}");

            var info = new BookingInfo { Customer = this.TenKhach, Start = start, End = end, Services = CollectServicesFromGrid() };
            EnsureRoomCharge(this.SoNgay);
            EnsureVipBreakfast();
            info.Services = CollectServicesFromGrid();

            var connStr = ConfigurationManager.ConnectionStrings["ConnStr"]?.ConnectionString;
            bool dbSaved = false;
            int bookingId = 0;
            string bookingCode = string.Empty;
            string customerEmail = string.Empty;
            
            if (!string.IsNullOrWhiteSpace(connStr))
            {
                try
                {
                    using (var conn = new SqlConnection(connStr))
                    {
                        conn.Open();
                        using (var tran = conn.BeginTransaction())
                        {
                            int customerId = 0;
                            string customerPhone = string.Empty;
                            string customerCCCD = string.Empty;
                            string customerGender = string.Empty;
                            string customerNationality = string.Empty;

                            // Tìm khách hàng theo tên
                            if (!string.IsNullOrWhiteSpace(this.TenKhach))
                            {
                                using (var cmd = new SqlCommand("SELECT TOP 1 CustomerId, Email, PhoneNumber, CCCD, Sex, Nationality FROM dbo.Customers WHERE FullName = @Name", conn, tran))
                                {
                                    cmd.Parameters.AddWithValue("@Name", this.TenKhach);
                                    using (var rd = cmd.ExecuteReader())
                                    {
                                        if (rd.Read())
                                        {
                                            customerId = Convert.ToInt32(rd["CustomerId"]);
                                            customerEmail = rd["Email"]?.ToString() ?? string.Empty;
                                            customerPhone = rd["PhoneNumber"]?.ToString() ?? string.Empty;
                                            customerCCCD = rd["CCCD"]?.ToString() ?? string.Empty;
                                            customerGender = rd["Sex"]?.ToString() ?? string.Empty;
                                            customerNationality = rd["Nationality"]?.ToString() ?? string.Empty;
                                        }
                                    }
                                }
                            }

                            // Tạo khách hàng mới nếu chưa có
                            if (customerId == 0 && !string.IsNullOrWhiteSpace(this.TenKhach))
                            {
                                using (var cmd = new SqlCommand(@"
                                    IF NOT EXISTS (SELECT 1 FROM dbo.Customers WHERE FullName = @Name)
                                    BEGIN
                                        INSERT INTO dbo.Customers(FullName) VALUES(@Name);
                                        SELECT CAST(SCOPE_IDENTITY() AS INT);
                                    END
                                    ELSE
                                    BEGIN
                                        SELECT TOP 1 CustomerId FROM dbo.Customers WHERE FullName = @Name;
                                    END", conn, tran))
                                {
                                    cmd.Parameters.AddWithValue("@Name", this.TenKhach);
                                    var obj = cmd.ExecuteScalar();
                                    if (obj != null && obj != DBNull.Value) customerId = Convert.ToInt32(obj);
                                }
                            }

                            // Lấy EmployeeID
                            int employeeId = 1;
                            try
                            {
                                using (var cmd = new SqlCommand("SELECT TOP 1 EmployeeId FROM dbo.Employees ORDER BY EmployeeId", conn, tran))
                                {
                                    var obj = cmd.ExecuteScalar();
                                    if (obj != null && obj != DBNull.Value) employeeId = Convert.ToInt32(obj);
                                }
                            }
                            catch { employeeId = 1; }

                            // Xác định bookingId
                            bookingId = GetActiveBookingId(conn, tran, labMaphong.Text.Trim());

                            if (bookingId == 0)
                            {
                                // Tạo mã booking dạng BK0001
                                using (var cmdMaxCode = new SqlCommand(@"
                                    SELECT ISNULL(MAX(CAST(SUBSTRING(BookingCode, 3, LEN(BookingCode)-2) AS INT)), 0) + 1 
                                    FROM dbo.HotelBookings WHERE BookingCode LIKE 'BK%' AND ISNUMERIC(SUBSTRING(BookingCode, 3, LEN(BookingCode)-2)) = 1", conn, tran))
                                {
                                    var maxVal = cmdMaxCode.ExecuteScalar();
                                    int nextNum = (maxVal != null && maxVal != DBNull.Value) ? Convert.ToInt32(maxVal) : 1;
                                    bookingCode = "BK" + nextNum.ToString("D4");
                                }
                                
                                using (var cmd = new SqlCommand(@"INSERT INTO dbo.HotelBookings(BookingCode, CustomerID, EmployeeID, CreatedDate, Status)
VALUES(@Code, @CustomerID, @EmployeeID, GETDATE(), @Status);
SELECT CAST(SCOPE_IDENTITY() AS INT);", conn, tran))
                                {
                                    cmd.Parameters.AddWithValue("@Code", bookingCode);
                                    cmd.Parameters.AddWithValue("@CustomerID", customerId == 0 ? (object)DBNull.Value : customerId);
                                    cmd.Parameters.AddWithValue("@EmployeeID", employeeId);
                                    cmd.Parameters.AddWithValue("@Status", targetStatus);
                                    var obj = cmd.ExecuteScalar();
                                    if (obj != null && obj != DBNull.Value) bookingId = Convert.ToInt32(obj);
                                }

                                decimal appliedPrice = GetRoomPrice();
                                using (var cmd = new SqlCommand(@"INSERT INTO dbo.BookingDetails(BookingID, RoomID, CheckIn, CheckOut, AppliedPrice, Note)
VALUES(@BookingID, @RoomID, @CheckIn, @CheckOut, @Price, NULL);", conn, tran))
                                {
                                    cmd.Parameters.AddWithValue("@BookingID", bookingId);
                                    cmd.Parameters.AddWithValue("@RoomID", labMaphong.Text);
                                    cmd.Parameters.Add(new SqlParameter("@CheckIn", System.Data.SqlDbType.DateTime) { Value = start });
                                    cmd.Parameters.Add(new SqlParameter("@CheckOut", System.Data.SqlDbType.DateTime) { Value = end });
                                    cmd.Parameters.AddWithValue("@Price", appliedPrice);
                                    cmd.ExecuteNonQuery();
                                }
                            }
                            else
                            {
                                // Lấy bookingCode hiện tại
                                using (var cmd = new SqlCommand("SELECT BookingCode FROM dbo.HotelBookings WHERE BookingId = @BID", conn, tran))
                                {
                                    cmd.Parameters.AddWithValue("@BID", bookingId);
                                    var obj = cmd.ExecuteScalar();
                                    if (obj != null && obj != DBNull.Value) bookingCode = obj.ToString();
                                }

                                // Cập nhật booking
                                using (var cmd = new SqlCommand("UPDATE dbo.HotelBookings SET Status = @Status, CustomerID = @CustomerID WHERE BookingId = @BID", conn, tran))
                                {
                                    cmd.Parameters.AddWithValue("@Status", targetStatus);
                                    cmd.Parameters.AddWithValue("@CustomerID", customerId == 0 ? (object)DBNull.Value : customerId);
                                    cmd.Parameters.AddWithValue("@BID", bookingId);
                                    cmd.ExecuteNonQuery();
                                }

                                // Cập nhật booking detail
                                decimal appliedPrice = GetRoomPrice();
                                using (var cmd = new SqlCommand(@"UPDATE dbo.BookingDetails SET CheckIn=@CheckIn, CheckOut=@CheckOut, AppliedPrice=@Price WHERE BookingID=@BookingID AND RoomID=@RoomID", conn, tran))
                                {
                                    cmd.Parameters.AddWithValue("@BookingID", bookingId);
                                    cmd.Parameters.AddWithValue("@RoomID", labMaphong.Text);
                                    cmd.Parameters.Add(new SqlParameter("@CheckIn", System.Data.SqlDbType.DateTime) { Value = start });
                                    cmd.Parameters.Add(new SqlParameter("@CheckOut", System.Data.SqlDbType.DateTime) { Value = end });
                                    cmd.Parameters.AddWithValue("@Price", appliedPrice);
                                    cmd.ExecuteNonQuery();
                                }
                            }

                            // Cập nhật trạng thái phòng
                            using (var cmd = new SqlCommand("UPDATE dbo.Rooms SET Status = @Status WHERE RoomId = @RoomID", conn, tran))
                            {
                                cmd.Parameters.AddWithValue("@Status", targetStatus);
                                cmd.Parameters.AddWithValue("@RoomID", labMaphong.Text);
                                cmd.ExecuteNonQuery();
                            }

                            // Lưu dịch vụ
                            var servicesToSave = CollectServicesFromGrid();
                            UpsertBookingServices(conn, tran, bookingId, servicesToSave);

                            tran.Commit();
                            dbSaved = true;

                            info.Email = customerEmail;
                            info.Phone = customerPhone;
                            info.IdCard = customerCCCD;
                            info.Gender = customerGender;
                            info.Nationality = customerNationality;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lưu vào CSDL thất bại: " + ex.Message, "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return; // Không tiếp tục nếu lưu CSDL thất bại
                }
            }

            if (!dbSaved)
            {
                MessageBox.Show("Không thể lưu vào CSDL. Vui lòng kiểm tra kết nối!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Cập nhật UI
            BookingManager.AddBooking(labMaphong.Text, info);
            _existing = info;
            _btnThanhToan.Visible = true;
            UpdatePaymentButtonByState(start, end);

            try
            {
                var listForm = Application.OpenForms.OfType<ListRoom>().FirstOrDefault();
                if (listForm != null)
                {
                    listForm.RefreshFromBookings(new[] { labMaphong.Text });
                }
            }
            catch { }

            // Gửi email xác nhận và thông báo kết quả
            bool emailSent = false;
            if (!string.IsNullOrWhiteSpace(customerEmail))
            {
                try
                {
                    var bookingRowInfo = new BookingRowInfo
                    {
                        Id = !string.IsNullOrWhiteSpace(bookingCode) ? bookingCode : labMaphong.Text,
                        Customer = info.Customer,
                        Date = DateTime.Now.ToString("dd/MM/yyyy"),
                        Detail = $"{labMaphong.Text} ({start:dd/MM/yyyy HH:mm} - {end:dd/MM/yyyy HH:mm})",
                        CCCD = info.IdCard,
                        SDT = info.Phone,
                        Email = customerEmail,
                        Gender = info.Gender,
                        Nationality = info.Nationality,
                        CreatedDate = DateTime.Now
                    };
                    EmailHelper.SendBookingConfirmation(bookingRowInfo);
                    emailSent = true;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Gửi email thất bại: {ex.Message}");
                }
            }

            // Thông báo kết quả
            string message = targetStatus.Contains("đặt") ? "Đã đặt phòng thành công!" : "Đã lưu thay đổi thành công!";
            if (!string.IsNullOrWhiteSpace(customerEmail))
            {
                message += emailSent ? "\n✓ Email xác nhận đã được gửi cho khách hàng." : "\n✗ Không thể gửi email xác nhận.";
            }
            MessageBox.Show(message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show("Bạn có chắc chắn muốn thoát?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }



        // Trong file Room_Details.cs

        private void AddService()
        {
            // Kiểm tra xem phòng đã có Booking trong Database chưa
            var checkBooking = GetActiveBookingFromDb(labMaphong.Text);
            
            if (checkBooking == null)
            {
                if (string.IsNullOrWhiteSpace(txtName.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên khách hàng trước khi thêm dịch vụ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtName.Focus();
                    return;
                }
                
                var confirm = MessageBox.Show("Phòng này chưa có booking. Tạo booking mới trước khi thêm dịch vụ?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm != DialogResult.Yes) return;
                
                // Tạo booking ngầm (không hiện thông báo)
                CreateBookingSilently();
            }

            // Mở form dịch vụ
            using (var f = new HotelServices_Form())
            {
                f.CurrentRoomID = labMaphong.Text.Trim();
                if (f.ShowDialog(this) == DialogResult.OK)
                {
                    LoadServicesForBooking(labMaphong.Text);
                    EnsureRoomCharge((int)nNgay.Value);
                }
            }
        }

        private void CreateBookingSilently()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text)) return;

            DateTime start = dtNgay.Value.Date + dtGio.Value.TimeOfDay;
            int days = Math.Max(1, (int)nNgay.Value);
            DateTime end = start.AddDays(days);
            string status = guna2ComboBox1.SelectedItem?.ToString() ?? "Phòng đang thuê";

            var connStr = ConfigurationManager.ConnectionStrings["ConnStr"]?.ConnectionString;
            if (string.IsNullOrWhiteSpace(connStr)) return;

            try
            {
                using (var conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    using (var tran = conn.BeginTransaction())
                    {
                        int customerId = 0;
                        using (var cmd = new SqlCommand("SELECT TOP 1 CustomerId FROM dbo.Customers WHERE FullName = @Name", conn, tran))
                        {
                            cmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
                            var obj = cmd.ExecuteScalar();
                            if (obj != null && obj != DBNull.Value) customerId = Convert.ToInt32(obj);
                        }

                        if (customerId == 0)
                        {
                            using (var cmd = new SqlCommand("INSERT INTO dbo.Customers(FullName) VALUES(@Name); SELECT CAST(SCOPE_IDENTITY() AS INT);", conn, tran))
                            {
                                cmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
                                var obj = cmd.ExecuteScalar();
                                if (obj != null && obj != DBNull.Value) customerId = Convert.ToInt32(obj);
                            }
                        }

                        string bookingCode = "PT" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
                        int bookingId = 0;
                        using (var cmd = new SqlCommand(@"INSERT INTO dbo.HotelBookings(BookingCode, CustomerID, EmployeeID, CreatedDate, Status)
VALUES(@Code, @CustomerID, 1, GETDATE(), @Status); SELECT CAST(SCOPE_IDENTITY() AS INT);", conn, tran))
                        {
                            cmd.Parameters.AddWithValue("@Code", bookingCode);
                            cmd.Parameters.AddWithValue("@CustomerID", customerId);
                            cmd.Parameters.AddWithValue("@Status", status);
                            var obj = cmd.ExecuteScalar();
                            if (obj != null && obj != DBNull.Value) bookingId = Convert.ToInt32(obj);
                        }

                        using (var cmd = new SqlCommand(@"INSERT INTO dbo.BookingDetails(BookingID, RoomID, CheckIn, CheckOut, AppliedPrice)
VALUES(@BID, @RoomID, @CheckIn, @CheckOut, @Price);", conn, tran))
                        {
                            cmd.Parameters.AddWithValue("@BID", bookingId);
                            cmd.Parameters.AddWithValue("@RoomID", labMaphong.Text.Trim());
                            cmd.Parameters.Add(new SqlParameter("@CheckIn", System.Data.SqlDbType.DateTime) { Value = start });
                            cmd.Parameters.Add(new SqlParameter("@CheckOut", System.Data.SqlDbType.DateTime) { Value = end });
                            cmd.Parameters.AddWithValue("@Price", GetRoomPrice());
                            cmd.ExecuteNonQuery();
                        }

                        using (var cmd = new SqlCommand("UPDATE dbo.Rooms SET Status = @Status WHERE RoomId = @RoomID", conn, tran))
                        {
                            cmd.Parameters.AddWithValue("@Status", status);
                            cmd.Parameters.AddWithValue("@RoomID", labMaphong.Text.Trim());
                            cmd.ExecuteNonQuery();
                        }

                        tran.Commit();
                        
                        // Cập nhật _existing để form biết đã có booking
                        _existing = new BookingInfo { Customer = txtName.Text.Trim(), Start = start, End = end };
                        _btnThanhToan.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("CreateBookingSilently error: " + ex.Message);
            }
        }

        private void AddOrUpdateServiceRow(string name, decimal unitPrice, int qty, bool overwriteQuantity = true)
        {
            if (qty < 1) qty = 1;
            foreach (DataGridViewRow r in guna2DataGridView2.Rows)
            {
                if (r.IsNewRow) continue;
                if (string.Equals(Convert.ToString(r.Cells[0].Value), name, StringComparison.OrdinalIgnoreCase))
                {
                    int oldQty = 0;
                    int.TryParse(Convert.ToString(r.Cells[1].Value), out oldQty);
                    int newQty = overwriteQuantity ? qty : oldQty + qty;
                    decimal amount = unitPrice * newQty;
                    r.Cells[0].Value = name;
                    r.Cells[1].Value = newQty;
                    r.Cells[2].Value = unitPrice.ToString("N0");
                    r.Cells[3].Value = amount.ToString("N0");
                    return;
                }
            }

            int idx = guna2DataGridView2.Rows.Add();
            var row = guna2DataGridView2.Rows[idx];
            decimal amountNew = unitPrice * qty;
            row.Cells[0].Value = name;
            row.Cells[1].Value = qty;
            row.Cells[2].Value = unitPrice.ToString("N0");
            row.Cells[3].Value = amountNew.ToString("N0");
        }

        private void RemoveService()
        {
            if (guna2DataGridView2.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow r in guna2DataGridView2.SelectedRows)
                {
                    if (r.IsNewRow) continue;
                    
                    // Không cho xóa Tiền phòng và Ăn sáng VIP
                    string serviceName = Convert.ToString(r.Cells[0].Value);
                    if (string.Equals(serviceName, ROOM_SERVICE_NAME, StringComparison.OrdinalIgnoreCase) ||
                        string.Equals(serviceName, "Ăn sáng (VIP)", StringComparison.OrdinalIgnoreCase))
                    {
                        MessageBox.Show($"Không thể xóa dịch vụ '{serviceName}'!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        continue;
                    }
                    
                    guna2DataGridView2.Rows.Remove(r);
                }
            }
            EnsureRoomCharge((int)nNgay.Value);
        }

        private void LoadServicesToGrid(List<ServiceItem> services)
        {
            guna2DataGridView2.Rows.Clear();
            if (services == null) return;
            foreach (var s in services)
            {
                int idx = guna2DataGridView2.Rows.Add();
                var row = guna2DataGridView2.Rows[idx];
                row.Cells[0].Value = s.Name;
                row.Cells[1].Value = s.Quantity;
                row.Cells[2].Value = s.UnitPrice.ToString("N0");
                row.Cells[3].Value = s.Amount.ToString("N0");
            }
        }

        private List<ServiceItem> CollectServicesFromGrid()
        {
            var list = new List<ServiceItem>();
            foreach (DataGridViewRow r in guna2DataGridView2.Rows)
            {
                if (r.IsNewRow) continue;
                string ten = Convert.ToString(r.Cells[0].Value);
                decimal price = 0;
                decimal.TryParse(Convert.ToString(r.Cells[2].Value), NumberStyles.Any, CultureInfo.CurrentCulture, out price);
                int qty = 1;
                int.TryParse(Convert.ToString(r.Cells[1].Value), out qty);
                if (string.IsNullOrWhiteSpace(ten)) continue;
                decimal amount = price * qty;
                r.Cells[3].Value = amount.ToString("N0");
                list.Add(new ServiceItem { Name = ten.Trim(), UnitPrice = price, Quantity = qty, Amount = amount });
            }
            return list;
        }

        private void SyncServicesToBooking()
        {
            var sv = CollectServicesFromGrid();
            if (_existing == null) _existing = new BookingInfo();
            _existing.Services = sv;
            BookingManager.AddBooking(labMaphong.Text, _existing);
        }

        private bool IsVipRoom()
        {
            var code = labMaphong.Text;
            if (string.IsNullOrWhiteSpace(code)) return false;
            int num;
            if (!int.TryParse(code.Trim().TrimStart('P', 'p'), out num)) return false;
            // Phòng đơn: 001-004 VIP, phòng đôi: 013-015 VIP, phòng gia đình: 021-023 VIP
            if (num >= 1 && num <= 12) return num <= 4;
            if (num >= 13 && num <= 20) return num <= 15;
            if (num >= 21) return num <= 23;
            return false;
        }

        private void EnsureVipBreakfast()
        {
            if (!IsVipRoom()) return;
            const string breakfastName = "Ăn sáng (VIP)";
            foreach (DataGridViewRow r in guna2DataGridView2.Rows)
            {
                if (r.IsNewRow) continue;
                if (string.Equals(Convert.ToString(r.Cells[0].Value), breakfastName, StringComparison.OrdinalIgnoreCase))
                    return; // đã có
            }

            int idx = guna2DataGridView2.Rows.Add();
            var row = guna2DataGridView2.Rows[idx];
            row.Cells[0].Value = breakfastName;
            row.Cells[1].Value = 1;
            row.Cells[2].Value = 0;
            row.Cells[3].Value = 0;
        }

        private bool IsBookedNotCheckedIn()
        {
            var statusText = guna2ComboBox1.SelectedItem as string;
            if (string.IsNullOrWhiteSpace(statusText)) statusText = SelectedStatus;
            if (!string.IsNullOrWhiteSpace(statusText) && statusText.ToLower().Contains("đặt"))
                return true;
            return false;
        }

        // --- HÀM NÀY ĐÃ ĐƯỢC SỬA ĐỂ GỌI DATABASE ---
        private decimal GetRoomPrice()
        {
            return GetCurrentPriceFromDB(labMaphong.Text);
        }

        private void UpdateVipBadge()
        {
            if (lblVipBadge != null)
            {
                lblVipBadge.Visible = IsVipRoom();
            }
        }

        private void UpdatePaymentButtonByState(DateTime start, DateTime end)
        {
            if (_btnThanhToan == null) return;
            if (IsBookedNotCheckedIn())
            {
                _btnThanhToan.Text = "Hủy phòng";
                _btnThanhToan.FillColor = Color.FromArgb(220, 53, 69);
            }
            else
            {
                _btnThanhToan.Text = "Thanh toán";
                _btnThanhToan.FillColor = Color.FromArgb(255, 159, 67);
            }
        }

        private void SetStatusSelection(string status)
        {
            if (guna2ComboBox1 == null || status == null) return;
            string normalized = status.ToLower();
            if (normalized.Contains("thuê"))
                guna2ComboBox1.SelectedItem = "Phòng đang thuê";
            else if (normalized.Contains("đặt"))
                guna2ComboBox1.SelectedItem = "Phòng đã đặt";
            else
                guna2ComboBox1.SelectedIndex = -1;
            SelectedStatus = guna2ComboBox1.SelectedItem as string;

            var start = dtNgay.Value.Date + dtGio.Value.TimeOfDay;
            var end = start.AddDays((int)nNgay.Value);
            UpdatePaymentButtonByState(start, end);
        }

        private void SetRoomTypeByCode(string maPhong)
        {
            if (string.IsNullOrWhiteSpace(maPhong) || guna2ComboBox2 == null) return;
            int num;
            if (int.TryParse(maPhong.Trim().TrimStart('P', 'p'), out num))
            {
                if (num >= 1 && num <= 12)
                    guna2ComboBox2.SelectedItem = "Phòng đơn";
                else if (num >= 13 && num <= 24)
                    guna2ComboBox2.SelectedItem = "Phòng đôi";
                else
                    guna2ComboBox2.SelectedItem = "Phòng gia đình";
            }
            SelectedRoomType = guna2ComboBox2.SelectedItem as string;
            if (guna2ComboBox2 != null) guna2ComboBox2.Enabled = false;
        }

        private void SetRoomTypeByName(string loaiPhong)
        {
            if (guna2ComboBox2 == null) return;
            if (string.IsNullOrWhiteSpace(loaiPhong)) { SetRoomTypeByCode(labMaphong.Text); return; }
            string low = loaiPhong.ToLowerInvariant();
            if (low.Contains("đơn")) guna2ComboBox2.SelectedItem = "Phòng đơn";
            else if (low.Contains("đôi")) guna2ComboBox2.SelectedItem = "Phòng đôi";
            else guna2ComboBox2.SelectedItem = "Phòng gia đình";
            SelectedRoomType = guna2ComboBox2.SelectedItem as string;
            guna2ComboBox2.Enabled = false;
        }

        private void Room_Details_Load(object sender, EventArgs e)
        {

        }

        private void DtGio_Click(object sender, EventArgs e)
        {
            ShowTimePicker();
        }

        private void DtGio_MouseDown(object sender, MouseEventArgs e)
        {
            ShowTimePicker();
        }

        private void ShowTimePicker()
        {
            using (var picker = new TimePickerForm(dtGio.Value))
            {
                if (picker.ShowDialog(this) == DialogResult.OK)
                {
                    var sel = picker.SelectedDateTime;
                    dtGio.Value = new DateTime(dtNgay.Value.Year, dtNgay.Value.Month, dtNgay.Value.Day, sel.Hour, sel.Minute, sel.Second);
                }
            }
        }

        private bool CancelBookingInDb(string roomId, DateTime start, DateTime end)
        {
            var connStr = ConfigurationManager.ConnectionStrings["ConnStr"]?.ConnectionString;
            if (string.IsNullOrWhiteSpace(connStr)) return false;
            try
            {
                using (var conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    using (var tran = conn.BeginTransaction())
                    {
                        try
                        {
                            var findSql = @"SELECT DISTINCT d.BookingID
FROM dbo.BookingDetails d
JOIN dbo.HotelBookings b ON d.BookingID = b.BookingID
WHERE d.RoomID = @RoomID
  AND b.Status NOT IN (N'Paid', N'Cancelled')
  AND d.CheckIn <= @End AND d.CheckOut >= @Start";

                            var affected = new List<int>();
                            using (var cmd = new SqlCommand(findSql, conn, tran))
                            {
                                cmd.Parameters.AddWithValue("@RoomID", roomId);
                                cmd.Parameters.Add(new SqlParameter("@Start", System.Data.SqlDbType.DateTime) { Value = start });
                                cmd.Parameters.Add(new SqlParameter("@End", System.Data.SqlDbType.DateTime) { Value = end });
                                using (var rd = cmd.ExecuteReader())
                                {
                                    while (rd.Read())
                                    {
                                        if (rd[0] != DBNull.Value) affected.Add(Convert.ToInt32(rd[0]));
                                    }
                                }
                            }

                            if (affected.Count == 0)
                            {
                                tran.Rollback();
                                return false;
                            }

                            var idsParam = string.Join("," , affected);
                            
                            // 1. Xóa BookingServices của phòng này trước (tránh lỗi FK)
                            var delServicesSql = $@"DELETE FROM dbo.BookingServices 
                                WHERE BookingID IN ({idsParam}) AND RoomID = @RoomID";
                            using (var delSvc = new SqlCommand(delServicesSql, conn, tran))
                            {
                                delSvc.Parameters.AddWithValue("@RoomID", roomId);
                                delSvc.ExecuteNonQuery();
                            }
                            
                            // 2. Xóa BookingDetails
                            var delSql = $@"DELETE FROM dbo.BookingDetails
WHERE BookingID IN ({idsParam})
  AND RoomID = @RoomID
  AND (CheckIn <= @End AND CheckOut >= @Start)";
                            using (var del = new SqlCommand(delSql, conn, tran))
                            {
                                del.Parameters.AddWithValue("@RoomID", roomId);
                                del.Parameters.Add(new SqlParameter("@Start", System.Data.SqlDbType.DateTime) { Value = start });
                                del.Parameters.Add(new SqlParameter("@End", System.Data.SqlDbType.DateTime) { Value = end });
                                del.ExecuteNonQuery();
                            }

                            // 3. Đánh dấu booking là Cancelled nếu không còn detail nào
                            var updSql = $@"UPDATE dbo.HotelBookings
SET Status = N'Cancelled'
WHERE BookingID IN ({idsParam})
  AND NOT EXISTS (SELECT 1 FROM dbo.BookingDetails d WHERE d.BookingID = dbo.HotelBookings.BookingID)";
                            using (var upd = new SqlCommand(updSql, conn, tran))
                            {
                                upd.ExecuteNonQuery();
                            }

                            // 4. Cập nhật trạng thái phòng về Phòng Trống
                            using (var cmdRoom = new SqlCommand("UPDATE dbo.Rooms SET Status = @Status WHERE RoomId = @RoomID", conn, tran))
                            {
                                cmdRoom.Parameters.AddWithValue("@Status", "Phòng Trống");
                                cmdRoom.Parameters.AddWithValue("@RoomID", roomId);
                                cmdRoom.ExecuteNonQuery();
                            }

                            tran.Commit();
                            return true;
                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();
                            Debug.WriteLine("CancelBookingInDb error: " + ex.Message);
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("CancelBookingInDb connection error: " + ex.Message);
                return false;
            }
        }

        private void LoadServicesForBooking(string roomId)
        {
            var connStr = ConfigurationManager.ConnectionStrings["ConnStr"]?.ConnectionString;
            if (string.IsNullOrWhiteSpace(connStr)) return;

            try
            {
                using (var conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    guna2DataGridView2.Rows.Clear();

                    string sqlGetID = @"SELECT TOP 1 BookingId 
                                FROM dbo.BookingDetails 
                                WHERE RoomId = @RoomID 
                                AND BookingId IN (SELECT BookingId FROM dbo.HotelBookings WHERE Status NOT IN (N'Paid', N'Cancelled'))";

                    int bookingId = 0;
                    using (var cmd = new SqlCommand(sqlGetID, conn))
                    {
                        cmd.Parameters.AddWithValue("@RoomID", roomId.Trim());
                        var obj = cmd.ExecuteScalar();
                        if (obj != null && obj != DBNull.Value) bookingId = Convert.ToInt32(obj);
                    }

                    if (bookingId > 0)
                    {
                        // Chỉ lấy dịch vụ của PHÒNG NÀY (không lấy dịch vụ phòng khác dù cùng booking)
                        string sqlSvc = @"SELECT ServiceName, UnitPrice, Quantity 
                                         FROM dbo.BookingServices 
                                         WHERE BookingID = @BID AND (RoomID = @RoomID OR RoomID IS NULL)";
                        using (var cmd = new SqlCommand(sqlSvc, conn))
                        {
                            cmd.Parameters.AddWithValue("@BID", bookingId);
                            cmd.Parameters.AddWithValue("@RoomID", roomId.Trim());
                            using (var rd = cmd.ExecuteReader())
                            {
                                while (rd.Read())
                                {
                                    string name = rd["ServiceName"].ToString();
                                    decimal price = Convert.ToDecimal(rd["UnitPrice"]);
                                    int qty = Convert.ToInt32(rd["Quantity"]);
                                    AddOrUpdateServiceRow(name, price, qty, overwriteQuantity: true);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Lỗi load dịch vụ: " + ex.Message);
            }
        }
    }
}