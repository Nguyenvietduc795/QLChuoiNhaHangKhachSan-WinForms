using System;
using System.Windows.Forms;
namespace QLChuoiNhaHangKhachSan.GUI
{
    partial class FormAddMatHang
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
            this.pnlCardaddItems = new Guna.UI2.WinForms.Guna2Panel();
            this.cbTypeaddItems = new Guna.UI2.WinForms.Guna2ComboBox();
            this.txMinStockaddItems = new Guna.UI2.WinForms.Guna2TextBox();
            this.txStockQuantityaddItems = new Guna.UI2.WinForms.Guna2TextBox();
            this.sepbottom = new Guna.UI2.WinForms.Guna2Separator();
            this.sepTop = new Guna.UI2.WinForms.Guna2Separator();
            this.bnExistaddItems = new Guna.UI2.WinForms.Guna2Button();
            this.btnCancelAddItems = new Guna.UI2.WinForms.Guna2Button();
            this.btnSaveAddItems = new Guna.UI2.WinForms.Guna2Button();
            this.txDefaultPriceaddItems = new Guna.UI2.WinForms.Guna2TextBox();
            this.txNameaddItems = new Guna.UI2.WinForms.Guna2TextBox();
            this.txUnitaddItems = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblTotalCaptionaddItems = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lblTotaladdItems = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.pnlCardaddItems.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlCardaddItems
            // 
            this.pnlCardaddItems.BackColor = System.Drawing.Color.Transparent;
            this.pnlCardaddItems.BorderRadius = 16;
            this.pnlCardaddItems.Controls.Add(this.cbTypeaddItems);
            this.pnlCardaddItems.Controls.Add(this.txMinStockaddItems);
            this.pnlCardaddItems.Controls.Add(this.txStockQuantityaddItems);
            this.pnlCardaddItems.Controls.Add(this.sepbottom);
            this.pnlCardaddItems.Controls.Add(this.sepTop);
            this.pnlCardaddItems.Controls.Add(this.bnExistaddItems);
            this.pnlCardaddItems.Controls.Add(this.btnCancelAddItems);
            this.pnlCardaddItems.Controls.Add(this.btnSaveAddItems);
            this.pnlCardaddItems.Controls.Add(this.txDefaultPriceaddItems);
            this.pnlCardaddItems.Controls.Add(this.txNameaddItems);
            this.pnlCardaddItems.Controls.Add(this.txUnitaddItems);
            this.pnlCardaddItems.Controls.Add(this.lblTotalCaptionaddItems);
            this.pnlCardaddItems.Controls.Add(this.lblTotaladdItems);
            this.pnlCardaddItems.FillColor = System.Drawing.Color.White;
            this.pnlCardaddItems.Location = new System.Drawing.Point(245, 194);
            this.pnlCardaddItems.Name = "pnlCardaddItems";
            this.pnlCardaddItems.Padding = new System.Windows.Forms.Padding(16);
            this.pnlCardaddItems.ShadowDecoration.BorderRadius = 16;
            this.pnlCardaddItems.ShadowDecoration.Depth = 10;
            this.pnlCardaddItems.ShadowDecoration.Enabled = true;
            this.pnlCardaddItems.Size = new System.Drawing.Size(760, 424);
            this.pnlCardaddItems.TabIndex = 6;
            // 
            // cbTypeaddItems
            // 
            this.cbTypeaddItems.BackColor = System.Drawing.Color.Transparent;
            this.cbTypeaddItems.BorderRadius = 10;
            this.cbTypeaddItems.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbTypeaddItems.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTypeaddItems.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cbTypeaddItems.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cbTypeaddItems.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cbTypeaddItems.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cbTypeaddItems.ItemHeight = 30;
            this.cbTypeaddItems.Items.AddRange(new object[] {
            "Kho Thiết bị",
            "Kho Nguyên liệu"});
            this.cbTypeaddItems.Location = new System.Drawing.Point(18, 119);
            this.cbTypeaddItems.Name = "cbTypeaddItems";
            this.cbTypeaddItems.Size = new System.Drawing.Size(350, 36);
            this.cbTypeaddItems.TabIndex = 27;
            // 
            // txMinStockaddItems
            // 
            this.txMinStockaddItems.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(231)))), ((int)(((byte)(235)))));
            this.txMinStockaddItems.BorderRadius = 10;
            this.txMinStockaddItems.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txMinStockaddItems.DefaultText = "";
            this.txMinStockaddItems.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txMinStockaddItems.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txMinStockaddItems.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txMinStockaddItems.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txMinStockaddItems.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
            this.txMinStockaddItems.FocusedState.FillColor = System.Drawing.Color.White;
            this.txMinStockaddItems.FocusedState.ForeColor = System.Drawing.Color.Black;
            this.txMinStockaddItems.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txMinStockaddItems.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
            this.txMinStockaddItems.HoverState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.txMinStockaddItems.HoverState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(163)))), ((int)(((byte)(175)))));
            this.txMinStockaddItems.Location = new System.Drawing.Point(391, 256);
            this.txMinStockaddItems.Name = "txMinStockaddItems";
            this.txMinStockaddItems.PlaceholderText = "Ngưỡng cảnh báo";
            this.txMinStockaddItems.SelectedText = "";
            this.txMinStockaddItems.ShadowDecoration.BorderRadius = 12;
            this.txMinStockaddItems.ShadowDecoration.Color = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
            this.txMinStockaddItems.Size = new System.Drawing.Size(349, 40);
            this.txMinStockaddItems.TabIndex = 13;
            // 
            // txStockQuantityaddItems
            // 
            this.txStockQuantityaddItems.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(231)))), ((int)(((byte)(235)))));
            this.txStockQuantityaddItems.BorderRadius = 10;
            this.txStockQuantityaddItems.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txStockQuantityaddItems.DefaultText = "";
            this.txStockQuantityaddItems.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txStockQuantityaddItems.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txStockQuantityaddItems.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txStockQuantityaddItems.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txStockQuantityaddItems.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
            this.txStockQuantityaddItems.FocusedState.FillColor = System.Drawing.Color.White;
            this.txStockQuantityaddItems.FocusedState.ForeColor = System.Drawing.Color.Black;
            this.txStockQuantityaddItems.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txStockQuantityaddItems.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
            this.txStockQuantityaddItems.HoverState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.txStockQuantityaddItems.HoverState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(163)))), ((int)(((byte)(175)))));
            this.txStockQuantityaddItems.Location = new System.Drawing.Point(19, 256);
            this.txStockQuantityaddItems.Name = "txStockQuantityaddItems";
            this.txStockQuantityaddItems.PlaceholderText = "Tồn";
            this.txStockQuantityaddItems.SelectedText = "";
            this.txStockQuantityaddItems.ShadowDecoration.BorderRadius = 12;
            this.txStockQuantityaddItems.ShadowDecoration.Color = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
            this.txStockQuantityaddItems.Size = new System.Drawing.Size(349, 40);
            this.txStockQuantityaddItems.TabIndex = 12;
            // 
            // sepbottom
            // 
            this.sepbottom.Location = new System.Drawing.Point(0, 334);
            this.sepbottom.Name = "sepbottom";
            this.sepbottom.Size = new System.Drawing.Size(759, 21);
            this.sepbottom.TabIndex = 8;
            // 
            // sepTop
            // 
            this.sepTop.Location = new System.Drawing.Point(0, 77);
            this.sepTop.Name = "sepTop";
            this.sepTop.Size = new System.Drawing.Size(759, 21);
            this.sepTop.TabIndex = 7;
            // 
            // bnExistaddItems
            // 
            this.bnExistaddItems.BackColor = System.Drawing.Color.White;
            this.bnExistaddItems.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(231)))), ((int)(((byte)(235)))));
            this.bnExistaddItems.BorderRadius = 12;
            this.bnExistaddItems.BorderThickness = 1;
            this.bnExistaddItems.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.bnExistaddItems.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.bnExistaddItems.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.bnExistaddItems.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.bnExistaddItems.FillColor = System.Drawing.SystemColors.ControlLight;
            this.bnExistaddItems.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold);
            this.bnExistaddItems.ForeColor = System.Drawing.Color.Black;
            this.bnExistaddItems.Location = new System.Drawing.Point(698, 19);
            this.bnExistaddItems.Name = "bnExistaddItems";
            this.bnExistaddItems.Size = new System.Drawing.Size(44, 40);
            this.bnExistaddItems.TabIndex = 11;
            this.bnExistaddItems.Text = "X";
            this.bnExistaddItems.Click += new System.EventHandler(this.bnExistaddItems_Click);
            // 
            // btnCancelAddItems
            // 
            this.btnCancelAddItems.BackColor = System.Drawing.Color.White;
            this.btnCancelAddItems.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(231)))), ((int)(((byte)(235)))));
            this.btnCancelAddItems.BorderRadius = 12;
            this.btnCancelAddItems.BorderThickness = 1;
            this.btnCancelAddItems.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnCancelAddItems.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnCancelAddItems.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnCancelAddItems.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnCancelAddItems.FillColor = System.Drawing.Color.White;
            this.btnCancelAddItems.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancelAddItems.ForeColor = System.Drawing.Color.Black;
            this.btnCancelAddItems.Location = new System.Drawing.Point(599, 361);
            this.btnCancelAddItems.Name = "btnCancelAddItems";
            this.btnCancelAddItems.Size = new System.Drawing.Size(62, 40);
            this.btnCancelAddItems.TabIndex = 6;
            this.btnCancelAddItems.Text = "Hủy";
            this.btnCancelAddItems.Click += new System.EventHandler(this.btnCancelAddItems_Click);
            // 
            // btnSaveAddItems
            // 
            this.btnSaveAddItems.BorderRadius = 12;
            this.btnSaveAddItems.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnSaveAddItems.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnSaveAddItems.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnSaveAddItems.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnSaveAddItems.FillColor = System.Drawing.Color.Black;
            this.btnSaveAddItems.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveAddItems.ForeColor = System.Drawing.Color.White;
            this.btnSaveAddItems.Location = new System.Drawing.Point(678, 361);
            this.btnSaveAddItems.Name = "btnSaveAddItems";
            this.btnSaveAddItems.Size = new System.Drawing.Size(61, 40);
            this.btnSaveAddItems.TabIndex = 5;
            this.btnSaveAddItems.Text = "Lưu";
            this.btnSaveAddItems.Click += new System.EventHandler(this.btnSaveAddItems_Click);
            // 
            // txDefaultPriceaddItems
            // 
            this.txDefaultPriceaddItems.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(231)))), ((int)(((byte)(235)))));
            this.txDefaultPriceaddItems.BorderRadius = 10;
            this.txDefaultPriceaddItems.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txDefaultPriceaddItems.DefaultText = "";
            this.txDefaultPriceaddItems.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txDefaultPriceaddItems.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txDefaultPriceaddItems.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txDefaultPriceaddItems.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txDefaultPriceaddItems.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
            this.txDefaultPriceaddItems.FocusedState.FillColor = System.Drawing.Color.White;
            this.txDefaultPriceaddItems.FocusedState.ForeColor = System.Drawing.Color.Black;
            this.txDefaultPriceaddItems.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.txDefaultPriceaddItems.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
            this.txDefaultPriceaddItems.HoverState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.txDefaultPriceaddItems.HoverState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(163)))), ((int)(((byte)(175)))));
            this.txDefaultPriceaddItems.Location = new System.Drawing.Point(392, 188);
            this.txDefaultPriceaddItems.Name = "txDefaultPriceaddItems";
            this.txDefaultPriceaddItems.PlaceholderText = "Giá nhập";
            this.txDefaultPriceaddItems.SelectedText = "";
            this.txDefaultPriceaddItems.ShadowDecoration.BorderRadius = 12;
            this.txDefaultPriceaddItems.ShadowDecoration.Color = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
            this.txDefaultPriceaddItems.Size = new System.Drawing.Size(349, 40);
            this.txDefaultPriceaddItems.TabIndex = 10;
            this.txDefaultPriceaddItems.TextChanged += new System.EventHandler(this.guna2TextBox3_TextChanged);
            // 
            // txNameaddItems
            // 
            this.txNameaddItems.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(231)))), ((int)(((byte)(235)))));
            this.txNameaddItems.BorderRadius = 10;
            this.txNameaddItems.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txNameaddItems.DefaultText = "";
            this.txNameaddItems.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txNameaddItems.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txNameaddItems.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txNameaddItems.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txNameaddItems.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
            this.txNameaddItems.FocusedState.FillColor = System.Drawing.Color.White;
            this.txNameaddItems.FocusedState.ForeColor = System.Drawing.Color.Black;
            this.txNameaddItems.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.txNameaddItems.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
            this.txNameaddItems.HoverState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.txNameaddItems.HoverState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(163)))), ((int)(((byte)(175)))));
            this.txNameaddItems.Location = new System.Drawing.Point(391, 115);
            this.txNameaddItems.Name = "txNameaddItems";
            this.txNameaddItems.PlaceholderText = "Tên hàng";
            this.txNameaddItems.SelectedText = "";
            this.txNameaddItems.ShadowDecoration.BorderRadius = 12;
            this.txNameaddItems.ShadowDecoration.Color = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
            this.txNameaddItems.Size = new System.Drawing.Size(349, 40);
            this.txNameaddItems.TabIndex = 9;
            this.txNameaddItems.TextChanged += new System.EventHandler(this.guna2TextBox2_TextChanged);
            // 
            // txUnitaddItems
            // 
            this.txUnitaddItems.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(231)))), ((int)(((byte)(235)))));
            this.txUnitaddItems.BorderRadius = 10;
            this.txUnitaddItems.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txUnitaddItems.DefaultText = "";
            this.txUnitaddItems.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txUnitaddItems.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txUnitaddItems.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txUnitaddItems.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txUnitaddItems.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
            this.txUnitaddItems.FocusedState.FillColor = System.Drawing.Color.White;
            this.txUnitaddItems.FocusedState.ForeColor = System.Drawing.Color.Black;
            this.txUnitaddItems.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.txUnitaddItems.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
            this.txUnitaddItems.HoverState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.txUnitaddItems.HoverState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(163)))), ((int)(((byte)(175)))));
            this.txUnitaddItems.Location = new System.Drawing.Point(19, 188);
            this.txUnitaddItems.Name = "txUnitaddItems";
            this.txUnitaddItems.PlaceholderText = "Đơn vị";
            this.txUnitaddItems.SelectedText = "";
            this.txUnitaddItems.ShadowDecoration.BorderRadius = 12;
            this.txUnitaddItems.ShadowDecoration.Color = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
            this.txUnitaddItems.Size = new System.Drawing.Size(349, 40);
            this.txUnitaddItems.TabIndex = 8;
            this.txUnitaddItems.TextChanged += new System.EventHandler(this.guna2TextBox1_TextChanged);
            // 
            // lblTotalCaptionaddItems
            // 
            this.lblTotalCaptionaddItems.BackColor = System.Drawing.Color.Transparent;
            this.lblTotalCaptionaddItems.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalCaptionaddItems.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblTotalCaptionaddItems.Location = new System.Drawing.Point(19, 52);
            this.lblTotalCaptionaddItems.Name = "lblTotalCaptionaddItems";
            this.lblTotalCaptionaddItems.Size = new System.Drawing.Size(160, 19);
            this.lblTotalCaptionaddItems.TabIndex = 2;
            this.lblTotalCaptionaddItems.Text = "Tạo mới hàng hóa cho kho";
            // 
            // lblTotaladdItems
            // 
            this.lblTotaladdItems.BackColor = System.Drawing.Color.Transparent;
            this.lblTotaladdItems.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotaladdItems.Location = new System.Drawing.Point(19, 19);
            this.lblTotaladdItems.Name = "lblTotaladdItems";
            this.lblTotaladdItems.Size = new System.Drawing.Size(141, 27);
            this.lblTotaladdItems.TabIndex = 1;
            this.lblTotaladdItems.Text = "Thêm mặt hàng";
            this.lblTotaladdItems.Click += new System.EventHandler(this.lblTotal_Click);
            // 
            // FormAddMatHang
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1270, 892);
            this.Controls.Add(this.pnlCardaddItems);
            this.Name = "FormAddMatHang";
            this.Text = "FormAddMatHang";
            this.Load += new System.EventHandler(this.FormAddMatHang_Load);
            this.pnlCardaddItems.ResumeLayout(false);
            this.pnlCardaddItems.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel pnlCardaddItems;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblTotalCaptionaddItems;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblTotaladdItems;
        private Guna.UI2.WinForms.Guna2TextBox txDefaultPriceaddItems;
        private Guna.UI2.WinForms.Guna2TextBox txNameaddItems;
        private Guna.UI2.WinForms.Guna2TextBox txUnitaddItems;
        private Guna.UI2.WinForms.Guna2Button btnSaveAddItems;
        private Guna.UI2.WinForms.Guna2Button btnCancelAddItems;
        private Guna.UI2.WinForms.Guna2Button bnExistaddItems;
        private Guna.UI2.WinForms.Guna2Separator sepbottom;
        private Guna.UI2.WinForms.Guna2Separator sepTop;
        private Guna.UI2.WinForms.Guna2TextBox txMinStockaddItems;
        private Guna.UI2.WinForms.Guna2TextBox txStockQuantityaddItems;
        private Guna.UI2.WinForms.Guna2ComboBox cbTypeaddItems;
    }
}