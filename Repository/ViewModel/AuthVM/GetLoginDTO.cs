using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.ViewModel.AuthVM
{
    public class GetLoginDTO
    {
        public UserDTO User { get; set; } = new UserDTO();
        public GetTokenDTO Token { get; set; } = new GetTokenDTO();
    }
}
