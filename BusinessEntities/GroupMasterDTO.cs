using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class GroupMasterDto
    {
        public int GroupID { get; set; }
        public string GroupCode { get; set; }
        public string GroupRefNumber { get; set; }
        public string GroupName { get; set; }
        public string TEGroupName { get; set; }
        public int PanchayatID { get; set; }
        public int VillageID { get; set; }
        public int ClusterID { get; set; }
        public DateTime FormationDate { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int MeetingFrequency { get; set; }
        public System.DateTime FederationTranStartDate { get; set; }
        public DateTime DateOfClosure { get; set; }
        public byte MeetingDay { get; set; }
        public TimeSpan MeetingStartTime { get; set; }
        public TimeSpan MeetingEndTime { get; set; }
        public int UserId { get; set; }
        public string ClusterName { get; set; }
        public string Village { get; set; }
        public List<GroupMasterDto> lstGroupMasterDto { get; set; }
        public decimal RegularSavingAmount { get; set; }
        public int RegularSavingsAhId { get; set; }
        public string RegularSavingAccountHead { get; set; }
        public string LockStatus { get; set; }
        public string LockStatusCode { get; set; }
        public DateTime MeetingDate { get; set; }
    }

    public class GroupMasterViewDto
    {
        public int GroupID { get; set; }
        public string GroupCode { get; set; }
        public string GroupName { get; set; }
        public string TEGroupName { get; set; }
        public string GroupRefNumber { get; set; }
        public string Cluster { get; set; }
        public string Village { get; set; }
        public string Panchayat { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime FormationDate { get; set; }
        public int MeetingFrequency { get; set; }
        public string MeetingFrequencyText { get; set; }
        public DateTime FederationTranStartDate { get; set; }
        public DateTime DateOfClosure { get; set; }
        public string Address { get; set; }
        public byte MeetingDay { get; set; }
        public TimeSpan MeetingStartTime { get; set; }
        public TimeSpan MeetingEndTime { get; set; }
        public string MeetingDayText
        {
            get
            {
                return !MeetingFrequencyText.Contains("WEEK") ?
                    (MeetingFrequency > 0 ? MeetingDay.ToString() : string.Empty) :
                    getWeekDay(MeetingDay);
            }
        }

        private string getWeekDay(int weekDay)
        {
            switch (weekDay)
            {
                case 1: return "Sunday";
                case 2: return "Monday";
                case 3: return "Tuesday";
                case 4: return "Wednesday";
                case 5: return "Thursday";
                case 6: return "Friday";
                case 7: return "Saturday";
                default: return string.Empty;
            }
        }
        public decimal RegularSavingAmount { get; set; }
        public string RegularSavingAccountHead { get; set; }
    }
}