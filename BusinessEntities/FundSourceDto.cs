using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class FundSourceDto
    {
        public int FundSourceID { get; set; }
        public string FundSourceCode { get; set; }
        public string FundSourceName { get; set; }
        public string TEFundSourseName { get; set; }
        public int StatusID { get; set; }
        public int UserId { get; set; }

    }
}
