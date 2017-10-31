using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MFIS.Web.Models
{
    public class TrialBalanceModel
    {
        public string Particulars { get;set; }
        public string OrganizationName { get; set; }
        public string OrganizationAddress { get; set; }
        public double Openingbalance { get; set; }
        public double Transactions { get; set; }
        public double Closing { get; set; }
        public string Ahcode { get; set; }
        public string Accounthaedname { get; set; }
        public double Debit1 { get; set; }
        public double Credit1 { get; set; }
        public double Debit2 { get; set; }
        public double Credit2 { get; set; }
        public double Debit3 { get; set; }
        public double Credit3 { get; set; }

        public double OpeningBalDrSum { get; set; }
        public double OpeningBalCrSum { get; set; }


        public double TranBalDrSum { get; set; }
        public double TranBalCrSum { get; set; }

        public double ClosingBalDrSum { get; set; }
        public double ClosingBalCrSum { get; set; }
    }
}