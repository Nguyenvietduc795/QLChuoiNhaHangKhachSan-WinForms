namespace QLChuoiNhaHangKhachSan.DTO
{
    public class WarehouseVoucherDTO
    {
        public int VoucherID { get; set; }
        public string VoucherCode { get; set; }
        public string VoucherType { get; set; }  // IMPORT | EXPORT
        public string WarehouseType { get; set; } // INGREDIENT | EQUIPMENT
        public string UnitCode { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
        public System.DateTime CreatedAt { get; set; }
    }

    public class WarehouseVoucherDetailDTO
    {
        public int VoucherDetailID { get; set; }
        public int VoucherID { get; set; }
        public int? IngredientID { get; set; }
        public int? EquipmentID { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string Unit { get; set; }
    }
}
