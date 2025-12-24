namespace QLChuoiNhaHangKhachSan.GUI
{
    partial class RegisterForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RegisterForm));
            this.clbLoginClose = new Guna.UI2.WinForms.Guna2ControlBox();
            this.btnRegister = new Guna.UI2.WinForms.Guna2Button();
            this.ckbRegister_ShowPassword = new Guna.UI2.WinForms.Guna2CheckBox();
            this.lblRegister_Password = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.txtRegister_Password = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblRegisterUserName = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.txtRegister_UserName = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblLoginTitle = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.ptbLoginAvt = new Guna.UI2.WinForms.Guna2PictureBox();
            this.lblRegister_ConfirmPasssword = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.txtRegister_ConfirmPassword = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblRegister = new Guna.UI2.WinForms.Guna2HtmlLabel();
            ((System.ComponentModel.ISupportInitialize)(this.ptbLoginAvt)).BeginInit();
            this.SuspendLayout();
            // 
            // clbLoginClose
            // 
            this.clbLoginClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.clbLoginClose.CustomClick = true;
            this.clbLoginClose.FillColor = System.Drawing.Color.Crimson;
            this.clbLoginClose.IconColor = System.Drawing.Color.White;
            this.clbLoginClose.Location = new System.Drawing.Point(433, -1);
            this.clbLoginClose.Name = "clbLoginClose";
            this.clbLoginClose.Size = new System.Drawing.Size(78, 37);
            this.clbLoginClose.TabIndex = 20;
            this.clbLoginClose.Click += new System.EventHandler(this.clbLoginClose_Click);
            // 
            // btnRegister
            // 
            this.btnRegister.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnRegister.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnRegister.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnRegister.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnRegister.FillColor = System.Drawing.Color.MidnightBlue;
            this.btnRegister.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRegister.ForeColor = System.Drawing.Color.White;
            this.btnRegister.Location = new System.Drawing.Point(100, 554);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new System.Drawing.Size(308, 52);
            this.btnRegister.TabIndex = 18;
            this.btnRegister.Text = "Register";
            this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);
            // 
            // ckbRegister_ShowPassword
            // 
            this.ckbRegister_ShowPassword.AutoSize = true;
            this.ckbRegister_ShowPassword.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.ckbRegister_ShowPassword.CheckedState.BorderRadius = 0;
            this.ckbRegister_ShowPassword.CheckedState.BorderThickness = 0;
            this.ckbRegister_ShowPassword.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.ckbRegister_ShowPassword.Font = new System.Drawing.Font("Segoe UI", 7.8F);
            this.ckbRegister_ShowPassword.Location = new System.Drawing.Point(100, 507);
            this.ckbRegister_ShowPassword.Name = "ckbRegister_ShowPassword";
            this.ckbRegister_ShowPassword.Size = new System.Drawing.Size(122, 21);
            this.ckbRegister_ShowPassword.TabIndex = 17;
            this.ckbRegister_ShowPassword.Text = "Show password";
            this.ckbRegister_ShowPassword.UncheckedState.BorderColor = System.Drawing.Color.Black;
            this.ckbRegister_ShowPassword.UncheckedState.BorderRadius = 0;
            this.ckbRegister_ShowPassword.UncheckedState.BorderThickness = 0;
            this.ckbRegister_ShowPassword.UncheckedState.FillColor = System.Drawing.Color.White;
            this.ckbRegister_ShowPassword.CheckedChanged += new System.EventHandler(this.ckbRegister_ShowPassword_CheckedChanged);
            // 
            // lblRegister_Password
            // 
            this.lblRegister_Password.BackColor = System.Drawing.Color.Transparent;
            this.lblRegister_Password.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRegister_Password.Location = new System.Drawing.Point(100, 323);
            this.lblRegister_Password.Name = "lblRegister_Password";
            this.lblRegister_Password.Size = new System.Drawing.Size(65, 22);
            this.lblRegister_Password.TabIndex = 16;
            this.lblRegister_Password.Text = "Password";
            // 
            // txtRegister_Password
            // 
            this.txtRegister_Password.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txtRegister_Password.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtRegister_Password.DefaultText = "";
            this.txtRegister_Password.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtRegister_Password.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtRegister_Password.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtRegister_Password.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtRegister_Password.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtRegister_Password.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtRegister_Password.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtRegister_Password.Location = new System.Drawing.Point(100, 352);
            this.txtRegister_Password.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtRegister_Password.Name = "txtRegister_Password";
            this.txtRegister_Password.PasswordChar = '*';
            this.txtRegister_Password.PlaceholderText = "";
            this.txtRegister_Password.SelectedText = "";
            this.txtRegister_Password.Size = new System.Drawing.Size(308, 44);
            this.txtRegister_Password.TabIndex = 15;
            // 
            // lblRegisterUserName
            // 
            this.lblRegisterUserName.BackColor = System.Drawing.Color.Transparent;
            this.lblRegisterUserName.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRegisterUserName.Location = new System.Drawing.Point(100, 244);
            this.lblRegisterUserName.Name = "lblRegisterUserName";
            this.lblRegisterUserName.Size = new System.Drawing.Size(73, 22);
            this.lblRegisterUserName.TabIndex = 14;
            this.lblRegisterUserName.Text = "User name";
            // 
            // txtRegister_UserName
            // 
            this.txtRegister_UserName.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txtRegister_UserName.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtRegister_UserName.DefaultText = "";
            this.txtRegister_UserName.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtRegister_UserName.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtRegister_UserName.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtRegister_UserName.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtRegister_UserName.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtRegister_UserName.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtRegister_UserName.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtRegister_UserName.Location = new System.Drawing.Point(100, 273);
            this.txtRegister_UserName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtRegister_UserName.Name = "txtRegister_UserName";
            this.txtRegister_UserName.PlaceholderText = "";
            this.txtRegister_UserName.SelectedText = "";
            this.txtRegister_UserName.Size = new System.Drawing.Size(308, 44);
            this.txtRegister_UserName.TabIndex = 13;
            // 
            // lblLoginTitle
            // 
            this.lblLoginTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblLoginTitle.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLoginTitle.Location = new System.Drawing.Point(153, 177);
            this.lblLoginTitle.Name = "lblLoginTitle";
            this.lblLoginTitle.Size = new System.Drawing.Size(187, 39);
            this.lblLoginTitle.TabIndex = 12;
            this.lblLoginTitle.Text = "Welcome here";
            // 
            // ptbLoginAvt
            // 
            this.ptbLoginAvt.Image = ((System.Drawing.Image)(resources.GetObject("ptbLoginAvt.Image")));
            this.ptbLoginAvt.ImageRotate = 0F;
            this.ptbLoginAvt.Location = new System.Drawing.Point(167, 29);
            this.ptbLoginAvt.Name = "ptbLoginAvt";
            this.ptbLoginAvt.Size = new System.Drawing.Size(144, 142);
            this.ptbLoginAvt.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ptbLoginAvt.TabIndex = 11;
            this.ptbLoginAvt.TabStop = false;
            // 
            // lblRegister_ConfirmPasssword
            // 
            this.lblRegister_ConfirmPasssword.BackColor = System.Drawing.Color.Transparent;
            this.lblRegister_ConfirmPasssword.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRegister_ConfirmPasssword.Location = new System.Drawing.Point(100, 414);
            this.lblRegister_ConfirmPasssword.Name = "lblRegister_ConfirmPasssword";
            this.lblRegister_ConfirmPasssword.Size = new System.Drawing.Size(122, 22);
            this.lblRegister_ConfirmPasssword.TabIndex = 22;
            this.lblRegister_ConfirmPasssword.Text = "Confirm Password";
            // 
            // txtRegister_ConfirmPassword
            // 
            this.txtRegister_ConfirmPassword.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txtRegister_ConfirmPassword.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtRegister_ConfirmPassword.DefaultText = "";
            this.txtRegister_ConfirmPassword.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtRegister_ConfirmPassword.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtRegister_ConfirmPassword.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtRegister_ConfirmPassword.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtRegister_ConfirmPassword.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtRegister_ConfirmPassword.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtRegister_ConfirmPassword.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtRegister_ConfirmPassword.Location = new System.Drawing.Point(100, 443);
            this.txtRegister_ConfirmPassword.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtRegister_ConfirmPassword.Name = "txtRegister_ConfirmPassword";
            this.txtRegister_ConfirmPassword.PasswordChar = '*';
            this.txtRegister_ConfirmPassword.PlaceholderText = "";
            this.txtRegister_ConfirmPassword.SelectedText = "";
            this.txtRegister_ConfirmPassword.Size = new System.Drawing.Size(308, 44);
            this.txtRegister_ConfirmPassword.TabIndex = 21;
            // 
            // lblRegister
            // 
            this.lblRegister.BackColor = System.Drawing.Color.Transparent;
            this.lblRegister.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRegister.ForeColor = System.Drawing.Color.Gray;
            this.lblRegister.Location = new System.Drawing.Point(219, 627);
            this.lblRegister.Name = "lblRegister";
            this.lblRegister.Size = new System.Drawing.Size(84, 25);
            this.lblRegister.TabIndex = 23;
            this.lblRegister.Text = "Login here";
            this.lblRegister.Click += new System.EventHandler(this.lblRegister_Click);
            // 
            // RegisterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(509, 694);
            this.Controls.Add(this.lblRegister);
            this.Controls.Add(this.lblRegister_ConfirmPasssword);
            this.Controls.Add(this.txtRegister_ConfirmPassword);
            this.Controls.Add(this.clbLoginClose);
            this.Controls.Add(this.btnRegister);
            this.Controls.Add(this.ckbRegister_ShowPassword);
            this.Controls.Add(this.lblRegister_Password);
            this.Controls.Add(this.txtRegister_Password);
            this.Controls.Add(this.lblRegisterUserName);
            this.Controls.Add(this.txtRegister_UserName);
            this.Controls.Add(this.lblLoginTitle);
            this.Controls.Add(this.ptbLoginAvt);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "RegisterForm";
            this.Text = "Register";
            ((System.ComponentModel.ISupportInitialize)(this.ptbLoginAvt)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Guna.UI2.WinForms.Guna2ControlBox clbLoginClose;
        private Guna.UI2.WinForms.Guna2Button btnRegister;
        private Guna.UI2.WinForms.Guna2CheckBox ckbRegister_ShowPassword;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblRegister_Password;
        private Guna.UI2.WinForms.Guna2TextBox txtRegister_Password;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblRegisterUserName;
        private Guna.UI2.WinForms.Guna2TextBox txtRegister_UserName;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblLoginTitle;
        private Guna.UI2.WinForms.Guna2PictureBox ptbLoginAvt;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblRegister_ConfirmPasssword;
        private Guna.UI2.WinForms.Guna2TextBox txtRegister_ConfirmPassword;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblRegister;
    }
}