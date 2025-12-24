using System; // Kiểu dữ liệu cơ bản
using System.Drawing; // Làm việc với màu sắc
using System.Linq; // LINQ để tính tổng/trung bình
using System.Windows.Forms; // WinForms core
using static QLChuoiNhaHangKhachSan.GUI.AddEmployeeForm; // Truy cập Employee, EmployeeData trực tiếp

namespace QLChuoiNhaHangKhachSan.GUI
{
    public partial class SalaryManageForm : Form // Form quản lý lương
    {
        private const int HoverShadow = 12; // Độ sâu shadow khi hover
        private const int NormalShadow = 5; // (Không dùng hiện tại) shadow mặc định
        private readonly Color HoverFill = Color.FromArgb(245, 248, 255); // Màu nền khi hover
        private readonly Padding _designPadding; // Lưu padding ban đầu của form

        public SalaryManageForm()
        {
            InitializeComponent(); // Khởi tạo UI từ Designer
            _designPadding = this.Padding; // Nhớ padding gốc để khôi phục khi embed
            AttachHover(pnlTotalPayroll); // Gắn hiệu ứng hover cho panel tổng lương
            AttachHover(pnlTotalEmployees); // Gắn hiệu ứng hover cho panel tổng nhân viên
            AttachHover(pnlAvargeSalary); // Gắn hiệu ứng hover cho panel lương trung bình
            this.Load += SalaryManageForm_Load; // Đăng ký sự kiện Load
            EmployeeData.EmployeesChanged += EmployeeData_EmployeesChanged; // Nghe sự kiện thay đổi danh sách nhân viên
        }

        private void SalaryManageForm_Load(object sender, EventArgs e)
        {
            RefreshFromEmployees(); // Lấy dữ liệu nhân viên và hiển thị
        }

        private void EmployeeData_EmployeesChanged(object sender, EventArgs e)
        {
            RefreshFromEmployees(); // Khi danh sách nhân viên đổi, cập nhật lại UI lương
        }

        public void RefreshFromEmployees()
        {
            var list = EmployeeData.Employees ?? new System.Collections.Generic.List<Employee>(); // Lấy danh sách nhân viên, fallback list rỗng
            int count = list.Count; // Số lượng nhân viên
            decimal total = list.Sum(emp => emp?.Salary ?? 0m); // Tổng lương (nếu null -> 0)
            decimal average = count > 0 ? total / count : 0m; // Lương trung bình

            lblActiveEmployees.Text = count.ToString(); // Cập nhật label số nhân viên
            lblTotalMoney.Text = total.ToString("N0") + " ₫"; // Cập nhật tổng lương định dạng nghìn
            lblAverageMoney.Text = average.ToString("N0") + " ₫"; // Cập nhật lương trung bình

            dgvSalaryEmployees.Rows.Clear(); // Xóa bảng cũ
            foreach (var emp in list) // Lặp từng nhân viên để thêm hàng mới
            {
                string status = emp.Status ?? "Active"; // Trạng thái mặc định Active
                int rowIndex = dgvSalaryEmployees.Rows.Add(
                    emp.FullName, // Tên
                    emp.Department, // Phòng ban
                    "", // Chưa dùng (cột placeholder)
                    emp.Salary.ToString("N0") + " ₫", // Lương hiển thị
                    "Monthly", // Kỳ lương cố định
                    status); // Trạng thái

                var row = dgvSalaryEmployees.Rows[rowIndex]; // Lấy row vừa thêm
                var statusCell = row.Cells[colEmployeeStatus.Name] as DataGridViewButtonCell; // Ô trạng thái dạng nút
                if (statusCell != null)
                {
                    statusCell.Value = status; // Gán text trạng thái
                    bool isActive = string.Equals(status, "Active", StringComparison.OrdinalIgnoreCase); // Kiểm tra active
                    statusCell.Style.BackColor = isActive ? Color.FromArgb(209, 250, 229) : Color.FromArgb(252, 165, 165); // Màu nền theo trạng thái
                    statusCell.Style.ForeColor = isActive ? Color.FromArgb(6, 95, 70) : Color.FromArgb(127, 29, 29); // Màu chữ theo trạng thái
                    statusCell.FlatStyle = FlatStyle.Flat; // Nút phẳng
                }
            }
        }

        private void AttachHover(Guna.UI2.WinForms.Guna2Panel panel)
        {
            panel.Tag = new PanelSnapshot(panel.FillColor, panel.ShadowDecoration.Shadow, panel.ShadowDecoration.Depth); // Lưu snapshot màu và shadow
            panel.MouseEnter += Panel_MouseEnter; // Gắn sự kiện hover vào panel
            panel.MouseLeave += Panel_MouseLeave; // Gắn sự kiện rời chuột
            foreach (Control child in panel.Controls) // Gắn tiếp cho control con để khi hover con vẫn đổi panel
            {
                child.MouseEnter += (s, e) => Panel_MouseEnter(panel, e);
                child.MouseLeave += (s, e) => Panel_MouseLeave(panel, e);
            }
        }

        private void Panel_MouseEnter(object sender, EventArgs e)
        {
            var panel = sender as Guna.UI2.WinForms.Guna2Panel; // Lấy panel từ sender
            if (panel == null || panel.Tag == null) return; // Nếu không hợp lệ thì bỏ
            var snap = (PanelSnapshot)panel.Tag; // Lấy snapshot (không dùng trong enter nhưng giữ cấu trúc)
            panel.FillColor = HoverFill; // Đổi màu nền hover
            panel.ShadowDecoration.Depth = HoverShadow; // Đổi độ sâu shadow khi hover
            panel.ShadowDecoration.Shadow = new Padding(10); // Tăng khoảng shadow
        }

        private void Panel_MouseLeave(object sender, EventArgs e)
        {
            var panel = sender as Guna.UI2.WinForms.Guna2Panel; // Lấy panel từ sender
            if (panel == null || panel.Tag == null) return; // Không hợp lệ thì bỏ
            var snap = (PanelSnapshot)panel.Tag; // Lấy snapshot gốc
            panel.FillColor = snap.Fill; // Khôi phục màu nền gốc
            panel.ShadowDecoration.Depth = snap.ShadowDepth; // Khôi phục độ sâu shadow gốc
            panel.ShadowDecoration.Shadow = snap.ShadowPadding; // Khôi phục padding shadow gốc
        }

        private struct PanelSnapshot // Lưu trạng thái màu/shadow của panel
        {
            public Color Fill { get; } // Màu nền
            public Padding ShadowPadding { get; } // Padding của shadow
            public int ShadowDepth { get; } // Độ sâu shadow
            public PanelSnapshot(Color fill, Padding shadowPadding, int shadowDepth)
            {
                Fill = fill;
                ShadowPadding = shadowPadding;
                ShadowDepth = shadowDepth;
            }
        }

        // Tắt viền/đổ bóng khi nhúng vào FormDashBoard
        public void EnableEmbedMode()
        {
            if (bldSalaryManageForm != null) // Nếu BorderlessForm tồn tại
            {
                bldSalaryManageForm.Dispose(); // Hủy để bỏ hiệu ứng borderless/shadow
            }
            this.Padding = _designPadding; // Giữ padding gốc để phần header không bị mất
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            EmployeeData.EmployeesChanged -= EmployeeData_EmployeesChanged; // Hủy đăng ký sự kiện khi form đóng
            base.OnFormClosed(e); // Gọi base
        }
    }
}
