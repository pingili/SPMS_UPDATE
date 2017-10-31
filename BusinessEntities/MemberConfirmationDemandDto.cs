using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class MemberConfirmationDto
    {
        public int MemberId { get; set; }
        public string MemberCode { get; set; }
        public string MemberName { get; set; }
        public int PSPrincipal { get; set; }
        public int PSInt { get; set; }
        public int SSPrincipal { get; set; }
        public int SSInt { get; set; }
        public DateTime SLoanDate { get; set; }
        public int SLoanPrincipal { get; set; }
        public int SLoanInt { get; set; }
        public DateTime BLoanDate { get; set; }
        public int BLoanPrincipal { get; set; }
        public int BLoanInt { get; set; }
        public DateTime HLoanDate { get; set; }
        public int HLoanPrincipal { get; set; }
        public int HLoanInt { get; set; }
    }

    public class MemberDemandSheetDto
    {
        public int MemberId { get; set; }
        public string MemberCode { get; set; }
        public string MemberName { get; set; }
        public int PSDemand { get; set; }
        public int SSDemand { get; set; }
        public int SLoanPrincipal { get; set; }
        public int SLoanInt { get; set; }
        public int BLoanPrincipal { get; set; }
        public int BLoanInt { get; set; }
        public int HLoanPrincipal { get; set; }
        public int HLoanInt { get; set; }
        public int TotalDemand { get; set; }
    }
}