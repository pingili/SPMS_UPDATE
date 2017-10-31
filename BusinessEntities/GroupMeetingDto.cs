using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreComponents;

namespace BusinessEntities
{
    public class GroupMeetingDto
    {
        public int GroupMeetingID { get; set; }
        public int GroupID { get; set; }
        public string MeetingObjective { get; set; }
        public string MeetingComments { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public DateTime MeetingDate { get; set; }
        public string DisplayMeetingDate
        {
            get
            {
                return MeetingDate.ToDisplayDateFormat();
            }
        }
        public DateTime TransactionDate { get; set; }
        public bool IsConducted { get; set; }
        public bool IsSplMeeting { get; set; }
        public int? Reason { get; set; }
        public string ReasonName { get; set; }
        public string OtherReason { get; set; }
        public int UserId { get; set; }
        public List<GroupMeetingMembersDto> lstgroupMembersDto { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int GroupMeetingDay { get; set; }
        public string MonthName { get; set; }
        public string MeetingType { get; set; }
        public int MeetingMemberCount { get; set; }
        
    }

    public class GroupMeetingMembersDto
    {
        public long GroupMeetingMemberID { get; set; }
        public string MemberName { get; set; }
        public int MemberID { get; set; }
        public bool IsAttended { get; set; }

    }

    public class GroupMeetingViewDto
    {
        public DateTime MeetingDate { get; set; }
        public bool IsConducted { get; set; }
        public string MeetingComments { get; set; }
        public string MeetingObjective { get; set; }
        public DateTime TransactionDate { get; set; }
        public bool IsSplMeeting { get; set; }
        public string ReasonName { get; set; }
        public string OtherReason { get; set; }
        public string Members { get; set; }
    }
}
