using BlackBeltCoder;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntities;

namespace DataLogic
{
    public class LoginDll 
    {
        public ResultDto ValidateLogin(string loginusername, string password)
        {
            ResultDto objResultDto = new ResultDto();
            try
            {
                AdoHelper obj = new AdoHelper();
                SqlParameter[] parms = new SqlParameter[2];

                parms[0] = new SqlParameter("@UserName", loginusername);
                parms[0].SqlDbType = SqlDbType.VarChar;

                parms[1] = new SqlParameter("@Password", password);
                parms[1].SqlDbType = System.Data.SqlDbType.VarChar;

                SqlDataReader drResult = obj.ExecDataReaderProc("uspValidateLogin", parms);
                while (drResult.Read())
                {
                    objResultDto.ObjectId = Convert.ToInt16(drResult["Result"].ToString());
                    objResultDto.Message = drResult["Message"].ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objResultDto;
        }

        public List<GroupMasterViewDto> GetSelectGroupDetails(int empId, out string clusterName)
        {
            List<GroupMasterViewDto> lstGroup = new List<GroupMasterViewDto>();
            GroupMasterViewDto group;
            AdoHelper obj = new AdoHelper();
            SqlParameter[] parms = new SqlParameter[2];

            parms[0] = new SqlParameter("@EmpId", empId);
            parms[0].SqlDbType = SqlDbType.Int;

            parms[1] = new SqlParameter("@Cluster", SqlDbType.VarChar, 500);
            parms[1].Direction = ParameterDirection.InputOutput;
            parms[1].Value = string.Empty;

            SqlDataReader dr = obj.ExecDataReaderProc("uspGetSelectGroupDetails", parms);

            while (dr.Read())
            {
                group = new GroupMasterViewDto();
                group.GroupID = Convert.ToInt32(dr["GroupID"]);
                group.GroupCode = Convert.ToString(dr["GroupCode"]);
                group.GroupRefNumber = Convert.ToString(dr["GroupRefNumber"]);
                group.GroupName = Convert.ToString(dr["GroupName"]);
                group.TEGroupName = Convert.ToString(dr["TEGroupName"]);
                group.Panchayat = Convert.ToString(dr["Panchayat"]);
                group.Village = Convert.ToString(dr["Village"]);
                group.MeetingDay = Convert.ToByte(dr["MeetingDay"]);

                lstGroup.Add(group);
            }

            clusterName = Convert.ToString(parms[1].Value);
            if (clusterName == string.Empty)
            {
                if (dr.NextResult() && dr.Read())
                {
                    clusterName = Convert.ToString(dr["ClusterName"]);
                }
            }



            return lstGroup;
        }
    }
}
