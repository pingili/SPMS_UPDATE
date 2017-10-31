using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class GroupInfo
    {
        public int GroupID { get; set; }
        public string GroupName { get; set; }
        public string GroupCode { get; set; }
        public string TEGroupName { get; set; }
        public string Village { get; set; }
        public string Cluster { get; set; }
        public byte MeetingDay { get; set; }
        public string LockStatus { get; set; }
        public string LockStatusCode { get; set; }
        public DateTime MeetingDate { get; set; }

        public string GroupDisplayName
        {
            get
            {
                return string.Format("Group : {0}({1}), Village : {2}, Cluster : {3}", GroupName, GroupCode, Village, Cluster);
            }
        }

        public string GroupMeetingDetailsDisplay
        {
            get
            {
                if (MeetingDate == default(DateTime))
                {
                    return string.Format("Transaction Status :  {0}, Transaction Month : {1}", "NA", "NA");
                }
                return string.Format("Transaction Status :  {0}, Transaction Month : {1}", LockStatus, MeetingDate.ToString("MMMM"));
            }
        }
    }
}