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
    public partial class FormAddMatHang : Form
    {
        public FormAddMatHang()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterParent;

            // Ensure default text behavior is set: set Tag values already in designer
            // Initialize fields with Tag values if empty
            InitializePlaceholders();

            // Wire the top-right X button to cancel/close
            this.guna2Button1.Click += BtnClose_Click;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void InitializePlaceholders()
        {
            foreach (Control c in pnlCardaddItems.Controls)
            {
                if (c is Guna.UI2.WinForms.Guna2TextBox tb)
                {
                    if (tb.Tag is string tag && string.IsNullOrEmpty(tb.Text))
                    {
                        tb.Text = tag;
                        tb.ForeColor = System.Drawing.Color.Gray;
                    }
                }
            }
        }

        private void lblTotal_Click(object sender, EventArgs e)
        {

        }

        private void txTimkiem_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2TextBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void cboDonVi2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnCancelAddItems_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnSaveAddItems_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void FormAddMatHang_Load(object sender, EventArgs e)
        {

        }

        // Clear default placeholder on enter
        private void TextBox_Enter_ClearDefault(object sender, EventArgs e)
        {
            if (sender is Guna.UI2.WinForms.Guna2TextBox tb)
            {
                if (tb.Tag is string tag && tb.Text == tag)
                {
                    tb.Text = string.Empty;
                    tb.ForeColor = System.Drawing.Color.Black;
                }
            }
        }

        // Restore placeholder if left empty
        private void TextBox_Leave_RestoreDefault(object sender, EventArgs e)
        {
            if (sender is Guna.UI2.WinForms.Guna2TextBox tb)
            {
                if (tb.Tag is string tag && string.IsNullOrEmpty(tb.Text))
                {
                    tb.Text = tag;
                    tb.ForeColor = System.Drawing.Color.Gray;
                }
            }
        }
    }
}
