using System;
using System.Collections.Generic;
using QLChuoiNhaHangKhachSan.DAL.Models;

namespace QLChuoiNhaHangKhachSan.GUI
{
    public static class EmployeeData
    {
        public static List<Employee> Employees = new List<Employee>();
        public static event EventHandler EmployeesChanged;
        public static void NotifyChanged() => EmployeesChanged?.Invoke(null, EventArgs.Empty);
    }
}
