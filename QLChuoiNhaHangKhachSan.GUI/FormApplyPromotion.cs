using System;
using System.Drawing;
using System.Windows.Forms;
using QLChuoiNhaHangKhachSan.BLL;
using QLChuoiNhaHangKhachSan.BLL.DTOs;

namespace QLChuoiNhaHangKhachSan.GUI
{
    /// <summary>
    /// Form cho phép nhập mã giảm giá khi thanh toán hóa đơn nhà hàng.
    /// Kiểm tra mã có hợp lệ và còn hạn không, trả về thông tin giảm giá.
    /// </summary>
    public class FormApplyPromotion : Form
    {
        private Label lblTitle;
        private Label lblCode;
        private TextBox txtPromotionCode;
        private Label lblInfo;
        private Button btnApply;
        private Button btnSkip;
        private Button btnCancel;

        private readonly PromotionService _promotionService = new PromotionService();
        private readonly decimal _originalAmount;
        private readonly string _customerType;

        /// <summary>
        /// Mã giảm giá đã áp dụng (null nếu không áp dụng)
        /// </summary>
        public string AppliedPromotionCode { get; private set; }

        /// <summary>
        /// Phần trăm giảm giá (0-100)
        /// </summary>
        public decimal DiscountPercent { get; private set; }

        /// <summary>
        /// Số tiền được giảm
        /// </summary>
        public decimal DiscountAmount { get; private set; }

        /// <summary>
        /// Số tiền sau khi giảm
        /// </summary>
        public decimal FinalAmount { get; private set; }

        /// <summary>
        /// Tên chương trình khuyến mãi
        /// </summary>
        public string PromotionName { get; private set; }

        public FormApplyPromotion(decimal originalAmount, string customerType = null)
        {
            _originalAmount = originalAmount;
            _customerType = customerType;
            FinalAmount = originalAmount;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Áp dụng mã giảm giá";
            this.ClientSize = new Size(450, 280);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.White;
            this.Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);

            lblTitle = new Label
            {
                Text = "ÁP DỤNG MÃ GIẢM GIÁ",
                Font = new Font("Arial", 16F, FontStyle.Bold, GraphicsUnit.Point, 0),
                ForeColor = Color.FromArgb(3, 35, 140),
                AutoSize = false,
                Size = new Size(430, 40),
                Location = new Point(10, 15),
                TextAlign = ContentAlignment.MiddleCenter
            };

            lblCode = new Label
            {
                Text = "Nhập mã giảm giá:",
                Font = new Font("Arial", 11F, FontStyle.Regular, GraphicsUnit.Point, 0),
                Location = new Point(20, 70),
                AutoSize = true
            };

            txtPromotionCode = new TextBox
            {
                Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point, 0),
                Location = new Point(20, 95),
                Size = new Size(410, 30),
                Text = ""
            };
            // Placeholder workaround cho .NET Framework 4.8
            txtPromotionCode.GotFocus += (s, ev) => { if (txtPromotionCode.ForeColor == Color.Gray) { txtPromotionCode.Text = ""; txtPromotionCode.ForeColor = Color.Black; } };
            txtPromotionCode.LostFocus += (s, ev) => { if (string.IsNullOrWhiteSpace(txtPromotionCode.Text)) { txtPromotionCode.Text = "Nhập mã, ID hoặc tên chương trình (VD: Tet, 1)"; txtPromotionCode.ForeColor = Color.Gray; } };
            txtPromotionCode.Text = "Nhập mã, ID hoặc tên chương trình (VD: Tet, 1)";
            txtPromotionCode.ForeColor = Color.Gray;
            txtPromotionCode.KeyDown += TxtPromotionCode_KeyDown;

            lblInfo = new Label
            {
                Text = string.Format("Tổng tiền: {0:N0} đ", _originalAmount),
                Font = new Font("Arial", 10F, FontStyle.Regular, GraphicsUnit.Point, 0),
                ForeColor = Color.Gray,
                Location = new Point(20, 135),
                AutoSize = false,
                Size = new Size(410, 50)
            };

            btnApply = new Button
            {
                Text = "Áp dụng",
                Font = new Font("Arial", 11F, FontStyle.Bold, GraphicsUnit.Point, 0),
                Size = new Size(120, 40),
                Location = new Point(20, 195),
                BackColor = Color.FromArgb(22, 163, 74),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnApply.FlatAppearance.BorderSize = 0;
            btnApply.Click += BtnApply_Click;

            btnSkip = new Button
            {
                Text = "Bỏ qua",
                Font = new Font("Arial", 11F, FontStyle.Regular, GraphicsUnit.Point, 0),
                Size = new Size(120, 40),
                Location = new Point(160, 195),
                BackColor = Color.FromArgb(59, 130, 246),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnSkip.FlatAppearance.BorderSize = 0;
            btnSkip.Click += BtnSkip_Click;

            btnCancel = new Button
            {
                Text = "Hủy",
                Font = new Font("Arial", 11F, FontStyle.Regular, GraphicsUnit.Point, 0),
                Size = new Size(120, 40),
                Location = new Point(300, 195),
                BackColor = Color.Gray,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += BtnCancel_Click;

            this.Controls.Add(lblTitle);
            this.Controls.Add(lblCode);
            this.Controls.Add(txtPromotionCode);
            this.Controls.Add(lblInfo);
            this.Controls.Add(btnApply);
            this.Controls.Add(btnSkip);
            this.Controls.Add(btnCancel);
        }

        private void TxtPromotionCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                BtnApply_Click(sender, e);
            }
        }

        private void BtnApply_Click(object sender, EventArgs e)
        {
            string code = txtPromotionCode.Text.Trim();
            if (string.IsNullOrEmpty(code))
            {
                MessageBox.Show("Vui lòng nhập mã giảm giá.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPromotionCode.Focus();
                return;
            }

            try
            {
                // Validate và lấy thông tin promotion
                PromotionDto promo = _promotionService.ValidateAndGetPromotion(code, _customerType);

                if (promo == null)
                {
                    MessageBox.Show($"Không tìm thấy mã giảm giá, ID hoặc tên chương trình '{code}' trong hệ thống.", "Mã không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPromotionCode.SelectAll();
                    txtPromotionCode.Focus();
                    return;
                }

                // Tính toán giảm giá
                AppliedPromotionCode = promo.PromotionCode;
                PromotionName = promo.ProgramName;
                DiscountPercent = promo.DiscountPercent;

                if (DiscountPercent <= 0)
                {
                    // Nếu DiscountPercent chưa được set, thử parse từ PromotionType (VD: "10%")
                    if (!string.IsNullOrEmpty(promo.PromotionType))
                    {
                        var typeText = promo.PromotionType.Replace("%", "").Trim();
                        if (decimal.TryParse(typeText, out var parsedPercent))
                        {
                            DiscountPercent = parsedPercent;
                        }
                    }
                }

                DiscountAmount = _originalAmount * DiscountPercent / 100m;
                FinalAmount = _originalAmount - DiscountAmount;

                // Hiển thị thông báo thành công
                string message = $"Áp dụng mã '{AppliedPromotionCode}' thành công!\n\n" +
                                 $"Chương trình: {PromotionName}\n" +
                                 $"Giảm: {DiscountPercent}%\n" +
                                 $"Số tiền giảm: {DiscountAmount:N0} đ\n" +
                                 $"Tổng thanh toán: {FinalAmount:N0} đ";

                MessageBox.Show(message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (InvalidOperationException ex)
            {
                // Mã hết hạn hoặc không áp dụng được
                MessageBox.Show(ex.Message, "Mã giảm giá không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPromotionCode.SelectAll();
                txtPromotionCode.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi kiểm tra mã giảm giá: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSkip_Click(object sender, EventArgs e)
        {
            // Bỏ qua, không áp dụng mã giảm giá
            AppliedPromotionCode = null;
            DiscountPercent = 0;
            DiscountAmount = 0;
            FinalAmount = _originalAmount;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
