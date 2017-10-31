using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MFIS.Web.Areas.Federation.Models
{
    public class InterestModel
    {
        public class InterestMasterDto
        {
            public int InterestID { get; set; }
            public string InterestCode { get; set; }
            public string InterestName { get; set; }
            public int PrincipalAHID { get; set; }
            public int InterestAHID { get; set; }
            public int PenalAHID { get; set; }
            public int Base { get; set; }
            public int? CaluculationMethod { get; set; }
            public int UserId { get; set; }
            public List<InterestRatesDto> InterestRates { get; set; }
        }

        public class InterestRatesDto
        {
            public int IntrestRateID { get; set; }
            public decimal ROI { get; set; }
            public decimal PenalROI { get; set; }
            public DateTime FromDate { get; set; }
            public DateTime ToDate { get; set; }
        }
    }
}