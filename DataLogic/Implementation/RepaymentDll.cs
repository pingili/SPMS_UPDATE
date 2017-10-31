using BlackBeltCoder;
using BusinessEntities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLogic.Implementation
{
    public class RepaymentDll
    {
        public List<RepaymentDto> GetRepayment(int loanMasterID,string transactionDate)
        {

            List<RepaymentDto> listRepaymentDto = new List<RepaymentDto>();
            try
            {
                AdoHelper obj = new AdoHelper();
                SqlParameter[] parms = new SqlParameter[2];

                parms[0] = new SqlParameter("@MemberID", loanMasterID);
                parms[0].SqlDbType = System.Data.SqlDbType.Int;

                parms[1] = new SqlParameter("@TransactionDate", transactionDate);
                parms[1].SqlDbType = System.Data.SqlDbType.VarChar;

                SqlDataReader Dr = obj.ExecDataReaderProc("uspGetRepayment", parms);
                while (Dr.Read())
                {
                    RepaymentDto objRepaymentDto = new RepaymentDto();
                    objRepaymentDto.AHID = Convert.ToInt32(Dr["AHID"]);
                    objRepaymentDto.Amount = Convert.ToDecimal(Dr["PrincipalBalance"]);
                    //objRepaymentDto.SLAccountAHID = Convert.ToInt32(Dr["SLAccountAHID"]);
                    objRepaymentDto.LoanRepaymentID = Convert.ToInt32(Dr["LoanMasterID"]);
                    listRepaymentDto.Add(objRepaymentDto);
                }
            }
            catch (Exception ex)
            {

            }
            return listRepaymentDto;

        }
    }
}
