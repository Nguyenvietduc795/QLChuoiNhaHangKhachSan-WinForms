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
    public partial class AddVipCustomers : Form
    {
        private readonly VipCustomers _parentForm;
        private bool _isEditMode;
        private int _editRowIndex = -1;

        public AddVipCustomers() : this(null)
        {
        }

        public AddVipCustomers(VipCustomers parent)
        {
            InitializeComponent();
            _parentForm = parent;
            // Khi người dùng chọn ngày bắt đầu, tự động set ngày hết hạn = ngày đó + 1 năm
            dtpDateStart.ValueChanged += DtpDateStart_ValueChanged;
        }

        // Dùng cho chế độ cập nhật: nạp dữ liệu vào form
        public void SetEditData(string id, string rank, string point, DateTime dateStart, DateTime dateEnd, int rowIndex)
        {
            _isEditMode = true;
            _editRowIndex = rowIndex;

            txbIDVIP.Text = id;
            txbIDVIP.ReadOnly = true; // Không cho sửa mã thành viên

            cboRankVIP.SelectedItem = rank;
            txbPointVIP.Text = point;
            dtpDateStart.Value = dateStart;
            dtpDateEnd.Value = dateEnd;

            // Đổi tiêu đề và text nút lưu khi cập nhật
            this.Text = "Cập nhật thành viên VIP";
            guna2HtmlLabel1.Text = "Cập nhật Thành Viên";
            btnSaveVIP.Text = "Cập nhật";
        }

        private void DtpDateStart_ValueChanged(object sender, EventArgs e)
        {
            // Giữ nguyên ngày & tháng, cộng thêm 1 năm
            var start = dtpDateStart.Value.Date;
            try
            {
                dtpDateEnd.Value = start.AddYears(1);
            }
            catch
            {
                // Trường hợp đặc biệt như 29/02 => lùi về 28/02 năm sau
                dtpDateEnd.Value = new DateTime(
                    start.Year + 1,
                    start.Month,
                    DateTime.DaysInMonth(start.Year + 1, start.Month));
            }
        }

        private void btnSaveVIP_Click(object sender, EventArgs e)
        {
            var id = txbIDVIP.Text.Trim();
            var rank = cboRankVIP.SelectedItem != null ? cboRankVIP.SelectedItem.ToString() : string.Empty;
            var point = txbPointVIP.Text.Trim();
            var dateStart = dtpDateStart.Value.Date;
            var dateEnd = dtpDateEnd.Value.Date;

            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(rank) || string.IsNullOrEmpty(point))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_parentForm != null)
            {
                if (_isEditMode)
                {
                    // Cập nhật lại dòng cũ (không đổi mã thành viên)
                    _parentForm.UpdateVipRow(_editRowIndex, rank, point, dateStart, dateEnd);
                }
                else
                {
                    // Thêm mới
                    _parentForm.AddVipRow(id, rank, point, dateStart, dateEnd);
                }
            }

            this.Close();
        }

        private void btnCancelVIP_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
