using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;

namespace QLChuoiNhaHangKhachSan.GUI
{
    public partial class frmBill : Form
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
        private readonly int _orderId;
        private readonly string _tableName;
        private string _customerEmail;
        private readonly PrintDocument _printDocument;
        private Bitmap _billBitmap;

        // Thông tin giảm giá
        private decimal _discountPercent;
        private decimal _discountAmount;
        private decimal _finalAmount;
        private string _appliedPromoCode;

        public frmBill()
        {
            InitializeComponent();

            _printDocument = new PrintDocument();
            _printDocument.PrintPage += PrintDocument_PrintPage;

            btnPrint.Click += guna2Button1_Click; // In hóa đơn
            guna2Button3.Click += guna2Button3_Click; // Thanh toán khi trả phòng (ghi nhận chưa thu tiền)
            this.Load += frmBill_Load;
        }

        public frmBill(int orderId, string tableName = null) : this()
        {
            _orderId = orderId;
            _tableName = tableName;
        }

        /// <summary>
        /// Thiết lập thông tin giảm giá để hiển thị trên hóa đơn
        /// </summary>
        /// <param name="discountPercent">Phần trăm giảm giá (0-100)</param>
        /// <param name="discountAmount">Số tiền được giảm</param>
        /// <param name="finalAmount">Tổng tiền sau giảm</param>
        /// <param name="promoCode">Mã giảm giá đã áp dụng</param>
        public void SetDiscountInfo(decimal discountPercent, decimal discountAmount, decimal finalAmount, string promoCode)
        {
            _discountPercent = discountPercent;
            _discountAmount = discountAmount;
            _finalAmount = finalAmount;
            _appliedPromoCode = promoCode;
        }

        private void guna2TextBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2HtmlLabel2_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmBill_Load(object sender, EventArgs e)
        {
            LoadBillData();
            ApplyDiscountDisplay();
        }

        /// <summary>
        /// Áp dụng hiển thị thông tin giảm giá lên giao diện
        /// </summary>
        private void ApplyDiscountDisplay()
        {
            if (_discountAmount > 0 && !string.IsNullOrEmpty(_appliedPromoCode))
            {
                // Cập nhật tổng tiền hiển thị
                txtTotalAmount.Text = _finalAmount.ToString("N0", CultureInfo.InvariantCulture);

                // Thêm dòng giảm giá vào danh sách món (nếu cần hiển thị chi tiết)
                // Hoặc có thể tạo label riêng để hiển thị thông tin giảm giá
                try
                {
                    // Thêm dòng hiển thị giảm giá ở cuối danh sách
                    dvgDishList.Rows.Add(
                        $"Giảm giá ({_appliedPromoCode} - {_discountPercent}%)",
                        "",
                        "",
                        $"-{_discountAmount:N0}");
                }
                catch { }
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            try
            {
                CaptureBillPanel();
                SendBillEmail();

                using (var printDialog = new PrintDialog())
                {
                    printDialog.Document = _printDocument;
                    if (printDialog.ShowDialog(this) == DialogResult.OK)
                    {
                        _printDocument.Print();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể in hóa đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CaptureBillPanel()
        {
            _billBitmap?.Dispose();
            _billBitmap = new Bitmap(pnlBill.Width, pnlBill.Height);
            pnlBill.DrawToBitmap(_billBitmap, new Rectangle(0, 0, pnlBill.Width, pnlBill.Height));
        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            if (_billBitmap == null)
            {
                CaptureBillPanel();
            }

            if (_billBitmap != null)
            {
                e.Graphics.DrawImage(_billBitmap, new Point(0, 0));
            }
        }

        private void LoadBillData()
        {
            if (_orderId <= 0) return;

            dvgDishList.Rows.Clear();

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    // Lấy thông tin chung của hóa đơn
                    using (var cmdOrder = new SqlCommand(
                        @"SELECT o.OrderId, o.DateCheckIn, o.SubTotal, o.Discount, o.Surcharge, o.TotalAmount,
                                 c.FullName AS CustomerName, c.Email, rt.TableName
                          FROM OrderTicket o
                          LEFT JOIN Customers c ON o.CustomerID = c.CustomerId
                          LEFT JOIN RestaurantTable rt ON o.TableID = rt.TableID
                          WHERE o.OrderId = @OrderId", conn))
                    {
                        cmdOrder.Parameters.AddWithValue("@OrderId", _orderId);

                        using (var reader = cmdOrder.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtBillCode.Text = _orderId.ToString(CultureInfo.InvariantCulture);
                                txtCustomer.Text = reader["CustomerName"] == DBNull.Value
                                    ? string.Empty
                                    : reader["CustomerName"].ToString();

                                _customerEmail = reader["Email"] == DBNull.Value
                                    ? null
                                    : reader["Email"].ToString();

                                string tableName = reader["TableName"] == DBNull.Value ? string.Empty : reader["TableName"].ToString();
                                txtTableCode.Text = string.IsNullOrWhiteSpace(_tableName) ? tableName : _tableName;

                                DateTime dateCheckIn = reader["DateCheckIn"] == DBNull.Value
                                    ? DateTime.Now
                                    : Convert.ToDateTime(reader["DateCheckIn"], CultureInfo.InvariantCulture);
                                txtDay.Text = dateCheckIn.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

                                decimal total = reader["TotalAmount"] == DBNull.Value
                                    ? 0m
                                    : Convert.ToDecimal(reader["TotalAmount"], CultureInfo.InvariantCulture);
                                txtTotalAmount.Text = total.ToString("N0", CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                MessageBox.Show("Không tìm thấy hóa đơn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                        }
                    }

                    // Lấy chi tiết món ăn
                    using (var cmdItems = new SqlCommand(
                        @"SELECT f.FoodName, i.Quantity, i.UnitPrice, i.LineTotal
                          FROM OrderTicketItem i
                          INNER JOIN Food f ON i.FoodID = f.FoodID
                          WHERE i.OrderId = @OrderId", conn))
                    {
                        cmdItems.Parameters.AddWithValue("@OrderId", _orderId);
                        using (var reader = cmdItems.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string foodName = reader["FoodName"].ToString();
                                int quantity = Convert.ToInt32(reader["Quantity"], CultureInfo.InvariantCulture);
                                decimal unitPrice = Convert.ToDecimal(reader["UnitPrice"], CultureInfo.InvariantCulture);
                                decimal lineTotal = Convert.ToDecimal(reader["LineTotal"], CultureInfo.InvariantCulture);

                                dvgDishList.Rows.Add(
                                    foodName,
                                    quantity,
                                    unitPrice.ToString("N0", CultureInfo.InvariantCulture),
                                    lineTotal.ToString("N0", CultureInfo.InvariantCulture));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu hóa đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SendBillEmail()
        {
            if (string.IsNullOrWhiteSpace(_customerEmail))
            {
                MessageBox.Show("Không có email khách hàng để gửi hóa đơn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (_billBitmap == null)
            {
                CaptureBillPanel();
            }

            try
            {
                using (var ms = new MemoryStream())
                {
                    _billBitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    ms.Position = 0;

                    using (var mail = new MailMessage())
                    {
                        mail.From = new MailAddress("lamtritua@gmail.com", "Chuỗi nhà hàng khách sạn Accor Hotels");
                        mail.To.Add(_customerEmail);
                        mail.Subject = "Hóa đơn";
                        mail.Body = "Quý khách vui lòng xem hóa đơn đính kèm. Cảm ơn quý khách đã sử dụng dịch vụ.";
                        mail.IsBodyHtml = false;

                        using (var attachment = new Attachment(ms, "HoaDon.png", "image/png"))
                        {
                            mail.Attachments.Add(attachment);

                            using (var smtp = new SmtpClient("smtp.gmail.com", 587))
                            {
                                smtp.EnableSsl = true;
                                smtp.Credentials = new NetworkCredential("lamtritua@gmail.com", "bfix gzdt yoeu yzdb");
                                smtp.Send(mail);
                            }
                        }
                    }
                }

                MessageBox.Show("Đã gửi hóa đơn đến email khách hàng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gửi email thất bại: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Ghi nhận hóa đơn ở trạng thái chưa thu tiền (trả sau), đồng bộ sang FormPayments.
        /// </summary>
        private void guna2Button3_Click(object sender, EventArgs e)
        {
            if (_orderId <= 0)
            {
                MessageBox.Show("Không xác định được OrderId để ghi nhận hóa đơn.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    using (var tran = conn.BeginTransaction())
                    {
                        try
                        {
                            int customerId = EnsureCustomerForOrder(conn, tran, _orderId);
                            decimal total = ParseAmount(txtTotalAmount.Text);

                            int invoiceId = EnsureUnpaidInvoice(conn, tran, customerId, _orderId, total);

                            // Làm sạch các giao dịch cũ để tránh hiển thị trạng thái Hoàn thành khi chưa thanh toán
                            using (var cmdDelTran = new SqlCommand("DELETE FROM Transactions WHERE InvoiceId = @InvoiceId", conn, tran))
                            {
                                cmdDelTran.Parameters.AddWithValue("@InvoiceId", invoiceId);
                                cmdDelTran.ExecuteNonQuery();
                            }

                            InsertPendingTransaction(conn, tran, invoiceId, total, _tableName);

                            tran.Commit();
                        }
                        catch
                        {
                            tran.Rollback();
                            throw;
                        }
                    }
                }

                MessageBox.Show("Đã ghi nhận hóa đơn chưa thu tiền.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                try { NotificationCenter.RaiseInvoiceChanged(); } catch { }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể ghi nhận hóa đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private decimal ParseAmount(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return 0m;
            var cleaned = new string(text.Where(c => char.IsDigit(c) || c == '-' || c == '.' || c == ',').ToArray());
            cleaned = cleaned.Replace(".", "").Replace(",", "");
            decimal.TryParse(cleaned, NumberStyles.Number, CultureInfo.InvariantCulture, out var v);
            return v;
        }

        private int EnsureCustomerForOrder(SqlConnection conn, SqlTransaction tran, int orderId)
        {
            // Lấy CustomerId từ OrderTicket nếu có
            using (var cmd = new SqlCommand("SELECT CustomerID FROM OrderTicket WHERE OrderId = @OrderId", conn, tran))
            {
                cmd.Parameters.AddWithValue("@OrderId", orderId);
                var obj = cmd.ExecuteScalar();
                if (obj != null && obj != DBNull.Value)
                {
                    return Convert.ToInt32(obj);
                }
            }

            // Không có khách -> tạo khách lẻ tạm
            using (var cmdNewCust = new SqlCommand(@"INSERT INTO Customers (FullName, CustomerType, CustomerCode, Source, CreatedAt)
OUTPUT INSERTED.CustomerId
VALUES (@FullName, @CustomerType, @CustomerCode, @Source, GETDATE());", conn, tran))
            {
                string tempCode = "NH" + (DateTime.UtcNow.Ticks % 1_000_000_000).ToString("D9");
                cmdNewCust.Parameters.AddWithValue("@FullName", string.IsNullOrWhiteSpace(_tableName) ? (object)"Khách lẻ" : (object)$"Khách bàn {_tableName}");
                cmdNewCust.Parameters.AddWithValue("@CustomerType", "NH_Thường");
                cmdNewCust.Parameters.AddWithValue("@CustomerCode", tempCode);
                cmdNewCust.Parameters.AddWithValue("@Source", "Nhà hàng");
                var obj = cmdNewCust.ExecuteScalar();
                return Convert.ToInt32(obj);
            }
        }

        private int EnsureUnpaidInvoice(SqlConnection conn, SqlTransaction tran, int customerId, int orderId, decimal total)
        {
            // Nếu đã có hóa đơn của Order này, cập nhật về trạng thái Unpaid và tổng tiền mới
            using (var cmdFind = new SqlCommand("SELECT TOP 1 InvoiceId FROM Invoices WHERE RestaurantOrderId = @OrderId ORDER BY InvoiceId DESC", conn, tran))
            {
                cmdFind.Parameters.AddWithValue("@OrderId", orderId);
                var obj = cmdFind.ExecuteScalar();
                if (obj != null && obj != DBNull.Value)
                {
                    int invId = Convert.ToInt32(obj);
                    using (var cmdUpd = new SqlCommand(@"UPDATE Invoices
SET CustomerId = @CustomerId,
    TotalAmount = @TotalAmount,
    InvoiceStatus = N'Unpaid',
    InvoiceDate = GETDATE()
WHERE InvoiceId = @InvoiceId", conn, tran))
                    {
                        cmdUpd.Parameters.AddWithValue("@CustomerId", customerId);
                        cmdUpd.Parameters.AddWithValue("@TotalAmount", total);
                        cmdUpd.Parameters.AddWithValue("@InvoiceId", invId);
                        cmdUpd.ExecuteNonQuery();
                    }
                    return invId;
                }
            }

            // Chưa có -> tạo mới
            const string sql = @"INSERT INTO Invoices (CustomerId, InvoiceType, RestaurantOrderId, TotalAmount, InvoiceStatus, InvoiceDate)
VALUES (@CustomerId, 'Restaurant', @OrderId, @TotalAmount, N'Unpaid', GETDATE());
SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using (var cmd = new SqlCommand(sql, conn, tran))
            {
                cmd.Parameters.AddWithValue("@CustomerId", customerId);
                cmd.Parameters.AddWithValue("@OrderId", orderId);
                cmd.Parameters.AddWithValue("@TotalAmount", total);
                var obj = cmd.ExecuteScalar();
                int newId = obj != null ? Convert.ToInt32(obj) : 0;

                // Xóa các hóa đơn cũ (nếu có) của cùng Order, giữ lại hóa đơn vừa tạo
                if (newId > 0)
                {
                    using (var cmdDel = new SqlCommand(@"DELETE FROM Invoices 
WHERE RestaurantOrderId = @OrderId AND InvoiceId <> @KeepId", conn, tran))
                    {
                        cmdDel.Parameters.AddWithValue("@OrderId", orderId);
                        cmdDel.Parameters.AddWithValue("@KeepId", newId);
                        cmdDel.ExecuteNonQuery();
                    }
                }

                return newId;
            }
        }

        private void InsertPendingTransaction(SqlConnection conn, SqlTransaction tran, int invoiceId, decimal total, string note)
        {
            int statusId = GetTransactionStatusId(conn, tran, "Đang chờ", 2);
            int methodId = 1; // Tiền mặt mặc định

            using (var cmd = new SqlCommand(@"INSERT INTO Transactions (InvoiceId, PaymentDate, Amount, MethodID, StatusID, Note)
VALUES (@InvoiceId, GETDATE(), @Amount, @MethodID, @StatusID, @Note);", conn, tran))
            {
                cmd.Parameters.AddWithValue("@InvoiceId", invoiceId);
                cmd.Parameters.AddWithValue("@Amount", total);
                cmd.Parameters.AddWithValue("@MethodID", methodId);
                cmd.Parameters.AddWithValue("@StatusID", statusId);
                cmd.Parameters.AddWithValue("@Note", string.IsNullOrWhiteSpace(note) ? (object)DBNull.Value : (object)$"Bàn {note} - trả sau");
                cmd.ExecuteNonQuery();
            }
        }

        private int GetTransactionStatusId(SqlConnection conn, SqlTransaction tran, string statusName, int fallback)
        {
            using (var cmd = new SqlCommand("SELECT TOP 1 StatusID FROM TransactionStatus WHERE StatusName = @Name", conn, tran))
            {
                cmd.Parameters.AddWithValue("@Name", statusName);
                var obj = cmd.ExecuteScalar();
                if (obj != null && obj != DBNull.Value)
                {
                    return Convert.ToInt32(obj);
                }
            }
            return fallback;
        }

        /// <summary>
        /// Populates the bill form with invoice data from FormInvoiceManagement.
        /// </summary>
        /// <param name="invoiceId">Invoice ID</param>
        /// <param name="customer">Customer name</param>
        /// <param name="date">Invoice date</param>
        /// <param name="amount">Total amount</param>
        /// <param name="status">Invoice status</param>
        /// <param name="payment">Payment method</param>
        /// <param name="items">List of line items (name, quantity, unit price, total)</param>
        public void PopulateFromInvoice(string invoiceId, string customer, string date, string amount, string status, string payment, List<Tuple<string, int, decimal, decimal>> items)
        {
            try
            {
                // Populate basic invoice information
                txtBillCode.Text = invoiceId ?? string.Empty;
                txtCustomer.Text = customer ?? string.Empty;
                txtDay.Text = date ?? string.Empty;
                txtTotalAmount.Text = amount ?? string.Empty;
                txtTableCode.Text = payment ?? string.Empty; // Reusing table code field for payment method

                // Clear and populate items grid
                dvgDishList.Rows.Clear();
                if (items != null)
                {
                    foreach (var item in items)
                    {
                        string itemName = item.Item1;
                        int quantity = item.Item2;
                        decimal unitPrice = item.Item3;
                        decimal lineTotal = item.Item4;

                        dvgDishList.Rows.Add(
                            itemName,
                            quantity,
                            unitPrice.ToString("N0", CultureInfo.InvariantCulture),
                            lineTotal.ToString("N0", CultureInfo.InvariantCulture));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi điền dữ liệu hóa đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}