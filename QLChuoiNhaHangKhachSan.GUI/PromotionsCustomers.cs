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
            InitPromotionTable();  // chỉ tạo bảng rỗng
            BindGrid();            // bind ra DataGridView (sẽ trống)
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

            // Không thêm dữ liệu mẫu nữa
            // _promotionTable.Rows.Add(...);
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

            // Danh sách các đối tượng được coi là VIP
            string[] vipTargets =
            {
                "Tất cả khách hàng VIP",
                "Vip hạng đồng",
                "Vip hạng bạc",
                "Vip hạng vàng",
                "Vip hạng bạch kim",
                "Vip hạng kim cương"
            };

            foreach (DataGridViewRow row in dgvListPromotion.Rows)
            {
                if (row.IsNewRow) continue;

                var obj = row.Cells["PromotionObject"].Value;
                if (obj == null) continue;

                var target = obj.ToString().Trim();

                // Nếu đối tượng áp dụng trùng với 1 trong các loại VIP (không phân biệt hoa/thường)
                foreach (var vip in vipTargets)
                {
                    if (string.Equals(target, vip, StringComparison.OrdinalIgnoreCase))
                    {
                        totalVip++;
                        break; // tránh đếm trùng nếu match nhiều cái
                    }
                }
            }

            guna2HtmlLabel7.Text = totalVip.ToString();
        }

        private void btnAddPromotion_Click(object sender, EventArgs e)
        {
            if (_promotionTable == null) return;

            using (var f = new PromotionAdd())
            {
                if (f.ShowDialog(this) == DialogResult.OK)
                {
                    string timeRange = string.Format("{0:dd/MM/yyyy} - {1:dd/MM/yyyy}", f.StartDate, f.EndDate);
                    string status = string.IsNullOrWhiteSpace(f.PromotionStatus)
                        ? "Còn"
                        : f.PromotionStatus.Trim();

                    _promotionTable.Rows.Add(
                        f.PromotionId,
                        f.PromotionName,
                        f.PromotionType,
                        f.PromotionObject,
                        timeRange,
                        status
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
