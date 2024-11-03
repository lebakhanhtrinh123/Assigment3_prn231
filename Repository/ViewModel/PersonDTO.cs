using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.ViewModel
{
    public class PersonDTO
    {
        public int PersonId { get; set; }
        public string? Fullname { get; set; }
        public DateOnly BirthDay { get; set; }
        public string? Phone { get; set; }
    }
}
