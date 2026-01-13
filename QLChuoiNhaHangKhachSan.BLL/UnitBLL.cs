using System.Data;
using QLChuoiNhaHangKhachSan.DAL;

namespace QLChuoiNhaHangKhachSan.BLL
{
    public class UnitBLL
    {
        private readonly UnitDAL _dal;

        public UnitBLL(string connStr)
        {
            _dal = new UnitDAL(connStr);
        }

        public DataTable GetUnitsByAreaAndType(int areaId, string unitType)
        {
            return _dal.GetUnitsByAreaAndType(areaId, unitType);
        }

        public DataTable GetUnitsByArea(int areaId)
        {
            return _dal.GetUnitsByArea(areaId);
        }
    }
}
