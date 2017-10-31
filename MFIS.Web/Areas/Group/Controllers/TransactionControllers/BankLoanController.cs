using AutoMapper;
using BusinessEntities;
using BusinessLogic;
//using BusinessLogic.Interface;
using MFIS.Web.Areas.Group.Models;
using MFIS.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CoreComponents;
using System.Text;
using DataLogic;
using Utilities;
using BusinessLogic.Implementation;

namespace MFIS.Web.Areas.Group.Controllers.TransactionControllers
{
    public class BankLoanController : BaseController
    {
        #region Global Variables

        private readonly BankLoanService _bankLoanService;
        private readonly MasterService _masterService;
        public BankLoanController()
        {
            _bankLoanService = new BankLoanService();
            _masterService = new MasterService();
        }
        #endregion Global Variables

        [HttpGet]
        public ActionResult CreateBankLoan(string id)
        {
            int loanmasterId = string.IsNullOrEmpty(id.DecryptString()) ? default(int) : Convert.ToInt32(id.DecryptString());
            int GroupId = GroupInfo.GroupID;
            LoadDropdowns();
            bool isView = false;
            if (Request.QueryString["isView"] != null)
                isView = Request.QueryString["isView"] == "1";

            ViewBag.isView = isView;

            BankLoanDto bankLoanDto = new BankLoanDto();
            if (loanmasterId > 0)
                bankLoanDto = _bankLoanService.GetBankLoanApplicationDetailsById(loanmasterId);
            bankLoanDto.GroupId = GroupId;
            return View(bankLoanDto);
        }

        [HttpPost]
        public ActionResult CreateBankLoan(BankLoanDto objLoan)
        {
            var resultDto = new ResultDto();
            int GroupId = GroupInfo.GroupID;
            objLoan.GroupId = GroupId;
            resultDto = _bankLoanService.InsertUpdateBankLoanApplication(objLoan);
            objLoan.LoanCode = resultDto.ObjectCode;
            objLoan.BankLoanId = resultDto.ObjectId;
            if (objLoan.BankLoanId > 0)
            {
                //_bankLoanService.SubmitLoanApplicationApproval((BankLoanDto)objLoan, UserInfo.UserID, objLoan.LoanMasterId, true);
                objLoan = _bankLoanService.GetBankLoanApplicationDetailsById(objLoan.BankLoanId);
            }
            TempData["Result"] = resultDto;
            LoadDropdowns();
            ViewBag.isView = false;
            return View(objLoan);
        }

        [HttpPost]
        public ActionResult ConfirmBankLoanApplication(BankLoanDto objLoan)
        {
            int count = _bankLoanService.SubmitLoanApplicationApproval((BankLoanDto)objLoan, UserInfo.UserID, objLoan.LoanMasterId);
            return RedirectToAction("MemberLoanApplicationLookup");
        }

        public ActionResult checkLoanOutstnding(int groupID, int SLAHID)
        {
            string linkageNumber = "";
            bool IsExisted = _bankLoanService.CheckLoanExisted(groupID, SLAHID, linkageNumber);
            return Json(new { IsExisted = IsExisted, LinkageNumber = linkageNumber });
        }

        private void LoadDropdowns()
        {

            int GroupId = GroupInfo.GroupID;

            TypeQueryResult lstBankAh = _masterService.GetTypeQueryResult("GROUP_OR_BANK_AH", GroupInfo.GroupID.ToString());
            ViewBag.slBankAh = new SelectList(lstBankAh.OrderBy(a => a.Name), "Id", "Name");

            TypeQueryResult lstGLAccounts = _masterService.GetTypeQueryResult("GROUP_BANK_LOAN_GL_AH");
            ViewBag.slGLAHS = new SelectList(lstGLAccounts.OrderBy(a => a.Name), "Id", "Name");

            TypeQueryResult lstSLAccounts = _masterService.GetTypeQueryResult("FED_LOAN_SL_HEADS", lstGLAccounts[0].Id.ToString());
            ViewBag.slSLAHS = new SelectList(lstSLAccounts.OrderBy(a => a.Name), "Id", "Name");


        }

        [HttpGet]
        public ActionResult BankLoanApplicationLookup()
        {
            int GroupID = GroupInfo.GroupID;
            List<BankLoanLookupDto> lstbankloanapplicationDto = new List<BankLoanLookupDto>();
            // lstbankloanapplicationDto = _bankLoanService.Lookup(GroupID, UserInfo.UserID);
            return View(lstbankloanapplicationDto);
        }

        [HttpGet]
        public ActionResult DeleteMemberLoanApplication(string Id)
        {
            int loanmasterId = DecryptQueryString(Id);

            if (loanmasterId < 1)
                return RedirectToAction("MemberLoanApplicationLookup");

            ResultDto resultDto = _bankLoanService.Delete(loanmasterId, UserInfo.UserID);

            TempData["Result"] = resultDto;

            return RedirectToAction("MemberLoanApplicationLookup");
        }

        [HttpGet]
        public ActionResult ActiveInactiveMemberLoanApplication(string Id)
        {
            int loanmasterId = DecryptQueryString(Id);

            if (loanmasterId < 1)
                return RedirectToAction("BankLoanApplicationLookup");

            ResultDto resultDto = _bankLoanService.ChangeStatus(loanmasterId, UserInfo.UserID);

            TempData["Result"] = resultDto;

            return RedirectToAction("MemberLoanApplicationLookup");
        }


        public ActionResult CheckLoanExisted(int InterestID, int MemberID)
        {

            bool IsChecked = false;
            MemberLoanDisbursementDataAccess dl = new MemberLoanDisbursementDataAccess();
            IsChecked = dl.CheckLoanExisted(MemberID, InterestID);
            return Json(IsChecked);

        }

        [HttpGet]
        public ActionResult BankApplicationView(string id, string type)
        {
            int loanMasterId = string.IsNullOrEmpty(id.DecryptString()) ? default(int) : Convert.ToInt32(id.DecryptString());
            MemberLoanApplicationViewDto objLoanViewDto = _bankLoanService.GetMemberLoanApplicationViewDetails(loanMasterId);
            ViewBag.isViewPage = type != "A";
            return View(objLoanViewDto);
        }

    }
}