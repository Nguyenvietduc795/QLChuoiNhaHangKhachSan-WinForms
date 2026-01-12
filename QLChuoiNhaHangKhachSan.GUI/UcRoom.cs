using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace QLChuoiNhaHangKhachSan.GUI
{
    public partial class UcRoom : UserControl
    {
        private DateTime endTime;
        private Timer timerCountdown;

        private readonly Color _freeBack = Color.FromArgb(216, 234, 248);
        private readonly Color _freeBorder = Color.FromArgb(146, 191, 229);
        private readonly Color _freeRoom = Color.FromArgb(10, 69, 110);
        private readonly Color _freeStatus = Color.FromArgb(24, 131, 74);
        private readonly Color _freeSub = Color.FromArgb(28, 41, 56);

        private readonly Color _reservedBack = Color.FromArgb(255, 236, 210);
        private readonly Color _reservedBorder = Color.FromArgb(214, 160, 90);
        private readonly Color _reservedRoom = Color.FromArgb(121, 74, 14);
        private readonly Color _reservedStatus = Color.FromArgb(185, 109, 0);
        private readonly Color _reservedSub = Color.FromArgb(92, 59, 28);
        private readonly Color _reservedFooterBack = Color.FromArgb(255, 232, 196);
        private readonly Color _reservedFooterBorder = Color.FromArgb(199, 138, 66);
        private readonly Color _reservedFooterText = Color.FromArgb(92, 59, 28);

        private readonly Color _rentedBack = Color.FromArgb(20, 31, 61);
        private readonly Color _rentedBorder = Color.FromArgb(63, 96, 149);
        private readonly Color _rentedRoom = Color.FromArgb(235, 242, 255);
        private readonly Color _rentedStatus = Color.FromArgb(255, 210, 92);
        private readonly Color _rentedSub = Color.FromArgb(202, 215, 239);

        public UcRoom()
        {
            InitializeComponent();

            // Avoid running runtime-only initialization in Designer
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            // QUAN TRỌNG: Phải đăng ký sự kiện click cho chính nó và các con
            this.Click += UcRoom_Click;
            RegisterEvents(this);

            // Khởi tạo timer
            timerCountdown = new Timer();
            timerCountdown.Interval = 1000;
            timerCountdown.Tick += TimerCountdown_Tick;
        }

        private void ApplyTheme(Color back, Color border, Color roomColor, Color statusColor, Color subColor)
        {
            if (pnlRoomContainer != null)
            {
                pnlRoomContainer.FillColor = back;
                pnlRoomContainer.FillColor2 = back;
                pnlRoomContainer.BorderColor = border;
                pnlRoomContainer.BorderThickness = 2;
            }

            if (labRoomNumber != null) labRoomNumber.ForeColor = roomColor;
            if (labTrangthai != null) labTrangthai.ForeColor = statusColor;
            if (lblStatus != null) lblStatus.ForeColor = subColor;
            if (labThoiGian != null) labThoiGian.ForeColor = subColor;
            if (labSanSang != null) labSanSang.ForeColor = subColor;
        }

        private void ApplyFooterTheme(Color back, Color border, Color text)
        {
            if (guna2GradientPanel1 != null)
            {
                guna2GradientPanel1.FillColor = back;
                guna2GradientPanel1.FillColor2 = back;
                guna2GradientPanel1.BorderColor = border;
            }
            if (labThoiGian != null) labThoiGian.ForeColor = text;
            if (labSanSang != null) labSanSang.ForeColor = text;
        }

        // Đã sửa: Hàm này giúp bấm vào nhãn hay hình ảnh đều mở được Form
        private void RegisterEvents(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                c.Click += UcRoom_Click;
                if (c.HasChildren) RegisterEvents(c);
            }
        }


        private void UcRoom_Click(object sender, EventArgs e)
        {
            string ma = labRoomNumber.Text;
            string ttHienTai = labTrangthai.Text; // Chữ "Phòng trống" nhỏ ở góc

            DateTime? viewTime = null;
            if (ViewTimeProvider != null)
            {
                viewTime = ViewTimeProvider();
            }

            using (Room_Details frm = new Room_Details(ma, ttHienTai, viewTime))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    // 1. Cập nhật tên khách (Bạn đã làm được phần này)
                    lblStatus.Text = frm.TenKhach;
                    labSanSang.Visible = false;
                    picSanSang.Visible = false;

                    // Cập nhật loại phòng lưu trong Tag để lọc
                    if (!string.IsNullOrWhiteSpace(frm.SelectedRoomType))
                    {
                        this.Tag = frm.SelectedRoomType.ToLower();
                    }

                    string status = frm.SelectedStatus ?? ttHienTai;

                    if (string.IsNullOrWhiteSpace(status))
                    {
                        SetFree();
                    }
                    else if (status.ToLower().Contains("đặt"))
                    {
                        SetReserved(frm.TenKhach);
                    }
                    else if (status.ToLower().Contains("thuê"))
                    {
                        SetRented(frm.TenKhach, frm.SoNgay);
                    }
                    else
                    {
                        SetFree();
                    }
                }
            }
        }
        public void CapNhatNhanPhong(string tenKhach, int soNgay)
        {
            lblStatus.Text = tenKhach;
            labTrangthai.Text = "Đang thuê";

            if (labSanSang != null) labSanSang.Visible = false;
            if (picSanSang != null) picSanSang.Visible = false;

            // Thiết lập thời gian kết thúc
            endTime = DateTime.Now.AddDays(soNgay);
            timerCountdown.Start(); // Kích hoạt đồng hồ chạy ngược
        }
        private void TimerCountdown_Tick(object sender, EventArgs e)
        {
            TimeSpan conLai = endTime - DateTime.Now;

            if (conLai.TotalSeconds <= 0)
            {
                timerCountdown.Stop();
                labThoiGian.Text = "Hết hạn!";
                labThoiGian.ForeColor = Color.Red;
            }
            else
            {
                labThoiGian.Text = string.Format("{0}d {1:00}:{2:00}:{3:00}",
                    conLai.Days, conLai.Hours, conLai.Minutes, conLai.Seconds);
            }
        }

        private void labRoomNumber_Click(object sender, EventArgs e)
        {

        }

        // Public helper to mark room as booked from external forms (BookingRoom_Details)
        public void MarkBooked(string tenKhach)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => MarkBooked(tenKhach)));
                return;
            }

            lblStatus.Text = tenKhach;
            labSanSang.Visible = false;
            picSanSang.Visible = false;
            labTrangthai.Text = "Phòng đã đặt";
            ApplyTheme(_reservedBack, _reservedBorder, _reservedRoom, _reservedStatus, _reservedSub);
            ApplyFooterTheme(_reservedFooterBack, _reservedFooterBorder, _reservedFooterText);
         }

        // New public helpers to update UI from ListRoom without accessing private members
        public void SetReserved(string tenKhach)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => SetReserved(tenKhach)));
                return;
            }
            lblStatus.Text = tenKhach;
            labTrangthai.Text = "Phòng đã đặt";
            ApplyTheme(_reservedBack, _reservedBorder, _reservedRoom, _reservedStatus, _reservedSub);
            labSanSang.Visible = false;
            if (picSanSang != null) picSanSang.Visible = false;
            ApplyFooterTheme(_reservedFooterBack, _reservedFooterBorder, _reservedFooterText);
          }

        public void SetRented(string tenKhach, int days)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => SetRented(tenKhach, days)));
                return;
            }
            lblStatus.Text = tenKhach;
            labTrangthai.Text = "Đang thuê";
            ApplyTheme(_rentedBack, _rentedBorder, _rentedRoom, _rentedStatus, _rentedSub);
            ApplyFooterTheme(Color.FromArgb(41, 57, 92), _rentedBorder, Color.White);
             // start countdown for days
             CapNhatNhanPhong(tenKhach, days);
        }

        public void SetFree()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => SetFree()));
                return;
            }
            timerCountdown.Stop();
            lblStatus.Text = "Phòng Trống";
            labTrangthai.Text = "Phòng trống";
            ApplyTheme(_freeBack, _freeBorder, _freeRoom, _freeStatus, _freeSub);
            ApplyFooterTheme(_freeBack, _freeBorder, _freeSub);
             if (labThoiGian != null)
             {
                 labThoiGian.Text = "Thời gian";
             }
             if (labSanSang != null)
             {
                 labSanSang.Text = "Sẵn sàng";
                 labSanSang.Visible = true;
             }
             if (picSanSang != null) picSanSang.Visible = true;
         }

        public void SetVip(bool isVip)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => SetVip(isVip)));
                return;
            }
            if (labVip != null)
            {
                labVip.Visible = isVip;
            }
        }

        // Được gán từ ListRoom để lấy thời gian đang xem (ngày/giờ) cho việc khóa ngày/giờ khi mở Room_Details
        public Func<DateTime> ViewTimeProvider { get; set; }

        private void lblStatus_Click(object sender, EventArgs e)
        {
            // no-op
        }

        private void UcRoom_Load(object sender, EventArgs e)
        {
            // no-op
        }
    }
 }