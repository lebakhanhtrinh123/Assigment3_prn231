using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Reponse
{
    public class GetVirusResponse
    {
        public string VirusName { get; set; } = string.Empty;
        public double? ResistanceRate { get; set; }
    }
}
