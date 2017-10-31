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
    public class MasterDal
    {
        public TypeQueryResult GetTypeQueryResult(string typeCode, string param1 = null, string param2 = null, string param3 = null, string param4 = null, string param5 = null, string param6 = null)
        {
            TypeQueryResult lstResult = new TypeQueryResult();
            TypeQueryDto obj = null;

            AdoHelper ado = new AdoHelper();
            SqlParameter[] param = new SqlParameter[7];

            param[0] = new SqlParameter("@TypeCode", typeCode);
            param[1] = new SqlParameter("@Param1", param1);
            param[2] = new SqlParameter("@Param2", param2);
            param[3] = new SqlParameter("@Param3", param3);
            param[4] = new SqlParameter("@Param4", param4);
            param[5] = new SqlParameter("@Param5", param5);
            param[6] = new SqlParameter("@Param6", param6);

            SqlDataReader dr = ado.ExecDataReaderProc("uspGetTypeQueryResult", param);

            while (dr.Read())
            {
                int fieldCount = dr.FieldCount;

                obj = new TypeQueryDto();
                obj.Id = Convert.ToString(dr["Id"]);
                if (fieldCount > 1 && dr["Name"] != DBNull.Value)
                    obj.Name = Convert.ToString(dr["Name"]);
                if (fieldCount > 2 && dr["Code"] != DBNull.Value)
                    obj.Code = Convert.ToString(dr["Code"]);
                if (fieldCount > 3 && dr["Value1"] != DBNull.Value)
                    obj.Value1 = Convert.ToString(dr["Value1"]);
                if (fieldCount > 4 && dr["Value2"] != DBNull.Value)
                    obj.Value2 = Convert.ToString(dr["Value2"]);
                if (fieldCount > 5 && dr["Value3"] != DBNull.Value)
                    obj.Value3 = Convert.ToString(dr["Value3"]);
                if (fieldCount > 6 && dr["Value4"] != DBNull.Value)
                    obj.Value4 = Convert.ToString(dr["Value4"]);
                if (fieldCount > 7 && dr["Value5"] != DBNull.Value)
                    obj.Value5 = Convert.ToString(dr["Value5"]);

                lstResult.Add(obj);
            }

            return lstResult;
        }

        public CurrentUser GetLoginMasterInfo(int userId, bool isFederation)
        {
            AdoHelper ado = new AdoHelper();
            SqlParameter[] param = new SqlParameter[2];

            param[0] = new SqlParameter("@UserId", userId);
            param[1] = new SqlParameter("@IsFedertaion", isFederation);

            SqlDataReader dr = ado.ExecDataReaderProc("uspGetMasterLoginInfo", param);

            CurrentUser objUser = new CurrentUser();
            if (dr.Read())
            {
                objUser.FinancialYearBegin = Convert.ToDateTime(dr["FINANCIAL_YEAR_BEGIN"]);
                objUser.FinancialYearEnd = Convert.ToDateTime(dr["FINANCIAL_YEAR_END"]);
                objUser.Photo = Convert.ToString(dr["Photo"]);
                objUser.UserName = Convert.ToString(dr["EmployeeName"]);
                objUser.UserID = userId;
                objUser.Role = Convert.ToString(dr["RoleName"]);
                objUser.RoleCode = Convert.ToString(dr["RoleCode"]);
            }
            if (dr.NextResult())
            {
                objUser.modules = new List<ModuleDto>();
                while (dr.Read())
                {
                    ModuleDto moduleDto = new ModuleDto();
                    moduleDto.ModuleId = DBNull.Value == dr["ModuleId"] ? 0 : Convert.ToInt16(dr["ModuleId"].ToString());
                    moduleDto.IsFederation = DBNull.Value == dr["IsFederation"] ? false : Boolean.Parse(dr["IsFederation"].ToString());
                    moduleDto.ModuleName = DBNull.Value == dr["ModuleName"] ? null : dr["ModuleName"].ToString();
                    moduleDto.ModuleCode = DBNull.Value == dr["ModuleCode"] ? "" : dr["ModuleCode"].ToString();
                    moduleDto.Url = DBNull.Value == dr["Url"] ? "" : dr["Url"].ToString();
                    moduleDto.ControlId = DBNull.Value == dr["ControlId"] ? "" : dr["ControlId"].ToString();
                    moduleDto.DisplayOrder = DBNull.Value == dr["DisplayOrder"] ? 0 : Convert.ToInt32(dr["DisplayOrder"].ToString());
                    moduleDto.ParentID = DBNull.Value == dr["ParentID"] ? 0 : Convert.ToInt32(dr["ParentID"].ToString());
                    moduleDto.IsSeed = DBNull.Value == dr["IsSeed"] ? true : Boolean.Parse(dr["IsSeed"].ToString());

                    objUser.modules.Add(moduleDto);
                }
            }
            
            return objUser;
        }

        public List<SelectListDto> GetMasterDropDownResult(string refCode)
        {
            List<SelectListDto> slResult = new List<SelectListDto>();
            AdoHelper ado = new AdoHelper();
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@DropDownType", refCode);

            SqlDataReader dr = ado.ExecDataReaderProc("uspGetDynamicDropDown", param);

            while (dr.Read())
            {
                slResult.Add(new SelectListDto()
                {
                    ID = Convert.ToInt32(dr["RefId"]),
                    Code = Convert.ToString(dr["RefCode"]),
                    Text = Convert.ToString(dr["RefValue"])
                });
            }
            return slResult;
        }
    }
}