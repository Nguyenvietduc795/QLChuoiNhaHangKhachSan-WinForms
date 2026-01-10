using System.Configuration;

namespace QLChuoiNhaHangKhachSan.GUI
{
    public static class AppSession
    {
        public static int CurrentAreaID
        {
            get
            {
                int.TryParse(
                    ConfigurationManager.AppSettings["CurrentAreaID"],
                    out int id
                );
                return id == 0 ? 1 : id;
            }
        }
    }
}
