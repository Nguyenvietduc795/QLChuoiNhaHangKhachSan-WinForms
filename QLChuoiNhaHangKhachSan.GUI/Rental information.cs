using System;
using System.Collections.Generic;
using System.Configuration; // Nhớ Add Reference: System.Configuration
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace QLChuoiNhaHangKhachSan.GUI
{
    public partial class Rental_information : Form
    {
        // --- CÁC BIẾN NHẬN DỮ LIỆU TỪ FORM ROOM_DETAILS ---
        public string CurrentRoomID { get; set; }
        public string TenKhachDaCo { get; set; }
        public DateTime SelectedCheckIn { get; set; } // Ngày khách chọn vào
        public DateTime SelectedCheckOut { get; set; } // Ngày khách chọn ra
        public decimal CurrentPrice { get; set; } // Giá đã tính toán
        public string IdCardDaCo { get; set; }
        public string PhoneDaCo { get; set; }
        public string GenderDaCo { get; set; }
        public string NationalityDaCo { get; set; }
        // Thông tin khách hàng chi tiết (được truyền sang khi đã nhập ở Room_Details/Booking)
        public string PrefillIdCard { get; set; }
        public string PrefillPhone { get; set; }
        public string PrefillAddress { get; set; }
        public string PrefillNationality { get; set; }
        public string PrefillEmail { get; set; }
        public string PrefillGender { get; set; }

        public Rental_information()
        {
            InitializeComponent();
        }

        // Class nội bộ cho Quốc gia
        private class CountryPhoneCode
        {
            public string Name { get; set; }
            public string DialCode { get; set; }
            public string Iso { get; set; }
            public string DisplayName => $"{Name} ({DialCode})";
        }

        // Expanded country -> dial code list. Add/remove as needed.
        private static readonly List<CountryPhoneCode> _countryPhoneCodes = new List<CountryPhoneCode>
        {
            new CountryPhoneCode { Name = "Vietnam", DialCode = "+84", Iso = "VN" },
            new CountryPhoneCode { Name = "United States", DialCode = "+1", Iso = "US" },
            new CountryPhoneCode { Name = "Canada", DialCode = "+1", Iso = "CA" },
            new CountryPhoneCode { Name = "United Kingdom", DialCode = "+44", Iso = "GB" },
            new CountryPhoneCode { Name = "Australia", DialCode = "+61", Iso = "AU" },
            new CountryPhoneCode { Name = "New Zealand", DialCode = "+64", Iso = "NZ" },
            new CountryPhoneCode { Name = "China", DialCode = "+86", Iso = "CN" },
            new CountryPhoneCode { Name = "Japan", DialCode = "+81", Iso = "JP" },
            new CountryPhoneCode { Name = "Korea (South)", DialCode = "+82", Iso = "KR" },
            new CountryPhoneCode { Name = "India", DialCode = "+91", Iso = "IN" },
            new CountryPhoneCode { Name = "Pakistan", DialCode = "+92", Iso = "PK" },
            new CountryPhoneCode { Name = "Bangladesh", DialCode = "+880", Iso = "BD" },
            new CountryPhoneCode { Name = "Indonesia", DialCode = "+62", Iso = "ID" },
            new CountryPhoneCode { Name = "Malaysia", DialCode = "+60", Iso = "MY" },
            new CountryPhoneCode { Name = "Singapore", DialCode = "+65", Iso = "SG" },
            new CountryPhoneCode { Name = "Thailand", DialCode = "+66", Iso = "TH" },
            new CountryPhoneCode { Name = "Philippines", DialCode = "+63", Iso = "PH" },
            new CountryPhoneCode { Name = "Vietnam", DialCode = "+84", Iso = "VN" },
            new CountryPhoneCode { Name = "China", DialCode = "+86", Iso = "CN" },
            new CountryPhoneCode { Name = "South Africa", DialCode = "+27", Iso = "ZA" },
            new CountryPhoneCode { Name = "Nigeria", DialCode = "+234", Iso = "NG" },
            new CountryPhoneCode { Name = "Kenya", DialCode = "+254", Iso = "KE" },
            new CountryPhoneCode { Name = "Egypt", DialCode = "+20", Iso = "EG" },
            new CountryPhoneCode { Name = "Morocco", DialCode = "+212", Iso = "MA" },
            new CountryPhoneCode { Name = "Turkey", DialCode = "+90", Iso = "TR" },
            new CountryPhoneCode { Name = "Russia", DialCode = "+7", Iso = "RU" },
            new CountryPhoneCode { Name = "Israel", DialCode = "+972", Iso = "IL" },
            new CountryPhoneCode { Name = "Saudi Arabia", DialCode = "+966", Iso = "SA" },
            new CountryPhoneCode { Name = "United Arab Emirates", DialCode = "+971", Iso = "AE" },
            new CountryPhoneCode { Name = "France", DialCode = "+33", Iso = "FR" },
            new CountryPhoneCode { Name = "Germany", DialCode = "+49", Iso = "DE" },
            new CountryPhoneCode { Name = "Italy", DialCode = "+39", Iso = "IT" },
            new CountryPhoneCode { Name = "Spain", DialCode = "+34", Iso = "ES" },
            new CountryPhoneCode { Name = "Netherlands", DialCode = "+31", Iso = "NL" },
            new CountryPhoneCode { Name = "Belgium", DialCode = "+32", Iso = "BE" },
            new CountryPhoneCode { Name = "Switzerland", DialCode = "+41", Iso = "CH" },
            new CountryPhoneCode { Name = "Austria", DialCode = "+43", Iso = "AT" },
            new CountryPhoneCode { Name = "Sweden", DialCode = "+46", Iso = "SE" },
            new CountryPhoneCode { Name = "Norway", DialCode = "+47", Iso = "NO" },
            new CountryPhoneCode { Name = "Denmark", DialCode = "+45", Iso = "DK" },
            new CountryPhoneCode { Name = "Finland", DialCode = "+358", Iso = "FI" },
            new CountryPhoneCode { Name = "Ireland", DialCode = "+353", Iso = "IE" },
            new CountryPhoneCode { Name = "Portugal", DialCode = "+351", Iso = "PT" },
            new CountryPhoneCode { Name = "Greece", DialCode = "+30", Iso = "GR" },
            new CountryPhoneCode { Name = "Poland", DialCode = "+48", Iso = "PL" },
            new CountryPhoneCode { Name = "Czech Republic", DialCode = "+420", Iso = "CZ" },
            new CountryPhoneCode { Name = "Hungary", DialCode = "+36", Iso = "HU" },
            new CountryPhoneCode { Name = "Romania", DialCode = "+40", Iso = "RO" },
            new CountryPhoneCode { Name = "Bulgaria", DialCode = "+359", Iso = "BG" },
            new CountryPhoneCode { Name = "Croatia", DialCode = "+385", Iso = "HR" },
            new CountryPhoneCode { Name = "Serbia", DialCode = "+381", Iso = "RS" },
            new CountryPhoneCode { Name = "Slovakia", DialCode = "+421", Iso = "SK" },
            new CountryPhoneCode { Name = "Slovenia", DialCode = "+386", Iso = "SI" },
            new CountryPhoneCode { Name = "Lithuania", DialCode = "+370", Iso = "LT" },
            new CountryPhoneCode { Name = "Latvia", DialCode = "+371", Iso = "LV" },
            new CountryPhoneCode { Name = "Estonia", DialCode = "+372", Iso = "EE" },
            new CountryPhoneCode { Name = "Mexico", DialCode = "+52", Iso = "MX" },
            new CountryPhoneCode { Name = "Argentina", DialCode = "+54", Iso = "AR" },
            new CountryPhoneCode { Name = "Brazil", DialCode = "+55", Iso = "BR" },
            new CountryPhoneCode { Name = "Chile", DialCode = "+56", Iso = "CL" },
            new CountryPhoneCode { Name = "Colombia", DialCode = "+57", Iso = "CO" },
            new CountryPhoneCode { Name = "Peru", DialCode = "+51", Iso = "PE" },
            new CountryPhoneCode { Name = "Venezuela", DialCode = "+58", Iso = "VE" }
        };

        private void Rental_information_Load(object sender, EventArgs e)
        {
            string name = !string.IsNullOrWhiteSpace(TenKhachDaCo) ? TenKhachDaCo : null;
            if (!string.IsNullOrEmpty(name))
            {
                txtHoTen.Text = name;
            }
            var idCard = PrefillIdCard ?? IdCardDaCo;
            if (!string.IsNullOrEmpty(idCard))
            {
                txtCCCD.Text = idCard;
            }
            var phone = PrefillPhone ?? PhoneDaCo;
            if (!string.IsNullOrEmpty(phone))
            {
                txtSDT.Text = phone;
            }
            var nationality = PrefillNationality ?? NationalityDaCo;
            if (!string.IsNullOrEmpty(nationality))
            {
                cbQuoctich.Text = nationality;
            }
            var gender = PrefillGender ?? GenderDaCo;
            if (!string.IsNullOrEmpty(gender))
            {
                var gen = gender.Trim().ToLowerInvariant();
                if (gen.Contains("nam"))
                    cboGioiTinh.SelectedItem = "Nam";
                else if (gen.Contains("nữ") || gen.Contains("nu"))
                    cboGioiTinh.SelectedItem = "Nữ";
                else
                    cboGioiTinh.Text = gender;
            }
            if (!string.IsNullOrWhiteSpace(PrefillAddress))
            {
                txtDiaChi.Text = PrefillAddress;
            }
            if (!string.IsNullOrWhiteSpace(PrefillEmail))
            {
                txtEmail.Text = PrefillEmail;
            }

            // Populate nationality dropdown and wire up selection handling
            try
            {
                PopulateCountryCombo();
            }
            catch { }

            // Hiển thị thông tin ngày giờ đã đặt nếu có
            if (SelectedCheckIn != DateTime.MinValue)
            {
                System.Diagnostics.Debug.WriteLine($"[Rental_information_Load] SelectedCheckIn={SelectedCheckIn:dd/MM/yyyy HH:mm}, SelectedCheckOut={SelectedCheckOut:dd/MM/yyyy HH:mm}");
            }

            // After populating, try selecting prefill nationality and update phone prefix
            try
            {
                if (!string.IsNullOrWhiteSpace(nationality) && cbQuoctich.Items.Count > 0)
                {
                    var match = _countryPhoneCodes.FirstOrDefault(x => string.Equals(x.Name, nationality, StringComparison.OrdinalIgnoreCase) || x.DisplayName.Contains(nationality));
                    if (match != null)
                    {
                        cbQuoctich.SelectedItem = match;
                        SetPhonePrefix(match.DialCode);
                    }
                }
                else if (cbQuoctich.Items.Count > 0 && cbQuoctich.SelectedItem == null)
                {
                    // if no nationality provided, try default Vietnam
                    var vn = _countryPhoneCodes.FirstOrDefault(x => x.Iso == "VN");
                    if (vn != null)
                    {
                        cbQuoctich.SelectedItem = vn;
                        SetPhonePrefix(vn.DialCode);
                    }
                }
            }
            catch { }

            // Enable Enter-to-save: preview keys and handle Enter to trigger save
            try
            {
                this.KeyPreview = true;
                this.KeyDown += Rental_information_KeyDown;
            }
            catch { }
        }

        private void CbQuoctich_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var sel = cbQuoctich.SelectedItem as CountryPhoneCode;
                if (sel == null) return;
                SetPhonePrefix(sel.DialCode);
            }
            catch { }
        }

        private void PopulateCountryCombo()
        {
            if (cbQuoctich == null) return;
            cbQuoctich.DisplayMember = "DisplayName";
            cbQuoctich.ValueMember = "DialCode";
            var list = _countryPhoneCodes.OrderBy(c => c.Name).ToList();
            cbQuoctich.Items.Clear();
            foreach (var c in list) cbQuoctich.Items.Add(c);
            cbQuoctich.SelectedIndexChanged -= CbQuoctich_SelectedIndexChanged;
            cbQuoctich.SelectedIndexChanged += CbQuoctich_SelectedIndexChanged;

            // default to Vietnam if present
            var vn = list.FirstOrDefault(x => x.Iso == "VN");
            if (vn != null && cbQuoctich.SelectedItem == null)
            {
                cbQuoctich.SelectedItem = vn;
                SetPhonePrefix(vn.DialCode);
            }
        }

        private void SetPhonePrefix(string dialCode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dialCode) || txtSDT == null) return;
                string current = txtSDT.Text ?? string.Empty;
                // remove any existing known dial prefix
                string remainder = current;
                var codes = _countryPhoneCodes.Select(c => c.DialCode).OrderByDescending(s => s.Length).ToList();
                foreach (var code in codes)
                {
                    if (current.StartsWith(code, StringComparison.Ordinal))
                    {
                        remainder = current.Substring(code.Length).TrimStart();
                        break;
                    }
                }
                txtSDT.Text = (dialCode + (string.IsNullOrEmpty(remainder) ? "" : " " + remainder)).Trim();
                txtSDT.SelectionStart = txtSDT.Text.Length;
            }
            catch { }
        }

        private string BuildPriceDetailsString()
        {
            if (SelectedCheckIn == DateTime.MinValue) SelectedCheckIn = DateTime.Now;
            if (SelectedCheckOut == DateTime.MinValue) SelectedCheckOut = SelectedCheckIn.AddDays(1);

            // Ensure base price is available
            if (CurrentPrice <= 0)
            {
                try
                {
                    var connStr = ConfigurationManager.ConnectionStrings["ConnStr"]?.ConnectionString;
                    if (!string.IsNullOrWhiteSpace(connStr))
                    {
                        using (var conn = new SqlConnection(connStr))
                        using (var cmd = new SqlCommand("SELECT TOP 1 AppliedPrice FROM dbo.v_RoomPrice WHERE RoomID = @RoomID", conn))
                        {
                            cmd.Parameters.AddWithValue("@RoomID", CurrentRoomID);
                            conn.Open();
                            var obj = cmd.ExecuteScalar();
                            if (obj != null && obj != DBNull.Value)
                                CurrentPrice = Convert.ToDecimal(obj);
                        }
                    }
                }
                catch { }
            }

            var sb = new StringBuilder();
            decimal grandTotal = 0m;
            decimal basePrice = CurrentPrice;

            sb.AppendLine($"📌 Phòng {CurrentRoomID}:");
            sb.AppendLine($"   Giá cơ bản: {basePrice:N0} VNĐ/đêm");

            int days = 0;
            for (DateTime date = SelectedCheckIn.Date; date < SelectedCheckOut.Date; date = date.AddDays(1))
            {
                days++;
                decimal multiplier = HolidayPriceConfig.GetPriceMultiplier(date);
                decimal dayPrice = basePrice * multiplier;
                grandTotal += dayPrice;

                if (multiplier > 1.0m)
                {
                    var holidayInfo = HolidayPriceConfig.GetHolidayInfo(date);
                    string reason = holidayInfo != null ? holidayInfo.Name :
                        (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday ? "Cuối tuần" : "");
                    sb.AppendLine($"   • {date:dd/MM/yyyy} ({date.DayOfWeek}): {dayPrice:N0} VNĐ (+{(multiplier - 1) * 100:0}% - {reason})");
                }
            }

            sb.AppendLine($"   Số đêm: {days}");
            sb.AppendLine($"   💰 Tổng phòng: {grandTotal:N0} VNĐ");
            sb.AppendLine();
            sb.AppendLine($"══════════════════════════");
            sb.AppendLine($"💵 TỔNG CỘNG: {grandTotal:N0} VNĐ");

            return sb.ToString();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            // === VALIDATION ===

            // 1. Kiểm tra Họ Tên - chỉ cho phép chữ cái và khoảng trắng
            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                MessageBox.Show("Vui lòng nhập họ tên khách hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHoTen.Focus();
                return;
            }
            if (!IsValidName(txtHoTen.Text))
            {
                MessageBox.Show("Họ tên chỉ được chứa chữ cái!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHoTen.Focus();
                return;
            }

            // 2. Kiểm tra CCCD - chỉ cho phép số
            if (string.IsNullOrWhiteSpace(txtCCCD.Text))
            {
                MessageBox.Show("Vui lòng nhập số CCCD!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCCCD.Focus();
                return;
            }
            if (!IsValidCCCD(txtCCCD.Text))
            {
                MessageBox.Show("CCCD chỉ được chứa số và phải có 12 ký tự!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCCCD.Focus();
                return;
            }

            // 3. Kiểm tra Số điện thoại - chỉ cho phép số (nếu có nhập)
            if (!string.IsNullOrWhiteSpace(txtSDT.Text))
            {
                if (!IsValidPhone(txtSDT.Text))
                {
                    MessageBox.Show("Số điện thoại không hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSDT.Focus();
                    return;
                }
                // Kiểm tra độ dài số điện thoại (loại bỏ khoảng trắng và dấu +)
                string phoneDigits = Regex.Replace(txtSDT.Text, @"[^\d]", "");
                if (phoneDigits.Length < 9 || phoneDigits.Length > 15)
                {
                    MessageBox.Show("Số điện thoại phải có từ 9-15 chữ số!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSDT.Focus();
                    return;
                }
            }

            // 4. Kiểm tra Email (nếu có nhập)
            if (txtEmail != null && !string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                if (!IsValidEmail(txtEmail.Text))
                {
                    MessageBox.Show("Email không hợp lệ! Vui lòng nhập đúng định dạng (ví dụ: example@gmail.com)", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtEmail.Focus();
                    return;
                }
            }

            // === KẾT THÚC VALIDATION ===

            // Lấy chuỗi kết nối từ file Config
            string connectionString = ConfigurationManager.ConnectionStrings["ConnStr"]?.ConnectionString;
            if (string.IsNullOrEmpty(connectionString))
            {
                MessageBox.Show("Chưa cấu hình chuỗi kết nối (ConnStr)!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Hiển thị chi tiết giá với phụ thu lễ/tết/cuối tuần trước khi lưu
            string priceDetails = BuildPriceDetailsString();
            var confirm = MessageBox.Show(
                $"Xác nhận nhận phòng cho khách: {txtHoTen.Text}\n\n" +
                $"Chi tiết giá:\n{priceDetails}\n" +
                "Bạn có muốn tiếp tục lưu?",
                "Xác nhận giá",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes)
                return;

            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // --- BƯỚC 1: XỬ LÝ KHÁCH HÀNG (Upsert) ---
                        int customerId = 0;
                        
                        string sqlCheckKhach = "SELECT TOP 1 CustomerID FROM Customer WHERE IdCard = @IdCard";
                        using (var cmdCheck = new SqlCommand(sqlCheckKhach, conn, transaction))
                        {
                            cmdCheck.Parameters.AddWithValue("@IdCard", txtCCCD.Text.Trim());
                            var result = cmdCheck.ExecuteScalar();
                            if (result != null && result != DBNull.Value) 
                                customerId = Convert.ToInt32(result);
                        }

                        if (customerId == 0)
                        {
                            string sqlInsertKhach = @"INSERT INTO Customer (FullName, IdCard, Phone, Gender, Nationality) 
                                                      VALUES (@FullName, @IdCard, @Phone, @Gender, @Nationality);
                                                      SELECT CAST(SCOPE_IDENTITY() AS INT);";
                            using (var cmdInsertKhach = new SqlCommand(sqlInsertKhach, conn, transaction))
                            {
                                cmdInsertKhach.Parameters.AddWithValue("@FullName", txtHoTen.Text.Trim());
                                cmdInsertKhach.Parameters.AddWithValue("@IdCard", txtCCCD.Text.Trim());
                                cmdInsertKhach.Parameters.AddWithValue("@Phone", txtSDT.Text ?? (object)DBNull.Value);
                                cmdInsertKhach.Parameters.AddWithValue("@Gender", "Nam"); 
                                cmdInsertKhach.Parameters.AddWithValue("@Nationality", "Vietnam"); 
                                customerId = Convert.ToInt32(cmdInsertKhach.ExecuteScalar());
                            }
                        }

                        // Ensure Email/Address are saved for this customer (insert or update)
                        try
                        {
                            var emailVal = string.IsNullOrWhiteSpace(txtEmail?.Text) ? (object)DBNull.Value : txtEmail.Text.Trim();
                            var addrVal = string.IsNullOrWhiteSpace(txtDiaChi?.Text) ? (object)DBNull.Value : txtDiaChi.Text.Trim();

                            // If customer already existed, update Email/Address
                            if (customerId > 0)
                            {
                                using (var cmdUpdCust = new SqlCommand(@"UPDATE dbo.Customer SET Email = COALESCE(@Email, Email), Address = COALESCE(@Address, Address) WHERE CustomerID = @CID", conn, transaction))
                                {
                                    cmdUpdCust.Parameters.AddWithValue("@Email", emailVal);
                                    cmdUpdCust.Parameters.AddWithValue("@Address", addrVal);
                                    cmdUpdCust.Parameters.AddWithValue("@CID", customerId);
                                    cmdUpdCust.ExecuteNonQuery();
                                }
                            }
                            else
                            {
                                // If no customerId (shouldn't happen), try to insert with email/address
                                using (var cmdTry = new SqlCommand(@"INSERT INTO dbo.Customer(FullName, IdCard, Phone, Gender, Nationality, Email, Address) VALUES(@FullName,@IdCard,@Phone,@Gender,@Nationality,@Email,@Address); SELECT CAST(SCOPE_IDENTITY() AS INT);", conn, transaction))
                                {
                                    cmdTry.Parameters.AddWithValue("@FullName", txtHoTen.Text.Trim());
                                    cmdTry.Parameters.AddWithValue("@IdCard", txtCCCD.Text.Trim());
                                    cmdTry.Parameters.AddWithValue("@Phone", txtSDT.Text ?? (object)DBNull.Value);
                                    cmdTry.Parameters.AddWithValue("@Gender", "Nam");
                                    cmdTry.Parameters.AddWithValue("@Nationality", "Vietnam");
                                    cmdTry.Parameters.AddWithValue("@Email", emailVal);
                                    cmdTry.Parameters.AddWithValue("@Address", addrVal);
                                    var obj = cmdTry.ExecuteScalar();
                                    if (obj != null && obj != DBNull.Value) customerId = Convert.ToInt32(obj);
                                }
                            }
                        }
                        catch { }

                        // --- BƯỚC 2: KIỂM TRA XEM ĐÃ CÓ BOOKING CHO PHÒNG NÀY CHƯA ---
                        int existingBookingId = 0;
                        string sqlFindBooking = @"SELECT TOP 1 b.BookingID 
                                                  FROM dbo.Booking b
                                                  JOIN dbo.BookingDetail d ON b.BookingID = d.BookingID
                                                  WHERE d.RoomID = @RoomID 
                                                    AND b.Status IN (N'Đặt', N'Open', N'Booked')
                                                  ORDER BY b.CreatedDate DESC";
                        using (var cmdFind = new SqlCommand(sqlFindBooking, conn, transaction))
                        {
                            cmdFind.Parameters.AddWithValue("@RoomID", CurrentRoomID?.Trim());
                            var obj = cmdFind.ExecuteScalar();
                            if (obj != null && obj != DBNull.Value)
                                existingBookingId = Convert.ToInt32(obj);
                        }

                        int bookingId = existingBookingId;

                        // --- BƯỚC 3: NẾU ĐÃ CÓ BOOKING -> CẬP NHẬT TRẠNG THÁI THÀNH "ĐANG THUÊ" ---
                        if (existingBookingId > 0)
                        {
                            // Cập nhật trạng thái booking từ "Đặt" -> "Đang thuê"
                            string sqlUpdateBooking = "UPDATE dbo.Booking SET Status = N'Đang thuê', CustomerID = @CustomerID WHERE BookingID = @BookingID";
                            using (var cmdUpd = new SqlCommand(sqlUpdateBooking, conn, transaction))
                            {
                                cmdUpd.Parameters.AddWithValue("@CustomerID", customerId);
                                cmdUpd.Parameters.AddWithValue("@BookingID", existingBookingId);
                                cmdUpd.ExecuteNonQuery();
                            }
                            System.Diagnostics.Debug.WriteLine($"[Rental_information] Updated existing Booking {existingBookingId} to 'Đang thuê'");
                        }
                        else
                        {
                            // --- NẾU CHƯA CÓ BOOKING -> TẠO MỚI ---
                            string bookingCode = "BK" + DateTime.Now.ToString("yyyyMMddHHmmss");
                            int employeeId = 1;

                            string sqlBooking = @"INSERT INTO Booking (BookingCode, CustomerID, EmployeeID, CreatedDate, Status) 
                                                  VALUES (@BookingCode, @CustomerID, @EmployeeID, GETDATE(), @Status);
                                                  SELECT CAST(SCOPE_IDENTITY() AS INT);";

                            using (var cmdBooking = new SqlCommand(sqlBooking, conn, transaction))
                            {
                                cmdBooking.Parameters.AddWithValue("@Status", "Đang thuê");
                                cmdBooking.Parameters.AddWithValue("@BookingCode", bookingCode);
                                cmdBooking.Parameters.AddWithValue("@CustomerID", customerId);
                                cmdBooking.Parameters.AddWithValue("@EmployeeID", employeeId);
                                bookingId = Convert.ToInt32(cmdBooking.ExecuteScalar());
                            }

                            // Tạo BookingDetail
                            if (SelectedCheckIn == DateTime.MinValue) SelectedCheckIn = DateTime.Now;
                            if (SelectedCheckOut == DateTime.MinValue) SelectedCheckOut = SelectedCheckIn.AddDays(1);

                            if (CurrentPrice == 0)
                            {
                                 using(var cmdPrice = new SqlCommand("SELECT TOP 1 AppliedPrice FROM v_RoomPrice WHERE RoomID=@RID", conn, transaction))
                                 {
                                     cmdPrice.Parameters.AddWithValue("@RID", CurrentRoomID);
                                     var p = cmdPrice.ExecuteScalar();
                                     if(p!=null) CurrentPrice = Convert.ToDecimal(p);
                                 }
                            }

                            string sqlDetail = @"INSERT INTO BookingDetail (BookingID, RoomID, CheckIn, CheckOut, AppliedPrice, Note) 
                                                 VALUES (@BookingID, @RoomID, @CheckIn, @CheckOut, @AppliedPrice, @Note)";
                            
                            using (var cmdDetail = new SqlCommand(sqlDetail, conn, transaction))
                            {
                                cmdDetail.Parameters.AddWithValue("@BookingID", bookingId);
                                cmdDetail.Parameters.AddWithValue("@RoomID", CurrentRoomID?.Trim());
                                cmdDetail.Parameters.Add(new SqlParameter("@CheckIn", SqlDbType.DateTime) { Value = SelectedCheckIn });
                                cmdDetail.Parameters.Add(new SqlParameter("@CheckOut", SqlDbType.DateTime) { Value = SelectedCheckOut });
                                cmdDetail.Parameters.AddWithValue("@AppliedPrice", CurrentPrice);
                                cmdDetail.Parameters.AddWithValue("@Note", "Khách lẻ (Thuê trực tiếp)");
                                cmdDetail.ExecuteNonQuery();
                            }
                        }

                        // --- BƯỚC 4: CẬP NHẬT TRẠNG THÁI PHÒNG -> "Phòng đang thuê" ---
                        string sqlUpdateRoom = "UPDATE Room SET Status = N'Phòng đang thuê' WHERE RoomID = @RoomID";
                        using (var cmdUpdateRoom = new SqlCommand(sqlUpdateRoom, conn, transaction))
                        {
                            cmdUpdateRoom.Parameters.AddWithValue("@RoomID", CurrentRoomID?.Trim());
                            cmdUpdateRoom.ExecuteNonQuery();
                        }

                        transaction.Commit();

                        System.Diagnostics.Debug.WriteLine($"[Rental_information] Success! Room={CurrentRoomID}, BookingID={bookingId}, CheckIn={SelectedCheckIn:dd/MM/yyyy HH:mm}, CheckOut={SelectedCheckOut:dd/MM/yyyy HH:mm}");

                        MessageBox.Show("Nhận phòng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // --- BƯỚC 5: CẬP NHẬT GIAO DIỆN CHÍNH ---
                        try
                        {
                            var memInfo = new BookingInfo
                            {
                                Customer = txtHoTen.Text.Trim(),
                                Start = SelectedCheckIn == DateTime.MinValue ? DateTime.Now : SelectedCheckIn,
                                End = SelectedCheckOut == DateTime.MinValue ? DateTime.Now.AddDays(1) : SelectedCheckOut,
                                Services = new List<ServiceItem>()
                            };
                            BookingManager.AddBooking(CurrentRoomID, memInfo);

                            var listForm = Application.OpenForms.OfType<ListRoom>().FirstOrDefault();
                            if (listForm != null)
                            {
                                listForm.MarkRoomsAsRented(new[] { CurrentRoomID }, memInfo.Customer, memInfo.End, memInfo.Start);
                                listForm.RefreshFromBookings(new[] { CurrentRoomID });
                            }
                            // Also refresh Booking list form so detailed booking row shows updated Email/Address
                            var bookingForm = Application.OpenForms.OfType<BooKing_Form>().FirstOrDefault();
                            if (bookingForm != null)
                            {
                                bookingForm.RefreshBookingsFromDb();
                            }
                        }
                        catch { }

                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show("Lỗi hệ thống: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void Rental_information_KeyDown(object sender, KeyEventArgs e)
        {
            // If Enter pressed, and focus is not in a multiline textbox, trigger save button
            if (e.KeyCode == Keys.Enter)
            {
                var active = this.ActiveControl;
                // ignore if focus is inside a multiline text box
                if (active is TextBoxBase tb && tb.Multiline) return;
                e.SuppressKeyPress = true;
                e.Handled = true;
                try
                {
                    // safe invoke save handler
                    btnLuu?.PerformClick();
                }
                catch { }
            }
        }

        // === VALIDATION HELPER METHODS ===

        /// <summary>
        /// Kiểm tra họ tên hợp lệ - chỉ chứa chữ cái (bao gồm tiếng Việt) và khoảng trắng
        /// </summary>
        private bool IsValidName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return false;
            // Cho phép chữ cái Unicode (bao gồm tiếng Việt), khoảng trắng và dấu
            return Regex.IsMatch(name.Trim(), @"^[\p{L}\s]+$");
        }

        /// <summary>
        /// Kiểm tra CCCD hợp lệ - chỉ chứa số và đúng 12 ký tự
        /// </summary>
        private bool IsValidCCCD(string cccd)
        {
            if (string.IsNullOrWhiteSpace(cccd)) return false;
            string cleaned = cccd.Trim();
            // CCCD Việt Nam có 12 số
            return Regex.IsMatch(cleaned, @"^\d{12}$");
        }

        /// <summary>
        /// Kiểm tra số điện thoại hợp lệ - chỉ chứa số, dấu + và khoảng trắng
        /// </summary>
        private bool IsValidPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone)) return false;
            // Cho phép số, dấu +, khoảng trắng và dấu gạch ngang
            return Regex.IsMatch(phone.Trim(), @"^[\d\s\+\-]+$");
        }

        /// <summary>
        /// Kiểm tra email hợp lệ theo chuẩn RFC 5322
        /// </summary>
        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            // Pattern email chuẩn
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email.Trim(), pattern, RegexOptions.IgnoreCase);
        }
    }
}