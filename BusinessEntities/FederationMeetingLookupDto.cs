using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
   public class FederationMeetingLookupDto
    {
        public int FederationMeetingID { get; set; }
        public DateTime MeetingDate { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Reason { get; set; }
        public bool IsConducted { get; set; }
        public bool IsSpecialMeeting { get; set; }
        public bool isLocked { get; set; }
        public string StatusCode { get; set; }
        public string Status { get; set; }
        public int Members { get; set; }
    }
}
