using System;
using System.Configuration;
using System.Windows.Forms;
using QLChuoiNhaHangKhachSan.BLL.Services;

namespace QLChuoiNhaHangKhachSan.GUI
{
    public partial class RegisterForm : Form
    {
        private readonly LoginService _loginService;

        public RegisterForm()
        {
            InitializeComponent();
            
            var connStr = ConfigurationManager.ConnectionStrings["ConnStr"]?.ConnectionString;
            _loginService = new LoginService(connStr);
            
            // Đặt PasswordChar mặc định
            txtRegister_Password.PasswordChar = '*';
            txtRegister_ConfirmPassword.PasswordChar = '*';
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                string username = txtRegister_UserName.Text.Trim();
                string password = txtRegister_Password.Text;
                string confirmPassword = txtRegister_ConfirmPassword.Text;

                // Validate input
                if (string.IsNullOrWhiteSpace(username))
                {
                    MessageBox.Show("Vui lòng nhập tên đăng nhập", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtRegister_UserName.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(password))
                {
                    MessageBox.Show("Vui lòng nhập mật khẩu", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtRegister_Password.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(confirmPassword))
                {
                    MessageBox.Show("Vui lòng xác nhận mật khẩu", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtRegister_ConfirmPassword.Focus();
                    return;
                }

                // Thực hiện đăng ký
                _loginService.Register(username, password, confirmPassword);

                MessageBox.Show("Đăng ký thành công!\nBạn có thể đăng nhập ngay bây giờ.", 
                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Chuyển về trang đăng nhập
                LoginForm loginForm = new LoginForm();
                loginForm.Show();
                this.Close();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Lỗi nhập liệu", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi không xác định: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void clbLoginClose_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn thoát ứng dụng?", "Xác nhận thoát", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void lblRegister_Click(object sender, EventArgs e)
        {
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
            this.Hide();
        }

        private void ckbRegister_ShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            bool show = ckbRegister_ShowPassword.Checked;
            ckbRegister_ShowPassword.Text = show ? "Hide password" : "Show password";
            txtRegister_Password.PasswordChar = show ? '\0' : '*';
            txtRegister_ConfirmPassword.PasswordChar = show ? '\0' : '*';
        }
    }
}
