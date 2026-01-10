using System.Collections.Generic;
using QLChuoiNhaHangKhachSan.DAL;
using QLChuoiNhaHangKhachSan.DTO;

namespace QLChuoiNhaHangKhachSan.BLL
{
    public class WarehouseVoucherBLL
    {
        private readonly WarehouseVoucherDAL _dal;

        public WarehouseVoucherBLL(string connStr)
        {
            _dal = new WarehouseVoucherDAL(connStr);
        }

        public string GetNextVoucherCode(bool isImport)
        {
            var prefix = isImport ? "PN" : "PX";
            return _dal.GetNextVoucherCode(prefix);
        }

        public WarehouseVoucherDTO CreateVoucher(string voucherType, string warehouseType, string unitCode, string status, string note)
        {
            return _dal.CreateVoucher(voucherType, warehouseType, unitCode, status, note);
        }

        public WarehouseVoucherDTO CreateVoucherWithDetails(WarehouseVoucherDTO voucher, IEnumerable<WarehouseVoucherDetailDTO> details)
        {
            return _dal.CreateVoucherWithDetails(voucher, details);
        }

        public List<WarehouseVoucherDTO> GetVouchersList()
        {
            return _dal.GetVouchersList();
        }
    }
}
