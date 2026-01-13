using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using QLChuoiNhaHangKhachSan.DAL.Models;

namespace QLChuoiNhaHangKhachSan.DAL
{
    // Repository làm việc trực tiếp với bảng Customers trong database
    public class CustomerDal
    {
        private readonly string _connectionString;

        public CustomerDal()
        {
            _connectionString =
                ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            EnsureTotalSpendingColumn();
        }

        private void EnsureTotalSpendingColumn()
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(@"
                IF COL_LENGTH('dbo.Customers', 'TotalSpending') IS NULL
                BEGIN
                    ALTER TABLE dbo.Customers
                    ADD TotalSpending DECIMAL(18, 2) NOT NULL CONSTRAINT DF_Customers_TotalSpending DEFAULT (0);
                END", conn))
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public List<Customer> GetAll()
        {
            var result = new List<Customer>();

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(@"
                SELECT CustomerId, FullName, Nationality, CCCD, Sex,
                       PhoneNumber, Email, Address, CustomerType, CreatedAt, TotalSpending
                FROM dbo.Customers;", conn))
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int ordNationality = reader.GetOrdinal("Nationality");
                        int ordCCCD = reader.GetOrdinal("CCCD");
                        int ordSex = reader.GetOrdinal("Sex");
                        int ordPhone = reader.GetOrdinal("PhoneNumber");
                        int ordEmail = reader.GetOrdinal("Email");
                        int ordAddress = reader.GetOrdinal("Address");
                        int ordType = reader.GetOrdinal("CustomerType");
                        int ordCreated = reader.GetOrdinal("CreatedAt");
                        int ordSpending = reader.GetOrdinal("TotalSpending");

                        var c = new Customer
                        {
                            CustomerId = reader.GetInt32(0),
                            FullName = reader.GetString(1),
                            Nationality = reader.IsDBNull(ordNationality) ? null : reader.GetString(ordNationality),
                            CCCD = reader.IsDBNull(ordCCCD) ? null : reader.GetString(ordCCCD),
                            Sex = reader.IsDBNull(ordSex) ? null : reader.GetString(ordSex),
                            PhoneNumber = reader.IsDBNull(ordPhone) ? null : reader.GetString(ordPhone),
                            Email = reader.IsDBNull(ordEmail) ? null : reader.GetString(ordEmail),
                            Address = reader.IsDBNull(ordAddress) ? null : reader.GetString(ordAddress),
                            CustomerType = reader.IsDBNull(ordType) ? null : reader.GetString(ordType),
                            CreatedAt = reader.GetDateTime(ordCreated),
                            TotalSpending = reader.IsDBNull(ordSpending) ? 0m : reader.GetDecimal(ordSpending)
                        };
                        result.Add(c);
                    }
                }
            }

            return result;
        }

        public int Insert(Customer c)
        {
            // Xác định prefix từ CustomerType, mặc định NH
            string prefix;
            if (string.IsNullOrWhiteSpace(c.CustomerType))
            {
                prefix = "NH";
                c.CustomerType = "NH_Thường";
            }
            else
            {
                var upper = c.CustomerType.ToUpperInvariant();
                prefix = upper.StartsWith("KS") ? "KS" : "NH";
                if (!upper.Contains("VIP") && !upper.Contains("THƯỜNG"))
                {
                    c.CustomerType = prefix + "_Thường";
                }
            }

            string source = prefix == "KS" ? "Khách sạn" : "Nhà hàng";
            string tempCode = prefix + DateTime.Now.ToString("yyMMddHHmmssfff"); // <=20 ký tự

            // Nếu CCCD rỗng, sinh giá trị tạm ngắn gọn (<=20 ký tự) để tránh trùng UNIQUE constraint trên CCCD
            string cccdValue;
            if (string.IsNullOrWhiteSpace(c.CCCD))
            {
                long suffix = DateTime.UtcNow.Ticks % 1_000_000_000; // 9 chữ số
                cccdValue = "TMP" + suffix.ToString("D9"); // tổng 12 ký tự
            }
            else
            {
                cccdValue = c.CCCD;
            }

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(@"
                INSERT INTO dbo.Customers
                    (FullName, Nationality, CCCD, Sex,
                     PhoneNumber, Email, Address, CustomerType, TotalSpending, CustomerCode, Source)
                VALUES
                    (@FullName, @Nationality, @CCCD, @Sex,
                     @PhoneNumber, @Email, @Address, @CustomerType, @TotalSpending, @CustomerCode, @Source);
                SELECT SCOPE_IDENTITY();", conn))
            {
                cmd.Parameters.AddWithValue("@FullName", (object)c.FullName ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Nationality", (object)c.Nationality ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@CCCD", (object)cccdValue ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Sex", (object)c.Sex ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@PhoneNumber", (object)c.PhoneNumber ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Email", (object)c.Email ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Address", (object)c.Address ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@CustomerType", (object)c.CustomerType ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@TotalSpending", c.TotalSpending);
                cmd.Parameters.AddWithValue("@CustomerCode", tempCode);
                cmd.Parameters.AddWithValue("@Source", source);

                conn.Open();
                var idObj = cmd.ExecuteScalar();
                int newId = Convert.ToInt32(idObj);

                // cập nhật mã chuẩn sau khi có ID
                string finalCode = prefix + newId.ToString("D4");
                using (var cmdUpdate = new SqlCommand(
                    "UPDATE dbo.Customers SET CustomerCode = @Code WHERE CustomerId = @Id", conn))
                {
                    cmdUpdate.Parameters.AddWithValue("@Code", finalCode);
                    cmdUpdate.Parameters.AddWithValue("@Id", newId);
                    cmdUpdate.ExecuteNonQuery();
                }

                return newId;
            }
        }

        public void Update(Customer c)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(@"
                UPDATE dbo.Customers
                SET FullName     = @FullName,
                    Nationality  = @Nationality,
                    CCCD         = @CCCD,
                    Sex          = @Sex,
                    PhoneNumber  = @PhoneNumber,
                    Email        = @Email,
                    Address      = @Address,
                    CustomerType = @CustomerType,
                    TotalSpending = @TotalSpending
                WHERE CustomerId = @CustomerId;", conn))
            {
                cmd.Parameters.AddWithValue("@CustomerId", c.CustomerId);
                cmd.Parameters.AddWithValue("@FullName",     (object)c.FullName     ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Nationality",  (object)c.Nationality  ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@CCCD",         (object)c.CCCD         ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Sex",          (object)c.Sex          ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@PhoneNumber",  (object)c.PhoneNumber  ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Email",        (object)c.Email        ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Address",      (object)c.Address      ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@CustomerType", (object)c.CustomerType ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@TotalSpending", c.TotalSpending);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int customerId)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(
                "DELETE FROM dbo.Customers WHERE CustomerId = @CustomerId;", conn))
            {
                cmd.Parameters.AddWithValue("@CustomerId", customerId);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public Customer GetByFullName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName)) return null;

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(@"
                SELECT CustomerId, FullName, Nationality, CCCD, Sex,
                       PhoneNumber, Email, Address, CustomerType, CreatedAt, TotalSpending
                FROM dbo.Customers
                WHERE FullName = @FullName;", conn))
            {
                cmd.Parameters.AddWithValue("@FullName", fullName);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.Read()) return null;

                    int ordNationality = reader.GetOrdinal("Nationality");
                    int ordCCCD = reader.GetOrdinal("CCCD");
                    int ordSex = reader.GetOrdinal("Sex");
                    int ordPhone = reader.GetOrdinal("PhoneNumber");
                    int ordEmail = reader.GetOrdinal("Email");
                    int ordAddress = reader.GetOrdinal("Address");
                    int ordType = reader.GetOrdinal("CustomerType");
                    int ordCreated = reader.GetOrdinal("CreatedAt");
                    int ordSpending = reader.GetOrdinal("TotalSpending");

                    return new Customer
                    {
                        CustomerId = reader.GetInt32(0),
                        FullName = reader.GetString(1),
                        Nationality = reader.IsDBNull(ordNationality) ? null : reader.GetString(ordNationality),
                        CCCD = reader.IsDBNull(ordCCCD) ? null : reader.GetString(ordCCCD),
                        Sex = reader.IsDBNull(ordSex) ? null : reader.GetString(ordSex),
                        PhoneNumber = reader.IsDBNull(ordPhone) ? null : reader.GetString(ordPhone),
                        Email = reader.IsDBNull(ordEmail) ? null : reader.GetString(ordEmail),
                        Address = reader.IsDBNull(ordAddress) ? null : reader.GetString(ordAddress),
                        CustomerType = reader.IsDBNull(ordType) ? null : reader.GetString(ordType),
                        CreatedAt = reader.GetDateTime(ordCreated),
                        TotalSpending = reader.IsDBNull(ordSpending) ? 0m : reader.GetDecimal(ordSpending)
                    };
                }
            }
        }

        public void UpdateSpendingInfo(int customerId, decimal totalSpending, string customerType)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(@"
                UPDATE dbo.Customers
                SET TotalSpending = @TotalSpending,
                    CustomerType = @CustomerType
                WHERE CustomerId = @CustomerId;", conn))
            {
                cmd.Parameters.AddWithValue("@CustomerId", customerId);
                cmd.Parameters.AddWithValue("@TotalSpending", totalSpending);
                cmd.Parameters.AddWithValue("@CustomerType", (object)customerType ?? DBNull.Value);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
