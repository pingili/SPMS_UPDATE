using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class PanchayatDto
    {
        public int PanchayatID { get; set; }
        public string PanchayatCode { get; set; }
        public string PanchayatName { get; set; }
        public string TEPanchayatName { get; set; }
        public int VillageID { get; set; }
        public int StatusID { get; set; }
        public int ClusterID { get; set; }
        public int DistrictID { get; set; }
        public int MandalID { get; set; }
        public int StateID { get; set; }
        public int UserId { get; set; }
    }
}
