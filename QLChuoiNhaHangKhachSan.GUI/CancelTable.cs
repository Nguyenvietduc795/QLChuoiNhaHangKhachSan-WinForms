using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;

namespace QLChuoiNhaHangKhachSan.GUI
{
    public partial class frmCancelTable : Form
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
        private Bitmap _billBitmap;
        private int _tableId;

        public frmCancelTable()
        {
            InitializeComponent();

            Load += frmCancelTable_Load;
            btnCancel.Click += btnCancel_Click;
            btnExit.Click += btnExit_Click;
        }

        private void frmCancelTable_Load(object sender, EventArgs e)
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
                                                WHERE b.Status = N'Đã đặt'
                                                ORDER BY b.BookingID DESC", conn))
                {
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtStick.Text = reader["BookingID"].ToString();
                            _tableId = Convert.ToInt32(reader["TableID"]);
                            var tableName = reader["TableName"] as string;
                            txtCodeTable.Text = string.IsNullOrWhiteSpace(tableName) ? _tableId.ToString() : tableName;

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
                            MessageBox.Show("Không có dữ liệu đặt bàn đang chờ hủy.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải thông tin đặt bàn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                if (_tableId == 0)
                {
                    MessageBox.Show("Không có thông tin bàn để hủy.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                CaptureBill();
                SendCancelEmail();
                UpdateTableToEmpty();
                MessageBox.Show("Đã hủy đặt bàn và thông báo qua email.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi hủy đặt bàn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void CaptureBill()
        {
            _billBitmap?.Dispose();
            _billBitmap = new Bitmap(pnlBill.Width, pnlBill.Height);
            pnlBill.DrawToBitmap(_billBitmap, new Rectangle(0, 0, pnlBill.Width, pnlBill.Height));
        }

        private void UpdateTableToEmpty()
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    // Cập nhật trạng thái booking thành Hủy
                    using (var cmdUpdateBooking = new SqlCommand(
                        "UPDATE BookingsTable SET Status = N'Đã hủy' WHERE TableID = @TableID AND Status = N'Đã đặt'", conn))
                    {
                        cmdUpdateBooking.Parameters.AddWithValue("@TableID", _tableId);
                        cmdUpdateBooking.ExecuteNonQuery();
                    }

                    // Cập nhật trạng thái bàn về trống (StatusID = 1)
                    using (var cmdUpdateTable = new SqlCommand(
                        "UPDATE RestaurantTable SET StatusID = 1 WHERE TableID = @TableID", conn))
                    {
                        cmdUpdateTable.Parameters.AddWithValue("@TableID", _tableId);
                        cmdUpdateTable.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi cập nhật trạng thái bàn: " + ex.Message);
            }
        }

        private void SendCancelEmail()
        {
            var recipient = txtEmail.Text?.Trim();
            if (string.IsNullOrEmpty(recipient))
            {
                MessageBox.Show("Không có email khách hàng để thông báo hủy.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                        mail.Subject = "Thông báo hủy đặt bàn";
                        mail.Body = "Đặt bàn của quý khách đã được hủy theo yêu cầu. Thông tin chi tiết đính kèm.";
                        mail.IsBodyHtml = false;

                        using (var attachment = new Attachment(ms, "PhieuHuyDatBan.png", "image/png"))
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
            }
            catch (Exception ex)
            {
                throw new Exception("Gửi email hủy thất bại: " + ex.Message);
            }
        }

		// Removed extra load handler that closed the form immediately (assigned in designer previously)
    }
}

