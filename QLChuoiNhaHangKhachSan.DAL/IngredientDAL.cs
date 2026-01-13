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
            const string sql = @"SELECT IngredientID,
                                         IngredientCode,
                                         IngredientName,
                                         Unit,
                                         DefaultPrice,
                                         StockQuantity,
                                         MinStock
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

        public (int NewId, string NewCode) CreateIngredient(string name, string unit, decimal defaultPrice, decimal stockQuantity, decimal minStock)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("dbo.usp_Ingredient_Create", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IngredientName", name);
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

        public DataTable GetActiveIngredientList()
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("dbo.usp_Ingredient_GetActiveList", conn))
            using (var adapter = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                var dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        public DataTable GetIngredientById(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("dbo.usp_Ingredient_GetById", conn))
            using (var adapter = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IngredientID", id);
                var dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        public void UpdateIngredient(int id, string name, string unit, decimal defaultPrice, decimal stockQty, decimal minStock)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("dbo.usp_Ingredient_Update", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IngredientID", id);
                cmd.Parameters.AddWithValue("@IngredientName", name);
                cmd.Parameters.AddWithValue("@Unit", unit);
                cmd.Parameters.AddWithValue("@DefaultPrice", defaultPrice);
                cmd.Parameters.AddWithValue("@StockQuantity", stockQty);
                cmd.Parameters.AddWithValue("@MinStock", minStock);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void StopIngredient(int ingredientId)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("dbo.usp_Ingredient_Stop", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IngredientID", ingredientId);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
