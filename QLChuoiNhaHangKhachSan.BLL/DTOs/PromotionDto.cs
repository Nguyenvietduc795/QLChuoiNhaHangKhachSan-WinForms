using System;

namespace QLChuoiNhaHangKhachSan.BLL.DTOs
{
    /// <summary>
    /// DTO dùng ?? truy?n d? li?u ?u ?ãi gi?a GUI và BLL.
    /// GUI ch? c?n bi?t DTO này, không c?n bi?t model trong DAL.
    /// </summary>
    public class PromotionDto
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
