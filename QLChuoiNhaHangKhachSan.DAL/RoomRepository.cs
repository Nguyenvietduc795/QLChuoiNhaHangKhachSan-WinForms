using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace QLChuoiNhaHangKhachSan.DAL
{
    public class RoomRepository
    {
        private readonly string _connectionString;

        public RoomRepository(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentException("Connection string is required", nameof(connectionString));

            _connectionString = connectionString;
        }

        public List<(string RoomCode, string RoomType)> GetRooms()
        {
            var result = new List<(string, string)>();

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("SELECT MaPhong, LoaiPhong FROM Phong", conn))
            {
                conn.Open();
                using (var rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        var code = rd["MaPhong"]?.ToString().Trim();
                        var type = rd["LoaiPhong"]?.ToString().Trim();
                        if (!string.IsNullOrWhiteSpace(code))
                        {
                            result.Add((code, type));
                        }
                    }
                }
            }

            return result;
        }
    }
}
