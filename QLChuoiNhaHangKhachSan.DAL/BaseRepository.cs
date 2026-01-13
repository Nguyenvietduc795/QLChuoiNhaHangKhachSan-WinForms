using System.Configuration;

namespace QLChuoiNhaHangKhachSan.DAL
{
    /// <summary>
    /// L?p c? s? cho các Repository, cung c?p chu?i k?t n?i CSDL
    /// </summary>
    public abstract class BaseRepository
    {
        protected readonly string _connectionString;

        protected BaseRepository()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["QuanLyChuoiNhaHangKhachSan"].ConnectionString;
        }

        protected BaseRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
    }
}
