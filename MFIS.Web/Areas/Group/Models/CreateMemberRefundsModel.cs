using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MFIS.Web.Areas.Group.Models
{
    public class CreateMemberRefundsModel
    {
        public string VoucherNumber { get; set; }
        public string Narration { get; set; }
        public decimal DrAmount { get; set; }
        public string EmployeeCode { get; set; }
        public DateTime TransactionDate { get; set; }
        public string EmployeeName { get; set; }
        public string MemberName { get; set; }
        public string MemberNo { get; set; }
        public int MemberID { get; set; }
        public string VoucherRefNumber { get; set; }
        public string PartyName { get; set; }
        public int GroupID { get; set; }
        public string Gropus { get; set; }
        public string SLACNo { get; set; }
        public string AccountHead { get; set; }
        public decimal OpeningBalance { get; set; }
        public decimal ClosingBalance { get; set; }
        public string GroupName { get; set; }
        public int ClusterID { get; set; }




    }
}