using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntities;
using DataLogic;
using DataLogic.Implementation;
//using BusinessLogic.Interface;

namespace BusinessLogic
{
    public class RoleModulesService 
    {
        #region Global Variables
        private readonly RoleModulesDal _RoleModulesDal;
        public RoleModulesService()
        {
            _RoleModulesDal = new RoleModulesDal();
        }
        #endregion
        public ResultDto Insert(RoleModulesDto RoleModule)
        {
            return InsertRoleModules(RoleModule);
        }
        public ResultDto Update(RoleModulesDto RoleModule)
        {
            ResultDto resultDto = new ResultDto();

            return InsertRoleModules(RoleModule);
        }

        public int InsertRoleModules(int roleId, string modules)
        {
            return _RoleModulesDal.InsertRoleModules(roleId, modules);
        }
        private ResultDto InsertRoleModules(RoleModulesDto RoleModulesDto)
        {
               ResultDto resultDto = new ResultDto();
               string objectName = "RoleModule";
               try
               {
                   int RoleModuleId = RoleModulesDto.RoleModuleId;

                   int rowAffected = _RoleModulesDal.InsertRoleModules(RoleModulesDto);

                   resultDto.ObjectId = RoleModuleId;

                   if (resultDto.ObjectId > 0)
                       resultDto.Message = string.Format("{0} details saved successfully", objectName);
                   else
                       resultDto.Message = string.Format("Error occured while saving {0} details", objectName);
               }
               catch (Exception)
               {
                   resultDto.Message = string.Format("Service layer error occured while saving the {0} details", objectName);
                   resultDto.ObjectId = -98;
               }

               return resultDto;
        }
        public List<RoleDto> Selectlist()
        {
            List<RoleDto> lstRoleDto = _RoleModulesDal.Selectlist();
            
           
            return lstRoleDto;
        }
        public List<ModulesDTO> ModulesCheckbox()
        {
            List<ModulesDTO> lstModulesDTO = _RoleModulesDal.ModulesCheckbox();
            

            return lstModulesDTO;
        }
        public List<ModuleActionDto> GetModuleActions(int id)
        {
            List<ModuleActionDto> lstModuleActions = _RoleModulesDal.GetModuleActions(id);
            return lstModuleActions;
        }
        public List<RoleModulesDto> GetAllRoleModules()
        {
            List<RoleModulesDto> lstRoleModules = _RoleModulesDal.GetAllRoleModules();
            return lstRoleModules;
        }


        
    }
}