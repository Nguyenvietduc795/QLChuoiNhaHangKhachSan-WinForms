using System;
using System.Configuration;
using System.Drawing;
using System.Windows.Forms;
using QLChuoiNhaHangKhachSan.BLL.Services;

namespace QLChuoiNhaHangKhachSan.GUI
{
    public partial class ChangePasswordForm : Form
    {
        private readonly LoginService _loginService;
        private readonly string _username;
        private readonly bool _isFirstLogin;

        /// <summary>
        /// Form ??i m?t kh?u
        /// </summary>
        /// <param name="username">Tên ??ng nh?p</param>
        /// <param name="isFirstLogin">Có ph?i ??ng nh?p l?n ??u không (n?u true, b?t bu?c ph?i ??i)</param>
        public ChangePasswordForm(string username, bool isFirstLogin = false)
        {
            InitializeComponent();

            var connStr = ConfigurationManager.ConnectionStrings["ConnStr"]?.ConnectionString;
            _loginService = new LoginService(connStr);
            _username = username;
            _isFirstLogin = isFirstLogin;

            // N?u là l?n ??u ??ng nh?p, không cho phép ?óng form mà không ??i m?t kh?u
            if (_isFirstLogin)
            {
                this.ControlBox = false;
                lblTitle.Text = "Đổi mật khẩu lần đầu";
                lblDescription.Text = "Đây là lần đầu tiên đăng nhập của bạn.\nVui lòng đổi mật khẩu để tiếp tục.";
            }
        }

        private void InitializeComponent()
        {
            this.lblTitle = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lblDescription = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lblNewPassword = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.txtNewPassword = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblConfirmPassword = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.txtConfirmPassword = new Guna.UI2.WinForms.Guna2TextBox();
            this.btnChangePassword = new Guna.UI2.WinForms.Guna2Button();
            this.btnCancel = new Guna.UI2.WinForms.Guna2Button();
            this.ckbShowPassword = new Guna.UI2.WinForms.Guna2CheckBox();
            this.lblPasswordHint = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = false;
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.MidnightBlue;
            this.lblTitle.Location = new System.Drawing.Point(22, 21);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(156, 34);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Đổi mật khẩu";
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = false;
            this.lblDescription.BackColor = System.Drawing.Color.Transparent;
            this.lblDescription.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblDescription.ForeColor = System.Drawing.Color.Gray;
            this.lblDescription.Location = new System.Drawing.Point(23, 59);
            this.lblDescription.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(291, 19);
            this.lblDescription.TabIndex = 1;
            this.lblDescription.Text = "Nhập mật khẩu mới để bảo vệ tài khoản của bạn.";
            // 
            // lblNewPassword
            // 
            this.lblNewPassword.AutoSize = false;
            this.lblNewPassword.BackColor = System.Drawing.Color.Transparent;
            this.lblNewPassword.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblNewPassword.Location = new System.Drawing.Point(22, 122);
            this.lblNewPassword.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.lblNewPassword.Name = "lblNewPassword";
            this.lblNewPassword.Size = new System.Drawing.Size(83, 19);
            this.lblNewPassword.TabIndex = 2;
            this.lblNewPassword.Text = "Mật khẩu mới";
            // 
            // txtNewPassword
            // 
            this.txtNewPassword.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txtNewPassword.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtNewPassword.DefaultText = "";
            this.txtNewPassword.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtNewPassword.Location = new System.Drawing.Point(22, 146);
            this.txtNewPassword.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtNewPassword.Name = "txtNewPassword";
            this.txtNewPassword.PasswordChar = '*';
            this.txtNewPassword.PlaceholderText = "Nhập mật khẩu mới";
            this.txtNewPassword.SelectedText = "";
            this.txtNewPassword.Size = new System.Drawing.Size(292, 36);
            this.txtNewPassword.TabIndex = 0;
            // 
            // lblConfirmPassword
            // 
            this.lblConfirmPassword.AutoSize = false;
            this.lblConfirmPassword.BackColor = System.Drawing.Color.Transparent;
            this.lblConfirmPassword.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblConfirmPassword.Location = new System.Drawing.Point(22, 195);
            this.lblConfirmPassword.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.lblConfirmPassword.Name = "lblConfirmPassword";
            this.lblConfirmPassword.Size = new System.Drawing.Size(113, 19);
            this.lblConfirmPassword.TabIndex = 3;
            this.lblConfirmPassword.Text = "Xác nhận mật khẩu";
            // 
            // txtConfirmPassword
            // 
            this.txtConfirmPassword.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txtConfirmPassword.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtConfirmPassword.DefaultText = "";
            this.txtConfirmPassword.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtConfirmPassword.Location = new System.Drawing.Point(22, 219);
            this.txtConfirmPassword.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtConfirmPassword.Name = "txtConfirmPassword";
            this.txtConfirmPassword.PasswordChar = '*';
            this.txtConfirmPassword.PlaceholderText = "Nhập lại mật khẩu mới";
            this.txtConfirmPassword.SelectedText = "";
            this.txtConfirmPassword.Size = new System.Drawing.Size(292, 36);
            this.txtConfirmPassword.TabIndex = 1;
            // 
            // btnChangePassword
            // 
            this.btnChangePassword.BorderRadius = 5;
            this.btnChangePassword.FillColor = System.Drawing.Color.MidnightBlue;
            this.btnChangePassword.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnChangePassword.ForeColor = System.Drawing.Color.White;
            this.btnChangePassword.Location = new System.Drawing.Point(22, 395);
            this.btnChangePassword.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnChangePassword.Name = "btnChangePassword";
            this.btnChangePassword.Size = new System.Drawing.Size(135, 37);
            this.btnChangePassword.TabIndex = 3;
            this.btnChangePassword.Text = "Đổi mật khẩu";
            this.btnChangePassword.Click += new System.EventHandler(this.btnChangePassword_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BorderRadius = 5;
            this.btnCancel.FillColor = System.Drawing.Color.Gray;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(297, 395);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(135, 37);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Hủy";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // ckbShowPassword
            // 
            this.ckbShowPassword.CheckedState.BorderRadius = 0;
            this.ckbShowPassword.CheckedState.BorderThickness = 0;
            this.ckbShowPassword.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ckbShowPassword.Location = new System.Drawing.Point(22, 292);
            this.ckbShowPassword.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ckbShowPassword.Name = "ckbShowPassword";
            this.ckbShowPassword.Size = new System.Drawing.Size(147, 17);
            this.ckbShowPassword.TabIndex = 2;
            this.ckbShowPassword.Text = "Hiển thị mật khẩu";
            this.ckbShowPassword.UncheckedState.BorderRadius = 0;
            this.ckbShowPassword.UncheckedState.BorderThickness = 0;
            this.ckbShowPassword.CheckedChanged += new System.EventHandler(this.ckbShowPassword_CheckedChanged);
            // 
            // lblPasswordHint
            // 
            this.lblPasswordHint.AutoSize = false;
            this.lblPasswordHint.BackColor = System.Drawing.Color.Transparent;
            this.lblPasswordHint.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblPasswordHint.ForeColor = System.Drawing.Color.Gray;
            this.lblPasswordHint.Location = new System.Drawing.Point(22, 326);
            this.lblPasswordHint.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.lblPasswordHint.Name = "lblPasswordHint";
            this.lblPasswordHint.Size = new System.Drawing.Size(410, 15);
            this.lblPasswordHint.TabIndex = 4;
            this.lblPasswordHint.Text = "Mật khẩu mới phải có ít nhất 6 kí tự, bao gồm:\n1 chữ hoa, 1 chữ thường và 1 số";
            // 
            // ChangePasswordForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(462, 524);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.lblNewPassword);
            this.Controls.Add(this.txtNewPassword);
            this.Controls.Add(this.lblConfirmPassword);
            this.Controls.Add(this.txtConfirmPassword);
            this.Controls.Add(this.ckbShowPassword);
            this.Controls.Add(this.lblPasswordHint);
            this.Controls.Add(this.btnChangePassword);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ChangePasswordForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Đổi mật khẩu";
            this.ResumeLayout(false);

        }

        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            try
            {
                string newPassword = txtNewPassword.Text;
                string confirmPassword = txtConfirmPassword.Text;

                // ??i m?t kh?u
                _loginService.ChangePasswordFirstTime(_username, newPassword, confirmPassword);

                MessageBox.Show("Đổii mật khẩu thành công!\nVui lòng sử dụng mật khẩu mới để đăng nhập.",
                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Lỗi nhập liệu",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi không xác định: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (_isFirstLogin)
            {
                MessageBox.Show("Bạn phải thay đổi mật khẩu để tiếp tục sử dụng hệ thống.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void ckbShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            bool show = ckbShowPassword.Checked;
            txtNewPassword.PasswordChar = show ? '\0' : '*';
            txtConfirmPassword.PasswordChar = show ? '\0' : '*';
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // N?u là l?n ??u ??ng nh?p và ch?a ??i m?t kh?u, không cho ?óng form
            if (_isFirstLogin && this.DialogResult != DialogResult.OK)
            {
                e.Cancel = true;
                MessageBox.Show("Bạn  phải thay đổi mật khẩu để tiếp tục sử dụng hệ thống.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            base.OnFormClosing(e);
        }

        // Designer controls
        private Guna.UI2.WinForms.Guna2HtmlLabel lblTitle;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblDescription;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblNewPassword;
        private Guna.UI2.WinForms.Guna2TextBox txtNewPassword;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblConfirmPassword;
        private Guna.UI2.WinForms.Guna2TextBox txtConfirmPassword;
        private Guna.UI2.WinForms.Guna2Button btnChangePassword;
        private Guna.UI2.WinForms.Guna2Button btnCancel;
        private Guna.UI2.WinForms.Guna2CheckBox ckbShowPassword;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblPasswordHint;
    }
}
