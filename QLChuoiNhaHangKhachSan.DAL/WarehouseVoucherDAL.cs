using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using QLChuoiNhaHangKhachSan.DTO;

namespace QLChuoiNhaHangKhachSan.DAL
{
    public class WarehouseVoucherDAL
    {
        private readonly string _connStr;

        public WarehouseVoucherDAL(string connStr)
        {
            _connStr = connStr;     
        }

        public string GetNextVoucherCode(string prefix)
        {
            try
            {   
                using (var conn = new SqlConnection(_connStr))
                using (var cmd = new SqlCommand("usp_WarehouseVoucher_NextCode", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Prefix", prefix);
                    var outCode = new SqlParameter("@NextCode", SqlDbType.NVarChar, 30) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(outCode);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    return outCode.Value as string;
                }
            }
            catch (SqlException ex)
            {
                throw new InvalidOperationException("Không lấy được mã phiếu mới. Kiểm tra kết nối và stored procedure dbo.usp_WarehouseVoucher_NextCode.", ex);
            }
        }

        public WarehouseVoucherDTO CreateVoucher(string voucherType, string warehouseType, string unitCode, string status, string note)
        {
            using (var conn = new SqlConnection(_connStr))
            using (var cmd = new SqlCommand("usp_WarehouseVoucher_Create", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@VoucherType", voucherType);
                cmd.Parameters.AddWithValue("@WarehouseType", warehouseType);
                cmd.Parameters.AddWithValue("@UnitCode", unitCode); 
                cmd.Parameters.AddWithValue("@Status", status ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Note", note ?? (object)DBNull.Value);
    
                var outId = new SqlParameter("@NewVoucherID", SqlDbType.Int) { Direction = ParameterDirection.Output };
                var outCode = new SqlParameter("@NewVoucherCode", SqlDbType.VarChar, 50) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(outId);
                cmd.Parameters.Add(outCode);

                conn.Open();
                cmd.ExecuteNonQuery();

                return new WarehouseVoucherDTO
                {
                    VoucherID = outId.Value != DBNull.Value ? (int)outId.Value : 0, 
                    VoucherCode = outCode.Value as string
                };
            }
        }

        public WarehouseVoucherDTO CreateVoucherWithDetails(WarehouseVoucherDTO voucher, IEnumerable<WarehouseVoucherDetailDTO> details)
        {
            if (voucher == null) throw new ArgumentNullException(nameof(voucher));
            if (details == null) throw new ArgumentNullException(nameof(details));

            using (var conn = new SqlConnection(_connStr))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        // đảm bảo sinh mã an toàn đồng thời
                        using (var isoCmd = new SqlCommand("SET TRANSACTION ISOLATION LEVEL SERIALIZABLE", conn, tran))
                        {
                            isoCmd.ExecuteNonQuery();
                        }

                        var prefix = string.Equals(voucher.VoucherType, "IMPORT", StringComparison.OrdinalIgnoreCase) ? "PN" : "PX";
                        int nextNumber;
                        using (var cmdSeq = new SqlCommand(@"SELECT MAX(CAST(SUBSTRING(VoucherCode,3,10) AS INT))
FROM dbo.WarehouseVoucher WITH (UPDLOCK, HOLDLOCK) WHERE VoucherCode LIKE @Prefix + '%'", conn, tran))
                        {
                            cmdSeq.Parameters.AddWithValue("@Prefix", prefix);
                            var result = cmdSeq.ExecuteScalar();
                            var maxNum = result == DBNull.Value || result == null ? 0 : Convert.ToInt32(result);
                            nextNumber = maxNum + 1;
                        }

                        var newCode = prefix + nextNumber.ToString("D3");

                        int newId;
                        using (var cmdIns = new SqlCommand(@"INSERT INTO dbo.WarehouseVoucher
    (VoucherCode, VoucherType, WarehouseType, UnitCode, Status, Note, CreatedAt)
VALUES (@VoucherCode, @VoucherType, @WarehouseType, @UnitCode, @Status, @Note, GETDATE());
SELECT CAST(SCOPE_IDENTITY() AS INT);", conn, tran))
                        {
                            cmdIns.Parameters.AddWithValue("@VoucherCode", newCode);
                            cmdIns.Parameters.AddWithValue("@VoucherType", voucher.VoucherType);
                            cmdIns.Parameters.AddWithValue("@WarehouseType", voucher.WarehouseType);
                            cmdIns.Parameters.AddWithValue("@UnitCode", (object)(voucher.UnitCode ?? (object)DBNull.Value));
                            cmdIns.Parameters.AddWithValue("@Status", (object)(voucher.Status ?? (object)DBNull.Value));
                            cmdIns.Parameters.AddWithValue("@Note", (object)(voucher.Note ?? (object)DBNull.Value));

                            newId = (int)cmdIns.ExecuteScalar();
                        }

                        foreach (var d in details)
                        {
                            using (var cmdDet = new SqlCommand(@"INSERT INTO dbo.WarehouseVoucherDetail
    (VoucherID, IngredientID, EquipmentID, Quantity, UnitPrice)
VALUES (@VoucherID, @IngredientID, @EquipmentID, @Quantity, @UnitPrice);", conn, tran))
                            {
                                cmdDet.Parameters.AddWithValue("@VoucherID", newId);
                                cmdDet.Parameters.AddWithValue("@IngredientID", (object)d.IngredientID ?? DBNull.Value);
                                cmdDet.Parameters.AddWithValue("@EquipmentID", (object)d.EquipmentID ?? DBNull.Value);
                                cmdDet.Parameters.AddWithValue("@Quantity", d.Quantity);
                                cmdDet.Parameters.AddWithValue("@UnitPrice", d.UnitPrice);
                                cmdDet.ExecuteNonQuery();
                            }
                        }

                        tran.Commit();

                        voucher.VoucherID = newId;
                        voucher.VoucherCode = newCode;
                        voucher.CreatedAt = DateTime.Now;
                        return voucher;
                    }
                    catch
                    {
                        tran.Rollback();
                        throw;
                    }
                }
            }
        }

        public List<WarehouseVoucherDTO> GetVouchersList()
        {
            var list = new List<WarehouseVoucherDTO>();
            using (var conn = new SqlConnection(_connStr))
            using (var cmd = new SqlCommand(@"SELECT VoucherID, VoucherCode, VoucherType, WarehouseType,
       UnitCode,
       Status, Note, CreatedAt
FROM dbo.WarehouseVoucher ORDER BY VoucherID DESC", conn))
            {
                conn.Open();
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        list.Add(new WarehouseVoucherDTO
                        {
                            VoucherID = rdr.GetInt32(0),
                            VoucherCode = rdr.GetString(1),
                            VoucherType = rdr.GetString(2),
                            WarehouseType = rdr.GetString(3),
                            UnitCode = rdr.IsDBNull(4) ? null : rdr.GetString(4),
                            Status = rdr.IsDBNull(5) ? null : rdr.GetString(5),
                            Note = rdr.IsDBNull(6) ? null : rdr.GetString(6),
                            CreatedAt = rdr.GetDateTime(7)
                        });
                    }
                }
            }
            return list;
        }
    }
}
