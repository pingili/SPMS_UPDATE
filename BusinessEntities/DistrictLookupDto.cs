using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
   public  class DistrictLookupDto
    {
        public int DistrictID { get; set; }
        public string DistrictCode { get; set; }
        public string District { get; set; }
        public string State { get; set; }
        public string StatusCode { get; set; }
        public string Status { get; set; }

    }
}
