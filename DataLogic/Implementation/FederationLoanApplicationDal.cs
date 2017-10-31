using BlackBeltCoder;
using BusinessEntities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLogic.Implementation
{
    public class FederationLoanApplicationDal
    {
        public FederationLoanApplicationDto GetGroupSLAccountByFedSLAcount(string Id)
        {
            //List<FederationLoanApplicationDto> lstDto = new List<FederationLoanApplicationDto>();
            FederationLoanApplicationDto dto = new FederationLoanApplicationDto();
            AdoHelper obj = new AdoHelper();
            SqlParameter[] parms = new SqlParameter[1];

            parms[0] = new SqlParameter("@FedSlAccount", Id);
            parms[0].SqlDbType = System.Data.SqlDbType.Int;

            SqlDataReader dr = obj.ExecDataReaderProc("spGetGroupSLAccountByFedSLAcount", parms);
            if (dr.Read())
            {
                dto = new FederationLoanApplicationDto();
                dto.GRSlAccountId = Convert.ToInt32(dr["GroupAHId"]);
                dto.GRSLAccount = Convert.ToString(dr["AHName"]);
                dto.GRGLAccount = Convert.ToString(dr["PAHName"]);
            }
            return dto;
        }

        public int GetMemberCountByGroupId(int Id)
        {
            int i;
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString);
            SqlCommand cmd = new SqlCommand("SPGetMemberCountByGroupId", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@GroupId", Id);
            con.Open();
            i = Convert.ToInt32(cmd.ExecuteScalar());
            return i;
        }

        public ResultDto CreateFedLoanAppln(FederationLoanApplicationDto obj)
        {
            ResultDto resultDto = new ResultDto();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString);
            SqlCommand cmd = new SqlCommand("SPGetMemberCountByGroupId", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ClusterId", obj.ClusterId);
            cmd.Parameters.AddWithValue("@SlAccountId", obj.SlAccountId);
            cmd.Parameters.AddWithValue("@GRSlAccountId", obj.GRSlAccountId);
            cmd.Parameters.AddWithValue("@GroupId", obj.GroupId);
            cmd.Parameters.AddWithValue("@LoanSanctionAmount", obj.LoanSanctionAmount);
            cmd.Parameters.AddWithValue("@NoOfInstallments", obj.NoOfInstallments);
            cmd.Parameters.AddWithValue("@TotalMembers", obj.TotalMembers);
            cmd.Parameters.AddWithValue("@GroupBankAccount", obj.GroupBankAccount);
            con.Open();
            resultDto.ObjectId= cmd.ExecuteNonQuery();
            return resultDto;
        }
    }
}
