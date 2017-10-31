using BusinessEntities;
using BusinessLogic;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MFIS.Web.Controllers
{
    public class RoleModulesController : BaseController
    {
        #region Global Variables
        private readonly RoleModulesService _roleModulesService;
        private readonly ModuleServices _modulesService;
        public RoleModulesController()
        {
            _roleModulesService = new RoleModulesService();
            _modulesService = new ModuleServices();

        }

        #endregion Global Variables
        [HttpGet]
        public ActionResult CreateRoleModules()
        {
            BindDropDownRoles();
            CheckBoxModules();
            return View();
        }
        [HttpPost]
        public ActionResult CreateRoleModules(FormCollection form)
        {
            string roleidInput = form.Get("roleid");
            int roleid=0;
            int.TryParse(roleidInput,out roleid);
            string modules = form.Get("modules[]");

            int effectedCount= _roleModulesService.InsertRoleModules(roleid, modules);

            return Json(new { isSuccess=true });
        }
        public ActionResult RoleModuleLookup()
        {
            List<RoleDto> lstRoles = _roleModulesService.Selectlist();
            return View(lstRoles);
        }
        private void BindDropDownRoles()
        {
            RoleModulesDto objRoleModulesDto = new RoleModulesDto();
            List<RoleDto> lstRoleDto = _roleModulesService.Selectlist();
            SelectList Slsroles = new SelectList(lstRoleDto, "RoleId", "RoleName");
            ViewBag.Roles = Slsroles;
        }
        public ActionResult RoleModulesLookup()
        {
            List<RoleModulesDto> lstRoleModules = _roleModulesService.GetAllRoleModules();

            return View(lstRoleModules);
        }
        public ActionResult ModulesBindByRoleId(int id)
        {
            List<ModuleDto> modules = _modulesService.GetModuleByRoleId(id);
            return Json(new { module = modules });
        }
        public ActionResult BindModulesByRoleId(int id)
        {
            List<ModuleDto> lstRoleModules = _modulesService.GetModuleByRoleId(id);

            return Json(new { Modules=lstRoleModules});
        }
        private void CheckBoxModules()
        {
            List<ModuleDto> modules = _modulesService.GetModuleAll();
            ViewBag.Modules = modules;

        }
        [HttpPost]
        public ActionResult ModuleActions(int id)
        {

            List<ModuleActionDto> lstModules = _roleModulesService.GetModuleActions(id);
            return Json(new { Modules = lstModules }, JsonRequestBehavior.AllowGet);

        }
        

        public ActionResult Insert(FormCollection Form)
        {
            //ResultDto objResultDto = new ResultDto();
            //RoleModulesDto objRoleModulesDto = new RoleModulesDto();
            //objRoleModulesDto.RoleModuleCode = Form["rolemodulecode"];
            //objRoleModulesDto.RoleId =Convert.ToInt32( Form["ddlRoles"]);
            //objRoleModulesDto.ModuleId = Convert.ToInt32(Form[""]);

            return View("CreateRoleModules");
        }

        //public ActionResult RoleModuleLookup() 
        //{
        //    return View();
        //}

    }
}
