using Repository.Request;
using Repository.ViewModel;
using Repository.ViewModel.AuthVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IAuthService
    {
        Task<GetLoginDTO> Login(LoginRequest loginRequest) ;
        Task<string> Register(RegisterRequest registerRequest);
    }
}
