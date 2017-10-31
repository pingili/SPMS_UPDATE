using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class GroupLoanDisbursementDto
    {
        public int LoanMasterId { get; set; }
        public string LoanCode { get; set; }
        public string LoanType { get; set; }
        public int MemberID { get; set; }
        public string MemberCode { get; set; }
        public string MemberName { get; set; }
        public int GroupID { get; set; }
        public string GroupName { get; set; }
        public string Groupcode { get; set; }
        public string Modee { get; set; }
        public string VillageCode { get; set; }
        public string VillageName { get; set; }
        public string ClusterCode { get; set; }
        public string ClusterName { get; set; }
        public DateTime LoanApplicationDate { get; set; }
        public string LoanPurpose { get; set; }
        public decimal LoanAmountApplied { get; set; }
        public byte NoofInstallmentsProposed { get; set; }
        public byte NoOfInstallments { get; set; }
        public string Mode { get; set; }
        public int UserID { get; set; }
        public decimal DisbursedAmount { get; set; }
        public string TransactionMode { get; set; }
        public DateTime DisbursementDate { get; set; }
        public int SLAccountNumber { get; set; }
        public string SLAccountName { get; set; }
        public int InterestMasterID { get; set; }
        public int InterestRateID { get; set; }
        public int GroupInterstRateID { get; set; }
        public decimal ROI { get; set; }
        public DateTime InstallmentStartFrom { get; set; }
        public DateTime FinalInstallmentDate { get; set; }
        public decimal MonthlyPrincipalDemand { get; set; }
        public int FundSourceID { get; set; }
        public int ProjectID { get; set; }
        public string LoanRefNumber { get; set; }
        public char VocherType { get; set; }
        public string AccountNumber { get; set; }
        public int BankName { get; set; }
        public string ChequeNumber { get; set; }
        public DateTime chequedate { get; set; }
        public DateTime LoanClosingDate { get; set; }
        public int LoanClosedBy { get; set; }
        public List<GroupMember> lstGroupmember { get; set; }
        public List<ChequeDetails> lstChequeDetails { get; set; }
        public int MeetingDay { get; set; }
        public int PrincipleAHId { get; set; }
        public string PrincipleAHName { get; set; }
        public string FederationBankAccountNumber { get; set; }
        public decimal OutStandingAmount { get; set; }
        public int InterestAHID { get; set; }
        public string InterestAHName { get; set; }
        public string StatusCode { get; set; }
        public string Status { get; set; }
        public int BankEntryId { get; set; }
        public string ProjectName { get; set; }

        //Vocher Genetationdto
        public decimal Amount { get; set; }
        public string VoucherNumber { get; set; }
        public DateTime TransactionDate { get; set; }
        //public string TransactionMode { get; set; }
        public int GroupBankEntryId { get; set; }
        //public string BankName { get; set; }
        //public string ChequeNumber { get; set; }
        public DateTime ChequeDate { get; set; }
        public int GRAccountMasterId { get; set; }
        public string GRVoucherNumber { get; set; }
        public int FPAccountMasterId { get; set; }
        public string FPVoucherNumber { get; set; }

        public List<ScheduleDTO> Schedule { get; set; }
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

    public class ScheduleDTO
    {
        public float PERIOD { get; set; }
        public DateTime PAYDATE { get; set; }
        public decimal PAYMENT { get; set; }
        public decimal Opening_Balance { get; set; }
        public decimal CURRENT_BALANCE { get; set; }
        public decimal INTEREST { get; set; }
        public decimal INTERESTRate { get; set; }
        public decimal PRINCIPAL { get; set; }
        public int DAYS { get; set; }
        public DateTime DisbursementDate { get; set; }

    }
    public class AdditionalSecurityDetailsDTO
    {

        public int LoanMasterID { get; set; }
        public int LoanSecurityDetails { get; set; }
        public string LoanSecurityCode { get; set; }
        public string Description { get; set; }
        public string LoanSecurityName { get; set; }
        public int LoanSecurityId { get; set; }

    }
    public class GuarantorDetailsDto
    {
        public int LoanGuarantorMemberID { get; set; }
        public int LoanMasterID { get; set; }
    }


}
