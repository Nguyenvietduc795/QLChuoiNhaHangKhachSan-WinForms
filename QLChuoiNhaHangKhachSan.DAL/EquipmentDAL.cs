using System.Configuration;
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
            const string sql = @"SELECT EquipmentID, EquipmentCode, EquipmentName, Unit, DefaultPrice, StockQuantity
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
    }
}
