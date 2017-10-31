using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
   public  class GroupLoanApplicationLookupDto
    {
        public int LoanMasterId { get; set; }
        public string LoanCode { get; set; }
        public string Purpose { get; set; }
        public string FundSourceName { get; set; }
        public string ProjectName { get; set; }
        public string StatusCode { get; set; }
        public string Status { get; set; }
    }
}
