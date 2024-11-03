using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Request
{
    public class RegisterRequest
    {
        public string Fullname { get; set; } = null!;
        public DateOnly BirthDay { get; set; }
        public string Phone { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
       
    }
}
