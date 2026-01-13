using System;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace QLChuoiNhaHangKhachSan.GUI
{
    public partial class BooKing_Form : Form
    {
        public BooKing_Form()
        {
            InitializeComponent();
            this.btnThuePhong.Click += Guna2Button1_Click;
            this.guna2DataGridView1.CellContentClick += Guna2DataGridView1_CellContentClick;
            this.guna2TextBox1.TextChanged += Guna2TextBox1_TextChanged;
            this.Load += BooKing_Form_Load;
            ConfigureSearchAutocomplete();
            RefreshAutocompleteSource();
            ConfigureDataGridView();
        }

        private void BooKing_Form_Load(object sender, EventArgs e)
        {
            LoadBookingsFromDatabase();
        }

        private void ConfigureDataGridView()
        {
            try
            {
                if (guna2DataGridView1.Columns.Count > 0)
                {
                    guna2DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                    guna2DataGridView1.Columns[0].FillWeight = 15; // Số Phiếu Thuê
                    guna2DataGridView1.Columns[1].FillWeight = 25; // Tên Khách Hàng
                    guna2DataGridView1.Columns[2].FillWeight = 15; // Ngày Lập
                    guna2DataGridView1.Columns[3].FillWeight = 12; // Nhân Viên
                    guna2DataGridView1.Columns[4].FillWeight = 33; // Chi Tiết (wider)
                    guna2DataGridView1.Columns[5].FillWeight = 5;  // Xóa

                    guna2DataGridView1.Columns[4].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                    guna2DataGridView1.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }

                guna2DataGridView1.AllowUserToAddRows = false;
                guna2DataGridView1.ReadOnly = true;
                guna2DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                guna2DataGridView1.MultiSelect = false;
                guna2DataGridView1.RowTemplate.Height = 60;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cấu hình DataGridView: " + ex.Message);
            }
        }

        // --- ĐÃ SỬA: Thêm hàm xử lý xóa trong SQL ---
        private bool DeleteBookingFromDb(string bookingCode)
        {
            var connStr = ConfigurationManager.ConnectionStrings["ConnStr"]?.ConnectionString;
            if (string.IsNullOrWhiteSpace(connStr)) return false;

            // Xóa theo thứ tự: BookingServices -> BookingDetails -> HotelBookings
            // để tránh lỗi khóa ngoại
            string query = @"
                DECLARE @BID INT;
                SELECT @BID = BookingID FROM dbo.HotelBookings WHERE BookingCode = @Code;

                IF @BID IS NOT NULL
                BEGIN
                    -- Xóa BookingServices trước (nếu có)
                    IF OBJECT_ID('dbo.BookingServices', 'U') IS NOT NULL
                    BEGIN
                        DELETE FROM dbo.BookingServices WHERE BookingID = @BID;
                    END
                    
                    -- Cập nhật trạng thái phòng về Phòng Trống
                    UPDATE dbo.Rooms 
                    SET Status = N'Phòng Trống' 
                    WHERE RoomId IN (SELECT RoomId FROM dbo.BookingDetails WHERE BookingID = @BID);
                    
                    -- Xóa BookingDetails
                    DELETE FROM dbo.BookingDetails WHERE BookingID = @BID;
                    
                    -- Xóa HotelBookings
                    DELETE FROM dbo.HotelBookings WHERE BookingID = @BID;
                END";

            using (var conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand(query, conn))
            {
                try
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@Code", bookingCode);
                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0; // Trả về true nếu xóa thành công
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi SQL khi xóa: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
        }

        private void LoadBookingsFromDatabase()
        {
            guna2DataGridView1.Rows.Clear();
            BookingManager.Clear();

            var connStr = ConfigurationManager.ConnectionStrings["ConnStr"]?.ConnectionString;
            if (string.IsNullOrWhiteSpace(connStr))
            {
                MessageBox.Show("Không tìm thấy connection string!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var bookings = new Dictionary<string, BookingRowInfo>(StringComparer.OrdinalIgnoreCase);
            var order = new List<string>();
            var details = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
            DateTime now = DateTime.Now;

            try
            {
                using (var conn = new SqlConnection(connStr))
                using (var cmd = new SqlCommand(@"SELECT b.BookingCode, b.CreatedDate, b.EmployeeID, b.Status, b.CancelReason,
                                                 c.FullName AS CustomerName, c.CCCD, c.PhoneNumber, c.Email, c.Sex AS Gender, c.Nationality,
                                                 d.RoomId, d.CheckIn, d.CheckOut,
                                                 e.FullName AS EmployeeName
                                              FROM dbo.HotelBookings b
                                              INNER JOIN dbo.Customers c ON b.CustomerId = c.CustomerId
                                              LEFT JOIN dbo.BookingDetails d ON b.BookingId = d.BookingId
                                              LEFT JOIN dbo.Employees e ON b.EmployeeID = e.EmployeeId
                                              ORDER BY b.CreatedDate DESC, b.BookingId DESC", conn))
                {
                    conn.Open();

                    int recordCount = 0;
                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            recordCount++;
                            string code = rd["BookingCode"]?.ToString();
                            if (string.IsNullOrWhiteSpace(code)) continue;

                            BookingRowInfo info;
                            if (!bookings.TryGetValue(code, out info))
                            {
                                DateTime? created = rd["CreatedDate"] != DBNull.Value ? (DateTime?)rd["CreatedDate"] : null;
                                // Lấy tên nhân viên từ CSDL
                                string employeeName = rd["EmployeeName"] != DBNull.Value ? rd["EmployeeName"].ToString() : null;
                                string staff = !string.IsNullOrWhiteSpace(employeeName) ? employeeName : 
                                              (rd["EmployeeID"] != DBNull.Value ? "NV #" + rd["EmployeeID"] : string.Empty);
                                
                                info = new BookingRowInfo
                                {
                                    Id = code,
                                    Customer = rd["CustomerName"]?.ToString() ?? "",
                                    Date = created.HasValue ? created.Value.ToString("dd/MM/yyyy") : "",
                                    Staff = staff,
                                    CCCD = rd["CCCD"]?.ToString() ?? "",
                                    SDT = rd["PhoneNumber"]?.ToString() ?? "",
                                    Email = rd["Email"]?.ToString() ?? "",
                                    Gender = rd["Gender"]?.ToString() ?? "",
                                    Nationality = rd["Nationality"]?.ToString() ?? "",
                                    CreatedDate = created
                                };
                                // set cancellation info if present
                                try
                                {
                                    var statusObj = rd["Status"];
                                    if (statusObj != null && statusObj != DBNull.Value)
                                    {
                                        var s = statusObj.ToString();
                                        info.IsCancelled = s != null && s.ToLower().Contains("cancel");
                                    }
                                    var reasonObj = rd["CancelReason"];
                                    if (reasonObj != null && reasonObj != DBNull.Value)
                                    {
                                        info.CancellationReason = reasonObj.ToString();
                                    }
                                }
                                catch { }
                                bookings[code] = info;
                                order.Add(code);
                            }

                            DateTime? checkIn = rd["CheckIn"] != DBNull.Value ? (DateTime?)rd["CheckIn"] : null;
                            DateTime? checkOut = rd["CheckOut"] != DBNull.Value ? (DateTime?)rd["CheckOut"] : null;

                            if (rd["RoomId"] != DBNull.Value)
                            {
                                var detailParts = new StringBuilder();
                                detailParts.Append(rd["RoomId"].ToString());
                                if (checkIn.HasValue || checkOut.HasValue)
                                {
                                    string start = checkIn.HasValue ? checkIn.Value.ToString("dd/MM/yyyy HH:mm") : "?";
                                    string end = checkOut.HasValue ? checkOut.Value.ToString("dd/MM/yyyy HH:mm") : "?";
                                    detailParts.Append(" (").Append(start).Append(" - ").Append(end).Append(")");
                                }

                                List<string> list;
                                if (!details.TryGetValue(code, out list))
                                {
                                    list = new List<string>();
                                    details[code] = list;
                                }
                                list.Add(detailParts.ToString());

                                if (checkOut.HasValue && checkOut.Value > now)
                                {
                                    BookingManager.AddBooking(rd["RoomId"].ToString(), new BookingInfo
                                    {
                                        Customer = info.Customer,
                                        Start = checkIn ?? now,
                                        End = checkOut.Value
                                    });
                                }
                            }
                        }
                    }

                    if (recordCount == 0)
                    {
                        // MessageBox.Show("Không có dữ liệu trong database!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                foreach (var code in order)
                {
                    BookingRowInfo info;
                    if (!bookings.TryGetValue(code, out info)) continue;
                    List<string> list;
                    if (details.TryGetValue(code, out list))
                    {
                        info.Detail = string.Join("\r\n", list);
                    }
                    AddBookingRowFromDb(info);
                }

                RefreshAutocompleteSource();
                guna2DataGridView1.Refresh();
                this.Text = $"Booking Form - {guna2DataGridView1.Rows.Count} phiếu đặt phòng";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string NormalizeText(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;
            var formD = input.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            foreach (var ch in formD)
            {
                var uc = CharUnicodeInfo.GetUnicodeCategory(ch);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(char.ToLowerInvariant(ch));
                }
            }
            return sb.ToString().Normalize(NormalizationForm.FormC);
        }

        private void ConfigureSearchAutocomplete()
        {
            try
            {
                guna2TextBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                guna2TextBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;
            }
            catch { }
        }

        private void RefreshAutocompleteSource()
        {
            if (guna2TextBox1.AutoCompleteCustomSource == null)
                guna2TextBox1.AutoCompleteCustomSource = new AutoCompleteStringCollection();

            var src = new AutoCompleteStringCollection();
            foreach (DataGridViewRow row in guna2DataGridView1.Rows)
            {
                if (row.IsNewRow) continue;
                var nameRaw = row.Cells[1].Value?.ToString();
                if (!string.IsNullOrWhiteSpace(nameRaw)) src.Add(nameRaw);
            }
            guna2TextBox1.AutoCompleteCustomSource = src;
        }

        private void Guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
            string q = NormalizeText(this.guna2TextBox1.Text);
            foreach (DataGridViewRow row in guna2DataGridView1.Rows)
            {
                if (row.IsNewRow) continue;
                var nameRaw = row.Cells[1].Value?.ToString() ?? string.Empty;
                var nameNorm = NormalizeText(nameRaw);
                row.Visible = string.IsNullOrEmpty(q) || nameNorm.Contains(q);
            }
        }

        // --- ĐÃ SỬA: Cập nhật sự kiện click để xóa thật trong DB ---
        private void Guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            int detailCol = 4;
            int deleteCol = 5;

            // Xử lý Xóa
            if (e.ColumnIndex == deleteCol)
            {
                // get the row before removal
                var row = guna2DataGridView1.Rows[e.RowIndex];

                string bookingCode = guna2DataGridView1.Rows[e.RowIndex].Cells[0].Value?.ToString();

                var dr = MessageBox.Show($"Bạn có chắc chắn muốn xóa phiếu '{bookingCode}' không?\nDữ liệu sẽ bị mất vĩnh viễn.",
                                         "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (dr == DialogResult.Yes)
                {
                    // 1. Xóa trong database
                    bool isDeleted = DeleteBookingFromDb(bookingCode);

                    if (isDeleted)
                    {
                        // 2. Nếu xóa DB thành công thì xóa trên giao diện
                        guna2DataGridView1.Rows.RemoveAt(e.RowIndex);
                        RefreshAutocompleteSource();

                        MessageBox.Show("Đã xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Xóa thất bại! Có thể do lỗi kết nối hoặc dữ liệu không tồn tại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            // Xử lý Xem chi tiết
            else if (e.ColumnIndex == detailCol)
            {
                var row = guna2DataGridView1.Rows[e.RowIndex];
                string bookingCode = Convert.ToString(row.Cells[0].Value);

                BookingRowInfo infoFromDb = null;
                try
                {
                    infoFromDb = GetBookingInfoFromDb(bookingCode);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                // Pass the DB-loaded info (if available) to the detail form. Fallback to row.Tag inside the ShowDetailForm.
                ShowDetailForm(row, infoFromDb);
            }
        }

        private BookingRowInfo GetBookingInfoFromDb(string bookingCode)
        {
            if (string.IsNullOrWhiteSpace(bookingCode)) return null;
            var connStr = ConfigurationManager.ConnectionStrings["ConnStr"]?.ConnectionString;
            if (string.IsNullOrWhiteSpace(connStr)) return null;

            var info = (BookingRowInfo)null;
            var details = new List<string>();

            using (var conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand(@"SELECT b.BookingCode, b.CreatedDate, b.EmployeeID, b.Status, b.CancelReason,
                                                 c.FullName AS CustomerName, c.CCCD, c.PhoneNumber, c.Email, c.Sex AS Gender, c.Nationality,
                                                 d.RoomId, d.CheckIn, d.CheckOut,
                                                 e.FullName AS EmployeeName
                                              FROM dbo.HotelBookings b
                                              INNER JOIN dbo.Customers c ON b.CustomerId = c.CustomerId
                                              LEFT JOIN dbo.BookingDetails d ON b.BookingId = d.BookingId
                                              LEFT JOIN dbo.Employees e ON b.EmployeeID = e.EmployeeId
                                              WHERE b.BookingCode = @Code
                                              ORDER BY d.BookingDetailId", conn))
            {
                cmd.Parameters.AddWithValue("@Code", bookingCode);
                conn.Open();
                using (var rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        if (info == null)
                        {
                            DateTime? created = rd["CreatedDate"] != DBNull.Value ? (DateTime?)rd["CreatedDate"] : null;
                            // Lấy tên nhân viên từ CSDL
                            string employeeName = rd["EmployeeName"] != DBNull.Value ? rd["EmployeeName"].ToString() : null;
                            string staff = !string.IsNullOrWhiteSpace(employeeName) ? employeeName : 
                                          (rd["EmployeeID"] != DBNull.Value ? "NV #" + rd["EmployeeID"] : string.Empty);
                            
                            info = new BookingRowInfo
                            {
                                Id = bookingCode,
                                Customer = rd["CustomerName"]?.ToString() ?? string.Empty,
                                Date = created.HasValue ? created.Value.ToString("dd/MM/yyyy") : string.Empty,
                                Staff = staff,
                                CCCD = rd["CCCD"]?.ToString() ?? string.Empty,
                                SDT = rd["PhoneNumber"]?.ToString() ?? string.Empty,
                                Email = rd["Email"]?.ToString() ?? string.Empty,
                                Gender = rd["Gender"]?.ToString() ?? string.Empty,
                                Nationality = rd["Nationality"]?.ToString() ?? string.Empty,
                                CreatedDate = created
                            };
                            try
                            {
                                var statusObj = rd["Status"];
                                if (statusObj != null && statusObj != DBNull.Value)
                                {
                                    var s = statusObj.ToString();
                                    info.IsCancelled = s != null && s.ToLower().Contains("cancel");
                                }
                                var reasonObj = rd["CancelReason"];
                                if (reasonObj != null && reasonObj != DBNull.Value)
                                {
                                    info.CancellationReason = reasonObj.ToString();
                                }
                            }
                            catch { }
                        }

                        if (rd["RoomId"] != DBNull.Value)
                        {
                            DateTime? checkIn = rd["CheckIn"] != DBNull.Value ? (DateTime?)rd["CheckIn"] : null;
                            DateTime? checkOut = rd["CheckOut"] != DBNull.Value ? (DateTime?)rd["CheckOut"] : null;
                            var sb = new StringBuilder();
                            sb.Append(rd["RoomId"].ToString());
                            if (checkIn.HasValue || checkOut.HasValue)
                            {
                                string start = checkIn.HasValue ? checkIn.Value.ToString("dd/MM/yyyy HH:mm") : "?";
                                string end = checkOut.HasValue ? checkOut.Value.ToString("dd/MM/yyyy HH:mm") : "?";
                                sb.Append(" (").Append(start).Append(" - ").Append(end).Append(")");
                            }
                            details.Add(sb.ToString());
                        }
                    }
                }
            }

            if (info != null)
            {
                info.Detail = details.Count > 0 ? string.Join("\r\n", details) : string.Empty;
            }
            return info;
        }

        private void ShowDetailForm(DataGridViewRow row, BookingRowInfo info)
        {
            using (Form f = new Form())
            {
                f.Text = "Chi tiết phiếu";
                f.StartPosition = FormStartPosition.CenterParent;
                f.Size = new Size(720, 520);
                f.Font = new Font("Segoe UI", 10.5F, FontStyle.Regular);
                f.BackColor = Color.WhiteSmoke;

                var container = new Panel
                {
                    Dock = DockStyle.Fill,
                    Padding = new Padding(20),
                    BackColor = Color.White
                };

                var header = new Label
                {
                    Text = "Chi tiết phiếu",
                    AutoSize = true,
                    Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(45, 55, 72),
                    Dock = DockStyle.Top,
                    Padding = new Padding(0, 0, 0, 14)
                };

                var table = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    ColumnCount = 2,
                    AutoSize = true,
                    AutoSizeMode = AutoSizeMode.GrowAndShrink,
                    CellBorderStyle = TableLayoutPanelCellBorderStyle.None,
                    Padding = new Padding(0, 10, 0, 0)
                };
                table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140));
                table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

                void AddRow(string title, string value)
                {
                    int r = table.RowCount;
                    table.RowStyles.Add(new RowStyle(SizeType.AutoSize));

                    var lblTitle = new Label
                    {
                        Text = title,
                        AutoSize = true,
                        Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                        ForeColor = Color.FromArgb(79, 90, 100),
                        Padding = new Padding(0, 4, 8, 4)
                    };
                    var lblValue = new Label
                    {
                        Text = value,
                        AutoSize = true,
                        Font = new Font("Segoe UI", 11F, FontStyle.Regular),
                        ForeColor = Color.FromArgb(33, 37, 41),
                        Padding = new Padding(0, 4, 0, 4),
                        MaximumSize = new Size(520, 0)
                    };

                    table.Controls.Add(lblTitle, 0, r);
                    table.Controls.Add(lblValue, 1, r);
                    table.RowCount++;
                }

                // Prefer DB-loaded info; fallback to row.Tag or cell values
                var infoTag = info ?? row.Tag as QLChuoiNhaHangKhachSan.GUI.BookingRowInfo;

                AddRow("Nhân viên", infoTag?.Staff ?? Convert.ToString(row.Cells[3].Value));
                AddRow("Số phiếu", infoTag?.Id ?? Convert.ToString(row.Cells[0].Value));
                AddRow("Khách hàng", infoTag?.Customer ?? Convert.ToString(row.Cells[1].Value));
                AddRow("Ngày lập", infoTag?.Date ?? Convert.ToString(row.Cells[2].Value));
                AddRow("CCCD", infoTag?.CCCD ?? "");
                AddRow("Email", infoTag?.Email ?? "");
                AddRow("SĐT", infoTag?.SDT ?? "");
                AddRow("Giới tính", infoTag?.Gender ?? "");
                AddRow("Quốc tịch", infoTag?.Nationality ?? "");

                var detailRaw = infoTag?.Detail ?? Convert.ToString(row.Cells[4].Value);
                var phongText = detailRaw?.Replace(") ", ")\r\n");
                AddRow("Phòng", phongText);

                // show cancellation reason if any
                if (infoTag?.IsCancelled == true)
                {
                    AddRow("Lý do hủy", infoTag.CancellationReason ?? "Không rõ");
                }

                var scroll = new Panel
                {
                    Dock = DockStyle.Fill,
                    AutoScroll = true
                };
                scroll.Controls.Add(table);

                container.Controls.Add(scroll);
                container.Controls.Add(header);
                f.Controls.Add(container);

                f.ShowDialog(this);
            }
        }

        private void Guna2Button1_Click(object sender, EventArgs e)
        {
            using (var dlg = new BookingRoom_Details())
            {
                dlg.IsDatTruoc = true;
                var allRooms = LoadAllRoomsFromDb();
                dlg.SetAvailableRooms(allRooms);

                var res = dlg.ShowDialog();

                if (res == DialogResult.OK)
                {
                    string khach = dlg.ResultCustomerName;
                    string cccd = dlg.ResultCCCD;
                    string sdt = dlg.ResultSDT;
                    string email = dlg.ResultEmail;
                    string gender = dlg.ResultGender;
                    string nat = dlg.ResultNationality;
                    string phong = dlg.ResultRooms;
                    string thoigian = dlg.ResultDateRange;
                    DateTime startDate = dlg.ResultStartDate;
                    DateTime endDate = dlg.ResultEndDate;
                    string detailText = dlg.ResultRoomDetails;

                    var addedInfo = AddBookingRowInternal(khach, cccd, sdt, phong, thoigian, startDate, endDate, gender, nat, detailText, email);

                    try
                    {
                        var listForm = Application.OpenForms.OfType<ListRoom>().FirstOrDefault();
                        if (listForm != null && !string.IsNullOrWhiteSpace(phong))
                        {
                            var codes = phong.Split(new[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                            // Luôn cập nhật trạng thái phòng ngay lập tức
                            listForm.MarkRoomsAsBooked(codes, khach);
                            listForm.RefreshFromBookings(codes);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    RefreshAutocompleteSource();

                    // Send confirmation email (best effort) using the created BookingRowInfo directly
                    if (addedInfo != null && !string.IsNullOrWhiteSpace(addedInfo.Email))
                    {
                        try
                        {
                            EmailHelper.SendBookingConfirmation(addedInfo);
                        }
                        catch (Exception exEmail)
                        {
                            System.Diagnostics.Debug.WriteLine("Email error: " + exEmail.Message);
                        }
                    }
                }
            }
        }

        private List<(string RoomCode, string RoomType)> LoadAllRoomsFromDb()
        {
            var result = new List<(string, string)>();
            try
            {
                var connStr = ConfigurationManager.ConnectionStrings["ConnStr"]?.ConnectionString;
                if (string.IsNullOrWhiteSpace(connStr))
                    return result;

                using (var conn = new SqlConnection(connStr))
                using (var cmd = new SqlCommand(
                    @"SELECT r.RoomId AS RoomNumber, 
                      CASE WHEN r.IsVip = 1 THEN rt.TypeName + N' VIP' ELSE rt.TypeName END AS TypeName 
                      FROM dbo.Rooms r 
                      INNER JOIN dbo.RoomTypes rt ON r.RoomTypeId = rt.RoomTypeId
                      ORDER BY r.RoomId", conn))
                {
                    conn.Open();
                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            string code = rd["RoomNumber"]?.ToString().Trim();
                            string type = rd["TypeName"]?.ToString().Trim();
                            if (!string.IsNullOrWhiteSpace(code))
                                result.Add((code, type));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không tải được danh sách phòng từ CSDL.\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return result;
        }

        private List<(string RoomCode, string RoomType)> LoadAvailableRoomsFromDb()
        {
            return LoadAllRoomsFromDb();
        }

        private BookingRowInfo AddBookingRowInternal(string khach, string cccd, string sdt, string phong, string thoigian, DateTime? startDate = null, DateTime? endDate = null, string gender = "", string nat = "", string detailOverride = null, string email = null)
        {
            int rowIndex = this.guna2DataGridView1.Rows.Add();
            var row = this.guna2DataGridView1.Rows[rowIndex];
            string id = "PT" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
            string ngayLap = DateTime.Now.ToShortDateString();
            row.Cells[0].Value = id;
            row.Cells[1].Value = khach;
            row.Cells[2].Value = ngayLap;
            row.Cells[3].Value = "Trường Phi";
            string detailText = detailOverride ?? (phong + " (" + thoigian + ")");
            row.Cells[4].Value = detailText;
            row.Cells[5].Value = "     X";

            var bookingInfo = new BookingRowInfo
            {
                Id = id,
                Customer = khach,
                Date = ngayLap,
                Nationality = nat,
                Detail = detailText,
                CCCD = cccd,
                SDT = sdt,
                Email = email,
                Gender = gender,
                Staff = "Trường Phi"
            };

            row.Tag = bookingInfo;

            if (!string.IsNullOrWhiteSpace(phong) && startDate.HasValue && endDate.HasValue)
            {
                var codes = phong.Split(new[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                BookingManager.AddBooking(codes, new BookingInfo
                {
                    Customer = khach,
                    Start = startDate.Value,
                    End = endDate.Value
                });
            }

            RefreshAutocompleteSource();

            return bookingInfo;
        }

        public void AddBooking(string khach, string cccd, string sdt, string phong, string thoigian)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => AddBooking(khach, cccd, sdt, phong, thoigian)));
                return;
            }
            AddBookingRowInternal(khach, cccd, sdt, phong, thoigian);
            RefreshAutocompleteSource();
        }

        private void AddBookingRowFromDb(BookingRowInfo info)
        {
            int rowIndex = this.guna2DataGridView1.Rows.Add();
            var row = this.guna2DataGridView1.Rows[rowIndex];
            row.Cells[0].Value = info.Id;
            row.Cells[1].Value = info.Customer;
            row.Cells[2].Value = info.Date;
            row.Cells[3].Value = info.Staff;
            var detailText = info.Detail ?? string.Empty;
            if (info.IsCancelled)
            {
                var note = $"Phòng này đã bị hủy: {info.CancellationReason ?? "Không rõ"}";
                detailText = note + (string.IsNullOrWhiteSpace(detailText) ? string.Empty : "\r\n" + detailText);
                try { row.DefaultCellStyle.ForeColor = Color.DarkGray; } catch { }
            }
            row.Cells[4].Value = detailText;
            row.Cells[5].Value = "     X";
            row.Tag = info;
        }

        private IEnumerable<string> GetActiveRoomCodesFromDb(DateTime reference)
        {
            var result = new List<string>();
            var connStr = ConfigurationManager.ConnectionStrings["ConnStr"]?.ConnectionString;
            if (string.IsNullOrWhiteSpace(connStr)) return result;

            try
            {
                using (var conn = new SqlConnection(connStr))
                using (var cmd = new SqlCommand(@"SELECT DISTINCT d.RoomId FROM dbo.BookingDetails d WHERE d.CheckOut > @RefTime", conn))
                {
                    cmd.Parameters.AddWithValue("@RefTime", reference);
                    conn.Open();
                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            var code = rd[0]?.ToString();
                            if (!string.IsNullOrWhiteSpace(code))
                                result.Add(code.Trim().ToUpper());
                        }
                    }
                }
            }
            catch { }
            return result;
        }


        private void btnDatPhong_Click(object sender, EventArgs e)
        {
        }

        // Public wrapper so other forms can request reload of booking list after DB changes
        public void RefreshBookingsFromDb()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(RefreshBookingsFromDb));
                return;
            }
            LoadBookingsFromDatabase();
        }

        // Mark a booking row in the grid as cancelled and attach a cancellation reason for display
        public void MarkBookingAsCancelled(string bookingCode, string reason)
        {
            if (string.IsNullOrWhiteSpace(bookingCode)) return;
            if (InvokeRequired)
            {
                Invoke(new Action(() => MarkBookingAsCancelled(bookingCode, reason)));
                return;
            }

            foreach (DataGridViewRow row in guna2DataGridView1.Rows)
            {
                if (row.IsNewRow) continue;
                var code = Convert.ToString(row.Cells[0].Value);
                if (string.Equals(code, bookingCode, StringComparison.OrdinalIgnoreCase))
                {
                    var info = row.Tag as BookingRowInfo ?? new BookingRowInfo { Id = code };
                    info.IsCancelled = true;
                    info.CancellationReason = reason ?? "Không rõ";

                    // Prepend cancellation note to detail column for visibility
                    var oldDetail = Convert.ToString(row.Cells[4].Value) ?? info.Detail ?? string.Empty;
                    var note = $"Phòng này đã bị hủy: {info.CancellationReason}";
                    row.Cells[4].Value = note + (string.IsNullOrWhiteSpace(oldDetail) ? string.Empty : "\r\n" + oldDetail);
                    row.Tag = info;

                    // visually mark row
                    try { row.DefaultCellStyle.ForeColor = Color.DarkGray; } catch { }
                    break;
                }
            }
        }
    }
}