using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class EmployeeDto
    {
        public int EmployeeID { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeRefCode { get; set; }
        public string SurName { get; set; }
        public string TESurName { get; set; }
        public string EmployeeName { get; set; }
        public string TEEmployeeName { get; set; }
        public string Photo { get; set; }
        public int BranchID { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public int ClusterID { get; set; }
        public string Gender { get; set; }
        public System.DateTime DOJ { get; set; }
        public int EducationQualification { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public System.DateTime DOB { get; set; }
        public int Designation { get; set; }
        public bool Disability { get; set; }
        public int BloodGroup { get; set; }
        public string MaritalStatus { get; set; }
        public int SocialCategory { get; set; }
        public string PresentAddress { get; set; }
        public string PermanentAddress { get; set; }
        public string EmergencyContactNumber { get; set; }
        public string EmergencyContactName { get; set; }
        public string NomineeName { get; set; }
        public int NomineeRelationship { get; set; }
        public string ParentGuardianName { get; set; }
        public int ParentGuardianRelationship { get; set; }
        public int FamilyIncome { get; set; }
        public byte EarningMembersInFamily { get; set; }
        public byte NonEarningMembersInFamily { get; set; }
        public decimal AssetsInNameOfEmployee { get; set; }
        public int StatusID { get; set; }
        public int UserID { get; set; }

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

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
        public int? Religion { get; set; }
        public System.DateTime DateOfRetirement { get; set; }
        public string LoginUserName { get; set; }
        public string LoginPassWord { get; set; }
        public string RoleName { get; set; }
        public int? RoleId { get; set; }
    }
    public class EmployeeViewDto
    {
        public string EmployeeCode { get; set; }
        public string EmployeeRefCode { get; set; }
        public string SurName { get; set; }
        public string TESurName { get; set; }
        public string EmployeeName { get; set; }
        public string TEEmployeeName { get; set; }
        public string Photo { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string Cluster { get; set; }
        public string Gender { get; set; }
        public System.DateTime DOJ { get; set; }
        public string EducationQualification { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public System.DateTime DOB { get; set; }
        public string Designation { get; set; }
        public System.DateTime DesignationFromDate { get; set; }
        public System.DateTime DesignationToDate { get; set; }
        public bool Disability { get; set; }
        public string BloodGroup { get; set; }
        public string MaritalStatus { get; set; }
        public string SocialCategory { get; set; }
        public string PresentAddress { get; set; }
        public string PermanentAddress { get; set; }
        public string EmergencyContactNumber { get; set; }
        public string EmergencyContactName { get; set; }
        public string NomineeName { get; set; }
        public string NomineeRelationship { get; set; }
        public string ParentGuardianName { get; set; }
        public string ParentGuardianRelationship { get; set; }
        public string FamilyIncome { get; set; }
        public byte EarningMembersInFamily { get; set; }
        public byte NonEarningMembersInFamily { get; set; }
        public decimal AssetsInNameOfEmployee { get; set; }
        public string Religion { get; set; }
        public System.DateTime DateOfRetirement { get; set; }
        public string LoginUserName { get; set; }
        public string LoginPassWord { get; set; }
        public string RoleName { get; set; }
        public int? RoleId { get; set; }

        public List<EmployeeKYCDto> EmployeeKYC { get; set; }
    }

    public class ClusterAssignmentDto
    {
        public int ClusterID { get; set; }
        public string ClusterName { get; set; }
        public string EmployeeName { get; set; }
        public string RoleName { get; set; }
        public int EmployeeId { get; set; }
    }
}
