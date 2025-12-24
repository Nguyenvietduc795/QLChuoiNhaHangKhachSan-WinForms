namespace QLChuoiNhaHangKhachSan.GUI
{
    partial class FormInvoiceManagement
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormInvoiceManagement));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlPaymentHeader = new Guna.UI2.WinForms.Guna2Panel();
            this.btnRemove = new Guna.UI2.WinForms.Guna2Button();
            this.lblHeaderSub = new System.Windows.Forms.Label();
            this.pnlQLHD = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.pnlPayment = new Guna.UI2.WinForms.Guna2TextBox();
            this.gnbtnALL = new Guna.UI2.WinForms.Guna2Button();
            this.gnbtnDont = new Guna.UI2.WinForms.Guna2Button();
            this.gnbtnDone = new Guna.UI2.WinForms.Guna2Button();
            this.gnbtnOverdue = new Guna.UI2.WinForms.Guna2Button();
            this.gnpnlLSHD = new Guna.UI2.WinForms.Guna2Panel();
            this.dgvTransaction = new System.Windows.Forms.DataGridView();
            this.dgvInvoiceID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvCustomer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dvgDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lbLSGD = new System.Windows.Forms.Label();
            this.pnlInvoicedetails = new Guna.UI2.WinForms.Guna2Panel();
            this.btnFix = new Guna.UI2.WinForms.Guna2Button();
            this.btnDeleteInvoice = new Guna.UI2.WinForms.Guna2Button();
            this.btnExportExcel = new Guna.UI2.WinForms.Guna2Button();
            this.btnPrintInvoice = new Guna.UI2.WinForms.Guna2Button();
            this.lblTitleInvoicedetails = new System.Windows.Forms.Label();
            this.pnlPaymentHeader.SuspendLayout();
            this.gnpnlLSHD.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTransaction)).BeginInit();
            this.pnlInvoicedetails.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlPaymentHeader
            // 
            this.pnlPaymentHeader.BackColor = System.Drawing.Color.Transparent;
            this.pnlPaymentHeader.BorderRadius = 20;
            this.pnlPaymentHeader.Controls.Add(this.btnRemove);
            this.pnlPaymentHeader.Controls.Add(this.lblHeaderSub);
            this.pnlPaymentHeader.Controls.Add(this.pnlQLHD);
            this.pnlPaymentHeader.Controls.Add(this.pnlPayment);
            this.pnlPaymentHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlPaymentHeader.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(31)))), ((int)(((byte)(51)))));
            this.pnlPaymentHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlPaymentHeader.Name = "pnlPaymentHeader";
            this.pnlPaymentHeader.ShadowDecoration.Enabled = true;
            this.pnlPaymentHeader.Size = new System.Drawing.Size(1805, 107);
            this.pnlPaymentHeader.TabIndex = 0;
            // 
            // btnRemove
            // 
            this.btnRemove.BorderColor = System.Drawing.Color.White;
            this.btnRemove.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnRemove.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnRemove.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnRemove.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnRemove.FillColor = System.Drawing.Color.White;
            this.btnRemove.Font = new System.Drawing.Font("Segoe UI", 14.25F);
            this.btnRemove.ForeColor = System.Drawing.Color.White;
            this.btnRemove.Image = ((System.Drawing.Image)(resources.GetObject("btnRemove.Image")));
            this.btnRemove.ImageSize = new System.Drawing.Size(25, 25);
            this.btnRemove.Location = new System.Drawing.Point(1435, 36);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.ShadowDecoration.BorderRadius = 10;
            this.btnRemove.ShadowDecoration.Color = System.Drawing.Color.White;
            this.btnRemove.ShadowDecoration.Depth = 10;
            this.btnRemove.ShadowDecoration.Enabled = true;
            this.btnRemove.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            this.btnRemove.ShadowDecoration.Shadow = new System.Windows.Forms.Padding(0);
            this.btnRemove.Size = new System.Drawing.Size(34, 26);
            this.btnRemove.TabIndex = 1;
            this.btnRemove.UseTransparentBackground = true;
            // 
            // lblHeaderSub
            // 
            this.lblHeaderSub.AutoSize = true;
            this.lblHeaderSub.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblHeaderSub.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(114)))), ((int)(((byte)(128)))));
            this.lblHeaderSub.Location = new System.Drawing.Point(205, 70);
            this.lblHeaderSub.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblHeaderSub.Name = "lblHeaderSub";
            this.lblHeaderSub.Size = new System.Drawing.Size(179, 25);
            this.lblHeaderSub.TabIndex = 3;
            this.lblHeaderSub.Text = "Theo dõi tình trạng ";
            // 
            // pnlQLHD
            // 
            this.pnlQLHD.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.pnlQLHD.BackColor = System.Drawing.Color.Transparent;
            this.pnlQLHD.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.pnlQLHD.ForeColor = System.Drawing.Color.White;
            this.pnlQLHD.Location = new System.Drawing.Point(117, 24);
            this.pnlQLHD.Name = "pnlQLHD";
            this.pnlQLHD.Padding = new System.Windows.Forms.Padding(20, 10, 20, 10);
            this.pnlQLHD.Size = new System.Drawing.Size(379, 61);
            this.pnlQLHD.TabIndex = 0;
            this.pnlQLHD.Text = "QUẢN LÝ HÓA ĐƠN";
            // 
            // pnlPayment
            // 
            this.pnlPayment.AutoRoundedCorners = true;
            this.pnlPayment.BorderRadius = 23;
            this.pnlPayment.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.pnlPayment.DefaultText = "";
            this.pnlPayment.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.pnlPayment.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.pnlPayment.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.pnlPayment.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.pnlPayment.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.pnlPayment.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.pnlPayment.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.pnlPayment.IconLeft = ((System.Drawing.Image)(resources.GetObject("pnlPayment.IconLeft")));
            this.pnlPayment.Location = new System.Drawing.Point(1034, 24);
            this.pnlPayment.Name = "pnlPayment";
            this.pnlPayment.PlaceholderText = "Nhập Mã Hóa Đơn/Tên Khách...";
            this.pnlPayment.SelectedText = "";
            this.pnlPayment.Size = new System.Drawing.Size(447, 49);
            this.pnlPayment.TabIndex = 1;
            // 
            // gnbtnALL
            // 
            this.gnbtnALL.AutoRoundedCorners = true;
            this.gnbtnALL.BorderColor = System.Drawing.Color.Navy;
            this.gnbtnALL.BorderRadius = 21;
            this.gnbtnALL.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.gnbtnALL.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.gnbtnALL.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.gnbtnALL.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.gnbtnALL.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(31)))), ((int)(((byte)(51)))));
            this.gnbtnALL.Font = new System.Drawing.Font("Segoe UI", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.gnbtnALL.ForeColor = System.Drawing.Color.White;
            this.gnbtnALL.Location = new System.Drawing.Point(117, 188);
            this.gnbtnALL.Name = "gnbtnALL";
            this.gnbtnALL.Size = new System.Drawing.Size(207, 45);
            this.gnbtnALL.TabIndex = 1;
            this.gnbtnALL.Text = "Tất Cả";
            // 
            // gnbtnDont
            // 
            this.gnbtnDont.AutoRoundedCorners = true;
            this.gnbtnDont.BorderColor = System.Drawing.Color.DarkOrange;
            this.gnbtnDont.BorderRadius = 21;
            this.gnbtnDont.BorderThickness = 2;
            this.gnbtnDont.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.gnbtnDont.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.gnbtnDont.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.gnbtnDont.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.gnbtnDont.FillColor = System.Drawing.Color.White;
            this.gnbtnDont.Font = new System.Drawing.Font("Segoe UI", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.gnbtnDont.ForeColor = System.Drawing.Color.Black;
            this.gnbtnDont.Location = new System.Drawing.Point(509, 188);
            this.gnbtnDont.Name = "gnbtnDont";
            this.gnbtnDont.Size = new System.Drawing.Size(197, 45);
            this.gnbtnDont.TabIndex = 2;
            this.gnbtnDont.Text = "Chưa Thu Tiền";
            // 
            // gnbtnDone
            // 
            this.gnbtnDone.AutoRoundedCorners = true;
            this.gnbtnDone.BorderColor = System.Drawing.Color.DarkGreen;
            this.gnbtnDone.BorderRadius = 21;
            this.gnbtnDone.BorderThickness = 2;
            this.gnbtnDone.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.gnbtnDone.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.gnbtnDone.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.gnbtnDone.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.gnbtnDone.FillColor = System.Drawing.Color.White;
            this.gnbtnDone.Font = new System.Drawing.Font("Segoe UI", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.gnbtnDone.ForeColor = System.Drawing.Color.DarkGreen;
            this.gnbtnDone.Location = new System.Drawing.Point(902, 188);
            this.gnbtnDone.Name = "gnbtnDone";
            this.gnbtnDone.Size = new System.Drawing.Size(194, 45);
            this.gnbtnDone.TabIndex = 3;
            this.gnbtnDone.Text = "Đã Thu Tiền";
            // 
            // gnbtnOverdue
            // 
            this.gnbtnOverdue.AutoRoundedCorners = true;
            this.gnbtnOverdue.BorderColor = System.Drawing.Color.Maroon;
            this.gnbtnOverdue.BorderRadius = 21;
            this.gnbtnOverdue.BorderThickness = 2;
            this.gnbtnOverdue.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.gnbtnOverdue.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.gnbtnOverdue.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.gnbtnOverdue.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.gnbtnOverdue.FillColor = System.Drawing.Color.White;
            this.gnbtnOverdue.Font = new System.Drawing.Font("Segoe UI", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.gnbtnOverdue.ForeColor = System.Drawing.Color.Black;
            this.gnbtnOverdue.Location = new System.Drawing.Point(1256, 188);
            this.gnbtnOverdue.Name = "gnbtnOverdue";
            this.gnbtnOverdue.Size = new System.Drawing.Size(203, 45);
            this.gnbtnOverdue.TabIndex = 4;
            this.gnbtnOverdue.Text = "Quá Hạn";
            // 
            // gnpnlLSHD
            // 
            this.gnpnlLSHD.BackColor = System.Drawing.Color.Transparent;
            this.gnpnlLSHD.BorderColor = System.Drawing.Color.Gray;
            this.gnpnlLSHD.BorderRadius = 15;
            this.gnpnlLSHD.BorderThickness = 2;
            this.gnpnlLSHD.Controls.Add(this.dgvTransaction);
            this.gnpnlLSHD.Controls.Add(this.lbLSGD);
            this.gnpnlLSHD.Location = new System.Drawing.Point(51, 345);
            this.gnpnlLSHD.Name = "gnpnlLSHD";
            this.gnpnlLSHD.Size = new System.Drawing.Size(1123, 652);
            this.gnpnlLSHD.TabIndex = 6;
            // 
            // dgvTransaction
            // 
            this.dgvTransaction.AllowUserToAddRows = false;
            this.dgvTransaction.AllowUserToResizeColumns = false;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.dgvTransaction.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvTransaction.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTransaction.BackgroundColor = System.Drawing.Color.White;
            this.dgvTransaction.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvTransaction.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Navy;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(20);
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.Navy;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvTransaction.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvTransaction.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTransaction.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvInvoiceID,
            this.dgvCustomer,
            this.dvgDate,
            this.dgvAmount,
            this.dgvStatus});
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvTransaction.DefaultCellStyle = dataGridViewCellStyle7;
            this.dgvTransaction.EnableHeadersVisualStyles = false;
            this.dgvTransaction.GridColor = System.Drawing.SystemColors.GrayText;
            this.dgvTransaction.Location = new System.Drawing.Point(30, 66);
            this.dgvTransaction.Name = "dgvTransaction";
            this.dgvTransaction.ReadOnly = true;
            this.dgvTransaction.RowHeadersVisible = false;
            this.dgvTransaction.RowHeadersWidth = 51;
            this.dgvTransaction.RowTemplate.Height = 24;
            this.dgvTransaction.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTransaction.Size = new System.Drawing.Size(1053, 553);
            this.dgvTransaction.TabIndex = 10;
            // 
            // dgvInvoiceID
            // 
            this.dgvInvoiceID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            dataGridViewCellStyle3.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.dgvInvoiceID.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvInvoiceID.HeaderText = "Mã GD";
            this.dgvInvoiceID.MinimumWidth = 100;
            this.dgvInvoiceID.Name = "dgvInvoiceID";
            this.dgvInvoiceID.ReadOnly = true;
            // 
            // dgvCustomer
            // 
            this.dgvCustomer.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.dgvCustomer.DefaultCellStyle = dataGridViewCellStyle4;
            this.dgvCustomer.HeaderText = "Khách Hàng";
            this.dgvCustomer.MinimumWidth = 100;
            this.dgvCustomer.Name = "dgvCustomer";
            this.dgvCustomer.ReadOnly = true;
            // 
            // dvgDate
            // 
            this.dvgDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dvgDate.HeaderText = "Ngày Giao Dịch";
            this.dvgDate.MinimumWidth = 100;
            this.dvgDate.Name = "dvgDate";
            this.dvgDate.ReadOnly = true;
            // 
            // dgvAmount
            // 
            this.dgvAmount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.dgvAmount.DefaultCellStyle = dataGridViewCellStyle5;
            this.dgvAmount.HeaderText = "Số Tiền";
            this.dgvAmount.MinimumWidth = 100;
            this.dgvAmount.Name = "dgvAmount";
            this.dgvAmount.ReadOnly = true;
            // 
            // dgvStatus
            // 
            this.dgvStatus.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(255)))), ((int)(((byte)(248)))));
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(182)))), ((int)(((byte)(122)))));
            this.dgvStatus.DefaultCellStyle = dataGridViewCellStyle6;
            this.dgvStatus.HeaderText = "Trạng Thái";
            this.dgvStatus.MinimumWidth = 100;
            this.dgvStatus.Name = "dgvStatus";
            this.dgvStatus.ReadOnly = true;
            // 
            // lbLSGD
            // 
            this.lbLSGD.AutoSize = true;
            this.lbLSGD.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lbLSGD.Location = new System.Drawing.Point(24, 18);
            this.lbLSGD.Name = "lbLSGD";
            this.lbLSGD.Size = new System.Drawing.Size(249, 33);
            this.lbLSGD.TabIndex = 6;
            this.lbLSGD.Text = "Lịch Sử Hóa Đơn";
            // 
            // pnlInvoicedetails
            // 
            this.pnlInvoicedetails.BackColor = System.Drawing.Color.Transparent;
            this.pnlInvoicedetails.BorderRadius = 15;
            this.pnlInvoicedetails.Controls.Add(this.btnFix);
            this.pnlInvoicedetails.Controls.Add(this.btnDeleteInvoice);
            this.pnlInvoicedetails.Controls.Add(this.btnExportExcel);
            this.pnlInvoicedetails.Controls.Add(this.btnPrintInvoice);
            this.pnlInvoicedetails.Controls.Add(this.lblTitleInvoicedetails);
            this.pnlInvoicedetails.FillColor = System.Drawing.Color.White;
            this.pnlInvoicedetails.Location = new System.Drawing.Point(1325, 345);
            this.pnlInvoicedetails.Name = "pnlInvoicedetails";
            this.pnlInvoicedetails.ShadowDecoration.Enabled = true;
            this.pnlInvoicedetails.Size = new System.Drawing.Size(458, 421);
            this.pnlInvoicedetails.TabIndex = 7;
            // 
            // btnFix
            // 
            this.btnFix.DefaultAutoSize = true;
            this.btnFix.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnFix.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnFix.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnFix.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnFix.FillColor = System.Drawing.Color.White;
            this.btnFix.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnFix.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(31)))), ((int)(((byte)(51)))));
            this.btnFix.Image = ((System.Drawing.Image)(resources.GetObject("btnFix.Image")));
            this.btnFix.ImageSize = new System.Drawing.Size(30, 30);
            this.btnFix.Location = new System.Drawing.Point(70, 254);
            this.btnFix.Name = "btnFix";
            this.btnFix.Size = new System.Drawing.Size(189, 40);
            this.btnFix.TabIndex = 8;
            this.btnFix.Text = "Sửa Hóa Đơn";
            // 
            // btnDeleteInvoice
            // 
            this.btnDeleteInvoice.DefaultAutoSize = true;
            this.btnDeleteInvoice.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnDeleteInvoice.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnDeleteInvoice.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnDeleteInvoice.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnDeleteInvoice.FillColor = System.Drawing.Color.White;
            this.btnDeleteInvoice.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnDeleteInvoice.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(31)))), ((int)(((byte)(51)))));
            this.btnDeleteInvoice.Image = ((System.Drawing.Image)(resources.GetObject("btnDeleteInvoice.Image")));
            this.btnDeleteInvoice.ImageSize = new System.Drawing.Size(30, 30);
            this.btnDeleteInvoice.Location = new System.Drawing.Point(70, 334);
            this.btnDeleteInvoice.Name = "btnDeleteInvoice";
            this.btnDeleteInvoice.Size = new System.Drawing.Size(192, 40);
            this.btnDeleteInvoice.TabIndex = 3;
            this.btnDeleteInvoice.Text = "Hủy Hóa Đơn";
            // 
            // btnExportExcel
            // 
            this.btnExportExcel.DefaultAutoSize = true;
            this.btnExportExcel.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnExportExcel.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnExportExcel.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnExportExcel.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnExportExcel.FillColor = System.Drawing.Color.White;
            this.btnExportExcel.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnExportExcel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(31)))), ((int)(((byte)(51)))));
            this.btnExportExcel.Image = ((System.Drawing.Image)(resources.GetObject("btnExportExcel.Image")));
            this.btnExportExcel.ImageSize = new System.Drawing.Size(30, 30);
            this.btnExportExcel.Location = new System.Drawing.Point(70, 172);
            this.btnExportExcel.Name = "btnExportExcel";
            this.btnExportExcel.Size = new System.Drawing.Size(165, 40);
            this.btnExportExcel.TabIndex = 2;
            this.btnExportExcel.Text = " Xuất Excel";
            // 
            // btnPrintInvoice
            // 
            this.btnPrintInvoice.DefaultAutoSize = true;
            this.btnPrintInvoice.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnPrintInvoice.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnPrintInvoice.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnPrintInvoice.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnPrintInvoice.FillColor = System.Drawing.Color.White;
            this.btnPrintInvoice.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnPrintInvoice.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(31)))), ((int)(((byte)(51)))));
            this.btnPrintInvoice.Image = ((System.Drawing.Image)(resources.GetObject("btnPrintInvoice.Image")));
            this.btnPrintInvoice.ImageSize = new System.Drawing.Size(30, 30);
            this.btnPrintInvoice.Location = new System.Drawing.Point(70, 89);
            this.btnPrintInvoice.Name = "btnPrintInvoice";
            this.btnPrintInvoice.Size = new System.Drawing.Size(184, 40);
            this.btnPrintInvoice.TabIndex = 1;
            this.btnPrintInvoice.Text = "  In Hóa Đơn  ";
            // 
            // lblTitleInvoicedetails
            // 
            this.lblTitleInvoicedetails.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblTitleInvoicedetails.Location = new System.Drawing.Point(104, 19);
            this.lblTitleInvoicedetails.Name = "lblTitleInvoicedetails";
            this.lblTitleInvoicedetails.Size = new System.Drawing.Size(244, 47);
            this.lblTitleInvoicedetails.TabIndex = 0;
            this.lblTitleInvoicedetails.Text = "Chi Tiết Hóa Đơn";
            // 
            // FormInvoiceManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(245)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(1805, 1009);
            this.Controls.Add(this.pnlInvoicedetails);
            this.Controls.Add(this.gnpnlLSHD);
            this.Controls.Add(this.gnbtnOverdue);
            this.Controls.Add(this.gnbtnDone);
            this.Controls.Add(this.gnbtnDont);
            this.Controls.Add(this.gnbtnALL);
            this.Controls.Add(this.pnlPaymentHeader);
            this.MinimumSize = new System.Drawing.Size(1200, 700);
            this.Name = "FormInvoiceManagement";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormInvoiceManagement";
            this.Load += new System.EventHandler(this.FormInvoiceManagement_Load);
            this.pnlPaymentHeader.ResumeLayout(false);
            this.pnlPaymentHeader.PerformLayout();
            this.gnpnlLSHD.ResumeLayout(false);
            this.gnpnlLSHD.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTransaction)).EndInit();
            this.pnlInvoicedetails.ResumeLayout(false);
            this.pnlInvoicedetails.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel pnlPaymentHeader;
        private Guna.UI2.WinForms.Guna2HtmlLabel pnlQLHD;
        private Guna.UI2.WinForms.Guna2Button gnbtnALL;
        private Guna.UI2.WinForms.Guna2Button gnbtnDont;
        private Guna.UI2.WinForms.Guna2Button gnbtnDone;
        private Guna.UI2.WinForms.Guna2Button gnbtnOverdue;
        private Guna.UI2.WinForms.Guna2Panel gnpnlLSHD;
        private System.Windows.Forms.Label lbLSGD;
        private Guna.UI2.WinForms.Guna2Panel pnlInvoicedetails;
        private Guna.UI2.WinForms.Guna2Button btnDeleteInvoice;
        private Guna.UI2.WinForms.Guna2Button btnExportExcel;
        private Guna.UI2.WinForms.Guna2Button btnPrintInvoice;
        private System.Windows.Forms.Label lblTitleInvoicedetails;
        private System.Windows.Forms.DataGridView dgvTransaction;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvInvoiceID;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvCustomer;
        private System.Windows.Forms.DataGridViewTextBoxColumn dvgDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvStatus;
        private Guna.UI2.WinForms.Guna2TextBox pnlPayment;
        private Guna.UI2.WinForms.Guna2Button btnFix;
        private System.Windows.Forms.Label lblHeaderSub;
        private Guna.UI2.WinForms.Guna2Button btnRemove;
    }
}