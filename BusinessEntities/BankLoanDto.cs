using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class BankLoanDto
    {
        public int LoanMasterId { get; set; }
        public string LoanCode { get; set; }
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public int BankEntryId { get; set; }
        public string GLAHName { get; set; }
        public int GLAHId { get; set; }
        public int SLAHId { get; set; }
        public string SLAHName { get; set; }
        public string LoanNumber { get; set; }
        public DateTime LoanRequestDate { get; set; }
        public decimal LoanAmountRequested { get; set; }
        public decimal LoanAmountApproved { get; set; }
        public DateTime LoanAmountApprovedDate { get; set; }
        public decimal DisbursedAmount { get; set; }
        public DateTime DisbursedDate { get; set; }
        public DateTime DueDate { get; set; }
        public int InterestRate { get; set; }
        public int NoofInstallments { get; set; }
        public decimal EMI { get; set; }

        public int UserID { get; set; }
        public string ReferenceNumber { get; set; }
        public string Status { get; set; }
        public string Narration { get; set; }

        public int BankLoanId { get; set; }

    }


    public class BankLoanViewDto
    {
        public int LoanMasterId { get; set; }
        public string LoanCode { get; set; }
        public string MemberName { get; set; }
        public DateTime LoanApplicationDate { get; set; }
        public string ProjectPurpose { get; set; }
        public string LoanPurpose { get; set; }
        public decimal LoanApplyAmount { get; set; }
        public int NoOfInstallmentsProposed { get; set; }
        public string LoanRepaymentMode { get; set; }
        public string LoanPrincipalAccountHead { get; set; }
        public string LoanInterestAccountHead { get; set; }
        public int InterestRate { get; set; }
        public int MonthlyDueDay { get; set; }
        public int ApprovalLevel { get; set; }
        public decimal DisbursementAmount { get; set; }
        public int NoOfInstallment { get; set; }
        public string LoanReferenceNumber { get; set; }
        public string TransactionMode { get; set; }
        public DateTime DisbursementDate { get; set; }
        public DateTime InstallmentStartFrom { get; set; }
        public DateTime InstallmentClosingDate { get; set; }
        public int MonthlyPrincipalDemand { get; set; }
        public string ChequeNumber { get; set; }
        public DateTime ChequeDate { get; set; }
        public string BankAccountHead { get; set; }
        public string StatusCode { get; set; }
        public string Status { get; set; }
        public string GroupApprovalComments { get; set; }
        public DateTime GroupActionDate { get; set; }
        public string ClusterApprovalComments { get; set; }
        public DateTime ClusterActionDate { get; set; }
        public string FederationApprovalComments { get; set; }
        public DateTime FederationActionDate { get; set; }
    }


    public class BankLoanLookupDto
    {
        public int BankLoanId { get; set; }
        public string LinkageNumber { get; set; }
        public string BankName { get; set; }
        public string SLAHName { get; set; }
        public decimal RequestedAmount { get; set; }
        public DateTime RequestedDate { get; set; }
        public decimal ApprovedAmount { get; set; }
        public DateTime ApprovedDate { get; set; }
        public decimal DisbursedAmount { get; set; }
        public DateTime DisbursedDate { get; set; }
        public int NoOfInstallments { get; set; }
        public decimal EMI { get; set; }
        public string StatusCode { get; set; }
        public string Status { get; set; }
    }


}