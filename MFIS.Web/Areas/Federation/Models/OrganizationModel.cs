
namespace MFIS.Web.Areas.Federation.Models
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class OrganizationModel
    {
        public int OrgID { set; get; }

        [DisplayName("Organization Name")]
        public string OrgName { set; get; }

        [DisplayName("Organization Name(Telugu)")]
        public string TEOrgName { set; get; }

        [DisplayName("Federation Code")]
        public string OrgCode { set; get; }

        [DisplayName("Registration Number")]
        public string RegistrationNumber { set; get; }

        [DisplayName("Registration Date")]
        // [DataType(DataType.Date)]
        // [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime RegistrationDate { set; get; }

        [DisplayName("Address")]
        public string Address { set; get; }

        [DisplayName("PAN")]
        public string PAN { set; get; }

        [DisplayName("TAN")]
        public string TAN { set; get; }

        [DisplayName("VAT")]
        public string VAT { set; get; }

        [DisplayName("TIN")]
        public string TIN { set; get; }

        [DisplayName("Member Retirement Age")]
        public int MemberRetirementAge { set; get; }

        [DisplayName("Employee Retirement Age")]
        public int EmpRetirementAge { set; get; }

        //[DisplayName("Groups")]
        //public bool IsMembershipCompositionGroup { set; get; }

        //[DisplayName("Individuals")]
        //public bool IsMembershipCompositionMember { set; get; }

        //[DisplayName("Groups and Individuals")]
        //public bool IsMembershipCompositionBoth { set; get; }

        [DisplayName("Groups and Individuals")]
        public char GroupsIndividuals { set; get; }

        public char MemCompGender { set; get; }

        public int UserID { get; set; }

        [DisplayName("Branch Code")]
        public string BranchCode { get; set; }

        public int BankEntryID { get; set; }

        public string BranchName { get; set; }
        [DisplayName("Bank Branch")]
        public string BankCode { get; set; }

        [DisplayName("Bank Name")]
        public int BankName { get; set; }

        [DisplayName("IFSC Code")]
        public string IFSC { get; set; }

        [DisplayName("Bank A/C Number")]
        public string AccountNumber { get; set; }

        [DisplayName("Bank Account Type")]
        public int AccountType { get; set; }

        public int NoOfBranches { get; set; }

        [DisplayName("State ")]
        public int NoOfStates { get; set; }

        [DisplayName("District ")]
        public int NoOfDistricts { get; set; }

        [DisplayName("Mandals Rural")]
        public int NoOfMandalsRural { get; set; }

        [DisplayName("Mandals Urban /Towns ")]
        public int NoOfMandalsUrban { get; set; }

        public DateTime FinancialYearStartDate { get; set; }

        public DateTime FinancialYearEndDate { get; set; }
    }
}