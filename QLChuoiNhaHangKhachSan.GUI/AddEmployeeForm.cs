using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using QLChuoiNhaHangKhachSan.DAL.Models;
using QLChuoiNhaHangKhachSan.DAL.Repositories;
using QLChuoiNhaHangKhachSan.BLL.Services;

namespace QLChuoiNhaHangKhachSan.GUI
{
    public partial class AddEmployeeForm : Form
    {
        // Đối tượng nhân viên đang được chỉnh sửa (null nếu đang thêm mới)
        private readonly Employee _editing;
        private readonly EmployeeService _service;

        // Cờ dùng để tắt tạm validation tên khi tự động chỉnh sửa text trong TextChanged
        private bool _suppressNameValidation; // tránh lặp TextChanged khi tự sửa nội dung

        public AddEmployeeForm() : this(null)
        {
        }

        // Edit mode ctor
        public AddEmployeeForm(Employee employeeToEdit)
        {
            InitializeComponent();

            var connStr = ConfigurationManager.ConnectionStrings["ConnStr"]?.ConnectionString;
            _service = new EmployeeService(connStr);

            // Gắn sự kiện kiểm tra ngay khi nhập tên
            txtEmployeeFullName.KeyPress += TxtEmployeeFullName_KeyPress;
            txtEmployeeFullName.TextChanged += TxtEmployeeFullName_TextChanged;
            // Gắn sự kiện chỉ cho phép nhập số ở trường lương và điện thoại
            txtDealSalary.KeyPress += NumericOnly_KeyPress;
            txtEmployeePhoneNumber.KeyPress += NumericOnly_KeyPress;

            // Lưu lại đối tượng đang chỉnh sửa nếu có, và điền sẵn dữ liệu
            _editing = employeeToEdit;
            if (_editing != null)
            {
                Prefill(_editing);
                cboEmployeeStatus.Enabled = true; // cho phép chỉnh sửa khi edit
            }
            else
            {
                // Thêm mới: luôn Inactive và không cho chọn khác
                cboEmployeeStatus.SelectedItem = "Inactive";
                cboEmployeeStatus.Enabled = false;
            }
        }

        // Danh sách cache tạm trong UI (giảm đổi code nhiều form); TODO: chuyển cache sang Service layer nếu cần
        public static class EmployeeData
        {
            public static List<Employee> Employees = new List<Employee>();
            public static event EventHandler EmployeesChanged;
            public static void NotifyChanged() => EmployeesChanged?.Invoke(null, EventArgs.Empty);
        }

        private void btnEmployeeSave_Click(object sender, EventArgs e)
        {
            // Lấy dữ liệu từ giao diện và bỏ khoảng trắng thừa
            string fullName = txtEmployeeFullName.Text.Trim();
            string email = txtEmployeeEmail.Text.Trim();
            string department = cboEmployeeDepartment.Text.Trim();
            string position = txtEmployeePosition.Text.Trim();
            // Nếu đang thêm mới, luôn dùng Inactive; nếu edit, lấy từ combo
            string status = _editing == null ? "Inactive" : cboEmployeeStatus.Text.Trim();
            string salaryRaw = txtDealSalary.Text.Trim(); // lương deal
            string coefRaw = txtSalaryCoefficient.Text.Trim(); // hệ số lương
            string phoneInput = txtEmployeePhoneNumber.Text.Trim();
            DateTime hireDate = dtEmployeeHireDate.Value;

            // Tách mã quốc gia + số (nếu người dùng nhập dạng "+84 0123456")
            string countryCode = null;
            string phone = phoneInput;
            var phoneMatch = Regex.Match(phoneInput, @"^(\+\d{1,4})\s*(\d+)$");
            if (phoneMatch.Success)
            {
                countryCode = phoneMatch.Groups[1].Value;
                phone = phoneMatch.Groups[2].Value; // giữ nguyên số 0 đầu
            }

            // Kiểm tra bắt buộc nhập đủ các trường
            if (string.IsNullOrWhiteSpace(fullName) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(department) ||
                string.IsNullOrWhiteSpace(position) ||
                string.IsNullOrWhiteSpace(salaryRaw) ||
                string.IsNullOrWhiteSpace(coefRaw) ||
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

            // Lương deal phải là số hợp lệ và không âm
            if (!decimal.TryParse(salaryRaw, out decimal dealSalary) || dealSalary < 0)
            {
                MessageBox.Show("Mức lương deal phải là số hợp lệ (>= 0).", "Lương không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDealSalary.Focus();
                return;
            }

            // Hệ số phải là số hợp lệ và >= 0
            if (!decimal.TryParse(coefRaw, out decimal salaryCoef) || salaryCoef < 0)
            {
                MessageBox.Show("Hệ số lương phải là số hợp lệ (>= 0).", "Hệ số không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSalaryCoefficient.Focus();
                return;
            }

            // Điện thoại: cho phép nhập "+84 0123" hoặc chỉ số; lưu lại số 0 đầu
            if (!Regex.IsMatch(phone, "^\\d+$"))
            {
                MessageBox.Show("Số điện thoại chỉ được nhập số.", "Số điện thoại không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmployeePhoneNumber.Focus();
                return;
            }

            // Tính lương theo công thức: hệ số * deal
            decimal computedSalary = dealSalary * salaryCoef;
            lblEmployeeSalary.Text = $"Lương: {computedSalary:N0} ₫";

            if (_editing != null)
            {
                // Cập nhật thông tin cho nhân viên đang chỉnh sửa
                _editing.FullName = fullName;
                _editing.Email = email;
                _editing.Department = department;
                _editing.Position = position;
                _editing.HireDate = hireDate;
                _editing.Status = status;
                _editing.Salary = computedSalary;
                _editing.SalaryCoefficient = salaryCoef;
                _editing.CountryCode = countryCode ?? _editing.CountryCode ?? "+84";
                _editing.Phone = phone;
            }
            else
            {
                // Tạo nhân viên mới và thêm vào danh sách (luôn Inactive)
                var emp = new Employee
                {
                    FullName = fullName,
                    Email = email,
                    Department = department,
                    Position = position,
                    HireDate = hireDate,
                    Status = "Inactive",
                    Salary = computedSalary,
                    SalaryCoefficient = salaryCoef,
                    CountryCode = countryCode ?? "+84",
                    Phone = phone
                };
                try { _service.Add(emp, dealSalary, salaryCoef); }
                catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
                {
                    MessageBox.Show("Email hoặc số điện thoại đã tồn tại.", "Trùng dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi lưu DB: {ex.Message}", "DB", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            // Bắn sự kiện thông báo danh sách thay đổi để màn hình khác có thể reload
            EmployeeData.NotifyChanged();

            // Đặt kết quả trả về cho form cha và đóng form
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        // hàm đổ dữ liệu nhân viên lên form khi edit
        private void Prefill(Employee emp)
        {
            
            txtEmployeeFullName.Text = emp.FullName;
            txtEmployeeEmail.Text = emp.Email;
            cboEmployeeDepartment.Text = emp.Department;
            txtEmployeePosition.Text = emp.Position;
            dtEmployeeHireDate.Value = emp.HireDate == default(DateTime) ? DateTime.Today : emp.HireDate;
            cboEmployeeStatus.Text = emp.Status ?? "Inactive"; // default Inactive khi thiếu
            txtDealSalary.Text = emp.Salary.ToString("0.##");
            txtSalaryCoefficient.Text = (emp.SalaryCoefficient > 0 ? emp.SalaryCoefficient : 1).ToString("0.##");
            var phoneDisplay = string.IsNullOrWhiteSpace(emp.CountryCode) ? emp.Phone : ($"{emp.CountryCode} {emp.Phone}").Trim();
            txtEmployeePhoneNumber.Text = phoneDisplay;

            // cho phép chọn trạng thái khi đang edit
            cboEmployeeStatus.Enabled = true;

            // Cập nhật tiêu đề form nếu có label
            if (lblAddEmployeeTitle != null)
            {
                lblAddEmployeeTitle.Text = $"Thông tin nhân viên";
            }
        }

        // KHI LOAD FORM THIET LAP TRANG THAI MAC DINH
        private void AddEmployeeForm_Load(object sender, EventArgs e)
        {
            // Mặc định trạng thái Inactive khi thêm mới
            if (_editing == null)
            {
                cboEmployeeStatus.SelectedItem = "Inactive";
                cboEmployeeStatus.Enabled = false;
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        // NGĂN NHẬP SỐ KHI GÕ TÊN
        private void TxtEmployeeFullName_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Nếu ký tự là số thì chặn không cho hiển thị
            if (char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // chặn ký tự số
            }
        }

        // NGĂN NHẬP SỐ KHI DÁN VÀO TEXTBOX TÊN
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
            bool isPhoneBox = sender == txtEmployeePhoneNumber;
            if (isPhoneBox && e.KeyChar == '+')
            {
                // Chỉ cho phép + ở đầu và chỉ một lần
                var box = txtEmployeePhoneNumber;
                if (box.SelectionStart == 0 && !box.Text.Contains("+"))
                {
                    return; // cho phép nhập + ở đầu
                }
            }

            if (isPhoneBox && char.IsWhiteSpace(e.KeyChar))
            {
                return; // cho phép khoảng trắng giữa mã quốc gia và số
            }

            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
