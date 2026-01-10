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

        private class BookingRowInfo
        {
            public string Id { get; set; }
            public string Customer { get; set; }
            public string Date { get; set; }
            public string Staff { get; set; }
            public string Detail { get; set; }
            public string CCCD { get; set; }
            public string SDT { get; set; }
            public string Gender { get; set; }
            public string Nationality { get; set; }
            public DateTime? CreatedDate { get; set; }
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

            // Xóa ở bảng Detail trước rồi mới xóa bảng Booking để tránh lỗi khóa ngoại
            string query = @"
                DECLARE @BID INT;
                SELECT @BID = BookingID FROM dbo.Booking WHERE BookingCode = @Code;

                IF @BID IS NOT NULL
                BEGIN
                    DELETE FROM dbo.BookingDetail WHERE BookingID = @BID;
                    DELETE FROM dbo.Booking WHERE BookingID = @BID;
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
                    MessageBox.Show("Lỗi SQL khi xóa: " + ex.Message);
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
                using (var cmd = new SqlCommand(@"SELECT b.BookingCode, b.CreatedDate, b.EmployeeID,
                                                 c.FullName AS CustomerName, c.IdCard, c.Phone, c.Gender, c.Nationality,
                                                 d.RoomID, d.CheckIn, d.CheckOut
                                              FROM dbo.Booking b
                                              INNER JOIN dbo.Customer c ON b.CustomerID = c.CustomerID
                                              LEFT JOIN dbo.BookingDetail d ON b.BookingID = d.BookingID
                                              ORDER BY b.CreatedDate DESC, b.BookingID DESC", conn))
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
                                string staff = rd["EmployeeID"] != DBNull.Value ? "NV #" + rd["EmployeeID"] : string.Empty;
                                info = new BookingRowInfo
                                {
                                    Id = code,
                                    Customer = rd["CustomerName"]?.ToString() ?? "",
                                    Date = created.HasValue ? created.Value.ToString("dd/MM/yyyy") : "",
                                    Staff = staff,
                                    CCCD = rd["IdCard"]?.ToString() ?? "",
                                    SDT = rd["Phone"]?.ToString() ?? "",
                                    Gender = rd["Gender"]?.ToString() ?? "",
                                    Nationality = rd["Nationality"]?.ToString() ?? "",
                                    CreatedDate = created
                                };
                                bookings[code] = info;
                                order.Add(code);
                            }

                            DateTime? checkIn = rd["CheckIn"] != DBNull.Value ? (DateTime?)rd["CheckIn"] : null;
                            DateTime? checkOut = rd["CheckOut"] != DBNull.Value ? (DateTime?)rd["CheckOut"] : null;

                            if (rd["RoomID"] != DBNull.Value)
                            {
                                var detailParts = new StringBuilder();
                                detailParts.Append(rd["RoomID"].ToString());
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
                                    BookingManager.AddBooking(rd["RoomID"].ToString(), new BookingInfo
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
                var info = row.Tag as BookingRowInfo;
                string cccd = info?.CCCD ?? "";
                string sdt = info?.SDT ?? "";
               
                string gender = info?.Gender ?? "";
                string nat = info?.Nationality ?? "";

                ShowDetailForm(row, cccd, sdt, gender, nat);
            }
        }

        private void ShowDetailForm(DataGridViewRow row, string cccd, string sdt, string gender, string nat)
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

                AddRow("Nhân viên", Convert.ToString(row.Cells[3].Value));
                AddRow("Số phiếu", Convert.ToString(row.Cells[0].Value));
                AddRow("Khách hàng", Convert.ToString(row.Cells[1].Value));
                AddRow("Ngày lập", Convert.ToString(row.Cells[2].Value));
                AddRow("CCCD", cccd);
                AddRow("SĐT", sdt);
                AddRow("Giới tính", gender);
                AddRow("Quốc tịch", nat);
                var infoTag = row.Tag as BookingRowInfo;
                var detailRaw = infoTag?.Detail ?? Convert.ToString(row.Cells[4].Value);
                var phongText = detailRaw?.Replace(") ", ")\r\n");
                AddRow("Phòng", phongText);

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
                    string gender = dlg.ResultGender;
                    string nat = dlg.ResultNationality;
                    string phong = dlg.ResultRooms;
                    string thoigian = dlg.ResultDateRange;
                    DateTime startDate = dlg.ResultStartDate;
                    DateTime endDate = dlg.ResultEndDate;
                    string detailText = dlg.ResultRoomDetails;

                    AddBookingRowInternal(khach, cccd, sdt, phong, thoigian, startDate, endDate, gender, nat, detailText);

                    try
                    {
                        var listForm = Application.OpenForms.OfType<ListRoom>().FirstOrDefault();
                        if (listForm != null && !string.IsNullOrWhiteSpace(phong))
                        {
                            var codes = phong.Split(new[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                            if (startDate.Date <= DateTime.Now.Date)
                            {
                                listForm.MarkRoomsAsBooked(codes, khach);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
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
                    @"SELECT r.RoomID AS RoomNumber, 
                      CASE WHEN r.IsVip = 1 THEN rt.RoomType + N' VIP' ELSE rt.RoomType END AS TypeName 
                      FROM dbo.Room r 
                      INNER JOIN dbo.RoomType rt ON r.RoomTypeID = rt.RoomTypeID
                      ORDER BY r.RoomID", conn))
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

        private void AddBookingRowInternal(string khach, string cccd, string sdt, string phong, string thoigian, DateTime? startDate = null, DateTime? endDate = null, string gender = "", string nat = "", string detailOverride = null)
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

            row.Tag = new BookingRowInfo
            {
                Id = id,
                Customer = khach,
                Date = ngayLap,
                Nationality = nat,
                Detail = detailText,
                CCCD = cccd,
                SDT = sdt,
                Gender = gender,
                Staff = "Trường Phi"
            };

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
            row.Cells[4].Value = info.Detail;
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
                using (var cmd = new SqlCommand(@"SELECT DISTINCT d.RoomID FROM dbo.BookingDetail d WHERE d.CheckOut > @RefTime", conn))
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
    }
}