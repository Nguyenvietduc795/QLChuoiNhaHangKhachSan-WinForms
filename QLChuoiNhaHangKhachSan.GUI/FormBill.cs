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
        private PrintDocument _printDoc;

        public frmBill()
        {
            InitializeComponent();

            // init print document
            _printDoc = new PrintDocument();
            _printDoc.PrintPage += PrintDoc_PrintPage;

            // wire print button (designer control: guna2Button1)
            try
            {
                this.guna2Button1.Click -= Guna2Button1_Click;
                this.guna2Button1.Click += Guna2Button1_Click;
            }
            catch
            {
                // ignore if control missing in designer mismatch
            }
        }

        /// <summary>
        /// Populate the bill UI from invoice data and line items.
        /// items: List of tuples (name, quantity, unitPrice, total)
        /// </summary>
        public void PopulateFromInvoice(string invoiceId, string customer, string dateText, string totalText, string status, string paymentMethod, List<Tuple<string, int, decimal, decimal>> items)
        {
            // Map simple fields to textboxes used in the Designer
            // Designer mapping (best-effort):
            // guna2TextBox1 -> invoice id or company? We'll set customer/invoice fields sensibly.
            try { this.guna2TextBox1.Text = invoiceId; } catch { }
            try { this.guna2TextBox2.Text = customer; } catch { }
            try { this.guna2TextBox3.Text = dateText; } catch { }
            try { this.guna2TextBox4.Text = paymentMethod; } catch { }

            // Clear grid and fill items
            try
            {
                this.guna2DataGridView1.Rows.Clear();
                if (items != null)
                {
                    foreach (var it in items)
                    {
                        int idx = this.guna2DataGridView1.Rows.Add();
                        var row = this.guna2DataGridView1.Rows[idx];
                        // columns in Designer: colName, colSL, colDonGia, colThanhTien
                        row.Cells["colName"].Value = it.Item1;
                        row.Cells["colSL"].Value = it.Item2.ToString();
                        row.Cells["colDonGia"].Value = it.Item3.ToString("N0");
                        row.Cells["colThanhTien"].Value = it.Item4.ToString("N0");
                    }
                }
            }
            catch { /* ignore designer mismatch */ }

            // Set totals (guna2TextBox5: total, guna2TextBox6: received, guna2TextBox7: change)
            try { this.guna2TextBox5.Text = totalText; } catch { }
            // received/change left empty: caller may set if available
            try { this.guna2TextBox6.Text = ""; } catch { }
            try { this.guna2TextBox7.Text = ""; } catch { }

            // Optionally set header labels (if any)
            try
            {
                // if there are labels to set invoice id/title, set them (designer may have different names)
            }
            catch { }
        }

        private void Guna2Button1_Click(object sender, EventArgs e)
        {
            // Show print preview using internal PrintDocument
            using (var preview = new PrintPreviewDialog())
            {
                preview.Document = _printDoc;
                // size/position preview reasonably
                preview.Width = 1000;
                preview.Height = 800;
                try
                {
                    preview.ShowDialog(this);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi mở xem trước in: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void PrintDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            // Render the bill panel to a bitmap and draw it on the print page.
            // This keeps layout faithful to Designer.
            if (this.pnlBill == null)
            {
                e.HasMorePages = false;
                return;
            }

            try
            {
                // scale panel to printable width while keeping aspect ratio
                var panel = this.pnlBill;
                using (var bmp = new Bitmap(panel.Width, panel.Height))
                {
                    panel.DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));

                    // Calculate scaling to fit within margin bounds
                    var margin = e.MarginBounds;
                    float scale = Math.Min((float)margin.Width / bmp.Width, (float)margin.Height / bmp.Height);

                    int drawW = (int)(bmp.Width * scale);
                    int drawH = (int)(bmp.Height * scale);
                    int drawX = margin.Left + (margin.Width - drawW) / 2;
                    int drawY = margin.Top + (margin.Height - drawH) / 2;

                    e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    e.Graphics.DrawImage(bmp, new Rectangle(drawX, drawY, drawW, drawH));
                }
            }
            catch (Exception ex)
            {
                // fallback: render a simple text if DrawToBitmap fails
                var font = new Font("Segoe UI", 12);
                e.Graphics.DrawString("Unable to render bill preview: " + ex.Message, font, Brushes.Black, e.MarginBounds.Left, e.MarginBounds.Top);
            }

            e.HasMorePages = false;
        }

        // Added: handler referenced by the Designer (this.Load += this.frmBill_Load)
        private void frmBill_Load(object sender, EventArgs e)
        {
            // Optional initialization for the bill form.
            // Keep empty if no startup work is needed.
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            DialogResult = MessageBox.Show(
                "Bạn có chắc muốn đóng hóa đơn không?",
                "Xác nhận",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (DialogResult == DialogResult.OK)
            {
                this.Close();
            }
        }

    }
}