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
        private Guna2TextBox txtPromotionCode;

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
        public string PromotionCode
        {
            get => txtPromotionCode.Text.Trim();
            set => txtPromotionCode.Text = value;
        }

        /// <summary>
        /// Lấy phần trăm giảm giá từ cboPromotionType (ví dụ: "10%" => 10)
        /// </summary>
        public decimal DiscountPercent
        {
            get
            {
                if (cboPromotionType.SelectedItem == null) return 0m;
                var text = cboPromotionType.SelectedItem.ToString().Replace("%", "").Trim();
                if (decimal.TryParse(text, out var val)) return val;
                return 0m;
            }
        }

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
            this.txtPromotionCode = new Guna.UI2.WinForms.Guna2TextBox();
            this.SuspendLayout();
            // 
            // lblId
            // 
            this.lblId.AutoSize = true;
            this.lblId.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblId.Location = new System.Drawing.Point(38, 90);
            this.lblId.Name = "lblId";
            this.lblId.Size = new System.Drawing.Size(133, 31);
            this.lblId.TabIndex = 1;
            this.lblId.Text = "Mã ưu đãi*";
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblName.Location = new System.Drawing.Point(38, 213);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(208, 31);
            this.lblName.TabIndex = 3;
            this.lblName.Text = "Tên chương trình*";
            // 
            // lblType
            // 
            this.lblType.AutoSize = true;
            this.lblType.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblType.Location = new System.Drawing.Point(526, 90);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(144, 31);
            this.lblType.TabIndex = 5;
            this.lblType.Text = "Loại ưu đãi*";
            // 
            // lblObject
            // 
            this.lblObject.AutoSize = true;
            this.lblObject.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblObject.Location = new System.Drawing.Point(528, 211);
            this.lblObject.Name = "lblObject";
            this.lblObject.Size = new System.Drawing.Size(228, 31);
            this.lblObject.TabIndex = 7;
            this.lblObject.Text = "Đối tượng áp dụng*";
            // 
            // lblStartDate
            // 
            this.lblStartDate.AutoSize = true;
            this.lblStartDate.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStartDate.Location = new System.Drawing.Point(47, 336);
            this.lblStartDate.Name = "lblStartDate";
            this.lblStartDate.Size = new System.Drawing.Size(168, 31);
            this.lblStartDate.TabIndex = 9;
            this.lblStartDate.Text = "Ngày bắt đầu*";
            this.lblStartDate.Click += new System.EventHandler(this.lblStartDate_Click);
            // 
            // lblEndDate
            // 
            this.lblEndDate.AutoSize = true;
            this.lblEndDate.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEndDate.Location = new System.Drawing.Point(528, 336);
            this.lblEndDate.Name = "lblEndDate";
            this.lblEndDate.Size = new System.Drawing.Size(174, 31);
            this.lblEndDate.TabIndex = 10;
            this.lblEndDate.Text = "Ngày kết thúc*";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.Location = new System.Drawing.Point(47, 468);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(132, 31);
            this.lblStatus.TabIndex = 13;
            this.lblStatus.Text = "Trạng thái*";
            // 
            // txtPromotionId
            // 
            this.txtPromotionId.BackColor = System.Drawing.Color.Transparent;
            this.txtPromotionId.BorderRadius = 15;
            this.txtPromotionId.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtPromotionId.DefaultText = "";
            this.txtPromotionId.Enabled = false;
            this.txtPromotionId.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPromotionId.ForeColor = System.Drawing.Color.Black;
            this.txtPromotionId.Location = new System.Drawing.Point(44, 127);
            this.txtPromotionId.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.txtPromotionId.Name = "txtPromotionId";
            this.txtPromotionId.PlaceholderText = "";
            this.txtPromotionId.ReadOnly = true;
            this.txtPromotionId.SelectedText = "";
            this.txtPromotionId.ShadowDecoration.BorderRadius = 15;
            this.txtPromotionId.ShadowDecoration.Color = System.Drawing.Color.DimGray;
            this.txtPromotionId.ShadowDecoration.Enabled = true;
            this.txtPromotionId.Size = new System.Drawing.Size(428, 66);
            this.txtPromotionId.TabIndex = 0;
            // 
            // txtPromotionName
            // 
            this.txtPromotionName.BackColor = System.Drawing.Color.Transparent;
            this.txtPromotionName.BorderRadius = 15;
            this.txtPromotionName.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtPromotionName.DefaultText = "";
            this.txtPromotionName.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPromotionName.ForeColor = System.Drawing.Color.Black;
            this.txtPromotionName.Location = new System.Drawing.Point(44, 250);
            this.txtPromotionName.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.txtPromotionName.Name = "txtPromotionName";
            this.txtPromotionName.PlaceholderText = "";
            this.txtPromotionName.SelectedText = "";
            this.txtPromotionName.ShadowDecoration.BorderRadius = 15;
            this.txtPromotionName.ShadowDecoration.Color = System.Drawing.Color.DimGray;
            this.txtPromotionName.ShadowDecoration.Enabled = true;
            this.txtPromotionName.Size = new System.Drawing.Size(428, 66);
            this.txtPromotionName.TabIndex = 1;
            // 
            // cboPromotionType
            // 
            this.cboPromotionType.BackColor = System.Drawing.Color.Transparent;
            this.cboPromotionType.BorderRadius = 15;
            this.cboPromotionType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboPromotionType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPromotionType.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cboPromotionType.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cboPromotionType.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboPromotionType.ForeColor = System.Drawing.Color.Black;
            this.cboPromotionType.ItemHeight = 50;
            this.cboPromotionType.Items.AddRange(new object[] {
            "10%",
            "20%",
            "30%",
            "40%",
            "50%"});
            this.cboPromotionType.Location = new System.Drawing.Point(532, 127);
            this.cboPromotionType.Name = "cboPromotionType";
            this.cboPromotionType.ShadowDecoration.BorderRadius = 15;
            this.cboPromotionType.ShadowDecoration.Color = System.Drawing.Color.DimGray;
            this.cboPromotionType.ShadowDecoration.Enabled = true;
            this.cboPromotionType.Size = new System.Drawing.Size(428, 56);
            this.cboPromotionType.TabIndex = 2;
            // 
            // cboPromotionObject
            // 
            this.cboPromotionObject.BackColor = System.Drawing.Color.Transparent;
            this.cboPromotionObject.BorderRadius = 15;
            this.cboPromotionObject.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboPromotionObject.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPromotionObject.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cboPromotionObject.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cboPromotionObject.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboPromotionObject.ForeColor = System.Drawing.Color.Black;
            this.cboPromotionObject.ItemHeight = 50;
            this.cboPromotionObject.Items.AddRange(new object[] {
            "Khách hàng thường",
            "Khách hàng Vip",
            "Tất cả "});
            this.cboPromotionObject.Location = new System.Drawing.Point(532, 250);
            this.cboPromotionObject.Name = "cboPromotionObject";
            this.cboPromotionObject.ShadowDecoration.BorderRadius = 15;
            this.cboPromotionObject.ShadowDecoration.Color = System.Drawing.Color.DimGray;
            this.cboPromotionObject.ShadowDecoration.Enabled = true;
            this.cboPromotionObject.Size = new System.Drawing.Size(428, 56);
            this.cboPromotionObject.TabIndex = 3;
            // 
            // dtpStartDate
            // 
            this.dtpStartDate.BackColor = System.Drawing.Color.Transparent;
            this.dtpStartDate.BorderRadius = 15;
            this.dtpStartDate.Checked = true;
            this.dtpStartDate.FillColor = System.Drawing.Color.White;
            this.dtpStartDate.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpStartDate.Location = new System.Drawing.Point(44, 379);
            this.dtpStartDate.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtpStartDate.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtpStartDate.Name = "dtpStartDate";
            this.dtpStartDate.ShadowDecoration.BorderRadius = 15;
            this.dtpStartDate.ShadowDecoration.Color = System.Drawing.Color.DimGray;
            this.dtpStartDate.ShadowDecoration.Enabled = true;
            this.dtpStartDate.Size = new System.Drawing.Size(428, 66);
            this.dtpStartDate.TabIndex = 4;
            this.dtpStartDate.Value = new System.DateTime(2025, 12, 30, 19, 5, 11, 871);
            // 
            // dtpEndDate
            // 
            this.dtpEndDate.BackColor = System.Drawing.Color.Transparent;
            this.dtpEndDate.BorderRadius = 15;
            this.dtpEndDate.Checked = true;
            this.dtpEndDate.FillColor = System.Drawing.Color.White;
            this.dtpEndDate.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpEndDate.Location = new System.Drawing.Point(532, 379);
            this.dtpEndDate.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtpEndDate.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.ShadowDecoration.BorderRadius = 15;
            this.dtpEndDate.ShadowDecoration.Color = System.Drawing.Color.DimGray;
            this.dtpEndDate.ShadowDecoration.Enabled = true;
            this.dtpEndDate.Size = new System.Drawing.Size(428, 66);
            this.dtpEndDate.TabIndex = 5;
            this.dtpEndDate.Value = new System.DateTime(2025, 12, 30, 19, 5, 11, 922);
            // 
            // cboStatus
            // 
            this.cboStatus.BackColor = System.Drawing.Color.Transparent;
            this.cboStatus.BorderRadius = 15;
            this.cboStatus.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStatus.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cboStatus.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cboStatus.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.cboStatus.ForeColor = System.Drawing.Color.Black;
            this.cboStatus.ItemHeight = 50;
            this.cboStatus.Items.AddRange(new object[] {
            "Còn",
            "Kết thúc",
            "Chưa bắt đầu"});
            this.cboStatus.Location = new System.Drawing.Point(44, 502);
            this.cboStatus.Name = "cboStatus";
            this.cboStatus.ShadowDecoration.BorderRadius = 15;
            this.cboStatus.ShadowDecoration.Color = System.Drawing.Color.DimGray;
            this.cboStatus.ShadowDecoration.Enabled = true;
            this.cboStatus.Size = new System.Drawing.Size(428, 56);
            this.cboStatus.TabIndex = 6;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.BorderRadius = 10;
            this.btnSave.FillColor = System.Drawing.Color.DodgerBlue;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(615, 587);
            this.btnSave.Name = "btnSave";
            this.btnSave.ShadowDecoration.BorderRadius = 10;
            this.btnSave.ShadowDecoration.Enabled = true;
            this.btnSave.Size = new System.Drawing.Size(163, 48);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "Lưu";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BorderRadius = 10;
            this.btnCancel.FillColor = System.Drawing.Color.Gainsboro;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnCancel.ForeColor = System.Drawing.Color.DimGray;
            this.btnCancel.Location = new System.Drawing.Point(797, 587);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.ShadowDecoration.BorderRadius = 10;
            this.btnCancel.ShadowDecoration.Enabled = true;
            this.btnCancel.Size = new System.Drawing.Size(163, 48);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Hủy";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // guna2HtmlLabel1
            // 
            this.guna2HtmlLabel1.AutoSize = false;
            this.guna2HtmlLabel1.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel1.Font = new System.Drawing.Font("Segoe UI", 22.2F, System.Drawing.FontStyle.Bold);
            this.guna2HtmlLabel1.Location = new System.Drawing.Point(-56, 12);
            this.guna2HtmlLabel1.Name = "guna2HtmlLabel1";
            this.guna2HtmlLabel1.Size = new System.Drawing.Size(423, 50);
            this.guna2HtmlLabel1.TabIndex = 0;
            this.guna2HtmlLabel1.Text = "Thêm Ưu Đãi";
            this.guna2HtmlLabel1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtPromotionCode
            // 
            this.txtPromotionCode.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtPromotionCode.DefaultText = "";
            this.txtPromotionCode.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtPromotionCode.Location = new System.Drawing.Point(0, 0);
            this.txtPromotionCode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtPromotionCode.Name = "txtPromotionCode";
            this.txtPromotionCode.PlaceholderText = "";
            this.txtPromotionCode.SelectedText = "";
            this.txtPromotionCode.Size = new System.Drawing.Size(229, 48);
            this.txtPromotionCode.TabIndex = 14;
            // 
            // PromotionAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1009, 667);
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
            this.Controls.Add(this.txtPromotionCode);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PromotionAdd";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Thêm ưu đãi";
            this.Load += new System.EventHandler(this.PromotionAdd_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void PromotionAdd_Load(object sender, EventArgs e)
        {

        }

        private void lblStartDate_Click(object sender, EventArgs e)
        {

        }
    }
}