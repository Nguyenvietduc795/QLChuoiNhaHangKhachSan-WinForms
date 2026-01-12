using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace QLChuoiNhaHangKhachSan.DAL
{
    /// <summary>
    /// Repository x? lý truy v?n d? li?u ??t bàn
    /// </summary>
    public class BookingRepository : BaseRepository
    {
        public BookingRepository() : base() { }

        /// <summary>
        /// L?y ??t bàn m?i nh?t (có th? l?c theo tr?ng thái)
        /// </summary>
        public BookingDTO GetLatestBooking(string status = null)
        {
            string query = @"SELECT TOP 1 
                                b.BookingID,
                                b.TableID,
                                rt.TableName,
                                b.CustomerID,
                                c.FullName,
                                c.PhoneNumber,
                                c.Email,
                                b.BookingDate,
                                b.BookingTime,
                                b.GuestCount,
                                b.Status,
                                b.CreatedDate
                            FROM BookingsTable b
                            LEFT JOIN RestaurantTable rt ON b.TableID = rt.TableID
                            LEFT JOIN Customers c ON b.CustomerID = c.CustomerId";

            if (!string.IsNullOrEmpty(status))
            {
                query += " WHERE b.Status = @Status";
            }

            query += " ORDER BY b.BookingID DESC";

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(query, conn))
            {
                if (!string.IsNullOrEmpty(status))
                {
                    cmd.Parameters.AddWithValue("@Status", status);
                }

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapBooking(reader);
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// L?y danh sách ??t bàn theo bàn
        /// </summary>
        public List<BookingDTO> GetBookingsByTable(int tableId)
        {
            var bookings = new List<BookingDTO>();
            string query = @"SELECT 
                                b.BookingID,
                                b.TableID,
                                rt.TableName,
                                b.CustomerID,
                                c.FullName,
                                c.PhoneNumber,
                                c.Email,
                                b.BookingDate,
                                b.BookingTime,
                                b.GuestCount,
                                b.Status,
                                b.CreatedDate
                            FROM BookingsTable b
                            LEFT JOIN RestaurantTable rt ON b.TableID = rt.TableID
                            LEFT JOIN Customers c ON b.CustomerID = c.CustomerId
                            WHERE b.TableID = @TableID
                            ORDER BY b.BookingID DESC";

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@TableID", tableId);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        bookings.Add(MapBooking(reader));
                    }
                }
            }

            return bookings;
        }

        /// <summary>
        /// Thêm m?i ??t bàn
        /// </summary>
        public int InsertBooking(int tableId, int customerId, DateTime bookingDate, TimeSpan bookingTime, int guestCount, string status)
        {
            string query = @"INSERT INTO BookingsTable (TableID, CustomerID, BookingDate, BookingTime, GuestCount, Status)
                             VALUES (@TableID, @CustomerID, @BookingDate, @BookingTime, @GuestCount, @Status);
                             SELECT SCOPE_IDENTITY();";

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@TableID", tableId);
                cmd.Parameters.AddWithValue("@CustomerID", customerId);
                cmd.Parameters.Add("@BookingDate", SqlDbType.Date).Value = bookingDate;
                cmd.Parameters.Add("@BookingTime", SqlDbType.Time).Value = bookingTime;
                cmd.Parameters.AddWithValue("@GuestCount", guestCount);
                cmd.Parameters.AddWithValue("@Status", status);

                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        /// <summary>
        /// C?p nh?t tr?ng thái ??t bàn
        /// </summary>
        public void UpdateBookingStatus(int tableId, string oldStatus, string newStatus)
        {
            string query = "UPDATE BookingsTable SET Status = @NewStatus WHERE TableID = @TableID AND Status = @OldStatus";

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@NewStatus", newStatus);
                cmd.Parameters.AddWithValue("@TableID", tableId);
                cmd.Parameters.AddWithValue("@OldStatus", oldStatus);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// H?y ??t bàn theo TableID
        /// </summary>
        public void CancelBooking(int tableId)
        {
            UpdateBookingStatus(tableId, "?ã ??t", "?ã h?y");
        }

        private BookingDTO MapBooking(SqlDataReader reader)
        {
            var booking = new BookingDTO
            {
                BookingID = Convert.ToInt32(reader["BookingID"]),
                TableID = Convert.ToInt32(reader["TableID"]),
                TableName = reader["TableName"] as string,
                CustomerID = reader["CustomerID"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["CustomerID"]),
                CustomerName = reader["FullName"] == DBNull.Value ? null : reader["FullName"].ToString(),
                PhoneNumber = reader["PhoneNumber"] == DBNull.Value ? null : reader["PhoneNumber"].ToString(),
                Email = reader["Email"] == DBNull.Value ? null : reader["Email"].ToString(),
                BookingDate = reader.GetDateTime(reader.GetOrdinal("BookingDate")),
                BookingTime = reader.GetTimeSpan(reader.GetOrdinal("BookingTime")),
                GuestCount = Convert.ToInt32(reader["GuestCount"]),
                Status = reader["Status"] == DBNull.Value ? null : reader["Status"].ToString(),
                CreatedDate = reader["CreatedDate"] == DBNull.Value ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("CreatedDate"))
            };

            return booking;
        }
    }
}
