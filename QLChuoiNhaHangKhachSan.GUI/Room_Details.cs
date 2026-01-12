using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        private Label _lblCheckout;
        private Guna2Button _btnThanhToan;
        private Button _btnAddService;
        private Button _btnRemoveService;
        private BookingInfo _existing;
        private ContextMenuStrip _menuService;

        private const decimal ROOM_PRICE_PER_NIGHT = 499000m;
        private const decimal VIP_ROOM_PRICE_PER_NIGHT = 599000m;
        private const string ROOM_SERVICE_NAME = "Tiền phòng";

        public Room_Details(string maPhong, string trangThai, DateTime? lockedViewTime = null, bool lockDateTimePickers = true)
        {
            InitializeComponent();

            _lockedViewTime = lockedViewTime;
            _lockDateTimePickers = lockDateTimePickers && lockedViewTime.HasValue;

            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                labMaphong.Text = maPhong;
                return;
            }

            labMaphong.Text = maPhong;
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

            BookingManager.TryGetBooking(maPhong, out _existing);
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

        private void ApplyLockedViewTime()
        {
            if (!_lockedViewTime.HasValue) return;
            var view = _lockedViewTime.Value;

            // Cố định ngày/giờ bắt đầu theo thời gian đang xem, số ngày giữ nguyên
            dtNgay.Value = view.Date;
            dtGio.Value = view;

            var days = (int)Math.Max(1, nNgay.Value);
            var end = view.AddDays(days);
            UpdateCheckoutLabel(view, end, days);

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
            decimal pricePerNight = GetRoomPrice();
            // nếu grid chưa có dòng tiền phòng, thêm vào; nếu có thì cập nhật
            bool found = false;
            foreach (DataGridViewRow r in guna2DataGridView2.Rows)
            {
                if (r.IsNewRow) continue;
                if (string.Equals(Convert.ToString(r.Cells[0].Value), ROOM_SERVICE_NAME, StringComparison.OrdinalIgnoreCase))
                {
                    found = true;
                    decimal amount = pricePerNight * days;
                    r.Cells[1].Value = days; // qty
                    r.Cells[2].Value = pricePerNight.ToString("N0");
                    r.Cells[3].Value = amount.ToString("N0");
                    break;
                }
            }
            if (!found)
            {
                decimal amount = pricePerNight * days;
                int idx = guna2DataGridView2.Rows.Add();
                var row = guna2DataGridView2.Rows[idx];
                row.Cells[0].Value = ROOM_SERVICE_NAME;
                row.Cells[1].Value = days;
                row.Cells[2].Value = pricePerNight.ToString("N0");
                row.Cells[3].Value = amount.ToString("N0");
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


                // sau khi thanh toán, trả phòng về trạng thái trống
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
                        // Chỉ refresh đúng phòng vừa thanh toán, không ảnh hưởng phòng khác
                        listForm.RefreshFromBookings(new[] { labMaphong.Text });
                    }
                }
                catch { }
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
            this.Close();
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
            using (var f = new Form())
            {
                f.Text = "Thêm dịch vụ";
                f.FormBorderStyle = FormBorderStyle.FixedDialog;
                f.StartPosition = FormStartPosition.CenterParent;
                f.ClientSize = new System.Drawing.Size(520, 320); // larger dialog

                int lblX = 20;
                int ctrlX = 20;
                int width = 480;

                var lblName = new Label { Left = lblX, Top = 20, Text = "Tên dịch vụ", AutoSize = true };
                var txtName = new TextBox { Left = ctrlX, Top = 45, Width = width };
                var lblPrice = new Label { Left = lblX, Top = 85, Text = "Đơn giá", AutoSize = true };
                var txtPrice = new TextBox { Left = ctrlX, Top = 110, Width = width };
                var lblQty = new Label { Left = lblX, Top = 150, Text = "Số lượng", AutoSize = true };
                var numQty = new NumericUpDown { Left = ctrlX, Top = 175, Width = 120, Minimum = 1, Maximum = 999, Value = 1 };
                var btnOk = new Button { Text = "OK", Left = 320, Width = 80, Top = 240, DialogResult = DialogResult.OK };
                var btnCancel = new Button { Text = "Cancel", Left = 410, Width = 80, Top = 240, DialogResult = DialogResult.Cancel };
                f.Controls.AddRange(new Control[] { lblName, txtName, lblPrice, txtPrice, lblQty, numQty, btnOk, btnCancel });
                f.AcceptButton = btnOk;
                f.CancelButton = btnCancel;

                if (f.ShowDialog(this) == DialogResult.OK)
                {
                    if (string.IsNullOrWhiteSpace(txtName.Text)) return;
                    decimal price = ParsePrice(txtPrice.Text);
                    int qty = (int)numQty.Value;
                    decimal amount = price * qty;
                    int idx = guna2DataGridView2.Rows.Add();
                    var row = guna2DataGridView2.Rows[idx];
                    row.Cells[0].Value = txtName.Text.Trim();
                    row.Cells[1].Value = qty;
                    row.Cells[2].Value = price.ToString("N0");
                    row.Cells[3].Value = amount.ToString("N0");
                    row.Tag = qty;
                }
            }
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

        private decimal GetRoomPrice()
        {
            return IsVipRoom() ? VIP_ROOM_PRICE_PER_NIGHT : ROOM_PRICE_PER_NIGHT;
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
                else if (num >= 13 && num <= 20)
                    guna2ComboBox2.SelectedItem = "Phòng đôi";
                else
                    guna2ComboBox2.SelectedItem = "Phòng gia đình";
            }
            SelectedRoomType = guna2ComboBox2.SelectedItem as string;
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