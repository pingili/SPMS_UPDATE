using BusinessEntities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;

namespace DataLogic
{
    public class RoleDal 
    {
        public List<RoleDto> GetLookUp()
        {
            List<RoleDto> lstRoles = new List<RoleDto>();
            SqlConnection con = new SqlConnection(DBConstants.MFIS_CS);
            SqlCommand cmd = new SqlCommand(ProcNames.uspRolesLookUp, con);
            cmd.CommandType = CommandType.StoredProcedure;

            con.Open();

            SqlDataReader dr = cmd.ExecuteReader();
            RoleDto objRole;
            while (dr.Read())
            {
                objRole = new RoleDto();
                objRole.RoleCode = Convert.ToString(dr["RoleCode"]);
                objRole.RoleName = Convert.ToString(dr["RoleName"]);
                objRole.RoleId = Convert.ToInt32(dr["RoleId"]);
                objRole.StatusCode = Convert.ToString(dr["StatusCode"]);
                lstRoles.Add(objRole);
            }

            con.Close();

            return lstRoles;
        }

        public int InsertUpdate(ref int roleId, string roleName, string roleCode)
        {
            int rowsAffected = default(int);

            SqlConnection con = new SqlConnection(DBConstants.MFIS_CS);
            SqlCommand cmd = new SqlCommand(ProcNames.uspRoleInsertUpdate, con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@RoleName", roleName);
            cmd.Parameters.AddWithValue("@RoleCode", roleCode);

            SqlParameter param = new SqlParameter("@RoleId", SqlDbType.Int);
            param.Direction = ParameterDirection.InputOutput;
            param.Value = roleId;
            cmd.Parameters.Add(param);

            con.Open();

            rowsAffected = cmd.ExecuteNonQuery();

            roleId = Convert.ToInt32(cmd.Parameters["@RoleId"].Value);

            con.Close();

            return rowsAffected;
        }
       


        public RoleDto GetById(int? ID)
        {
            List<RoleDto> lstrole = new List<RoleDto>();
            SqlConnection con = new SqlConnection(DBConstants.MFIS_CS);
            SqlCommand cmd = new SqlCommand(ProcNames.uspRolesGetByRoleID, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@RoleId", ID);
            con.Open();
            SqlDataReader res = cmd.ExecuteReader();
            RoleDto roledto = new RoleDto();
            if (res.Read())
            {
                roledto.RoleId = Convert.ToInt32(res["RoleId"]);
                roledto.RoleName = Convert.ToString(res["RoleName"]);
                roledto.RoleCode = Convert.ToString(res["RoleCode"]);
            }
            con.Close();
            return roledto;
        }


        public int DeleteRole(ref int id)
        {
            int rowsCount = default(int);
            SqlConnection con = new SqlConnection(DBConstants.MFIS_CS);
            SqlCommand cmd = new SqlCommand(ProcNames.uspRoleDelete, con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter param = new SqlParameter("@RoleId", SqlDbType.Int);
            param.Direction = ParameterDirection.InputOutput;
            param.Value = id;
            cmd.Parameters.Add(param);
            con.Open();
            rowsCount = cmd.ExecuteNonQuery();
            id = Convert.ToInt32(cmd.Parameters["@RoleId"].Value);
            con.Close();
            return rowsCount;
        }



        public int ChangeStatus(ref int id, ref string StatusCode)
        {
            int rowsCount = default(int);
            SqlConnection con = new SqlConnection(DBConstants.MFIS_CS);
            SqlCommand cmd = new SqlCommand(ProcNames.uspRoleChangeStatus, con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter param = new SqlParameter("@RoleId", SqlDbType.Int);
            param.Direction = ParameterDirection.InputOutput;
            param.Value = id;
            cmd.Parameters.Add(param);
            SqlParameter paramStatusCode = new SqlParameter("@StatusCode", SqlDbType.VarChar,20);
            paramStatusCode.Direction = ParameterDirection.InputOutput;
            paramStatusCode.Value = string.Empty;
            cmd.Parameters.Add(paramStatusCode);
            con.Open();
            rowsCount = cmd.ExecuteNonQuery();
            id = Convert.ToInt32(cmd.Parameters["@RoleId"].Value);
            StatusCode = Convert.ToString(cmd.Parameters["@StatusCode"].Value);

            con.Close();
            return rowsCount;
        }
    }
}
