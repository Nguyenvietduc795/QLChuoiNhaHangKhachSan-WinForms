using QLChuoiNhaHangKhachSan.GUI; // n?u ch?a có

public partial class MainForm : Form
{
    private void btnCustomers_Click(object sender, EventArgs e)
    {
        CustomersList f = new CustomersList(); // dùng ?úng tên form
        f.Show();
    }
}