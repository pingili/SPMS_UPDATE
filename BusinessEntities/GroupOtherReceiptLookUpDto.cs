using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class GroupOtherReceiptLookUpDto
    {
        public long AccountMasterID { get; set; }
        public string VoucherNumber { get; set; }
        public string VoucherRefNumber { get; set; }
        public string TransactionMode { get; set; }
        public string AHName { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal Amount { get; set; }
        public string Narration { get; set; }
        public string StatusCode { get; set; }
        public string Status { get; set; }
        public bool IsEdit { get; set; }
        public bool IsDelete { get; set; }
        public string LockStatus { get; set; }
        public string LockStatusCode { get; set; }
    }
}
