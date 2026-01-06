using System.Collections.Generic;
using QLChuoiNhaHangKhachSan.DAL;
using QLChuoiNhaHangKhachSan.DAL.Models;

namespace QLChuoiNhaHangKhachSan.BLL
{
    // Service tầng BLL cho nghiệp vụ liên quan đến ưu đãi (Promotions)
    public class PromotionService
    {
        private readonly PromotionDal _dal = new PromotionDal();

        public List<Promotion> GetAllPromotions()
        {
            return _dal.GetAll();
        }

        public int AddPromotion(Promotion p)
        {
            return _dal.Insert(p);
        }

        public void UpdatePromotion(Promotion p)
        {
            _dal.Update(p);
        }

        public void DeletePromotion(int promotionId)
        {
            _dal.Delete(promotionId);
        }
    }
}
