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
    public partial class frmMenu : Form
    {
        // Định nghĩa event để thông báo khi một món ăn được thêm
        public event EventHandler<DishAddedEventArgs> DishAdded;

        public frmMenu()
        {
            InitializeComponent();

            this.Load += new System.EventHandler(this.Menu_Load);
            dgvDsMon.SelectionChanged += new System.EventHandler(this.UpdateAddButtonState);
        }

        private void Menu_Load(object sender, EventArgs e)
        {
            // --- THIẾT LẬP BẢNG DANH SÁCH MÓN ĂN (dgvDsMon) ---
            dgvDsMon.Columns.Clear();
            dgvDsMon.Rows.Clear();
            dgvDsMon.Columns.Add("STT", "STT");
            dgvDsMon.Columns.Add("TenMon", "Tên món");
            dgvDsMon.Columns.Add("Loai", "Loại");
            dgvDsMon.Columns.Add("DonGia", "Đơn giá");
            dgvDsMon.Columns.Add("TrangThai", "Trạng thái");
            dgvDsMon.Columns["TenMon"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            // Thêm dữ liệu mẫu cho dgvDsMon
            dgvDsMon.Rows.Add("1", "Phở Bò", "Món chính", 50000, "Đang bán");
            dgvDsMon.Rows.Add("2", "Bún Chả", "Món chính", 45000, "Đang bán");
            dgvDsMon.Rows.Add("3", "Nem Rán", "Món phụ", 30000, "Đang bán");
            dgvDsMon.Rows.Add("4", "Trà Đá", "Đồ uống", 5000, "Đang bán");
            dgvDsMon.Rows.Add("5", "Bia Hà Nội", "Đồ uống", 15000, "Hết hàng");

            // --- THIẾT LẬP BẢNG MÓN ĐÃ GỌI (dgvDishMenu) ---
            dgvDishMenu.Columns.Clear();
            dgvDishMenu.Rows.Clear();
            dgvDishMenu.Columns.Add("TenMon", "Tên món");      // <-- Tên cột: TenMon
            dgvDishMenu.Columns.Add("SoLuong", "Số lượng");    // <-- Tên cột: SoLuong
            dgvDishMenu.Columns.Add("DonGia", "Đơn giá");      // <-- Tên cột: DonGia
            dgvDishMenu.Columns.Add("ThanhTien", "Thành tiền"); // <-- Tên cột: ThanhTien
            dgvDishMenu.Columns["TenMon"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            // Vô hiệu hóa nút "Thêm" ban đầu
            bntAdd.Enabled = false;
            UpdateTotalDishTypes();
        }

        private void UpdateAddButtonState(object sender, EventArgs e)
        {
            bntAdd.Enabled = dgvDsMon.SelectedRows.Count > 0;
        }

        private void bntAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvDsMon.SelectedRows.Count == 0) return;

                var selectedRow = dgvDsMon.SelectedRows[0];

                // 1. Kiểm tra xem món có đang bán không (dùng tên cột đã đặt)
                if (selectedRow.Cells["TrangThai"].Value.ToString() != "Đang bán")
                {
                    MessageBox.Show("Món này hiện không có sẵn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 2. Lấy thông tin món ăn từ dgvDsMon (dùng tên cột đã đặt)
                string tenMon = selectedRow.Cells["TenMon"].Value.ToString();
                decimal donGia = Convert.ToDecimal(selectedRow.Cells["DonGia"].Value);
                int soLuong = 1; // Mặc định thêm 1, có thể thay bằng NumericUpDown

                // 3. Kiểm tra xem món đã tồn tại trong dgvDishMenu chưa
                foreach (DataGridViewRow existingRow in dgvDishMenu.Rows)
                {
                    if (existingRow.Cells["TenMon"].Value != null && existingRow.Cells["TenMon"].Value.ToString() == tenMon)
                    {
                        // Nếu đã có, chỉ tăng số lượng
                        int currentQuantity = Convert.ToInt32(existingRow.Cells["SoLuong"].Value);
                        existingRow.Cells["SoLuong"].Value = currentQuantity + soLuong;
                        existingRow.Cells["ThanhTien"].Value = (currentQuantity + soLuong) * donGia;
                        
                        // Cập nhật lại tổng tiền
                        UpdateTotalAmount();
                        return; // Kết thúc sau khi cập nhật
                    }
                }

                // 4. Nếu món chưa có, thêm dòng mới vào dgvDishMenu
                decimal thanhTien = soLuong * donGia;
                dgvDishMenu.Rows.Add(tenMon, soLuong, donGia, thanhTien);

                // 5. Cập nhật lại tổng tiền
                UpdateTotalAmount();
            }
            catch (Exception ex)
            {
                // Nếu có lỗi (thường là do sai tên cột), thông báo sẽ hiện ra
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Hàm mới để tính và cập nhật tổng tiền
        private void UpdateTotalAmount()
        {
            decimal total = 0;
            foreach (DataGridViewRow row in dgvDishMenu.Rows)
            {
                total += Convert.ToDecimal(row.Cells["ThanhTien"].Value);
            }
            txtTotal.Text = total.ToString("N0"); // Định dạng số cho dễ đọc
        }

        private void UpdateTotalDishTypes()
        {
            // Sử dụng HashSet để đếm các tên món khác nhau
            HashSet<string> uniqueDishes = new HashSet<string>();
            foreach (DataGridViewRow row in dgvDsMon.Rows)
            {
                if (row.Cells["TenMon"].Value != null)
                {
                    uniqueDishes.Add(row.Cells["TenMon"].Value.ToString());
                }
            }
            txtSum.Text = uniqueDishes.Count.ToString();
        }

        protected virtual void OnDishAdded(DishAddedEventArgs e)
        {
            DishAdded?.Invoke(this, e);
        }

        // Các event trống giữ nguyên
        private void dgvDsMon_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void guna2PictureBox1_Click(object sender, EventArgs e) { }
        private void guna2Button3_Click(object sender, EventArgs e) { }
        private void guna2Button1_Click(object sender, EventArgs e) { }
        private void cboTilter_SelectedIndexChanged(object sender, EventArgs e) { }
        private void guna2HtmlLabel3_Click(object sender, EventArgs e) { }

        private void txtItemCode_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtDishName_TextChanged(object sender, EventArgs e)
        {

        }

        private void cboNumber_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtTotal_TextChanged(object sender, EventArgs e)
        {

        }

        private void pnlList_Paint(object sender, PaintEventArgs e)
        {

        }
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {

        }
    }
    public class DishAddedEventArgs : EventArgs
    {
        public string TenMon { get; }
        public decimal DonGia { get; }
        public int SoLuong { get; }

        public DishAddedEventArgs(string tenMon, decimal donGia, int soLuong)
        {
            TenMon = tenMon;
            DonGia = donGia;
            SoLuong = soLuong;
        }
    }
}
