using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using QLChuoiNhaHangKhachSan.BLL;
using QLChuoiNhaHangKhachSan.BLL.DTOs;

namespace QLChuoiNhaHangKhachSan.GUI
{
    public partial class PromotionsCustomers : Form
    {
        private readonly PromotionService _promotionService = new PromotionService();
        private DataTable _promotionsTable;

        public PromotionsCustomers()
        {
            InitializeComponent();

            dgvListPromotion.AutoGenerateColumns = false;
            dgvListPromotion.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvListPromotion.MultiSelect = true;
        }

        private void PromotionsCustomers_Load(object sender, EventArgs e)
        {
            KhoiTaoBangUuDai();
            NapDuLieuUuDaiTuSql();
            HienThiLenGrid();

            CapNhatThongKe(); // nếu muốn hiển thị tổng số ưu đãi, đang chạy...
        }

        private void KhoiTaoBangUuDai()
        {
            _promotionsTable = new DataTable();
            _promotionsTable.Columns.Add("Id");          // cột 0, ẩn
            _promotionsTable.Columns.Add("Mã ưu đãi");  // cột 1, hiển thị
            _promotionsTable.Columns.Add("Tên chương trình");
            _promotionsTable.Columns.Add("Loại ưu đãi");
            _promotionsTable.Columns.Add("Đối tượng áp dụng");
            _promotionsTable.Columns.Add("Thời hạn");
            _promotionsTable.Columns.Add("Trạng thái");
        }

        /// <summary>
        /// Load danh sách ưu đãi vào dgvListPromotion
        /// </summary>
        private void NapDuLieuUuDaiTuSql()
        {
            if (_promotionsTable == null)
                KhoiTaoBangUuDai();

            _promotionsTable.Rows.Clear();

            var promotions = _promotionService.GetAllPromotions();

            foreach (var p in promotions)
            {
                _promotionsTable.Rows.Add(
    p.PromotionId.ToString(),   // Id
    p.PromotionCode,            // Mã ưu đãi hiển thị
    p.ProgramName,
    p.PromotionType,
    p.TargetAudience,
    p.ExpirationDate?.ToString("dd/MM/yyyy"),
    p.Status
);
            }
        }

        private void HienThiLenGrid()
        {
            dgvListPromotion.DataSource = _promotionsTable;

            // nếu muốn HIỂN THỊ lại Id thì không set Visible = false
            if (dgvListPromotion.Columns["Id"] != null)
            {
                dgvListPromotion.Columns["Id"].HeaderText = "Mã ưu đãi (ID)";
            }
        }

        private void CapNhatThongKe()
        {
            if (_promotionsTable == null) return;

            var rows = _promotionsTable.AsEnumerable()
                                       .Where(r => r.RowState != DataRowState.Deleted);

            int tongUuDai = rows.Count();
            guna2HtmlLabel4.Text = tongUuDai.ToString();

            // Ví dụ: đếm ưu đãi có "VIP" trong Đối tượng áp dụng
            int uuDaiVip = rows.Count(r =>
                (r.Field<string>("Đối tượng áp dụng") ?? "")
                    .IndexOf("VIP", StringComparison.OrdinalIgnoreCase) >= 0);

            guna2HtmlLabel7.Text = uuDaiVip.ToString();
        }

        /// <summary>
        /// Cập nhật số "Tổng số ưu đãi đang chạy" ở label guna2HtmlLabel4
        /// </summary>
        

        /// <summary>
        /// Cập nhật số "Ưu đãi dành cho khách hàng VIP" ở label guna2HtmlLabel7
        /// </summary>
        

        private void btnAddPromotion_Click(object sender, EventArgs e)
        {
            if (_promotionsTable == null) return;

            using (var f = new PromotionAdd())
            {
                if (f.ShowDialog(this) == DialogResult.OK)
                {
                    try
                    {
                        // 1. Tạo đối tượng PromotionDto từ form nhập
                        var promotion = new PromotionDto
                        {
                            PromotionCode  = f.PromotionCode,   // NEW
                            ProgramName    = f.PromotionName,
                            PromotionType  = f.PromotionType,
                            TargetAudience = f.PromotionObject,
                            ExpirationDate = f.EndDate,
                            Status         = string.IsNullOrWhiteSpace(f.PromotionStatus)
                                                ? "Còn"
                                                : f.PromotionStatus.Trim()
                        };

                        // 2. Lưu xuống SQL, lấy ra ID mới
                        int newId = _promotionService.AddPromotion(promotion);

                        // 3. Thêm vào DataTable để hiển thị
                        string timeRange = string.Format("{0:dd/MM/yyyy} - {1:dd/MM/yyyy}",
                                                         f.StartDate, f.EndDate);

                        _promotionsTable.Rows.Add(
    newId.ToString(),           // Id
    promotion.PromotionCode,    // Mã ưu đãi
    promotion.ProgramName,
    promotion.PromotionType,
    promotion.TargetAudience,
    timeRange,
    promotion.Status
);

                        HienThiLenGrid();
                        CapNhatThongKe();

                        MessageBox.Show("Đã thêm ưu đãi mới vào cơ sở dữ liệu.",
                            "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi lưu ưu đãi xuống SQL:\n" + ex.Message,
                            "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnDeletePromotion_Click(object sender, EventArgs e)
        {
            if (_promotionsTable == null) return;
            if (dgvListPromotion.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn một ưu đãi để xóa.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Lấy DataRow tương ứng với dòng đang chọn
            var rowView = dgvListPromotion.CurrentRow.DataBoundItem as DataRowView;
            if (rowView == null)
            {
                MessageBox.Show("Không thể lấy thông tin ưu đãi được chọn.",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DataRow row = rowView.Row;
string idStr = row.Field<string>("Id");

if (!int.TryParse(idStr, out int id))
{
    MessageBox.Show("Mã ưu đãi không hợp lệ, không thể xóa.",
        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
    return;
}

            var confirm = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa ưu đãi có mã {idStr}?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            try
            {
                // 1. Xóa trong SQL
                _promotionService.DeletePromotion(id);

                // 2. Xóa trong DataTable (UI)
                _promotionsTable.Rows.Remove(row);

                HienThiLenGrid();
                CapNhatThongKe();

                MessageBox.Show("Đã xóa ưu đãi khỏi cơ sở dữ liệu.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa ưu đãi trong SQL:\n" + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnFindPromotion_Click(object sender, EventArgs e)
        {
            if (_promotionsTable == null) return;

            string keyword = tbFindPromotion.Text.Trim();

            if (string.IsNullOrEmpty(keyword))
            {
                dgvListPromotion.DataSource = _promotionsTable;
                CapNhatThongKe();
                return;
            }

            var view = new DataView(_promotionsTable);
            string safeKeyword = keyword.Replace("'", "''");

            view.RowFilter =
                $"[Mã ưu đãi] LIKE '%{safeKeyword}%' OR [Tên chương trình] LIKE '%{safeKeyword}%'";
            
            dgvListPromotion.DataSource = view;

            // thống kê lại trên dữ liệu đã lọc
            CapNhatThongKe();
        }

        private void dgvListPromotion_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Nếu có thay đổi Đối tượng áp dụng / Trạng thái ngay trên grid thì:
            // UpdateTotalRunningPromotions();
            // UpdateTotalVipPromotions();
        }

        private void guna2Button8_Click(object sender, EventArgs e)
        {
            // Nạp lại từ SQL rồi hiển thị
            NapDuLieuUuDaiTuSql();
            HienThiLenGrid();
            CapNhatThongKe();
        }

        private void guna2HtmlLabel7_Click(object sender, EventArgs e)
        {

        }
    }
}
