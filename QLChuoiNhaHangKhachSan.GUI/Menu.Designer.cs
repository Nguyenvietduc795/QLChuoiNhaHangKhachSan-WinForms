namespace QLChuoiNhaHangKhachSan.GUI
{
    partial class frmMenu
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMenu));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlList = new Guna.UI2.WinForms.Guna2Panel();
            this.txtSum = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblDishList = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2HtmlLabel7 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2HtmlLabel1 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.dgvDsMon = new Guna.UI2.WinForms.Guna2DataGridView();
            this.colCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTenMon = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGia = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTrangThai = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnSearch = new Guna.UI2.WinForms.Guna2Button();
            this.cboTilter = new Guna.UI2.WinForms.Guna2ComboBox();
            this.txtSearch = new Guna.UI2.WinForms.Guna2TextBox();
            this.pnlDetail = new Guna.UI2.WinForms.Guna2Panel();
            this.txtTableNumber = new Guna.UI2.WinForms.Guna2TextBox();
            this.btnCancel = new Guna.UI2.WinForms.Guna2Button();
            this.dgvDishMenu = new Guna.UI2.WinForms.Guna2DataGridView();
            this.colCodeM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTenM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDonGia = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSL = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTTien = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGChu = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtTotal = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblTotal = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.txtNumber = new Guna.UI2.WinForms.Guna2NumericUpDown();
            this.bntAdd = new Guna.UI2.WinForms.Guna2Button();
            this.txtNote = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblNote = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.btnComfirm = new Guna.UI2.WinForms.Guna2Button();
            this.btnRepair = new Guna.UI2.WinForms.Guna2Button();
            this.txtDishName = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtItemCode = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblNumber = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lblTableNumber = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lblDishName = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lblItemCode = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lblInfor = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.pnlList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDsMon)).BeginInit();
            this.pnlDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDishMenu)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNumber)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlList
            // 
            this.pnlList.AutoRoundedCorners = true;
            this.pnlList.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.pnlList.BorderRadius = 474;
            this.pnlList.Controls.Add(this.txtSum);
            this.pnlList.Controls.Add(this.lblDishList);
            this.pnlList.Controls.Add(this.guna2HtmlLabel7);
            this.pnlList.Controls.Add(this.guna2HtmlLabel1);
            this.pnlList.Controls.Add(this.dgvDsMon);
            this.pnlList.Controls.Add(this.btnSearch);
            this.pnlList.Controls.Add(this.cboTilter);
            this.pnlList.Controls.Add(this.txtSearch);
            this.pnlList.FillColor = System.Drawing.SystemColors.ButtonFace;
            this.pnlList.Location = new System.Drawing.Point(3, 8);
            this.pnlList.Name = "pnlList";
            this.pnlList.Size = new System.Drawing.Size(958, 951);
            this.pnlList.TabIndex = 37;
            this.pnlList.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlList_Paint);
            // 
            // txtSum
            // 
            this.txtSum.BorderRadius = 10;
            this.txtSum.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtSum.DefaultText = "";
            this.txtSum.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtSum.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtSum.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtSum.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtSum.Enabled = false;
            this.txtSum.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtSum.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSum.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtSum.Location = new System.Drawing.Point(143, 840);
            this.txtSum.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.txtSum.Name = "txtSum";
            this.txtSum.PlaceholderText = "";
            this.txtSum.SelectedText = "";
            this.txtSum.Size = new System.Drawing.Size(118, 33);
            this.txtSum.TabIndex = 7;
            // 
            // lblDishList
            // 
            this.lblDishList.AutoSize = false;
            this.lblDishList.BackColor = System.Drawing.Color.CornflowerBlue;
            this.lblDishList.Font = new System.Drawing.Font("Segoe UI", 25.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDishList.Location = new System.Drawing.Point(0, 0);
            this.lblDishList.Name = "lblDishList";
            this.lblDishList.Size = new System.Drawing.Size(1222, 58);
            this.lblDishList.TabIndex = 6;
            this.lblDishList.Text = "Danh sách món";
            this.lblDishList.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // guna2HtmlLabel7
            // 
            this.guna2HtmlLabel7.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel7.Location = new System.Drawing.Point(464, 56);
            this.guna2HtmlLabel7.Name = "guna2HtmlLabel7";
            this.guna2HtmlLabel7.Size = new System.Drawing.Size(3, 2);
            this.guna2HtmlLabel7.TabIndex = 5;
            this.guna2HtmlLabel7.Text = null;
            // 
            // guna2HtmlLabel1
            // 
            this.guna2HtmlLabel1.AutoSize = false;
            this.guna2HtmlLabel1.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel1.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel1.Location = new System.Drawing.Point(8, 840);
            this.guna2HtmlLabel1.Name = "guna2HtmlLabel1";
            this.guna2HtmlLabel1.Size = new System.Drawing.Size(155, 21);
            this.guna2HtmlLabel1.TabIndex = 4;
            this.guna2HtmlLabel1.Text = "Tổng số món:";
            // 
            // dgvDsMon
            // 
            this.dgvDsMon.AllowUserToDeleteRows = false;
            this.dgvDsMon.AllowUserToResizeColumns = false;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgvDsMon.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle8;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDsMon.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.dgvDsMon.ColumnHeadersHeight = 30;
            this.dgvDsMon.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dgvDsMon.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCode,
            this.colTenMon,
            this.colType,
            this.colGia,
            this.colTrangThai});
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvDsMon.DefaultCellStyle = dataGridViewCellStyle10;
            this.dgvDsMon.GridColor = System.Drawing.Color.DimGray;
            this.dgvDsMon.Location = new System.Drawing.Point(8, 121);
            this.dgvDsMon.Name = "dgvDsMon";
            this.dgvDsMon.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle11.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDsMon.RowHeadersDefaultCellStyle = dataGridViewCellStyle11;
            this.dgvDsMon.RowHeadersVisible = false;
            this.dgvDsMon.RowHeadersWidth = 51;
            this.dgvDsMon.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvDsMon.RowTemplate.Height = 40;
            this.dgvDsMon.Size = new System.Drawing.Size(947, 705);
            this.dgvDsMon.TabIndex = 3;
            this.dgvDsMon.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvDsMon.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dgvDsMon.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dgvDsMon.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dgvDsMon.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dgvDsMon.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dgvDsMon.ThemeStyle.GridColor = System.Drawing.Color.DimGray;
            this.dgvDsMon.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            this.dgvDsMon.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvDsMon.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvDsMon.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgvDsMon.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dgvDsMon.ThemeStyle.HeaderStyle.Height = 30;
            this.dgvDsMon.ThemeStyle.ReadOnly = false;
            this.dgvDsMon.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvDsMon.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvDsMon.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvDsMon.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgvDsMon.ThemeStyle.RowsStyle.Height = 40;
            this.dgvDsMon.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvDsMon.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgvDsMon.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDsMon_CellContentClick);
            // 
            // colCode
            // 
            this.colCode.FillWeight = 48.82507F;
            this.colCode.HeaderText = "Mã món";
            this.colCode.MinimumWidth = 6;
            this.colCode.Name = "colCode";
            // 
            // colTenMon
            // 
            this.colTenMon.FillWeight = 119.8152F;
            this.colTenMon.HeaderText = "Tên món";
            this.colTenMon.MinimumWidth = 6;
            this.colTenMon.Name = "colTenMon";
            // 
            // colType
            // 
            this.colType.FillWeight = 96.23577F;
            this.colType.HeaderText = "Loại";
            this.colType.Name = "colType";
            // 
            // colGia
            // 
            this.colGia.FillWeight = 119.8152F;
            this.colGia.HeaderText = "Giá";
            this.colGia.MinimumWidth = 6;
            this.colGia.Name = "colGia";
            // 
            // colTrangThai
            // 
            this.colTrangThai.FillWeight = 96.23577F;
            this.colTrangThai.HeaderText = "Trạng thái";
            this.colTrangThai.MinimumWidth = 6;
            this.colTrangThai.Name = "colTrangThai";
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnSearch.BorderRadius = 10;
            this.btnSearch.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnSearch.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnSearch.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnSearch.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnSearch.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Image = ((System.Drawing.Image)(resources.GetObject("btnSearch.Image")));
            this.btnSearch.Location = new System.Drawing.Point(706, 78);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.ShadowDecoration.BorderRadius = 1;
            this.btnSearch.ShadowDecoration.Depth = 10;
            this.btnSearch.ShadowDecoration.Shadow = new System.Windows.Forms.Padding(0);
            this.btnSearch.Size = new System.Drawing.Size(249, 36);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Text = "Tìm kiếm";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // cboTilter
            // 
            this.cboTilter.AutoRoundedCorners = true;
            this.cboTilter.BackColor = System.Drawing.Color.Transparent;
            this.cboTilter.BorderColor = System.Drawing.Color.Black;
            this.cboTilter.BorderRadius = 17;
            this.cboTilter.BorderStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            this.cboTilter.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboTilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTilter.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cboTilter.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cboTilter.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cboTilter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cboTilter.ItemHeight = 30;
            this.cboTilter.Items.AddRange(new object[] {
            "Tất cả",
            "Món khai vị",
            "Món chính",
            "Tráng miệng",
            "Đồ uống"});
            this.cboTilter.Location = new System.Drawing.Point(337, 78);
            this.cboTilter.Name = "cboTilter";
            this.cboTilter.Size = new System.Drawing.Size(354, 36);
            this.cboTilter.StartIndex = 0;
            this.cboTilter.TabIndex = 0;
            // 
            // txtSearch
            // 
            this.txtSearch.AutoSize = true;
            this.txtSearch.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtSearch.BackgroundImage")));
            this.txtSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.txtSearch.BorderColor = System.Drawing.Color.Black;
            this.txtSearch.BorderRadius = 10;
            this.txtSearch.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtSearch.DefaultText = "";
            this.txtSearch.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtSearch.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtSearch.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtSearch.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtSearch.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtSearch.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtSearch.Location = new System.Drawing.Point(17, 78);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.PlaceholderText = "Tìm tên món";
            this.txtSearch.SelectedText = "";
            this.txtSearch.Size = new System.Drawing.Size(300, 36);
            this.txtSearch.TabIndex = 0;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // pnlDetail
            // 
            this.pnlDetail.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.pnlDetail.BorderRadius = 10;
            this.pnlDetail.BorderStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            this.pnlDetail.BorderThickness = 2;
            this.pnlDetail.Controls.Add(this.txtTableNumber);
            this.pnlDetail.Controls.Add(this.btnCancel);
            this.pnlDetail.Controls.Add(this.dgvDishMenu);
            this.pnlDetail.Controls.Add(this.txtTotal);
            this.pnlDetail.Controls.Add(this.lblTotal);
            this.pnlDetail.Controls.Add(this.txtNumber);
            this.pnlDetail.Controls.Add(this.bntAdd);
            this.pnlDetail.Controls.Add(this.txtNote);
            this.pnlDetail.Controls.Add(this.lblNote);
            this.pnlDetail.Controls.Add(this.btnComfirm);
            this.pnlDetail.Controls.Add(this.btnRepair);
            this.pnlDetail.Controls.Add(this.txtDishName);
            this.pnlDetail.Controls.Add(this.txtItemCode);
            this.pnlDetail.Controls.Add(this.lblNumber);
            this.pnlDetail.Controls.Add(this.lblTableNumber);
            this.pnlDetail.Controls.Add(this.lblDishName);
            this.pnlDetail.Controls.Add(this.lblItemCode);
            this.pnlDetail.Controls.Add(this.lblInfor);
            this.pnlDetail.FillColor = System.Drawing.SystemColors.ButtonFace;
            this.pnlDetail.Location = new System.Drawing.Point(967, 8);
            this.pnlDetail.Name = "pnlDetail";
            this.pnlDetail.Size = new System.Drawing.Size(749, 961);
            this.pnlDetail.TabIndex = 38;
            // 
            // txtTableNumber
            // 
            this.txtTableNumber.BorderRadius = 10;
            this.txtTableNumber.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtTableNumber.DefaultText = "";
            this.txtTableNumber.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtTableNumber.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtTableNumber.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtTableNumber.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtTableNumber.Enabled = false;
            this.txtTableNumber.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtTableNumber.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTableNumber.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtTableNumber.Location = new System.Drawing.Point(172, 159);
            this.txtTableNumber.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtTableNumber.Name = "txtTableNumber";
            this.txtTableNumber.PlaceholderText = "";
            this.txtTableNumber.SelectedText = "";
            this.txtTableNumber.Size = new System.Drawing.Size(443, 32);
            this.txtTableNumber.TabIndex = 23;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BorderRadius = 10;
            this.btnCancel.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnCancel.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnCancel.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnCancel.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnCancel.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 13.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.ImageSize = new System.Drawing.Size(40, 40);
            this.btnCancel.Location = new System.Drawing.Point(597, 810);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.ShadowDecoration.Depth = 20;
            this.btnCancel.ShadowDecoration.Enabled = true;
            this.btnCancel.Size = new System.Drawing.Size(137, 38);
            this.btnCancel.TabIndex = 22;
            this.btnCancel.Text = "Hủy";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // dgvDishMenu
            // 
            this.dgvDishMenu.AllowUserToDeleteRows = false;
            this.dgvDishMenu.AllowUserToResizeColumns = false;
            dataGridViewCellStyle12.BackColor = System.Drawing.Color.White;
            this.dgvDishMenu.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle12;
            this.dgvDishMenu.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle13.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle13.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle13.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle13.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle13.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDishMenu.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle13;
            this.dgvDishMenu.ColumnHeadersHeight = 22;
            this.dgvDishMenu.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dgvDishMenu.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCodeM,
            this.colTenM,
            this.colDonGia,
            this.colSL,
            this.colTTien,
            this.colGChu});
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle14.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle14.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle14.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle14.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvDishMenu.DefaultCellStyle = dataGridViewCellStyle14;
            this.dgvDishMenu.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvDishMenu.Location = new System.Drawing.Point(3, 379);
            this.dgvDishMenu.Name = "dgvDishMenu";
            this.dgvDishMenu.RowHeadersVisible = false;
            this.dgvDishMenu.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvDishMenu.Size = new System.Drawing.Size(747, 397);
            this.dgvDishMenu.TabIndex = 21;
            this.dgvDishMenu.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvDishMenu.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dgvDishMenu.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dgvDishMenu.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dgvDishMenu.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dgvDishMenu.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dgvDishMenu.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvDishMenu.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            this.dgvDishMenu.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvDishMenu.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvDishMenu.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgvDishMenu.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dgvDishMenu.ThemeStyle.HeaderStyle.Height = 22;
            this.dgvDishMenu.ThemeStyle.ReadOnly = false;
            this.dgvDishMenu.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvDishMenu.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvDishMenu.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvDishMenu.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgvDishMenu.ThemeStyle.RowsStyle.Height = 22;
            this.dgvDishMenu.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvDishMenu.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgvDishMenu.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDishMenu_CellContentClick);
            // 
            // colCodeM
            // 
            this.colCodeM.HeaderText = "Mã món";
            this.colCodeM.Name = "colCodeM";
            // 
            // colTenM
            // 
            this.colTenM.HeaderText = "Tên món";
            this.colTenM.Name = "colTenM";
            // 
            // colDonGia
            // 
            this.colDonGia.HeaderText = "Đơn giá";
            this.colDonGia.Name = "colDonGia";
            // 
            // colSL
            // 
            this.colSL.HeaderText = "Số lượng";
            this.colSL.Name = "colSL";
            // 
            // colTTien
            // 
            this.colTTien.HeaderText = "Thành tiền";
            this.colTTien.Name = "colTTien";
            // 
            // colGChu
            // 
            this.colGChu.HeaderText = "Ghi chú";
            this.colGChu.Name = "colGChu";
            // 
            // txtTotal
            // 
            this.txtTotal.BorderRadius = 10;
            this.txtTotal.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtTotal.DefaultText = "";
            this.txtTotal.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtTotal.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtTotal.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtTotal.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtTotal.Enabled = false;
            this.txtTotal.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtTotal.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTotal.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtTotal.Location = new System.Drawing.Point(172, 317);
            this.txtTotal.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtTotal.Name = "txtTotal";
            this.txtTotal.PlaceholderText = "";
            this.txtTotal.SelectedText = "";
            this.txtTotal.Size = new System.Drawing.Size(443, 32);
            this.txtTotal.TabIndex = 20;
            this.txtTotal.TextChanged += new System.EventHandler(this.txtTotal_TextChanged);
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = false;
            this.lblTotal.BackColor = System.Drawing.Color.Transparent;
            this.lblTotal.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotal.Location = new System.Drawing.Point(98, 324);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(162, 25);
            this.lblTotal.TabIndex = 19;
            this.lblTotal.Text = "Tổng tiền";
            // 
            // txtNumber
            // 
            this.txtNumber.BackColor = System.Drawing.Color.Transparent;
            this.txtNumber.BorderRadius = 10;
            this.txtNumber.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtNumber.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNumber.Location = new System.Drawing.Point(172, 197);
            this.txtNumber.Name = "txtNumber";
            this.txtNumber.Size = new System.Drawing.Size(443, 36);
            this.txtNumber.TabIndex = 18;
            // 
            // bntAdd
            // 
            this.bntAdd.BackColor = System.Drawing.Color.Transparent;
            this.bntAdd.BorderRadius = 10;
            this.bntAdd.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.bntAdd.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.bntAdd.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.bntAdd.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.bntAdd.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(70)))), ((int)(((byte)(229)))));
            this.bntAdd.Font = new System.Drawing.Font("Segoe UI", 13.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bntAdd.ForeColor = System.Drawing.Color.White;
            this.bntAdd.ImageSize = new System.Drawing.Size(30, 30);
            this.bntAdd.Location = new System.Drawing.Point(19, 810);
            this.bntAdd.Name = "bntAdd";
            this.bntAdd.ShadowDecoration.Depth = 20;
            this.bntAdd.ShadowDecoration.Enabled = true;
            this.bntAdd.Size = new System.Drawing.Size(140, 38);
            this.bntAdd.TabIndex = 16;
            this.bntAdd.Text = "Thêm";
            this.bntAdd.Click += new System.EventHandler(this.bntAdd_Click);
            // 
            // txtNote
            // 
            this.txtNote.BorderRadius = 10;
            this.txtNote.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtNote.DefaultText = "";
            this.txtNote.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtNote.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtNote.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtNote.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtNote.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtNote.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNote.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtNote.Location = new System.Drawing.Point(172, 240);
            this.txtNote.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtNote.Name = "txtNote";
            this.txtNote.PlaceholderText = "";
            this.txtNote.SelectedText = "";
            this.txtNote.Size = new System.Drawing.Size(443, 69);
            this.txtNote.TabIndex = 15;
            // 
            // lblNote
            // 
            this.lblNote.AutoSize = false;
            this.lblNote.BackColor = System.Drawing.Color.Transparent;
            this.lblNote.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNote.Location = new System.Drawing.Point(98, 256);
            this.lblNote.Name = "lblNote";
            this.lblNote.Size = new System.Drawing.Size(175, 25);
            this.lblNote.TabIndex = 14;
            this.lblNote.Text = "Ghi chú";
            // 
            // btnComfirm
            // 
            this.btnComfirm.BackColor = System.Drawing.Color.Transparent;
            this.btnComfirm.BorderRadius = 10;
            this.btnComfirm.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnComfirm.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnComfirm.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnComfirm.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnComfirm.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.btnComfirm.Font = new System.Drawing.Font("Segoe UI", 13.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnComfirm.ForeColor = System.Drawing.Color.White;
            this.btnComfirm.ImageSize = new System.Drawing.Size(40, 40);
            this.btnComfirm.Location = new System.Drawing.Point(403, 810);
            this.btnComfirm.Name = "btnComfirm";
            this.btnComfirm.ShadowDecoration.Depth = 20;
            this.btnComfirm.ShadowDecoration.Enabled = true;
            this.btnComfirm.Size = new System.Drawing.Size(137, 38);
            this.btnComfirm.TabIndex = 13;
            this.btnComfirm.Text = "Xác nhận";
            this.btnComfirm.Click += new System.EventHandler(this.guna2Button3_Click);
            // 
            // btnRepair
            // 
            this.btnRepair.BackColor = System.Drawing.Color.Transparent;
            this.btnRepair.BorderRadius = 10;
            this.btnRepair.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnRepair.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnRepair.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnRepair.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnRepair.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(163)))), ((int)(((byte)(74)))));
            this.btnRepair.Font = new System.Drawing.Font("Segoe UI", 13.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRepair.ForeColor = System.Drawing.Color.White;
            this.btnRepair.ImageSize = new System.Drawing.Size(30, 30);
            this.btnRepair.Location = new System.Drawing.Point(205, 810);
            this.btnRepair.Name = "btnRepair";
            this.btnRepair.ShadowDecoration.Depth = 20;
            this.btnRepair.ShadowDecoration.Enabled = true;
            this.btnRepair.Size = new System.Drawing.Size(140, 38);
            this.btnRepair.TabIndex = 12;
            this.btnRepair.Text = "Xóa";
            // 
            // txtDishName
            // 
            this.txtDishName.BorderRadius = 10;
            this.txtDishName.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtDishName.DefaultText = "";
            this.txtDishName.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtDishName.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtDishName.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtDishName.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtDishName.Enabled = false;
            this.txtDishName.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtDishName.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDishName.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtDishName.Location = new System.Drawing.Point(172, 116);
            this.txtDishName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtDishName.Name = "txtDishName";
            this.txtDishName.PlaceholderText = "";
            this.txtDishName.SelectedText = "";
            this.txtDishName.Size = new System.Drawing.Size(443, 32);
            this.txtDishName.TabIndex = 8;
            this.txtDishName.TextChanged += new System.EventHandler(this.txtDishName_TextChanged);
            // 
            // txtItemCode
            // 
            this.txtItemCode.BorderRadius = 10;
            this.txtItemCode.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtItemCode.DefaultText = "";
            this.txtItemCode.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtItemCode.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtItemCode.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtItemCode.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtItemCode.Enabled = false;
            this.txtItemCode.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtItemCode.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtItemCode.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtItemCode.Location = new System.Drawing.Point(172, 76);
            this.txtItemCode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtItemCode.Name = "txtItemCode";
            this.txtItemCode.PlaceholderText = "";
            this.txtItemCode.SelectedText = "";
            this.txtItemCode.Size = new System.Drawing.Size(443, 32);
            this.txtItemCode.TabIndex = 7;
            this.txtItemCode.TextChanged += new System.EventHandler(this.txtItemCode_TextChanged);
            // 
            // lblNumber
            // 
            this.lblNumber.AutoSize = false;
            this.lblNumber.BackColor = System.Drawing.Color.Transparent;
            this.lblNumber.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNumber.Location = new System.Drawing.Point(98, 208);
            this.lblNumber.Name = "lblNumber";
            this.lblNumber.Size = new System.Drawing.Size(175, 25);
            this.lblNumber.TabIndex = 4;
            this.lblNumber.Text = "Số lượng";
            // 
            // lblTableNumber
            // 
            this.lblTableNumber.AutoSize = false;
            this.lblTableNumber.BackColor = System.Drawing.Color.Transparent;
            this.lblTableNumber.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTableNumber.Location = new System.Drawing.Point(107, 166);
            this.lblTableNumber.Name = "lblTableNumber";
            this.lblTableNumber.Size = new System.Drawing.Size(118, 25);
            this.lblTableNumber.TabIndex = 3;
            this.lblTableNumber.Text = "Số bàn";
            // 
            // lblDishName
            // 
            this.lblDishName.AutoSize = false;
            this.lblDishName.BackColor = System.Drawing.Color.Transparent;
            this.lblDishName.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDishName.Location = new System.Drawing.Point(98, 123);
            this.lblDishName.Name = "lblDishName";
            this.lblDishName.Size = new System.Drawing.Size(162, 25);
            this.lblDishName.TabIndex = 2;
            this.lblDishName.Text = "Tên món";
            // 
            // lblItemCode
            // 
            this.lblItemCode.AutoSize = false;
            this.lblItemCode.BackColor = System.Drawing.Color.Transparent;
            this.lblItemCode.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblItemCode.Location = new System.Drawing.Point(98, 83);
            this.lblItemCode.Name = "lblItemCode";
            this.lblItemCode.Size = new System.Drawing.Size(158, 25);
            this.lblItemCode.TabIndex = 1;
            this.lblItemCode.Text = "Mã món";
            this.lblItemCode.Click += new System.EventHandler(this.guna2HtmlLabel3_Click);
            // 
            // lblInfor
            // 
            this.lblInfor.AutoSize = false;
            this.lblInfor.BackColor = System.Drawing.Color.CornflowerBlue;
            this.lblInfor.Font = new System.Drawing.Font("Segoe UI", 25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInfor.Location = new System.Drawing.Point(0, 0);
            this.lblInfor.Name = "lblInfor";
            this.lblInfor.Size = new System.Drawing.Size(749, 58);
            this.lblInfor.TabIndex = 0;
            this.lblInfor.Text = "Thông tin món";
            this.lblInfor.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmMenu
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(1716, 961);
            this.Controls.Add(this.pnlDetail);
            this.Controls.Add(this.pnlList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmMenu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmMenu";
            this.Load += new System.EventHandler(this.Menu_Load);
            this.pnlList.ResumeLayout(false);
            this.pnlList.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDsMon)).EndInit();
            this.pnlDetail.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDishMenu)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNumber)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Guna.UI2.WinForms.Guna2Panel pnlList;
        private Guna.UI2.WinForms.Guna2TextBox txtSearch;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel1;
        private Guna.UI2.WinForms.Guna2DataGridView dgvDsMon;
        private Guna.UI2.WinForms.Guna2Button btnSearch;
        private Guna.UI2.WinForms.Guna2ComboBox cboTilter;
        private Guna.UI2.WinForms.Guna2Panel pnlDetail;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblTableNumber;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblDishName;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblItemCode;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblInfor;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblNumber;
        private Guna.UI2.WinForms.Guna2TextBox txtDishName;
        private Guna.UI2.WinForms.Guna2TextBox txtItemCode;
        private Guna.UI2.WinForms.Guna2Button btnComfirm;
        private Guna.UI2.WinForms.Guna2Button btnRepair;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblDishList;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel7;
        private Guna.UI2.WinForms.Guna2TextBox txtNote;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblNote;
        private Guna.UI2.WinForms.Guna2Button bntAdd;
        private Guna.UI2.WinForms.Guna2TextBox txtTotal;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblTotal;
        private Guna.UI2.WinForms.Guna2NumericUpDown txtNumber;
        private Guna.UI2.WinForms.Guna2DataGridView dgvDishMenu;
        private Guna.UI2.WinForms.Guna2TextBox txtSum;
        private Guna.UI2.WinForms.Guna2Button btnCancel;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTenMon;
        private System.Windows.Forms.DataGridViewTextBoxColumn colType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGia;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTrangThai;
        private Guna.UI2.WinForms.Guna2TextBox txtTableNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCodeM;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTenM;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDonGia;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSL;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTTien;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGChu;
    }
}