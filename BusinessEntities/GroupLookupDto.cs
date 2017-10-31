using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class GroupLookupDto
    {
        public int BankEntryID { get; set; }
        public string EntityCode { get; set; }
        public string GroupCode { get; set; }
        public int GroupID { get; set; }
        public string GroupName { get; set; }
        public string GroupRefNumber { get; set; }
        public string Panchayat { get; set; }
        public string ClusterName { get; set; }
        public DateTime FormationDate { get; set; }
        public DateTime FederationTranStartDate { get; set; }
        public byte MeetingDay { get; set; }
        public string StatusCode { get; set; }
        public string Status { get; set; }
    }
}
