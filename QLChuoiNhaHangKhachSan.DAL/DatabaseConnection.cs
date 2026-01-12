using System;
using System.Data.SqlClient;

namespace QLChuoiNhaHangKhachSan.DAL
{
    /// <summary>
    /// L?p qu?n lý k?t n?i database t?p trung (Singleton pattern).
    /// Ch? c?n g?i DatabaseConnection.ConnectionString m?t l?n ?? l?y connection string.
    /// </summary>
    public static class DatabaseConnection
    {
        // Connection string m?c ??nh cho database QuanLyChuoiNhaHangKhachSan
        private static string _connectionString = @"Data Source=.;Initial Catalog=QuanLyChuoiNhaHangKhachSan;Integrated Security=True";
        private static readonly object _lock = new object();

        /// <summary>
        /// Connection string hi?n t?i.
        /// </summary>
        public static string ConnectionString
        {
            get
            {
                lock (_lock)
                {
                    return _connectionString;
                }
            }
        }

        /// <summary>
        /// Thi?t l?p connection string th? công.
        /// Nên g?i t? GUI layer khi kh?i ??ng ?ng d?ng n?u c?n override.
        /// </summary>
        public static void SetConnectionString(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentException("Connection string không ???c ?? tr?ng", nameof(connectionString));

            lock (_lock)
            {
                _connectionString = connectionString;
            }
        }

        /// <summary>
        /// T?o và tr? v? m?t SqlConnection m?i (ch?a m?).
        /// Caller có trách nhi?m m? và ?óng connection.
        /// </summary>
        public static SqlConnection CreateConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        /// <summary>
        /// Ki?m tra k?t n?i database có ho?t ??ng không.
        /// </summary>
        public static bool TestConnection()
        {
            try
            {
                using (var conn = CreateConnection())
                {
                    conn.Open();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
