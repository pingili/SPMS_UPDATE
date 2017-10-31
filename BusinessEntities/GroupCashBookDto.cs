using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class GroupCashBookDto
    {
        public string MemberName { get; set; }
        public string OrganizationName { get; set; }
        public string MemberCode { get; set; }
        public string GroupCode { get; set; }
        public string GroupName { get; set; }
        public string Neighbourhood { get; set; }
        public string Village { get; set; }
        public string Cluster { get; set; }
        public string GroupLeader1 { get; set; }
        public string GroupLeader2 { get; set; }
        public string StaffName { get; set; }
        public List<cashreceiptsdto> lstcashreceiptsdto { get; set; }
        public List<cashvoucherdto> lstcashvoucherdto { get; set; }

    }
    public class cashreceiptsdto
    {
        public decimal OpeiningBalance { get; set; }
        public decimal LoanNumber { get; set; }
        public string AHCode { get; set; }
        public string ReceiptNumber { get; set; }
        public string TotalReceipt { get; set; }
        public string ReceiptTotal { get; set; }
        public decimal LoanPrincipal { get; set; }
        public decimal LoanInterest { get; set; }
    }
    public class cashvoucherdto
    {
        public string VoucherNumber { get; set; }
        public string LoanNumber { get; set; }
        public string AHCode { get; set; }
        public string TotalPayment { get; set; }
        public decimal LoanPrincipal { get; set; }
        public string ClosingBalance { get; set; }
        public string Paymentstotal { get; set; }
    }
}
