using System;
using System.Data;
using System.Windows.Forms;

namespace QLChuoiNhaHangKhachSan.GUI
{
    public partial class PromotionsCustomers : Form
    {
        private DataTable _promotionTable;

        public PromotionsCustomers()
        {
            InitializeComponent();
        }

        private void PromotionsCustomers_Load(object sender, EventArgs e)
        {
            InitPromotionTable();  // khởi tạo dữ liệu 1 lần
            BindGrid();            // bind lên DataGridView + cập nhật tổng
        }

        /// <summary>
        /// Load danh sách ưu đãi vào dgvListPromotion
        /// </summary>
        private void LoadPromotions()
        {
            // Sau này bạn có thể đổi thành đọc từ DB/BLL rồi gán vào _promotionTable
            BindGrid();
        }

        private void InitPromotionTable()
        {
            _promotionTable = new DataTable();
            _promotionTable.Columns.Add("Mã ưu đãi", typeof(string));
            _promotionTable.Columns.Add("Tên chương trình", typeof(string));
            _promotionTable.Columns.Add("Loại ưu đãi", typeof(string));
            _promotionTable.Columns.Add("Đối tượng áp dụng", typeof(string));
            _promotionTable.Columns.Add("Thời hạn", typeof(string));
            _promotionTable.Columns.Add("Trạng thái", typeof(string));

            // Dữ liệu mẫu ban đầu
            _promotionTable.Rows.Add("KM01", "Sale Tết", "10%", "Tất cả", "25/12/2025", "Còn");
            _promotionTable.Rows.Add("KM02", "Sinh nhật", "Voucher", "VIP", "20/12/2025", "Hết");
            _promotionTable.Rows.Add("KM03", "Kỷ niệm thành lập", "20%", "Tất cả", "01/01/2026", "Chưa đến");
            _promotionTable.Rows.Add("KM04", "Sale Tết âm lịch", "15%", "Tất cả", "01/02/2026 đến hết tháng", "Chưa đến");
        }

        private void BindGrid()
        {
            dgvListPromotion.AutoGenerateColumns = false;
            dgvListPromotion.DataSource = _promotionTable;

            UpdateTotalRunningPromotions();
            UpdateTotalVipPromotions();
        }

        /// <summary>
        /// Cập nhật số "Tổng số ưu đãi đang chạy" ở label guna2HtmlLabel4
        /// </summary>
        private void UpdateTotalRunningPromotions()
        {
            int totalRunning = 0;

            foreach (DataGridViewRow row in dgvListPromotion.Rows)
            {
                if (row.IsNewRow) continue;

                var statusObj = row.Cells["PromotionStatus"].Value;
                if (statusObj == null) continue;

                var status = statusObj.ToString().Trim();

                // “Còn” => đang chạy
                if (string.Equals(status, "Còn", StringComparison.OrdinalIgnoreCase))
                {
                    totalRunning++;
                }
            }

            guna2HtmlLabel4.Text = totalRunning.ToString();
        }

        /// <summary>
        /// Cập nhật số "Ưu đãi dành cho khách hàng VIP" ở label guna2HtmlLabel7
        /// </summary>
        private void UpdateTotalVipPromotions()
        {
            int totalVip = 0;

            foreach (DataGridViewRow row in dgvListPromotion.Rows)
            {
                if (row.IsNewRow) continue;

                var obj = row.Cells["PromotionObject"].Value;
                if (obj == null) continue;

                var target = obj.ToString().Trim();

                // Đếm tất cả ưu đãi có đối tượng áp dụng là "VIP"
                if (string.Equals(target, "VIP", StringComparison.OrdinalIgnoreCase))
                {
                    totalVip++;
                }
            }

            // Hiển thị lên ô “Ưu đãi dành cho khách hàng VIP”
            guna2HtmlLabel7.Text = totalVip.ToString();
        }

        private void btnAddPromotion_Click(object sender, EventArgs e)
        {
            if (_promotionTable == null) return;

            using (var f = new PromotionAdd())
            {
                if (f.ShowDialog(this) == DialogResult.OK)
                {
                    _promotionTable.Rows.Add(
                        f.PromotionId,
                        f.PromotionName,
                        f.PromotionType,
                        f.PromotionObject,
                        f.PromotionTime,
                        f.PromotionStatus
                    );
                    BindGrid();
                }
            }
        }

        private void btnDeletePromotion_Click(object sender, EventArgs e)
        {
            if (_promotionTable == null) return;
            if (dgvListPromotion.CurrentRow == null) return;

            int rowIndex = dgvListPromotion.CurrentRow.Index;
            if (rowIndex >= 0 && rowIndex < _promotionTable.Rows.Count)
            {
                _promotionTable.Rows.RemoveAt(rowIndex);
            }

            BindGrid(); // cập nhật grid + tổng
        }

        private void btnFindPromotion_Click(object sender, EventArgs e)
        {
            if (_promotionTable == null) return;

            string keyword = tbFindPromotion.Text.Trim();

            if (string.IsNullOrEmpty(keyword))
            {
                // Không nhập gì -> hiển thị lại toàn bộ
                dgvListPromotion.DataSource = _promotionTable;
                UpdateTotalRunningPromotions();
                UpdateTotalVipPromotions();
                return;
            }

            // Lọc theo Mã ưu đãi hoặc Tên chương trình (chứa keyword, không phân biệt hoa thường)
            // Dùng DataView để filter
            var view = new DataView(_promotionTable);

            // Escape ' để tránh lỗi filter
            string safeKeyword = keyword.Replace("'", "''");

            view.RowFilter =
                $"[Mã ưu đãi] LIKE '%{safeKeyword}%' OR [Tên chương trình] LIKE '%{safeKeyword}%'";

            dgvListPromotion.DataSource = view;

            // Cập nhật lại 2 tổng dựa trên kết quả đã lọc
            UpdateTotalRunningPromotions();
            UpdateTotalVipPromotions();
        }

        private void dgvListPromotion_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Nếu có thay đổi Đối tượng áp dụng / Trạng thái ngay trên grid thì:
            // UpdateTotalRunningPromotions();
            // UpdateTotalVipPromotions();
        }

        private void guna2Button8_Click(object sender, EventArgs e)
        {
            // Refresh lại grid từ _promotionTable (không tạo dữ liệu mới)
            BindGrid();
        }
    }
}
