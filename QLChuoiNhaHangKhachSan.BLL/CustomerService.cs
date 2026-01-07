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

        /// <summary>
        /// Lấy tất cả khách hàng, trả về danh sách DTO cho GUI.
        /// </summary>
        public List<CustomerDto> GetAllCustomers()
        {
            var customers = _dal.GetAll();
            return customers.Select(MapToDto).ToList();
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
                CreatedAt = entity.CreatedAt
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
                CreatedAt = dto.CreatedAt
            };
        }

        #endregion
    }
}
