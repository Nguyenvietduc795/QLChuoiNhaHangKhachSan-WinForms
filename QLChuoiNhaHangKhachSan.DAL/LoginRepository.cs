using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using QLChuoiNhaHangKhachSan.DAL.Models;

namespace QLChuoiNhaHangKhachSan.DAL.Repositories
{
    public class LoginRepository
    {
        private readonly string _connStr;

        /// <summary>
        /// L?y connection string t? c?u hình
        /// </summary>
        private static string GetConnectionString()
        {
            var connStrSetting = ConfigurationManager.ConnectionStrings["ConnStr"];
            if (connStrSetting != null && !string.IsNullOrWhiteSpace(connStrSetting.ConnectionString))
            {
                return connStrSetting.ConnectionString;
            }
            connStrSetting = ConfigurationManager.ConnectionStrings["DbConnection"];
            return connStrSetting?.ConnectionString ?? string.Empty;
        }

        /// <summary>
        /// Constructor m?c ??nh - s? d?ng connection string t? c?u hình
        /// </summary>
        public LoginRepository()
        {
            _connStr = GetConnectionString();
        }

        /// <summary>
        /// Constructor v?i connection string tùy ch?nh
        /// </summary>
        public LoginRepository(string connStr)
        {
            _connStr = connStr ?? GetConnectionString();
        }

        // Ki?m tra username có t?n t?i không
        public bool UsernameExists(string username)
        {
            const string sql = "SELECT COUNT(*) FROM Employees WHERE UserName = @Username";
            using (var conn = new SqlConnection(_connStr))
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Username", username);
                conn.Open();
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }

        // L?y thông tin user theo username
        public Login GetUserByUsername(string username)
        {
            const string sql = @"
                SELECT EmployeeId, UserName, PasswordHash, PasswordSalt, Status, HireDate, MustChangePassword
                FROM Employees
                WHERE UserName = @Username";

            using (var conn = new SqlConnection(_connStr))
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Username", username);
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var pwdHash = reader.IsDBNull(2) ? null : (byte[])reader[2];
                        var pwdSalt = reader.IsDBNull(3) ? null : (byte[])reader[3];
                        return new Login
                        {
                            LoginId = reader.GetInt32(0),
                            Username = reader.GetString(1),
                            PasswordHash = pwdHash,
                            PasswordSalt = pwdSalt,
                            Status = reader.IsDBNull(4) ? "Active" : reader.GetString(4),
                            DateCreated = reader.IsDBNull(5) ? DateTime.Now : reader.GetDateTime(5),
                            MustChangePassword = !reader.IsDBNull(6) && reader.GetBoolean(6)
                        };
                    }
                }
            }
            return null;
        }

        // ??ng ký user m?i
        public void RegisterUser(string username, byte[] passwordHash, byte[] passwordSalt)
        {
            const string sql = @"
                INSERT INTO Employees (FullName, UserName, PasswordHash, PasswordSalt, Status)
                VALUES (@FullName, @UserName, @PasswordHash, @PasswordSalt, @Status)";

            using (var conn = new SqlConnection(_connStr))
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@FullName", username);
                cmd.Parameters.AddWithValue("@UserName", username);
                cmd.Parameters.AddWithValue("@PasswordHash", (object)passwordHash ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@PasswordSalt", (object)passwordSalt ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Status", "Active");

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// C?p nh?t m?t kh?u và ?ánh d?u ?ã ??i m?t kh?u l?n ??u
        /// </summary>
        public void UpdatePassword(string username, byte[] passwordHash, byte[] passwordSalt)
        {
            const string sql = @"
                UPDATE Employees 
                SET PasswordHash = @PasswordHash, 
                    PasswordSalt = @PasswordSalt, 
                    MustChangePassword = 0 
                WHERE UserName = @Username";

            using (var conn = new SqlConnection(_connStr))
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@PasswordHash", (object)passwordHash ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@PasswordSalt", (object)passwordSalt ?? DBNull.Value);

                conn.Open();
                int rows = cmd.ExecuteNonQuery();
                if (rows == 0)
                {
                    throw new InvalidOperationException("Không tìm th?y tài kho?n ?? c?p nh?t m?t kh?u");
                }
            }
        }

        /// <summary>
        /// ??t c? yêu c?u ??i m?t kh?u
        /// </summary>
        public void SetMustChangePassword(string username, bool mustChange)
        {
            const string sql = @"UPDATE Employees SET MustChangePassword = @MustChange WHERE UserName = @Username";

            using (var conn = new SqlConnection(_connStr))
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@MustChange", mustChange);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void SetStatusActive(string username)
        {
            const string sql = "UPDATE Employees SET Status = 'Active' WHERE UserName = @Username";
            using (var conn = new SqlConnection(_connStr))
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Username", username);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // C?p nh?t th?i gian ??ng nh?p cu?i
        public void UpdateLastLogin(string username)
        {
            // Schema hi?n t?i không có tr??ng LastLogin; b? qua ?? tránh l?i DB.
        }
    }
}
