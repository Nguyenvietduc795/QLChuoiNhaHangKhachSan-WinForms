using System;
using System.Collections.Generic;
using System.Linq;

namespace QLChuoiNhaHangKhachSan.GUI
{
    /// <summary>
    /// C?u hình t?ng giá cho các ngày l? t?t
    /// </summary>
    public static class HolidayPriceConfig
    {
        /// <summary>
        /// Thông tin ngày l?
        /// </summary>
        public class HolidayInfo
        {
            public string Name { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            /// <summary>
            /// T? l? t?ng giá (1.0 = không t?ng, 1.5 = t?ng 50%, 2.0 = t?ng 100%)
            /// </summary>
            public decimal PriceMultiplier { get; set; }
        }

        /// <summary>
        /// Danh sách các ngày l? v?i t? l? t?ng giá
        /// Có th? c?u hình l?i hàng n?m ho?c load t? database
        /// </summary>
        public static List<HolidayInfo> Holidays { get; set; } = new List<HolidayInfo>();

        static HolidayPriceConfig()
        {
            // Kh?i t?o danh sách ngày l? m?c ??nh cho n?m hi?n t?i
            InitializeDefaultHolidays(DateTime.Now.Year);
        }

        /// <summary>
        /// Kh?i t?o danh sách ngày l? m?c ??nh cho m?t n?m
        /// </summary>
        public static void InitializeDefaultHolidays(int year)
        {
            Holidays.Clear();

            // T?t D??ng l?ch (1/1) - t?ng 30%
            Holidays.Add(new HolidayInfo
            {
                Name = "T?t D??ng l?ch",
                StartDate = new DateTime(year, 1, 1),
                EndDate = new DateTime(year, 1, 1),
                PriceMultiplier = 1.3m
            });

            // T?t Nguyên ?án (kho?ng cu?i tháng 1 - ??u tháng 2 âm l?ch) - t?ng 50%
            // C?n ?i?u ch?nh hàng n?m theo l?ch âm
            // M?c ??nh set cho kho?ng 25/1 - 5/2
            Holidays.Add(new HolidayInfo
            {
                Name = "T?t Nguyên ?án",
                StartDate = new DateTime(year, 1, 25),
                EndDate = new DateTime(year, 2, 5),
                PriceMultiplier = 1.4m
            });

            // Gi? T? Hùng V??ng (10/3 âm l?ch) - kho?ng tháng 4 d??ng l?ch - t?ng 20%
            Holidays.Add(new HolidayInfo
            {
                Name = "Gi? T? Hùng V??ng",
                StartDate = new DateTime(year, 4, 18),
                EndDate = new DateTime(year, 4, 18),
                PriceMultiplier = 1.2m
            });

            // Ngày Gi?i phóng mi?n Nam (30/4) - t?ng 30%
            Holidays.Add(new HolidayInfo
            {
                Name = "Ngày Gi?i phóng mi?n Nam",
                StartDate = new DateTime(year, 4, 30),
                EndDate = new DateTime(year, 4, 30),
                PriceMultiplier = 1.2m
            });

            // Ngày Qu?c t? Lao ??ng (1/5) - t?ng 30%
            Holidays.Add(new HolidayInfo
            {
                Name = "Ngày Qu?c t? Lao ??ng",
                StartDate = new DateTime(year, 5, 1),
                EndDate = new DateTime(year, 5, 1),
                PriceMultiplier = 1.2m
            });

            // L? 30/4 - 1/5 (n?u g?p c? k? ngh?) - t?ng 30%
            Holidays.Add(new HolidayInfo
            {
                Name = "Ngh? l? 30/4 - 1/5",
                StartDate = new DateTime(year, 4, 29),
                EndDate = new DateTime(year, 5, 3),
                PriceMultiplier = 1.2m
            });

            // Qu?c khánh (2/9) - t?ng 30%
            Holidays.Add(new HolidayInfo
            {
                Name = "Qu?c khánh",
                StartDate = new DateTime(year, 9, 2),
                EndDate = new DateTime(year, 9, 3),
                PriceMultiplier = 1.2m
            });

            // Giáng sinh (24-25/12) - t?ng 25%
            Holidays.Add(new HolidayInfo
            {
                Name = "Giáng sinh",
                StartDate = new DateTime(year, 12, 24),
                EndDate = new DateTime(year, 12, 25),
                PriceMultiplier = 1.2m
            });

            // Giao th?a - T?t D??ng l?ch n?m sau (31/12) - t?ng 40%
            Holidays.Add(new HolidayInfo
            {
                Name = "?êm Giao th?a",
                StartDate = new DateTime(year, 12, 31),
                EndDate = new DateTime(year, 12, 31),
                PriceMultiplier = 1.4m
            });

            // Cu?i tu?n (Th? 7, Ch? nh?t) - tùy ch?n, có th? b?t/t?t
            // Không thêm vào m?c ??nh, dùng ph??ng th?c riêng
        }

        /// <summary>
        /// T? l? t?ng giá cu?i tu?n (m?c ??nh 1.2 = t?ng 20%)
        /// </summary>
        public static decimal WeekendMultiplier { get; set; } = 1.1m;

        /// <summary>
        /// B?t/t?t t?ng giá cu?i tu?n
        /// </summary>
        public static bool EnableWeekendPricing { get; set; } = true;

        /// <summary>
        /// Ki?m tra m?t ngày có ph?i ngày l? không và tr? v? t? l? t?ng giá
        /// </summary>
        public static decimal GetPriceMultiplier(DateTime date)
        {
            decimal maxMultiplier = 1.0m;

            // Ki?m tra ngày l?
            foreach (var holiday in Holidays)
            {
                if (date.Date >= holiday.StartDate.Date && date.Date <= holiday.EndDate.Date)
                {
                    if (holiday.PriceMultiplier > maxMultiplier)
                        maxMultiplier = holiday.PriceMultiplier;
                }
            }

            // Ki?m tra cu?i tu?n
            if (EnableWeekendPricing && WeekendMultiplier > 1.0m)
            {
                if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                {
                    if (WeekendMultiplier > maxMultiplier)
                        maxMultiplier = WeekendMultiplier;
                }
            }

            return maxMultiplier;
        }

        /// <summary>
        /// Tính giá v?i t? l? t?ng theo ngày
        /// </summary>
        public static decimal CalculatePrice(decimal basePrice, DateTime date)
        {
            return basePrice * GetPriceMultiplier(date);
        }

        /// <summary>
        /// Tính t?ng giá cho kho?ng th?i gian (t? ngày ??n ngày)
        /// </summary>
        public static decimal CalculateTotalPrice(decimal basePricePerDay, DateTime checkIn, DateTime checkOut)
        {
            decimal total = 0m;
            for (DateTime date = checkIn.Date; date < checkOut.Date; date = date.AddDays(1))
            {
                total += CalculatePrice(basePricePerDay, date);
            }
            return total;
        }

        /// <summary>
        /// L?y thông tin ngày l? c?a m?t ngày (n?u có)
        /// </summary>
        public static HolidayInfo GetHolidayInfo(DateTime date)
        {
            return Holidays.FirstOrDefault(h => date.Date >= h.StartDate.Date && date.Date <= h.EndDate.Date);
        }

        /// <summary>
        /// Thêm ngày l? tùy ch?nh
        /// </summary>
        public static void AddHoliday(string name, DateTime start, DateTime end, decimal multiplier)
        {
            Holidays.Add(new HolidayInfo
            {
                Name = name,
                StartDate = start,
                EndDate = end,
                PriceMultiplier = multiplier
            });
        }

        /// <summary>
        /// Xóa ngày l? theo tên
        /// </summary>
        public static void RemoveHoliday(string name)
        {
            Holidays.RemoveAll(h => h.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// C?p nh?t T?t Nguyên ?án cho n?m c? th? (vì theo âm l?ch nên c?n set th? công)
        /// </summary>
        public static void SetLunarNewYear(int year, DateTime start, DateTime end, decimal multiplier = 1.5m)
        {
            RemoveHoliday("T?t Nguyên ?án");
            AddHoliday("T?t Nguyên ?án", start, end, multiplier);
        }
    }
}
