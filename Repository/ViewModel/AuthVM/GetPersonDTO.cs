using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.ViewModel.AuthVM
{
    public class GetPersonDTO
    {
        public int PersonId { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
