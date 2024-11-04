using AutoMapper;
using BusinessObject;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using Repository.Request;
using Repository.ViewModel;
using Repository.ViewModel.AuthVM;
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

        public async Task<GetPersonDTO> AddPersonAsync(AddPersonRequest addPersonRequest)
        {
            Person? person = new Person
            {
                
                PersonId = addPersonRequest.PersonId,
                Fullname = addPersonRequest.Fullname,
                BirthDay = DateOnly.FromDateTime(addPersonRequest.BirthDay),
                Phone = addPersonRequest.Phone,
                PersonViruses = new List<PersonVirus>()
            };
            foreach (var virusRequest in addPersonRequest.viruses ?? Enumerable.Empty<VirusRequest>())
            {
                var virus = await _unitOfWork.GetRepository<Virus>()
                                     .Entities
                                     .FirstOrDefaultAsync(v => v.VirusName == virusRequest.VirusName);
                if (virus == null)
                {
                    virus = new Virus { VirusId = 4 , VirusName = virusRequest.VirusName ,Treatment = "Acyclovir" };
                    await _unitOfWork.GetRepository<Virus>().AddAsync(virus);
                }
                var personVirus = new PersonVirus
                {
                    PersonId = person.PersonId,
                    VirusId = virus.VirusId,
                    ResistanceRate = virusRequest.ResistanceRate
                };
                await _unitOfWork.GetRepository<PersonVirus>().AddAsync(personVirus);
            }
            await _unitOfWork.GetRepository<Person>().AddAsync(person);

            await _unitOfWork.SaveAsync();


            return new GetPersonDTO()
            {
                PersonId = person.PersonId,
                Message = "Person and viruses added successfully"
            };
        }
    }
}
