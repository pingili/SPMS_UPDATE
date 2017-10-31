using BlackBeltCoder;
using BusinessEntities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLogic
{
    public class MemberDll
    {
        public ResultDto UpdateMemberLeaderShip(int memberid, string leaderShipLevel, int designation, DateTime fromDate, int userid)
        {
            ResultDto objResultDto = new ResultDto();
            try
            {
                AdoHelper obj = new AdoHelper();
                SqlParameter[] parms = new SqlParameter[5];

                parms[0] = new SqlParameter("@MemberID", memberid);
                parms[0].SqlDbType = SqlDbType.Int;

                parms[1] = new SqlParameter("@LeaderShipLevel", leaderShipLevel);
                parms[1].SqlDbType = System.Data.SqlDbType.Char;

                parms[2] = new SqlParameter("@Designation", designation);
                parms[2].SqlDbType = SqlDbType.Int;

                parms[3] = new SqlParameter("@FromDate", fromDate);
                parms[3].SqlDbType = System.Data.SqlDbType.Date;

                parms[4] = new SqlParameter("@UserId", userid);
                parms[4].SqlDbType = SqlDbType.Int;

                int count = obj.ExecNonQueryProc("uspMemberLeadershipUpdate", parms);
                if(count>0)
                {
                    objResultDto.ObjectId = memberid;
                    objResultDto.Message = "Leadership details updated successfully";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objResultDto;
        }

    }
}
//uspMemberLeadershipUpdate