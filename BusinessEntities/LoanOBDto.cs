using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{

    public class LoanOBLookup
    {
        public int LoanMasrterID { get; set; }
        public string SLAccountNumber { get; set; }
        public decimal? LoanAmountGiven { get; set; }
        public string LoanDisbursedDate { get; set; }
        public decimal? PrincipalOutstanding { get; set; }
        public decimal? PrincipalDue { get; set; }
        public decimal? InterestDue { get; set; }
        public decimal? MonthlyDemand { get; set; }
        public decimal? ROI { get; set; }
        public int? GroupID { get; set; }
        public string  GroupName { get; set; }
        public int MemberID { get; set; }
        public string MemberName { get; set; }
    }
    public class LoanOBLookupPivot
    {
        public int LoanMasrterID { get; set; }
        public decimal? LoanAmountGiven1 { get; set; }
        public decimal? PrincipalOutstanding1 { get; set; }
        public decimal? LoanAmountGiven2 { get; set; }
        public decimal? PrincipalOutstanding2 { get; set; }
        public decimal? LoanAmountGiven3 { get; set; }
        public decimal? PrincipalOutstanding3 { get; set; }
        public int? MemberID { get; set; }
        public int? GroupID { get; set; }
        public string MemberName { get; set; }
        public string GroupName { get; set; }
    }

    public class LoanOBByLoanDto
    {
        public int GroupId { get; set; }
        public int MemberId { get; set; }
        public int UserID { get; set; }
        public char LoanType { get; set; }
        public int LoanMasrterID { get; set; }


        public int PrincipalAHID { get; set; }
        public string SLAccountNumber { get; set; }
        public int SLAccountNumberAHID { get; set; }
        public decimal LoanAmountGiven { get; set; }
        public string LoanDisbursedDate { get; set; }
        public int Purpose { get; set; }
        public int Fundsource { get; set; }
        public int Project { get; set; }
        public int Period { get; set; }
        public string FinalInstallmentDate { get; set; }
        public string LastPaidDate { get; set; }
        public decimal PrincipalOutstanding { get; set; }
        public decimal InterestDue { get; set; }
        public decimal Total { get; set; }
        public decimal PrincipalOD { get; set; }
        public decimal InterestOD { get; set; }
        public int Interest { get; set; }
        public decimal MonthlyDemand { get; set; }
        public int DueDay { get; set; }
        public int ROI { get; set; }


    }
    public class LoanOBDto
    {
        public int GroupId { get; set; }
        public int MemberID { get; set; }
        public int UserID { get; set; }
        public char LoanType { get; set; }
        public int DueDay { get; set; }
        public int VillageID { get; set; }
        public int ClusterID { get; set; }

        public string AHCode1 { get; set; }
        public string AHCode2 { get; set; }
        public string AHCode3 { get; set; }
        public string AHCode4 { get; set; }

        public string AHName1 { get; set; }
        public string AHName2 { get; set; }
        public string AHName3 { get; set; }
        public string AHName4 { get; set; }

        public int PrincipalAHID1 { get; set; }
        public int PrincipalAHID2 { get; set; }
        public int PrincipalAHID3 { get; set; }
        public int PrincipalAHID4 { get; set; }

        public int LoanMasrterID1 { get; set; }
        public int LoanMasrterID2 { get; set; }
        public int LoanMasrterID3 { get; set; }
        public int LoanMasrterID4 { get; set; }

        public string SLAccountNumber1 { get; set; }
        public string SLAccountNumber2 { get; set; }
        public string SLAccountNumber3 { get; set; }
        public string SLAccountNumber4 { get; set; }

        public int SLAccountNumberAHID1 { get; set; }
        public int SLAccountNumberAHID2 { get; set; }
        public int SLAccountNumberAHID3 { get; set; }
        public int SLAccountNumberAHID4 { get; set; }

        public decimal LoanAmountGiven1 { get; set; }
        public decimal LoanAmountGiven2 { get; set; }
        public decimal LoanAmountGiven3 { get; set; }
        public decimal LoanAmountGiven4 { get; set; }

        public string LoanDisbursedDate1 { get; set; }
        public string LoanDisbursedDate2 { get; set; }
        public string LoanDisbursedDate3 { get; set; }
        public string LoanDisbursedDate4 { get; set; }

        public int Purpose1 { get; set; }
        public int Purpose2 { get; set; }
        public int Purpose3 { get; set; }
        public int Purpose4 { get; set; }

        public int Fundsource1 { get; set; }
        public int Fundsource2 { get; set; }
        public int Fundsource3 { get; set; }
        public int Fundsource4 { get; set; }

        public int Project1 { get; set; }
        public int Project2 { get; set; }
        public int Project3 { get; set; }
        public int Project4 { get; set; }

        public int Period1 { get; set; }
        public int Period2 { get; set; }
        public int Period3 { get; set; }
        public int Period4 { get; set; }

        public string FinalInstallmentDate1 { get; set; }
        public string FinalInstallmentDate2 { get; set; }
        public string FinalInstallmentDate3 { get; set; }
        public string FinalInstallmentDate4 { get; set; }

        public string LastPaidDate1 { get; set; }
        public string LastPaidDate2 { get; set; }
        public string LastPaidDate3 { get; set; }
        public string LastPaidDate4 { get; set; }

        public decimal PrincipalOutstanding1 { get; set; }
        public decimal PrincipalOutstanding2 { get; set; }
        public decimal PrincipalOutstanding3 { get; set; }
        public decimal PrincipalOutstanding4 { get; set; }

        public decimal InterestDue1 { get; set; }
        public decimal InterestDue2 { get; set; }
        public decimal InterestDue3 { get; set; }
        public decimal InterestDue4 { get; set; }

        public decimal Total1 { get; set; }
        public decimal Total2 { get; set; }
        public decimal Total3 { get; set; }
        public decimal Total4 { get; set; }

        public decimal PrincipalOD1 { get; set; }
        public decimal PrincipalOD2 { get; set; }
        public decimal PrincipalOD3 { get; set; }
        public decimal PrincipalOD4 { get; set; }

        public decimal InterestOD1 { get; set; }
        public decimal InterestOD2 { get; set; }
        public decimal InterestOD3 { get; set; }
        public decimal InterestOD4 { get; set; }

        public int Interest1 { get; set; }
        public int Interest2 { get; set; }
        public int Interest3 { get; set; }
        public int Interest4 { get; set; }

        public decimal MonthlyDemand1 { get; set; }
        public decimal MonthlyDemand2 { get; set; }
        public decimal MonthlyDemand3 { get; set; }
        public decimal MonthlyDemand4 { get; set; }

        public int ROI1 { get; set; }
        public int ROI2 { get; set; }
        public int ROI3 { get; set; }
        public int ROI4 { get; set; }

        public bool IsDisbursed1 { get; set; }
        public bool IsDisbursed2 { get; set; }
        public bool IsDisbursed3 { get; set; }
        public bool IsDisbursed4 { get; set; }
    }
}