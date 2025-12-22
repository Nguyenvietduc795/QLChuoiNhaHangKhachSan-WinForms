using System;
using System.Drawing;
using System.Windows.Forms;

namespace QLChuoiNhaHangKhachSan.GUI
{
    public partial class SalaryManageForm : Form
    {
        private const int HoverShadow = 12;
        private const int NormalShadow = 5;
        private readonly Color HoverFill = Color.FromArgb(245, 248, 255);
        private readonly Padding _designPadding;

        public SalaryManageForm()
        {
            InitializeComponent();
            _designPadding = this.Padding; // remember designer padding
            AttachHover(pnlTotalPayroll);
            AttachHover(pnlTotalEmployees);
            AttachHover(pnlAvargeSalary);
        }

        private void AttachHover(Guna.UI2.WinForms.Guna2Panel panel)
        {
            panel.Tag = new PanelSnapshot(panel.FillColor, panel.ShadowDecoration.Shadow, panel.ShadowDecoration.Depth);
            panel.MouseEnter += Panel_MouseEnter;
            panel.MouseLeave += Panel_MouseLeave;
            foreach (Control child in panel.Controls)
            {
                child.MouseEnter += (s, e) => Panel_MouseEnter(panel, e);
                child.MouseLeave += (s, e) => Panel_MouseLeave(panel, e);
            }
        }

        private void Panel_MouseEnter(object sender, EventArgs e)
        {
            var panel = sender as Guna.UI2.WinForms.Guna2Panel;
            if (panel == null || panel.Tag == null) return;
            var snap = (PanelSnapshot)panel.Tag;
            panel.FillColor = HoverFill;
            panel.ShadowDecoration.Depth = HoverShadow;
            panel.ShadowDecoration.Shadow = new Padding(10);
        }

        private void Panel_MouseLeave(object sender, EventArgs e)
        {
            var panel = sender as Guna.UI2.WinForms.Guna2Panel;
            if (panel == null || panel.Tag == null) return;
            var snap = (PanelSnapshot)panel.Tag;
            panel.FillColor = snap.Fill;
            panel.ShadowDecoration.Depth = snap.ShadowDepth;
            panel.ShadowDecoration.Shadow = snap.ShadowPadding;
        }

        private struct PanelSnapshot
        {
            public Color Fill { get; }
            public Padding ShadowPadding { get; }
            public int ShadowDepth { get; }
            public PanelSnapshot(Color fill, Padding shadowPadding, int shadowDepth)
            {
                Fill = fill;
                ShadowPadding = shadowPadding;
                ShadowDepth = shadowDepth;
            }
        }

        // Tắt viền/đổ bóng khi nhúng vào FormDashBoard
        public void EnableEmbedMode()
        {
            if (bldSalaryManageForm != null)
            {
                bldSalaryManageForm.Dispose(); // remove borderless behavior
            }
            // keep the original padding so headers stay visible
            this.Padding = _designPadding;
        }
    }
}
