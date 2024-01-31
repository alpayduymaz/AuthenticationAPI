using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Systems
{
    public class Login
    {
        [MaxLength(150)]
        public string Email { get; set; }

        [MaxLength(150)]
        public string Password { get; set; }
    }

    public class ResponseLogin
    {
        public string Token { get; set; }
        public LoginUser LoginUser { get; set; }
    }

    public class LoginUser
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Image { get; set; }
        public string FirstFireLink { get; set; }
        public int WorkYear { get; set; }
        public string IpAddress { get; set; }
        public string HostName { get; set; }
        public int? SicilId { get; set; }

        public int? UserRolId { get; set; }
        public string Email { get; set; }
    }
}
