using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Shared
{
    public class Token
    {
        public int UserId { get; set; }
        public int[] UserRoleId { get; set; }
        public string FullName { get; set; }
    }
}
