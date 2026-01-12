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
    public partial class frmReservationForm : Form
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["QuanLyChuoiNhaHangKhachSan"].ConnectionString;
        private readonly PrintDocument _printDocument = new PrintDocument();
        private Bitmap _billBitmap;

        public frmReservationForm()
        {
            InitializeComponent();

            Load += frmReservationForm_Load;
            btnPrint.Click += btnPrint_Click;
            btnExit.Click += btnExit_Click;
            _printDocument.PrintPage += PrintDocument_PrintPage;
        }

        private void guna2TextBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void frmReservationForm_Load(object sender, EventArgs e)
        {
            LoadLatestBooking();
        }

        private void LoadLatestBooking()
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                using (var cmd = new SqlCommand(@"SELECT TOP 1 
                                                    b.BookingID,
                                                    b.TableID,
                                                    rt.TableName,
                                                    b.CustomerID,
                                                    c.FullName,
                                                    c.PhoneNumber,
                                                    c.Email,
                                                    b.BookingDate,
                                                    b.BookingTime,
                                                    b.GuestCount,
                                                    b.Status,
                                                    b.CreatedDate
                                                FROM BookingsTable b
                                                LEFT JOIN RestaurantTable rt ON b.TableID = rt.TableID
                                                LEFT JOIN Customers c ON b.CustomerID = c.CustomerId
                                                ORDER BY b.BookingID DESC", conn))
                {
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtStick.Text = reader["BookingID"].ToString(); // Mã đặt bàn
                            var tableName = reader["TableName"] as string;
                            var tableId = reader["TableID"].ToString();
                            txtCodeTable.Text = string.IsNullOrWhiteSpace(tableName) ? tableId : tableName; // Mã bàn

                            // Ngày/giờ đặt và ngày lập
                            DateTime bookingDate = reader.GetDateTime(reader.GetOrdinal("BookingDate"));
                            TimeSpan bookingTime = reader.GetTimeSpan(reader.GetOrdinal("BookingTime"));
                            DateTime? createdDate = reader["CreatedDate"] == DBNull.Value
                                ? (DateTime?)null
                                : reader.GetDateTime(reader.GetOrdinal("CreatedDate"));

                            txtDay.Text = bookingDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                            txtTime.Text = bookingDate.Date.Add(bookingTime).ToString("HH:mm", CultureInfo.InvariantCulture);
                            txtDayTable.Text = (createdDate ?? bookingDate.Date.Add(bookingTime)).ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

                            txtCustomer.Text = reader["FullName"] == DBNull.Value ? string.Empty : reader["FullName"].ToString();
                            txtPhone.Text = reader["PhoneNumber"] == DBNull.Value ? string.Empty : reader["PhoneNumber"].ToString();
                            txtEmail.Text = reader["Email"] == DBNull.Value ? string.Empty : reader["Email"].ToString();
                            txtNumber.Text = reader["GuestCount"].ToString();
                        }
                        else
                        {
                            MessageBox.Show("Không có dữ liệu đặt bàn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải thông tin đặt bàn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                CaptureBill();
                SendReservationEmail();

                using (var dlg = new PrintDialog())
                {
                    dlg.Document = _printDocument;
                    if (dlg.ShowDialog(this) == DialogResult.OK)
                    {
                        _printDocument.Print();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể in phiếu đặt bàn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            using (var cancelForm = new frmCancelTable())
            {
                cancelForm.StartPosition = FormStartPosition.CenterParent;
                cancelForm.ShowDialog(this);
            }
        }

        private void CaptureBill()
        {
            _billBitmap?.Dispose();
            _billBitmap = new Bitmap(pnlBill.Width, pnlBill.Height);
            pnlBill.DrawToBitmap(_billBitmap, new Rectangle(0, 0, pnlBill.Width, pnlBill.Height));
        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            if (_billBitmap == null)
            {
                CaptureBill();
            }

            if (_billBitmap != null)
            {
                e.Graphics.DrawImage(_billBitmap, new Point(0, 0));
            }
        }

        private void SendReservationEmail()
        {
            var recipient = txtEmail.Text?.Trim();
            if (string.IsNullOrEmpty(recipient))
            {
                MessageBox.Show("Không có email khách hàng để gửi phiếu đặt bàn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (_billBitmap == null)
            {
                CaptureBill();
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
                        mail.To.Add(recipient);
                        mail.Subject = "Phiếu đặt bàn của bạn";
                        mail.Body = "Quý khách vui lòng xem phiếu đặt bàn đính kèm. Cảm ơn quý khách";
                        mail.IsBodyHtml = false;

                        using (var attachment = new Attachment(ms, "PhieuDatBan.png", "image/png"))
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

                MessageBox.Show("Đã gửi phiếu đặt bàn đến email khách hàng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gửi email thất bại: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmReservationForm_Load_1(object sender, EventArgs e)
        {

        }
    }
}
