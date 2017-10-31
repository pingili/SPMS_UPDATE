using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class ProjectDto
    {
        public int ProjectID { get; set; }
        public int FundSourceID { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public int Purpose { get; set; }
        public int Type { get; set; }
        public int UserID { get; set; }
    }

    public class ProjectLookupDto
    {
        public int ProjectID { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public string FundSourceName { get; set; }
        public string Purpose { get; set; }
        public string Type { get; set; }
        public string StatusCode { get; set; }
        public string Status { get; set; }
    }
}
