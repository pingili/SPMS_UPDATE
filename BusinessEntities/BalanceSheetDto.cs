using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class BalanceSheetDto
    {
        public int AHID { get; set; }
        public long AccountTranID { get; set; }
        public string AHName { get; set; }
        public string AHCode { get; set; }
        public string TDate { get; set; }
        public decimal DrAmount { get; set; }
        public decimal CrAmount { get; set; }
        public decimal Balance { get; set; }
        public double Credit1  { get; set; }
        public double Debit1 { get; set; }
        public double Credit2 { get; set; }
        public double Debit2 { get; set; }

        public List<TrialBalanceDto> lstAssects { get; set; }
        public List<TrialBalanceDto> lstLiabilities { get; set; }

    }
}
