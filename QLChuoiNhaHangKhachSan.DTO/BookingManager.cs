using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace QLChuoiNhaHangKhachSan.GUI
{
    public class ServiceItem
    {
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal Amount { get; set; }
    }

    public class BookingInfo
    {
        public string Customer { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public List<ServiceItem> Services { get; set; } = new List<ServiceItem>();
    }

    public static class BookingManager
    {
        private static ConcurrentDictionary<string, BookingInfo> _bookings = new ConcurrentDictionary<string, BookingInfo>(StringComparer.OrdinalIgnoreCase);

        public static void AddBooking(string roomCode, BookingInfo info)
        {
            if (string.IsNullOrWhiteSpace(roomCode) || info == null) return;
            _bookings[roomCode.Trim().ToUpper()] = info;
        }

        public static void AddBooking(IEnumerable<string> roomCodes, BookingInfo info)
        {
            if (roomCodes == null || info == null) return;
            foreach (var c in roomCodes)
            {
                if (string.IsNullOrWhiteSpace(c)) continue;
                _bookings[c.Trim().ToUpper()] = info;
            }
        }

        public static bool TryGetBooking(string roomCode, out BookingInfo info)
        {
            info = null;
            if (string.IsNullOrWhiteSpace(roomCode)) return false;
            return _bookings.TryGetValue(roomCode.Trim().ToUpper(), out info);
        }

        public static IEnumerable<KeyValuePair<string, BookingInfo>> GetAllBookings()
        {
            return _bookings;
        }

        public static void RemoveBooking(string roomCode)
        {
            if (string.IsNullOrWhiteSpace(roomCode)) return;
            _bookings.TryRemove(roomCode.Trim().ToUpper(), out _);
        }

        public static void Clear()
        {
            _bookings.Clear();
        }
    }
}
