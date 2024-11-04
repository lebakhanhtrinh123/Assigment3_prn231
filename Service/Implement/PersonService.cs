using AutoMapper;
using BusinessObject;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using Repository.Reponse;
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

        public async Task<List<GetPersonResponse>> getPersons() // Đổi tên hàm thành getPersons
        {
            var persons = await _unitOfWork.Persons.Entities
                .Include(p => p.PersonViruses) // Bao gồm các virus liên quan
                .ThenInclude(pv => pv.Virus)    // Bao gồm chi tiết virus
                .ToListAsync();

            // Tạo danh sách personResponse, không bao giờ là null
            var personResponse = persons.Select(person => new GetPersonResponse
            {
                PersonId = person.PersonId,
                Fullname = person.Fullname,
                Phone = person.Phone,
                BirthDay = person.BirthDay,
                Viruses = person.PersonViruses.Select(pv => new GetVirusResponse
                {
                    VirusName = pv.Virus.VirusName,
                    ResistanceRate = pv.ResistanceRate
                }).ToList()
            }).ToList();

            return personResponse; // Trả về danh sách, sẽ là một danh sách rỗng nếu không có người
        }

        public async Task<GetPersonByIdResponse> getPersonById(int personId)
        {
            var person = await _unitOfWork.Persons.Entities
               .Include(p => p.PersonViruses)
               .ThenInclude(pv => pv.Virus)
               .FirstOrDefaultAsync(n => n.PersonId == personId);
            if(person == null)
            {
                return null;
            }
            var personResponse = new GetPersonByIdResponse
            {
                Fullname = person.Fullname,
                Phone = person.Phone,
                BirthDay = person.BirthDay,
                Viruses = person.PersonViruses.Select(pv => new GetVirusResponse
                {
                    VirusName = pv.Virus.VirusName,
                    ResistanceRate = pv.ResistanceRate
                }).ToList()
            };
            return personResponse;
        }

        public async Task<string> DeletePerson(int personId)
        {
            var person = await _unitOfWork.Persons.Entities
                       .Include(p => p.PersonViruses) 
                         .ThenInclude(pv => pv.Virus)
                        .FirstOrDefaultAsync(n => n.PersonId == personId);
            if (person == null)
            {
                throw new Exception("Không tìm thấy người với ID đã cho."); 
            }
            if (person.PersonViruses.Any())
            {
                foreach (var personVirus in person.PersonViruses)
                {
                    await _unitOfWork.GetRepository<PersonVirus>().DeleteAsync(personVirus.VirusId); 
                }
            }
            else
            await _unitOfWork.Persons.DeleteAsync(personId);
            await _unitOfWork.SaveAsync();
            return "Person and related viruses deleted successfully";
        }
    }
}
