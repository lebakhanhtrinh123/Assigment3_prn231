using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Request
{
    public class AddPersonRequest
    {
        public int PersonId { get; set; }

        public string Fullname { get; set; } = null!;

        public DateTime BirthDay { get; set; }

        public string Phone { get; set; } = null!;
        public List<VirusRequest>? viruses { get; set; }
    }
}
