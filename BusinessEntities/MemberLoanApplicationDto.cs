using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class MemberLoanApplicationDto : MemberLoanApprovalDto
    {
        public int LoanMasterId { get; set; }
        public string LoanCode { get; set; }
        public string LoanType { get; set; }
        public int MemberID { get; set; }
        public string MemberName { get; set; }
        public int GroupID { get; set; }
        public DateTime LoanApplicationDate { get; set; }
        public int LoanPurpose { get; set; }
        public int FundSourceId { get; set; }
        public decimal LoanAmountApplied { get; set; }
        public byte NoofInstallmentsProposed { get; set; }
        public int Mode { get; set; }
        public int UserID { get; set; }
        public int ProjectID { get; set; }
        public int InterestMasterID { get; set; }
        public int InterestRateID { get; set; }
        public int ApprovalLevel { get; set; }
        public string StatusCode { get; set; }
        public string Status { get; set; }
        public string ReferenceNumber { get; set; }
        public string SourceOfFund { get; set; }
        public string TransactionMode { get; set; }
        public string PrincipleAHName { get; set; }
        public string InterestAHName { get; set; }
        public int ROI { get; set; }
        public int MeetingDay { get; set; }
        public List<MemberLoanApprovalDto> lstApprovals { get; set; }
    }

    public class MemberLoanApprovalDto
    {
        public string ApprovalType { get; set; }
        public int LoanSanctionAmount { get; set; }
        public int NoOfInstallments { get; set; }
        public string ApprovalComments { get; set; }
        public string Action { get; set; }
        public DateTime ActionDate { get; set; }
        public string ActionBy { get; set; }
    }

    public class MemberLoanApplicationViewDto
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

    public class LoanAccountHeadDto
    {
        public int InterestRateId { get; set; }
        public int InterestMasterId { get; set; }
        public int PrincipalAHId { get; set; }
        public int InterestAHId { get; set; }
        public string PrincipalAHName { get; set; }
        public string InterestAHName { get; set; }
        public int ROI { get; set; }
    }
    public class MemberLoanApplicationLookupDto
    {
        public int LoanMasterId { get; set; }
        public string LoanCode { get; set; }
        public string MemberName { get; set; }
        public string Purpose { get; set; }
        public decimal LoanAmountApplied { get; set; }
        public decimal MDSanctionAmount { get; set; }
        public string LoanType { get; set; }
        public decimal DisbursedAmount { get; set; }
        public DateTime DisbursementDate { get; set; }
        public int ApprovalLevel { get; set; }
        public string StatusCode { get; set; }
        public string Status { get; set; }
        public bool CanEdit { get; set; }
        public bool CanView { get; set; }
        public bool CanDelete { get; set; }
        public bool CanApprove { get; set; }
    }


    public class MemberLoanDisbursementDto
    {
        public int LoanMasterId { get; set; }
        public string LoanCode { get; set; }
        public int MemberID { get; set; }
        public string MemberName { get; set; }
        public DateTime LoanApplicationDate { get; set; }
        public decimal LoanAmountApplied { get; set; }
        public byte NoofInstallmentsProposed { get; set; }
        public int InterestRateID { get; set; }
        public int InterestMasterID { get; set; }
        public int ApprovalLevel { get; set; }
        public string StatusCode { get; set; }
        public string Status { get; set; }
        public string ReferenceNumber { get; set; }
        public string SourceOfFund { get; set; }
        public string TransactionMode { get; set; }
        public string PrincipleAHName { get; set; }
        public string InterestAHName { get; set; }
        public int ROI { get; set; }
        public int MeetingDay { get; set; }
        public List<MemberLoanApprovalDto> lstApprovals { get; set; }
        public string FrequencyMode { get; set; }
        public string ProjectName { get; set; }
        public string LoanPurposeName { get; set; }
        public decimal MDSanctionAmount { get; set; }
        public int MDInstallments { get; set; }
        public decimal DisbursedAmount { get; set; }
        public int NoOfInstallments { get; set; }
        public DateTime DisbursementDate { get; set; }
        public string ChequeNumber { get; set; }
        public DateTime ChequeDate { get; set; }
        public int BankEntryId { get; set; }
        public DateTime InstallmentStartFrom { get; set; }
        public DateTime FinalInstallmentDate { get; set; }
        public int MonthlyPrincipalDemand { get; set; }
        public string DisbursementComments { get; set; }
        public List<ScheduleDTO> Schedule { get; set; }
        public string PaymentVoucherNumber { get; set; }
        public int AccountMasterId { get; set; }
    }
}