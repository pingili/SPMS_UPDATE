using AutoMapper;
using BusinessEntities;
using BusinessLogic;
//using BusinessLogic.Interface;
using MFIS.Web.Areas.Federation.Models;
using MFIS.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CoreComponents;
using System.Text;

namespace MFIS.Web.Areas.Federation.Controllers.TransactionControllers
{
    public class GroupLoanApplicationController : BaseController
    {
         
        #region Global Variables
        private readonly GroupService _groupService;
        private readonly FundSourceService _fundsourseService;
        private readonly PanchayatService _panchayatService;
        private readonly LoanPurposeService _loanpurposeService;
        private readonly GroupLoanApplicationService _grouploanapplicationService;
        private readonly ProjectService _projectService;
        public GroupLoanApplicationController()
        {
            _groupService = new GroupService();
            _fundsourseService = new FundSourceService();
            _panchayatService = new PanchayatService();
            _loanpurposeService = new LoanPurposeService();
            _grouploanapplicationService = new GroupLoanApplicationService();
            _projectService = new ProjectService();
        }
        #endregion Global Variables
        [HttpGet]
        public ActionResult CreateGroupLoanApplication(string id)
        {
            int loanmasterId = string.IsNullOrEmpty(id.DecryptString()) ? default(int) : Convert.ToInt32(id.DecryptString());
            
            GroupLoanApplicationDto grouploanapplicationDto = new GroupLoanApplicationDto();
            if (loanmasterId > 0)
            {
                //grouploanapplicationDto = _grouploanapplicationService.GetByID(loanmasterId);
                grouploanapplicationDto = _grouploanapplicationService.GetGroupApplicationDetailsByID(loanmasterId);
            }

            GroupLoanApplicationModel objgrouploanmodel = Mapper.Map<GroupLoanApplicationDto, GroupLoanApplicationModel>(grouploanapplicationDto);
            if (objgrouploanmodel.GroupID > 0)
            {
                JsonResult groupDetails = GetGroupName(objgrouploanmodel.GroupID);

                objgrouploanmodel.GroupName = groupDetails.Data.GetType().GetProperty("GroupName").GetValue(groupDetails.Data, null).ToString();
                objgrouploanmodel.VillageCode = groupDetails.Data.GetType().GetProperty("VillageCode").GetValue(groupDetails.Data, null).ToString();
                objgrouploanmodel.VillageName = groupDetails.Data.GetType().GetProperty("VillageName").GetValue(groupDetails.Data, null).ToString();
                objgrouploanmodel.ClusterCode = groupDetails.Data.GetType().GetProperty("ClusterCode").GetValue(groupDetails.Data, null).ToString();
                objgrouploanmodel.ClusterName = groupDetails.Data.GetType().GetProperty("ClusterName").GetValue(groupDetails.Data, null).ToString();
            }

            LoadGroupLoanApplicationDropDowns();

            return View(objgrouploanmodel);
        }
        [HttpPost]
        public ActionResult CreateGroupLoanApplication(GroupLoanApplicationModel loanapplicationmodel)
        {
            var resultDto = new ResultDto();
            var loanapplicationDto = Mapper.Map<GroupLoanApplicationModel, GroupLoanApplicationDto>(loanapplicationmodel);
            loanapplicationDto.UserID = UserInfo.UserID;
            if (loanapplicationDto.LoanMasterId == 0)
                resultDto = _grouploanapplicationService.Insert(loanapplicationDto);
            else
                resultDto = _grouploanapplicationService.Update(loanapplicationDto);

            LoadGroupLoanApplicationDropDowns();

            ViewBag.Result = resultDto;
            loanapplicationmodel.LoanMasterId = resultDto.ObjectId;
            loanapplicationmodel.LoanCode = resultDto.ObjectCode;

            return View(loanapplicationmodel);
        }

        private void LoadGroupLoanApplicationDropDowns()
        {
            List<GroupLookupDto> lstGroupDto = _groupService.Lookup();
            SelectList lstgroup = new SelectList(lstGroupDto, "GroupID", "GroupCode");
            ViewBag.GroupNames = lstgroup;

            List<SelectListDto> lstProjects = _projectService.GetProjectSelectList();
            SelectList slProjects = new SelectList(lstProjects, "ID", "Text");
            ViewBag.projects = slProjects;

            List<LoanPurposeLookupDto> lstLoanpurposeDto = _loanpurposeService.Lookup();
            SelectList lstloanpurpose = new SelectList(lstLoanpurposeDto, "LoanPurposeID", "Purpose");
            ViewBag.LoanPurposeName = lstloanpurpose;
            List<FundSourceLookup> lstFundSourseDto = _fundsourseService.Lookup();
            SelectList lstfundsourse = new SelectList(lstFundSourseDto, "FundSourceID", "FundSourceName");
            ViewBag.FundSourse = lstfundsourse;
        }

        public JsonResult GetGroupName(int id)
        {
            GroupMasterDto groupMasterDto = _groupService.GetByID(id);
            PanchayatLookupDto panchayatlookupDto = _panchayatService.GetByGroupID(id);
            return Json(new { VillageName = panchayatlookupDto.Village, ClusterName = panchayatlookupDto.Cluster, GroupName = groupMasterDto.GroupName, VillageCode = panchayatlookupDto.VillageCode, ClusterCode = panchayatlookupDto.ClusterCode });
        }
        public ActionResult GroupLoanApplicationLookup()
        {
            var lstgrouploanapplicationDto = _grouploanapplicationService.Lookup();
            return View(lstgrouploanapplicationDto);
        }
        [HttpGet]
        public ActionResult DeleteGroupLoanApplication(string Id)
        {
            int loanmasterId = DecryptQueryString(Id);

            if (loanmasterId < 1)
                return RedirectToAction("GroupLoanApplicationLookup");

            ResultDto resultDto = _grouploanapplicationService.Delete(loanmasterId, UserInfo.UserID);

            TempData["Result"] = resultDto;

            return RedirectToAction("GroupLoanApplicationLookup");
        }

        [HttpGet]
        public ActionResult ActiveInactiveGroupLoanApplication(string Id)
        {
            int loanmasterId = DecryptQueryString(Id);

            if (loanmasterId < 1)
                return RedirectToAction("GroupLoanApplicationLookup");

            ResultDto resultDto = _grouploanapplicationService.ChangeStatus(loanmasterId, UserInfo.UserID);

            TempData["Result"] = resultDto;

            return RedirectToAction("GroupLoanApplicationLookup");
        }
        [HttpGet]
        public ActionResult LoanApplicationView(string id)
        {
            int loanmasterId = string.IsNullOrEmpty(id.DecryptString()) ? default(int) : Convert.ToInt32(id.DecryptString());
            if (loanmasterId<=0)
                return RedirectToAction("GroupLoanApplicationLookup");
            GroupLoanApplicationDto grouploanapplicationdto = _grouploanapplicationService.GetViewById(loanmasterId);
            return View(grouploanapplicationdto);
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

    }
}
