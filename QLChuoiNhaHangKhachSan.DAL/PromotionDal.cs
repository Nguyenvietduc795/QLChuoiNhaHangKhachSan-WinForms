using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using QLChuoiNhaHangKhachSan.DAL.Models;

namespace QLChuoiNhaHangKhachSan.DAL
{
    // Repository làm việc trực tiếp với bảng Promotions trong database
    public class PromotionDal
    {
        private readonly string _connectionString;

        public PromotionDal()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
        }

        public List<Promotion> GetAll()
        {
            var result = new List<Promotion>();

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(@"
                SELECT PromotionId, PromotionCode, ProgramName, PromotionType,
                       TargetAudience, ExpirationDate, Status, CreatedAt
                FROM dbo.Promotions;", conn))
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var p = new Promotion
                        {
                            PromotionId    = reader.GetInt32(0),
                            PromotionCode  = reader.IsDBNull(1) ? null : reader.GetString(1),
                            ProgramName    = reader.GetString(2),
                            PromotionType  = reader.GetString(3),
                            TargetAudience = reader.IsDBNull(4) ? null : reader.GetString(4),
                            ExpirationDate = reader.IsDBNull(5) ? (DateTime?)null : reader.GetDateTime(5),
                            Status         = reader.GetString(6),
                            CreatedAt      = reader.GetDateTime(7)
                        };
                        result.Add(p);
                    }
                }
            }

            return result;
        }

        public int Insert(Promotion p)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(@"
                INSERT INTO dbo.Promotions
                    (PromotionCode, ProgramName, PromotionType, TargetAudience,
                     ExpirationDate, Status)
                VALUES
                    (@PromotionCode, @ProgramName, @PromotionType, @TargetAudience,
                     @ExpirationDate, @Status);
                SELECT SCOPE_IDENTITY();", conn))
            {
                cmd.Parameters.AddWithValue("@PromotionCode", (object)p.PromotionCode ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ProgramName", (object)p.ProgramName ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@PromotionType", (object)p.PromotionType ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@TargetAudience", (object)p.TargetAudience ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ExpirationDate",
                    (object)(p.ExpirationDate ?? (object)DBNull.Value));
                cmd.Parameters.AddWithValue("@Status", (object)p.Status ?? DBNull.Value);

                conn.Open();
                var idObj = cmd.ExecuteScalar();
                return Convert.ToInt32(idObj);
            }
        }

        public void Update(Promotion p)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(@"
                UPDATE dbo.Promotions
                SET PromotionCode  = @PromotionCode,
                    ProgramName    = @ProgramName,
                    PromotionType  = @PromotionType,
                    TargetAudience = @TargetAudience,
                    ExpirationDate = @ExpirationDate,
                    Status         = @Status
                WHERE PromotionId  = @PromotionId;", conn))
            {
                cmd.Parameters.AddWithValue("@PromotionId", p.PromotionId);
                cmd.Parameters.AddWithValue("@PromotionCode", (object)p.PromotionCode ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ProgramName", (object)p.ProgramName ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@PromotionType", (object)p.PromotionType ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@TargetAudience", (object)p.TargetAudience ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ExpirationDate",
                    (object)(p.ExpirationDate ?? (object)DBNull.Value));
                cmd.Parameters.AddWithValue("@Status", (object)p.Status ?? DBNull.Value);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int promotionId)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(
                "DELETE FROM dbo.Promotions WHERE PromotionId = @PromotionId;", conn))
            {
                cmd.Parameters.AddWithValue("@PromotionId", promotionId);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
