using System;
using System.Collections.Generic;
using QLChuoiNhaHangKhachSan.DAL;

namespace QLChuoiNhaHangKhachSan.BLL
{
    /// <summary>
    /// Enum tr?ng thái bàn
    /// </summary>
    public enum TableStatusEnum
    {
        Trong = 1,      // Tr?ng
        CoKhach = 2,    // Có khách
        DaDat = 3       // ?ã ??t
    }

    /// <summary>
    /// Service x? lý logic nghi?p v? liên quan ??n bàn
    /// </summary>
    public class TableService
    {
        private readonly TableRepository _tableRepository;

        public TableService()
        {
            _tableRepository = new TableRepository();
        }

        /// <summary>
        /// L?y t?t c? bàn
        /// </summary>
        public List<TableDTO> GetAllTables()
        {
            return _tableRepository.GetAllTables();
        }

        /// <summary>
        /// L?y bàn theo tên
        /// </summary>
        public TableDTO GetTableByName(string tableName)
        {
            return _tableRepository.GetTableByName(tableName);
        }

        /// <summary>
        /// L?y bàn theo ID
        /// </summary>
        public TableDTO GetTableById(int tableId)
        {
            return _tableRepository.GetTableById(tableId);
        }

        /// <summary>
        /// C?p nh?t tr?ng thái bàn
        /// </summary>
        public void UpdateTableStatus(int tableId, TableStatusEnum status)
        {
            _tableRepository.UpdateTableStatus(tableId, (int)status);
        }

        /// <summary>
        /// ??t bàn v? tr?ng thái tr?ng
        /// </summary>
        public void SetTableEmpty(int tableId)
        {
            _tableRepository.SetTableEmpty(tableId);
        }

        /// <summary>
        /// ??t bàn v? tr?ng thái có khách
        /// </summary>
        public void SetTableOccupied(int tableId)
        {
            _tableRepository.SetTableOccupied(tableId);
        }

        /// <summary>
        /// ??t bàn v? tr?ng thái ?ã ??t
        /// </summary>
        public void SetTableReserved(int tableId)
        {
            _tableRepository.SetTableReserved(tableId);
        }

        /// <summary>
        /// Chuy?n ??i StatusName thành enum
        /// </summary>
        public TableStatusEnum MapStatusNameToEnum(string statusName)
        {
            if (string.IsNullOrEmpty(statusName))
                return TableStatusEnum.Trong;

            if (statusName.Equals("Có khách", StringComparison.OrdinalIgnoreCase))
                return TableStatusEnum.CoKhach;

            if (statusName.Equals("?ã ??t", StringComparison.OrdinalIgnoreCase))
                return TableStatusEnum.DaDat;

            return TableStatusEnum.Trong;
        }

        /// <summary>
        /// L?y tên hi?n th? tr?ng thái
        /// </summary>
        public string GetStatusDisplayName(TableStatusEnum status)
        {
            switch (status)
            {
                case TableStatusEnum.CoKhach:
                    return "Có khách";
                case TableStatusEnum.DaDat:
                    return "?ã ??t";
                default:
                    return "Bàn tr?ng";
            }
        }

        /// <summary>
        /// Trích xu?t s? bàn t? text
        /// </summary>
        public int ExtractTableNumber(string tableText)
        {
            if (string.IsNullOrWhiteSpace(tableText)) return 0;

            var match = System.Text.RegularExpressions.Regex.Match(tableText.Trim(), @"\d+");
            if (match.Success)
            {
                int number;
                if (int.TryParse(match.Value, out number))
                    return number;
            }

            return 0;
        }
    }
}
