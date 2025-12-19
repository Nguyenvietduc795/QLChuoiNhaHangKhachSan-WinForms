namespace QLChuoiNhaHangKhachSan.GUI
{
    partial class FormInventory
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormInventory));
            this.pnlSidebar = new Guna.UI2.WinForms.Guna2Panel();
            this.guna2Separator1 = new Guna.UI2.WinForms.Guna2Separator();
            this.btnLogout = new Guna.UI2.WinForms.Guna2Button();
            this.btnReports = new Guna.UI2.WinForms.Guna2Button();
            this.btnPayments = new Guna.UI2.WinForms.Guna2Button();
            this.btnInventory = new Guna.UI2.WinForms.Guna2Button();
            this.btnHotels = new Guna.UI2.WinForms.Guna2Button();
            this.btnRestaurants = new Guna.UI2.WinForms.Guna2Button();
            this.btnCustomers = new Guna.UI2.WinForms.Guna2Button();
            this.btnEmplyees = new Guna.UI2.WinForms.Guna2Button();
            this.btnHome = new Guna.UI2.WinForms.Guna2Button();
            this.lblSubLogo = new System.Windows.Forms.Label();
            this.lblLogo = new System.Windows.Forms.Label();
            this.pnlTopbar = new Guna.UI2.WinForms.Guna2Panel();
            this.lblSubtitle = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlMainWrapper = new Guna.UI2.WinForms.Guna2Panel();
            this.cardTotal = new Guna.UI2.WinForms.Guna2Panel();
            this.pnlIconTotal = new Guna.UI2.WinForms.Guna2Panel();
            this.iconBox = new FontAwesome.Sharp.IconPictureBox();
            this.chipTotal = new Guna.UI2.WinForms.Guna2Chip();
            this.lblTotal = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lblTotalCaption = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            this.lbStockLowCaption = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lbStockLow = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.cardLow = new Guna.UI2.WinForms.Guna2Chip();
            this.guna2Panel2 = new Guna.UI2.WinForms.Guna2Panel();
            this.guna2Panel3 = new Guna.UI2.WinForms.Guna2Panel();
            this.lbHealthyStockCaption = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lbHealthyStock = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.cardHealthy = new Guna.UI2.WinForms.Guna2Chip();
            this.guna2Panel4 = new Guna.UI2.WinForms.Guna2Panel();
            this.iconWarning = new FontAwesome.Sharp.IconPictureBox();
            this.iconCheck = new FontAwesome.Sharp.IconPictureBox();
            this.spFilter = new Guna.UI2.WinForms.Guna2Panel();
            this.tlpFilter = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cboLoaiKho = new Guna.UI2.WinForms.Guna2ComboBox();
            this.pnlSidebar.SuspendLayout();
            this.pnlTopbar.SuspendLayout();
            this.pnlMainWrapper.SuspendLayout();
            this.cardTotal.SuspendLayout();
            this.pnlIconTotal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconBox)).BeginInit();
            this.guna2Panel1.SuspendLayout();
            this.guna2Panel2.SuspendLayout();
            this.guna2Panel3.SuspendLayout();
            this.guna2Panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconWarning)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconCheck)).BeginInit();
            this.spFilter.SuspendLayout();
            this.tlpFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlSidebar
            // 
            this.pnlSidebar.Controls.Add(this.guna2Separator1);
            this.pnlSidebar.Controls.Add(this.btnLogout);
            this.pnlSidebar.Controls.Add(this.btnReports);
            this.pnlSidebar.Controls.Add(this.btnPayments);
            this.pnlSidebar.Controls.Add(this.btnInventory);
            this.pnlSidebar.Controls.Add(this.btnHotels);
            this.pnlSidebar.Controls.Add(this.btnRestaurants);
            this.pnlSidebar.Controls.Add(this.btnCustomers);
            this.pnlSidebar.Controls.Add(this.btnEmplyees);
            this.pnlSidebar.Controls.Add(this.btnHome);
            this.pnlSidebar.Controls.Add(this.lblSubLogo);
            this.pnlSidebar.Controls.Add(this.lblLogo);
            this.pnlSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSidebar.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(31)))), ((int)(((byte)(51)))));
            this.pnlSidebar.Location = new System.Drawing.Point(0, 0);
            this.pnlSidebar.Margin = new System.Windows.Forms.Padding(2);
            this.pnlSidebar.Name = "pnlSidebar";
            this.pnlSidebar.ShadowDecoration.Enabled = true;
            this.pnlSidebar.Size = new System.Drawing.Size(193, 727);
            this.pnlSidebar.TabIndex = 1;
            // 
            // guna2Separator1
            // 
            this.guna2Separator1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(31)))), ((int)(((byte)(51)))));
            this.guna2Separator1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.guna2Separator1.Location = new System.Drawing.Point(9, 84);
            this.guna2Separator1.Margin = new System.Windows.Forms.Padding(2);
            this.guna2Separator1.Name = "guna2Separator1";
            this.guna2Separator1.Size = new System.Drawing.Size(176, 8);
            this.guna2Separator1.TabIndex = 11;
            // 
            // btnLogout
            // 
            this.btnLogout.BorderColor = System.Drawing.Color.AliceBlue;
            this.btnLogout.BorderRadius = 10;
            this.btnLogout.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;
            this.btnLogout.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(68)))), ((int)(((byte)(124)))));
            this.btnLogout.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnLogout.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnLogout.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnLogout.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnLogout.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnLogout.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(31)))), ((int)(((byte)(51)))));
            this.btnLogout.FocusedColor = System.Drawing.Color.Transparent;
            this.btnLogout.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogout.ForeColor = System.Drawing.Color.White;
            this.btnLogout.Location = new System.Drawing.Point(0, 690);
            this.btnLogout.Margin = new System.Windows.Forms.Padding(2);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(193, 37);
            this.btnLogout.TabIndex = 10;
            this.btnLogout.Text = "Đăng xuất";
            this.btnLogout.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnLogout.TextOffset = new System.Drawing.Point(20, 0);
            // 
            // btnReports
            // 
            this.btnReports.BorderColor = System.Drawing.Color.AliceBlue;
            this.btnReports.BorderRadius = 10;
            this.btnReports.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;
            this.btnReports.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(68)))), ((int)(((byte)(124)))));
            this.btnReports.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnReports.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnReports.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnReports.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnReports.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(31)))), ((int)(((byte)(51)))));
            this.btnReports.FocusedColor = System.Drawing.Color.Transparent;
            this.btnReports.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReports.ForeColor = System.Drawing.Color.White;
            this.btnReports.Location = new System.Drawing.Point(24, 619);
            this.btnReports.Margin = new System.Windows.Forms.Padding(2);
            this.btnReports.Name = "btnReports";
            this.btnReports.Size = new System.Drawing.Size(135, 37);
            this.btnReports.TabIndex = 9;
            this.btnReports.Text = "Báo cáo";
            this.btnReports.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnReports.TextOffset = new System.Drawing.Point(20, 0);
            // 
            // btnPayments
            // 
            this.btnPayments.BorderColor = System.Drawing.Color.AliceBlue;
            this.btnPayments.BorderRadius = 10;
            this.btnPayments.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;
            this.btnPayments.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(68)))), ((int)(((byte)(124)))));
            this.btnPayments.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnPayments.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnPayments.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnPayments.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnPayments.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(31)))), ((int)(((byte)(51)))));
            this.btnPayments.FocusedColor = System.Drawing.Color.Transparent;
            this.btnPayments.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPayments.ForeColor = System.Drawing.Color.White;
            this.btnPayments.Location = new System.Drawing.Point(24, 552);
            this.btnPayments.Margin = new System.Windows.Forms.Padding(2);
            this.btnPayments.Name = "btnPayments";
            this.btnPayments.Size = new System.Drawing.Size(135, 37);
            this.btnPayments.TabIndex = 8;
            this.btnPayments.Text = "Thanh toán";
            this.btnPayments.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnPayments.TextOffset = new System.Drawing.Point(20, 0);
            // 
            // btnInventory
            // 
            this.btnInventory.BorderColor = System.Drawing.Color.AliceBlue;
            this.btnInventory.BorderRadius = 10;
            this.btnInventory.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;
            this.btnInventory.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(68)))), ((int)(((byte)(124)))));
            this.btnInventory.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnInventory.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnInventory.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnInventory.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnInventory.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(31)))), ((int)(((byte)(51)))));
            this.btnInventory.FocusedColor = System.Drawing.Color.Transparent;
            this.btnInventory.Font = new System.Drawing.Font("Segoe UI", 10.2F);
            this.btnInventory.ForeColor = System.Drawing.Color.White;
            this.btnInventory.Location = new System.Drawing.Point(24, 483);
            this.btnInventory.Margin = new System.Windows.Forms.Padding(2);
            this.btnInventory.Name = "btnInventory";
            this.btnInventory.Size = new System.Drawing.Size(135, 37);
            this.btnInventory.TabIndex = 7;
            this.btnInventory.Text = "Kho";
            this.btnInventory.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnInventory.TextOffset = new System.Drawing.Point(20, 0);
            // 
            // btnHotels
            // 
            this.btnHotels.BorderColor = System.Drawing.Color.AliceBlue;
            this.btnHotels.BorderRadius = 10;
            this.btnHotels.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;
            this.btnHotels.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(68)))), ((int)(((byte)(124)))));
            this.btnHotels.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnHotels.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnHotels.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnHotels.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnHotels.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(31)))), ((int)(((byte)(51)))));
            this.btnHotels.FocusedColor = System.Drawing.Color.Transparent;
            this.btnHotels.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHotels.ForeColor = System.Drawing.Color.White;
            this.btnHotels.Location = new System.Drawing.Point(24, 338);
            this.btnHotels.Margin = new System.Windows.Forms.Padding(2);
            this.btnHotels.Name = "btnHotels";
            this.btnHotels.Size = new System.Drawing.Size(135, 37);
            this.btnHotels.TabIndex = 6;
            this.btnHotels.Text = "Khách sạn";
            this.btnHotels.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnHotels.TextOffset = new System.Drawing.Point(20, 0);
            // 
            // btnRestaurants
            // 
            this.btnRestaurants.BorderColor = System.Drawing.Color.AliceBlue;
            this.btnRestaurants.BorderRadius = 10;
            this.btnRestaurants.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;
            this.btnRestaurants.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(68)))), ((int)(((byte)(124)))));
            this.btnRestaurants.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnRestaurants.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnRestaurants.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnRestaurants.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnRestaurants.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(31)))), ((int)(((byte)(51)))));
            this.btnRestaurants.FocusedColor = System.Drawing.Color.Transparent;
            this.btnRestaurants.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRestaurants.ForeColor = System.Drawing.Color.White;
            this.btnRestaurants.Location = new System.Drawing.Point(24, 411);
            this.btnRestaurants.Margin = new System.Windows.Forms.Padding(2);
            this.btnRestaurants.Name = "btnRestaurants";
            this.btnRestaurants.Size = new System.Drawing.Size(135, 37);
            this.btnRestaurants.TabIndex = 5;
            this.btnRestaurants.Text = "Nhà hàng";
            this.btnRestaurants.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnRestaurants.TextOffset = new System.Drawing.Point(20, 0);
            // 
            // btnCustomers
            // 
            this.btnCustomers.BorderColor = System.Drawing.Color.AliceBlue;
            this.btnCustomers.BorderRadius = 10;
            this.btnCustomers.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;
            this.btnCustomers.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(68)))), ((int)(((byte)(124)))));
            this.btnCustomers.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnCustomers.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnCustomers.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnCustomers.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnCustomers.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(31)))), ((int)(((byte)(51)))));
            this.btnCustomers.FocusedColor = System.Drawing.Color.Transparent;
            this.btnCustomers.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCustomers.ForeColor = System.Drawing.Color.White;
            this.btnCustomers.Location = new System.Drawing.Point(24, 264);
            this.btnCustomers.Margin = new System.Windows.Forms.Padding(2);
            this.btnCustomers.Name = "btnCustomers";
            this.btnCustomers.Size = new System.Drawing.Size(135, 37);
            this.btnCustomers.TabIndex = 4;
            this.btnCustomers.Text = "Khách hàng";
            this.btnCustomers.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnCustomers.TextOffset = new System.Drawing.Point(20, 0);
            // 
            // btnEmplyees
            // 
            this.btnEmplyees.BorderColor = System.Drawing.Color.AliceBlue;
            this.btnEmplyees.BorderRadius = 10;
            this.btnEmplyees.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;
            this.btnEmplyees.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(68)))), ((int)(((byte)(124)))));
            this.btnEmplyees.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnEmplyees.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnEmplyees.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnEmplyees.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnEmplyees.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(31)))), ((int)(((byte)(51)))));
            this.btnEmplyees.FocusedColor = System.Drawing.Color.Transparent;
            this.btnEmplyees.Font = new System.Drawing.Font("Segoe UI", 10.2F);
            this.btnEmplyees.ForeColor = System.Drawing.Color.White;
            this.btnEmplyees.Location = new System.Drawing.Point(24, 196);
            this.btnEmplyees.Margin = new System.Windows.Forms.Padding(2);
            this.btnEmplyees.Name = "btnEmplyees";
            this.btnEmplyees.Size = new System.Drawing.Size(135, 37);
            this.btnEmplyees.TabIndex = 3;
            this.btnEmplyees.Text = "Nhân viên";
            this.btnEmplyees.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnEmplyees.TextOffset = new System.Drawing.Point(20, 0);
            // 
            // btnHome
            // 
            this.btnHome.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(31)))), ((int)(((byte)(51)))));
            this.btnHome.BorderRadius = 10;
            this.btnHome.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;
            this.btnHome.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(68)))), ((int)(((byte)(124)))));
            this.btnHome.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnHome.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnHome.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnHome.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnHome.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(31)))), ((int)(((byte)(51)))));
            this.btnHome.FocusedColor = System.Drawing.Color.Transparent;
            this.btnHome.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHome.ForeColor = System.Drawing.Color.White;
            this.btnHome.Image = ((System.Drawing.Image)(resources.GetObject("btnHome.Image")));
            this.btnHome.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnHome.ImageSize = new System.Drawing.Size(25, 25);
            this.btnHome.Location = new System.Drawing.Point(24, 125);
            this.btnHome.Margin = new System.Windows.Forms.Padding(2);
            this.btnHome.Name = "btnHome";
            this.btnHome.Size = new System.Drawing.Size(135, 37);
            this.btnHome.TabIndex = 2;
            this.btnHome.Text = "Trang chủ";
            this.btnHome.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnHome.TextOffset = new System.Drawing.Point(20, 0);
            // 
            // lblSubLogo
            // 
            this.lblSubLogo.AutoSize = true;
            this.lblSubLogo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(31)))), ((int)(((byte)(51)))));
            this.lblSubLogo.Font = new System.Drawing.Font("Microsoft YaHei UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSubLogo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblSubLogo.Location = new System.Drawing.Point(28, 52);
            this.lblSubLogo.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSubLogo.Name = "lblSubLogo";
            this.lblSubLogo.Size = new System.Drawing.Size(151, 20);
            this.lblSubLogo.TabIndex = 1;
            this.lblSubLogo.Text = "Management System";
            // 
            // lblLogo
            // 
            this.lblLogo.AutoSize = true;
            this.lblLogo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(31)))), ((int)(((byte)(51)))));
            this.lblLogo.Font = new System.Drawing.Font("Microsoft YaHei UI", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLogo.ForeColor = System.Drawing.Color.White;
            this.lblLogo.Location = new System.Drawing.Point(18, 7);
            this.lblLogo.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblLogo.Name = "lblLogo";
            this.lblLogo.Size = new System.Drawing.Size(169, 36);
            this.lblLogo.TabIndex = 0;
            this.lblLogo.Text = "RH-GROUP";
            // 
            // pnlTopbar
            // 
            this.pnlTopbar.Controls.Add(this.lblSubtitle);
            this.pnlTopbar.Controls.Add(this.lblTitle);
            this.pnlTopbar.FillColor = System.Drawing.Color.White;
            this.pnlTopbar.Location = new System.Drawing.Point(192, 0);
            this.pnlTopbar.Name = "pnlTopbar";
            this.pnlTopbar.ShadowDecoration.Depth = 1;
            this.pnlTopbar.ShadowDecoration.Enabled = true;
            this.pnlTopbar.Size = new System.Drawing.Size(872, 92);
            this.pnlTopbar.TabIndex = 2;
            // 
            // lblSubtitle
            // 
            this.lblSubtitle.BackColor = System.Drawing.Color.Transparent;
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSubtitle.Location = new System.Drawing.Point(46, 56);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(293, 17);
            this.lblSubtitle.TabIndex = 1;
            this.lblSubtitle.Text = "Theo dõi tồn kho nguyên liệu & thiết bị theo hệ thống.";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.BackColor = System.Drawing.Color.White;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(40, 13);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(160, 32);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Mức tồn kho";
            // 
            // pnlMainWrapper
            // 
            this.pnlMainWrapper.BackColor = System.Drawing.Color.Transparent;
            this.pnlMainWrapper.Controls.Add(this.spFilter);
            this.pnlMainWrapper.Controls.Add(this.guna2Panel3);
            this.pnlMainWrapper.Controls.Add(this.guna2Panel1);
            this.pnlMainWrapper.Controls.Add(this.cardTotal);
            this.pnlMainWrapper.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(251)))));
            this.pnlMainWrapper.Location = new System.Drawing.Point(192, 84);
            this.pnlMainWrapper.Name = "pnlMainWrapper";
            this.pnlMainWrapper.ShadowDecoration.BorderRadius = 12;
            this.pnlMainWrapper.ShadowDecoration.Depth = 1;
            this.pnlMainWrapper.ShadowDecoration.Enabled = true;
            this.pnlMainWrapper.Size = new System.Drawing.Size(872, 643);
            this.pnlMainWrapper.TabIndex = 3;
            this.pnlMainWrapper.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlMainWrapper_Paint);
            // 
            // cardTotal
            // 
            this.cardTotal.BackColor = System.Drawing.Color.Transparent;
            this.cardTotal.BorderRadius = 14;
            this.cardTotal.Controls.Add(this.lblTotalCaption);
            this.cardTotal.Controls.Add(this.lblTotal);
            this.cardTotal.Controls.Add(this.chipTotal);
            this.cardTotal.Controls.Add(this.pnlIconTotal);
            this.cardTotal.FillColor = System.Drawing.Color.White;
            this.cardTotal.Location = new System.Drawing.Point(17, 28);
            this.cardTotal.Name = "cardTotal";
            this.cardTotal.Padding = new System.Windows.Forms.Padding(16);
            this.cardTotal.ShadowDecoration.Depth = 2;
            this.cardTotal.ShadowDecoration.Enabled = true;
            this.cardTotal.Size = new System.Drawing.Size(236, 92);
            this.cardTotal.TabIndex = 0;
            // 
            // pnlIconTotal
            // 
            this.pnlIconTotal.Controls.Add(this.iconBox);
            this.pnlIconTotal.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(242)))), ((int)(((byte)(255)))));
            this.pnlIconTotal.Location = new System.Drawing.Point(19, 13);
            this.pnlIconTotal.Name = "pnlIconTotal";
            this.pnlIconTotal.Size = new System.Drawing.Size(36, 36);
            this.pnlIconTotal.TabIndex = 0;
            // 
            // iconBox
            // 
            this.iconBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(242)))), ((int)(((byte)(255)))));
            this.iconBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("iconBox.BackgroundImage")));
            this.iconBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.iconBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.iconBox.IconChar = FontAwesome.Sharp.IconChar.None;
            this.iconBox.IconColor = System.Drawing.SystemColors.ControlText;
            this.iconBox.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconBox.IconSize = 15;
            this.iconBox.Location = new System.Drawing.Point(10, 3);
            this.iconBox.Name = "iconBox";
            this.iconBox.Size = new System.Drawing.Size(15, 30);
            this.iconBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.iconBox.TabIndex = 0;
            this.iconBox.TabStop = false;
            // 
            // chipTotal
            // 
            this.chipTotal.BorderColor = System.Drawing.Color.White;
            this.chipTotal.BorderRadius = 12;
            this.chipTotal.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(242)))), ((int)(((byte)(255)))));
            this.chipTotal.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chipTotal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(70)))), ((int)(((byte)(229)))));
            this.chipTotal.IsClosable = false;
            this.chipTotal.Location = new System.Drawing.Point(126, 16);
            this.chipTotal.Name = "chipTotal";
            this.chipTotal.Size = new System.Drawing.Size(100, 27);
            this.chipTotal.TabIndex = 0;
            this.chipTotal.Text = "Hàng tồn kho";
            this.chipTotal.Click += new System.EventHandler(this.chipTotal_Click);
            // 
            // lblTotal
            // 
            this.lblTotal.BackColor = System.Drawing.Color.Transparent;
            this.lblTotal.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotal.Location = new System.Drawing.Point(20, 53);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(43, 22);
            this.lblTotal.TabIndex = 1;
            this.lblTotal.Text = "1,248";
            // 
            // lblTotalCaption
            // 
            this.lblTotalCaption.BackColor = System.Drawing.Color.Transparent;
            this.lblTotalCaption.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalCaption.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblTotalCaption.Location = new System.Drawing.Point(20, 74);
            this.lblTotalCaption.Name = "lblTotalCaption";
            this.lblTotalCaption.Size = new System.Drawing.Size(99, 15);
            this.lblTotalCaption.TabIndex = 2;
            this.lblTotalCaption.Text = "Tổng số lượng tồn";
            // 
            // guna2Panel1
            // 
            this.guna2Panel1.BackColor = System.Drawing.Color.Transparent;
            this.guna2Panel1.BorderRadius = 14;
            this.guna2Panel1.Controls.Add(this.lbStockLowCaption);
            this.guna2Panel1.Controls.Add(this.lbStockLow);
            this.guna2Panel1.Controls.Add(this.cardLow);
            this.guna2Panel1.Controls.Add(this.guna2Panel2);
            this.guna2Panel1.FillColor = System.Drawing.Color.White;
            this.guna2Panel1.Location = new System.Drawing.Point(324, 28);
            this.guna2Panel1.Name = "guna2Panel1";
            this.guna2Panel1.Padding = new System.Windows.Forms.Padding(16);
            this.guna2Panel1.ShadowDecoration.BorderRadius = 12;
            this.guna2Panel1.ShadowDecoration.Depth = 2;
            this.guna2Panel1.ShadowDecoration.Enabled = true;
            this.guna2Panel1.Size = new System.Drawing.Size(236, 92);
            this.guna2Panel1.TabIndex = 1;
            // 
            // lbStockLowCaption
            // 
            this.lbStockLowCaption.BackColor = System.Drawing.Color.Transparent;
            this.lbStockLowCaption.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbStockLowCaption.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lbStockLowCaption.Location = new System.Drawing.Point(20, 74);
            this.lbStockLowCaption.Name = "lbStockLowCaption";
            this.lbStockLowCaption.Size = new System.Drawing.Size(102, 15);
            this.lbStockLowCaption.TabIndex = 2;
            this.lbStockLowCaption.Text = "Hàng tồn kho thấp";
            // 
            // lbStockLow
            // 
            this.lbStockLow.BackColor = System.Drawing.Color.Transparent;
            this.lbStockLow.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbStockLow.Location = new System.Drawing.Point(20, 53);
            this.lbStockLow.Name = "lbStockLow";
            this.lbStockLow.Size = new System.Drawing.Size(21, 22);
            this.lbStockLow.TabIndex = 1;
            this.lbStockLow.Text = "18";
            this.lbStockLow.Click += new System.EventHandler(this.guna2HtmlLabel3_Click);
            // 
            // cardLow
            // 
            this.cardLow.BorderColor = System.Drawing.Color.White;
            this.cardLow.BorderRadius = 12;
            this.cardLow.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(243)))), ((int)(((byte)(199)))));
            this.cardLow.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cardLow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(83)))), ((int)(((byte)(9)))));
            this.cardLow.IsClosable = false;
            this.cardLow.Location = new System.Drawing.Point(167, 13);
            this.cardLow.Name = "cardLow";
            this.cardLow.Size = new System.Drawing.Size(59, 30);
            this.cardLow.TabIndex = 0;
            this.cardLow.Text = "Chú ý";
            // 
            // guna2Panel2
            // 
            this.guna2Panel2.Controls.Add(this.iconWarning);
            this.guna2Panel2.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(247)))), ((int)(((byte)(237)))));
            this.guna2Panel2.Location = new System.Drawing.Point(19, 13);
            this.guna2Panel2.Name = "guna2Panel2";
            this.guna2Panel2.Size = new System.Drawing.Size(36, 36);
            this.guna2Panel2.TabIndex = 0;
            // 
            // guna2Panel3
            // 
            this.guna2Panel3.BackColor = System.Drawing.Color.Transparent;
            this.guna2Panel3.BorderRadius = 14;
            this.guna2Panel3.Controls.Add(this.lbHealthyStockCaption);
            this.guna2Panel3.Controls.Add(this.lbHealthyStock);
            this.guna2Panel3.Controls.Add(this.cardHealthy);
            this.guna2Panel3.Controls.Add(this.guna2Panel4);
            this.guna2Panel3.FillColor = System.Drawing.Color.White;
            this.guna2Panel3.Location = new System.Drawing.Point(622, 28);
            this.guna2Panel3.Name = "guna2Panel3";
            this.guna2Panel3.Padding = new System.Windows.Forms.Padding(16);
            this.guna2Panel3.ShadowDecoration.BorderRadius = 12;
            this.guna2Panel3.ShadowDecoration.Depth = 2;
            this.guna2Panel3.ShadowDecoration.Enabled = true;
            this.guna2Panel3.Size = new System.Drawing.Size(236, 92);
            this.guna2Panel3.TabIndex = 2;
            // 
            // lbHealthyStockCaption
            // 
            this.lbHealthyStockCaption.BackColor = System.Drawing.Color.Transparent;
            this.lbHealthyStockCaption.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbHealthyStockCaption.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lbHealthyStockCaption.Location = new System.Drawing.Point(20, 74);
            this.lbHealthyStockCaption.Name = "lbHealthyStockCaption";
            this.lbHealthyStockCaption.Size = new System.Drawing.Size(89, 15);
            this.lbHealthyStockCaption.TabIndex = 2;
            this.lbHealthyStockCaption.Text = "Tồn kho ổn định";
            // 
            // lbHealthyStock
            // 
            this.lbHealthyStock.BackColor = System.Drawing.Color.Transparent;
            this.lbHealthyStock.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbHealthyStock.Location = new System.Drawing.Point(20, 53);
            this.lbHealthyStock.Name = "lbHealthyStock";
            this.lbHealthyStock.Size = new System.Drawing.Size(38, 22);
            this.lbHealthyStock.TabIndex = 1;
            this.lbHealthyStock.Text = "97 %";
            // 
            // cardHealthy
            // 
            this.cardHealthy.BorderColor = System.Drawing.Color.White;
            this.cardHealthy.BorderRadius = 12;
            this.cardHealthy.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(250)))), ((int)(((byte)(229)))));
            this.cardHealthy.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cardHealthy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(4)))), ((int)(((byte)(120)))), ((int)(((byte)(87)))));
            this.cardHealthy.IsClosable = false;
            this.cardHealthy.Location = new System.Drawing.Point(174, 13);
            this.cardHealthy.Name = "cardHealthy";
            this.cardHealthy.Size = new System.Drawing.Size(52, 30);
            this.cardHealthy.TabIndex = 0;
            this.cardHealthy.Text = "Tốt";
            // 
            // guna2Panel4
            // 
            this.guna2Panel4.Controls.Add(this.iconCheck);
            this.guna2Panel4.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(253)))), ((int)(((byte)(245)))));
            this.guna2Panel4.Location = new System.Drawing.Point(19, 13);
            this.guna2Panel4.Name = "guna2Panel4";
            this.guna2Panel4.Size = new System.Drawing.Size(36, 36);
            this.guna2Panel4.TabIndex = 0;
            // 
            // iconWarning
            // 
            this.iconWarning.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(247)))), ((int)(((byte)(237)))));
            this.iconWarning.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("iconWarning.BackgroundImage")));
            this.iconWarning.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.iconWarning.ForeColor = System.Drawing.SystemColors.ControlText;
            this.iconWarning.IconChar = FontAwesome.Sharp.IconChar.None;
            this.iconWarning.IconColor = System.Drawing.SystemColors.ControlText;
            this.iconWarning.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconWarning.IconSize = 15;
            this.iconWarning.Location = new System.Drawing.Point(11, 3);
            this.iconWarning.Name = "iconWarning";
            this.iconWarning.Size = new System.Drawing.Size(15, 30);
            this.iconWarning.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.iconWarning.TabIndex = 1;
            this.iconWarning.TabStop = false;
            // 
            // iconCheck
            // 
            this.iconCheck.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(253)))), ((int)(((byte)(245)))));
            this.iconCheck.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("iconCheck.BackgroundImage")));
            this.iconCheck.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.iconCheck.ForeColor = System.Drawing.SystemColors.ControlText;
            this.iconCheck.IconChar = FontAwesome.Sharp.IconChar.None;
            this.iconCheck.IconColor = System.Drawing.SystemColors.ControlText;
            this.iconCheck.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconCheck.IconSize = 15;
            this.iconCheck.Location = new System.Drawing.Point(11, 3);
            this.iconCheck.Name = "iconCheck";
            this.iconCheck.Size = new System.Drawing.Size(15, 30);
            this.iconCheck.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.iconCheck.TabIndex = 2;
            this.iconCheck.TabStop = false;
            // 
            // spFilter
            // 
            this.spFilter.BorderRadius = 14;
            this.spFilter.Controls.Add(this.tlpFilter);
            this.spFilter.FillColor = System.Drawing.Color.White;
            this.spFilter.Location = new System.Drawing.Point(17, 137);
            this.spFilter.Name = "spFilter";
            this.spFilter.ShadowDecoration.BorderRadius = 16;
            this.spFilter.ShadowDecoration.Color = System.Drawing.Color.Gray;
            this.spFilter.ShadowDecoration.Depth = 8;
            this.spFilter.ShadowDecoration.Enabled = true;
            this.spFilter.Size = new System.Drawing.Size(841, 70);
            this.spFilter.TabIndex = 3;
            // 
            // tlpFilter
            // 
            this.tlpFilter.ColumnCount = 5;
            this.tlpFilter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 46.05598F));
            this.tlpFilter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 53.94402F));
            this.tlpFilter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 181F));
            this.tlpFilter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 137F));
            this.tlpFilter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 129F));
            this.tlpFilter.Controls.Add(this.label3, 2, 0);
            this.tlpFilter.Controls.Add(this.label2, 1, 0);
            this.tlpFilter.Controls.Add(this.label1, 0, 0);
            this.tlpFilter.Controls.Add(this.cboLoaiKho, 0, 1);
            this.tlpFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpFilter.Location = new System.Drawing.Point(0, 0);
            this.tlpFilter.Name = "tlpFilter";
            this.tlpFilter.RowCount = 2;
            this.tlpFilter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFilter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tlpFilter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpFilter.Size = new System.Drawing.Size(841, 70);
            this.tlpFilter.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(175, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "Loại Kho";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.label2.Location = new System.Drawing.Point(184, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(206, 24);
            this.label2.TabIndex = 1;
            this.label2.Text = "Đơn vị (NH/KS)";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.label3.Location = new System.Drawing.Point(396, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(175, 24);
            this.label3.TabIndex = 2;
            this.label3.Text = "Tìm kiếm";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboLoaiKho
            // 
            this.cboLoaiKho.BackColor = System.Drawing.Color.Transparent;
            this.cboLoaiKho.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.cboLoaiKho.BorderRadius = 10;
            this.cboLoaiKho.Dock = System.Windows.Forms.DockStyle.Right;
            this.cboLoaiKho.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboLoaiKho.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLoaiKho.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cboLoaiKho.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cboLoaiKho.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cboLoaiKho.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cboLoaiKho.ItemHeight = 30;
            this.cboLoaiKho.Location = new System.Drawing.Point(38, 27);
            this.cboLoaiKho.Name = "cboLoaiKho";
            this.cboLoaiKho.Size = new System.Drawing.Size(140, 36);
            this.cboLoaiKho.TabIndex = 3;
            this.cboLoaiKho.SelectedIndexChanged += new System.EventHandler(this.guna2ComboBox1_SelectedIndexChanged);
            // 
            // FormInventory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1062, 727);
            this.Controls.Add(this.pnlMainWrapper);
            this.Controls.Add(this.pnlTopbar);
            this.Controls.Add(this.pnlSidebar);
            this.Name = "FormInventory";
            this.Text = "FormInventory";
            this.pnlSidebar.ResumeLayout(false);
            this.pnlSidebar.PerformLayout();
            this.pnlTopbar.ResumeLayout(false);
            this.pnlTopbar.PerformLayout();
            this.pnlMainWrapper.ResumeLayout(false);
            this.cardTotal.ResumeLayout(false);
            this.cardTotal.PerformLayout();
            this.pnlIconTotal.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.iconBox)).EndInit();
            this.guna2Panel1.ResumeLayout(false);
            this.guna2Panel1.PerformLayout();
            this.guna2Panel2.ResumeLayout(false);
            this.guna2Panel3.ResumeLayout(false);
            this.guna2Panel3.PerformLayout();
            this.guna2Panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.iconWarning)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconCheck)).EndInit();
            this.spFilter.ResumeLayout(false);
            this.tlpFilter.ResumeLayout(false);
            this.tlpFilter.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel pnlSidebar;
        private Guna.UI2.WinForms.Guna2Separator guna2Separator1;
        private Guna.UI2.WinForms.Guna2Button btnLogout;
        private Guna.UI2.WinForms.Guna2Button btnReports;
        private Guna.UI2.WinForms.Guna2Button btnPayments;
        private Guna.UI2.WinForms.Guna2Button btnInventory;
        private Guna.UI2.WinForms.Guna2Button btnHotels;
        private Guna.UI2.WinForms.Guna2Button btnRestaurants;
        private Guna.UI2.WinForms.Guna2Button btnCustomers;
        private Guna.UI2.WinForms.Guna2Button btnEmplyees;
        private Guna.UI2.WinForms.Guna2Button btnHome;
        private System.Windows.Forms.Label lblSubLogo;
        private System.Windows.Forms.Label lblLogo;
        private Guna.UI2.WinForms.Guna2Panel pnlTopbar;
        private System.Windows.Forms.Label lblTitle;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblSubtitle;
        private Guna.UI2.WinForms.Guna2Panel pnlMainWrapper;
        private Guna.UI2.WinForms.Guna2Panel cardTotal;
        private Guna.UI2.WinForms.Guna2Panel pnlIconTotal;
        private FontAwesome.Sharp.IconPictureBox iconBox;
        private Guna.UI2.WinForms.Guna2Chip chipTotal;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblTotalCaption;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblTotal;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel3;
        private Guna.UI2.WinForms.Guna2HtmlLabel lbHealthyStockCaption;
        private Guna.UI2.WinForms.Guna2HtmlLabel lbHealthyStock;
        private Guna.UI2.WinForms.Guna2Chip cardHealthy;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel4;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private Guna.UI2.WinForms.Guna2HtmlLabel lbStockLowCaption;
        private Guna.UI2.WinForms.Guna2HtmlLabel lbStockLow;
        private Guna.UI2.WinForms.Guna2Chip cardLow;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel2;
        private FontAwesome.Sharp.IconPictureBox iconWarning;
        private FontAwesome.Sharp.IconPictureBox iconCheck;
        private Guna.UI2.WinForms.Guna2Panel spFilter;
        private System.Windows.Forms.TableLayoutPanel tlpFilter;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private Guna.UI2.WinForms.Guna2ComboBox cboLoaiKho;
    }
}