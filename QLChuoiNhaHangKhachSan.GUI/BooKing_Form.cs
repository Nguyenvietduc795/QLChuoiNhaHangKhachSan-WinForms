using System;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Windows.Forms;
using System.Drawing;

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
            SeedSampleRows();
            RefreshAutocompleteSource();
        }

        // Bạn có thể sửa danh sách mẫu này để đổi dữ liệu mặc định
        private readonly (string Customer, string Room, string From, string To, string CCCD, string SDT, string Gender, string Nationality)[] _sampleData = new[]
        {
            ("Nguyễn Tiến Linh", "P001", "21/12/2025 11:04", "21/12/2025 12:00", "012345678901", "0901234567", "Nam", "Việt Nam"),
            ("Nguyen Văn Tùng",   "P003", "21/12/2025 13:00", "23/12/2025 14:00", "987654321000", "0912345678", "Nam", "Việt Nam"),
            ("Tran Thi Hạnh ",     "P012", "22/12/2025 09:00", "24/12/2025 10:00", "112233445566", "0923456789", "Nữ", "Việt Nam"),
            ("Lê Chí Hải",       "P001", "22/12/2025 10:30", "23/12/2025 11:30", "223344556677", "0934567890", "Nam", "Việt Nam"),
            ("Phạm Đình Thư",     "P001", "23/12/2025 20:00", "25/12/2025 09:00", "334455667788", "0945678901", "Nam", "Việt Nam"),
            ("Hoàng Tiến",    "P003", "23/12/2025 14:00", "27/12/2025 15:00", "445566778899", "0956789012", "Nam", "Việt Nam"),
            ("Nguyễn Anh",       "P011", "24/12/2025 16:00", "25/12/2025 17:00", "556677889900", "0967890123", "Nam", "Việt Nam"),
            ("Bui Giang",      "P007", "24/12/2025 18:00", "24/12/2025 19:00", "667788990011", "0978901234", "Nam", "Việt Nam"),
            ("Đặng Hùng",     "P015", "25/12/2025 07:30", "25/12/2025 08:30", "778899001122", "0989012345", "Nam", "Việt Nam"),
            ("Đỗ Thị Trinh",       "P019", "25/12/2025 09:30", "25/12/2025 10:30", "889900112233", "0990123456", "Nữ", "Việt Nam"),
        };

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

        private void SeedSampleRows()
        {
            // Nếu đã có dữ liệu (bất kỳ dòng nào không phải NewRow) thì không seed nữa
            bool hasRealRow = guna2DataGridView1.Rows.Cast<DataGridViewRow>().Any(r => !r.IsNewRow);
            if (hasRealRow) return;

            foreach (var item in _sampleData)
            {
                string chitiet = $"{item.Room} ({item.From} - {item.To})";
                AddBookingRowInternal(item.Customer, item.CCCD, item.SDT, item.Room, chitiet, item.Gender, item.Nationality);
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
                f.Size = new Size(800, 500);
                f.Font = new Font("Microsoft YaHei UI", 11F, FontStyle.Regular);

                var txt = new TextBox
                {
                    Multiline = true,
                    ReadOnly = true,
                    Dock = DockStyle.Fill,
                    ScrollBars = ScrollBars.Vertical,
                    Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular)
                };

                string details =
                    "Nhân viên: " + (row.Cells[3].Value ?? "") + Environment.NewLine +
                    "Số phiếu: " + (row.Cells[0].Value ?? "") + Environment.NewLine +
                    "Khách Hàng: " + (row.Cells[1].Value ?? "") + Environment.NewLine +
                    "Ngày lập: " + (row.Cells[2].Value ?? "") + Environment.NewLine +
                    "CCCD: " + cccd + Environment.NewLine +
                    "SĐT: " + sdt + Environment.NewLine +
                    "Giới tính: " + gender + Environment.NewLine +
                    "Quốc tịch: " + nat + Environment.NewLine +
                    "Phòng: " + (row.Cells[4].Value ?? "");


                txt.Text = details;
                f.Controls.Add(txt);
                f.ShowDialog(this);
            }
        }

        private void Guna2Button1_Click(object sender, EventArgs e)
        {
            using (var dlg = new BookingRoom_Details())
            {
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

                    AddBookingRowInternal(khach, cccd, sdt, phong, thoigian, gender, nat);

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

        private void AddBookingRowInternal(string khach, string cccd, string sdt, string phong, string thoigian, string gender = "", string nat = "")
        {
            int rowIndex = this.guna2DataGridView1.Rows.Add();
            var row = this.guna2DataGridView1.Rows[rowIndex];
            string id = "PT" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
            string ngayLap = DateTime.Now.ToShortDateString();
            row.Cells[0].Value = id;
            row.Cells[1].Value = khach;
            row.Cells[2].Value = ngayLap;
            row.Cells[3].Value = "Trường Phi";
            row.Cells[4].Value = phong + " (" + thoigian + ")";
            row.Cells[5].Value = "     X";

            row.Tag = new BookingRowInfo
            {
                Id = id,
                Customer = khach,
                Date = ngayLap,
                Nationality = nat,
                Detail = phong + " (" + thoigian + ")",
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
