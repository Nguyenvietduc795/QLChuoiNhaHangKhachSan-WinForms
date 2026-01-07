using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace QLChuoiNhaHangKhachSan.GUI
{
    public partial class FormPayments : Form
    {
        private List<Transaction> _transactions = new List<Transaction>();
        private List<Transaction> _allTransactions = new List<Transaction>();

        // shared fonts/colors
        private readonly Font _dataFont = new Font("Segoe UI", 11F, FontStyle.Regular);
        private readonly Font _idFont = new Font("Segoe UI", 11F, FontStyle.Bold);
        private readonly Font _statusFont = new Font("Segoe UI", 11F, FontStyle.Bold);

        public FormPayments()
        {
            InitializeComponent();
            this.Load += FormPayments_Load;

            // wire events for filtering (designer names)
            this.btnPending.Click += BtnPending_Click;
            this.btnSuccessful.Click += BtnSuccess_Click;
            this.btnRevenue.Click += BtnAll_Click;
            if (this.btnUnsuccessful != null)
                this.btnUnsuccessful.Click += BtnUnsuccessful_Click;

            // quick-action buttons
            if (this.btnDownload != null)
                this.btnDownload.Click += BtnDownload_Click;
            if (this.btnFix != null)
                this.btnFix.Click += BtnEdit_Click;

            // btnCreateInvoice is wired in Designer to btnCreateInvoice_Click

            // ensure single selection and custom selection color
            this.dgvTransaction.MultiSelect = false;
            this.dgvTransaction.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvTransaction.EnableHeadersVisualStyles = false;

            // set consistent fonts for grid (applies to new rows/cells)
            this.dgvTransaction.DefaultCellStyle.Font = _dataFont;
            this.dgvTransaction.RowTemplate.Height = 36;

            // Chart interaction
            if (this.chart2 != null)
                this.chart2.MouseClick += Chart2_MouseClick;
        }

        private void FormPayments_Load(object sender, EventArgs e)
        {
            // header styling
            this.dgvTransaction.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(3, 35, 140);
            this.dgvTransaction.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            this.dgvTransaction.ColumnHeadersDefaultCellStyle.Padding = new Padding(10, 12, 10, 12);
            this.dgvTransaction.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);

            // selection appearance: data rows selected -> light gray; header stays unchanged
            this.dgvTransaction.DefaultCellStyle.SelectionBackColor = Color.FromArgb(245, 245, 245);
            this.dgvTransaction.DefaultCellStyle.SelectionForeColor = Color.Black;
            this.dgvTransaction.RowHeadersVisible = false;



            // chart
            ConfigureChart();
            LoadChartData();

            LoadTransactionsToGrid(_transactions);
            UpdateSummary();

            this.dgvTransaction.SelectionChanged += DgvTransaction_SelectionChanged;
        }

        private void DgvTransaction_SelectionChanged(object sender, EventArgs e)
        {
            // nothing special
        }

        private void LoadTransactionsToGrid(IEnumerable<Transaction> items)
        {
            dgvTransaction.Rows.Clear();

            foreach (var t in items)
            {
                int idx = dgvTransaction.Rows.Add();
                var row = dgvTransaction.Rows[idx];

                var cellId = row.Cells["dgvInvoiceID"];
                cellId.Value = t.InvoiceId;
                cellId.Style.Font = _idFont;
                cellId.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                cellId.Style.ForeColor = Color.Black;
                cellId.Style.BackColor = Color.White;

                var cellCustomer = row.Cells["dgvCustomer"];
                cellCustomer.Value = t.Customer;
                cellCustomer.Style.Font = _dataFont;
                cellCustomer.Style.ForeColor = Color.Black;
                cellCustomer.Style.BackColor = Color.White;
                cellCustomer.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;

                var cellDate = row.Cells["dvgDate"];
                cellDate.Value = t.Date.ToString("dd/MM/yyyy");
                cellDate.Style.Font = _dataFont;
                cellDate.Style.ForeColor = Color.Black;
                cellDate.Style.BackColor = Color.White;
                cellDate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                var cellAmount = row.Cells["dgvAmount"];
                cellAmount.Value = t.Amount.ToString("N0") + " đ";
                cellAmount.Style.Font = _dataFont;
                cellAmount.Style.ForeColor = Color.Black;
                cellAmount.Style.BackColor = Color.White;
                cellAmount.Style.Alignment = DataGridViewContentAlignment.MiddleRight;

                var cellStatus = row.Cells["dgvStatus"];
                cellStatus.Value = StatusToString(t.Status);
                cellStatus.Style.Font = _statusFont;
                cellStatus.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                cellStatus.Style.BackColor = StatusColor(t.Status);
                cellStatus.Style.ForeColor = StatusTextColor(t.Status);
                cellStatus.Style.Padding = new Padding(6);
            }

            dgvTransaction.ClearSelection();
        }

        private string StatusToString(TransactionStatus s)
        {
            // always show Vietnamese in grid
            switch (s)
            {
                case TransactionStatus.Success: return "Hoàn thành";
                case TransactionStatus.Pending: return "Đang chờ";
                case TransactionStatus.Failed:  return "Không thành công";
                default: return s.ToString();
            }
        }

        private Color StatusColor(TransactionStatus s)
        {
            switch (s)
            {
                case TransactionStatus.Success: return Color.FromArgb(220, 255, 245);
                case TransactionStatus.Pending: return Color.FromArgb(255, 219, 77);
                case TransactionStatus.Failed:  return Color.FromArgb(255, 179, 179);
                default: return Color.White;
            }
        }

        private Color StatusTextColor(TransactionStatus s)
        {
            switch (s)
            {
                case TransactionStatus.Success: return Color.FromArgb(0, 128, 64);
                case TransactionStatus.Pending: return Color.FromArgb(153, 102, 0);
                case TransactionStatus.Failed:  return Color.FromArgb(153, 0, 0);
                default: return Color.Black;
            }
        }

        #region Chart methods
        private void ConfigureChart()
        {
            if (chart2 == null) return;

            chart2.Series.Clear();
            chart2.Legends.Clear();
            chart2.ChartAreas.Clear();

            var area = new ChartArea("ChartArea1");
            area.BackColor = Color.Transparent;
            area.AxisX.Enabled = AxisEnabled.False;
            area.AxisY.Enabled = AxisEnabled.False;
            chart2.ChartAreas.Add(area);

            var legend = new Legend("Legend1") {
                Docking = Docking.Bottom,
                Alignment = StringAlignment.Center,
                LegendStyle = LegendStyle.Row,
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", 9F, FontStyle.Regular)
            };
            chart2.Legends.Add(legend);

            var series = new Series("Payments")
            {
                ChartArea = "ChartArea1",
                Legend = "Legend1",
                ChartType = SeriesChartType.Doughnut
            };
            series["DoughnutRadius"] = "60";
            series["PieLabelStyle"] = "Outside";
            series.IsValueShownAsLabel = true;
            series.Label = "#PERCENT\n#VALY{N0} đ";
            series.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            chart2.Series.Add(series);
            chart2.BackColor = Color.Transparent;
        }

        private void LoadChartData()
        {
            var series = chart2.Series["Payments"];
            if (series == null) return;
            series.Points.Clear();

            var grouped = _allTransactions
                .GroupBy(t => t.Method)
                .Select(g => new { Method = g.Key, Total = g.Sum(x => x.Amount) })
                .ToList();

            AddChartPoint(series, PaymentMethod.Cash, grouped, "Tiền mặt", Color.FromArgb(0,153,0));
            AddChartPoint(series, PaymentMethod.Card, grouped, "Thẻ", Color.FromArgb(0,77,204));
            AddChartPoint(series, PaymentMethod.Other, grouped, "Khác", Color.FromArgb(153,0,153));
            chart2.Invalidate();
        }

        private void AddChartPoint(Series series, PaymentMethod method, IEnumerable<dynamic> grouped, string legendText, Color color)
        {
            double value = 0;
            var found = grouped.FirstOrDefault(g => g.Method == method);
            if (found != null) value = (double)found.Total;

            int idx = series.Points.AddY(value);
            var point = series.Points[idx];
            point.LegendText = legendText;
            point.AxisLabel = legendText;
            point.Color = color;
            point["Exploded"] = "False";
            point.Tag = method;
        }

        private void Chart2_MouseClick(object sender, MouseEventArgs e)
        {
            var result = chart2.HitTest(e.X, e.Y);
            if (result == null) return;

            if (result.Series != null && result.PointIndex >= 0)
            {
                var pt = result.Series.Points[result.PointIndex];
                foreach (var p in result.Series.Points) p["Exploded"] = "False";
                pt["Exploded"] = "True";

                if (pt.Tag is PaymentMethod pm) FilterTransactionsByPaymentMethod(pm);
                else FilterTransactionsByLabel(pt.AxisLabel ?? pt.LegendText);
            }
            else
            {
                var s = chart2.Series.FirstOrDefault();
                if (s != null) foreach (var p in s.Points) p["Exploded"] = "False";
                BtnAll_Click(this, EventArgs.Empty);
            }
        }
        #endregion

        private void FilterTransactionsByPaymentMethod(PaymentMethod method)
        {
            _transactions = _allTransactions.Where(x => x.Method == method).ToList();
            LoadTransactionsToGrid(_transactions);
            dgvTransaction.ClearSelection();
        }

        private void FilterTransactionsByLabel(string label)
        {
            if (string.IsNullOrEmpty(label)) { BtnAll_Click(this, EventArgs.Empty); return; }
            if (label.Contains("Tiền")) FilterTransactionsByPaymentMethod(PaymentMethod.Cash);
            else if (label.Contains("Thẻ")) FilterTransactionsByPaymentMethod(PaymentMethod.Card);
            else if (label.Contains("Khác")) FilterTransactionsByPaymentMethod(PaymentMethod.Other);
            else BtnAll_Click(this, EventArgs.Empty);
        }

        private void UpdateSummary()
        {
            var totalRevenue = _allTransactions.Sum(x => x.Amount);
            var pendingCount = _allTransactions.Count(x => x.Status == TransactionStatus.Pending);
            var successCount = _allTransactions.Count(x => x.Status == TransactionStatus.Success);
            var failedCount = _allTransactions.Count(x => x.Status == TransactionStatus.Failed);

            this.btnRevenue.Text = totalRevenue.ToString("N0") + " đ";
            this.btnPending.Text = pendingCount.ToString();
            this.btnSuccessful.Text = successCount.ToString();
            if (this.btnUnsuccessful != null) this.btnUnsuccessful.Text = failedCount.ToString();

            this.lblTotalRevenueTitle.Text = "Total Revenue";
            this.lblPendingInvoice.Text = "Active Customers";
            this.lblSuccessfulPayment.Text = "1 Minute Ago";
        }

        private void BtnPending_Click(object sender, EventArgs e)
        {
            _transactions = _allTransactions.Where(x => x.Status == TransactionStatus.Pending).ToList();
            LoadTransactionsToGrid(_transactions);
            dgvTransaction.ClearSelection();
        }

        private void BtnSuccess_Click(object sender, EventArgs e)
        {
            _transactions = _allTransactions.Where(x => x.Status == TransactionStatus.Success).ToList();
            LoadTransactionsToGrid(_transactions);
            dgvTransaction.ClearSelection();
        }

        private void BtnUnsuccessful_Click(object sender, EventArgs e)
        {
            _transactions = _allTransactions.Where(x => x.Status == TransactionStatus.Failed).ToList();
            LoadTransactionsToGrid(_transactions);
            dgvTransaction.ClearSelection();
        }

        private void BtnAll_Click(object sender, EventArgs e)
        {
            _transactions = new List<Transaction>(_allTransactions);
            LoadTransactionsToGrid(_transactions);
            dgvTransaction.ClearSelection();

            var s = chart2.Series.FirstOrDefault();
            if (s != null) foreach (var p in s.Points) p["Exploded"] = "False";
        }

        // --- New / Edit / Download functionality below ---

        // Create invoice button (wired in Designer)
        private void btnCreateInvoice_Click(object sender, EventArgs e)
        {
            // generate next invoice id automatically
            var nextId = GetNextInvoiceId();
            using (var dlg = new InvoiceEditorForm(nextId))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    var invoice = dlg.Result;
                    // no need to check id uniqueness because generated id is based on data, but keep safety
                    if (_allTransactions.Any(t => t.InvoiceId == invoice.InvoiceId))
                    {
                        MessageBox.Show("Mã hóa đơn đã tồn tại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    _allTransactions.Insert(0, invoice);
                    LoadChartData();
                    UpdateSummary();
                    LoadTransactionsToGrid(_allTransactions);
                }
            }
        }

        // compute next invoice id using existing numeric suffixes (keeps width)
        private string GetNextInvoiceId()
        {
            int maxNum = 0;
            int maxDigits = 3; // default width INV-001
            foreach (var t in _allTransactions)
            {
                if (string.IsNullOrWhiteSpace(t.InvoiceId)) continue;
                // extract continuous digits at end or all digits
                var digits = new string(t.InvoiceId.Where(char.IsDigit).ToArray());
                if (string.IsNullOrEmpty(digits)) continue;
                if (int.TryParse(digits, out int n))
                {
                    if (n > maxNum) maxNum = n;
                    if (digits.Length > maxDigits) maxDigits = digits.Length;
                }
            }

            int next = maxNum + 1;
            var format = "D" + Math.Max(maxDigits, 3);
            return $"INV-{next.ToString(format)}";
        }

        // Download current view to CSV
        private void BtnDownload_Click(object sender, EventArgs e)
        {
            using (var dlg = new SaveFileDialog())
            {
                dlg.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
                dlg.FileName = "PaymentsReport.csv";
                if (dlg.ShowDialog(this) != DialogResult.OK) return;

                try
                {
                    using (var sw = new StreamWriter(dlg.FileName, false, Encoding.UTF8))
                    {
                        sw.WriteLine("Mã Giao Dịch,Khách Hàng,Ngày Giao Dịch,Số Tiền,Trạng Thái,Phương Thức");
                        foreach (var t in _transactions)
                        {
                            sw.WriteLine($"{t.InvoiceId},{EscapeCsv(t.Customer)},{t.Date:yyyy-MM-dd},{t.Amount},{StatusToString(t.Status)},{MethodToVN(t.Method)}");
                        }
                    }

                    MessageBox.Show("Báo cáo đã được lưu.", "Hoàn tất", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi lưu báo cáo: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private static string EscapeCsv(string s)
        {
            if (string.IsNullOrEmpty(s)) return "";
            if (s.Contains(",") || s.Contains("\"") || s.Contains("\n"))
                return "\"" + s.Replace("\"", "\"\"") + "\"";
            return s;
        }

        // Edit selected invoice
        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (dgvTransaction.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn 1 giao dịch để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var row = dgvTransaction.SelectedRows[0];
            var id = (row.Cells["dgvInvoiceID"].Value ?? "").ToString();
            var existing = _allTransactions.FirstOrDefault(t => t.InvoiceId == id);
            if (existing == null)
            {
                MessageBox.Show("Không tìm thấy hóa đơn.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (var dlg = new InvoiceEditorForm(existing))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    var updated = dlg.Result;
                    var idx = _allTransactions.FindIndex(t => t.InvoiceId == existing.InvoiceId);
                    if (idx >= 0) _allTransactions[idx] = updated;
                    LoadChartData();
                    UpdateSummary();
                    LoadTransactionsToGrid(_allTransactions);
                }
            }
        }

        // Simple invoice editor used for create/edit
        private class InvoiceEditorForm : Form
        {
            public Transaction Result { get; private set; }

            private TextBox txtId;
            private TextBox txtCustomer;
            private DateTimePicker dtpDate;
            private NumericUpDown numAmount;
            private ComboBox cboStatus;
            private ComboBox cboMethod;
            private Button btnOk;

            public InvoiceEditorForm() { InitializeControls(); }

            // new ctor: receive generated id and prefill + lock id
            public InvoiceEditorForm(string generatedId) : this()
            {
                txtId.Text = generatedId;
                txtId.Enabled = false;
            }

            public InvoiceEditorForm(Transaction existing) : this()
            {
                txtId.Text = existing.InvoiceId;
                txtId.Enabled = false;
                txtCustomer.Text = existing.Customer;
                dtpDate.Value = existing.Date;
                numAmount.Value = (decimal)existing.Amount;
                cboStatus.SelectedItem = StatusToVN(existing.Status);
                cboMethod.SelectedItem = MethodToVN(existing.Method);
            }

            private void InitializeControls()
            {
                this.Text = "Tạo / Sửa hóa đơn";
                this.FormBorderStyle = FormBorderStyle.FixedDialog;
                this.StartPosition = FormStartPosition.CenterParent;
                this.ClientSize = new Size(420, 320);
                this.MaximizeBox = false;
                this.MinimizeBox = false;

                var lblId = new Label { Text = "Mã hóa đơn", Location = new Point(12, 14), AutoSize = true };
                txtId = new TextBox { Location = new Point(12, 36), Width = 380 };

                var lblCustomer = new Label { Text = "Khách hàng", Location = new Point(12, 70), AutoSize = true };
                txtCustomer = new TextBox { Location = new Point(12, 92), Width = 380 };

                var lblDate = new Label { Text = "Ngày", Location = new Point(12, 126), AutoSize = true };
                dtpDate = new DateTimePicker { Location = new Point(12, 148), Width = 200, Format = DateTimePickerFormat.Custom, CustomFormat = "dd/MM/yyyy" };

                var lblAmount = new Label { Text = "Số tiền", Location = new Point(12, 182), AutoSize = true };
                numAmount = new NumericUpDown { Location = new Point(12, 204), Width = 200, Maximum = 1000000000, DecimalPlaces = 0, Increment = 1000 };

                var lblStatus = new Label { Text = "Trạng thái", Location = new Point(230, 126), AutoSize = true };
                cboStatus = new ComboBox { Location = new Point(230, 148), Width = 162, DropDownStyle = ComboBoxStyle.DropDownList };
                // Vietnamese items
                cboStatus.Items.AddRange(new[] { "Hoàn thành", "Đang chờ", "Không thành công" });
                cboStatus.SelectedIndex = 0;

                var lblMethod = new Label { Text = "Phương thức", Location = new Point(230, 182), AutoSize = true };
                cboMethod = new ComboBox { Location = new Point(230, 204), Width = 162, DropDownStyle = ComboBoxStyle.DropDownList };
                cboMethod.Items.AddRange(new[] { "Tiền mặt", "Thẻ", "Khác" });
                cboMethod.SelectedIndex = 0;

                btnOk = new Button { Text = "OK", DialogResult = DialogResult.OK, Location = new Point(220, 250), Width = 80 };
                var btnCancel = new Button { Text = "Hủy", DialogResult = DialogResult.Cancel, Location = new Point(312, 250), Width = 80 };
                btnOk.Click += BtnOk_Click;

                this.Controls.AddRange(new Control[] {
                    lblId, txtId, lblCustomer, txtCustomer, lblDate, dtpDate, lblAmount, numAmount,
                    lblStatus, cboStatus, lblMethod, cboMethod, btnOk, btnCancel
                });
            }

            private void BtnOk_Click(object sender, EventArgs e)
            {
                if (string.IsNullOrWhiteSpace(txtId.Text)) { MessageBox.Show("Mã hóa đơn không được để trống."); this.DialogResult = DialogResult.None; return; }
                if (string.IsNullOrWhiteSpace(txtCustomer.Text)) { MessageBox.Show("Khách hàng không được để trống."); this.DialogResult = DialogResult.None; return; }

                var status = VNToStatus(cboStatus.SelectedItem.ToString());
                var method = VNToMethod(cboMethod.SelectedItem.ToString());

                Result = new Transaction(txtId.Text.Trim(), txtCustomer.Text.Trim(), dtpDate.Value.Date, (double)numAmount.Value, status, method);
            }

            // helpers for VN <-> enum
            private static string StatusToVN(TransactionStatus s)
            {
                switch (s)
                {
                    case TransactionStatus.Success: return "Hoàn thành";
                    case TransactionStatus.Pending: return "Đang chờ";
                    case TransactionStatus.Failed:  return "Không thành công";
                    default: return s.ToString();
                }
            }

            private static TransactionStatus VNToStatus(string vn)
            {
                switch (vn)
                {
                    case "Hoàn thành": return TransactionStatus.Success;
                    case "Đang chờ": return TransactionStatus.Pending;
                    case "Không thành công": return TransactionStatus.Failed;
                    default:
                        return TransactionStatus.Pending;
                }
            }

            private static string MethodToVN(PaymentMethod m)
            {
                switch (m)
                {
                    case PaymentMethod.Cash: return "Tiền mặt";
                    case PaymentMethod.Card: return "Thẻ";
                    case PaymentMethod.Other: return "Khác";
                    default: return m.ToString();
                }
            }

            private static PaymentMethod VNToMethod(string vn)
            {
                switch (vn)
                {
                    case "Tiền mặt": return PaymentMethod.Cash;
                    case "Thẻ": return PaymentMethod.Card;
                    case "Khác": return PaymentMethod.Other;
                    default: return PaymentMethod.Other;
                }
            }
        }

        // helper for export mapping
        private static string MethodToVN(PaymentMethod m)
        {
            switch (m)
            {
                case PaymentMethod.Cash: return "Tiền mặt";
                case PaymentMethod.Card: return "Thẻ";
                case PaymentMethod.Other: return "Khác";
                default: return m.ToString();
            }
        }

        // Transaction model
        private class Transaction
        {
            public string InvoiceId { get; }
            public string Customer { get; }
            public DateTime Date { get; }
            public double Amount { get; }
            public TransactionStatus Status { get; }
            public PaymentMethod Method { get; }

            public Transaction(string id, string customer, DateTime date, double amount, TransactionStatus status, PaymentMethod method)
            {
                InvoiceId = id;
                Customer = customer;
                Date = date;
                Amount = amount;
                Status = status;
                Method = method;
            }
        }

        private enum TransactionStatus { Pending, Success, Failed }
        private enum PaymentMethod { Cash, Card, Other }

        // keep existing empty handlers to satisfy designer wiring
        private void guna2Panel3_Paint(object sender, PaintEventArgs e) { }
        private void guna2Panel2_Paint(object sender, PaintEventArgs e) { }
        private void lblLogo_Click(object sender, EventArgs e) { }
        private void pnlSidebar_Paint(object sender, PaintEventArgs e) { }
        private void guna2Separator1_Click(object sender, EventArgs e) { }
        private void btnLogout_Click(object sender, EventArgs e) { }
        private void btnReports_Click(object sender, EventArgs e) { }
        private void btnPayments_Click(object sender, EventArgs e) { }
        private void btnInventory_Click(object sender, EventArgs e) { }
        private void btnHotels_Click(object sender, EventArgs e) { }
        private void btnRestaurants_Click(object sender, EventArgs e) { }
        private void btnCustomers_Click(object sender, EventArgs e) { }
        private void btnEmplyees_Click(object sender, EventArgs e) { }
        private void btnHome_Click(object sender, EventArgs e) { }
        private void lblSubLogo_Click(object sender, EventArgs e) { }
        private void Gn2pnlPaymentMain_Paint(object sender, PaintEventArgs e) { }
        private void pnlPaymentHeader_Paint(object sender, PaintEventArgs e) { }
        private void lblPaymentTitle_Click(object sender, EventArgs e) { }
        private void lblRevenueTitle_Click(object sender, EventArgs e) { }
        private void iconRevenue_Click(object sender, EventArgs e) { }
        private void guna2Panel1_Paint(object sender, PaintEventArgs e) { }
        private void iconTotalRevenue_Click(object sender, EventArgs e) { }
        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void dataGridView4_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void dataGridView5_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void lblHeaderSub_Click(object sender, EventArgs e) { }
    }
}
