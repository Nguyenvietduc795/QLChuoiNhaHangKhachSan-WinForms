using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using QLChuoiNhaHangKhachSan.DAL.Models;
using QLChuoiNhaHangKhachSan.DAL.Repositories;

namespace QLChuoiNhaHangKhachSan.BLL.Services
{
    public class EmployeeService
    {
        private readonly EmployeeRepository _repo;
        public EmployeeService(string connStr)
        {
            _repo = new EmployeeRepository(connStr);
        }

        public List<Employee> GetAll() => _repo.GetAllWithActiveContract();

        public void Add(Employee emp, decimal dealSalary, decimal salaryCoef)
        {
            Validate(emp, dealSalary, salaryCoef);
            _repo.Insert(emp, dealSalary, salaryCoef);
        }

        //hàm cập nhật nhân viên
        private void Validate(Employee emp, decimal dealSalary, decimal salaryCoef)
        {
            if (emp == null) throw new ArgumentNullException(nameof(emp));

            emp.FullName = emp.FullName?.Trim();
            emp.Email = emp.Email?.Trim();
            emp.Department = emp.Department?.Trim();
            emp.Position = emp.Position?.Trim();
            emp.Phone = emp.Phone?.Trim();
            emp.CountryCode = emp.CountryCode?.Trim();

            if (string.IsNullOrWhiteSpace(emp.FullName)) throw new ArgumentException("Tên không được để trống"); 
            if (Regex.IsMatch(emp.FullName, "\\d")) throw new ArgumentException("Tên không được chứa số");
            if (string.IsNullOrWhiteSpace(emp.Email)) throw new ArgumentException("Email không được để trống");
            if (!Regex.IsMatch(emp.Email, @"^[^@\s]+@gmail\.com$", RegexOptions.IgnoreCase)) throw new ArgumentException("Gmail chưa hợp lệ");

            // Country code: cho phép dùng +84, +1, +65... tối đa 4 chữ số
            if (string.IsNullOrWhiteSpace(emp.CountryCode)) emp.CountryCode = "+84";
            if (!Regex.IsMatch(emp.CountryCode, @"^\+\d{1,4}$")) throw new ArgumentException("Mã quốc gia không hợp lệ (ví dụ: +84)");

            if (string.IsNullOrWhiteSpace(emp.Phone) || !Regex.IsMatch(emp.Phone, "^\\d+$")) throw new ArgumentException("Số điện thoại không hợp lệ");
            if (dealSalary < 0) throw new ArgumentException("Lương deal phải >= 0");
            if (salaryCoef < 0) throw new ArgumentException("Hệ số phải >= 0");
            if (emp.HireDate == default(DateTime)) emp.HireDate = DateTime.Today;
            if (string.IsNullOrWhiteSpace(emp.Status)) emp.Status = "Inactive";
        }


        //hàm cập nhật trạng thái nhân viên
        public void UpdateStatus(int employeeId, string newStatus, decimal currentSalary)
        {
            if (string.IsNullOrWhiteSpace(newStatus)) throw new ArgumentException("Trạng thái không hợp lệ", nameof(newStatus));
            // Không cho chuyển sang Inactive nếu lương > 0
            if (string.Equals(newStatus, "Inactive", StringComparison.OrdinalIgnoreCase) && currentSalary > 0)
            {
                throw new InvalidOperationException("Không thể chuyển sang Inactive khi lương > 0");
            }
            _repo.UpdateStatus(employeeId, newStatus);
        }

        //hàm hủy kích hoạt nhân viên
        public void DeactivateEmployee(int employeeId)
        {
            _repo.DeactivateAndResetSalary(employeeId);
        }

        //hàm lọc nhân viên
        public List<Employee> Filter(string term, string status)
        {
            var source = _repo.GetAllWithActiveContract() ?? new List<Employee>();
            IEnumerable<Employee> query = source;

            if (string.Equals(status, "Active", StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(emp => string.Equals(emp?.Status, "Active", StringComparison.OrdinalIgnoreCase));
            }
            else if (string.Equals(status, "Inactive", StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(emp => string.Equals(emp?.Status, "Inactive", StringComparison.OrdinalIgnoreCase));
            }

            term = (term ?? string.Empty).Trim().ToLowerInvariant();
            if (!string.IsNullOrWhiteSpace(term))
            {
                query = query.Where(emp =>
                {
                    if (emp == null) return false;
                    return (emp.FullName ?? string.Empty).ToLowerInvariant().Contains(term)
                        || (emp.Email ?? string.Empty).ToLowerInvariant().Contains(term)
                        || (emp.Department ?? string.Empty).ToLowerInvariant().Contains(term)
                        || (emp.Position ?? string.Empty).ToLowerInvariant().Contains(term)
                        || (emp.Phone ?? string.Empty).ToLowerInvariant().Contains(term);
                });
            }

            return query.ToList();
        }

        //hàm gợi ý tên nhân viên
        public List<string> SuggestNames(string term)
        {
            var source = _repo.GetAllWithActiveContract() ?? new List<Employee>();
            term = (term ?? string.Empty).Trim().ToLowerInvariant();
            var query = source
                .Where(e => e != null && !string.IsNullOrWhiteSpace(e.FullName))
                .Select(e => e.FullName);

            if (!string.IsNullOrWhiteSpace(term))
            {
                query = query.Where(name => name != null && name.ToLowerInvariant().Contains(term));
            }

            return query.Distinct(StringComparer.OrdinalIgnoreCase).ToList();
        }
    }
}
