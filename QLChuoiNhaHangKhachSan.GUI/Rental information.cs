using System;
using System.Collections.Generic;
using System.Configuration; // Nhớ Add Reference: System.Configuration
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
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

        private static readonly List<CountryPhoneCode> _countryPhoneCodes = new List<CountryPhoneCode>
        {
            new CountryPhoneCode { Name = "Vietnam", DialCode = "+84", Iso = "VN" },
            new CountryPhoneCode { Name = "United States", DialCode = "+1", Iso = "US" },
            new CountryPhoneCode { Name = "Japan", DialCode = "+81", Iso = "JP" },
            new CountryPhoneCode { Name = "Korea", DialCode = "+82", Iso = "KR" },
            new CountryPhoneCode { Name = "China", DialCode = "+86", Iso = "CN" },
            // ... (Bạn có thể thêm danh sách dài của bạn ở đây)
        };

        private void Rental_information_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(TenKhachDaCo))
            {
                txtHoTen.Text = TenKhachDaCo;
            }

            // Hiển thị thông tin ngày giờ đã đặt nếu có
            if (SelectedCheckIn != DateTime.MinValue)
            {
                System.Diagnostics.Debug.WriteLine($"[Rental_information_Load] SelectedCheckIn={SelectedCheckIn:dd/MM/yyyy HH:mm}, SelectedCheckOut={SelectedCheckOut:dd/MM/yyyy HH:mm}");
            }
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
            // 1. Kiểm tra dữ liệu nhập
            if (string.IsNullOrWhiteSpace(txtHoTen.Text) || string.IsNullOrWhiteSpace(txtCCCD.Text))
            {
                MessageBox.Show("Vui lòng nhập Tên và CCCD/CMND.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

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
    }
}