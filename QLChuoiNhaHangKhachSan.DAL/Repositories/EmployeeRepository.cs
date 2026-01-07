using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using QLChuoiNhaHangKhachSan.DAL.Models;

namespace QLChuoiNhaHangKhachSan.DAL.Repositories
{
    public class EmployeeRepository
    {
        private readonly string _connStr;
        public EmployeeRepository(string connStr)
        {
            _connStr = connStr ?? throw new ArgumentNullException(nameof(connStr));
        }

        public List<Employee> GetAllWithActiveContract()
        {
            var result = new List<Employee>();
            const string sql = @"
SELECT e.EmployeeId, e.FullName, e.Email, e.CountryCode, e.Phone,
       d.Name AS Department, p.Name AS Position,
       e.HireDate, e.Status,
       ca.DealSalary, ca.SalaryCoefficient,
       ISNULL(ca.DealSalary * ca.SalaryCoefficient,0) AS Salary
FROM Employees e
LEFT JOIN Departments d ON e.DepartmentId = d.DepartmentId
LEFT JOIN Positions  p ON e.PositionId  = p.PositionId
OUTER APPLY (
    SELECT TOP 1 DealSalary, SalaryCoefficient
    FROM EmploymentContract c
    WHERE c.EmployeeId = e.EmployeeId AND c.IsActive = 1
    ORDER BY c.EffectiveFrom DESC
) ca;";

            using (var conn = new SqlConnection(_connStr))
            using (var cmd = new SqlCommand(sql, conn))
            {
                conn.Open();
                using (var rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        var countryCode = rd["CountryCode"] as string ?? string.Empty;
                        var phoneNumber = rd["Phone"] as string ?? string.Empty;
                        result.Add(new Employee
                        {
                            EmployeeId = rd["EmployeeId"] == DBNull.Value ? 0 : (int)rd["EmployeeId"],
                            FullName = rd["FullName"] as string,
                            Email = rd["Email"] as string,
                            CountryCode = countryCode,
                            Department = rd["Department"] as string ?? "",
                            Position = rd["Position"] as string ?? "",
                            HireDate = rd["HireDate"] == DBNull.Value ? default(DateTime) : (DateTime)rd["HireDate"],
                            Status = rd["Status"] as string,
                            Salary = rd["Salary"] == DBNull.Value ? 0 : (decimal)rd["Salary"],
                            SalaryCoefficient = rd["SalaryCoefficient"] == DBNull.Value ? 0 : (decimal)rd["SalaryCoefficient"],
                            Phone = phoneNumber
                        });
                    }
                }
            }
            return result;
        }

        public void Insert(Employee emp, decimal dealSalary, decimal salaryCoef)
        {
            using (var conn = new SqlConnection(_connStr))
            {
                conn.Open();
                using (var tx = conn.BeginTransaction())
                {
                    int? deptId = ResolveDepartmentId(conn, tx, emp.Department);
                    int? posId = ResolvePositionId(conn, tx, emp.Position, deptId);

                    int employeeId;
                    using (var cmd = new SqlCommand(@"
INSERT INTO Employees (FullName, Email, CountryCode, Phone, DepartmentId, PositionId, HireDate, Status)
OUTPUT inserted.EmployeeId
VALUES (@FullName, @Email, @CountryCode, @Phone, @DeptId, @PosId, @HireDate, @Status);", conn, tx))
                    {
                        cmd.Parameters.AddWithValue("@FullName", (object)emp.FullName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Email", (object)emp.Email ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@CountryCode", (object)emp.CountryCode ?? "+84");
                        cmd.Parameters.AddWithValue("@Phone", (object)emp.Phone ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@DeptId", (object)deptId ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@PosId", (object)posId ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@HireDate", emp.HireDate == default(DateTime) ? DateTime.Today : emp.HireDate);
                        cmd.Parameters.AddWithValue("@Status", emp.Status ?? "Inactive");
                        employeeId = (int)cmd.ExecuteScalar();
                    }

                    using (var cmd = new SqlCommand(@"
INSERT INTO EmploymentContract (EmployeeId, DealSalary, SalaryCoefficient, EffectiveFrom, EffectiveTo, Currency, IsActive)
VALUES (@EmployeeId, @DealSalary, @SalaryCoefficient, @EffectiveFrom, NULL, 'VND', 1);", conn, tx))
                    {
                        cmd.Parameters.AddWithValue("@EmployeeId", employeeId);
                        cmd.Parameters.AddWithValue("@DealSalary", dealSalary);
                        cmd.Parameters.AddWithValue("@SalaryCoefficient", salaryCoef);
                        cmd.Parameters.AddWithValue("@EffectiveFrom", emp.HireDate == default(DateTime) ? DateTime.Today : emp.HireDate);
                        cmd.ExecuteNonQuery();
                    }

                    tx.Commit();
                }
            }
        }

        public void UpdateStatus(int employeeId, string newStatus)
        {
            using (var conn = new SqlConnection(_connStr))
            using (var cmd = new SqlCommand("UPDATE Employees SET Status = @Status WHERE EmployeeId = @Id", conn))
            {
                cmd.Parameters.AddWithValue("@Status", (object)newStatus ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Id", employeeId);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeactivateAndResetSalary(int employeeId)
        {
            using (var conn = new SqlConnection(_connStr))
            {
                conn.Open();
                using (var tx = conn.BeginTransaction())
                {
                    // Deactivate current contracts
                    using (var cmd = new SqlCommand(@"
UPDATE EmploymentContract 
SET IsActive = 0, EffectiveTo = GETDATE() 
WHERE EmployeeId = @Id AND IsActive = 1", conn, tx))
                    {
                        cmd.Parameters.AddWithValue("@Id", employeeId);
                        cmd.ExecuteNonQuery();
                    }

                    // Create new zero-salary contract (DealSalary=0, Coefficient=1)
                    using (var cmd = new SqlCommand(@"
INSERT INTO EmploymentContract (EmployeeId, DealSalary, SalaryCoefficient, EffectiveFrom, EffectiveTo, Currency, IsActive)
VALUES (@Id, 0, 1, GETDATE(), NULL, 'VND', 1)", conn, tx))
                    {
                        cmd.Parameters.AddWithValue("@Id", employeeId);
                        cmd.ExecuteNonQuery();
                    }

                    // Update employee status to Inactive
                    using (var cmd = new SqlCommand(@"
UPDATE Employees SET Status = 'Inactive' WHERE EmployeeId = @Id", conn, tx))
                    {
                        cmd.Parameters.AddWithValue("@Id", employeeId);
                        cmd.ExecuteNonQuery();
                    }

                    tx.Commit();
                }
            }
        }

        private static int? ResolveDepartmentId(SqlConnection conn, SqlTransaction tx, string deptName)
        {
            if (string.IsNullOrWhiteSpace(deptName)) return null;
            using (var cmd = new SqlCommand("SELECT TOP 1 DepartmentId FROM Departments WHERE Name = @Name", conn, tx))
            {
                cmd.Parameters.AddWithValue("@Name", deptName);
                var val = cmd.ExecuteScalar();
                if (val == null || val == DBNull.Value)
                {
                    using (var insertCmd = new SqlCommand("INSERT INTO Departments (Name) OUTPUT inserted.DepartmentId VALUES (@Name)", conn, tx))
                    {
                        insertCmd.Parameters.AddWithValue("@Name", deptName);
                        return Convert.ToInt32(insertCmd.ExecuteScalar());
                    }
                }
                return Convert.ToInt32(val);
            }
        }

        private static int? ResolvePositionId(SqlConnection conn, SqlTransaction tx, string posName, int? deptId)
        {
            if (string.IsNullOrWhiteSpace(posName)) return null;
            using (var cmd = new SqlCommand(@"
SELECT TOP 1 PositionId FROM Positions
WHERE Name = @Name AND (@DeptId IS NULL OR DepartmentId = @DeptId)", conn, tx))
            {
                cmd.Parameters.AddWithValue("@Name", posName);
                cmd.Parameters.AddWithValue("@DeptId", (object)deptId ?? DBNull.Value);
                var val = cmd.ExecuteScalar();
                if (val == null || val == DBNull.Value)
                {
                    using (var insertCmd = new SqlCommand(@"INSERT INTO Positions (Name, DepartmentId)
OUTPUT inserted.PositionId
VALUES (@Name, @DeptId)", conn, tx))
                    {
                        insertCmd.Parameters.AddWithValue("@Name", posName);
                        insertCmd.Parameters.AddWithValue("@DeptId", (object)deptId ?? DBNull.Value);
                        return Convert.ToInt32(insertCmd.ExecuteScalar());
                    }
                }
                return Convert.ToInt32(val);
            }
        }

    }
}
