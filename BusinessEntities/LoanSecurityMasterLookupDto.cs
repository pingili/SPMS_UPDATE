using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class LoanSecurityMasterLookupDto
    {
        public int LoanSecurityID { get; set; }
        public string LoanSecurityCode { get; set; }
        public string Type { get; set; }
        public string LoanSecurityName { get; set; }
        public string Status { get; set; }
        public string StatusCode { get; set; }
    }
}
