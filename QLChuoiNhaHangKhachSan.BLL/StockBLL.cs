using System.Data;
using QLChuoiNhaHangKhachSan.DAL;

namespace QLChuoiNhaHangKhachSan.BLL
{
    public class StockBLL
    {
        private readonly StockDAL _dal;

        public StockBLL(string connStr)
        {
            _dal = new StockDAL(connStr);
        }

        public DataTable GetStockList(string warehouseType, string keyword)
        {
            return _dal.GetStockList(warehouseType, keyword);
        }

        public int GetTotalStockQuantity()
        {
            return _dal.GetTotalStockQuantity();
        }

        public int GetLowStockCount()
        {
            return _dal.GetLowStockCount();
        }

        public int GetStableStockCount()
        {
            return _dal.GetStableStockCount();
        }
    }
}
