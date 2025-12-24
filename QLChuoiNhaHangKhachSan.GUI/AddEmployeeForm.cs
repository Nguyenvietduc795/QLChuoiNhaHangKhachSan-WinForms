using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace QLChuoiNhaHangKhachSan.GUI
{
    public partial class AddEmployeeForm : Form
    {
        // Đối tượng nhân viên đang được chỉnh sửa (null nếu đang thêm mới)
        private readonly Employee _editing;

        // Cờ dùng để tắt tạm validation tên khi tự động chỉnh sửa text trong TextChanged
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
            // Gắn sự kiện chỉ cho phép nhập số ở trường lương và điện thoại
            txtEmployeeSalary.KeyPress += NumericOnly_KeyPress;
            txtEmployeePhoneNumber.KeyPress += NumericOnly_KeyPress;

            // Lưu lại đối tượng đang chỉnh sửa nếu có, và điền sẵn dữ liệu
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
            public static event EventHandler EmployeesChanged;
            public static void NotifyChanged()
            {
                EmployeesChanged?.Invoke(null, EventArgs.Empty);
            }
        }

        private void btnEmployeeSave_Click(object sender, EventArgs e)
        {
            // Lấy dữ liệu từ giao diện và bỏ khoảng trắng thừa
            string fullName = txtEmployeeFullName.Text.Trim();
            string email = txtEmployeeEmail.Text.Trim();
            string department = cboEmployeeDepartment.Text.Trim();
            string position = txtEmployeePosition.Text.Trim();
            string status = cboEmployeeStatus.Text.Trim();
            string salaryRaw = txtEmployeeSalary.Text.Trim();
            string phone = txtEmployeePhoneNumber.Text.Trim();
            DateTime hireDate = dtEmployeeHireDate.Value;

            // Kiểm tra bắt buộc nhập đủ các trường
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

            // Email phải đúng định dạng Gmail
            if (!Regex.IsMatch(email, @"^[^@\s]+@gmail\.com$", RegexOptions.IgnoreCase))
            {
                MessageBox.Show("Email phải đúng định dạng Gmail (ví dụ: ten@gmail.com).", "Email không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmployeeEmail.Focus();
                return;
            }

            // Tên không được chứa chữ số
            if (Regex.IsMatch(fullName, @"\d"))
            {
                MessageBox.Show("Tên không được chứa số.", "Tên không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmployeeFullName.Focus();
                return;
            }

            // Lương phải là số hợp lệ và không âm
            if (!decimal.TryParse(salaryRaw, out decimal salary) || salary < 0)
            {
                MessageBox.Show("Mức lương phải là số hợp lệ (>= 0).", "Lương không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmployeeSalary.Focus();
                return;
            }

            // Điện thoại chỉ cho phép ký tự số
            if (!Regex.IsMatch(phone, "^\\d+$"))
            {
                MessageBox.Show("Số điện thoại chỉ được nhập số.", "Số điện thoại không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmployeePhoneNumber.Focus();
                return;
            }

            if (_editing != null)
            {
                // Cập nhật thông tin cho nhân viên đang chỉnh sửa
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
                // Tạo nhân viên mới và thêm vào danh sách
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

            // Bắn sự kiện thông báo danh sách thay đổi để màn hình khác có thể reload
            EmployeeData.NotifyChanged();

            // Đặt kết quả trả về cho form cha và đóng form
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void Prefill(Employee emp)
        {
            // Đổ dữ liệu của nhân viên đang chỉnh sửa lên form
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
            // Nếu ký tự là số thì chặn không cho hiển thị
            if (char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // chặn ký tự số
            }
        }

        // Nếu dán/nhập có số, yêu cầu sửa và tự loại bỏ số
        private void TxtEmployeeFullName_TextChanged(object sender, EventArgs e)
        {
            // Khi đang tự sửa text thì bỏ qua để tránh vòng lặp sự kiện
            if (_suppressNameValidation) return;

            var text = txtEmployeeFullName.Text;
            if (Regex.IsMatch(text, @"\d"))
            {
                MessageBox.Show("Tên không được chứa số. Vui lòng nhập lại.", "Tên không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _suppressNameValidation = true;
                // Loại bỏ tất cả chữ số đã nhập/dán
                txtEmployeeFullName.Text = Regex.Replace(text, @"\d", "");
                // Đặt lại vị trí con trỏ cuối chuỗi sau khi sửa
                txtEmployeeFullName.SelectionStart = txtEmployeeFullName.Text.Length;
                _suppressNameValidation = false;
            }
        }

        // Ngăn nhập chữ cho trường số (lương, điện thoại)
        private void NumericOnly_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Nếu không phải phím điều khiển và không phải số thì chặn
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
