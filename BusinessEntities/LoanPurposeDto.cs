using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
   public class LoanPurposeDto
    {
        public int LoanPurposeID { get; set; }
        public int Category { get; set; }
        public int Project { get; set; }
        public string LoanPurposeCode { get; set; }
        public string TELoanPurpose { get; set; }
        public string Purpose { get; set; }
        public int UserID { get; set; }
        public int ProjectID { get; set; }
    }
}
