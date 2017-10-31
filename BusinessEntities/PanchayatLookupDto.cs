using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class PanchayatLookupDto
    {
        public int PanchayatID { get; set; }
        public string PanchayatCode { get; set; }
        public string Panchayat { get; set; }
        public string Village { get; set; }
        public string Cluster { get; set; }
        public string Mandal { get; set; }
        public string District { get; set; }
        public string State { get; set; }
        public int StatusID { get; set; }
        public string StatusCode { get; set; }
        public string Status { get; set; }
        public string VillageCode { get; set; }
        public string ClusterCode { get; set; }
    }
}
