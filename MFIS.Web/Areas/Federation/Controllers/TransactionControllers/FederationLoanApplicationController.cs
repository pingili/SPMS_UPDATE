using BusinessEntities;
using BusinessLogic;
using BusinessLogic.Implementation;
using MFIS.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace MFIS.Web.Areas.Federation.Controllers.TransactionControllers
{
    public class FederationLoanApplicationController : BaseController
    {
        private readonly ClusterService _clusterService;
        private readonly GroupService _groupService;
        private readonly ContraEntryService _ContraEntryService;
        private readonly MasterService _masterService;
        private readonly GroupOtherReceiptService _groupOtherReceiptService;
        private readonly FederationLoanApplicationService _federationLoanApplicationService;
        public FederationLoanApplicationController()
        {
            _clusterService = new ClusterService();
            _groupService = new GroupService();
            _ContraEntryService = new ContraEntryService();
            _masterService = new MasterService();
            _groupOtherReceiptService = new GroupOtherReceiptService();
            _federationLoanApplicationService = new FederationLoanApplicationService();
        }

        public ActionResult CreateFedLoanAppln()
        {
            FederationLoanApplicationDto objDto = new FederationLoanApplicationDto();
            LoadFederationLoanApplnDropdowns();

            return View(objDto);
        }
        [HttpPost]
        public ActionResult CreateFedLoanAppln(FederationLoanApplicationDto fedLoanAppln)
        {
           ResultDto resDto= _federationLoanApplicationService.CreateFedLoanAppln(fedLoanAppln);
            return View();
        }

        public void LoadFederationLoanApplnDropdowns()
        {
            List<SelectListDto> lstClusters = _clusterService.GetClusterSelectList();
            SelectList slClusters = new SelectList(lstClusters, "ID", "Text");
            ViewBag.clusters = slClusters;

            TypeQueryResult lst = _masterService.GetTypeQueryResult("FED_LOAN_GL_HEAD");
            ViewBag.lstGLAcHeads = new SelectList(lst.OrderBy(a => a.Name), "Id", "Name");
        }

        public ActionResult BindDropdown(string flag, int Id)
        {
            StringBuilder sbOptions = new StringBuilder();
            if (flag == "Cluster")
            {
                List<SelectListDto> lstGroups = _groupService.GetGroupByClusterID(Id);
                foreach (var item in lstGroups)
                {
                    sbOptions.Append("<option value=" + item.ID + ">" + item.Text + "</option>");
                }
            }
            if (flag == "Group")
            {
                TypeQueryResult lstBankAh = _masterService.GetTypeQueryResult("GROUP_OR_BANK_AH", Id.ToString());
                foreach (var item in lstBankAh)
                {
                    sbOptions.Append("<option value=" + item.Id + ">" + item.Name + "</option>");
                }

                var count= _federationLoanApplicationService.GetMemberCountByGroupId(Id);
                return Json(new { sbOptions=sbOptions.ToString(), MemberCount = count});

            }
            if (flag == "GLAccount")
            {
                TypeQueryResult lstSLAccounts = _masterService.GetTypeQueryResult("FED_LOAN_SL_HEADS", Id.ToString());
                foreach (var item in lstSLAccounts)
                {
                    sbOptions.Append("<option value=" + item.Id + ">" + item.Name + "</option>");
                }
            }
            return Json(sbOptions.ToString());
        }

        public ActionResult BindGRSLAccountHead(string Id)
        {
            var lst = _federationLoanApplicationService.GetGroupSLAccountByFedSLAcount(Id);
            return Json(new { GRGLA = lst.GRGLAccount, GRGLId = lst.GRGLAccountId, GRSLA = lst.GRSLAccount });
        }
    }
}
