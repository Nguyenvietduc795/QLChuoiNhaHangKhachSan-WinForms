using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            ThemTieuDeNhom("Phòng Đơn (12)");
            TaoNhomPhong(1, 12, "Phòng đơn");

            ThemTieuDeNhom("Phòng Đôi (8)");
            TaoNhomPhong(13, 20, "Phòng đôi");

            ThemTieuDeNhom("Phòng Gia Đình (8)");
            TaoNhomPhong(21, 28, "Phòng gia đình");

            LocKetHop(); // Lọc mặc định khi mở app

            // Apply any existing bookings to UI
            RefreshBookingStates();
        }

        // Hàm bổ trợ để tạo phòng theo dải số i
        private void TaoNhomPhong(int start, int end, string loaiPhong)
        {
            int vipCount = 0;
            string loaiLower = loaiPhong.ToLower();
            if (loaiLower.Contains("đơn")) vipCount = 4; // 4 phòng VIP đầu cho phòng đơn
            else if (loaiLower.Contains("đôi")) vipCount = 3; // 3 phòng VIP đầu cho phòng đôi
            else if (loaiLower.Contains("gia đình")) vipCount = 3; // 3 phòng VIP đầu cho phòng gia đình

            for (int i = start; i <= end; i++)
            {
                UcRoom room = new UcRoom();
                room.ViewTimeProvider = () => GetSelectedViewTime();
                room.labRoomNumber.Text = "P" + i.ToString("D3");

                bool isVip = (i - start) < vipCount;
                string tagValue = loaiPhong.ToLower().Trim();
                if (isVip) tagValue += " vip";
                room.Tag = tagValue; // Lưu loại phòng + vip
                room.SetVip(isVip);


                room.Click += (s, e) => {
                    string ma = room.labRoomNumber.Text; // Ví dụ: "P001"
                    string trangThaiHienTai = room.labTrangthai.Text; // Ví dụ: "Phòng Trống"
                    var viewTime = GetSelectedViewTime();

                    // Build list of currently unavailable rooms from LayoutPhong controls
                    var booked = new List<string>();
                    foreach (Control c in LayoutPhong.Controls)
                    {
                        if (c is UcRoom ur)
                        {
                            var code = ur.labRoomNumber.Text?.Trim();
                            var tt = ur.labTrangthai.Text?.ToLower() ?? "";
                            if (!string.IsNullOrEmpty(code) && (tt.Contains("đặt") || tt.Contains("thuê")))
                            {
                                booked.Add(code);
                            }
                        }
                    }

                    // Open BookingRoom_Details preselecting this room
                    using (var booking = new BookingRoom_Details(ma, viewTime))
                    {
                        // exclude already booked rooms so available list matches current UI
                        booking.SetUnavailableRooms(booked);

                        if (booking.ShowDialog() == DialogResult.OK)
                        {
                            // Update this UcRoom UI
                            room.MarkBooked(booking.ResultCustomerName);
                            room.SetVip(isVip);

                            // register booking already handled inside BookingRoom_Details via BookingManager

                            // Refresh all rooms from manager to update other rooms if needed
                            RefreshBookingStates();

                            // Also add booking row to BooKing_Form if it's open
                            try
                            {
                                var bokForm = Application.OpenForms.OfType<BooKing_Form>().FirstOrDefault();
                                if (bokForm != null)
                                {
                                    bokForm.AddBooking(booking.ResultCustomerName, booking.ResultCCCD, booking.ResultSDT, booking.ResultRooms, booking.ResultDateRange);
                                }
                            }
                            catch
                            {
                                // ignore
                            }
                        }
                    }
                };
                LayoutPhong.Controls.Add(room);
            }
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

        // Refresh UI of rooms according to BookingManager
        private void RefreshBookingStates(HashSet<string> filterRoomCodes = null)
        {
            var viewTime = GetSelectedViewTime();
            foreach (Control ctr in LayoutPhong.Controls)
            {
                if (ctr is UcRoom room)
                {
                    var code = room.labRoomNumber.Text?.Trim().ToUpper();
                    if (string.IsNullOrEmpty(code)) continue;

                    // Nếu có danh sách filter thì chỉ cập nhật các phòng trong danh sách đó
                    if (filterRoomCodes != null && !filterRoomCodes.Contains(code))
                        continue;

                    if (BookingManager.TryGetBooking(code, out var info))
                    {
                        // Determine state based on selected view time
                        if (viewTime < info.Start)
                        {
                            // Reserved in future
                            room.SetReserved(info.Customer);
                        }
                        else if (viewTime >= info.Start && viewTime < info.End)
                        {
                            // Currently rented
                            int days = Math.Max(1, (int)Math.Ceiling((info.End - info.Start).TotalDays));
                            room.SetRented(info.Customer, days);
                        }
                        else
                        {
                            // booking expired; treat as free
                            room.SetFree();
                        }
                    }
                    else
                    {
                        // no booking
                        room.SetFree();
                    }
                }
            }

            UpdateSearchAutocomplete();
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
