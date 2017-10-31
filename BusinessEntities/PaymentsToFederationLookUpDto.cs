using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
   public class PaymentsToFederationLookUpDto
    {
        public long AccountMasterID { get; set; }
        public DateTime TransactionDate { get; set; }
        public string VoucherNumber { get; set; }
        public string BranchName { get; set; }
        public string GroupCode { get; set; }
        public decimal Amount { get; set; }
        public int EmployeeID { get; set; }
        public string EmployeeCode { get; set; }
        public string StatusCode { get; set; }
        public string Status { get; set; }
    }

}
