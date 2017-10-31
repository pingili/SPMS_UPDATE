using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class MemberDto
    {
        public int MemberID { get; set; }
        public string MemberCode { get; set; }
        public string MemberRefCode { get; set; }
        public string SurName { get; set; }
        public string TESurName { get; set; }
        public string MemberName { get; set; }
        public string TEMemberName { get; set; }
        public string Photo { get; set; }
        public int GroupID { get; set; }
        public string GroupCode { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfAdmission { get; set; }
        public DateTime DateOfRetirement { get; set; }
        public int? EducationQualification { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public DateTime DOB { get; set; }
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

        public int designation { get; set; }
        public DateTime FromDate { get; set; }
        public string leaderShipLevel { get; set; }

    }

    public class MemberViewDto
    {
        public int MemberID { get; set; }

        public string MemberCode { get; set; }
        public string MemberRefCode { get; set; }
        public string SurName { get; set; }
        public string TESurName { get; set; }
        public string MemberName { get; set; }
        public string TEMemberName { get; set; }
        public string Photo { get; set; }
        public string GroupCode { get; set; }
        public string GroupName { get; set; }
        public string Panchayat { get; set; }
        public DateTime DateOfAdmission { get; set; }
        public string Gender { get; set; }
        public string EducationQualification { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public DateTime DOB { get; set; }
        public string Occupation { get; set; }
        public bool Disability { get; set; }
        public string MaritalStatus { get; set; }
        public DateTime DateOfRetirement { get; set; }
        public string Religion { get; set; }
        public string NomineeName { get; set; }
        public string NomineeRelationship { get; set; }
        public string ParentGuardianName { get; set; }
        public string ParentGuardianRelationship { get; set; }
        public string SocialCategory { get; set; }
        public int MonthlyIncome { get; set; }
        public byte EarningMembersInFamily { get; set; }
        public byte NonEarningMembersInFamily { get; set; }
        public string IncomeFrequency { get; set; }
        public decimal InvestmentSize { get; set; }
        public int AssetsInNameOfMember { get; set; }
        public string AccountHead { get; set; }
        public string KYCType { get; set; }
        public string FileName { get; set; }
        public string ActualFileName { get; set; }

        public List<MemberKYCDto> MemberKYC { get; set; }
		

    }
    
}
