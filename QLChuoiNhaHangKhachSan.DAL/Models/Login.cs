using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLChuoiNhaHangKhachSan.DAL.Models
{

        public class Login
        {
            public int LoginId { get; set; }
            public string Username { get; set; }
            public byte[] PasswordHash { get; set; }
            public byte[] PasswordSalt { get; set; }
            public string Status { get; set; }
            public DateTime DateCreated { get; set; }
            /// <summary>
            /// Cờ đánh dấu người dùng phải đổi mật khẩu khi đăng nhập lần đầu
            /// </summary>
            public bool MustChangePassword { get; set; }
        }
    
}
