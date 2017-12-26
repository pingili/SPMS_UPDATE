using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusinessEntities
{
    public class MemberLoanClosure
    {

        public int LoanMasterId { get; set; }
        public decimal PrincipleDemand { get; set; }
        public decimal InterestDemand { get; set; }

    }
}