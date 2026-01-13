namespace QLChuoiNhaHangKhachSan.DTO
{
    public class SelectItemDTO
    {
        public int ItemID { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }

        public int IngredientID { get; set; }
        public string IngredientCode { get; set; }
        public string IngredientName { get; set; }
        public string Unit { get; set; }
        public decimal StockQuantity { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal MinStock { get; set; }
        public decimal Total => Quantity * UnitPrice;
        public string DisplayItem => $"{ItemCode ?? IngredientCode} : {ItemName ?? IngredientName} ({Unit})";
    }
}