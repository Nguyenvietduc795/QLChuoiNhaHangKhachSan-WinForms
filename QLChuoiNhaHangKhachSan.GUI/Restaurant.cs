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
    public partial class frmRestaurant : Form
    {
        private Button currentSelectedTable = null;
        public frmRestaurant()
        {
            InitializeComponent();
        }
        // Định nghĩa 3 trạng thái của bàn
        public enum TableStatus
        {
            Trong,      // Không có khách (Màu nâu)
            CoKhach,    // Có khách (Màu xanh)
            DaDat       // Đã đặt (Màu hồng)
        }
        private void UpdateButtonState(TableStatus status)
        {
            // Dùng đúng tên nút như trong Designer
            switch (status)
            {
                case TableStatus.Trong:
                    SetButtonState(btnTableChoose, true);
                    SetButtonState(btnOrder, true);
                    SetButtonState(btnPay, false);
                    SetButtonState(btnCancel, false); // <--- Bàn trống thì không được hủy
                    break;
                case TableStatus.CoKhach:
                    SetButtonState(btnTableChoose, false);
                    SetButtonState(btnOrder, true);
                    SetButtonState(btnPay, true);
                    SetButtonState(btnCancel, false); // <--- Đang ăn thì không được hủy (phải thanh toán)
                    break;
                case TableStatus.DaDat:
                    SetButtonState(btnTableChoose, false);
                    SetButtonState(btnOrder, true);
                    SetButtonState(btnPay, false);
                    SetButtonState(btnCancel, true);  // <--- Chỉ bật nút Hủy khi bàn Đã Đặt
                    break;
            }
        }
        // Hàm này dùng để reset trạng thái khi chưa chọn bàn nào (hoặc bỏ chọn)
        private void ResetButtonState()
        {
            SetButtonState(btnTableChoose, false); // Nút Đặt bàn
            SetButtonState(btnCancel, false);      // Nút Hủy đặt
            SetButtonState(btnOrder, false);       // Nút Gọi món
            SetButtonState(btnPay, false);         // Nút Thanh toán

            // Xóa tên bàn hiển thị trên TextBox (nếu có)
            txtTableNumber.Text = "";
        }

        // Sửa lại hàm SetButtonState để dùng cho Guna2Button
        private void SetButtonState(Control btn, bool enable)
        {
            btn.Enabled = enable;
            if (enable)
            {
                if (btn.Name == "btnTableChoose") btn.BackColor = Color.Salmon;
                else if (btn.Name == "btnOrder") btn.BackColor = Color.ForestGreen;
                else if (btn.Name == "btnPay") btn.BackColor = Color.Blue;
                else if (btn.Name == "btnCancel") btn.BackColor = Color.Red; // <--- Thêm dòng này
            }
            else
            {
                btn.BackColor = Color.Gray;
            }
        }
        private void Form3_Load(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void guna2HtmlLabel2_Click(object sender, EventArgs e)
        {

        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pnlSidebar_Paint(object sender, PaintEventArgs e)
        {

        }
        private void Table_Click(object sender, EventArgs e)
        {
            if (currentSelectedTable != null)
            {
                // Đặt độ dày viền về 0 để làm mất viền đen
                currentSelectedTable.FlatAppearance.BorderSize = 0;
            }
            // Lấy cái nút vừa được bấm (sender) ép kiểu về Button
            Button btn = (Button)sender;

            // Lưu nút đó vào biến nhớ
            currentSelectedTable = btn;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderColor = Color.Black; 
            btn.FlatAppearance.BorderSize = 3;
            txtTableNumber.Text = btn.Text;
            // Kiểm tra Tag của bàn để biết trạng thái. Nếu Tag null thì mặc định là Trống.
            TableStatus status = TableStatus.Trong;
            if (btn.Tag != null)
            {
                status = (TableStatus)btn.Tag;
            }

            // Cập nhật các nút chức năng theo trạng thái
            UpdateButtonState(status);
        }
        private void btnGoiMon_Click(object sender, EventArgs e)
        {

        }
        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            if (currentSelectedTable == null)
            {
                MessageBox.Show("Vui lòng chọn bàn cần thanh toán ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Logic: Có Khách -> Thanh Toán -> Trở về Bàn Trống
            DialogResult result = MessageBox.Show("Xác nhận thanh toán?", "Thông báo", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                currentSelectedTable.BackColor = SystemColors.Control; // Màu nâu (Bàn trống - Chỉnh theo màu gốc của bạn)
                currentSelectedTable.Tag = TableStatus.Trong;       // Lưu trạng thái
                frmBill billform=new frmBill();
                billform.ShowDialog();
                MessageBox.Show("Thanh toán thành công. Bàn đã trống.", "Thông báo");

                // Cập nhật lại trạng thái nút
                UpdateButtonState(TableStatus.Trong);
            }
        }


        private void btnTableChoose_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem người dùng đã chọn bàn nào chưa
            if (currentSelectedTable == null)
            {
                MessageBox.Show("Vui lòng chọn một bàn trước khi đặt bàn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Nếu đã chọn rồi thì đổi màu nền sang màu Xanh (Có khách)
            currentSelectedTable.BackColor = Color.FromArgb(255, 128, 128); // Mã màu xanh Emerald hoặc dùng Color.LimeGreen

            // Đổi màu chữ nếu cần để dễ đọc
            currentSelectedTable.ForeColor = Color.White;

            // Thông báo
            MessageBox.Show("Đã thiết lập trạng thái 'Đã đặt' cho bàn " + currentSelectedTable.Text, "Thành công");

            // Hiện form Menu1 (frmMenu)
            frmMenu Menu = new frmMenu();
            Menu.ShowDialog();

            // Reset biến chọn (để tránh bấm nhầm lần sau)
            currentSelectedTable = null;
        }
        private void frmRestaurant_Load(object sender, EventArgs e)
        {
            SetButtonState(btnTableChoose, false);
            SetButtonState(btnOrder, false);
            SetButtonState(btnPay, false);
            SetButtonState(btnCancel, false); // <--- Thêm dòng này

            // Gán trạng thái ban đầu cho tất cả các bàn là Trống
            foreach (Control c in this.Controls)
            {
                if (c is Button && c.Name.StartsWith("btnTable"))
                {
                    c.Tag = TableStatus.Trong;
                    c.BackColor = Color.SaddleBrown; // Màu bàn trống
                }
            }
            // Gọi hàm reset để khóa tất cả các nút ngay từ đầu
            ResetButtonState();

            // Gán trạng thái ban đầu cho tất cả các bàn là Trống
            foreach (Control c in this.Controls)
            {
                if (c is Button && c.Name.StartsWith("btnTable"))
                {
                    c.Tag = TableStatus.Trong;
                    c.BackColor = Color.SaddleBrown; // Màu bàn trống
                }
            }
        }

        private void txtTableNumber_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra xem đã chọn bàn chưa
            if (currentSelectedTable == null)
            {
                MessageBox.Show("Vui lòng chọn bàn cần hủy!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Lấy trạng thái hiện tại của bàn từ Tag
            TableStatus status = TableStatus.Trong; // Mặc định
            if (currentSelectedTable.Tag != null)
            {
                status = (TableStatus)currentSelectedTable.Tag;
            }

            // 3. Kiểm tra logic: Chỉ cho hủy khi bàn ở trạng thái "Đã Đặt"
            if (status == TableStatus.DaDat)
            {
                DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn hủy đặt bàn số " + currentSelectedTable.Text + " không?",
                                                      "Xác nhận hủy",
                                                      MessageBoxButtons.YesNo,
                                                      MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Cập nhật lại trạng thái về TRỐNG
                    currentSelectedTable.Tag = TableStatus.Trong;
                    currentSelectedTable.BackColor = SystemColors.Control; // Trả về màu bàn trống gốc

                    // Cập nhật lại trạng thái các nút chức năng
                    UpdateButtonState(TableStatus.Trong);

                    MessageBox.Show("Đã hủy đặt bàn thành công!", "Thông báo");
                }
            }
            else
            {
                // Nếu bàn đang Trống hoặc Đang có khách ăn
                MessageBox.Show("Chỉ có thể hủy bàn ở trạng thái 'Đã đặt'!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        // Sự kiện Click vào nền Form (để bỏ chọn bàn)
        private void frmRestaurant_Click(object sender, EventArgs e)
        {
            // 1. Nếu đang có bàn được chọn thì xóa viền của nó đi
            if (currentSelectedTable != null)
            {
                currentSelectedTable.FlatAppearance.BorderSize = 0;
            }

            // 2. Gán biến nhớ về null (không chọn bàn nào cả)
            currentSelectedTable = null;

            // 3. Khóa toàn bộ 4 nút chức năng
            ResetButtonState();
        }
    }
}
