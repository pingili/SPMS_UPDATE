using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MFIS.Web.Areas.Federation.Models;
using MFIS.Web.Controllers;
//using BusinessLogic.Interface;
using BusinessLogic;
using BusinessEntities;
using AutoMapper;
using System.Dynamic;
using CoreComponents;

namespace MFIS.Web.Areas.Federation.Controllers
{
    public class DistrictController : BaseController
    {
        log4net.ILog _log = log4net.LogManager.GetLogger(typeof(DistrictController));  //Declaring Log4Net  

        #region Global Variables

        private DistrictService _districtService;
        private StateService _stateservice;

        public DistrictController()
        {
            _districtService = new DistrictService();
            _stateservice = new StateService();
        }

        #endregion Global Variables


        [HttpGet]
        public ActionResult CreateDistrict(string Id)
        {
            int districtId = string.IsNullOrEmpty(Id.DecryptString()) ? default(int) : Convert.ToInt32(Id.DecryptString());
            var districtDto = new DistrictDto();
            var districtModel = new DistrictModel();
            var stateSelectListDto = _stateservice.GetStateSelectList();
            var stateSelectList = new SelectList(stateSelectListDto, "ID", "Text");
            ViewBag.states = stateSelectList;

            if (districtId>0)
            {
                districtDto = _districtService.GetByID(districtId);
                districtModel = Mapper.Map<DistrictDto, DistrictModel>(districtDto);
            }

            ViewBag.Result = new ResultDto();
            return View(districtModel);
        }

        [HttpPost]
        public ActionResult CreateDistrict(DistrictModel objDistrict)
        {
            var resultDto = new ResultDto();
            if (ModelState.IsValid)
            {
                var districtDto = Mapper.Map<DistrictModel, DistrictDto>(objDistrict);
                districtDto.UserId = UserInfo.UserID;
                if (districtDto.DistrictID == 0)
                    resultDto = _districtService.Insert(districtDto);
                else
                    resultDto = _districtService.Update(districtDto);
                if (resultDto.ObjectId > 0)
                {
                    districtDto = _districtService.GetByID(resultDto.ObjectId);
                    objDistrict = Mapper.Map<DistrictDto, DistrictModel>(districtDto);
                    resultDto.ObjectCode = districtDto.DistrictCode;
                }
            }
            var stateSelectListDto = _stateservice.GetStateSelectList();
            var stateSelectList = new SelectList(stateSelectListDto, "ID", "Text");
            ViewBag.states = stateSelectList;

            ViewBag.Result = resultDto;
            return View(objDistrict);
        }
        [HttpGet]
        public ActionResult DistrictLookup()
        {
            var lstDistrics = _districtService.Lookup();
            return View(lstDistrics);
        }
        //[HttpGet]
        //public ActionResult DeleteDistrict(int id)
        //{
        //    //bool IsSuccess = _districtService.DeleteDistrictByID(id);

        //    return RedirectToAction("DistrictLookup");
        //}
        [HttpGet]
        public ActionResult DeleteDistrict(string Id)
        {
            int DistrictId = DecryptQueryString(Id);

            if (DistrictId < 1)
                return RedirectToAction("DistrictLookup");

            ResultDto resultDto = _districtService.Delete(DistrictId, UserInfo.UserID);

            TempData["Result"] = resultDto;

            return RedirectToAction("DistrictLookup");
        }

        [HttpGet]
        public ActionResult ActiveInactiveDistrict(string Id)
        {
            int DistrictId = DecryptQueryString(Id);

            if (DistrictId < 1)
                return RedirectToAction("DistrictLookup");

            ResultDto resultDto = _districtService.ChangeStatus(DistrictId, UserInfo.UserID);

            TempData["Result"] = resultDto;

            return RedirectToAction("DistrictLookup");
        }

    }
}
