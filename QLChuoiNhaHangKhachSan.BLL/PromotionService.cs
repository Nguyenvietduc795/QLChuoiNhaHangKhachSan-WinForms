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
                CreatedAt = entity.CreatedAt
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
                CreatedAt = dto.CreatedAt
            };
        }

        #endregion
    }
}
