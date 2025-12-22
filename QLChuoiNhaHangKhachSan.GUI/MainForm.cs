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

            // gán event cho nút Khách hàng
            this.btnCustomers.Click += BtnCustomers_Click;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string connStr = ConfigurationManager
                                .ConnectionStrings["ConnStr"]
                                .ConnectionString;

            MessageBox.Show(connStr);
        }

        private void BtnCustomers_Click(object sender, EventArgs e)
        {
            var f = new CustomersList();
            f.StartPosition = FormStartPosition.CenterScreen;
            f.Show(); // hoặc f.ShowDialog(this) nếu muốn modal
        }
    }
}
