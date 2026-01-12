using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;

namespace QLChuoiNhaHangKhachSan.GUI
{
    public partial class frmBill : Form
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["QuanLyChuoiNhaHangKhachSan"].ConnectionString;
        private readonly int _orderId;
        private readonly string _tableName;
        private string _customerEmail;
        private readonly PrintDocument _printDocument;
        private Bitmap _billBitmap;

        public frmBill()
        {
            InitializeComponent();

            _printDocument = new PrintDocument();
            _printDocument.PrintPage += PrintDocument_PrintPage;

            btnPrint.Click += guna2Button1_Click; // In hóa đơn
            this.Load += frmBill_Load;
        }

        public frmBill(int orderId, string tableName = null) : this()
        {
            _orderId = orderId;
            _tableName = tableName;
        }

        private void guna2TextBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2HtmlLabel2_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {

        }

        private void frmBill_Load(object sender, EventArgs e)
        {
            LoadBillData();
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
    }
}