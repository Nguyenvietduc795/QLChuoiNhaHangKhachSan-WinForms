using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Globalization;
using System.IO;
using System.Drawing.Printing;
using Guna.UI2.WinForms;

namespace QLChuoiNhaHangKhachSan.GUI
{
    public partial class FormInvoiceManagement : Form
    {
        // Printing helpers
        private PrintDocument _printDocument;
        private string _printContent;

        public FormInvoiceManagement()
        {
            InitializeComponent();
            this.Load += FormInvoiceManagement_Load;

            // Init print document
            _printDocument = new PrintDocument();
            _printDocument.PrintPage += PrintDocument_PrintPage;
        }

        private void FormInvoiceManagement_Load(object sender, EventArgs e)
        {
            // Normalize grid layout and behavior
            ConfigureDataGridViewLayout();

            // Ensure hidden column to store payment method (so chart can aggregate)
            EnsurePaymentMethodColumn();

            // Wire action buttons in designer (names from Designer.cs)
            if (this.btnPrintInvoice != null)
            {
                this.btnPrintInvoice.Click -= BtnPrintInvoice_Click;
                this.btnPrintInvoice.Click += BtnPrintInvoice_Click;
            }
            if (this.btnExportExcel != null)
            {
                this.btnExportExcel.Click -= BtnExportExcel_Click;
                this.btnExportExcel.Click += BtnExportExcel_Click;
            }
            if (this.btnDeleteInvoice != null)
            {
                this.btnDeleteInvoice.Click -= BtnDeleteInvoice_Click;
                this.btnDeleteInvoice.Click += BtnDeleteInvoice_Click;
            }
            // Wire edit (was previously a separate xem/sửa button) - use details panel button 'btnFix'
            if (this.btnFix != null)
            {
                this.btnFix.Click -= BtnFix_Click;
                this.btnFix.Click += BtnFix_Click;
            }

            // Wire new remove (clear) button placed near the search box
            if (this.btnRemove != null)
            {
                this.btnRemove.Click -= BtnRemove_Click;
                this.btnRemove.Click += BtnRemove_Click;
                // initial visibility depends on current text
                this.btnRemove.Visible = !string.IsNullOrWhiteSpace(this.pnlPayment?.Text);
            }

            // Wire status filter buttons (Design area)
            WireStatusFilterButtons();

            // Clear existing rows (designer created columns are used)
            dgvTransaction.Rows.Clear();

            // Sample data — replace with DB/API later
            AddTransaction("INV-001", "Nguyễn Văn A", new DateTime(2025, 12, 19), 1250000m, "Hoàn thành", "Tiền Mặt");
            AddTransaction("INV-002", "Trần Thị B", new DateTime(2025, 12, 20), 450000m, "Đang chờ", "Thẻ");
            AddTransaction("INV-003", "Lê Văn C", new DateTime(2025, 12, 21), 230000m, "Hoàn thành", "MoMo");
            AddTransaction("INV-004", "Phạm Thị D", new DateTime(2025, 12, 16), 780000m, "Không thành công", "Tiền Mặt");
            AddTransaction("INV-005", "Hoàng Văn E", new DateTime(2025, 12, 10), 950000m, "Đang chờ", "Thẻ");

            AddTransaction("INV-006", "Lữ Nhựt Linh", new DateTime(2025, 12, 1), 950000m, "Đang chờ", "MoMo");
            AddTransaction("INV-007", "Hứa Mỹ Lam", new DateTime(2025, 12, 9), 950000m, "Hoàn thành", "Tiền Mặt");
            AddTransaction("INV-008", "Nguyễn Thị Hồng Gấm", new DateTime(2025, 12, 8), 950000m, "Không thành công", "Thẻ");
            AddTransaction("INV-009", "Nguyễn Trường Phi", new DateTime(2025, 12, 7), 950000m, "Hoàn thành", "MoMo");
            AddTransaction("INV-010", "Lâm Trí Tùa", new DateTime(2025, 12, 29), 950000m, "Hoàn thành", "Tiền Mặt");

            // No row selected by default
            dgvTransaction.ClearSelection();
            dgvTransaction.CurrentCell = null;

            // Update chart from grid values (aggregates by payment method)
            UpdateChartFromGrid();

            // Wire existing designer search control (pnlPayment is a Guna2TextBox in designer)
            if (this.pnlPayment != null)
            {
                // remove previous handler to avoid double-wiring in designer-run/debug cycles
                this.pnlPayment.TextChanged -= PnlPayment_TextChanged;
                this.pnlPayment.TextChanged += PnlPayment_TextChanged;
            }

            // Apply default filter state (All)
            ApplyStatusFilter(row => true);
            SetActiveFilterButton(gnbtnALL);
        }

        private void EnsurePaymentMethodColumn()
        {
            // Add a hidden column to store payment method if it's not present.
            if (dgvTransaction.Columns["PaymentMethod"] == null)
            {
                var col = new DataGridViewTextBoxColumn
                {
                    Name = "PaymentMethod",
                    HeaderText = "Method",
                    Visible = false
                };
                dgvTransaction.Columns.Add(col);
            }
        }

        private void ConfigureDataGridViewLayout()
        {
            // Prevent variable row heights and resizing by user
            dgvTransaction.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dgvTransaction.RowTemplate.Height = 36;
            dgvTransaction.AllowUserToResizeRows = false;

            // Header sizing and style -> keep Navy header bar
            dgvTransaction.ColumnHeadersHeight = 48;
            dgvTransaction.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvTransaction.EnableHeadersVisualStyles = false;
            dgvTransaction.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy; // <- Navy header
            dgvTransaction.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvTransaction.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvTransaction.ColumnHeadersDefaultCellStyle.Padding = new Padding(12, 8, 12, 8);
            dgvTransaction.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);

            // Cell padding and font consistency
            dgvTransaction.DefaultCellStyle.Padding = new Padding(10, 6, 10, 6);
            dgvTransaction.DefaultCellStyle.Font = new Font("Segoe UI", 11F);

            // Show a subtle light gray when a row is selected so user can see which row is active
            dgvTransaction.DefaultCellStyle.SelectionBackColor = Color.LightGray;
            dgvTransaction.DefaultCellStyle.SelectionForeColor = Color.Black;

            // Column width proportions
            dgvTransaction.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            if (dgvTransaction.Columns["dgvInvoiceID"] != null) dgvTransaction.Columns["dgvInvoiceID"].FillWeight = 12;
            if (dgvTransaction.Columns["dgvCustomer"] != null) dgvTransaction.Columns["dgvCustomer"].FillWeight = 32;
            if (dgvTransaction.Columns["dvgDate"] != null) dgvTransaction.Columns["dvgDate"].FillWeight = 18;
            if (dgvTransaction.Columns["dgvAmount"] != null) dgvTransaction.Columns["dgvAmount"].FillWeight = 20;
            if (dgvTransaction.Columns["dgvStatus"] != null) dgvTransaction.Columns["dgvStatus"].FillWeight = 18;

            // Specific alignment
            if (dgvTransaction.Columns["dgvInvoiceID"] != null)
            {
                dgvTransaction.Columns["dgvInvoiceID"].DefaultCellStyle.Padding = new Padding(12, 0, 0, 0);
                dgvTransaction.Columns["dgvInvoiceID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            }
            if (dgvTransaction.Columns["dgvAmount"] != null)
            {
                dgvTransaction.Columns["dgvAmount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            if (dgvTransaction.Columns["dgvStatus"] != null)
            {
                dgvTransaction.Columns["dgvStatus"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            // Use CellFormatting to color status cells so styling survives sorting/redraws
            dgvTransaction.CellFormatting -= DgvTransaction_CellFormatting;
            dgvTransaction.CellFormatting += DgvTransaction_CellFormatting;

            // Ensure clicked row selection doesn't change status cell appearance logic
            dgvTransaction.CellClick -= DgvTransaction_CellClick;
            dgvTransaction.CellClick += DgvTransaction_CellClick;

            // Minor look/feel
            dgvTransaction.RowHeadersVisible = false;
            dgvTransaction.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTransaction.MultiSelect = false;
            dgvTransaction.ReadOnly = true;
        }

        private void AddTransaction(string invoiceId, string customer, DateTime date, decimal amount, string status, string paymentMethod)
        {
            // Format the date and amount to match UI
            var dateText = date.ToString("dd/MM/yyyy");
            var amountText = amount.ToString("N0", CultureInfo.GetCultureInfo("vi-VN")) + " ₫";

            // Add row: existing designer columns + hidden PaymentMethod column (ensure it exists)
            // Rows.Add will match current column count, so ensure PaymentMethod column was added earlier.
            dgvTransaction.Rows.Add(invoiceId, customer, dateText, amountText, status, paymentMethod);
        }

        // Keep selection behavior predictable and do not override colors
        private void DgvTransaction_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return; // header
            if (e.RowIndex >= dgvTransaction.Rows.Count) return;

            dgvTransaction.ClearSelection();
            var row = dgvTransaction.Rows[e.RowIndex];
            row.Selected = true;

            // Make the first visible cell the current cell (prevents focus jump)
            for (int i = 0; i < row.Cells.Count; i++)
            {
                if (row.Cells[i].Visible)
                {
                    dgvTransaction.CurrentCell = row.Cells[i];
                    break;
                }
            }
        }

        // Color mapping for exactly three statuses; do NOT override the global selection color
        private void DgvTransaction_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvTransaction.Columns == null || e.Value == null) return;
            var col = dgvTransaction.Columns[e.ColumnIndex];
            if (col == null) return;

            if (col.Name == "dgvStatus")
            {
                var statusText = e.Value.ToString().Trim();

                // Normalize only three statuses: "Đang chờ", "Hoàn thành", "Không thành công"
                if (string.Equals(statusText, "Đang chờ", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(statusText, "Dang cho", StringComparison.OrdinalIgnoreCase))
                {
                    e.CellStyle.BackColor = Color.Gold;
                    e.CellStyle.ForeColor = Color.Black;
                }
                else if (string.Equals(statusText, "Hoàn thành", StringComparison.OrdinalIgnoreCase) ||
                         string.Equals(statusText, "Hoan thanh", StringComparison.OrdinalIgnoreCase))
                {
                    e.CellStyle.BackColor = Color.FromArgb(233, 255, 248);
                    e.CellStyle.ForeColor = Color.FromArgb(0, 122, 78);
                }
                else // anything else treated as "Không thành công"
                {
                    e.CellStyle.BackColor = Color.LightCoral;
                    e.CellStyle.ForeColor = Color.White;
                }

                e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                e.CellStyle.Font = new Font(dgvTransaction.DefaultCellStyle.Font, FontStyle.Bold);

                // DO NOT change SelectionBackColor so the row highlight remains light gray
            }

            // Keep amount right-aligned
            if (col.Name == "dgvAmount")
            {
                e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
        }

        /// <summary>
        /// Aggregate totals from dgvTransaction.PaymentMethod and dgvAmount then populate chart1PaymentMethods.
        /// If the chart control is not present in the designer this method is a safe no-op.
        /// </summary>
        private void UpdateChartFromGrid()
        {
            // The designer does not include a chart control by default.
            // Implement aggregation here when you add a chart (e.g., chart1PaymentMethods).
            if (dgvTransaction == null || dgvTransaction.Rows.Count == 0) return;

            // Simple aggregation example (keeps method local and avoids referencing missing chart):
            var totals = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase);
            foreach (DataGridViewRow row in dgvTransaction.Rows)
            {
                if (row.IsNewRow) continue;
                string method = null;
                decimal amount = 0m;

                // PaymentMethod is stored in the hidden column we added (if present)
                if (dgvTransaction.Columns["PaymentMethod"] != null)
                {
                    var val = row.Cells["PaymentMethod"].Value;
                    method = val != null ? val.ToString() : "Unknown";
                }
                else
                {
                    method = "Unknown";
                }

                // Parse amount column (formatted like "1.250.000 ₫")
                try
                {
                    var amtCell = row.Cells["dgvAmount"].Value?.ToString();
                    if (!string.IsNullOrWhiteSpace(amtCell))
                    {
                        amount = ParseFormattedAmount(amtCell);
                    }
                }
                catch
                {
                    amount = 0m;
                }

                if (!totals.ContainsKey(method)) totals[method] = 0m;
                totals[method] += amount;
            }

            // At this point totals contains aggregated values by payment method.
            // If you add a Chart control to the designer, populate it here.
        }

        private string FormatCurrencyLabel(decimal amount)
        {
            return amount.ToString("N0", CultureInfo.GetCultureInfo("vi-VN")) + " đ";
        }

        /// <summary>
        /// Parse formatted currency string using Vietnamese culture first, fall back to invariant.
        /// Accepts strings like "1.250.000 ₫", "1,250,000 ₫", "1250000".
        /// </summary>
        private decimal ParseFormattedAmount(string formatted)
        {
            if (string.IsNullOrWhiteSpace(formatted)) return 0m;
            // Remove currency symbols and whitespace
            var s = formatted.Replace("₫", "").Replace("đ", "").Trim();

            // Try parse using vi-VN (thousands: '.', decimal: ',')
            if (decimal.TryParse(s, NumberStyles.Number, CultureInfo.GetCultureInfo("vi-VN"), out var v))
                return v;

            // Fallback: remove common separators and parse invariant
            var cleaned = new string(s.Where(c => char.IsDigit(c) || c == '-' || c == '.' || c == ',').ToArray());
            cleaned = cleaned.Replace(".", "").Replace(",", "");
            if (decimal.TryParse(cleaned, NumberStyles.Number, CultureInfo.InvariantCulture, out v))
                return v;

            return 0m;
        }

        private void guna2Panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lblLogo_Click(object sender, EventArgs e)
        {

        }

        private void pnlSidebar_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2Separator1_Click(object sender, EventArgs e)
        {

        }

        private void btnLogout_Click(object sender, EventArgs e)
        {

        }

        private void btnReports_Click(object sender, EventArgs e)
        {

        }

        private void btnPayments_Click(object sender, EventArgs e)
        {

        }

        private void btnInventory_Click(object sender, EventArgs e)
        {

        }

        private void btnHotels_Click(object sender, EventArgs e)
        {

        }

        private void btnRestaurants_Click(object sender, EventArgs e)
        {

        }

        private void btnCustomers_Click(object sender, EventArgs e)
        {

        }

        private void btnEmplyees_Click(object sender, EventArgs e)
        {

        }

        private void btnHome_Click(object sender, EventArgs e)
        {

        }

        private void lblSubLogo_Click(object sender, EventArgs e)
        {

        }

        private void Gn2pnlPaymentMain_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pnlPaymentHeader_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnCreateInvoice_Click(object sender, EventArgs e)
        {

        }

        private void lblPaymentTitle_Click(object sender, EventArgs e)
        {

        }

        private void lblRevenueTitle_Click(object sender, EventArgs e)
        {

        }

        private void iconRevenue_Click(object sender, EventArgs e)
        {

        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void iconTotalRevenue_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView4_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView5_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void guna2Button10_Click(object sender, EventArgs e)
        {

        }

        private void pnlPaymentContent_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pnlSummary_Paint(object sender, PaintEventArgs e)
        {

        }

        /// <summary>
        /// Handle TextChanged from the designer search control (pnlPayment).
        /// Filters by invoice id or customer name.
        /// </summary>
        private void PnlPayment_TextChanged(object sender, EventArgs e)
        {
            if (this.pnlPayment == null) return;
            var q = (this.pnlPayment.Text ?? "").Trim();

            // If user cleared the box, show all rows.
            FilterTransactions(q);

            // Re-apply the currently selected status filter to keep both filters combined
            // Find active button
            if (IsButtonActive(gnbtnALL)) ApplyStatusFilter(row => true);
            else if (IsButtonActive(gnbtnDont)) ApplyStatusFilter(IsUnpaidRow);
            else if (IsButtonActive(gnbtnDone)) ApplyStatusFilter(IsPaidRow);
            else if (IsButtonActive(gnbtnOverdue)) ApplyStatusFilter(IsOverdueRow);

            // Toggle visibility of the clear ('x') button placed next to the search box
            if (this.btnRemove != null)
            {
                this.btnRemove.Visible = !string.IsNullOrWhiteSpace(this.pnlPayment.Text);
            }
        }

        /// <summary>
        /// Clear search text when user clicks the small 'x' button beside the search box.
        /// The designer button is named 'btnRemove'.
        /// </summary>
        private void BtnRemove_Click(object sender, EventArgs e)
        {
            if (this.pnlPayment == null) return;

            // Clearing text triggers PnlPayment_TextChanged which re-applies filters and hides the button.
            this.pnlPayment.Text = string.Empty;

            // Return focus to the search box so user can immediately type again.
            try
            {
                this.pnlPayment.Focus();
            }
            catch
            {
                // ignore focus errors in designer/runtime mismatch
            }
        }

        /// <summary>
        /// Filter grid rows by invoice id or customer name (case-insensitive).
        /// Pass an empty/whitespace string to clear filter.
        /// </summary>
        private void FilterTransactions(string query)
        {
            if (dgvTransaction == null) return;

            var q = (query ?? "").Trim();
            if (string.IsNullOrEmpty(q))
            {
                // show all rows (we keep NewRow visible)
                foreach (DataGridViewRow r in dgvTransaction.Rows)
                {
                    if (!r.IsNewRow) r.Visible = true;
                }
                return;
            }

            var qLower = q.ToLowerInvariant();

            // Column names used in this form
            var colId = dgvTransaction.Columns["dgvInvoiceID"];
            var colCustomer = dgvTransaction.Columns["dgvCustomer"];

            foreach (DataGridViewRow r in dgvTransaction.Rows)
            {
                if (r.IsNewRow)
                {
                    r.Visible = true;
                    continue;
                }

                bool match = false;
                try
                {
                    if (colId != null)
                    {
                        var v = r.Cells[colId.Index].Value?.ToString() ?? "";
                        if (v.ToLowerInvariant().Contains(qLower)) match = true;
                    }
                    if (!match && colCustomer != null)
                    {
                        var v = r.Cells[colCustomer.Index].Value?.ToString() ?? "";
                        if (v.ToLowerInvariant().Contains(qLower)) match = true;
                    }
                }
                catch
                {
                    match = false;
                }

                r.Visible = match;
            }
        }

        // -------------------- New: Print / Export / Delete handlers --------------------

        private DataGridViewRow GetSelectedRow()
        {
            if (dgvTransaction == null) return null;
            if (dgvTransaction.CurrentRow != null && !dgvTransaction.CurrentRow.IsNewRow)
                return dgvTransaction.CurrentRow;
            if (dgvTransaction.SelectedRows != null && dgvTransaction.SelectedRows.Count > 0)
                return dgvTransaction.SelectedRows[0];
            return null;
        }

        private void BtnPrintInvoice_Click(object sender, EventArgs e)
        {
            var row = GetSelectedRow();
            if (row == null)
            {
                MessageBox.Show("Vui lòng chọn một hóa đơn để in.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Compose printable content with larger layout to better fit the preview window/page
            var sb = new StringBuilder();
            sb.AppendLine("CHI TIẾT HÓA ĐƠN");
            sb.AppendLine("----------------------------");
            sb.AppendLine("Mã Hóa Đơn: " + (row.Cells["dgvInvoiceID"]?.Value?.ToString() ?? ""));
            sb.AppendLine("Khách Hàng: " + (row.Cells["dgvCustomer"]?.Value?.ToString() ?? ""));
            sb.AppendLine("Ngày: " + (row.Cells["dvgDate"]?.Value?.ToString() ?? ""));
            sb.AppendLine("Số Tiền: " + (row.Cells["dgvAmount"]?.Value?.ToString() ?? ""));
            sb.AppendLine("Trạng Thái: " + (row.Cells["dgvStatus"]?.Value?.ToString() ?? ""));
            if (dgvTransaction.Columns["PaymentMethod"] != null)
                sb.AppendLine("Phương Thức: " + (row.Cells["PaymentMethod"]?.Value?.ToString() ?? ""));

            _printContent = sb.ToString();

            // Print preview
            using (var preview = new PrintPreviewDialog())
            {
                preview.Document = _printDocument;
                preview.Width = 1500;
                preview.Height = 1200;
                try
                {
                    preview.ShowDialog(this);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi mở xem trước in: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            var margin = e.MarginBounds;

            // Larger fonts to better fill the printable area
            var headerFont = new Font("Segoe UI", 32, FontStyle.Bold);
            var bodyFont = new Font("Segoe UI", 24);
            var brush = Brushes.Black;

            float y = margin.Top;
            float lineHeight = bodyFont.GetHeight(e.Graphics) + 6;

            // Draw centered header
            var header = "CHI TIẾT HÓA ĐƠN";
            var headerSize = e.Graphics.MeasureString(header, headerFont);
            e.Graphics.DrawString(header, headerFont, brush, margin.Left + (margin.Width - headerSize.Width) / 2, y);
            y += headerSize.Height + 8;

            // Separator
            e.Graphics.DrawLine(Pens.Black, margin.Left, y, margin.Right, y);
            y += 8;

            var lines = _printContent?.Split(new[] { Environment.NewLine }, StringSplitOptions.None) ?? new string[0];

            foreach (var line in lines)
            {
                // Wrap long lines if needed (basic wrap)
                var remaining = line;
                while (!string.IsNullOrEmpty(remaining))
                {
                    // Determine maximum characters that fit
                    int fitChars, linesFilled;
                    e.Graphics.MeasureString(remaining, bodyFont, new SizeF(margin.Width, lineHeight), StringFormat.GenericTypographic, out fitChars, out linesFilled);

                    string toDraw = remaining;
                    if (fitChars < remaining.Length)
                    {
                        toDraw = remaining.Substring(0, fitChars);
                        // attempt to trim to last space for better wrapping
                        var lastSpace = toDraw.LastIndexOf(' ');
                        if (lastSpace > 0 && fitChars > 10)
                        {
                            toDraw = remaining.Substring(0, lastSpace);
                            fitChars = lastSpace;
                        }
                    }

                    e.Graphics.DrawString(toDraw, bodyFont, brush, margin.Left, y);
                    y += lineHeight;

                    remaining = remaining.Length > fitChars ? remaining.Substring(fitChars).TrimStart() : "";
                    if (y + lineHeight > margin.Bottom)
                    {
                        e.HasMorePages = false;
                        return;
                    }
                }
            }

            e.HasMorePages = false;
        }

        private void BtnExportExcel_Click(object sender, EventArgs e)
        {
            var row = GetSelectedRow();
            if (row == null)
            {
                MessageBox.Show("Vui lòng chọn một hóa đơn để xuất.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var invoiceId = row.Cells["dgvInvoiceID"]?.Value?.ToString() ?? "invoice";
            using (var sfd = new SaveFileDialog())
            {
                sfd.Title = "Xuất hóa đơn";
                sfd.FileName = invoiceId + ".csv";
                sfd.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
                sfd.DefaultExt = "csv";

                if (sfd.ShowDialog(this) != DialogResult.OK) return;

                try
                {
                    // Write CSV (UTF8 with BOM) so Excel opens Vietnamese characters correctly.
                    using (var sw = new StreamWriter(sfd.FileName, false, new UTF8Encoding(true)))
                    {
                        // header
                        sw.WriteLine("Mã Hóa Đơn,Khách Hàng,Ngày,Số Tiền,Trạng Thái,Phương Thức");
                        var payment = dgvTransaction.Columns["PaymentMethod"] != null ? row.Cells["PaymentMethod"]?.Value?.ToString() : "";
                        var customer = row.Cells["dgvCustomer"]?.Value?.ToString() ?? "";
                        var date = row.Cells["dvgDate"]?.Value?.ToString() ?? "";
                        var amount = row.Cells["dgvAmount"]?.Value?.ToString() ?? "";
                        var status = row.Cells["dgvStatus"]?.Value?.ToString() ?? "";
                        // escape commas by wrapping values in quotes
                        string Escape(string s) => "\"" + (s ?? "").Replace("\"", "\"\"") + "\"";
                        sw.WriteLine(string.Join(",", new[] {
                            Escape(invoiceId),
                            Escape(customer),
                            Escape(date),
                            Escape(amount),
                            Escape(status),
                            Escape(payment)
                        }));
                    }

                    MessageBox.Show("Đã xuất thành công: " + sfd.FileName, "Hoàn tất", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xuất file: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnDeleteInvoice_Click(object sender, EventArgs e)
        {
            var row = GetSelectedRow();
            if (row == null)
            {
                MessageBox.Show("Vui lòng chọn một hóa đơn để hủy.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var invoiceId = row.Cells["dgvInvoiceID"]?.Value?.ToString() ?? "";
            var confirm = MessageBox.Show($"Bạn có chắc muốn hủy hóa đơn '{invoiceId}' không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;

            try
            {
                dgvTransaction.Rows.Remove(row);
                UpdateChartFromGrid();
                MessageBox.Show("Đã hủy hóa đơn.", "Hoàn tất", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể hủy hóa đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Edit button handler (details panel) - opens modal edit form and applies changes to the selected row
        private void BtnFix_Click(object sender, EventArgs e)
        {
            var row = GetSelectedRow();
            if (row == null)
            {
                MessageBox.Show("Vui lòng chọn một hóa đơn để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var id = row.Cells["dgvInvoiceID"]?.Value?.ToString() ?? "";
            var customer = row.Cells["dgvCustomer"]?.Value?.ToString() ?? "";
            DateTime dateVal = DateTime.Today;
            var dateText = row.Cells["dvgDate"]?.Value?.ToString() ?? "";
            if (!DateTime.TryParseExact(dateText, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateVal))
            {
                DateTime.TryParse(dateText, CultureInfo.GetCultureInfo("vi-VN"), DateTimeStyles.None, out dateVal);
            }
            var amount = ParseFormattedAmount(row.Cells["dgvAmount"]?.Value?.ToString() ?? "0");
            var status = row.Cells["dgvStatus"]?.Value?.ToString() ?? "";
            var payment = dgvTransaction.Columns["PaymentMethod"] != null ? row.Cells["PaymentMethod"]?.Value?.ToString() : "";

            using (var edit = new FormInvoiceEdit(id, customer, dateVal, amount, status, payment))
            {
                var dr = edit.ShowDialog(this);
                if (dr == DialogResult.OK)
                {
                    // Apply changes back to row
                    row.Cells["dgvCustomer"].Value = edit.Customer;
                    row.Cells["dvgDate"].Value = edit.Date.ToString("dd/MM/yyyy");
                    row.Cells["dgvAmount"].Value = edit.Amount.ToString("N0", CultureInfo.GetCultureInfo("vi-VN")) + " ₫";
                    row.Cells["dgvStatus"].Value = edit.Status;
                    if (dgvTransaction.Columns["PaymentMethod"] != null)
                        row.Cells["PaymentMethod"].Value = edit.PaymentMethod;

                    UpdateChartFromGrid();
                    MessageBox.Show("Đã cập nhật hóa đơn.", "Hoàn tất", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        // -------------------------------------------------------------------------
        // -------------------- New: status filter logic for design buttons -------
        // -------------------------------------------------------------------------

        private void WireStatusFilterButtons()
        {
            // Attach handlers safely (remove existing handlers first)
            if (gnbtnALL != null)
            {
                gnbtnALL.Click -= GnbtnALL_Click;
                gnbtnALL.Click += GnbtnALL_Click;
            }
            if (gnbtnDont != null)
            {
                gnbtnDont.Click -= GnbtnDont_Click;
                gnbtnDont.Click += GnbtnDont_Click;
            }
            if (gnbtnDone != null)
            {
                gnbtnDone.Click -= GnbtnDone_Click;
                gnbtnDone.Click += GnbtnDone_Click;
            }
            if (gnbtnOverdue != null)
            {
                gnbtnOverdue.Click -= GnbtnOverdue_Click;
                gnbtnOverdue.Click += GnbtnOverdue_Click;
            }
        }

        private void GnbtnALL_Click(object sender, EventArgs e)
        {
            ApplyStatusFilter(row => true);
            SetActiveFilterButton(gnbtnALL);
        }

        private void GnbtnDont_Click(object sender, EventArgs e)
        {
            ApplyStatusFilter(IsUnpaidRow);
            SetActiveFilterButton(gnbtnDont);
        }

        private void GnbtnDone_Click(object sender, EventArgs e)
        {
            ApplyStatusFilter(IsPaidRow);
            SetActiveFilterButton(gnbtnDone);
        }

        private void GnbtnOverdue_Click(object sender, EventArgs e)
        {
            ApplyStatusFilter(IsOverdueRow);
            SetActiveFilterButton(gnbtnOverdue);
        }

        // Apply a status predicate while preserving the current text-search filter.
        private void ApplyStatusFilter(Func<DataGridViewRow, bool> statusPredicate)
        {
            if (dgvTransaction == null) return;

            // First apply text search so we don't show rows that should be hidden by the search box
            var currentSearch = this.pnlPayment?.Text ?? "";
            FilterTransactions(currentSearch);

            // Now restrict further by status predicate
            foreach (DataGridViewRow row in dgvTransaction.Rows)
            {
                if (row.IsNewRow) continue;

                // If already hidden by text filter, keep it hidden
                if (!row.Visible)
                {
                    row.Visible = false;
                    continue;
                }

                try
                {
                    row.Visible = statusPredicate(row);
                }
                catch
                {
                    row.Visible = false;
                }
            }
        }

        // Status helpers

        private bool IsPaidRow(DataGridViewRow row)
        {
            var status = (row.Cells["dgvStatus"]?.Value?.ToString() ?? "").Trim().ToLowerInvariant();
            // Treat common paid variants as paid
            if (string.IsNullOrEmpty(status)) return false;
            if (status.Contains("hoàn") || status.Contains("hoan") || // "Hoàn thành"
                (status.Contains("đã") || status.Contains("da")) && status.Contains("thu")) // "Đã thu tiền"
                return true;
            return false;
        }

        private bool IsUnpaidRow(DataGridViewRow row)
        {
            var status = (row.Cells["dgvStatus"]?.Value?.ToString() ?? "").Trim().ToLowerInvariant();
            if (string.IsNullOrEmpty(status)) return false;
            // common unpaid variants: "Đang chờ", "Chưa thu tiền"
            if (status.Contains("đang") || status.Contains("dang") || status.Contains("chưa") || status.Contains("chua"))
                return true;
            return false;
        }

        private bool IsOverdueRow(DataGridViewRow row)
        {
            // Overdue = date < today AND not paid
            var dateText = (row.Cells["dvgDate"]?.Value?.ToString() ?? "").Trim();
            if (string.IsNullOrEmpty(dateText)) return false;

            DateTime dt;
            if (!DateTime.TryParseExact(dateText, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
            {
                // try vi-VN parsing fallback
                if (!DateTime.TryParse(dateText, CultureInfo.GetCultureInfo("vi-VN"), DateTimeStyles.None, out dt))
                    return false;
            }

            if (dt.Date >= DateTime.Today) return false;
            // not paid
            return !IsPaidRow(row);
        }

        // Visual state for filter buttons (simple active style)
        private void SetActiveFilterButton(Guna2Button active)
        {
            if (gnbtnALL != null) ResetFilterButtonStyle(gnbtnALL);
            if (gnbtnDont != null) ResetFilterButtonStyle(gnbtnDont);
            if (gnbtnDone != null) ResetFilterButtonStyle(gnbtnDone);
            if (gnbtnOverdue != null) ResetFilterButtonStyle(gnbtnOverdue);

            if (active == null) return;

            // active style: filled background and white text
            active.FillColor = Color.FromArgb(26, 31, 51);
            active.ForeColor = Color.White;
            active.BorderThickness = 0;
            active.Font = new Font(active.Font, FontStyle.Bold);
        }

        private bool IsButtonActive(Guna2Button btn)
        {
            if (btn == null) return false;
            return btn.ForeColor == Color.White && btn.FillColor == Color.FromArgb(26, 31, 51);
        }

        private void ResetFilterButtonStyle(Guna2Button btn)
        {
            if (btn == null) return;
            // revert to designer-like neutral state
            btn.FillColor = Color.White;
            btn.ForeColor = Color.Black;
            btn.BorderThickness = 2;
            // restore border color from names for better UX
            if (btn == gnbtnDont) btn.BorderColor = Color.DarkOrange;
            else if (btn == gnbtnDone) btn.BorderColor = Color.DarkGreen;
            else if (btn == gnbtnOverdue) btn.BorderColor = Color.Maroon;
            else btn.BorderColor = Color.FromArgb(26, 31, 51);
            btn.Font = new Font(btn.Font, FontStyle.Bold);
        }

        private void btnRemove_Click_1(object sender, EventArgs e)
        {

        }

        // -------------------------------------------------------------------------
    }
}

namespace QLChuoiNhaHangKhachSan.GUI
{
    // Simple edit dialog used by FormInvoiceManagement.gnbtnCheck_Click.
    // Created programmatically so it can be copied directly into the project.
    public class FormInvoiceEdit : Form
    {
        private TextBox txtInvoiceId;
        private TextBox txtCustomer;
        private DateTimePicker dtpDate;
        private TextBox txtAmount;
        private ComboBox cmbStatus;
        private ComboBox cmbPaymentMethod;
        private Button btnSave;
        private Button btnCancel;

        public string InvoiceId => txtInvoiceId.Text;
        public string Customer => txtCustomer.Text;
        public DateTime Date => dtpDate.Value.Date;
        public decimal Amount
        {
            get
            {
                var s = txtAmount.Text ?? "";
                s = s.Replace("₫", "").Replace("đ", "").Trim();
                // Try parse with vi-VN first
                if (decimal.TryParse(s, NumberStyles.Number, CultureInfo.GetCultureInfo("vi-VN"), out var v))
                    return v;
                // fallback: remove separators and parse invariant
                var cleaned = new string(s.Where(c => char.IsDigit(c) || c == '-' || c == '.' || c == ',').ToArray());
                cleaned = cleaned.Replace(".", "").Replace(",", "");
                if (decimal.TryParse(cleaned, NumberStyles.Number, CultureInfo.InvariantCulture, out v))
                    return v;
                return 0m;
            }
        }
        public string Status => cmbStatus.SelectedItem?.ToString() ?? cmbStatus.Text;
        public string PaymentMethod => cmbPaymentMethod.SelectedItem?.ToString() ?? cmbPaymentMethod.Text;

        public FormInvoiceEdit(string invoiceId, string customer, DateTime date, decimal amount, string status, string paymentMethod)
        {
            InitializeComponent();

            txtInvoiceId.Text = invoiceId;
            txtCustomer.Text = customer;
            dtpDate.Value = date;
            txtAmount.Text = amount.ToString("N0", CultureInfo.GetCultureInfo("vi-VN"));
            // Populate status options and select existing
            cmbStatus.Items.AddRange(new object[] { "Đã thu tiền", "Đang chờ", "Hoàn thành", "Không thành công", "Hủy bỏ" });
            if (!string.IsNullOrWhiteSpace(status) && cmbStatus.Items.Contains(status))
                cmbStatus.SelectedItem = status;
            else
                cmbStatus.Text = status;

            // Payment method options
            cmbPaymentMethod.Items.AddRange(new object[] { "Tiền Mặt", "Thẻ", "MoMo", "Other" });
            if (!string.IsNullOrWhiteSpace(paymentMethod) && cmbPaymentMethod.Items.Contains(paymentMethod))
                cmbPaymentMethod.SelectedItem = paymentMethod;
            else
                cmbPaymentMethod.Text = paymentMethod;
        }

        private void InitializeComponent()
        {
            this.Text = "Xem / Sửa Hóa Đơn";
            this.ClientSize = new Size(420, 300);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;

            var lblId = new Label { Text = "Mã Hóa Đơn", Left = 12, Top = 14, Width = 100 };
            txtInvoiceId = new TextBox { Left = 120, Top = 10, Width = 280, ReadOnly = true };

            var lblCustomer = new Label { Text = "Khách Hàng", Left = 12, Top = 50, Width = 100 };
            txtCustomer = new TextBox { Left = 120, Top = 46, Width = 280 };

            var lblDate = new Label { Text = "Ngày", Left = 12, Top = 86, Width = 100 };
            dtpDate = new DateTimePicker { Left = 120, Top = 82, Width = 160, Format = DateTimePickerFormat.Custom, CustomFormat = "dd/MM/yyyy" };

            var lblAmount = new Label { Text = "Số Tiền", Left = 12, Top = 124, Width = 100 };
            txtAmount = new TextBox { Left = 120, Top = 120, Width = 160 };

            var lblStatus = new Label { Text = "Trạng Thái", Left = 12, Top = 162, Width = 100 };
            cmbStatus = new ComboBox { Left = 120, Top = 158, Width = 160, DropDownStyle = ComboBoxStyle.DropDown };

            var lblPayment = new Label { Text = "Phương Thức", Left = 12, Top = 200, Width = 100 };
            cmbPaymentMethod = new ComboBox { Left = 120, Top = 196, Width = 160, DropDownStyle = ComboBoxStyle.DropDown };

            btnSave = new Button { Text = "Lưu", Left = 260, Width = 70, Top = 238, DialogResult = DialogResult.OK };
            btnSave.Click += BtnSave_Click;

            btnCancel = new Button { Text = "Hủy", Left = 342, Width = 70, Top = 238, DialogResult = DialogResult.Cancel };
            btnCancel.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };

            this.Controls.Add(lblId);
            this.Controls.Add(txtInvoiceId);
            this.Controls.Add(lblCustomer);
            this.Controls.Add(txtCustomer);
            this.Controls.Add(lblDate);
            this.Controls.Add(dtpDate);
            this.Controls.Add(lblAmount);
            this.Controls.Add(txtAmount);
            this.Controls.Add(lblStatus);
            this.Controls.Add(cmbStatus);
            this.Controls.Add(lblPayment);
            this.Controls.Add(cmbPaymentMethod);
            this.Controls.Add(btnSave);
            this.Controls.Add(btnCancel);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            // Basic validation
            if (string.IsNullOrWhiteSpace(txtCustomer.Text))
            {
                MessageBox.Show("Tên khách hàng không được để trống.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Try parse amount using vi-VN first
            var s = txtAmount.Text ?? "";
            s = s.Replace("₫", "").Replace("đ", "").Trim();
            if (!decimal.TryParse(s, NumberStyles.Number, CultureInfo.GetCultureInfo("vi-VN"), out var v))
            {
                // fallback: strip separators and parse invariant
                var cleaned = new string(s.Where(c => char.IsDigit(c) || c == '-' || c == '.' || c == ',').ToArray());
                cleaned = cleaned.Replace(".", "").Replace(",", "");
                if (!decimal.TryParse(cleaned, NumberStyles.Number, CultureInfo.InvariantCulture, out v))
                {
                    MessageBox.Show("Số tiền không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            // ensure a status is chosen (allow custom text though)
            if (string.IsNullOrWhiteSpace(cmbStatus.Text))
            {
                MessageBox.Show("Vui lòng chọn trạng thái.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // all good -> close with OK
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
