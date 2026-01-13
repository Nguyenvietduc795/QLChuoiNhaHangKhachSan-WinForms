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
    public partial class FormRevenue : Form
    {
        public FormRevenue()
        {
            InitializeComponent();
            this.Load += FormRevenue_Load;
        }

        private void FormRevenue_Load(object sender, EventArgs e)
        {
            ConfigureDataGridView();
            LoadSampleData();
            UpdateChartData();
            AdjustDataGridViewHeight();
        }

        private void ConfigureDataGridView()
        {
            // Đổi tên header thành tiếng Anh như mẫu
            dgvRevenue.Columns["colCategory"].HeaderText = "Danh mục";
            dgvRevenue.Columns["colRevenue"].HeaderText = "Doanh thu";
            dgvRevenue.Columns["colPercentage"].HeaderText = "Phần trăm";

            // Tùy chỉnh style cho header
            dgvRevenue.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(15, 23, 42);
            dgvRevenue.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvRevenue.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            dgvRevenue.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvRevenue.ColumnHeadersDefaultCellStyle.Padding = new Padding(15, 10, 10, 10);
            dgvRevenue.ColumnHeadersHeight = 50;

            // Tùy chỉnh style cho các cell
            dgvRevenue.DefaultCellStyle.BackColor = Color.FromArgb(15, 23, 42);
            dgvRevenue.DefaultCellStyle.ForeColor = Color.White;
            dgvRevenue.DefaultCellStyle.Font = new Font("Segoe UI", 10.5F, FontStyle.Regular);
            dgvRevenue.DefaultCellStyle.SelectionBackColor = Color.FromArgb(15, 23, 42); // Giống màu nền
            dgvRevenue.DefaultCellStyle.SelectionForeColor = Color.White; // Giống màu chữ
            dgvRevenue.DefaultCellStyle.Padding = new Padding(15, 10, 10, 10);

            // Loại bỏ viền giữa các cell
            dgvRevenue.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvRevenue.GridColor = Color.FromArgb(30, 41, 59);

            // Tắt hoàn toàn selection và click
            dgvRevenue.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRevenue.MultiSelect = false;
            dgvRevenue.ReadOnly = true;
            dgvRevenue.AllowUserToAddRows = false;
            dgvRevenue.AllowUserToDeleteRows = false;
            dgvRevenue.AllowUserToResizeRows = false;
            dgvRevenue.AllowUserToResizeColumns = false;
            dgvRevenue.RowHeadersVisible = false;
            dgvRevenue.EnableHeadersVisualStyles = false;

            // Tắt scrollbar
            dgvRevenue.ScrollBars = ScrollBars.None;

            // Tắt highlight khi click
            dgvRevenue.CellClick += (s, e) => dgvRevenue.ClearSelection();
            dgvRevenue.CellMouseEnter += (s, e) => dgvRevenue.ClearSelection();
            dgvRevenue.SelectionChanged += (s, e) => dgvRevenue.ClearSelection();

            // Tăng chiều cao dòng
            dgvRevenue.RowTemplate.Height = 50;
            dgvRevenue.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;

            // Đặt width cho các cột theo tỷ lệ
            int totalWidth = dgvRevenue.Width - 60;
            dgvRevenue.Columns["colCategory"].Width = (int)(totalWidth * 0.40);
            dgvRevenue.Columns["colRevenue"].Width = (int)(totalWidth * 0.30);
            dgvRevenue.Columns["colPercentage"].Width = (int)(totalWidth * 0.30);
        }

        private void LoadSampleData()
        {
            // Xóa dữ liệu cũ
            dgvRevenue.Rows.Clear();

            // Thêm dữ liệu mẫu
            int row1 = dgvRevenue.Rows.Add("Nhà hàng", "1.000.000.000 VND", "70.8%");
            int row2 = dgvRevenue.Rows.Add("Khách sạn", "290.000.000 VND", "29.2%");
            int totalRowIndex = dgvRevenue.Rows.Add("Tổng cộng", "1.290.000.000 VND", "100%");

            // Style cho dòng "Restaurant"
            DataGridViewRow restaurantRow = dgvRevenue.Rows[row1];
            restaurantRow.DefaultCellStyle.BackColor = Color.FromArgb(15, 23, 42);
            restaurantRow.DefaultCellStyle.ForeColor = Color.White;
            restaurantRow.DefaultCellStyle.SelectionBackColor = Color.FromArgb(15, 23, 42);
            restaurantRow.DefaultCellStyle.SelectionForeColor = Color.White;

            // Style cho dòng "Hotel"
            DataGridViewRow hotelRow = dgvRevenue.Rows[row2];
            hotelRow.DefaultCellStyle.BackColor = Color.FromArgb(15, 23, 42);
            hotelRow.DefaultCellStyle.ForeColor = Color.White;
            hotelRow.DefaultCellStyle.SelectionBackColor = Color.FromArgb(15, 23, 42);
            hotelRow.DefaultCellStyle.SelectionForeColor = Color.White;

            // Tô đậm và đổi màu nền cho dòng Total
            DataGridViewRow totalRow = dgvRevenue.Rows[totalRowIndex];
            totalRow.DefaultCellStyle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            totalRow.DefaultCellStyle.BackColor = Color.FromArgb(20, 30, 48);
            totalRow.DefaultCellStyle.ForeColor = Color.White;
            totalRow.DefaultCellStyle.SelectionBackColor = Color.FromArgb(20, 30, 48);
            totalRow.DefaultCellStyle.SelectionForeColor = Color.White;

            // Căn chỉnh cột
            dgvRevenue.Columns["colCategory"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvRevenue.Columns["colRevenue"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvRevenue.Columns["colPercentage"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            // Vô hiệu hóa selection
            dgvRevenue.ClearSelection();
        }

        private void AdjustDataGridViewHeight()
        {
            // Tính chiều cao cần thiết: header + 3 rows
            int headerHeight = dgvRevenue.ColumnHeadersHeight;
            int rowHeight = dgvRevenue.RowTemplate.Height;
            int rowCount = dgvRevenue.Rows.Count;

            // Tổng chiều cao = header + (số dòng × chiều cao mỗi dòng) + padding
            int totalHeight = headerHeight + (rowCount * rowHeight) + 5;

            // Đặt chiều cao cho DataGridView
            dgvRevenue.Height = totalHeight;
        }

        private void UpdateChartData()
        {
            // Xóa dữ liệu cũ trong chart
            chartRevenue.Series.Clear();

            // Tạo một series duy nhất với 2 data points
            var series = new System.Windows.Forms.DataVisualization.Charting.Series("Guest Distribution");
            series.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;

            // Thêm data point cho Nhà hàng (màu xanh dương)
            var pointRestaurant = series.Points.Add(1000);
            pointRestaurant.Color = Color.FromArgb(59, 130, 246);
            pointRestaurant.AxisLabel = "Nhà hàng";
            pointRestaurant.Label = "1.000.0000.000 VND";
            pointRestaurant.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            pointRestaurant.LabelForeColor = Color.White;

            // Thêm data point cho Khách sạn (màu xanh lá)
            var pointHotel = series.Points.Add(290);
            pointHotel.Color = Color.FromArgb(52, 211, 153);
            pointHotel.AxisLabel = "Khách sạn";
            pointHotel.Label = "290.000.000 VND";
            pointHotel.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            pointHotel.LabelForeColor = Color.White;

            chartRevenue.Series.Add(series);

            // Tùy chỉnh chart area
            var chartArea = chartRevenue.ChartAreas[0];
            chartArea.BackColor = Color.FromArgb(30, 41, 59);

            // Tùy chỉnh trục X
            chartArea.AxisX.MajorGrid.Enabled = false;
            chartArea.AxisX.LineColor = Color.Transparent;
            chartArea.AxisX.LabelStyle.ForeColor = Color.White;
            chartArea.AxisX.LabelStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            chartArea.AxisX.MajorTickMark.Enabled = false;

            // Tùy chỉnh trục Y
            chartArea.AxisY.MajorGrid.Enabled = false;
            chartArea.AxisY.LineColor = Color.Transparent;
            chartArea.AxisY.LabelStyle.Enabled = false;
            chartArea.AxisY.MajorTickMark.Enabled = false;

            // Đặt tiêu đề
            if (chartRevenue.Titles.Count == 0)
            {
                chartRevenue.Titles.Add("Phân bố doanh thu");
            }
            chartRevenue.Titles[0].Text = "Phân bố doanh thu";
            chartRevenue.Titles[0].Font = new Font("Segoe UI", 13, FontStyle.Bold);
            chartRevenue.Titles[0].ForeColor = Color.White;
            chartRevenue.Titles[0].Alignment = ContentAlignment.MiddleLeft;
            chartRevenue.Titles[0].Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Top;

            // Đặt nền cho toàn bộ chart
            chartRevenue.BackColor = Color.FromArgb(30, 41, 59);

            // Tắt legend mặc định
            chartRevenue.Legends.Clear();

            // Tăng kích thước cột
            series["PointWidth"] = "0.7";
        }

        private void dgvRevenue_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
