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
using Utilities;
using CoreComponents;

namespace MFIS.Web.Areas.Federation.Controllers
{
    public class LoanSecurityMasterController : BaseController
    {
        #region Global Variables
        private LoanSecurityMasterService _loansecurityMasterService;
        public LoanSecurityMasterController()
        {
            _loansecurityMasterService = new LoanSecurityMasterService();
        }

        #endregion Global Variables
        [HttpGet]
        public ActionResult CreateLoanSecurityMaster(string id)
        {
            int loanSecurityMasteId = string.IsNullOrEmpty(id.DecryptString()) ? default(int) : Convert.ToInt32(id.DecryptString());
            SelectList loansecuritymaster = GetDropDownListByMasterCode(Enums.RefMasterCodes.LOAN_SECURITY_TYPE);
            ViewBag.LoanSecurityMaster = loansecuritymaster;

            LoanSecurityMasterDto loanSecurityMasterDto = new LoanSecurityMasterDto();
            if (loanSecurityMasteId > 0)
                loanSecurityMasterDto = _loansecurityMasterService.GetByID(loanSecurityMasteId);

            LoanSecurityMasterModel objLoanSecurityMasterModel = Mapper.Map<LoanSecurityMasterDto, LoanSecurityMasterModel>(loanSecurityMasterDto);

            return View(objLoanSecurityMasterModel);

        }
        [HttpPost]
        public ActionResult CreateLoanSecurityMaster(LoanSecurityMasterModel objLoanSecurityMasterModel)
        {
            var resultDto = new ResultDto();
            if (ModelState.IsValid)
            {
                var loansecurityMasterDto = Mapper.Map<LoanSecurityMasterModel, LoanSecurityMasterDto>(objLoanSecurityMasterModel);
                loansecurityMasterDto.UserID = UserInfo.UserID;
                if (loansecurityMasterDto.LoanSecurityID > 0)
                    resultDto = _loansecurityMasterService.Update(loansecurityMasterDto);
                else
                    resultDto = _loansecurityMasterService.Insert(loansecurityMasterDto);
                if (resultDto.ObjectId > 0)
                {
                    loansecurityMasterDto = _loansecurityMasterService.GetByID(resultDto.ObjectId);
                    objLoanSecurityMasterModel = AutoMapperEntityConfiguration.Cast<LoanSecurityMasterModel>(loansecurityMasterDto);
                    resultDto.ObjectCode = objLoanSecurityMasterModel.LoanSecurityCode;
                }
            }
            SelectList loansecuritymaster = GetDropDownListByMasterCode(Enums.RefMasterCodes.LOAN_SECURITY_TYPE);
            ViewBag.LoanSecurityMaster = loansecuritymaster;

            ViewBag.Result = resultDto;

            return View(objLoanSecurityMasterModel);
        }

        [HttpGet]
        public ActionResult LoanSecurityMasterLookup()
        {
            var loansecurityMasterDto = _loansecurityMasterService.Lookup();
            return View(loansecurityMasterDto);
        }
        [HttpGet]
        public ActionResult DeleteLoanSecurity(string Id)
        {
            int loanSecurityId = DecryptQueryString(Id);

            if (loanSecurityId < 1)
                return RedirectToAction("LoanSecurityMasterLookUp");

            ResultDto resultDto = _loansecurityMasterService.Delete(loanSecurityId, UserInfo.UserID);

            TempData["Result"] = resultDto;

            return RedirectToAction("LoanSecurityMasterLookUp");
        }

        [HttpGet]
        public ActionResult ActiveInactiveLoanSecurityMaster(string Id)
        {
            int loanSecurityId = DecryptQueryString(Id);

            if (loanSecurityId < 1)
                return RedirectToAction("LoanSecurityMasterLookUp");

            ResultDto resultDto =  _loansecurityMasterService.ChangeStatus(loanSecurityId, UserInfo.UserID);

            TempData["Result"] = resultDto;

            return RedirectToAction("LoanSecurityMasterLookUp");
        }

   
    }
}
