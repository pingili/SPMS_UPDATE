using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class GeneralPaymentsDto
    {
        public long AccountMasterID { get; set; }

        public DateTime TransactionDate { get; set; }
        public string VoucherNumber { get; set; }
        public string VoucherRefNumber { get; set; }
        public string ReceiptNumber { get; set; }
        public string ReceiptRefNumber { get; set; }
        public string PartyName { get; set; }
        public int EmployeeID { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public int AHID { get; set; }
        public int? SubHeadID { get; set; }
        public string GroupCode { get; set; }
        public string GroupName { get; set; }
        public string ClusterName { get; set; }
        public string TransactionMode { get; set; }
        public int TransactionType { get; set; }
        public decimal Amount { get; set; }
        public string ChequeNumber { get; set; }
        public string Type { get; set; }
        public DateTime ChequeDate { get; set; }
        public int? BankAccount { get; set; }
        public string BankAccountName { get; set; }
        public string Narration { get; set; }
        public string RefId { get; set; }
        public bool IsGroup { get; set; }
        public int? GroupID { get; set; }
        public int? ClusterID { get; set; }
        public int UserID { get; set; }
        public string AccountName { get; set; }
        public string BAHCode { get; set; }
        public int SLAccountId { get; set; }
        public string SLAccountName { get; set; }
        public decimal Balance { get; set; }
        public decimal CrTotal { get; set; }
        public List<AddAmountDto> Addamount { get; set; }
        public bool IsMaster { get; set; }
        public bool IsPairedRecord { get; set; }
    }
    public class AddAmountDto
    {
        public long AccountTranID { get; set; }
        public int AHID { get; set; }
        public int AmountId { get; set; }
        public string AHCode { get; set; }
        public string AHName { get; set; }
        public decimal DrAmount { get; set; }
        public decimal CrAmount { get; set; }
        public string SLAccount { get; set; }
        public string SLAccountName { get; set; }
        public decimal Balance { get; set; }
        public string Type { get; set; }
        public bool IsMaster { get; set; }
    }

    public class GroupGeneralPaymentDto
    {
        public string VoucherNumber { get; set; }

        public long AccountMasterID { get; set; }

        public string TransactionMode { get; set; }

        public DateTime TransactionDate { get; set; }

        public string VoucherRefNumber { get; set; }

        public int CollectionAgent { get; set; }

        public string CollectionAgentName { get; set; }

        public string ChequeNumber { get; set; }

        public DateTime ChequeDate { get; set; }

        public string Narration { get; set; }

        public int BankEntryId { get; set; }

        public string ToAhNameForView { get; set; }

        public decimal TotalAmount { get; set; }

        public List<GroupGeneralPaymentTranDto> TransactionsList { get; set; }
    }
    public class GroupGeneralPaymentTranDto
    {
        public int GLAccountId { get; set; }
        public int SLAccountId { get; set; }
        public string GLAccount { get; set; }
        public string SLAccount { get; set; }
        public decimal Amount { get; set; }
    }

    public class GroupMemberPaymentDto
    {
        public string VoucherNumber { get; set; }

        public long AccountMasterID { get; set; }

        public int MemberId { get; set; }

        public string MemberName { get; set; }

        public string TransactionMode { get; set; }

        public DateTime TransactionDate { get; set; }

        public string VoucherRefNumber { get; set; }

        public int CollectionAgent { get; set; }

        public string CollectionAgentName { get; set; }

        public string ChequeNumber { get; set; }

        public DateTime ChequeDate { get; set; }

        public string Narration { get; set; }

        public int BankEntryId { get; set; }

        public string ToAhNameForView { get; set; }

        public decimal TotalAmount { get; set; }

        public List<GroupMemberPaymentTranDto> TransactionsList { get; set; }
    }
    public class GroupMemberPaymentTranDto
    {
        public int GLAccountId { get; set; }
        public int SLAccountId { get; set; }
        public string GLAccount { get; set; }
        public string SLAccount { get; set; }
        public decimal Amount { get; set; }
    }

    public class GroupMemberPaymentLookupDto
    {
        public long AccountMasterID { get; set; }
        public string VoucherNumber { get; set; }
        public string MemberName { get; set; }
        public string AHName { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal Amount { get; set; }
        public string Narration { get; set; }
        public string StatusCode { get; set; }
        public string Status { get; set; }
        public bool IsEdit { get; set; }
        public bool IsDelete { get; set; }
        public string LockStatus { get; set; }
        public string LockStatusCode { get; set; }
    }
}