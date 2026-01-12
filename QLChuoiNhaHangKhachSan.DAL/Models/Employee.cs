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
        
        /// <summary>
        /// L??ng deal (t? EmploymentContract.DealSalary)
        /// </summary>
        public decimal DealSalary { get; set; }
        
        /// <summary>
        /// H? s? l??ng (t? EmploymentContract.SalaryCoefficient)
        /// </summary>
        public decimal SalaryCoefficient { get; set; }
        
        /// <summary>
        /// L??ng th?c t? = DealSalary * SalaryCoefficient
        /// </summary>
        public decimal Salary { get; set; }
        
        public string Phone { get; set; }
        public string CountryCode { get; set; }
        
        /// <summary>
        /// Tên ??ng nh?p (Username) c?a nhân viên
        /// </summary>
        public string UserName { get; set; }
        
        /// <summary>
        /// C? ?ánh d?u ph?i ??i m?t kh?u khi ??ng nh?p l?n ??u
        /// </summary>
        public bool MustChangePassword { get; set; }
    }
}
