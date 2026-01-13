using System;

namespace QLChuoiNhaHangKhachSan.DAL.DTO
{
    public class IngredientDTO
    {
        public int IngredientID { get; set; }
        public string IngredientCode { get; set; }
        public string IngredientName { get; set; }
        public string Unit { get; set; }
        public decimal DefaultPrice { get; set; }
        public decimal StockQuantity { get; set; }

        public string DisplayText
        {
            get { return $"{IngredientCode} : {IngredientName} ({Unit})"; }
        }
    }
}
