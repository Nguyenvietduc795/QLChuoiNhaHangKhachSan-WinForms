namespace QLChuoiNhaHangKhachSan.GUI
{
    partial class SalaryManageForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SalaryManageForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.bldSalaryManageForm = new Guna.UI2.WinForms.Guna2BorderlessForm(this.components);
            this.lblSalaryManage_SubTitle = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lblSalaryManage_Title = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.pnlTotalPayroll = new Guna.UI2.WinForms.Guna2Panel();
            this.lblTotalMoney = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lblTotalPayroll = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.btnPayroll = new Guna.UI2.WinForms.Guna2Button();
            this.pnlTotalEmployees = new Guna.UI2.WinForms.Guna2Panel();
            this.lblActiveEmployees = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lblTotalActiveEmployees = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.btnActiveEmployees = new Guna.UI2.WinForms.Guna2Button();
            this.pnlAvargeSalary = new Guna.UI2.WinForms.Guna2Panel();
            this.lblAverageMoney = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lblAverageSalary = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.btnAveargeSalary = new Guna.UI2.WinForms.Guna2Button();
            this.pnlSalaryEmployees = new Guna.UI2.WinForms.Guna2Panel();
            this.dgvSalaryEmployees = new Guna.UI2.WinForms.Guna2DataGridView();
            this.colEmployeeFullName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEmployeeDepartment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEmployeeAvatar = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEmployeeSalary = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEmployeeFrequency = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEmployeeStatus = new System.Windows.Forms.DataGridViewButtonColumn();
            this.lblSalaryEmployee = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.summaryLayout = new System.Windows.Forms.TableLayoutPanel();
            this.mainLayout = new System.Windows.Forms.TableLayoutPanel();
            this.pnlTotalPayroll.SuspendLayout();
            this.pnlTotalEmployees.SuspendLayout();
            this.pnlAvargeSalary.SuspendLayout();
            this.pnlSalaryEmployees.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSalaryEmployees)).BeginInit();
            this.summaryLayout.SuspendLayout();
            this.mainLayout.SuspendLayout();
            this.SuspendLayout();
            // 
            // bldSalaryManageForm
            // 
            this.bldSalaryManageForm.BorderRadius = 10;
            this.bldSalaryManageForm.ContainerControl = this;
            this.bldSalaryManageForm.DockIndicatorTransparencyValue = 0.6D;
            this.bldSalaryManageForm.TransparentWhileDrag = true;
            // 
            // lblSalaryManage_SubTitle
            // 
            this.lblSalaryManage_SubTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblSalaryManage_SubTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSalaryManage_SubTitle.ForeColor = System.Drawing.Color.Gray;
            this.lblSalaryManage_SubTitle.Location = new System.Drawing.Point(36, 80);
            this.lblSalaryManage_SubTitle.Name = "lblSalaryManage_SubTitle";
            this.lblSalaryManage_SubTitle.Size = new System.Drawing.Size(322, 22);
            this.lblSalaryManage_SubTitle.TabIndex = 3;
            this.lblSalaryManage_SubTitle.Text = "Tổng quan về lương và thông tin thanh toán";
            // 
            // lblSalaryManage_Title
            // 
            this.lblSalaryManage_Title.BackColor = System.Drawing.Color.Transparent;
            this.lblSalaryManage_Title.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSalaryManage_Title.Location = new System.Drawing.Point(30, 20);
            this.lblSalaryManage_Title.Name = "lblSalaryManage_Title";
            this.lblSalaryManage_Title.Size = new System.Drawing.Size(366, 56);
            this.lblSalaryManage_Title.TabIndex = 2;
            this.lblSalaryManage_Title.Text = "Quản lí bảng lương";
            // 
            // pnlTotalPayroll
            // 
            this.pnlTotalPayroll.BackColor = System.Drawing.Color.Transparent;
            this.pnlTotalPayroll.BorderRadius = 12;
            this.pnlTotalPayroll.Controls.Add(this.lblTotalMoney);
            this.pnlTotalPayroll.Controls.Add(this.lblTotalPayroll);
            this.pnlTotalPayroll.Controls.Add(this.btnPayroll);
            this.pnlTotalPayroll.FillColor = System.Drawing.Color.White;
            this.pnlTotalPayroll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTotalPayroll.Margin = new System.Windows.Forms.Padding(10);
            this.pnlTotalPayroll.ShadowDecoration.Depth = 5;
            this.pnlTotalPayroll.ShadowDecoration.Enabled = true;
            this.pnlTotalPayroll.Size = new System.Drawing.Size(328, 132);
            this.pnlTotalPayroll.TabIndex = 4;
            // 
            // lblTotalMoney
            // 
            this.lblTotalMoney.BackColor = System.Drawing.Color.White;
            this.lblTotalMoney.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalMoney.Location = new System.Drawing.Point(21, 76);
            this.lblTotalMoney.Margin = new System.Windows.Forms.Padding(4);
            this.lblTotalMoney.Name = "lblTotalMoney";
            this.lblTotalMoney.Size = new System.Drawing.Size(69, 42);
            this.lblTotalMoney.TabIndex = 8;
            this.lblTotalMoney.Text = "$0,0";
            // 
            // lblTotalPayroll
            // 
            this.lblTotalPayroll.BackColor = System.Drawing.Color.White;
            this.lblTotalPayroll.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalPayroll.ForeColor = System.Drawing.Color.Black;
            this.lblTotalPayroll.Location = new System.Drawing.Point(113, 24);
            this.lblTotalPayroll.Margin = new System.Windows.Forms.Padding(4);
            this.lblTotalPayroll.Name = "lblTotalPayroll";
            this.lblTotalPayroll.Size = new System.Drawing.Size(188, 22);
            this.lblTotalPayroll.TabIndex = 7;
            this.lblTotalPayroll.Text = "Tổng tiền lương hàng tháng";
            // 
            // btnPayroll
            // 
            this.btnPayroll.BorderRadius = 10;
            this.btnPayroll.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnPayroll.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnPayroll.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnPayroll.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnPayroll.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(240)))), ((int)(((byte)(254)))));
            this.btnPayroll.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnPayroll.ForeColor = System.Drawing.Color.White;
            this.btnPayroll.Image = ((System.Drawing.Image)(resources.GetObject("btnPayroll.Image")));
            this.btnPayroll.ImageSize = new System.Drawing.Size(35, 35);
            this.btnPayroll.Location = new System.Drawing.Point(21, 14);
            this.btnPayroll.Margin = new System.Windows.Forms.Padding(4);
            this.btnPayroll.Name = "btnPayroll";
            this.btnPayroll.Size = new System.Drawing.Size(71, 38);
            this.btnPayroll.TabIndex = 4;
            // 
            // pnlTotalEmployees
            // 
            this.pnlTotalEmployees.BackColor = System.Drawing.Color.Transparent;
            this.pnlTotalEmployees.BorderRadius = 12;
            this.pnlTotalEmployees.Controls.Add(this.lblActiveEmployees);
            this.pnlTotalEmployees.Controls.Add(this.lblTotalActiveEmployees);
            this.pnlTotalEmployees.Controls.Add(this.btnActiveEmployees);
            this.pnlTotalEmployees.FillColor = System.Drawing.Color.White;
            this.pnlTotalEmployees.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTotalEmployees.Margin = new System.Windows.Forms.Padding(10);
            this.pnlTotalEmployees.ShadowDecoration.Depth = 5;
            this.pnlTotalEmployees.ShadowDecoration.Enabled = true;
            this.pnlTotalEmployees.Size = new System.Drawing.Size(325, 132);
            this.pnlTotalEmployees.TabIndex = 5;
            // 
            // lblActiveEmployees
            // 
            this.lblActiveEmployees.BackColor = System.Drawing.Color.White;
            this.lblActiveEmployees.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblActiveEmployees.Location = new System.Drawing.Point(21, 76);
            this.lblActiveEmployees.Margin = new System.Windows.Forms.Padding(4);
            this.lblActiveEmployees.Name = "lblActiveEmployees";
            this.lblActiveEmployees.Size = new System.Drawing.Size(22, 42);
            this.lblActiveEmployees.TabIndex = 8;
            this.lblActiveEmployees.Text = "0";
            // 
            // lblTotalActiveEmployees
            // 
            this.lblTotalActiveEmployees.BackColor = System.Drawing.Color.White;
            this.lblTotalActiveEmployees.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalActiveEmployees.ForeColor = System.Drawing.Color.Black;
            this.lblTotalActiveEmployees.Location = new System.Drawing.Point(113, 24);
            this.lblTotalActiveEmployees.Margin = new System.Windows.Forms.Padding(4);
            this.lblTotalActiveEmployees.Name = "lblTotalActiveEmployees";
            this.lblTotalActiveEmployees.Size = new System.Drawing.Size(123, 22);
            this.lblTotalActiveEmployees.TabIndex = 7;
            this.lblTotalActiveEmployees.Text = "Tổng số nhân viên";
            // 
            // btnActiveEmployees
            // 
            this.btnActiveEmployees.BorderRadius = 10;
            this.btnActiveEmployees.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnActiveEmployees.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnActiveEmployees.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnActiveEmployees.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnActiveEmployees.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(240)))), ((int)(((byte)(254)))));
            this.btnActiveEmployees.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnActiveEmployees.ForeColor = System.Drawing.Color.White;
            this.btnActiveEmployees.Image = ((System.Drawing.Image)(resources.GetObject("btnActiveEmployees.Image")));
            this.btnActiveEmployees.ImageSize = new System.Drawing.Size(35, 35);
            this.btnActiveEmployees.Location = new System.Drawing.Point(21, 14);
            this.btnActiveEmployees.Margin = new System.Windows.Forms.Padding(4);
            this.btnActiveEmployees.Name = "btnActiveEmployees";
            this.btnActiveEmployees.Size = new System.Drawing.Size(71, 38);
            this.btnActiveEmployees.TabIndex = 4;
            // 
            // pnlAvargeSalary
            // 
            this.pnlAvargeSalary.BackColor = System.Drawing.Color.Transparent;
            this.pnlAvargeSalary.BorderRadius = 12;
            this.pnlAvargeSalary.Controls.Add(this.lblAverageMoney);
            this.pnlAvargeSalary.Controls.Add(this.lblAverageSalary);
            this.pnlAvargeSalary.Controls.Add(this.btnAveargeSalary);
            this.pnlAvargeSalary.FillColor = System.Drawing.Color.White;
            this.pnlAvargeSalary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlAvargeSalary.Margin = new System.Windows.Forms.Padding(10);
            this.pnlAvargeSalary.ShadowDecoration.Depth = 5;
            this.pnlAvargeSalary.ShadowDecoration.Enabled = true;
            this.pnlAvargeSalary.Size = new System.Drawing.Size(326, 132);
            this.pnlAvargeSalary.TabIndex = 9;
            // 
            // lblAverageMoney
            // 
            this.lblAverageMoney.BackColor = System.Drawing.Color.White;
            this.lblAverageMoney.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAverageMoney.Location = new System.Drawing.Point(21, 76);
            this.lblAverageMoney.Margin = new System.Windows.Forms.Padding(4);
            this.lblAverageMoney.Name = "lblAverageMoney";
            this.lblAverageMoney.Size = new System.Drawing.Size(69, 42);
            this.lblAverageMoney.TabIndex = 8;
            this.lblAverageMoney.Text = "$0,0";
            // 
            // lblAverageSalary
            // 
            this.lblAverageSalary.BackColor = System.Drawing.Color.White;
            this.lblAverageSalary.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAverageSalary.ForeColor = System.Drawing.Color.Black;
            this.lblAverageSalary.Location = new System.Drawing.Point(113, 24);
            this.lblAverageSalary.Margin = new System.Windows.Forms.Padding(4);
            this.lblAverageSalary.Name = "lblAverageSalary";
            this.lblAverageSalary.Size = new System.Drawing.Size(196, 22);
            this.lblAverageSalary.TabIndex = 7;
            this.lblAverageSalary.Text = "Trung bình lương hàng tháng";
            // 
            // btnAveargeSalary
            // 
            this.btnAveargeSalary.BorderRadius = 10;
            this.btnAveargeSalary.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnAveargeSalary.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnAveargeSalary.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnAveargeSalary.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnAveargeSalary.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(240)))), ((int)(((byte)(254)))));
            this.btnAveargeSalary.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnAveargeSalary.ForeColor = System.Drawing.Color.White;
            this.btnAveargeSalary.Image = ((System.Drawing.Image)(resources.GetObject("btnAveargeSalary.Image")));
            this.btnAveargeSalary.ImageSize = new System.Drawing.Size(35, 35);
            this.btnAveargeSalary.Location = new System.Drawing.Point(21, 14);
            this.btnAveargeSalary.Margin = new System.Windows.Forms.Padding(4);
            this.btnAveargeSalary.Name = "btnAveargeSalary";
            this.btnAveargeSalary.Size = new System.Drawing.Size(71, 38);
            this.btnAveargeSalary.TabIndex = 4;
            // 
            // pnlSalaryEmployees
            // 
            this.pnlSalaryEmployees.BackColor = System.Drawing.Color.Transparent;
            this.pnlSalaryEmployees.BorderRadius = 12;
            this.pnlSalaryEmployees.Controls.Add(this.dgvSalaryEmployees);
            this.pnlSalaryEmployees.Controls.Add(this.lblSalaryEmployee);
            this.pnlSalaryEmployees.FillColor = System.Drawing.Color.White;
            this.pnlSalaryEmployees.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSalaryEmployees.Padding = new System.Windows.Forms.Padding(15, 55, 15, 15);
            this.pnlSalaryEmployees.Location = new System.Drawing.Point(3, 185);
            this.pnlSalaryEmployees.Name = "pnlSalaryEmployees";
            this.pnlSalaryEmployees.ShadowDecoration.Enabled = true;
            this.pnlSalaryEmployees.Size = new System.Drawing.Size(1078, 538);
            this.pnlSalaryEmployees.TabIndex = 10;
            // 
            // lblSalaryEmployee
            // 
            this.lblSalaryEmployee.BackColor = System.Drawing.Color.Transparent;
            this.lblSalaryEmployee.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblSalaryEmployee.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSalaryEmployee.Location = new System.Drawing.Point(15, 15);
            this.lblSalaryEmployee.Margin = new System.Windows.Forms.Padding(0);
            this.lblSalaryEmployee.Name = "lblSalaryEmployee";
            this.lblSalaryEmployee.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.lblSalaryEmployee.Size = new System.Drawing.Size(361, 54);
            this.lblSalaryEmployee.TabIndex = 0;
            this.lblSalaryEmployee.Text = "Bảng lương tất cả nhân viên";
            // 
            // dgvSalaryEmployees
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.dgvSalaryEmployees.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSalaryEmployees.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvSalaryEmployees.ColumnHeadersHeight = 40;
            this.dgvSalaryEmployees.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colEmployeeFullName,
            this.colEmployeeDepartment,
            this.colEmployeeAvatar,
            this.colEmployeeSalary,
            this.colEmployeeFrequency,
            this.colEmployeeStatus});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(250)))), ((int)(((byte)(251)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(246)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvSalaryEmployees.DefaultCellStyle = dataGridViewCellStyle4;
            this.dgvSalaryEmployees.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(231)))), ((int)(((byte)(235)))));
            this.dgvSalaryEmployees.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSalaryEmployees.Location = new System.Drawing.Point(15, 69);
            this.dgvSalaryEmployees.Name = "dgvSalaryEmployees";
            this.dgvSalaryEmployees.ReadOnly = true;
            this.dgvSalaryEmployees.RowHeadersVisible = false;
            this.dgvSalaryEmployees.RowHeadersWidth = 51;
            this.dgvSalaryEmployees.RowTemplate.Height = 46;
            this.dgvSalaryEmployees.Size = new System.Drawing.Size(1048, 454);
            this.dgvSalaryEmployees.TabIndex = 1;
            this.dgvSalaryEmployees.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvSalaryEmployees.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dgvSalaryEmployees.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dgvSalaryEmployees.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dgvSalaryEmployees.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dgvSalaryEmployees.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dgvSalaryEmployees.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(231)))), ((int)(((byte)(235)))));
            this.dgvSalaryEmployees.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            this.dgvSalaryEmployees.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvSalaryEmployees.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvSalaryEmployees.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgvSalaryEmployees.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvSalaryEmployees.ThemeStyle.HeaderStyle.Height = 40;
            this.dgvSalaryEmployees.ThemeStyle.ReadOnly = true;
            this.dgvSalaryEmployees.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvSalaryEmployees.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvSalaryEmployees.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvSalaryEmployees.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgvSalaryEmployees.ThemeStyle.RowsStyle.Height = 46;
            this.dgvSalaryEmployees.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvSalaryEmployees.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            // 
            // colEmployeeFullName
            // 
            this.colEmployeeFullName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colEmployeeFullName.FillWeight = 40F;
            this.colEmployeeFullName.HeaderText = "Nhân viên";
            this.colEmployeeFullName.MinimumWidth = 180;
            this.colEmployeeFullName.Name = "colEmployeeFullName";
            this.colEmployeeFullName.ReadOnly = true;
            // 
            // colEmployeeDepartment
            // 
            this.colEmployeeDepartment.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colEmployeeDepartment.FillWeight = 27.68682F;
            this.colEmployeeDepartment.HeaderText = "Bộ phận";
            this.colEmployeeDepartment.MinimumWidth = 6;
            this.colEmployeeDepartment.Name = "colEmployeeDepartment";
            this.colEmployeeDepartment.ReadOnly = true;
            this.colEmployeeDepartment.Width = 180;
            // 
            // colEmployeeAvatar
            // 
            this.colEmployeeAvatar.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colEmployeeAvatar.HeaderText = "";
            this.colEmployeeAvatar.MinimumWidth = 6;
            this.colEmployeeAvatar.Name = "colEmployeeAvatar";
            this.colEmployeeAvatar.ReadOnly = true;
            this.colEmployeeAvatar.Width = 50;
            // 
            // colEmployeeSalary
            // 
            this.colEmployeeSalary.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            this.colEmployeeSalary.DefaultCellStyle = dataGridViewCellStyle3;
            this.colEmployeeSalary.FillWeight = 52.74676F;
            this.colEmployeeSalary.HeaderText = "Lương";
            this.colEmployeeSalary.MinimumWidth = 6;
            this.colEmployeeSalary.Name = "colEmployeeSalary";
            this.colEmployeeSalary.ReadOnly = true;
            this.colEmployeeSalary.Width = 140;
            // 
            // colEmployeeFrequency
            // 
            this.colEmployeeFrequency.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colEmployeeFrequency.FillWeight = 22.13433F;
            this.colEmployeeFrequency.HeaderText = "Trả theo";
            this.colEmployeeFrequency.MinimumWidth = 6;
            this.colEmployeeFrequency.Name = "colEmployeeFrequency";
            this.colEmployeeFrequency.ReadOnly = true;
            this.colEmployeeFrequency.Width = 140;
            // 
            // colEmployeeStatus
            // 
            this.colEmployeeStatus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.colEmployeeStatus.HeaderText = "Trạng thái";
            this.colEmployeeStatus.MinimumWidth = 6;
            this.colEmployeeStatus.Name = "colEmployeeStatus";
            this.colEmployeeStatus.ReadOnly = true;
            this.colEmployeeStatus.UseColumnTextForButtonValue = true;
            // 
            // summaryLayout
            // 
            this.summaryLayout.ColumnCount = 3;
            this.summaryLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.summaryLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.summaryLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.summaryLayout.Controls.Add(this.pnlTotalPayroll, 0, 0);
            this.summaryLayout.Controls.Add(this.pnlTotalEmployees, 1, 0);
            this.summaryLayout.Controls.Add(this.pnlAvargeSalary, 2, 0);
            this.summaryLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.summaryLayout.Location = new System.Drawing.Point(3, 3);
            this.summaryLayout.Name = "summaryLayout";
            this.summaryLayout.RowCount = 1;
            this.summaryLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.summaryLayout.Size = new System.Drawing.Size(1078, 176);
            this.summaryLayout.TabIndex = 12;
            // 
            // mainLayout
            // 
            this.mainLayout.ColumnCount = 1;
            this.mainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainLayout.Controls.Add(this.summaryLayout, 0, 0);
            this.mainLayout.Controls.Add(this.pnlSalaryEmployees, 0, 1);
            this.mainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainLayout.Location = new System.Drawing.Point(0, 0);
            this.mainLayout.Name = "mainLayout";
            this.mainLayout.RowCount = 2;
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 182F));
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainLayout.Size = new System.Drawing.Size(1084, 726);
            this.mainLayout.TabIndex = 13;
            // 
            // SalaryManageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1084, 726);
            this.Controls.Add(this.mainLayout);
            this.Controls.Add(this.lblSalaryManage_SubTitle);
            this.Controls.Add(this.lblSalaryManage_Title);
            this.Padding = new System.Windows.Forms.Padding(10, 100, 10, 10);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SalaryManageForm";
            this.Text = "ManageSalaryForm";
            this.pnlTotalPayroll.ResumeLayout(false);
            this.pnlTotalPayroll.PerformLayout();
            this.pnlTotalEmployees.ResumeLayout(false);
            this.pnlTotalEmployees.PerformLayout();
            this.pnlAvargeSalary.ResumeLayout(false);
            this.pnlAvargeSalary.PerformLayout();
            this.pnlSalaryEmployees.ResumeLayout(false);
            this.pnlSalaryEmployees.PerformLayout();
            this.summaryLayout.ResumeLayout(false);
            this.mainLayout.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSalaryEmployees)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Guna.UI2.WinForms.Guna2BorderlessForm bldSalaryManageForm;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblSalaryManage_SubTitle;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblSalaryManage_Title;
        private Guna.UI2.WinForms.Guna2Panel pnlTotalPayroll;
        private Guna.UI2.WinForms.Guna2Button btnPayroll;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblTotalPayroll;
        private Guna.UI2.WinForms.Guna2Panel pnlAvargeSalary;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblAverageMoney;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblAverageSalary;
        private Guna.UI2.WinForms.Guna2Button btnAveargeSalary;
        private Guna.UI2.WinForms.Guna2Panel pnlTotalEmployees;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblActiveEmployees;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblTotalActiveEmployees;
        private Guna.UI2.WinForms.Guna2Button btnActiveEmployees;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblTotalMoney;
        private Guna.UI2.WinForms.Guna2Panel pnlSalaryEmployees;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblSalaryEmployee;
        private Guna.UI2.WinForms.Guna2DataGridView dgvSalaryEmployees;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEmployeeFullName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEmployeeDepartment;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEmployeeAvatar;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEmployeeSalary;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEmployeeFrequency;
        private System.Windows.Forms.DataGridViewButtonColumn colEmployeeStatus;
        private System.Windows.Forms.TableLayoutPanel summaryLayout;
        private System.Windows.Forms.TableLayoutPanel mainLayout;
    }
}