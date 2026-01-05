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
    public partial class VipCustomers : Form
    {
        public VipCustomers()
        {
            InitializeComponent();

            // Gắn sự kiện cho nút Thêm nếu chưa gắn trong Designer
            this.bntAddVIP.Click += bntAddVIP_Click;
            // Gắn sự kiện cho nút Xóa và Cập nhật
            this.bntDelectVIP.Click += bntDelectVIP_Click;
            this.bntUpdateVIP.Click += bntUpdateVIP_Click;
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {

        }

        // Mở form thêm thành viên VIP
        private void bntAddVIP_Click(object sender, EventArgs e)
        {
            using (var frm = new AddVipCustomers(this))
            {
                frm.ShowDialog();
            }
        }

        // Hàm để AddVipCustomers đẩy dữ liệu vào lưới
        public void AddVipRow(string id, string rank, string point, DateTime dateStart, DateTime dateEnd)
        {
            dgvListVIP.Rows.Add(id, rank, point, dateStart.ToShortDateString(), dateEnd.ToShortDateString());
        }

        // Xóa thành viên đang chọn
        private void bntDelectVIP_Click(object sender, EventArgs e)
        {
            if (dgvListVIP.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một thành viên cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var result = MessageBox.Show("Bạn có chắc chắn muốn xóa thành viên này không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                foreach (DataGridViewRow row in dgvListVIP.SelectedRows)
                {
                    if (!row.IsNewRow)
                    {
                        dgvListVIP.Rows.Remove(row);
                    }
                }
            }
        }

        // Cập nhật thành viên đang chọn
        private void bntUpdateVIP_Click(object sender, EventArgs e)
        {
            if (dgvListVIP.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một thành viên cần cập nhật.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var row = dgvListVIP.SelectedRows[0];
            if (row.IsNewRow)
            {
                return;
            }

            // Lấy dữ liệu hiện tại
            string id = Convert.ToString(row.Cells["IDVIP"].Value);
            string rank = Convert.ToString(row.Cells["RankVIP"].Value);
            string point = Convert.ToString(row.Cells["PointVIP"].Value);
            DateTime dateStart;
            DateTime.TryParse(Convert.ToString(row.Cells["DateStart"].Value), out dateStart);
            DateTime dateEnd;
            DateTime.TryParse(Convert.ToString(row.Cells["DateEnd"].Value), out dateEnd);

            using (var frm = new AddVipCustomers(this))
            {
                // Truyền dữ liệu sang form cập nhật
                frm.SetEditData(id, rank, point, dateStart, dateEnd, row.Index);
                frm.ShowDialog();
            }
        }

        // Hàm để cập nhật lại một dòng trong dgvListVIP (dùng khi form cập nhật lưu lại)
        public void UpdateVipRow(int rowIndex, string rank, string point, DateTime dateStart, DateTime dateEnd)
        {
            if (rowIndex < 0 || rowIndex >= dgvListVIP.Rows.Count)
                return;

            var row = dgvListVIP.Rows[rowIndex];
            if (row.IsNewRow)
                return;

            row.Cells["RankVIP"].Value = rank;
            row.Cells["PointVIP"].Value = point;
            row.Cells["DateStart"].Value = dateStart.ToShortDateString();
            row.Cells["DateEnd"].Value = dateEnd.ToShortDateString();
        }
    }
}
