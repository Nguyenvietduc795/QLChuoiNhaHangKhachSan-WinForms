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
    public partial class CustomersReport : Form
    {
        public CustomersReport()
        {
            InitializeComponent();
            this.Load += CustomersReport_Load;
        }
        private void CustomersReport_Load(object sender, EventArgs e)
        {
            ConfigureDataGridView();
            LoadSampleData();
            UpdateChartData();
            AdjustDataGridViewHeight();
        }

        private void ConfigureDataGridView()
        {
            // Đổi tên header thành tiếng Anh như mẫu
            dgvGuestReport.Columns["colCategory"].HeaderText = "Danh mục";
            dgvGuestReport.Columns["colGuest"].HeaderText = "Khách hàng";
            dgvGuestReport.Columns["colPercentage"].HeaderText = "Phần trăm";

            // Tùy chỉnh style cho header
            dgvGuestReport.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(15, 23, 42);
            dgvGuestReport.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvGuestReport.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            dgvGuestReport.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvGuestReport.ColumnHeadersDefaultCellStyle.Padding = new Padding(15, 10, 10, 10);
            dgvGuestReport.ColumnHeadersHeight = 50;

            // Tùy chỉnh style cho các cell
            dgvGuestReport.DefaultCellStyle.BackColor = Color.FromArgb(15, 23, 42);
            dgvGuestReport.DefaultCellStyle.ForeColor = Color.White;
            dgvGuestReport.DefaultCellStyle.Font = new Font("Segoe UI", 10.5F, FontStyle.Regular);
            dgvGuestReport.DefaultCellStyle.SelectionBackColor = Color.FromArgb(15, 23, 42); // Giống màu nền
            dgvGuestReport.DefaultCellStyle.SelectionForeColor = Color.White; // Giống màu chữ
            dgvGuestReport.DefaultCellStyle.Padding = new Padding(15, 10, 10, 10);

            // Loại bỏ viền giữa các cell
            dgvGuestReport.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvGuestReport.GridColor = Color.FromArgb(30, 41, 59);

            // Tắt hoàn toàn selection và click
            dgvGuestReport.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvGuestReport.MultiSelect = false;
            dgvGuestReport.ReadOnly = true;
            dgvGuestReport.AllowUserToAddRows = false;
            dgvGuestReport.AllowUserToDeleteRows = false;
            dgvGuestReport.AllowUserToResizeRows = false;
            dgvGuestReport.AllowUserToResizeColumns = false;
            dgvGuestReport.RowHeadersVisible = false;
            dgvGuestReport.EnableHeadersVisualStyles = false;

            // Tắt scrollbar
            dgvGuestReport.ScrollBars = ScrollBars.None;

            // Tắt highlight khi click
            dgvGuestReport.CellClick += (s, e) => dgvGuestReport.ClearSelection();
            dgvGuestReport.CellMouseEnter += (s, e) => dgvGuestReport.ClearSelection();
            dgvGuestReport.SelectionChanged += (s, e) => dgvGuestReport.ClearSelection();

            // Tăng chiều cao dòng
            dgvGuestReport.RowTemplate.Height = 50;
            dgvGuestReport.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;

            // Đặt width cho các cột theo tỷ lệ
            int totalWidth = dgvGuestReport.Width - 60;
            dgvGuestReport.Columns["colCategory"].Width = (int)(totalWidth * 0.40);
            dgvGuestReport.Columns["colGuest"].Width = (int)(totalWidth * 0.30);
            dgvGuestReport.Columns["colPercentage"].Width = (int)(totalWidth * 0.30);
        }

        private void LoadSampleData()
        {
            // Xóa dữ liệu cũ
            dgvGuestReport.Rows.Clear();

            // Thêm dữ liệu mẫu
            int row1 = dgvGuestReport.Rows.Add("Nhà hàng", "1.000", "70.8%");
            int row2 = dgvGuestReport.Rows.Add("Khách sạn", "290", "29.2%");
            int totalRowIndex = dgvGuestReport.Rows.Add("Tổng cộng", "1.290", "100%");

            // Style cho dòng "Restaurant"
            DataGridViewRow restaurantRow = dgvGuestReport.Rows[row1];
            restaurantRow.DefaultCellStyle.BackColor = Color.FromArgb(15, 23, 42);
            restaurantRow.DefaultCellStyle.ForeColor = Color.White;
            restaurantRow.DefaultCellStyle.SelectionBackColor = Color.FromArgb(15, 23, 42);
            restaurantRow.DefaultCellStyle.SelectionForeColor = Color.White;

            // Style cho dòng "Hotel"
            DataGridViewRow hotelRow = dgvGuestReport.Rows[row2];
            hotelRow.DefaultCellStyle.BackColor = Color.FromArgb(15, 23, 42);
            hotelRow.DefaultCellStyle.ForeColor = Color.White;
            hotelRow.DefaultCellStyle.SelectionBackColor = Color.FromArgb(15, 23, 42);
            hotelRow.DefaultCellStyle.SelectionForeColor = Color.White;

            // Tô đậm và đổi màu nền cho dòng Total
            DataGridViewRow totalRow = dgvGuestReport.Rows[totalRowIndex];
            totalRow.DefaultCellStyle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            totalRow.DefaultCellStyle.BackColor = Color.FromArgb(20, 30, 48);
            totalRow.DefaultCellStyle.ForeColor = Color.White;
            totalRow.DefaultCellStyle.SelectionBackColor = Color.FromArgb(20, 30, 48);
            totalRow.DefaultCellStyle.SelectionForeColor = Color.White;

            // Căn chỉnh cột
            dgvGuestReport.Columns["colCategory"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvGuestReport.Columns["colGuest"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvGuestReport.Columns["colPercentage"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            // Vô hiệu hóa selection
            dgvGuestReport.ClearSelection();
        }

        private void AdjustDataGridViewHeight()
        {
            // Tính chiều cao cần thiết: header + 3 rows
            int headerHeight = dgvGuestReport.ColumnHeadersHeight;
            int rowHeight = dgvGuestReport.RowTemplate.Height;
            int rowCount = dgvGuestReport.Rows.Count;

            // Tổng chiều cao = header + (số dòng × chiều cao mỗi dòng) + padding
            int totalHeight = headerHeight + (rowCount * rowHeight) + 5;

            // Đặt chiều cao cho DataGridView
            dgvGuestReport.Height = totalHeight;
        }

        private void UpdateChartData()
        {
            // Xóa dữ liệu cũ trong chart
            chartGuestReport.Series.Clear();
            
            // Tạo một series duy nhất với 2 data points
            var series = new System.Windows.Forms.DataVisualization.Charting.Series("Guest Distribution");
            series.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
            
            // Thêm data point cho Nhà hàng (màu xanh dương)
            var pointRestaurant = series.Points.Add(1000);
            pointRestaurant.Color = Color.FromArgb(59, 130, 246);
            pointRestaurant.AxisLabel = "Nhà hàng";
            pointRestaurant.Label = "1.000";
            pointRestaurant.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            pointRestaurant.LabelForeColor = Color.White;
            
            // Thêm data point cho Khách sạn (màu xanh lá)
            var pointHotel = series.Points.Add(290);
            pointHotel.Color = Color.FromArgb(52, 211, 153);
            pointHotel.AxisLabel = "Khách sạn";
            pointHotel.Label = "290";
            pointHotel.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            pointHotel.LabelForeColor = Color.White;
            
            chartGuestReport.Series.Add(series);
            
            // Tùy chỉnh chart area
            var chartArea = chartGuestReport.ChartAreas[0];
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
            if (chartGuestReport.Titles.Count == 0)
            {
                chartGuestReport.Titles.Add("Phân bố khách hàng");
            }
            chartGuestReport.Titles[0].Text = "Phân bố khách hàng";
            chartGuestReport.Titles[0].Font = new Font("Segoe UI", 13, FontStyle.Bold);
            chartGuestReport.Titles[0].ForeColor = Color.White;
            chartGuestReport.Titles[0].Alignment = ContentAlignment.MiddleLeft;
            chartGuestReport.Titles[0].Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Top;
            
            // Đặt nền cho toàn bộ chart
            chartGuestReport.BackColor = Color.FromArgb(30, 41, 59);
            
            // Tắt legend mặc định
            chartGuestReport.Legends.Clear();
            
            // Tăng kích thước cột
            series["PointWidth"] = "0.7";
        }
    }
}
