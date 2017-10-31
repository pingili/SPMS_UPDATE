using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MFIS.Web.Areas.Group.Models
{
    public class MemberModel
    {
        public int MemberID { get; set; }
        public string MemberCode { get; set; }
        public string MemberRefCode { get; set; }
        [Required]
        public string SurName { get; set; }
        public string TESurName { get; set; }
        [Required]
        public string MemberName { get; set; }
        public string TEMemberName { get; set; }
        public string Photo { get; set; }
        [Required]
        public int GroupID { get; set; }
        public string GroupCode { get; set; }
        public string GroupName { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public DateTime DateOfAdmission { get; set; }
        public int? EducationQualification { get; set; }
        public string MobileNumber { get; set; }
     
        public string Email { get; set; }
        public DateTime DOB { get; set; }
        public DateTime DateOfRetirement { get; set; }
        public int? OccupationID { get; set; }
        public bool Disability { get; set; }
        public string MaritalStatus { get; set; }

        public string NomineeName { get; set; }
        public int? NomineeRelationship { get; set; }
        public string ParentGuardianName { get; set; }
        public int? ParentGuardianRelationship { get; set; }
        public int? SocialCategory { get; set; }
        public int MonthlyIncome { get; set; }
        public byte EarningMembersInFamily { get; set; }
        public byte NonEarningMembersInFamily { get; set; }
        public int? IncomeFrequency { get; set; }
        public decimal InvestmentSize { get; set; }
        public int AssetsInNameOfMember { get; set; }
        public int AccountHeadID { get; set; }
        public int Religion { get; set; }
        public int UserID { get; set; }

        public string village { get; set; }
        public string cluster { get; set; }
        public string panchayat { get; set; }
        public List<MemberKycModel> MemberkycList { get; set; }

        public string AadharNo { get; set; }
        public string VoterNo { get; set; }
        public string PANNo { get; set; }
        public string RationNo { get; set; }
        public string JobcardNo { get; set; }
        public string AadharImagePath { get; set; }
        public string VoterImagePath { get; set; }
        public string PANImagePath { get; set; }
        public string RationImagePath { get; set; }
        public string JobcardImagePath { get; set; }
        public int MemberKYCID { get; set; }

        public string FederationDesigination { get; set; }
        public string GroupDesigination { get; set; }
        public string clusetrDesigination { get; set; }



        public int designation { get; set; }
        public DateTime FromDate { get; set; }
        public string leaderShipLevel { get; set; }

    }
    public class MemberKycModel
    {
        public int MemberID { get; set; }
        public int MemberKYCID { get; set; }
        public int KYCType { get; set; }
        public string FileName { get; set; }
        public string ActualFileName { get; set; }
        public int StatusID { get; set; }
        public int UserID { get; set; }
        public string KYCNumber { get; set; }
 
    }
}