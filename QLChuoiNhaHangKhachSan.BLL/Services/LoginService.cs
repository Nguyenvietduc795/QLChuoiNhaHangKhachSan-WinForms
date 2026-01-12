using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using QLChuoiNhaHangKhachSan.DAL.Models;
using QLChuoiNhaHangKhachSan.DAL.Repositories;

namespace QLChuoiNhaHangKhachSan.BLL.Services
{
    public class LoginService
    {
        private readonly LoginRepository _repo;

        /// <summary>
        /// Constructor mặc định - sử dụng DatabaseConnection.ConnectionString
        /// </summary>
        public LoginService()
        {
            _repo = new LoginRepository();
        }

        /// <summary>
        /// Constructor với connection string tùy chỉnh
        /// </summary>
        public LoginService(string connStr)
        {
            _repo = new LoginRepository(connStr);
        }

        // Mã hóa password bằng SHA256
        private byte[] HashPassword(string password, byte[] salt)
        {
            using (var sha256 = SHA256.Create())
            {
                var pwdBytes = Encoding.UTF8.GetBytes(password);
                var combined = new byte[pwdBytes.Length + (salt?.Length ?? 0)];
                Buffer.BlockCopy(pwdBytes, 0, combined, 0, pwdBytes.Length);
                if (salt != null && salt.Length > 0)
                {
                    Buffer.BlockCopy(salt, 0, combined, pwdBytes.Length, salt.Length);
                }
                return sha256.ComputeHash(combined);
            }
        }

        private byte[] GenerateSalt(int size = 16)
        {
            var salt = new byte[size];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        // Validate thông tin đăng ký
        private void ValidateRegistration(string username, string password, string confirmPassword)
        {
            // Kiểm tra username
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Tên đăng nhập không được để trống");

            if (username.Length < 3)
                throw new ArgumentException("Tên đăng nhập phải có ít nhất 3 ký tự");

            if (username.Length > 50)
                throw new ArgumentException("Tên đăng nhập không được quá 50 ký tự");

            if (!Regex.IsMatch(username, "^[a-zA-Z0-9_]+$"))
                throw new ArgumentException("Tên đăng nhập chỉ được chứa chữ cái, số và dấu gạch dưới");

            // Kiểm tra password
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Mật khẩu không được để trống");

            if (password.Length < 6)
                throw new ArgumentException("Mật khẩu phải có ít nhất 6 ký tự");

            if (password != confirmPassword)
                throw new ArgumentException("Mật khẩu xác nhận không khớp");

            // Kiểm tra username đã tồn tại
            if (_repo.UsernameExists(username))
                throw new ArgumentException("Tên đăng nhập đã tồn tại");
        }

        /// <summary>
        /// Validate mật khẩu mới
        /// </summary>
        private void ValidateNewPassword(string newPassword, string confirmPassword)
        {
            if (string.IsNullOrWhiteSpace(newPassword))
                throw new ArgumentException("Mật khẩu mới không được để trống");

            if (newPassword.Length < 6)
                throw new ArgumentException("Mật khẩu phải có ít nhất 6 ký tự");

            if (newPassword.Length > 100)
                throw new ArgumentException("Mật khẩu không được quá 100 ký tự");

            // Kiểm tra độ mạnh mật khẩu
            if (!Regex.IsMatch(newPassword, @"[A-Z]"))
                throw new ArgumentException("Mật khẩu phải có ít nhất 1 chữ hoa");

            if (!Regex.IsMatch(newPassword, @"[a-z]"))
                throw new ArgumentException("Mật khẩu phải có ít nhất 1 chữ thường");

            if (!Regex.IsMatch(newPassword, @"\d"))
                throw new ArgumentException("Mật khẩu phải có ít nhất 1 số");

            if (newPassword != confirmPassword)
                throw new ArgumentException("Mật khẩu xác nhận không khớp");
        }

        // Đăng ký user mới
        public void Register(string username, string password, string confirmPassword)
        {
            ValidateRegistration(username, password, confirmPassword);
            var salt = GenerateSalt();
            var passwordHash = HashPassword(password, salt);
            _repo.RegisterUser(username, passwordHash, salt);
        }

        // Đăng nhập
        public Login Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Tên đăng nhập không được để trống");

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Mật khẩu không được để trống");

            var user = _repo.GetUserByUsername(username);

            if (user == null)
                throw new InvalidOperationException("Tên đăng nhập hoặc mật khẩu không chính xác");

            var passwordHash = HashPassword(password, user.PasswordSalt ?? new byte[0]);

            if (user.PasswordHash == null || user.PasswordHash.Length == 0 || !passwordHash.SequenceEqual(user.PasswordHash))
                throw new InvalidOperationException("Tên đăng nhập hoặc mật khẩu không chính xác");

            // Cho phép đăng nhập nếu:
            // 1. Status = "Active" (tài khoản đã kích hoạt)
            // 2. Status = "Inactive" VÀ MustChangePassword = true (lần đầu đăng nhập)
            // Không cho phép nếu Status != "Active" và đã đổi mật khẩu rồi (tài khoản bị khóa)
            if (user.Status != "Active" && !user.MustChangePassword)
                throw new InvalidOperationException("Tài khoản đã bị khóa");

            // Cập nhật thời gian đăng nhập
            _repo.UpdateLastLogin(username);

            return user;
        }

        /// <summary>
        /// Đổi mật khẩu (dùng khi đăng nhập lần đầu hoặc user tự đổi)
        /// </summary>
        /// <param name="username">Tên đăng nhập</param>
        /// <param name="currentPassword">Mật khẩu hiện tại</param>
        /// <param name="newPassword">Mật khẩu mới</param>
        /// <param name="confirmPassword">Xác nhận mật khẩu mới</param>
        public void ChangePassword(string username, string currentPassword, string newPassword, string confirmPassword)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Tên đăng nhập không được để trống");

            if (string.IsNullOrWhiteSpace(currentPassword))
                throw new ArgumentException("Mật khẩu hiện tại không được để trống");

            // Kiểm tra mật khẩu mới
            ValidateNewPassword(newPassword, confirmPassword);

            // Kiểm tra mật khẩu mới không được trùng mật khẩu cũ
            if (currentPassword == newPassword)
                throw new ArgumentException("Mật khẩu mới không được trùng mật khẩu hiện tại");

            // Lấy thông tin user
            var user = _repo.GetUserByUsername(username);
            if (user == null)
                throw new InvalidOperationException("Không tìm thấy tài khoản");

            // Kiểm tra mật khẩu hiện tại
            var currentHash = HashPassword(currentPassword, user.PasswordSalt ?? new byte[0]);
            if (user.PasswordHash == null || !currentHash.SequenceEqual(user.PasswordHash))
                throw new InvalidOperationException("Mật khẩu hiện tại không chính xác");

            // Tạo mật khẩu mới
            var newSalt = GenerateSalt();
            var newHash = HashPassword(newPassword, newSalt);

            // Cập nhật mật khẩu (đồng thời đặt MustChangePassword = false)
            _repo.UpdatePassword(username, newHash, newSalt);
        }

        /// <summary>
        /// Đổi mật khẩu lần đầu (không cần kiểm tra mật khẩu cũ nghiêm ngặt vì đã xác thực qua Login)
        /// </summary>
        public void ChangePasswordFirstTime(string username, string newPassword, string confirmPassword)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Tên đăng nhập không được để trống");

            // Kiểm tra mật khẩu mới
            ValidateNewPassword(newPassword, confirmPassword);

            // Lấy thông tin user
            var user = _repo.GetUserByUsername(username);
            if (user == null)
                throw new InvalidOperationException("Không tìm thấy tài khoản");

            // Tạo mật khẩu mới
            var newSalt = GenerateSalt();
            var newHash = HashPassword(newPassword, newSalt);

            // Cập nhật mật khẩu (đồng thời đặt MustChangePassword = false)
            _repo.UpdatePassword(username, newHash, newSalt);

            // Sau khi đổi mật khẩu lần đầu, chuyển trạng thái sang Active
            _repo.SetStatusActive(username);
        }

        /// <summary>
        /// Kiểm tra xem user có cần đổi mật khẩu lần đầu không
        /// </summary>
        public bool RequiresPasswordChange(string username)
        {
            var user = _repo.GetUserByUsername(username);
            return user != null && user.MustChangePassword;
        }

        // Kiểm tra username đã tồn tại
        public bool UsernameExists(string username)
        {
            return _repo.UsernameExists(username);
        }
    }
}
