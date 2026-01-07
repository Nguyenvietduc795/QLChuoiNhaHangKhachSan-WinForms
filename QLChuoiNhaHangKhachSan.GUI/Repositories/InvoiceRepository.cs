using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using QLChuoiNhaHangKhachSan.GUI;

namespace QLChuoiNhaHangKhachSan.GUI.Repositories
{
    public static class InvoiceRepository
    {
        // Save invoice and finalize booking/room in single transaction.
        // bookingInfo: the BookingInfo (Start/End/Customer/Services)
        // services: list of ServiceItem extracted from UI
        public static void SaveInvoiceAndFinalize(string roomId, BookingInfo bookingInfo, IEnumerable<ServiceItem> services, decimal totalAmount, string invoiceFileName = null)
        {
            var connStr = ConfigurationManager.ConnectionStrings["ConnStr"]?.ConnectionString;
            if (string.IsNullOrWhiteSpace(connStr))
                throw new InvalidOperationException("Missing connection string 'ConnStr'.");

            using (var conn = new SqlConnection(connStr))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        DateTime now = DateTime.Now;

                        // Try locate BookingID for this room/time (best-effort)
                        int? bookingId = null;
                        using (var cmd = new SqlCommand(@"SELECT TOP 1 BookingID FROM dbo.BookingDetail
WHERE RoomID = @RoomID AND CheckIn = @CheckIn AND CheckOut = @CheckOut", conn, tran))
                        {
                            cmd.Parameters.AddWithValue("@RoomID", roomId);
                            cmd.Parameters.AddWithValue("@CheckIn", bookingInfo?.Start ?? DateTime.MinValue);
                            cmd.Parameters.AddWithValue("@CheckOut", bookingInfo?.End ?? DateTime.MinValue);
                            var obj = cmd.ExecuteScalar();
                            if (obj != null && obj != DBNull.Value) bookingId = Convert.ToInt32(obj);
                        }

                        // Insert Invoice
                        int invoiceId;
                        using (var cmd = new SqlCommand(@"INSERT INTO dbo.Invoice(BookingID, RoomID, InvoiceDate, TotalAmount, FileName, Note)
VALUES(@BookingID, @RoomID, @InvoiceDate, @Amount, @FileName, NULL);
SELECT CAST(SCOPE_IDENTITY() AS INT);", conn, tran))
                        {
                            cmd.Parameters.AddWithValue("@BookingID", bookingId.HasValue ? (object)bookingId.Value : (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@RoomID", roomId ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@InvoiceDate", now);
                            cmd.Parameters.AddWithValue("@Amount", totalAmount);
                            cmd.Parameters.AddWithValue("@FileName", string.IsNullOrWhiteSpace(invoiceFileName) ? (object)DBNull.Value : invoiceFileName);
                            invoiceId = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        // Insert invoice details for each service
                        if (services != null)
                        {
                            using (var cmd = new SqlCommand(@"INSERT INTO dbo.InvoiceDetail(InvoiceID, ServiceName, Quantity, UnitPrice, Amount)
VALUES(@InvoiceID, @ServiceName, @Quantity, @UnitPrice, @Amount);", conn, tran))
                            {
                                cmd.Parameters.Add("@InvoiceID", SqlDbType.Int).Value = invoiceId;
                                cmd.Parameters.Add("@ServiceName", SqlDbType.NVarChar, 200);
                                cmd.Parameters.Add("@Quantity", SqlDbType.Int);
                                cmd.Parameters.Add("@UnitPrice", SqlDbType.Decimal).Precision = 18;
                                cmd.Parameters["@UnitPrice"].Scale = 2;
                                cmd.Parameters.Add("@Amount", SqlDbType.Decimal).Precision = 18;
                                cmd.Parameters["@Amount"].Scale = 2;

                                foreach (var s in services)
                                {
                                    cmd.Parameters["@ServiceName"].Value = s.Name;
                                    cmd.Parameters["@Quantity"].Value = s.Quantity;
                                    cmd.Parameters["@UnitPrice"].Value = s.UnitPrice;
                                    cmd.Parameters["@Amount"].Value = s.Amount;
                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }

                        // Close active booking details for this room so UI will show room as free
                        using (var cmd = new SqlCommand(@"
UPDATE dbo.BookingDetail
SET CheckOut = @Now
WHERE RoomID = @RoomID AND CheckOut > @Now", conn, tran))
                        {
                            cmd.Parameters.AddWithValue("@Now", now);
                            cmd.Parameters.AddWithValue("@RoomID", roomId);
                            cmd.ExecuteNonQuery();
                        }

                        // Mark Booking.Status = 'Paid' when bookingId exists
                        if (bookingId.HasValue)
                        {
                            using (var cmd = new SqlCommand("UPDATE dbo.Booking SET Status = @Status WHERE BookingID = @BookingID", conn, tran))
                            {
                                cmd.Parameters.AddWithValue("@Status", "Paid");
                                cmd.Parameters.AddWithValue("@BookingID", bookingId.Value);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        // Update Room.Status -> Vacant (use the value your UI expects)
                        using (var cmd = new SqlCommand("UPDATE dbo.Room SET Status = @Status WHERE RoomID = @RoomID", conn, tran))
                        {
                            cmd.Parameters.AddWithValue("@Status", "Vacant");
                            cmd.Parameters.AddWithValue("@RoomID", roomId);
                            cmd.ExecuteNonQuery();
                        }

                        tran.Commit();
                    }
                    catch
                    {
                        try { tran.Rollback(); } catch { }
                        throw;
                    }
                }
            }
        }
    }
}