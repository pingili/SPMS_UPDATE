using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class OrganizationDto
    {
        public int OrgID { set; get; }
        public string OrgName { set; get; }
        public string TEOrgName { set; get; }
        public string OrgCode { set; get; }
        public string RegistrationNumber { set; get; }
        public DateTime RegistrationDate { set; get; }
        public string Address { set; get; }
        public string PAN { set; get; }
        public string TAN { set; get; }
        public string VAT { set; get; }
        public string TIN { set; get; }
        public int MemberRetirementAge { set; get; }
        public int EmpRetirementAge { set; get; }
        //public bool IsMembershipCompositionGroup { set; get; }
        //public bool IsMembershipCompositionMember { set; get; }
        //public bool IsMembershipCompositionBoth { set; get; }
        public string GroupsIndividuals { get; set; }
        public string MemCompGender { set; get; }
        public int NoOfBranches { get; set; }
        public int NoOfStates { get; set; }
        public int NoOfDistricts { get; set; }
        public int NoOfMandalsRural { get; set; }
        public int NoOfMandalsUrban { get; set; }
        public int UserID { get; set; }
        public bool IsActive { get; set; }

        public DateTime FinancialYearStartDate { get; set; }

        public DateTime FinancialYearEndDate { get; set; }
    }
}
