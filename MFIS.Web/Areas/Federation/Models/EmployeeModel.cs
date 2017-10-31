using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MFIS.Web.Areas.Federation.Models
{
    public class EmployeeModel
    {

        public int EmployeeID { get; set; }

        [Required(ErrorMessage = "Panchayat  is required")]
        [RegularExpression(@"(\S)+", ErrorMessage = "White space is not allowed")]
        [DisplayName("Employee Code")]
        public string EmployeeCode { get; set; }

        [DisplayName("Employee Ref Code")]
        public string EmployeeRefCode { get; set; }

        [Required]
        [DisplayName("SurName")]
        public string SurName { get; set; }

        [Required]
        [DisplayName("Account Type")]
        public string AccountType { get; set; }

        [DisplayName("SurName(Telugu)")]
        public string TESurName { get; set; }

        [Required]
        [DisplayName("Name")]
        public string EmployeeName { get; set; }

        [Required]
        [DisplayName("Employee Name(Telugu)")]
        public string TEEmployeeName { get; set; }
        public string Photo { get; set; }

        //[DisplayName("BranchID")]
        [Required]
        public int BranchID { get; set; }


        [DisplayName("Bank Name")]
        public string BankName { get; set; }

        [Required]
        [DisplayName("Bank Branch")]
        public string BankBranch { get; set; }

        [Required]
        [DisplayName("Branch Code")]
        public string BranchCode { get; set; }

        [DisplayName("Branch Name")]
        public string BranchName { get; set; }

        [Required]
        [DisplayName("Cluster")]
        public int ClusterID { get; set; }

        [DisplayName("Gender")]
        public string Gender { get; set; }

        [DisplayName("Date of Joining")]
        public System.DateTime DOJ { get; set; }

        [DisplayName("Education Qualification")]
        public int EducationQualification { get; set; }

        [DisplayName("Mobile Number")]
        public string MobileNumber { get; set; }

        [DisplayName("Email")]
        public string Email { get; set; }

        [DisplayName("Date of Birth")]
        public System.DateTime DOB { get; set; }

        [Required]
        [DisplayName("Designation")]
        public int Designation { get; set; }

        [DisplayName("Disability")]
        public bool Disability { get; set; }
        [DisplayName("Blood Group")]
        public int BloodGroup { get; set; }


        [DisplayName("Marital Status")]
        public string MaritalStatus { get; set; }


        [DisplayName("Social Category")]
        public int SocialCategory { get; set; }

        [DisplayName("Present Address")]
        public string PresentAddress { get; set; }


        [DisplayName("Permanent Address")]
        public string PermanentAddress { get; set; }

        [DisplayName("Emergency Contact Number")]
        public string EmergencyContactNumber { get; set; }

        [DisplayName("Emergency Contact Name")]
        public string EmergencyContactName { get; set; }

        [DisplayName("Nominee Name")]
        public string NomineeName { get; set; }

        [DisplayName("Nominee Relationship")]
        public int NomineeRelationship { get; set; }

        [DisplayName("Parent/Guardian/Spouse Name")]
        public string ParentGuardianName { get; set; }

        [DisplayName("Guardian/Spouse Relationship")]
        public int ParentGuardianRelationship { get; set; }

        [DisplayName("Family Income")]
        public int FamilyIncome { get; set; }

        [DisplayName("Earning Members in Family")]
        public byte EarningMembersInFamily { get; set; }

        [DisplayName("Dependant/Non-earning members in family")]
        public byte NonEarningMembersInFamily { get; set; }

        [DisplayName("Assets in name of Employee")]
        public decimal AssetsInNameOfEmployee { get; set; }

        [DisplayName("From Date")]
        public DateTime FromDate { get; set; }

        [DisplayName("To Date")]
        public DateTime ToDate { get; set; }

        [DisplayName("Date of Retirement")]
        public DateTime DateofRetirement { get; set; }

        [DisplayName("Aadhar Number")]
        public string AadharNumber { get; set; }

        [DisplayName("Voter ID")]
        public string VoterID { get; set; }

        [DisplayName("Aadhar Uplaod")]
        public string AadharUplaod { get; set; }

        [DisplayName("PAN Card")]
        public string PANCard { get; set; }


        [DisplayName("Ration Card")]
        public string RationCard { get; set; }


        [DisplayName("PAN Card Uplaod")]
        public string PANCardUplaod { get; set; }

        [DisplayName("Voter ID Uploaded")]
        public string VoterIDUploaded { get; set; }

        [DisplayName("Ration Card Upload")]
        public string RationCardUploaded { get; set; }

        [DisplayName("IFSC Code")]
        public string IFSCCode { get; set; }


        public string Religion { get; set; }

        public int StatusID { get; set; }
        public int UserID { get; set; }
        public List<EmployeeKycmodel> EmplyeekycList{get; set;}

        public List<EmployeeDisignationXREF> lstEmployeeDisignationXREF { get; set; }


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
        public string BankAccountNo { get; set; }
        public string BankImagePath { get; set; }

        public string LoginUserName { get; set; }
        public string LoginPassWord { get; set; }
        public string RoleName { get; set; }
        public int RoleId { get; set; }

    }
    public class EmployeeKycmodel
    {
        public int EmployeeKYCID { get; set; }
        public int EmployeeID { get; set; }
        public int KYCType { get; set; }
        public string FileName { get; set; }
        public string ActualFileName { get; set; }
        public int StatusID { get; set; }
        public int UserID { get; set; }
    
    }
    public class EmployeeDisignationXREF
    {
        public int EmpDisgID            { get; set; }
        public int EmployeeID           { get; set; }
        public int DisignationID        { get; set; }
        public DateTime FromDate        { get; set; }
        public int AssignedBy           { get; set; }
        public DateTime ToDate          { get; set; }
        public int UnAssignedBy         { get; set; }


    }

}