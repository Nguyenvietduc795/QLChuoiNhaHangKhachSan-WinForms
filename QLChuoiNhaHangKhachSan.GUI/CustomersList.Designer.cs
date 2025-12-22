namespace QLChuoiNhaHangKhachSan.GUI
{
    partial class CustomersList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomersList));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.gnVIPCustomers = new Guna.UI2.WinForms.Guna2Panel();
            this.guna2Button2 = new Guna.UI2.WinForms.Guna2Button();
            this.lblVIPCount = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lblVIPCustomers = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2HtmlLabel2 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2HtmlLabel1 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.bntAddCustomers = new Guna.UI2.WinForms.Guna2Button();
            this.pnHeader2 = new Guna.UI2.WinForms.Guna2Panel();
            this.gnTotalCustomers = new Guna.UI2.WinForms.Guna2Panel();
            this.guna2Button3 = new Guna.UI2.WinForms.Guna2Button();
            this.lblTotalCustomerCountNumber = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lblTotalCustomerCount = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.btnUpdateCustomers = new Guna.UI2.WinForms.Guna2Button();
            this.btnDelectCustomers = new Guna.UI2.WinForms.Guna2Button();
            this.btnFind = new Guna.UI2.WinForms.Guna2Button();
            this.tbFindCustomers = new Guna.UI2.WinForms.Guna2TextBox();
            this.pnHeader1 = new Guna.UI2.WinForms.Guna2Panel();
            this.guna2Button4 = new Guna.UI2.WinForms.Guna2Button();
            this.lblCustomerManagement = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.dgvListCustomers = new Guna.UI2.WinForms.Guna2DataGridView();
            this.CustomersID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CustomersName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CustomersPhone = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CustomersAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CustomersType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.guna2HtmlLabel3 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2Panel3 = new Guna.UI2.WinForms.Guna2Panel();
            this.guna2Button1 = new Guna.UI2.WinForms.Guna2Button();
            this.gnVIPCustomers.SuspendLayout();
            this.pnHeader2.SuspendLayout();
            this.gnTotalCustomers.SuspendLayout();
            this.pnHeader1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvListCustomers)).BeginInit();
            this.guna2Panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // gnVIPCustomers
            // 
            this.gnVIPCustomers.BackColor = System.Drawing.Color.Transparent;
            this.gnVIPCustomers.BorderRadius = 25;
            this.gnVIPCustomers.Controls.Add(this.guna2Button2);
            this.gnVIPCustomers.Controls.Add(this.lblVIPCount);
            this.gnVIPCustomers.Controls.Add(this.lblVIPCustomers);
            this.gnVIPCustomers.FillColor = System.Drawing.Color.White;
            this.gnVIPCustomers.Font = new System.Drawing.Font("Microsoft YaHei UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gnVIPCustomers.Location = new System.Drawing.Point(15, 149);
            this.gnVIPCustomers.Name = "gnVIPCustomers";
            this.gnVIPCustomers.ShadowDecoration.BorderRadius = 20;
            this.gnVIPCustomers.ShadowDecoration.Enabled = true;
            this.gnVIPCustomers.Size = new System.Drawing.Size(406, 114);
            this.gnVIPCustomers.TabIndex = 4;
            // 
            // guna2Button2
            // 
            this.guna2Button2.BorderRadius = 10;
            this.guna2Button2.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button2.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button2.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2Button2.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2Button2.FillColor = System.Drawing.Color.Gainsboro;
            this.guna2Button2.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2Button2.ForeColor = System.Drawing.Color.White;
            this.guna2Button2.Image = ((System.Drawing.Image)(resources.GetObject("guna2Button2.Image")));
            this.guna2Button2.ImageSize = new System.Drawing.Size(60, 60);
            this.guna2Button2.Location = new System.Drawing.Point(286, 8);
            this.guna2Button2.Name = "guna2Button2";
            this.guna2Button2.Size = new System.Drawing.Size(100, 96);
            this.guna2Button2.TabIndex = 6;
            // 
            // lblVIPCount
            // 
            this.lblVIPCount.AutoSize = false;
            this.lblVIPCount.BackColor = System.Drawing.Color.Transparent;
            this.lblVIPCount.Font = new System.Drawing.Font("Microsoft YaHei UI", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVIPCount.Location = new System.Drawing.Point(15, 52);
            this.lblVIPCount.Name = "lblVIPCount";
            this.lblVIPCount.Size = new System.Drawing.Size(47, 68);
            this.lblVIPCount.TabIndex = 3;
            this.lblVIPCount.Text = "1";
            this.lblVIPCount.Click += new System.EventHandler(this.lblVIPCount_Click);
            // 
            // lblVIPCustomers
            // 
            this.lblVIPCustomers.AutoSize = false;
            this.lblVIPCustomers.BackColor = System.Drawing.Color.Transparent;
            this.lblVIPCustomers.Font = new System.Drawing.Font("Microsoft YaHei UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVIPCustomers.Location = new System.Drawing.Point(15, 8);
            this.lblVIPCustomers.Name = "lblVIPCustomers";
            this.lblVIPCustomers.Size = new System.Drawing.Size(425, 63);
            this.lblVIPCustomers.TabIndex = 3;
            this.lblVIPCustomers.Text = "Khách hàng VIP";
            this.lblVIPCustomers.Click += new System.EventHandler(this.lblVIPCustomers_Click);
            // 
            // guna2HtmlLabel2
            // 
            this.guna2HtmlLabel2.AutoSize = false;
            this.guna2HtmlLabel2.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel2.Font = new System.Drawing.Font("Microsoft YaHei UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel2.Location = new System.Drawing.Point(48, 8);
            this.guna2HtmlLabel2.Name = "guna2HtmlLabel2";
            this.guna2HtmlLabel2.Size = new System.Drawing.Size(345, 53);
            this.guna2HtmlLabel2.TabIndex = 0;
            this.guna2HtmlLabel2.Text = "Tổng khách hàng";
            // 
            // guna2HtmlLabel1
            // 
            this.guna2HtmlLabel1.AutoSize = false;
            this.guna2HtmlLabel1.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel1.Font = new System.Drawing.Font("Microsoft YaHei UI", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel1.Location = new System.Drawing.Point(171, 57);
            this.guna2HtmlLabel1.Name = "guna2HtmlLabel1";
            this.guna2HtmlLabel1.Size = new System.Drawing.Size(47, 52);
            this.guna2HtmlLabel1.TabIndex = 2;
            this.guna2HtmlLabel1.Text = "0";
            // 
            // bntAddCustomers
            // 
            this.bntAddCustomers.BackColor = System.Drawing.Color.Transparent;
            this.bntAddCustomers.BorderRadius = 15;
            this.bntAddCustomers.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.bntAddCustomers.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.bntAddCustomers.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.bntAddCustomers.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.bntAddCustomers.FillColor = System.Drawing.Color.Navy;
            this.bntAddCustomers.Font = new System.Drawing.Font("Microsoft JhengHei UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bntAddCustomers.ForeColor = System.Drawing.Color.White;
            this.bntAddCustomers.Image = ((System.Drawing.Image)(resources.GetObject("bntAddCustomers.Image")));
            this.bntAddCustomers.ImageSize = new System.Drawing.Size(40, 40);
            this.bntAddCustomers.Location = new System.Drawing.Point(1609, 87);
            this.bntAddCustomers.Name = "bntAddCustomers";
            this.bntAddCustomers.ShadowDecoration.BorderRadius = 15;
            this.bntAddCustomers.ShadowDecoration.Enabled = true;
            this.bntAddCustomers.Size = new System.Drawing.Size(405, 166);
            this.bntAddCustomers.TabIndex = 6;
            this.bntAddCustomers.Text = "Thêm khách hàng";
            this.bntAddCustomers.Click += new System.EventHandler(this.bntAddCustomers_Click);
            // 
            // pnHeader2
            // 
            this.pnHeader2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnHeader2.BackColor = System.Drawing.Color.Transparent;
            this.pnHeader2.BorderColor = System.Drawing.Color.White;
            this.pnHeader2.BorderRadius = 15;
            this.pnHeader2.Controls.Add(this.bntAddCustomers);
            this.pnHeader2.Controls.Add(this.gnTotalCustomers);
            this.pnHeader2.Controls.Add(this.gnVIPCustomers);
            this.pnHeader2.Controls.Add(this.btnUpdateCustomers);
            this.pnHeader2.Controls.Add(this.btnDelectCustomers);
            this.pnHeader2.Controls.Add(this.btnFind);
            this.pnHeader2.Controls.Add(this.tbFindCustomers);
            this.pnHeader2.CustomBorderColor = System.Drawing.Color.White;
            this.pnHeader2.FillColor = System.Drawing.Color.White;
            this.pnHeader2.Location = new System.Drawing.Point(1, 126);
            this.pnHeader2.Name = "pnHeader2";
            this.pnHeader2.ShadowDecoration.BorderRadius = 10;
            this.pnHeader2.ShadowDecoration.Color = System.Drawing.Color.Gray;
            this.pnHeader2.ShadowDecoration.Enabled = true;
            this.pnHeader2.Size = new System.Drawing.Size(1928, 269);
            this.pnHeader2.TabIndex = 7;
            this.pnHeader2.Paint += new System.Windows.Forms.PaintEventHandler(this.pnHeader2_Paint);
            // 
            // gnTotalCustomers
            // 
            this.gnTotalCustomers.BackColor = System.Drawing.Color.Transparent;
            this.gnTotalCustomers.BorderColor = System.Drawing.Color.White;
            this.gnTotalCustomers.BorderRadius = 25;
            this.gnTotalCustomers.Controls.Add(this.guna2Button3);
            this.gnTotalCustomers.Controls.Add(this.lblTotalCustomerCountNumber);
            this.gnTotalCustomers.Controls.Add(this.lblTotalCustomerCount);
            this.gnTotalCustomers.FillColor = System.Drawing.Color.White;
            this.gnTotalCustomers.ForeColor = System.Drawing.Color.White;
            this.gnTotalCustomers.Location = new System.Drawing.Point(15, 18);
            this.gnTotalCustomers.Name = "gnTotalCustomers";
            this.gnTotalCustomers.ShadowDecoration.BorderRadius = 20;
            this.gnTotalCustomers.ShadowDecoration.Enabled = true;
            this.gnTotalCustomers.Size = new System.Drawing.Size(406, 114);
            this.gnTotalCustomers.TabIndex = 9;
            // 
            // guna2Button3
            // 
            this.guna2Button3.BorderRadius = 10;
            this.guna2Button3.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button3.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button3.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2Button3.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2Button3.FillColor = System.Drawing.Color.Gainsboro;
            this.guna2Button3.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2Button3.ForeColor = System.Drawing.Color.White;
            this.guna2Button3.Image = ((System.Drawing.Image)(resources.GetObject("guna2Button3.Image")));
            this.guna2Button3.ImageSize = new System.Drawing.Size(60, 60);
            this.guna2Button3.Location = new System.Drawing.Point(286, 10);
            this.guna2Button3.Name = "guna2Button3";
            this.guna2Button3.Size = new System.Drawing.Size(100, 94);
            this.guna2Button3.TabIndex = 5;
            this.guna2Button3.Click += new System.EventHandler(this.guna2Button3_Click);
            // 
            // lblTotalCustomerCountNumber
            // 
            this.lblTotalCustomerCountNumber.BackColor = System.Drawing.Color.White;
            this.lblTotalCustomerCountNumber.Font = new System.Drawing.Font("Microsoft YaHei UI", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalCustomerCountNumber.ForeColor = System.Drawing.Color.Black;
            this.lblTotalCustomerCountNumber.Location = new System.Drawing.Point(12, 52);
            this.lblTotalCustomerCountNumber.Name = "lblTotalCustomerCountNumber";
            this.lblTotalCustomerCountNumber.Size = new System.Drawing.Size(26, 52);
            this.lblTotalCustomerCountNumber.TabIndex = 2;
            this.lblTotalCustomerCountNumber.Text = "0";
            // 
            // lblTotalCustomerCount
            // 
            this.lblTotalCustomerCount.AutoSize = false;
            this.lblTotalCustomerCount.BackColor = System.Drawing.Color.White;
            this.lblTotalCustomerCount.Font = new System.Drawing.Font("Microsoft YaHei UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalCustomerCount.ForeColor = System.Drawing.Color.Black;
            this.lblTotalCustomerCount.Location = new System.Drawing.Point(12, 8);
            this.lblTotalCustomerCount.Name = "lblTotalCustomerCount";
            this.lblTotalCustomerCount.Size = new System.Drawing.Size(460, 62);
            this.lblTotalCustomerCount.TabIndex = 0;
            this.lblTotalCustomerCount.Text = "Tổng khách hàng";
            this.lblTotalCustomerCount.Click += new System.EventHandler(this.lblTotalCustomerCount_Click);
            // 
            // btnUpdateCustomers
            // 
            this.btnUpdateCustomers.BackColor = System.Drawing.Color.Transparent;
            this.btnUpdateCustomers.BorderRadius = 10;
            this.btnUpdateCustomers.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnUpdateCustomers.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnUpdateCustomers.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnUpdateCustomers.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnUpdateCustomers.FillColor = System.Drawing.Color.Green;
            this.btnUpdateCustomers.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpdateCustomers.ForeColor = System.Drawing.Color.White;
            this.btnUpdateCustomers.Image = ((System.Drawing.Image)(resources.GetObject("btnUpdateCustomers.Image")));
            this.btnUpdateCustomers.ImageSize = new System.Drawing.Size(40, 40);
            this.btnUpdateCustomers.Location = new System.Drawing.Point(493, 87);
            this.btnUpdateCustomers.Name = "btnUpdateCustomers";
            this.btnUpdateCustomers.ShadowDecoration.BorderRadius = 10;
            this.btnUpdateCustomers.ShadowDecoration.Enabled = true;
            this.btnUpdateCustomers.Size = new System.Drawing.Size(978, 75);
            this.btnUpdateCustomers.TabIndex = 3;
            this.btnUpdateCustomers.Text = "Cập nhật khách hàng";
            this.btnUpdateCustomers.Click += new System.EventHandler(this.btnUpdateCustomers_Click);
            // 
            // btnDelectCustomers
            // 
            this.btnDelectCustomers.BackColor = System.Drawing.Color.Transparent;
            this.btnDelectCustomers.BorderRadius = 10;
            this.btnDelectCustomers.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnDelectCustomers.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnDelectCustomers.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnDelectCustomers.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnDelectCustomers.FillColor = System.Drawing.Color.Maroon;
            this.btnDelectCustomers.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelectCustomers.ForeColor = System.Drawing.Color.White;
            this.btnDelectCustomers.Image = ((System.Drawing.Image)(resources.GetObject("btnDelectCustomers.Image")));
            this.btnDelectCustomers.ImageSize = new System.Drawing.Size(40, 40);
            this.btnDelectCustomers.Location = new System.Drawing.Point(493, 178);
            this.btnDelectCustomers.Name = "btnDelectCustomers";
            this.btnDelectCustomers.ShadowDecoration.BorderRadius = 10;
            this.btnDelectCustomers.ShadowDecoration.Enabled = true;
            this.btnDelectCustomers.Size = new System.Drawing.Size(978, 75);
            this.btnDelectCustomers.TabIndex = 2;
            this.btnDelectCustomers.Text = "Xóa khách hàng";
            this.btnDelectCustomers.Click += new System.EventHandler(this.btnDelectCustomers_Click);
            // 
            // btnFind
            // 
            this.btnFind.BackColor = System.Drawing.Color.Transparent;
            this.btnFind.BorderRadius = 10;
            this.btnFind.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnFind.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnFind.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnFind.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnFind.FillColor = System.Drawing.Color.Navy;
            this.btnFind.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFind.ForeColor = System.Drawing.Color.White;
            this.btnFind.Location = new System.Drawing.Point(1852, 18);
            this.btnFind.Name = "btnFind";
            this.btnFind.ShadowDecoration.BorderRadius = 10;
            this.btnFind.ShadowDecoration.Enabled = true;
            this.btnFind.Size = new System.Drawing.Size(162, 56);
            this.btnFind.TabIndex = 1;
            this.btnFind.Text = "Tìm";
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // tbFindCustomers
            // 
            this.tbFindCustomers.BackColor = System.Drawing.Color.Transparent;
            this.tbFindCustomers.BorderRadius = 10;
            this.tbFindCustomers.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tbFindCustomers.DefaultText = "";
            this.tbFindCustomers.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.tbFindCustomers.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.tbFindCustomers.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tbFindCustomers.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tbFindCustomers.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tbFindCustomers.Font = new System.Drawing.Font("Microsoft JhengHei UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbFindCustomers.ForeColor = System.Drawing.Color.Black;
            this.tbFindCustomers.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tbFindCustomers.Location = new System.Drawing.Point(493, 18);
            this.tbFindCustomers.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbFindCustomers.Name = "tbFindCustomers";
            this.tbFindCustomers.PlaceholderForeColor = System.Drawing.Color.Gray;
            this.tbFindCustomers.PlaceholderText = "Tìm kím khách hàng...";
            this.tbFindCustomers.SelectedText = "";
            this.tbFindCustomers.ShadowDecoration.Enabled = true;
            this.tbFindCustomers.Size = new System.Drawing.Size(1353, 56);
            this.tbFindCustomers.TabIndex = 0;
            this.tbFindCustomers.TextChanged += new System.EventHandler(this.tbFindCustomers_TextChanged);
            // 
            // pnHeader1
            // 
            this.pnHeader1.BackColor = System.Drawing.Color.Transparent;
            this.pnHeader1.BorderRadius = 10;
            this.pnHeader1.Controls.Add(this.guna2Button4);
            this.pnHeader1.Controls.Add(this.lblCustomerManagement);
            this.pnHeader1.Location = new System.Drawing.Point(1, 23);
            this.pnHeader1.Name = "pnHeader1";
            this.pnHeader1.ShadowDecoration.Color = System.Drawing.Color.Silver;
            this.pnHeader1.ShadowDecoration.Enabled = true;
            this.pnHeader1.Size = new System.Drawing.Size(2326, 100);
            this.pnHeader1.TabIndex = 5;
            // 
            // guna2Button4
            // 
            this.guna2Button4.BackColor = System.Drawing.Color.Transparent;
            this.guna2Button4.BorderRadius = 15;
            this.guna2Button4.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button4.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button4.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2Button4.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2Button4.FillColor = System.Drawing.Color.WhiteSmoke;
            this.guna2Button4.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2Button4.ForeColor = System.Drawing.Color.White;
            this.guna2Button4.Image = ((System.Drawing.Image)(resources.GetObject("guna2Button4.Image")));
            this.guna2Button4.ImageSize = new System.Drawing.Size(40, 40);
            this.guna2Button4.Location = new System.Drawing.Point(424, 14);
            this.guna2Button4.Name = "guna2Button4";
            this.guna2Button4.ShadowDecoration.BorderRadius = 15;
            this.guna2Button4.ShadowDecoration.Enabled = true;
            this.guna2Button4.Size = new System.Drawing.Size(75, 68);
            this.guna2Button4.TabIndex = 10;
            // 
            // lblCustomerManagement
            // 
            this.lblCustomerManagement.AutoSize = false;
            this.lblCustomerManagement.BackColor = System.Drawing.Color.Transparent;
            this.lblCustomerManagement.Font = new System.Drawing.Font("Microsoft YaHei UI", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomerManagement.Location = new System.Drawing.Point(15, 23);
            this.lblCustomerManagement.Name = "lblCustomerManagement";
            this.lblCustomerManagement.Size = new System.Drawing.Size(836, 74);
            this.lblCustomerManagement.TabIndex = 2;
            this.lblCustomerManagement.Text = "Quản lý khách hàng";
            this.lblCustomerManagement.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            // 
            // dgvListCustomers
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.dgvListCustomers.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvListCustomers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dgvListCustomers.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Navy;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft YaHei UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.Navy;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvListCustomers.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvListCustomers.ColumnHeadersHeight = 40;
            this.dgvListCustomers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CustomersID,
            this.CustomersName,
            this.CustomersPhone,
            this.CustomersAddress,
            this.CustomersType});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvListCustomers.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvListCustomers.GridColor = System.Drawing.Color.Black;
            this.dgvListCustomers.Location = new System.Drawing.Point(11, 106);
            this.dgvListCustomers.Name = "dgvListCustomers";
            this.dgvListCustomers.RowHeadersVisible = false;
            this.dgvListCustomers.RowHeadersWidth = 51;
            this.dgvListCustomers.RowTemplate.Height = 35;
            this.dgvListCustomers.Size = new System.Drawing.Size(2022, 639);
            this.dgvListCustomers.TabIndex = 8;
            this.dgvListCustomers.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvListCustomers.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dgvListCustomers.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dgvListCustomers.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dgvListCustomers.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dgvListCustomers.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dgvListCustomers.ThemeStyle.GridColor = System.Drawing.Color.Black;
            this.dgvListCustomers.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            this.dgvListCustomers.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvListCustomers.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Microsoft YaHei UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvListCustomers.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgvListCustomers.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvListCustomers.ThemeStyle.HeaderStyle.Height = 40;
            this.dgvListCustomers.ThemeStyle.ReadOnly = false;
            this.dgvListCustomers.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvListCustomers.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvListCustomers.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvListCustomers.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgvListCustomers.ThemeStyle.RowsStyle.Height = 35;
            this.dgvListCustomers.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvListCustomers.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            // 
            // CustomersID
            // 
            this.CustomersID.DataPropertyName = "Mã khách hàng";
            this.CustomersID.HeaderText = "Mã khách hàng";
            this.CustomersID.MinimumWidth = 6;
            this.CustomersID.Name = "CustomersID";
            // 
            // CustomersName
            // 
            this.CustomersName.DataPropertyName = "Tên khách hàng";
            this.CustomersName.FillWeight = 150F;
            this.CustomersName.HeaderText = "Tên khách hàng";
            this.CustomersName.MinimumWidth = 6;
            this.CustomersName.Name = "CustomersName";
            // 
            // CustomersPhone
            // 
            this.CustomersPhone.DataPropertyName = "Số điện thoại";
            this.CustomersPhone.HeaderText = "Số điện thoại";
            this.CustomersPhone.MinimumWidth = 6;
            this.CustomersPhone.Name = "CustomersPhone";
            // 
            // CustomersAddress
            // 
            this.CustomersAddress.DataPropertyName = "Địa chỉ";
            this.CustomersAddress.FillWeight = 150F;
            this.CustomersAddress.HeaderText = "Địa chỉ ";
            this.CustomersAddress.MinimumWidth = 6;
            this.CustomersAddress.Name = "CustomersAddress";
            // 
            // CustomersType
            // 
            this.CustomersType.DataPropertyName = "Loại khách";
            this.CustomersType.HeaderText = "Loại khách";
            this.CustomersType.MinimumWidth = 6;
            this.CustomersType.Name = "CustomersType";
            // 
            // guna2HtmlLabel3
            // 
            this.guna2HtmlLabel3.AutoSize = false;
            this.guna2HtmlLabel3.BackColor = System.Drawing.Color.White;
            this.guna2HtmlLabel3.Font = new System.Drawing.Font("Microsoft YaHei UI", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel3.Location = new System.Drawing.Point(15, 15);
            this.guna2HtmlLabel3.Name = "guna2HtmlLabel3";
            this.guna2HtmlLabel3.Size = new System.Drawing.Size(890, 74);
            this.guna2HtmlLabel3.TabIndex = 4;
            this.guna2HtmlLabel3.Text = "Quản lý danh sách khách hàng";
            this.guna2HtmlLabel3.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            // 
            // guna2Panel3
            // 
            this.guna2Panel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.guna2Panel3.BackColor = System.Drawing.Color.Transparent;
            this.guna2Panel3.BorderRadius = 15;
            this.guna2Panel3.Controls.Add(this.guna2Button1);
            this.guna2Panel3.Controls.Add(this.guna2HtmlLabel3);
            this.guna2Panel3.Controls.Add(this.dgvListCustomers);
            this.guna2Panel3.FillColor = System.Drawing.Color.White;
            this.guna2Panel3.Location = new System.Drawing.Point(1, 401);
            this.guna2Panel3.Name = "guna2Panel3";
            this.guna2Panel3.ShadowDecoration.BorderRadius = 15;
            this.guna2Panel3.ShadowDecoration.Enabled = true;
            this.guna2Panel3.Size = new System.Drawing.Size(1911, 812);
            this.guna2Panel3.TabIndex = 10;
            // 
            // guna2Button1
            // 
            this.guna2Button1.BackColor = System.Drawing.Color.Transparent;
            this.guna2Button1.BorderRadius = 25;
            this.guna2Button1.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button1.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button1.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2Button1.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2Button1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.guna2Button1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.guna2Button1.ForeColor = System.Drawing.Color.White;
            this.guna2Button1.Image = ((System.Drawing.Image)(resources.GetObject("guna2Button1.Image")));
            this.guna2Button1.ImageSize = new System.Drawing.Size(40, 40);
            this.guna2Button1.Location = new System.Drawing.Point(631, 3);
            this.guna2Button1.Name = "guna2Button1";
            this.guna2Button1.ShadowDecoration.BorderRadius = 20;
            this.guna2Button1.ShadowDecoration.Color = System.Drawing.Color.Silver;
            this.guna2Button1.ShadowDecoration.Enabled = true;
            this.guna2Button1.Size = new System.Drawing.Size(94, 74);
            this.guna2Button1.TabIndex = 9;
            // 
            // CustomersList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1924, 1055);
            this.Controls.Add(this.pnHeader1);
            this.Controls.Add(this.guna2Panel3);
            this.Controls.Add(this.pnHeader2);
            this.Name = "CustomersList";
            this.Text = "CustomersList";
            this.Load += new System.EventHandler(this.CustomersList_Load);
            this.gnVIPCustomers.ResumeLayout(false);
            this.pnHeader2.ResumeLayout(false);
            this.gnTotalCustomers.ResumeLayout(false);
            this.gnTotalCustomers.PerformLayout();
            this.pnHeader1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvListCustomers)).EndInit();
            this.guna2Panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Guna.UI2.WinForms.Guna2Panel gnVIPCustomers;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblVIPCount;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblVIPCustomers;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel2;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel1;
        private Guna.UI2.WinForms.Guna2Button bntAddCustomers;
        private Guna.UI2.WinForms.Guna2Panel pnHeader2;
        private Guna.UI2.WinForms.Guna2TextBox tbFindCustomers;
        private Guna.UI2.WinForms.Guna2Panel pnHeader1;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblCustomerManagement;
        private Guna.UI2.WinForms.Guna2Button btnFind;
        private Guna.UI2.WinForms.Guna2DataGridView dgvListCustomers;
        private System.Windows.Forms.DataGridViewTextBoxColumn CustomersID;
        private System.Windows.Forms.DataGridViewTextBoxColumn CustomersName;
        private System.Windows.Forms.DataGridViewTextBoxColumn CustomersPhone;
        private System.Windows.Forms.DataGridViewTextBoxColumn CustomersAddress;
        private System.Windows.Forms.DataGridViewTextBoxColumn CustomersType;
        private Guna.UI2.WinForms.Guna2Button btnUpdateCustomers;
        private Guna.UI2.WinForms.Guna2Button btnDelectCustomers;
        private Guna.UI2.WinForms.Guna2Button guna2Button3;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblTotalCustomerCountNumber;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblTotalCustomerCount;
        private Guna.UI2.WinForms.Guna2Panel gnTotalCustomers;
        private Guna.UI2.WinForms.Guna2Button guna2Button2;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel3;
        private Guna.UI2.WinForms.Guna2Button guna2Button4;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel3;
        private Guna.UI2.WinForms.Guna2Button guna2Button1;
    }
}