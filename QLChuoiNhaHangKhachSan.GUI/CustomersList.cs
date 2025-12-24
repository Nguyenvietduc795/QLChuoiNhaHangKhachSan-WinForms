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
        }

        private void KhoiTaoBangKhachHang()
        {
            _customersTable = new DataTable();

            _customersTable.Columns.Add("Mã khách hàng");
            _customersTable.Columns.Add("Tên khách hàng");
            _customersTable.Columns.Add("Số điện thoại");
            _customersTable.Columns.Add("Địa chỉ");
            _customersTable.Columns.Add("Loại khách");

            _customersTable.Rows.Add("KH001", "Lữ Nhựt Linh", "0702856480", "Đồng Tháp", "VIP");
            _customersTable.Rows.Add("KH002", "Nguyễn Trường Phi", "0923532971", "Bến Tre", "Thường");
            _customersTable.Rows.Add("KH003", "Nguyễn Thị Hồng Gấm", "0363227415", "Tiền Giang", "Thường");
            _customersTable.Rows.Add("KH004", "Hứa Mỹ Lam", "0367724702", "Đồng Tháp", "Thường");
            _customersTable.Rows.Add("KH005", "Lâm Trí Tùa", "0395918171", "Sốc Trăng", "Thường");
            _customersTable.Rows.Add("KH006", "Nguyễn Việt Đức", "0973879105", "Cần Thơ", "Thường");
        }

        private void HienThiLenGrid()
        {
            dgvListCustomers.DataSource = _customersTable;
        }

        private void CapNhatThongKe()
        {
            if (_customersTable == null) return;

            int tongKhach = _customersTable.Rows.Count;
            int soVip = _customersTable.AsEnumerable()
                .Count(r => string.Equals(r.Field<string>("Loại khách"), "VIP", StringComparison.OrdinalIgnoreCase));

            // Tiêu đề "Tổng khách hàng" giữ nguyên
            // Số tổng khách nằm ở lblTotalCustomerCountNumber
            lblTotalCustomerCountNumber.Text = tongKhach.ToString();
            lblVIPCount.Text = soVip.ToString();
        }

        private void TimKiemKhachHang(string tuKhoa)
        {
            if (_customersTable == null)
                return;

            string filter = string.Empty;

            if (!string.IsNullOrWhiteSpace(tuKhoa))
            {
                string safeKeyword = tuKhoa.Replace("'", "''");
                filter =
                    $"[Mã khách hàng] LIKE '%{safeKeyword}%' OR " +
                    $"[Tên khách hàng] LIKE '%{safeKeyword}%'";

                DataView view = _customersTable.DefaultView;
                view.RowFilter = filter;

                dgvListCustomers.DataSource = view;
            }
            else
            {
                dgvListCustomers.DataSource = _customersTable;
            }

            // Sau khi thay đổi data source thì cập nhật lại label
            CapNhatThongKe();
        }

        private void tbFindCustomers_TextChanged(object sender, EventArgs e)
        {
            // Tìm kiếm realtime khi gõ
            TimKiemKhachHang(tbFindCustomers.Text);
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            // Tìm kiếm khi bấm nút
            TimKiemKhachHang(tbFindCustomers.Text);
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
                    if (_customersTable != null)
                    {
                        _customersTable.Rows.Add(
                            f.CustomerId,
                            f.CustomerName,
                            f.PhoneNumber,
                            f.Address,
                            f.CustomerType
                        );

                        HienThiLenGrid();
                        CapNhatThongKe();
                    }
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
                f.Mode = CustomerAdd.CustomerFormMode.Edit;

                f.CustomerId = row.Field<string>("Mã khách hàng");
                f.CustomerName = row.Field<string>("Tên khách hàng");
                f.PhoneNumber = row.Field<string>("Số điện thoại");
                f.Address = row.Field<string>("Địa chỉ");
                f.CustomerType = row.Field<string>("Loại khách");

                if (f.ShowDialog() == DialogResult.OK)
                {
                    row["Tên khách hàng"] = f.CustomerName;
                    row["Số điện thoại"] = f.PhoneNumber;
                    row["Địa chỉ"] = f.Address;
                    row["Loại khách"] = f.CustomerType;

                    row.AcceptChanges();
                    CapNhatThongKe();
                }
            }
        }
    }
}
