using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace QLChuoiNhaHangKhachSan.DAL
{
    /// <summary>
    /// Repository x? lý truy v?n d? li?u hóa ??n/order
    /// </summary>
    public class OrderRepository : BaseRepository
    {
        public OrderRepository() : base() { }

        /// <summary>
        /// L?y thông tin order theo OrderId
        /// </summary>
        public OrderDTO GetOrderById(int orderId)
        {
            string query = @"SELECT o.OrderId, o.TableID, rt.TableName, o.CustomerID, 
                                    c.FullName AS CustomerName, c.Email AS CustomerEmail,
                                    o.GuestCount, o.DateCheckIn, o.DateCheckOut, o.Status,
                                    o.SubTotal, o.Discount, o.Surcharge, o.TotalAmount
                             FROM OrderTicket o
                             LEFT JOIN Customers c ON o.CustomerID = c.CustomerId
                             LEFT JOIN RestaurantTable rt ON o.TableID = rt.TableID
                             WHERE o.OrderId = @OrderId";

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@OrderId", orderId);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapOrder(reader);
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// L?y order pending m?i nh?t c?a m?t bàn
        /// </summary>
        public OrderDTO GetPendingOrderByTable(int tableId)
        {
            string query = @"SELECT TOP 1 o.OrderId, o.TableID, rt.TableName, o.CustomerID, 
                                    c.FullName AS CustomerName, c.Email AS CustomerEmail,
                                    o.GuestCount, o.DateCheckIn, o.DateCheckOut, o.Status,
                                    o.SubTotal, o.Discount, o.Surcharge, o.TotalAmount
                             FROM OrderTicket o
                             LEFT JOIN Customers c ON o.CustomerID = c.CustomerId
                             LEFT JOIN RestaurantTable rt ON o.TableID = rt.TableID
                             WHERE o.TableID = @TableID AND o.Status = N'Pending'
                             ORDER BY o.OrderId DESC";

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@TableID", tableId);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapOrder(reader);
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// L?y danh sách chi ti?t món theo OrderId
        /// </summary>
        public List<OrderItemDTO> GetOrderItems(int orderId)
        {
            var items = new List<OrderItemDTO>();
            string query = @"SELECT oti.OrderItemId, oti.OrderId, oti.FoodID, f.FoodName, 
                                    oti.Quantity, oti.UnitPrice, oti.LineTotal, oti.Note
                             FROM OrderTicketItem oti
                             INNER JOIN Food f ON oti.FoodID = f.FoodID
                             WHERE oti.OrderId = @OrderId";

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@OrderId", orderId);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        items.Add(new OrderItemDTO
                        {
                            OrderItemId = reader["OrderItemId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["OrderItemId"]),
                            OrderId = Convert.ToInt32(reader["OrderId"]),
                            FoodID = Convert.ToInt32(reader["FoodID"]),
                            FoodName = reader["FoodName"].ToString(),
                            Quantity = Convert.ToInt32(reader["Quantity"]),
                            UnitPrice = Convert.ToDecimal(reader["UnitPrice"]),
                            LineTotal = Convert.ToDecimal(reader["LineTotal"]),
                            Note = reader["Note"] == DBNull.Value ? null : reader["Note"].ToString()
                        });
                    }
                }
            }

            return items;
        }

        /// <summary>
        /// T?o order m?i
        /// </summary>
        public int InsertOrder(int tableId, int customerId, int guestCount, decimal subTotal)
        {
            string query = @"INSERT INTO OrderTicket (TableID, CustomerID, GuestCount, DateCheckIn, Status, SubTotal, TotalAmount)
                             VALUES (@TableID, @CustomerID, @GuestCount, GETDATE(), N'Pending', @SubTotal, @SubTotal);
                             SELECT SCOPE_IDENTITY();";

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@TableID", tableId);
                cmd.Parameters.AddWithValue("@CustomerID", customerId);
                cmd.Parameters.AddWithValue("@GuestCount", guestCount);
                cmd.Parameters.AddWithValue("@SubTotal", subTotal);

                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        /// <summary>
        /// Thêm món vào order (s? d?ng connection và transaction t? ngoài)
        /// </summary>
        public void InsertOrderItem(SqlConnection conn, SqlTransaction tran, int orderId, int foodId, int quantity, decimal unitPrice, string note)
        {
            string query = @"INSERT INTO OrderTicketItem (OrderId, FoodID, Quantity, UnitPrice, Note)
                             VALUES (@OrderId, @FoodID, @Quantity, @UnitPrice, @Note);";

            using (var cmd = new SqlCommand(query, conn, tran))
            {
                cmd.Parameters.AddWithValue("@OrderId", orderId);
                cmd.Parameters.AddWithValue("@FoodID", foodId);
                cmd.Parameters.AddWithValue("@Quantity", quantity);
                cmd.Parameters.AddWithValue("@UnitPrice", unitPrice);
                cmd.Parameters.AddWithValue("@Note", string.IsNullOrEmpty(note) ? (object)DBNull.Value : note);

                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// C?p nh?t t?ng ti?n order
        /// </summary>
        public void UpdateOrderTotal(SqlConnection conn, SqlTransaction tran, int orderId, decimal subTotal)
        {
            string query = "UPDATE OrderTicket SET SubTotal = @SubTotal, TotalAmount = @SubTotal WHERE OrderId = @OrderId";

            using (var cmd = new SqlCommand(query, conn, tran))
            {
                cmd.Parameters.AddWithValue("@SubTotal", subTotal);
                cmd.Parameters.AddWithValue("@OrderId", orderId);
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Hoàn thành order (thanh toán)
        /// </summary>
        public void CompleteOrder(int orderId)
        {
            string query = "UPDATE OrderTicket SET Status = N'Completed', DateCheckOut = GETDATE() WHERE OrderId = @OrderId";

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@OrderId", orderId);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Thêm giao d?ch thanh toán
        /// </summary>
        public void InsertTransaction(SqlConnection conn, SqlTransaction tran, int orderId, string paymentMethod, decimal amount)
        {
            string query = "INSERT INTO Transactions (OrderId, PaymentMethod, Amount) VALUES (@OrderId, @PaymentMethod, @Amount)";

            using (var cmd = new SqlCommand(query, conn, tran))
            {
                cmd.Parameters.AddWithValue("@OrderId", orderId);
                cmd.Parameters.AddWithValue("@PaymentMethod", paymentMethod);
                cmd.Parameters.AddWithValue("@Amount", amount);
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Tính t?ng ti?n t? chi ti?t món
        /// </summary>
        public decimal CalculateTotalFromItems(int orderId)
        {
            string query = "SELECT ISNULL(SUM(LineTotal), 0) FROM OrderTicketItem WHERE OrderId = @OrderId";

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@OrderId", orderId);
                conn.Open();
                return Convert.ToDecimal(cmd.ExecuteScalar());
            }
        }

        /// <summary>
        /// M? connection ?? s? d?ng transaction
        /// </summary>
        public SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

        private OrderDTO MapOrder(SqlDataReader reader)
        {
            return new OrderDTO
            {
                OrderId = Convert.ToInt32(reader["OrderId"]),
                TableID = Convert.ToInt32(reader["TableID"]),
                TableName = reader["TableName"] == DBNull.Value ? null : reader["TableName"].ToString(),
                CustomerID = reader["CustomerID"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["CustomerID"]),
                CustomerName = reader["CustomerName"] == DBNull.Value ? null : reader["CustomerName"].ToString(),
                CustomerEmail = reader["CustomerEmail"] == DBNull.Value ? null : reader["CustomerEmail"].ToString(),
                GuestCount = reader["GuestCount"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["GuestCount"]),
                DateCheckIn = reader["DateCheckIn"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["DateCheckIn"]),
                DateCheckOut = reader["DateCheckOut"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["DateCheckOut"]),
                Status = reader["Status"] == DBNull.Value ? null : reader["Status"].ToString(),
                SubTotal = reader["SubTotal"] == DBNull.Value ? 0m : Convert.ToDecimal(reader["SubTotal"]),
                Discount = reader["Discount"] == DBNull.Value ? 0m : Convert.ToDecimal(reader["Discount"]),
                Surcharge = reader["Surcharge"] == DBNull.Value ? 0m : Convert.ToDecimal(reader["Surcharge"]),
                TotalAmount = reader["TotalAmount"] == DBNull.Value ? 0m : Convert.ToDecimal(reader["TotalAmount"])
            };
        }
    }
}
