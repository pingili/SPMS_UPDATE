using BusinessEntities;
//using BusinessLogic.Interface;
using DataLogic.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class RepaymentService 
    {
        public List<RepaymentDto> GetRepayment(int LoanMasterID,string transactionDate)
        {
            return new RepaymentDll().GetRepayment(LoanMasterID, transactionDate);
        }
    }
}
