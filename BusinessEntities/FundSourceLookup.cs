using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class FundSourceLookup
    {
        public int FundSourceID { get; set; }
        public string FundSourceCode { get; set; }
        public string FundSourceName { get; set; }
        public string StatusCode { get; set; }
        public string Status { get; set; }
    }
}
