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
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void ptbLoginAvt_Click(object sender, EventArgs e)
        {

        }

        private void clbLoginClose_Click(object sender, EventArgs e)
        {
           if (MessageBox.Show("Bạn có chắc chắn muốn thoát ứng dụng?", "Xác nhận thoát", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {

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
            ckbLogin_ShowPassword.Text = ckbLogin_ShowPassword.Checked ? "Show password" : "Hiden password";
            txtLogin_Password.PasswordChar = show ? '\0' : '*';
        }
    }
}
