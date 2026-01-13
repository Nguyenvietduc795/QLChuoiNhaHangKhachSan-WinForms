using QLChuoiNhaHangKhachSan.GUI.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace QLChuoiNhaHangKhachSan.GUI
{
    public partial class BookingRoom_Details : Form
    {
        public bool IsDatTruoc = false;
        private class CountryPhoneCode
        {
            public string Name { get; set; }
            public string DialCode { get; set; }
            public string Iso { get; set; }
            public string DisplayName => string.Format("{0} ({1})", Name, DialCode);
        }


        private static readonly List<CountryPhoneCode> _countryPhoneCodes = new List<CountryPhoneCode>
    {
      new CountryPhoneCode { Name = "Afghanistan", DialCode = "+93", Iso = "AF" },
      new CountryPhoneCode { Name = "Albania", DialCode = "+355", Iso = "AL" },
      new CountryPhoneCode { Name = "Algeria", DialCode = "+213", Iso = "DZ" },
      new CountryPhoneCode { Name = "Andorra", DialCode = "+376", Iso = "AD" },
      new CountryPhoneCode { Name = "Angola", DialCode = "+244", Iso = "AO" },
      new CountryPhoneCode { Name = "Antigua and Barbuda", DialCode = "+1268", Iso = "AG" },
      new CountryPhoneCode { Name = "Argentina", DialCode = "+54", Iso = "AR" },
      new CountryPhoneCode { Name = "Armenia", DialCode = "+374", Iso = "AM" },
      new CountryPhoneCode { Name = "Australia", DialCode = "+61", Iso = "AU" },
      new CountryPhoneCode { Name = "Austria", DialCode = "+43", Iso = "AT" },
      new CountryPhoneCode { Name = "Azerbaijan", DialCode = "+994", Iso = "AZ" },
      new CountryPhoneCode { Name = "Bahamas", DialCode = "+1242", Iso = "BS" },
      new CountryPhoneCode { Name = "Bahrain", DialCode = "+973", Iso = "BH" },
      new CountryPhoneCode { Name = "Bangladesh", DialCode = "+880", Iso = "BD" },
      new CountryPhoneCode { Name = "Barbados", DialCode = "+1246", Iso = "BB" },
      new CountryPhoneCode { Name = "Belarus", DialCode = "+375", Iso = "BY" },
      new CountryPhoneCode { Name = "Belgium", DialCode = "+32", Iso = "BE" },
      new CountryPhoneCode { Name = "Belize", DialCode = "+501", Iso = "BZ" },
      new CountryPhoneCode { Name = "Benin", DialCode = "+229", Iso = "BJ" },
      new CountryPhoneCode { Name = "Bhutan", DialCode = "+975", Iso = "BT" },
      new CountryPhoneCode { Name = "Bolivia", DialCode = "+591", Iso = "BO" },
      new CountryPhoneCode { Name = "Bosnia and Herzegovina", DialCode = "+387", Iso = "BA" },
      new CountryPhoneCode { Name = "Botswana", DialCode = "+267", Iso = "BW" },
      new CountryPhoneCode { Name = "Brazil", DialCode = "+55", Iso = "BR" },
      new CountryPhoneCode { Name = "Brunei", DialCode = "+673", Iso = "BN" },
      new CountryPhoneCode { Name = "Bulgaria", DialCode = "+359", Iso = "BG" },
      new CountryPhoneCode { Name = "Burkina Faso", DialCode = "+226", Iso = "BF" },
      new CountryPhoneCode { Name = "Burundi", DialCode = "+257", Iso = "BI" },
      new CountryPhoneCode { Name = "Cabo Verde", DialCode = "+238", Iso = "CV" },
      new CountryPhoneCode { Name = "Cambodia", DialCode = "+855", Iso = "KH" },
      new CountryPhoneCode { Name = "Cameroon", DialCode = "+237", Iso = "CM" },
      new CountryPhoneCode { Name = "Canada", DialCode = "+1", Iso = "CA" },
      new CountryPhoneCode { Name = "Central African Republic", DialCode = "+236", Iso = "CF" },
      new CountryPhoneCode { Name = "Chad", DialCode = "+235", Iso = "TD" },
      new CountryPhoneCode { Name = "Chile", DialCode = "+56", Iso = "CL" },
      new CountryPhoneCode { Name = "China", DialCode = "+86", Iso = "CN" },
      new CountryPhoneCode { Name = "Colombia", DialCode = "+57", Iso = "CO" },
      new CountryPhoneCode { Name = "Comoros", DialCode = "+269", Iso = "KM" },
      new CountryPhoneCode { Name = "Congo", DialCode = "+242", Iso = "CG" },
      new CountryPhoneCode { Name = "Costa Rica", DialCode = "+506", Iso = "CR" },
      new CountryPhoneCode { Name = "Croatia", DialCode = "+385", Iso = "HR" },
      new CountryPhoneCode { Name = "Cuba", DialCode = "+53", Iso = "CU" },
      new CountryPhoneCode { Name = "Cyprus", DialCode = "+357", Iso = "CY" },
      new CountryPhoneCode { Name = "Czech Republic", DialCode = "+420", Iso = "CZ" },
      new CountryPhoneCode { Name = "Denmark", DialCode = "+45", Iso = "DK" },
      new CountryPhoneCode { Name = "Djibouti", DialCode = "+253", Iso = "DJ" },
      new CountryPhoneCode { Name = "Dominica", DialCode = "+1767", Iso = "DM" },
      new CountryPhoneCode { Name = "Dominican Republic", DialCode = "+1", Iso = "DO" },
      new CountryPhoneCode { Name = "Ecuador", DialCode = "+593", Iso = "EC" },
      new CountryPhoneCode { Name = "Egypt", DialCode = "+20", Iso = "EG" },
      new CountryPhoneCode { Name = "El Salvador", DialCode = "+503", Iso = "SV" },
      new CountryPhoneCode { Name = "Equatorial Guinea", DialCode = "+240", Iso = "GQ" },
      new CountryPhoneCode { Name = "Eritrea", DialCode = "+291", Iso = "ER" },
      new CountryPhoneCode { Name = "Estonia", DialCode = "+372", Iso = "EE" },
      new CountryPhoneCode { Name = "Eswatini", DialCode = "+268", Iso = "SZ" },
      new CountryPhoneCode { Name = "Ethiopia", DialCode = "+251", Iso = "ET" },
      new CountryPhoneCode { Name = "Fiji", DialCode = "+679", Iso = "FJ" },
      new CountryPhoneCode { Name = "Finland", DialCode = "+358", Iso = "FI" },
      new CountryPhoneCode { Name = "France", DialCode = "+33", Iso = "FR" },
      new CountryPhoneCode { Name = "Gabon", DialCode = "+241", Iso = "GA" },
      new CountryPhoneCode { Name = "Gambia", DialCode = "+220", Iso = "GM" },
      new CountryPhoneCode { Name = "Georgia", DialCode = "+995", Iso = "GE" },
      new CountryPhoneCode { Name = "Germany", DialCode = "+49", Iso = "DE" },
      new CountryPhoneCode { Name = "Ghana", DialCode = "+233", Iso = "GH" },
      new CountryPhoneCode { Name = "Greece", DialCode = "+30", Iso = "GR" },
      new CountryPhoneCode { Name = "Grenada", DialCode = "+1473", Iso = "GD" },
      new CountryPhoneCode { Name = "Guatemala", DialCode = "+502", Iso = "GT" },
      new CountryPhoneCode { Name = "Guinea", DialCode = "+224", Iso = "GN" },
      new CountryPhoneCode { Name = "Guinea-Bissau", DialCode = "+245", Iso = "GW" },
      new CountryPhoneCode { Name = "Guyana", DialCode = "+592", Iso = "GY" },
      new CountryPhoneCode { Name = "Haiti", DialCode = "+509", Iso = "HT" },
      new CountryPhoneCode { Name = "Honduras", DialCode = "+504", Iso = "HN" },
      new CountryPhoneCode { Name = "Hungary", DialCode = "+36", Iso = "HU" },
      new CountryPhoneCode { Name = "Iceland", DialCode = "+354", Iso = "IS" },
      new CountryPhoneCode { Name = "India", DialCode = "+91", Iso = "IN" }
,      new CountryPhoneCode { Name = "Indonesia", DialCode = "+62", Iso = "ID" },
      new CountryPhoneCode { Name = "Iran", DialCode = "+98", Iso = "IR" },
      new CountryPhoneCode { Name = "Iraq", DialCode = "+964", Iso = "IQ" },
      new CountryPhoneCode { Name = "Ireland", DialCode = "+353", Iso = "IE" },
      new CountryPhoneCode { Name = "Israel", DialCode = "+972", Iso = "IL" },
      new CountryPhoneCode { Name = "Italy", DialCode = "+39", Iso = "IT" },
      new CountryPhoneCode { Name = "Jamaica", DialCode = "+1876", Iso = "JM" },
      new CountryPhoneCode { Name = "Japan", DialCode = "+81", Iso = "JP" },
      new CountryPhoneCode { Name = "Jordan", DialCode = "+962", Iso = "JO" },
      new CountryPhoneCode { Name = "Kazakhstan", DialCode = "+7", Iso = "KZ" },
      new CountryPhoneCode { Name = "Kenya", DialCode = "+254", Iso = "KE" },
      new CountryPhoneCode { Name = "Kiribati", DialCode = "+686", Iso = "KI" },
      new CountryPhoneCode { Name = "Kuwait", DialCode = "+965", Iso = "KW" },
      new CountryPhoneCode { Name = "Kyrgyzstan", DialCode = "+996", Iso = "KG" },
      new CountryPhoneCode { Name = "Laos", DialCode = "+856", Iso = "LA" },
      new CountryPhoneCode { Name = "Latvia", DialCode = "+371", Iso = "LV" },
      new CountryPhoneCode { Name = "Lebanon", DialCode = "+961", Iso = "LB" },
      new CountryPhoneCode { Name = "Lesotho", DialCode = "+266", Iso = "LS" },
      new CountryPhoneCode { Name = "Liberia", DialCode = "+231", Iso = "LR" },
      new CountryPhoneCode { Name = "Libya", DialCode = "+218", Iso = "LY" },
      new CountryPhoneCode { Name = "Liechtenstein", DialCode = "+423", Iso = "LI" },
      new CountryPhoneCode { Name = "Lithuania", DialCode = "+370", Iso = "LT" },
      new CountryPhoneCode { Name = "Luxembourg", DialCode = "+352", Iso = "LU" },
      new CountryPhoneCode { Name = "Madagascar", DialCode = "+261", Iso = "MG" },
      new CountryPhoneCode { Name = "Malawi", DialCode = "+265", Iso = "MW" },
      new CountryPhoneCode { Name = "Malaysia", DialCode = "+60", Iso = "MY" },
      new CountryPhoneCode { Name = "Maldives", DialCode = "+960", Iso = "MV" },
      new CountryPhoneCode { Name = "Mali", DialCode = "+223", Iso = "ML" },
      new CountryPhoneCode { Name = "Malta", DialCode = "+356", Iso = "MT" },
      new CountryPhoneCode { Name = "Marshall Islands", DialCode = "+692", Iso = "MH" },
      new CountryPhoneCode { Name = "Mauritania", DialCode = "+222", Iso = "MR" },
      new CountryPhoneCode { Name = "Mauritius", DialCode = "+230", Iso = "MU" },
      new CountryPhoneCode { Name = "Mexico", DialCode = "+52", Iso = "MX" },
      new CountryPhoneCode { Name = "Micronesia", DialCode = "+691", Iso = "FM" },
      new CountryPhoneCode { Name = "Moldova", DialCode = "+373", Iso = "MD" },
      new CountryPhoneCode { Name = "Monaco", DialCode = "+377", Iso = "MC" },
      new CountryPhoneCode { Name = "Mongolia", DialCode = "+976", Iso = "MN" },
      new CountryPhoneCode { Name = "Montenegro", DialCode = "+382", Iso = "ME" },
      new CountryPhoneCode { Name = "Morocco", DialCode = "+212", Iso = "MA" },
      new CountryPhoneCode { Name = "Mozambique", DialCode = "+258", Iso = "MZ" },
      new CountryPhoneCode { Name = "Myanmar", DialCode = "+95", Iso = "MM" },
      new CountryPhoneCode { Name = "Namibia", DialCode = "+264", Iso = "NA" },
      new CountryPhoneCode { Name = "Nauru", DialCode = "+674", Iso = "NR" },
      new CountryPhoneCode { Name = "Nepal", DialCode = "+977", Iso = "NP" },
      new CountryPhoneCode { Name = "Netherlands", DialCode = "+31", Iso = "NL" },
      new CountryPhoneCode { Name = "New Zealand", DialCode = "+64", Iso = "NZ" },
      new CountryPhoneCode { Name = "Nicaragua", DialCode = "+505", Iso = "NI" },
      new CountryPhoneCode { Name = "Niger", DialCode = "+227", Iso = "NE" },
      new CountryPhoneCode { Name = "Nigeria", DialCode = "+234", Iso = "NG" },
      new CountryPhoneCode { Name = "North Korea", DialCode = "+850", Iso = "KP" },
      new CountryPhoneCode { Name = "North Macedonia", DialCode = "+389", Iso = "MK" },
      new CountryPhoneCode { Name = "Norway", DialCode = "+47", Iso = "NO" },
      new CountryPhoneCode { Name = "Oman", DialCode = "+968", Iso = "OM" },
      new CountryPhoneCode { Name = "Pakistan", DialCode = "+92", Iso = "PK" },
      new CountryPhoneCode { Name = "Palau", DialCode = "+680", Iso = "PW" },
      new CountryPhoneCode { Name = "Panama", DialCode = "+507", Iso = "PA" },
      new CountryPhoneCode { Name = "Papua New Guinea", DialCode = "+675", Iso = "PG" },
      new CountryPhoneCode { Name = "Paraguay", DialCode = "+595", Iso = "PY" },
      new CountryPhoneCode { Name = "Peru", DialCode = "+51", Iso = "PE" },
      new CountryPhoneCode { Name = "Philippines", DialCode = "+63", Iso = "PH" },
      new CountryPhoneCode { Name = "Poland", DialCode = "+48", Iso = "PL" },
      new CountryPhoneCode { Name = "Portugal", DialCode = "+351", Iso = "PT" },
      new CountryPhoneCode { Name = "Qatar", DialCode = "+974", Iso = "QA" },
      new CountryPhoneCode { Name = "Romania", DialCode = "+40", Iso = "RO" },
      new CountryPhoneCode { Name = "Russia", DialCode = "+7", Iso = "RU" },
      new CountryPhoneCode { Name = "Rwanda", DialCode = "+250", Iso = "RW" },
      new CountryPhoneCode { Name = "Saint Kitts and Nevis", DialCode = "+1869", Iso = "KN" },
      new CountryPhoneCode { Name = "Saint Lucia", DialCode = "+1758", Iso = "LC" },
      new CountryPhoneCode { Name = "Saint Vincent and the Grenadines", DialCode = "+1784", Iso = "VC" },
      new CountryPhoneCode { Name = "Samoa", DialCode = "+685", Iso = "WS" },
      new CountryPhoneCode { Name = "San Marino", DialCode = "+378", Iso = "SM" },
      new CountryPhoneCode { Name = "Sao Tome and Principe", DialCode = "+239", Iso = "ST" },
      new CountryPhoneCode { Name = "Saudi Arabia", DialCode = "+966", Iso = "SA" },
      new CountryPhoneCode { Name = "Senegal", DialCode = "+221", Iso = "SN" },
      new CountryPhoneCode { Name = "Serbia", DialCode = "+381", Iso = "RS" },
      new CountryPhoneCode { Name = "Seychelles", DialCode = "+248", Iso = "SC" },
      new CountryPhoneCode { Name = "Sierra Leone", DialCode = "+232", Iso = "SL" },
      new CountryPhoneCode { Name = "Singapore", DialCode = "+65", Iso = "SG" },
      new CountryPhoneCode { Name = "Slovakia", DialCode = "+421", Iso = "SK" },
      new CountryPhoneCode { Name = "Slovenia", DialCode = "+386", Iso = "SI" },
      new CountryPhoneCode { Name = "Solomon Islands", DialCode = "+677", Iso = "SB" },
      new CountryPhoneCode { Name = "Somalia", DialCode = "+252", Iso = "SO" },
      new CountryPhoneCode { Name = "South Africa", DialCode = "+27", Iso = "ZA" },
      new CountryPhoneCode { Name = "South Korea", DialCode = "+82", Iso = "KR" },
      new CountryPhoneCode { Name = "South Sudan", DialCode = "+211", Iso = "SS" },
      new CountryPhoneCode { Name = "Spain", DialCode = "+34", Iso = "ES" },
      new CountryPhoneCode { Name = "Sri Lanka", DialCode = "+94", Iso = "LK" },
      new CountryPhoneCode { Name = "Sudan", DialCode = "+249", Iso = "SD" },
      new CountryPhoneCode { Name = "Suriname", DialCode = "+597", Iso = "SR" },
      new CountryPhoneCode { Name = "Sweden", DialCode = "+46", Iso = "SE" },
      new CountryPhoneCode { Name = "Switzerland", DialCode = "+41", Iso = "CH" },
      new CountryPhoneCode { Name = "Syria", DialCode = "+963", Iso = "SY" },
      new CountryPhoneCode { Name = "Taiwan", DialCode = "+886", Iso = "TW" },
      new CountryPhoneCode { Name = "Tajikistan", DialCode = "+992", Iso = "TJ" },
      new CountryPhoneCode { Name = "Tanzania", DialCode = "+255", Iso = "TZ" },
      new CountryPhoneCode { Name = "Thailand", DialCode = "+66", Iso = "TH" },
      new CountryPhoneCode { Name = "Timor-Leste", DialCode = "+670", Iso = "TL" },
      new CountryPhoneCode { Name = "Togo", DialCode = "+228", Iso = "TG" },
      new CountryPhoneCode { Name = "Tonga", DialCode = "+676", Iso = "TO" },
      new CountryPhoneCode { Name = "Trinidad and Tobago", DialCode = "+1868", Iso = "TT" },
      new CountryPhoneCode { Name = "Tunisia", DialCode = "+216", Iso = "TN" },
      new CountryPhoneCode { Name = "Turkey", DialCode = "+90", Iso = "TR" },
      new CountryPhoneCode { Name = "Turkmenistan", DialCode = "+993", Iso = "TM" },
      new CountryPhoneCode { Name = "Tuvalu", DialCode = "+688", Iso = "TV" },
      new CountryPhoneCode { Name = "Uganda", DialCode = "+256", Iso = "UG" },
      new CountryPhoneCode { Name = "Ukraine", DialCode = "+380", Iso = "UA" },
      new CountryPhoneCode { Name = "United Arab Emirates", DialCode = "+971", Iso = "AE" },
      new CountryPhoneCode { Name = "United Kingdom", DialCode = "+44", Iso = "GB" },
      new CountryPhoneCode { Name = "United States", DialCode = "+1", Iso = "US" },
      new CountryPhoneCode { Name = "Uruguay", DialCode = "+598", Iso = "UY" },
      new CountryPhoneCode { Name = "Uzbekistan", DialCode = "+998", Iso = "UZ" },
      new CountryPhoneCode { Name = "Vanuatu", DialCode = "+678", Iso = "VU" },
      new CountryPhoneCode { Name = "Vatican City", DialCode = "+379", Iso = "VA" },
      new CountryPhoneCode { Name = "Venezuela", DialCode = "+58", Iso = "VE" },
      new CountryPhoneCode { Name = "Vietnam", DialCode = "+84", Iso = "VN" },
      new CountryPhoneCode { Name = "Yemen", DialCode = "+967", Iso = "YE" },
      new CountryPhoneCode { Name = "Zambia", DialCode = "+260", Iso = "ZM" },
      new CountryPhoneCode { Name = "Zimbabwe", DialCode = "+263", Iso = "ZW" }
    };

        // Public properties for callers
        public string ResultCustomerName { get; private set; }
        public string ResultCCCD { get; private set; }
        public string ResultSDT { get; private set; }
        public string ResultRooms { get; private set; }
        public string ResultDateRange { get; private set; }
        public string ResultGender { get; private set; }
        public string ResultNationality { get; private set; }
        public string ResultEmail { get; private set; }
        public string ResultAddress { get; private set; }
        public DateTime ResultStartDate { get; private set; }
        public DateTime ResultEndDate { get; private set; }
        public string ResultRoomDetails { get; private set; }

        // Khai báo bảng tạm toàn cục để quản lý dữ liệu
        DataTable dtTrong = new DataTable();
        DataTable dtChon = new DataTable();

        private List<string> _preselectedRooms;
        private HashSet<string> _unavailableRooms = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private ContextMenuStrip _menuTrong;
        private ContextMenuStrip _menuChon;
        private NumericUpDown _qtyEditor;
        private string _editingRoomCode;

        private DateTime? _lockedViewTime;
        private bool _lockDateTimePickers;

        // Nguồn phòng cung cấp từ bên ngoài (DB) và lookup loại phòng
        private readonly List<(string Code, string Type)> _roomsFromSource = new List<(string Code, string Type)>();
        private readonly Dictionary<string, string> _roomTypeLookup = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        private const string ROOM_SERVICE_NAME = "Tiền phòng";

        public BookingRoom_Details()
        {
            InitializeComponent();
            // Bật DoubleBuffered để giao diện mượt hơn
            this.DoubleBuffered = true;
            ApplyModernTheme();
            this.Load += BookingRoom_Details_Load;
            PopulateCountryCombo();
            if (guna2ComboBox1 != null)
                guna2ComboBox1.SelectedIndexChanged += Guna2ComboBox1_SelectedIndexChanged;

            

            // Context menu cho list phòng trống và đã chọn (thay cho nút bấm Thêm/Xóa)
            _menuTrong = new ContextMenuStrip();
            var mAdd = new ToolStripMenuItem("Thêm vào danh sách chọn");
            mAdd.Click += (s, e) => MoveSelectedTrongToChon();
            _menuTrong.Items.Add(mAdd);

            _menuChon = new ContextMenuStrip();
            var mRemove = new ToolStripMenuItem("Xóa khỏi danh sách chọn");
            mRemove.Click += (s, e) => RemoveSelectedChon();
            _menuChon.Items.Add(mRemove);

            lvPhongTrong.ContextMenuStrip = _menuTrong;
            lvPhongChon.ContextMenuStrip = _menuChon;

            // cấu hình cột nút (Thêm/Xóa) có tiêu đề, canh giữa
            if (lvPhongTrong.Columns.Count >= 3)
            {
                lvPhongTrong.Columns[2].Text = "Thêm";
                lvPhongTrong.Columns[2].TextAlign = HorizontalAlignment.Center;
                lvPhongTrong.Columns[2].Width = 60;
            }
            if (lvPhongChon.Columns.Count >= 5)
            {
                lvPhongChon.Columns[4].Text = "Xóa";
                lvPhongChon.Columns[4].TextAlign = HorizontalAlignment.Center;
                lvPhongChon.Columns[4].Width = 60;
            }

            // Inline editor cho cột Số người
            _qtyEditor = new NumericUpDown
            {
                Minimum = 1,
                Maximum = 100,
                Visible = false,
                BorderStyle = BorderStyle.FixedSingle
            };
            _qtyEditor.Leave += QtyEditor_Leave;
            _qtyEditor.KeyDown += QtyEditor_KeyDown;
            lvPhongChon.Controls.Add(_qtyEditor);
            lvPhongChon.MouseDown += LvPhongChon_MouseDown;

            // clock-style time picker for giờ vào/ra
            dtpGioBatDau.MouseDown += DtpGioBatDau_MouseDown;
            dtpGioBatDau.Click += DtpGioBatDau_Click;
            dtpGioKetThuc.MouseDown += DtpGioKetThuc_MouseDown;
            dtpGioKetThuc.Click += DtpGioKetThuc_Click;

            // Khi thay đổi ngày bắt đầu/kết thúc -> cập nhật lại danh sách phòng trống
            dtpNgayBatDau.ValueChanged += DateRange_Changed;
            dtpNgayKetThuc.ValueChanged += DateRange_Changed;
            dtpGioBatDau.ValueChanged += DateRange_Changed;
            dtpGioKetThuc.ValueChanged += DateRange_Changed;

            // Owner draw icon columns
            lvPhongTrong.OwnerDraw = true;
            lvPhongTrong.DrawColumnHeader += Lv_DrawColumnHeader;
            lvPhongTrong.DrawItem += Lv_DrawItem;
            lvPhongTrong.DrawSubItem += LvPhongTrong_DrawSubItem;
            lvPhongTrong.MouseDown += LvPhongTrong_MouseDown;

            lvPhongChon.OwnerDraw = true;
            lvPhongChon.DrawColumnHeader += Lv_DrawColumnHeader;
            lvPhongChon.DrawItem += Lv_DrawItem;
            lvPhongChon.DrawSubItem += LvPhongChon_DrawSubItem;
            lvPhongChon.MouseDown += LvPhongChon_MouseDown_Remove;

            // dùng cùng ImageList cho icon vẽ tay
            if (lvPhongChon.SmallImageList == null)
                lvPhongChon.SmallImageList = imageList1;
            if (lvPhongTrong.SmallImageList == null)
                lvPhongTrong.SmallImageList = imageList1;
        }

        /// <summary>
        /// Khi thay đổi ngày/giờ bắt đầu hoặc kết thúc -> lọc lại danh sách phòng trống
        /// </summary>
        private void DateRange_Changed(object sender, EventArgs e)
        {
            RefreshAvailableRoomsList();
        }

        /// <summary>
        /// Lọc lại danh sách phòng trống dựa trên khoảng thời gian được chọn
        /// </summary>
        private void RefreshAvailableRoomsList()
        {
            // Kiểm tra nếu dtTrong chưa được khởi tạo
            if (dtTrong == null || dtTrong.Columns.Count == 0)
                return;

            DateTime startDate = dtpNgayBatDau.Value.Date + dtpGioBatDau.Value.TimeOfDay;
            DateTime endDate = dtpNgayKetThuc.Value.Date + dtpGioKetThuc.Value.TimeOfDay;

            // Nếu ngày kết thúc <= ngày bắt đầu, hiển thị tất cả phòng (không lọc)
            if (endDate <= startDate)
            {
                dtTrong.Clear();
                foreach (var room in _roomsFromSource)
                {
                    bool alreadyChosen = dtChon != null && dtChon.Columns.Count > 0 && dtChon.Rows.Cast<DataRow>()
                      .Any(r => string.Equals(r["maphong"].ToString(), room.Code, StringComparison.OrdinalIgnoreCase));
                    if (alreadyChosen) continue;
                    dtTrong.Rows.Add(room.Code, string.IsNullOrWhiteSpace(room.Type) ? "" : room.Type);
                }
                PopulateListViewsFromDataTables();
                return;
            }

            // Lấy danh sách phòng đã đặt trong khoảng thời gian này từ database
            var bookedRooms = GetBookedRoomsInRange(startDate, endDate);

            // Debug: hiển thị số phòng bị booked
            // MessageBox.Show($"Từ {startDate:dd/MM/yyyy HH:mm} đến {endDate:dd/MM/yyyy HH:mm}\nPhòng đã đặt: {string.Join(", ", bookedRooms)}\nTổng: {bookedRooms.Count}");

            // Cập nhật danh sách phòng trống
            dtTrong.Clear();
            foreach (var room in _roomsFromSource)
            {
                // Bỏ qua phòng đã được chọn
                bool alreadyChosen = dtChon != null && dtChon.Columns.Count > 0 && dtChon.Rows.Cast<DataRow>()
          .Any(r => string.Equals(r["maphong"].ToString(), room.Code, StringComparison.OrdinalIgnoreCase));
                if (alreadyChosen) continue;

                // Bỏ qua phòng đã bị đặt trong khoảng thời gian này
                if (bookedRooms.Contains(room.Code)) continue;

                dtTrong.Rows.Add(room.Code, string.IsNullOrWhiteSpace(room.Type) ? "" : room.Type);
            }

            PopulateListViewsFromDataTables();
        }

        /// <summary>
        /// Lấy danh sách mã phòng đã được đặt trong khoảng thời gian từ database
        /// Sử dụng logic: CheckIn < EndDate AND CheckOut > StartDate (overlap)
        /// </summary>
        private HashSet<string> GetBookedRoomsInRange(DateTime start, DateTime end)
        {
            var result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var connStr = ConfigurationManager.ConnectionStrings["ConnStr"]?.ConnectionString;
            if (string.IsNullOrWhiteSpace(connStr)) return result;

            try
            {
                using (var conn = new SqlConnection(connStr))
                using (var cmd = new SqlCommand(@"
                    SELECT DISTINCT d.RoomId
                    FROM dbo.BookingDetails d
                    WHERE d.CheckIn < @EndDate AND d.CheckOut > @StartDate", conn))
                {
                    cmd.Parameters.AddWithValue("@StartDate", start);
                    cmd.Parameters.AddWithValue("@EndDate", end);
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
            catch (Exception ex)
            {
                // Log lỗi nếu cần
                System.Diagnostics.Debug.WriteLine("GetBookedRoomsInRange error: " + ex.Message);
            }

            return result;
        }

        private void DtpGioBatDau_Click(object sender, EventArgs e)
        {
            ShowTimePicker(dtpGioBatDau);
        }

        private void DtpGioBatDau_MouseDown(object sender, MouseEventArgs e)
        {
            ShowTimePicker(dtpGioBatDau);
        }

        private void DtpGioKetThuc_Click(object sender, EventArgs e)
        {
            ShowTimePicker(dtpGioKetThuc);
        }

        private void DtpGioKetThuc_MouseDown(object sender, MouseEventArgs e)
        {
            ShowTimePicker(dtpGioKetThuc);
        }

        private void ShowTimePicker(Guna.UI2.WinForms.Guna2DateTimePicker picker)
        {
            if (picker == null) return;
            using (var clock = new TimePickerForm(picker.Value))
            {
                if (clock.ShowDialog(this) == DialogResult.OK)
                {
                    picker.Value = clock.SelectedDateTime;
                }
            }
        }

        private void LvPhongChon_MouseDown(object sender, MouseEventArgs e)
        {
            var hit = lvPhongChon.HitTest(e.Location);
            if (hit.Item != null && hit.SubItem != null)
            {
                int subIndex = hit.Item.SubItems.IndexOf(hit.SubItem);
                // Cột 1 (index 1) là Số người
                if (subIndex == 1)
                {
                    _editingRoomCode = hit.Item.SubItems[0].Text;
                    int current = 1;
                    int.TryParse(hit.SubItem.Text, out current);
                    _qtyEditor.Value = Math.Max(1, current);
                    _qtyEditor.Bounds = hit.SubItem.Bounds;
                    _qtyEditor.Visible = true;
                    _qtyEditor.BringToFront();
                    _qtyEditor.Focus();
                }
                else
                {
                    _qtyEditor.Visible = false;
                }
            }
            else
            {
                _qtyEditor.Visible = false;
            }
        }

        private void QtyEditor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                CommitQuantityEdit();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                _qtyEditor.Visible = false;
                e.Handled = true;
            }
        }

        private void QtyEditor_Leave(object sender, EventArgs e)
        {
            // Khi mất focus thì lưu lại
            if (_qtyEditor.Visible)
            {
                CommitQuantityEdit();
            }
        }

        private void CommitQuantityEdit()
        {
            if (string.IsNullOrEmpty(_editingRoomCode))
            {
                _qtyEditor.Visible = false;
                return;
            }

            int newVal = (int)_qtyEditor.Value;
            var row = dtChon.Rows.Cast<DataRow>().FirstOrDefault(r => string.Equals(r["maphong"].ToString(), _editingRoomCode, StringComparison.OrdinalIgnoreCase));
            if (row != null)
            {
                row["Soluong"] = newVal;
                PopulateListViewsFromDataTables();
            }
            _qtyEditor.Visible = false;
            _editingRoomCode = null;
        }

        // ctor overloads so callers can preselect rooms
        public BookingRoom_Details(string preselect) : this()
        {
            if (!string.IsNullOrWhiteSpace(preselect))
                _preselectedRooms = new List<string> { preselect.Trim().ToUpper() };
        }

        public BookingRoom_Details(IEnumerable<string> preselect) : this()
        {
            if (preselect != null)
                _preselectedRooms = preselect.Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s.Trim().ToUpper()).ToList();
        }

        // ctor cho phép khóa ngày/giờ theo thời gian đang xem ở danh sách phòng
        public BookingRoom_Details(string preselect, DateTime viewTime, bool lockDateTime = true) : this(preselect)
        {
            _lockedViewTime = viewTime;
            _lockDateTimePickers = lockDateTime;
        }

        public BookingRoom_Details(IEnumerable<string> preselect, DateTime viewTime, bool lockDateTime = true) : this(preselect)
        {
            _lockedViewTime = viewTime;
            _lockDateTimePickers = lockDateTime;
        }

        // Allow caller to pass rooms that should not appear in available list
        public void SetUnavailableRooms(IEnumerable<string> codes)
        {
            _unavailableRooms.Clear();
            if (codes == null) return;
            foreach (var c in codes)
                _unavailableRooms.Add(c.Trim().ToUpper());
        }

        // Cung cấp danh sách phòng từ DB hoặc nguồn dữ liệu bên ngoài
        public void SetAvailableRooms(IEnumerable<(string RoomCode, string RoomType)> rooms)
        {
            _roomsFromSource.Clear();
            _roomTypeLookup.Clear();
            if (rooms == null) return;
            foreach (var r in rooms)
            {
                if (string.IsNullOrWhiteSpace(r.RoomCode)) continue;
                var code = r.RoomCode.Trim().ToUpper();
                var type = r.RoomType ?? string.Empty;
                _roomsFromSource.Add((code, type));
                _roomTypeLookup[code] = type;
            }
        }

        private void BookingRoom_Details_Load(object sender, EventArgs e)
        {
            ApplyLockedViewTime();

            // 1. Thiết lập cấu trúc cho bảng Phòng Trống
            dtTrong = new DataTable();
            dtTrong.Columns.Add("maphong");
            dtTrong.Columns.Add("LoaiPhong");

            // 2. Thiết lập cấu trúc cho bảng Phòng Chọn (thêm cột Soluong)
            dtChon = new DataTable();
            dtChon.Columns.Add("maphong");
            dtChon.Columns.Add("NgayBD", typeof(DateTime));
            dtChon.Columns.Add("NgayKT", typeof(DateTime));
            dtChon.Columns.Add("Soluong", typeof(int));

            // 3. Lọc và hiển thị danh sách phòng trống theo khoảng thời gian
            RefreshAvailableRoomsList();

            // Wire lvPhongChon double-click to allow remove or edit quantity
            this.lvPhongChon.MouseDoubleClick += LvPhongChon_MouseDoubleClick;

            // If preselected rooms exist, move them to chosen list
            if (_preselectedRooms != null && _preselectedRooms.Count > 0)
            {
                foreach (var code in _preselectedRooms)
                {
                    var node = lvPhongTrong.Items.Cast<ListViewItem>().FirstOrDefault(it => string.Equals(it.Text, code, StringComparison.OrdinalIgnoreCase));
                    if (node != null)
                    {
                        MoveRoomToChosen(node);
                    }
                }
            }

            // không tự đồng bộ ngày cho tất cả phòng để cho phép chỉnh riêng từng phòng
        }

        private void PopulateListViewsFromDataTables()
        {
            lvPhongTrong.Items.Clear();
            foreach (DataRow r in dtTrong.Rows.Cast<DataRow>().OrderBy(r => r["maphong"].ToString()))
            {
                var item = new ListViewItem(r["maphong"].ToString());
                item.SubItems.Add(r["LoaiPhong"].ToString());
                var addSub = item.SubItems.Add("+");
                // plus text is centered via column settings
                item.Tag = r["LoaiPhong"].ToString();
                lvPhongTrong.Items.Add(item);
            }
            if (lvPhongTrong.Columns.Count > 0)
            {
                lvPhongTrong.Columns[lvPhongTrong.Columns.Count - 1].Width = 60;
            }

            lvPhongChon.Items.Clear();
            foreach (DataRow r in dtChon.Rows.Cast<DataRow>().OrderBy(r => r["maphong"].ToString()))
            {
                var item = new ListViewItem(r["maphong"].ToString());
                item.SubItems.Add(r.Table.Columns.Contains("Soluong") ? r["Soluong"].ToString() : "1");
                DateTime bd = r.Table.Columns.Contains("NgayBD") && r["NgayBD"] != DBNull.Value ? (DateTime)r["NgayBD"] : dtpNgayBatDau.Value;
                DateTime kt = r.Table.Columns.Contains("NgayKT") && r["NgayKT"] != DBNull.Value ? (DateTime)r["NgayKT"] : dtpNgayKetThuc.Value;
                item.SubItems.Add(bd.ToShortDateString());
                item.SubItems.Add(kt.ToShortDateString());
                var delSub = item.SubItems.Add("X");
                // delete text centered via column settings
                lvPhongChon.Items.Add(item);
            }
            if (lvPhongChon.Columns.Count > 0)
            {
                lvPhongChon.Columns[lvPhongChon.Columns.Count - 1].Width = 60;
            }

            lvPhongTrong.Invalidate();
            lvPhongChon.Invalidate();
        }

        private void MoveSelectedTrongToChon()
        {
            if (lvPhongTrong.SelectedItems.Count == 0) return;
            MoveRoomToChosen(lvPhongTrong.SelectedItems[0]);
        }

        private void RemoveSelectedChon()
        {
            if (lvPhongChon.SelectedItems.Count == 0) return;
            var sel = lvPhongChon.SelectedItems[0];
            RemoveChosenItem(sel.Text);
        }

        private string InferRoomType(string code)
        {
            int num;
            if (!int.TryParse(code.Trim().TrimStart('P', 'p'), out num)) return "";
            if (num <= 12) return "Phòng Đơn";
            if (num <= 20) return "Phòng Đôi";
            return "Phòng Gia Đình";
        }

        private void MoveRoomToChosen(ListViewItem item)
        {
            string maP = item.Text;
            // nếu đã có trong dtChon thì bỏ qua
            if (dtChon.Rows.Cast<DataRow>().Any(r => string.Equals(r["maphong"].ToString(), maP, StringComparison.OrdinalIgnoreCase)))
                return;

            DateTime ngayBD = dtpNgayBatDau.Value.Date + dtpGioBatDau.Value.TimeOfDay;
            DateTime ngayKT = dtpNgayKetThuc.Value.Date + dtpGioKetThuc.Value.TimeOfDay;

            dtChon.Rows.Add(maP, ngayBD, ngayKT, 1);

            var row = dtTrong.Rows.Cast<DataRow>().FirstOrDefault(r => string.Equals(r["maphong"].ToString(), maP, StringComparison.OrdinalIgnoreCase));
            if (row != null) dtTrong.Rows.Remove(row);

            PopulateListViewsFromDataTables();
        }

        // Allow editing quantity or removing from chosen list via double-click
        private void LvPhongChon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var hit = lvPhongChon.HitTest(e.Location);
            if (hit.Item == null) return;

            int subIndex = hit.Item.SubItems.IndexOf(hit.SubItem);
            string code = hit.Item.Text;

            // Nếu double-click vào cột Số người -> bật editor inline
            if (subIndex == 1)
            {
                _editingRoomCode = code;
                int current = 1;
                int.TryParse(hit.SubItem.Text, out current);
                _qtyEditor.Value = Math.Max(1, current);
                _qtyEditor.Bounds = hit.SubItem.Bounds;
                _qtyEditor.Visible = true;
                _qtyEditor.BringToFront();
                _qtyEditor.Focus();
                return;
            }

            // Double-click cột Ngày BD / Ngày KT để chỉnh riêng
            if (subIndex == 2 || subIndex == 3)
            {
                EditRoomDates(code);
                return;
            }

            // Double-click cột khác -> hỏi xóa
            var result = MessageBox.Show("Xóa phòng này khỏi danh sách?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                RemoveChosenItem(code);
                _qtyEditor.Visible = false;
            }
        }

        // Simple prompt dialog (replacement for Interaction.InputBox)
        private string PromptForString(string title, string prompt, string defaultValue)
        {
            using (Form form = new Form())
            {
                form.Text = title;
                form.FormBorderStyle = FormBorderStyle.FixedDialog;
                form.StartPosition = FormStartPosition.CenterParent;
                form.ClientSize = new Size(300, 110);

                Label lbl = new Label() { Left = 10, Top = 10, Text = prompt, AutoSize = true };
                TextBox txt = new TextBox() { Left = 10, Top = 35, Width = 270, Text = defaultValue };
                Button btnOk = new Button() { Text = "OK", Left = 125, Width = 70, Top = 65, DialogResult = DialogResult.OK };
                Button btnCancel = new Button() { Text = "Cancel", Left = 205, Width = 70, Top = 65, DialogResult = DialogResult.Cancel };

                form.Controls.Add(lbl);
                form.Controls.Add(txt);
                form.Controls.Add(btnOk);
                form.Controls.Add(btnCancel);
                form.AcceptButton = btnOk;
                form.CancelButton = btnCancel;

                return form.ShowDialog() == DialogResult.OK ? txt.Text : defaultValue;
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show("Bạn có chắc chắn muốn hủy đặt phòng không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close(); // Đóng Form
            }
        }


        private void btnLuu_Click(object sender, EventArgs e)
        {
            // === VALIDATION ===

            // 1. Kiểm tra Họ Tên - chỉ cho phép chữ cái và khoảng trắng
            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                MessageBox.Show("Vui lòng nhập họ tên khách hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHoTen.Focus();
                return;
            }
            if (!IsValidName(txtHoTen.Text))
            {
                MessageBox.Show("Họ tên chỉ được chứa chữ cái và khoảng trắng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHoTen.Focus();
                return;
            }

            // 2. Kiểm tra CCCD - chỉ cho phép số
            if (string.IsNullOrWhiteSpace(txtCCCD.Text))
            {
                MessageBox.Show("Vui lòng nhập số CCCD!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCCCD.Focus();
                return;
            }
            if (!IsValidCCCD(txtCCCD.Text))
            {
                MessageBox.Show("CCCD chỉ được chứa số và phải có 12 ký tự!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCCCD.Focus();
                return;
            }

            // 3. Kiểm tra Số điện thoại - chỉ cho phép số và ký tự + (đầu số quốc gia)
            if (string.IsNullOrWhiteSpace(txtSDT.Text))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSDT.Focus();
                return;
            }
            if (!IsValidPhone(txtSDT.Text))
            {
                MessageBox.Show("Số điện thoại không hợp lệ (chỉ chứa số, dấu + và khoảng trắng)!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSDT.Focus();
                return;
            }
            // Kiểm tra độ dài số điện thoại (loại bỏ khoảng trắng và dấu +)
            string phoneDigits = Regex.Replace(txtSDT.Text, @"[^\d]", "");
            if (phoneDigits.Length < 9 || phoneDigits.Length > 15)
            {
                MessageBox.Show("Số điện thoại phải có từ 9-15 chữ số!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSDT.Focus();
                return;
            }

            // 4. Kiểm tra Email (nếu có nhập)
            if (guna2TextBox1 != null && !string.IsNullOrWhiteSpace(guna2TextBox1.Text))
            {
                if (!IsValidEmail(guna2TextBox1.Text))
                {
                    MessageBox.Show("Email không hợp lệ! Vui lòng nhập đúng định dạng (ví dụ: example@gmail.com)", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    guna2TextBox1.Focus();
                    return;
                }
            }

            // === KẾT THÚC VALIDATION ===

            if (dtChon.Rows.Count > 0)
            {
                ResultCustomerName = txtHoTen.Text;
                ResultCCCD = txtCCCD.Text;
                ResultSDT = txtSDT.Text;
                ResultGender = cboGioiTinh != null ? cboGioiTinh.Text : string.Empty;
                ResultNationality = guna2ComboBox1 != null ? guna2ComboBox1.Text : string.Empty;
                ResultEmail = guna2TextBox1 != null ? guna2TextBox1.Text : string.Empty;
                ResultAddress = txtDiaChi != null ? txtDiaChi.Text : string.Empty;
                ResultRooms = string.Join(" ", dtChon.Rows.Cast<DataRow>()
                                   .Select(r => r["maphong"].ToString()));
                ResultStartDate = dtpNgayBatDau.Value.Date + dtpGioBatDau.Value.TimeOfDay;
                ResultEndDate = dtpNgayKetThuc.Value.Date + dtpGioKetThuc.Value.TimeOfDay;
                ResultDateRange = ResultStartDate.ToString("dd/MM/yyyy HH:mm") + " - " + ResultEndDate.ToString("dd/MM/yyyy HH:mm");
                ResultRoomDetails = BuildRoomDetailsString();

                var rooms = dtChon.Rows.Cast<DataRow>()
                           .Select(r => r["maphong"].ToString())
                           .ToList();

                // === KIỂM TRA TRÙNG LỊCH ===
                foreach (DataRow r in dtChon.Rows)
                {
                    var room = r["maphong"].ToString();
                    var start = r["NgayBD"] != DBNull.Value ? (DateTime)r["NgayBD"] : ResultStartDate;
                    var end = r["NgayKT"] != DBNull.Value ? (DateTime)r["NgayKT"] : ResultEndDate;

                    BookingInfo existing;
                    if (BookingManager.TryGetBooking(room, out existing))
                    {
                        // overlap nếu: start < existing.End && end > existing.Start
                        if (start < existing.End && end > existing.Start)
                        {
                            MessageBox.Show(
                              $"Phòng {room} đã được đặt bởi khách {existing.Customer}\n" +
                              $"Từ: {existing.Start}\nĐến: {existing.End}\n\n" +
                              "Vui lòng chọn phòng khác hoặc thay đổi thời gian.",
                              "Trùng lịch đặt phòng",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Warning);

                            return; // KHÔNG cho lưu
                        }
                    }
                }

                // === HIỂN THỊ BẢNG GIÁ CHI TIẾT ===
                string priceDetails = BuildPriceDetailsString();
                var confirmResult = MessageBox.Show(
                  $"Xác nhận đặt phòng cho khách: {txtHoTen.Text}\n\n" +
                  $"Chi tiết giá:\n{priceDetails}\n\n" +
                  "Bạn có muốn tiếp tục lưu?",
                  "Xác nhận đặt phòng",
                  MessageBoxButtons.YesNo,
                  MessageBoxIcon.Question);

                if (confirmResult != DialogResult.Yes)
                    return;

                // lưu từng phòng với start/end riêng
                foreach (DataRow r in dtChon.Rows)
                {
                    var room = r["maphong"].ToString();
                    var start = r["NgayBD"] != DBNull.Value ? (DateTime)r["NgayBD"] : ResultStartDate;
                    var end = r["NgayKT"] != DBNull.Value ? (DateTime)r["NgayKT"] : ResultEndDate;
                    var info = new BookingInfo { Customer = ResultCustomerName, Start = start, End = end };
                    BookingManager.AddBooking(room, info);
                }

                // Lưu xuống CSDL và cập nhật trạng thái phòng
                SaveBookingToDatabase();

               
            try
                {
                    // For each chosen room create an invoice record (room charge only).
                    foreach (DataRow r in dtChon.Rows)
                    {
                        string room = r["maphong"].ToString();
                        DateTime start = r["NgayBD"] != DBNull.Value ? (DateTime)r["NgayBD"] : ResultStartDate;
                        DateTime end = r["NgayKT"] != DBNull.Value ? (DateTime)r["NgayKT"] : ResultEndDate;

                        // fetch base price from v_RoomPrice
                        decimal basePrice = 0m;
                        var connStr = ConfigurationManager.ConnectionStrings["ConnStr"]?.ConnectionString;
                        if (!string.IsNullOrWhiteSpace(connStr))
                        {
                            using (var conn = new SqlConnection(connStr))
                            using (var cmd = new SqlCommand("SELECT TOP 1 AppliedPrice FROM dbo.v_RoomPrice WHERE RoomID = @RoomID", conn))
                            {
                                cmd.Parameters.AddWithValue("@RoomID", room);
                                conn.Open();
                                var obj = cmd.ExecuteScalar();
                                if (obj != null && obj != DBNull.Value) basePrice = Convert.ToDecimal(obj);
                            }
                        }

                        // compute total for this room with holiday/weekend rules
                        decimal roomTotal = HolidayPriceConfig.CalculateTotalPrice(basePrice, start, end);

                        // prepare service list with single "Tiền phòng" line (you can add more services if needed)
                        var svcList = new List<ServiceItem>
        {
            new ServiceItem
            {
                Name = ROOM_SERVICE_NAME,
                Quantity = Math.Max(1, (int)Math.Ceiling((end - start).TotalDays)),
                UnitPrice = basePrice,
                Amount = roomTotal
            }
        };

                        // booking info minimal (used by repository to find BookingDetail); not strictly required but helpful
                        var info = new BookingInfo { Customer = ResultCustomerName, Start = start, End = end, Services = svcList };

                        // Persist invoice and finalize room/booking in DB (transactional)
                        InvoiceRepository.SaveInvoiceAndFinalize(room, info, svcList, roomTotal, null);
                    }
                }
                catch (Exception exInvoice)
                {
                    // Log and inform user — decide policy: abort finalization or continue.
                    MessageBox.Show("Lưu hóa đơn tự động thất bại: " + exInvoice.Message + "\nVui lòng kiểm tra CSDL.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    // Optionally return; // stop closing form if you want full atomicity
                }

                try
                {
                    var listForm = Application.OpenForms.OfType<ListRoom>().FirstOrDefault();
                    if (listForm != null)
                    {
                        // === SỬA Ở ĐÂY: Thêm điều kiện kiểm tra ngày ===

                        // Chỉ đổi màu đỏ (Đang thuê) trên giao diện NẾU ngày bắt đầu <= hiện tại
                        if (ResultStartDate.Date <= DateTime.Now.Date)
                        {
                            listForm.MarkRoomsAsBooked(rooms, ResultCustomerName);
                        }
                        else
                        {
                            // Nếu đặt tương lai, KHÔNG gọi MarkRoomsAsBooked.
                            // Phòng sẽ giữ nguyên màu xanh (Trống).
                            // Nếu bạn muốn hiển thị màu vàng (Đặt trước), hãy gọi hàm đó ở đây (nếu có).
                        }

                        listForm.RefreshFromBookings(rooms);
                    }
                }
                catch { }

                MessageBox.Show("Đã lưu thông tin đặt phòng thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        /// <summary>
        /// Tạo chuỗi chi tiết giá với thông tin tăng giá lễ/cuối tuần
        /// </summary>
        private string BuildPriceDetailsString()
        {
            var sb = new System.Text.StringBuilder();
            decimal grandTotal = 0m;

            var connStr = ConfigurationManager.ConnectionStrings["ConnStr"]?.ConnectionString;
            if (string.IsNullOrWhiteSpace(connStr))
                return "Không thể tính giá (thiếu connection string)";

            try
            {
                using (var conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    foreach (DataRow r in dtChon.Rows)
                    {
                        var room = r["maphong"].ToString();
                        var start = r["NgayBD"] != DBNull.Value ? (DateTime)r["NgayBD"] : ResultStartDate;
                        var end = r["NgayKT"] != DBNull.Value ? (DateTime)r["NgayKT"] : ResultEndDate;

                        // Lấy giá cơ bản
                        decimal basePrice = 0m;
                        using (var cmd = new SqlCommand("SELECT TOP 1 AppliedPrice FROM dbo.v_RoomPrice WHERE RoomID = @RoomID", conn))
                        {
                            cmd.Parameters.AddWithValue("@RoomID", room);
                            var obj = cmd.ExecuteScalar();
                            if (obj != null && obj != DBNull.Value)
                                basePrice = Convert.ToDecimal(obj);
                        }

                        sb.AppendLine($"📌 Phòng {room}:");
                        sb.AppendLine($"   Giá cơ bản: {basePrice:N0} VNĐ/đêm");

                        decimal roomTotal = 0m;
                        int days = 0;

                        for (DateTime date = start.Date; date < end.Date; date = date.AddDays(1))
                        {
                            days++;
                            decimal multiplier = HolidayPriceConfig.GetPriceMultiplier(date);
                            decimal dayPrice = basePrice * multiplier;
                            roomTotal += dayPrice;

                            // Chỉ hiển thị ngày có tăng giá
                            if (multiplier > 1.0m)
                            {
                                var holidayInfo = HolidayPriceConfig.GetHolidayInfo(date);
                                string reason = holidayInfo != null ? holidayInfo.Name :
                                  (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday ? "Cuối tuần" : "");
                                sb.AppendLine($"   • {date:dd/MM/yyyy} ({date.DayOfWeek}): {dayPrice:N0} VNĐ (+{(multiplier - 1) * 100:0}% - {reason})");
                            }
                        }

                        sb.AppendLine($"   Số đêm: {days}");
                        sb.AppendLine($"   💰 Tổng phòng {room}: {roomTotal:N0} VNĐ");
                        sb.AppendLine();

                        grandTotal += roomTotal;
                    }

                    sb.AppendLine($"══════════════════════════");
                    sb.AppendLine($"💵 TỔNG CỘNG: {grandTotal:N0} VNĐ");
                }
            }
            catch (Exception ex)
            {
                sb.AppendLine($"Lỗi tính giá: {ex.Message}");
            }

            return sb.ToString();
        }

        private void guna2CirclePictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2Panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lvPhongTrong_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lvPhongTrong.SelectedItems.Count == 0) return;
            MoveRoomToChosen(lvPhongTrong.SelectedItems[0]);
        }

        private void Lv_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void Lv_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            // để DrawSubItem xử lý hoặc vẽ mặc định
            e.DrawDefault = true;
        }

        private void LvPhongTrong_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            if (e.ColumnIndex == lvPhongTrong.Columns.Count - 1)
            {
                var bounds = e.Bounds;
                using (var brush = new SolidBrush(Color.FromArgb(230, 255, 230)))
                using (var pen = new Pen(Color.ForestGreen, 1))
                {
                    e.Graphics.FillRectangle(brush, bounds);
                    e.Graphics.DrawRectangle(pen, bounds.Left + 2, bounds.Top + 2, bounds.Width - 4, bounds.Height - 4);
                }

                if (imageList1 != null && imageList1.Images.Count > 0)
                {
                    var img = imageList1.Images[0];
                    int x = bounds.Left + (bounds.Width - img.Width) / 2;
                    int y = bounds.Top + (bounds.Height - img.Height) / 2;
                    e.Graphics.DrawImage(img, x, y, img.Width, img.Height);
                }
                else
                {
                    int cx = bounds.Left + bounds.Width / 2;
                    int cy = bounds.Top + bounds.Height / 2;
                    using (var pen = new Pen(Color.ForestGreen, 2))
                    {
                        e.Graphics.DrawEllipse(pen, cx - 12, cy - 12, 24, 24);
                        e.Graphics.DrawLine(pen, cx - 8, cy, cx + 8, cy);
                        e.Graphics.DrawLine(pen, cx, cy - 8, cx, cy + 8);
                    }
                    using (var brush = new SolidBrush(Color.ForestGreen))
                    using (var font = new Font("Segoe UI", 16, FontStyle.Bold))
                    {
                        var size = e.Graphics.MeasureString(e.SubItem.Text, font);
                        e.Graphics.DrawString(e.SubItem.Text, font, brush, cx - size.Width / 2, cy - size.Height / 2 - 2);
                    }
                }
            }
            else
            {
                e.DrawDefault = true;
            }
        }

        private void LvPhongChon_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            if (e.ColumnIndex == lvPhongChon.Columns.Count - 1)
            {
                var bounds = e.Bounds;
                using (var brush = new SolidBrush(Color.FromArgb(255, 230, 230)))
                using (var pen = new Pen(Color.IndianRed, 1))
                {
                    e.Graphics.FillRectangle(brush, bounds);
                    e.Graphics.DrawRectangle(pen, bounds.Left + 2, bounds.Top + 2, bounds.Width - 4, bounds.Height - 4);
                }

                if (imageList1 != null && imageList1.Images.Count > 1)
                {
                    var img = imageList1.Images[1];
                    int x = bounds.Left + (bounds.Width - img.Width) / 2;
                    int y = bounds.Top + (bounds.Height - img.Height) / 2;
                    e.Graphics.DrawImage(img, x, y, img.Width, img.Height);
                }
                else
                {
                    var b = bounds;
                    using (var pen = new Pen(Color.IndianRed, 2))
                    {
                        e.Graphics.DrawRectangle(pen, b.Left + 6, b.Top + 6, b.Width - 12, b.Height - 12);
                        e.Graphics.DrawLine(pen, b.Left + 10, b.Top + 10, b.Right - 10, b.Bottom - 10);
                        e.Graphics.DrawLine(pen, b.Right - 10, b.Top + 10, b.Left + 10, b.Bottom - 10);
                    }
                    using (var brush = new SolidBrush(Color.IndianRed))
                    using (var font = new Font("Segoe UI", 16, FontStyle.Bold))
                    {
                        var size = e.Graphics.MeasureString(e.SubItem.Text, font);
                        e.Graphics.DrawString(e.SubItem.Text, font, brush, b.Left + (b.Width - size.Width) / 2, b.Top + (b.Height - size.Height) / 2 - 2);
                    }
                }
            }
            else
            {
                e.DrawDefault = true;
            }
        }

        private void LvPhongTrong_MouseDown(object sender, MouseEventArgs e)
        {
            var hit = lvPhongTrong.HitTest(e.Location);
            var item = hit.Item;
            if (item == null) return;

            int colIndex = hit.SubItem != null ? item.SubItems.IndexOf(hit.SubItem) : GetColumnIndexAtX(lvPhongTrong, e.X);
            int actionCol = lvPhongTrong.Columns.Count - 1;
            if (colIndex == actionCol)
            {
                MoveRoomToChosen(item);
                return;
            }
        }

        private void LvPhongChon_MouseDown_Remove(object sender, MouseEventArgs e)
        {
            var hit = lvPhongChon.HitTest(e.Location);
            var item = hit.Item;
            if (item == null) return;

            int colIndex = hit.SubItem != null ? item.SubItems.IndexOf(hit.SubItem) : GetColumnIndexAtX(lvPhongChon, e.X);
            int actionCol = lvPhongChon.Columns.Count - 1;
            if (colIndex == actionCol)
            {
                RemoveChosenItem(item.Text);
                _qtyEditor.Visible = false;
                return;
            }
        }

        private int GetColumnIndexAtX(ListView lv, int x)
        {
            int cur = 0;
            for (int i = 0; i < lv.Columns.Count; i++)
            {
                cur += lv.Columns[i].Width;
                if (x < cur) return i;
            }
            return -1;
        }

        private void RemoveChosenItem(string code)
        {
            if (string.IsNullOrWhiteSpace(code)) return;
            var row = dtChon.Rows.Cast<DataRow>().FirstOrDefault(r => string.Equals(r["maphong"].ToString(), code, StringComparison.OrdinalIgnoreCase));
            if (row != null) dtChon.Rows.Remove(row);

            // chỉ thêm lại vào phòng trống nếu chưa có
            bool existsTrong = dtTrong.Rows.Cast<DataRow>().Any(r => string.Equals(r["maphong"].ToString(), code, StringComparison.OrdinalIgnoreCase));
            if (!existsTrong)
            {
                string type;
                if (!_roomTypeLookup.TryGetValue(code.Trim().ToUpper(), out type))
                {
                    type = InferRoomType(code);
                }
                dtTrong.Rows.Add(code, type);
            }
            PopulateListViewsFromDataTables();
        }

        private void ApplyModernTheme()
        {
            try
            {
                this.BackColor = Color.FromArgb(245, 248, 252);
                if (guna2Panel1 != null) guna2Panel1.FillColor = Color.WhiteSmoke;
                if (guna2Panel3 != null)
                {
                    guna2Panel3.FillColor = Color.White;
                    guna2Panel3.BorderThickness = 0;
                }

                // Buttons
                if (btnLuu != null)
                {
                    btnLuu.FillColor = Color.FromArgb(0, 120, 215);
                    btnLuu.Font = new Font("Segoe UI", 10.5f, FontStyle.Bold);
                }
                if (btnHuy != null)
                {
                    btnHuy.FillColor = Color.FromArgb(220, 53, 69);
                    btnHuy.Font = new Font("Segoe UI", 10.5f, FontStyle.Bold);
                }

                // Inputs
                txtHoTen.BorderThickness = txtCCCD.BorderThickness = txtSDT.BorderThickness = txtDiaChi.BorderThickness = guna2ComboBox1.BorderThickness = 1;
                txtHoTen.BorderColor = txtCCCD.BorderColor = txtSDT.BorderColor = txtDiaChi.BorderColor = guna2ComboBox1.BorderColor = Color.FromArgb(210, 220, 230);
                txtHoTen.FillColor = txtCCCD.FillColor = txtSDT.FillColor = txtDiaChi.FillColor = guna2ComboBox1.FillColor = Color.FromArgb(248, 250, 252);
                cboGioiTinh.FillColor = Color.FromArgb(248, 250, 252);

                // Date/time pickers
                dtpNgayBatDau.FillColor = dtpNgayKetThuc.FillColor = Color.White;
                dtpGioBatDau.FillColor = dtpGioKetThuc.FillColor = Color.White;

                // ListViews styling
                StyleListView(lvPhongTrong);
                StyleListView(lvPhongChon);
            }
            catch { }
        }

        private void StyleListView(ListView lv)
        {
            if (lv == null) return;
            lv.BackColor = Color.FromArgb(249, 252, 255);
            lv.ForeColor = Color.FromArgb(36, 48, 64);
            lv.BorderStyle = BorderStyle.None;
            lv.FullRowSelect = true;
            lv.GridLines = false;
            lv.HeaderStyle = ColumnHeaderStyle.Nonclickable;
        }

        private void ApplyLockedViewTime()
        {
            if (!_lockedViewTime.HasValue) return;
            var view = _lockedViewTime.Value;

            // Căn start theo ngày/giờ đang xem, end mặc định +1 ngày cùng giờ
            dtpNgayBatDau.Value = view.Date;
            dtpNgayKetThuc.Value = view.Date.AddDays(1);
            dtpGioBatDau.Value = view;
            dtpGioKetThuc.Value = view.AddDays(1);

            if (_lockDateTimePickers)
            {
                dtpNgayBatDau.Enabled = false;
                dtpNgayKetThuc.Enabled = false;
                dtpGioBatDau.Enabled = false;
                dtpGioKetThuc.Enabled = false;
            }
        }

        private void EditRoomDates(string roomCode)
        {
            var row = dtChon.Rows.Cast<DataRow>().FirstOrDefault(r => string.Equals(r["maphong"].ToString(), roomCode, StringComparison.OrdinalIgnoreCase));
            if (row == null) return;
            DateTime curBd = row["NgayBD"] != DBNull.Value ? (DateTime)row["NgayBD"] : dtpNgayBatDau.Value;
            DateTime curKt = row["NgayKT"] != DBNull.Value ? (DateTime)row["NgayKT"] : dtpNgayKetThuc.Value;

            using (var f = new Form())
            {
                f.Text = "Chỉnh ngày phòng " + roomCode;
                f.FormBorderStyle = FormBorderStyle.FixedDialog;
                f.StartPosition = FormStartPosition.CenterParent;
                f.ClientSize = new Size(320, 160);

                var lblBd = new Label { Left = 10, Top = 10, Width = 120, Text = "Ngày/giờ bắt đầu" };
                var dtBd = new DateTimePicker { Left = 10, Top = 30, Width = 290, Format = DateTimePickerFormat.Custom, CustomFormat = "dd/MM/yyyy HH:mm" };
                dtBd.Value = curBd;
                var lblKt = new Label { Left = 10, Top = 65, Width = 120, Text = "Ngày/giờ kết thúc" };
                var dtKt = new DateTimePicker { Left = 10, Top = 85, Width = 290, Format = DateTimePickerFormat.Custom, CustomFormat = "dd/MM/yyyy HH:mm" };
                dtKt.Value = curKt;
                var btnOk = new Button { Text = "OK", Left = 145, Width = 70, Top = 120, DialogResult = DialogResult.OK };
                var btnCancel = new Button { Text = "Hủy", Left = 225, Width = 70, Top = 120, DialogResult = DialogResult.Cancel };
                f.Controls.AddRange(new Control[] { lblBd, dtBd, lblKt, dtKt, btnOk, btnCancel });
                f.AcceptButton = btnOk;
                f.CancelButton = btnCancel;

                if (f.ShowDialog(this) == DialogResult.OK)
                {
                    if (dtKt.Value <= dtBd.Value)
                    {
                        MessageBox.Show("Ngày/giờ kết thúc phải sau ngày/giờ bắt đầu", "Cảnh báo");
                        return;
                    }
                    row["NgayBD"] = dtBd.Value;
                    row["NgayKT"] = dtKt.Value;
                    PopulateListViewsFromDataTables();
                }
            }
        }

        private string BuildRoomDetailsString()
        {
            return string.Join("\r\n", dtChon.Rows.Cast<DataRow>()
                      .Select(r =>
                      {
                          var bd = r["NgayBD"] != DBNull.Value ? (DateTime)r["NgayBD"] : dtpNgayBatDau.Value;
                          var kt = r["NgayKT"] != DBNull.Value ? (DateTime)r["NgayKT"] : dtpNgayKetThuc.Value;
                          return r["maphong"] + " (" + bd.ToString("dd/MM/yyyy HH:mm") + " - " + kt.ToString("dd/MM/yyyy HH:mm") + ")";
                      }));
        }

        private void PopulateCountryCombo()
        {
            if (guna2ComboBox1 == null) return;
            guna2ComboBox1.DisplayMember = "DisplayName";
            guna2ComboBox1.ValueMember = "DialCode";
            guna2ComboBox1.DataSource = _countryPhoneCodes
              .OrderBy(c => c.Name)
              .ToList();

            var defaultCountry = _countryPhoneCodes.FirstOrDefault(c => string.Equals(c.Iso, "VN", StringComparison.OrdinalIgnoreCase));
            if (defaultCountry != null)
            {
                var idx = guna2ComboBox1.Items
                  .Cast<object>()
                  .Select((item, index) => new { item, index })
                  .FirstOrDefault(x => (x.item as CountryPhoneCode)?.Iso == defaultCountry.Iso);
                if (idx != null)
                {
                    guna2ComboBox1.SelectedIndex = idx.index;
                    SetPhonePrefix(defaultCountry.DialCode);
                }
            }
        }

        private void Guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selected = guna2ComboBox1.SelectedItem as CountryPhoneCode;
            if (selected == null) return;
            SetPhonePrefix(selected.DialCode);
        }

        private void SetPhonePrefix(string dialCode)
        {
            if (string.IsNullOrWhiteSpace(dialCode) || txtSDT == null) return;

            string current = txtSDT.Text ?? string.Empty;
            string remainder = current;

            foreach (var code in _countryPhoneCodes.Select(c => c.DialCode).Where(c => !string.IsNullOrEmpty(c)))
            {
                if (current.StartsWith(code, StringComparison.Ordinal))
                {
                    remainder = current.Substring(code.Length).TrimStart();
                    break;
                }
            }

            txtSDT.Text = string.Format("{0} {1}", dialCode, remainder).TrimEnd();
            txtSDT.SelectionStart = txtSDT.Text.Length;
        }

        private void SaveBookingsToDatabase()
        {
            // legacy placeholder
        }

        private void SaveBookingToDatabase()
        {
            var connStr = ConfigurationManager.ConnectionStrings["ConnStr"]?.ConnectionString;
            if (string.IsNullOrWhiteSpace(connStr)) return;

            try
            {
                using (var conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    using (var tran = conn.BeginTransaction())
                    {
                        int customerId = UpsertCustomer(conn, tran, ResultCustomerName, ResultCCCD, ResultSDT, ResultGender, ResultNationality);
                        int employeeId = GetDefaultEmployeeId(conn, tran);
                        if (employeeId <= 0)
                            throw new InvalidOperationException("Không tìm thấy nhân viên mặc định.");

                        string bookingCode = "PT" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
                        int bookingId = InsertBooking(conn, tran, bookingCode, customerId, employeeId);

                        foreach (DataRow r in dtChon.Rows)
                        {
                            var room = r["maphong"].ToString();
                            var start = r["NgayBD"] != DBNull.Value ? (DateTime)r["NgayBD"] : ResultStartDate;
                            var end = r["NgayKT"] != DBNull.Value ? (DateTime)r["NgayKT"] : ResultEndDate;
                            // Sử dụng giá có tính ngày lễ
                            decimal price = GetAppliedPriceWithHoliday(conn, tran, room, start, end);
                            InsertBookingDetail(conn, tran, bookingId, room, start, end, price);
                            UpdateRoomStatus(conn, tran, room, "Đặt");
                        }

                        tran.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không lưu được dữ liệu xuống CSDL.\n" + ex.Message, "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int UpsertCustomer(SqlConnection conn, SqlTransaction tran, string fullName, string cccd, string phone, string gender, string nationality)
        {
            // ưu tiên tìm theo CCCD, nếu không có thì theo Phone, nếu không có nữa thì theo FullName
            int id = 0;
            if (!string.IsNullOrWhiteSpace(cccd))
            {
                using (var cmd = new SqlCommand("SELECT TOP 1 CustomerId FROM dbo.Customers WHERE CCCD = @IdCard", conn, tran))
                {
                    cmd.Parameters.AddWithValue("@IdCard", cccd);
                    var obj = cmd.ExecuteScalar();
                    if (obj != null && obj != DBNull.Value) id = Convert.ToInt32(obj);
                }
            }
            if (id == 0 && !string.IsNullOrWhiteSpace(phone))
            {
                using (var cmd = new SqlCommand("SELECT TOP 1 CustomerId FROM dbo.Customers WHERE PhoneNumber = @Phone", conn, tran))
                {
                    cmd.Parameters.AddWithValue("@Phone", phone);
                    var obj = cmd.ExecuteScalar();
                    if (obj != null && obj != DBNull.Value) id = Convert.ToInt32(obj);
                }
            }
            if (id == 0 && !string.IsNullOrWhiteSpace(fullName))
            {
                using (var cmd = new SqlCommand("SELECT TOP 1 CustomerId FROM dbo.Customers WHERE FullName = @Name", conn, tran))
                {
                    cmd.Parameters.AddWithValue("@Name", fullName);
                    var obj = cmd.ExecuteScalar();
                    if (obj != null && obj != DBNull.Value) id = Convert.ToInt32(obj);
                }
            }

            // NEW: if still not found and incoming CCCD is empty, try to find any customer row where CCCD IS NULL
            // that matches phone, email or full name to avoid inserting duplicate NULLs which violate UNIQUE constraint
            if (id == 0 && string.IsNullOrWhiteSpace(cccd))
            {
                using (var cmd = new SqlCommand(@"SELECT TOP 1 CustomerId FROM dbo.Customers 
                                                 WHERE CCCD IS NULL 
                                                   AND (
                                                       (PhoneNumber IS NOT NULL AND PhoneNumber = @Phone) 
                                                       OR (Email IS NOT NULL AND Email = @Email)
                                                       OR (FullName IS NOT NULL AND FullName = @Name)
                                                   )", conn, tran))
                {
                    cmd.Parameters.AddWithValue("@Phone", string.IsNullOrWhiteSpace(phone) ? (object)DBNull.Value : phone);
                    cmd.Parameters.AddWithValue("@Email", string.IsNullOrWhiteSpace(ResultEmail) ? (object)DBNull.Value : ResultEmail);
                    cmd.Parameters.AddWithValue("@Name", string.IsNullOrWhiteSpace(fullName) ? (object)DBNull.Value : fullName);
                    var obj = cmd.ExecuteScalar();
                    if (obj != null && obj != DBNull.Value) id = Convert.ToInt32(obj);
                }
            }

            // Nếu tìm thấy khách hàng, cập nhật thêm Email và Address nếu có
            if (id != 0)
            {
                using (var cmd = new SqlCommand(@"UPDATE dbo.Customers SET 
                    Email = COALESCE(@Email, Email), 
                    Address = COALESCE(@Address, Address)
                    WHERE CustomerId = @ID", conn, tran))
                {
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.Parameters.AddWithValue("@Email", string.IsNullOrWhiteSpace(ResultEmail) ? (object)DBNull.Value : ResultEmail);
                    cmd.Parameters.AddWithValue("@Address", string.IsNullOrWhiteSpace(ResultAddress) ? (object)DBNull.Value : ResultAddress);
                    cmd.ExecuteNonQuery();
                }
                return id;
            }

            // Tạo khách hàng mới với đầy đủ thông tin bao gồm Email và Address
            using (var cmd = new SqlCommand(@"INSERT INTO dbo.Customers(FullName, CCCD, PhoneNumber, Sex, Nationality, Email, Address)
VALUES(@Name, @IdCard, @Phone, @Gender, @Nationality, @Email, @Address);
SELECT CAST(SCOPE_IDENTITY() AS INT);", conn, tran))
            {
                cmd.Parameters.AddWithValue("@Name", string.IsNullOrWhiteSpace(fullName) ? (object)DBNull.Value : fullName);
                cmd.Parameters.AddWithValue("@IdCard", string.IsNullOrWhiteSpace(cccd) ? (object)DBNull.Value : cccd);
                cmd.Parameters.AddWithValue("@Phone", string.IsNullOrWhiteSpace(phone) ? (object)DBNull.Value : phone);
                cmd.Parameters.AddWithValue("@Gender", string.IsNullOrWhiteSpace(gender) ? (object)DBNull.Value : gender);
                cmd.Parameters.AddWithValue("@Nationality", string.IsNullOrWhiteSpace(nationality) ? (object)DBNull.Value : nationality);
                cmd.Parameters.AddWithValue("@Email", string.IsNullOrWhiteSpace(ResultEmail) ? (object)DBNull.Value : ResultEmail);
                cmd.Parameters.AddWithValue("@Address", string.IsNullOrWhiteSpace(ResultAddress) ? (object)DBNull.Value : ResultAddress);
                id = Convert.ToInt32(cmd.ExecuteScalar());
            }

            return id;
        }

        private int GetDefaultEmployeeId(SqlConnection conn, SqlTransaction tran)
        {
            using (var cmd = new SqlCommand("SELECT TOP 1 EmployeeId FROM dbo.Employees ORDER BY EmployeeId", conn, tran))
            {
                var obj = cmd.ExecuteScalar();
                if (obj == null || obj == DBNull.Value) return 0;
                return Convert.ToInt32(obj);
            }
        }

        private int InsertBooking(SqlConnection conn, SqlTransaction tran, string code, int customerId, int employeeId)
        {
            using (var cmd = new SqlCommand(@"INSERT INTO dbo.HotelBookings(CustomerId, EmployeeId, CreatedDate, Status)
VALUES(@CustomerID, @EmployeeID, GETDATE(), N'Open');
SELECT CAST(SCOPE_IDENTITY() AS INT);", conn, tran))
            {
                cmd.Parameters.AddWithValue("@CustomerID", customerId == 0 ? (object)DBNull.Value : customerId);
                cmd.Parameters.AddWithValue("@EmployeeID", employeeId == 0 ? (object)DBNull.Value : employeeId);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        private void InsertBookingDetail(SqlConnection conn, SqlTransaction tran, int bookingId, string roomId, DateTime checkIn, DateTime checkOut, decimal price)
        {
            using (var cmd = new SqlCommand(@"INSERT INTO dbo.BookingDetails(BookingId, RoomId, CheckIn, CheckOut, AppliedPrice, Note)
VALUES(@BookingID, @RoomID, @CheckIn, @CheckOut, @Price, NULL);", conn, tran))
            {
                cmd.Parameters.AddWithValue("@BookingID", bookingId);
                cmd.Parameters.AddWithValue("@RoomID", roomId);
                cmd.Parameters.Add(new SqlParameter("@CheckIn", System.Data.SqlDbType.DateTime) { Value = checkIn });
                cmd.Parameters.Add(new SqlParameter("@CheckOut", System.Data.SqlDbType.DateTime) { Value = checkOut });
                cmd.Parameters.AddWithValue("@Price", price);
                cmd.ExecuteNonQuery();
            }
        }

        private decimal GetAppliedPrice(SqlConnection conn, SqlTransaction tran, string roomId)
        {
            decimal basePrice = 0m;
            using (var cmd = new SqlCommand("SELECT TOP 1 AppliedPrice FROM dbo.v_RoomPrice WHERE RoomId = @RoomID", conn, tran))
            {
                cmd.Parameters.AddWithValue("@RoomID", roomId);
                var obj = cmd.ExecuteScalar();
                if (obj != null && obj != DBNull.Value)
                    basePrice = Convert.ToDecimal(obj);
            }
            return basePrice;
        }

        /// <summary>
        /// Tính giá phòng có áp dụng tăng giá ngày lễ
        /// </summary>
        private decimal GetAppliedPriceWithHoliday(SqlConnection conn, SqlTransaction tran, string roomId, DateTime checkIn, DateTime checkOut)
        {
            decimal basePrice = GetAppliedPrice(conn, tran, roomId);
            if (basePrice <= 0) return 0m;

            // Tính tổng giá theo từng ngày với tỷ lệ tăng giá ngày lễ
            return HolidayPriceConfig.CalculateTotalPrice(basePrice, checkIn, checkOut);
        }

        private void UpdateRoomStatus(SqlConnection conn, SqlTransaction tran, string roomCode, string status)
        {
            if (string.IsNullOrWhiteSpace(roomCode)) return;
            using (var cmd = new SqlCommand("UPDATE dbo.Rooms SET Status = @Status WHERE RoomId = @RoomID", conn, tran))
            {
                cmd.Parameters.AddWithValue("@RoomID", roomCode);
                cmd.Parameters.AddWithValue("@Status", status);
                cmd.ExecuteNonQuery();
            }
        }

        private void guna2PictureBox6_Click(object sender, EventArgs e)
        {

        }

        private void txtCCCD_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtSDT_TextChanged(object sender, EventArgs e)
        {
        }

        private void dtpGioBatDau_ValueChanged(object sender, EventArgs e)
        {

        }

        // === VALIDATION HELPER METHODS ===

        /// <summary>
        /// Kiểm tra họ tên hợp lệ - chỉ chứa chữ cái (bao gồm tiếng Việt) và khoảng trắng
        /// </summary>
        private bool IsValidName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return false;
            // Cho phép chữ cái Unicode (bao gồm tiếng Việt), khoảng trắng và dấu
            return Regex.IsMatch(name.Trim(), @"^[\p{L}\s]+$");
        }

        /// <summary>
        /// Kiểm tra CCCD hợp lệ - chỉ chứa số và đúng 12 ký tự
        /// </summary>
        private bool IsValidCCCD(string cccd)
        {
            if (string.IsNullOrWhiteSpace(cccd)) return false;
            string cleaned = cccd.Trim();
            // CCCD Việt Nam có 12 số
            return Regex.IsMatch(cleaned, @"^\d{12}$");
        }

        /// <summary>
        /// Kiểm tra số điện thoại hợp lệ - chỉ chứa số, dấu + và khoảng trắng
        /// </summary>
        private bool IsValidPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone)) return false;
            // Cho phép số, dấu +, khoảng trắng và dấu gạch ngang
            return Regex.IsMatch(phone.Trim(), @"^[\d\s\+\-]+$");
        }

        /// <summary>
        /// Kiểm tra email hợp lệ theo chuẩn RFC 5322
        /// </summary>
        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            // Pattern email chuẩn
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email.Trim(), pattern, RegexOptions.IgnoreCase);
        }
    }
}
