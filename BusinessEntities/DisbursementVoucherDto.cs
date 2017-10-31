using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class DisbursementVoucherDto
    {
        public decimal Amount { get; set; }
        public string VoucherNumber { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionMode { get; set; }
        public int BankID { get; set; }
        public string BankName { get; set; }
        public string ChequeNumber { get; set; }
        public DateTime ChequeDate { get; set; }
    }
}
