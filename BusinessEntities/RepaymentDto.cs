using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class RepaymentDto
    {
        public int LoanRepaymentID { get; set; }
        public int AHID { get; set; }
        public int SLAccountAHID { get; set; }
        public decimal Amount { get; set; }
        public decimal InterestAmount { get; set; }
    }
	
}
