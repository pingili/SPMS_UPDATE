using BusinessEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using BusinessLogic.Interface;
using DataLogic;
namespace BusinessLogic
{
    public class LoginBll 
    {
        public ResultDto ValidateLogin(string loginusername, string password)
        {
            ResultDto objResultDto = new ResultDto();
            try
            {
                LoginDll objLoginDll = new LoginDll();
                objResultDto = objLoginDll.ValidateLogin(loginusername, password);
            }
            catch (Exception ex)
            {
                objResultDto.Message = "OOPS! Something wroing, Please try later.";
            }
            return objResultDto;
        }

        public List<GroupMasterViewDto> GetSelectGroupDetails(int empId, out string clusterName)
        {
            LoginDll objLoginDll = new LoginDll();
            return objLoginDll.GetSelectGroupDetails(empId, out clusterName);
        }
    }
}
