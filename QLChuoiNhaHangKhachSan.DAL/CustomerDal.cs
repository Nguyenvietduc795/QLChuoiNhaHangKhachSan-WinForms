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
                ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
        }

        public List<Customer> GetAll()
        {
            var result = new List<Customer>();

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(@"
                SELECT CustomerId, FullName, Nationality, CCCD, Sex,
                       PhoneNumber, Email, Address, CustomerType, CreatedAt
                FROM dbo.Customers;", conn))
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var c = new Customer
                        {
                            CustomerId   = reader.GetInt32(0),
                            FullName     = reader.GetString(1),
                            Nationality  = reader.IsDBNull(2) ? null : reader.GetString(2),
                            CCCD         = reader.IsDBNull(3) ? null : reader.GetString(3),
                            Sex          = reader.IsDBNull(4) ? null : reader.GetString(4),
                            PhoneNumber  = reader.IsDBNull(5) ? null : reader.GetString(5),
                            Email        = reader.IsDBNull(6) ? null : reader.GetString(6),
                            Address      = reader.IsDBNull(7) ? null : reader.GetString(7),
                            CustomerType = reader.IsDBNull(8) ? null : reader.GetString(8),
                            CreatedAt    = reader.GetDateTime(9)
                        };
                        result.Add(c);
                    }
                }
            }

            return result;
        }

        public int Insert(Customer c)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(@"
                INSERT INTO dbo.Customers
                    (FullName, Nationality, CCCD, Sex,
                     PhoneNumber, Email, Address, CustomerType)
                VALUES
                    (@FullName, @Nationality, @CCCD, @Sex,
                     @PhoneNumber, @Email, @Address, @CustomerType);
                SELECT SCOPE_IDENTITY();", conn))
            {
                cmd.Parameters.AddWithValue("@FullName", (object)c.FullName ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Nationality", (object)c.Nationality ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@CCCD", (object)c.CCCD ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Sex", (object)c.Sex ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@PhoneNumber", (object)c.PhoneNumber ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Email", (object)c.Email ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Address", (object)c.Address ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@CustomerType", (object)c.CustomerType ?? DBNull.Value);

                conn.Open();
                var idObj = cmd.ExecuteScalar();
                return Convert.ToInt32(idObj);
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
                    CustomerType = @CustomerType
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
    }
}
