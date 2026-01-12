using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using QLChuoiNhaHangKhachSan.DAL;

namespace QLChuoiNhaHangKhachSan.BLL
{
    /// <summary>
    /// Service x? lý logic nghi?p v? liên quan ??n món ?n
    /// </summary>
    public class FoodService
    {
        private readonly FoodRepository _foodRepository;

        public FoodService()
        {
            _foodRepository = new FoodRepository();
        }

        /// <summary>
        /// L?y t?t c? món ?n
        /// </summary>
        public List<FoodDTO> GetAllFoods()
        {
            return _foodRepository.GetAllFoods();
        }

        /// <summary>
        /// L?y t?t c? lo?i món ?n
        /// </summary>
        public List<FoodCategoryDTO> GetAllCategories()
        {
            return _foodRepository.GetAllCategories();
        }

        /// <summary>
        /// L?y t?t c? tr?ng thái món ?n
        /// </summary>
        public List<FoodStatusDTO> GetAllStatuses()
        {
            return _foodRepository.GetAllStatuses();
        }

        /// <summary>
        /// L?y món ?n theo ID
        /// </summary>
        public FoodDTO GetFoodById(int foodId)
        {
            return _foodRepository.GetFoodById(foodId);
        }

        /// <summary>
        /// Tìm ki?m và l?c món ?n
        /// </summary>
        public List<FoodDTO> SearchFoods(string searchTerm, string categoryFilter)
        {
            var allFoods = _foodRepository.GetAllFoods();

            var filtered = allFoods.AsEnumerable();

            // L?c theo lo?i
            if (!string.IsNullOrWhiteSpace(categoryFilter) && 
                !categoryFilter.Equals("T?t c?", StringComparison.OrdinalIgnoreCase))
            {
                string targetCategory = RemoveDiacritics(categoryFilter.Trim());
                filtered = filtered.Where(f => 
                    RemoveDiacritics(f.CategoryName ?? string.Empty)
                        .Equals(targetCategory, StringComparison.OrdinalIgnoreCase));
            }

            // L?c theo t? khóa tìm ki?m
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                string normalizedSearch = RemoveDiacritics(searchTerm.Trim());
                filtered = filtered.Where(f =>
                    RemoveDiacritics(f.FoodName ?? string.Empty).IndexOf(normalizedSearch, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    f.FoodID.ToString().Contains(searchTerm.Trim()));
            }

            return filtered.ToList();
        }

        /// <summary>
        /// Thêm món ?n m?i
        /// </summary>
        public int AddFood(string foodName, int categoryId, decimal price, int statusId)
        {
            if (string.IsNullOrWhiteSpace(foodName))
            {
                throw new ArgumentException("Vui lòng nh?p tên món.");
            }

            if (price < 0)
            {
                throw new ArgumentException("??n giá không h?p l?.");
            }

            return _foodRepository.InsertFood(foodName.Trim(), categoryId, price, statusId);
        }

        /// <summary>
        /// C?p nh?t món ?n
        /// </summary>
        public void UpdateFood(int foodId, string foodName, int categoryId, decimal price, int statusId)
        {
            var existing = _foodRepository.GetFoodById(foodId);
            if (existing == null)
            {
                throw new Exception("Không tìm th?y món c?n s?a.");
            }

            // N?u tr?ng thì gi? giá tr? c?
            string name = string.IsNullOrWhiteSpace(foodName) ? existing.FoodName : foodName.Trim();
            
            _foodRepository.UpdateFood(foodId, name, categoryId, price, statusId);
        }

        /// <summary>
        /// Xóa món ?n
        /// </summary>
        public void DeleteFood(int foodId)
        {
            _foodRepository.DeleteFood(foodId);
        }

        /// <summary>
        /// Ki?m tra món có ?ang bán không
        /// </summary>
        public bool IsFoodAvailable(FoodDTO food)
        {
            if (food == null) return false;

            string[] allowedNames = { "?ang bán", "?ang ph?c v?", "Dang ban", "Dang phuc vu" };
            return food.StatusID == 1 || 
                   allowedNames.Any(s => (food.StatusName ?? string.Empty).Equals(s, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Parse giá t? chu?i (h? tr? ??nh d?ng s? v?i d?u ph?y/ch?m)
        /// </summary>
        public bool TryParsePrice(string priceText, out decimal price)
        {
            price = 0;
            if (string.IsNullOrWhiteSpace(priceText)) return false;

            var rawPrice = priceText.Trim().Replace(".", string.Empty).Replace(",", string.Empty);
            return decimal.TryParse(rawPrice, out price) && price >= 0;
        }

        private static string RemoveDiacritics(string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;
            var normalized = text.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            foreach (var ch in normalized)
            {
                var cat = CharUnicodeInfo.GetUnicodeCategory(ch);
                if (cat != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(ch);
                }
            }
            return sb.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
