using BusinessEntities;
using BusinessLogic;
using AutoMapper;
using BusinessLogic.AutoMapper;
//using BusinessLogic.Interface;
using CoreComponents;
using MFIS.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Utilities;

namespace MFIS.Web.Areas.Group.Controllers.TransactionControllers
{
    public class ContraEntryController : BaseController
    {
        // GET: /Group/ContraEntry/

        #region Global Variables
        private readonly BankService _bankService;
        private readonly AccountHeadService _accountheadService;
        private readonly ContraEntryService _ContraEntryService;
        private readonly EmployeeService _employeeService;

        public ContraEntryController()
        {
            _ContraEntryService = new ContraEntryService();
            _employeeService = new EmployeeService();
            _bankService = new BankService();
            _accountheadService = new AccountHeadService();
        }
        #endregion Global Variables

        //--------------Contra Entry With Drawl--------------------------//

        #region Contra Entry WithDrawl

        #region CreateContraEntryCashWithdrawl

        [HttpGet]
        public ActionResult CreateContraEntryCashWithdrawl(string id)
        {

            long AccountMasterID = string.IsNullOrEmpty(id.DecryptString()) ? default(long) : Convert.ToInt32(id.DecryptString());
            ContraEntryWithDrawlDto contraEntryWithDrawlDto = new ContraEntryWithDrawlDto();
            if (AccountMasterID > 0)
            {
                contraEntryWithDrawlDto = _ContraEntryService.GroupContraEntryWithDrawlGetByAccountMasterId(AccountMasterID);
            }


            int groupId = GroupInfo.GroupID;
            BankMasterViewDto objBank = new BankMasterViewDto();
            List<BankMasterViewDto> lstAllBanks = _ContraEntryService.GetAllGroupBanksByGroupId(groupId);
            SelectList lstgroupBanks = new SelectList(lstAllBanks, "AHID", "AccountNumber", objBank.BankEntryID);
            ViewBag.GroupBanks = lstgroupBanks;

            AccountHeadDto objAccountHead = _accountheadService.GetCashInHandAccount(false);
            int ahId = objAccountHead.AHID;

            AccountHeadDto closeingBalance = _accountheadService.GetAccountHeadViewBalanceSummary(ahId, false,groupId);
            objAccountHead.ClosingBalance = closeingBalance.ClosingBalance;
            ViewBag.CashInHandDetails = objAccountHead;
            int EmployeeID = UserInfo.UserID;
            contraEntryWithDrawlDto.UserID = UserInfo.UserID;
            EmployeeDto obj = _employeeService.GetByID(EmployeeID);

            contraEntryWithDrawlDto.EmployeeName = obj.EmployeeName;
            contraEntryWithDrawlDto.EmployeeCode = obj.EmployeeCode;

            if (contraEntryWithDrawlDto.contraEntryWithDrawlTransactions != null && contraEntryWithDrawlDto.contraEntryWithDrawlTransactions.Count > 0)
            {
                var totalCrAmmount = contraEntryWithDrawlDto.contraEntryWithDrawlTransactions[contraEntryWithDrawlDto.contraEntryWithDrawlTransactions.Count() - 1].CrAmount;
                ViewBag.TotalCrAmmount = totalCrAmmount;
            }
            return View(contraEntryWithDrawlDto);
        }

        #endregion

        #region CreateContraEntryCashWithdrawl

        [HttpPost]
        public ActionResult CreateContraEntryCashWithdrawl(FormCollection form)
        {
            var contraEntryWithDrawlDto = ReadFormDataWithDrawl(form);
            var resultDto = new ResultDto();

            if (contraEntryWithDrawlDto.AccountMasterID == 0)
                resultDto = _ContraEntryService.GroupInsertContraEntryWithDrawl(contraEntryWithDrawlDto);
            else
                resultDto = _ContraEntryService.GroupUpdateContraEntryWithDrawl(contraEntryWithDrawlDto);

            if (resultDto.ObjectId > 0)
            {
                resultDto.ObjectCode = contraEntryWithDrawlDto.VoucherNumber;
            }
            ViewBag.Result = resultDto;

            int groupId = GroupInfo.GroupID;
            BankMasterViewDto objBank = new BankMasterViewDto();
            List<BankMasterViewDto> lstAllBanks = _ContraEntryService.GetAllGroupBanksByGroupId(groupId);
            SelectList lstgroupBanks = new SelectList(lstAllBanks, "AHID", "AccountNumber", objBank.BankEntryID);
            ViewBag.GroupBanks = lstgroupBanks;

            AccountHeadDto objAccountHead = _accountheadService.GetCashInHandAccount(false);
            int ahId = objAccountHead.AHID;
            AccountHeadDto closeingBalance = _accountheadService.GetAccountHeadViewBalanceSummary(ahId, false, groupId);

            objAccountHead.ClosingBalance = closeingBalance.ClosingBalance;
            ViewBag.CashInHandDetails = objAccountHead;

            contraEntryWithDrawlDto = _ContraEntryService.GroupContraEntryWithDrawlGetByAccountMasterId(resultDto.ObjectId);

            var totalCrAmmount = contraEntryWithDrawlDto.contraEntryWithDrawlTransactions.Count() < 1 ? 0 : contraEntryWithDrawlDto.contraEntryWithDrawlTransactions[contraEntryWithDrawlDto.contraEntryWithDrawlTransactions.Count() - 1].CrAmount;
            ViewBag.TotalCrAmmount = totalCrAmmount;

            return View(contraEntryWithDrawlDto);
        }

        #endregion

        #region ReadFormDataWithDrawl

        private ContraEntryWithDrawlDto ReadFormDataWithDrawl(FormCollection form)
        {
            ContraEntryWithDrawlDto contraEntryWithDrawlDto = new ContraEntryWithDrawlDto();

            contraEntryWithDrawlDto.AccountMasterID = Convert.ToInt32(form["hdnAccountMasterID"]);
            contraEntryWithDrawlDto.EmployeeID = UserInfo.UserID;
            DateTime transactionDate = form["TransactionDate"].ConvertToDateTime();
            DateTime chequeDate = form["ChequeDate"].ConvertToDateTime();

            contraEntryWithDrawlDto.TransactionDate = transactionDate;
            contraEntryWithDrawlDto.VoucherNumber = Convert.ToString(form["hdnVoucherNumber"]);
            contraEntryWithDrawlDto.VoucherRefNumber = Convert.ToString(form["VoucherRefNumber"]);
            contraEntryWithDrawlDto.PartyName = Convert.ToString(form["PartyName"]);
            contraEntryWithDrawlDto.TransactionMode = "C";
            contraEntryWithDrawlDto.AHID = Convert.ToInt32(form["hdnCashInHandAHID"]);
            contraEntryWithDrawlDto.EmployeeID = UserInfo.UserID;
            contraEntryWithDrawlDto.TransactionType = Convert.ToInt32(form["TransactionType"]);
            contraEntryWithDrawlDto.Amount = Convert.ToInt32(form["TotalAmmount"]);
            contraEntryWithDrawlDto.ChequeNumber = Convert.ToString(form["ChequeNumber"]);
            contraEntryWithDrawlDto.ChequeDate = chequeDate;
            contraEntryWithDrawlDto.Narration = Convert.ToString(form["Narration"]);
            contraEntryWithDrawlDto.UserID = UserInfo.UserID;
            contraEntryWithDrawlDto.GroupID = GroupInfo.GroupID;

            int maxIndex = Convert.ToInt32(form["hdnMaxRateIndex"]);

            contraEntryWithDrawlDto.contraEntryWithDrawlTransactions = new List<ContraEntryWithDrawlTransactionsDto>();
            ContraEntryWithDrawlTransactionsDto contraEntrywithdrawltransactions = null;
            for (int i = 1; i <= maxIndex; i++)
            {
                contraEntrywithdrawltransactions = new ContraEntryWithDrawlTransactionsDto();
                contraEntrywithdrawltransactions.AHID = Convert.ToInt32(form["hdnAHID_" + i]); ;
                contraEntrywithdrawltransactions.CrAmount = Convert.ToDecimal(form["hdnDrAmount_" + i]);
                contraEntrywithdrawltransactions.DrAmount = Convert.ToDecimal(form["hdnCrAmount_" + i]);
                contraEntryWithDrawlDto.contraEntryWithDrawlTransactions.Add(contraEntrywithdrawltransactions);
            }

            contraEntrywithdrawltransactions = new ContraEntryWithDrawlTransactionsDto();
            contraEntrywithdrawltransactions.AHID = Convert.ToInt32(form["hdnCashInHandAHID"]);           //Convert.ToInt32(form["AHID"]);
            contraEntrywithdrawltransactions.CrAmount = 0;
            contraEntrywithdrawltransactions.DrAmount = contraEntryWithDrawlDto.contraEntryWithDrawlTransactions.Sum(s => s.CrAmount);
            contraEntrywithdrawltransactions.IsMaster = true;
            contraEntryWithDrawlDto.contraEntryWithDrawlTransactions.Add(contraEntrywithdrawltransactions);
            return contraEntryWithDrawlDto;
        }

        #endregion

        #region ContraEntryCashWithDrawlLookup

        public ActionResult ContraEntryCashWithDrawlLookup()
        {
            var lstContraEntryCashWithDrawlLookup = _ContraEntryService.GroupContraEntryWithDrawlLookup(GroupInfo.GroupID, UserInfo.UserID);
            return View(lstContraEntryCashWithDrawlLookup);
        }

        #endregion

        #region DeleteContraEntryWithDrawl

        [HttpGet]
        public ActionResult DeleteContraEntryWithDrawl(string Id)
        {
            int AccountmasterId = DecryptQueryString(Id);

            if (AccountmasterId < 1)
                return RedirectToAction("ContraEntryCashWithdrawlLookup");

            ResultDto resultDto = _ContraEntryService.GroupDeleteContraEntryWithDrawl(AccountmasterId, UserInfo.UserID);

            TempData["Result"] = resultDto;

            return RedirectToAction("ContraEntryCashWithdrawlLookup");
        }

        #endregion

        #region ActiveInactiveContraEntryWithDrawl

        [HttpGet]
        public ActionResult ActiveInactiveContraEntryWithDrawl(string Id)
        {
            int AccountmasterId = DecryptQueryString(Id);

            if (AccountmasterId < 1)
                return RedirectToAction("ContraEntryCashWithDrawlLookup");

            ResultDto resultDto = _ContraEntryService.GroupChangeStatusContraEntryWithDrawl(AccountmasterId, UserInfo.UserID);

            TempData["Result"] = resultDto;

            return RedirectToAction("ContraEntryCashWithDrawlLookup");
        }

        #endregion

        #endregion

        //-------------------Common For Both ----------------------------//

        #region getBankName

        public JsonResult getBankName(int id)
        {
            BankMasterDto bankMasterDto = _ContraEntryService.GetBankNameByAHID(id);
            return Json(new { bankName = bankMasterDto.BName });
        }

        #endregion

        #region GetAccountHeadClosingBalnces

        private ContraEntryWithDrawlTransactionsDto GetAccountHeadClosingBalnces()
        {
            ContraEntryWithDrawlTransactionsDto objcontraEntryWithDrawlTransactionsDto = new ContraEntryWithDrawlTransactionsDto();
            objcontraEntryWithDrawlTransactionsDto = _ContraEntryService.GetAccountHeadClosingBalnces();
            return objcontraEntryWithDrawlTransactionsDto;
        }

        #endregion

        #region ViewBalanceSummary
        public ActionResult ViewBalanceSummary(int ahId, bool isGroup)
        {
            int groupId = GroupInfo.GroupID;
            AccountHeadDto accountHeadDto = _accountheadService.GetAccountHeadViewBalanceSummary(ahId, isGroup, groupId);
            return Json(new { ClosingBalance = accountHeadDto.ClosingBalance, BalanceType = accountHeadDto.BalanceType });
        }
        #endregion ViewBalanceSummary

        //--------------- Contra Entry Deposited--------------------------//


        #region Contra Entry Deposited

        #region CreateContraEntryDeposited

        [HttpGet]
        public ActionResult CreateContraEntryDeposited(string id)
        {
            long AccountMasterID = string.IsNullOrEmpty(id.DecryptString()) ? default(long) : Convert.ToInt32(id.DecryptString());
            ContraEntryDepositedDto contraEntryDepositedDto = new ContraEntryDepositedDto();
            if (AccountMasterID > 0)
            {
                contraEntryDepositedDto = _ContraEntryService.GroupContraEntryDepositedGetByAccountMasterId(AccountMasterID);
            }

            int groupId = GroupInfo.GroupID;
            BankMasterViewDto objBank = new BankMasterViewDto();
            List<BankMasterViewDto> lstAllBanks = _ContraEntryService.GetAllGroupBanksByGroupId(groupId);
            SelectList lstgroupBanks = new SelectList(lstAllBanks, "AHID", "AccountNumber", objBank.BankEntryID);
            ViewBag.GroupBanks = lstgroupBanks;

            AccountHeadDto objAccountHead = _accountheadService.GetCashInHandAccount(false);
            int ahId = objAccountHead.AHID;
            AccountHeadDto closeingBalance = _accountheadService.GetAccountHeadViewBalanceSummary(ahId, false,groupId);

            objAccountHead.ClosingBalance = closeingBalance.ClosingBalance;
            ViewBag.CashInHandDetails = objAccountHead;

            int EmployeeID = UserInfo.UserID;
            contraEntryDepositedDto.UserID = UserInfo.UserID;
            EmployeeDto obj = _employeeService.GetByID(EmployeeID);

            contraEntryDepositedDto.EmployeeName = obj.EmployeeName;
            contraEntryDepositedDto.EmployeeCode = obj.EmployeeCode;

            if (contraEntryDepositedDto.contraEntryDepositedTransactions != null && contraEntryDepositedDto.contraEntryDepositedTransactions.Count > 0)
            {
                var totalCrAmmount = contraEntryDepositedDto.contraEntryDepositedTransactions[contraEntryDepositedDto.contraEntryDepositedTransactions.Count() - 1].CrAmount;
                ViewBag.TotalCrAmmount = totalCrAmmount;
            }

            return View(contraEntryDepositedDto);
        }

        #endregion

        #region CreateContraEntryDeposited

        [HttpPost]
        public ActionResult CreateContraEntryDeposited(FormCollection form)
        {
            var contraEntryDepositedDto = ReadFormDataDeposited(form);
            var resultDto = new ResultDto();

            if (contraEntryDepositedDto.AccountMasterID == 0)
                resultDto = _ContraEntryService.GroupInsertContraEntryDeposited(contraEntryDepositedDto);
            else
                resultDto = _ContraEntryService.GroupUpdateContraEntryDeposited(contraEntryDepositedDto);

            if (resultDto.ObjectId > 0)
            {
                resultDto.ObjectCode = contraEntryDepositedDto.VoucherNumber;
            }
            ViewBag.Result = resultDto;

            int groupId = GroupInfo.GroupID;
            BankMasterViewDto objBank = new BankMasterViewDto();
            List<BankMasterViewDto> lstAllBanks = _ContraEntryService.GetAllGroupBanksByGroupId(groupId);
            SelectList lstgroupBanks = new SelectList(lstAllBanks, "AHID", "AccountNumber", objBank.BankEntryID);
            ViewBag.GroupBanks = lstgroupBanks;

            AccountHeadDto objAccountHead = _accountheadService.GetCashInHandAccount(false);
            int ahId = objAccountHead.AHID;
            AccountHeadDto closeingBalance = _accountheadService.GetAccountHeadViewBalanceSummary(ahId, false,groupId);

            objAccountHead.ClosingBalance = closeingBalance.ClosingBalance;
            ViewBag.CashInHandDetails = objAccountHead;
            contraEntryDepositedDto = _ContraEntryService.GroupContraEntryDepositedGetByAccountMasterId(resultDto.ObjectId);

            var totalCrAmmount = contraEntryDepositedDto.contraEntryDepositedTransactions[contraEntryDepositedDto.contraEntryDepositedTransactions.Count() - 1].CrAmount;
            ViewBag.TotalCrAmmount = totalCrAmmount;

            return View(contraEntryDepositedDto);
        }

        #endregion

        #region ReadFormDataDeposited

        private ContraEntryDepositedDto ReadFormDataDeposited(FormCollection form)
        {
            ContraEntryDepositedDto contraEntryDepositedDto = new ContraEntryDepositedDto();

            contraEntryDepositedDto.AccountMasterID = Convert.ToInt32(form["hdnAccountMasterID"]);
            DateTime transactionDate = form["TransactionDate"].ConvertToDateTime();
            contraEntryDepositedDto.TransactionDate = transactionDate;
            contraEntryDepositedDto.VoucherNumber = Convert.ToString(form["hdnVoucherNumber"]);
            contraEntryDepositedDto.VoucherRefNumber = Convert.ToString(form["VoucherNumber"]);
            contraEntryDepositedDto.PartyName = Convert.ToString(form["PartyName"]);
            contraEntryDepositedDto.TransactionMode = "C";
            contraEntryDepositedDto.AHID = Convert.ToInt32(form["hdnCashInHandAHID"]);
            contraEntryDepositedDto.EmployeeID = UserInfo.UserID; ;
            contraEntryDepositedDto.TransactionType = Convert.ToInt32(form["TransactionType"]);
            contraEntryDepositedDto.Amount = Convert.ToInt32(form["TotalAmmount"]);
            contraEntryDepositedDto.Narration = Convert.ToString(form["Narration"]);
            contraEntryDepositedDto.UserID = UserInfo.UserID;
            contraEntryDepositedDto.GroupID = GroupInfo.GroupID;

            int maxIndex = Convert.ToInt32(form["hdnMaxRateIndex"]);

            contraEntryDepositedDto.contraEntryDepositedTransactions = new List<ContraEntryDepositedTransactionsDto>();
            ContraEntryDepositedTransactionsDto contraEntrydepositedtransactions = null;
            for (int i = 1; i <= maxIndex; i++)
            {
                contraEntrydepositedtransactions = new ContraEntryDepositedTransactionsDto();
                contraEntrydepositedtransactions.AHID = Convert.ToInt32(form["hdnAHID_" + i]); ;
                contraEntrydepositedtransactions.CrAmount = Convert.ToDecimal(form["hdnCrAmount_" + i]);
                contraEntrydepositedtransactions.DrAmount = Convert.ToDecimal(form["hdnDrAmount_" + i]);
                contraEntryDepositedDto.contraEntryDepositedTransactions.Add(contraEntrydepositedtransactions);
            }

            contraEntrydepositedtransactions = new ContraEntryDepositedTransactionsDto();
            contraEntrydepositedtransactions.AHID = Convert.ToInt32(form["hdnCashInHandAHID"]);
            contraEntrydepositedtransactions.DrAmount = 0;
            contraEntrydepositedtransactions.CrAmount = contraEntryDepositedDto.contraEntryDepositedTransactions.Sum(s => s.DrAmount);
            contraEntrydepositedtransactions.IsMaster = true;
            contraEntryDepositedDto.contraEntryDepositedTransactions.Add(contraEntrydepositedtransactions);
            return contraEntryDepositedDto;
        }

        #endregion

        #region ContraEntryDepositedLookup

        public ActionResult ContraEntryDepositedLookup()
        {
            var lstContraEntryCashDepositedLookup = _ContraEntryService.GroupContraEntryDepositedLookup(GroupInfo.GroupID, UserInfo.UserID);
            return View(lstContraEntryCashDepositedLookup);
        }

        #endregion

        #region DeleteContraEntryDeposited

        [HttpGet]
        public ActionResult DeleteContraEntryDeposited(string Id)
        {
            int AccountmasterId = DecryptQueryString(Id);

            if (AccountmasterId < 1)
                return RedirectToAction("ContraEntryDepositedLookup");

            ResultDto resultDto = _ContraEntryService.GroupDeleteContraEntryDeposited(AccountmasterId, UserInfo.UserID);

            TempData["Result"] = resultDto;

            return RedirectToAction("ContraEntryDepositedLookup");
        }

        #endregion

        #region ActiveInactiveContraEntryWithDrawl

        [HttpGet]
        public ActionResult ActiveInactiveContraEntryDeposited(string Id)
        {
            int AccountmasterId = DecryptQueryString(Id);

            if (AccountmasterId < 1)
                return RedirectToAction("ContraEntryDepositedLookup");

            ResultDto resultDto = _ContraEntryService.GroupChangeStatusContraEntryDeposited(AccountmasterId, UserInfo.UserID);

            TempData["Result"] = resultDto;

            return RedirectToAction("ContraEntryDepositedLookup");
        }

        #endregion

        #endregion

    }
}

