using BusinessEntities;
using BusinessLogic;
//using BusinessLogic.Interface;
using BusinessLogic.Implementation;
using CoreComponents;
using CoreComponents.Security;
using MFIS.Web.Areas.Federation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MFIS.Web.Controllers
{
    public class LoginController : BaseController
    {
        //
        // GET: /Federation/Login/
        private readonly GroupService _groupService;
        public LoginController()
        {
            _groupService = new GroupService();
        }

        [HttpGet]
        public ActionResult CreateLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateLogin(FormCollection form)
        {
            try
            {
                string username = form.Get("username");
                string password = form.Get("password");
                string type = form.Get("ddlLoginMode");

                bool isFederation = type.ToLower() != "group";

                LoginBll objLoginBll = new LoginBll();
                ResultDto objResultDto = objLoginBll.ValidateLogin(username, password);
                if (objResultDto.ObjectId > 0)
                {
                    UserInfo = new MasterService().GetLoginMasterInfo(objResultDto.ObjectId, isFederation);
                    return Json(new { message = objResultDto.Message, isSueecess = true });
                }
                else
                {
                    return Json(new { message = objResultDto.Message, isSueecess = false });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public ActionResult InitialLanching()
        {
            if (UserInfo.modules != null && UserInfo.modules.Count > 0)
            {
                ModuleDto parentModule = UserInfo.modules.FindAll(p => p.ParentID == 0).OrderBy(o => o.DisplayOrder).ToList().FirstOrDefault();
                ModuleDto LunchingModuleDto = UserInfo.modules.FindAll(m => m.ParentID == parentModule.ModuleId).OrderBy(o => o.DisplayOrder).ToList().FirstOrDefault();
                Session["Modules"] = UserInfo.modules;
                if (GroupInfo == null || GroupInfo.GroupID < 1)
                {
                    string[] url = LunchingModuleDto.Url.Split('/');
                    if (url.Length > 2)
                        return RedirectToAction(url[2], url[1], new { Area = url[0] });
                    else
                        return RedirectToAction(url[1], url[0]);
                }
                else
                {
                    return RedirectToAction("ViewGroup", "Group", new { Area = "Federation", id = GroupInfo.GroupID });
                }
            }
            else
            {
                return new EmptyResult();
            }
        }

        [HttpGet]
        public ActionResult SelectGroup()
        {
            LoginBll _loginService = new LoginBll();

            string clusterName = string.Empty;

            var lstGroupMasterViewDto = _loginService.GetSelectGroupDetails(UserInfo.UserID, out clusterName);

            ViewBag.ClusterName = clusterName;

            return View(lstGroupMasterViewDto);
        }

        [HttpPost]
        public ActionResult SelectGroup(int groupId)
        {
            //int iGroupId = default(int);
            //int.TryParse(groupId.DecryptString(), out iGroupId);
            if (groupId < 1)
                return Json(new { isSueecess = false });

            try
            {
                GroupService _groupService = new GroupService();
                GroupMasterDto group = _groupService.GetByID(groupId);
                ViewBag.lstGroups = group;
                GroupInfo objGroupInfo = new BusinessEntities.GroupInfo();
                objGroupInfo.GroupID = groupId;
                objGroupInfo.GroupName = group.GroupName;
                objGroupInfo.GroupCode = group.GroupCode;
                objGroupInfo.TEGroupName = group.TEGroupName;
                objGroupInfo.Village = group.Village;
                objGroupInfo.Cluster = group.ClusterName;
                objGroupInfo.MeetingDay = group.MeetingDay;
                objGroupInfo.MeetingDate = group.MeetingDate;
                objGroupInfo.LockStatus = group.LockStatus;
                objGroupInfo.LockStatusCode = group.LockStatusCode;
                GroupInfo = objGroupInfo;

                return Json(new { isSueecess = true });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public string GetEncryptString(string str)
        {
            return str.DecryptString();
        }
    }
}
