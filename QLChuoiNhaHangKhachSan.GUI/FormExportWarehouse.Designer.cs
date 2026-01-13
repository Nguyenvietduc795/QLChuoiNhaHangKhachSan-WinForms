namespace QLChuoiNhaHangKhachSan.GUI
{
    partial class FormExportWarehouse
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
            this.DGVexportkho = new System.Windows.Forms.DataGridView();
            this.bnaddrowexportKho = new Guna.UI2.WinForms.Guna2Button();
            this.bnupdaterowexportKho = new Guna.UI2.WinForms.Guna2Button();
            this.bndeleterowexportkho = new Guna.UI2.WinForms.Guna2Button();
            this.lbdonviexport = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lbtypeKhoExport = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.cbdonviexportkho = new Guna.UI2.WinForms.Guna2ComboBox();
            this.cbtypekhoexport = new Guna.UI2.WinForms.Guna2ComboBox();
            this.lbstatusexport = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.SepBottomUpdateItems = new Guna.UI2.WinForms.Guna2Separator();
            this.sepTopUpdateItems = new Guna.UI2.WinForms.Guna2Separator();
            this.BtnExistExport = new Guna.UI2.WinForms.Guna2Button();
            this.cbStatusExport = new Guna.UI2.WinForms.Guna2ComboBox();
            this.btnCancelExportKho = new Guna.UI2.WinForms.Guna2Button();
            this.bnCreateExportPhieu = new Guna.UI2.WinForms.Guna2Button();
            this.txtMaPhieuexport = new Guna.UI2.WinForms.Guna2TextBox();
            this.lbMaPhieuExport = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lbExportKho = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.pnlCardExportWarehouse = new Guna.UI2.WinForms.Guna2Panel();
            ((System.ComponentModel.ISupportInitialize)(this.DGVexportkho)).BeginInit();
            this.pnlCardExportWarehouse.SuspendLayout();
            this.SuspendLayout();
            // 
            // DGVexportkho
            // 
            this.DGVexportkho.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.DGVexportkho.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGVexportkho.Location = new System.Drawing.Point(31, 347);
            this.DGVexportkho.Name = "DGVexportkho";
            this.DGVexportkho.Size = new System.Drawing.Size(681, 150);
            this.DGVexportkho.TabIndex = 20;
            // 
            // bnaddrowexportKho
            // 
            this.bnaddrowexportKho.BorderRadius = 12;
            this.bnaddrowexportKho.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.bnaddrowexportKho.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.bnaddrowexportKho.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.bnaddrowexportKho.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.bnaddrowexportKho.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(163)))), ((int)(((byte)(74)))));
            this.bnaddrowexportKho.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bnaddrowexportKho.ForeColor = System.Drawing.Color.White;
            this.bnaddrowexportKho.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(128)))), ((int)(((byte)(61)))));
            this.bnaddrowexportKho.Location = new System.Drawing.Point(55, 289);
            this.bnaddrowexportKho.Name = "bnaddrowexportKho";
            this.bnaddrowexportKho.Size = new System.Drawing.Size(132, 40);
            this.bnaddrowexportKho.TabIndex = 19;
            this.bnaddrowexportKho.Text = "+ Thêm dòng";
            this.bnaddrowexportKho.Click += new System.EventHandler(this.bnaddrowexportKho_Click);
            // 
            // bnupdaterowexportKho
            // 
            this.bnupdaterowexportKho.BorderRadius = 12;
            this.bnupdaterowexportKho.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.bnupdaterowexportKho.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.bnupdaterowexportKho.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.bnupdaterowexportKho.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.bnupdaterowexportKho.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(70)))), ((int)(((byte)(229)))));
            this.bnupdaterowexportKho.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bnupdaterowexportKho.ForeColor = System.Drawing.Color.White;
            this.bnupdaterowexportKho.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(56)))), ((int)(((byte)(202)))));
            this.bnupdaterowexportKho.Location = new System.Drawing.Point(197, 289);
            this.bnupdaterowexportKho.Name = "bnupdaterowexportKho";
            this.bnupdaterowexportKho.Size = new System.Drawing.Size(132, 40);
            this.bnupdaterowexportKho.TabIndex = 18;
            this.bnupdaterowexportKho.Text = "Sửa dòng";
            // 
            // bndeleterowexportkho
            // 
            this.bndeleterowexportkho.BorderRadius = 12;
            this.bndeleterowexportkho.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.bndeleterowexportkho.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.bndeleterowexportkho.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.bndeleterowexportkho.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.bndeleterowexportkho.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.bndeleterowexportkho.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bndeleterowexportkho.ForeColor = System.Drawing.Color.White;
            this.bndeleterowexportkho.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.bndeleterowexportkho.Location = new System.Drawing.Point(335, 289);
            this.bndeleterowexportkho.Name = "bndeleterowexportkho";
            this.bndeleterowexportkho.Size = new System.Drawing.Size(132, 40);
            this.bndeleterowexportkho.TabIndex = 17;
            this.bndeleterowexportkho.Text = "Xóa dòng";
            // 
            // lbdonviexport
            // 
            this.lbdonviexport.BackColor = System.Drawing.Color.Transparent;
            this.lbdonviexport.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbdonviexport.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lbdonviexport.Location = new System.Drawing.Point(402, 186);
            this.lbdonviexport.Name = "lbdonviexport";
            this.lbdonviexport.Size = new System.Drawing.Size(40, 19);
            this.lbdonviexport.TabIndex = 16;
            this.lbdonviexport.Text = "Đơn vị";
            // 
            // lbtypeKhoExport
            // 
            this.lbtypeKhoExport.BackColor = System.Drawing.Color.Transparent;
            this.lbtypeKhoExport.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbtypeKhoExport.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lbtypeKhoExport.Location = new System.Drawing.Point(19, 186);
            this.lbtypeKhoExport.Name = "lbtypeKhoExport";
            this.lbtypeKhoExport.Size = new System.Drawing.Size(54, 19);
            this.lbtypeKhoExport.TabIndex = 15;
            this.lbtypeKhoExport.Text = "Loại Kho";
            // 
            // cbdonviexportkho
            // 
            this.cbdonviexportkho.BackColor = System.Drawing.Color.Transparent;
            this.cbdonviexportkho.BorderRadius = 10;
            this.cbdonviexportkho.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbdonviexportkho.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbdonviexportkho.DropDownWidth = 362;
            this.cbdonviexportkho.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cbdonviexportkho.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cbdonviexportkho.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cbdonviexportkho.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cbdonviexportkho.ItemHeight = 30;
            this.cbdonviexportkho.Items.AddRange(new object[] {
            "Loại kho : Nguyên liệu",
            "Loại kho : Thiết bị"});
            this.cbdonviexportkho.Location = new System.Drawing.Point(393, 221);
            this.cbdonviexportkho.Name = "cbdonviexportkho";
            this.cbdonviexportkho.Size = new System.Drawing.Size(349, 36);
            this.cbdonviexportkho.TabIndex = 14;
            // 
            // cbtypekhoexport
            // 
            this.cbtypekhoexport.BackColor = System.Drawing.Color.Transparent;
            this.cbtypekhoexport.BorderRadius = 10;
            this.cbtypekhoexport.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbtypekhoexport.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbtypekhoexport.DropDownWidth = 362;
            this.cbtypekhoexport.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cbtypekhoexport.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cbtypekhoexport.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cbtypekhoexport.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cbtypekhoexport.ItemHeight = 30;
            this.cbtypekhoexport.Items.AddRange(new object[] {
            "Loại kho : Nguyên liệu",
            "Loại kho : Thiết bị"});
            this.cbtypekhoexport.Location = new System.Drawing.Point(19, 221);
            this.cbtypekhoexport.Name = "cbtypekhoexport";
            this.cbtypekhoexport.Size = new System.Drawing.Size(349, 36);
            this.cbtypekhoexport.TabIndex = 13;
            // 
            // lbstatusexport
            // 
            this.lbstatusexport.BackColor = System.Drawing.Color.Transparent;
            this.lbstatusexport.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbstatusexport.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lbstatusexport.Location = new System.Drawing.Point(402, 97);
            this.lbstatusexport.Name = "lbstatusexport";
            this.lbstatusexport.Size = new System.Drawing.Size(65, 19);
            this.lbstatusexport.TabIndex = 12;
            this.lbstatusexport.Text = "Trạng Thái";
            // 
            // SepBottomUpdateItems
            // 
            this.SepBottomUpdateItems.Location = new System.Drawing.Point(1, 534);
            this.SepBottomUpdateItems.Name = "SepBottomUpdateItems";
            this.SepBottomUpdateItems.Size = new System.Drawing.Size(759, 21);
            this.SepBottomUpdateItems.TabIndex = 8;
            // 
            // sepTopUpdateItems
            // 
            this.sepTopUpdateItems.Location = new System.Drawing.Point(0, 77);
            this.sepTopUpdateItems.Name = "sepTopUpdateItems";
            this.sepTopUpdateItems.Size = new System.Drawing.Size(759, 21);
            this.sepTopUpdateItems.TabIndex = 7;
            // 
            // BtnExistExport
            // 
            this.BtnExistExport.BackColor = System.Drawing.Color.White;
            this.BtnExistExport.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(231)))), ((int)(((byte)(235)))));
            this.BtnExistExport.BorderRadius = 12;
            this.BtnExistExport.BorderThickness = 1;
            this.BtnExistExport.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.BtnExistExport.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.BtnExistExport.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.BtnExistExport.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.BtnExistExport.FillColor = System.Drawing.SystemColors.ControlLight;
            this.BtnExistExport.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold);
            this.BtnExistExport.ForeColor = System.Drawing.Color.Black;
            this.BtnExistExport.Location = new System.Drawing.Point(698, 19);
            this.BtnExistExport.Name = "BtnExistExport";
            this.BtnExistExport.Size = new System.Drawing.Size(44, 40);
            this.BtnExistExport.TabIndex = 11;
            this.BtnExistExport.Text = "X";
            this.BtnExistExport.Click += new System.EventHandler(this.BtnExistExport_Click);
            // 
            // cbStatusExport
            // 
            this.cbStatusExport.BackColor = System.Drawing.Color.Transparent;
            this.cbStatusExport.BorderRadius = 10;
            this.cbStatusExport.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbStatusExport.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbStatusExport.DropDownWidth = 362;
            this.cbStatusExport.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cbStatusExport.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cbStatusExport.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cbStatusExport.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cbStatusExport.ItemHeight = 30;
            this.cbStatusExport.Items.AddRange(new object[] {
            "Loại kho : Nguyên liệu",
            "Loại kho : Thiết bị"});
            this.cbStatusExport.Location = new System.Drawing.Point(392, 122);
            this.cbStatusExport.Name = "cbStatusExport";
            this.cbStatusExport.Size = new System.Drawing.Size(349, 36);
            this.cbStatusExport.TabIndex = 5;
            // 
            // btnCancelExportKho
            // 
            this.btnCancelExportKho.BackColor = System.Drawing.Color.White;
            this.btnCancelExportKho.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(231)))), ((int)(((byte)(235)))));
            this.btnCancelExportKho.BorderRadius = 12;
            this.btnCancelExportKho.BorderThickness = 1;
            this.btnCancelExportKho.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnCancelExportKho.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnCancelExportKho.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnCancelExportKho.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnCancelExportKho.FillColor = System.Drawing.Color.White;
            this.btnCancelExportKho.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancelExportKho.ForeColor = System.Drawing.Color.Black;
            this.btnCancelExportKho.Location = new System.Drawing.Point(519, 561);
            this.btnCancelExportKho.Name = "btnCancelExportKho";
            this.btnCancelExportKho.Size = new System.Drawing.Size(67, 40);
            this.btnCancelExportKho.TabIndex = 6;
            this.btnCancelExportKho.Text = "Hủy";
            // 
            // bnCreateExportPhieu
            // 
            this.bnCreateExportPhieu.BorderRadius = 12;
            this.bnCreateExportPhieu.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.bnCreateExportPhieu.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.bnCreateExportPhieu.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.bnCreateExportPhieu.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.bnCreateExportPhieu.FillColor = System.Drawing.Color.Black;
            this.bnCreateExportPhieu.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bnCreateExportPhieu.ForeColor = System.Drawing.Color.White;
            this.bnCreateExportPhieu.Location = new System.Drawing.Point(601, 560);
            this.bnCreateExportPhieu.Name = "bnCreateExportPhieu";
            this.bnCreateExportPhieu.Size = new System.Drawing.Size(141, 40);
            this.bnCreateExportPhieu.TabIndex = 5;
            this.bnCreateExportPhieu.Text = "Tạo phiếu xuất";
            // 
            // txtMaPhieuexport
            // 
            this.txtMaPhieuexport.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(231)))), ((int)(((byte)(235)))));
            this.txtMaPhieuexport.BorderRadius = 10;
            this.txtMaPhieuexport.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtMaPhieuexport.DefaultText = "PNxxx";
            this.txtMaPhieuexport.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtMaPhieuexport.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtMaPhieuexport.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtMaPhieuexport.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtMaPhieuexport.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
            this.txtMaPhieuexport.FocusedState.FillColor = System.Drawing.Color.White;
            this.txtMaPhieuexport.FocusedState.ForeColor = System.Drawing.Color.Black;
            this.txtMaPhieuexport.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMaPhieuexport.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
            this.txtMaPhieuexport.HoverState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.txtMaPhieuexport.HoverState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(163)))), ((int)(((byte)(175)))));
            this.txtMaPhieuexport.Location = new System.Drawing.Point(19, 122);
            this.txtMaPhieuexport.Name = "txtMaPhieuexport";
            this.txtMaPhieuexport.PlaceholderText = "";
            this.txtMaPhieuexport.SelectedText = "";
            this.txtMaPhieuexport.ShadowDecoration.BorderRadius = 12;
            this.txtMaPhieuexport.ShadowDecoration.Color = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
            this.txtMaPhieuexport.Size = new System.Drawing.Size(349, 40);
            this.txtMaPhieuexport.TabIndex = 7;
            // 
            // lbMaPhieuExport
            // 
            this.lbMaPhieuExport.BackColor = System.Drawing.Color.Transparent;
            this.lbMaPhieuExport.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbMaPhieuExport.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lbMaPhieuExport.Location = new System.Drawing.Point(19, 97);
            this.lbMaPhieuExport.Name = "lbMaPhieuExport";
            this.lbMaPhieuExport.Size = new System.Drawing.Size(58, 19);
            this.lbMaPhieuExport.TabIndex = 2;
            this.lbMaPhieuExport.Text = "Mã phiếu";
            // 
            // lbExportKho
            // 
            this.lbExportKho.BackColor = System.Drawing.Color.Transparent;
            this.lbExportKho.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbExportKho.Location = new System.Drawing.Point(19, 19);
            this.lbExportKho.Name = "lbExportKho";
            this.lbExportKho.Size = new System.Drawing.Size(175, 27);
            this.lbExportKho.TabIndex = 1;
            this.lbExportKho.Text = "Tạo phiếu xuất kho";
            // 
            // pnlCardExportWarehouse
            // 
            this.pnlCardExportWarehouse.BackColor = System.Drawing.Color.Transparent;
            this.pnlCardExportWarehouse.BorderRadius = 16;
            this.pnlCardExportWarehouse.Controls.Add(this.DGVexportkho);
            this.pnlCardExportWarehouse.Controls.Add(this.bnaddrowexportKho);
            this.pnlCardExportWarehouse.Controls.Add(this.bnupdaterowexportKho);
            this.pnlCardExportWarehouse.Controls.Add(this.bndeleterowexportkho);
            this.pnlCardExportWarehouse.Controls.Add(this.lbdonviexport);
            this.pnlCardExportWarehouse.Controls.Add(this.lbtypeKhoExport);
            this.pnlCardExportWarehouse.Controls.Add(this.cbdonviexportkho);
            this.pnlCardExportWarehouse.Controls.Add(this.cbtypekhoexport);
            this.pnlCardExportWarehouse.Controls.Add(this.lbstatusexport);
            this.pnlCardExportWarehouse.Controls.Add(this.SepBottomUpdateItems);
            this.pnlCardExportWarehouse.Controls.Add(this.sepTopUpdateItems);
            this.pnlCardExportWarehouse.Controls.Add(this.BtnExistExport);
            this.pnlCardExportWarehouse.Controls.Add(this.cbStatusExport);
            this.pnlCardExportWarehouse.Controls.Add(this.btnCancelExportKho);
            this.pnlCardExportWarehouse.Controls.Add(this.bnCreateExportPhieu);
            this.pnlCardExportWarehouse.Controls.Add(this.txtMaPhieuexport);
            this.pnlCardExportWarehouse.Controls.Add(this.lbMaPhieuExport);
            this.pnlCardExportWarehouse.Controls.Add(this.lbExportKho);
            this.pnlCardExportWarehouse.FillColor = System.Drawing.Color.White;
            this.pnlCardExportWarehouse.Location = new System.Drawing.Point(226, 62);
            this.pnlCardExportWarehouse.Name = "pnlCardExportWarehouse";
            this.pnlCardExportWarehouse.Padding = new System.Windows.Forms.Padding(16);
            this.pnlCardExportWarehouse.ShadowDecoration.BorderRadius = 16;
            this.pnlCardExportWarehouse.ShadowDecoration.Depth = 10;
            this.pnlCardExportWarehouse.ShadowDecoration.Enabled = true;
            this.pnlCardExportWarehouse.Size = new System.Drawing.Size(760, 619);
            this.pnlCardExportWarehouse.TabIndex = 9;
            // 
            // FormExportWarehouse
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1213, 743);
            this.Controls.Add(this.pnlCardExportWarehouse);
            this.Name = "FormExportWarehouse";
            this.Text = "FormExportWarehouse";
            ((System.ComponentModel.ISupportInitialize)(this.DGVexportkho)).EndInit();
            this.pnlCardExportWarehouse.ResumeLayout(false);
            this.pnlCardExportWarehouse.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView DGVexportkho;
        private Guna.UI2.WinForms.Guna2Button bnaddrowexportKho;
        private Guna.UI2.WinForms.Guna2Button bnupdaterowexportKho;
        private Guna.UI2.WinForms.Guna2Button bndeleterowexportkho;
        private Guna.UI2.WinForms.Guna2HtmlLabel lbdonviexport;
        private Guna.UI2.WinForms.Guna2HtmlLabel lbtypeKhoExport;
        private Guna.UI2.WinForms.Guna2ComboBox cbdonviexportkho;
        private Guna.UI2.WinForms.Guna2ComboBox cbtypekhoexport;
        private Guna.UI2.WinForms.Guna2HtmlLabel lbstatusexport;
        private Guna.UI2.WinForms.Guna2Separator SepBottomUpdateItems;
        private Guna.UI2.WinForms.Guna2Separator sepTopUpdateItems;
        private Guna.UI2.WinForms.Guna2Button BtnExistExport;
        private Guna.UI2.WinForms.Guna2ComboBox cbStatusExport;
        private Guna.UI2.WinForms.Guna2Button btnCancelExportKho;
        private Guna.UI2.WinForms.Guna2Button bnCreateExportPhieu;
        private Guna.UI2.WinForms.Guna2TextBox txtMaPhieuexport;
        private Guna.UI2.WinForms.Guna2HtmlLabel lbMaPhieuExport;
        private Guna.UI2.WinForms.Guna2HtmlLabel lbExportKho;
        private Guna.UI2.WinForms.Guna2Panel pnlCardExportWarehouse;
    }
}