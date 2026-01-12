using System;

namespace QLChuoiNhaHangKhachSan.BLL.DTOs
{
    /// <summary>
    /// DTO dùng ?? truy?n d? li?u khách hàng gi?a GUI và BLL.
    /// GUI ch? c?n bi?t DTO này, không c?n bi?t model trong DAL.
    /// </summary>
    public class CustomerDto
    {
        public int CustomerId { get; set; }
        public string FullName { get; set; }
        public string Nationality { get; set; }
        public string CCCD { get; set; }
        public string Sex { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string CustomerType { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal TotalSpending { get; set; }
    }
}
