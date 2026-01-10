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
    public partial class FormStopItems : Form
    {
        public FormStopItems()
        {
            InitializeComponent();
            InitializePlaceholders();

            // Wire close/cancel buttons
            this.BtnExistUpdateItems.Click += BtnClose_Click;
            this.btnCancelStopItems.Click += BtnClose_Click;
        }

        // New constructor to accept a code to prefill
        public FormStopItems(string code) : this()
        {
            if (!string.IsNullOrWhiteSpace(code))
            {
                try
                {
                    txMaHangStopItems.Text = code;
                    txMaHangStopItems.ForeColor = System.Drawing.Color.Black;
                }
                catch { }
            }
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
