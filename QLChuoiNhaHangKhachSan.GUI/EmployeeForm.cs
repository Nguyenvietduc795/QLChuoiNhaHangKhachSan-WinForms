using Guna.UI2.WinForms; // Thư viện UI Guna2
using System; // Cung cấp kiểu cơ bản
using System.Drawing; // Làm việc với màu sắc/kích thước
using System.Windows.Forms; // WinForms core
using static QLChuoiNhaHangKhachSan.GUI.AddEmployeeForm; // Dùng trực tiếp Employee, EmployeeData

namespace QLChuoiNhaHangKhachSan.GUI
{
    public partial class EmployeeForm : Form // Form hiển thị/ quản lý nhân viên
    {
        public EmployeeForm()
        {
            InitializeComponent(); // Khởi tạo UI từ Designer
            this.Load += EmployeeForm_Load; // Đăng ký sự kiện Load của form
        }

        // Tắt viền/đổ bóng khi nhúng vào FormDashBoard
        public void EnableEmbedMode()
        {
            if (bldManageEmployeeForm != null) // Nếu BorderlessForm tồn tại
            {
                bldManageEmployeeForm.Dispose(); // Giải phóng để bỏ hiệu ứng đổ bóng/viền
            }
        }

        private void EmployeeForm_Load(object sender, EventArgs e)
        {
            LoadEmployees();    // Nạp và render thẻ nhân viên
            UpdateCardWidths(); // Căn chỉnh kích thước thẻ theo chiều rộng panel
        }

        // thêm nhân viên mới
        private void btnAddEmployee_Click(object sender, EventArgs e)
        {
            using (var frm = new AddEmployeeForm()) // Mở form thêm mới
            {
                if (frm.ShowDialog() == DialogResult.OK) // Nếu lưu thành công
                {
                    LoadEmployees(); // Refresh danh sách
                }
            }
        }

        // Cập nhật kích thước thẻ khi panel thay đổi kích thước
        private void flpEmployees_SizeChanged(object sender, EventArgs e)
        {
            UpdateCardWidths(); // Khi panel đổi kích thước, cập nhật chiều rộng thẻ
        }

        // Cập nhật chiều rộng thẻ nhân viên dựa trên chiều rộng panel
        private void UpdateCardWidths()
        {
            if (flpEmployees.Controls.Count == 0) return; // Không có thẻ thì bỏ qua

            int availableWidth = flpEmployees.ClientSize.Width - flpEmployees.Padding.Horizontal; // Chiều rộng còn lại
            if (availableWidth <= 0) return; // Không đủ rộng thì dừng

            int marginX = pnlCardEmployeeTemplate.Margin.Horizontal; // Tổng margin ngang của card
            int minWidth = Math.Max(pnlCardEmployeeTemplate.MinimumSize.Width, 300); // Đặt minWidth mặc định >=300

            int bestColumns = 1; // Số cột tối ưu mặc định
            int bestWidth = availableWidth; // Chiều rộng tối ưu mặc định
            int maxColumns = Math.Max(1, (availableWidth + marginX) / (minWidth + marginX)); // Số cột tối đa có thể thử
            for (int cols = 1; cols <= maxColumns; cols++) // Duyệt số cột
            {
                int totalMargin = marginX * (cols); // Tổng margin cho số cột hiện tại
                int widthPerCol = (availableWidth - totalMargin) / cols; // Chiều rộng mỗi cột
                if (widthPerCol >= minWidth && widthPerCol <= availableWidth) // Đủ rộng và hợp lệ
                {
                    bestColumns = cols; // Chọn số cột này
                    bestWidth = widthPerCol; // Cập nhật chiều rộng tốt nhất
                }
            }

            if (bestColumns == 1 && bestWidth < minWidth) // Nếu chỉ 1 cột mà vẫn nhỏ hơn minWidth
            {
                bestWidth = Math.Min(minWidth, availableWidth); // Cố định về minWidth hoặc giới hạn khả dụng
            }

            foreach (Control c in flpEmployees.Controls) // Áp dụng cho từng card
            {
                c.Width = bestWidth; // Gán chiều rộng tối ưu
                c.Height = pnlCardEmployeeTemplate.Size.Height; // Chiều cao theo template
            }

            flpEmployees.PerformLayout(); // Yêu cầu bố trí lại
        }

        // Render employee cards từ panel template ẩn
        private void LoadEmployees()
        {
            flpEmployees.Controls.Clear(); // Xóa thẻ cũ

            var list = EmployeeData.Employees; // Lấy danh sách nhân viên
            if (list == null || list.Count == 0) return; // Rỗng thì dừng

            foreach (var emp in list) // Lặp từng nhân viên
            {
                flpEmployees.Controls.Add(CreateEmployeeCard(emp)); // Tạo thẻ và thêm vào panel
            }

            UpdateCardWidths(); // Căn lại kích thước sau khi thêm
        }

        private Control CreateEmployeeCard(Employee emp)
        {
            // Tạo panel card dựa trên template
            var card = new Guna2Panel
            {
                BackColor = pnlCardEmployeeTemplate.BackColor, // Nền
                FillColor = pnlCardEmployeeTemplate.FillColor, // Màu fill
                BorderColor = pnlCardEmployeeTemplate.BorderColor, // Màu viền
                BorderThickness = pnlCardEmployeeTemplate.BorderThickness, // Độ dày viền
                BorderRadius = pnlCardEmployeeTemplate.BorderRadius, // Bo góc
                Size = pnlCardEmployeeTemplate.Size, // Kích thước
                Margin = pnlCardEmployeeTemplate.Margin, // Margin
                ShadowDecoration =
                {
                    BorderRadius = pnlCardEmployeeTemplate.ShadowDecoration.BorderRadius, // Bo góc shadow
                    Color = pnlCardEmployeeTemplate.ShadowDecoration.Color, // Màu shadow
                    Depth = pnlCardEmployeeTemplate.ShadowDecoration.Depth, // Độ sâu shadow
                    Enabled = pnlCardEmployeeTemplate.ShadowDecoration.Enabled // Bật/tắt shadow
                },
                MinimumSize = pnlCardEmployeeTemplate.MinimumSize, // Kích thước tối thiểu
                Visible = true // Hiển thị
            };

            // Ảnh đại diện (avatar)
            var avatar = new Guna2CirclePictureBox
            {
                FillColor = pnlEmployeeAvt.FillColor, // Màu nền avatar
                Image = pnlEmployeeAvt.Image, // Ảnh từ template
                ImageRotate = 0F, // Không xoay ảnh
                Location = pnlEmployeeAvt.Location, // Vị trí
                Size = pnlEmployeeAvt.Size, // Kích thước
                SizeMode = PictureBoxSizeMode.StretchImage, // Co giãn vừa khung
                ShadowDecoration = { Mode = pnlEmployeeAvt.ShadowDecoration.Mode } // Kiểu shadow
            };

            // Label tên
            var lblName = new Guna2HtmlLabel
            {
                Text = emp.FullName, // Nội dung tên
                Font = lblEmployeeName.Font, // Font theo template
                ForeColor = lblEmployeeName.ForeColor, // Màu chữ
                Location = lblEmployeeName.Location, // Vị trí
                AutoSize = true // Tự co giãn theo text
            };

            // Label chức vụ
            var lblPos = new Guna2HtmlLabel
            {
                Text = emp.Position,
                Font = lblEmployeePos.Font,
                ForeColor = lblEmployeePos.ForeColor,
                Location = lblEmployeePos.Location,
                AutoSize = true
            };

            // Label email
            var lblMail = new Guna2HtmlLabel
            {
                Text = emp.Email,
                Font = lblEmployeeMail.Font,
                ForeColor = lblEmployeeMail.ForeColor,
                Location = lblEmployeeMail.Location,
                AutoSize = true
            };

            // Label phòng ban
            var lblDept = new Guna2HtmlLabel
            {
                Text = emp.Department,
                Font = lblEmployeeDeapartment.Font,
                ForeColor = lblEmployeeDeapartment.ForeColor,
                Location = lblEmployeeDeapartment.Location,
                AutoSize = true
            };

            // Label ngày vào làm
            var hireText = emp.HireDate != default(DateTime) ? emp.HireDate.ToString("MMMM dd, yyyy") : ""; // Chuỗi ngày
            var lblHire = new Guna2HtmlLabel
            {
                Text = hireText,
                Font = lblEmployeeHireDate.Font,
                ForeColor = lblEmployeeHireDate.ForeColor,
                Location = lblEmployeeHireDate.Location,
                AutoSize = true
            };

            // Lương: định dạng tiền tệ đơn giản
            var salaryText = emp.Salary > 0 ? emp.Salary.ToString("N0") + " ₫" : "0 ₫"; // Chuỗi lương
            var lblSalary = new Guna2HtmlLabel
            {
                Text = salaryText,
                Font = lblEmployeeSalary.Font,
                ForeColor = lblEmployeeSalary.ForeColor,
                Location = lblEmployeeSalary.Location,
                AutoSize = true
            };
            var btnSalary = new Guna2Button
            {
                Size = btnEmployeeSalary.Size, // Kích thước icon lương
                Location = btnEmployeeSalary.Location, // Vị trí
                Image = btnEmployeeSalary.Image, // Icon
                FillColor = btnEmployeeSalary.FillColor, // Màu nền
                DisabledState = btnEmployeeSalary.DisabledState, // Trạng thái disable
                ImageSize = btnEmployeeSalary.ImageSize // Kích thước icon
            };

            // Điện thoại
            var phoneText = string.IsNullOrWhiteSpace(emp.Phone) ? "" : emp.Phone; // Nếu rỗng thì hiển thị trống
            var lblPhone = new Guna2HtmlLabel
            {
                Text = phoneText,
                Font = lblEmployeePhoneNumber.Font,
                ForeColor = lblEmployeePhoneNumber.ForeColor,
                Location = lblEmployeePhoneNumber.Location,
                AutoSize = true
            };
            var btnPhone = new Guna2Button
            {
                Size = btnEmployeePhoneNumber.Size,
                Location = btnEmployeePhoneNumber.Location,
                Image = btnEmployeePhoneNumber.Image,
                FillColor = btnEmployeePhoneNumber.FillColor,
                DisabledState = btnEmployeePhoneNumber.DisabledState,
                ImageSize = btnEmployeePhoneNumber.ImageSize
            };

            // Trạng thái: đổi màu nếu không Active
            string status = emp.Status ?? "Active"; // Mặc định Active nếu null
            var btnStatus = new Guna2Button
            {
                Text = status,
                Location = btnEmployeeStatus.Location,
                Size = btnEmployeeStatus.Size,
                BorderRadius = btnEmployeeStatus.BorderRadius,
                FillColor = status == "Active" ? btnEmployeeStatus.FillColor : Color.LightGray,
                ForeColor = status == "Active" ? btnEmployeeStatus.ForeColor : Color.Black
            };

            // Icon mail
            var btnMailIcon = new Guna2Button
            {
                Location = btnEmployeeMail.Location,
                Size = btnEmployeeMail.Size,
                FillColor = btnEmployeeMail.FillColor,
                Image = btnEmployeeMail.Image,
                ImageSize = btnEmployeeMail.ImageSize
            };

            // Icon phòng ban
            var btnDeptIcon = new Guna2Button
            {
                Location = btnEmployeeDepartment.Location,
                Size = btnEmployeeDepartment.Size,
                FillColor = btnEmployeeDepartment.FillColor,
                Image = btnEmployeeDepartment.Image,
                ImageSize = btnEmployeeDepartment.ImageSize
            };

            // Icon ngày vào làm
            var btnHireIcon = new Guna2Button
            {
                Location = btnEmployeeHireDate.Location,
                Size = btnEmployeeHireDate.Size,
                FillColor = btnEmployeeHireDate.FillColor,
                Image = btnEmployeeHireDate.Image,
                ImageSize = btnEmployeeHireDate.ImageSize
            };

            // Nút sửa: mở form edit và refresh khi lưu
            var btnEdit = new Guna2Button
            {
                Text = btnEmployeeEdit.Text,
                Location = btnEmployeeEdit.Location,
                Size = btnEmployeeEdit.Size,
                FillColor = btnEmployeeEdit.FillColor,
                ForeColor = btnEmployeeEdit.ForeColor,
                Font = btnEmployeeEdit.Font,
                Tag = emp // Lưu đối tượng nhân viên để dùng khi click
            };
            btnEdit.Click += (s, e) =>
            {
                var target = (AddEmployeeForm.Employee)((Control)s).Tag; // Lấy nhân viên từ Tag
                using (var frm = new AddEmployeeForm(target)) // Mở form ở chế độ chỉnh sửa
                {
                    if (frm.ShowDialog() == DialogResult.OK) // Nếu lưu
                    {
                        LoadEmployees(); // Refresh danh sách
                    }
                }
            };

            // Nút xóa: hỏi xác nhận, xóa, bắn sự kiện, refresh
            var btnDelete = new Guna2Button
            {
                Text = btnEmployeeDelete.Text,
                Location = btnEmployeeDelete.Location,
                Size = btnEmployeeDelete.Size,
                FillColor = btnEmployeeDelete.FillColor,
                ForeColor = btnEmployeeDelete.ForeColor,
                Font = btnEmployeeDelete.Font,
                Tag = emp
            };
            btnDelete.Click += (s, e) =>
            {
                var target = (AddEmployeeForm.Employee)((Control)s).Tag; // Nhân viên cần xóa
                var result = MessageBox.Show("Bạn có chắc muốn xóa nhân viên này không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    EmployeeData.Employees.Remove(target); // Xóa khỏi danh sách
                    EmployeeData.NotifyChanged(); // Thông báo thay đổi
                    LoadEmployees(); // Refresh danh sách
                    MessageBox.Show("Nhân viên đã được xóa.", "Xóa thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            };

            // Thêm tất cả control con vào card
            card.Controls.Add(avatar);
            card.Controls.Add(lblName);
            card.Controls.Add(lblPos);
            card.Controls.Add(lblMail);
            card.Controls.Add(lblDept);
            card.Controls.Add(lblHire);
            card.Controls.Add(btnSalary);
            card.Controls.Add(lblSalary);
            card.Controls.Add(btnPhone);
            card.Controls.Add(lblPhone);
            card.Controls.Add(btnStatus);
            card.Controls.Add(btnMailIcon);
            card.Controls.Add(btnDeptIcon);
            card.Controls.Add(btnHireIcon);
            card.Controls.Add(btnEdit);
            card.Controls.Add(btnDelete);

            return card; // Trả card hoàn chỉnh
        }

        private void lblEmployee_SubTitle_Click(object sender, EventArgs e)
        {
            
        }

        private void btnEmployeeDelete_Click(object sender, EventArgs e)
        {
            // Handler mẫu cho button template (không dùng khi render thẻ động)
            var result = MessageBox.Show("Bạn có chắc muốn xóa nhân viên này không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                MessageBox.Show("Nhân viên đã được xóa.", "Xóa thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnEmployeeEdit_Click(object sender, EventArgs e)
        {
            // Handler mẫu cho button template (không dùng khi render thẻ động)
            MessageBox.Show("Bạn có chắc muốn chỉnh sửa thông tin nhân viên này không?", "Xác nhận chỉnh sửa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        private void lblEmployeeName_Click(object sender, EventArgs e)
        {
            // Sự kiện click tên (chưa sử dụng)
        }
    }
}
