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

namespace MFIS.Web.Areas.Group.Controllers
{
    public class LoanApplicationController : BaseController
    {
        
        #region Global Variables
        private readonly GroupService _groupService;
        private readonly PanchayatService _panchayatService;
        private readonly ReferenceValueService _referenceValueService;
        private readonly LoanPurposeService _loanpurposeService;
        private readonly LoanApplicationService _loanapplicationService;
        public LoanApplicationController()
        {
            _groupService = new GroupService();
            _referenceValueService = new ReferenceValueService();
            _panchayatService = new PanchayatService();
            _loanpurposeService = new LoanPurposeService();
            _loanapplicationService = new LoanApplicationService();
        }
        #endregion Global Variables
        [HttpGet]
        public ActionResult CreateLoanApplication()
        {
            List<GroupLookupDto> lstGroupDto = _groupService.Lookup();
            SelectList lstgroup = new SelectList(lstGroupDto, "GroupID", "GroupCode");
            ViewBag.GroupNames = lstgroup;

            List<LoanPurposeLookupDto> lstLoanpurposeDto = _loanpurposeService.Lookup();
            SelectList lstloanpurpose = new SelectList(lstLoanpurposeDto, "LoanPurposeID", "Purpose");
            ViewBag.LoanPurposeName = lstloanpurpose;

            return View();
        }
        [HttpPost]
        public ActionResult CreateLoanApplication(LoanApplicationModel loanapplicationmodel)
        {
            List<GroupLookupDto> lstGroupDto = _groupService.Lookup();
            SelectList lstgroup = new SelectList(lstGroupDto, "GroupID", "GroupCode");
            ViewBag.GroupNames = lstgroup;

            List<LoanPurposeLookupDto> lstLoanpurposeDto = _loanpurposeService.Lookup();
            SelectList lstloanpurpose = new SelectList(lstLoanpurposeDto, "LoanPurposeID", "Purpose");
            ViewBag.LoanPurposeName = lstloanpurpose;

            var resultDto = new ResultDto();
            var loanapplicationDto = Mapper.Map<LoanApplicationModel, LoanApplicationDto>(loanapplicationmodel);
            loanapplicationDto.UserID = UserInfo.UserID;
            if (loanapplicationDto.LoanMasterId == 0)
                resultDto = _loanapplicationService.Insert(loanapplicationDto);
            else
                resultDto = _loanapplicationService.Update(loanapplicationDto);

            ViewBag.Result = resultDto;

            return View(loanapplicationmodel);
        }
        public JsonResult GetGroupName(int id)
        {
            GroupMasterDto groupMasterDto = _groupService.GetByID(id);
            PanchayatLookupDto panchayatlookupDto = _panchayatService.GetByGroupID(id);
            return Json(new { VillageName = panchayatlookupDto.Village, ClusterName = panchayatlookupDto.Cluster, GroupName = groupMasterDto.GroupName ,VillageCode=panchayatlookupDto.VillageCode,ClusterCode=panchayatlookupDto.ClusterCode});
        }
    }
}
