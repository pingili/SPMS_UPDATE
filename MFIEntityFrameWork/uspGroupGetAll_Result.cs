//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MFIEntityFrameWork
{
    using System;
    
    public partial class uspGroupGetAll_Result
    {
        public int GroupID { get; set; }
        public string GroupCode { get; set; }
        public string GroupRefNumber { get; set; }
        public string GroupName { get; set; }
        public string TEGroupName { get; set; }
        public int PanchayatID { get; set; }
        public System.DateTime FormationDate { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int MeetingFrequency { get; set; }
        public System.DateTime FederationTranStartDate { get; set; }
        public Nullable<System.DateTime> DateOfClosure { get; set; }
        public Nullable<byte> MeetingDay { get; set; }
        public Nullable<System.TimeSpan> MeetingStartTime { get; set; }
        public Nullable<System.TimeSpan> MeetingEndTime { get; set; }
        public int StatusID { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public string Panchayat { get; set; }
    }
}
