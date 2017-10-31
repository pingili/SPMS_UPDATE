using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
  public  class GeneralLedgerDto
    {
        public DateTime Date { get; set; }
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
        public DateTime TDate { get; set; }
        public decimal DrAmount { get; set; }
        public decimal CrAmount { get; set; }
        public decimal Balance { get; set; }
    }
}
