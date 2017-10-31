using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class ContraEntryModel
    {
        public class ContraEntryDto
        {
            public int AccountMasterID { get; set; }
            public DateTime TransactionDate { get; set; }
            public string VoucherNumber { get; set; }
            public string VoucherRefNumber { get; set; }
            public string PartyName { get; set; }
            public int EmployeeID { get; set; }
            public string EmployeeCode { get; set; }
            public string EmployeeName { get; set; }
            public int AHID { get; set; }
            public int SubHeadID { get; set; }
            public int TransactionType { get; set; }
            public decimal Amount { get; set; }
            public string TransactionMode { get; set; }
            public string ChequeNumber { get; set; }
            public DateTime ChequeDate { get; set; }
            public int BankAccount { get; set; }
            public string BankAccountName { get; set; }
            public string Narration { get; set; }
            public int StatusID { get; set; }
            public bool IsGroup { get; set; }
            public int GroupID { get; set; }

            public int AccountTranID { get; set; }
            public decimal CrAmount { get; set; }
            public decimal DrAmount { get; set; }
            public bool IsActive { get; set; }
            public string SLACNo { get; set; }
            public string AccountHead { get; set; }

            public List<ContraEntryTransactionsDto> ContraEntryTransactions { get; set; }
        }

        public class ContraEntryTransactionsDto
        {
            public int BankAccount { get; set; }
            public string BankAccountName { get; set; }
            public decimal CrAmount { get; set; }
            public decimal DrAmount { get; set; }
        }
    }
}
