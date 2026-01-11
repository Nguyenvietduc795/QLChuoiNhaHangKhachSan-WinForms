using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace QLChuoiNhaHangKhachSan.GUI
{
    public class FormVoucherDetails : Form
    {
        private readonly DataGridView _dgvDetails;
        private readonly Label _lblHeader;

        public FormVoucherDetails(string voucherCode, DataTable details)
        {
            Text = "Chi ti?t phi?u" + (string.IsNullOrWhiteSpace(voucherCode) ? string.Empty : $" - {voucherCode}");
            StartPosition = FormStartPosition.CenterParent;
            MinimizeBox = false;
            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Size = new Size(800, 500);

            _lblHeader = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0),
                Text = string.IsNullOrWhiteSpace(voucherCode) ? "Chi ti?t phi?u" : $"Chi ti?t phi?u {voucherCode}",
                Location = new Point(16, 16)
            };

            _dgvDetails = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AutoGenerateColumns = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                DataSource = details,
                Margin = new Padding(16)
            };

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                ColumnCount = 1,
                Padding = new Padding(16)
            };

            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            layout.Controls.Add(_lblHeader, 0, 0);
            layout.Controls.Add(_dgvDetails, 0, 1);

            Controls.Add(layout);
        }
    }
}
