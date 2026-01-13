using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QLChuoiNhaHangKhachSan.BLL.Services;
using QLChuoiNhaHangKhachSan.DAL.Models;
using static QLChuoiNhaHangKhachSan.GUI.AddEmployeeForm;

namespace QLChuoiNhaHangKhachSan.GUI
{
    public partial class EmployeeForm : Form
    {
        private readonly EmployeeService _service;
        private List<Employee> _filteredEmployees = new List<Employee>();
        private string _statusFilter = "All";
        private readonly AutoCompleteStringCollection _nameSuggestSource = new AutoCompleteStringCollection();
        private const int DesiredColumns = 3;
        private const int CardMinWidth = 320;
        private const int CardHeight = 340;

        public EmployeeForm()
        {
            InitializeComponent();
            this.Load += EmployeeForm_Load;

            txtEmployeeSearch.KeyDown += (s, e) => 
            {
                if (e.KeyCode == Keys.Enter)
                {
                    ApplySearchAndRender();
                    e.Handled = true; // Chặn tiếng "bíp" của windows
                    e.SuppressKeyPress = true;
                }
            };
            
            btnAll.Click += (s, e) => SetStatusFilter("All");
            btnActive.Click += (s, e) => SetStatusFilter("Active");
            btnInactive.Click += (s, e) => SetStatusFilter("Inactive");

            txtEmployeeSearch.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtEmployeeSearch.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtEmployeeSearch.AutoCompleteCustomSource = _nameSuggestSource;

            var connStr = ConfigurationManager.ConnectionStrings["ConnStr"]?.ConnectionString;
            _service = new EmployeeService(connStr);
        }

        public void EnableEmbedMode()
        {
            if (bldManageEmployeeForm != null)
            {
                bldManageEmployeeForm.Dispose();
            }
        }

        private void EmployeeForm_Load(object sender, EventArgs e)
        {
            try
            {
                RefreshEmployeesFromDb();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không tải được dữ liệu: {ex.Message}", "DB", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            ApplySearchAndRender();
            UpdateCardWidths();
        }

        private void RefreshEmployeesFromDb()
        {
            EmployeeData.Employees = _service.GetAll();
            UpdateNameSuggestions(EmployeeData.Employees);
        }

        private void btnAddEmployee_Click(object sender, EventArgs e)
        {
            using (var frm = new AddEmployeeForm())
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    RefreshEmployeesFromDb();
                    ApplySearchAndRender();
                }
            }
        }

        private void flpEmployees_SizeChanged(object sender, EventArgs e)
        {
            UpdateCardWidths();
        }

        private int ComputeCardWidth()
        {
            int availableWidth = flpEmployees.ClientSize.Width - flpEmployees.Padding.Horizontal - SystemInformation.VerticalScrollBarWidth;
            if (availableWidth <= 0) return Math.Max(pnlCardEmployeeTemplate.MinimumSize.Width, CardMinWidth);

            int marginX = pnlCardEmployeeTemplate.Margin.Horizontal;
            int columns = Math.Min(DesiredColumns, Math.Max(1, (availableWidth + marginX) / (CardMinWidth + marginX)));
            int width = (availableWidth - (marginX * columns)) / columns;
            return Math.Max(CardMinWidth, width);
        }

        private void UpdateCardWidths()
        {
            if (flpEmployees.Controls.Count == 0) return;

            int targetWidth = ComputeCardWidth();
            int targetHeight = Math.Max(CardHeight, pnlCardEmployeeTemplate.MinimumSize.Height);

            foreach (Control c in flpEmployees.Controls)
            {
                // Skip the hidden template panel
                if (!c.Visible) continue;
                
                ApplyCardLayout(c, targetWidth, targetHeight);
            }

            flpEmployees.PerformLayout();
        }

        private void LoadEmployees(List<Employee> source)
        {
            flpEmployees.Controls.Clear();

            var list = source;
            if (list == null || list.Count == 0) return;

            int targetWidth = ComputeCardWidth();
            int targetHeight = Math.Max(CardHeight, pnlCardEmployeeTemplate.MinimumSize.Height);

            foreach (var emp in list)
            {
                flpEmployees.Controls.Add(CreateEmployeeCard(emp, targetWidth, targetHeight));
            }
        }

        private void ApplySearchAndRender()
        {
            var term = (txtEmployeeSearch.Text ?? string.Empty).Trim();
            _filteredEmployees = _service.Filter(term, _statusFilter);

            var names = _service.SuggestNames(term);
            UpdateNameSuggestions(names);

            LoadEmployees(_filteredEmployees);
        }

        private void SetStatusFilter(string status)
        {
            _statusFilter = status;
            HighlightStatusButtons();
            ApplySearchAndRender();
        }

        private void HighlightStatusButtons()
        {
            var selectedFill = Color.Black;
            var selectedFore = Color.White;
            var normalFill = Color.White;
            var normalFore = Color.Black;

            void SetBtn(Guna2Button btn, bool selected)
            {
                btn.FillColor = selected ? selectedFill : normalFill;
                btn.ForeColor = selected ? selectedFore : normalFore;
            }

            SetBtn(btnAll, _statusFilter == "All");
            SetBtn(btnActive, _statusFilter == "Active");
            SetBtn(btnInactive, _statusFilter == "Inactive");
        }

        private void UpdateNameSuggestions(IEnumerable<string> names)
        {
            _nameSuggestSource.Clear();
            if (names == null) return;
            _nameSuggestSource.AddRange(names.ToArray());
        }

        private void UpdateNameSuggestions(IEnumerable<Employee> employees)
        {
            var names = employees?
                .Where(e => e != null && !string.IsNullOrWhiteSpace(e.FullName))
                .Select(e => e.FullName)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToArray() ?? Array.Empty<string>();

            UpdateNameSuggestions(names);
        }

        private Control CreateEmployeeCard(Employee emp, int width, int height)
        {
            string status = string.IsNullOrWhiteSpace(emp.Status) ? "Inactive" : emp.Status.Trim();
            bool isActive = string.Equals(status, "Active", StringComparison.OrdinalIgnoreCase);
            string nextStatus = isActive ? "Inactive" : "Active";

            var card = new Guna2Panel
            {
                BackColor = pnlCardEmployeeTemplate.BackColor,
                FillColor = pnlCardEmployeeTemplate.FillColor,
                BorderColor = pnlCardEmployeeTemplate.BorderColor,
                BorderThickness = pnlCardEmployeeTemplate.BorderThickness,
                BorderRadius = pnlCardEmployeeTemplate.BorderRadius,
                Margin = pnlCardEmployeeTemplate.Margin,
                // --- SỬA 1: TẠM TẮT ĐỔ BÓNG (SHADOW) ---
                // ShadowDecoration của Guna gán động rất dễ gây tràn bộ nhớ
                // ShadowDecoration =
                // {
                //     BorderRadius = pnlCardEmployeeTemplate.ShadowDecoration.BorderRadius,
                //     Color = pnlCardEmployeeTemplate.ShadowDecoration.Color,
                //     Depth = pnlCardEmployeeTemplate.ShadowDecoration.Depth,
                //     Enabled = pnlCardEmployeeTemplate.ShadowDecoration.Enabled
                // },
                // ----------------------------------------
                MinimumSize = new Size(CardMinWidth, CardHeight),
                Name = "card"
            };

            var avatar = new Guna2CirclePictureBox
            {
                Name = "avatar",
                FillColor = pnlEmployeeAvt.FillColor,
                // --- SỬA 2: QUAN TRỌNG NHẤT - KHÔNG GÁN ẢNH TỪ TEMPLATE ---
                // Dòng dưới đây là nguyên nhân chính gây sập khi tìm kiếm
                // Image = pnlEmployeeAvt.Image, 
                // ----------------------------------------------------------
                // Thay vào đó, hãy để ảnh rỗng hoặc gán null an toàn
                Image = null, 
                ImageRotate = 0F,
                SizeMode = PictureBoxSizeMode.StretchImage,
                // Tắt luôn shadow của avatar
                // ShadowDecoration = { Mode = pnlEmployeeAvt.ShadowDecoration.Mode } 
            };

            var lblName = new Guna2HtmlLabel
            {
                Name = "lblName",
                Text = emp.FullName,
                Font = lblEmployeeName.Font,
                ForeColor = lblEmployeeName.ForeColor,
                AutoSize = true
            };

            var lblPos = new Guna2HtmlLabel
            {
                Name = "lblPos",
                Text = emp.Position,
                Font = lblEmployeePos.Font,
                ForeColor = lblEmployeePos.ForeColor,
                AutoSize = true
            };

            var lblMail = new Guna2HtmlLabel
            {
                Name = "lblMail",
                Text = emp.Email,
                Font = lblEmployeeMail.Font,
                ForeColor = lblEmployeeMail.ForeColor,
                AutoSize = true
            };

            var lblDept = new Guna2HtmlLabel
            {
                Name = "lblDept",
                Text = emp.Department,
                Font = lblEmployeeDeapartment.Font,
                ForeColor = lblEmployeeDeapartment.ForeColor,
                AutoSize = true
            };

            var lblHire = new Guna2HtmlLabel
            {
                Name = "lblHire",
                Text = emp.HireDate != default(DateTime) ? emp.HireDate.ToString("MMMM dd, yyyy") : string.Empty,
                Font = lblEmployeeHireDate.Font,
                ForeColor = lblEmployeeHireDate.ForeColor,
                AutoSize = true
            };

            var lblSalary = new Guna2HtmlLabel
            {
                Name = "lblSalary",
                Text = emp.Salary > 0 ? emp.Salary.ToString("N0") + " ₫" : "0 ₫",
                Font = lblEmployeeSalary.Font,
                ForeColor = lblEmployeeSalary.ForeColor,
                AutoSize = true
            };
            var btnSalary = new Guna2Button
            {
                Name = "btnSalary",
                Size = btnEmployeeSalary.Size,
                Image = btnEmployeeSalary.Image,
                FillColor = btnEmployeeSalary.FillColor,
                DisabledState = btnEmployeeSalary.DisabledState,
                ImageSize = btnEmployeeSalary.ImageSize
            };

            var lblPhone = new Guna2HtmlLabel
            {
                Name = "lblPhone",
                Text = string.IsNullOrWhiteSpace(emp.Phone) ? string.Empty : emp.Phone,
                Font = lblEmployeePhoneNumber.Font,
                ForeColor = lblEmployeePhoneNumber.ForeColor,
                AutoSize = true
            };
            var btnPhone = new Guna2Button
            {
                Name = "btnPhone",
                Size = btnEmployeePhoneNumber.Size,
                Image = btnEmployeePhoneNumber.Image,
                FillColor = btnEmployeePhoneNumber.FillColor,
                DisabledState = btnEmployeePhoneNumber.DisabledState,
                ImageSize = btnEmployeePhoneNumber.ImageSize
            };

            var btnStatus = new Guna2Button
            {
                Name = "btnStatus",
                BorderRadius = btnEmployeeStatus.BorderRadius,
                FillColor = isActive ? Color.FromArgb(209, 250, 229) : Color.Silver,
                ForeColor = isActive ? Color.FromArgb(6, 95, 70) : Color.Black,
                Font = btnEmployeeStatus.Font,
                Text = status,
                Size = new Size(82, 26)
            };

            var btnActivate = new Guna2Button
            {
                Name = "btnAction",
                Text = nextStatus == "Active" ? "Active" : "Inactive",
                FillColor = nextStatus == "Active" ? Color.FromArgb(209, 250, 229) : Color.FromArgb(255, 224, 192),
                ForeColor = nextStatus == "Active" ? Color.FromArgb(6, 95, 70) : Color.FromArgb(192, 64, 0),
                Font = btnEmployeeActive.Font,
                BorderRadius = btnEmployeeActive.BorderRadius,
                Size = new Size(128, 36)
            };
            btnActivate.Click += (s, e) =>
            {
                var currentStatus = emp.Status ?? "Inactive";
                var targetStatus = string.Equals(currentStatus, "Active", StringComparison.OrdinalIgnoreCase) ? "Inactive" : "Active";
                
                // Show confirmation when deactivating employee with salary > 0
                if (string.Equals(targetStatus, "Inactive", StringComparison.OrdinalIgnoreCase) && emp.Salary > 0)
                {
                    var result = MessageBox.Show(
                        $"Chuyển {emp.FullName} sang Inactive sẽ đặt lương về 0 VND.\n\nBạn có muốn tiếp tục?",
                        "Xác nhận",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);
                    
                    if (result != DialogResult.Yes) return;
                    
                    try
                    {
                        _service.DeactivateEmployee(emp.EmployeeId);
                        RefreshEmployeesFromDb();
                        EmployeeData.NotifyChanged();
                        ApplySearchAndRender();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    return;
                }

                // Standard status toggle
                try
                {
                    _service.UpdateStatus(emp.EmployeeId, targetStatus, emp.Salary);
                    RefreshEmployeesFromDb();
                    EmployeeData.NotifyChanged();
                    ApplySearchAndRender();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Không cập nhật được trạng thái: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            var btnMailIcon = new Guna2Button
            {
                Name = "btnMail",
                Size = btnEmployeeMail.Size,
                FillColor = btnEmployeeMail.FillColor,
                Image = btnEmployeeMail.Image,
                ImageSize = btnEmployeeMail.ImageSize
            };

            var btnDeptIcon = new Guna2Button
            {
                Name = "btnDept",
                Size = btnEmployeeDepartment.Size,
                FillColor = btnEmployeeDepartment.FillColor,
                Image = btnEmployeeDepartment.Image,
                ImageSize = btnEmployeeDepartment.ImageSize
            };

            var btnHireIcon = new Guna2Button
            {
                Name = "btnHire",
                Size = btnEmployeeHireDate.Size,
                FillColor = btnEmployeeHireDate.FillColor,
                Image = btnEmployeeHireDate.Image,
                ImageSize = btnEmployeeHireDate.ImageSize
            };

            var btnEdit = new Guna2Button
            {
                Name = "btnEdit",
                Text = btnEmployeeEdit.Text,
                Size = new Size(120, 36),
                FillColor = btnEmployeeEdit.FillColor,
                ForeColor = btnEmployeeEdit.ForeColor,
                Font = btnEmployeeEdit.Font,
                BorderRadius = btnEmployeeEdit.BorderRadius,
                Tag = emp
            };
            btnEdit.Click += (s, e) =>
            {
                var target = (Employee)((Control)s).Tag;
                using (var frm = new AddEmployeeForm(target))
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        RefreshEmployeesFromDb();
                        EmployeeData.NotifyChanged();
                        ApplySearchAndRender();
                    }
                }
            };

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
            card.Controls.Add(btnActivate);

            ApplyCardLayout(card, width, height);
            return card;
        }

        private void ApplyCardLayout(Control card, int width, int height)
        {
            card.Width = width;
            card.Height = height;

            int padding = 16;
            int lineSpacing = 30;
            int iconSize = 28;

            var avatar = card.Controls["avatar"] as Guna2CirclePictureBox;
            var lblName = card.Controls["lblName"] as Guna2HtmlLabel;
            var lblPos = card.Controls["lblPos"] as Guna2HtmlLabel;
            var btnStatus = card.Controls["btnStatus"] as Guna2Button;
            var btnMailIcon = card.Controls["btnMail"] as Guna2Button;
            var lblMail = card.Controls["lblMail"] as Guna2HtmlLabel;
            var btnDeptIcon = card.Controls["btnDept"] as Guna2Button;
            var lblDept = card.Controls["lblDept"] as Guna2HtmlLabel;
            var btnHireIcon = card.Controls["btnHire"] as Guna2Button;
            var lblHire = card.Controls["lblHire"] as Guna2HtmlLabel;
            var btnSalary = card.Controls["btnSalary"] as Guna2Button;
            var lblSalary = card.Controls["lblSalary"] as Guna2HtmlLabel;
            var btnPhone = card.Controls["btnPhone"] as Guna2Button;
            var lblPhone = card.Controls["lblPhone"] as Guna2HtmlLabel;
            var btnEdit = card.Controls["btnEdit"] as Guna2Button;
            var btnAction = card.Controls["btnAction"] as Guna2Button;

            // Return early if any required controls are missing
            if (avatar == null || lblName == null || lblPos == null || btnStatus == null ||
                btnMailIcon == null || lblMail == null || btnDeptIcon == null || lblDept == null ||
                btnHireIcon == null || lblHire == null || btnSalary == null || lblSalary == null ||
                btnPhone == null || lblPhone == null || btnEdit == null || btnAction == null)
            {
                return;
            }

            avatar.Size = new Size(56, 56);
            avatar.Location = new Point(padding, padding);

            lblName.Location = new Point(avatar.Right + 12, padding);
            lblPos.Location = new Point(avatar.Right + 12, lblName.Bottom + 2);

            btnStatus.Size = new Size(82, 26);
            btnStatus.Location = new Point(width - padding - btnStatus.Width, padding + 4);

            int y = Math.Max(avatar.Bottom, btnStatus.Bottom) + 12;

            void PlaceRow(Guna2Button icon, Guna2HtmlLabel label)
            {
                icon.Size = new Size(iconSize, iconSize);
                icon.Location = new Point(padding, y);
                label.Location = new Point(icon.Right + 10, y + (iconSize - label.Height) / 2);
                y += lineSpacing;
            }

            PlaceRow(btnMailIcon, lblMail);
            PlaceRow(btnDeptIcon, lblDept);
            PlaceRow(btnHireIcon, lblHire);
            PlaceRow(btnSalary, lblSalary);
            PlaceRow(btnPhone, lblPhone);

            int buttonY = height - padding - btnEdit.Height;
            btnEdit.Location = new Point(padding, buttonY);
            btnAction.Location = new Point(width - padding - btnAction.Width, buttonY);
        }

        private void lblEmployee_SubTitle_Click(object sender, EventArgs e)
        {
            // No-op handler (wired from designer)
        }
    }
}
