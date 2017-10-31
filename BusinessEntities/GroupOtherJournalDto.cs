using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BusinessEntities
{
    public class GroupJournalDto
    {
        public int AccountMasterID { get; set; }
        public string VoucherNumber { get; set; }
        public int MemberId { get; set; }
        public string MemberName { get; set; }
        public DateTime TransactionDate { get; set; }
        public string EmployeeName { get; set; }
        public int OnBehalfOfEmpId { get; set; }
        public string OnBehalfOfEmpName { get; set; }
        public string VocherRefNumber { get; set; }
        public int FGLAccountId { get; set; }
        public int FSLAccountId { get; set; }
        public string FGLAccountName { get; set; }
        public string FSLAccountName { get; set; }
        public string CrDr { get; set; }
        public decimal FAmount { get; set; }
        public string Narration { get; set; }
        public List<GroupJournalTranDto> TransactionsList { get; set; }

    }
    public class GroupJournalTranDto
    {
        [XmlIgnore]
        public int GLAccountId { get; set; }
        public int SLAccountId { get; set; }
        [XmlIgnore]
        public string GLAccount { get; set; }
        [XmlIgnore]
        public string SLAccount { get; set; }
        public decimal Amount { get; set; }
    }

    public class GroupJournalLookUpDto
    {
        public long AccountMasterID { get; set; }
        public string VoucherNumber { get; set; }
        public string MemberName { get; set; }
        public string AHName { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal Amount { get; set; }
        public string Narration { get; set; }
        public string StatusCode { get; set; }
        public string Status { get; set; }
        public string AmountTranMode { get; set; }
        public string LockStatusCode { get; set; }
        public bool IsEdit { get; set; }
        public bool IsDelete { get; set; }
        public string LockStatus { get; set; }
        //public string LockStatusCode { get; set; }


    }
}
