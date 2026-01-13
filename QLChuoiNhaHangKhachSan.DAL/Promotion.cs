using System;

namespace QLChuoiNhaHangKhachSan.DAL.Models
{
    // DTO cho bảng Promotions, dùng ở DAL (Repository) và BLL (Service)
    public class Promotion
    {
        public int PromotionId { get; set; }
        public string PromotionCode { get; set; }
        public string ProgramName { get; set; }
        public string PromotionType { get; set; }
        public string TargetAudience { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
