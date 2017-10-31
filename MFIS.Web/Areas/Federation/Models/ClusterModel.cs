using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MFIS.Web.Areas.Federation.Models
{
    public class ClusterModel
    {
        public int ClusterID { get; set; }

        public string ClusterCode { get; set; }

        [DisplayName("Cluster Name")]
        public string ClusterName { get; set; }

        [DisplayName("Mandal")]
        public string MandalName { get; set; }

        [DisplayName("State")]
        public int StateID { get; set; }
        public int StatusID { get; set; }

        [DisplayName("Cluster Name (Telugu)")]
        public string TEClusterName { get; set; }

        [DisplayName("Cluster Start Date")]
        public DateTime StartDate { get; set; }
        
        [DisplayName("District")]
        public int DistrictID { get; set; }

        [DisplayName("Mandal")]
        public int MandalID { get; set; }
        [DisplayName("Branch")]
        public int BranchID { get; set; }

        [DisplayName("Branch")]
        public string BranchName { get; set; }

        public string StateName { get; set; }

        [DisplayName("Contact No")]
        public string Phone { get; set; }
        
        [DisplayName("Address")]
        public string Address { get; set; }

        [DisplayName("Leader")]
        public int Leader { get; set; }

        [DisplayName("From Date")]
        public DateTime LeaderFromDate { get; set; }

        [DisplayName("To Date")]
        public DateTime LeaderToDate { get; set; }
        public int UserID { get; set; }
    }
}