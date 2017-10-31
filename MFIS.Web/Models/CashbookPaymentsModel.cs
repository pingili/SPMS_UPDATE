﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MFIS.Web.Models
{
    public class CashbookPaymentsModel
    {
       public int MemberID { get; set; }
        public string MemberName { get; set; }
        public int LoanNumber { get; set; }
        public string VoucherNo { get; set; }
        public double TotalPayment { get; set; }
        public string AHCode1 { get; set; }
        public string AHCode2 { get; set; }
        public string AHCode3 { get; set; }
        public string AHCode4 { get; set; }
        public string AHCode5 { get; set; }
       public double LoanPrincipal { get; set; }

        
    }
}