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

            using (Room_Details frm = new Room_Details(ma, ttHienTai))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    // 1. Cập nhật tên khách (Bạn đã làm được phần này)
                    lblStatus.Text = frm.TenKhach;
                    labSanSang.Visible = false;
                    picSanSang.Visible = false;

                    
                    if (ttHienTai.ToLower().Contains("trống"))
                    {
                        labTrangthai.Text = "Phòng đã đặt"; // Đổi chữ ở góc nhỏ

                        pnlRoomContainer.FillColor = Color.Green;
                        lblStatus.ForeColor = Color.Black;     // Chữ đen trên nền vàng
                        labTrangthai.ForeColor = Color.DarkSlateGray;
                    }
                    else if (ttHienTai.ToLower().Contains("đã đặt"))
                    {
                        labTrangthai.Text = "Đang thuê";
                        pnlRoomContainer.FillColor = Color.MidnightBlue;
                        lblStatus.ForeColor = Color.White;     // Chữ trắng trên nền xanh đậm
                        labRoomNumber.ForeColor = Color.White;
                        labTrangthai.ForeColor = Color.Yellow; // Trạng thái màu vàng cho nổi bật
                        this.CapNhatNhanPhong(frm.TenKhach, frm.SoNgay);
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
            pnlRoomContainer.FillColor = Color.Khaki;
            lblStatus.ForeColor = Color.Black;
            labTrangthai.ForeColor = Color.DarkSlateGray;
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
            pnlRoomContainer.FillColor = Color.Khaki;
            lblStatus.ForeColor = Color.Black;
            labTrangthai.ForeColor = Color.DarkSlateGray;
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
            pnlRoomContainer.FillColor = Color.MidnightBlue;
            lblStatus.ForeColor = Color.White;
            labRoomNumber.ForeColor = Color.White;
            labTrangthai.ForeColor = Color.Yellow;
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
            pnlRoomContainer.FillColor = Color.LightGray;
            lblStatus.ForeColor = Color.Black;
            labRoomNumber.ForeColor = Color.Black;
            labTrangthai.ForeColor = Color.Black;
            if (labThoiGian != null)
            {
                labThoiGian.Text = "Thời gian";
                labThoiGian.ForeColor = Color.Black;
            }
            if (labSanSang != null)
            {
                labSanSang.Text = "Sẵn sàng";
                labSanSang.ForeColor = Color.Black;
                labSanSang.Visible = true;
            }
            if (picSanSang != null) picSanSang.Visible = true;
        }
    }
}