using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
  public  class LoanApplicationDto
    {
      public int LoanMasterId { get; set; }
      public string LoanCode{get;set;}
      public string LoanType{get;set;}
      public int MemberID{get;set;}
      public int GroupID{get;set;}
      public DateTime LoanApplicationDate{get;set;}
      public int LoanPurpose{get;set;}
      public int FundSourceId { get; set; }
      public decimal LoanAmountApplied{get;set;}
      public byte NoofInstallmentsProposed{get;set;}
      public int Mode{get;set;}
      public int UserID { get; set; }
      public int ProjectID { get; set; }
      public int InterestMasterID { get; set; }
    }
}
