using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MFIS.Web.Models
{
    public class GeneralLedgerModel
    {
        //public DateTime Date { get; set; }
        //public string VoucherType { get; set; }
        //public int VoucherNo { get; set; }
        //public string TransactionAHCode { get; set; }
        //public double Debit { get; set; }
        //public double Credit { get; set; }
        //public double Balance { get; set; }
        //public string BalanceType { get; set; }
        public int AHID { get; set; }
        public string AHCode { get; set; }
        public long AccountTranID { get; set; }
        public string AHName { get; set; }
        public string TDate { get; set; }
        public decimal DrAmount { get; set; }
        public decimal CrAmount { get; set; }
        public decimal Balance { get; set; }
    }
}