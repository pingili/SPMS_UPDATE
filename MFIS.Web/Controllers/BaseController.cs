using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLogic;
using Utilities;
//using BusinessLogic.Interface;
using System.Globalization;
using CoreComponents;
using BusinessEntities;
using BusinessLogic.Implementation;

namespace MFIS.Web.Controllers
{

    public class BaseController : Controller, IActionFilter, IExceptionFilter
    {
        #region Global Variables

        private readonly CommonService _commonService;
        private readonly GroupService _groupService;
        private readonly BankService _bankService;
        private readonly VillageService _villageService;
        private readonly ClusterService _clusterService;
        private readonly InterestService _interestService;
        private readonly AccountHeadService _accountHeadService;
        private readonly PanchayatService _panchayatService;
        private readonly MemberService _memberService;

        //public readonly nt UserInfo.UserID;
        protected CultureInfo provider = CultureInfo.InvariantCulture;
        public CurrentUser UserInfo
        {
            get
            {
                return (CurrentUser)HttpContext.Session[Constants.SessionKeys.SK_USERINFO];
            }
            set
            {
                HttpContext.Session[Constants.SessionKeys.SK_USERINFO] = value;
            }
        }
        public GroupInfo GroupInfo
        {
            get
            {
                return (GroupInfo)HttpContext.Session[Constants.SessionKeys.SK_GROUPINFO];
            }
            set
            {
                HttpContext.Session[Constants.SessionKeys.SK_GROUPINFO] = value;
            }
        }

        public BaseController()
        {

            _groupService = new GroupService();
            _bankService = new BankService();
            _villageService = new VillageService();
            _clusterService = new ClusterService();
            _interestService = new InterestService();
            _accountHeadService = new AccountHeadService();
            _panchayatService = new PanchayatService();
            _memberService = new MemberService();
            _commonService = new CommonService();
            //UserInfo.UserID = SESS_USER_ID;
        }
        #endregion Global Variables

        public SelectList GetDropDownListByMasterCode(Enums.RefMasterCodes refMastercode)
        {
            var refValues = _commonService.PrepareSelectListByRefCode(refMastercode.ToString());

            SelectList dropDownList = new SelectList(refValues, Constants.REF_ID, Constants.REF_VALUE);

            return dropDownList;
        }

        private int SESS_USER_ID
        {
            get
            {
                int userID = UserInfo.UserID;

                try
                {
                    int.TryParse(HttpContext.Session[Constants.SessionKeys.SK_USERID].ToString(), out userID);
                }
                catch
                {

                }
                return userID;
            }
        }

        private CurrentUser SESS_USERINFO
        {
            get
            {
                return (CurrentUser)HttpContext.Session[Constants.SessionKeys.SK_USERINFO];
            }
            set
            {
                HttpContext.Session[Constants.SessionKeys.SK_USERINFO] = value;
            }
        }

        public List<BankMasterDto> GetBanksList(FormCollection form)
        {
            int arrlst = Convert.ToInt32(Request.Form["hdnMaxRateIndex"]);
            //int count = Convert.ToInt32(Request.Form["Arrlst"].Length);
            long objectid = Convert.ToInt64(Request.Form["hdnObjectID"]);
            List<BankMasterDto> lstbank = new List<BankMasterDto>();

            for (int i = 1; i <= arrlst; i++)
            {
                if (form["hdnentryId_" + i] == null)
                    continue;

                BankMasterDto objbank = new BankMasterDto();
                if (form["hdnentryId_" + i] == string.Empty || form["hdnentryId_" + i] == "0")
                    objbank.BankEntryID = 0;
                else
                    objbank.BankEntryID = Convert.ToInt32(form["hdnentryId_" + i]);

                var BANKNAME = Convert.ToInt32(form["hdnbankname_" + i]);

                objbank.BankName = BANKNAME;
                if (form["hdnbname_" + i] != null)
                    objbank.BName = Convert.ToString(form["hdnbname_" + i]);
                objbank.BranchName = form["hdnbranchcode_" + i];
                objbank.IFSC = form["hdnifsc_" + i];
                objbank.AccountType = Convert.ToInt32(form["hdnaccounttypeid_" + i]);
                if (form["hdnaccounttypeText_" + i] != null)
                    objbank.AccountTypeText = Convert.ToString(form["hdnaccounttypeText_" + i]);
                objbank.AccountNumber = form["hdnaccountnumber_" + i];
                lstbank.Add(objbank);
            }

            return lstbank;
        }

        public void UpdateGroupInfoSessionbyGroupId(int groupId)
        {
            GroupService _groupService = new GroupService();

            GroupMasterDto group = _groupService.GetByID(groupId);
            
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
        }

        public int DecryptQueryString(string encryptedId)
        {
            return string.IsNullOrEmpty(encryptedId.DecryptString()) ? default(int) : Convert.ToInt32(encryptedId.DecryptString());
        }
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }

        [HttpGet]
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("CreateLogin", "Login");
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!(filterContext.RouteData.Values["controller"].ToString().ToLower() == "login" &&
                (filterContext.RouteData.Values["action"].ToString().ToLower() == "createlogin"
                || filterContext.RouteData.Values["action"].ToString().ToLower() == "login")))
            {
                if (UserInfo != null && UserInfo.UserID != null && UserInfo.UserID > 0)
                {
                    if (GroupInfo != null && GroupInfo.GroupID > 0)
                    {
                        ViewBag.OpeningMeetingDate = GroupInfo.MeetingDate;
                    }

                    ViewBag.FinancialYearBegin = UserInfo.FinancialYearBegin;
                    ViewBag.FinancialYearEnd = UserInfo.FinancialYearEnd;

                }
                else
                {
                    Session.Clear();
                    filterContext.Result = new RedirectResult("~/Login/CreateLogin");

                }
            }
        }

        #region Reference Master - DynamicDropdowns
        public SelectList GetMasterDropDownSelectList(string refCode)
        {
            MasterService _service = new MasterService();
            var slResultDto = _service.GetMasterDropDownResult(refCode);
            SelectList sl = new SelectList(slResultDto, "ID", "Text");
            return sl;
        }

        public List<SelectListDto> GetMasterDropDownDto(string refCode)
        {
            MasterService _service = new MasterService();
            var slResultDto = _service.GetMasterDropDownResult(refCode);
            return slResultDto;
        }
        #endregion

        //protected override void OnException(ExceptionContext filterContext)
        //{
        //    if (filterContext.ExceptionHandled)
        //    {
        //        return;
        //    }

        //    Session["Message"] = filterContext.Exception.Message;
        //    filterContext.Result = new ViewResult
        //    {
        //        ViewName = "~/Views/Shared/Error.cshtml"
        //    };
        //    filterContext.ExceptionHandled = true;
        //}
    }
}
