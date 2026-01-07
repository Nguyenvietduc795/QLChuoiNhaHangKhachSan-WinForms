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
            public string PasswordHash { get; set; }
            public string Status { get; set; }
            public DateTime DateCreated { get; set; }
        }
    
}
