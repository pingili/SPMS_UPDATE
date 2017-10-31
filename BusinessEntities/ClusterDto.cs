using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class ClusterDto
    {
        public int ClusterID { get; set; }
        public string ClusterCode { get; set; }
        public string ClusterName { get; set; }
        public string TEClusterName { get; set; }
        public DateTime StartDate { get; set; }
        public int MandalID { get; set; }
        public int BranchID { get; set; }
        public int DistrictID { get; set; }
        public int StateID { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public int Leader { get; set; }
        public DateTime LeaderFromDate { get; set; }
        public DateTime LeaderToDate { get; set; }
        public int UserID { get; set; }
    }

    public class ClusterLookupDto
    {
        public int ClusterID { get; set; }
        public string ClusterCode { get; set; }
        public string ClusterName { get; set; }
        public string BranchName { get; set; }
        public string MandalName { get; set; }
        public string DistrictName { get; set; }
        public string StateName { get; set; }
        public string StatusCode { get; set; }
        public string Status { get; set; }
    }
    public class ClusterViewDto
    {
        public int ClusterID { get; set; }
        public string ClusterCode { get; set; }
        public string ClusterName { get; set; }
        public DateTime StartDate { get; set; }
        public string State { get; set; }
        public string District { get; set; }
        public string Mandal { get; set; }
        public string Branch { get; set; }
        public string Phone { get; set; }
        public string Leader { get; set; }
        public DateTime FromDate { get; set; }
        public string Address { get; set; }
        public string TEClusterName { get; set; }

    }
}
