using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        private Label _lblCheckout;
        private Guna2Button _btnThanhToan;
        private Button _btnAddService;
        private Button _btnRemoveService;
        private BookingInfo _existing;
        private ContextMenuStrip _menuService;

        private const decimal ROOM_PRICE_PER_NIGHT = 499000m;
        private const string ROOM_SERVICE_NAME = "Tiền phòng";

        public Room_Details(string maPhong, string trangThai)
        {
            InitializeComponent();

            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                labMaphong.Text = maPhong;
                return;
            }

            labMaphong.Text = maPhong;

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
                AutoSize = true,
                Visible = false,
                BorderRadius = 8,
                FillColor = System.Drawing.Color.FromArgb(76, 132, 255),
                ForeColor = System.Drawing.Color.White,
                Padding = new Padding(8, 4, 8, 4),
                Font = btnNhanphong.Font,
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
                EnsureRoomCharge(days); // đảm bảo luôn có dòng tiền phòng
                _btnThanhToan.Visible = true;
            }
            else
            {
                EnsureRoomCharge((int)nNgay.Value);
                UpdateCheckoutLabel(dtNgay.Value, dtNgay.Value.AddDays((int)nNgay.Value), (int)nNgay.Value);
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
            // nếu grid chưa có dòng tiền phòng, thêm vào; nếu có thì cập nhật
            bool found = false;
            foreach (DataGridViewRow r in guna2DataGridView2.Rows)
            {
                if (r.IsNewRow) continue;
                if (string.Equals(Convert.ToString(r.Cells[0].Value), ROOM_SERVICE_NAME, StringComparison.OrdinalIgnoreCase))
                {
                    found = true;
                    decimal amount = ROOM_PRICE_PER_NIGHT * days;
                    r.Cells[1].Value = days; // qty
                    r.Cells[2].Value = ROOM_PRICE_PER_NIGHT.ToString("N0");
                    r.Cells[3].Value = amount.ToString("N0");
                    break;
                }
            }
            if (!found)
            {
                decimal amount = ROOM_PRICE_PER_NIGHT * days;
                int idx = guna2DataGridView2.Rows.Add();
                var row = guna2DataGridView2.Rows[idx];
                row.Cells[0].Value = ROOM_SERVICE_NAME;
                row.Cells[1].Value = days;
                row.Cells[2].Value = ROOM_PRICE_PER_NIGHT.ToString("N0");
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
                string room = labMaphong.Text;
                DateTime start = _existing.Start;
                DateTime end = _existing.End;
                int days = Math.Max(1, (int)Math.Ceiling((end - start).TotalDays));

                decimal serviceTotal = 0;
                var serviceLines = new StringBuilder();
                foreach (var s in _existing.Services)
                {
                    serviceTotal += s.Amount;
                    serviceLines.AppendLine($" - {s.Name}: {s.Quantity} x {s.UnitPrice:N0} = {s.Amount:N0}");
                }

                StringBuilder content = new StringBuilder();
                content.AppendLine("HOA DON THANH TOAN");
                content.AppendLine($"Phong: {room}");
                content.AppendLine($"Khach hang: {_existing.Customer}");
                content.AppendLine($"Check-in: {start:dd/MM/yyyy HH:mm}");
                content.AppendLine($"Check-out: {end:dd/MM/yyyy HH:mm}");
                content.AppendLine($"So ngay o: {days}");
                content.AppendLine("-----------------------------------------");
                content.AppendLine("Dich vu:");
                content.Append(serviceLines.ToString());
                content.AppendLine($"Tong dich vu: {serviceTotal:N0}");
                content.AppendLine($"Ngay lap: {DateTime.Now:dd/MM/yyyy HH:mm}");
                content.AppendLine("-----------------------------------------");
                content.AppendLine("Cam on quy khach!");

                File.WriteAllText(sfd.FileName, content.ToString());
                MessageBox.Show("Đã lưu hóa đơn (dạng .txt).", "Thông báo");

                // sau khi thanh toán, trả phòng về trạng thái trống
                BookingManager.RemoveBooking(room);
                _existing = null;
                _btnThanhToan.Visible = false;
                txtName.Text = string.Empty;
                guna2DataGridView2.Rows.Clear();
                EnsureRoomCharge((int)nNgay.Value);

                try
                {
                    var listForm = Application.OpenForms.OfType<ListRoom>().FirstOrDefault();
                    listForm?.RefreshFromBookings();
                }
                catch { }
            }
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

            DateTime start = dtNgay.Value.Date + dtGio.Value.TimeOfDay;
            DateTime end = start.AddDays(this.SoNgay);
            var info = new BookingInfo { Customer = this.TenKhach, Start = start, End = end, Services = CollectServicesFromGrid() };
            // đảm bảo tiền phòng luôn có trong service
            EnsureRoomCharge(this.SoNgay);
            info.Services = CollectServicesFromGrid();
            BookingManager.AddBooking(labMaphong.Text, info);
            _existing = info;
            _btnThanhToan.Visible = true;

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
            }
        }

        private void AddService()
        {
            using (var f = new Form())
            {
                f.Text = "Thêm dịch vụ";
                f.FormBorderStyle = FormBorderStyle.FixedDialog;
                f.StartPosition = FormStartPosition.CenterParent;
                f.ClientSize = new System.Drawing.Size(300, 180);

                var lblName = new Label { Left = 10, Top = 10, Text = "Tên dịch vụ", AutoSize = true };
                var txtName = new TextBox { Left = 10, Top = 30, Width = 270 };
                var lblPrice = new Label { Left = 10, Top = 60, Text = "Đơn giá", AutoSize = true };
                var txtPrice = new TextBox { Left = 10, Top = 80, Width = 270 };
                var lblQty = new Label { Left = 10, Top = 110, Text = "Số lượng", AutoSize = true };
                var numQty = new NumericUpDown { Left = 100, Top = 108, Width = 80, Minimum = 1, Maximum = 999, Value = 1 };
                var btnOk = new Button { Text = "OK", Left = 130, Width = 70, Top = 140, DialogResult = DialogResult.OK };
                var btnCancel = new Button { Text = "Cancel", Left = 210, Width = 70, Top = 140, DialogResult = DialogResult.Cancel };
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
                decimal price = 0; decimal amount = 0;
                decimal.TryParse(Convert.ToString(r.Cells[2].Value), NumberStyles.Any, CultureInfo.CurrentCulture, out price);
                decimal.TryParse(Convert.ToString(r.Cells[3].Value), NumberStyles.Any, CultureInfo.CurrentCulture, out amount);
                int qty = 1;
                int.TryParse(Convert.ToString(r.Cells[1].Value), out qty);
                if (string.IsNullOrWhiteSpace(ten)) continue;
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
    }
}