using System;

namespace QLChuoiNhaHangKhachSan.DAL
{
    /// <summary>
    /// Thông tin ??t bàn
    /// </summary>
    public class BookingDTO
    {
        public int BookingID { get; set; }
        public int TableID { get; set; }
        public string TableName { get; set; }
        public int? CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime BookingDate { get; set; }
        public TimeSpan BookingTime { get; set; }
        public int GuestCount { get; set; }
        public string Status { get; set; }
        public DateTime? CreatedDate { get; set; }
    }

    /// <summary>
    /// Thông tin hóa ??n
    /// </summary>
    public class OrderDTO
    {
        public int OrderId { get; set; }
        public int TableID { get; set; }
        public string TableName { get; set; }
        public int? CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public int? GuestCount { get; set; }
        public DateTime? DateCheckIn { get; set; }
        public DateTime? DateCheckOut { get; set; }
        public string Status { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal Surcharge { get; set; }
        public decimal TotalAmount { get; set; }
    }

    /// <summary>
    /// Chi ti?t món trong hóa ??n
    /// </summary>
    public class OrderItemDTO
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public int FoodID { get; set; }
        public string FoodName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal { get; set; }
        public string Note { get; set; }
    }

    /// <summary>
    /// Thông tin món ?n
    /// </summary>
    public class FoodDTO
    {
        public int FoodID { get; set; }
        public string FoodName { get; set; }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public decimal Price { get; set; }
        public int StatusID { get; set; }
        public string StatusName { get; set; }
    }

    /// <summary>
    /// Thông tin lo?i món ?n
    /// </summary>
    public class FoodCategoryDTO
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
    }

    /// <summary>
    /// Thông tin tr?ng thái món ?n
    /// </summary>
    public class FoodStatusDTO
    {
        public int StatusID { get; set; }
        public string StatusName { get; set; }
    }

    /// <summary>
    /// Thông tin bàn
    /// </summary>
    public class TableDTO
    {
        public int TableID { get; set; }
        public string TableName { get; set; }
        public int StatusID { get; set; }
        public string StatusName { get; set; }
    }

    /// <summary>
    /// Thông tin khách hàng
    /// </summary>
    public class CustomerDTO
    {
        public int CustomerID { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
