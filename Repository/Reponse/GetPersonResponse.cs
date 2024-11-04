using Repository.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Reponse
{
    public  class GetPersonResponse
    {
        public int PersonId { get; set; }

        public string Fullname { get; set; } = null!;

        public DateOnly BirthDay { get; set; }

        public string Phone { get; set; } = null!;
        public List<GetVirusResponse> Viruses { get; set; } = new List<GetVirusResponse>();
    }
}
