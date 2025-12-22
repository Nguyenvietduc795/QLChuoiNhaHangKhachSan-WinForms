using System;
using System.Linq;
using System.Windows.Forms;

namespace QLChuoiNhaHangKhachSan.GUI
{
    public partial class MainForm : Form
    {
        private Form activeChildForm = null;

        public MainForm()
        {
            InitializeComponent();
            this.Load += MainForm_Load;
        }

        private void openChildForm(Form childForm)
        {
            // Close previously opened child
            if (activeChildForm != null && !activeChildForm.IsDisposed)
            {
                activeChildForm.Close();
            }

            activeChildForm = childForm;

            // Prepare child form to act as a control
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;

            // Add to a container if one exists; otherwise add directly to the MainForm
            Control host = this.Controls.Cast<Control>().FirstOrDefault(c => c.Name.Equals("pnlContainer", StringComparison.OrdinalIgnoreCase))
                           ?? this.Controls.Cast<Control>().FirstOrDefault(c => c.Name.Equals("panelDesktop", StringComparison.OrdinalIgnoreCase))
                           ?? this; // fallback to the form itself

            host.Controls.Add(childForm);
            childForm.BringToFront();
            childForm.Show();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Show the dashboard when MainForm opens
            openChildForm(new FormDashBoard());
        }
    }
}
