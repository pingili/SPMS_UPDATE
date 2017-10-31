using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BusinessEntities
{
    public class ReceiptMasterDto 
    { 
        public long AccountMasterID { get; set; }
        public int AHID { get; set; }
        public string AHName { get; set; }
        public int CodeSno { get; set; }
        public DateTime TransactionDate { get; set; }
        public string VoucherNumber { get; set; }
        public int? MemberId { get; set; }
        public string MemberCode { get; set; }
        public string MemberName { get; set; }
        public string JournalEntryRefNo { get; set; }
        public string ReceiptNumber { get; set; }
        public string VoucherRefNumber { get; set; }
        public string PartyName { get; set; }
        public int EmployeeID { get; set; }
        public string ReferenceNumber { get; set; }
        public string AHCode { get; set; }
       // public string AHName { get; set; }
        
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }

        
        
        public int? SubHeadID { get; set; }
        public int TransactionType { get; set; }
        public decimal Amount { get; set; }
        public string TransactionMode { get; set; }
        public string ChequeNumber { get; set; }
        public DateTime? ChequeDate { get; set; }
        public int? BankAccount { get; set; }
        public string Narration { get; set; }
        public int StatusID { get; set; }
        public bool IsGroup { get; set; }
        public int? GroupID { get; set; }
        public int ClusterID { get; set; }
        public string GroupCode { get; set; }
        public string GroupName { get; set; }
        public bool IsActive { get; set; }
        public string AccountHead { get; set; }
        public decimal CrTotal { get; set; }
        public decimal DrTotal { get; set; }
       // public decimal DrAmount { get; set; }
       

         //Added Non Existing Columns in the database
        public string GroupBankAccountNumber { get; set; }
        public string GroupBankAccountName { get; set; }
        public string FederationBankAccountNumber { get; set; }
        public string FederationBankAccountName { get; set; }
        public int VillageID { get; set; }
        //public int ClusterID { get; set; }

        public List<ReceiptTranscationDto> lstGroupReceiptTranscationDto { get; set; }

        public List<RepaymentDto> lstRepeyment{get;set;}

      
        //Account Head Table Properties
        public string TE_AHName { get; set; }
        public int AHType { get; set; }
        public bool IsMemberTransaction { get; set; }
        public int AHLevel { get; set; }
        public bool IsFederation { get; set; }
        public int UserID { get; set; }
        public decimal CurrentYearBalance { get; set; }
        public bool IsMaster { get; set; }
    }
    public class ReceiptTranscationDto
    {
        public int? AHID { get; set; }
        public long AccountMasterID { get; set; }
        public string AHCode { get; set; }
        public string AHName { get; set; }
        public string Type { get; set; }
        [XmlIgnore]
        public bool ISLAccount { get; set; }
        [XmlIgnore]
        public string SLAcNo { get; set; }
        [XmlIgnore]
        public string SLAccName { get; set; }
        [XmlIgnore]
        public decimal OpeningBalance { get; set; }
        [XmlIgnore]
        public decimal ClosingBalance { get; set; }
        public long AccountTranID { get; set; }
        [XmlIgnore]
        public int ParentAHID { get; set; }
        public decimal CrAmount { get; set; }
        public decimal DrAmount { get; set; }
        public decimal Balance { get; set; }
        public bool IsMaster { get; set; }
        
        public List<RepaymentDto> lstRepeyment { get; set; }
    }
}
