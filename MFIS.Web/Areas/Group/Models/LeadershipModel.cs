using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MFIS.Web.Areas.Group.Models
{
    public class LeadershipModel
    {
        public int LeadershipID { get; set; }
        public int LeadershipLevel { get; set; }
        public int ObjectID { get; set; }
        public int LeadershipTitle { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int MemberID { get; set; }
        public int BranchID { get; set; }
        public int ClusterID { get; set; }
        public int GroupID { get; set; }
        public int EmployeeID { get; set; }
        public int StatusID { get; set; }
        public string Member { get; set; }
        public string Branch { get; set; }
        public string Cluster { get; set; }
        public string Group { get; set; }
        public string LeadershipTitleName { get; set; }
        public string LeadershipLevelName { get; set; }

        public int UserID { get; set; }
    }
}