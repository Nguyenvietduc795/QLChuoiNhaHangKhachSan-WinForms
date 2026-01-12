using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace QLChuoiNhaHangKhachSan.GUI
{
    public partial class CustomerAdd : Form
    {
        // Các property để form ngoài lấy dữ liệu sau khi DialogResult == OK
        public string CustomerId    => guna2TextBox1.Text.Trim();
        public string CustomerName  => guna2TextBox2.Text.Trim();
        public string Phone         => guna2TextBox3.Text.Trim();
        public string Address       => guna2TextBox4.Text.Trim();
        public string Email         => guna2TextBox5.Text.Trim();

        public CustomerAdd()
        {
            InitializeComponent();
            WireEvents();
        }

        private void WireEvents()
        {
            // Thoát form
            guna2Button2.Click += (s, e) => Close();
        }

        private void CustomerAdd_Load(object sender, EventArgs e)
        {
            // Có thể tự động sinh mã khách hàng ở đây nếu cần
            // guna2TextBox1.Text = TaoMaKhachHang();
        }

        private void guna2HtmlLabel3_Click(object sender, EventArgs e)
        {
            // Không cần xử lý
        }

        // Chỉ cho phép nhập số cho SĐT
        private void guna2TextBox3_TextChanged(object sender, EventArgs e)
        {
            var tb = (Guna.UI2.WinForms.Guna2TextBox)sender;
            if (!Regex.IsMatch(tb.Text, @"^\d*$"))
            {
                int selStart = tb.SelectionStart - 1;
                tb.Text = Regex.Replace(tb.Text, @"\D", "");
                if (selStart < 0) selStart = 0;
                if (selStart > tb.Text.Length) selStart = tb.Text.Length;
                tb.SelectionStart = selStart;
            }
        }

        // Nút "Thêm"
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs())
                return;

            // TODO: Gọi BUS/DAL để lưu vào DB nếu cần

            MessageBox.Show("Thêm khách hàng thành công!", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            DialogResult = DialogResult.OK;
            Close();
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(guna2TextBox1.Text))
            {
                ShowError("Vui lòng nhập mã khách hàng.");
                guna2TextBox1.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(guna2TextBox2.Text))
            {
                ShowError("Vui lòng nhập tên khách hàng.");
                guna2TextBox2.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(guna2TextBox3.Text))
            {
                ShowError("Vui lòng nhập số điện thoại.");
                guna2TextBox3.Focus();
                return false;
            }

            if (!Regex.IsMatch(guna2TextBox3.Text.Trim(), @"^\d{9,11}$"))
            {
                ShowError("Số điện thoại không hợp lệ (9-11 chữ số).");
                guna2TextBox3.Focus();
                return false;
            }

            if (!string.IsNullOrWhiteSpace(guna2TextBox5.Text) &&
                !Regex.IsMatch(guna2TextBox5.Text.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                ShowError("Email không hợp lệ.");
                guna2TextBox5.Focus();
                return false;
            }

            return true;
        }

        private void ShowError(string message)
        {
            MessageBox.Show(message, "Lỗi",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}