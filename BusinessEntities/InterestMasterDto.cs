using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class InterestMasterDto
    {
        public int InterestID { get; set; }
        public string InterestCode { get; set; }
        public string InterestName { get; set; }
        public int? PrincipalAHID { get; set; }
        public int InterestAHID { get; set; }
        public int? PenalAHID { get; set; }
        public int Base { get; set; }
        public int? CaluculationMethod { get; set; }
        public int UserId { get; set; }
        public decimal InterestRate { get; set; }
        public int DueDay { get; set; }
        public decimal PenalROI { get; set; }
        public List<InterestRatesDto> InterestRates { get; set; }
        public int InterestRateID { get; set; }

        public string PrincipalAHCode { get; set; }
        public string PrincipalAHName { get; set; }
        public string InterestAHName { get; set; }
        public string AHName { get; set; }

    }

    public class InterestRatesDto
    {
        public int IntrestRateID { get; set; }
        public decimal ROI { get; set; }
        public decimal PenalROI { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }

    public class InterestViewDto
    {
        public string InterestCode { get; set; }
        public string InterestName { get; set; }
        public string PrincipalAH { get; set; }
        public string InterestAH { get; set; }
        public string PenalAH { get; set; }
        public string Base { get; set; }
        public string CalculationMethod { get; set; }
        
        public List<InterestRatesDto> InterestRates { get; set; }
    }
}
