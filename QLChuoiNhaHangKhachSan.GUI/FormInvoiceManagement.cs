using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static QLChuoiNhaHangKhachSan.GUI.FormPayments;

namespace QLChuoiNhaHangKhachSan.GUI
{
    public partial class FormInvoiceManagement : Form
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["MyConn"]?.ConnectionString
            ?? @"Data Source=.\SQLEXPRESS;Initial Catalog=QuanLyChuoiNhaHangKhachSan;Integrated Security=True";
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

            // Disable hover visuals for action + filter buttons so nothing changes on mouse-over.
            // Visuals will update only on click (SetActiveFilterButton / click handlers).
            DisableHoverVisuals(
                btnPrintInvoice, btnExportExcel, btnDeleteInvoice, btnFix, btnRemove,
                gnbtnALL, gnbtnDont, gnbtnDone, gnbtnOverdue
            );

            // Clear existing rows (designer created columns are used)
            dgvTransaction.Rows.Clear();

            // Load actual data from DB
            LoadDataFromDB();

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

            // Subscribe to cross-form notifications
            try
            {
                NotificationCenter.InvoiceChanged += NotificationCenter_InvoiceChanged;
                this.FormClosing += FormInvoiceManagement_FormClosing;
            }
            catch { }
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

        private void LoadDataFromDB()
        {
            try
            {
                dgvTransaction.Rows.Clear();
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    // Sử dụng Procedure đã tạo trong Database
                    using (SqlCommand cmd = new SqlCommand("sp_GetAllInvoices", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        conn.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                // Lấy dữ liệu từ Database
                                string id = rdr["InvoiceID"].ToString();
                                string customer = rdr["CustomerName"].ToString();

                                // HIỆN GIỜ BỰ RÕ: dd/MM/yyyy HH:mm:ss
                                DateTime dateValue = Convert.ToDateTime(rdr["InvoiceDate"]);
                                string dateFormatted = dateValue.ToString("dd/MM/yyyy HH:mm:ss");

                                decimal amount = Convert.ToDecimal(rdr["Amount"]);
                                // Thêm System.Globalization để hết lỗi CultureInfo
                                string amountText = amount.ToString("N0", System.Globalization.CultureInfo.GetCultureInfo("vi-VN")) + " ₫";
                                string status = rdr["StatusName"].ToString();

                                // SỬA LỖI CS0103: Thêm trực tiếp vào Grid, không dùng biến 'row' rời rạc bên ngoài
                                dgvTransaction.Rows.Add(id, customer, dateFormatted, amountText, status);
                            }
                        }
                    }
                }
                UpdateChartFromGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi nạp dữ liệu: " + ex.Message);
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

                // Support both giao dịch và hóa đơn trạng thái
                if (statusText.Equals("Đã thu tiền", StringComparison.OrdinalIgnoreCase) ||
                    statusText.Equals("Hoàn thành", StringComparison.OrdinalIgnoreCase) ||
                    statusText.Equals("Hoan thanh", StringComparison.OrdinalIgnoreCase))
                {
                    e.CellStyle.BackColor = Color.FromArgb(233, 255, 248);
                    e.CellStyle.ForeColor = Color.FromArgb(0, 122, 78);
                }
                else if (statusText.Equals("Chưa thu tiền", StringComparison.OrdinalIgnoreCase) ||
                         statusText.Equals("Đang chờ", StringComparison.OrdinalIgnoreCase) ||
                         statusText.Equals("Dang cho", StringComparison.OrdinalIgnoreCase))
                {
                    e.CellStyle.BackColor = Color.Gold;
                    e.CellStyle.ForeColor = Color.Black;
                }
                else if (statusText.Equals("Quá hạn", StringComparison.OrdinalIgnoreCase))
                {
                    e.CellStyle.BackColor = Color.IndianRed;
                    e.CellStyle.ForeColor = Color.White;
                }
                else // anything else treated as thất bại
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

            // Read main fields
            var invoiceId = row.Cells["dgvInvoiceID"]?.Value?.ToString() ?? "";
            var customer = row.Cells["dgvCustomer"]?.Value?.ToString() ?? "";
            var date = row.Cells["dvgDate"]?.Value?.ToString() ?? "";
            var amount = row.Cells["dgvAmount"]?.Value?.ToString() ?? "";
            var status = row.Cells["dgvStatus"]?.Value?.ToString() ?? "";
            var payment = dgvTransaction.Columns["PaymentMethod"] != null ? row.Cells["PaymentMethod"]?.Value?.ToString() : "";

            // Collect items from the invoice row(s) if you store them somewhere.
            // In this implementation we don't have detailed line-items in the invoice grid,
            // so we create a single line representing the invoice. If you have a data source
            // for invoice lines, populate 'items' from that source instead.
            var items = new List<Tuple<string, int, decimal, decimal>>();

            // If you have a convention to include items in hidden columns or related store, extract them.
            // Fallback: add a single summary row.
            decimal parsedAmount = ParseFormattedAmount(amount);
            items.Add(Tuple.Create("Tổng hóa đơn", 1, parsedAmount, parsedAmount));

            // Create and populate the Bill form
            using (var bill = new frmBill())
            {
                bill.PopulateFromInvoice(invoiceId, customer, date, amount, status, payment, items);
                // Center and show as modal dialog. frmBill handles its own print/preview button.
                bill.StartPosition = FormStartPosition.CenterParent;
                bill.ShowDialog(this);
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
            // Lấy dòng đang được chọn
            var rowSelected = GetSelectedRow();
            if (rowSelected == null)
            {
                MessageBox.Show("Vui lòng chọn một hóa đơn để hủy.", "Thông báo");
                return;
            }

            string invoiceId = rowSelected.Cells["dgvInvoiceID"].Value.ToString();
            var confirm = MessageBox.Show($"Bạn có chắc muốn hủy hóa đơn '{invoiceId}' không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirm == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(_connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("sp_DeleteInvoice", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@id", invoiceId);
                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                    // Load lại dữ liệu để cập nhật Grid và thông báo thành công
                    LoadDataFromDB();
                    CustomMessageBox.Show("Xóa thành công!", this); //
                    try { NotificationCenter.RaiseInvoiceChanged(); } catch { }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi hệ thống: " + ex.Message);
                }
            }
        }

        // Edit button handler (details panel) - opens modal edit form and applies changes to the selected row
        private void BtnFix_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra xem người dùng đã chọn hóa đơn nào chưa
            var row = GetSelectedRow();
            if (row == null)
            {
                MessageBox.Show("Vui lòng chọn một hóa đơn từ danh sách để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 2. Lấy dữ liệu hiện tại từ dòng đang chọn để đưa lên Form sửa
            string id = row.Cells["dgvInvoiceID"].Value?.ToString() ?? "";
            string customer = row.Cells["dgvCustomer"].Value?.ToString() ?? "";
            string status = row.Cells["dgvStatus"].Value?.ToString() ?? "";

            // Xử lý lấy ngày tháng (bao gồm cả giờ phút nếu có)
            DateTime dateVal = DateTime.Today;
            string dateText = row.Cells["dvgDate"].Value?.ToString() ?? "";
            if (!DateTime.TryParseExact(dateText, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateVal))
            {
                DateTime.TryParse(dateText, CultureInfo.GetCultureInfo("vi-VN"), DateTimeStyles.None, out dateVal);
            }

            // Xử lý lấy số tiền (loại bỏ chữ ₫ và dấu chấm phân cách)
            decimal amount = ParseFormattedAmount(row.Cells["dgvAmount"].Value?.ToString() ?? "0");

            // Lấy phương thức thanh toán (từ cột ẩn nếu có)
            string payment = dgvTransaction.Columns["PaymentMethod"] != null ? row.Cells["PaymentMethod"].Value?.ToString() : "Tiền Mặt";

            // 3. Hiển thị Form sửa hóa đơn
            using (var editForm = new FormInvoiceEdit(id, customer, dateVal, amount, status, payment))
            {
                if (editForm.ShowDialog(this) == DialogResult.OK)
                {
                    try
                    {
                        // 4. Lưu thay đổi vào Database thông qua Stored Procedure
                        using (SqlConnection conn = new SqlConnection(_connectionString))
                        {
                            conn.Open();
                            using (var trans = conn.BeginTransaction())
                            {
                                try
                                {
                                    // Update Invoices via stored procedure
                                    using (SqlCommand cmd = new SqlCommand("sp_UpdateInvoice", conn, trans))
                                    {
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.AddWithValue("@id", editForm.InvoiceId);
                                        cmd.Parameters.AddWithValue("@name", editForm.Customer);
                                        cmd.Parameters.AddWithValue("@amount", editForm.Amount);
                                        cmd.Parameters.AddWithValue("@statusName", editForm.Status);
                                        cmd.ExecuteNonQuery();
                                    }

                                    // Đồng bộ Transactions để FormPayments không mất dòng
                                    using (SqlCommand cmdTrans = new SqlCommand(@"UPDATE Transactions
                                                                  SET CustomerName = @name,
                                                                      Amount = @amount,
                                                                      StatusID = @statusID
                                                                  WHERE InvoiceID = @id", conn, trans))
                                    {
                                        cmdTrans.Parameters.AddWithValue("@id", editForm.InvoiceId);
                                        cmdTrans.Parameters.AddWithValue("@name", editForm.Customer);
                                        cmdTrans.Parameters.AddWithValue("@amount", editForm.Amount);
                                        cmdTrans.Parameters.AddWithValue("@statusID", MapStatusToId(editForm.Status));
                                        cmdTrans.ExecuteNonQuery();
                                    }

                                    trans.Commit();
                                }
                                catch
                                {
                                    try { trans.Rollback(); } catch { }
                                    throw;
                                }
                            }
                        }

                        // 5. CẬP NHẬT LẠI GIAO DIỆN FORM CHÍNH
                        RefreshGridAndKeepRowVisible(id);

                        MessageBox.Show($"Cập nhật hóa đơn {id} thành công!", "Hoàn tất", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        try { NotificationCenter.RaiseInvoiceChanged(); } catch { }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi lưu vào Database: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
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

        private void RefreshGridAndKeepRowVisible(string invoiceId)
        {
            LoadDataFromDB();

            // Re-apply the active status filter so the UI stays consistent
            if (IsButtonActive(gnbtnDont)) ApplyStatusFilter(IsUnpaidRow);
            else if (IsButtonActive(gnbtnDone)) ApplyStatusFilter(IsPaidRow);
            else if (IsButtonActive(gnbtnOverdue)) ApplyStatusFilter(IsOverdueRow);
            else
            {
                ApplyStatusFilter(row => true);
                SetActiveFilterButton(gnbtnALL);
            }

            // Try to keep the edited invoice visible/selected. If it was hidden by a filter,
            // fall back to the "All" view so the user can still see it.
            if (!TrySelectRowById(invoiceId))
            {
                ApplyStatusFilter(row => true);
                SetActiveFilterButton(gnbtnALL);
                TrySelectRowById(invoiceId);
            }
        }

        private bool TrySelectRowById(string invoiceId)
        {
            if (string.IsNullOrWhiteSpace(invoiceId) || dgvTransaction == null) return false;

            foreach (DataGridViewRow r in dgvTransaction.Rows)
            {
                if (r.IsNewRow) continue;

                var idVal = r.Cells["dgvInvoiceID"]?.Value?.ToString();
                if (string.Equals(idVal, invoiceId, StringComparison.OrdinalIgnoreCase))
                {
                    if (!r.Visible) return false;

                    dgvTransaction.ClearSelection();
                    r.Selected = true;
                    for (int i = 0; i < r.Cells.Count; i++)
                    {
                        if (r.Cells[i].Visible)
                        {
                            dgvTransaction.CurrentCell = r.Cells[i];
                            break;
                        }
                    }
                    return true;
                }
            }

            return false;
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

            // ensure hover state won't override the active look
            SyncHoverStateWithCurrent(active);
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

            // ensure hover state synced so hovering does not change appearance
            SyncHoverStateWithCurrent(btn);
        }

        private void btnRemove_Click_1(object sender, EventArgs e)
        {

        }

        private void btnPrintInvoice_Click_1(object sender, EventArgs e)
        {

        }

        private void btnExportExcel_Click_1(object sender, EventArgs e)
        {

        }

        // -------------------------------------------------------------------------

        // -------------------- New: helpers to disable hover visuals -----------------
        // The goal: prevent any hover visual change for specified Guna2Buttons.
        // They will remain visually unchanged when the mouse moves over them;
        // only clicks (which call SetActiveFilterButton / click handlers) change appearance.

        private void DisableHoverVisuals(params Guna2Button[] buttons)
        {
            if (buttons == null) return;
            foreach (var btn in buttons)
            {
                if (btn == null) continue;

                // Remove previous handlers to avoid duplicate subscriptions
                btn.MouseEnter -= Btn_NoHover_MouseEnter;
                btn.MouseMove -= Btn_NoHover_MouseEnter;
                btn.MouseLeave -= Btn_NoHover_MouseLeave;

                // Attach handlers that keep hover state equal to the current state
                btn.MouseEnter += Btn_NoHover_MouseEnter;
                btn.MouseMove += Btn_NoHover_MouseEnter;
                btn.MouseLeave += Btn_NoHover_MouseLeave;

                // initial sync so designer hover settings don't show
                SyncHoverStateWithCurrent(btn);

                btn.PressedColor = btn.FillColor;
            }
        }


        private void Btn_NoHover_MouseEnter(object sender, EventArgs e)
        {
            var b = sender as Guna2Button;
            if (b == null) return;

            try
            {
                // Keep HoverState colors in sync with the current visual state.
                // Some Guna versions expose FillColor/ForeColor/BorderColor on HoverState
                // but do NOT expose BorderThickness on ButtonState — avoid accessing it.
                b.HoverState.FillColor = b.FillColor;
                b.HoverState.ForeColor = b.ForeColor;
                b.HoverState.BorderColor = b.BorderColor;
            }
            catch
            {
                // If HoverState properties are not available (different Guna version),
                // silently ignore to preserve runtime stability.
            }
            try
            {
                // Keep HoverState colors in sync with the current visual state.
                // Some Guna versions expose FillColor/ForeColor/BorderColor on HoverState
                // but do NOT expose BorderThickness on ButtonState — avoid accessing it.
                b.HoverState.FillColor = b.FillColor;
                b.HoverState.ForeColor = b.ForeColor;
                b.HoverState.BorderColor = b.BorderColor;
            }
            catch
            {
                // If HoverState properties are not available (different Guna version),
                // silently ignore to preserve runtime stability.
            }
            }

        private void Btn_NoHover_MouseLeave(object sender, EventArgs e)
        {
            var b = sender as Guna2Button;
            if (b == null) return;
            // re-sync on leave as well
            b.HoverState.FillColor = b.FillColor;
            b.HoverState.ForeColor = b.ForeColor;
            b.HoverState.BorderColor = b.BorderColor;
        }

        private void SyncHoverStateWithCurrent(Guna2Button b)
        {
            if (b == null) return;
            b.HoverState.FillColor = b.FillColor;
            b.HoverState.ForeColor = b.ForeColor;
            b.HoverState.BorderColor = b.BorderColor;
        }
        
        private int MapStatusToId(string status)
        {
            if (string.IsNullOrWhiteSpace(status)) return 2; // pending default
            var s = status.Trim().ToLowerInvariant();
            if (s.Contains("đã thu") || s.Contains("da thu") || s.Contains("hoàn")) return 1; // success
            if (s.Contains("quá hạn") || s.Contains("qua han")) return 3; // overdue/fail
            if (s.Contains("không") || s.Contains("khong")) return 3; // explicit fail
            if (s.Contains("chưa thu") || s.Contains("chua thu") || s.Contains("đang chờ") || s.Contains("dang cho")) return 2; // pending
            return 2; // default pending
        }
        // --- NotificationCenter handlers ---
        private void NotificationCenter_InvoiceChanged(object sender, EventArgs e)
        {
            if (!this.IsHandleCreated) return;
            try
            {
                this.BeginInvoke(new Action(() =>
                {
                    try
                    {
                        LoadDataFromDB();
                        UpdateChartFromGrid();
                    }
                    catch { }
                }));
            }
            catch { }
        }

        private void FormInvoiceManagement_FormClosing(object sender, FormClosingEventArgs e)
        {
            try { NotificationCenter.InvoiceChanged -= NotificationCenter_InvoiceChanged; } catch { }
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

        // Added optional isNew parameter (default false).
        public FormInvoiceEdit(string invoiceId, string customer, DateTime date, decimal amount, string status, string paymentMethod, bool isNew = false)
        {
            InitializeComponent();

            // InvoiceId: editable when creating new record, read-only otherwise
            txtInvoiceId.Text = invoiceId ?? string.Empty;
            txtInvoiceId.ReadOnly = !isNew;
            txtInvoiceId.Enabled = isNew;
            txtInvoiceId.TabStop = isNew;
            txtInvoiceId.BackColor = isNew ? SystemColors.Window : SystemColors.ControlLight;

            txtCustomer.Text = customer ?? string.Empty;
            dtpDate.Value = date;
            txtAmount.Text = amount.ToString("N0", CultureInfo.GetCultureInfo("vi-VN"));

            // Populate status options and select existing
            cmbStatus.Items.Clear();
            // Đồng bộ đúng trạng thái có trong DB để tránh mất dòng sau khi lưu
            cmbStatus.Items.AddRange(new object[] { "Đã thu tiền", "Chưa thu tiền", "Quá hạn" });
            if (!string.IsNullOrWhiteSpace(status) && cmbStatus.Items.Contains(status))
                cmbStatus.SelectedItem = status;
            else
                cmbStatus.Text = status;

            // Payment method options
            cmbPaymentMethod.Items.Clear();
            cmbPaymentMethod.Items.AddRange(new object[] { "Tiền Mặt", "Thẻ", "MoMo", "Khác" });
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
            txtInvoiceId = new TextBox { Left = 120, Top = 10, Width = 280,
                ReadOnly = true,
                Enabled = false,
                TabStop = false,
                BackColor = SystemColors.ControlLight,
                ForeColor = SystemColors.ControlText
            };

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

            // If InvoiceId is editable (create mode), ensure it is provided
            if (txtInvoiceId.Enabled && string.IsNullOrWhiteSpace(txtInvoiceId.Text))
            {
                MessageBox.Show("Mã hóa đơn không được để trống.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
