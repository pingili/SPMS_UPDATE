using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class InterestLookupDto
    {
        public int InterestID { get; set; }
        public string InterestCode { get; set; }
        public string InterestName { get; set; }
        public string InterestAccountHead { get; set; }
        public string PrincipalAHName { get; set; }
        public string Base { get; set; }
        public string CaluculationMethod { get; set; }
        public string Status { get; set; }
        public string StatusCode { get; set; }
        public decimal ROI { get; set; }
    }
}
