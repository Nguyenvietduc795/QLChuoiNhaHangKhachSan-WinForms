using Guna.UI2.WinForms;
using System;
using System.Drawing;
using System.Windows.Forms;
using static QLChuoiNhaHangKhachSan.GUI.AddEmployeeForm;

namespace QLChuoiNhaHangKhachSan.GUI
{
    public partial class EmployeeForm : Form
    {
        public EmployeeForm()
        {
            InitializeComponent();
            this.Load += EmployeeForm_Load;
        }

        // Tắt viền/đổ bóng khi nhúng vào FormDashBoard
        
            public void EnableEmbedMode()
        {
            if (bldManageEmployeeForm != null)
            {
                bldManageEmployeeForm.Dispose(); // or set HasFormShadow/DockForm as needed
            }
        }
        

        private void EmployeeForm_Load(object sender, EventArgs e)
        {
            LoadEmployees();
            UpdateCardWidths();
        }

        private void btnAddEmployee_Click(object sender, EventArgs e)
        {
            using (var frm = new AddEmployeeForm())
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    LoadEmployees(); // refresh after saving
                }
            }
        }

        private void flpEmployees_SizeChanged(object sender, EventArgs e)
        {
            UpdateCardWidths();
        }

        private void UpdateCardWidths()
        {
            if (flpEmployees.Controls.Count == 0) return;

            int availableWidth = flpEmployees.ClientSize.Width - flpEmployees.Padding.Horizontal;
            if (availableWidth <= 0) return;

            int marginX = pnlCardEmployeeTemplate.Margin.Horizontal; // total horizontal margin of one card
            int minWidth = Math.Max(pnlCardEmployeeTemplate.MinimumSize.Width, 300);

            // Try to fit as many columns as possible while keeping width >= minWidth and minimizing leftover space
            int bestColumns = 1;
            int bestWidth = availableWidth;
            int maxColumns = Math.Max(1, (availableWidth + marginX) / (minWidth + marginX));
            for (int cols = 1; cols <= maxColumns; cols++)
            {
                int totalMargin = marginX * (cols);
                int widthPerCol = (availableWidth - totalMargin) / cols;
                if (widthPerCol >= minWidth && widthPerCol <= availableWidth)
                {
                    bestColumns = cols;
                    bestWidth = widthPerCol;
                }
            }

            // If even 1 column can’t reach minWidth, fall back to single column with minWidth cap
            if (bestColumns == 1 && bestWidth < minWidth)
            {
                bestWidth = Math.Min(minWidth, availableWidth);
            }

            foreach (Control c in flpEmployees.Controls)
            {
                c.Width = bestWidth;
                c.Height = pnlCardEmployeeTemplate.Size.Height;
            }

            flpEmployees.PerformLayout();
        }

        // Render employee cards from the hidden template panel
        private void LoadEmployees()
        {
            flpEmployees.Controls.Clear();

            var list = EmployeeData.Employees;
            if (list == null || list.Count == 0) return;

            foreach (var emp in list)
            {
                flpEmployees.Controls.Add(CreateEmployeeCard(emp));
            }

            UpdateCardWidths();
        }

        private Control CreateEmployeeCard(Employee emp)
        {
            // clone panel basics
            var card = new Guna2Panel
            {
                BackColor = pnlCardEmployeeTemplate.BackColor,
                FillColor = pnlCardEmployeeTemplate.FillColor,
                BorderColor = pnlCardEmployeeTemplate.BorderColor,
                BorderThickness = pnlCardEmployeeTemplate.BorderThickness,
                BorderRadius = pnlCardEmployeeTemplate.BorderRadius,
                Size = pnlCardEmployeeTemplate.Size,
                Margin = pnlCardEmployeeTemplate.Margin,
                ShadowDecoration =
                {
                    BorderRadius = pnlCardEmployeeTemplate.ShadowDecoration.BorderRadius,
                    Color = pnlCardEmployeeTemplate.ShadowDecoration.Color,
                    Depth = pnlCardEmployeeTemplate.ShadowDecoration.Depth,
                    Enabled = pnlCardEmployeeTemplate.ShadowDecoration.Enabled
                },
                MinimumSize = pnlCardEmployeeTemplate.MinimumSize,
                Visible = true
            };

            // avatar
            var avatar = new Guna2CirclePictureBox
            {
                FillColor = pnlEmployeeAvt.FillColor,
                Image = pnlEmployeeAvt.Image,
                ImageRotate = 0F,
                Location = pnlEmployeeAvt.Location,
                Size = pnlEmployeeAvt.Size,
                SizeMode = PictureBoxSizeMode.StretchImage,
                ShadowDecoration = { Mode = pnlEmployeeAvt.ShadowDecoration.Mode }
            };

            // name
            var lblName = new Guna2HtmlLabel
            {
                Text = emp.FullName,
                Font = lblEmployeeName.Font,
                ForeColor = lblEmployeeName.ForeColor,
                Location = lblEmployeeName.Location,
                AutoSize = true
            };

            // position
            var lblPos = new Guna2HtmlLabel
            {
                Text = emp.Position,
                Font = lblEmployeePos.Font,
                ForeColor = lblEmployeePos.ForeColor,
                Location = lblEmployeePos.Location,
                AutoSize = true
            };

            // email
            var lblMail = new Guna2HtmlLabel
            {
                Text = emp.Email,
                Font = lblEmployeeMail.Font,
                ForeColor = lblEmployeeMail.ForeColor,
                Location = lblEmployeeMail.Location,
                AutoSize = true
            };

            // department
            var lblDept = new Guna2HtmlLabel
            {
                Text = emp.Department,
                Font = lblEmployeeDeapartment.Font,
                ForeColor = lblEmployeeDeapartment.ForeColor,
                Location = lblEmployeeDeapartment.Location,
                AutoSize = true
            };

            // hire date
            var hireText = emp.HireDate != default(DateTime) ? emp.HireDate.ToString("MMMM dd, yyyy") : "";
            var lblHire = new Guna2HtmlLabel
            {
                Text = hireText,
                Font = lblEmployeeHireDate.Font,
                ForeColor = lblEmployeeHireDate.ForeColor,
                Location = lblEmployeeHireDate.Location,
                AutoSize = true
            };

            // salary
            var salaryText = emp.Salary > 0 ? emp.Salary.ToString("N0") + " ₫" : "0 ₫";
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
                Size = btnEmployeeSalary.Size,
                Location = btnEmployeeSalary.Location,
                Image = btnEmployeeSalary.Image,
                FillColor = btnEmployeeSalary.FillColor,
                DisabledState = btnEmployeeSalary.DisabledState,
                ImageSize = btnEmployeeSalary.ImageSize
            };

            // phone
            var phoneText = string.IsNullOrWhiteSpace(emp.Phone) ? "" : emp.Phone;
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

            // status
            string status = emp.Status ?? "Active";
            var btnStatus = new Guna2Button
            {
                Text = status,
                Location = btnEmployeeStatus.Location,
                Size = btnEmployeeStatus.Size,
                BorderRadius = btnEmployeeStatus.BorderRadius,
                FillColor = status == "Active" ? btnEmployeeStatus.FillColor : Color.LightGray,
                ForeColor = status == "Active" ? btnEmployeeStatus.ForeColor : Color.Black
            };

            // mail icon
            var btnMailIcon = new Guna2Button
            {
                Location = btnEmployeeMail.Location,
                Size = btnEmployeeMail.Size,
                FillColor = btnEmployeeMail.FillColor,
                Image = btnEmployeeMail.Image,
                ImageSize = btnEmployeeMail.ImageSize
            };

            // department icon
            var btnDeptIcon = new Guna2Button
            {
                Location = btnEmployeeDepartment.Location,
                Size = btnEmployeeDepartment.Size,
                FillColor = btnEmployeeDepartment.FillColor,
                Image = btnEmployeeDepartment.Image,
                ImageSize = btnEmployeeDepartment.ImageSize
            };

            // hire icon
            var btnHireIcon = new Guna2Button
            {
                Location = btnEmployeeHireDate.Location,
                Size = btnEmployeeHireDate.Size,
                FillColor = btnEmployeeHireDate.FillColor,
                Image = btnEmployeeHireDate.Image,
                ImageSize = btnEmployeeHireDate.ImageSize
            };

            // edit
            var btnEdit = new Guna2Button
            {
                Text = btnEmployeeEdit.Text,
                Location = btnEmployeeEdit.Location,
                Size = btnEmployeeEdit.Size,
                FillColor = btnEmployeeEdit.FillColor,
                ForeColor = btnEmployeeEdit.ForeColor,
                Font = btnEmployeeEdit.Font,
                Tag = emp
            };
            btnEdit.Click += (s, e) =>
            {
                var target = (AddEmployeeForm.Employee)((Control)s).Tag;
                using (var frm = new AddEmployeeForm(target))
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        LoadEmployees();
                    }
                }
            };

            // delete
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
                var target = (AddEmployeeForm.Employee)((Control)s).Tag;
                var result = MessageBox.Show("Bạn có chắc muốn xóa nhân viên này không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    EmployeeData.Employees.Remove(target);
                    LoadEmployees();
                    MessageBox.Show("Nhân viên đã được xóa.", "Xóa thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            };

            // assemble
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

            return card;
        }

        private void lblEmployee_SubTitle_Click(object sender, EventArgs e)
        {
        }

        private void btnEmployeeDelete_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Bạn có chắc muốn xóa nhân viên này không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                MessageBox.Show("Nhân viên đã được xóa.", "Xóa thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnEmployeeEdit_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Bạn có chắc muốn chỉnh sửa thông tin nhân viên này không?", "Xác nhận chỉnh sửa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        private void lblEmployeeName_Click(object sender, EventArgs e)
        {

        }
    }
}
