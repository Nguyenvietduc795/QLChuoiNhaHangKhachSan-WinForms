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
    public partial class FormUpdateItems : Form
    {
        public FormUpdateItems()
        {
            InitializeComponent();
            this.Load += FormUpdateItems_Load;
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterParent;

            // Wire close/cancel buttons
            this.BtnExistUpdateItems.Click += BtnClose_Click;
            this.btnCancelUpdateItems.Click += BtnClose_Click;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void FormUpdateItems_Load(object sender, EventArgs e)
        {
            InitializePlaceholders();
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
