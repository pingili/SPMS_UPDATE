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
    public class ModuleActionDal 
    {
        SqlConnection con = new SqlConnection(DBConstants.MFIS_CS);
        SqlCommand cmd;
        SqlDataReader dr;
        public List<ModulesDTO> GetModules()
        {
            List<ModulesDTO> lstModulesDto = new List<ModulesDTO>();
            cmd = new SqlCommand(ProcNames.uspModuleGetAll, con);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                ModulesDTO obj = new ModulesDTO();
                obj.ModuleId = Convert.ToInt32(dr["ModuleId"]);
                obj.ModuleName = Convert.ToString(dr["ModuleName"]);
                lstModulesDto.Add(obj);
            }
            con.Close();
            return lstModulesDto;
        }

        public int InsertUpdateModuleAction(ref int moduleActionId, string ModuleActionCode, string ActionName, int ModuleId, string Url)
        {
            int rowsAffected = default(int);

            SqlCommand cmd = new SqlCommand(ProcNames.uspModuleActionInsertUpdate, con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ModuleActionCode", ModuleActionCode);
            cmd.Parameters.AddWithValue("@ActionName", ActionName);
            cmd.Parameters.AddWithValue("@ModuleId", ModuleId);
            cmd.Parameters.AddWithValue("@Url", Url);

            SqlParameter param = new SqlParameter("@ModuleActionId", SqlDbType.Int);
            param.Direction = ParameterDirection.InputOutput;
            param.Value = moduleActionId;
            cmd.Parameters.Add(param);

            con.Open();
            rowsAffected = cmd.ExecuteNonQuery();
            con.Close();
            moduleActionId = Convert.ToInt32(cmd.Parameters["@ModuleActionId"].Value);



            return rowsAffected;
        }
        public List<ModuleActionLookup> GetLookUp()
        {
            List<ModuleActionLookup> lstModuleAction = new List<ModuleActionLookup>();

            cmd = new SqlCommand(ProcNames.uspModuleActionLookup, con);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                ModuleActionLookup obj = new ModuleActionLookup();
                obj.ModuleActionId = Convert.ToInt32(dr["ModuleActionId"]);
                obj.ModuleActionCode = Convert.ToString(dr["ModuleActionCode"]);
                obj.ModuleName = Convert.ToString(dr["ModuleName"]);
                obj.ActionName = Convert.ToString(dr["ActionName"]);
                obj.Url = Convert.ToString(dr["Url"]);
                obj.Status = Convert.ToInt32(dr["Status"]);
                obj.StatusCode = Convert.ToString(dr["StatusCode"]);
                lstModuleAction.Add(obj);
            }
            con.Close();
            return lstModuleAction;

        }


        public ModuleActionDto GetModuleActionByID(int moduleActionID)
        {
            ModuleActionDto objModuleAction = new ModuleActionDto();
            cmd = new SqlCommand(ProcNames.uspModuleActionGetByID, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ModuleActionId", moduleActionID);
            con.Open();
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                objModuleAction.ModuleActionId = Convert.ToInt32(dr["ModuleActionId"]);
                objModuleAction.ModuleActionCode = Convert.ToString(dr["ModuleActionCode"]);
                objModuleAction.ModuleId = Convert.ToInt32(dr["ModuleId"]);
                objModuleAction.ActionName = Convert.ToString(dr["ActionName"]);
                objModuleAction.Url = Convert.ToString(dr["Url"]);
            }
            con.Close();
            return objModuleAction;
        }
        public int DeleteModuleAction(ref int moduleActionId)
        {
            int rowsAffected = default(int);

            SqlCommand cmd = new SqlCommand(ProcNames.uspModuleActionDelete, con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter param = new SqlParameter("@ModuleActionId", SqlDbType.Int);
            param.Direction = ParameterDirection.InputOutput;
            param.Value = moduleActionId;
            cmd.Parameters.Add(param);
            con.Open();
            rowsAffected = cmd.ExecuteNonQuery();
            con.Close();
            moduleActionId = Convert.ToInt32(cmd.Parameters["@ModuleActionId"].Value);
            return rowsAffected;
        }

        public int ChangeStatus(ref int moduleActionId, ref string statusCode)
        {
            int affectedRows = default(int);

            SqlCommand cmd = new SqlCommand(ProcNames.uspModuleActionChangeStatus, con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter param = new SqlParameter("@ModuleActionId", SqlDbType.Int);
            param.Direction = ParameterDirection.InputOutput;
            param.Value = moduleActionId;
            cmd.Parameters.Add(param);
            SqlParameter paramStatusCode = new SqlParameter("@StatusCode", SqlDbType.VarChar, 20);
            paramStatusCode.Direction = ParameterDirection.InputOutput;
            paramStatusCode.Value = statusCode;
            cmd.Parameters.Add(paramStatusCode);
            con.Open();
            affectedRows = cmd.ExecuteNonQuery();
            con.Close();
            moduleActionId = Convert.ToInt32(cmd.Parameters["@ModuleActionId"].Value);
            statusCode = Convert.ToString(cmd.Parameters["@StatusCode"].Value);
            return affectedRows;
        }

        public List<ModuleActionDto> GetModuleActionsByUserId(int userid,bool isFederation)
        {
            List<ModuleActionDto> moduleActoins = new List<ModuleActionDto>();
            try
            {
                AdoHelper obj = new AdoHelper();
                SqlParameter[] parms = new SqlParameter[2];

                parms[0] = new SqlParameter("@UserId", userid);
                parms[0].SqlDbType = SqlDbType.Int;

                parms[1] = new SqlParameter("@IsFedertaion", isFederation);
                parms[1].SqlDbType = SqlDbType.Bit;

                SqlDataReader drResult = obj.ExecDataReaderProc("uspGetModlueActionsByUserId", parms);
                while (drResult.Read())
                {
                    ModuleActionDto moduleActionDto = new ModuleActionDto();
                    moduleActionDto.ActionName = drResult["ActionName"].ToString();
                    moduleActionDto.ModuleActionCode = drResult["ModuleActionCode"].ToString();
                    moduleActionDto.ModuleActionId = Convert.ToInt32(drResult["ModuleActionId"].ToString());
                    moduleActionDto.ModuleId = Convert.ToInt32(drResult["ModuleId"].ToString());
                    moduleActionDto.ModuleName = drResult["ModuleName"].ToString();
                    moduleActionDto.Status = Convert.ToInt32(drResult["Status"].ToString());
                    moduleActionDto.Url = drResult["Url"].ToString();
                    moduleActoins.Add(moduleActionDto);
                }
            }
            catch (Exception)
            {
                
            }
            return moduleActoins;
        }
    }
}
