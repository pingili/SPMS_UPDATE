using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
   public class DepositDto
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
        public string TransactionMode { get; set; }
        public int TransactionType { get; set; }
        public decimal Amount { get; set; }
        public string ChequeNumber { get; set; }
        public string Type { get; set; }
        public DateTime ChequeDate { get; set; }
        public int? BankAccount { get; set; }
        public int SLAccountId { get; set; }
        public string SLAccountName { get; set; }
        public string BankAccountName { get; set; }
        public string Narration { get; set; }
        public string RefId { get; set; }
        public bool IsGroup { get; set; }
        public int? GroupID { get; set; }
        public int UserID { get; set; }
        public string AccountName { get; set; }
        public decimal CrTotal { get; set; }
        public List<AddAmountDto> Addamount { get; set; }
    }
}
