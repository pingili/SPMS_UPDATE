using BusinessEntities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLogic.Implementation
{
    public class FedReportsDll
    {
        public List<DataEntryStatusDBDto> GetDataEntryStatusReport(int groupId = 0)
        {
            List<DataEntryStatusDBDto> lst = new List<DataEntryStatusDBDto>();
            SqlConnection con = new SqlConnection(DBConstants.MFIS_CS);
            SqlCommand cmd = new SqlCommand("uspGroupDataEntryStatusReport", con);
            if (groupId > 0)
                cmd.Parameters.AddWithValue("@GroupId", groupId);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            DataEntryStatusDBDto obj;
            while (dr.Read())
            {
                obj = new DataEntryStatusDBDto();
                obj.GroupId = Convert.ToInt32(dr["GroupId"]);
                obj.GroupCode = Convert.ToString(dr["GroupCode"]);
                obj.GroupName = Convert.ToString(dr["GroupName"]);
                obj.ClusterCode = Convert.ToString(dr["ClusterCode"]);
                obj.ClusterName = Convert.ToString(dr["ClusterName"]);
                obj.GroupOBStatus = Convert.ToString(dr["OB_STATUS"]);
                obj.SavingsMemberCount = Convert.ToInt32(dr["SavingsMemberCount"]);
                obj.isConducted = Convert.ToBoolean(dr["IsConducted"]);
                obj.Apr = Convert.ToInt32(dr["Apr"]);
                obj.May = Convert.ToInt32(dr["May"]);
                obj.Jun = Convert.ToInt32(dr["Jun"]);
                obj.Jul = Convert.ToInt32(dr["Jul"]);
                obj.Aug = Convert.ToInt32(dr["Aug"]);
                obj.Sep = Convert.ToInt32(dr["Sep"]);
                obj.Oct = Convert.ToInt32(dr["Oct"]);
                obj.Nov = Convert.ToInt32(dr["Nov"]);
                obj.Dec = Convert.ToInt32(dr["Dec"]);
                obj.Jan = Convert.ToInt32(dr["Jan"]);
                obj.Feb = Convert.ToInt32(dr["Feb"]);
                obj.Mar = Convert.ToInt32(dr["Mar"]);

                lst.Add(obj);
            }
            con.Close();
            return lst;
        }
    }
}
