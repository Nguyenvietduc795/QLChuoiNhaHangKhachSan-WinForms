using System;
using System.Windows.Forms;

namespace QLChuoiNhaHangKhachSan.GUI
{
    public partial class CustomerAdd : Form
    {
        public enum CustomerFormMode
        {
            Add,
            Edit
        }

        public CustomerFormMode Mode { get; set; } = CustomerFormMode.Add;

        // Thuộc tính public để đọc/ghi dữ liệu
        public string CustomerId
        {
            get => TxbCustomersID.Text.Trim();
            set => TxbCustomersID.Text = value;
        }

        public string CustomerName
        {
            get => TxbCustomersName.Text.Trim();
            set => TxbCustomersName.Text = value;
        }

        public string PhoneNumber
        {
            get => TxbCustomersNumberPhone.Text.Trim();
            set => TxbCustomersNumberPhone.Text = value;
        }

        public string Address
        {
            get => TxbAddress.Text.Trim();
            set => TxbAddress.Text = value;
        }

        public string CustomerType
        {
            get => cboCustomerType.Text.Trim();
            set => cboCustomerType.Text = value;
        }

        public CustomerAdd()
        {
            InitializeComponent();
        }

        private void CustomerAdd_Load(object sender, EventArgs e)
        {
            if (Mode == CustomerFormMode.Edit)
            {
                // Nếu là sửa, không cho sửa mã khách hàng
                TxbCustomersID.ReadOnly = true;
                guna2HtmlLabel1.Text = "Cập nhật khách hàng";
                guna2Button1.Text = "Lưu";
            }
            else
            {
                TxbCustomersID.ReadOnly = false;
                guna2HtmlLabel1.Text = "Thêm khách hàng mới";
                guna2Button1.Text = "Thêm";
            }
        }

        // Nút Thêm / Lưu (guna2Button1)
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            // Có thể thêm validate ở đây nếu cần

            DialogResult = DialogResult.OK;
            Close();
        }

        // Nút Thoát (guna2Button2)
        private void guna2Button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        // Các handler cũ (để Designer không lỗi)
        private void guna2HtmlLabel3_Click(object sender, EventArgs e)
        {
        }

        private void guna2TextBox3_TextChanged(object sender, EventArgs e)
        {
        }
    }
}
