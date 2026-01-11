using System;
using System.Data;
using System.Data.SqlClient;

namespace QLChuoiNhaHangKhachSan.DAL
{
    public class StockDAL
    {
        private readonly string _connStr;

        public StockDAL(string connStr)
        {
            _connStr = connStr;
        }

        public DataTable GetStockList(string warehouseType, string keyword)
        {
            using (var conn = new SqlConnection(_connStr))
            using (var cmd = new SqlCommand("usp_Stock_GetList", conn))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@WarehouseType", (object)warehouseType ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Keyword", string.IsNullOrWhiteSpace(keyword) ? (object)DBNull.Value : keyword);

                var dt = new DataTable();
                conn.Open();
                da.Fill(dt);
                return dt;
            }
        }

        public int GetTotalStockQuantity()
        {
            return ExecuteScalarInt("usp_KPI_TotalStockQuantity");
        }

        public int GetLowStockCount()
        {
            return ExecuteScalarInt("usp_KPI_LowStockCount");
        }

        public int GetStableStockCount()
        {
            return ExecuteScalarInt("usp_KPI_StableStockCount");
        }

        private int ExecuteScalarInt(string storedProcedure)
        {
            using (var conn = new SqlConnection(_connStr))
            using (var cmd = new SqlCommand(storedProcedure, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                var result = cmd.ExecuteScalar();
                if (result == null || result == DBNull.Value) return 0;
                return Convert.ToInt32(result);
            }
        }
    }
}
