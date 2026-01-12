using System; // Kiểu dữ liệu cơ bản
using System.Linq; // Dùng LINQ nếu cần
using System.Windows.Forms; // Thư viện WinForms
using System.Configuration; // Đọc cấu hình App.config
using System.Collections.Generic; // Lưu snapshot hover
using Guna.UI2.WinForms; // Control Guna2

namespace QLChuoiNhaHangKhachSan.GUI
{
    public partial class FormDashBoard : Form // Form dashboard chính
    {
        private Form activeChildForm = null; // Form con hiện tại đang hiển thị
        private Panel pnlContentHost; // Panel chứa form con (giữ sidebar nguyên)

        // Lưu kích thước/ vị trí gốc để phóng to thu nhỏ khi hover
        private readonly Dictionary<Guna2Button, HoverSnapshot> _hoverButtonSnapshots = new Dictionary<Guna2Button, HoverSnapshot>();
        private const int HoverGrowPixels = 8; // Mỗi chiều tăng thêm khi hover
        private const int HoverShadowGrow = 4; // Mỗi cạnh shadow tăng thêm khi hover
        private const int HoverShadowDepthBoost = 2; // Độ sâu shadow tăng khi hover

        public FormDashBoard()
        {
            InitializeComponent(); // Khởi tạo UI từ Designer
            CreateContentHost(); // Tạo panel host cho form con

            // Đảm bảo gắn sự kiện click cho các nút chính (phòng trường hợp designer chưa gắn)
            this.btnListStaff.Click += btnListStaff_Click;
            this.btnSalaryManage.Click += btnSalaryManage_Click;
            this.btnHome.Click += btnHome_Click;

            // Gắn hiệu ứng hover phóng to nhẹ cho các card thống kê
            AttachZoomHover(btnCardValue);
            AttachZoomHover(btnTotalCustomers);
            AttachZoomHover(btnRoomBooking);
            AttachZoomHover(btnRestaurentBooking);
        }

        // Khởi tạo panel host đặt form con vào (nội dung thay đổi, sidebar giữ nguyên)
        private void CreateContentHost()
        {
            pnlContentHost = new Panel
            {
                Name = "pnlContentHost", // Tên control
                Dock = DockStyle.Fill, // Lấp đầy vùng trống (trừ sidebar dock left)
                BackColor = System.Drawing.Color.White, // Nền trắng
                Visible = false // Ban đầu ẩn
            };
            this.Controls.Add(pnlContentHost); // Thêm vào form
            this.Controls.SetChildIndex(pnlContentHost, 0); // Đặt vị trí phía trên các control khác
            pnlContentHost.BringToFront(); // Đưa lên trên cùng
        }

        // Hiển thị một form con bên trong panel host
        private void ShowChild(Form child)
        {
            // Nếu đã có form con, đóng và giải phóng
            if (activeChildForm != null && !activeChildForm.IsDisposed)
            {
                activeChildForm.Close();
                activeChildForm.Dispose();
            }

            activeChildForm = child; // Lưu form mới
            child.TopLevel = false; // Không phải top-level (để nhúng)
            child.FormBorderStyle = FormBorderStyle.None; // Bỏ viền
            child.Dock = DockStyle.Fill; // Lấp đầy host

            // Tắt đổ bóng viền cho form nhúng (nếu form có BorderlessForm)
            if (child is SalaryManageForm smf) smf.EnableEmbedMode();
            if (child is EmployeeForm ef) ef.EnableEmbedMode();

            pnlContentHost.Controls.Clear(); // Xóa control cũ trong host
            pnlContentHost.Controls.Add(child); // Thêm form con mới
            pnlContentHost.Visible = true; // Hiển thị host
            pnlContentHost.BringToFront(); // Đưa host lên trên
            child.Show(); // Hiển thị form con
        }

        // Ẩn form con, quay về dashboard gốc
        private void HideChild()
        {
            if (activeChildForm != null && !activeChildForm.IsDisposed)
            {
                activeChildForm.Close(); // Đóng form con
                activeChildForm = null; // Xóa tham chiếu
            }
            pnlContentHost.Visible = false; // Ẩn host
        }

        // Khi dashboard load: thu gọn submenu, đọc cấu hình nếu có
        private void Form1_Load(object sender, EventArgs e)
        {
            CloseAllSubMenus(); // Thu gọn tất cả submenu

            try
            {
                string _connectionString = ConfigurationManager.ConnectionStrings["MyConn"]?.ConnectionString
            ?? @"Data Source=.\SQLEXPRESS;Initial Catalog=QuanLyChuoiNhaHangKhachSan;Integrated Security=True"; // Lấy chuỗi kết nối (nếu cần)
                // MessageBox.Show(connStr); // Debug
            }
            catch { } // Bỏ qua nếu lỗi cấu hình
        }

        // Thu gọn toàn bộ các menu con về 50px (ẩn)
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

        private void lblUserRole_Click(object sender, EventArgs e) { }
        private void guna2Chip4_Click(object sender, EventArgs e) { }

        // Đổi trạng thái nút được chọn, tắt highlight các nút khác trong sidebar
        private void SetActiveButton(object sender)
        {
            // Duyệt toàn bộ control trong flow layout (sidebar)
            foreach (Control container in flpSidebar.Controls)
            {
                // Nếu container là Panel/Guna2Panel thì duyệt các button con
                if (container is Panel || container is Guna.UI2.WinForms.Guna2Panel)
                {
                    foreach (Control c in container.Controls)
                    {
                        if (c is Guna.UI2.WinForms.Guna2Button btn) btn.Checked = false; // Bỏ chọn
                    }
                }
                // Nếu container trực tiếp là button
                else if (container is Guna.UI2.WinForms.Guna2Button btn)
                {
                    btn.Checked = false; // Bỏ chọn
                }
            }

            // Đánh dấu nút vừa click là checked
            if (sender is Guna.UI2.WinForms.Guna2Button clickedBtn)
            {
                clickedBtn.Checked = true;
            }
        }

        // Nút Trang chủ: thu gọn submenu và ẩn form con
        private void btnHome_Click(object sender, EventArgs e)
        {
            SetActiveButton(sender); // Đánh dấu nút
            CloseAllSubMenus(); // Thu gọn menu
            HideChild(); // Ẩn form con (trở về dashboard)
        }

        /************************************************************************************************************/
        // Nhóm Nhân viên
        bool isStaffExpanded = false; // Biến trạng thái (hiện chưa dùng)
        private void btnStaff_Click(object sender, EventArgs e)
        {
            SetActiveButton(sender);

            if (pnlStaff_group.Height == 50) // Nếu đang thu gọn
            {
                CloseAllSubMenus();
                pnlStaff_group.Height = 130; // Mở rộng
            }
            else
            {
                pnlStaff_group.Height = 50; // Thu gọn
            }
        }

        // Danh sách nhân viên: mở EmployeeForm
        private void btnListStaff_Click(object sender, EventArgs e)
        {
            SetActiveButton(sender);
            ShowChild(new EmployeeForm()); // Nhúng form danh sách nhân viên
        }

        // Bảng lương: mở SalaryManageForm
        private void btnSalaryManage_Click(object sender, EventArgs e)
        {
            SetActiveButton(sender);
            ShowChild(new SalaryManageForm()); // Nhúng form bảng lương
        }

        /************************************************************************************************************/
        // Nhóm Khách hàng
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
            ShowChild(new CustomersList());
        }

        private void btnLoyaltyProgram_Click(object sender, EventArgs e)
        {
            SetActiveButton(sender);
            ShowChild(new PromotionsCustomers());
        }

        private void pnlSidebar_Paint(object sender, PaintEventArgs e) { }

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
            ShowChild(new ListRoom());//Nhúng form quản lý phòng
        }
        private void btnBooking_Click(object sender, EventArgs e)
        {
            SetActiveButton(sender);
        }
        private void btnBookingRoom_Click(object sender, EventArgs e)
        {
            SetActiveButton(sender);
            ShowChild(new BooKing_Form());//Nhúng form đặt phòng
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
            ShowChild(new frmRestaurant());//Nhúng form nha hang
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            SetActiveButton(sender);
            ShowChild(new frmMenu());//Nhúng form menu
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
            ShowChild(new FormInventory());//Nhúng form nguyên liệu
        }

        private void btnEquipment_Click(object sender, EventArgs e)
        {
            SetActiveButton(sender);
            ShowChild(new FormInventory2());//Nhúng form thiết bị
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
            ShowChild(new FormInvoiceManagement());//Nhúng form hóa đơn
        }

        private void btnTransaction_Click(object sender, EventArgs e)
        {
            SetActiveButton(sender);
            ShowChild(new FormPayments());//Nhúng form lịch sử giao dịch
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

        // Đăng xuất: hỏi xác nhận rồi đóng form
        private void btnLogout_Click(object sender, EventArgs e)
        {
            SetActiveButton(sender);
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?", "Xác nhận đăng xuất", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Close(); // Đóng dashboard
            }
        }

        // Handler trống
        private void btnDotBlue_Click(object sender, EventArgs e) { }
        private void guna2HtmlLabel7_Click(object sender, EventArgs e) { }

        // Gắn sự kiện hover phóng to nhẹ cho button card
        private void AttachZoomHover(Guna2Button btn)
        {
            if (btn == null || _hoverButtonSnapshots.ContainsKey(btn)) return; // Bỏ nếu null/đã gắn
            // Lưu trạng thái ban đầu: kích thước, vị trí, shadow
            _hoverButtonSnapshots[btn] = new HoverSnapshot(btn.Size, btn.Location, btn.ShadowDecoration.Shadow, btn.ShadowDecoration.Depth, btn.ShadowDecoration.Enabled);
            // Đăng ký sự kiện hover vào/ra
            btn.MouseEnter += HoverButton_MouseEnter;
            btn.MouseLeave += HoverButton_MouseLeave;
        }

        private void HoverButton_MouseEnter(object sender, EventArgs e)
        {
            var btn = sender as Guna2Button; // Button được hover
            if (btn == null) return;
            if (!_hoverButtonSnapshots.TryGetValue(btn, out var snap)) return; // Không có snapshot thì thôi

            // Tăng kích thước nhẹ và dịch tâm để giữ nguyên cảm giác căn giữa
            btn.Size = new System.Drawing.Size(snap.Size.Width + HoverGrowPixels, snap.Size.Height + HoverGrowPixels);
            btn.Location = new System.Drawing.Point(snap.Location.X - HoverGrowPixels / 2, snap.Location.Y - HoverGrowPixels / 2);

            // Làm đậm shadow hơn khi hover
            btn.ShadowDecoration.Enabled = true;
            btn.ShadowDecoration.Shadow = new Padding(
                snap.ShadowPadding.Left + HoverShadowGrow,
                snap.ShadowPadding.Top + HoverShadowGrow,
                snap.ShadowPadding.Right + HoverShadowGrow,
                snap.ShadowPadding.Bottom + HoverShadowGrow);
            btn.ShadowDecoration.Depth = snap.ShadowDepth + HoverShadowDepthBoost;
        }

        private void HoverButton_MouseLeave(object sender, EventArgs e)
        {
            var btn = sender as Guna2Button; // Button được rời chuột
            if (btn == null) return;
            if (!_hoverButtonSnapshots.TryGetValue(btn, out var snap)) return; // Không có snapshot thì thôi

            // Khôi phục kích thước, vị trí và shadow gốc
            btn.Size = snap.Size;
            btn.Location = snap.Location;
            btn.ShadowDecoration.Shadow = snap.ShadowPadding;
            btn.ShadowDecoration.Depth = snap.ShadowDepth;
            btn.ShadowDecoration.Enabled = snap.ShadowEnabled;
        }

        private struct HoverSnapshot
        {
            public System.Drawing.Size Size { get; } // Kích thước gốc
            public System.Drawing.Point Location { get; } // Vị trí gốc
            public Padding ShadowPadding { get; } // Padding shadow gốc
            public int ShadowDepth { get; } // Độ sâu shadow gốc
            public bool ShadowEnabled { get; } // Shadow ban đầu bật/tắt
            public HoverSnapshot(System.Drawing.Size size, System.Drawing.Point location, Padding shadowPadding, int shadowDepth, bool shadowEnabled)
            {
                Size = size;
                Location = location;
                ShadowPadding = shadowPadding;
                ShadowDepth = shadowDepth;
                ShadowEnabled = shadowEnabled;
            }
        }
    }

}
