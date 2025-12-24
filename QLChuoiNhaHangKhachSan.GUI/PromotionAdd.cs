using System;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace QLChuoiNhaHangKhachSan.GUI
{
    public partial class PromotionAdd : Form
    {
        private Label lblId;
        private Label lblName;
        private Label lblType;
        private Label lblObject;
        private Label lblStartDate;
        private Label lblEndDate;
        private Label lblStatus;

        private Guna2TextBox txtPromotionId;
        private Guna2TextBox txtPromotionName;
        private Guna2ComboBox cboPromotionType;
        private Guna2ComboBox cboPromotionObject;
        private Guna2DateTimePicker dtpStartDate;
        private Guna2DateTimePicker dtpEndDate;
        private Guna2ComboBox cboStatus;

        private Guna2Button btnSave;
        private Guna2HtmlLabel guna2HtmlLabel1;
        private Guna2Button btnCancel;

        public string PromotionId { get; private set; }
        public string PromotionName { get; private set; }
        public string PromotionType { get; private set; }
        public string PromotionObject { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public string PromotionStatus { get; private set; }

        public PromotionAdd()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            PromotionId = txtPromotionId.Text.Trim();
            PromotionName = txtPromotionName.Text.Trim();
            PromotionType = cboPromotionType.SelectedItem != null
                ? cboPromotionType.SelectedItem.ToString()
                : string.Empty;
            PromotionObject = cboPromotionObject.SelectedItem != null
                ? cboPromotionObject.SelectedItem.ToString()
                : string.Empty;

            StartDate = dtpStartDate.Value.Date;
            EndDate = dtpEndDate.Value.Date;

            PromotionStatus = cboStatus.SelectedItem != null
                ? cboStatus.SelectedItem.ToString()
                : string.Empty;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void InitializeComponent()
        {
            this.lblId = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.lblType = new System.Windows.Forms.Label();
            this.lblObject = new System.Windows.Forms.Label();
            this.lblStartDate = new System.Windows.Forms.Label();
            this.lblEndDate = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.txtPromotionId = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtPromotionName = new Guna.UI2.WinForms.Guna2TextBox();
            this.cboPromotionType = new Guna.UI2.WinForms.Guna2ComboBox();
            this.cboPromotionObject = new Guna.UI2.WinForms.Guna2ComboBox();
            this.dtpStartDate = new Guna.UI2.WinForms.Guna2DateTimePicker();
            this.dtpEndDate = new Guna.UI2.WinForms.Guna2DateTimePicker();
            this.cboStatus = new Guna.UI2.WinForms.Guna2ComboBox();
            this.btnSave = new Guna.UI2.WinForms.Guna2Button();
            this.btnCancel = new Guna.UI2.WinForms.Guna2Button();
            this.guna2HtmlLabel1 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.SuspendLayout();
            // 
            // lblId
            // 
            this.lblId.AutoSize = true;
            this.lblId.Font = new System.Drawing.Font("Segoe UI", 10.2F);
            this.lblId.Location = new System.Drawing.Point(40, 90);
            this.lblId.Name = "lblId";
            this.lblId.Size = new System.Drawing.Size(87, 23);
            this.lblId.TabIndex = 1;
            this.lblId.Text = "Mã ưu đãi";
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("Segoe UI", 10.2F);
            this.lblName.Location = new System.Drawing.Point(40, 160);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(140, 23);
            this.lblName.TabIndex = 3;
            this.lblName.Text = "Tên chương trình";
            // 
            // lblType
            // 
            this.lblType.AutoSize = true;
            this.lblType.Font = new System.Drawing.Font("Segoe UI", 10.2F);
            this.lblType.Location = new System.Drawing.Point(40, 230);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(94, 23);
            this.lblType.TabIndex = 5;
            this.lblType.Text = "Loại ưu đãi";
            // 
            // lblObject
            // 
            this.lblObject.AutoSize = true;
            this.lblObject.Font = new System.Drawing.Font("Segoe UI", 10.2F);
            this.lblObject.Location = new System.Drawing.Point(40, 300);
            this.lblObject.Name = "lblObject";
            this.lblObject.Size = new System.Drawing.Size(156, 23);
            this.lblObject.TabIndex = 7;
            this.lblObject.Text = "Đối tượng áp dụng";
            // 
            // lblStartDate
            // 
            this.lblStartDate.AutoSize = true;
            this.lblStartDate.Font = new System.Drawing.Font("Segoe UI", 10.2F);
            this.lblStartDate.Location = new System.Drawing.Point(40, 370);
            this.lblStartDate.Name = "lblStartDate";
            this.lblStartDate.Size = new System.Drawing.Size(105, 23);
            this.lblStartDate.TabIndex = 9;
            this.lblStartDate.Text = "Ngày bắt đầu";
            // 
            // lblEndDate
            // 
            this.lblEndDate.AutoSize = true;
            this.lblEndDate.Font = new System.Drawing.Font("Segoe UI", 10.2F);
            this.lblEndDate.Location = new System.Drawing.Point(376, 370);
            this.lblEndDate.Name = "lblEndDate";
            this.lblEndDate.Size = new System.Drawing.Size(108, 23);
            this.lblEndDate.TabIndex = 10;
            this.lblEndDate.Text = "Ngày kết thúc";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 10.2F);
            this.lblStatus.Location = new System.Drawing.Point(40, 440);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(87, 23);
            this.lblStatus.TabIndex = 13;
            this.lblStatus.Text = "Trạng thái";
            // 
            // txtPromotionId
            // 
            this.txtPromotionId.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtPromotionId.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtPromotionId.Location = new System.Drawing.Point(44, 116);
            this.txtPromotionId.Name = "txtPromotionId";
            this.txtPromotionId.Size = new System.Drawing.Size(632, 36);
            this.txtPromotionId.TabIndex = 0;
            // 
            // txtPromotionName
            // 
            this.txtPromotionName.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtPromotionName.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtPromotionName.Location = new System.Drawing.Point(44, 186);
            this.txtPromotionName.Name = "txtPromotionName";
            this.txtPromotionName.Size = new System.Drawing.Size(632, 36);
            this.txtPromotionName.TabIndex = 1;
            // 
            // cboPromotionType
            // 
            this.cboPromotionType.BackColor = System.Drawing.Color.Transparent;
            this.cboPromotionType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboPromotionType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPromotionType.FocusedColor = System.Drawing.Color.FromArgb(94, 148, 255);
            this.cboPromotionType.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.cboPromotionType.ForeColor = System.Drawing.Color.Black;
            this.cboPromotionType.ItemHeight = 30;
            this.cboPromotionType.Items.AddRange(new object[] { "10%", "20%", "30%", "40%", "50%" });
            this.cboPromotionType.Location = new System.Drawing.Point(44, 256);
            this.cboPromotionType.Name = "cboPromotionType";
            this.cboPromotionType.Size = new System.Drawing.Size(632, 36);
            this.cboPromotionType.TabIndex = 2;
            // 
            // cboPromotionObject
            // 
            this.cboPromotionObject.BackColor = System.Drawing.Color.Transparent;
            this.cboPromotionObject.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboPromotionObject.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPromotionObject.FocusedColor = System.Drawing.Color.FromArgb(94, 148, 255);
            this.cboPromotionObject.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.cboPromotionObject.ForeColor = System.Drawing.Color.Black;
            this.cboPromotionObject.ItemHeight = 30;
            this.cboPromotionObject.Items.AddRange(new object[] { "VIP", "Tất cả" });
            this.cboPromotionObject.Location = new System.Drawing.Point(44, 326);
            this.cboPromotionObject.Name = "cboPromotionObject";
            this.cboPromotionObject.Size = new System.Drawing.Size(632, 36);
            this.cboPromotionObject.TabIndex = 3;
            // 
            // dtpStartDate
            // 
            this.dtpStartDate.Checked = true;
            this.dtpStartDate.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.dtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpStartDate.Location = new System.Drawing.Point(44, 396);
            this.dtpStartDate.Name = "dtpStartDate";
            this.dtpStartDate.Size = new System.Drawing.Size(300, 36);
            this.dtpStartDate.TabIndex = 4;
            // 
            // dtpEndDate
            // 
            this.dtpEndDate.Checked = true;
            this.dtpEndDate.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.dtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpEndDate.Location = new System.Drawing.Point(380, 396);
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.Size = new System.Drawing.Size(296, 36);
            this.dtpEndDate.TabIndex = 5;
            // 
            // cboStatus
            // 
            this.cboStatus.BackColor = System.Drawing.Color.Transparent;
            this.cboStatus.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStatus.FocusedColor = System.Drawing.Color.FromArgb(94, 148, 255);
            this.cboStatus.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.cboStatus.ForeColor = System.Drawing.Color.Black;
            this.cboStatus.ItemHeight = 30;
            this.cboStatus.Items.AddRange(new object[] { "Còn", "Kết thúc", "Chưa bắt đầu" });
            this.cboStatus.Location = new System.Drawing.Point(44, 466);
            this.cboStatus.Name = "cboStatus";
            this.cboStatus.Size = new System.Drawing.Size(632, 36);
            this.cboStatus.TabIndex = 6;
            // 
            // btnSave
            // 
            this.btnSave.BorderRadius = 8;
            this.btnSave.FillColor = System.Drawing.Color.DodgerBlue;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(44, 520);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(310, 48);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "Lưu";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BorderRadius = 8;
            this.btnCancel.FillColor = System.Drawing.Color.Gainsboro;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnCancel.ForeColor = System.Drawing.Color.DimGray;
            this.btnCancel.Location = new System.Drawing.Point(366, 520);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(310, 48);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Hủy";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // guna2HtmlLabel1
            // 
            this.guna2HtmlLabel1.AutoSize = false;
            this.guna2HtmlLabel1.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel1.Font = new System.Drawing.Font("Segoe UI", 22.2F, System.Drawing.FontStyle.Bold);
            this.guna2HtmlLabel1.Location = new System.Drawing.Point(84, 25);
            this.guna2HtmlLabel1.Name = "guna2HtmlLabel1";
            this.guna2HtmlLabel1.Size = new System.Drawing.Size(423, 50);
            this.guna2HtmlLabel1.TabIndex = 0;
            this.guna2HtmlLabel1.Text = "Thêm Ưu Đãi";
            this.guna2HtmlLabel1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PromotionAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(720, 640);
            this.Controls.Add(this.guna2HtmlLabel1);
            this.Controls.Add(this.lblId);
            this.Controls.Add(this.txtPromotionId);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.txtPromotionName);
            this.Controls.Add(this.lblType);
            this.Controls.Add(this.cboPromotionType);
            this.Controls.Add(this.lblObject);
            this.Controls.Add(this.cboPromotionObject);
            this.Controls.Add(this.lblStartDate);
            this.Controls.Add(this.dtpStartDate);
            this.Controls.Add(this.lblEndDate);
            this.Controls.Add(this.dtpEndDate);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.cboStatus);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PromotionAdd";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Thêm ưu đãi";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}