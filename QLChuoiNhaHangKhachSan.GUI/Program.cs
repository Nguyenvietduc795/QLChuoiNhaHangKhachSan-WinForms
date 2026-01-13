using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLChuoiNhaHangKhachSan.GUI
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            // Bật xử lý ngoại lệ bị corrupt state (AccessViolationException)
            try
            {
                // Connection string được đọc trực tiếp từ App.config trong từng Repository
                // Không cần thiết lập ở đây nữa

                // Thiết lập DPI awareness trước khi khởi tạo form
                if (Environment.OSVersion.Version.Major >= 6)
                {
                    SetProcessDPIAware();
                }

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                // Thêm xử lý ngoại lệ toàn cục
                Application.ThreadException += Application_ThreadException;
                AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

                // Khởi động ứng dụng với LoginForm
                Application.Run(new LoginForm());
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khởi động ứng dụng: {ex.Message}\n\nChi tiết: {ex.StackTrace}", 
                    "Lỗi nghiêm trọng", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            MessageBox.Show($"Lỗi ứng dụng: {e.Exception.Message}\n\nChi tiết: {e.Exception.StackTrace}",
                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;
            MessageBox.Show($"Lỗi không xử lý được: {ex?.Message}\n\nChi tiết: {ex?.StackTrace}",
                "Lỗi nghiêm trọng", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
