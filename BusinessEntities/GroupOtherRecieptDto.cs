using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class GroupOtherRecieptDto
    {
        public string VoucherNumber { get; set; }
        public long AccountMasterID { get; set; }
        public string TransactionMode { get; set; }
        public DateTime TransactionDate { get; set; }
        public string VoucherRefNumber { get; set; }
        public int CollectionAgent { get; set; }
        public int GLAccountId { get; set; }
        public int SLAccountId { get; set; }
        public string ChequeNumber { get; set; }
        public DateTime ChequeDate { get; set; }
        public string Narration { get; set; }
        public int? BankEntryId { get; set; }
        public decimal Amount { get; set; }
        public int UserId { get; set; }

        public string AHName { get; set; }
        public string CollectionAgentName { get; set; }

        //public string ReceiptNumber { get; set; }
        //public string EmployeeCode { get; set; }
        //public string ReceiptRefNumber { get; set; }
        //public int? SubHeadID { get; set; }
        //public int TransactionType { get; set; }
        //public string Type { get; set; }
        //public string BankAccountName { get; set; }
        //public string RefId { get; set; }
        //public bool IsGroup { get; set; }
        //public int? GroupID { get; set; }
        //public string GroupName { get; set; }
        //public string GroupCode { get; set; }
        //public int? ClusterID { get; set; }
        //public string ClusterName { get; set; }
        //public int UserID { get; set; }
        //public string AccountName { get; set; }
        //public string AccountCode { get; set; }
        //public decimal Balance { get; set; }
        //public decimal CrTotal { get; set; }
        //public bool IsPairedRecord { get; set; }
        //public List<AddAmountDto> Addamount { get; set; }
    }

    public class GroupOtherRecieptUploadValidateInfo
    {
        public List<UGOREmployeeDto> Employees { get; set; }
        public List<UGORBankDto> GroupBanks { get; set; }
        public List<UGORMeetingDto> GroupMeetings { get; set; }
        public List<UGORAccountHeadDto> SlAccountHeads { get; set; }

        public int MeetingMonth { get; set; }
        public int MeetingYear { get; set; }
    }

    public class UGOREmployeeDto
    {
        public int EmployeeID { get; set; }
        public string EmployeeCode { get; set; }
    }
    public class UGORBankDto
    {
        public int BankEntryId { get; set; }
        public string BankCode { get; set; }
    }

    public class UGORMeetingDto
    {
        public DateTime MeetingDate { get; set; }
        public bool isConducted { get; set; }
    }

    public class UGORAccountHeadDto
    {
        public int AHId { get; set; }
        public string AHCode { get; set; }
    }
}