using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QLChuoiNhaHangKhachSan.BLL;
using QLChuoiNhaHangKhachSan.BLL.DTOs;

namespace QLChuoiNhaHangKhachSan.GUI
{
    public partial class CustomersList : Form
    {
        // Bảng dữ liệu khách hàng dùng chung cho form
        private DataTable _customersTable;
        private readonly CustomerService _customerService = new CustomerService();

        public CustomersList()
        {
            InitializeComponent();

            // Cho phép vẽ lại header theo chiều cao mới
            dgvListCustomers.EnableHeadersVisualStyles = false;
            dgvListCustomers.ColumnHeadersHeightSizeMode =
                DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvListCustomers.ColumnHeadersHeight = 90;
            dgvListCustomers.ThemeStyle.HeaderStyle.Height = 90;

            dgvListCustomers.AutoGenerateColumns = false;
            dgvListCustomers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

           
            dgvListCustomers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvListCustomers.MultiSelect = true;

 
        }

        private void CustomersList_Load(object sender, EventArgs e)
        {
            KhoiTaoBangKhachHang(); // tạo cấu trúc cột
            NapDuLieuTuSql();       // <<< lấy dữ liệu từ SQL đổ vào _customersTable
            HienThiLenGrid();       // gán DataSource cho dgvListCustomers
            CapNhatThongKe();       // cập nhật tổng số, số VIP
            CapNhatSoKhachHangThuong(); // cập nhật số khách thường
        }

        private void KhoiTaoBangKhachHang()
        {
            _customersTable = new DataTable();

            _customersTable.Columns.Add("Mã khách hàng");
            _customersTable.Columns.Add("Tên khách hàng");
            _customersTable.Columns.Add("Quốc tịch");   // đúng đường dẫn cột
            _customersTable.Columns.Add("CCCD");
            _customersTable.Columns.Add("Giới tính");
            _customersTable.Columns.Add("Số điện thoại");
            _customersTable.Columns.Add("Gmail");
            _customersTable.Columns.Add("Địa chỉ");
            _customersTable.Columns.Add("Loại khách");

            // Không thêm Rows ở đây, để form mở lên là bảng trống
        }
        private void NapDuLieuTuSql()
        {
            // Nếu DataTable chưa được khởi tạo thì khởi tạo
            if (_customersTable == null)
                KhoiTaoBangKhachHang();

            // Xóa hết dòng cũ trong bảng
            _customersTable.Rows.Clear();

            // Lấy toàn bộ khách hàng từ SQL thông qua CustomerService
            var customers = _customerService.GetAllCustomers();

            // Duyệt từng khách hàng và thêm vào DataTable
            foreach (var c in customers)
            {
                _customersTable.Rows.Add(
                    c.CustomerId.ToString(), // "Mã khách hàng"
                    c.FullName,              // "Tên khách hàng"
                    c.Nationality,           // "Quốc tịch"
                    c.CCCD,                  // "CCCD"
                    c.Sex,                   // "Giới tính"
                    c.PhoneNumber,           // "Số điện thoại"
                    c.Email,                 // "Gmail"
                    c.Address,               // "Địa chỉ"
                    c.CustomerType           // "Loại khách"
                );
            }
        }

        private void HienThiLenGrid()
        {
            dgvListCustomers.DataSource = _customersTable;
        }

        private void CapNhatThongKe()
        {
            if (_customersTable == null) return;

            // Chỉ lấy các dòng chưa bị xóa
            var rows = _customersTable.AsEnumerable()
                              .Where(r => r.RowState != DataRowState.Deleted);

            int tongKhach = rows.Count();
            int soVip = rows.Count(r =>
        string.Equals(r.Field<string>("Loại khách"), "VIP",
                      StringComparison.OrdinalIgnoreCase));

            lblTotalCustomerCountNumber.Text = tongKhach.ToString();
            lblVIPCount.Text = soVip.ToString();
        }

        private void TimKiemKhachHang(string tuKhoa)
        {
            if (_customersTable == null)
                return;

            DataView view = _customersTable.DefaultView;

            if (!string.IsNullOrWhiteSpace(tuKhoa))
            {
                string safeKeyword = tuKhoa.Replace("'", "''");
                view.RowFilter =
                    $"[Mã khách hàng] LIKE '%{safeKeyword}%' OR " +
                    $"[Tên khách hàng] LIKE '%{safeKeyword}%'";
            }
            else
            {
                // XÓA LỌC KHI Ô TÌM KIẾM RỖNG
                view.RowFilter = string.Empty;
            }

            dgvListCustomers.DataSource = view;
            CapNhatThongKe();
            CapNhatSoKhachHangThuong();
        }

        private void tbFindCustomers_TextChanged(object sender, EventArgs e)
        {
            // Tìm realtime khi gõ
            TimKiemKhachHang(tbFindCustomers.Text);
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            // Không làm gì, hoặc cũng có thể gọi lại:
            // TimKiemKhachHang(tbFindCustomers.Text);
        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Nếu bạn không dùng sự kiện này, có thể xóa hẳn handler trong Designer
        }

        private void bntAddCustomers_Click(object sender, EventArgs e)
        {
            using (var f = new CustomerAdd())
            {
                f.Mode = CustomerAdd.CustomerFormMode.Add;

                if (f.ShowDialog() == DialogResult.OK)
                {
                    var customer = new CustomerDto
                    {
                        FullName     = f.CustomerName,
                        Nationality  = f.Nationality,
                        CCCD         = f.CCCD,
                        Sex          = f.Sex,
                        PhoneNumber  = f.PhoneNumber,
                        Email        = f.Email,
                        Address      = f.Address,
                        CustomerType = f.CustomerType
                    };

                    // 1. Lưu xuống SQL, ID sẽ tự tăng trong DB
                    int newId = _customerService.AddCustomer(customer);

                    // 2. Gán lại vào form (để nếu bạn muốn hiển thị hoặc dùng sau này)
                    f.CustomerId = newId.ToString(); // sẽ đổ vào TxbCustomersID

                    // 3. Thêm vào DataTable để hiển thị
                    _customersTable.Rows.Add(
                        newId.ToString(),        // "Mã khách hàng"
                        customer.FullName,
                        customer.Nationality,
                        customer.CCCD,
                        customer.Sex,
                        customer.PhoneNumber,
                        customer.Email,
                        customer.Address,
                        customer.CustomerType
                    );

                    CapNhatThongKe();
                    CapNhatSoKhachHangThuong();
                }
            }
        }

        private void pnlSidebar_Paint(object sender, PaintEventArgs e)
        {
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
        }

        private void lblTotalCustomerCount_Click(object sender, EventArgs e)
        {
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
        }

        private void lblVIPCount_Click(object sender, EventArgs e)
        {
        }

        private void lblVIPCustomers_Click(object sender, EventArgs e)
        {
        }

        private void pnHeader2_Paint(object sender, PaintEventArgs e)
        {
        }

        private void btnDelectCustomers_Click(object sender, EventArgs e)
        {
            if (dgvListCustomers.SelectedRows == null || dgvListCustomers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn ít nhất một khách hàng để xóa.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var selectedRows = dgvListCustomers.SelectedRows
                .Cast<DataGridViewRow>()
                .Select(r => r.DataBoundItem as DataRowView)
                .Where(rv => rv != null)
                .ToList();

            var selectedIds = selectedRows
                .Select(rv => rv.Row.Field<string>("Mã khách hàng"))
                .Where(id => !string.IsNullOrEmpty(id))
                .ToList();

            if (selectedIds.Count == 0)
            {
                MessageBox.Show("Không lấy được mã khách hàng để xóa.",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string message = "Bạn có chắc chắn muốn xóa các khách hàng sau:\n" +
                             string.Join(", ", selectedIds) + " ?";

            var confirm = MessageBox.Show(message,
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
                return;

            try
            {
                // 1. Xóa trong SQL
                foreach (var idStr in selectedIds)
                {
                    if (int.TryParse(idStr, out int id))
                    {
                        _customerService.DeleteCustomer(id);
                    }
                }

                // 2. Xóa trong DataTable
                foreach (var rowView in selectedRows)
                {
                    rowView.Row.Delete();
                }

                CapNhatThongKe();
                CapNhatSoKhachHangThuong();

                MessageBox.Show("Đã xóa khách hàng trong cơ sở dữ liệu.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa khách hàng trong SQL:\n" + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdateCustomers_Click(object sender, EventArgs e)
        {
            if (dgvListCustomers.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn một khách hàng để cập nhật.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var rowView = dgvListCustomers.CurrentRow.DataBoundItem as DataRowView;
            if (rowView == null)
            {
                MessageBox.Show("Không thể lấy thông tin khách hàng được chọn.",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DataRow row = rowView.Row;

            using (var f = new CustomerAdd())
            {
                f.Mode         = CustomerAdd.CustomerFormMode.Edit;

                f.CustomerId   = row.Field<string>("Mã khách hàng");
                f.CustomerName = row.Field<string>("Tên khách hàng");
                f.CCCD        = row.Field<string>("CCCD");
                f.Sex         = row.Field<string>("Giới tính");
                f.PhoneNumber = row.Field<string>("Số điện thoại");
                f.Email       = row.Field<string>("Gmail");
                f.Address     = row.Field<string>("Địa chỉ");
                f.CustomerType= row.Field<string>("Loại khách");
                f.Nationality = row.Field<string>("Quốc tịch");

                if (f.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // 1. Cập nhật xuống SQL
                        var customer = new CustomerDto
                        {
                            CustomerId   = int.Parse(row.Field<string>("Mã khách hàng")),
                            FullName     = f.CustomerName,
                            Nationality  = f.Nationality,
                            CCCD         = f.CCCD,
                            Sex          = f.Sex,
                            PhoneNumber  = f.PhoneNumber,
                            Email        = f.Email,
                            Address      = f.Address,
                            CustomerType = f.CustomerType
                        };

                        _customerService.UpdateCustomer(customer);

                        // 2. Cập nhật lại DataTable để hiển thị
                        row["Tên khách hàng"] = f.CustomerName;
                        row["CCCD"]           = f.CCCD;
                        row["Giới tính"]      = f.Sex;
                        row["Số điện thoại"]  = f.PhoneNumber;
                        row["Gmail"]          = f.Email;
                        row["Địa chỉ"]        = f.Address;
                        row["Loại khách"]     = f.CustomerType;
                        row["Quốc tịch"]      = f.Nationality;

                        row.AcceptChanges();
                        CapNhatThongKe();
                        CapNhatSoKhachHangThuong();

                        MessageBox.Show("Đã cập nhật khách hàng trong cơ sở dữ liệu.",
                            "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi cập nhật khách hàng xuống SQL:\n" + ex.Message,
                            "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {

        }

        private int DemKhachHangThuong()
        {
            if (_customersTable == null)
                return 0;

            return _customersTable.AsEnumerable()
                .Where(r => r.RowState != DataRowState.Deleted)
                .Count(r => string.Equals(
                    r.Field<string>("Loại khách"),
                    "Thường",
                    StringComparison.OrdinalIgnoreCase));
        }

        private void CapNhatSoKhachHangThuong()
        {
            int soKhachHangThuong = DemKhachHangThuong();
            guna2HtmlLabel5.Text = soKhachHangThuong.ToString();
        }

        private void bntFilterCustomers_Click(object sender, EventArgs e)
        {
            if (_customersTable == null)
                return;

            string selectedType = cboFilterCustomers.SelectedItem?.ToString();
            DataView view = _customersTable.DefaultView;

            if (string.IsNullOrEmpty(selectedType) || selectedType == "Tất cả")
            {
                // Hiện lại tất cả khách hàng
                view.RowFilter = string.Empty;
            }
            else
            {
                // Lọc theo loại khách đã chọn: VIP hoặc Thường
                view.RowFilter = $"[Loại khách] = '{selectedType}'";
            }

            dgvListCustomers.DataSource = view;

            CapNhatThongKe();
            CapNhatSoKhachHangThuong();
        }

        private void lblCustomerManagement_Click(object sender, EventArgs e)
        {

        }

        private void guna2HtmlLabel3_Click(object sender, EventArgs e)
        {

        }

        private void dgvListCustomers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void guna2GradientPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

       

        
    }
}
