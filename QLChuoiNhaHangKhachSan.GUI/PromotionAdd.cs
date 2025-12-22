using System;
using System.Windows.Forms;

namespace QLChuoiNhaHangKhachSan.GUI
{
    public partial class PromotionAdd : Form
    {
        private Label lblId;
        private Label lblName;
        private Label lblType;
        private Label lblObject;
        private Label lblTime;
        private Label lblStatus;

        private TextBox txtPromotionId;
        private TextBox txtPromotionName;
        private TextBox txtPromotionType;
        private TextBox txtPromotionObject;
        private TextBox txtPromotionTime;
        private TextBox txtPromotionStatus;

        private Button btnSave;
        private Button btnCancel;

        public string PromotionId { get; private set; }
        public string PromotionName { get; private set; }
        public string PromotionType { get; private set; }
        public string PromotionObject { get; private set; }
        public string PromotionTime { get; private set; }
        public string PromotionStatus { get; private set; }

        public PromotionAdd()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // TODO: validate dữ liệu nếu cần

            PromotionId = txtPromotionId.Text.Trim();
            PromotionName = txtPromotionName.Text.Trim();
            PromotionType = txtPromotionType.Text.Trim();
            PromotionObject = txtPromotionObject.Text.Trim();
            PromotionTime = txtPromotionTime.Text.Trim();
            PromotionStatus = txtPromotionStatus.Text.Trim();

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
            this.lblTime = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.txtPromotionId = new System.Windows.Forms.TextBox();
            this.txtPromotionName = new System.Windows.Forms.TextBox();
            this.txtPromotionType = new System.Windows.Forms.TextBox();
            this.txtPromotionObject = new System.Windows.Forms.TextBox();
            this.txtPromotionTime = new System.Windows.Forms.TextBox();
            this.txtPromotionStatus = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblId
            // 
            this.lblId.AutoSize = true;
            this.lblId.Font = new System.Drawing.Font("Microsoft YaHei UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblId.Location = new System.Drawing.Point(13, 31);
            this.lblId.Name = "lblId";
            this.lblId.Size = new System.Drawing.Size(123, 30);
            this.lblId.TabIndex = 0;
            this.lblId.Text = "Mã ưu đãi";
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("Microsoft YaHei UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblName.Location = new System.Drawing.Point(13, 85);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(201, 30);
            this.lblName.TabIndex = 2;
            this.lblName.Text = "Tên chương trình";
            // 
            // lblType
            // 
            this.lblType.AutoSize = true;
            this.lblType.Font = new System.Drawing.Font("Microsoft YaHei UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblType.Location = new System.Drawing.Point(13, 157);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(134, 30);
            this.lblType.TabIndex = 4;
            this.lblType.Text = "Loại ưu đãi";
            // 
            // lblObject
            // 
            this.lblObject.AutoSize = true;
            this.lblObject.Font = new System.Drawing.Font("Microsoft YaHei UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblObject.Location = new System.Drawing.Point(13, 235);
            this.lblObject.Name = "lblObject";
            this.lblObject.Size = new System.Drawing.Size(123, 30);
            this.lblObject.TabIndex = 6;
            this.lblObject.Text = "Đối tượng";
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Font = new System.Drawing.Font("Microsoft YaHei UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTime.Location = new System.Drawing.Point(13, 306);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(108, 30);
            this.lblTime.TabIndex = 8;
            this.lblTime.Text = "Thời hạn";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft YaHei UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.Location = new System.Drawing.Point(13, 383);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(126, 30);
            this.lblStatus.TabIndex = 10;
            this.lblStatus.Text = "Trạng thái";
            this.lblStatus.Click += new System.EventHandler(this.lblStatus_Click);
            // 
            // txtPromotionId
            // 
            this.txtPromotionId.Font = new System.Drawing.Font("Microsoft YaHei UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPromotionId.Location = new System.Drawing.Point(263, 24);
            this.txtPromotionId.Name = "txtPromotionId";
            this.txtPromotionId.Size = new System.Drawing.Size(321, 37);
            this.txtPromotionId.TabIndex = 1;
            // 
            // txtPromotionName
            // 
            this.txtPromotionName.Font = new System.Drawing.Font("Microsoft YaHei UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPromotionName.Location = new System.Drawing.Point(263, 82);
            this.txtPromotionName.Name = "txtPromotionName";
            this.txtPromotionName.Size = new System.Drawing.Size(321, 37);
            this.txtPromotionName.TabIndex = 3;
            // 
            // txtPromotionType
            // 
            this.txtPromotionType.Font = new System.Drawing.Font("Microsoft YaHei UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPromotionType.Location = new System.Drawing.Point(263, 154);
            this.txtPromotionType.Name = "txtPromotionType";
            this.txtPromotionType.Size = new System.Drawing.Size(321, 37);
            this.txtPromotionType.TabIndex = 5;
            // 
            // txtPromotionObject
            // 
            this.txtPromotionObject.Font = new System.Drawing.Font("Microsoft YaHei UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPromotionObject.Location = new System.Drawing.Point(263, 228);
            this.txtPromotionObject.Name = "txtPromotionObject";
            this.txtPromotionObject.Size = new System.Drawing.Size(321, 37);
            this.txtPromotionObject.TabIndex = 7;
            // 
            // txtPromotionTime
            // 
            this.txtPromotionTime.Font = new System.Drawing.Font("Microsoft YaHei UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPromotionTime.Location = new System.Drawing.Point(263, 303);
            this.txtPromotionTime.Name = "txtPromotionTime";
            this.txtPromotionTime.Size = new System.Drawing.Size(321, 37);
            this.txtPromotionTime.TabIndex = 9;
            // 
            // txtPromotionStatus
            // 
            this.txtPromotionStatus.Font = new System.Drawing.Font("Microsoft YaHei UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPromotionStatus.Location = new System.Drawing.Point(263, 376);
            this.txtPromotionStatus.Name = "txtPromotionStatus";
            this.txtPromotionStatus.Size = new System.Drawing.Size(321, 37);
            this.txtPromotionStatus.TabIndex = 11;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.Green;
            this.btnSave.Location = new System.Drawing.Point(23, 504);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(124, 63);
            this.btnSave.TabIndex = 12;
            this.btnSave.Text = "Lưu";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Maroon;
            this.btnCancel.Location = new System.Drawing.Point(460, 504);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(124, 63);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "Hủy";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // PromotionAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(633, 618);
            this.Controls.Add(this.lblId);
            this.Controls.Add(this.txtPromotionId);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.txtPromotionName);
            this.Controls.Add(this.lblType);
            this.Controls.Add(this.txtPromotionType);
            this.Controls.Add(this.lblObject);
            this.Controls.Add(this.txtPromotionObject);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.txtPromotionTime);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.txtPromotionStatus);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.Name = "PromotionAdd";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Thêm ưu đãi";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void lblStatus_Click(object sender, EventArgs e)
        {

        }
    }
}