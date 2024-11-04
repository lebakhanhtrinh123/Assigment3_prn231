using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Request
{
    public class VirusRequest
    {
        public string VirusName { get; set; } = null!;
        public double? ResistanceRate { get; set; }
    }
}
