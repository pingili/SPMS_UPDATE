using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MFIS.Web.Areas.Federation.Models
{
    public class GeneralPaymentsModel
    {
        public int AccountMasterID { get; set; }
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public DateTime TransactionDate { get; set; }
        public string VoucherNumber { get; set; }
        public string VoucherRefNumber { get; set; }
        public string  PartyName{ get; set; }
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
        public string Narration { get; set; }
    }
}