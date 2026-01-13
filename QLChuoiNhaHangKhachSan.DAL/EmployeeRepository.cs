using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using QLChuoiNhaHangKhachSan.DAL.Models;

namespace QLChuoiNhaHangKhachSan.DAL.Repositories
{
    public class EmployeeRepository
    {
        private readonly string _connStr;

        /// <summary>
        /// L?y connection string t? c?u hình
        /// </summary>
        private static string GetConnectionString()
        {
            var connStrSetting = ConfigurationManager.ConnectionStrings["ConnStr"];
            if (connStrSetting != null && !string.IsNullOrWhiteSpace(connStrSetting.ConnectionString))
            {
                return connStrSetting.ConnectionString;
            }
            connStrSetting = ConfigurationManager.ConnectionStrings["DbConnection"];
            return connStrSetting?.ConnectionString ?? string.Empty;
        }

        /// <summary>
        /// Constructor m?c ??nh - s? d?ng connection string t? c?u hình
        /// </summary>
        public EmployeeRepository()
        {
            _connStr = GetConnectionString();
        }

        /// <summary>
        /// Constructor v?i connection string tùy ch?nh
        /// </summary>
        public EmployeeRepository(string connStr)
        {
            _connStr = connStr ?? GetConnectionString();
        }

        public List<Employee> GetAllWithActiveContract()
        {
            var result = new List<Employee>();
            const string sql = @"
SELECT e.EmployeeId, e.FullName, e.Email, e.Phone, e.UserName, e.MustChangePassword,
       d.Name AS Department, p.Name AS Position,
       e.HireDate, e.Status,
       ca.DealSalary, ca.SalaryCoefficient,
       ISNULL(ca.DealSalary * ca.SalaryCoefficient,0) AS Salary
FROM Employees e
LEFT JOIN Positions p ON e.PositionId = p.PositionId
LEFT JOIN Departments d ON p.DepartmentId = d.DepartmentId
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
                        var phoneNumber = rd["Phone"] as string ?? string.Empty;
                        result.Add(new Employee
                        {
                            EmployeeId = rd["EmployeeId"] == DBNull.Value ? 0 : (int)rd["EmployeeId"],
                            FullName = rd["FullName"] as string,
                            Email = rd["Email"] as string,
                            CountryCode = string.Empty,
                            Department = rd["Department"] as string ?? "",
                            Position = rd["Position"] as string ?? "",
                            HireDate = rd["HireDate"] == DBNull.Value ? default(DateTime) : (DateTime)rd["HireDate"],
                            Status = rd["Status"] as string,
                            DealSalary = rd["DealSalary"] == DBNull.Value ? 0 : (decimal)rd["DealSalary"],
                            SalaryCoefficient = rd["SalaryCoefficient"] == DBNull.Value ? 0 : (decimal)rd["SalaryCoefficient"],
                            Salary = rd["Salary"] == DBNull.Value ? 0 : (decimal)rd["Salary"],
                            Phone = phoneNumber,
                            UserName = rd["UserName"] as string,
                            MustChangePassword = rd["MustChangePassword"] != DBNull.Value && (bool)rd["MustChangePassword"]
                        });
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Thêm nhân viên m?i và t?o tài kho?n ??ng nh?p
        /// </summary>
        /// <param name="emp">Thông tin nhân viên</param>
        /// <param name="dealSalary">L??ng deal</param>
        /// <param name="salaryCoef">H? s? l??ng</param>
        /// <param name="username">Tên ??ng nh?p</param>
        /// <param name="passwordHash">M?t kh?u ?ã hash</param>
        /// <param name="passwordSalt">Salt c?a m?t kh?u</param>
        /// <returns>ID c?a nhân viên v?a t?o</returns>
        public int InsertWithCredentials(Employee emp, decimal dealSalary, decimal salaryCoef, 
            string username, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var conn = new SqlConnection(_connStr))
            {
                conn.Open();
                using (var tx = conn.BeginTransaction())
                {
                    // Ki?m tra Email ?ã t?n t?i
                    if (!string.IsNullOrWhiteSpace(emp.Email))
                    {
                        using (var checkCmd = new SqlCommand("SELECT COUNT(*) FROM Employees WHERE Email = @Email", conn, tx))
                        {
                            checkCmd.Parameters.AddWithValue("@Email", emp.Email);
                            int emailCount = (int)checkCmd.ExecuteScalar();
                            if (emailCount > 0)
                            {
                                throw new InvalidOperationException($"Email '{emp.Email}' ?ã t?n t?i trong h? th?ng");
                            }
                        }
                    }

                    // Ki?m tra Phone ?ã t?n t?i
                    if (!string.IsNullOrWhiteSpace(emp.Phone))
                    {
                        using (var checkCmd = new SqlCommand("SELECT COUNT(*) FROM Employees WHERE Phone = @Phone", conn, tx))
                        {
                            checkCmd.Parameters.AddWithValue("@Phone", emp.Phone);
                            int phoneCount = (int)checkCmd.ExecuteScalar();
                            if (phoneCount > 0)
                            {
                                throw new InvalidOperationException($"S? ?i?n tho?i '{emp.Phone}' ?ã t?n t?i trong h? th?ng");
                            }
                        }
                    }

                    // Ki?m tra UserName ?ã t?n t?i
                    if (!string.IsNullOrWhiteSpace(username))
                    {
                        using (var checkCmd = new SqlCommand("SELECT COUNT(*) FROM Employees WHERE UserName = @UserName", conn, tx))
                        {
                            checkCmd.Parameters.AddWithValue("@UserName", username);
                            int userCount = (int)checkCmd.ExecuteScalar();
                            if (userCount > 0)
                            {
                                throw new InvalidOperationException($"Tên ??ng nh?p '{username}' ?ã t?n t?i trong h? th?ng");
                            }
                        }
                    }

                    var departmentName = string.IsNullOrWhiteSpace(emp.Department) ? "Unassigned" : emp.Department.Trim();
                    int? deptId = ResolveDepartmentId(conn, tx, departmentName);
                    int? posId = ResolvePositionId(conn, tx, emp.Position, deptId);

                    int employeeId;
                    using (var cmd = new SqlCommand(@"
INSERT INTO Employees (FullName, Email, Phone, PositionId, HireDate, Status, UserName, PasswordHash, PasswordSalt, MustChangePassword)
OUTPUT inserted.EmployeeId
VALUES (@FullName, @Email, @Phone, @PosId, @HireDate, @Status, @UserName, @PasswordHash, @PasswordSalt, @MustChangePassword);", conn, tx))
                    {
                        cmd.Parameters.AddWithValue("@FullName", (object)(emp.FullName ?? string.Empty));
                        cmd.Parameters.AddWithValue("@Email", (object)(emp.Email) ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Phone", (object)(emp.Phone) ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@PosId", (object)posId ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@HireDate", emp.HireDate == default(DateTime) ? DateTime.Today : emp.HireDate);
                        // Khi t?o tài kho?n m?i, Status là "Inactive", s? chuy?n thành "Active" khi ??ng nh?p l?n ??u
                        cmd.Parameters.AddWithValue("@Status", "Inactive");
                        cmd.Parameters.AddWithValue("@UserName", (object)username ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@PasswordHash", (object)passwordHash ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@PasswordSalt", (object)passwordSalt ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@MustChangePassword", true);
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
                    return employeeId;
                }
            }
        }

        public void Insert(Employee emp, decimal dealSalary, decimal salaryCoef)
        {
            using (var conn = new SqlConnection(_connStr))
            {
                conn.Open();
                using (var tx = conn.BeginTransaction())
                {
                    // Ki?m tra Email ?ã t?n t?i (ch? khi thêm m?i)
                    if (!string.IsNullOrWhiteSpace(emp.Email))
                    {
                        using (var checkCmd = new SqlCommand("SELECT COUNT(*) FROM Employees WHERE Email = @Email", conn, tx))
                        {
                            checkCmd.Parameters.AddWithValue("@Email", emp.Email);
                            int emailCount = (int)checkCmd.ExecuteScalar();
                            if (emailCount > 0)
                            {
                                throw new InvalidOperationException($"Email '{emp.Email}' ?ã t?n t?i trong h? th?ng");
                            }
                        }
                    }

                    // Ki?m tra Phone ?ã t?n t?i (ch? khi thêm m?i)
                    if (!string.IsNullOrWhiteSpace(emp.Phone))
                    {
                        using (var checkCmd = new SqlCommand("SELECT COUNT(*) FROM Employees WHERE Phone = @Phone", conn, tx))
                        {
                            checkCmd.Parameters.AddWithValue("@Phone", emp.Phone);
                            int phoneCount = (int)checkCmd.ExecuteScalar();
                            if (phoneCount > 0)
                            {
                                throw new InvalidOperationException($"S? ?i?n tho?i '{emp.Phone}' ?ã t?n t?i trong h? th?ng");
                            }
                        }
                    }

                    var departmentName = string.IsNullOrWhiteSpace(emp.Department) ? "Unassigned" : emp.Department.Trim();
                    int? deptId = ResolveDepartmentId(conn, tx, departmentName);
                    int? posId = ResolvePositionId(conn, tx, emp.Position, deptId);

                    int employeeId;
                    using (var cmd = new SqlCommand(@"
INSERT INTO Employees (FullName, Email, Phone, PositionId, HireDate, Status)
OUTPUT inserted.EmployeeId
VALUES (@FullName, @Email, @Phone, @PosId, @HireDate, @Status);", conn, tx))
                    {
                        cmd.Parameters.AddWithValue("@FullName", (object)(emp.FullName ?? string.Empty));
                        cmd.Parameters.AddWithValue("@Email", (object)(emp.Email) ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Phone", (object)(emp.Phone) ?? DBNull.Value);
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

        /// <summary>
        /// C?p nh?t thông tin nhân viên (bao g?m c? h?p ??ng l??ng)
        /// </summary>
        public void Update(Employee emp, decimal dealSalary, decimal salaryCoef)
        {
            using (var conn = new SqlConnection(_connStr))
            {
                conn.Open();
                using (var tx = conn.BeginTransaction())
                {
                    // Ki?m tra Email trùng (ngo?i tr? chính nhân viên ?ang update)
                    if (!string.IsNullOrWhiteSpace(emp.Email))
                    {
                        using (var checkCmd = new SqlCommand(
                            "SELECT COUNT(*) FROM Employees WHERE Email = @Email AND EmployeeId != @Id", conn, tx))
                        {
                            checkCmd.Parameters.AddWithValue("@Email", emp.Email);
                            checkCmd.Parameters.AddWithValue("@Id", emp.EmployeeId);
                            int emailCount = (int)checkCmd.ExecuteScalar();
                            if (emailCount > 0)
                            {
                                throw new InvalidOperationException($"Email '{emp.Email}' ?ã t?n t?i trong h? th?ng");
                            }
                        }
                    }

                    // Ki?m tra Phone trùng
                    if (!string.IsNullOrWhiteSpace(emp.Phone))
                    {
                        using (var checkCmd = new SqlCommand(
                            "SELECT COUNT(*) FROM Employees WHERE Phone = @Phone AND EmployeeId != @Id", conn, tx))
                        {
                            checkCmd.Parameters.AddWithValue("@Phone", emp.Phone);
                            checkCmd.Parameters.AddWithValue("@Id", emp.EmployeeId);
                            int phoneCount = (int)checkCmd.ExecuteScalar();
                            if (phoneCount > 0)
                            {
                                throw new InvalidOperationException($"S? ?i?n tho?i '{emp.Phone}' ?ã t?n t?i trong h? th?ng");
                            }
                        }
                    }

                    var departmentName = string.IsNullOrWhiteSpace(emp.Department) ? "Unassigned" : emp.Department.Trim();
                    int? deptId = ResolveDepartmentId(conn, tx, departmentName);
                    int? posId = ResolvePositionId(conn, tx, emp.Position, deptId);

                    // C?p nh?t thông tin nhân viên
                    using (var cmd = new SqlCommand(@"
UPDATE Employees SET 
    FullName = @FullName,
    Email = @Email,
    Phone = @Phone,
    PositionId = @PosId,
    HireDate = @HireDate,
    Status = @Status
WHERE EmployeeId = @Id", conn, tx))
                    {
                        cmd.Parameters.AddWithValue("@Id", emp.EmployeeId);
                        cmd.Parameters.AddWithValue("@FullName", (object)(emp.FullName ?? string.Empty));
                        cmd.Parameters.AddWithValue("@Email", (object)(emp.Email) ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Phone", (object)(emp.Phone) ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@PosId", (object)posId ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@HireDate", emp.HireDate == default(DateTime) ? DateTime.Today : emp.HireDate);
                        cmd.Parameters.AddWithValue("@Status", emp.Status ?? "Inactive");
                        cmd.ExecuteNonQuery();
                    }

                    // L?y thông tin h?p ??ng hi?n t?i ?? so sánh
                    decimal currentDealSalary = 0;
                    decimal currentCoef = 0;
                    using (var cmd = new SqlCommand(@"
SELECT TOP 1 DealSalary, SalaryCoefficient FROM EmploymentContract 
WHERE EmployeeId = @Id AND IsActive = 1 
ORDER BY EffectiveFrom DESC", conn, tx))
                    {
                        cmd.Parameters.AddWithValue("@Id", emp.EmployeeId);
                        using (var rd = cmd.ExecuteReader())
                        {
                            if (rd.Read())
                            {
                                currentDealSalary = rd.IsDBNull(0) ? 0 : rd.GetDecimal(0);
                                currentCoef = rd.IsDBNull(1) ? 0 : rd.GetDecimal(1);
                            }
                        }
                    }

                    // N?u l??ng thay ??i, t?o h?p ??ng m?i
                    if (currentDealSalary != dealSalary || currentCoef != salaryCoef)
                    {
                        // ?óng h?p ??ng c?
                        using (var cmd = new SqlCommand(@"
UPDATE EmploymentContract SET IsActive = 0, EffectiveTo = GETDATE() 
WHERE EmployeeId = @Id AND IsActive = 1", conn, tx))
                        {
                            cmd.Parameters.AddWithValue("@Id", emp.EmployeeId);
                            cmd.ExecuteNonQuery();
                        }

                        // T?o h?p ??ng m?i
                        using (var cmd = new SqlCommand(@"
INSERT INTO EmploymentContract (EmployeeId, DealSalary, SalaryCoefficient, EffectiveFrom, EffectiveTo, Currency, IsActive)
VALUES (@EmployeeId, @DealSalary, @SalaryCoefficient, GETDATE(), NULL, 'VND', 1);", conn, tx))
                        {
                            cmd.Parameters.AddWithValue("@EmployeeId", emp.EmployeeId);
                            cmd.Parameters.AddWithValue("@DealSalary", dealSalary);
                            cmd.Parameters.AddWithValue("@SalaryCoefficient", salaryCoef);
                            cmd.ExecuteNonQuery();
                        }
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

        /// <summary>
        /// Ki?m tra UserName ?ã t?n t?i ch?a
        /// </summary>
        public bool UsernameExists(string username)
        {
            using (var conn = new SqlConnection(_connStr))
            using (var cmd = new SqlCommand("SELECT COUNT(*) FROM Employees WHERE UserName = @UserName", conn))
            {
                cmd.Parameters.AddWithValue("@UserName", username);
                conn.Open();
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        private static bool IsIdentityColumn(SqlConnection conn, SqlTransaction tx, string tableName, string columnName)
        {
            using (var cmd = new SqlCommand($"SELECT COLUMNPROPERTY(object_id('{tableName}'), '{columnName}', 'IsIdentity')", conn, tx))
            {
                var val = cmd.ExecuteScalar();
                return val != null && val != DBNull.Value && Convert.ToInt32(val) == 1;
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
                    var isIdentity = IsIdentityColumn(conn, tx, "Departments", "DepartmentId");
                    if (isIdentity)
                    {
                        using (var insertCmd = new SqlCommand("INSERT INTO Departments (Name) OUTPUT inserted.DepartmentId VALUES (@Name)", conn, tx))
                        {
                            insertCmd.Parameters.AddWithValue("@Name", deptName);
                            return Convert.ToInt32(insertCmd.ExecuteScalar());
                        }
                    }
                    else
                    {
                        int newId;
                        using (var idCmd = new SqlCommand("SELECT ISNULL(MAX(DepartmentId),0) + 1 FROM Departments", conn, tx))
                        {
                            newId = Convert.ToInt32(idCmd.ExecuteScalar());
                        }
                        using (var insertCmd = new SqlCommand("INSERT INTO Departments (DepartmentId, Name) VALUES (@Id, @Name)", conn, tx))
                        {
                            insertCmd.Parameters.AddWithValue("@Id", newId);
                            insertCmd.Parameters.AddWithValue("@Name", deptName);
                            insertCmd.ExecuteNonQuery();
                        }
                        return newId;
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
                    var isIdentity = IsIdentityColumn(conn, tx, "Positions", "PositionId");
                    if (isIdentity)
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
                    else
                    {
                        int newId;
                        using (var idCmd = new SqlCommand("SELECT ISNULL(MAX(PositionId),0) + 1 FROM Positions", conn, tx))
                        {
                            newId = Convert.ToInt32(idCmd.ExecuteScalar());
                        }
                        using (var insertCmd = new SqlCommand(@"INSERT INTO Positions (PositionId, Name, DepartmentId)
VALUES (@Id, @Name, @DeptId)", conn, tx))
                        {
                            insertCmd.Parameters.AddWithValue("@Id", newId);
                            insertCmd.Parameters.AddWithValue("@Name", posName);
                            insertCmd.Parameters.AddWithValue("@DeptId", (object)deptId ?? DBNull.Value);
                            insertCmd.ExecuteNonQuery();
                        }
                        return newId;
                    }
                }
                return Convert.ToInt32(val);
            }
        }
    }
}
