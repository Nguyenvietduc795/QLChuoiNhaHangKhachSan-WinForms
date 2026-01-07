using System;
using System.Data;
using System.Data.SqlClient;
using QLChuoiNhaHangKhachSan.DAL.Models;

namespace QLChuoiNhaHangKhachSan.DAL.Repositories
{
    public class LoginRepository
    {
        private readonly string _connStr;

        public LoginRepository(string connStr)
        {
            _connStr = connStr ?? throw new ArgumentNullException(nameof(connStr));
        }

        // Ki?m tra username có t?n t?i không
        public bool UsernameExists(string username)
        {
            const string sql = "SELECT COUNT(*) FROM users WHERE userName = @Username";
            
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
                SELECT id, userName, password, status, dateCreated 
                FROM users 
                WHERE userName = @Username";
            
            using (var conn = new SqlConnection(_connStr))
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Username", username);
                conn.Open();
                
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Login
                        {
                            LoginId = reader.GetInt32(0),
                            Username = reader.GetString(1),
                            PasswordHash = reader.GetString(2),
                            Status = reader.IsDBNull(3) ? "Active" : reader.GetString(3),
                            DateCreated = reader.IsDBNull(4) ? DateTime.Now : reader.GetDateTime(4)
                        };
                    }
                }
            }
            
            return null;
        }

        // ??ng ký user m?i
        public void RegisterUser(string username, string passwordHash)
        {
            const string sql = @"
                INSERT INTO users (userName, password, status, dateCreated) 
                VALUES (@Username, @PasswordHash, @Status, @DateCreated)";
            
            using (var conn = new SqlConnection(_connStr))
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);
                cmd.Parameters.AddWithValue("@Status", "Active");
                cmd.Parameters.AddWithValue("@DateCreated", DateTime.Now);
                
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // C?p nh?t th?i gian ??ng nh?p cu?i
        public void UpdateLastLogin(string username)
        {
            const string sql = "UPDATE users SET dateCreated = @LastLogin WHERE userName = @Username";
            
            using (var conn = new SqlConnection(_connStr))
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@LastLogin", DateTime.Now);
                
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
