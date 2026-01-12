using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace QLChuoiNhaHangKhachSan.DAL
{
    /// <summary>
    /// Repository x? lý truy v?n d? li?u món ?n
    /// </summary>
    public class FoodRepository : BaseRepository
    {
        public FoodRepository() : base() { }

        /// <summary>
        /// L?y t?t c? món ?n
        /// </summary>
        public List<FoodDTO> GetAllFoods()
        {
            var foods = new List<FoodDTO>();
            string query = @"SELECT f.FoodID, f.FoodName, f.CategoryID, c.CategoryName, 
                                    f.Price, f.StatusID, s.StatusName
                             FROM Food f
                             LEFT JOIN FoodCategory c ON f.CategoryID = c.CategoryID
                             LEFT JOIN FoodStatus s ON f.StatusID = s.StatusID";

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        foods.Add(MapFood(reader));
                    }
                }
            }

            return foods;
        }

        /// <summary>
        /// L?y t?t c? lo?i món ?n
        /// </summary>
        public List<FoodCategoryDTO> GetAllCategories()
        {
            var categories = new List<FoodCategoryDTO>();
            string query = "SELECT CategoryID, CategoryName FROM FoodCategory ORDER BY CategoryName";

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        categories.Add(new FoodCategoryDTO
                        {
                            CategoryID = reader.GetInt32(0),
                            CategoryName = reader.GetString(1)
                        });
                    }
                }
            }

            return categories;
        }

        /// <summary>
        /// L?y t?t c? tr?ng thái món ?n
        /// </summary>
        public List<FoodStatusDTO> GetAllStatuses()
        {
            var statuses = new List<FoodStatusDTO>();
            string query = "SELECT StatusID, StatusName FROM FoodStatus ORDER BY StatusID";

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        statuses.Add(new FoodStatusDTO
                        {
                            StatusID = reader.GetInt32(0),
                            StatusName = reader.GetString(1)
                        });
                    }
                }
            }

            return statuses;
        }

        /// <summary>
        /// Thêm món ?n m?i
        /// </summary>
        public int InsertFood(string foodName, int categoryId, decimal price, int statusId)
        {
            string query = @"INSERT INTO Food (FoodName, CategoryID, Price, StatusID)
                             VALUES (@FoodName, @CategoryID, @Price, @StatusID);
                             SELECT SCOPE_IDENTITY();";

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@FoodName", foodName);
                cmd.Parameters.AddWithValue("@CategoryID", categoryId);
                cmd.Parameters.AddWithValue("@Price", price);
                cmd.Parameters.AddWithValue("@StatusID", statusId);

                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        /// <summary>
        /// C?p nh?t món ?n
        /// </summary>
        public void UpdateFood(int foodId, string foodName, int categoryId, decimal price, int statusId)
        {
            string query = @"UPDATE Food
                             SET FoodName = @FoodName,
                                 CategoryID = @CategoryID,
                                 Price = @Price,
                                 StatusID = @StatusID
                             WHERE FoodID = @FoodID;";

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@FoodName", foodName);
                cmd.Parameters.AddWithValue("@CategoryID", categoryId);
                cmd.Parameters.AddWithValue("@Price", price);
                cmd.Parameters.AddWithValue("@StatusID", statusId);
                cmd.Parameters.AddWithValue("@FoodID", foodId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Xóa món ?n
        /// </summary>
        public void DeleteFood(int foodId)
        {
            string query = "DELETE FROM Food WHERE FoodID = @FoodID";

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@FoodID", foodId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// L?y món ?n theo ID
        /// </summary>
        public FoodDTO GetFoodById(int foodId)
        {
            string query = @"SELECT f.FoodID, f.FoodName, f.CategoryID, c.CategoryName, 
                                    f.Price, f.StatusID, s.StatusName
                             FROM Food f
                             LEFT JOIN FoodCategory c ON f.CategoryID = c.CategoryID
                             LEFT JOIN FoodStatus s ON f.StatusID = s.StatusID
                             WHERE f.FoodID = @FoodID";

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@FoodID", foodId);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapFood(reader);
                    }
                }
            }

            return null;
        }

        private FoodDTO MapFood(SqlDataReader reader)
        {
            return new FoodDTO
            {
                FoodID = reader.GetInt32(reader.GetOrdinal("FoodID")),
                FoodName = reader.GetString(reader.GetOrdinal("FoodName")),
                CategoryID = reader.IsDBNull(reader.GetOrdinal("CategoryID")) ? 0 : reader.GetInt32(reader.GetOrdinal("CategoryID")),
                CategoryName = reader.IsDBNull(reader.GetOrdinal("CategoryName")) ? string.Empty : reader.GetString(reader.GetOrdinal("CategoryName")),
                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                StatusID = reader.IsDBNull(reader.GetOrdinal("StatusID")) ? 0 : reader.GetInt32(reader.GetOrdinal("StatusID")),
                StatusName = reader.IsDBNull(reader.GetOrdinal("StatusName")) ? string.Empty : reader.GetString(reader.GetOrdinal("StatusName"))
            };
        }
    }
}
