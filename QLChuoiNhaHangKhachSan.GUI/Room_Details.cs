using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
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

            if (trangThai == "Phòng Trống")
            {
                btnNhanphong.Text = "Đặt phòng";
            }
            else if (trangThai == "Phòng đã đặt")
            {
                btnNhanphong.Text = "Nhận phòng";
            }
            else
            {
                btnNhanphong.Text = "Lưu thay đổi";
            }

            // Khóa ngày/giờ theo thời gian đang xem nếu được truyền vào
            ApplyLockedViewTime();

            // Thử lấy từ BookingManager trước, nếu không có thì lấy từ DB
            BookingManager.TryGetBooking(maPhong, out _existing);
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
                dtGio.Value = start;
                int days = Math.Max(1, (int)Math.Ceiling((end - start).TotalDays));
                nNgay.Value = days;
                UpdateCheckoutLabel(start, end, days);
                LoadServicesToGrid(_existing.Services);
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
                // MessageBox.Show("Lỗi lấy giá: " + ex.Message);
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
FROM dbo.BookingDetail d
JOIN dbo.Booking b ON d.BookingID = b.BookingID
JOIN dbo.Customer c ON b.CustomerID = c.CustomerID
WHERE d.RoomID = @RoomID AND d.CheckIn <= @RefTime AND d.CheckOut > @RefTime", conn))
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
                dtGio.Value = view;

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

        private void BtnThanhToan_Click(object sender, EventArgs e)
        {
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
                }
                return;
            }

            var sfd = new SaveFileDialog
            {
                Title = "Lưu hóa đơn",
                Filter = "Text Files (*.txt)|*.txt|All files (*.*)|*.*",
                FileName = $"HoaDon_{labMaphong.Text}_{DateTime.Now:yyyyMMddHHmmss}.txt"
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

                // best-effort save to DB and finalize — if DB save fails we abort finalizing so UI/db stay consistent
                try
                {
                    QLChuoiNhaHangKhachSan.GUI.Repositories.InvoiceRepository.SaveInvoiceAndFinalize(
                        labMaphong.Text, _existing, services, totalAmount, sfd.FileName);
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
        /// Best-effort: insert a minimal payment/invoice row to database for records.
        /// This uses a small generic table structure: dbo.Payment( RoomID, Amount, PaidDate, Note ).
        /// If your schema is different, adapt SQL to your actual invoice tables (Invoice/InvoiceDetail).
        /// Errors are rethrown to caller for logging/diagnostics.
        /// </summary>
        private void SaveInvoiceToDatabase(string invoiceFilePath)
        {
            decimal totalAmount = 0m;
            // compute total from current grid
            foreach (DataGridViewRow r in guna2DataGridView2.Rows)
            {
                if (r.IsNewRow) continue;
                decimal cell;
                if (decimal.TryParse(Convert.ToString(r.Cells[3].Value), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.CurrentCulture, out cell))
                    totalAmount += cell;
            }

            var connStr = ConfigurationManager.ConnectionStrings["ConnStr"]?.ConnectionString;
            if (string.IsNullOrWhiteSpace(connStr))
                return;

            using (var conn = new SqlConnection(connStr))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        // Try inserting into a generic Payment table if it exists.
                        // Adjust this SQL to match your real invoice schema if present.
                        using (var cmd = new SqlCommand(@"
IF OBJECT_ID('dbo.Payment', 'U') IS NOT NULL
BEGIN
    INSERT INTO dbo.Payment(RoomID, Amount, PaidDate, Note)
    VALUES(@RoomID, @Amount, @PaidDate, @Note);
END
ELSE
BEGIN
    -- If Payment table doesn't exist, write a lightweight audit record into Booking (fallback)
    INSERT INTO dbo.Booking(BookingCode, CustomerID, EmployeeID, CreatedDate, Status)
    VALUES(@FallbackCode, NULL, NULL, GETDATE(), N'Paid');
END
", conn, tran))
                        {
                            cmd.Parameters.AddWithValue("@RoomID", labMaphong.Text);
                            cmd.Parameters.AddWithValue("@Amount", totalAmount);
                            cmd.Parameters.AddWithValue("@PaidDate", DateTime.Now);
                            cmd.Parameters.AddWithValue("@Note", Path.GetFileName(invoiceFilePath) ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@FallbackCode", "INV" + DateTime.Now.ToString("yyyyMMddHHmmss"));
                            cmd.ExecuteNonQuery();
                        }

                        // commit only if above succeeded
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

        /// <summary>
        /// Update room status in database.
        /// </summary>
        private void UpdateRoomStatusInDatabase(string roomCode, string status)
        {
            if (string.IsNullOrWhiteSpace(roomCode)) return;
            var connStr = ConfigurationManager.ConnectionStrings["ConnStr"]?.ConnectionString;
            if (string.IsNullOrWhiteSpace(connStr)) return;

            using (var conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("UPDATE dbo.Room SET Status = @Status WHERE RoomID = @RoomID", conn))
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
            _lblCheckout.Text = $"Check-out: {end:dd/MM/yyyy HH:mm} | Số ngày: {days}";
        }

        private void btnNhanphong_Click(object sender, EventArgs e)
        {
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
            DateTime end = start.AddDays(this.SoNgay);
            var info = new BookingInfo { Customer = this.TenKhach, Start = start, End = end, Services = CollectServicesFromGrid() };
            // đảm bảo tiền phòng luôn có trong service
            EnsureRoomCharge(this.SoNgay);
            EnsureVipBreakfast();
            info.Services = CollectServicesFromGrid();
            BookingManager.AddBooking(labMaphong.Text, info);
            _existing = info;
            _btnThanhToan.Visible = true;
            UpdatePaymentButtonByState(start, end);

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
            if (guna2ComboBox1.SelectedItem != null)
            {
                string sel = guna2ComboBox1.SelectedItem.ToString();
                if (sel.Contains("đặt")) btnNhanphong.Text = "Đặt phòng";
                else if (sel.Contains("thuê")) btnNhanphong.Text = "Nhận phòng";
                SelectedStatus = sel;
                var start = dtNgay.Value.Date + dtGio.Value.TimeOfDay;
                var end = start.AddDays((int)nNgay.Value);
                UpdatePaymentButtonByState(start, end);
            }
        }

        private void AddService()
        {
            using (var f = new HotelServices_Form())
            {
                if (f.ShowDialog(this) == DialogResult.OK && f.SelectedServices != null)
                {
                    foreach (var svc in f.SelectedServices)
                    {
                        AddOrUpdateServiceRow(svc.Name, svc.UnitPrice, svc.Quantity);
                    }
                    EnsureRoomCharge((int)nNgay.Value);
                }
            }
        }

        private void AddOrUpdateServiceRow(string name, decimal unitPrice, int qty)
        {
            if (qty < 1) qty = 1;
            foreach (DataGridViewRow r in guna2DataGridView2.Rows)
            {
                if (r.IsNewRow) continue;
                if (string.Equals(Convert.ToString(r.Cells[0].Value), name, StringComparison.OrdinalIgnoreCase))
                {
                    int oldQty = 0;
                    int.TryParse(Convert.ToString(r.Cells[1].Value), out oldQty);
                    int newQty = oldQty + qty;
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
                    dtGio.Value = picker.SelectedDateTime;
                }
            }
        }
    }
}