using AutoMapper;
using BusinessObject;
using Repository.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Mapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<ViroCureUser, UserDTO>().ReverseMap();
        }
    }
}

