using System;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;

namespace QLChuoiNhaHangKhachSan.GUI
{
    public static class EmailHelper
    {
        // Read SMTP config from app.config appSettings. Set these keys in App.config:
        // smtpHost, smtpPort, smtpUser, smtpPass (optional), smtpFrom, smtpEnableSsl (true/false)
        // For security, smtpPass can be provided via environment variable SMTP_PASS.
        private static string GetSetting(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        private static SmtpClient CreateSmtpClient()
        {
            var host = GetSetting("smtpHost");
            int port = 25;
            int.TryParse(GetSetting("smtpPort"), out port);
            var user = GetSetting("smtpUser");

            // Try to read SMTP password from environment variable first (more secure)
            var pass = Environment.GetEnvironmentVariable("SMTP_PASS");
            if (string.IsNullOrEmpty(pass))
            {
                // fallback to appSettings if env var not set
                pass = GetSetting("smtpPass");
            }

            bool enableSsl = false;
            bool.TryParse(GetSetting("smtpEnableSsl"), out enableSsl);

            if (string.IsNullOrWhiteSpace(host))
            {
                Log("SMTP host is not configured. Please add 'smtpHost' to appSettings.");
                throw new InvalidOperationException("SMTP host is not configured. Please add 'smtpHost' to appSettings.");
            }

            var client = new SmtpClient(host, port)
            {
                EnableSsl = enableSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            if (!string.IsNullOrWhiteSpace(user))
            {
                if (!string.IsNullOrEmpty(pass))
                {
                    client.Credentials = new NetworkCredential(user, pass);
                }
                else
                {
                    // If no password provided, still set Credentials to default network credentials to avoid anonymous send attempts
                    client.Credentials = CredentialCache.DefaultNetworkCredentials;
                    Log("SMTP password not provided via environment variable or appSettings. Using default network credentials.");
                }
            }
            return client;
        }

        private static string GetFromAddress()
        {
            var from = GetSetting("smtpFrom");
            if (string.IsNullOrWhiteSpace(from))
            {
                Log("From address is not configured. Please add 'smtpFrom' to appSettings.");
                throw new InvalidOperationException("From address is not configured. Please add 'smtpFrom' to appSettings.");
            }
            return from;
        }

        public static void SendBookingConfirmation(QLChuoiNhaHangKhachSan.GUI.BookingRowInfo info)
        {
            try
            {
                if (info == null || string.IsNullOrWhiteSpace(info.Email)) return;

                string subject = $"Xác nhận đặt phòng - {info.Id}";
                var sb = new StringBuilder();
                sb.AppendLine("<!doctype html><html><head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\"/></head><body>");
                sb.AppendLine($"<p>Xin chào <strong>{info.Customer}</strong>,</p>");
                sb.AppendLine("<p>Cám ơn bạn đã đặt phòng tại khách sạn chúng tôi. Dưới đây là thông tin chi tiết đặt phòng của bạn:</p>");
                sb.AppendLine("<ul>");
                sb.AppendLine($"<li><strong>Số phiếu:</strong> {info.Id}</li>");
                sb.AppendLine($"<li><strong>Ngày lập:</strong> {info.Date}</li>");
                sb.AppendLine($"<li><strong>Phòng & Thời gian:</strong><br/>{info.Detail?.Replace("\r\n", "<br/>")}</li>");
                sb.AppendLine($"<li><strong>CCCD:</strong> {info.CCCD}</li>");
                sb.AppendLine($"<li><strong>SĐT:</strong> {info.SDT}</li>");
                sb.AppendLine($"<li><strong>Giới tính:</strong> {info.Gender}</li>");
                sb.AppendLine("</ul>");
                sb.AppendLine("<p>Qúy Khách Xin Vui Lòng Đến Trước 15p Để Làm Thủ Tục Nhận Phòng.</p>");
                sb.AppendLine("<p>Hân Hạnh Được Phục Vụ Quý Khách!</p>");
                sb.AppendLine("<p>Ảnh thông tin chi tiết được đính kèm bên dưới.</p>");
                sb.AppendLine("<p>Trân trọng,<br/>RH - Cần Thơ</p>");
                sb.AppendLine("</body></html>");

                using (var client = CreateSmtpClient())
                using (var msg = new MailMessage())
                {
                    msg.From = new MailAddress(GetFromAddress());
                    msg.To.Add(new MailAddress(info.Email));
                    msg.Subject = subject;

                    // Ensure UTF-8 encodings to support Vietnamese characters
                    msg.SubjectEncoding = Encoding.UTF8;
                    msg.BodyEncoding = Encoding.UTF8;
                    msg.HeadersEncoding = Encoding.UTF8;
                    msg.IsBodyHtml = true;

                    // set body and create alternate view with UTF-8
                    msg.Body = sb.ToString();
                    var av = AlternateView.CreateAlternateViewFromString(sb.ToString(), Encoding.UTF8, MediaTypeNames.Text.Html);
                    msg.AlternateViews.Add(av);

                    // attach generated image of booking detail
                    var imageStream = CreateBookingDetailImage(info);
                    if (imageStream != null)
                    {
                        imageStream.Position = 0;
                        var attach = new Attachment(imageStream, $"booking_{info.Id}.png", "image/png");
                        attach.NameEncoding = Encoding.UTF8;
                        attach.ContentType.CharSet = "utf-8";
                        // The stream will be disposed when MailMessage.Dispose is called
                        msg.Attachments.Add(attach);
                    }

                    client.Send(msg);
                }
            }
            catch (Exception ex)
            {
                Log(ex);
            }
        }

        public static void SendCancellationEmail(QLChuoiNhaHangKhachSan.GUI.BookingRowInfo info)
        {
            try
            {
                if (info == null || string.IsNullOrWhiteSpace(info.Email)) return;

                string subject = $"Xác nhận hủy đặt phòng - {info.Id}";
                var sb = new StringBuilder();
                sb.AppendLine("<!doctype html><html><head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\"/></head><body>");
                sb.AppendLine($"<p>Xin chào <strong>{info.Customer}</strong>,</p>");
                sb.AppendLine("<p>Đơn đặt phòng của bạn đã được hủy theo yêu cầu. Thông tin đặt phòng:</p>");
                sb.AppendLine("<ul>");

                sb.AppendLine($"<li><strong>Số phiếu:</strong> {info.Id}</li>");
                sb.AppendLine($"<li><strong>Ngày Hủy:</strong> {info.Date}</li>");
                sb.AppendLine($"<li><strong>Phòng & Thời gian:</strong><br/>{info.Detail?.Replace("\r\n", "<br/>")}</li>");
                sb.AppendLine("</ul>");
                if (info.IsCancelled && !string.IsNullOrWhiteSpace(info.CancellationReason))
                {
                    sb.AppendLine($"<p><strong>Lý do hủy:</strong> {info.CancellationReason}</p>");
                }
                sb.AppendLine("<p>Nếu bạn cần hỗ trợ thêm, vui lòng liên hệ chúng tôi. Trân trọng cảm ơn.</p>");
                sb.AppendLine("<p>Trân trọng,<br/>RH - Cần Thơ</p>");
                sb.AppendLine("</body></html>");

                using (var client = CreateSmtpClient())
                using (var msg = new MailMessage())
                {
                    msg.From = new MailAddress(GetFromAddress());
                    msg.To.Add(new MailAddress(info.Email));
                    msg.Subject = subject;

                    msg.SubjectEncoding = Encoding.UTF8;
                    msg.BodyEncoding = Encoding.UTF8;
                    msg.HeadersEncoding = Encoding.UTF8;
                    msg.IsBodyHtml = true;

                    msg.Body = sb.ToString();
                    var av = AlternateView.CreateAlternateViewFromString(sb.ToString(), Encoding.UTF8, MediaTypeNames.Text.Html);
                    msg.AlternateViews.Add(av);

                    var imageStream = CreateBookingDetailImage(info);
                    if (imageStream != null)
                    {
                        imageStream.Position = 0;
                        var attach = new Attachment(imageStream, $"booking_{info.Id}_canceled.png", "image/png");
                        attach.NameEncoding = Encoding.UTF8;
                        attach.ContentType.CharSet = "utf-8";
                        msg.Attachments.Add(attach);
                    }

                    client.Send(msg);
                }
            }
            catch (Exception ex)
            {
                Log(ex);
            }
        }

        // Create a simple PNG image containing booking info text. Returns a MemoryStream (caller must dispose when MailMessage disposes attachment)
        private static MemoryStream CreateBookingDetailImage(QLChuoiNhaHangKhachSan.GUI.BookingRowInfo info)
        {
            try
            {
                int width = 700;
                int height = 420;
                var bmp = new Bitmap(width, height);
                using (var g = Graphics.FromImage(bmp))
                {
                    g.Clear(Color.White);
                    var titleFont = new Font("Segoe UI", 16, FontStyle.Bold);
                    var headerFont = new Font("Segoe UI", 11, FontStyle.Bold);
                    var textFont = new Font("Segoe UI", 11, FontStyle.Regular);

                    int x = 20;
                    int y = 20;
                    g.DrawString("CHI TIẾT ĐẶT PHÒNG", titleFont, Brushes.DarkBlue, x, y);
                    y += 40;

                    void DrawLine(string label, string value)
                    {
                        g.DrawString(label, headerFont, Brushes.Black, x, y);
                        g.DrawString(value ?? "", textFont, Brushes.Black, x + 160, y);
                        y += 26;
                    }

                    DrawLine("Số phiếu:", info?.Id ?? "");
                    DrawLine("Tên khách:", info?.Customer ?? "");
                    DrawLine("Ngày lập:", info?.Date ?? "");
                    DrawLine("CCCD:", info?.CCCD ?? "");
                    DrawLine("SĐT:", info?.SDT ?? "");
                    DrawLine("Email:", info?.Email ?? "");
                    DrawLine("Giới tính:", info?.Gender ?? "");
                    DrawLine("Quốc tịch:", info?.Nationality ?? "");

                    y += 6;
                    g.DrawString("Phòng & Thời gian:", headerFont, Brushes.Black, x, y);
                    y += 24;

                    // wrap detail text
                    var format = new StringFormat();
                    var rect = new RectangleF(x, y, width - 40, height - y - 20);
                    g.DrawString(info?.Detail ?? "", textFont, Brushes.Black, rect, format);
                }

                var ms = new MemoryStream();
                bmp.Save(ms, ImageFormat.Png);
                ms.Position = 0;
                return ms;
            }
            catch (Exception ex)
            {
                Log(ex);
                return null;
            }
        }

        private static readonly object _logLock = new object();
        private static void Log(string message)
        {
            try
            {
                string dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory ?? "", "logs");
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                string file = Path.Combine(dir, "email.log");
                var line = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}{Environment.NewLine}";
                lock (_logLock)
                {
                    File.AppendAllText(file, line, Encoding.UTF8);
                }
            }
            catch { /* swallow logging errors */ }
        }

        private static void Log(Exception ex)
        {
            try
            {
                var sb = new StringBuilder();
                sb.AppendLine("Exception: " + ex.Message);
                sb.AppendLine(ex.ToString());
                if (ex.InnerException != null)
                {
                    sb.AppendLine("Inner Exception: " + ex.InnerException.Message);
                    sb.AppendLine(ex.InnerException.ToString());
                }
                Log(sb.ToString());
            }
            catch { /* swallow logging errors */ }
        }

        // Overload: accept BookingInfo (used by Room_Details) and optional roomCode to include in detail
        public static void SendCancellationEmail(BookingInfo info, string roomCode = null, string reason = null)
        {
            if (info == null) return;
            var bri = ToRowInfo(info, roomCode);
            bri.IsCancelled = true;
            bri.CancellationReason = reason;
            SendCancellationEmail(bri);
        }

        public static void SendBookingConfirmation(BookingInfo info, string roomCode = null)
        {
            if (info == null) return;
            var bri = ToRowInfo(info, roomCode);
            SendBookingConfirmation(bri);
        }

        private static BookingRowInfo ToRowInfo(BookingInfo info, string roomCode)
        {
            var id = roomCode ?? "";
            try
            {
                if (!string.IsNullOrWhiteSpace(roomCode)) id = roomCode + "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
            }
            catch { id = roomCode ?? ""; }

            var detail = string.Empty;
            try
            {
                if (!string.IsNullOrWhiteSpace(roomCode))
                {
                    detail = roomCode + " (" + info.Start.ToString("dd/MM/yyyy HH:mm") + " - " + info.End.ToString("dd/MM/yyyy HH:mm") + ")";
                }
                else
                {
                    detail = info.Start.ToString("dd/MM/yyyy HH:mm") + " - " + info.End.ToString("dd/MM/yyyy HH:mm");
                }
            }
            catch { detail = string.Empty; }

            return new BookingRowInfo
            {
                Id = id,
                Customer = info.Customer,
                Date = DateTime.Now.ToString("dd/MM/yyyy"),
                Staff = string.Empty,
                Detail = detail,
                CCCD = info.IdCard,
                SDT = info.Phone,
                Email = info.Email,
                Gender = info.Gender,
                Nationality = info.Nationality,
                CreatedDate = DateTime.Now
            };
        }
    }
}
