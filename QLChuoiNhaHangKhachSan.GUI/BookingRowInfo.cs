using System;

namespace QLChuoiNhaHangKhachSan.GUI
{
    public class BookingRowInfo
    {
        public string Id { get; set; }
        public string Customer { get; set; }
        public string Date { get; set; }
        public string Staff { get; set; }
        public string Detail { get; set; }
        public string CCCD { get; set; }
        public string SDT { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string Nationality { get; set; }
        public DateTime? CreatedDate { get; set; }

        // New: cancellation info
        public bool IsCancelled { get; set; }
        public string CancellationReason { get; set; }
    }
}
