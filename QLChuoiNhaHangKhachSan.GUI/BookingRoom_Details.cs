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

        // Khai báo bảng tạm toàn cục để quản lý dữ liệu
        DataTable dtTrong = new DataTable();
        DataTable dtChon = new DataTable();

        private List<string> _preselectedRooms;
        private HashSet<string> _unavailableRooms = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private ContextMenuStrip _menuTrong;
        private ContextMenuStrip _menuChon;
        private NumericUpDown _qtyEditor;
        private string _editingRoomCode;

        public BookingRoom_Details()
        {
            InitializeComponent();
            // Bật DoubleBuffered để giao diện mượt hơn
            this.DoubleBuffered = true;
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

        // Allow caller to pass rooms that should not appear in available list
        public void SetUnavailableRooms(IEnumerable<string> codes)
        {
            _unavailableRooms.Clear();
            if (codes == null) return;
            foreach (var c in codes)
                _unavailableRooms.Add(c.Trim().ToUpper());
        }

        private void BookingRoom_Details_Load(object sender, EventArgs e)
        {
            // 1. Thiết lập dữ liệu mẫu cho bảng Phòng Trống
            dtTrong = new DataTable();
            dtTrong.Columns.Add("maphong"); // Khớp với Name trong Design
            dtTrong.Columns.Add("LoaiPhong");

            // create P001..P030 mapping with types to match ListRoom grouping
            for (int i = 1; i <= 30; i++)
            {
                string code = "P" + i.ToString("D3");
                if (_unavailableRooms.Contains(code))
                    continue; // skip unavailable

                string loai;
                if (i <= 12) loai = "Phòng Đơn";
                else if (i <= 20) loai = "Phòng Đôi";
                else loai = "Phòng Gia Đình";
                dtTrong.Rows.Add(code, loai);
            }

            // 2. Thiết lập cấu trúc cho bảng Phòng Chọn (thêm cột Soluong)
            dtChon = new DataTable();
            dtChon.Columns.Add("maphong");
            dtChon.Columns.Add("NgayKT");
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
        }

        private void PopulateListViewsFromDataTables()
        {
            lvPhongTrong.Items.Clear();
            foreach (DataRow r in dtTrong.Rows.Cast<DataRow>().OrderBy(r => r["maphong"].ToString()))
            {
                var item = new ListViewItem(r["maphong"].ToString());
                item.SubItems.Add(r["LoaiPhong"].ToString());
                item.Tag = r["LoaiPhong"].ToString();
                lvPhongTrong.Items.Add(item);
            }

            lvPhongChon.Items.Clear();
            foreach (DataRow r in dtChon.Rows.Cast<DataRow>().OrderBy(r => r["maphong"].ToString()))
            {
                var item = new ListViewItem(r["maphong"].ToString());
                // For chosen list, we show columns: mã, số người, ngàyBD, ngàyKT
                item.SubItems.Add(r.Table.Columns.Contains("Soluong") ? r["Soluong"].ToString() : "1");
                item.SubItems.Add(DateTime.Now.ToShortDateString());
                item.SubItems.Add(r["NgayKT"].ToString());
                lvPhongChon.Items.Add(item);
            }
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
            string code = sel.Text;

            // remove from dtChon and add back to dtTrong
            var row = dtChon.Rows.Cast<DataRow>().FirstOrDefault(r => string.Equals(r["maphong"].ToString(), code, StringComparison.OrdinalIgnoreCase));
            if (row != null) dtChon.Rows.Remove(row);

            string type = InferRoomType(code);
            dtTrong.Rows.Add(code, type);

            PopulateListViewsFromDataTables();
        }

        private string InferRoomType(string code)
        {
            int num = int.Parse(code.Substring(1));
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

            string ngayGioKT = dtpNgayKetThuc.Value.ToShortDateString() + " " + dtpGioKetThuc.Value.ToShortTimeString();

            dtChon.Rows.Add(maP, ngayGioKT, 1);

            var row = dtTrong.Rows.Cast<DataRow>().FirstOrDefault(r => string.Equals(r["maphong"].ToString(), maP, StringComparison.OrdinalIgnoreCase));
            if (row != null) dtTrong.Rows.Remove(row);

            PopulateListViewsFromDataTables();
        }

        // Allow editing quantity or removing from chosen list via double-click
        private void LvPhongChon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lvPhongChon.SelectedItems.Count == 0) return;
            var sel = lvPhongChon.SelectedItems[0];
            string code = sel.Text;

            var result = MessageBox.Show("Chọn Yes để xóa phòng, No để chỉnh số người, Cancel để hủy", "Hành động", MessageBoxButtons.YesNoCancel);
            if (result == DialogResult.Yes)
            {
                // remove from dtChon and add back to dtTrong
                var row = dtChon.Rows.Cast<DataRow>().FirstOrDefault(r => string.Equals(r["maphong"].ToString(), code, StringComparison.OrdinalIgnoreCase));
                if (row != null) dtChon.Rows.Remove(row);

                // add back to dtTrong with default type (infer from code)
                string type = "Phòng Đơn";
                int num = int.Parse(code.Substring(1));
                if (num <= 12) type = "Phòng Đơn";
                else if (num <= 20) type = "Phòng Đôi";
                else type = "Phòng Gia Đình";
                dtTrong.Rows.Add(code, type);

                PopulateListViewsFromDataTables();
            }
            else if (result == DialogResult.No)
            {
                // edit quantity
                string old = sel.SubItems.Count > 1 ? sel.SubItems[1].Text : "1";
                string input = PromptForString("Sửa số người", "Nhập số người mới", old);
                if (int.TryParse(input, out int q) && q > 0)
                {
                    var row = dtChon.Rows.Cast<DataRow>().FirstOrDefault(r => string.Equals(r["maphong"].ToString(), code, StringComparison.OrdinalIgnoreCase));
                    if (row != null)
                    {
                        row["Soluong"] = q;
                        PopulateListViewsFromDataTables();
                    }
                }
                else
                {
                    MessageBox.Show("Số nhập không hợp lệ");
                }
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
            this.DialogResult = DialogResult.Cancel;
            this.Close(); // Đóng Form
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
                ResultDateRange = dtpNgayBatDau.Value.ToShortDateString() + " " +
                                  dtpGioBatDau.Value.ToShortTimeString() + " - " +
                                  dtpNgayKetThuc.Value.ToShortDateString() + " " +
                                  dtpGioKetThuc.Value.ToShortTimeString();

                var start = dtpNgayBatDau.Value.Date + dtpGioBatDau.Value.TimeOfDay;
                var end   = dtpNgayKetThuc.Value.Date + dtpGioKetThuc.Value.TimeOfDay;
                var rooms = dtChon.Rows.Cast<DataRow>()
                                       .Select(r => r["maphong"].ToString())
                                       .ToList();

                // === KIỂM TRA TRÙNG LỊCH ===
                foreach (var room in rooms)
                {
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

                var info = new BookingInfo { Customer = ResultCustomerName, Start = start, End = end };
                BookingManager.AddBooking(rooms, info);

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
    }
}