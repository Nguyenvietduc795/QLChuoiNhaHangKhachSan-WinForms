using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Text;

namespace QLChuoiNhaHangKhachSan.BLL.Services
{
    /// <summary>
    /// Dịch vụ gửi email thông báo tài khoản cho nhân viên
    /// </summary>
    public class EmailService
    {
        private readonly string _smtpHost;
        private readonly int _smtpPort;
        private readonly string _smtpUser;
        private readonly string _smtpPassword;
        private readonly bool _enableSsl;
        private readonly string _fromEmail;
        private readonly string _fromName;

        /// <summary>
        /// Constructor với cấu hình mặc định (Gmail SMTP)
        /// </summary>
        public EmailService() : this("smtp.gmail.com", 587, "", "", true, "", "He thong Quan ly Nha hang Khach san")
        {
        }

        /// <summary>
        /// Constructor với cấu hình tùy chỉnh
        /// </summary>
        public EmailService(string smtpHost, int smtpPort, string smtpUser, string smtpPassword,
            bool enableSsl, string fromEmail, string fromName)
        {
            _smtpHost = string.IsNullOrWhiteSpace(smtpHost) ? "smtp.gmail.com" : smtpHost;
            _smtpPort = smtpPort > 0 ? smtpPort : 587;
            _smtpUser = smtpUser ?? "";
            _smtpPassword = smtpPassword ?? "";
            _enableSsl = enableSsl;
            _fromEmail = string.IsNullOrWhiteSpace(fromEmail) ? _smtpUser : fromEmail;
            _fromName = string.IsNullOrWhiteSpace(fromName) ? "He thong Quan ly Nha hang Khach san" : fromName;
        }

        /// <summary>
        /// Gửi email thông báo tài khoản đăng nhập cho nhân viên mới
        /// </summary>
        /// <param name="toEmail">Email nhân viên</param>
        /// <param name="employeeName">Tên nhân viên</param>
        /// <param name="username">Tên đăng nhập</param>
        /// <param name="password">Mật khẩu tạm thời</param>
        public void SendAccountCredentials(string toEmail, string employeeName, string username, string password)
        {
            if (string.IsNullOrWhiteSpace(_smtpUser) || string.IsNullOrWhiteSpace(_smtpPassword))
            {
                // Nếu chưa cấu hình SMTP, bỏ qua việc gửi email (chỉ hiển thị thông tin tài khoản cho admin)
                return;
            }

            var subject = "Thong tin tai khoan dang nhap he thong";
            var body = $@"<!DOCTYPE html>
<html>
<head>
    <meta charset=""UTF-8"">
    <meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8"">
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background-color: #2c3e50; color: white; padding: 20px; text-align: center; }}
        .content {{ padding: 20px; background-color: #f9f9f9; }}
        .credentials {{ background-color: #ecf0f1; padding: 15px; border-radius: 5px; margin: 15px 0; }}
        .warning {{ color: #e74c3c; font-weight: bold; }}
        .footer {{ text-align: center; padding: 10px; color: #7f8c8d; font-size: 12px; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h2>Chao mung {employeeName}!</h2>
        </div>
        <div class='content'>
            <p>Xin chao <strong>{employeeName}</strong>,</p>
            <p>Tai khoan dang nhap he thong cua ban da duoc tao thanh cong.</p>
            
            <div class='credentials'>
                <p><strong>Thong tin dang nhap:</strong></p>
                <p>* Ten dang nhap: <strong>{username}</strong></p>
                <p>* Mat khau tam thoi: <strong>{password}</strong></p>
            </div>
            
            <p class='warning'>** Luu y quan trong:</p>
            <ul>
                <li>Khi dang nhap lan dau tien, ban se duoc yeu cau doi mat khau.</li>
                <li>Vui long khong chia se thong tin dang nhap nay voi bat ky ai.</li>
                <li>Neu ban khong yeu cau tai khoan nay, vui long lien he quan tri vien ngay.</li>
            </ul>
        </div>
        <div class='footer'>
            <p>Email nay duoc gui tu dong tu He thong Quan ly Nha hang Khach san.</p>
            <p>Vui long khong tra loi email nay.</p>
        </div>
    </div>
</body>
</html>";

            SendEmail(toEmail, subject, body, isHtml: true);
        }

        /// <summary>
        /// Gửi email với nội dung tùy chỉnh
        /// </summary>
        public void SendEmail(string toEmail, string subject, string body, bool isHtml = false)
        {
            if (string.IsNullOrWhiteSpace(toEmail))
                throw new ArgumentException("Email nguoi nhan khong duoc de trong", nameof(toEmail));

            if (string.IsNullOrWhiteSpace(_smtpUser) || string.IsNullOrWhiteSpace(_smtpPassword))
                return;

            using (var message = new MailMessage())
            {
                message.From = new MailAddress(_fromEmail, _fromName, Encoding.UTF8);
                message.To.Add(new MailAddress(toEmail));
                message.Subject = subject;
                message.SubjectEncoding = Encoding.UTF8;
                message.BodyEncoding = Encoding.UTF8;
                message.HeadersEncoding = Encoding.UTF8;

                if (isHtml)
                {
                    // Sử dụng AlternateView với content type chỉ định charset UTF-8
                    var htmlView = AlternateView.CreateAlternateViewFromString(body, Encoding.UTF8, "text/html");
                    message.AlternateViews.Add(htmlView);
                    message.IsBodyHtml = true;
                }
                else
                {
                    message.Body = body;
                }

                using (var client = new SmtpClient(_smtpHost, _smtpPort))
                {
                    client.EnableSsl = _enableSsl;
                    client.Credentials = new NetworkCredential(_smtpUser, _smtpPassword);
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.Timeout = 30000;
                    client.Send(message);
                }
            }
        }

        /// <summary>
        /// Gửi email bất đồng bộ
        /// </summary>
        public async Task SendEmailAsync(string toEmail, string subject, string body, bool isHtml = false)
        {
            if (string.IsNullOrWhiteSpace(toEmail))
                throw new ArgumentException("Email nguoi nhan khong duoc de trong", nameof(toEmail));

            if (string.IsNullOrWhiteSpace(_smtpUser) || string.IsNullOrWhiteSpace(_smtpPassword))
            {
                return;
            }

            using (var message = new MailMessage())
            {
                message.From = new MailAddress(_fromEmail, _fromName, Encoding.UTF8);
                message.To.Add(new MailAddress(toEmail));
                message.Subject = subject;
                message.SubjectEncoding = Encoding.UTF8;
                message.BodyEncoding = Encoding.UTF8;
                message.HeadersEncoding = Encoding.UTF8;

                if (isHtml)
                {
                    var htmlView = AlternateView.CreateAlternateViewFromString(body, Encoding.UTF8, "text/html");
                    message.AlternateViews.Add(htmlView);
                    message.IsBodyHtml = true;
                }
                else
                {
                    message.Body = body;
                }

                using (var client = new SmtpClient(_smtpHost, _smtpPort))
                {
                    client.EnableSsl = _enableSsl;
                    client.Credentials = new NetworkCredential(_smtpUser, _smtpPassword);
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.Timeout = 30000;

                    await client.SendMailAsync(message);
                }
            }
        }

        /// <summary>
        /// Gửi email thông báo tài khoản bất đồng bộ
        /// </summary>
        public async Task SendAccountCredentialsAsync(string toEmail, string employeeName, string username, string password)
        {
            await Task.Run(() => SendAccountCredentials(toEmail, employeeName, username, password));
        }
    }
}
