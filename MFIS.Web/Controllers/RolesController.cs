//using BusinessLogic.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLogic;
using BusinessEntities;
using CoreComponents;

namespace MFIS.Web.Controllers
{
    public class RolesController : BaseController
    {
        #region Global Variables
        private RolesService _rolesService;
        public RolesController()
        {
            _rolesService = new RolesService();
        }

        #endregion Global Variables
        [HttpGet]
        public ActionResult Role(int? ID)
        {
            //int ID=string.IsNullOrEmpty(id.DecryptString()) ? default(int) : Convert.ToInt32(id.DecryptString());
                
           var roleDto=new RoleDto();

            if (ID >0)
            {
                roleDto = _rolesService.GetById(ID);
            }

            return View(roleDto);
        }
        [HttpPost]
        public ActionResult Role(int RoleId,String RoleName, string RoleCode)
        {
            
            ResultDto objResult = new ResultDto(); 

            RoleDto objrolesdto = new RoleDto();
            objrolesdto.RoleId = RoleId;
            objrolesdto.RoleName = RoleName;
            objrolesdto.RoleCode = RoleCode;
            objResult = _rolesService.Insert(objrolesdto);
         

           ViewBag.Result = objResult;

           return View("Role");
        }
        [HttpGet]
        public ActionResult RolesLookUp()
        {
            var lstRoles = _rolesService.GetLookUp();
            return View(lstRoles);
        }
        public ActionResult DeleteRole(int id)
        {
            if(id<1)
            {
                return RedirectToAction("RolesLookUp");
            }
            ResultDto resultDto=new ResultDto();
            resultDto=_rolesService.RoleDelete(id);

            TempData["Result"] = resultDto;
            return RedirectToAction("RolesLookUp");
        }
        public ActionResult ChangeStatus(int id)
        {
            if (id < 1)
            {
                return RedirectToAction("RolesLookUp");
            }
            ResultDto resultDto = new ResultDto();
            resultDto = _rolesService.ChangeStatus(id);

            TempData["Result"] = resultDto;
            return RedirectToAction("RolesLookUp");
 
        }

    }
}

