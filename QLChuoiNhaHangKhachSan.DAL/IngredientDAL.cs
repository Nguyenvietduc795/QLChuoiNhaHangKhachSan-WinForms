using System.Data;
using System.Data.SqlClient;

namespace QLChuoiNhaHangKhachSan.DAL
{
    public class IngredientDAL
    {
        private readonly string _connectionString;

        public IngredientDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DataTable GetActiveIngredientsForCombo()
        {
            var dt = new DataTable();

            const string sql = @"SELECT IngredientID, IngredientCode, IngredientName, Unit, DefaultPrice
                                 FROM dbo.Ingredient
                                 WHERE IsActive = 1
                                 ORDER BY IngredientCode";

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            using (var adapter = new SqlDataAdapter(cmd))
            {
                adapter.Fill(dt);
            }

            return dt;
        }

        public DataTable GetActiveIngredientsForExport()
        {
            var dt = new DataTable();
            const string sql = @"SELECT IngredientID, IngredientCode, IngredientName, Unit, DefaultPrice, StockQuantity
                                 FROM dbo.Ingredient
                                 WHERE IsActive = 1 AND StockQuantity > 0
                                 ORDER BY IngredientCode";
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
