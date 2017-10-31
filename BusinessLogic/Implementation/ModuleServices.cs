using BusinessEntities;
using BusinessLogic.AutoMapper;
using DataLogic;
using MFIEntityFrameWork;
using System;
using System.Collections.Generic;

namespace BusinessLogic
{
    public class ModuleServices 
    {
        #region Global Variables
        private readonly MFISDBContext _dbContext;
        private readonly CommonService _commonService;
        public ModuleServices()
        {
            _dbContext = new MFISDBContext();
            _commonService = new CommonService();
            AutoMapperEntityConfiguration.Configure();
        }
        #endregion
        public ModulesDTO insert(ModulesDTO modeldto)
        {
            try
            {
                //var count = _dbContext.uspModulesInsertUpdate(modeldto.ModuleId, modeldto.ModuleName, modeldto.ModuleType, modeldto.Url, modeldto.Status);

            }
            catch(Exception ex)
            {
                throw ex;
            }
            return modeldto;
        }

        public List<ModuleDto> GetModuleByUserID(int userid,bool isFederation)
        {
            ModuleDll moduleDll = new ModuleDll();
            return moduleDll.GetModuleByUserID(userid, isFederation);
        }
        public List<ModuleDto> GetModuleByRoleId(int roleId )
        {
            ModuleDll moduleDll = new ModuleDll();
            return moduleDll.GetModuleByRoleId(roleId);
        }
        public List<ModuleDto> GetModuleAll()
        {
            ModuleDll moduleDll = new ModuleDll();
            return moduleDll.GetModuleAll();
        }

    }
}
