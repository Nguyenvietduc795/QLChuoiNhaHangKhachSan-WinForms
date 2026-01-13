using System.Data;
using System.Data.SqlClient;

namespace QLChuoiNhaHangKhachSan.DAL
{
    public class EquipmentDAL
    {
        private readonly string _connectionString;

        public EquipmentDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DataTable GetEquipmentForImport()
        {
            var dt = new DataTable();
            const string sql = @"SELECT EquipmentID, EquipmentCode, EquipmentName, Unit, DefaultPrice
                                 FROM dbo.Equipment
                                 WHERE IsActive = 1
                                 ORDER BY EquipmentCode";
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            using (var adapter = new SqlDataAdapter(cmd))
            {
                adapter.Fill(dt);
            }
            return dt;
        }

        public DataTable GetEquipmentForExport()
        {
            var dt = new DataTable();
            const string sql = @"SELECT EquipmentID,
                                         EquipmentCode,
                                         EquipmentName,
                                         Unit,
                                         DefaultPrice,
                                         StockQuantity,
                                         MinStock
                                 FROM dbo.Equipment
                                 WHERE IsActive = 1 AND StockQuantity > 0
                                 ORDER BY EquipmentCode";
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            using (var adapter = new SqlDataAdapter(cmd))
            {
                adapter.Fill(dt);
            }
            return dt;
        }

        public (int NewId, string NewCode) CreateEquipment(string name, string unit, decimal defaultPrice, decimal stockQuantity, decimal minStock)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("dbo.usp_Equipment_Create", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EquipmentName", name);
                cmd.Parameters.AddWithValue("@Unit", unit);
                cmd.Parameters.AddWithValue("@DefaultPrice", defaultPrice);
                cmd.Parameters.AddWithValue("@StockQuantity", stockQuantity);
                cmd.Parameters.AddWithValue("@MinStock", minStock);

                var idParam = new SqlParameter("@NewId", SqlDbType.Int) { Direction = ParameterDirection.Output };
                var codeParam = new SqlParameter("@NewCode", SqlDbType.NVarChar, 20) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(idParam);
                cmd.Parameters.Add(codeParam);

                conn.Open();
                cmd.ExecuteNonQuery();

                return ((int)idParam.Value, (string)codeParam.Value);
            }
        }

        public DataTable GetActiveEquipmentList()
        {
            const string sql = @"SELECT EquipmentID,
                                         EquipmentCode,
                                         EquipmentName,
                                         Unit,
                                         DefaultPrice,
                                         StockQuantity,
                                         MinStock
                                  FROM dbo.Equipment
                                  WHERE IsActive = 1
                                  ORDER BY EquipmentCode";

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            using (var adapter = new SqlDataAdapter(cmd))
            {
                var dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        public DataTable GetEquipmentById(int id)
        {
            const string sql = @"SELECT EquipmentID,
                                         EquipmentCode,
                                         EquipmentName,
                                         Unit,
                                         DefaultPrice,
                                         StockQuantity,
                                         MinStock,
                                         IsActive
                                  FROM dbo.Equipment
                                  WHERE EquipmentID = @EquipmentID";

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            using (var adapter = new SqlDataAdapter(cmd))
            {
                cmd.Parameters.AddWithValue("@EquipmentID", id);
                var dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        public void UpdateEquipment(int id, string name, string unit, decimal defaultPrice, decimal stockQty, decimal minStock)
        {
            const string sql = @"UPDATE dbo.Equipment
SET EquipmentName = @EquipmentName,
    Unit = @Unit,
    DefaultPrice = @DefaultPrice,
    StockQuantity = @StockQuantity,
    MinStock = @MinStock
WHERE EquipmentID = @EquipmentID";

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@EquipmentID", id);
                cmd.Parameters.AddWithValue("@EquipmentName", name);
                cmd.Parameters.AddWithValue("@Unit", unit);
                cmd.Parameters.AddWithValue("@DefaultPrice", defaultPrice);
                cmd.Parameters.AddWithValue("@StockQuantity", stockQty);
                cmd.Parameters.AddWithValue("@MinStock", minStock);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void StopEquipment(int equipmentId)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("dbo.usp_Equipment_Stop", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EquipmentID", equipmentId);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
