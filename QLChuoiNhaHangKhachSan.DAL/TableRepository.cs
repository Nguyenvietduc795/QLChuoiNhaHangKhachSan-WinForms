using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace QLChuoiNhaHangKhachSan.DAL
{
    /// <summary>
    /// Repository x? lý truy v?n d? li?u bàn
    /// </summary>
    public class TableRepository : BaseRepository
    {
        public TableRepository() : base() { }

        /// <summary>
        /// L?y t?t c? bàn v?i tr?ng thái
        /// </summary>
        public List<TableDTO> GetAllTables()
        {
            var tables = new List<TableDTO>();
            string query = @"SELECT rt.TableID, rt.TableName, rt.StatusID, ts.StatusName
                             FROM RestaurantTable rt
                             INNER JOIN TableStatus ts ON rt.StatusID = ts.StatusID";

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tables.Add(new TableDTO
                        {
                            TableID = reader.GetInt32(0),
                            TableName = reader.GetString(1),
                            StatusID = reader.GetInt32(2),
                            StatusName = reader.GetString(3)
                        });
                    }
                }
            }

            return tables;
        }

        /// <summary>
        /// L?y bàn theo tên
        /// </summary>
        public TableDTO GetTableByName(string tableName)
        {
            string query = @"SELECT rt.TableID, rt.TableName, rt.StatusID, ts.StatusName
                             FROM RestaurantTable rt
                             INNER JOIN TableStatus ts ON rt.StatusID = ts.StatusID
                             WHERE rt.TableName = @TableName OR rt.TableName = N'Bàn ' + @TableNumber";

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@TableName", tableName);
                cmd.Parameters.AddWithValue("@TableNumber", tableName);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new TableDTO
                        {
                            TableID = reader.GetInt32(0),
                            TableName = reader.GetString(1),
                            StatusID = reader.GetInt32(2),
                            StatusName = reader.GetString(3)
                        };
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// L?y bàn theo ID
        /// </summary>
        public TableDTO GetTableById(int tableId)
        {
            string query = @"SELECT rt.TableID, rt.TableName, rt.StatusID, ts.StatusName
                             FROM RestaurantTable rt
                             INNER JOIN TableStatus ts ON rt.StatusID = ts.StatusID
                             WHERE rt.TableID = @TableID";

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@TableID", tableId);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new TableDTO
                        {
                            TableID = reader.GetInt32(0),
                            TableName = reader.GetString(1),
                            StatusID = reader.GetInt32(2),
                            StatusName = reader.GetString(3)
                        };
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// C?p nh?t tr?ng thái bàn
        /// </summary>
        public void UpdateTableStatus(int tableId, int statusId)
        {
            string query = "UPDATE RestaurantTable SET StatusID = @StatusID WHERE TableID = @TableID";

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@StatusID", statusId);
                cmd.Parameters.AddWithValue("@TableID", tableId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// C?p nh?t tr?ng thái bàn (s? d?ng connection và transaction t? ngoài)
        /// </summary>
        public void UpdateTableStatus(SqlConnection conn, SqlTransaction tran, int tableId, int statusId)
        {
            string query = "UPDATE RestaurantTable SET StatusID = @StatusID WHERE TableID = @TableID";

            using (var cmd = new SqlCommand(query, conn, tran))
            {
                cmd.Parameters.AddWithValue("@StatusID", statusId);
                cmd.Parameters.AddWithValue("@TableID", tableId);
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// ??t bàn v? tr?ng thái tr?ng (StatusID = 1)
        /// </summary>
        public void SetTableEmpty(int tableId)
        {
            UpdateTableStatus(tableId, 1);
        }

        /// <summary>
        /// ??t bàn v? tr?ng thái có khách (StatusID = 2)
        /// </summary>
        public void SetTableOccupied(int tableId)
        {
            UpdateTableStatus(tableId, 2);
        }

        /// <summary>
        /// ??t bàn v? tr?ng thái ?ã ??t (StatusID = 3)
        /// </summary>
        public void SetTableReserved(int tableId)
        {
            UpdateTableStatus(tableId, 3);
        }
    }
}
