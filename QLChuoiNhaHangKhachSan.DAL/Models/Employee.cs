using System;

namespace QLChuoiNhaHangKhachSan.DAL.Models
{
    // DTO nhân viên dùng chung gi?a các t?ng DAL/BLL/GUI
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
        public DateTime HireDate { get; set; }
        public string Status { get; set; }
        public decimal Salary { get; set; }
        public string Phone { get; set; }
        public string CountryCode { get; set; }
        public decimal SalaryCoefficient { get; set; }
    }

}
