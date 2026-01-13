namespace QLChuoiNhaHangKhachSan.DTO
{
    public class InventoryItemDetailDTO
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public decimal DefaultPrice { get; set; }
        public decimal StockQuantity { get; set; }
        public decimal MinStock { get; set; }
        public WarehouseType Type { get; set; }

        public string DisplayLabel
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Code)) return Name ?? string.Empty;
                if (string.IsNullOrWhiteSpace(Name)) return Code;
                return Code.Trim() + " - " + Name.Trim();
            }
        }
    }
}
