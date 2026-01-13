namespace QLChuoiNhaHangKhachSan.GUI
{
    partial class FormRevenue
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            this.lblRevenueTitle = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            this.dtpRevenue_StartDate = new Guna.UI2.WinForms.Guna2DateTimePicker();
            this.guna2Panel2 = new Guna.UI2.WinForms.Guna2Panel();
            this.lblRevenue_EndDate = new System.Windows.Forms.Label();
            this.dtpRevenue_EndDate = new Guna.UI2.WinForms.Guna2DateTimePicker();
            this.lblRevenue_StartDate = new System.Windows.Forms.Label();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.chartRevenue = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.dgvRevenue = new Guna.UI2.WinForms.Guna2DataGridView();
            this.colCategory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRevenue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPercentage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.guna2Panel1.SuspendLayout();
            this.guna2Panel2.SuspendLayout();
            this.pnlContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartRevenue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRevenue)).BeginInit();
            this.SuspendLayout();
            // 
            // lblRevenueTitle
            // 
            this.lblRevenueTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblRevenueTitle.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRevenueTitle.Location = new System.Drawing.Point(20, 15);
            this.lblRevenueTitle.Name = "lblRevenueTitle";
            this.lblRevenueTitle.Size = new System.Drawing.Size(356, 56);
            this.lblRevenueTitle.TabIndex = 0;
            this.lblRevenueTitle.Text = "Báo cáo doanh thu";
            // 
            // guna2Panel1
            // 
            this.guna2Panel1.Controls.Add(this.lblRevenueTitle);
            this.guna2Panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.guna2Panel1.Location = new System.Drawing.Point(0, 85);
            this.guna2Panel1.Name = "guna2Panel1";
            this.guna2Panel1.Size = new System.Drawing.Size(1082, 90);
            this.guna2Panel1.TabIndex = 10;
            // 
            // dtpRevenue_StartDate
            // 
            this.dtpRevenue_StartDate.BorderRadius = 8;
            this.dtpRevenue_StartDate.Checked = true;
            this.dtpRevenue_StartDate.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(6)))), ((int)(((byte)(23)))));
            this.dtpRevenue_StartDate.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtpRevenue_StartDate.ForeColor = System.Drawing.Color.White;
            this.dtpRevenue_StartDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpRevenue_StartDate.Location = new System.Drawing.Point(140, 23);
            this.dtpRevenue_StartDate.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtpRevenue_StartDate.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtpRevenue_StartDate.Name = "dtpRevenue_StartDate";
            this.dtpRevenue_StartDate.Size = new System.Drawing.Size(260, 50);
            this.dtpRevenue_StartDate.TabIndex = 8;
            this.dtpRevenue_StartDate.Value = new System.DateTime(2026, 1, 6, 21, 5, 24, 551);
            // 
            // guna2Panel2
            // 
            this.guna2Panel2.Controls.Add(this.lblRevenue_EndDate);
            this.guna2Panel2.Controls.Add(this.dtpRevenue_EndDate);
            this.guna2Panel2.Controls.Add(this.lblRevenue_StartDate);
            this.guna2Panel2.Controls.Add(this.dtpRevenue_StartDate);
            this.guna2Panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.guna2Panel2.Location = new System.Drawing.Point(0, 0);
            this.guna2Panel2.Name = "guna2Panel2";
            this.guna2Panel2.Padding = new System.Windows.Forms.Padding(20, 10, 20, 10);
            this.guna2Panel2.Size = new System.Drawing.Size(1082, 85);
            this.guna2Panel2.TabIndex = 11;
            // 
            // lblRevenue_EndDate
            // 
            this.lblRevenue_EndDate.AutoSize = true;
            this.lblRevenue_EndDate.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRevenue_EndDate.Location = new System.Drawing.Point(550, 30);
            this.lblRevenue_EndDate.Name = "lblRevenue_EndDate";
            this.lblRevenue_EndDate.Size = new System.Drawing.Size(117, 23);
            this.lblRevenue_EndDate.TabIndex = 11;
            this.lblRevenue_EndDate.Text = "Ngày kết thúc";
            // 
            // dtpRevenue_EndDate
            // 
            this.dtpRevenue_EndDate.BorderRadius = 8;
            this.dtpRevenue_EndDate.Checked = true;
            this.dtpRevenue_EndDate.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(6)))), ((int)(((byte)(23)))));
            this.dtpRevenue_EndDate.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtpRevenue_EndDate.ForeColor = System.Drawing.Color.White;
            this.dtpRevenue_EndDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpRevenue_EndDate.Location = new System.Drawing.Point(685, 23);
            this.dtpRevenue_EndDate.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtpRevenue_EndDate.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtpRevenue_EndDate.Name = "dtpRevenue_EndDate";
            this.dtpRevenue_EndDate.Size = new System.Drawing.Size(260, 50);
            this.dtpRevenue_EndDate.TabIndex = 10;
            this.dtpRevenue_EndDate.Value = new System.DateTime(2026, 1, 6, 21, 5, 24, 551);
            // 
            // lblRevenue_StartDate
            // 
            this.lblRevenue_StartDate.AutoSize = true;
            this.lblRevenue_StartDate.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRevenue_StartDate.Location = new System.Drawing.Point(20, 30);
            this.lblRevenue_StartDate.Name = "lblRevenue_StartDate";
            this.lblRevenue_StartDate.Size = new System.Drawing.Size(114, 23);
            this.lblRevenue_StartDate.TabIndex = 9;
            this.lblRevenue_StartDate.Text = "Ngày bắt đầu";
            // 
            // pnlContent
            // 
            this.pnlContent.Controls.Add(this.chartRevenue);
            this.pnlContent.Controls.Add(this.dgvRevenue);
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(0, 175);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Padding = new System.Windows.Forms.Padding(40, 10, 40, 20);
            this.pnlContent.Size = new System.Drawing.Size(1082, 550);
            this.pnlContent.TabIndex = 14;
            // 
            // chartRevenue
            // 
            this.chartRevenue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea2.Name = "ChartArea1";
            this.chartRevenue.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.chartRevenue.Legends.Add(legend2);
            this.chartRevenue.Location = new System.Drawing.Point(40, 240);
            this.chartRevenue.Name = "chartRevenue";
            series3.ChartArea = "ChartArea1";
            series3.Legend = "Legend1";
            series3.Name = "Khách sạn";
            series4.ChartArea = "ChartArea1";
            series4.Legend = "Legend1";
            series4.Name = "Nhà hàng";
            this.chartRevenue.Series.Add(series3);
            this.chartRevenue.Series.Add(series4);
            this.chartRevenue.Size = new System.Drawing.Size(1000, 280);
            this.chartRevenue.TabIndex = 13;
            this.chartRevenue.Text = "chart1";
            // 
            // dgvRevenue
            // 
            this.dgvRevenue.AllowUserToAddRows = false;
            this.dgvRevenue.AllowUserToDeleteRows = false;
            this.dgvRevenue.AllowUserToResizeColumns = false;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.White;
            this.dgvRevenue.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvRevenue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(6)))), ((int)(((byte)(23)))));
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
            dataGridViewCellStyle7.Padding = new System.Windows.Forms.Padding(10);
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvRevenue.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dgvRevenue.ColumnHeadersHeight = 45;
            this.dgvRevenue.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dgvRevenue.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCategory,
            this.colRevenue,
            this.colPercentage});
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(6)))), ((int)(((byte)(23)))));
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvRevenue.DefaultCellStyle = dataGridViewCellStyle10;
            this.dgvRevenue.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.dgvRevenue.Location = new System.Drawing.Point(40, 20);
            this.dgvRevenue.Name = "dgvRevenue";
            this.dgvRevenue.ReadOnly = true;
            this.dgvRevenue.RowHeadersVisible = false;
            this.dgvRevenue.RowHeadersWidth = 51;
            this.dgvRevenue.RowTemplate.Height = 32;
            this.dgvRevenue.Size = new System.Drawing.Size(1000, 210);
            this.dgvRevenue.TabIndex = 0;
            this.dgvRevenue.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvRevenue.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dgvRevenue.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dgvRevenue.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dgvRevenue.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dgvRevenue.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dgvRevenue.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.dgvRevenue.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            this.dgvRevenue.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvRevenue.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvRevenue.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgvRevenue.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dgvRevenue.ThemeStyle.HeaderStyle.Height = 45;
            this.dgvRevenue.ThemeStyle.ReadOnly = true;
            this.dgvRevenue.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvRevenue.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvRevenue.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvRevenue.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgvRevenue.ThemeStyle.RowsStyle.Height = 32;
            this.dgvRevenue.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvRevenue.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgvRevenue.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvRevenue_CellContentClick);
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
            // 
            // colRevenue
            // 
            this.colRevenue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colRevenue.DataPropertyName = "Revenue";
            dataGridViewCellStyle8.Format = "0.00 VND";
            this.colRevenue.DefaultCellStyle = dataGridViewCellStyle8;
            this.colRevenue.FillWeight = 35F;
            this.colRevenue.HeaderText = "Doanh thu";
            this.colRevenue.MinimumWidth = 6;
            this.colRevenue.Name = "colRevenue";
            this.colRevenue.ReadOnly = true;
            // 
            // colPercentage
            // 
            this.colPercentage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colPercentage.DataPropertyName = "Percentage";
            dataGridViewCellStyle9.Format = "0.##\'%\'";
            this.colPercentage.DefaultCellStyle = dataGridViewCellStyle9;
            this.colPercentage.FillWeight = 30F;
            this.colPercentage.HeaderText = "Phần trăm";
            this.colPercentage.MinimumWidth = 6;
            this.colPercentage.Name = "colPercentage";
            this.colPercentage.ReadOnly = true;
            // 
            // FormRevenue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1082, 725);
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.guna2Panel1);
            this.Controls.Add(this.guna2Panel2);
            this.Name = "FormRevenue";
            this.Text = "FormRevenue";
            this.guna2Panel1.ResumeLayout(false);
            this.guna2Panel1.PerformLayout();
            this.guna2Panel2.ResumeLayout(false);
            this.guna2Panel2.PerformLayout();
            this.pnlContent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartRevenue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRevenue)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2HtmlLabel lblRevenueTitle;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private Guna.UI2.WinForms.Guna2DateTimePicker dtpRevenue_StartDate;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel2;
        private System.Windows.Forms.Label lblRevenue_EndDate;
        private Guna.UI2.WinForms.Guna2DateTimePicker dtpRevenue_EndDate;
        private System.Windows.Forms.Label lblRevenue_StartDate;
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartRevenue;
        private Guna.UI2.WinForms.Guna2DataGridView dgvRevenue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCategory;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRevenue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPercentage;
    }
}