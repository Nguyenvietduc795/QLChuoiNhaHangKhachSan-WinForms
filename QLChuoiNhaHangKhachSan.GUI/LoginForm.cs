using System;
using System.Configuration;
using System.Windows.Forms;
using QLChuoiNhaHangKhachSan.BLL.Services;
using QLChuoiNhaHangKhachSan.DAL.Models;

namespace QLChuoiNhaHangKhachSan.GUI
{
    public partial class LoginForm : Form
    {
        private readonly LoginService _loginService;
        private bool _loginSuccess = false; // Flag để theo dõi trạng thái đăng nhập

        public LoginForm()
        {
            InitializeComponent();
            
            var connStr = ConfigurationManager.ConnectionStrings["ConnStr"]?.ConnectionString;
            _loginService = new LoginService(connStr);
            
            // Đặt PasswordChar mặc định
            txtLogin_Password.PasswordChar = '*';
        }

        private void ptbLoginAvt_Click(object sender, EventArgs e)
        {
            // Avatar click handler
        }

        private void clbLoginClose_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn thoát ứng dụng?", "Xác nhận thoát", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string username = txtLogin_UserName.Text.Trim();
                string password = txtLogin_Password.Text;

                // Validate input
                if (string.IsNullOrWhiteSpace(username))
                {
                    MessageBox.Show("Vui lòng nhập tên đăng nhập", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtLogin_UserName.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(password))
                {
                    MessageBox.Show("Vui lòng nhập mật khẩu", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtLogin_Password.Focus();
                    return;
                }

                // Thực hiện đăng nhập
                Login user = _loginService.Login(username, password);

                if (user != null)
                {
                    // Kiểm tra xem có cần đổi mật khẩu lần đầu không
                    if (user.MustChangePassword)
                    {
                        MessageBox.Show("Đây là lần đăng nhập đầu tiên của bạn.\nVui lòng đổi mật khẩu để tiếp tục.",
                            "Yêu cầu đổi mật khẩu", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        using (var changePasswordForm = new ChangePasswordForm(username, isFirstLogin: true))
                        {
                            if (changePasswordForm.ShowDialog() != DialogResult.OK)
                            {
                                // Nếu không đổi mật khẩu thành công, không cho đăng nhập
                                return;
                            }
                        }

                        MessageBox.Show("Đổi mật khẩu thành công!\nVui lòng đăng nhập lại với mật khẩu mới.",
                            "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Xóa password field để user nhập lại
                        txtLogin_Password.Text = "";
                        txtLogin_Password.Focus();
                        return;
                    }

                    MessageBox.Show($"Đăng nhập thành công!\nChào mừng {user.Username}", 
                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Đánh dấu đăng nhập thành công
                    _loginSuccess = true;

                    // Mở FormDashBoard và đóng form login
                    FormDashBoard dashboard = new FormDashBoard();
                    dashboard.FormClosed += (s, args) => Application.Exit();
                    dashboard.Show();
                    this.Hide();
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Lỗi nhập liệu", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Đăng nhập thất bại", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi không xác định: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lblRegister_Click(object sender, EventArgs e)
        {
            RegisterForm registerForm = new RegisterForm();
            registerForm.Show();
            this.Hide();
        }

        private void ckbLogin_ShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            bool show = ckbLogin_ShowPassword.Checked;
            ckbLogin_ShowPassword.Text = show ? "Hide password" : "Show password";
            txtLogin_Password.PasswordChar = show ? '\0' : '*';
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            // Chỉ đóng ứng dụng khi form login đóng mà chưa đăng nhập thành công
            if (!_loginSuccess)
            {
                Application.Exit();
            }
            base.OnFormClosed(e);
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }
    }
}
