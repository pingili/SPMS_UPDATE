using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class BankMasterLookupDto
    {
        public int BankEntryID { get; set; }
        public string BankCode { get; set; }
        public string BranchName { get; set; }
        public string BankName { get; set; }
        public string IFSC { get; set; }
        public string AccountNumber { get; set; }
        public string AccountType { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public bool isMasterEntry { get; set; }
        public int StatusID { get; set; }
        public string status { get; set; }
        public string StatusCode { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public int ObjectID { get; set; }
        public int IsConsumed { get; set; }
        public string isFederation { get; set; }
        public string ClusterName { get; set; }
        public string GroupName { get; set; }
        public int GroupId { get; set; }
        public bool IsBankAlreadyConsumed
        {
            get
            {
                return IsConsumed > 0;
            }
        }
    }
}
