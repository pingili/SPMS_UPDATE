using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class GroupLoanDisbursementLookupDto
    {
        public int LoanMasterID { get; set; }
        public string LoanCode { get; set; }
        public int GroupID { get; set; }
        public string groupName { get; set; }
        public decimal LoanAmountApplied { get; set; }
        public int LoanPurpose { get; set; }
        public string Purpose { get; set; }
        public string StatusCode { get; set; }
        public string Status { get; set; }
        public string MemberName { get; set; }
        public int? MemberId { get; set; }
        public decimal DisbursedAmount { get; set; }
        public DateTime DisbursementDate { get; set; }
        public decimal OutStandingAmount { get; set; }
    }
}
