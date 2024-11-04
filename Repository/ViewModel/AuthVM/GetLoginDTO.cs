using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.ViewModel.AuthVM
{
    public class GetLoginDTO
    {
        
        public string Message { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public UserDTO User { get; set; } = new UserDTO();
    }
}
