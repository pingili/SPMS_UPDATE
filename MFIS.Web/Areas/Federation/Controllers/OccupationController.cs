using AutoMapper;
using BusinessEntities;
using BusinessLogic;
using CoreComponents;
using MFIS.Web.Areas.Federation.Models;
using MFIS.Web.Controllers;
using System;
using System.Web.Mvc;
using Utilities;
namespace MFIS.Web.Areas.Federation.Controllers
{
    public class OccupationController : BaseController
    {
        #region Global Variables
        private OccupationService _OccupationService;
        private CommonService _CommonService;
        public OccupationController()
        {
            _OccupationService = new OccupationService();
            _CommonService = new CommonService();
        }
        #endregion Global Variables
        [HttpGet]
        public ActionResult CreateOccupation(string ID)
        {

            //SelectList occupationCategory = GetDropDownListByMasterCode(Enums.RefMasterCodes.OCCUPATION_TYPE);

         

            int occupationId = string.IsNullOrEmpty(ID.DecryptString()) ? default(int) : Convert.ToInt32(ID.DecryptString());

            SelectList occupationCategory = GetDropDownListByMasterCode(Enums.RefMasterCodes.OCCUPATION_TYPE);
            ViewBag.OccupationCategory = occupationCategory;


            OccupationDto occupationDto = new OccupationDto();

            if (occupationId > 0)
                occupationDto = _OccupationService.GetByID(occupationId);
            OccupationModel occupationmodel = Mapper.Map<OccupationDto, OccupationModel>(occupationDto);
            ViewBag.Result = new ResultDto();
            return View(occupationmodel);
            //OccupationModel occupationModel = new OccupationModel();
            //if (ID.HasValue)
            //{
            //    occupationDto = _OccupationService.GetByID(ID.Value);
            //    occupationModel = Mapper.Map<OccupationDto, OccupationModel>(occupationDto);
          //  //}
          //  if (occupationId>0)
          //      occupationDto= _OccupationService.GetByID(occupationId);

            
          //  ViewBag.Result = new ResultDto();

          ////  SelectList occupationCategory = GetDropDownListByMasterCode(Enums.RefMasterCodes.OCCUPATION_TYPE);
          //  ViewBag.OccupationCategory = occupationCategory;
          //  return View(occupationmodel);
        }
        [HttpPost]
        public ActionResult CreateOccupation(OccupationModel ObjOccupation)
        {
            var resultDto = new ResultDto();
            if (ModelState.IsValid)
            {
                var occupationDto = Mapper.Map<OccupationModel, OccupationDto>(ObjOccupation);
                occupationDto.UserId = UserInfo.UserID;
                if (occupationDto.OccupationID == 0)
                   resultDto= _OccupationService.Insert(occupationDto);
                else
                    resultDto = _OccupationService.Update(occupationDto);

                if (resultDto.ObjectId > 0)
                {
                    occupationDto = _OccupationService.GetByID(resultDto.ObjectId);
                    ObjOccupation = Mapper.Map<OccupationDto, OccupationModel>(occupationDto);
                    resultDto.ObjectCode = occupationDto.OccupationCode;
                }

            }

            SelectList occupationCategory = GetDropDownListByMasterCode(Enums.RefMasterCodes.OCCUPATION_TYPE);
            ViewBag.OccupationCategory = occupationCategory;

            ViewBag.Result = resultDto;
            return View(ObjOccupation);
        }
        [HttpGet]
        public ActionResult OccupationLookUp()
        {
            var lstOccupatiins = _OccupationService.Lookup();
            return View(lstOccupatiins);
        }


        [HttpGet]
        public ActionResult DeleteOccupation(string Id)
        {
            int occupationId = DecryptQueryString(Id);

            if (occupationId < 1)
                return RedirectToAction("OccupationLookUp");

            ResultDto resultDto = _OccupationService.Delete(occupationId, UserInfo.UserID);

            TempData["Result"] = resultDto;

            return RedirectToAction("OccupationLookUp");
        }

        [HttpGet]
        public ActionResult ActiveInactiveOccupation(string Id)
        {
            int occupationId = DecryptQueryString(Id);

            if (occupationId < 1)
                return RedirectToAction("OccupationLookUp");

            ResultDto resultDto = _OccupationService.ChangeStatus(occupationId, UserInfo.UserID);

            TempData["Result"] = resultDto;

            return RedirectToAction("OccupationLookUp");
        }

    }
}
