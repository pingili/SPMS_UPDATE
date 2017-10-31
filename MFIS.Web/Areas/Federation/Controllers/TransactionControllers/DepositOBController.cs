using BusinessEntities;
using BusinessLogic;
using MFIS.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using System.Data;

namespace MFIS.Web.Areas.Federation.Controllers.TransactionControllers
{
    public class DepositOBController : BaseController
    {
        private readonly VillageService _villageService;
        private readonly GroupService _groupService;
        private readonly ClusterService _clusterService;
        private readonly InterestService _interestService;
        private readonly AccountHeadService _accountHeadService;

        public DepositOBController()
        {
            _villageService = new VillageService();
            _groupService = new GroupService();
            _clusterService = new ClusterService();
            _interestService = new InterestService();
            _accountHeadService = new AccountHeadService();
        }

        public ActionResult GetDepositOB(int? id)
        {
            int GroupID = Convert.ToInt32(id);
            DepositOBDto lstDepositOBDto = new DepositOBDto();
            DepositOBBll objDepositOBBll = new DepositOBBll();
            if (GroupID > 0)
            {
                lstDepositOBDto = objDepositOBBll.GetByID(GroupID);
            }
            List<SelectListDto> lstClusters = _clusterService.GetClusterSelectList();
            SelectList slClusters = new SelectList(lstClusters, "ID", "Text");
            ViewBag.clusters = slClusters;

            List<SelectListDto> lstInterests = _interestService.GetDepositInterestsSelectList();
            SelectList slInterests = new SelectList(lstInterests, "ID", "Text");
            ViewBag.interests = slInterests;


            List<SelectListDto> lstvilllages = _villageService.GetVillageSelectList();
            SelectList slvillages = new SelectList(lstvilllages, "ID", "Text");
            ViewBag.village = slvillages;

            List<SelectListDto> lstgroups = _groupService.GetGroupsSelectList();
            SelectList slgroups = new SelectList(lstgroups, "ID", "Text");
            ViewBag.group = slgroups;

            return View(lstDepositOBDto);
        }

        [HttpPost]
        public JsonResult SaveDepositOB(DepositOBDto depositOBDto)
        {
            DepositOBBll objDepositOBBll = new DepositOBBll();
            depositOBDto.UserID = UserInfo.UserID;
            depositOBDto = objDepositOBBll.InsertLoanOB(depositOBDto);
            string Message = null;
            if (depositOBDto.Id1 != 0 || depositOBDto.Id2 != 0 || depositOBDto.Id3 != 0 || depositOBDto.Id4 != 0)
            {
                Message = "Successfully Inserted";

            }
            else 
            {
                Message = "Not Inserted";
            }
            return Json(new { result = Message });
        }
        public JsonResult GetInterestDetails(int id)
        {
            InterestService _interestService = new InterestService();
            InterestMasterDto interestMasterDto = _interestService.GetByIDFedExt(id);
            return Json(new { result = interestMasterDto });
        }
        public ActionResult BindDropDowns(string flag, int Id)
        {
            StringBuilder sbOptions = new StringBuilder();
            if (flag == "Cluster")
            {
                List<SelectListDto> lstvillageDto = _villageService.GetVillageByClusterID(Id);
                foreach (var item in lstvillageDto)
                {
                    sbOptions.Append("<option value=" + item.ID + ">" + item.Text + "</option>");
                }
            }
            else if (flag == "Village")
            {
                List<SelectListDto> lstGroups = _groupService.GetGroupByVillageID(Id);
                foreach (var item in lstGroups)
                {
                    sbOptions.Append("<option value=" + item.ID + ">" + item.Text + "</option>");
                }
            }
            return Content(sbOptions.ToString());

        }

        public ActionResult DepositLookup()
        {
            DepositOBBll DepositOBBll = new DepositOBBll();
            List<DepositOBLookup> lstDepositOB = DepositOBBll.DepositOBLookUpList(false,0);
            return View(lstDepositOB);
        }
    }
}
