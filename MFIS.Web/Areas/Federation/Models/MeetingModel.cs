using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MFIS.Web.Areas.Federation.Models
{
    public class MeetingModel
    {
        public int FederationMeetingId { get; set; }
        public string Reason { get; set; }
        public bool splMeeting{get;set;}
        public bool NotConducted { get; set; }
        public string otherReason { get; set; }
        public DateTime MeetingDate { get; set; }
        public DateTime RVColDate { get; set; }
        public TimeSpan StartTme {get;set; }
        public TimeSpan EndTime { get; set; }
        public string MeetingObjective { get; set; }
        public int statusId { get; set; }
        public string Comments { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ModifyBy { get; set; }
        public DateTime ModifiedOn { get; set; }


        
    }


    public class FederationMeetingMembers
    {
        public int MemberId { get; set; }
        public bool IsAttended { get; set; }
    }


}