using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLChuoiNhaHangKhachSan.GUI
{
    public partial class CustomersList : Form
    {
        // Bảng dữ liệu khách hàng dùng chung cho form
        private DataTable _customersTable;

        public CustomersList()
        {
            InitializeComponent();

            dgvListCustomers.AutoGenerateColumns = false;
            dgvListCustomers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvListCustomers.ColumnHeadersHeightSizeMode =
                DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvListCustomers.ColumnHeadersHeight = 50;

            // Cho phép chọn nhiều dòng, chọn theo hàng
            dgvListCustomers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvListCustomers.MultiSelect = true;
        }

        private void CustomersList_Load(object sender, EventArgs e)
        {
            KhoiTaoBangKhachHang();
            HienThiLenGrid();
            CapNhatThongKe();
            CapNhatSoKhachHangThuong();
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
                if (f.ShowDialog() == DialogResult.OK)
                {
                    var id      = f.CustomerId;
                    var name    = f.CustomerName;
                    var cccd    = f.CCCD;
                    var sex     = f.Sex;
                    var phone   = f.PhoneNumber;
                    var email   = f.Email;
                    var address = f.Address;
                    var type    = f.CustomerType;

                    _customersTable.Rows.Add(
    id,
    name,
    f.Nationality,   // dùng đúng property
    cccd,
    sex,
    phone,
    email,
    address,
    type);
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

            var selectedIds = dgvListCustomers.SelectedRows
                .Cast<DataGridViewRow>()
                .Select(r => r.Cells["CustomersID"].Value?.ToString()) // ĐÃ SỬA TỪ "Mã khách hàng" -> "CustomersID"
                .Where(id => !string.IsNullOrEmpty(id))
                .ToList();

            string message = "Bạn có chắc chắn muốn xóa các khách hàng sau:\n" +
                             string.Join(", ", selectedIds) + " ?";

            var confirm = MessageBox.Show(message,
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
                return;

            var rowsToDelete = dgvListCustomers.SelectedRows
                .Cast<DataGridViewRow>()
                .Select(r => r.DataBoundItem as DataRowView)
                .Where(rv => rv != null)
                .ToList();

            foreach (var rowView in rowsToDelete)
            {
                rowView.Row.Delete();
            }

            CapNhatThongKe();
            CapNhatSoKhachHangThuong();
        }

        private void btnUpdateCustomers_Click(object sender, EventArgs e)
        {
            if (dgvListCustomers.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn một khách hàng để cập nhật.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Lấy DataRowView tương ứng với dòng đang chọn
            var rowView = dgvListCustomers.CurrentRow.DataBoundItem as DataRowView;
            if (rowView == null)
            {
                MessageBox.Show("Không thể lấy thông tin khách hàng được chọn.",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DataRow row = rowView.Row;

            // Mở form cập nhật, truyền dữ liệu hiện tại sang
            using (var f = new CustomerAdd())
            {
                f.Mode         = CustomerAdd.CustomerFormMode.Edit;

                f.CustomerId   = row.Field<string>("Mã khách hàng");
                f.CustomerName = row.Field<string>("Tên khách hàng");
                f.CCCD        = row.Field<string>("CCCD");
                f.Sex         = row.Field<string>("Giới tính");
                f.PhoneNumber = row.Field<string>("Số điện thoại");
                f.Email       = row.Field<string>("Gmail");      // THÊM DÒNG NÀY
                f.Address     = row.Field<string>("Địa chỉ");
                f.CustomerType= row.Field<string>("Loại khách");
                f.Nationality = row.Field<string>("Quốc tịch");

                if (f.ShowDialog() == DialogResult.OK)
                {
                    row["Tên khách hàng"] = f.CustomerName;
                    row["CCCD"]           = f.CCCD;
                    row["Giới tính"]      = f.Sex;
                    row["Số điện thoại"]  = f.PhoneNumber;
                    row["Gmail"]          = f.Email;             // THÊM DÒNG NÀY
                    row["Địa chỉ"]        = f.Address;
                    row["Loại khách"]     = f.CustomerType;
                    row["Quốc tịch"]      = f.Nationality;

                    row.AcceptChanges();
                    CapNhatThongKe();
                    CapNhatSoKhachHangThuong();
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
    }
}
