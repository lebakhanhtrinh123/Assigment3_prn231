using AutoMapper;
using BusinessObject;
using Repository.Interface;
using Repository.ViewModel;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Implement
{
    public class PersonService : IPersonService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PersonService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PersonDTO> GetPersonAsync(int id)
        {
            var person = await _unitOfWork.Persons.GetByIdAsync(id);
            return _mapper.Map<PersonDTO>(person);
        }

        public async Task AddPersonAsync(PersonDTO personDto)
        {
            var person = _mapper.Map<Person>(personDto);
            await _unitOfWork.Persons.AddAsync(person);
            await _unitOfWork.SaveAsync();
        }
    }
}
