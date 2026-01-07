using System;
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

        public LoginService(string connStr)
        {
            _repo = new LoginRepository(connStr);
        }

        // Mã hóa password bằng SHA256
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
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

        // Đăng ký user mới
        public void Register(string username, string password, string confirmPassword)
        {
            ValidateRegistration(username, password, confirmPassword);
            string passwordHash = HashPassword(password);
            _repo.RegisterUser(username, passwordHash);
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

            string passwordHash = HashPassword(password);
            
            if (user.PasswordHash != passwordHash)
                throw new InvalidOperationException("Tên đăng nhập hoặc mật khẩu không chính xác");

            if (user.Status != "Active")
                throw new InvalidOperationException("Tài khoản đã bị khóa");

            // Cập nhật thời gian đăng nhập
            _repo.UpdateLastLogin(username);

            return user;
        }

        // Kiểm tra username đã tồn tại
        public bool UsernameExists(string username)
        {
            return _repo.UsernameExists(username);
        }
    }
}
