using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
   public class LoanSecurityDto
    {
        public int LoanSecurityID { get; set; }
        public int Type { get; set; }
        public string LoanSecurityCode { get; set; }
        public string LoanSecurityName { get; set; }
        public int StatusID { get; set; }
        public int UserID { get; set; }
        public string RefValue { get; set; }
    }
}
