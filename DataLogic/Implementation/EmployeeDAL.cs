using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntities;
using BlackBeltCoder;
using System.Data.SqlClient;
using System.Data;

namespace DataLogic.Implementation
{
    public class EmployeeDAL
    {
        public ResultDto ManageClusterAssignments(int employeeId, string strClustersVsEmpXML)
        {
            SqlConnection con = new SqlConnection(DBConstants.MFIS_CS);
            ResultDto res = new ResultDto();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;
                cmd.CommandText = "uspEmpclusterXrefInsertUpdate";
                cmd.Parameters.AddWithValue("@EmployeeID", employeeId);
                cmd.Parameters.AddWithValue("@ClusterList", strClustersVsEmpXML);
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
            return res;
        }

        public List<ClusterAssignmentDto> ClusterGetByEmpID(int ID)
        {
            SqlConnection con = new SqlConnection(DBConstants.MFIS_CS);
            List<ClusterAssignmentDto> listcluster = new List<ClusterAssignmentDto>();
            SqlCommand cmd = new SqlCommand("uspClusterDetailsgetbyEmpid", con);
            cmd.Parameters.AddWithValue("@EmpID", ID);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            ClusterAssignmentDto clusterdto = null;
            while (dr.Read())
            {
                clusterdto = new ClusterAssignmentDto();
                clusterdto.ClusterID = Convert.ToInt32(dr["ClusterID"]);
                listcluster.Add(clusterdto);
            }
            return listcluster;
        }

        public List<ClusterAssignmentDto> GetAllClusterAssignments()
        {
            SqlConnection con = new SqlConnection(DBConstants.MFIS_CS);
            List<ClusterAssignmentDto> lstClusterAssignments = new List<ClusterAssignmentDto>();
            SqlCommand cmd = new SqlCommand("uspAssignClustertoEmployeedetails", con);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            ClusterAssignmentDto clusterdto = null;
            while (dr.Read())
            {
                clusterdto = new ClusterAssignmentDto();
                clusterdto.ClusterID = Convert.ToInt32(dr["EMPid"]);
                clusterdto.ClusterName = Convert.ToString(dr["ClusterName"]);
                clusterdto.EmployeeName = Convert.ToString(dr["EmployeeName"]);
                clusterdto.RoleName = Convert.ToString(dr["RoleName"]);

                lstClusterAssignments.Add(clusterdto);
            }
            return lstClusterAssignments;
        }
    }
}
