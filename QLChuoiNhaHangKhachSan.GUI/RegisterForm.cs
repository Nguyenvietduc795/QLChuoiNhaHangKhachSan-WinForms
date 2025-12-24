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
    public partial class RegisterForm : Form
    {
        public RegisterForm()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {

        }

        private void clbLoginClose_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Bạn có chắc chắn muốn thoát ứng dụng?", "Xác nhận thoát", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Close();
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
            ckbRegister_ShowPassword.Text = ckbRegister_ShowPassword.Checked ? "Show password" : "Hiden password";
            txtRegister_Password.PasswordChar = show ? '\0' : '*';
            txtRegister_ConfirmPassword.PasswordChar = show ? '\0' : '*';
        }
    }
}
