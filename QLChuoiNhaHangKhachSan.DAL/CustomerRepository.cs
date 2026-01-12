using System;
using System.Data.SqlClient;

namespace QLChuoiNhaHangKhachSan.DAL
{
    /// <summary>
    /// Repository x? lý truy v?n d? li?u khách hàng
    /// </summary>
    public class CustomerRepository : BaseRepository
    {
        public CustomerRepository() : base() { }

        /// <summary>
        /// Tìm khách hàng theo s? ?i?n tho?i
        /// </summary>
        public CustomerDTO FindByPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone)) return null;

            string query = "SELECT CustomerId, FullName, PhoneNumber, Email, CreatedAt FROM Customers WHERE PhoneNumber = @Phone";

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Phone", phone.Trim());
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapCustomer(reader);
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Tìm khách hàng theo email
        /// </summary>
        public CustomerDTO FindByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return null;

            string query = "SELECT CustomerId, FullName, PhoneNumber, Email, CreatedAt FROM Customers WHERE Email = @Email";

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Email", email.Trim());
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapCustomer(reader);
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// L?y khách hàng theo ID
        /// </summary>
        public CustomerDTO GetById(int customerId)
        {
            string query = "SELECT CustomerId, FullName, PhoneNumber, Email, CreatedAt FROM Customers WHERE CustomerId = @CustomerId";

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@CustomerId", customerId);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapCustomer(reader);
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Thêm khách hàng m?i
        /// </summary>
        public int InsertCustomer(string fullName, string phone, string email)
        {
            string query = @"INSERT INTO Customers (FullName, PhoneNumber, Email, CreatedAt)
                             VALUES (@FullName, @Phone, @Email, GETDATE());
                             SELECT SCOPE_IDENTITY();";

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@FullName", string.IsNullOrWhiteSpace(fullName) ? (object)DBNull.Value : fullName);
                cmd.Parameters.AddWithValue("@Phone", string.IsNullOrWhiteSpace(phone) ? (object)DBNull.Value : phone.Trim());
                cmd.Parameters.AddWithValue("@Email", string.IsNullOrWhiteSpace(email) ? (object)DBNull.Value : email.Trim());

                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        /// <summary>
        /// Tìm ho?c t?o khách hàng theo S?T/Email
        /// </summary>
        public int FindOrCreate(string fullName, string phone, string email)
        {
            // ?u tiên tìm theo S?T
            var customer = FindByPhone(phone);
            if (customer != null) return customer.CustomerID;

            // N?u không có S?T ho?c không tìm th?y, th? tìm theo Email
            customer = FindByEmail(email);
            if (customer != null) return customer.CustomerID;

            // Không tìm th?y => t?o m?i
            return InsertCustomer(fullName, phone, email);
        }

        /// <summary>
        /// L?y thông tin khách hàng theo TableID (t? OrderTicket ho?c BookingsTable)
        /// </summary>
        public CustomerDTO GetCustomerByTable(int tableId)
        {
            // ?u tiên OrderTicket m?i nh?t có CustomerID
            string query = @"SELECT TOP 1 c.CustomerId, c.FullName, c.PhoneNumber, c.Email, c.CreatedAt
                             FROM OrderTicket o
                             INNER JOIN Customers c ON o.CustomerID = c.CustomerId
                             WHERE o.TableID = @TableID AND o.CustomerID IS NOT NULL
                             ORDER BY o.OrderId DESC";

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@TableID", tableId);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapCustomer(reader);
                    }
                }
            }

            // N?u không có OrderTicket có khách, l?y booking m?i nh?t có CustomerID
            query = @"SELECT TOP 1 c.CustomerId, c.FullName, c.PhoneNumber, c.Email, c.CreatedAt
                      FROM BookingsTable b
                      INNER JOIN Customers c ON b.CustomerID = c.CustomerId
                      WHERE b.TableID = @TableID AND b.CustomerID IS NOT NULL
                      ORDER BY b.BookingID DESC";

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@TableID", tableId);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapCustomer(reader);
                    }
                }
            }

            return null;
        }

        private CustomerDTO MapCustomer(SqlDataReader reader)
        {
            return new CustomerDTO
            {
                CustomerID = Convert.ToInt32(reader["CustomerId"]),
                FullName = reader["FullName"] == DBNull.Value ? null : reader["FullName"].ToString(),
                PhoneNumber = reader["PhoneNumber"] == DBNull.Value ? null : reader["PhoneNumber"].ToString(),
                Email = reader["Email"] == DBNull.Value ? null : reader["Email"].ToString(),
                CreatedAt = reader["CreatedAt"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["CreatedAt"])
            };
        }
    }
}
