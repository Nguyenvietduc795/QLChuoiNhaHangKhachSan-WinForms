using Guna.UI2.WinForms;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Configuration; // <- needed for ConfigurationManager



namespace QLChuoiNhaHangKhachSan.GUI

{

    public partial class FormPayments : Form

    {

        // CONNECTION STRING: reads App.config <connectionStrings>["MyConn"], fallback to local DB

        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["MyConn"]?.ConnectionString

            ?? @"Data Source=LAPTOP-JF845UR9\SQLEXPRESS02;Initial Catalog=QuanLyChuoiNhaHangKhachSan;Integrated Security=True";



        private List<Transaction> _transactions = new List<Transaction>();

        private List<Transaction> _allTransactions = new List<Transaction>();

        // shared fonts/colors

        private readonly Font _dataFont = new Font("Segoe UI", 11F, FontStyle.Regular);

        private readonly Font _idFont = new Font("Segoe UI", 11F, FontStyle.Bold);

        private readonly Font _statusFont = new Font("Segoe UI", 11F, FontStyle.Bold);



        // Add field to store original visuals (place near other private fields)

        // Fully-qualify Cursor to avoid ambiguity with Charting.Cursor

        private readonly Dictionary<Control, (Color Back, Color Fore, System.Windows.Forms.Cursor Cursor)> _noHoverStates =

            new Dictionary<Control, (Color Back, Color Fore, System.Windows.Forms.Cursor Cursor)>();



        public FormPayments()

        {

            InitializeComponent();

            this.Load += FormPayments_Load;

            if (this.btnDelete != null)
                this.btnDelete.Click += btnDelete_Click;

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



            // Load data from database (falls back to sample data if DB call fails)

            LoadDataFromDB();



            // chart

            ConfigureChart();

            LoadChartData();



            // Transactions into grid and summary

            _transactions = new List<Transaction>(_allTransactions);

            LoadTransactionsToGrid(_transactions);

            UpdateSummary();



            // Prevent hover visuals on the summary / quick-action controls.

            // Removed iconRevenue / iconTotalRevenue (they aren't defined in Designer) to fix CS0103.

            DisableHoverForControls(

                btnRevenue, btnPending, btnSuccessful, btnUnsuccessful,

                btnCreateInvoice, btnDownload, btnFix,

                pnlSummary, lblTotalRevenueTitle, lblPendingInvoice, lblSuccessfulPayment

            );



            // Ensure Guna2 buttons do not change visual on hover/press
            try

            {

                DisableGuna2ButtonVisuals(btnCreateInvoice);

                DisableGuna2ButtonVisuals(btnDownload);

                DisableGuna2ButtonVisuals(btnFix);

            }

            catch { /* ignore if any control is null */ }



            this.dgvTransaction.SelectionChanged += DgvTransaction_SelectionChanged;

            // Subscribe to invoice changes from other forms so this view stays in sync
            try
            {
                NotificationCenter.InvoiceChanged += NotificationCenter_InvoiceChanged;
                this.FormClosing += FormPayments_FormClosing;
            }
            catch { }

        }



        private void DgvTransaction_SelectionChanged(object sender, EventArgs e)

        {

            // nothing special

        }



        // ---------- NEW: Database load implementation ----------

        private void LoadDataFromDB()
        {
            try
            {
                var list = new List<Transaction>();
                using (var conn = new SqlConnection(_connectionString))
                using (var cmd = new SqlCommand("sp_GetAllTransactions", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    using (var rdr = cmd.ExecuteReader())
                    {
                        // helper to check column presence
                        Func<IDataRecord, string, bool> hasCol = (rec, name) =>
                        {
                            for (int i = 0; i < rec.FieldCount; i++)
                                if (string.Equals(rec.GetName(i), name, StringComparison.OrdinalIgnoreCase)) return true;
                            return false;
                        };

                        while (rdr.Read())
                        {
                            string invoiceId = hasCol(rdr, "InvoiceID") ? rdr["InvoiceID"]?.ToString() ?? "" : "";
                            string customer = hasCol(rdr, "CustomerName") ? rdr["CustomerName"]?.ToString() ?? "" : "";

                            DateTime date = DateTime.Today;
                            if (hasCol(rdr, "TransactionDate"))
                            {
                                try { date = Convert.ToDateTime(rdr["TransactionDate"]); } catch { }
                            }

                            double amount = 0;
                            if (hasCol(rdr, "Amount"))
                            {
                                try { amount = Convert.ToDouble(rdr["Amount"]); } catch { }
                            }

                            string methodName = "";
                            if (hasCol(rdr, "MethodName")) methodName = rdr["MethodName"]?.ToString() ?? "";
                            else if (hasCol(rdr, "Method")) methodName = rdr["Method"]?.ToString() ?? "";

                            string statusName = "";
                            if (hasCol(rdr, "StatusName")) statusName = rdr["StatusName"]?.ToString() ?? "";
                            else if (hasCol(rdr, "Status")) statusName = rdr["Status"]?.ToString() ?? "";

                            var method = ParseMethodFromVN(methodName);
                            var status = ParseStatusFromVN(statusName);

                            list.Add(new Transaction(invoiceId, customer, date, amount, status, method));
                        }
                    }
                }


                _allTransactions = list.Any() ? list : (_allTransactions == null || !_allTransactions.Any() ? GetMockData() : _allTransactions);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi kết nối/đọc dữ liệu từ cơ sở dữ liệu.\nHiển thị dữ liệu mẫu.\n\nChi tiết: " + ex.Message, "Lỗi DB", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                if (_allTransactions == null || !_allTransactions.Any()) _allTransactions = GetMockData();
            }
        }



        // Mapping helpers to parse Vietnamese names from DB into enums

        private TransactionStatus ParseStatusFromVN(string v)

        {

            if (string.IsNullOrWhiteSpace(v)) return TransactionStatus.Pending;

            v = v.Trim();

            if (v.Equals("Hoàn thành", StringComparison.OrdinalIgnoreCase)) return TransactionStatus.Success;

            if (v.Equals("Không thành công", StringComparison.OrdinalIgnoreCase)) return TransactionStatus.Failed;

            return TransactionStatus.Pending;

        }



        private PaymentMethod ParseMethodFromVN(string v)

        {

            if (string.IsNullOrWhiteSpace(v)) return PaymentMethod.Other;

            v = v.Trim();

            if (v.Equals("Tiền mặt", StringComparison.OrdinalIgnoreCase) || v.Equals("Cash", StringComparison.OrdinalIgnoreCase)) return PaymentMethod.Cash;

            if (v.Equals("Thẻ", StringComparison.OrdinalIgnoreCase) || v.Equals("Card", StringComparison.OrdinalIgnoreCase)) return PaymentMethod.Card;

            if (v.Equals("MoMo", StringComparison.OrdinalIgnoreCase)) return PaymentMethod.MoMo;

            return PaymentMethod.Other;

        }

        // -------------------------------------------------------



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

                cellDate.Value = t.Date.ToString("dd/MM/yyyy HH:mm:ss");

                cellDate.Style.Font = _dataFont;

                cellDate.Style.ForeColor = Color.Black;

                cellDate.Style.BackColor = Color.White;

                cellDate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;



                // Lấy ô số tiền và định dạng chuẩn: dấu chấm phân cách hàng nghìn, thêm chữ đ ở cuối
                var cellAmount = row.Cells["dgvAmount"];
                cellAmount.Value = t.Amount.ToString("N0", new System.Globalization.CultureInfo("vi-VN")) + " đ";
                cellAmount.Style.Alignment = DataGridViewContentAlignment.MiddleRight; // Cho tiền nằm bên phải cho dễ nhìn



                var cellStatus = row.Cells["dgvStatus"];

                cellStatus.Value = StatusToString(t.Status);

                cellStatus.Style.Font = _statusFont;

                cellStatus.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                cellStatus.Style.BackColor = StatusColor(t.Status);

                cellStatus.Style.ForeColor = StatusTextColor(t.Status);

                cellStatus.Style.Padding = new Padding(6);



                // New: populate payment method cell if the grid has such a column (names vary by Designer)

                var methodCell = GetPaymentMethodCell(row);

                if (methodCell != null)

                {
                    methodCell.Value = MethodToVN(t.Method);

                    methodCell.Style.Font = _dataFont;

                    methodCell.Style.ForeColor = Color.Black;

                    methodCell.Style.BackColor = Color.White;

                    methodCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                }

            }



            dgvTransaction.ClearSelection();

        }



        private DataGridViewCell GetPaymentMethodCell(DataGridViewRow row)

        {

            if (row == null || dgvTransaction.Columns == null || dgvTransaction.Columns.Count == 0) return null;



            // Try common column names used in Designer; be tolerant to casing.

            var col = dgvTransaction.Columns.Cast<DataGridViewColumn>()

                .FirstOrDefault(c =>

                    string.Equals(c.Name, "dgvMethod", StringComparison.OrdinalIgnoreCase)

                    || string.Equals(c.Name, "dgvPTTT", StringComparison.OrdinalIgnoreCase)

                    || string.Equals(c.Name, "dgvPaymentMethod", StringComparison.OrdinalIgnoreCase)

                    || (!string.IsNullOrEmpty(c.HeaderText) && c.HeaderText.IndexOf("PTT", StringComparison.OrdinalIgnoreCase) >= 0)

                );



            if (col != null) return row.Cells[col.Index];



            // If none found, attempt to find a column whose HeaderText includes "PTTT" or "Phương"

            col = dgvTransaction.Columns.Cast<DataGridViewColumn>()

                .FirstOrDefault(c => !string.IsNullOrEmpty(c.HeaderText) && (c.HeaderText.IndexOf("PTTT", StringComparison.OrdinalIgnoreCase) >= 0 || c.HeaderText.IndexOf("Phương", StringComparison.OrdinalIgnoreCase) >= 0));



            if (col != null) return row.Cells[col.Index];



            // fallback: if grid has more than 5 columns assume last column is method (safe fallback for some layouts)

            if (dgvTransaction.Columns.Count > 5) return row.Cells[dgvTransaction.Columns.Count - 1];



            return null;

        }



        private string StatusToString(TransactionStatus s)

        {

            // always show Vietnamese in grid

            switch (s)

            {

                case TransactionStatus.Success: return "Hoàn thành";

                case TransactionStatus.Pending: return "Đang chờ";

                case TransactionStatus.Failed: return "Không thành công";

                default: return s.ToString();

            }

        }



        private Color StatusColor(TransactionStatus s)

        {

            switch (s)

            {

                case TransactionStatus.Success: return Color.FromArgb(220, 255, 245);

                case TransactionStatus.Pending: return Color.FromArgb(255, 219, 77);

                case TransactionStatus.Failed: return Color.FromArgb(255, 179, 179);

                default: return Color.White;

            }

        }



        private Color StatusTextColor(TransactionStatus s)
        {

            switch (s)

            {

                case TransactionStatus.Success: return Color.FromArgb(0, 128, 64);

                case TransactionStatus.Pending: return Color.FromArgb(153, 102, 0);

                case TransactionStatus.Failed: return Color.FromArgb(153, 0, 0);

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



            var legend = new Legend("Legend1")
            {

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
            if (chart2 == null || chart2.Series.Count == 0) return;

            var series = chart2.Series["Payments"];
            series.Points.Clear();
            series.ChartType = SeriesChartType.Doughnut;

            // Tính tổng tiền theo từng phương thức thanh toán từ danh sách _allTransactions (dữ liệu thật từ DB)
            var groupedData = _allTransactions
                .GroupBy(t => t.Method)
                .Select(g => new {
                    Method = g.Key,
                    Total = g.Sum(x => x.Amount)
                })
                .ToList();

            // Thêm các điểm vào biểu đồ
            AddChartPoint(series, PaymentMethod.Cash, groupedData, "Tiền mặt", Color.FromArgb(0, 153, 0));
            AddChartPoint(series, PaymentMethod.Card, groupedData, "Thẻ", Color.FromArgb(0, 77, 204));
            AddChartPoint(series, PaymentMethod.MoMo, groupedData, "MoMo", Color.FromArgb(227, 25, 125));
            AddChartPoint(series, PaymentMethod.Other, groupedData, "Khác", Color.FromArgb(153, 0, 153));

            chart2.Invalidate(); // Vẽ lại biểu đồ
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

            else if (label.Contains("MoMo")) FilterTransactionsByPaymentMethod(PaymentMethod.MoMo);

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



        // Visual flash helper: briefly set a light gray visual for the clicked control.

        private async Task FlashClickVisual(Control c, Color flashColor, int durationMs = 140)

        {

            if (c == null) return;



            try

            {

                // Guna2Button has FillColor property; use cast when available

                if (c is Guna2Button g)

                {

                    var origFill = g.FillColor;

                    var origFore = g.ForeColor;

                    try

                    {

                        g.FillColor = flashColor;

                        g.ForeColor = Color.Black;

                        await Task.Delay(durationMs);

                    }

                    finally

                    {

                        try { g.FillColor = origFill; } catch { }

                        try { g.ForeColor = origFore; } catch { }

                    }

                }

                else

                {

                    var origBack = c.BackColor;

                    var origFore = c.ForeColor;

                    try

                    {

                        c.BackColor = flashColor;

                        c.ForeColor = Color.Black;

                        await Task.Delay(durationMs);

                    }

                    finally

                    {

                        try { c.BackColor = origBack; } catch { }

                        try { c.ForeColor = origFore; } catch { }

                    }

                }

            }
            catch

            {

                // swallow visual errors - do not interrupt main flow

            }

        }



        // Create invoice: flash then show dialog

        private async void btnCreateInvoice_Click(object sender, EventArgs e)
        {
            await FlashClickVisual(btnCreateInvoice, Color.FromArgb(245, 245, 245));
            var nextId = GetNextInvoiceId();

            using (var dlg = new InvoiceEditorForm(nextId))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    var invoice = dlg.Result;
                    try
                    {
                        using (SqlConnection conn = new SqlConnection(_connectionString))
                        {
                            conn.Open();
                            using (var trans = conn.BeginTransaction())
                            {
                                try
                                {
                                    // 1) Insert Invoices (schema: InvoiceDate, TotalAmount, PaymentStatus)
                                    int newInvoiceIdentity = 0;
                                    string invoiceCode = invoice.InvoiceId; // fallback to UI id
                                    using (SqlCommand cmdInv = new SqlCommand(@"INSERT INTO Invoices (InvoiceDate, TotalAmount, PaymentStatus)
VALUES (@date, @amount, @statusName);
SELECT CAST(SCOPE_IDENTITY() AS INT);", conn, trans))
                                    {
                                        cmdInv.Parameters.AddWithValue("@date", invoice.Date);
                                        cmdInv.Parameters.AddWithValue("@amount", invoice.Amount);
                                        cmdInv.Parameters.AddWithValue("@statusName", StatusToString(invoice.Status));
                                        var obj = cmdInv.ExecuteScalar();
                                        if (obj != null && int.TryParse(obj.ToString(), out newInvoiceIdentity))
                                        {
                                            invoiceCode = $"INV-{newInvoiceIdentity.ToString("D5")}";
                                        }
                                    }

                                    // 2) Insert Transactions using the resolved invoiceCode
                                    string sqlTrans = @"INSERT INTO Transactions (InvoiceID, CustomerName, TransactionDate, Amount, MethodID, StatusID) 
                                 VALUES (@id, @name, @date, @amount, @methodID, @statusID)";
                                    using (SqlCommand cmd1 = new SqlCommand(sqlTrans, conn, trans))
                                    {
                                        cmd1.Parameters.AddWithValue("@id", invoiceCode);
                                        cmd1.Parameters.AddWithValue("@name", invoice.Customer);
                                        cmd1.Parameters.AddWithValue("@date", invoice.Date);
                                        cmd1.Parameters.AddWithValue("@amount", invoice.Amount);
                                        cmd1.Parameters.AddWithValue("@methodID", GetMethodID(MethodToVN(invoice.Method)));
                                        cmd1.Parameters.AddWithValue("@statusID", GetStatusID(StatusToString(invoice.Status)));
                                        cmd1.ExecuteNonQuery();
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

                        // Nạp lại dữ liệu để hóa đơn mới hiện lên bảng ngay lập tức
                        LoadDataFromDB();
                        _transactions = new List<Transaction>(_allTransactions);
                        LoadTransactionsToGrid(_transactions);
                        UpdateSummary();

                        CustomMessageBox.Show("Lưu hóa đơn thành công!", this);
                        try { NotificationCenter.RaiseInvoiceChanged(); } catch { }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi lưu: " + ex.Message);
                    }
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



        // Download: flash then perform save

        private async void BtnDownload_Click(object sender, EventArgs e)

        {

            await FlashClickVisual(btnDownload, Color.FromArgb(245, 245, 245));



            using (var dlg = new FormDownload(this))

            {

                dlg.StartPosition = FormStartPosition.CenterParent;

                dlg.ShowDialog(this);

            }

        }



        public void ExportTransactions(DateTime? from, DateTime? to)

        {

            // Choose the source list (current filtered _transactions if available, otherwise all)
            var items = (_transactions != null && _transactions.Count > 0) ? new List<Transaction>(_transactions) : new List<Transaction>(_allTransactions);



            if (from.HasValue)

            {

                items = items.Where(t => t.Date.Date >= from.Value.Date).ToList();

            }



            if (to.HasValue)

            {

                items = items.Where(t => t.Date.Date <= to.Value.Date).ToList();

            }



            using (var dlg = new SaveFileDialog())

            {

                dlg.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";

                dlg.FileName = $"PaymentsReport_{(from.HasValue ? from.Value.ToString("yyyyMMdd") : "all")}_{(to.HasValue ? to.Value.ToString("yyyyMMdd") : "all")}.csv";

                if (dlg.ShowDialog() != DialogResult.OK) return;



                try

                {

                    using (var sw = new StreamWriter(dlg.FileName, false, Encoding.UTF8))

                    {

                        sw.WriteLine("Mã Giao Dịch,Khách Hàng,Ngày Giao Dịch,Số Tiền,Trạng Thái,Phương Thức Thanh Toán");

                        foreach (var t in items)

                        {

                            sw.WriteLine($"{t.InvoiceId},{EscapeCsv(t.Customer)},{t.Date:yyyy-MM-dd HH:mm:ss},{t.Amount},{StatusToString(t.Status)},{MethodToVN(t.Method)}");

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



        private static string EscapeCsv(string value)

        {

            if (string.IsNullOrEmpty(value)) return "";



            // Nếu có dấu phẩy, xuống dòng hoặc dấu nháy → bọc bằng ""

            if (value.Contains(",") || value.Contains("\"") || value.Contains("\n"))

            {

                value = value.Replace("\"", "\"\"");

                return $"\"{value}\"";

            }

            return value;

        }



        // Edit (btnFix): flash then open editor

        private async void BtnEdit_Click(object sender, EventArgs e)
        {
            await FlashClickVisual(btnFix, Color.FromArgb(245, 245, 245));

            // 1. Kiểm tra xem người dùng đã chọn dòng nào trên Grid chưa
            if (dgvTransaction.SelectedRows.Count == 0)
            {
                CustomMessageBox.Show("Vui lòng chọn 1 giao dịch để sửa.", this, CustomMessageBox.BoxType.Warning);
                return;
            }

            // 2. Lấy thông tin hóa đơn hiện tại
            var row = dgvTransaction.SelectedRows[0];
            var id = (row.Cells["dgvInvoiceID"].Value ?? "").ToString();
            var existing = _allTransactions.FirstOrDefault(t => t.InvoiceId == id);

            if (existing == null) return;

            // 3. Mở Form sửa và chờ người dùng nhấn OK
            using (var dlg = new InvoiceEditorForm(existing))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    var updated = dlg.Result;

                    try
                    {
                        using (SqlConnection conn = new SqlConnection(_connectionString))
                        {
                            conn.Open();
                            using (var trans = conn.BeginTransaction())
                            {
                                try
                                {
                                    // 4. Lệnh SQL Update (Đồng bộ với Database)
                                    string sql = @"UPDATE Transactions 
                                 SET CustomerName = @name, 
                                     Amount = @amount, 
                                     MethodID = @methodID, 
                                     StatusID = @statusID 
                                 WHERE InvoiceID = @id";

                                    using (SqlCommand cmd = new SqlCommand(sql, conn, trans))
                                    {
                                        cmd.Parameters.AddWithValue("@id", updated.InvoiceId);
                                        cmd.Parameters.AddWithValue("@name", updated.Customer);
                                        cmd.Parameters.AddWithValue("@amount", updated.Amount);
                                        cmd.Parameters.AddWithValue("@methodID", GetMethodID(MethodToVN(updated.Method)));
                                        cmd.Parameters.AddWithValue("@statusID", GetStatusID(StatusToString(updated.Status)));
                                        cmd.ExecuteNonQuery();
                                    }

                                    // Đồng bộ bảng Invoices (schema: InvoiceDate, TotalAmount, PaymentStatus)
                                    string sqlInv = @"UPDATE Invoices 
                                                      SET TotalAmount = @amount, 
                                                          PaymentStatus = @statusName, 
                                                          InvoiceDate = @date
                                                      WHERE InvoiceCode = @id";
                                    using (SqlCommand cmdInv = new SqlCommand(sqlInv, conn, trans))
                                    {
                                        cmdInv.Parameters.AddWithValue("@id", updated.InvoiceId);
                                        cmdInv.Parameters.AddWithValue("@amount", updated.Amount);
                                        cmdInv.Parameters.AddWithValue("@statusName", StatusToString(updated.Status));
                                        cmdInv.Parameters.AddWithValue("@date", updated.Date);
                                        cmdInv.ExecuteNonQuery();
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

                        // 5. CẬP NHẬT LẠI GIAO DIỆN
                        LoadDataFromDB(); // Kéo dữ liệu mới nhất từ SQL về
                        _transactions = new List<Transaction>(_allTransactions); // Cập nhật list hiển thị
                        LoadTransactionsToGrid(_transactions); // Vẽ lại bảng
                        UpdateSummary(); // Tính lại tổng tiền Dashboard
                        LoadChartData(); // Vẽ lại biểu đồ tròn

                        CustomMessageBox.Show("Cập nhật hóa đơn thành công!", this, CustomMessageBox.BoxType.Success);
                        try { NotificationCenter.RaiseInvoiceChanged(); } catch { }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi cập nhật Database: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
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
                dtpDate = new DateTimePicker { Location = new Point(12, 148), Width = 200, Format = DateTimePickerFormat.Custom, CustomFormat = "dd/MM/yyyy", ShowUpDown = true };



                var lblAmount = new Label { Text = "Số tiền", Location = new Point(12, 182), AutoSize = true };

                numAmount = new NumericUpDown { Location = new Point(12, 204), Width = 200, Maximum = 1000000000, DecimalPlaces = 0, Increment = 1000 };



                var lblStatus = new Label { Text = "Trạng thái", Location = new Point(230, 126), AutoSize = true };

                cboStatus = new ComboBox { Location = new Point(230, 148), Width = 162, DropDownStyle = ComboBoxStyle.DropDownList };

                // Vietnamese items

                cboStatus.Items.AddRange(new[] { "Hoàn thành", "Đang chờ", "Không thành công" });

                cboStatus.SelectedIndex = 0;



                var lblMethod = new Label { Text = "Phương Thức thanh toán", Location = new Point(230, 182), AutoSize = true };

                cboMethod = new ComboBox { Location = new Point(230, 204), Width = 162, DropDownStyle = ComboBoxStyle.DropDownList };

                cboMethod.Items.AddRange(new[] { "Tiền mặt", "Thẻ", "MoMo", "Khác" });

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

                if (string.IsNullOrWhiteSpace(txtCustomer.Text)) { MessageBox.Show("Khách hàng không được để trống."); this.DialogResult = DialogResult.None; return;}



                var status = VNToStatus(cboStatus.SelectedItem.ToString());

                var method = VNToMethod(cboMethod.SelectedItem.ToString());



                Result = new Transaction(txtId.Text.Trim(), txtCustomer.Text.Trim(), dtpDate.Value, (double)numAmount.Value, status, method);

            }



            // helpers for VN <-> enum

            private static string StatusToVN(TransactionStatus s)

            {

                switch (s)

                {

                    case TransactionStatus.Success: return "Hoàn thành";

                    case TransactionStatus.Pending: return "Đang chờ";

                    case TransactionStatus.Failed: return "Không thành công";
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

                    case PaymentMethod.MoMo: return "MoMo";


                    case PaymentMethod.Other: return "Khác";

                    default: return m.ToString();

                }

            }



            private static PaymentMethod VNToMethod(String vn)

            {

                switch (vn)

                {

                    case "Tiền mặt": return PaymentMethod.Cash;

                    case "Thẻ": return PaymentMethod.Card;

                    case "MoMo": return PaymentMethod.MoMo;


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

                case PaymentMethod.MoMo: return "MoMo";



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

        private enum PaymentMethod { Cash, Card, MoMo, Other }



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



        // Reflection-safe helper to neutralize Guna2 button hover/pressed visuals.

        // Kept minimal and defensive so it compiles even if Guna2 internals vary.

        private void SyncGunaButtonStates(Control c)

        {

            if (c == null) return;



            try

            {

                var ctrlType = c.GetType();

                var full = ctrlType.FullName ?? string.Empty;

                if (!full.Contains("Guna.UI2.WinForms.Guna2Button")) return;



                var fillProp = ctrlType.GetProperty("FillColor");

                var foreProp = ctrlType.GetProperty("ForeColor");

                var borderProp = ctrlType.GetProperty("BorderColor");



                var fillVal = fillProp != null ? fillProp.GetValue(c) : null;

                var foreVal = foreProp != null ? foreProp.GetValue(c) : null;

                var borderVal = borderProp != null ? borderProp.GetValue(c) : null;



                // sync HoverState / PressedState / CheckedState if present
                foreach (var stateName in new[] { "HoverState", "PressedState", "CheckedState" })

                {

                    try

                    {

                        var stateProp = ctrlType.GetProperty(stateName);

                        if (stateProp == null) continue;

                        var stateObj = stateProp.GetValue(c);

                        if (stateObj == null) continue;



                        var stType = stateObj.GetType();

                        stType.GetProperty("FillColor")?.SetValue(stateObj, fillVal);

                        stType.GetProperty("ForeColor")?.SetValue(stateObj, foreVal);

                        stType.GetProperty("BorderColor")?.SetValue(stateObj, borderVal);

                    }

                    catch { /* ignore per-state failures */ }

                }



                // PressedColor fallback

                try

                {

                    var pressedColorProp = ctrlType.GetProperty("PressedColor");

                    if (pressedColorProp != null && fillVal != null)

                    {

                        pressedColorProp.SetValue(c, fillVal);

                    }

                }

                catch { }

            }

            catch { }

        }



        private void DisableHoverForControls(params Control[] controls)

        {

            if (controls == null) return;



            foreach (var c in controls)

            {

                if (c == null) continue;



                try

                {

                    if (!_noHoverStates.ContainsKey(c))

                        _noHoverStates[c] = (c.BackColor, c.ForeColor, c.Cursor);



                    // detach then attach to avoid duplicate handlers

                    c.MouseEnter -= Control_NoHover_MouseEnter;

                    c.MouseMove -= Control_NoHover_MouseEnter;

                    c.MouseLeave -= Control_NoHover_MouseLeave;



                    c.MouseEnter += Control_NoHover_MouseEnter;

                    c.MouseMove += Control_NoHover_MouseEnter;

                    c.MouseLeave += Control_NoHover_MouseLeave;



                    try { c.Cursor = Cursors.Default; } catch { }



                    // Try Guna2-specific neutralization when available

                    try

                    {

                        if (c is Guna2Button g2b)

                        {

                            DisableGuna2ButtonVisuals(g2b);

                        }

                        else

                        {

                            // best-effort reflection for other Guna-like controls

                            SyncGunaButtonStates(c);

                        }

                    }

                    catch { /* ignore Guna-specific failures */ }



                    // For standard WinForms Button minimize hover/press visuals

                    if (c is Button btn)

                    {

                        try
                        {

                            btn.FlatStyle = FlatStyle.Flat;

                            btn.FlatAppearance.MouseOverBackColor = btn.BackColor;

                            btn.FlatAppearance.MouseDownBackColor = btn.BackColor;

                            btn.FlatAppearance.BorderSize = 0;

                            btn.UseVisualStyleBackColor = false;

                        }

                        catch { }

                    }

                }

                catch { /* ignore per-control issues */ }

            }

        }



        private void DisableGuna2ButtonVisuals(Guna2Button b)

        {

            if (b == null) return;



            try

            {

                // best-effort direct API (wrapped in try to avoid compile/runtime mismatch)

                try { b.Animated = false; } catch { }



                try

                {

                    var fill = b.FillColor;

                    var fore = b.ForeColor;

                    var border = b.BorderColor;



                    // HoverState

                    try

                    {

                        var hover = b.GetType().GetProperty("HoverState")?.GetValue(b);

                        if (hover != null)

                        {

                            hover.GetType().GetProperty("FillColor")?.SetValue(hover, fill);

                            hover.GetType().GetProperty("ForeColor")?.SetValue(hover, fore);

                            hover.GetType().GetProperty("BorderColor")?.SetValue(hover, border);

                        }

                    }

                    catch { }



                    // PressedState / PressedColor

                    try

                    {

                        var pressed = b.GetType().GetProperty("PressedState")?.GetValue(b);

                        if (pressed != null)

                        {

                            pressed.GetType().GetProperty("FillColor")?.SetValue(pressed, fill);

                            pressed.GetType().GetProperty("ForeColor")?.SetValue(pressed, fore);

                            pressed.GetType().GetProperty("BorderColor")?.SetValue(pressed, border);

                        }



                        var pressedColorProp = b.GetType().GetProperty("PressedColor");

                        if (pressedColorProp != null) pressedColorProp.SetValue(b, fill);

                    }

                    catch { }



                    // CheckedState

                    try

                    {

                        var chk = b.GetType().GetProperty("CheckedState")?.GetValue(b);

                        if (chk != null)

                        {

                            chk.GetType().GetProperty("FillColor")?.SetValue(chk, fill);

                            chk.GetType().GetProperty("ForeColor")?.SetValue(chk, fore);

                            chk.GetType().GetProperty("BorderColor")?.SetValue(chk, border);
                        }

                    }

                    catch { }

                }

                catch { }

            }

            catch { }

        }



        // Add these methods inside the FormPayments class (near other handlers).

        // They match the names used when attaching events in DisableHoverForControls

        // and satisfy Designer wiring for chart and grid.



        private void Control_NoHover_MouseEnter(object sender, EventArgs e)

        {

            var c = sender as Control;

            if (c == null) return;



            try

            {

                if (_noHoverStates.TryGetValue(c, out var orig))

                {

                    c.BackColor = orig.Back;

                    c.ForeColor = orig.Fore;

                    try { c.Cursor = orig.Cursor; } catch { }

                    c.Invalidate();

                }



                // Best-effort: neutralize Guna2 visual states for this control

                try { SyncGunaButtonStates(c); } catch { }

            }

            catch { }

        }



        private void Control_NoHover_MouseLeave(object sender, EventArgs e)

        {

            var c = sender as Control;

            if (c == null) return;



            try

            {

                if (_noHoverStates.TryGetValue(c, out var orig))

                {

                    c.BackColor = orig.Back;

                    c.ForeColor = orig.Fore;

                    try { c.Cursor = orig.Cursor; } catch { }

                    c.Invalidate();

                }



                // Restore or re-sync Guna2 internal states if needed

                try { SyncGunaButtonStates(c); } catch { }

            }

            catch { }

        }



        // Designer no-op handlers (names must match Designer wiring)

        private void chart2_Click(object sender, EventArgs e)

        {

            // Intentionally empty — Chart2_MouseClick handles mouse clicks with position.

        }



        private void dgvTransaction_CellContentClick(object sender, DataGridViewCellEventArgs e)

        {

            // Intentionally empty - keep Designer wiring stable.

        }



        private void btnDownload_Click_1(object sender, EventArgs e)

        {



        }

        private List<Transaction> GetMockData()
        {
            var today = DateTime.Today;
            return new List<Transaction>
            {
                //new Transaction("INV-001", "Nguyễn Văn A", today.AddDays(-18), 1250000, TransactionStatus.Success, PaymentMethod.Cash),
                //new Transaction("INV-002", "Trần Thị B", today.AddDays(-16), 450000, TransactionStatus.Failed, PaymentMethod.Card),
                //new Transaction("INV-003", "Lê Văn C", today.AddDays(-10), 230000, TransactionStatus.Pending, PaymentMethod.MoMo),
                //new Transaction("INV-004", "Hứa Mỹ Lam", today.AddDays(-2), 12345000, TransactionStatus.Success, PaymentMethod.Other),
                //new Transaction("INV-005", "Hứa Gia Tường", today.AddDays(-6), 10000000, TransactionStatus.Pending, PaymentMethod.Card)
            };
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvTransaction.SelectedRows.Count == 0)
            {
                CustomMessageBox.Show("Vui lòng chọn 1 giao dịch để xóa.", this, CustomMessageBox.BoxType.Warning);
                return;
            }

            var row = dgvTransaction.SelectedRows[0];
            string id = row.Cells["dgvInvoiceID"].Value.ToString();

            if (MessageBox.Show($"Bạn có chắc muốn xóa hóa đơn {id}?",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(_connectionString))
                    {
                        conn.Open();
                        using (var trans = conn.BeginTransaction())
                        {
                            try
                            {
                                using (SqlCommand cmd = new SqlCommand("DELETE FROM Transactions WHERE InvoiceID = @id", conn, trans))
                                {
                                    cmd.Parameters.AddWithValue("@id", id);
                                    cmd.ExecuteNonQuery();
                                }

                                using (SqlCommand cmdInv = new SqlCommand("DELETE FROM Invoices WHERE InvoiceCode = @id", conn, trans))
                                {
                                    cmdInv.Parameters.AddWithValue("@id", id);
                                    cmdInv.ExecuteNonQuery();
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

                    // --- ĐOẠN NÀY QUAN TRỌNG ĐỂ FORM CẬP NHẬT ---
                    LoadDataFromDB(); // 1. Lấy lại dữ liệu mới từ SQL
                    _transactions = new List<Transaction>(_allTransactions); // 2. Cập nhật list tạm
                    LoadTransactionsToGrid(_transactions); // 3. Vẽ lại cái bảng (Dòng bị xóa sẽ mất)
                    UpdateSummary(); // 4. Tính lại tổng tiền trên mấy cái ô dashboard

                    CustomMessageBox.Show("Xóa thành công!", this);
                    try { NotificationCenter.RaiseInvoiceChanged(); } catch { }
                }
                catch (Exception ex) { MessageBox.Show("Lỗi: " + ex.Message); }
            }
        }

        public class CustomMessageBox : Form
        {
            public enum BoxType { Success, Error, Info, Warning }

            private readonly Label _lblMessage;
            private readonly Button _btnOk;
            private readonly Label _lblIcon;
            private readonly Panel _topAccent;

            public CustomMessageBox(string message, BoxType type = BoxType.Success)
            {
                // Form basics
                this.FormBorderStyle = FormBorderStyle.None;
                this.TopMost = true;
                this.StartPosition = FormStartPosition.CenterParent;
                this.BackColor = Color.White;
                this.ClientSize = new Size(440, 180);
                this.MaximizeBox = false;
                this.MinimizeBox = false;

                // Rounded corners
                this.Load += (s, e) => { ApplyRoundedCorners(10); };

                // Top accent strip
                _topAccent = new Panel
                {
                    Dock = DockStyle.Top,
                    Height = 8,
                    BackColor = GetAccentColor(type)
                };
                this.Controls.Add(_topAccent);

                // Icon label (large glyph)
                _lblIcon = new Label
                {
                    AutoSize = false,
                    Size = new Size(72, 72),
                    Location = new Point((this.ClientSize.Width - 72) / 2, 14),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI Symbol", 28F, FontStyle.Bold),
                    ForeColor = GetAccentColor(type),
                    BackColor = Color.Transparent
                };
                _lblIcon.Text = GetSymbol(type);
                this.Controls.Add(_lblIcon);

                // Message
                _lblMessage = new Label
                {
                    AutoSize = false,
                    Width = this.ClientSize.Width - 36,
                    Height = 40,
                    Location = new Point(18, 96),
                    Text = message,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(0, 102, 51),
                    BackColor = Color.Transparent
                };

                // Adjust message color for non-success types
                if (type == BoxType.Error) _lblMessage.ForeColor = Color.FromArgb(153, 0, 0);
                else if (type == BoxType.Info) _lblMessage.ForeColor = Color.FromArgb(3, 35, 140);
                else if (type == BoxType.Warning) _lblMessage.ForeColor = Color.FromArgb(191, 87, 0);


                this.Controls.Add(_lblMessage);

                // OK button
                _btnOk = new Button
                {
                    Text = "OK",
                    Size = new Size(110, 38),
                    Location = new Point((this.ClientSize.Width - 110) / 2, this.ClientSize.Height - 52),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = GetAccentColor(type),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                    DialogResult = DialogResult.OK
                };
                _btnOk.FlatAppearance.BorderSize = 0;
                _btnOk.Click += (s, e) => { this.DialogResult = DialogResult.OK; this.Close(); };
                _btnOk.MouseDown += (s, e) => { this.DialogResult = DialogResult.OK; this.Close(); };
                _btnOk.MouseUp += (s, e) => { this.DialogResult = DialogResult.OK; this.Close(); };
                 this.Controls.Add(_btnOk);

                // Accessibility / keyboard
                this.AcceptButton = _btnOk;
                this.KeyPreview = true;
                this.KeyDown += (s, e) => { if (e.KeyCode == Keys.Escape) this.Close(); };
                // ensure first click triggers immediately
                this.Shown += (s, e) =>
                {
                    try
                    {
                        this.Activate();
                        this.BringToFront();
                        _btnOk.Select();
                        _btnOk.Focus();
                        this.ActiveControl = _btnOk;
                    }
                    catch { }
                };

                // Close on any mouse down inside dialog (covers rare focus issues)
                this.MouseDown += (s, e) => { this.DialogResult = DialogResult.OK; this.Close(); };
                _lblIcon.MouseDown += (s, e) => { this.DialogResult = DialogResult.OK; this.Close(); };
                _lblMessage.MouseDown += (s, e) => { this.DialogResult = DialogResult.OK; this.Close(); };
                _topAccent.MouseDown += (s, e) => { this.DialogResult = DialogResult.OK; this.Close(); };


                // Make small shadow effect via border (subtle)
                this.Paint += (s, e) =>
                {
                    using (var pen = new Pen(Color.FromArgb(220, 220, 220)))
                    {
                        var rect = this.ClientRectangle;
                        rect.Width -= 1;
                        rect.Height -= 1;
                        e.Graphics.DrawRectangle(pen, rect);
                    }
                };
            }

            private void ApplyRoundedCorners(int radius)
            {
                using (var path = new System.Drawing.Drawing2D.GraphicsPath())
                {
                    var rect = new Rectangle(0, 0, this.Width, this.Height);
                    int d = radius * 2;
                    path.AddArc(rect.Left, rect.Top, d, d, 180, 90);
                    path.AddArc(rect.Right - d, rect.Top, d, d, 270, 90);
                    path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
                    path.AddArc(rect.Left, rect.Bottom - d, d, d, 90, 90);
                    path.CloseAllFigures();
                    this.Region = new Region(path);
                }
            }

            private static string GetSymbol(BoxType t)
            {
                switch (t)
                {
                    case BoxType.Success: return "✔";
                    case BoxType.Error: return "✖";
                    case BoxType.Info: return "ℹ";
                    case BoxType.Warning: return "!";
                    default: return "✔";
                }
            }

            private static Color GetAccentColor(BoxType t)
            {
                switch (t)
                {
                    case BoxType.Success: return Color.FromArgb(3, 35, 140); // deep blue accent (button color)
                    case BoxType.Error: return Color.FromArgb(194, 57, 52);
                    case BoxType.Info: return Color.FromArgb(0, 123, 255);
                    case BoxType.Warning: return Color.FromArgb(255, 140, 0); // new orange tone
                    default: return Color.FromArgb(3, 35, 140);
                }
            }

            // Preserve existing call-site: CustomMessageBox.Show(message, parent)
            public static void Show(string message, Form parent)
            {
                using (var box = new CustomMessageBox(message, BoxType.Success))
                {
                    box.ShowDialog(parent);
                }
            }

            // Optional overloads for type-specific calls:
            public static void Show(string message, Form parent, BoxType type)
            {
                using (var box = new CustomMessageBox(message, type))
                {
                    box.ShowDialog(parent);
                }
            }
        }
        private int GetMethodID(string methodName)
        {
            if (methodName.Contains("MoMo")) return 1;
            if (methodName.Contains("Thẻ")) return 2;
            if (methodName.Contains("Tiền mặt")) return 3;
            return 4; // Khác
        }

        private int GetStatusID(string statusName)
        {
            if (statusName.Contains("Hoàn thành")) return 1;
            if (statusName.Contains("Đang chờ")) return 2;
            return 3; // Không thành công
        }

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
                        _transactions = new List<Transaction>(_allTransactions);
                        LoadTransactionsToGrid(_transactions);
                        UpdateSummary();
                        LoadChartData();
                    }
                    catch { }
                }));
            }
            catch { }
        }

        private void FormPayments_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                // Hủy đăng ký sự kiện để tránh lỗi khi form đã đóng
                NotificationCenter.InvoiceChanged -= NotificationCenter_InvoiceChanged;
            }
            catch { }
        }

        private void FormPayments_Load_1(object sender, EventArgs e)
        {

        }
    }

}

