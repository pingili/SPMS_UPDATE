using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class GroupInterestRatesDto
    {
        public int GroupInterestID { get; set; }
        public string PrincipalAH { get; set; }
        public int PrincipalAHID { get; set; }
        public string InterestAH { get; set; }
        public int InterestAHID { get; set; }
        public string PenalAH { get; set; }
        public int PenalAHID { get; set; }
        public int Base { get; set; }
        public string BaseText { get; set; }
        public string CaluculationMethod { get; set; }
        public int CaluculationMethodId { get; set; }
        public Decimal ROI { get; set; }
        public Decimal PenalROI { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public bool IsActive { get; set; }
        public char Type { get; set; }
        
    }
}
