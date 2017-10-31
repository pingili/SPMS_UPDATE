using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
  public   class OccupationLookupDto
    {
        public int OccupationID { get; set; }
        public string OccupationCode { get; set; }
        public string Occupation { get; set; }
        public bool IsActive { get; set; }
        public string OccupationCategory { get; set; }
        public string StatusCode { get; set; }
        public string Status { get; set; }
    }
}
