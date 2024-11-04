using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.ViewModel
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public string Email { get; set; } = null!;
        public string? Role { get; set; }
    }
}
