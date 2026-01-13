namespace QLChuoiNhaHangKhachSan.GUI
{
    partial class FormDownload
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblLoaiTG = new System.Windows.Forms.Label();
            this.btnHuy = new Guna.UI2.WinForms.Guna2Button();
            this.dtpFrom = new Guna.UI2.WinForms.Guna2DateTimePicker();
            this.btnTaiXuat = new Guna.UI2.WinForms.Guna2Button();
            this.dtpTo = new Guna.UI2.WinForms.Guna2DateTimePicker();
            this.lblFrom = new System.Windows.Forms.Label();
            this.lblTo = new System.Windows.Forms.Label();
            this.pnlshadowMain = new Guna.UI2.WinForms.Guna2ShadowPanel();
            this.pnlshadowMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.BackColor = System.Drawing.Color.White;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(15)))), ((int)(((byte)(15)))));
            this.lblTitle.Location = new System.Drawing.Point(165, 23);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(198, 45);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Tải Báo Cáo";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblTitle.Click += new System.EventHandler(this.lblTitle_Click);
            // 
            // lblLoaiTG
            // 
            this.lblLoaiTG.AutoSize = true;
            this.lblLoaiTG.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblLoaiTG.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.lblLoaiTG.Location = new System.Drawing.Point(21, 83);
            this.lblLoaiTG.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblLoaiTG.Name = "lblLoaiTG";
            this.lblLoaiTG.Size = new System.Drawing.Size(194, 30);
            this.lblLoaiTG.TabIndex = 1;
            this.lblLoaiTG.Text = "Chọn loại thời gian";
            // 
            // btnHuy
            // 
            this.btnHuy.AutoRoundedCorners = true;
            this.btnHuy.BorderColor = System.Drawing.Color.Silver;
            this.btnHuy.BorderRadius = 19;
            this.btnHuy.BorderThickness = 1;
            this.btnHuy.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnHuy.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnHuy.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnHuy.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnHuy.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnHuy.FillColor = System.Drawing.Color.White;
            this.btnHuy.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnHuy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.btnHuy.Location = new System.Drawing.Point(26, 307);
            this.btnHuy.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnHuy.Name = "btnHuy";
            this.btnHuy.Size = new System.Drawing.Size(189, 41);
            this.btnHuy.TabIndex = 4;
            this.btnHuy.Text = "Hủy";
            this.btnHuy.Click += new System.EventHandler(this.btnHuy_Click);
            // 
            // dtpFrom
            // 
            this.dtpFrom.BorderRadius = 10;
            this.dtpFrom.Checked = true;
            this.dtpFrom.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(36)))), ((int)(((byte)(64)))));
            this.dtpFrom.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.dtpFrom.ForeColor = System.Drawing.Color.Gainsboro;
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Long;
            this.dtpFrom.Location = new System.Drawing.Point(26, 200);
            this.dtpFrom.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dtpFrom.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtpFrom.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(206, 67);
            this.dtpFrom.TabIndex = 1;
            this.dtpFrom.Value = new System.DateTime(2026, 1, 5, 16, 4, 11, 621);
            // 
            // btnTaiXuat
            // 
            this.btnTaiXuat.AutoRoundedCorners = true;
            this.btnTaiXuat.BorderRadius = 19;
            this.btnTaiXuat.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTaiXuat.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnTaiXuat.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnTaiXuat.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnTaiXuat.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnTaiXuat.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(12)))), ((int)(((byte)(12)))), ((int)(((byte)(12)))));
            this.btnTaiXuat.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnTaiXuat.ForeColor = System.Drawing.Color.White;
            this.btnTaiXuat.Location = new System.Drawing.Point(265, 307);
            this.btnTaiXuat.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnTaiXuat.Name = "btnTaiXuat";
            this.btnTaiXuat.Size = new System.Drawing.Size(195, 41);
            this.btnTaiXuat.TabIndex = 5;
            this.btnTaiXuat.Text = "Tải Xuất";
            this.btnTaiXuat.Click += new System.EventHandler(this.btnTaiXuat_Click);
            // 
            // dtpTo
            // 
            this.dtpTo.BorderRadius = 10;
            this.dtpTo.Checked = true;
            this.dtpTo.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(36)))), ((int)(((byte)(64)))));
            this.dtpTo.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.dtpTo.ForeColor = System.Drawing.Color.Gainsboro;
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Long;
            this.dtpTo.Location = new System.Drawing.Point(265, 200);
            this.dtpTo.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dtpTo.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtpTo.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(209, 67);
            this.dtpTo.TabIndex = 2;
            this.dtpTo.Value = new System.DateTime(2026, 1, 5, 16, 5, 21, 34);
            // 
            // lblFrom
            // 
            this.lblFrom.AutoSize = true;
            this.lblFrom.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblFrom.Location = new System.Drawing.Point(67, 144);
            this.lblFrom.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(85, 25);
            this.lblFrom.TabIndex = 6;
            this.lblFrom.Text = "Từ ngày";
            // 
            // lblTo
            // 
            this.lblTo.AutoSize = true;
            this.lblTo.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblTo.Location = new System.Drawing.Point(308, 144);
            this.lblTo.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(97, 25);
            this.lblTo.TabIndex = 7;
            this.lblTo.Text = "Đến ngày";
            // 
            // pnlshadowMain
            // 
            this.pnlshadowMain.BackColor = System.Drawing.Color.Transparent;
            this.pnlshadowMain.Controls.Add(this.lblTo);
            this.pnlshadowMain.Controls.Add(this.lblFrom);
            this.pnlshadowMain.Controls.Add(this.dtpTo);
            this.pnlshadowMain.Controls.Add(this.btnTaiXuat);
            this.pnlshadowMain.Controls.Add(this.dtpFrom);
            this.pnlshadowMain.Controls.Add(this.btnHuy);
            this.pnlshadowMain.Controls.Add(this.lblLoaiTG);
            this.pnlshadowMain.Controls.Add(this.lblTitle);
            this.pnlshadowMain.FillColor = System.Drawing.Color.White;
            this.pnlshadowMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.pnlshadowMain.Location = new System.Drawing.Point(23, 29);
            this.pnlshadowMain.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pnlshadowMain.Name = "pnlshadowMain";
            this.pnlshadowMain.Padding = new System.Windows.Forms.Padding(10, 10, 10, 10);
            this.pnlshadowMain.Radius = 20;
            this.pnlshadowMain.ShadowColor = System.Drawing.Color.DarkGray;
            this.pnlshadowMain.ShadowDepth = 8;
            this.pnlshadowMain.ShadowShift = 4;
            this.pnlshadowMain.Size = new System.Drawing.Size(498, 413);
            this.pnlshadowMain.TabIndex = 0;
            this.pnlshadowMain.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlshadowMain_Paint);
            // 
            // FormDownload
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(560, 481);
            this.Controls.Add(this.pnlshadowMain);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.Name = "FormDownload";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tải Báo Cáo";
            this.Load += new System.EventHandler(this.FormDownload_Load_1);
            this.pnlshadowMain.ResumeLayout(false);
            this.pnlshadowMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblLoaiTG;
        private Guna.UI2.WinForms.Guna2Button btnHuy;
        private Guna.UI2.WinForms.Guna2DateTimePicker dtpFrom;
        private Guna.UI2.WinForms.Guna2Button btnTaiXuat;
        private Guna.UI2.WinForms.Guna2DateTimePicker dtpTo;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.Label lblTo;
        private Guna.UI2.WinForms.Guna2ShadowPanel pnlshadowMain;
    }
}