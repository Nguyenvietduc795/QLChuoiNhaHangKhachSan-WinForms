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

            // Updated SQL to match the database schema: Rooms + RoomTypes
            const string sql = @"SELECT r.RoomId AS MaPhong, rt.TypeName AS LoaiPhong
                                 FROM dbo.Rooms r
                                 LEFT JOIN dbo.RoomTypes rt ON r.RoomTypeId = rt.RoomTypeId";

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                conn.Open();
                using (var rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        var code = rd["MaPhong"]?.ToString().Trim();
                        var type = rd["LoaiPhong"] != DBNull.Value ? rd["LoaiPhong"].ToString().Trim() : string.Empty;
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
