using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace QLChuoiNhaHangKhachSan.DAL
{
    public class UnitDAL
    {
        private readonly string _connStr;

        public UnitDAL(string connStr)
        {
            _connStr = connStr;
        }

        public DataTable GetUnitsByAreaAndType(int areaId, string unitType)
        {
            try
            {
                var dt = LoadUnitsFromLegacyTables(areaId);
                return FilterByUnitType(dt, unitType);
            }
            catch (SqlException ex) when (ShouldFallbackToUnifiedTable(ex))
            {
                return LoadUnitsFromUnifiedTable(areaId, unitType);
            }
        }

        public DataTable GetUnitsByArea(int areaId)
        {
            return GetUnitsByAreaAndType(areaId, null);
        }

        private DataTable LoadUnitsFromLegacyTables(int areaId)
        {
            var dt = new DataTable();

            const string legacySql = @"
SELECT HotelCode       AS UnitCode,
       HotelName       AS UnitName,
       'HOTEL'         AS UnitType
FROM dbo.Hotel
WHERE AreaID = @AreaID

UNION ALL

SELECT RestaurantCode  AS UnitCode,
       RestaurantName  AS UnitName,
       'RESTAURANT'    AS UnitType
FROM dbo.Restaurant
WHERE AreaID = @AreaID

ORDER BY UnitType, UnitCode;";

            using (var conn = new SqlConnection(_connStr))
            using (var cmd = new SqlCommand(legacySql, conn))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.Parameters.AddWithValue("@AreaID", areaId);
                da.Fill(dt);
            }

            return dt;
        }

        private DataTable LoadUnitsFromUnifiedTable(int areaId, string unitType)
        {
            var candidateTables = new[] { "Unit", "Units", "WarehouseUnit" };
            foreach (var tableName in candidateTables)
            {
                if (!TableExists("dbo", tableName)) continue;
                return LoadUnitsFromSpecificTable("dbo", tableName, areaId, unitType);
            }

            return BuildFallbackUnits(unitType);
        }

        private DataTable LoadUnitsFromSpecificTable(string schema, string table, int areaId, string unitType)
        {
            var sql = $@"
SELECT UnitCode,
       UnitName,
       UnitType
FROM [{schema}].[{table}]
WHERE AreaID = @AreaID";

            if (!string.IsNullOrWhiteSpace(unitType))
            {
                sql += " AND UnitType = @UnitType";
            }

            sql += " ORDER BY UnitType, UnitCode";

            var dt = new DataTable();
            using (var conn = new SqlConnection(_connStr))
            using (var cmd = new SqlCommand(sql, conn))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.Parameters.AddWithValue("@AreaID", areaId);
                if (!string.IsNullOrWhiteSpace(unitType))
                {
                    cmd.Parameters.AddWithValue("@UnitType", unitType);
                }

                da.Fill(dt);
            }

            return dt;
        }

        private static DataTable FilterByUnitType(DataTable dt, string unitType)
        {
            if (dt == null || string.IsNullOrWhiteSpace(unitType)) return dt;

            var dv = dt.DefaultView;
            dv.RowFilter = $"UnitType = '{unitType.Replace("'", "''")}'";
            return dv.ToTable();
        }

        private static bool ShouldFallbackToUnifiedTable(SqlException ex)
        {
            if (ex == null) return false;
            return ex.Errors.Cast<SqlError>().Any(err => err.Number == 208);
        }

        private bool TableExists(string schema, string table)
        {
            const string sql = @"SELECT CASE WHEN OBJECT_ID(@ObjName, 'U') IS NULL THEN 0 ELSE 1 END";
            using (var conn = new SqlConnection(_connStr))
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@ObjName", $"{schema}.{table}");
                conn.Open();
                var result = cmd.ExecuteScalar();
                return result != null && Convert.ToInt32(result) == 1;
            }
        }

        private static DataTable BuildFallbackUnits(string unitType)
        {
            var dt = new DataTable();
            dt.Columns.Add("UnitCode", typeof(string));
            dt.Columns.Add("UnitName", typeof(string));
            dt.Columns.Add("UnitType", typeof(string));

            var defaults = new List<(string Code, string Name, string Type)>
            {
                ("NH01", "Nhà hàng mặc định", "RESTAURANT"),
                ("KS01", "Khách sạn mặc định", "HOTEL")
            };

            foreach (var unit in defaults)
            {
                dt.Rows.Add(unit.Code, unit.Name, unit.Type);
            }

            return FilterByUnitType(dt, unitType);
        }

    }
}
