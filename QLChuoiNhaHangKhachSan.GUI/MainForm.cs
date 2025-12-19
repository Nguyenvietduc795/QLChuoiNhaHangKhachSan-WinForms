using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace QLChuoiNhaHangKhachSan.GUI
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string connStr = ConfigurationManager
                                .ConnectionStrings["ConnStr"]
                                .ConnectionString;

            MessageBox.Show(connStr);
        }

        private void btnKho_Click(object sender, EventArgs e)
        {
 
        }
    }
}
