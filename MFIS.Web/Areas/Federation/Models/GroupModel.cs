using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MFIS.Web.Areas.Federation.Models
{
    public class GroupModel
    {
        public int GroupID { get; set; }
        
        public string GroupCode { get; set; }
        
        [Required(ErrorMessage = "Group Code is required")]
        public string GroupRefNumber { get; set; }
        
        [Required(ErrorMessage = "Group Name is required")]
        public string GroupName { get; set; }
        
        public string TEGroupName { get; set; }
        
        [Required(ErrorMessage = "Please Select Panchayat")]
        public int PanchayatID { get; set; }

        public int VillageID { get; set; }

        public int ClusterID { get; set; }
        
        [Required(ErrorMessage = "formation date is required")]
        public System.DateTime FormationDate { get; set; }
        
        [Required(ErrorMessage = "Phone is required")]
        public string Phone { get; set; }
        
        public string Email { get; set; }
        
        public string Address { get; set; }
        
        [Required(ErrorMessage = "Group Meeting frequency is required")]
        public int MeetingFrequency { get; set; }

        public System.DateTime FederationTranStartDate { get; set; }
        
        public DateTime DateOfClosure { get; set; }
        
        public byte MeetingDay { get; set; }

        public TimeSpan MeetingStartTime { get; set; }

        public TimeSpan MeetingEndTime { get; set; }

        public decimal RegularSavingAmount { get; set; }

        public int RegularSavingsAhId { get; set; }

        public string RegularSavingAccountHead { get; set; }
    }
}