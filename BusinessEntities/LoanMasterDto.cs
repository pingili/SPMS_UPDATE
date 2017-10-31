using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
  public class LoanMasterDto
    {
      public int LoanMasterID { get; set; }
      public int AHID { get; set; }
      public string AHName { get; set; }
      public string AHCode { get; set; }
      public int MemberID { get; set; }
      public string DrAmount { get; set; }

    }
}
