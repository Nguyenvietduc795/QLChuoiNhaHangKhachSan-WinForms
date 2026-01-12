using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace QLChuoiNhaHangKhachSan.GUI
{
    public partial class BookingRoom_Details : Form
    {
        // Public properties for callers
        public string ResultCustomerName { get; private set; }
        public string ResultCCCD { get; private set; }
        public string ResultSDT { get; private set; }
        public string ResultRooms { get; private set; }
        public string ResultDateRange { get; private set; }
        public string ResultGender { get; private set; }
        public string ResultNationality { get; private set; }
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

        public BookingRoom_Details()
        {
            InitializeComponent();
            // Bật DoubleBuffered để giao diện mượt hơn
            this.DoubleBuffered = true;
            ApplyModernTheme();
             this.Load += BookingRoom_Details_Load;

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
            using (var clock = new frmTime(picker.Value))
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

             // 1. Thiết lập dữ liệu nguồn cho bảng Phòng Trống từ SetAvailableRooms
             dtTrong = new DataTable();
             dtTrong.Columns.Add("maphong"); // Khớp với Name trong Design
             dtTrong.Columns.Add("LoaiPhong");

            foreach (var room in _roomsFromSource)
            {
                if (_unavailableRooms.Contains(room.Code)) continue; // skip unavailable
                dtTrong.Rows.Add(room.Code, string.IsNullOrWhiteSpace(room.Type) ? "" : room.Type);
            }

            // 2. Thiết lập cấu trúc cho bảng Phòng Chọn (thêm cột Soluong)
            dtChon = new DataTable();
            dtChon.Columns.Add("maphong");
            dtChon.Columns.Add("NgayBD", typeof(DateTime));
            dtChon.Columns.Add("NgayKT", typeof(DateTime));
            dtChon.Columns.Add("Soluong", typeof(int));

            // Populate ListViews from DataTables
            PopulateListViewsFromDataTables();

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
            if (dtChon.Rows.Count > 0)
            {
                ResultCustomerName = txtHoTen.Text;
                ResultCCCD = txtCCCD.Text;
                ResultSDT = txtSDT.Text;
                ResultGender = cboGioiTinh != null ? cboGioiTinh.Text : string.Empty;
                ResultNationality = txtQuocTich != null ? txtQuocTich.Text : string.Empty;
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
 
                 MessageBox.Show("Đã lưu thông tin đặt phòng cho khách hàng: " + txtHoTen.Text);
 
                // lưu từng phòng với start/end riêng
                foreach (DataRow r in dtChon.Rows)
                {
                    var room = r["maphong"].ToString();
                    var start = r["NgayBD"] != DBNull.Value ? (DateTime)r["NgayBD"] : ResultStartDate;
                    var end = r["NgayKT"] != DBNull.Value ? (DateTime)r["NgayKT"] : ResultEndDate;
                    var info = new BookingInfo { Customer = ResultCustomerName, Start = start, End = end };
                    BookingManager.AddBooking(room, info);
                }

                try
                {
                    var listForm = Application.OpenForms.OfType<ListRoom>().FirstOrDefault();
                    if (listForm != null)
                    {
                        listForm.MarkRoomsAsBooked(rooms, ResultCustomerName);
                    }
                }
                catch { }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn ít nhất một phòng!");
            }
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
                txtHoTen.BorderThickness = txtCCCD.BorderThickness = txtSDT.BorderThickness = txtDiaChi.BorderThickness = txtQuocTich.BorderThickness = 1;
                txtHoTen.BorderColor = txtCCCD.BorderColor = txtSDT.BorderColor = txtDiaChi.BorderColor = txtQuocTich.BorderColor = Color.FromArgb(210, 220, 230);
                txtHoTen.FillColor = txtCCCD.FillColor = txtSDT.FillColor = txtDiaChi.FillColor = txtQuocTich.FillColor = Color.FromArgb(248, 250, 252);
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
     }
 }