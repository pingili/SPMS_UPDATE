using AutoMapper;
using BusinessEntities;
using BusinessLogic.AutoMapper;
using BusinessLogic;
//using BusinessLogic.Interface;
using MFIS.Web.Areas.Federation.Models;
using MFIS.Web.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using CoreComponents;

namespace MFIS.Web.Areas.Federation.Controllers
{
    public class PanchayatController : BaseController
    {
        #region Global Variables

        private readonly PanchayatService _PanchayatService;
        private readonly StateService _stateservice;
        private readonly MandalService _mandalService;
        private readonly ClusterService _clusterService;
        private readonly VillageService _villageService;
        private readonly CommonService _CommonService;
        private readonly DistrictService _DistrictService;



        public PanchayatController()
        {
            _PanchayatService = new PanchayatService();
            _stateservice = new StateService();
            _clusterService = new ClusterService();
            _mandalService = new MandalService();
            _villageService = new VillageService();
            _CommonService = new CommonService();
            _DistrictService = new DistrictService();
        }

        #endregion Global Variables

        [HttpGet]
        public ActionResult CreatePanchayat(string Id)
        {


            int PanchayatId = string.IsNullOrEmpty(Id.DecryptString()) ? default(int) : Convert.ToInt32(Id.DecryptString());
            PanchayatDto objPanchayatDto = new PanchayatDto();
            if (PanchayatId > 0)
                objPanchayatDto = _PanchayatService.GetByID(PanchayatId);


           
            PanchayatModel panchayatModel = Mapper.Map<PanchayatDto, PanchayatModel>(objPanchayatDto);
            LoanDropDowns(panchayatModel.StateID, panchayatModel.DistrictID, panchayatModel.MandalID, panchayatModel.ClusterID, panchayatModel.VillageID);
            ViewBag.Result = new ResultDto();

            return View(panchayatModel);
        }

        [HttpPost]
        public ActionResult CreatePanchayat(FormCollection Form)
        {
            var resultDto = new ResultDto();

            int mandalID = default(int);
            int clusterId = default(int);
            int villageId = default(int);
            int districtId = default(int);
            int stateId = default(int);
            int.TryParse(Request.Form["MandalID"], out mandalID);
            int.TryParse(Request.Form["ClusterID"], out clusterId);
            int.TryParse(Request.Form["VillageID"], out villageId);
            int.TryParse(Request.Form["DistrictID"], out districtId);
            int.TryParse(Request.Form["StateID"], out stateId);
            PanchayatModel panchayatModel = new PanchayatModel();
            panchayatModel.MandalID = mandalID;
            panchayatModel.ClusterID = clusterId;
            panchayatModel.VillageID = villageId;
            panchayatModel.DistrictID = districtId;
            panchayatModel.StateID = stateId;
            panchayatModel.PanchayatID = Convert.ToInt32(Request.Form["PanchayatID"]);
            panchayatModel.PanchayatName = Convert.ToString(Request.Form["PanchayatName"]);
            panchayatModel.TEPanchayatName = Convert.ToString(Request.Form["TEPanchayatName"]);

            if (ModelState.IsValid)
            {
                var panchayatDto = Mapper.Map<PanchayatModel, PanchayatDto>(panchayatModel);
                panchayatDto.UserId = UserInfo.UserID;

                if (panchayatDto.PanchayatID == 0)
                    resultDto = _PanchayatService.Insert(panchayatDto);
                else
                    resultDto = _PanchayatService.Update(panchayatDto);
                if (resultDto.ObjectId > 0)
                {
                    panchayatDto = _PanchayatService.GetByID(resultDto.ObjectId);
                    panchayatModel = Mapper.Map<PanchayatDto, PanchayatModel>(panchayatDto);
                    //objModel = AutoMapperEntityConfiguration.Cast<PanchayatModel>(panchayatDto);
                }
            }
            LoanDropDowns(panchayatModel.StateID, panchayatModel.DistrictID, panchayatModel.MandalID, panchayatModel.ClusterID, panchayatModel.VillageID);

            ViewBag.Result = resultDto;

            return View(panchayatModel);
        }

        public void LoanDropDowns(int stateid,int districtid,int mandalid,int clusterid,int villageid)
        {

            
            List<SelectListDto> stateSelectList = _stateservice.GetStateSelectList();
            SelectList SlstStates = new SelectList(stateSelectList, "ID", "Text");
            ViewBag.states = SlstStates;
            if (stateid > 0)
            {
                List<SelectListDto> districtSelectList = _DistrictService.GetDistrictsByStateID(stateid);
                SelectList lstdistrict = new SelectList(districtSelectList, "ID", "Text");
                ViewBag.district = lstdistrict;

                if (districtid > 0)
                {

                    List<SelectListDto> mandalSelectList = _mandalService.GetMandalByDistrictID(districtid);
                    SelectList lStmandal = new SelectList(mandalSelectList, "ID", "Text");
                    ViewBag.mandal = lStmandal;
                    if (mandalid > 0)
                    {
                        List<SelectListDto> clusterSelectList = _clusterService.GetClusterByMandalID(mandalid);
                        SelectList lStClusters = new SelectList(clusterSelectList, "ID", "Text");
                        ViewBag.clusters = lStClusters;

                        if (clusterid > 0)
                        {
                            List<SelectListDto> villageSelectList = _villageService.GetVillageByClusterID(clusterid);
                            SelectList lstvillage = new SelectList(villageSelectList, "ID", "Text");
                            ViewBag.village = lstvillage;
                        }
                    }
                }
            }
        
        }
        [HttpGet]
        public ActionResult PanchayatLookup()
        {
            var lstPanchayat = _PanchayatService.Lookup();
            return View(lstPanchayat);
        }
        public ActionResult BindDropDowns(string flag, int Id)
        {

            StringBuilder sbOptions = new StringBuilder();
            if (flag == "State")
            {
                List<SelectListDto> lstStateDto = _DistrictService.GetDistrictsByStateID(Id);
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
                {
                    sbOptions.Append("<option value=" + item.ID + ">" + item.Text + "</option>");
                }
            }
            return Content(sbOptions.ToString());

        }
        [HttpGet]
        public ActionResult DeletePanchayat(string Id)
        {
            int panchayatId = DecryptQueryString(Id);

            if (panchayatId < 1)
                return RedirectToAction("PanchayatLookUp");

            ResultDto resultDto = _PanchayatService.Delete(panchayatId, UserInfo.UserID);

            TempData["Result"] = resultDto;

            return RedirectToAction("PanchayatLookUp");
        }

        [HttpGet]
        public ActionResult ActiveInactivePanchayat(string Id)
        {
            int panchayatId = DecryptQueryString(Id);

            if (panchayatId < 1)
                return RedirectToAction("PanchayatLookUp");

            ResultDto resultDto = _PanchayatService.ChangeStatus(panchayatId, UserInfo.UserID);

            TempData["Result"] = resultDto;

            return RedirectToAction("PanchayatLookUp");
        }

        [HttpGet]
        public ActionResult ViewPanchayat(string id)
        {
            int panchayatId = DecryptQueryString(id);

            if (panchayatId <= 0)
                return RedirectToAction("PanchayatLookUp");

            var panchayatViewDto = _PanchayatService.GetViewByID(panchayatId);

            return View(panchayatViewDto);
        }
       


    }
}




