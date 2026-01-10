using System.Data;
using System.Data.SqlClient;

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
            var dt = new DataTable();

            using (var conn = new SqlConnection(_connStr))
            using (var cmd = new SqlCommand(@"
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

        ORDER BY UnitType, UnitCode;
    ", conn))
            {
                cmd.Parameters.AddWithValue("@AreaID", areaId);
                using (var da = new SqlDataAdapter(cmd))
                    da.Fill(dt);
            }

            if (!string.IsNullOrEmpty(unitType))
            {
                var dv = dt.DefaultView;
                dv.RowFilter = $"UnitType = '{unitType.Replace("'", "''")}'";
                dt = dv.ToTable();
            }

            return dt;
        }

        public DataTable GetUnitsByArea(int areaId)
        {
            return GetUnitsByAreaAndType(areaId, null);
        }

    }
}
