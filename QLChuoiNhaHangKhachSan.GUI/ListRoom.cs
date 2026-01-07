using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;

namespace QLChuoiNhaHangKhachSan.GUI
{
    public partial class ListRoom : Form
    {

        public ListRoom()
        {
            InitializeComponent();
            // Gọi nạp phòng theo phân loại khi mở Form
            NapDanhSachPhongTheoLoai();

            // Autocomplete cho ô tìm kiếm
            TryInitSearchAutocomplete();

            // Hook time picker to open clock-style dialog
            if (dtGio != null)
            {
                dtGio.MouseDown += DtGio_MouseDown;
                dtGio.Click += DtGio_Click;
            }
        }

        private void TryInitSearchAutocomplete()
        {
            try
            {
                if (txtTimkiem != null)
                {
                    txtTimkiem.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    txtTimkiem.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    UpdateSearchAutocomplete();
                }
            }
            catch { }
        }

        private void UpdateSearchAutocomplete()
        {
            if (txtTimkiem == null) return;
            var src = new AutoCompleteStringCollection();
            foreach (Control ctr in LayoutPhong.Controls)
            {
                if (ctr is UcRoom room)
                {
                    var code = room.labRoomNumber.Text;
                    var guest = room.lblStatus.Text;
                    if (!string.IsNullOrWhiteSpace(code)) src.Add(code.Trim());
                    if (!string.IsNullOrWhiteSpace(guest) && !guest.Equals("Phòng Trống", StringComparison.OrdinalIgnoreCase))
                        src.Add(guest.Trim());
                }
            }
            txtTimkiem.AutoCompleteCustomSource = src;
        }

        private void NapDanhSachPhongTheoLoai()
        {
            LayoutPhong.Controls.Clear();

            var connStr = ConfigurationManager.ConnectionStrings["ConnStr"]?.ConnectionString;
            if (string.IsNullOrWhiteSpace(connStr))
            {
                return;
            }

            var rooms = new List<(string RoomNumber, string Status, string TypeName)>();
            try
            {
                using (var conn = new SqlConnection(connStr))
                using (var cmd = new SqlCommand(
                    "SELECT r.RoomID AS RoomNumber, r.Status, CASE WHEN r.IsVip = 1 THEN rt.RoomType + N' VIP' ELSE rt.RoomType END AS TypeName FROM dbo.Room r JOIN dbo.RoomType rt ON r.RoomTypeID = rt.RoomTypeID ORDER BY r.RoomID", conn))
                {
                    conn.Open();
                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            rooms.Add((rd.GetString(0), rd.GetString(1), rd.GetString(2)));
                        }
                    }
                }
            }
            catch
            {
                return;
            }

            // Nhóm theo loại (đơn/đôi/gia đình)
            Func<string, string> mapGroup = t =>
            {
                var low = t.ToLowerInvariant();
                if (low.Contains("đơn")) return "Phòng Đơn";
                if (low.Contains("đôi")) return "Phòng Đôi";
                return "Phòng Gia Đình";
            };

            var order = new List<string> { "Phòng Đơn", "Phòng Đôi", "Phòng Gia Đình" };
            var grouped = rooms.GroupBy(r => mapGroup(r.TypeName))
                               .OrderBy(g => order.IndexOf(g.Key));

            foreach (var g in grouped)
            {
                ThemTieuDeNhom($"{g.Key} ({g.Count()})");

                foreach (var r in g)
                {
                    var typeName = r.TypeName; // capture for click handler
                    UcRoom room = new UcRoom();
                    room.ViewTimeProvider = () => GetSelectedViewTime();
                    room.labRoomNumber.Text = r.RoomNumber;

                    bool isVip = typeName.ToLowerInvariant().Contains("vip");
                    string tagValue = g.Key.ToLower().Trim();
                    if (isVip) tagValue += " vip";
                    room.Tag = tagValue; // dùng cho lọc
                    room.AccessibleDescription = typeName; // lưu đúng loại từ DB để truyền sang form chi tiết
                    room.SetVip(isVip);

                    // Set status
                    var status = r.Status.ToLowerInvariant();
                    if (status.Contains("thuê") || status.Contains("nhận") || status.Contains("sử dụng"))
                        room.SetRented("", 1);
                    else if (status.Contains("đặt") || status.Contains("giữ"))
                        room.SetReserved("");
                    else
                        room.SetFree();

                    // click handler giữ nguyên logic cũ, nhưng khóa loại phòng và chỉ cho phép phòng này
                    room.Click += (s, e) =>
                    {
                        string ma = room.labRoomNumber.Text;
                        string loaiPhong = room.AccessibleDescription ?? typeName;
                        var viewTime = GetSelectedViewTime();

                        var booked = new List<string>();
                        foreach (Control c in LayoutPhong.Controls)
                        {
                            if (c is UcRoom ur)
                            {
                                var code = ur.labRoomNumber.Text?.Trim();
                                var tt = ur.labTrangthai.Text?.ToLower() ?? "";
                                if (!string.IsNullOrEmpty(code) && (tt.Contains("đặt") || tt.Contains("thuê")))
                                    booked.Add(code);
                            }
                        }

                        using (var booking = new Room_Details(ma, room.labTrangthai.Text, loaiPhong, viewTime))
                        {
                            if (booking.ShowDialog() == DialogResult.OK)
                            {
                                room.MarkBooked(booking.TenKhach);
                                room.SetVip(isVip);
                                RefreshBookingStates();
                                try
                                {
                                    var bokForm = Application.OpenForms.OfType<BooKing_Form>().FirstOrDefault();
                                    if (bokForm != null)
                                    {
                                        bokForm.AddBooking(booking.TenKhach, "", "", ma, "");
                                    }
                                }
                                catch { }
                            }
                        }
                    };

                    LayoutPhong.Controls.Add(room);
                }
            }

            LocKetHop();
            RefreshBookingStates();
        }

        private void ThemTieuDeNhom(string tenNhom)
        {
            Label lbl = new Label();
            lbl.Text = tenNhom.ToUpper();
            lbl.Font = new Font("Microsoft Sans Serif", 12, FontStyle.Bold);
            lbl.ForeColor = Color.FromArgb(26, 31, 51);
            // Độ rộng bằng Layout để ép các ô phòng xuống hàng mới
            lbl.Size = new Size(LayoutPhong.Width -30, 30);
            lbl.TextAlign = ContentAlignment.BottomLeft;
            lbl.Padding = new Padding(10, 0, 0, 10);

            LayoutPhong.Controls.Add(lbl);
        }
        private void LocKetHop()
        {
            // 1. Lấy điều kiện (Giữ nguyên như bạn đã viết)
            string locTrangThai = radTrong.Checked ? "trống" :
                                  radDangthue.Checked ? "thuê" :
                                  radDadat.Checked ? "đặt" : "tất cả";

            string locLoaiPhong = radDon.Checked ? "đơn" :
                                  radDoi.Checked ? "đôi" :
                                  radGiadinh.Checked ? "gia đình" : "tất cả";

            foreach (Control ctr in LayoutPhong.Controls)
            {
                if (ctr is UcRoom room)
                {
                    // Lấy văn bản thực tế và chuyển về chữ thường
                    string ttThucTe = room.labTrangthai.Text.ToLower();
                    string lpThucTe = room.Tag != null ? room.Tag.ToString().ToLower() : "";


                    bool khopTT = (locTrangThai == "tất cả" || ttThucTe.Contains(locTrangThai));
                    bool khopLP = (locLoaiPhong == "tất cả" || lpThucTe.Contains(locLoaiPhong));
                    room.Visible = (khopTT && khopLP);
                }
                else if (ctr is Label lbl)
                {

                    lbl.Visible = true;
                }
            }
        }

        // Public method so external forms can mark rooms as booked
        public void MarkRoomsAsBooked(IEnumerable<string> roomCodes, string tenKhach)
        {
            if (roomCodes == null) return;
            // Normalize codes set for fast lookup
            var set = new HashSet<string>(roomCodes.Where(r => !string.IsNullOrWhiteSpace(r)).Select(r => r.Trim().ToUpper()));
            foreach (Control ctr in LayoutPhong.Controls)
            {
                if (ctr is UcRoom room)
                {
                    string code = room.labRoomNumber.Text?.Trim().ToUpper();
                    if (set.Contains(code))
                    {
                        room.MarkBooked(tenKhach);
                    }
                }
            }
        }

        // Refresh UI of rooms according to DB bookings at selected time
        private void RefreshBookingStates(HashSet<string> filterRoomCodes = null)
        {
            var viewTime = GetSelectedViewTime();
            var activeBookings = GetActiveBookingsFromDb(viewTime);
            var lookup = activeBookings.GroupBy(b => b.RoomCode.ToUpper())
                                       .ToDictionary(g => g.Key, g => g.First());

            foreach (Control ctr in LayoutPhong.Controls)
            {
                if (ctr is UcRoom room)
                {
                    var code = room.labRoomNumber.Text?.Trim().ToUpper();
                    if (string.IsNullOrEmpty(code)) continue;
                    if (filterRoomCodes != null && !filterRoomCodes.Contains(code))
                        continue;

                    RoomBookingState state;
                    if (lookup.TryGetValue(code, out state))
                    {
                        // Chỉ hiển thị trạng thái khi viewTime nằm trong khoảng [Start, End)
                        if (viewTime >= state.Start && viewTime < state.End)
                        {
                            // Truyền thời gian checkout thực tế và viewTime để đếm ngược chính xác
                            room.SetRented(state.Customer, state.End, viewTime);
                        }
                        else
                        {
                            // Nếu viewTime < Start hoặc viewTime >= End thì phòng trống
                            room.SetFree();
                        }
                    }
                    else
                    {
                        room.SetFree();
                    }
                }
            }

            UpdateSearchAutocomplete();
        }

        private struct RoomBookingState
        {
            public string RoomCode;
            public string Customer;
            public DateTime Start;
            public DateTime End;
        }

        private List<RoomBookingState> GetActiveBookingsFromDb(DateTime reference)
        {
            var result = new List<RoomBookingState>();
            var connStr = ConfigurationManager.ConnectionStrings["ConnStr"]?.ConnectionString;
            if (string.IsNullOrWhiteSpace(connStr)) return result;

            try
            {
                using (var conn = new SqlConnection(connStr))
                using (var cmd = new SqlCommand(@"SELECT d.RoomID, b.CustomerID, c.FullName, d.CheckIn, d.CheckOut
FROM dbo.BookingDetail d
JOIN dbo.Booking b ON d.BookingID = b.BookingID
JOIN dbo.Customer c ON b.CustomerID = c.CustomerID
WHERE d.CheckIn <= @RefTime AND d.CheckOut > @RefTime", conn))
                {
                    cmd.Parameters.AddWithValue("@RefTime", reference);
                    conn.Open();
                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            var room = rd["RoomID"]?.ToString();
                            if (string.IsNullOrWhiteSpace(room)) continue;
                            DateTime? start = rd["CheckIn"] != DBNull.Value ? (DateTime?)rd["CheckIn"] : null;
                            DateTime? end = rd["CheckOut"] != DBNull.Value ? (DateTime?)rd["CheckOut"] : null;
                            if (!start.HasValue || !end.HasValue) continue;
                            result.Add(new RoomBookingState
                            {
                                RoomCode = room.Trim(),
                                Customer = rd["FullName"]?.ToString(),
                                Start = start.Value,
                                End = end.Value
                            });
                        }
                    }
                }
            }
            catch { }

            return result;
        }

        private void txtTimkiem_TextChanged(object sender, EventArgs e)
        {
            string tuKhoa = txtTimkiem.Text.ToLower().Trim();
            foreach (Control ctr in LayoutPhong.Controls)
            {

                if (ctr is UcRoom room)
                {
                    string soPhong = room.labRoomNumber.Text.ToLower();
                    string tenKhach = room.lblStatus.Text.ToLower();
                    if (string.IsNullOrEmpty(tuKhoa))
                    {
                        LocKetHop();
                    }
                    else
                    {
                        room.Visible = (soPhong.Contains(tuKhoa) || tenKhach.Contains(tuKhoa));
                    }
                }
                else if (ctr is Label lbl)
                {

                    lbl.Visible = true;
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {

            DialogResult dr = MessageBox.Show("Bạn có chắc chắn muốn thoát chương trình không?",
                                             "Xác nhận thoát",
                                             MessageBoxButtons.YesNo,
                                             MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
        private void btnMini_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnMax_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
            }
        }

        private void FormBookinng_Load(object sender, EventArgs e)
        {

        }

        private void guna2Panel2_Paint(object sender, PaintEventArgs e)
        {

        }


        private void radTrong_CheckedChanged(object sender, EventArgs e)
        {

            if (radTrong.Checked) LocKetHop();
        }

        private void dtGio_ValueChanged(object sender, EventArgs e)
        {
            RefreshBookingStates();
        }

        // New handlers: open the clock-style TimePickerForm and apply result back to dtGio
        private void DtGio_Click(object sender, EventArgs e)
        {
            ShowTimePicker(dtGio);
        }

        private void DtGio_MouseDown(object sender, MouseEventArgs e)
        {
            ShowTimePicker(dtGio);
        }

        // Generic ShowTimePicker that works with Guna2DateTimePicker or standard DateTimePicker
        private void ShowTimePicker(Control pickerControl)
        {
            if (pickerControl == null) return;

            DateTime initial = DateTime.Now;
            try
            {
                var prop = pickerControl.GetType().GetProperty("Value");
                if (prop != null)
                {
                    var val = prop.GetValue(pickerControl, null);
                    if (val is DateTime dt) initial = dt;
                }
            }
            catch { /* ignore reflection issues */ }

            using (var clock = new TimePickerForm(initial))
            {
                if (clock.ShowDialog(this) == DialogResult.OK)
                {
                    try
                    {
                        var prop = pickerControl.GetType().GetProperty("Value");
                        if (prop != null && prop.CanWrite)
                        {
                            prop.SetValue(pickerControl, clock.SelectedDateTime, null);
                        }
                    }
                    catch { /* ignore set errors */ }

                    // Update UI to reflect the new view time
                    RefreshBookingStates();
                }
            }
        }

        private void radDadat_CheckedChanged(object sender, EventArgs e)
        {
            if (radDadat.Checked) LocKetHop();
        }

        private void radDangthue_CheckedChanged(object sender, EventArgs e)
        {
            if (radDangthue.Checked) LocKetHop();
        }

        private void radTatcaphong_CheckedChanged(object sender, EventArgs e)
        {
             if(radTatcaphong.Checked) LocKetHop();
         }

        private void radDon_CheckedChanged(object sender, EventArgs e)
        {
            if (radDon.Checked) LocKetHop();
        }

        private void radDoi_CheckedChanged(object sender, EventArgs e)
        {
            if (radDoi.Checked) LocKetHop();
        }

        private void radGiadinh_CheckedChanged(object sender, EventArgs e)
        {
            if (radGiadinh.Checked) LocKetHop();
        }

        private void radTatca_CheckedChanged(object sender, EventArgs e)
        {
            if (radTatca.Checked) LocKetHop();
        }

        private void guna2PictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        public void RefreshFromBookings()
        {
            RefreshBookingStates();
        }

        public void RefreshFromBookings(IEnumerable<string> roomCodes)
        {
            var set = roomCodes == null
                ? null
                : new HashSet<string>(roomCodes.Where(r => !string.IsNullOrWhiteSpace(r)).Select(r => r.Trim().ToUpper()));
            RefreshBookingStates(set);
        }

        private DateTime GetSelectedViewTime()
        {
            if (dtNgay != null && dtGio != null)
            {
                return dtNgay.Value.Date + dtGio.Value.TimeOfDay;
            }
            return DateTime.Now;
        }

        private void dtNgay_ValueChanged(object sender, EventArgs e)
        {
            RefreshBookingStates();
        }
    }
}
