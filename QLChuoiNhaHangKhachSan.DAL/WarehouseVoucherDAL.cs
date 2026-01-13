using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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
                cmd.Parameters.AddWithValue("@BranchCode", string.IsNullOrWhiteSpace(unitCode) ? (object)DBNull.Value : unitCode);
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

                        var unitCodeValue = (object)(voucher.UnitCode ?? (object)DBNull.Value);
                        var restaurantCodeValue = (object)DBNull.Value;
                        var hotelCodeValue = (object)DBNull.Value;
                        if (string.Equals(voucher.WarehouseType, "INGREDIENT", StringComparison.OrdinalIgnoreCase))
                        {
                            restaurantCodeValue = unitCodeValue;
                        }
                        else if (string.Equals(voucher.WarehouseType, "EQUIPMENT", StringComparison.OrdinalIgnoreCase))
                        {
                            hotelCodeValue = unitCodeValue;
                        }

                        int newId;
                        using (var cmdIns = new SqlCommand(@"INSERT INTO dbo.WarehouseVoucher
    (VoucherCode, VoucherType, WarehouseType, UnitCode, RestaurantsCode, HotelCode, Status, Note, CreatedAt)
VALUES (@VoucherCode, @VoucherType, @WarehouseType, @UnitCode, @RestaurantsCode, @HotelCode, @Status, @Note, GETDATE());
SELECT CAST(SCOPE_IDENTITY() AS INT);", conn, tran))
                        {
                            cmdIns.Parameters.AddWithValue("@VoucherCode", newCode);
                            cmdIns.Parameters.AddWithValue("@VoucherType", voucher.VoucherType);
                            cmdIns.Parameters.AddWithValue("@WarehouseType", voucher.WarehouseType);
                            cmdIns.Parameters.AddWithValue("@UnitCode", unitCodeValue ?? (object)DBNull.Value);
                            cmdIns.Parameters.AddWithValue("@RestaurantsCode", restaurantCodeValue);
                            cmdIns.Parameters.AddWithValue("@HotelCode", hotelCodeValue);
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

        public void UpdateVoucherStatus(int voucherId, string status)
        {
            using (var conn = new SqlConnection(_connStr))
            using (var cmd = new SqlCommand("UPDATE dbo.WarehouseVoucher SET Status = @Status WHERE VoucherID = @VoucherID", conn))
            {
                cmd.Parameters.AddWithValue("@VoucherID", voucherId);
                cmd.Parameters.AddWithValue("@Status", (object)(status ?? (object)DBNull.Value));
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public List<WarehouseVoucherDTO> GetVouchersList()
        {
            var list = new List<WarehouseVoucherDTO>();
            using (var conn = new SqlConnection(_connStr))
            using (var cmd = new SqlCommand(@"SELECT VoucherID, VoucherCode, VoucherType, WarehouseType,
       COALESCE(UnitCode, RestaurantsCode, HotelCode) AS UnitCode,
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

        public WarehouseVoucherDTO GetVoucherById(int voucherId)
        {
            using (var conn = new SqlConnection(_connStr))
            using (var cmd = new SqlCommand(@"SELECT VoucherID, VoucherCode, VoucherType, WarehouseType,
       COALESCE(UnitCode, RestaurantsCode, HotelCode) AS UnitCode,
       Status, Note, CreatedAt
FROM dbo.WarehouseVoucher WHERE VoucherID = @VoucherID", conn))
            {
                cmd.Parameters.AddWithValue("@VoucherID", voucherId);
                conn.Open();
                using (var rdr = cmd.ExecuteReader())
                {
                    if (!rdr.Read()) return null;
                    return new WarehouseVoucherDTO
                    {
                        VoucherID = rdr.GetInt32(0),
                        VoucherCode = rdr.GetString(1),
                        VoucherType = rdr.GetString(2),
                        WarehouseType = rdr.GetString(3),
                        UnitCode = rdr.IsDBNull(4) ? null : rdr.GetString(4),
                        Status = rdr.IsDBNull(5) ? null : rdr.GetString(5),
                        Note = rdr.IsDBNull(6) ? null : rdr.GetString(6),
                        CreatedAt = rdr.GetDateTime(7)
                    };
                }
            }
        }

        public List<WarehouseVoucherDetailDTO> GetVoucherDetails(int voucherId)
        {
            var list = new List<WarehouseVoucherDetailDTO>();
            using (var conn = new SqlConnection(_connStr))
            using (var cmd = new SqlCommand(@"SELECT d.VoucherDetailID, d.VoucherID, d.IngredientID, d.EquipmentID,
       d.Quantity, d.UnitPrice, d.LineTotal,
       COALESCE(i.IngredientCode, e.EquipmentCode) AS ItemCode,
       COALESCE(i.IngredientName, e.EquipmentName) AS ItemName,
       COALESCE(i.Unit, e.Unit) AS Unit
FROM dbo.WarehouseVoucherDetail d
LEFT JOIN dbo.Ingredient i ON d.IngredientID = i.IngredientID
LEFT JOIN dbo.Equipment e ON d.EquipmentID = e.EquipmentID
WHERE d.VoucherID = @VoucherID
ORDER BY d.VoucherDetailID", conn))
            {
                cmd.Parameters.AddWithValue("@VoucherID", voucherId);
                conn.Open();
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        list.Add(new WarehouseVoucherDetailDTO
                        {
                            VoucherDetailID = rdr.GetInt32(0),
                            VoucherID = rdr.GetInt32(1),
                            IngredientID = rdr.IsDBNull(2) ? (int?)null : rdr.GetInt32(2),
                            EquipmentID = rdr.IsDBNull(3) ? (int?)null : rdr.GetInt32(3),
                            Quantity = rdr.GetDecimal(4),
                            UnitPrice = rdr.GetDecimal(5),
                            LineTotal = rdr.GetDecimal(6),
                            ItemCode = rdr.IsDBNull(7) ? null : rdr.GetString(7),
                            ItemName = rdr.IsDBNull(8) ? null : rdr.GetString(8),
                            Unit = rdr.IsDBNull(9) ? null : rdr.GetString(9)
                        });
                    }
                }
            }
            return list;
        }

        public void InsertVoucherDetails(int voucherId, IEnumerable<WarehouseVoucherDetailDTO> details)
        {
            if (details == null) throw new ArgumentNullException(nameof(details));
            var detailList = details.ToList();
            if (detailList.Count == 0) throw new ArgumentException("Danh sách chi tiết trống", nameof(details));

            using (var conn = new SqlConnection(_connStr))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        foreach (var detail in detailList)
                        {
                            using (var cmd = new SqlCommand(@"INSERT INTO dbo.WarehouseVoucherDetail
(VoucherID, IngredientID, EquipmentID, Quantity, UnitPrice)
VALUES (@VoucherID, @IngredientID, @EquipmentID, @Quantity, @UnitPrice);", conn, tran))
                            {
                                cmd.Parameters.AddWithValue("@VoucherID", voucherId);
                                cmd.Parameters.AddWithValue("@IngredientID", (object)detail.IngredientID ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@EquipmentID", (object)detail.EquipmentID ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@Quantity", detail.Quantity);
                                cmd.Parameters.AddWithValue("@UnitPrice", detail.UnitPrice);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        tran.Commit();
                    }
                    catch
                    {
                        tran.Rollback();
                        throw;
                    }
                }
            }
        }

        public void UpdateVoucherDraft(WarehouseVoucherDTO voucher, IEnumerable<WarehouseVoucherDetailDTO> details)
        {
            if (voucher == null) throw new ArgumentNullException(nameof(voucher));
            if (details == null) throw new ArgumentNullException(nameof(details));
            var detailList = details.ToList();
            if (detailList.Count == 0) throw new ArgumentException("Danh sách chi tiết trống", nameof(details));

            using (var conn = new SqlConnection(_connStr))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        string warehouseType = null;
                        using (var cmdType = new SqlCommand("SELECT WarehouseType FROM dbo.WarehouseVoucher WHERE VoucherID = @VoucherID", conn, tran))
                        {
                            cmdType.Parameters.AddWithValue("@VoucherID", voucher.VoucherID);
                            var typeObj = cmdType.ExecuteScalar();
                            warehouseType = typeObj as string;
                        }

                        var unitCodeValue = (object)(voucher.UnitCode ?? (object)DBNull.Value);
                        var restaurantCodeValue = (object)DBNull.Value;
                        var hotelCodeValue = (object)DBNull.Value;
                        if (string.Equals(warehouseType, "INGREDIENT", StringComparison.OrdinalIgnoreCase))
                        {
                            restaurantCodeValue = unitCodeValue;
                        }
                        else if (string.Equals(warehouseType, "EQUIPMENT", StringComparison.OrdinalIgnoreCase))
                        {
                            hotelCodeValue = unitCodeValue;
                        }

                        using (var cmdUpdate = new SqlCommand(@"UPDATE dbo.WarehouseVoucher
SET UnitCode = @UnitCode,
    RestaurantsCode = @RestaurantsCode,
    HotelCode = @HotelCode,
    Status = @Status,
    Note = @Note
WHERE VoucherID = @VoucherID", conn, tran))
                        {
                            cmdUpdate.Parameters.AddWithValue("@UnitCode", unitCodeValue);
                            cmdUpdate.Parameters.AddWithValue("@RestaurantsCode", restaurantCodeValue);
                            cmdUpdate.Parameters.AddWithValue("@HotelCode", hotelCodeValue);
                            cmdUpdate.Parameters.AddWithValue("@Status", (object)(voucher.Status ?? (object)DBNull.Value));
                            cmdUpdate.Parameters.AddWithValue("@Note", (object)(voucher.Note ?? (object)DBNull.Value));
                            cmdUpdate.Parameters.AddWithValue("@VoucherID", voucher.VoucherID);
                            cmdUpdate.ExecuteNonQuery();
                        }

                        using (var cmdDelete = new SqlCommand("DELETE FROM dbo.WarehouseVoucherDetail WHERE VoucherID = @VoucherID", conn, tran))
                        {
                            cmdDelete.Parameters.AddWithValue("@VoucherID", voucher.VoucherID);
                            cmdDelete.ExecuteNonQuery();
                        }

                        foreach (var detail in detailList)
                        {
                            using (var cmdInsert = new SqlCommand(@"INSERT INTO dbo.WarehouseVoucherDetail
(VoucherID, IngredientID, EquipmentID, Quantity, UnitPrice)
VALUES (@VoucherID, @IngredientID, @EquipmentID, @Quantity, @UnitPrice);", conn, tran))
                            {
                                cmdInsert.Parameters.AddWithValue("@VoucherID", voucher.VoucherID);
                                cmdInsert.Parameters.AddWithValue("@IngredientID", (object)detail.IngredientID ?? DBNull.Value);
                                cmdInsert.Parameters.AddWithValue("@EquipmentID", (object)detail.EquipmentID ?? DBNull.Value);
                                cmdInsert.Parameters.AddWithValue("@Quantity", detail.Quantity);
                                cmdInsert.Parameters.AddWithValue("@UnitPrice", detail.UnitPrice);
                                cmdInsert.ExecuteNonQuery();
                            }
                        }

                        tran.Commit();
                    }
                    catch
                    {
                        tran.Rollback();
                        throw;
                    }
                }
            }
        }

        public int? GetIngredientIdByCode(string ingredientCode)
        {
            if (string.IsNullOrWhiteSpace(ingredientCode)) return null;

            using (var conn = new SqlConnection(_connStr))
            using (var cmd = new SqlCommand("SELECT IngredientID FROM dbo.Ingredient WHERE IngredientCode = @Code", conn))
            {
                cmd.Parameters.AddWithValue("@Code", ingredientCode);
                conn.Open();
                var result = cmd.ExecuteScalar();
                return result == null || result == DBNull.Value ? (int?)null : Convert.ToInt32(result);
            }
        }

        public int? GetEquipmentIdByCode(string equipmentCode)
        {
            if (string.IsNullOrWhiteSpace(equipmentCode)) return null;

            using (var conn = new SqlConnection(_connStr))
            using (var cmd = new SqlCommand("SELECT EquipmentID FROM dbo.Equipment WHERE EquipmentCode = @Code", conn))
            {
                cmd.Parameters.AddWithValue("@Code", equipmentCode);
                conn.Open();
                var result = cmd.ExecuteScalar();
                return result == null || result == DBNull.Value ? (int?)null : Convert.ToInt32(result);
            }
        }

        public void ApplyStockByCode(string voucherCode)
        {
            if (string.IsNullOrWhiteSpace(voucherCode)) throw new ArgumentException("VoucherCode không hợp lệ", nameof(voucherCode));

            using (var conn = new SqlConnection(_connStr))
            using (var cmd = new SqlCommand("usp_WarehouseVoucher_ApplyStockByCode", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@VoucherCode", voucherCode);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public DataTable GetVoucherDetails(string voucherCode)
        {
            if (string.IsNullOrWhiteSpace(voucherCode)) throw new ArgumentException("VoucherCode không hợp lệ", nameof(voucherCode));

            var dt = new DataTable();
            using (var conn = new SqlConnection(_connStr))
            using (var cmd = new SqlCommand("usp_WarehouseVoucher_GetDetailsByCode", conn))
            using (var adapter = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@VoucherCode", voucherCode);
                adapter.Fill(dt);
            }

            return dt;
        }
    }
}
