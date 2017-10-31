using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class VillageLookupDto
    {
        public int VillageId { get; set; }
        public string VillageCode { get; set; }
        public string Village { get; set; }
        public string Cluster { get; set; }
        public string Mandal { get; set; }
        public string District { get; set; }
        public string State { get; set; }
        public string StatusCode { get; set; }
        public string Status { get; set; }

    }
}
