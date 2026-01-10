using System.Data;
using QLChuoiNhaHangKhachSan.DAL;

namespace QLChuoiNhaHangKhachSan.BLL
{
    public class EquipmentBLL
    {
        private readonly EquipmentDAL _equipmentDal;

        public EquipmentBLL(string connectionString)
        {
            _equipmentDal = new EquipmentDAL(connectionString);
        }

        public DataTable GetEquipmentForImport()
        {
            return _equipmentDal.GetEquipmentForImport();
        }

        public DataTable GetEquipmentForExport()
        {
            return _equipmentDal.GetEquipmentForExport();
        }
    }
}
