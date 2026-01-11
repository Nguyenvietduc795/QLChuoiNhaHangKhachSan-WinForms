using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using QLChuoiNhaHangKhachSan.DAL;
using QLChuoiNhaHangKhachSan.DTO;

namespace QLChuoiNhaHangKhachSan.BLL
{
    public class WarehouseVoucherBLL
    {
        private const string StatusDraft = "Nháp";

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

        public int? ResolveIngredientIdByCode(string ingredientCode)
        {
            return _dal.GetIngredientIdByCode(ingredientCode);
        }

        public int? ResolveEquipmentIdByCode(string equipmentCode)
        {
            return _dal.GetEquipmentIdByCode(equipmentCode);
        }

        public WarehouseVoucherDTO CreateVoucherAndApplyStock(WarehouseVoucherDTO voucher, IEnumerable<WarehouseVoucherDetailDTO> details)
        {
            if (voucher == null) throw new System.ArgumentNullException(nameof(voucher));
            if (details == null || !details.Any()) throw new System.ArgumentException("Danh sách chi ti?t phi?u tr?ng", nameof(details));

            var desiredStatus = NormalizeStatus(voucher.Status);
            var shouldApply = !IsDraft(desiredStatus);

            var created = _dal.CreateVoucher(voucher.VoucherType, voucher.WarehouseType, voucher.UnitCode, StatusDraft, voucher.Note);
            _dal.InsertVoucherDetails(created.VoucherID, details);

            if (shouldApply)
            {
                _dal.ApplyStockByCode(created.VoucherCode);
                _dal.UpdateVoucherStatus(created.VoucherID, desiredStatus);
                created.Status = desiredStatus;
            }
            else
            {
                created.Status = StatusDraft;
            }

            return created;
        }

        public DataTable GetDetailsByCode(string voucherCode)
        {
            return _dal.GetVoucherDetails(voucherCode);
        }

        public WarehouseVoucherDTO GetVoucherById(int voucherId)
        {
            return _dal.GetVoucherById(voucherId);
        }

        public List<WarehouseVoucherDetailDTO> GetVoucherDetails(int voucherId)
        {
            return _dal.GetVoucherDetails(voucherId);
        }

        public void UpdateDraftVoucher(WarehouseVoucherDTO voucher, IEnumerable<WarehouseVoucherDetailDTO> details)
        {
            if (voucher == null) throw new ArgumentNullException(nameof(voucher));
            if (details == null || !details.Any()) throw new ArgumentException("Danh sách chi ti?t phi?u tr?ng", nameof(details));

            var desiredStatus = NormalizeStatus(voucher.Status);
            var shouldApply = !IsDraft(desiredStatus);

            var current = _dal.GetVoucherById(voucher.VoucherID);
            if (current == null) throw new InvalidOperationException("Không tìm th?y phi?u c?n s?a.");
            if (!IsDraft(current.Status)) throw new InvalidOperationException("Ch? s?a ???c phi?u ?ang ? tr?ng thái 'Nháp'.");

            voucher.VoucherCode = current.VoucherCode;
            voucher.VoucherType = current.VoucherType;
            voucher.WarehouseType = current.WarehouseType;

            var draftUpdate = new WarehouseVoucherDTO
            {
                VoucherID = voucher.VoucherID,
                UnitCode = voucher.UnitCode,
                Status = StatusDraft,
                Note = voucher.Note
            };

            _dal.UpdateVoucherDraft(draftUpdate, details);

            if (shouldApply)
            {
                _dal.ApplyStockByCode(voucher.VoucherCode);
                _dal.UpdateVoucherStatus(voucher.VoucherID, desiredStatus);
            }
        }

        private static bool IsDraft(string status)
        {
            return string.Equals(status, StatusDraft, StringComparison.OrdinalIgnoreCase);
        }

        private static string NormalizeStatus(string status)
        {
            return string.IsNullOrWhiteSpace(status) ? StatusDraft : status.Trim();
        }
    }
}
