using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
  public class FederationMeetingDTO
    {
        public int FederationMeetingId { get; set; }
        public DateTime MeetingDate { get; set; }
        public TimeSpan MeetingTime { get; set; }
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool IsSplMeeting { get; set; }
        public bool IsConducted { get; set; }
        public bool Conducted { get; set; }
        public int? Reason { get; set; }
        public string OtherReason { get; set; }
        public DateTime RVColDate { get; set; }
        public string MeetingObjective { get; set; }
        public string MeetingComments { get; set; }
        public int statusId { get; set; }
        public DateTime TransactionDate { get; set; }

        public bool IsLocked { get; set; }
        public string Comments { get; set; }
        public TimeSpan StartTime { get; set; }
        public int UserId { get; set; }
        public int EmployeeId { get; set; }
        public string Employeename { get; set; }
        public List<MeetingMembersDTO> lstFederationMemberDto { get; set; }
    }

  public class MeetingMembersDTO
  {
      public long FederationMeetingMemberID { get; set; }
      public string MemberName { get; set; }
      public int MemberID { get; set; }
      public bool IsAttended { get; set; }

  }
}
