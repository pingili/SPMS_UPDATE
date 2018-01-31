using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BusinessEntities
{
    public class GroupMemberReceiptDto
    {
        public long AccountMasterID { get; set; }
        public string TransactionMode { get; set; }
        public int MemberId { get; set; }
        public string VoucherNumber { get; set; }
        public string VoucherRefNumber { get; set; }
        public DateTime TransactionDate { get; set; }
        public int CollectionAgent { get; set; }
        public int BankEntryId { get; set; }
        public string Narration { get; set; }
        public decimal TotalAmount { get; set; }
        public string ChequeNumber { get; set; }
        public DateTime ChequeDate { get; set; }
        public List<GroupMemberReceiptTranDto> Transactions { get; set; }
    }

    public class GroupMemberReceiptTranDto
    {
        [XmlIgnore]
        public string GLAccount { get; set; }
        [XmlIgnore]
        public string SLAccount { get; set; }
        [XmlElement("AHID")]
        public int SLAccountId { get; set; }
        [XmlElement("SLAHID")]
        public int SubAhId { get; set; }
        [XmlIgnore]
        public string ReferenceNumber { get; set; }
        [XmlIgnore]
        public int LoanMasterId { get; set; }
        public decimal Amount { get; set; }
        public decimal InterestDue { get; set; }
        public decimal PrincipleDue { get; set; }
    }

    public class GroupMemberDemandDto
    {
        public int Seq { get; set; }
        public int GLAhId { get; set; }
        public string GLAhName { get; set; }
        public int SLAhId { get; set; }
        public string SLAhName { get; set; }
        public int SubAhId { get; set; }
        public string SubAhName { get; set; }
        public int LoanMasterId { get; set; }
        public string ReferenceNumber { get; set; }
        public int Demand { get; set; }
    }

    public class GroupMemberReceiptLookupDto
    {
        public long AccountMasterID { get; set; }
        public DateTime TransactionDate { get; set; }
        public string VoucherNumber { get; set; }
        public decimal Amount { get; set; }
        public string MemberName { get; set; }
        public string StatusCode { get; set; }
        public string Narration { get; set; }
        public bool IsEdit { get; set; }
        public bool IsDelete { get; set; }
        public string LockStatus { get; set; }
        public string LockStatusCode { get; set; }
    }


    public class GroupMemberReceiptViewDto
    {
        public string TransactionMode { get; set; }
        public DateTime TransactionDate { get; set; }
        public string MemberName { get; set; }
        public string VoucherNumber { get; set; }
        public string VoucherRefNumber { get; set; }
        public string ChequeNumber { get; set; }
        public DateTime ChequeDate { get; set; }
        public string CollectionAgentName { get; set; }
        public string BankAccountHead { get; set; }
        public string Narration { get; set; }
        public decimal TotalAmount { get; set; }
        public List<GroupMemberReceiptTranDto> Transactions { get; set; }
    }
}
