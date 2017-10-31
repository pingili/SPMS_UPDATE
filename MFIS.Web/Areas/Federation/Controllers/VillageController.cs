using AutoMapper;
using BusinessEntities;
using BusinessLogic.AutoMapper;
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

namespace MFIS.Web.Areas.Federation.Controllers
{
    public class VillageController : BaseController
    {
        #region Global Variables
        private readonly StateService _stateService;
        private readonly MandalService _mandalService;
        private readonly DistrictService _districtservice;
        private readonly ClusterService _clusterService;
        private readonly VillageService _villageService;
        public VillageController()
        {
            _villageService = new VillageService();
            _mandalService = new MandalService();
            _stateService = new StateService();
            _districtservice = new DistrictService();
            _clusterService = new ClusterService();
        }

        #endregion Global Variables

        [HttpGet]
        public ActionResult CreateVillage(string Id)
        {
            int villageId = DecryptQueryString(Id);

            VillageDto villageDto = new VillageDto();

            if (villageId > 0)
                villageDto = _villageService.GetByID(villageId);

            VillageModel objVillage = Mapper.Map<VillageDto, VillageModel>(villageDto);

            LoadDropDowns(objVillage.StateID, objVillage.DistrictID, objVillage.MandalID, objVillage.ClusterID);

            return View(objVillage);
        }
        
        [HttpPost]
        public ActionResult CreateVillage(FormCollection Form)
        {
            var resultDto = new ResultDto();

            int villageId = default(int);
            int clusterId = default(int);
            
            int.TryParse(Request.Form["VillageID"], out villageId);
            int.TryParse(Request.Form["ClusterID"], out clusterId);

            VillageModel objVillage = new VillageModel();
            objVillage.VillageID = villageId;
            objVillage.ClusterID = clusterId;
            objVillage.Village = Convert.ToString(Request.Form["txtVillage"]);
            objVillage.TEVillageName = Convert.ToString(Request.Form["TEVillageName"]);
            objVillage.VillageCode = Convert.ToString(Request.Form["VillageCode"]);

            if (ModelState.IsValid)
            {
                var villageDto = Mapper.Map<VillageModel, VillageDto>(objVillage);
                villageDto.UserID = UserInfo.UserID;
                if (villageDto.VillageID > 0)
                    resultDto = _villageService.Update(villageDto);
                else
                    resultDto = _villageService.Insert(villageDto);
                if (resultDto.ObjectId > 0)
                {
                    villageDto = _villageService.GetByID(resultDto.ObjectId);
                    objVillage = AutoMapperEntityConfiguration.Cast<VillageModel>(villageDto);
                    resultDto.ObjectCode = villageDto.VillageCode;
                }
            }

            LoadDropDowns(objVillage.StateID, objVillage.DistrictID, objVillage.MandalID, objVillage.ClusterID);
            ViewBag.Result = resultDto;
            return View(objVillage);
        }

        private void LoadDropDowns(int stateId, int districtId, int mandalId, int clusterId)
        {
            List<SelectListDto> lstselectDto = _stateService.GetStateSelectList();
            SelectList lststates = new SelectList(lstselectDto, "ID", "Text");
            ViewBag.StateNames = lststates;

            if (stateId > 0)
            {
                List<SelectListDto> lstdistricts = _districtservice.GetDistrictsByStateID(stateId);
                SelectList lstdistrict = new SelectList(lstdistricts, "ID", "Text");
                ViewBag.Districts = lstdistrict;

                if (districtId > 0)
                {
                    List<SelectListDto> lstmandalDto = _mandalService.GetMandalByDistrictID(districtId);
                    SelectList lstmandals = new SelectList(lstmandalDto, "ID", "Text");
                    ViewBag.Mandals = lstmandals;

                    if (mandalId > 0)
                    {
                        List<SelectListDto> lstclusterDto = _clusterService.GetClusterByMandalID(mandalId);
                        SelectList lstcluster = new SelectList(lstclusterDto, "ID", "Text");
                        ViewBag.Clusters = lstcluster;
                    }
                }
            }
          
        }

        [HttpGet]
        public ActionResult VillageLookup()
        {
            var villageDto = _villageService.Lookup();
            return View(villageDto);
        }
        public ActionResult BindDropDowns(string flag, int Id)
        {

            StringBuilder sbOptions = new StringBuilder();
            if (flag == "State")
            {
                List<SelectListDto> lstStateDto = _districtservice.GetDistrictsByStateID(Id);
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


            else if (flag == "Mandal")
            {
                List<SelectListDto> lstClusterDto = _clusterService.GetClusterByMandalID(Id);
                foreach (var item in lstClusterDto)
                {
                    sbOptions.Append("<option value=" + item.ID + ">" + item.Text + "</option>");
                }
                List<EmployeeDto> lstemp = new List<EmployeeDto>();


            }
            else if (flag == "Cluster")
            {

                List<SelectListDto> lstvillageDto = _villageService.GetVillageByClusterID(Id);
                foreach (var item in lstvillageDto)

                    sbOptions.Append("<option value=" + item.ID + ">" + item.Text + "</option>");

            }
            return Content(sbOptions.ToString());

        }
        [HttpGet]
        public ActionResult DeleteVillage(string Id)
        {
            int villageId = DecryptQueryString(Id);

            if (villageId < 1)
                return RedirectToAction("VillageLookUp");

            ResultDto resultDto = _villageService.Delete(villageId, UserInfo.UserID);

            TempData["Result"] = resultDto;

            return RedirectToAction("VillageLookUp");
        }

        [HttpGet]
        public ActionResult ActiveInactiveVillage(string Id)
        {
            int villageId = DecryptQueryString(Id);

            if (villageId < 1)
                return RedirectToAction("VillageLookUp");

            ResultDto resultDto = _villageService.ChangeStatus(villageId, UserInfo.UserID);

            TempData["Result"] = resultDto;

            return RedirectToAction("VillageLookUp");
        }
        
        [HttpGet]
        public ActionResult ViewVillage(string id)
        {
            int villageId = DecryptQueryString(id);

            if (villageId <= 0)
                return RedirectToAction("VillageLookUp");

            var villageViewDto = _villageService.GetViewByID(villageId);

            return View(villageViewDto);
        }
    }
}
