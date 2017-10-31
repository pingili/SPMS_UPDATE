using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
   public class SelectGroupDto
    {
        public string ClusterName { get; set; }
        public string Village { get; set; }
        public int GroupID { get; set; }
        public string GroupCode { get; set; }
        public string GroupName { get; set; }
        public string TEGroupName { get; set; }
        public int ClusterID { get; set; }
        public int VillageID { get; set; }
    }
}
