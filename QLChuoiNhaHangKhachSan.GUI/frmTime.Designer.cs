namespace QLChuoiNhaHangKhachSan.GUI
{
    partial class frmTime
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
            this._lblTime = new System.Windows.Forms.Label();
            this._pnlHours = new System.Windows.Forms.Panel();
            this._pnlMinutes = new System.Windows.Forms.Panel();
            this._btnOk = new System.Windows.Forms.Button();
            this._btnPm = new System.Windows.Forms.Button();
            this._btnAm = new System.Windows.Forms.Button();
            this.bottomPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.bottomPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // _lblTime
            // 
            this._lblTime.Dock = System.Windows.Forms.DockStyle.Top;
            this._lblTime.Font = new System.Drawing.Font("Segoe UI", 28F, System.Drawing.FontStyle.Bold);
            this._lblTime.Location = new System.Drawing.Point(0, 0);
            this._lblTime.Name = "_lblTime";
            this._lblTime.Size = new System.Drawing.Size(472, 80);
            this._lblTime.TabIndex = 3;
            this._lblTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // _pnlHours
            // 
            this._pnlHours.Dock = System.Windows.Forms.DockStyle.Top;
            this._pnlHours.Location = new System.Drawing.Point(0, 80);
            this._pnlHours.Name = "_pnlHours";
            this._pnlHours.Padding = new System.Windows.Forms.Padding(10);
            this._pnlHours.Size = new System.Drawing.Size(472, 220);
            this._pnlHours.TabIndex = 2;
            // 
            // _pnlMinutes
            // 
            this._pnlMinutes.Dock = System.Windows.Forms.DockStyle.Top;
            this._pnlMinutes.Location = new System.Drawing.Point(0, 300);
            this._pnlMinutes.Name = "_pnlMinutes";
            this._pnlMinutes.Padding = new System.Windows.Forms.Padding(10);
            this._pnlMinutes.Size = new System.Drawing.Size(472, 220);
            this._pnlMinutes.TabIndex = 1;
            this._pnlMinutes.Visible = false;
            // 
            // _btnOk
            // 
            this._btnOk.AutoSize = true;
            this._btnOk.Location = new System.Drawing.Point(374, 13);
            this._btnOk.Name = "_btnOk";
            this._btnOk.Padding = new System.Windows.Forms.Padding(12, 6, 12, 6);
            this._btnOk.Size = new System.Drawing.Size(75, 35);
            this._btnOk.TabIndex = 0;
            this._btnOk.Text = "OK";
            // 
            // _btnPm
            // 
            this._btnPm.AutoSize = true;
            this._btnPm.Location = new System.Drawing.Point(293, 13);
            this._btnPm.Name = "_btnPm";
            this._btnPm.Padding = new System.Windows.Forms.Padding(10, 6, 10, 6);
            this._btnPm.Size = new System.Drawing.Size(75, 35);
            this._btnPm.TabIndex = 1;
            this._btnPm.Text = "PM";
            // 
            // _btnAm
            // 
            this._btnAm.AutoSize = true;
            this._btnAm.Location = new System.Drawing.Point(212, 13);
            this._btnAm.Name = "_btnAm";
            this._btnAm.Padding = new System.Windows.Forms.Padding(10, 6, 10, 6);
            this._btnAm.Size = new System.Drawing.Size(75, 35);
            this._btnAm.TabIndex = 2;
            this._btnAm.Text = "AM";
            // 
            // bottomPanel
            // 
            this.bottomPanel.Controls.Add(this._btnOk);
            this.bottomPanel.Controls.Add(this._btnPm);
            this.bottomPanel.Controls.Add(this._btnAm);
            this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.bottomPanel.Location = new System.Drawing.Point(0, 360);
            this.bottomPanel.Name = "bottomPanel";
            this.bottomPanel.Padding = new System.Windows.Forms.Padding(10);
            this.bottomPanel.Size = new System.Drawing.Size(472, 60);
            this.bottomPanel.TabIndex = 0;
            // 
            // frmTime
            // 
            this.ClientSize = new System.Drawing.Size(472, 420);
            this.Controls.Add(this.bottomPanel);
            this.Controls.Add(this._pnlMinutes);
            this.Controls.Add(this._pnlHours);
            this.Controls.Add(this._lblTime);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmTime";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Chọn giờ";
            this.bottomPanel.ResumeLayout(false);
            this.bottomPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label _lblTime;
        private System.Windows.Forms.Panel _pnlHours;
        private System.Windows.Forms.Panel _pnlMinutes;
        private System.Windows.Forms.Button _btnOk;
        private System.Windows.Forms.Button _btnPm;
        private System.Windows.Forms.Button _btnAm;
        private System.Windows.Forms.FlowLayoutPanel bottomPanel;
    }
}
