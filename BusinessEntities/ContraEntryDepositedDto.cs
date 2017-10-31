using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BusinessEntities
{
    public class ContraEntryDepositedDto
    {
        public long AccountMasterID { get; set; }
        public DateTime TransactionDate { get; set; }
        public string VoucherNumber { get; set; }
        public string VoucherRefNumber { get; set; }
        public string PartyName { get; set; }
        public int EmployeeID { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public int AHID { get; set; }
        public string AHName { get; set; }
        public int SubHeadID { get; set; }
        public int TransactionType { get; set; }
        public decimal Amount { get; set; }
        public string TransactionMode { get; set; }
        //public string ChequeNumber { get; set; }
       // public DateTime ChequeDate { get; set; }
        public int? BankAccount { get; set; }
        public string BankAccountName { get; set; }
        public string Narration { get; set; }
        public int StatusID { get; set; }
        public bool IsGroup { get; set; }
        public bool IsFederation { get; set; }
        public int GroupID { get; set; }
        public string GroupName { get; set; }
        public int ClusterId { get; set; }
        public string ClusterName { get; set; }
        public int CodeSno { get; set; }
        public int UserID { get; set; }
        public decimal CrTotal { get; set; }

       // [XmlIgnore]
       // public int AccountTranID { get; set; }
        public decimal CrAmount { get; set; }
        public decimal DrAmount { get; set; }
        public bool IsActive { get; set; }
        public string SLACNo { get; set; }
        public string AccountHead { get; set; }

        public List<ContraEntryDepositedTransactionsDto> contraEntryDepositedTransactions { get; set; }
    }
    public class ContraEntryDepositedTransactionsDto
    {
        public int AHID { get; set; }
        [XmlIgnore]
        public string BankAccount { get; set; }
        [XmlIgnore]
        public string BankAccountName { get; set; }
        [XmlIgnore]
        public int AmountId { get; set; }
        public decimal CrAmount { get; set; }
        public decimal DrAmount { get; set; }
        public decimal ClosingBalance { get; set; }
        public string Narration { get; set; }
        public string AHCode { get; set; }
        public string AHName { get; set; }
        public string Type { get; set; }
        public long AccountTranID { get; set; }
        public bool IsMaster { get; set; }
    }
    public class ContraEntryDepositedLookupDto
    {
        public long AccountMasterID { get; set; }
        public string VoucherNumber { get; set; }
        public string AHName { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal Amount { get; set; }
        public string Narration { get; set; }
        public string StatusCode { get; set; }
        public string Status { get; set; }
        public bool IsEdit { get; set; }
        public bool IsDelete { get; set; }
        public string LockStatusCode { get; set; }
        public string LockStatus { get; set; }
    }
}
