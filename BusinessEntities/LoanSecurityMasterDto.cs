using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class LoanSecurityMasterDto
    {
        public int LoanSecurityID { get; set; }
        public int Type { get; set; }
        public string LoanSecurityCode { get; set; }
        public string LoanSecurityName { get; set; }
        public int UserID { get; set; }
    }
}
