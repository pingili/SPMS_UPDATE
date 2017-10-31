using AutoMapper;
using BusinessEntities;
using BusinessLogic;
//using BusinessLogic.Interface;
using MFIS.Web.Areas.Federation.Models;
using MFIS.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using CoreComponents;
namespace MFIS.Web.Areas.Federation.Controllers
{
    public class ClusterController : BaseController
    {
        #region Global Variables
        private readonly MandalService _mandalService;
        private readonly StateService _stateService;
        private readonly DistrictService _districtService;
        private readonly BranchService _branchService;
        private readonly EmployeeService _employeeService;
        private readonly ClusterService _clusterService;

        public ClusterController()
        {
            _mandalService = new MandalService();
            _stateService = new StateService();
            _districtService = new DistrictService();
            _branchService = new BranchService();
            _employeeService = new EmployeeService();
            _clusterService = new ClusterService();
        }

        #endregion Global Variables

        [HttpGet]
        public ActionResult CreateCluster(string id)
        {
            int ClusterId = string.IsNullOrEmpty(id.DecryptString()) ? default(int) : Convert.ToInt32(id.DecryptString());
            var clusterDto = new ClusterDto();
            var clusterModel = new ClusterModel();

            LoadDropDowns();

            if (ClusterId>0)
            {
                clusterDto = _clusterService.GetByID(ClusterId);
                clusterModel = Mapper.Map<ClusterDto, ClusterModel>(clusterDto);
            }

            ViewBag.Result = new ResultDto();
            return View(clusterModel);
        }
        [HttpPost]
        public ActionResult CreateCluster(ClusterModel objCluster)
        {
            var resultDto = new ResultDto();
            //if (ModelState.IsValid)
            //{
                var clusterDto = Mapper.Map<ClusterModel, ClusterDto>(objCluster);
                clusterDto.UserID = UserInfo.UserID;
                if (clusterDto.ClusterID == 0)
                    resultDto = _clusterService.Insert(clusterDto);
                else
                    resultDto = _clusterService.Update(clusterDto);
                if (resultDto.ObjectId > 0)
                {
                    clusterDto = _clusterService.GetByID(resultDto.ObjectId);
                    objCluster = Mapper.Map<ClusterDto, ClusterModel>(clusterDto);
                    resultDto.ObjectCode = clusterDto.ClusterCode;
                }
            //}

            LoadDropDowns();

            ViewBag.Result = resultDto;
            return View(objCluster);
        }

        private void LoadDropDowns()
        {
            var stateSelectListDto = _stateService.GetStateSelectList();
            var stateSelectList = new SelectList(stateSelectListDto, "ID", "Text",0);
            ViewBag.States = stateSelectList;

            var districtSelectListDto = _districtService.GetDistrictSelectList();
            var districtSelectList = new SelectList(districtSelectListDto, "ID", "Text",0);
            ViewBag.Districts = districtSelectList;

            var mandalSelectListDto = _mandalService.GetMandalSelectList();
            var mandalSelectList = new SelectList(mandalSelectListDto, "ID", "Text",0);
            ViewBag.Mandals = mandalSelectList;

            var branchSelectListDto = _branchService.GetBranchSelectList();
            var branchSelectList = new SelectList(branchSelectListDto, "ID", "Text");
            ViewBag.Branches = branchSelectList;

            var employeeSelectListDto = _employeeService.GetEmployeeSelectList();
            var employeeSelectList = new SelectList(employeeSelectListDto, "ID", "Text");
            ViewBag.Leaders = employeeSelectList;
        }

        [HttpGet]
        public ActionResult ClusterLookUp()
        {
            var lstClusterLookupDto = _clusterService.Lookup();
            return View("ClusterLookUp", lstClusterLookupDto);
        }

        //[HttpGet]
        //public ActionResult DeleteMandal(int id)
        //{
        //    //ToDo: Need to implement
        //    return RedirectToAction("MandalLookup");
        //}
        public ActionResult BindDropDowns(string flag, int Id)
        {

            StringBuilder sbOptions = new StringBuilder();
            if (flag == "State")
            {
                List<SelectListDto> lstStateDto = _districtService.GetDistrictsByStateID(Id);
                foreach (var item in lstStateDto)
                {
                    sbOptions.Append("<option value=" + item.ID + ">" + item.Text + "</option>");
                }
            }
            else if (flag == "District")
            {

                List<SelectListDto> lstMandalDto = _mandalService.GetMandalByDistrictID(Id);
                foreach (var item in lstMandalDto)

                    sbOptions.Append("<option value=" + item.ID + ">" + item.Text + "</option>");
            }
            return Content(sbOptions.ToString());

        }

        [HttpGet]
        public ActionResult DeleteCluster(string Id)
        {
            int clusterId = DecryptQueryString(Id);

            if (clusterId < 1)
                return RedirectToAction("ClusterLookUp");

            ResultDto resultDto = _clusterService.Delete(clusterId, UserInfo.UserID);

            TempData["Result"] = resultDto;

            return RedirectToAction("ClusterLookUp");
        }

        [HttpGet]
        public ActionResult ActiveInactiveCluster(string Id)
        {
            int clusterId = DecryptQueryString(Id);

            if (clusterId < 1)
                return RedirectToAction("ClusterLookUp");

            ResultDto resultDto = _clusterService.ChangeStatus(clusterId, UserInfo.UserID);

            TempData["Result"] = resultDto;

            return RedirectToAction("ClusterLookUp");
        }
        [HttpGet]
        public ActionResult ViewCluster(string id)
        {
           // int clusterId = string.IsNullOrEmpty(id.DecryptString()) ? default(int) : Convert.ToInt32(id.DecryptString());

            LoadDropDowns();

            int clusterId = DecryptQueryString(id);

            if (clusterId <= 0)
                return RedirectToAction("ClusterLookup");

            var clusterViewDto = _clusterService.GetViewByID(clusterId);

            return View(clusterViewDto);
        }

    }
}