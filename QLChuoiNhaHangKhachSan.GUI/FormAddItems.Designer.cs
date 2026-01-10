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
            this.guna2Separator1 = new Guna.UI2.WinForms.Guna2Separator();
            this.sepTop = new Guna.UI2.WinForms.Guna2Separator();
            this.guna2Button1 = new Guna.UI2.WinForms.Guna2Button();
            this.cboDonVi2 = new Guna.UI2.WinForms.Guna2ComboBox();
            this.btnCancelAddItems = new Guna.UI2.WinForms.Guna2Button();
            this.btnSaveAddItems = new Guna.UI2.WinForms.Guna2Button();
            this.txNguonCanhBaoaddItems = new Guna.UI2.WinForms.Guna2TextBox();
            this.txTenHangaddItems = new Guna.UI2.WinForms.Guna2TextBox();
            this.txDonviaddItems = new Guna.UI2.WinForms.Guna2TextBox();
            this.txMaHangaddItems = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblTotalCaption = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lblTotal = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.pnlCardaddItems.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlCardaddItems
            // 
            this.pnlCardaddItems.BackColor = System.Drawing.Color.Transparent;
            this.pnlCardaddItems.BorderRadius = 16;
            this.pnlCardaddItems.Controls.Add(this.guna2Separator1);
            this.pnlCardaddItems.Controls.Add(this.sepTop);
            this.pnlCardaddItems.Controls.Add(this.guna2Button1);
            this.pnlCardaddItems.Controls.Add(this.cboDonVi2);
            this.pnlCardaddItems.Controls.Add(this.btnCancelAddItems);
            this.pnlCardaddItems.Controls.Add(this.btnSaveAddItems);
            this.pnlCardaddItems.Controls.Add(this.txNguonCanhBaoaddItems);
            this.pnlCardaddItems.Controls.Add(this.txTenHangaddItems);
            this.pnlCardaddItems.Controls.Add(this.txDonviaddItems);
            this.pnlCardaddItems.Controls.Add(this.txMaHangaddItems);
            this.pnlCardaddItems.Controls.Add(this.lblTotalCaption);
            this.pnlCardaddItems.Controls.Add(this.lblTotal);
            this.pnlCardaddItems.FillColor = System.Drawing.Color.White;
            this.pnlCardaddItems.Location = new System.Drawing.Point(245, 194);
            this.pnlCardaddItems.Name = "pnlCardaddItems";
            this.pnlCardaddItems.Padding = new System.Windows.Forms.Padding(16);
            this.pnlCardaddItems.ShadowDecoration.BorderRadius = 16;
            this.pnlCardaddItems.ShadowDecoration.Depth = 10;
            this.pnlCardaddItems.ShadowDecoration.Enabled = true;
            this.pnlCardaddItems.Size = new System.Drawing.Size(760, 420);
            this.pnlCardaddItems.TabIndex = 6;
            // 
            // guna2Separator1
            // 
            this.guna2Separator1.Location = new System.Drawing.Point(1, 321);
            this.guna2Separator1.Name = "guna2Separator1";
            this.guna2Separator1.Size = new System.Drawing.Size(759, 21);
            this.guna2Separator1.TabIndex = 8;
            // 
            // sepTop
            // 
            this.sepTop.Location = new System.Drawing.Point(0, 77);
            this.sepTop.Name = "sepTop";
            this.sepTop.Size = new System.Drawing.Size(759, 21);
            this.sepTop.TabIndex = 7;
            // 
            // guna2Button1
            // 
            this.guna2Button1.BackColor = System.Drawing.Color.White;
            this.guna2Button1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(231)))), ((int)(((byte)(235)))));
            this.guna2Button1.BorderRadius = 12;
            this.guna2Button1.BorderThickness = 1;
            this.guna2Button1.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button1.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button1.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2Button1.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2Button1.FillColor = System.Drawing.SystemColors.ControlLight;
            this.guna2Button1.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold);
            this.guna2Button1.ForeColor = System.Drawing.Color.Black;
            this.guna2Button1.Location = new System.Drawing.Point(698, 19);
            this.guna2Button1.Name = "guna2Button1";
            this.guna2Button1.Size = new System.Drawing.Size(44, 40);
            this.guna2Button1.TabIndex = 11;
            this.guna2Button1.Text = "X";
            // 
            // cboDonVi2
            // 
            this.cboDonVi2.BackColor = System.Drawing.Color.Transparent;
            this.cboDonVi2.BorderRadius = 10;
            this.cboDonVi2.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboDonVi2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDonVi2.DropDownWidth = 362;
            this.cboDonVi2.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cboDonVi2.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cboDonVi2.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cboDonVi2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cboDonVi2.ItemHeight = 30;
            this.cboDonVi2.Items.AddRange(new object[] {
            "Loại kho : Nguyên liệu",
            "Loại kho : Thiết bị"});
            this.cboDonVi2.Location = new System.Drawing.Point(19, 261);
            this.cboDonVi2.Name = "cboDonVi2";
            this.cboDonVi2.Size = new System.Drawing.Size(349, 36);
            this.cboDonVi2.TabIndex = 5;
            this.cboDonVi2.SelectedIndexChanged += new System.EventHandler(this.cboDonVi2_SelectedIndexChanged);
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
            this.btnCancelAddItems.Location = new System.Drawing.Point(601, 345);
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
            this.btnSaveAddItems.Location = new System.Drawing.Point(680, 345);
            this.btnSaveAddItems.Name = "btnSaveAddItems";
            this.btnSaveAddItems.Size = new System.Drawing.Size(61, 40);
            this.btnSaveAddItems.TabIndex = 5;
            this.btnSaveAddItems.Text = "Lưu";
            this.btnSaveAddItems.Click += new System.EventHandler(this.btnSaveAddItems_Click);
            // 
            // txNguonCanhBaoaddItems
            // 
            this.txNguonCanhBaoaddItems.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(231)))), ((int)(((byte)(235)))));
            this.txNguonCanhBaoaddItems.BorderRadius = 10;
            this.txNguonCanhBaoaddItems.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txNguonCanhBaoaddItems.DefaultText = "Ngưỡng cảnh báo";
            this.txNguonCanhBaoaddItems.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txNguonCanhBaoaddItems.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txNguonCanhBaoaddItems.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txNguonCanhBaoaddItems.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txNguonCanhBaoaddItems.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
            this.txNguonCanhBaoaddItems.FocusedState.FillColor = System.Drawing.Color.White;
            this.txNguonCanhBaoaddItems.FocusedState.ForeColor = System.Drawing.Color.Black;
            this.txNguonCanhBaoaddItems.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txNguonCanhBaoaddItems.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
            this.txNguonCanhBaoaddItems.HoverState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.txNguonCanhBaoaddItems.HoverState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(163)))), ((int)(((byte)(175)))));
            this.txNguonCanhBaoaddItems.Location = new System.Drawing.Point(392, 188);
            this.txNguonCanhBaoaddItems.Name = "txNguonCanhBaoaddItems";
            this.txNguonCanhBaoaddItems.PlaceholderText = "";
            this.txNguonCanhBaoaddItems.SelectedText = "";
            this.txNguonCanhBaoaddItems.ShadowDecoration.BorderRadius = 12;
            this.txNguonCanhBaoaddItems.ShadowDecoration.Color = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
            this.txNguonCanhBaoaddItems.Size = new System.Drawing.Size(349, 40);
            this.txNguonCanhBaoaddItems.TabIndex = 10;
            this.txNguonCanhBaoaddItems.TextChanged += new System.EventHandler(this.guna2TextBox3_TextChanged);
            // 
            // txTenHangaddItems
            // 
            this.txTenHangaddItems.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(231)))), ((int)(((byte)(235)))));
            this.txTenHangaddItems.BorderRadius = 10;
            this.txTenHangaddItems.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txTenHangaddItems.DefaultText = "Tên hàng";
            this.txTenHangaddItems.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txTenHangaddItems.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txTenHangaddItems.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txTenHangaddItems.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txTenHangaddItems.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
            this.txTenHangaddItems.FocusedState.FillColor = System.Drawing.Color.White;
            this.txTenHangaddItems.FocusedState.ForeColor = System.Drawing.Color.Black;
            this.txTenHangaddItems.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txTenHangaddItems.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
            this.txTenHangaddItems.HoverState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.txTenHangaddItems.HoverState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(163)))), ((int)(((byte)(175)))));
            this.txTenHangaddItems.Location = new System.Drawing.Point(392, 122);
            this.txTenHangaddItems.Name = "txTenHangaddItems";
            this.txTenHangaddItems.PlaceholderText = "";
            this.txTenHangaddItems.SelectedText = "";
            this.txTenHangaddItems.ShadowDecoration.BorderRadius = 12;
            this.txTenHangaddItems.ShadowDecoration.Color = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
            this.txTenHangaddItems.Size = new System.Drawing.Size(349, 40);
            this.txTenHangaddItems.TabIndex = 9;
            this.txTenHangaddItems.TextChanged += new System.EventHandler(this.guna2TextBox2_TextChanged);
            // 
            // txDonviaddItems
            // 
            this.txDonviaddItems.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(231)))), ((int)(((byte)(235)))));
            this.txDonviaddItems.BorderRadius = 10;
            this.txDonviaddItems.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txDonviaddItems.DefaultText = "Đơn vị";
            this.txDonviaddItems.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txDonviaddItems.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txDonviaddItems.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txDonviaddItems.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txDonviaddItems.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
            this.txDonviaddItems.FocusedState.FillColor = System.Drawing.Color.White;
            this.txDonviaddItems.FocusedState.ForeColor = System.Drawing.Color.Black;
            this.txDonviaddItems.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txDonviaddItems.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
            this.txDonviaddItems.HoverState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.txDonviaddItems.HoverState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(163)))), ((int)(((byte)(175)))));
            this.txDonviaddItems.Location = new System.Drawing.Point(19, 188);
            this.txDonviaddItems.Name = "txDonviaddItems";
            this.txDonviaddItems.PlaceholderText = "";
            this.txDonviaddItems.SelectedText = "";
            this.txDonviaddItems.ShadowDecoration.BorderRadius = 12;
            this.txDonviaddItems.ShadowDecoration.Color = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
            this.txDonviaddItems.Size = new System.Drawing.Size(349, 40);
            this.txDonviaddItems.TabIndex = 8;
            this.txDonviaddItems.TextChanged += new System.EventHandler(this.guna2TextBox1_TextChanged);
            // 
            // txMaHangaddItems
            // 
            this.txMaHangaddItems.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(231)))), ((int)(((byte)(235)))));
            this.txMaHangaddItems.BorderRadius = 10;
            this.txMaHangaddItems.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txMaHangaddItems.DefaultText = "Mã hàng";
            this.txMaHangaddItems.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txMaHangaddItems.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txMaHangaddItems.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txMaHangaddItems.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txMaHangaddItems.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
            this.txMaHangaddItems.FocusedState.FillColor = System.Drawing.Color.White;
            this.txMaHangaddItems.FocusedState.ForeColor = System.Drawing.Color.Black;
            this.txMaHangaddItems.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txMaHangaddItems.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
            this.txMaHangaddItems.HoverState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(41)))), ((int)(((byte)(55)))));
            this.txMaHangaddItems.HoverState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(163)))), ((int)(((byte)(175)))));
            this.txMaHangaddItems.Location = new System.Drawing.Point(19, 122);
            this.txMaHangaddItems.Name = "txMaHangaddItems";
            this.txMaHangaddItems.PlaceholderText = "";
            this.txMaHangaddItems.SelectedText = "";
            this.txMaHangaddItems.ShadowDecoration.BorderRadius = 12;
            this.txMaHangaddItems.ShadowDecoration.Color = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
            this.txMaHangaddItems.Size = new System.Drawing.Size(349, 40);
            this.txMaHangaddItems.TabIndex = 7;
            this.txMaHangaddItems.TextChanged += new System.EventHandler(this.txTimkiem_TextChanged);
            // 
            // lblTotalCaption
            // 
            this.lblTotalCaption.BackColor = System.Drawing.Color.Transparent;
            this.lblTotalCaption.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalCaption.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblTotalCaption.Location = new System.Drawing.Point(19, 52);
            this.lblTotalCaption.Name = "lblTotalCaption";
            this.lblTotalCaption.Size = new System.Drawing.Size(160, 19);
            this.lblTotalCaption.TabIndex = 2;
            this.lblTotalCaption.Text = "Tạo mới hàng hóa cho kho";
            // 
            // lblTotal
            // 
            this.lblTotal.BackColor = System.Drawing.Color.Transparent;
            this.lblTotal.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotal.Location = new System.Drawing.Point(19, 19);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(141, 27);
            this.lblTotal.TabIndex = 1;
            this.lblTotal.Text = "Thêm mặt hàng";
            this.lblTotal.Click += new System.EventHandler(this.lblTotal_Click);
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
        private Guna.UI2.WinForms.Guna2HtmlLabel lblTotalCaption;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblTotal;
        private Guna.UI2.WinForms.Guna2TextBox txMaHangaddItems;
        private Guna.UI2.WinForms.Guna2TextBox txNguonCanhBaoaddItems;
        private Guna.UI2.WinForms.Guna2TextBox txTenHangaddItems;
        private Guna.UI2.WinForms.Guna2TextBox txDonviaddItems;
        private Guna.UI2.WinForms.Guna2Button btnSaveAddItems;
        private Guna.UI2.WinForms.Guna2Button btnCancelAddItems;
        private Guna.UI2.WinForms.Guna2Button guna2Button1;
        private Guna.UI2.WinForms.Guna2ComboBox cboDonVi2;
        private Guna.UI2.WinForms.Guna2Separator guna2Separator1;
        private Guna.UI2.WinForms.Guna2Separator sepTop;
    }
}