using Guna.UI2.WinForms;
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
using QLChuoiNhaHangKhachSan.GUI;

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
        // simple strategy: clear then re-insert
        using (var del = new SqlCommand("DELETE FROM dbo.BookingServices WHERE BookingID = @BID", conn, tran))
        {
            del.Parameters.AddWithValue("@BID", bookingId);
            del.ExecuteNonQuery();
        }
        if (services == null || services.Count == 0) return;
        foreach (var s in services)
        {
            using (var ins = new SqlCommand(@"INSERT INTO dbo.BookingServices(BookingID, ServiceName, Quantity, UnitPrice, TotalAmount)
VALUES(@BID, @Name, @Qty, @Price, @Total)", conn, tran))
            {
                ins.Parameters.AddWithValue("@BID", bookingId);
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

                bool ok = CancelBookingInDb(labMaphong.Text, start, end);
                if (ok)
                {
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

                    // 2. Gọi hàm xóa dưới Database
                    bool dbSuccess = CancelBookingInDb(labMaphong.Text, start, end);

                    if (dbSuccess)
                    {
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

            var sfd = new SaveFileDialog
            {
                Title = "Lưu hóa đơn",
                Filter = "Text Files (*.txt)|*.txt|All files (*.*)|*.*",
                FileName = $"HoaDon_{labMaphong.Text}_{DateTime.Now:yyyyMMddHHmm}.txt"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                EnsureRoomCharge((int)nNgay.Value);
                SyncServicesToBooking(); // cập nhật dịch vụ vào booking trước khi in
                WriteInvoiceText(sfd.FileName);
                MessageBox.Show("Đã lưu hóa đơn (TXT).", "Thông báo");

                // compute totalAmount from grid (same logic you already have)
                decimal totalAmount = 0m;
                var services = CollectServicesFromGrid();
                foreach (var s in services) totalAmount += s.Amount;

                // Use internal SaveInvoiceToDatabase to persist and finalize booking
                try
                {
                    SaveInvoiceToDatabase(sfd.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lưu hóa đơn vào DB thất bại: " + ex.Message + "\nHủy thao tác hoàn tất phòng. Vui lòng kiểm tra kết nối CSDL.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // do NOT remove in-memory booking or change UI when DB persistence failed
                }

                // if we reach here DB save succeeded -> remove in-memory booking and refresh UI
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
                        // Refresh only the room just paid — RefreshBookingStates will read DB + in-memory and show free
                        listForm.RefreshFromBookings(new[] { labMaphong.Text });
                     }
                }
                catch { }
            }
        }

        /// <summary>
        /// Best-effort: insert a minimal payment/invoice row to database for records and finalize booking.
        /// This will mark Booking(s) that overlap current booking interval as Paid and set Room.Status = N'Phòng Trống'.
        /// Errors are rethrown to caller for logging/diagnostics.
        /// </summary>
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
                        // Sửa câu truy vấn để chắc chắn tìm ra booking gần nhất chưa thanh toán
                        string findSql = @"
                    SELECT TOP 1 b.BookingId 
                    FROM dbo.HotelBookings b
                    JOIN dbo.BookingDetails d ON b.BookingId = d.BookingId
                    WHERE d.RoomId = @RoomID 
                      AND b.Status NOT IN (N'Paid', N'Cancelled') 
                    ORDER BY b.CreatedDate DESC";

                        using (var cmd = new SqlCommand(findSql, conn, tran))
                        {
                            // QUAN TRỌNG: Thêm .Trim() để tránh lỗi dư khoảng trắng (ví dụ 'P001 ' khác 'P001')
                            cmd.Parameters.AddWithValue("@RoomID", labMaphong.Text.Trim());
                            var obj = cmd.ExecuteScalar();
                            if (obj != null && obj != DBNull.Value) foundBookingId = Convert.ToInt32(obj);
                        }

                        // 3. Kiểm tra xem có tìm thấy booking không
                        if (foundBookingId > 0)
                        {
                            // Cập nhật Booking thành Paid
                            using (var upd = new SqlCommand("UPDATE dbo.HotelBookings SET Status = N'Paid' WHERE BookingId = @BID", conn, tran))
                            {
                                upd.Parameters.AddWithValue("@BID", foundBookingId);
                                upd.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            // Nếu không tìm thấy booking dưới DB nhưng trên phần mềm vẫn đang hiển thị
                            // Có thể do dữ liệu chưa được Lưu (Save) xuống DB trước khi bấm Thanh toán
                            // Ta vẫn cho phép chạy tiếp để trả phòng về Trống, nhưng nên cảnh báo nhẹ hoặc log lại
                            System.Diagnostics.Debug.WriteLine("Cảnh cáo: Không tìm thấy BookingID dưới CSDL để cập nhật Paid.");
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

                        // 5. Quan trọng: Trả phòng về trạng thái "Phòng Trống"
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
                        // Ném lỗi ra để BtnThanhToan_Click bắt được và hiện thông báo
                        throw new Exception("Lỗi Database: " + ex.Message);
                    }
                }
            }
        }
        /// <summary>
        /// Update room status in database.
        /// </summary>
        private void UpdateRoomStatusInDatabase(string roomCode, string status)
        {
            if (string.IsNullOrWhiteSpace(roomCode)) return;
            var connStr = ConfigurationManager.ConnectionStrings["ConnStr"]?.ConnectionString;
            if (string.IsNullOrWhiteSpace(connStr)) return;

            using (var conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("UPDATE dbo.Rooms SET Status = @Status WHERE RoomId = @RoomID", conn))
            {
                cmd.Parameters.AddWithValue("@RoomID", roomCode);
                cmd.Parameters.AddWithValue("@Status", status);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private void WriteInvoiceText(string path)
        {
            if (_existing == null) return;
            string room = labMaphong.Text;
            DateTime start = _existing.Start;
            DateTime end = _existing.End;
            int days = Math.Max(1, (int)Math.Ceiling((end - start).TotalDays));

            // Always re-sync services to ensure latest totals
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
                // đảm bảo amount chính xác
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
            _lblCheckout.Text = $"Check-out: {end:đd/MM/yyyy HH:mm} | Số ngày: {days}";
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

            this.TenKhach = txtName.Text;
            this.SoNgay = (int)nNgay.Value;
            this.SelectedStatus = guna2ComboBox1.SelectedItem != null ? guna2ComboBox1.SelectedItem.ToString() : null;
            this.SelectedRoomType = guna2ComboBox2.SelectedItem != null ? guna2ComboBox2.SelectedItem.ToString() : null;

            DateTime start = dtNgay.Value.Date + dtGio.Value.TimeOfDay;
            
            // QUAN TRỌNG: Đảm bảo số ngày ít nhất là 1
            if (this.SoNgay <= 0)
                this.SoNgay = 1;
            
            DateTime end = start.AddDays(this.SoNgay);
            
            // KIỂM TRA: Nếu end <= start thì tự động set end = start + 1 ngày
            if (end <= start)
            {
                this.SoNgay = 1;
                end = start.AddDays(1);
                MessageBox.Show($"Cảnh báo: Thời gian kết thúc không hợp lệ. Đã tự động điều chỉnh về {end:dd/MM/yyyy HH:mm}", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
            // Log for debugging DB values
            Debug.WriteLine($"[PerformSave] Room={labMaphong.Text} Start={start:yyyy-MM-dd HH:mm:ss} End={end:yyyy-MM-dd HH:mm:ss} Days={this.SoNgay}");
            
            var info = new BookingInfo { Customer = this.TenKhach, Start = start, End = end, Services = CollectServicesFromGrid() };
            // đảm bảo tiền phòng luôn có trong service
            EnsureRoomCharge(this.SoNgay);
            EnsureVipBreakfast();
            info.Services = CollectServicesFromGrid();

            // Try to persist booking to database so ListRoom reads it from DB
            var connStr = ConfigurationManager.ConnectionStrings["ConnStr"]?.ConnectionString;
            bool dbSaved = false;
            int bookingId = 0;
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
                            if (!string.IsNullOrWhiteSpace(this.TenKhach))
                            {
                                using (var cmd = new SqlCommand("SELECT TOP 1 CustomerId FROM dbo.Customers WHERE FullName = @Name", conn, tran))
                                {
                                    cmd.Parameters.AddWithValue("@Name", this.TenKhach);
                                    var obj = cmd.ExecuteScalar();
                                    if (obj != null && obj != DBNull.Value) customerId = Convert.ToInt32(obj);
                                }
                            }

                            if (customerId == 0 && !string.IsNullOrWhiteSpace(this.TenKhach))
                            {
                                using (var cmd = new SqlCommand(@"INSERT INTO dbo.Customers(FullName) VALUES(@Name); SELECT CAST(SCOPE_IDENTITY() AS INT);", conn, tran))
                                {
                                    cmd.Parameters.AddWithValue("@Name", this.TenKhach);
                                    var obj = cmd.ExecuteScalar();
                                    if (obj != null && obj != DBNull.Value) customerId = Convert.ToInt32(obj);
                                }
                            }

                            int employeeId = 0;
                            using (var cmd = new SqlCommand("SELECT TOP 1 EmployeeID FROM dbo.Employee ORDER BY EmployeeID", conn, tran))
                            {
                                var obj = cmd.ExecuteScalar();
                                if (obj != null && obj != DBNull.Value) employeeId = Convert.ToInt32(obj);
                            }

                            // determine bookingId: try existing, else create new
                            bookingId = GetActiveBookingId(conn, tran, labMaphong.Text.Trim());

                            if (bookingId == 0)
                            {
                                string bookingCode = "PT" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
                                using (var cmd = new SqlCommand(@"INSERT INTO dbo.Booking(BookingCode, CustomerID, EmployeeID, CreatedDate, Status)
    VALUES(@Code, @CustomerID, @EmployeeID, GETDATE(), @Status);
    SELECT CAST(SCOPE_IDENTITY() AS INT);", conn, tran))
                                {
                                    cmd.Parameters.AddWithValue("@Code", bookingCode);
                                    cmd.Parameters.AddWithValue("@CustomerID", customerId == 0 ? (object)DBNull.Value : customerId);
                                    cmd.Parameters.AddWithValue("@EmployeeID", employeeId == 0 ? (object)DBNull.Value : employeeId);
                                    cmd.Parameters.AddWithValue("@Status", string.IsNullOrWhiteSpace(SelectedStatus) ? (object)"Đặt" : SelectedStatus);
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
                                // update booking status/customer if needed
                                using (var cmd = new SqlCommand("UPDATE dbo.Booking SET Status = @Status, CustomerID = @CustomerID WHERE BookingID = @BID", conn, tran))
                                {
                                    cmd.Parameters.AddWithValue("@Status", string.IsNullOrWhiteSpace(SelectedStatus) ? (object)"Đặt" : SelectedStatus);
                                    cmd.Parameters.AddWithValue("@CustomerID", customerId == 0 ? (object)DBNull.Value : customerId);
                                    cmd.Parameters.AddWithValue("@BID", bookingId);
                                    cmd.ExecuteNonQuery();
                                }

                                // update booking detail time/price
                                decimal appliedPrice = GetRoomPrice();
                                using (var cmd = new SqlCommand(@"UPDATE dbo.BookingDetails
SET CheckIn=@CheckIn, CheckOut=@CheckOut, AppliedPrice=@Price
WHERE BookingID=@BookingID AND RoomID=@RoomID", conn, tran))
                                {
                                    cmd.Parameters.AddWithValue("@BookingID", bookingId);
                                    cmd.Parameters.AddWithValue("@RoomID", labMaphong.Text);
                                    cmd.Parameters.Add(new SqlParameter("@CheckIn", System.Data.SqlDbType.DateTime) { Value = start });
                                    cmd.Parameters.Add(new SqlParameter("@CheckOut", System.Data.SqlDbType.DateTime) { Value = end });
                                    cmd.Parameters.AddWithValue("@Price", appliedPrice);
                                    cmd.ExecuteNonQuery();
                                }
                            }

                            // Update room status in DB
                            using (var cmd = new SqlCommand("UPDATE dbo.Rooms SET Status = @Status WHERE RoomId = @RoomID", conn, tran))
                            {
                                cmd.Parameters.AddWithValue("@Status", string.IsNullOrWhiteSpace(SelectedStatus) ? (object)"Đặt" : SelectedStatus);
                                cmd.Parameters.AddWithValue("@RoomID", labMaphong.Text);
                                cmd.ExecuteNonQuery();
                            }

                            // Upsert services for this booking
                            var servicesToSave = CollectServicesFromGrid();
                            UpsertBookingServices(conn, tran, bookingId, servicesToSave);

                            tran.Commit();
                            Debug.WriteLine($"[PerformSave][DB] Room={labMaphong.Text} Start={start:yyyy-MM-dd HH:mm:ss} End={end:yyyy-MM-dd HH:mm:ss}");
                            dbSaved = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lưu vào CSDL thất bại: " + ex.Message, "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dbSaved = false;
                }
            }

            // Keep in-memory booking for immediate UI and fallback when DB save fails
            BookingManager.AddBooking(labMaphong.Text, info);
            _existing = info;
            _btnThanhToan.Visible = true;
            UpdatePaymentButtonByState(start, end);

            try
            {
                var listForm = Application.OpenForms.OfType<ListRoom>().FirstOrDefault();
                if (listForm != null)
                {
                    listForm.MarkRoomsAsRented(new[] { labMaphong.Text }, info.Customer, info.End, info.Start);
                }
            }
            catch { }

            MessageBox.Show("Đã lưu thay đổi thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

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
            // BƯỚC 1: Đảm bảo phòng đã có Booking trong Database trước khi mở form Dịch vụ
            // Nếu phòng đang trống hoặc chưa có ID booking, ta gọi hàm Lưu để tạo Booking trước
            if (IsBookedNotCheckedIn() || (SelectedStatus != null && SelectedStatus.ToLower().Contains("thuê")))
            {
                // Kiểm tra xem trong DB đã có booking chưa
                var checkBooking = GetBookingFromDb(labMaphong.Text, DateTime.Now);
                if (checkBooking == null)
                {
                    // Nếu chưa có trong DB (dù giao diện đang hiện có khách) -> Lưu ngay để tạo ID
                    BtnLuu_Click(null, null);
                }
            }
            else
            {
                // Nếu là phòng trống hoàn toàn -> Cảnh báo
                var confirm = MessageBox.Show("Phòng này chưa được đặt/nhận. Bạn có muốn tạo lượt khách mới không?", "Xác nhận", MessageBoxButtons.YesNo);
                if (confirm == DialogResult.Yes)
                {
                    BtnLuu_Click(null, null); // Tạo booking
                }
                else
                {
                    return; // Không làm gì cả
                }
            }

            // BƯỚC 2: Mở form dịch vụ (Lúc này chắc chắn đã có BookingID trong DB)
            using (var f = new HotelServices_Form())
            {
                f.CurrentRoomID = labMaphong.Text.Trim(); // Truyền mã phòng

                // Mở form chọn
                if (f.ShowDialog(this) == DialogResult.OK)
                {
                    // Sau khi form kia lưu xong, tải lại dữ liệu lên lưới bên này để hiển thị
                    LoadServicesForBooking(labMaphong.Text);

                    // Cập nhật lại tổng tiền hiển thị
                    EnsureRoomCharge((int)nNgay.Value);
                }
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
                    if (!r.IsNewRow) guna2DataGridView2.Rows.Remove(r);
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
                    // Combine the selected time with the date from dtNgay to avoid using
                    // the TimePicker's internal date which may differ and cause displayed
                    // start/end to look incorrect.
                    var sel = picker.SelectedDateTime;
                    dtGio.Value = new DateTime(dtNgay.Value.Year, dtNgay.Value.Month, dtNgay.Value.Day, sel.Hour, sel.Minute, sel.Second);
                }
            }
        }

        /// <summary>
        /// Cancel booking rows in DB that overlap the provided interval for the given room.
        /// Returns true when at least one booking detail was removed or room status updated.
        /// </summary>
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
                        // Use a single T-SQL batch to capture affected BookingIDs, delete details, update empty bookings and set room status.
                        var sql = @"DECLARE @Start DATETIME = @pStart;
DECLARE @End DATETIME = @pEnd;
CREATE TABLE #AffectedBookings(BookingID INT PRIMARY KEY);
INSERT INTO #AffectedBookings(BookingID)
SELECT DISTINCT BookingID FROM dbo.BookingDetails
WHERE RoomID = @pRoomID
AND (
    -- overlapping
    (CheckIn < @End AND CheckOut > @Start)
    OR (CheckIn >= @Start AND CheckIn < @End)
    OR (CheckOut > @Start AND CheckOut <= @End)
);

IF EXISTS(SELECT 1 FROM #AffectedBookings)
BEGIN
    DELETE d
    FROM dbo.BookingDetails d
    JOIN #AffectedBookings a ON d.BookingID = a.BookingID
    WHERE d.RoomID = @pRoomID
      AND (
        (d.CheckIn < @End AND d.CheckOut > @Start)
        OR (d.CheckIn >= @Start AND d.CheckIn < @End)
        OR (d.CheckOut > @Start && d.CheckOut <= @End)
      );

    -- For any booking that now has no details, mark Cancelled
    UPDATE b
    SET b.Status = N'Cancelled'
    FROM dbo.HotelBookings b
    JOIN #AffectedBookings a ON b.BookingID = a.BookingID
    WHERE NOT EXISTS (SELECT 1 FROM dbo.BookingDetails d WHERE d.BookingID = b.BookingID);

    -- Set room status to Phòng Trống
    UPDATE dbo.Rooms SET Status = @pStatus WHERE RoomID = @pRoomID;

    SELECT 1 AS RowsAffected;
END
ELSE
BEGIN
    SELECT 0 AS RowsAffected;
END";

                        using (var cmd = new SqlCommand(sql, conn, tran))
                        {
                            cmd.Parameters.Add(new SqlParameter("@pRoomID", System.Data.SqlDbType.NVarChar, 50) { Value = roomId });
                            cmd.Parameters.Add(new SqlParameter("@pStart", System.Data.SqlDbType.DateTime) { Value = start });
                            cmd.Parameters.Add(new SqlParameter("@pEnd", System.Data.SqlDbType.DateTime) { Value = end });
                            cmd.Parameters.Add(new SqlParameter("@pStatus", System.Data.SqlDbType.NVarChar, 50) { Value = "Phòng Trống" });
                            var res = cmd.ExecuteScalar();
                            int rows = 0;
                            if (res != null && int.TryParse(res.ToString(), out rows))
                            {
                                if (rows > 0)
                                {
                                    tran.Commit();
                                    return true;
                                }
                            }
                        }

                        tran.Rollback();
                        return false;
                     }
                 }
             }
             catch (Exception ex)
             {
                 Debug.WriteLine("CancelBookingInDb error: " + ex.Message);
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
                    // clear current grid to avoid double-counting
                    guna2DataGridView2.Rows.Clear();

                    string sqlGetID = @"SELECT TOP 1 BookingID 
                                FROM dbo.BookingDetails 
                                WHERE RoomID = @RoomID 
                                AND BookingID IN (SELECT BookingID FROM dbo.HotelBookings WHERE Status NOT IN (N'Paid', N'Cancelled'))";

                    int bookingId = 0;
                    using (var cmd = new SqlCommand(sqlGetID, conn))
                    {
                        cmd.Parameters.AddWithValue("@RoomID", roomId);
                        var obj = cmd.ExecuteScalar();
                        if (obj != null && obj != DBNull.Value) bookingId = Convert.ToInt32(obj);
                    }

                    if (bookingId > 0)
                    {
                        string sqlSvc = "SELECT ServiceName, UnitPrice, Quantity FROM dbo.BookingServices WHERE BookingID = @BID";
                        using (var cmd = new SqlCommand(sqlSvc, conn))
                        {
                            cmd.Parameters.AddWithValue("@BID", bookingId);
                            using (var rd = cmd.ExecuteReader())
                            {
                                while (rd.Read())
                                {
                                    string name = rd["ServiceName"].ToString();
                                    decimal price = Convert.ToDecimal(rd["UnitPrice"]);
                                    int qty = Convert.ToInt32(rd["Quantity"]);

                                    // overwrite quantity from DB to avoid doubling
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