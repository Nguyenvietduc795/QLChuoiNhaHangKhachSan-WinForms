using System;
using System.Collections.Generic;
using System.Linq;
using QLChuoiNhaHangKhachSan.BLL.DTOs;
using QLChuoiNhaHangKhachSan.DAL;
using QLChuoiNhaHangKhachSan.DAL.Models;

namespace QLChuoiNhaHangKhachSan.BLL
{
    /// <summary>
    /// Service tầng BLL cho nghiệp vụ liên quan đến ưu đãi (Promotions).
    /// GUI chỉ giao tiếp với BLL thông qua PromotionDto.
    /// </summary>
    public class PromotionService
    {
        private readonly PromotionDal _dal = new PromotionDal();

        /// <summary>
        /// Lấy tất cả ưu đãi, trả về danh sách DTO cho GUI.
        /// </summary>
        public List<PromotionDto> GetAllPromotions()
        {
            var promotions = _dal.GetAll();
            return promotions.Select(MapToDto).ToList();
        }

        /// <summary>
        /// Kiểm tra mã giảm giá có hợp lệ và còn hạn không.
        /// Tìm kiếm theo PromotionCode, PromotionId, hoặc ProgramName.
        /// Trả về PromotionDto nếu hợp lệ, null nếu không tìm thấy, throws exception nếu hết hạn.
        /// </summary>
        /// <param name="promotionCode">Mã giảm giá, ID, hoặc tên chương trình cần kiểm tra</param>
        /// <param name="customerType">Loại khách hàng (để kiểm tra đối tượng áp dụng)</param>
        /// <returns>PromotionDto nếu hợp lệ</returns>
        public PromotionDto ValidateAndGetPromotion(string promotionCode, string customerType = null)
        {
            if (string.IsNullOrWhiteSpace(promotionCode))
                return null;

            var searchTerm = promotionCode.Trim();
            var allPromotions = GetAllPromotions();
            
            // 1. Tìm theo PromotionCode (ưu tiên cao nhất)
            var promo = allPromotions.FirstOrDefault(p =>
                !string.IsNullOrEmpty(p.PromotionCode) &&
                string.Equals(p.PromotionCode, searchTerm, StringComparison.OrdinalIgnoreCase));

            // 2. Nếu không tìm thấy, thử tìm theo PromotionId
            if (promo == null && int.TryParse(searchTerm, out int promoId))
            {
                promo = allPromotions.FirstOrDefault(p => p.PromotionId == promoId);
            }

            // 3. Nếu vẫn không tìm thấy, thử tìm theo ProgramName (tên chương trình)
            if (promo == null)
            {
                promo = allPromotions.FirstOrDefault(p =>
                    !string.IsNullOrEmpty(p.ProgramName) &&
                    string.Equals(p.ProgramName, searchTerm, StringComparison.OrdinalIgnoreCase));
            }

            if (promo == null)
                return null;

            // Kiểm tra ngày hết hạn
            if (promo.ExpirationDate.HasValue && promo.ExpirationDate.Value.Date < DateTime.Today)
            {
                string displayCode = !string.IsNullOrEmpty(promo.PromotionCode) ? promo.PromotionCode : promo.ProgramName;
                throw new InvalidOperationException($"Mã giảm giá '{displayCode}' đã hết hạn sử dụng (hết hạn ngày {promo.ExpirationDate.Value:dd/MM/yyyy}).");
            }

            // Kiểm tra trạng thái
            if (!string.IsNullOrEmpty(promo.Status) &&
                (promo.Status.Equals("Kết thúc", StringComparison.OrdinalIgnoreCase) ||
                 promo.Status.Equals("Hết hạn", StringComparison.OrdinalIgnoreCase)))
            {
                string displayCode = !string.IsNullOrEmpty(promo.PromotionCode) ? promo.PromotionCode : promo.ProgramName;
                throw new InvalidOperationException($"Mã giảm giá '{displayCode}' đã kết thúc chương trình.");
            }

            // Kiểm tra đối tượng áp dụng (nếu có chỉ định)
            if (!string.IsNullOrEmpty(promo.TargetAudience) && !string.IsNullOrEmpty(customerType))
            {
                var target = promo.TargetAudience.ToUpperInvariant();
                var custType = customerType.ToUpperInvariant();

                // "Tất cả" hoặc không giới hạn thì cho qua
                if (!target.Contains("TẤT CẢ") && !target.Contains("ALL"))
                {
                    bool isVipPromo = target.Contains("VIP");
                    bool isVipCustomer = custType.Contains("VIP");

                    if (isVipPromo && !isVipCustomer)
                    {
                        string displayCode = !string.IsNullOrEmpty(promo.PromotionCode) ? promo.PromotionCode : promo.ProgramName;
                        throw new InvalidOperationException($"Mã giảm giá '{displayCode}' chỉ áp dụng cho khách hàng VIP.");
                    }
                }
            }

            return promo;
        }

        /// <summary>
        /// Thêm ưu đãi mới, nhận DTO từ GUI, trả về ID mới.
        /// </summary>
        public int AddPromotion(PromotionDto dto)
        {
            var promotion = MapToEntity(dto);
            return _dal.Insert(promotion);
        }

        /// <summary>
        /// Cập nhật ưu đãi, nhận DTO từ GUI.
        /// </summary>
        public void UpdatePromotion(PromotionDto dto)
        {
            var promotion = MapToEntity(dto);
            _dal.Update(promotion);
        }

        /// <summary>
        /// Xóa ưu đãi theo ID.
        /// </summary>
        public void DeletePromotion(int promotionId)
        {
            _dal.Delete(promotionId);
        }

        #region Mapping Methods

        /// <summary>
        /// Chuyển từ Entity (DAL) sang DTO (BLL).
        /// </summary>
        private PromotionDto MapToDto(Promotion entity)
        {
            return new PromotionDto
            {
                PromotionId = entity.PromotionId,
                PromotionCode = entity.PromotionCode,
                ProgramName = entity.ProgramName,
                PromotionType = entity.PromotionType,
                TargetAudience = entity.TargetAudience,
                ExpirationDate = entity.ExpirationDate,
                Status = entity.Status,
                CreatedAt = entity.CreatedAt,
                DiscountPercent = entity.DiscountPercent
            };
        }

        /// <summary>
        /// Chuyển từ DTO (BLL) sang Entity (DAL).
        /// </summary>
        private Promotion MapToEntity(PromotionDto dto)
        {
            return new Promotion
            {
                PromotionId = dto.PromotionId,
                PromotionCode = dto.PromotionCode,
                ProgramName = dto.ProgramName,
                PromotionType = dto.PromotionType,
                TargetAudience = dto.TargetAudience,
                ExpirationDate = dto.ExpirationDate,
                Status = dto.Status,
                CreatedAt = dto.CreatedAt,
                DiscountPercent = dto.DiscountPercent
            };
        }

        #endregion
    }
}
