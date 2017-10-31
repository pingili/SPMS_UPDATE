using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MFIS.Web.Areas.Federation.Models
{
    public class GroupLoanDisbursementModel
    {
        public class GroupLoanDisbursementDto
        {
            public int LoanMasterId { get; set; }
            public string LoanCode { get; set; }
            public string LoanType { get; set; }
            public int MemberID { get; set; }
            public int GroupID { get; set; }
            public string GroupName { get; set; }
            public string VillageCode { get; set; }
            public string VillageName { get; set; }
            public string ClusterCode { get; set; }
            public string ClusterName { get; set; }
            public DateTime LoanApplicationDate { get; set; }
            public int LoanPurpose { get; set; }
            public decimal LoanAmountApplied { get; set; }
            public byte NoofInstallmentsProposed { get; set; }
            public int Mode { get; set; }
            public int UserID { get; set; }
            public List<GroupMember> lstGroupmember { get; set; }
            public List<ChequeDetails> lstChequeDetails { get; set; }

        }
        public class GroupMember
        {
            public int MemberID { get; set; }
            public string MemberCode { get; set; }
            public string MemberName { get; set; }

        }
        public class ChequeDetails
        {
            public int LoanChequeID { get; set; }
            public int LoanMasterID { get; set; }
            public string ChequeNumber { get; set; }
            public DateTime ChequeDate { get; set; }
            public decimal Amount { get; set; }
            public int BankName { get; set; }
            public string Branch { get; set; }
            public int StatusID { get; set; }
            public int userId { get; set; }

        }
    }
}