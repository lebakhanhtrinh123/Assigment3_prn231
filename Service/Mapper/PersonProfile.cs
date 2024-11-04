using AutoMapper;
using BusinessObject;
using Repository.Reponse;
using Repository.ViewModel;

namespace Service.Mapper
{
    public class PersonProfile : Profile
    {
        public PersonProfile()
        {
            CreateMap<GetPersonResponse, Person>().ReverseMap();
        }
    }
}
