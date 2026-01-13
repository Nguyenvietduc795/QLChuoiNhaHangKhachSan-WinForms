namespace QLChuoiNhaHangKhachSan.GUI
{
    partial class EmployeeForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EmployeeForm));
            this.lblEmployee_Title = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lblEmployee_SubTitle = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.bldManageEmployeeForm = new Guna.UI2.WinForms.Guna2BorderlessForm(this.components);
            this.btnAddEmployee = new Guna.UI2.WinForms.Guna2Button();
            this.pnlEmployeeSearch = new Guna.UI2.WinForms.Guna2Panel();
            this.btnInactive = new Guna.UI2.WinForms.Guna2Button();
            this.btnActive = new Guna.UI2.WinForms.Guna2Button();
            this.btnAll = new Guna.UI2.WinForms.Guna2Button();
            this.txtEmployeeSearch = new Guna.UI2.WinForms.Guna2TextBox();
            this.guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            this.flpEmployees = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlCardEmployeeTemplate = new Guna.UI2.WinForms.Guna2Panel();
            this.lblEmployeePhoneNumber = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.btnEmployeePhoneNumber = new Guna.UI2.WinForms.Guna2Button();
            this.lblEmployeeSalary = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.btnEmployeeSalary = new Guna.UI2.WinForms.Guna2Button();
            this.btnEmployeeActive = new Guna.UI2.WinForms.Guna2Button();
            this.btnEmployeeEdit = new Guna.UI2.WinForms.Guna2Button();
            this.btnEmployeeHireDate = new Guna.UI2.WinForms.Guna2Button();
            this.btnEmployeeDepartment = new Guna.UI2.WinForms.Guna2Button();
            this.btnEmployeeMail = new Guna.UI2.WinForms.Guna2Button();
            this.btnEmployeeStatus = new Guna.UI2.WinForms.Guna2Button();
            this.lblEmployeeHireDate = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lblEmployeeDeapartment = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lblEmployeeMail = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lblEmployeePos = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lblEmployeeName = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.pnlEmployeeAvt = new Guna.UI2.WinForms.Guna2CirclePictureBox();
            this.pnlEmployeeSearch.SuspendLayout();
            this.flpEmployees.SuspendLayout();
            this.pnlCardEmployeeTemplate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlEmployeeAvt)).BeginInit();
            this.SuspendLayout();
            // 
            // lblEmployee_Title
            // 
            this.lblEmployee_Title.AutoSize = false;
            this.lblEmployee_Title.BackColor = System.Drawing.Color.Transparent;
            this.lblEmployee_Title.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmployee_Title.Location = new System.Drawing.Point(22, 16);
            this.lblEmployee_Title.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.lblEmployee_Title.Name = "lblEmployee_Title";
            this.lblEmployee_Title.Size = new System.Drawing.Size(571, 47);
            this.lblEmployee_Title.TabIndex = 0;
            this.lblEmployee_Title.Text = "Danh sách nhân viên";
            // 
            // lblEmployee_SubTitle
            // 
            this.lblEmployee_SubTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblEmployee_SubTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmployee_SubTitle.ForeColor = System.Drawing.Color.Gray;
            this.lblEmployee_SubTitle.Location = new System.Drawing.Point(24, 65);
            this.lblEmployee_SubTitle.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.lblEmployee_SubTitle.Name = "lblEmployee_SubTitle";
            this.lblEmployee_SubTitle.Size = new System.Drawing.Size(209, 19);
            this.lblEmployee_SubTitle.TabIndex = 1;
            this.lblEmployee_SubTitle.Text = "Quản lí các nhân viên trong công ty";
            this.lblEmployee_SubTitle.Click += new System.EventHandler(this.lblEmployee_SubTitle_Click);
            // 
            // bldManageEmployeeForm
            // 
            this.bldManageEmployeeForm.ContainerControl = this;
            this.bldManageEmployeeForm.DockIndicatorTransparencyValue = 0.6D;
            this.bldManageEmployeeForm.TransparentWhileDrag = true;
            // 
            // btnAddEmployee
            // 
            this.btnAddEmployee.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddEmployee.BorderRadius = 10;
            this.btnAddEmployee.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnAddEmployee.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnAddEmployee.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnAddEmployee.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnAddEmployee.FillColor = System.Drawing.Color.Navy;
            this.btnAddEmployee.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddEmployee.ForeColor = System.Drawing.Color.Honeydew;
            this.btnAddEmployee.Location = new System.Drawing.Point(700, 24);
            this.btnAddEmployee.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnAddEmployee.Name = "btnAddEmployee";
            this.btnAddEmployee.Size = new System.Drawing.Size(148, 46);
            this.btnAddEmployee.TabIndex = 2;
            this.btnAddEmployee.Text = "+ Thêm nhân viên";
            this.btnAddEmployee.Click += new System.EventHandler(this.btnAddEmployee_Click);
            // 
            // pnlEmployeeSearch
            // 
            this.pnlEmployeeSearch.Controls.Add(this.btnInactive);
            this.pnlEmployeeSearch.Controls.Add(this.btnActive);
            this.pnlEmployeeSearch.Controls.Add(this.btnAll);
            this.pnlEmployeeSearch.Controls.Add(this.txtEmployeeSearch);
            this.pnlEmployeeSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlEmployeeSearch.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(250)))), ((int)(((byte)(251)))));
            this.pnlEmployeeSearch.Location = new System.Drawing.Point(0, 94);
            this.pnlEmployeeSearch.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pnlEmployeeSearch.Name = "pnlEmployeeSearch";
            this.pnlEmployeeSearch.Padding = new System.Windows.Forms.Padding(15, 16, 15, 16);
            this.pnlEmployeeSearch.Size = new System.Drawing.Size(900, 48);
            this.pnlEmployeeSearch.TabIndex = 4;
            // 
            // btnInactive
            // 
            this.btnInactive.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnInactive.BorderRadius = 18;
            this.btnInactive.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnInactive.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnInactive.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnInactive.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnInactive.FillColor = System.Drawing.Color.White;
            this.btnInactive.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInactive.ForeColor = System.Drawing.Color.Black;
            this.btnInactive.Location = new System.Drawing.Point(730, 5);
            this.btnInactive.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnInactive.Name = "btnInactive";
            this.btnInactive.Size = new System.Drawing.Size(81, 37);
            this.btnInactive.TabIndex = 3;
            this.btnInactive.Text = "Inactive";
            // 
            // btnActive
            // 
            this.btnActive.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnActive.BorderRadius = 18;
            this.btnActive.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnActive.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnActive.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnActive.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnActive.FillColor = System.Drawing.Color.White;
            this.btnActive.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnActive.ForeColor = System.Drawing.Color.Black;
            this.btnActive.Location = new System.Drawing.Point(622, 5);
            this.btnActive.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnActive.Name = "btnActive";
            this.btnActive.Size = new System.Drawing.Size(81, 37);
            this.btnActive.TabIndex = 2;
            this.btnActive.Text = "Active";
            // 
            // btnAll
            // 
            this.btnAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAll.BorderRadius = 18;
            this.btnAll.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnAll.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnAll.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnAll.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnAll.FillColor = System.Drawing.Color.Black;
            this.btnAll.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAll.ForeColor = System.Drawing.Color.White;
            this.btnAll.Location = new System.Drawing.Point(512, 5);
            this.btnAll.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnAll.Name = "btnAll";
            this.btnAll.Size = new System.Drawing.Size(81, 37);
            this.btnAll.TabIndex = 1;
            this.btnAll.Text = "Tất cả";
            // 
            // txtEmployeeSearch
            // 
            this.txtEmployeeSearch.BorderRadius = 20;
            this.txtEmployeeSearch.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtEmployeeSearch.DefaultText = "";
            this.txtEmployeeSearch.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtEmployeeSearch.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtEmployeeSearch.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtEmployeeSearch.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtEmployeeSearch.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtEmployeeSearch.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtEmployeeSearch.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtEmployeeSearch.IconLeft = ((System.Drawing.Image)(resources.GetObject("txtEmployeeSearch.IconLeft")));
            this.txtEmployeeSearch.Location = new System.Drawing.Point(34, 6);
            this.txtEmployeeSearch.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.txtEmployeeSearch.Name = "txtEmployeeSearch";
            this.txtEmployeeSearch.PlaceholderText = "Tìm kiếm";
            this.txtEmployeeSearch.SelectedText = "";
            this.txtEmployeeSearch.ShadowDecoration.Depth = 0;
            this.txtEmployeeSearch.Size = new System.Drawing.Size(450, 37);
            this.txtEmployeeSearch.TabIndex = 0;
            // 
            // guna2Panel1
            // 
            this.guna2Panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.guna2Panel1.Location = new System.Drawing.Point(0, 0);
            this.guna2Panel1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.guna2Panel1.Name = "guna2Panel1";
            this.guna2Panel1.Size = new System.Drawing.Size(900, 94);
            this.guna2Panel1.TabIndex = 5;
            // 
            // flpEmployees
            // 
            this.flpEmployees.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flpEmployees.AutoScroll = true;
            this.flpEmployees.Controls.Add(this.pnlCardEmployeeTemplate);
            this.flpEmployees.Location = new System.Drawing.Point(19, 196);
            this.flpEmployees.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.flpEmployees.Name = "flpEmployees";
            this.flpEmployees.Padding = new System.Windows.Forms.Padding(9, 10, 9, 10);
            this.flpEmployees.Size = new System.Drawing.Size(856, 327);
            this.flpEmployees.TabIndex = 3;
            this.flpEmployees.SizeChanged += new System.EventHandler(this.flpEmployees_SizeChanged);
            // 
            // pnlCardEmployeeTemplate
            // 
            this.pnlCardEmployeeTemplate.BackColor = System.Drawing.Color.Transparent;
            this.pnlCardEmployeeTemplate.BorderColor = System.Drawing.Color.Gainsboro;
            this.pnlCardEmployeeTemplate.BorderRadius = 12;
            this.pnlCardEmployeeTemplate.BorderThickness = 1;
            this.pnlCardEmployeeTemplate.Controls.Add(this.lblEmployeePhoneNumber);
            this.pnlCardEmployeeTemplate.Controls.Add(this.btnEmployeePhoneNumber);
            this.pnlCardEmployeeTemplate.Controls.Add(this.lblEmployeeSalary);
            this.pnlCardEmployeeTemplate.Controls.Add(this.btnEmployeeSalary);
            this.pnlCardEmployeeTemplate.Controls.Add(this.btnEmployeeActive);
            this.pnlCardEmployeeTemplate.Controls.Add(this.btnEmployeeEdit);
            this.pnlCardEmployeeTemplate.Controls.Add(this.btnEmployeeHireDate);
            this.pnlCardEmployeeTemplate.Controls.Add(this.btnEmployeeDepartment);
            this.pnlCardEmployeeTemplate.Controls.Add(this.btnEmployeeMail);
            this.pnlCardEmployeeTemplate.Controls.Add(this.btnEmployeeStatus);
            this.pnlCardEmployeeTemplate.Controls.Add(this.lblEmployeeHireDate);
            this.pnlCardEmployeeTemplate.Controls.Add(this.lblEmployeeDeapartment);
            this.pnlCardEmployeeTemplate.Controls.Add(this.lblEmployeeMail);
            this.pnlCardEmployeeTemplate.Controls.Add(this.lblEmployeePos);
            this.pnlCardEmployeeTemplate.Controls.Add(this.lblEmployeeName);
            this.pnlCardEmployeeTemplate.Controls.Add(this.pnlEmployeeAvt);
            this.pnlCardEmployeeTemplate.FillColor = System.Drawing.Color.White;
            this.pnlCardEmployeeTemplate.Location = new System.Drawing.Point(18, 20);
            this.pnlCardEmployeeTemplate.Margin = new System.Windows.Forms.Padding(9, 10, 9, 10);
            this.pnlCardEmployeeTemplate.MinimumSize = new System.Drawing.Size(240, 276);
            this.pnlCardEmployeeTemplate.Name = "pnlCardEmployeeTemplate";
            this.pnlCardEmployeeTemplate.ShadowDecoration.BorderRadius = 12;
            this.pnlCardEmployeeTemplate.ShadowDecoration.Color = System.Drawing.Color.White;
            this.pnlCardEmployeeTemplate.ShadowDecoration.Depth = 20;
            this.pnlCardEmployeeTemplate.ShadowDecoration.Enabled = true;
            this.pnlCardEmployeeTemplate.ShadowDecoration.Shadow = new System.Windows.Forms.Padding(8);
            this.pnlCardEmployeeTemplate.Size = new System.Drawing.Size(270, 276);
            this.pnlCardEmployeeTemplate.TabIndex = 16;
            this.pnlCardEmployeeTemplate.Visible = false;
            // 
            // lblEmployeePhoneNumber
            // 
            this.lblEmployeePhoneNumber.BackColor = System.Drawing.Color.Transparent;
            this.lblEmployeePhoneNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmployeePhoneNumber.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblEmployeePhoneNumber.Location = new System.Drawing.Point(59, 207);
            this.lblEmployeePhoneNumber.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.lblEmployeePhoneNumber.Name = "lblEmployeePhoneNumber";
            this.lblEmployeePhoneNumber.Size = new System.Drawing.Size(96, 17);
            this.lblEmployeePhoneNumber.TabIndex = 14;
            this.lblEmployeePhoneNumber.Text = "+84 123 456 789";
            // 
            // btnEmployeePhoneNumber
            // 
            this.btnEmployeePhoneNumber.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnEmployeePhoneNumber.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnEmployeePhoneNumber.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnEmployeePhoneNumber.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnEmployeePhoneNumber.FillColor = System.Drawing.Color.White;
            this.btnEmployeePhoneNumber.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnEmployeePhoneNumber.ForeColor = System.Drawing.Color.White;
            this.btnEmployeePhoneNumber.Image = ((System.Drawing.Image)(resources.GetObject("btnEmployeePhoneNumber.Image")));
            this.btnEmployeePhoneNumber.ImageSize = new System.Drawing.Size(25, 25);
            this.btnEmployeePhoneNumber.Location = new System.Drawing.Point(19, 205);
            this.btnEmployeePhoneNumber.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnEmployeePhoneNumber.Name = "btnEmployeePhoneNumber";
            this.btnEmployeePhoneNumber.Size = new System.Drawing.Size(27, 26);
            this.btnEmployeePhoneNumber.TabIndex = 13;
            // 
            // lblEmployeeSalary
            // 
            this.lblEmployeeSalary.BackColor = System.Drawing.Color.Transparent;
            this.lblEmployeeSalary.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmployeeSalary.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblEmployeeSalary.Location = new System.Drawing.Point(59, 178);
            this.lblEmployeeSalary.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.lblEmployeeSalary.Name = "lblEmployeeSalary";
            this.lblEmployeeSalary.Size = new System.Drawing.Size(27, 17);
            this.lblEmployeeSalary.TabIndex = 12;
            this.lblEmployeeSalary.Text = "$0.0";
            // 
            // btnEmployeeSalary
            // 
            this.btnEmployeeSalary.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnEmployeeSalary.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnEmployeeSalary.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnEmployeeSalary.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnEmployeeSalary.FillColor = System.Drawing.Color.White;
            this.btnEmployeeSalary.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnEmployeeSalary.ForeColor = System.Drawing.Color.White;
            this.btnEmployeeSalary.Image = ((System.Drawing.Image)(resources.GetObject("btnEmployeeSalary.Image")));
            this.btnEmployeeSalary.ImageSize = new System.Drawing.Size(25, 25);
            this.btnEmployeeSalary.Location = new System.Drawing.Point(19, 174);
            this.btnEmployeeSalary.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnEmployeeSalary.Name = "btnEmployeeSalary";
            this.btnEmployeeSalary.Size = new System.Drawing.Size(27, 26);
            this.btnEmployeeSalary.TabIndex = 12;
            // 
            // btnEmployeeActive
            // 
            this.btnEmployeeActive.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnEmployeeActive.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnEmployeeActive.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnEmployeeActive.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnEmployeeActive.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(250)))), ((int)(((byte)(229)))));
            this.btnEmployeeActive.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEmployeeActive.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(6)))), ((int)(((byte)(95)))), ((int)(((byte)(70)))));
            this.btnEmployeeActive.Location = new System.Drawing.Point(155, 245);
            this.btnEmployeeActive.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnEmployeeActive.Name = "btnEmployeeActive";
            this.btnEmployeeActive.Size = new System.Drawing.Size(104, 31);
            this.btnEmployeeActive.TabIndex = 11;
            this.btnEmployeeActive.Text = "Active";
            // 
            // btnEmployeeEdit
            // 
            this.btnEmployeeEdit.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnEmployeeEdit.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnEmployeeEdit.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnEmployeeEdit.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnEmployeeEdit.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.btnEmployeeEdit.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEmployeeEdit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.btnEmployeeEdit.Location = new System.Drawing.Point(19, 245);
            this.btnEmployeeEdit.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnEmployeeEdit.Name = "btnEmployeeEdit";
            this.btnEmployeeEdit.Size = new System.Drawing.Size(99, 31);
            this.btnEmployeeEdit.TabIndex = 10;
            this.btnEmployeeEdit.Text = "Edit";
            // 
            // btnEmployeeHireDate
            // 
            this.btnEmployeeHireDate.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnEmployeeHireDate.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnEmployeeHireDate.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnEmployeeHireDate.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnEmployeeHireDate.FillColor = System.Drawing.Color.White;
            this.btnEmployeeHireDate.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnEmployeeHireDate.ForeColor = System.Drawing.Color.White;
            this.btnEmployeeHireDate.Image = ((System.Drawing.Image)(resources.GetObject("btnEmployeeHireDate.Image")));
            this.btnEmployeeHireDate.ImageSize = new System.Drawing.Size(25, 25);
            this.btnEmployeeHireDate.Location = new System.Drawing.Point(19, 143);
            this.btnEmployeeHireDate.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnEmployeeHireDate.Name = "btnEmployeeHireDate";
            this.btnEmployeeHireDate.Size = new System.Drawing.Size(27, 26);
            this.btnEmployeeHireDate.TabIndex = 9;
            // 
            // btnEmployeeDepartment
            // 
            this.btnEmployeeDepartment.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnEmployeeDepartment.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnEmployeeDepartment.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnEmployeeDepartment.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnEmployeeDepartment.FillColor = System.Drawing.Color.White;
            this.btnEmployeeDepartment.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnEmployeeDepartment.ForeColor = System.Drawing.Color.White;
            this.btnEmployeeDepartment.Image = ((System.Drawing.Image)(resources.GetObject("btnEmployeeDepartment.Image")));
            this.btnEmployeeDepartment.ImageSize = new System.Drawing.Size(25, 25);
            this.btnEmployeeDepartment.Location = new System.Drawing.Point(19, 112);
            this.btnEmployeeDepartment.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnEmployeeDepartment.Name = "btnEmployeeDepartment";
            this.btnEmployeeDepartment.Size = new System.Drawing.Size(27, 26);
            this.btnEmployeeDepartment.TabIndex = 8;
            // 
            // btnEmployeeMail
            // 
            this.btnEmployeeMail.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnEmployeeMail.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnEmployeeMail.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnEmployeeMail.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnEmployeeMail.FillColor = System.Drawing.Color.White;
            this.btnEmployeeMail.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnEmployeeMail.ForeColor = System.Drawing.Color.White;
            this.btnEmployeeMail.Image = ((System.Drawing.Image)(resources.GetObject("btnEmployeeMail.Image")));
            this.btnEmployeeMail.ImageSize = new System.Drawing.Size(25, 25);
            this.btnEmployeeMail.Location = new System.Drawing.Point(19, 79);
            this.btnEmployeeMail.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnEmployeeMail.Name = "btnEmployeeMail";
            this.btnEmployeeMail.Size = new System.Drawing.Size(27, 28);
            this.btnEmployeeMail.TabIndex = 7;
            // 
            // btnEmployeeStatus
            // 
            this.btnEmployeeStatus.BorderRadius = 10;
            this.btnEmployeeStatus.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnEmployeeStatus.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnEmployeeStatus.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnEmployeeStatus.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnEmployeeStatus.FillColor = System.Drawing.Color.Silver;
            this.btnEmployeeStatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnEmployeeStatus.ForeColor = System.Drawing.Color.Black;
            this.btnEmployeeStatus.Location = new System.Drawing.Point(199, 20);
            this.btnEmployeeStatus.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnEmployeeStatus.Name = "btnEmployeeStatus";
            this.btnEmployeeStatus.Size = new System.Drawing.Size(69, 23);
            this.btnEmployeeStatus.TabIndex = 6;
            this.btnEmployeeStatus.Text = "Inactive";
            // 
            // lblEmployeeHireDate
            // 
            this.lblEmployeeHireDate.BackColor = System.Drawing.Color.Transparent;
            this.lblEmployeeHireDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmployeeHireDate.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblEmployeeHireDate.Location = new System.Drawing.Point(59, 147);
            this.lblEmployeeHireDate.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.lblEmployeeHireDate.Name = "lblEmployeeHireDate";
            this.lblEmployeeHireDate.Size = new System.Drawing.Size(115, 17);
            this.lblEmployeeHireDate.TabIndex = 5;
            this.lblEmployeeHireDate.Text = "September 14, 2024";
            // 
            // lblEmployeeDeapartment
            // 
            this.lblEmployeeDeapartment.BackColor = System.Drawing.Color.Transparent;
            this.lblEmployeeDeapartment.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmployeeDeapartment.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblEmployeeDeapartment.Location = new System.Drawing.Point(59, 119);
            this.lblEmployeeDeapartment.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.lblEmployeeDeapartment.Name = "lblEmployeeDeapartment";
            this.lblEmployeeDeapartment.Size = new System.Drawing.Size(28, 17);
            this.lblEmployeeDeapartment.TabIndex = 4;
            this.lblEmployeeDeapartment.Text = "CEO";
            // 
            // lblEmployeeMail
            // 
            this.lblEmployeeMail.BackColor = System.Drawing.Color.Transparent;
            this.lblEmployeeMail.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmployeeMail.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblEmployeeMail.Location = new System.Drawing.Point(59, 87);
            this.lblEmployeeMail.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.lblEmployeeMail.Name = "lblEmployeeMail";
            this.lblEmployeeMail.Size = new System.Drawing.Size(138, 17);
            this.lblEmployeeMail.TabIndex = 3;
            this.lblEmployeeMail.Text = "honggam@hrgroup.com";
            // 
            // lblEmployeePos
            // 
            this.lblEmployeePos.BackColor = System.Drawing.Color.Transparent;
            this.lblEmployeePos.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmployeePos.ForeColor = System.Drawing.Color.Gray;
            this.lblEmployeePos.Location = new System.Drawing.Point(71, 47);
            this.lblEmployeePos.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.lblEmployeePos.Name = "lblEmployeePos";
            this.lblEmployeePos.Size = new System.Drawing.Size(84, 17);
            this.lblEmployeePos.TabIndex = 2;
            this.lblEmployeePos.Text = "Tổng giám đốc";
            // 
            // lblEmployeeName
            // 
            this.lblEmployeeName.BackColor = System.Drawing.Color.Transparent;
            this.lblEmployeeName.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmployeeName.Location = new System.Drawing.Point(71, 16);
            this.lblEmployeeName.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.lblEmployeeName.Name = "lblEmployeeName";
            this.lblEmployeeName.Size = new System.Drawing.Size(114, 32);
            this.lblEmployeeName.TabIndex = 1;
            this.lblEmployeeName.Text = "Hồng Gấm";
            // 
            // pnlEmployeeAvt
            // 
            this.pnlEmployeeAvt.FillColor = System.Drawing.Color.Silver;
            this.pnlEmployeeAvt.ImageRotate = 0F;
            this.pnlEmployeeAvt.Location = new System.Drawing.Point(19, 20);
            this.pnlEmployeeAvt.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pnlEmployeeAvt.Name = "pnlEmployeeAvt";
            this.pnlEmployeeAvt.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            this.pnlEmployeeAvt.Size = new System.Drawing.Size(41, 45);
            this.pnlEmployeeAvt.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pnlEmployeeAvt.TabIndex = 0;
            this.pnlEmployeeAvt.TabStop = false;
            // 
            // EmployeeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 609);
            this.Controls.Add(this.pnlEmployeeSearch);
            this.Controls.Add(this.flpEmployees);
            this.Controls.Add(this.btnAddEmployee);
            this.Controls.Add(this.lblEmployee_SubTitle);
            this.Controls.Add(this.lblEmployee_Title);
            this.Controls.Add(this.guna2Panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "EmployeeForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EmployeeForm";
            this.pnlEmployeeSearch.ResumeLayout(false);
            this.flpEmployees.ResumeLayout(false);
            this.pnlCardEmployeeTemplate.ResumeLayout(false);
            this.pnlCardEmployeeTemplate.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlEmployeeAvt)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Guna.UI2.WinForms.Guna2HtmlLabel lblEmployee_Title;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblEmployee_SubTitle;
        private Guna.UI2.WinForms.Guna2BorderlessForm bldManageEmployeeForm;
        private Guna.UI2.WinForms.Guna2Button btnAddEmployee;
        private Guna.UI2.WinForms.Guna2Panel pnlEmployeeSearch;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private Guna.UI2.WinForms.Guna2TextBox txtEmployeeSearch;
        private Guna.UI2.WinForms.Guna2Button btnActive;
        private Guna.UI2.WinForms.Guna2Button btnAll;
        private Guna.UI2.WinForms.Guna2Button btnInactive;
        private System.Windows.Forms.FlowLayoutPanel flpEmployees;
        private Guna.UI2.WinForms.Guna2Panel pnlCardEmployeeTemplate;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblEmployeePhoneNumber;
        private Guna.UI2.WinForms.Guna2Button btnEmployeePhoneNumber;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblEmployeeSalary;
        private Guna.UI2.WinForms.Guna2Button btnEmployeeSalary;
        private Guna.UI2.WinForms.Guna2Button btnEmployeeActive;
        private Guna.UI2.WinForms.Guna2Button btnEmployeeEdit;
        private Guna.UI2.WinForms.Guna2Button btnEmployeeHireDate;
        private Guna.UI2.WinForms.Guna2Button btnEmployeeDepartment;
        private Guna.UI2.WinForms.Guna2Button btnEmployeeMail;
        private Guna.UI2.WinForms.Guna2Button btnEmployeeStatus;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblEmployeeHireDate;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblEmployeeDeapartment;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblEmployeeMail;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblEmployeePos;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblEmployeeName;
        private Guna.UI2.WinForms.Guna2CirclePictureBox pnlEmployeeAvt;
    }
}