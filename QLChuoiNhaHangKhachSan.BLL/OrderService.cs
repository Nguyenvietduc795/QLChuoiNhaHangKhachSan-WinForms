using System;
using System.Collections.Generic;
using QLChuoiNhaHangKhachSan.DAL;

namespace QLChuoiNhaHangKhachSan.BLL
{
    /// <summary>
    /// DTO cho món ?n trong gi? hàng (g?i món)
    /// </summary>
    public class CartItemDTO
    {
        public int FoodID { get; set; }
        public string FoodName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string Note { get; set; }
    }

    /// <summary>
    /// Service x? lý logic nghi?p v? liên quan ??n order/hóa ??n
    /// </summary>
    public class OrderService
    {
        private readonly OrderRepository _orderRepository;
        private readonly TableRepository _tableRepository;
        private readonly CustomerRepository _customerRepository;

        public OrderService()
        {
            _orderRepository = new OrderRepository();
            _tableRepository = new TableRepository();
            _customerRepository = new CustomerRepository();
        }

        /// <summary>
        /// L?y thông tin order theo ID
        /// </summary>
        public OrderDTO GetOrderById(int orderId)
        {
            return _orderRepository.GetOrderById(orderId);
        }

        /// <summary>
        /// L?y danh sách chi ti?t món c?a order
        /// </summary>
        public List<OrderItemDTO> GetOrderItems(int orderId)
        {
            return _orderRepository.GetOrderItems(orderId);
        }

        /// <summary>
        /// L?y order pending c?a bàn
        /// </summary>
        public OrderDTO GetPendingOrderByTableName(string tableName)
        {
            var table = _tableRepository.GetTableByName(tableName);
            if (table == null) return null;

            return _orderRepository.GetPendingOrderByTable(table.TableID);
        }

        /// <summary>
        /// T?o ho?c c?p nh?t order v?i các món m?i
        /// </summary>
        public int CreateOrUpdateOrder(string tableName, int customerId, int guestCount, List<CartItemDTO> items)
        {
            var table = _tableRepository.GetTableByName(tableName);
            if (table == null)
            {
                throw new Exception("Không tìm th?y bàn trong h? th?ng.");
            }

            // Tính t?ng ti?n các món m?i
            decimal addedSubTotal = 0m;
            foreach (var item in items)
            {
                addedSubTotal += item.Quantity * item.UnitPrice;
            }

            using (var conn = _orderRepository.CreateConnection())
            {
                conn.Open();
                var tran = conn.BeginTransaction();

                try
                {
                    // Tìm order pending hi?n có
                    var existingOrder = _orderRepository.GetPendingOrderByTable(table.TableID);
                    int orderId;
                    decimal currentSubTotal = 0m;

                    if (existingOrder != null)
                    {
                        orderId = existingOrder.OrderId;
                        currentSubTotal = existingOrder.SubTotal;
                    }
                    else
                    {
                        // T?o order m?i
                        orderId = _orderRepository.InsertOrder(table.TableID, customerId, guestCount, addedSubTotal);
                    }

                    // Thêm các món m?i
                    foreach (var item in items)
                    {
                        _orderRepository.InsertOrderItem(conn, tran, orderId, item.FoodID, item.Quantity, item.UnitPrice, item.Note);
                    }

                    // C?p nh?t t?ng ti?n c?ng d?n
                    decimal newSubTotal = currentSubTotal + addedSubTotal;
                    _orderRepository.UpdateOrderTotal(conn, tran, orderId, newSubTotal);

                    // C?p nh?t tr?ng thái bàn thành "Có khách" (StatusID = 2)
                    _tableRepository.UpdateTableStatus(conn, tran, table.TableID, 2);

                    tran.Commit();
                    return orderId;
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// Thanh toán order
        /// </summary>
        public void CompletePayment(int orderId, string paymentMethod = "Ti?n m?t")
        {
            var order = _orderRepository.GetOrderById(orderId);
            if (order == null)
            {
                throw new Exception("Không tìm th?y hóa ??n.");
            }

            decimal totalAmount = order.TotalAmount;
            if (totalAmount <= 0)
            {
                totalAmount = _orderRepository.CalculateTotalFromItems(orderId);
            }

            using (var conn = _orderRepository.CreateConnection())
            {
                conn.Open();
                var tran = conn.BeginTransaction();

                try
                {
                    // C?p nh?t tr?ng thái order
                    using (var cmd = new System.Data.SqlClient.SqlCommand(
                        "UPDATE OrderTicket SET Status = N'Completed', DateCheckOut = GETDATE() WHERE OrderId = @OrderId", conn, tran))
                    {
                        cmd.Parameters.AddWithValue("@OrderId", orderId);
                        cmd.ExecuteNonQuery();
                    }

                    // Thêm giao d?ch thanh toán
                    _orderRepository.InsertTransaction(conn, tran, orderId, paymentMethod, totalAmount);

                    // C?p nh?t tr?ng thái bàn v? tr?ng
                    _tableRepository.UpdateTableStatus(conn, tran, order.TableID, 1);

                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// L?y danh sách món theo bàn (t? order pending)
        /// </summary>
        public List<OrderItemDTO> GetDishListByTableName(string tableName)
        {
            var table = _tableRepository.GetTableByName(tableName);
            if (table == null) return new List<OrderItemDTO>();

            var order = _orderRepository.GetPendingOrderByTable(table.TableID);
            if (order == null) return new List<OrderItemDTO>();

            return _orderRepository.GetOrderItems(order.OrderId);
        }
    }
}
