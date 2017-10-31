using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
   public class LeadershipLookupDto
    {
        public int LeadershipID { get; set; }
        public string LeadershipLevel { get; set; }
        public string LeadershipTitle { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string StatusCode { get; set; }
        public string Status { get; set; }

        public string MemberName { get; set; }
        public string BranchName { get; set; }
        public string ClusterName { get; set; }
        public string GroupName { get; set; }
        public string Leadership { get; set; }

    }
}
