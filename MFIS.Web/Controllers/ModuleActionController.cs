using BusinessEntities;
using BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CoreComponents;

namespace MFIS.Web.Controllers
{
    public class ModuleActionController : BaseController
    {
        #region Global Variables
        private readonly ModuleActionService _moduleActionService;
        public ModuleActionController()
        {
            _moduleActionService = new ModuleActionService();
        }
        #endregion Global Variables
        [HttpGet]
        public ActionResult AddModuleAction( string id)
        {
            int ID=string.IsNullOrEmpty(id) ? default(int) : Convert.ToInt32(id);
           
            var moduleActionDto = new ModuleActionDto();
            if (ID > 0)
            {
                moduleActionDto = _moduleActionService.GetModuleActionByID(ID);
            }
            BindDropdowns();
           
            return View(moduleActionDto);
        }
        private void BindDropdowns()
        {
            List<ModulesDTO> modulesSelectList = _moduleActionService.GetModules();
            SelectList moduleSelectList = new SelectList(modulesSelectList, "ModuleId", "ModuleName", "Select Module");
            ViewBag.modulesSelectList = moduleSelectList;
        }

        [HttpPost]
        public ActionResult AddModuleAction(ModuleActionDto moduleActionDto)
        {
            BindDropdowns();
            var resultDto = new ResultDto();
            if (moduleActionDto.ModuleActionId == 0)
                resultDto = _moduleActionService.Insert(moduleActionDto);
            else
                resultDto = _moduleActionService.Update(moduleActionDto);
            ViewBag.Result = resultDto;
            return View(moduleActionDto);
        }
        [HttpGet]
        public ActionResult ModuleActionLookup()
        {
            var lstmoduleActionLookupDto = _moduleActionService.GetLookUp();
            return View("ModuleActionLookup", lstmoduleActionLookupDto);
        }
        //public ActionResult InsertModuleAction(int mName,string action,string Url,int status)
        //{
        //    ModuleActionDto objDto=new ModuleActionDto();
        //    objDto.ModuleId=mName;
        //    objDto.ActionName=action;
        //    objDto.Url=Url;
        //    objDto.Status=status;
        //    var resultDto = new ResultDto();
        //  resultDto=  _moduleActionService.Insert(objDto);
        //  ViewBag.Result = resultDto;
        //  return View(objDto);
        //}
        [HttpGet]
        public ActionResult DeleteModuleAction(string id)
        {
            int ID = string.IsNullOrEmpty(id) ? default(int) : Convert.ToInt32(id);

            if (ID < 1)
                return RedirectToAction("ModuleActionLookup");

            ResultDto resultDto = _moduleActionService.Delete(ID);

            TempData["Result"] = resultDto;

            return RedirectToAction("ModuleActionLookup");

        }
        [HttpGet]
        public ActionResult ActiveInactiveModuleAction(string Id)
        {
            int ID = string.IsNullOrEmpty(Id) ? default(int) : Convert.ToInt32(Id);

            if (ID < 1)
                return RedirectToAction("ModuleActionLookup");
            ResultDto resultDto = new ResultDto();
             resultDto = _moduleActionService.ChangeStatus(ID);

            TempData["Result"] = resultDto;

            return RedirectToAction("ModuleActionLookup");
        }
    }
}
