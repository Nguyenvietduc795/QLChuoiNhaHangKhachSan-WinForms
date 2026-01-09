using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLChuoiNhaHangKhachSan.GUI
{
    public partial class frmBill : Form
    {
        private readonly PrintDocument _printDocument;
        private Bitmap _billBitmap;

        public frmBill()
        {
            InitializeComponent();

            _printDocument = new PrintDocument();
            _printDocument.PrintPage += PrintDocument_PrintPage;

            guna2Button1.Click += guna2Button1_Click; // In hóa đơn
        }

        private void guna2TextBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2HtmlLabel2_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            try
            {
                CaptureBillPanel();

                using (var printDialog = new PrintDialog())
                {
                    printDialog.Document = _printDocument;
                    if (printDialog.ShowDialog(this) == DialogResult.OK)
                    {
                        _printDocument.Print();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể in hóa đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CaptureBillPanel()
        {
            _billBitmap?.Dispose();
            _billBitmap = new Bitmap(pnlBill.Width, pnlBill.Height);
            pnlBill.DrawToBitmap(_billBitmap, new Rectangle(0, 0, pnlBill.Width, pnlBill.Height));
        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            if (_billBitmap == null)
            {
                CaptureBillPanel();
            }

            if (_billBitmap != null)
            {
                e.Graphics.DrawImage(_billBitmap, new Point(0, 0));
            }
        }
    }
}
