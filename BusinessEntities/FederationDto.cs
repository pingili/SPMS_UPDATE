using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public  class FederationDto
    {

        public int FederationMeetingID { get; set; }
        public DateTime MeetingDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string MeetingObjective { get; set; }
        public string MeetingComments { get; set; }
        public int StatusID { get; set; }
        public int UserId { get; set; }
    }
}
