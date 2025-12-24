using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace QLChuoiNhaHangKhachSan.GUI
{
    public partial class AddEmployeeForm : Form
    {
        private readonly Employee _editing;
        private bool _suppressNameValidation; // tránh lặp TextChanged khi tự sửa nội dung

        public AddEmployeeForm() : this(null)
        {
        }

        // Edit mode ctor
        public AddEmployeeForm(Employee employeeToEdit)
        {
            InitializeComponent();

            // Gắn sự kiện kiểm tra ngay khi nhập tên
            txtEmployeeFullName.KeyPress += TxtEmployeeFullName_KeyPress;
            txtEmployeeFullName.TextChanged += TxtEmployeeFullName_TextChanged;
            txtEmployeeSalary.KeyPress += NumericOnly_KeyPress;
            txtEmployeePhoneNumber.KeyPress += NumericOnly_KeyPress;

            _editing = employeeToEdit;
            if (_editing != null)
            {
                Prefill(_editing);
            }
        }

        // Định nghĩa lớp Employee để lưu trữ thông tin nhân viên
        public class Employee
        {
            public string FullName { get; set; }
            public string Email { get; set; }
            public string Department { get; set; }
            public string Position { get; set; }
            public DateTime HireDate { get; set; }
            public string Status { get; set; }
            public decimal Salary { get; set; } // Mức lương
            public string Phone { get; set; }
        }

        // Danh sách để lưu trữ nhân viên
        public static class EmployeeData
        {
            public static List<Employee> Employees = new List<Employee>();
        }

        private void btnEmployeeSave_Click(object sender, EventArgs e)
        {
            // Validation
            string fullName = txtEmployeeFullName.Text.Trim();
            string email = txtEmployeeEmail.Text.Trim();
            string department = cboEmployeeDepartment.Text.Trim();
            string position = txtEmployeePosition.Text.Trim();
            string status = cboEmployeeStatus.Text.Trim();
            string salaryRaw = txtEmployeeSalary.Text.Trim();
            string phone = txtEmployeePhoneNumber.Text.Trim();
            DateTime hireDate = dtEmployeeHireDate.Value;

            if (string.IsNullOrWhiteSpace(fullName) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(department) ||
                string.IsNullOrWhiteSpace(position) ||
                string.IsNullOrWhiteSpace(status) ||
                string.IsNullOrWhiteSpace(salaryRaw) ||
                string.IsNullOrWhiteSpace(phone))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (Regex.IsMatch(fullName, @"\d"))
            {
                MessageBox.Show("Tên không được chứa số.", "Tên không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmployeeFullName.Focus();
                return;
            }

            if (!decimal.TryParse(salaryRaw, out decimal salary) || salary < 0)
            {
                MessageBox.Show("Mức lương phải là số hợp lệ (>= 0).", "Lương không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmployeeSalary.Focus();
                return;
            }

            if (!Regex.IsMatch(phone, "^\\d+$"))
            {
                MessageBox.Show("Số điện thoại chỉ được nhập số.", "Số điện thoại không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmployeePhoneNumber.Focus();
                return;
            }

            if (_editing != null)
            {
                // Update existing
                _editing.FullName = fullName;
                _editing.Email = email;
                _editing.Department = department;
                _editing.Position = position;
                _editing.HireDate = hireDate;
                _editing.Status = status;
                _editing.Salary = salary;
                _editing.Phone = phone;
            }
            else
            {
                // Add new
                var emp = new Employee
                {
                    FullName = fullName,
                    Email = email,
                    Department = department,
                    Position = position,
                    HireDate = hireDate,
                    Status = status,
                    Salary = salary,
                    Phone = phone
                };
                EmployeeData.Employees.Add(emp);
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void Prefill(Employee emp)
        {
            txtEmployeeFullName.Text = emp.FullName;
            txtEmployeeEmail.Text = emp.Email;
            cboEmployeeDepartment.Text = emp.Department;
            txtEmployeePosition.Text = emp.Position;
            dtEmployeeHireDate.Value = emp.HireDate == default(DateTime) ? DateTime.Today : emp.HireDate;
            cboEmployeeStatus.Text = emp.Status ?? "Active";
            txtEmployeeSalary.Text = emp.Salary.ToString("0.##");
            txtEmployeePhoneNumber.Text = emp.Phone;

            // Update form title/label to indicate edit mode
            if (lblAddEmployeeTitle != null)
            {
                lblAddEmployeeTitle.Text = $"Thông tin nhân viên";
            }
        }

        private void AddEmployeeForm_Load(object sender, EventArgs e)
        {
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        // Ngăn nhập ký tự số ngay khi gõ
        private void TxtEmployeeFullName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // chặn ký tự số
            }
        }

        // Nếu dán/nhập có số, yêu cầu sửa và tự loại bỏ số
        private void TxtEmployeeFullName_TextChanged(object sender, EventArgs e)
        {
            if (_suppressNameValidation) return;

            var text = txtEmployeeFullName.Text;
            if (Regex.IsMatch(text, @"\d"))
            {
                MessageBox.Show("Tên không được chứa số. Vui lòng nhập lại.", "Tên không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _suppressNameValidation = true;
                txtEmployeeFullName.Text = Regex.Replace(text, @"\d", "");
                txtEmployeeFullName.SelectionStart = txtEmployeeFullName.Text.Length;
                _suppressNameValidation = false;
            }
        }

        // Ngăn nhập chữ cho trường số (lương, điện thoại)
        private void NumericOnly_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
