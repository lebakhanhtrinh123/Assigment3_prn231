using Repository.Reponse;
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
    public interface IPersonService
    {
        Task<PersonDTO> GetPersonAsync(int id);
        Task<GetPersonDTO> AddPersonAsync(AddPersonRequest addPersonRequest);
        Task<List<GetPersonResponse>> getPersons();
        Task<GetPersonByIdResponse> getPersonById(int personId);
    }
}
