using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace QLChuoiNhaHangKhachSan.GUI
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 1. Khởi tạo trạng thái Sidebar: Đóng tất cả các panel nhóm
            CloseAllSubMenus();

            // Cấu hình kết nối (giữ nguyên của bạn)
            try
            {
                string connStr = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
                // MessageBox.Show(connStr);
            }
            catch { }
        }

        // Hàm đóng tất cả các Menu con - Hãy thêm tất cả các Panel nhóm của bạn vào đây
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
        private void SetActiveButton(object sender)
        {
            // Duyệt qua flpSidebar để reset trạng thái Checked của tất cả các nút
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

            // Bật màu cho nút vừa nhấn
            if (sender is Guna.UI2.WinForms.Guna2Button clickedBtn)
            {
                clickedBtn.Checked = true;
            }
        }
        // Xử lý sự kiện click cho các nút
        //nút home
        private void btnHome_Click(object sender, EventArgs e)
        {
            SetActiveButton(sender); // Chỉ nút này sáng màu
            CloseAllSubMenus();      // Các nút con của Nhân viên... sẽ bị thu lại
        }
        /************************************************************************************************************/
        //nút nhân viên
        bool isStaffExpanded = false; // Biến trạng thái mở rộng của nhóm Nhân viên
        private void btnStaff_Click(object sender, EventArgs e)
        {

            SetActiveButton(sender); // Highlight nút cha

            if (pnlStaff_group.Height == 50)
            {
                CloseAllSubMenus(); // Đóng các nhóm khác trước khi mở nhóm này
                pnlStaff_group.Height = 130; // Mở rộng
            }
            else
            {
                pnlStaff_group.Height = 50; // Đóng lại
            }
        }

        private void btnListStaff_Click(object sender, EventArgs e)
        {
            SetActiveButton(sender); // Chỉ đổi màu highlight cho nút con
                                 // Gọi UserControl hoặc Form danh sách tại đây
        }

        private void btnPayRoll_Click(object sender, EventArgs e)
        {
            SetActiveButton(sender); // Chỉ đổi màu highlight cho nút con
                                     // Gọi UserControl hoặc Form danh sách tại đây
        }

        /************************************************************************************************************/
        //nút khách hàng
        private void btnCustomers_Click(object sender, EventArgs e)
        {
            SetActiveButton(sender); // Highlight nút cha

            if (pnlCustomer_group.Height == 50)
            {
                CloseAllSubMenus(); // Đóng các nhóm khác trước khi mở nhóm này
                pnlCustomer_group.Height = 130; // Mở rộng
            }
            else
            {
                pnlCustomer_group.Height = 50; // Đóng lại
            }
        }

        private void btnCustomerList_Click(object sender, EventArgs e)
        {
            SetActiveButton(sender); // Chỉ đổi màu highlight cho nút con
                                     // Gọi UserControl hoặc Form danh sách tại đây
        }

        private void btnLoyaltyProgram_Click(object sender, EventArgs e)
        {
            SetActiveButton(sender); // Chỉ đổi màu highlight cho nút con
                                     // Gọi UserControl hoặc Form danh sách tại đây
        }

        private void pnlSidebar_Paint(object sender, PaintEventArgs e)
        {

        }
        /************************************************************************************************************/
        //nút khách sạn
        private void btnHotels_Click(object sender, EventArgs e)
        {
            SetActiveButton(sender); // Highlight nút cha

            if (pnlHotel_group.Height == 50)
            {
                CloseAllSubMenus(); // Đóng các nhóm khác trước khi mở nhóm này
                pnlHotel_group.Height = 130; // Mở rộng
            }
            else
            {
                pnlHotel_group.Height = 50; // Đóng lại
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
        //nút nhà hàng
        private void btnRestaurants_Click(object sender, EventArgs e)
        {
            SetActiveButton(sender); // Highlight nút cha

            if (pnlRestaurent_group.Height == 50)
            {
                CloseAllSubMenus(); // Đóng các nhóm khác trước khi mở nhóm này
                pnlRestaurent_group.Height = 130; // Mở rộng
            }
            else
            {
                pnlRestaurent_group.Height = 50; // Đóng lại
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

        //nút kho
        private void btnInventory_Click(object sender, EventArgs e)
        {
            SetActiveButton(sender); // Highlight nút cha

            if (pnlInvetory_group.Height == 50)
            {
                CloseAllSubMenus(); // Đóng các nhóm khác trước khi mở nhóm này
                pnlInvetory_group.Height = 130; // Mở rộng
            }
            else
            {
                pnlInvetory_group.Height = 50; // Đóng lại
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
        //nút thanh toán
        private void btnPayments_Click(object sender, EventArgs e)
        {
            SetActiveButton(sender); // Highlight nút cha

            if (pnlPayment_group.Height == 50)
            {
                CloseAllSubMenus(); // Đóng các nhóm khác trước khi mở nhóm này
                pnlPayment_group.Height = 130; // Mở rộng
            }
            else
            {
                pnlPayment_group.Height = 50; // Đóng lại
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
        //nút báo cáo
        private void btnReports_Click(object sender, EventArgs e)
        {
            SetActiveButton(sender); // Highlight nút cha

            if (pnlReport_group.Height == 50)
            {
                CloseAllSubMenus(); // Đóng các nhóm khác trước khi mở nhóm này
                pnlReport_group.Height = 130; // Mở rộng
            }
            else
            {
                pnlReport_group.Height = 50; // Đóng lại
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

        private void btnLogout_Click(object sender, EventArgs e)
        {
            SetActiveButton(sender); // Highlight nút cha
        }
    }

}
