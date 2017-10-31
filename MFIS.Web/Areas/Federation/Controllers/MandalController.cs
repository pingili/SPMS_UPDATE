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
using Utilities;
using CoreComponents;

namespace MFIS.Web.Areas.Federation.Controllers
{
    public class MandalController : BaseController
    {
        #region Global Variables
        private readonly MandalService _mandalService;
        private readonly StateService _stateService;
        private readonly DistrictService _districtService;
        public MandalController()
        {
            _mandalService = new MandalService();
            _stateService = new StateService();
            _districtService = new DistrictService();
        }
        #endregion Global Variables

        [HttpGet]
        public ActionResult CreateMandal(string id)
        {
            //int mandalId = DecryptQueryString(id);
            int mandalId = string.IsNullOrEmpty(id.DecryptString()) ? default(int) : Convert.ToInt32(id.DecryptString());

            var mandalDto = new MandalDto();
            var mandalModel = new MandalModel();

            var stateSelectListDto = _stateService.GetStateSelectList();
            var stateSelectList = new SelectList(stateSelectListDto, "ID", "Text");
            ViewBag.States = stateSelectList;

            var districtSelectListDto = _districtService.GetDistrictSelectList();
            var districtSelectList = new SelectList(districtSelectListDto, "ID", "Text");
            ViewBag.Districts = districtSelectList;

            if (mandalId > 0)
            {
                mandalDto = _mandalService.GetByID(mandalId);
                mandalModel = Mapper.Map<MandalDto, MandalModel>(mandalDto);
            }

            ViewBag.Result = new ResultDto();
            return View(mandalModel);
        }

        [HttpPost]
        public ActionResult CreateMandal(MandalModel objMandal)
        {
            var resultDto = new ResultDto();
            if (ModelState.IsValid)
            {
                var mandalDto = Mapper.Map<MandalModel, MandalDto>(objMandal);
                mandalDto.UserID = UserInfo.UserID;
                if (mandalDto.MandalID == 0)
                    resultDto = _mandalService.Insert(mandalDto);
                else
                    resultDto = _mandalService.Update(mandalDto);
                if (resultDto.ObjectId > 0)
                {
                    mandalDto = _mandalService.GetByID(resultDto.ObjectId);
                    objMandal = Mapper.Map<MandalDto, MandalModel>(mandalDto);
                    resultDto.ObjectCode = mandalDto.MandalCode;
                }
            }
            var stateSelectListDto = _stateService.GetStateSelectList();
            var stateSelectList = new SelectList(stateSelectListDto, "ID", "Text");
            ViewBag.States = stateSelectList;

            var districtSelectListDto = _districtService.GetDistrictSelectList();
            var districtSelectList = new SelectList(districtSelectListDto, "ID", "Text");
            ViewBag.Districts = districtSelectList;

            ViewBag.Result = resultDto;

            return View(objMandal);
        }

        [HttpGet]
        public ActionResult MandalLookup()
        {
            var lstMandals = _mandalService.Lookup();
            return View(lstMandals);
        }

        [HttpGet]
        public ActionResult DeleteMandal(string id)
        {
            int mandalId = DecryptQueryString(id);

            if (mandalId < 1)
                return RedirectToAction("MandalLookup");

            ResultDto resultDto = _mandalService.Delete(mandalId, UserInfo.UserID);

            TempData["Result"] = resultDto;

            return RedirectToAction("MandalLookup");

        }
        [HttpGet]
        public ActionResult ActiveInactiveMandal(string id)
        {
            int mandalId = DecryptQueryString(id);
            if (mandalId < 1)
                return RedirectToAction("MandalLookup");
            ResultDto resultdto = _mandalService.ChangeStatus(mandalId, UserInfo.UserID);
            TempData["Result"] = resultdto;
            return RedirectToAction("MandalLookup");

        }
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
            return Content(sbOptions.ToString());
        }
    }
}
