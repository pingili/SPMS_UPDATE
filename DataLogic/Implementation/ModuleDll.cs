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
    public class ModuleDll 
    {
        public List<ModuleDto> GetModuleByUserID(int userid, bool isFederation)
        {
            List<ModuleDto> modules = new List<ModuleDto>();
            try
            {
                AdoHelper obj = new AdoHelper();
                SqlParameter[] parms = new SqlParameter[2];

                parms[0] = new SqlParameter("@UserId", userid);
                parms[0].SqlDbType = SqlDbType.Int;

                parms[1] = new SqlParameter("@IsFedertaion", isFederation);
                parms[1].SqlDbType = SqlDbType.Bit;

                SqlDataReader drResult = obj.ExecDataReaderProc("uspModulesGetByUserId", parms);
                while (drResult.Read())
                {
                    ModuleDto moduleDto = new ModuleDto();
                    moduleDto.ModuleId = DBNull.Value == drResult["ModuleId"] ? 0 : Convert.ToInt16(drResult["ModuleId"].ToString());
                    moduleDto.IsFederation = DBNull.Value == drResult["IsFederation"] ? false : Boolean.Parse(drResult["IsFederation"].ToString());
                    moduleDto.ModuleName = DBNull.Value == drResult["ModuleName"] ? null : drResult["ModuleName"].ToString();
                    moduleDto.ModuleCode = DBNull.Value == drResult["ModuleCode"] ? "" : drResult["ModuleCode"].ToString();
                    moduleDto.Url = DBNull.Value == drResult["Url"] ? "" : drResult["Url"].ToString();
                    moduleDto.ControlId = DBNull.Value == drResult["ControlId"] ? "" : drResult["ControlId"].ToString();
                    moduleDto.DisplayOrder = DBNull.Value == drResult["DisplayOrder"] ? 0 : Convert.ToInt32(drResult["DisplayOrder"].ToString());
                    moduleDto.ParentID = DBNull.Value == drResult["ParentID"] ? 0 : Convert.ToInt32(drResult["ParentID"].ToString());
                    moduleDto.IsSeed = DBNull.Value == drResult["IsSeed"] ? true : Boolean.Parse(drResult["IsSeed"].ToString());

                    modules.Add(moduleDto);
                }
            }
            catch (Exception)
            {

            }
            return modules;
        }
        public List<ModuleDto> GetModuleAll()
        {
            List<ModuleDto> modules = new List<ModuleDto>();
            try
            {
                AdoHelper obj = new AdoHelper();
                
                SqlDataReader drResult = obj.ExecDataReaderProc("uspModulesGetAll");
                while (drResult.Read())
                {
                    ModuleDto moduleDto = new ModuleDto();
                    moduleDto.ModuleId = DBNull.Value == drResult["ModuleId"] ? 0 : Convert.ToInt16(drResult["ModuleId"].ToString());
                    moduleDto.IsFederation = DBNull.Value == drResult["IsFederation"] ? false : Boolean.Parse(drResult["IsFederation"].ToString());
                    moduleDto.ModuleName = DBNull.Value == drResult["ModuleName"] ? null : drResult["ModuleName"].ToString();
                    moduleDto.ModuleCode = DBNull.Value == drResult["ModuleCode"] ? "" : drResult["ModuleCode"].ToString();
                    moduleDto.Url = DBNull.Value == drResult["Url"] ? "" : drResult["Url"].ToString();
                    moduleDto.ControlId = DBNull.Value == drResult["ControlId"] ? "" : drResult["ControlId"].ToString();
                    moduleDto.DisplayOrder = DBNull.Value == drResult["DisplayOrder"] ? 0 : Convert.ToInt32(drResult["DisplayOrder"].ToString());
                    moduleDto.ParentID = DBNull.Value == drResult["ParentID"] ? 0 : Convert.ToInt32(drResult["ParentID"].ToString());
                    moduleDto.IsSeed = DBNull.Value == drResult["IsSeed"] ? true : Boolean.Parse(drResult["IsSeed"].ToString());

                    modules.Add(moduleDto);
                }
            }
            catch (Exception)
            {

            }
            return modules;
        }

        public List<ModuleDto> GetModuleByRoleId(int roleId )
        {
            List<ModuleDto> modules = new List<ModuleDto>();
            try
            {
                AdoHelper obj = new AdoHelper();
                SqlParameter[] parms = new SqlParameter[1];

                parms[0] = new SqlParameter("@RoleId", roleId);
                parms[0].SqlDbType = SqlDbType.Int;

               
                SqlDataReader drResult = obj.ExecDataReaderProc("uspModulesGetByRoleId", parms);
                while (drResult.Read())
                {
                    ModuleDto moduleDto = new ModuleDto();
                    moduleDto.ModuleId = DBNull.Value == drResult["ModuleId"] ? 0 : Convert.ToInt16(drResult["ModuleId"].ToString());
                    moduleDto.IsFederation = DBNull.Value == drResult["IsFederation"] ? false : Boolean.Parse(drResult["IsFederation"].ToString());
                    moduleDto.ModuleName = DBNull.Value == drResult["ModuleName"] ? null : drResult["ModuleName"].ToString();
                    moduleDto.ModuleCode = DBNull.Value == drResult["ModuleCode"] ? "" : drResult["ModuleCode"].ToString();
                    moduleDto.Url = DBNull.Value == drResult["Url"] ? "" : drResult["Url"].ToString();
                    moduleDto.ControlId = DBNull.Value == drResult["ControlId"] ? "" : drResult["ControlId"].ToString();
                    moduleDto.DisplayOrder = DBNull.Value == drResult["DisplayOrder"] ? 0 : Convert.ToInt32(drResult["DisplayOrder"].ToString());
                    moduleDto.ParentID = DBNull.Value == drResult["ParentID"] ? 0 : Convert.ToInt32(drResult["ParentID"].ToString());
                    moduleDto.IsSeed = DBNull.Value == drResult["IsSeed"] ? true : Boolean.Parse(drResult["IsSeed"].ToString());

                    modules.Add(moduleDto);
                }
            }
            catch (Exception)
            {

            }
            return modules;
        }
    }
}
