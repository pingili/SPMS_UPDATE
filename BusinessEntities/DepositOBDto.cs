using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class DepositOBDto
    {
        public int GroupId { get; set; }
        public int MemberId { get; set; }
        public string MemberCode { get; set; }
        public int UserID { get; set; }
        public char LoanType { get; set; }
        public int DueDay { get; set; }
        public int VillageID { get; set; }
        public int ClusterID { get; set; }

        public int Id1 { get; set; }
        public int Id2 { get; set; }
        public int Id3 { get; set; }
        public int Id4 { get; set; }


        public int Interest1 { get; set; }
        public int Interest2 { get; set; }
        public int Interest3 { get; set; }
        public int Interest4 { get; set; }

        public string SLAccountNumber1 { get; set; }
        public string SLAccountNumber2 { get; set; }
        public string SLAccountNumber3 { get; set; }
        public string SLAccountNumber4 { get; set; }

        public int SLAccountNumberAHID1 { get; set; }
        public int SLAccountNumberAHID2 { get; set; }
        public int SLAccountNumberAHID3 { get; set; }
        public int SLAccountNumberAHID4 { get; set; }

        public decimal DepositBalance1 { get; set; }
        public decimal DepositBalance2 { get; set; }
        public decimal DepositBalance3 { get; set; }
        public decimal DepositBalance4 { get; set; }

        public decimal IneterestDue1 { get; set; }
        public decimal IneterestDue2 { get; set; }
        public decimal IneterestDue3 { get; set; }
        public decimal IneterestDue4 { get; set; }

        public DateTime DepositDate1 { get; set; }
        public DateTime DepositDate2 { get; set; }
        public DateTime DepositDate3 { get; set; }
        public DateTime DepositDate4 { get; set; }

        public int Period1 { get; set; }
        public int Period2 { get; set; }
        public int Period3 { get; set; }
        public int Period4 { get; set; }

        public DateTime MeturityDate1 { get; set; }
        public DateTime MeturityDate2 { get; set; }
        public DateTime MeturityDate3 { get; set; }
        public DateTime MeturityDate4 { get; set; }

        public DateTime LastPaidDate1 { get; set; }
        public DateTime LastPaidDate2 { get; set; }
        public DateTime LastPaidDate3 { get; set; }
        public DateTime LastPaidDate4 { get; set; }

        public decimal DemandAmountPerMonth1 { get; set; }
        public decimal DemandAmountPerMonth2 { get; set; }
        public decimal DemandAmountPerMonth3 { get; set; }
        public decimal DemandAmountPerMonth4 { get; set; }


        public string AHCode1 { get; set; }
        public string AHCode2 { get; set; }
        public string AHCode3 { get; set; }
        public string AHCode4 { get; set; }

        public string AHName1 { get; set; }
        public string AHName2 { get; set; }
        public string AHName3 { get; set; }
        public string AHName4 { get; set; }

        public int ROI1 { get; set; }
        public int ROI2 { get; set; }
        public int ROI3 { get; set; }
        public int ROI4 { get; set; }
    }

    public class DepositOBBySlAccountDto
    {
        public int GroupId { get; set; }
        public int? MemberId { get; set; }
        public int UserID { get; set; }
        public int ID { get; set; }
        public bool IsMemberDeposit { get; set; }
        public string SLAccountNumber { get; set; }
        public int SLAccountNumberAHID { get; set; }
        public decimal DepositAmount { get; set; }
        public decimal InterestAcmount { get; set; }
        public DateTime DepositDate { get; set; }
        public int Period { get; set; }
        public DateTime MeturityDate { get; set; }
        public DateTime LastPaidDate { get; set; }
        public decimal DemandAmountPerMonth { get; set; }
        public int ROI { get; set; }
        public int InterestMasterID { get; set; }
        public int InterestRateID { get; set; }
      
    }

    public class DepositOBLookup
    {

        public int    GroupId { get; set; }
        public int MemberId { get; set; }
        public string MemberName { get; set; }
        public string GroupName { get; set; }
        public string SLAccountNumber { get; set; }
        public decimal? DepositAmount { get; set; }
        public string InterestDue { get; set; }
        public decimal? Interest { get; set; }
        public decimal? MonthlyDemand { get; set; }
        public decimal? ROI { get; set; }
    }
    public class DepositOBPPivot
    {
        public int MemberId { get; set; }
        public string MemberName { get; set; }
        public decimal? DepositAmount1 { get; set; }
        public decimal? DepositAmount2 { get; set; }
        public decimal? DepositAmount3 { get; set; }
        public decimal? DepositAmount4 { get; set; }

    }

}
