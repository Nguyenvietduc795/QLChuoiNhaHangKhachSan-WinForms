namespace QLChuoiNhaHangKhachSan.GUI
{
    partial class CustomersReport
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvGuestReport = new Guna.UI2.WinForms.Guna2DataGridView();
            this.colCategory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGuest = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPercentage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblGuestTitle = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            this.guna2Panel2 = new Guna.UI2.WinForms.Guna2Panel();
            this.lblGuestReport_EndDate = new System.Windows.Forms.Label();
            this.dtpGuestReport_EndDate = new Guna.UI2.WinForms.Guna2DateTimePicker();
            this.lblGuestReport_StartDate = new System.Windows.Forms.Label();
            this.dtpGuestReport_StartDate = new Guna.UI2.WinForms.Guna2DateTimePicker();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.chartGuestReport = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGuestReport)).BeginInit();
            this.guna2Panel1.SuspendLayout();
            this.guna2Panel2.SuspendLayout();
            this.pnlContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartGuestReport)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvGuestReport
            // 
            this.dgvGuestReport.AllowUserToAddRows = false;
            this.dgvGuestReport.AllowUserToDeleteRows = false;
            this.dgvGuestReport.AllowUserToResizeColumns = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.dgvGuestReport.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvGuestReport.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                        | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(6)))), ((int)(((byte)(23)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(10);
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvGuestReport.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvGuestReport.ColumnHeadersHeight = 45;
            this.dgvGuestReport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dgvGuestReport.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCategory,
            this.colGuest,
            this.colPercentage});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(6)))), ((int)(((byte)(23)))));
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvGuestReport.DefaultCellStyle = dataGridViewCellStyle5;
            this.dgvGuestReport.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.dgvGuestReport.Location = new System.Drawing.Point(40, 20);
            this.dgvGuestReport.Name = "dgvGuestReport";
            this.dgvGuestReport.ReadOnly = true;
            this.dgvGuestReport.RowHeadersVisible = false;
            this.dgvGuestReport.RowHeadersWidth = 51;
            this.dgvGuestReport.RowTemplate.Height = 32;
            this.dgvGuestReport.Size = new System.Drawing.Size(1000, 210);
            this.dgvGuestReport.TabIndex = 0;
            this.dgvGuestReport.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            // 
            // colCategory
            // 
            this.colCategory.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colCategory.DataPropertyName = "category";
            this.colCategory.FillWeight = 35F;
            this.colCategory.HeaderText = "Danh mục";
            this.colCategory.MinimumWidth = 80;
            this.colCategory.Name = "colCategory";
            this.colCategory.ReadOnly = true;
            this.colCategory.Width = 334;
            // 
            // colGuest
            // 
            this.colGuest.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colGuest.DataPropertyName = "Guest";
            dataGridViewCellStyle3.Format = "0.00";
            this.colGuest.DefaultCellStyle = dataGridViewCellStyle3;
            this.colGuest.FillWeight = 35F;
            this.colGuest.HeaderText = "Khách hàng";
            this.colGuest.MinimumWidth = 6;
            this.colGuest.Name = "colGuest";
            this.colGuest.ReadOnly = true;
            this.colGuest.Width = 333;
            // 
            // colPercentage
            // 
            this.colPercentage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colPercentage.DataPropertyName = "Percentage";
            dataGridViewCellStyle4.Format = "0.##'%'";
            this.colPercentage.DefaultCellStyle = dataGridViewCellStyle4;
            this.colPercentage.FillWeight = 30F;
            this.colPercentage.HeaderText = "Phần trăm";
            this.colPercentage.MinimumWidth = 6;
            this.colPercentage.Name = "colPercentage";
            this.colPercentage.ReadOnly = true;
            this.colPercentage.Width = 333;
            // 
            // lblGuestTitle
            // 
            this.lblGuestTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblGuestTitle.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGuestTitle.Location = new System.Drawing.Point(20, 15);
            this.lblGuestTitle.Name = "lblGuestTitle";
            this.lblGuestTitle.Size = new System.Drawing.Size(502, 56);
            this.lblGuestTitle.TabIndex = 0;
            this.lblGuestTitle.Text = "Báo cáo lượng khách hàng";
            // 
            // guna2Panel1
            // 
            this.guna2Panel1.Controls.Add(this.lblGuestTitle);
            this.guna2Panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.guna2Panel1.Location = new System.Drawing.Point(0, 85);
            this.guna2Panel1.Name = "guna2Panel1";
            this.guna2Panel1.Size = new System.Drawing.Size(1082, 90);
            this.guna2Panel1.TabIndex = 10;
            // 
            // guna2Panel2
            // 
            this.guna2Panel2.Controls.Add(this.lblGuestReport_EndDate);
            this.guna2Panel2.Controls.Add(this.dtpGuestReport_EndDate);
            this.guna2Panel2.Controls.Add(this.lblGuestReport_StartDate);
            this.guna2Panel2.Controls.Add(this.dtpGuestReport_StartDate);
            this.guna2Panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.guna2Panel2.Location = new System.Drawing.Point(0, 0);
            this.guna2Panel2.Name = "guna2Panel2";
            this.guna2Panel2.Padding = new System.Windows.Forms.Padding(20, 10, 20, 10);
            this.guna2Panel2.Size = new System.Drawing.Size(1082, 85);
            this.guna2Panel2.TabIndex = 11;
            // 
            // lblGuestReport_EndDate
            // 
            this.lblGuestReport_EndDate.AutoSize = true;
            this.lblGuestReport_EndDate.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGuestReport_EndDate.Location = new System.Drawing.Point(550, 30);
            this.lblGuestReport_EndDate.Name = "lblGuestReport_EndDate";
            this.lblGuestReport_EndDate.Size = new System.Drawing.Size(117, 23);
            this.lblGuestReport_EndDate.TabIndex = 11;
            this.lblGuestReport_EndDate.Text = "Ngày kết thúc";
            // 
            // dtpGuestReport_EndDate
            // 
            this.dtpGuestReport_EndDate.BorderRadius = 8;
            this.dtpGuestReport_EndDate.Checked = true;
            this.dtpGuestReport_EndDate.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(6)))), ((int)(((byte)(23)))));
            this.dtpGuestReport_EndDate.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtpGuestReport_EndDate.ForeColor = System.Drawing.Color.White;
            this.dtpGuestReport_EndDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpGuestReport_EndDate.Location = new System.Drawing.Point(685, 23);
            this.dtpGuestReport_EndDate.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtpGuestReport_EndDate.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtpGuestReport_EndDate.Name = "dtpGuestReport_EndDate";
            this.dtpGuestReport_EndDate.Size = new System.Drawing.Size(260, 50);
            this.dtpGuestReport_EndDate.TabIndex = 10;
            this.dtpGuestReport_EndDate.Value = new System.DateTime(2026, 1, 6, 21, 5, 24, 551);
            // 
            // lblGuestReport_StartDate
            // 
            this.lblGuestReport_StartDate.AutoSize = true;
            this.lblGuestReport_StartDate.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGuestReport_StartDate.Location = new System.Drawing.Point(20, 30);
            this.lblGuestReport_StartDate.Name = "lblGuestReport_StartDate";
            this.lblGuestReport_StartDate.Size = new System.Drawing.Size(114, 23);
            this.lblGuestReport_StartDate.TabIndex = 9;
            this.lblGuestReport_StartDate.Text = "Ngày bắt đầu";
            // 
            // dtpGuestReport_StartDate
            // 
            this.dtpGuestReport_StartDate.BorderRadius = 8;
            this.dtpGuestReport_StartDate.Checked = true;
            this.dtpGuestReport_StartDate.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(6)))), ((int)(((byte)(23)))));
            this.dtpGuestReport_StartDate.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtpGuestReport_StartDate.ForeColor = System.Drawing.Color.White;
            this.dtpGuestReport_StartDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpGuestReport_StartDate.Location = new System.Drawing.Point(140, 23);
            this.dtpGuestReport_StartDate.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtpGuestReport_StartDate.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtpGuestReport_StartDate.Name = "dtpGuestReport_StartDate";
            this.dtpGuestReport_StartDate.Size = new System.Drawing.Size(260, 50);
            this.dtpGuestReport_StartDate.TabIndex = 8;
            this.dtpGuestReport_StartDate.Value = new System.DateTime(2026, 1, 6, 21, 5, 24, 551);
            // 
            // pnlContent
            // 
            this.pnlContent.Controls.Add(this.chartGuestReport);
            this.pnlContent.Controls.Add(this.dgvGuestReport);
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(0, 175);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Padding = new System.Windows.Forms.Padding(40, 10, 40, 20);
            this.pnlContent.Size = new System.Drawing.Size(1082, 498);
            this.pnlContent.TabIndex = 17;
            // 
            // chartGuestReport
            // 
            this.chartGuestReport.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea1.Name = "ChartArea1";
            this.chartGuestReport.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chartGuestReport.Legends.Add(legend1);
            this.chartGuestReport.Location = new System.Drawing.Point(40, 240);
            this.chartGuestReport.Name = "chartGuestReport";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Khách sạn";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Nhà hàng";
            this.chartGuestReport.Series.Add(series1);
            this.chartGuestReport.Series.Add(series2);
            this.chartGuestReport.Size = new System.Drawing.Size(1000, 240);
            this.chartGuestReport.TabIndex = 17;
            this.chartGuestReport.Text = "chart1";
            // 
            // CustomersReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1082, 673);
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.guna2Panel1);
            this.Controls.Add(this.guna2Panel2);
            this.Name = "CustomersReport";
            this.Text = "CustomersReport";
            ((System.ComponentModel.ISupportInitialize)(this.dgvGuestReport)).EndInit();
            this.guna2Panel1.ResumeLayout(false);
            this.guna2Panel1.PerformLayout();
            this.guna2Panel2.ResumeLayout(false);
            this.guna2Panel2.PerformLayout();
            this.pnlContent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartGuestReport)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2DataGridView dgvGuestReport;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCategory;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGuest;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPercentage;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblGuestTitle;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel2;
        private System.Windows.Forms.Label lblGuestReport_EndDate;
        private Guna.UI2.WinForms.Guna2DateTimePicker dtpGuestReport_EndDate;
        private System.Windows.Forms.Label lblGuestReport_StartDate;
        private Guna.UI2.WinForms.Guna2DateTimePicker dtpGuestReport_StartDate;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartGuestReport;
        private System.Windows.Forms.Panel pnlContent;
    }
}