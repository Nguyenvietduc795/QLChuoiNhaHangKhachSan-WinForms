using System;
using QLChuoiNhaHangKhachSan.DAL;

namespace QLChuoiNhaHangKhachSan.BLL
{
    /// <summary>
    /// Service x? lý logic nghi?p v? liên quan ??n ??t bàn
    /// </summary>
    public class BookingService
    {
        private readonly BookingRepository _bookingRepository;
        private readonly TableRepository _tableRepository;
        private readonly CustomerRepository _customerRepository;

        public BookingService()
        {
            _bookingRepository = new BookingRepository();
            _tableRepository = new TableRepository();
            _customerRepository = new CustomerRepository();
        }

        /// <summary>
        /// L?y ??t bàn m?i nh?t (không l?c tr?ng thái)
        /// </summary>
        public BookingDTO GetLatestBooking()
        {
            return _bookingRepository.GetLatestBooking();
        }

        /// <summary>
        /// L?y ??t bàn ?ang ch? h?y (status = "?ã ??t")
        /// </summary>
        public BookingDTO GetLatestPendingBooking()
        {
            return _bookingRepository.GetLatestBooking("?ã ??t");
        }

        /// <summary>
        /// T?o ??t bàn m?i
        /// </summary>
        public int CreateBooking(string tableName, string customerName, string phone, string email, 
            DateTime bookingDate, TimeSpan bookingTime, int guestCount)
        {
            // Tìm ho?c t?o khách hàng
            int customerId = _customerRepository.FindOrCreate(customerName, phone, email);

            // Tìm bàn
            var table = _tableRepository.GetTableByName(tableName);
            if (table == null)
            {
                throw new Exception("Không tìm th?y bàn trong h? th?ng.");
            }

            // T?o ??t bàn
            int bookingId = _bookingRepository.InsertBooking(table.TableID, customerId, bookingDate, bookingTime, guestCount, "?ã ??t");

            // C?p nh?t tr?ng thái bàn thành "?ã ??t" (StatusID = 3)
            _tableRepository.SetTableReserved(table.TableID);

            return bookingId;
        }

        /// <summary>
        /// H?y ??t bàn
        /// </summary>
        public void CancelBooking(int tableId)
        {
            // C?p nh?t tr?ng thái booking thành "?ã h?y"
            _bookingRepository.CancelBooking(tableId);

            // C?p nh?t tr?ng thái bàn v? tr?ng (StatusID = 1)
            _tableRepository.SetTableEmpty(tableId);
        }

        /// <summary>
        /// H?y ??t bàn theo tên bàn
        /// </summary>
        public void CancelBookingByTableName(string tableName)
        {
            var table = _tableRepository.GetTableByName(tableName);
            if (table == null)
            {
                throw new Exception("Không tìm th?y bàn trong h? th?ng.");
            }

            CancelBooking(table.TableID);
        }
    }
}
