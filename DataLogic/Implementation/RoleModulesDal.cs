using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntities;
using System.Data;
using System.Data.SqlClient;
using BlackBeltCoder;

namespace DataLogic
{
    public class RoleModulesDal 
    {
        public List<RoleDto> Selectlist()
        {
            List<RoleDto> lstRoleDto = new List<RoleDto>();
            SqlConnection con = new SqlConnection(DBConstants.MFIS_CS);
            SqlCommand cmd = new SqlCommand(ProcNames.uspRolesLookUp, con);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            RoleDto objRoleDto;
            while (sdr.Read())
            {
                objRoleDto = new RoleDto();
                objRoleDto.RoleId = Convert.ToInt32(sdr["RoleId"]);
                objRoleDto.RoleName = Convert.ToString(sdr["RoleName"]);
                if (objRoleDto.RoleId == 4013)
                    continue;
                lstRoleDto.Add(objRoleDto);

            }
            con.Close();
            return lstRoleDto;
        }
        public List<ModulesDTO> ModulesCheckbox()
        {
            List<ModulesDTO> lstModulesDTO = new List<ModulesDTO>();
            SqlConnection con = new SqlConnection(DBConstants.MFIS_CS);
            SqlCommand cmd = new SqlCommand(ProcNames.uspModuleGetAll, con);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            ModulesDTO objModulesDTO;

            while (sdr.Read())
            {
                objModulesDTO = new ModulesDTO();
                objModulesDTO.ModuleId = Convert.ToInt32(sdr["ModuleId"]);
                objModulesDTO.ModuleName = Convert.ToString(sdr["ModuleName"]);
                lstModulesDTO.Add(objModulesDTO);
            }
            con.Close();
            return lstModulesDTO;
        }
        public List<ModuleActionDto> GetModuleActions(int id)
        {
            List<ModuleActionDto> lstModuleActionDto = new List<ModuleActionDto>();
            SqlConnection con = new SqlConnection(DBConstants.MFIS_CS);
            SqlCommand cmd = new SqlCommand(ProcNames.uspGetModuleActionsByModuleID, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ModuleId", id);
            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            ModuleActionDto objModuleActionDto;
            while (sdr.Read())
            {
                objModuleActionDto = new ModuleActionDto();
                objModuleActionDto.ModuleActionId = Convert.ToInt32(sdr["ModuleActionId"]);
                objModuleActionDto.ActionName = Convert.ToString(sdr["ActionName"]);
                lstModuleActionDto.Add(objModuleActionDto);
            }
            con.Close();
            return lstModuleActionDto;


        }
        public int InsertRoleModules(int roleId, string modules)
        {
            int effectedCount = 0;
            try
            {
                AdoHelper adoHelper = new AdoHelper();
                SqlParameter[] parms = new SqlParameter[2];

                parms[0] = new SqlParameter("@RoleID", roleId);
                parms[0].SqlDbType = SqlDbType.Int;

                parms[1] = new SqlParameter("@ModuleIds", modules);
                parms[1].SqlDbType = SqlDbType.VarChar;
                effectedCount = adoHelper.ExecNonQueryProc(ProcNames.uspRoleModulesInsertUpdate, parms);

            }
            catch (Exception ex)
            {


            }
            return effectedCount;
        }
        public int InsertRoleModules(RoleModulesDto RoleModulesDto)
        {
            int rowseffected = default(int);
            SqlConnection con = new SqlConnection(DBConstants.MFIS_CS);
            SqlCommand cmdRoleModule = new SqlCommand(ProcNames.uspRoleModulesInsertUpdate, con);
            cmdRoleModule.CommandType = CommandType.StoredProcedure;
            cmdRoleModule.Parameters.AddWithValue("@RoleModuleCode", RoleModulesDto.RoleModuleCode);
            cmdRoleModule.Parameters.AddWithValue("@Roleid", RoleModulesDto.RoleId);
            cmdRoleModule.Parameters.AddWithValue("@ModuleId", RoleModulesDto.ModuleId);
            cmdRoleModule.Parameters.AddWithValue("@ModuleActionId", RoleModulesDto.ModuleActionId);

            // cmdRoleModule.Parameters.AddWithValue("@RoleModuleId", ParameterDirection.Output);
            SqlParameter param = new SqlParameter("@RoleModuleId", SqlDbType.Int);
            param.Direction = ParameterDirection.InputOutput;
            param.Value = RoleModulesDto.RoleModuleId;
            cmdRoleModule.Parameters.Add(param);

            con.Open();
            rowseffected = cmdRoleModule.ExecuteNonQuery();
            RoleModulesDto.RoleModuleId = Convert.ToInt32(cmdRoleModule.Parameters["@RoleModuleId"].Value);

            return rowseffected;

        }
        public List<RoleModulesDto> GetAllRoleModules()
        {
            List<RoleModulesDto> lstall = new List<RoleModulesDto>();
            SqlConnection con = new SqlConnection(DBConstants.MFIS_CS);
            SqlCommand cmd = new SqlCommand(ProcNames.uspGetRoleModulesLookup, con);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            RoleModulesDto objroles;
            while (dr.Read())
            {
                objroles = new RoleModulesDto();
                objroles.RoleModuleId = Convert.ToInt32(dr["RoleModuleId"]);
                objroles.RoleName = Convert.ToString(dr["RoleName"]);
                objroles.MainModule = Convert.ToString(dr["MainModule"]);
                objroles.SubModules = Convert.ToString(dr["SubModule"]);
                objroles.StatusCode = Convert.ToString(dr["StatusCode"]);
                lstall.Add(objroles);

            }
            return lstall;
        }


        
    }

}




