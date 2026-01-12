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
    public partial class FormDownload : Form
    {
        private readonly FormPayments _paymentsForm;

        // New ctor that accepts the FormPayments instance
        public FormDownload(FormPayments paymentsForm)
        {
            _paymentsForm = paymentsForm;
            InitializeComponent();
        }

        // Keep parameterless ctor for Designer compatibility (optional) but it won't have a paymentsForm
        public FormDownload() : this(null) { }

        private void FormDownload_Load(object sender, EventArgs e)
        {
            // Initialize date controls hidden by default (combobox was removed)
            lblFrom.Visible = true; // show date pickers so user can choose range
            lblTo.Visible = true;
            dtpFrom.Visible = true;
            dtpTo.Visible = true;

            // sensible default values
            dtpFrom.Value = DateTime.Today;
            dtpTo.Value = DateTime.Today;
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnTaiXuat_Click(object sender, EventArgs e)
        {
            if (dtpFrom.Value.Date > dtpTo.Value.Date)
            {
                MessageBox.Show("Ngày bắt đầu phải nhỏ hơn hoặc bằng ngày kết thúc!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_paymentsForm != null)
            {
                _paymentsForm.ExportTransactions(dtpFrom.Value.Date, dtpTo.Value.Date);
                this.DialogResult = DialogResult.OK;
                this.Close();
                return;
            }

            MessageBox.Show(
                $"Xuất báo cáo từ {dtpFrom.Value:dd/MM/yyyy HH:mm:ss} đến {dtpTo.Value:dd/MM/yyyy HH:mm:ss}\n(FormPayments không được cung cấp).",
                "Thông báo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        // keep stubs to match Designer wiring if any
        private void cbLoaiThoiGian_SelectedIndexChanged(object sender, EventArgs e) { }
        private void FormDownload_Load_1(object sender, EventArgs e) { }
        private void pnlshadowMain_Paint(object sender, PaintEventArgs e) { }

        private void lblTitle_Click(object sender, EventArgs e)
        {
            if (lblTitle != null)
            {
                lblTitle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
                lblTitle.ForeColor = Color.Black; // Đảm bảo màu đen kịt để không bị mờ
            }
        }
    }
}
