using AutoMapper;
using BusinessEntities;
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

namespace MFIS.Web.Areas.Federation.Controllers
{
    public class BranchController : BaseController
    {

        #region Global Variables
        private readonly BranchService _branchService;
        private readonly EmployeeService _employeeService;
        public BranchController()
        {
            _branchService = new BranchService();
            _employeeService = new EmployeeService();
        }
        #endregion Global Variables

        #region CreateBranch
        [HttpGet]
        public ActionResult CreateBranch(string id)
        {
            int BranchId = string.IsNullOrEmpty(id.DecryptString()) ? default(int) : Convert.ToInt32(id.DecryptString());
            var branchDto = new BranchDto();
            var branchModel = new BranchModel();

            if (BranchId>0)
            {
                branchDto = _branchService.GetByID(BranchId);
                branchModel = Mapper.Map<BranchDto, BranchModel>(branchDto);
            }
            
            //List<SelectListDto> employeeSelectListDto = _employeeService.GetEmployeeSelectList();
            //SelectList employeeSelectList = new SelectList(employeeSelectListDto, "ID", "Text", "Select Employee");
            //ViewBag.employeeSelectList = employeeSelectList;

            ViewBag.Result = new ResultDto();
            return View(branchModel);
        }
        #endregion

        #region ViewBranch
        [HttpGet]
        public ActionResult ViewBranch(string id)
        {
            int branchId = DecryptQueryString(id);

            if (branchId <= 0)
                return RedirectToAction("BranchLookup");

            var branchViewDto = _branchService.GetViewByID(branchId);
            
            return View(branchViewDto);
        }
        #endregion

        #region CreateBranch
        [HttpPost]
        public ActionResult CreateBranch(BranchModel branchModel)
        {
            var resultDto = new ResultDto();
           
                var branchDto = Mapper.Map<BranchModel, BranchDto>(branchModel);
                branchDto.UserID = UserInfo.UserID;

                if (branchDto.BranchID == 0)
                    resultDto = _branchService.Insert(branchDto);
                else
                    resultDto = _branchService.Update(branchDto);

                if (resultDto.ObjectId > 0)
                {
                    branchDto = _branchService.GetByID(resultDto.ObjectId);
                    branchModel = Mapper.Map<BranchDto, BranchModel>(branchDto);
                    resultDto.ObjectCode = branchDto.BranchCode;
                }
            
            //List<SelectListDto> employeeSelectListDto = _employeeService.GetEmployeeSelectList();
            //SelectList employeeSelectList = new SelectList(employeeSelectListDto, "ID", "Text", "Select Employee");
            //ViewBag.employeeSelectList = employeeSelectList;

            ViewBag.Result = resultDto;
            return View(branchModel);
        }
        #endregion

        #region BranchLookup
        [HttpGet]
        public ActionResult BranchLookup()
        {
            var lstBranchDto = _branchService.GetLookup();
            return View("BranchLookup", lstBranchDto);
        }
        #endregion

        #region DeleteBranch
        public ActionResult DeleteBranch(string Id)
        {
            int branchId = DecryptQueryString(Id);

            if (branchId < 1)
                return RedirectToAction("BranchLookUp");

            ResultDto resultDto = _branchService.Delete(branchId, UserInfo.UserID);

            TempData["Result"] = resultDto;

            return RedirectToAction("BranchLookUp");
        }
        #endregion

        #region ActiveInactiveBranch
        [HttpGet]
        public ActionResult ActiveInactiveBranch(string Id)
        {
            int branchId = DecryptQueryString(Id);

            if (branchId < 1)
                return RedirectToAction("BranchLookUp");

            ResultDto resultDto = _branchService.ChangeStatus(branchId, UserInfo.UserID);

            TempData["Result"] = resultDto;

            return RedirectToAction("BranchLookUp");
        }
        #endregion

    }
}
