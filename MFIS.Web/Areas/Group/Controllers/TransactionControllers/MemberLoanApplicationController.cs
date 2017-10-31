using AutoMapper;
using BusinessEntities;
using BusinessLogic;
//using BusinessLogic.Interface;
using MFIS.Web.Areas.Group.Models;
using MFIS.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CoreComponents;
using System.Text;
using DataLogic;
using Utilities;
using BusinessLogic.Implementation;

namespace MFIS.Web.Areas.Group.Controllers.TransactionControllers
{
    public class MemberLoanApplicationController : BaseController
    {
        #region Global Variables
        private readonly MemberService _memberService;
        private readonly LoanPurposeService _loanpurposeService;
        private readonly GroupOtherReceiptService _groupOtherReceiptService;
        private readonly MemberLoanApplicationService _memberloanapplicationService;
        private readonly ProjectService _projectService;
        public MemberLoanApplicationController()
        {
            _memberService = new MemberService();
            _groupOtherReceiptService = new GroupOtherReceiptService();
            _loanpurposeService = new LoanPurposeService();
            _memberloanapplicationService = new MemberLoanApplicationService();
            _projectService = new ProjectService();
        }
        #endregion Global Variables

        [HttpGet]
        public ActionResult CreateMemberLoanApplication(string id)
        {
            int loanmasterId = string.IsNullOrEmpty(id.DecryptString()) ? default(int) : Convert.ToInt32(id.DecryptString());

            int GroupId = GroupInfo.GroupID;

            LoadDropdowns();

            bool isView = false;
            if (Request.QueryString["isView"] != null)
                isView = Request.QueryString["isView"] == "1";

            ViewBag.isView = isView;

            MemberLoanApplicationDto memberLoanApplicationDto = new MemberLoanApplicationDto();
            if (loanmasterId > 0)
                memberLoanApplicationDto = _memberloanapplicationService.GetMemberLoanApplicationDetailsById(loanmasterId);

            return View(memberLoanApplicationDto);
        }

        [HttpPost]
        public ActionResult CreateMemberLoanApplication(MemberLoanApplicationDto objLoan)
        {
            var resultDto = new ResultDto();
            int GroupId = GroupInfo.GroupID;

            bool IsChecked = false;

            if (objLoan.LoanMasterId <= 0)
            {
                MemberLoanDisbursementDataAccess dl = new MemberLoanDisbursementDataAccess();
                IsChecked = dl.CheckLoanExisted(objLoan.MemberID, objLoan.InterestMasterID);
                if (IsChecked)
                {
                    resultDto.Message = "Unable to create Loan: Outstanding Amount Exists for this member";
                }
            }

            objLoan = LoadMemberLoanApprovalInfo(objLoan);
            objLoan.UserID = UserInfo.UserID;
            objLoan.GroupID = GroupInfo.GroupID;

            if (!IsChecked)
            {
                if (string.IsNullOrEmpty(objLoan.StatusCode) || objLoan.StatusCode == "INITIATED")
                {
                    resultDto = _memberloanapplicationService.InsertUpdateMemberLoanApplication(objLoan);

                    objLoan.LoanCode = resultDto.ObjectCode;
                    objLoan.LoanMasterId = resultDto.ObjectId;
                }

                if (objLoan.LoanMasterId > 0)
                {
                    _memberloanapplicationService.SubmitLoanApplicationApproval((MemberLoanApprovalDto)objLoan, UserInfo.UserID, objLoan.LoanMasterId, true);
                    objLoan = _memberloanapplicationService.GetMemberLoanApplicationDetailsById(objLoan.LoanMasterId);
                }
            }

            TempData["Result"] = resultDto;

            LoadDropdowns();

            return View(objLoan);
        }

        [HttpPost]
        public ActionResult ConfirmMemberLoanApplication(MemberLoanApplicationDto objLoan)
        {
            LoadMemberLoanApprovalInfo(objLoan);

            int count = _memberloanapplicationService.SubmitLoanApplicationApproval((MemberLoanApprovalDto)objLoan, UserInfo.UserID, objLoan.LoanMasterId);

            return RedirectToAction("MemberLoanApplicationLookup");
        }

        private MemberLoanApplicationDto LoadMemberLoanApprovalInfo(MemberLoanApplicationDto objLoan)
        {
            string statusCode = objLoan.StatusCode;
            int loanSanctionAmount = default(int);
            int NoOfInstallments = default(int);

            if (statusCode == "INITIATED" || string.IsNullOrWhiteSpace(statusCode))
            {
                int.TryParse(Request.Form["GLSactionAmount"], out loanSanctionAmount);
                int.TryParse(Request.Form["GLInstallments"], out NoOfInstallments);

                objLoan.ApprovalType = "CA";
                objLoan.LoanSanctionAmount = loanSanctionAmount;
                objLoan.NoOfInstallments = NoOfInstallments;
                objLoan.ApprovalComments = Convert.ToString(Request.Form["GLComments"]);
                objLoan.Action = Convert.ToString(Request.Form["GLAction"]);
                objLoan.ActionDate = Convert.ToDateTime(Request.Form["lblGLActionDate"]);
            }
            else if (statusCode == "FOR_CDA_REVIEW")
            {
                int.TryParse(Request.Form["CDASactionAmount"], out loanSanctionAmount);
                int.TryParse(Request.Form["CDAInstallments"], out NoOfInstallments);
                objLoan.ApprovalType = "CDA";
                objLoan.LoanSanctionAmount = loanSanctionAmount;
                objLoan.NoOfInstallments = NoOfInstallments;
                objLoan.ApprovalComments = Convert.ToString(Request.Form["CDAComments"]);
                objLoan.Action = Convert.ToString(Request.Form["hdnCDAAction"]);
                objLoan.ActionDate = Convert.ToDateTime(Request.Form["lblCDAActionDate"]);
            }
            else if (statusCode == "FOR_RI_REVIEW")
            {
                int.TryParse(Request.Form["RISactionAmount"], out loanSanctionAmount);
                int.TryParse(Request.Form["RIInstallments"], out NoOfInstallments);
                objLoan.ApprovalType = "RI";
                objLoan.LoanSanctionAmount = loanSanctionAmount;
                objLoan.NoOfInstallments = NoOfInstallments;
                objLoan.ApprovalComments = Convert.ToString(Request.Form["RIComments"]);
                objLoan.Action = Convert.ToString(Request.Form["hdnRIAction"]);
                objLoan.ActionDate = Convert.ToDateTime(Request.Form["lblRIActionDate"]);
            }
            else if (statusCode == "FOR_MD_APPROVAL")
            {
                int.TryParse(Request.Form["MDSactionAmount"], out loanSanctionAmount);
                int.TryParse(Request.Form["MDInstallments"], out NoOfInstallments);
                objLoan.ApprovalType = "MD";
                objLoan.LoanSanctionAmount = loanSanctionAmount;
                objLoan.NoOfInstallments = NoOfInstallments;
                objLoan.ApprovalComments = Convert.ToString(Request.Form["MDComments"]);
                objLoan.Action = Convert.ToString(Request.Form["hdnMDAction"]);
                objLoan.ActionDate = Convert.ToDateTime(Request.Form["lblMDActionDate"]);
            }
            return objLoan;
        }

        private void LoadDropdowns()
        {
            List<MemberLookupDto> lstMemberDto = _memberService.LookUp(GroupInfo.GroupID);
            SelectList lstmember = new SelectList(lstMemberDto, "MemberID", "MemberName");
            ViewBag.MemberNames = lstmember;

            List<SelectListDto> lstProjects = _projectService.GetProjectSelectList();
            SelectList slProjects = new SelectList(lstProjects, "ID", "Text");
            ViewBag.projects = slProjects;

            List<LoanPurposeLookupDto> lstLoanpurposeDto = _loanpurposeService.Lookup();
            SelectList lstloanpurpose = new SelectList(lstLoanpurposeDto, "LoanPurposeID", "Purpose");
            ViewBag.LoanPurposeName = lstloanpurpose;

            List<GroupMeetingDto> lstGroupMeetings = _groupOtherReceiptService.GetGroupOpenMeetingDates(GroupInfo.GroupID);
            ViewBag.MonthMeetings = new SelectList(lstGroupMeetings, "DisplayMeetingDate", "DisplayMeetingDate");

            InterestService _interestService = new InterestService();
            List<SelectListDto> slistInterestDto = _interestService.GetInterestsSelectList(GroupInfo.GroupID);
            ViewBag.Interest = new SelectList(slistInterestDto, "ID", "Text");

            List<SelectListDto> modes = GetMasterDropDownDto("MEETING_FREQUENCY");
            ViewBag.Modes = new SelectList(modes, "ID", "Text", modes.Find(l => l.Code == "MONTH").ID);

            ViewBag.MeetingDueDay = GroupInfo.MeetingDay;

            ViewBag.RoleCode = UserInfo.RoleCode;
        }

        [HttpGet]
        public ActionResult MemberLoanApplicationLookup()
        {
            int GroupID = GroupInfo.GroupID;
            var lstmemberloanapplicationDto = _memberloanapplicationService.Lookup(GroupID, UserInfo.UserID);
            return View(lstmemberloanapplicationDto);
        }

        [HttpGet]
        public ActionResult DeleteMemberLoanApplication(string Id)
        {
            int loanmasterId = DecryptQueryString(Id);

            if (loanmasterId < 1)
                return RedirectToAction("MemberLoanApplicationLookup");

            ResultDto resultDto = _memberloanapplicationService.Delete(loanmasterId, UserInfo.UserID);

            TempData["Result"] = resultDto;

            return RedirectToAction("MemberLoanApplicationLookup");
        }

        [HttpGet]
        public ActionResult ActiveInactiveMemberLoanApplication(string Id)
        {
            int loanmasterId = DecryptQueryString(Id);

            if (loanmasterId < 1)
                return RedirectToAction("MemberLoanApplicationLookup");

            ResultDto resultDto = _memberloanapplicationService.ChangeStatus(loanmasterId, UserInfo.UserID);

            TempData["Result"] = resultDto;

            return RedirectToAction("MemberLoanApplicationLookup");
        }

        public ActionResult GetLoanPurposeByProjectID(int ProjectID)
        {
            StringBuilder sbOptions = new StringBuilder();
            List<LoanPurposeDto> lstLoanPurposeDto = new List<LoanPurposeDto>();
            lstLoanPurposeDto = _loanpurposeService.GetLoanPurposeByProjectID(ProjectID);
            foreach (var item in lstLoanPurposeDto)
            {
                sbOptions.Append("<option value=" + item.LoanPurposeID + ">" + item.Purpose + "</option>");
            }
            return Content(sbOptions.ToString());
        }

        public ActionResult GetLoanAccountHeadInterestDetails(int groupInterestId)
        {
            var lstInterestRates = _memberloanapplicationService.GetGroupLoanDepositAccountHeads(GroupInfo.GroupID, groupInterestId, "L");

            LoanAccountHeadDto objInterestRates = lstInterestRates.Count > 0 ? lstInterestRates[0] : null;

            if (objInterestRates == null)
                objInterestRates = new LoanAccountHeadDto();

            return Json(new { ROI = objInterestRates.ROI, interestRateId = objInterestRates.InterestRateId, DueDay = GroupInfo.MeetingDay, PrincipalAhName = objInterestRates.PrincipalAHName, InterestAhName = objInterestRates.InterestAHName }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CheckLoanExisted(int InterestID, int MemberID)
        {

            bool IsChecked = false;
            MemberLoanDisbursementDataAccess dl = new MemberLoanDisbursementDataAccess();
            IsChecked = dl.CheckLoanExisted(MemberID, InterestID);
            return Json(IsChecked);

        }

        [HttpGet]
        public ActionResult MemberLoanApplicationView(string id, string type)
        {
            int loanMasterId = string.IsNullOrEmpty(id.DecryptString()) ? default(int) : Convert.ToInt32(id.DecryptString());
            MemberLoanApplicationViewDto objLoanViewDto = _memberloanapplicationService.GetMemberLoanApplicationViewDetails(loanMasterId);
            ViewBag.isViewPage = type != "A";
            return View(objLoanViewDto);
        }

        [HttpPost]
        public ActionResult MemberLoanApplicationView(FormCollection form)
        {
            string id = form.Get("LoanMasterId");
            string statusCode = form.Get("StatusCode");
            string comments = string.Empty;
            string action = "APP";


            if (statusCode == "INITIATED" || statusCode == "FOR_GROUP_REVERIFY")
            {
                comments = form.Get("GroupApprovalComments");
            }
            else
            {
                action = form.Get("ddlAction");

                if (statusCode == "FOR_CLUSTER_REVIEW" || statusCode == "FOR_CLUSTER_REVERIFY")
                {
                    comments = form.Get("ClusterApprovalComments");
                }
                else
                {
                    comments = form.Get("FederationApprovalComments");
                }
            }

            Enums.ApprovalActions actionType = Utilities.Enums.ApprovalActions.APP;
            if (action == "CAN")
                actionType = Utilities.Enums.ApprovalActions.CAN;
            else if (action == "REJ")
                actionType = Utilities.Enums.ApprovalActions.REJ;

            string loanCode = form.Get("LoanCode");

            int loanMasterId = string.IsNullOrEmpty(id.DecryptString()) ? default(int) : Convert.ToInt32(id.DecryptString());
            ResultDto res = _memberloanapplicationService.ApproveMemberLoanApplication(loanMasterId, loanCode, actionType, comments, UserInfo.UserID);

            TempData["Result"] = res;
            if (res.ObjectId > 0)
            {
                return RedirectToAction("MemberLoanApplicationLookup");
            }
            else
            {
                MemberLoanApplicationViewDto objLoanViewDto = _memberloanapplicationService.GetMemberLoanApplicationViewDetails(loanMasterId);
                ViewBag.isViewPage = false;
                return View(objLoanViewDto);
            }
        }
    }
}