using System.Collections.Generic;
using QLChuoiNhaHangKhachSan.DAL;
using QLChuoiNhaHangKhachSan.DAL.Models;

namespace QLChuoiNhaHangKhachSan.BLL
{
    // Service tầng BLL, dùng để viết nghiệp vụ xung quanh khách hàng
    // và gọi xuống Repository (CustomerDal) để truy cập database.
    public class CustomerService
    {
        private readonly CustomerDal _dal = new CustomerDal();

        public List<Customer> GetAllCustomers()
        {
            // Có thể thêm logic filter, sort, validate ở đây nếu cần
            return _dal.GetAll();
        }

        public int AddCustomer(Customer customer)
        {
            // Ví dụ: validate dữ liệu trước khi lưu
            // (hiện tại giữ nguyên để không phá chức năng)
            return _dal.Insert(customer);
        }

        public void UpdateCustomer(Customer customer)
        {
            _dal.Update(customer);
        }

        public void DeleteCustomer(int customerId)
        {
            _dal.Delete(customerId);
        }
    }
}
