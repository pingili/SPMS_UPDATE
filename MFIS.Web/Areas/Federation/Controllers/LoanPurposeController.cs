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
    public class LoanPurposeController : BaseController
    {
        #region Global Variables

        private LoanPurposeService _loanpurposeService;
        private ProjectService _projectService;

        public LoanPurposeController()
        {
            _loanpurposeService = new LoanPurposeService();
            _projectService = new ProjectService();
        }

        #endregion Global Variables

        [HttpGet]
        public ActionResult CreateLoanPurpose(string id)
        {
            int loanPurposeId = string.IsNullOrEmpty(id.DecryptString()) ? default(int) : Convert.ToInt32(id.DecryptString());
            
            //List<SelectListDto> loanpurpose = _projectService.GetProjectSelectList();
            //SelectList lst = new SelectList(loanpurpose,"ID","Text");
            //ViewBag.LoanPurpose = lst;

            List<SelectListDto> Projects = _projectService.GetProjectSelectList();
            SelectList lst = new SelectList(Projects, "ID", "Text");
            ViewBag.Projects = lst;


            LoanPurposeDto loanPurposeDto = new LoanPurposeDto();
            if (loanPurposeId>0)
                loanPurposeDto = _loanpurposeService.GetByID(loanPurposeId);

            LoanPurposeModel objLoanPurposeModel = Mapper.Map<LoanPurposeDto, LoanPurposeModel>(loanPurposeDto);

            return View(objLoanPurposeModel);
        }

        [HttpPost]
        public ActionResult CreateLoanPurpose(LoanPurposeModel objLoanPurpose)
        {
            var resultDto = new ResultDto();
            if (ModelState.IsValid)
            {
                var loanPurposeDto = Mapper.Map<LoanPurposeModel, LoanPurposeDto>(objLoanPurpose);
                
                loanPurposeDto.UserID = UserInfo.UserID;
                if (loanPurposeDto.LoanPurposeID == 0)
                    resultDto = _loanpurposeService.Insert(loanPurposeDto);
                else
                    resultDto = _loanpurposeService.Update(loanPurposeDto);
                if (resultDto.ObjectId > 0)
                {
                    loanPurposeDto = _loanpurposeService.GetByID(resultDto.ObjectId);
                    objLoanPurpose = AutoMapperEntityConfiguration.Cast<LoanPurposeModel>(loanPurposeDto);
                    resultDto.ObjectCode = loanPurposeDto.LoanPurposeCode;
                }
            }

            //SelectList loanpurpose = GetDropDownListByMasterCode(Enums.RefMasterCodes.LOAN_PURPOSE_TYPE);
            //ViewBag.LoanPurpose = loanpurpose;

            List<SelectListDto> Projects = _projectService.GetProjectSelectList();
            SelectList lst = new SelectList(Projects, "ID", "Text");
            ViewBag.Projects = lst;


            ViewBag.Result = resultDto;

            return View(objLoanPurpose);
        }

        [HttpGet]
        public ActionResult LoanPurposeLookup()
        {
            var lstLoanpurposeDto = _loanpurposeService.Lookup();
            return View(lstLoanpurposeDto);
        }
        [HttpGet]
        public ActionResult DeleteLoanPurpose(string Id)
        {
            int loanPurposeId = DecryptQueryString(Id);

            if (loanPurposeId < 1)
                return RedirectToAction("LoanPurposeLookUp");

            ResultDto resultDto = _loanpurposeService.Delete(loanPurposeId, UserInfo.UserID);

            TempData["Result"] = resultDto;

            return RedirectToAction("LoanPurposeLookUp");
        }

        [HttpGet]
        public ActionResult ActiveInactiveLoanPurpose(string Id)
        {
            int loanpurposeId = DecryptQueryString(Id);

            if (loanpurposeId < 1)
                return RedirectToAction("LoanPurposeLookUp");

            ResultDto resultDto = _loanpurposeService.ChangeStatus(loanpurposeId, UserInfo.UserID);

            TempData["Result"] = resultDto;

            return RedirectToAction("LoanPurposeLookUp");
        }

    }
}