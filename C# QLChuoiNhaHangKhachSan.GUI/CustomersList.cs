using System;
using System.Windows.Forms;

namespace QLChuoiNhaHangKhachSan.GUI
{
    public partial class CustomersList : Form
    {
        public CustomersList()
        {
            InitializeComponent();
        }

        private void CustomersList_Load(object sender, EventArgs e)
        {
            // Nếu trước đây bạn có code gì ở Load (vd: tính tổng KH từ DB) thì đặt lại ở đây.
            // Hiện tại để trống để không đụng vào dữ liệu thiết kế sẵn.
        }

        // Nút "Thêm khách hàng"
        private void bntAddCustomers_Click(object sender, EventArgs e)
        {
            // Tạm thời không làm gì, chỉ để form chạy y như ban đầu
            // Khi bạn muốn viết lại chức năng thêm, mình sẽ làm lại từng bước,
            // nhưng có tính đến dữ liệu đang thiết kế/đọc từ DB, không xoá DataSource hiện tại.
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            // xử lý tìm kiếm (để trống)
        }

        private void tbFindCustomers_TextChanged(object sender, EventArgs e)
        {
            // xử lý lọc theo text (để trống)
        }

        private void lblVIPCount_Click(object sender, EventArgs e)
        {
        }

        private void lblVIPCustomers_Click(object sender, EventArgs e)
        {
        }

        private void lblTotalCustomerCount_Click(object sender, EventArgs e)
        {
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
        }
    }
}