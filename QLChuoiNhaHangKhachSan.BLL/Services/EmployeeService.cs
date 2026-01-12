using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using QLChuoiNhaHangKhachSan.DAL.Models;
using QLChuoiNhaHangKhachSan.DAL.Repositories;

namespace QLChuoiNhaHangKhachSan.BLL.Services
{
    public class EmployeeService
    {
        private readonly EmployeeRepository _repo;
        private EmailService _emailService;

        /// <summary>
        /// Constructor mặc định - sử dụng DatabaseConnection.ConnectionString
        /// </summary>
        public EmployeeService()
        {
            _repo = new EmployeeRepository();
            _emailService = new EmailService();
        }

        /// <summary>
        /// Constructor với connection string tùy chỉnh
        /// </summary>
        public EmployeeService(string connStr)
        {
            _repo = new EmployeeRepository(connStr);
            _emailService = new EmailService();
        }

        /// <summary>
        /// Cấu hình SMTP cho dịch vụ email
        /// </summary>
        public void ConfigureEmail(string smtpHost, int smtpPort, string smtpUser, string smtpPassword,
            bool enableSsl, string fromEmail, string fromName)
        {
            _emailService = new EmailService(smtpHost, smtpPort, smtpUser, smtpPassword, enableSsl, fromEmail, fromName);
        }

        public List<Employee> GetAll() => _repo.GetAllWithActiveContract();

        public void Add(Employee emp, decimal dealSalary, decimal salaryCoef)
        {
            Validate(emp, dealSalary, salaryCoef);
            _repo.Insert(emp, dealSalary, salaryCoef);
        }

        /// <summary>
        /// Thêm nhân viên mới và tạo tài khoản đăng nhập, gửi email thông báo
        /// </summary>
        /// <param name="emp">Thông tin nhân viên</param>
        /// <param name="dealSalary">Lương deal</param>
        /// <param name="salaryCoef">Hệ số lương</param>
        /// <param name="sendEmail">Có gửi email thông báo tài khoản hay không</param>
        /// <returns>Thông tin tài khoản đã tạo (username, password tạm)</returns>
        public (string Username, string TempPassword) AddWithAccount(Employee emp, decimal dealSalary, decimal salaryCoef, bool sendEmail = true)
        {
            Validate(emp, dealSalary, salaryCoef);

            // Tạo username từ email (phần trước @)
            string username = GenerateUsername(emp.Email);

            // Kiểm tra username đã tồn tại
            if (_repo.UsernameExists(username))
            {
                // Thêm số vào cuối để tạo username unique
                int suffix = 1;
                string baseUsername = username;
                while (_repo.UsernameExists(username))
                {
                    username = $"{baseUsername}{suffix}";
                    suffix++;
                }
            }

            // Tạo mật khẩu tạm thời ngẫu nhiên
            string tempPassword = GenerateTemporaryPassword();

            // Hash mật khẩu
            byte[] salt = GenerateSalt();
            byte[] passwordHash = HashPassword(tempPassword, salt);

            // Lưu vào DB
            _repo.InsertWithCredentials(emp, dealSalary, salaryCoef, username, passwordHash, salt);

            // Gửi email thông báo tài khoản
            if (sendEmail)
            {
                try
                {
                    _emailService.SendAccountCredentials(emp.Email, emp.FullName, username, tempPassword);
                }
                catch (Exception)
                {
                    // Nếu gửi email thất bại, vẫn trả về thông tin để hiển thị cho admin
                    // Log lỗi nếu cần
                }
            }

            return (username, tempPassword);
        }

        /// <summary>
        /// Cập nhật thông tin nhân viên
        /// </summary>
        public void Update(Employee emp, decimal dealSalary, decimal salaryCoef)
        {
            Validate(emp, dealSalary, salaryCoef);
            _repo.Update(emp, dealSalary, salaryCoef);
        }

        /// <summary>
        /// Tạo username từ email
        /// </summary>
        private string GenerateUsername(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email không được để trống");

            // Lấy phần trước @
            int atIndex = email.IndexOf('@');
            if (atIndex <= 0)
                throw new ArgumentException("Email không hợp lệ");

            string username = email.Substring(0, atIndex).ToLower();

            // Loại bỏ ký tự đặc biệt, chỉ giữ chữ cái, số và dấu gạch dưới
            username = Regex.Replace(username, @"[^a-z0-9_]", "");

            // Đảm bảo username có ít nhất 3 ký tự
            if (username.Length < 3)
            {
                username = "user" + username;
            }

            return username;
        }

        /// <summary>
        /// Tạo mật khẩu tạm thời ngẫu nhiên
        /// </summary>
        private string GenerateTemporaryPassword(int length = 10)
        {
            const string upperChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lowerChars = "abcdefghijklmnopqrstuvwxyz";
            const string digitChars = "0123456789";
            const string allChars = upperChars + lowerChars + digitChars;

            var random = new Random();
            var password = new StringBuilder();

            // Đảm bảo có ít nhất 1 chữ hoa, 1 chữ thường, 1 số
            password.Append(upperChars[random.Next(upperChars.Length)]);
            password.Append(lowerChars[random.Next(lowerChars.Length)]);
            password.Append(digitChars[random.Next(digitChars.Length)]);

            // Thêm các ký tự còn lại
            for (int i = 3; i < length; i++)
            {
                password.Append(allChars[random.Next(allChars.Length)]);
            }

            // Xáo trộn mật khẩu
            return new string(password.ToString().OrderBy(x => random.Next()).ToArray());
        }

        /// <summary>
        /// Tạo salt cho mật khẩu
        /// </summary>
        private byte[] GenerateSalt(int size = 16)
        {
            var salt = new byte[size];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        /// <summary>
        /// Hash mật khẩu với salt
        /// </summary>
        private byte[] HashPassword(string password, byte[] salt)
        {
            using (var sha256 = SHA256.Create())
            {
                var pwdBytes = Encoding.UTF8.GetBytes(password);
                var combined = new byte[pwdBytes.Length + salt.Length];
                Buffer.BlockCopy(pwdBytes, 0, combined, 0, pwdBytes.Length);
                Buffer.BlockCopy(salt, 0, combined, pwdBytes.Length, salt.Length);
                return sha256.ComputeHash(combined);
            }
        }

        //hàm cập nhật nhân viên
        private void Validate(Employee emp, decimal dealSalary, decimal salaryCoef)
        {
            if (emp == null) throw new ArgumentNullException(nameof(emp));

            emp.FullName = emp.FullName?.Trim();
            emp.Email = emp.Email?.Trim();
            emp.Department = emp.Department?.Trim();
            emp.Position = emp.Position?.Trim();
            emp.Phone = emp.Phone?.Trim();
            emp.CountryCode = emp.CountryCode?.Trim();

            if (string.IsNullOrWhiteSpace(emp.FullName)) throw new ArgumentException("Tên không được để trống"); 
            if (Regex.IsMatch(emp.FullName, "\\d")) throw new ArgumentException("Tên không được chứa số");
            if (string.IsNullOrWhiteSpace(emp.Email)) throw new ArgumentException("Email không được để trống");
            if (!Regex.IsMatch(emp.Email, @"^[^@\s]+@gmail\.com$", RegexOptions.IgnoreCase)) throw new ArgumentException("Gmail chưa hợp lệ");

            // Country code: cho phép dùng +84, +1, +65... tối đa 4 chữ số
            if (string.IsNullOrWhiteSpace(emp.CountryCode)) emp.CountryCode = "+84";
            if (!Regex.IsMatch(emp.CountryCode, @"^\+\d{1,4}$")) throw new ArgumentException("Mã quốc gia không hợp lệ (ví dụ: +84)");

            if (string.IsNullOrWhiteSpace(emp.Phone) || !Regex.IsMatch(emp.Phone, "^\\d+$")) throw new ArgumentException("Số điện thoại không hợp lệ");
            if (dealSalary < 0) throw new ArgumentException("Lương deal phải >= 0");
            if (salaryCoef < 0) throw new ArgumentException("Hệ số phải >= 0");
            if (emp.HireDate == default(DateTime)) emp.HireDate = DateTime.Today;
            if (string.IsNullOrWhiteSpace(emp.Status)) emp.Status = "Inactive";
        }


        //hàm cập nhật trạng thái nhân viên
        public void UpdateStatus(int employeeId, string newStatus, decimal currentSalary)
        {
            if (string.IsNullOrWhiteSpace(newStatus)) throw new ArgumentException("Trạng thái không hợp lệ", nameof(newStatus));
            // Không cho chuyển sang Inactive nếu lương > 0
            if (string.Equals(newStatus, "Inactive", StringComparison.OrdinalIgnoreCase) && currentSalary > 0)
            {
                throw new InvalidOperationException("Không thể chuyển sang Inactive khi lương > 0");
            }
            _repo.UpdateStatus(employeeId, newStatus);
        }

        //hàm hủy kích hoạt nhân viên
        public void DeactivateEmployee(int employeeId)
        {
            _repo.DeactivateAndResetSalary(employeeId);
        }

        //hàm lọc nhân viên
        public List<Employee> Filter(string term, string status)
        {
            var source = _repo.GetAllWithActiveContract() ?? new List<Employee>();
            IEnumerable<Employee> query = source;

            if (string.Equals(status, "Active", StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(emp => string.Equals(emp?.Status, "Active", StringComparison.OrdinalIgnoreCase));
            }
            else if (string.Equals(status, "Inactive", StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(emp => string.Equals(emp?.Status, "Inactive", StringComparison.OrdinalIgnoreCase));
            }

            term = (term ?? string.Empty).Trim().ToLowerInvariant();
            if (!string.IsNullOrWhiteSpace(term))
            {
                query = query.Where(emp =>
                {
                    if (emp == null) return false;
                    return (emp.FullName ?? string.Empty).ToLowerInvariant().Contains(term)
                        || (emp.Email ?? string.Empty).ToLowerInvariant().Contains(term)
                        || (emp.Department ?? string.Empty).ToLowerInvariant().Contains(term)
                        || (emp.Position ?? string.Empty).ToLowerInvariant().Contains(term)
                        || (emp.Phone ?? string.Empty).ToLowerInvariant().Contains(term);
                });
            }

            return query.ToList();
        }

        //hàm gợi ý tên nhân viên
        public List<string> SuggestNames(string term)
        {
            var source = _repo.GetAllWithActiveContract() ?? new List<Employee>();
            term = (term ?? string.Empty).Trim().ToLowerInvariant();
            var query = source
                .Where(e => e != null && !string.IsNullOrWhiteSpace(e.FullName))
                .Select(e => e.FullName);

            if (!string.IsNullOrWhiteSpace(term))
            {
                query = query.Where(name => name != null && name.ToLowerInvariant().Contains(term));
            }

            return query.Distinct(StringComparer.OrdinalIgnoreCase).ToList();
        }
    }
}
