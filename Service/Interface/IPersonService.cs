using Repository.ViewModel;
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
        Task AddPersonAsync(PersonDTO personDto);
    }
}
