using System;

namespace QLChuoiNhaHangKhachSan.GUI
{
    /// <summary>
    /// Lightweight pub/sub for cross-form refresh notifications.
    /// </summary>
    public static class NotificationCenter
    {
        public static event EventHandler InvoiceChanged;

        public static void RaiseInvoiceChanged()
        {
            try { InvoiceChanged?.Invoke(null, EventArgs.Empty); }
            catch { /* swallow to avoid UI crash */ }
        }
    }
}
