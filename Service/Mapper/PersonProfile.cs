using AutoMapper;
using BusinessObject;
using Repository.ViewModel;

namespace Service.Mapper
{
    public class PersonProfile : Profile
    {
        public PersonProfile()
        {
            CreateMap<PersonDTO, Person>();
            CreateMap<Person, PersonDTO>();
        }
    }
}
