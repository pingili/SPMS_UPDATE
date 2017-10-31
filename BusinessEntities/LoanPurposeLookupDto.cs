using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class LoanPurposeLookupDto
    {
        public int LoanPurposeID { get; set; }
        public string LoanPurposeCode { get; set; }
        public string Category { get; set; }
        public string ProjectName { get; set; }
        public string Purpose { get; set; }
        public string status { get; set; }
        public string StatusCode { get; set; }
    }
}
