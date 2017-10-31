using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MFIS.Web.Areas.Federation.Models
{
    public class LoanSecurityModel
    {
        public int LoanSecurityID { get; set; }
        public int Type { get; set; }
        public string LoanSecurityCode { get; set; }
        public string LoanSecurityName { get; set; }
        public int StatusID { get; set; }
    }
}