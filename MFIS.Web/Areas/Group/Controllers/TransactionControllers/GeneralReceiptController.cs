using BusinessEntities;
using BusinessLogic;
//using BusinessLogic.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CoreComponents;
using MFIS.Web.Controllers;
using System.Text;
using BusinessLogic.Implementation;
namespace MFIS.Web.Areas.Group.Controllers.TransactionControllers
{
    public class GeneralReceiptController : BaseController
    {
        #region Global Variables
        private readonly AccountHeadService _accountheadService;
        private readonly EmployeeService _employeeService;
        private readonly GroupGeneralReceiptService _generalReceiptService;
        private readonly GroupReceiptService _groupReceiptService;
        private readonly BankService _bankService;
        public GeneralReceiptController()
        {
            _accountheadService = new AccountHeadService();
            _employeeService = new EmployeeService();
            _generalReceiptService = new GroupGeneralReceiptService();
            _groupReceiptService = new GroupReceiptService();
            _bankService = new BankService();
        }
        #endregion Global Variables
        [HttpGet]
        public ActionResult CreateGeneralReceipt(string Id)
        {
            long AccountMasterId = string.IsNullOrEmpty(Id.DecryptString()) ? default(int) : Convert.ToInt64(Id.DecryptString());
            GeneralReceiptDto objDto = new GeneralReceiptDto();
            ViewBag.LockStatus = GroupInfo.LockStatus;
            if (AccountMasterId > 0)
            {
                objDto = _generalReceiptService.GetByID(AccountMasterId);
                if (objDto.AccountMasterID > 0)
                {
                    if (objDto.Addamount.Count > 0)
                    {
                        List<AddAmountDto> lstAccounts = new List<AddAmountDto>();

                        var list = objDto.Addamount;
                        foreach (var i in list)
                        {
                            AddAmountDto Addamountdto = new AddAmountDto();
                            int ahId = i.AHID;
                            bool isFederation = true;
                            AccountHeadDto accountHeadDto = _accountheadService.GetAccountHeadViewBalanceSummary(ahId, isFederation);
                            if (accountHeadDto.ClosingBalance != 0)
                            {
                                Decimal ClosingBalance = accountHeadDto.ClosingBalance + i.CrAmount;
                                Addamountdto.Balance = ClosingBalance;
                            }
                            Addamountdto.AHID = i.AHID;
                            Addamountdto.AHCode = i.AHCode;
                            Addamountdto.AHName = i.AHName;
                            Addamountdto.CrAmount = i.CrAmount;
                            Addamountdto.DrAmount = i.DrAmount;
                            lstAccounts.Add(Addamountdto);
                        }
                        objDto.Addamount = lstAccounts;
                    }
                }

                else
                {
                    ViewBag.Message = "General receipt was Not Active";
                }
            }

            //List<SelectListDto> lstselectDto = _accountheadService.GetGeneralReceiptLedgersDropDown(true);
            //SelectList lstahcode = new SelectList(lstselectDto, "ID", "Text");
            //ViewBag.ahcodes = lstahcode;

            List<AccountHeadDto> lstAccountHeadDtos = _accountheadService.GetGroupAccountTree(GroupInfo.GroupID);
            lstAccountHeadDtos = lstAccountHeadDtos.FindAll(l => l.AHLevel > 3);
            //List<SelectListDto> lstselectDto = _accountheadService.GetGeneralReceiptLedgersDropDown(true);
            SelectList lstahcode = new SelectList(lstAccountHeadDtos.OrderBy(l => l.AHNameAndCode), "AHID", "AHNameAndCode");
            ViewBag.ahcodes = lstahcode;

            BankMasterBll bankBll = new BankMasterBll();
            List<BankMasterDto> lstFedBanks = bankBll.GetGroupBanks(GroupInfo.GroupID);
            SelectList lstFederationBanks = new SelectList(lstFedBanks, "BankEntryID", "AccountNumber", "Select Bank");
            ViewBag.federationbanks = lstFederationBanks;
            int EmployeeID = UserInfo.UserID;
            EmployeeDto ObjEmployee = _employeeService.GetByID(EmployeeID);
            objDto.EmployeeCode = ObjEmployee.EmployeeCode;
            objDto.EmployeeName = ObjEmployee.EmployeeName;

            if (objDto.AccountMasterID > 0 && objDto.TransactionMode == "B")
            {
                objDto.AccountName = lstFedBanks.Find(l => l.AHID == objDto.AHID).AHName;
            }
            return View(objDto);
        }
        [HttpPost]
        public ActionResult CreateGeneralReceipt(FormCollection form)
        {
            bool isFederation = true;
            List<AccountHeadDto> lstslaccounts = _accountheadService.GetAll(isFederation).FindAll(f => f.AHLevel > 4 && f.IsSLAccount == false);
            SelectList slaccountno = new SelectList(lstslaccounts, "AHID", "AHCode");
            ViewBag.slaccounts = slaccountno;
            List<SelectListDto> lstselectDto = _accountheadService.GetGeneralReceiptLedgersDropDown(true);
            SelectList lstahcode = new SelectList(lstselectDto, "ID", "Text");
            ViewBag.ahcodes = lstahcode;
            BankMasterViewDto objBank = new BankMasterViewDto();
            List<BankMasterViewDto> lstFedBanks = _groupReceiptService.GetFederationBanks();
            SelectList lstFederationBanks = new SelectList(lstFedBanks, "BankEntryID", "AccountNumber", objBank.BankEntryID);
            ViewBag.federationbanks = lstFederationBanks;
            long AccountMasterID = Convert.ToInt64(Request.Form["hdnObjectID"]);
            var generalreceiptDto = ReadFormData(form);
            var resultDto = new ResultDto();
            if (generalreceiptDto.AccountMasterID == 0)
                resultDto = _generalReceiptService.Insert(generalreceiptDto);
            else
                resultDto = _generalReceiptService.Update(generalreceiptDto);
            int EmployeeID = UserInfo.UserID;
            EmployeeDto ObjEmployee = _employeeService.GetByID(EmployeeID);
            generalreceiptDto.EmployeeCode = ObjEmployee.EmployeeCode;
            generalreceiptDto.EmployeeName = ObjEmployee.EmployeeName;
            ViewBag.Result = resultDto;
            ViewBag.LockStatus = GroupInfo.LockStatus;
            return View(generalreceiptDto);
        }
        private GeneralReceiptDto ReadFormData(FormCollection form)
        {
            GeneralReceiptDto generalreceiptDto = new GeneralReceiptDto();
            generalreceiptDto.AccountMasterID = Convert.ToInt64(Request.Form["hdnObjectID"]);
            if (generalreceiptDto.AccountMasterID == 0)
            {
                int accountMasterID = default(int);
                int.TryParse(form["AccountMasterID"], out accountMasterID);
            }
            generalreceiptDto.UserID = UserInfo.UserID;
            generalreceiptDto.EmployeeID = UserInfo.UserID;
            if (form["BankAccount"] != null && form["BankAccount"] != string.Empty && form["BankAccount"] != "0")
            {
                generalreceiptDto.BankAccount = Convert.ToInt32(form["BankAccount"]);
            }
            generalreceiptDto.AHID = Convert.ToInt32(form["AccountHeadId"]);
            generalreceiptDto.Amount = Convert.ToDecimal(form["DRAmountTotal"]);
            generalreceiptDto.GroupID = GroupInfo.GroupID;
            generalreceiptDto.VoucherRefNumber = Convert.ToString(form["VoucherRefNumber"]);
            generalreceiptDto.PartyName = Convert.ToString(form["PartyName"]);
            generalreceiptDto.TransactionMode = Convert.ToString(form["TransactionMode"]);
            generalreceiptDto.ChequeNumber = Convert.ToString(form["ChequeNumber"]);
            generalreceiptDto.Narration = Convert.ToString(form["Narration"]);
            generalreceiptDto.TransactionType = Convert.ToInt32(form["TransactionType"]);

            string dttranformat = form["TransactionDate"].Contains("/") ? "dd/MMM/yyyy" : "dd-MMM-yyyy";
            DateTime dtTransactionDate = DateTime.ParseExact(form["TransactionDate"], dttranformat, provider);
            DateTime dtChequeDate = DateTime.Now;
            if (form["ChequeDate"] != null && form["ChequeDate"] != string.Empty)
            {
                string dtcheckdateformant = form["ChequeDate"].Contains("/") ? "dd/MMM/yyyy" : "dd-MMM-yyyy";
                dtChequeDate = DateTime.ParseExact(form["ChequeDate"], dtcheckdateformant, provider);
            }

            generalreceiptDto.TransactionDate = dtTransactionDate;
            generalreceiptDto.ChequeDate = dtChequeDate;

            int maxIndex = Convert.ToInt32(form["hdnMaxRateIndex"]);
            generalreceiptDto.Addamount = new List<AddAmountDto>();
            AddAmountDto Amount = null;
            for (int i = 1; i <= maxIndex; i++)
            {
                if (form["hdnAccountCode_" + i] == null) continue;

                Amount = new AddAmountDto();
                if ((form["hdnSLAccountNo_" + i]) != "")
                {
                    Amount.AHID = Convert.ToInt32(form["hdnSLAccountNo_" + i]);
                }
                else
                {
                    Amount.AHID = Convert.ToInt32(form["hdnAHID_" + i]);
                }
                Amount.AHCode = form["hdnAccountCode_" + i];
                Amount.Type = form["hdntypeBy_" + i];
                Amount.AHName = form["hdnAccountName_" + i];
                Amount.DrAmount = Convert.ToDecimal(form["hdnDrAmount_" + i]);
                Amount.CrAmount = Convert.ToDecimal(form["hdnCrAmount_" + i]);
                Amount.IsMaster = false;
                generalreceiptDto.Addamount.Add(Amount);
            }
            var MasterAmount = new AddAmountDto();
            MasterAmount.AHID = Convert.ToInt32(form["AccountHeadId"]);
            MasterAmount.CrAmount = 0;
            MasterAmount.DrAmount = Convert.ToDecimal(form["DRAmountTotal"]);
            MasterAmount.IsMaster = true;
            generalreceiptDto.Addamount.Add(MasterAmount);
            return generalreceiptDto;
        }
        public JsonResult GetAccountName(int id)
        {
            StringBuilder sbOptions = new StringBuilder();
            AccountHeadDto accountheaddto = _accountheadService.GetByID(id);
            List<SelectListDto> lstslaccounts = _accountheadService.GetSlAccountsGetByParentAhID(id, GroupInfo.GroupID);
            foreach (var item in lstslaccounts)
            {
                sbOptions.Append("<option value=" + item.ID + ">" + item.Text + "</option>");
            }

            return Json(new { AccountName = accountheaddto.AHName, SLAccounts = sbOptions.ToString() });
        }
        public JsonResult GetFAccountName(int id)
        {
            BankMasterDto objBank = _bankService.GetByID(id);
            return Json(new { FaccountName = objBank.BName, AHID = objBank.AHID, AccountName = objBank.AHName, BankAccountCode = objBank.AHCode });
        }
        public JsonResult GetAHID(string Id)
        {
            string isfed = Request.Form["isfederation"];
            bool isFederation = Convert.ToBoolean(isfed);

            AccountHeadDto objAccountHead = _accountheadService.GetCashInHandAccount(isFederation);
            return Json(new { AccountName = objAccountHead.AHName, AHID = objAccountHead.AHID, AHCode = objAccountHead.AHCode });
        }
        [HttpGet]
        public ActionResult GeneralReceiptLookup()
        {
            var lstGeneralReceiptDto = _generalReceiptService.Lookup();
            return View(lstGeneralReceiptDto);
        }
        public ActionResult ViewBalanceSummary(int ahId, bool isFederation)
        {
            AccountHeadDto accountHeadDto = _accountheadService.GetAccountHeadViewBalanceSummary(ahId, isFederation);
            return Json(new { ClosingBalance = accountHeadDto.ClosingBalance, BalanceType = accountHeadDto.BalanceType });
        }
        [HttpGet]
        public ActionResult DeleteGeneralReceipt(string Id)
        {
            int AccountmasterId = DecryptQueryString(Id);

            if (AccountmasterId < 1)
                return RedirectToAction("GeneralReceiptLookup");

            ResultDto resultDto = _generalReceiptService.Delete(AccountmasterId, UserInfo.UserID);

            TempData["Result"] = resultDto;

            return RedirectToAction("GeneralReceiptLookup");
        }

        [HttpGet]
        public ActionResult ActiveInactiveGeneralReceipt(string Id)
        {
            int AccountmasterId = DecryptQueryString(Id);

            if (AccountmasterId < 1)
                return RedirectToAction("GeneralReceiptLookup");

            ResultDto resultDto = _generalReceiptService.ChangeStatus(AccountmasterId, UserInfo.UserID);

            TempData["Result"] = resultDto;

            return RedirectToAction("GeneralReceiptLookup");
        }
    }
}
