//using BusinessLogic.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntities;
using MFIEntityFrameWork;
using BusinessLogic.AutoMapper;
using System.Data.Entity.Core.Objects;
using DataLogic;
using Utilities;

namespace BusinessLogic
{
    public class ModuleActionService 
    {
        #region Global Variables
        private readonly ModuleActionDal _moduleActionDal;
        private readonly CommonService _commonService;
        public ModuleActionService()
        {
            _moduleActionDal = new ModuleActionDal();
            _commonService = new CommonService();
            AutoMapperEntityConfiguration.Configure();
        }
        #endregion
        public ResultDto Insert(ModuleActionDto moduleAction)
        {
           return InsertUpdate(moduleAction);
        }
        public ResultDto Update(ModuleActionDto moduleAction)
        {
            return InsertUpdate(moduleAction);

        }
        private ResultDto InsertUpdate(ModuleActionDto moduleAction )
        {
            ResultDto resultDto = new ResultDto();
            string objectName = "ModuleAction";
            try
            {
               //ObjectParameter prmModuleactionId = new ObjectParameter("ModuleActionId", moduleAction.ModuleActionId);
                int moduleActionId = moduleAction.ModuleActionId;

               int effectedRow = _moduleActionDal.InsertUpdateModuleAction(ref moduleActionId,moduleAction.ModuleActionCode,moduleAction.ActionName,moduleAction.ModuleId,moduleAction.Url);
               resultDto.ObjectId = moduleActionId;

               if (resultDto.ObjectId > 0)
                    resultDto.Message = string.Format("{0} Details Saved Successfully ", objectName);
               
                else
                    resultDto.Message = string.Format("Error Occured while Saving {0} Details", objectName);
            }
            catch (Exception)
            {
                resultDto.Message = string.Format("Service layer error occured while saving the {0} details", objectName);
                resultDto.ObjectId = -98;

            }
            return resultDto;
        }    

        public List<ModulesDTO> GetModules()
        {
            List<ModulesDTO> modulesList = _moduleActionDal.GetModules();
            return modulesList;
        }
        public List<ModuleActionLookup> GetLookUp()
        {
            List<ModuleActionLookup> lstModuleActions = _moduleActionDal.GetLookUp();
            return lstModuleActions;
        }


        public ModuleActionDto GetModuleActionByID(int moduleActionID)
        {
             ModuleActionDto moduleActions = _moduleActionDal.GetModuleActionByID(moduleActionID);
            return moduleActions;

        }


        public ResultDto Delete(int moduleActionId)
        {
            ResultDto resultDto = new ResultDto();
            string objectName = "Module Action";

            try
            {
                int effectedCount = _moduleActionDal.DeleteModuleAction(ref moduleActionId); 
                resultDto.ObjectId = moduleActionId;
                if (resultDto.ObjectId > 0)
                    resultDto.Message = string.Format("{0} : Details Deleted Successfully", objectName);
                else
                    resultDto.Message = string.Format("Error Occured while Deleting {0} Details", objectName);
            }
            catch (Exception)
            {
                resultDto.Message = string.Format("Service layer error Occured while Deleting the {0} Details", objectName);
                resultDto.ObjectId = -98;

            }
            return resultDto;
        }

        public ResultDto ChangeStatus(int moduleActionId)
        {
            ResultDto resultDto = new ResultDto();
            string objectName = "Module Action";
           string StatusCode=string.Empty;
            string statusCode;
            try
            {

                int effectedCount = _moduleActionDal.ChangeStatus(ref moduleActionId,ref StatusCode);

                resultDto.ObjectId = moduleActionId;
               statusCode = StatusCode;
                if (resultDto.ObjectId > 0)
                    resultDto.Message = string.Format("{0} : details {1} successfully", objectName, statusCode == Constants.StatusCodes.Active ? "activated" : "inactivated");
                else
                    resultDto.Message = string.Format("Error occured while {0} {1} details", statusCode == Constants.StatusCodes.Active ? "activated" : "inactivated", objectName);
            }
            catch (Exception)
            {
                resultDto.Message = string.Format("Service layer error occured while {0} details", objectName);
                resultDto.ObjectId = -98;

            }
            return resultDto;
        }

        public List<ModuleActionDto> GetModuleActionsByUserId(int userid, bool isFederation)
        {
            ModuleActionDal objModuleActionDal = new ModuleActionDal();
            return objModuleActionDal.GetModuleActionsByUserId(userid, isFederation);
        }

    }
}
