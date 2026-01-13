using System;
using System.Collections.Generic;
using System.Linq;
using QLChuoiNhaHangKhachSan.BLL.DTOs;
using QLChuoiNhaHangKhachSan.DAL;
using QLChuoiNhaHangKhachSan.DAL.Models;

namespace QLChuoiNhaHangKhachSan.BLL
{
    /// <summary>
    /// Service tầng BLL, dùng để viết nghiệp vụ xung quanh khách hàng
    /// và gọi xuống Repository (CustomerDal) để truy cập database.
    /// GUI chỉ giao tiếp với BLL thông qua CustomerDto.
    /// </summary>
    public class CustomerService
    {
        private readonly CustomerDal _dal = new CustomerDal();
        private const decimal VipThreshold = 30_000_000m;

        /// <summary>
        /// Lấy tất cả khách hàng, trả về danh sách DTO cho GUI.
        /// </summary>
        public List<CustomerDto> GetAllCustomers()
        {
            var customers = _dal.GetAll();
            return customers.Select(MapToDto).ToList();
        }

        /// <summary>
        /// Lấy khách hàng theo tên đầy đủ (so khớp chính xác).
        /// </summary>
        public CustomerDto GetCustomerByFullName(string fullName)
        {
            var customer = _dal.GetByFullName(fullName);
            return customer == null ? null : MapToDto(customer);
        }

        /// <summary>
        /// Thêm khách hàng mới, nhận DTO từ GUI, trả về ID mới.
        /// </summary>
        public int AddCustomer(CustomerDto dto)
        {
            var customer = MapToEntity(dto);
            return _dal.Insert(customer);
        }

        /// <summary>
        /// Cập nhật khách hàng, nhận DTO từ GUI.
        /// </summary>
        public void UpdateCustomer(CustomerDto dto)
        {
            var customer = MapToEntity(dto);
            _dal.Update(customer);
        }

        /// <summary>
        /// Xóa khách hàng theo ID.
        /// </summary>
        public void DeleteCustomer(int customerId)
        {
            _dal.Delete(customerId);
        }

        /// <summary>
        /// Cộng/trừ chi tiêu cho khách hàng theo tên và tự động nâng hạng khi vượt ngưỡng.
        /// </summary>
        public void AdjustSpending(string fullName, decimal delta)
        {
            if (string.IsNullOrWhiteSpace(fullName) || delta == 0) return;

            var customer = _dal.GetByFullName(fullName);
            if (customer == null) return;

            customer.TotalSpending += delta;
            if (customer.TotalSpending < 0) customer.TotalSpending = 0;

            if (customer.TotalSpending >= VipThreshold && !string.Equals(customer.CustomerType, "VIP", StringComparison.OrdinalIgnoreCase))
            {
                customer.CustomerType = "VIP";
            }

            _dal.UpdateSpendingInfo(customer.CustomerId, customer.TotalSpending, customer.CustomerType);
        }

        #region Mapping Methods

        /// <summary>
        /// Chuyển từ Entity (DAL) sang DTO (BLL).
        /// </summary>
        private CustomerDto MapToDto(Customer entity)
        {
            return new CustomerDto
            {
                CustomerId = entity.CustomerId,
                FullName = entity.FullName,
                Nationality = entity.Nationality,
                CCCD = entity.CCCD,
                Sex = entity.Sex,
                PhoneNumber = entity.PhoneNumber,
                Email = entity.Email,
                Address = entity.Address,
                CustomerType = entity.CustomerType,
                CreatedAt = entity.CreatedAt,
                TotalSpending = entity.TotalSpending
            };
        }

        /// <summary>
        /// Chuyển từ DTO (BLL) sang Entity (DAL).
        /// </summary>
        private Customer MapToEntity(CustomerDto dto)
        {
            return new Customer
            {
                CustomerId = dto.CustomerId,
                FullName = dto.FullName,
                Nationality = dto.Nationality,
                CCCD = dto.CCCD,
                Sex = dto.Sex,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email,
                Address = dto.Address,
                CustomerType = dto.CustomerType,
                CreatedAt = dto.CreatedAt,
                TotalSpending = dto.TotalSpending
            };
        }

        #endregion
    }
}
