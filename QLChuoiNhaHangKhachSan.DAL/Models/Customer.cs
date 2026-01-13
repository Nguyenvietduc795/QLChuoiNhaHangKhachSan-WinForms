using System;

namespace QLChuoiNhaHangKhachSan.DAL.Models
{
    // DTO cho bảng Customers, dùng ở DAL (Repository) và BLL (Service)
    public class Customer
    {
        public int CustomerId { get; set; }      // CustomerId (PK)
        public string FullName { get; set; }     // FullName
        public string Nationality { get; set; }  // Nationality
        public string CCCD { get; set; }         // CCCD
        public string Sex { get; set; }          // Sex
        public string PhoneNumber { get; set; }  // PhoneNumber
        public string Email { get; set; }        // Email
        public string Address { get; set; }      // Address
        public string CustomerType { get; set; } // CustomerType: VIP / Thường
        public DateTime CreatedAt { get; set; }  // CreatedAt
        public decimal TotalSpending { get; set; } // Tổng chi tiêu tích lũy
    }
}
