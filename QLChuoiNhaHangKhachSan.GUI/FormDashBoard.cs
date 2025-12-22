using System;
using System.Linq;
using System.Windows.Forms;
using System.Configuration;

namespace QLChuoiNhaHangKhachSan.GUI
{
    public partial class FormDashBoard : Form
    {
        // Form con đang hiển thị
        private Form activeChildForm = null;
        // Panel chứa nội dung form con (EmployeeForm, ...)
        private Panel pnlContentHost;

        public FormDashBoard()
        {
            InitializeComponent();
            CreateContentHost(); // Tạo panel chứa form con, giữ nguyên sidebar

            // Đảm bảo sự kiện click được gắn (phòng khi designer không gắn)
            this.btnListStaff.Click += btnListStaff_Click;
            this.btnSalaryManage.Click += btnSalaryManage_Click;
            this.btnHome.Click += btnHome_Click;
        }

        // Khởi tạo panel host cho form con
        private void CreateContentHost()
        {
            pnlContentHost = new Panel
            {
                Name = "pnlContentHost",
                Dock = DockStyle.Fill,
                BackColor = System.Drawing.Color.White,
                Visible = false
            };
            // Đặt host lên trên các control khác (trừ sidebar dock left)
            this.Controls.Add(pnlContentHost);
            this.Controls.SetChildIndex(pnlContentHost, 0);
            pnlContentHost.BringToFront();
        }

        // Hiển thị một form con bên trong host
        private void ShowChild(Form child)
        {
            // Đóng form con cũ nếu còn
            if (activeChildForm != null && !activeChildForm.IsDisposed)
            {
                activeChildForm.Close();
                activeChildForm.Dispose();
            }

            activeChildForm = child;
            child.TopLevel = false;
            child.FormBorderStyle = FormBorderStyle.None;
            child.Dock = DockStyle.Fill;

            // Tắt đổ bóng viền cho form nhúng
            if (child is SalaryManageForm smf) smf.EnableEmbedMode();
            if (child is EmployeeForm ef) ef.EnableEmbedMode();

            pnlContentHost.Controls.Clear();
            pnlContentHost.Controls.Add(child);
            pnlContentHost.Visible = true;
            pnlContentHost.BringToFront();
            child.Show();
        }

        // Ẩn form con, quay lại nội dung dashboard
        private void HideChild()
        {
            if (activeChildForm != null && !activeChildForm.IsDisposed)
            {
                activeChildForm.Close();
                activeChildForm = null;
            }
            pnlContentHost.Visible = false;
        }

        // Khi load dashboard: thu gọn submenu, đọc cấu hình (nếu có)
        private void Form1_Load(object sender, EventArgs e)
        {
            CloseAllSubMenus();

            try
            {
                string connStr = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
                // MessageBox.Show(connStr);
            }
            catch { }
        }

        // Thu gọn toàn bộ menu con
        private void CloseAllSubMenus()
        {
            pnlStaff_group.Height = 50;
            pnlCustomer_group.Height = 50;
            pnlHotel_group.Height = 50;
            pnlRestaurent_group.Height = 50;
            pnlInvetory_group.Height = 50;
            pnlPayment_group.Height = 50;
            pnlReport_group.Height = 50;
            pnlLogout_group.Height = 50;
        }

        private void lblUserRole_Click(object sender, EventArgs e)
        {
        }

        private void guna2Chip4_Click(object sender, EventArgs e)
        {
        }

        // Đổi trạng thái nút được chọn, tắt highlight các nút khác
        private void SetActiveButton(object sender)
        {
            foreach (Control container in flpSidebar.Controls)
            {
                if (container is Panel || container is Guna.UI2.WinForms.Guna2Panel)
                {
                    foreach (Control c in container.Controls)
                    {
                        if (c is Guna.UI2.WinForms.Guna2Button btn) btn.Checked = false;
                    }
                }
                else if (container is Guna.UI2.WinForms.Guna2Button btn)
                {
                    btn.Checked = false;
                }
            }

            if (sender is Guna.UI2.WinForms.Guna2Button clickedBtn)
            {
                clickedBtn.Checked = true;
            }
        }

        // Nút Trang chủ: thu gọn submenu và ẩn form con
        private void btnHome_Click(object sender, EventArgs e)
        {
            SetActiveButton(sender);
            CloseAllSubMenus();
            HideChild();
        }

        /************************************************************************************************************/
        // Nhóm Nhân viên: mở/đóng submenu
        bool isStaffExpanded = false; // Biến trạng thái mở rộng của nhóm Nhân viên
        private void btnStaff_Click(object sender, EventArgs e)
        {
            SetActiveButton(sender);

            if (pnlStaff_group.Height == 50)
            {
                CloseAllSubMenus();
                pnlStaff_group.Height = 130; // Mở rộng
            }
            else
            {
                pnlStaff_group.Height = 50; // Thu gọn
            }
        }

        // Danh sách nhân viên: mở EmployeeForm trong khu vực nội dung
        private void btnListStaff_Click(object sender, EventArgs e)
        {
            SetActiveButton(sender);
            ShowChild(new EmployeeForm());
        }

        // Bảng lương (chưa gắn form cụ thể)
        private void btnSalaryManage_Click(object sender, EventArgs e)
        {
            SetActiveButton(sender);
            ShowChild(new SalaryManageForm());
        }

        /************************************************************************************************************/
        // Nhóm Khách hàng: mở/đóng submenu
        private void btnCustomers_Click(object sender, EventArgs e)
        {
            SetActiveButton(sender);

            if (pnlCustomer_group.Height == 50)
            {
                CloseAllSubMenus();
                pnlCustomer_group.Height = 130;
            }
            else
            {
                pnlCustomer_group.Height = 50;
            }
        }

        private void btnCustomerList_Click(object sender, EventArgs e)
        {
            SetActiveButton(sender);
            // TODO: ShowChild(new CustomerListForm());
        }

        private void btnLoyaltyProgram_Click(object sender, EventArgs e)
        {
            SetActiveButton(sender);
            // TODO: ShowChild(new LoyaltyProgramForm());
        }

        private void pnlSidebar_Paint(object sender, PaintEventArgs e)
        {
        }

        /************************************************************************************************************/
        // Nhóm Khách sạn
        private void btnHotels_Click(object sender, EventArgs e)
        {
            SetActiveButton(sender);

            if (pnlHotel_group.Height == 50)
            {
                CloseAllSubMenus();
                pnlHotel_group.Height = 130;
            }
            else
            {
                pnlHotel_group.Height = 50;
            }
        }

        private void btnRoomList_Click(object sender, EventArgs e)
        {
            SetActiveButton(sender);
        }
        private void btnBooking_Click(object sender, EventArgs e)
        {
            SetActiveButton(sender);
        }
        private void btnBookingRoom_Click(object sender, EventArgs e)
        {
            SetActiveButton(sender);
        }

        /************************************************************************************************************/
        // Nhóm Nhà hàng
        private void btnRestaurants_Click(object sender, EventArgs e)
        {
            SetActiveButton(sender);

            if (pnlRestaurent_group.Height == 50)
            {
                CloseAllSubMenus();
                pnlRestaurent_group.Height = 130;
            }
            else
            {
                pnlRestaurent_group.Height = 50;
            }
        }

        private void btnTableList_Click(object sender, EventArgs e)
        {
            SetActiveButton(sender);
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            SetActiveButton(sender);
        }

        /************************************************************************************************************/
        // Nhóm Kho
        private void btnInventory_Click(object sender, EventArgs e)
        {
            SetActiveButton(sender);

            if (pnlInvetory_group.Height == 50)
            {
                CloseAllSubMenus();
                pnlInvetory_group.Height = 130;
            }
            else
            {
                pnlInvetory_group.Height = 50;
            }

        }

        private void btnIngredient_Click(object sender, EventArgs e)
        {
            SetActiveButton(sender);
        }

        private void btnEquipment_Click(object sender, EventArgs e)
        {
            SetActiveButton(sender);
        }

        /************************************************************************************************************/
        // Nhóm Thanh toán
        private void btnPayments_Click(object sender, EventArgs e)
        {
            SetActiveButton(sender);

            if (pnlPayment_group.Height == 50)
            {
                CloseAllSubMenus();
                pnlPayment_group.Height = 130;
            }
            else
            {
                pnlPayment_group.Height = 50;
            }
        }

        private void btnInvoice_Click(object sender, EventArgs e)
        {
            SetActiveButton(sender);
        }

        private void btnTransaction_Click(object sender, EventArgs e)
        {
            SetActiveButton(sender);
        }

        /************************************************************************************************************/
        // Nhóm Báo cáo
        private void btnReports_Click(object sender, EventArgs e)
        {
            SetActiveButton(sender);

            if (pnlReport_group.Height == 50)
            {
                CloseAllSubMenus();
                pnlReport_group.Height = 130;
            }
            else
            {
                pnlReport_group.Height = 50;
            }
        }

        private void btnFinancial_Click(object sender, EventArgs e)
        {
            SetActiveButton(sender);
        }

        private void btnTotalCustomer_Click(object sender, EventArgs e)
        {
            SetActiveButton(sender);
        }

        // Đăng xuất
        private void btnLogout_Click(object sender, EventArgs e)
        {
            SetActiveButton(sender);
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?", "Xác nhận đăng xuất", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        // Handler trống
        private void btnDotBlue_Click(object sender, EventArgs e)
        {
        }

        private void guna2HtmlLabel7_Click(object sender, EventArgs e)
        {
        }
    }

}
