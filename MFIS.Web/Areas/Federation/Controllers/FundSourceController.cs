using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using BusinessEntities;
using BusinessLogic;
//using BusinessLogic.Interface;
using MFIS.Web.Areas.Federation.Models;
using MFIS.Web.Controllers;
using CoreComponents;

namespace MFIS.Web.Areas.Federation.Controllers
{
    public class FundSourceController : BaseController
    {
        #region Global Variables
        private readonly FundSourceService _fundSourceService;
        public FundSourceController()
        {
            _fundSourceService = new FundSourceService();
        }
        #endregion Global Variables

        [HttpGet]
        public ActionResult CreateFundSource(string Id)
        {
            int fundSourceId = DecryptQueryString(Id);
            var fundSourceDto = new FundSourceDto();

            var objFundsourceModel = new FundSourceModel();
            if (fundSourceId > 0)
            {

                fundSourceDto = _fundSourceService.GetByID(fundSourceId);

                objFundsourceModel = Mapper.Map<FundSourceDto, FundSourceModel>(fundSourceDto);
            }
            ViewBag.Result = new ResultDto();

            return View(objFundsourceModel);

        }

        [HttpPost]
        public ActionResult CreateFundSource(FundSourceModel objFundSourceModel)
        {
            var resultDto = new ResultDto();
            if (ModelState.IsValid)
            {
                var fundSourceDto = Mapper.Map<FundSourceModel, FundSourceDto>(objFundSourceModel);
                fundSourceDto.UserId = UserInfo.UserID;
                if (fundSourceDto.FundSourceID == 0)
                    resultDto = _fundSourceService.Insert(fundSourceDto);
                else
                    resultDto = _fundSourceService.Update(fundSourceDto);
                if (resultDto.ObjectId > 0)
                {
                    fundSourceDto = _fundSourceService.GetByID(resultDto.ObjectId);
                    objFundSourceModel = Mapper.Map<FundSourceDto, FundSourceModel>(fundSourceDto);
                    resultDto.ObjectCode = fundSourceDto.FundSourceCode;
                }
            }
            ViewBag.Result = resultDto;
            return View(objFundSourceModel);
        }

        [HttpGet]
        public ActionResult FundSourceLookUp()
        {
            var lstfundsourse = _fundSourceService.Lookup();
            return View(lstfundsourse);
        }

        [HttpGet]
        public ActionResult DeleteFundsource(string Id)
        {
            int fundSourceId = DecryptQueryString(Id);

            if (fundSourceId < 1)
                return RedirectToAction("FundSourceLookUp");

            ResultDto resultDto = _fundSourceService.Delete(fundSourceId, UserInfo.UserID);

            TempData["Result"] = resultDto;

            return RedirectToAction("FundSourceLookUp");
        }

        [HttpGet]
        public ActionResult ActiveInactiveFundsource(string Id)
        {
            int fundSourceId = DecryptQueryString(Id);

            if (fundSourceId < 1)
                return RedirectToAction("FundSourceLookUp");

            ResultDto resultDto = _fundSourceService.ChangeStatus(fundSourceId, UserInfo.UserID);

            TempData["Result"] = resultDto;

            return RedirectToAction("FundSourceLookUp");
        }
    }
}
