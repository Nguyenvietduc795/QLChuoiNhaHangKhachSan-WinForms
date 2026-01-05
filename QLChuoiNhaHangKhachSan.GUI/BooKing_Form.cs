using System;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;

namespace QLChuoiNhaHangKhachSan.GUI
{
    public partial class BooKing_Form : Form
    {
        public BooKing_Form()
        {
            InitializeComponent();
            this.guna2Button1.Click += Guna2Button1_Click;
            this.guna2DataGridView1.CellContentClick += Guna2DataGridView1_CellContentClick;
            this.guna2TextBox1.TextChanged += Guna2TextBox1_TextChanged;
            ConfigureSearchAutocomplete();
            RefreshAutocompleteSource();
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

        private void Guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            int detailCol = 4; // adjust index if different
            int deleteCol = 5; // adjust index if different

            if (e.ColumnIndex == deleteCol)
            {
                var dr = MessageBox.Show("Bạn có chắc chắn muốn xóa phiếu này?", "Xác nhận", MessageBoxButtons.YesNo);
                if (dr == DialogResult.Yes)
                {
                    guna2DataGridView1.Rows.RemoveAt(e.RowIndex);
                }
            }
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
                // xuống dòng giữa các phòng/khoảng thời gian
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
                var availableRooms = LoadAvailableRoomsFromDb();
                dlg.SetAvailableRooms(availableRooms);

                var unavailable = BookingManager.GetAllBookings().Select(kvp => kvp.Key);
                dlg.SetUnavailableRooms(unavailable);

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
                    string detailText = dlg.ResultRoomDetails;
 
                    AddBookingRowInternal(khach, cccd, sdt, phong, thoigian, startDate, gender, nat, detailText);

                    try
                    {
                        var listForm = Application.OpenForms.OfType<ListRoom>().FirstOrDefault();
                        if (listForm != null && !string.IsNullOrWhiteSpace(phong))
                        {
                            var codes = phong.Split(new[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                            listForm.MarkRoomsAsBooked(codes, khach);
                        }
                    }
                    catch { }
                }
            }
        }

        private List<(string RoomCode, string RoomType)> LoadAvailableRoomsFromDb()
        {
            // Database connection removed; return an empty list or populate from another source if available.
            return new List<(string, string)>();
        }

        private void AddBookingRowInternal(string khach, string cccd, string sdt, string phong, string thoigian, DateTime? startDate = null, string gender = "", string nat = "", string detailOverride = null)
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
 }
 }
