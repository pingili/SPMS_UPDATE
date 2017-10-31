using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
   public class DistrictDto
    {
        public int DistrictID { get; set; }
        public string DistrictCode { get; set; }
        public string District { get; set; }
        public string TEDistrictName { get; set; }
        public int StateID { get; set; }
        public int UserId { get; set; }
    }
}
