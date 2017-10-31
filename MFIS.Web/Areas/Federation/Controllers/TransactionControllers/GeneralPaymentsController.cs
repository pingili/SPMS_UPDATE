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
using MFIEntityFrameWork;
using System.Text;

namespace MFIS.Web.Areas.Federation.Controllers.TransactionControllers
{
    public class GeneralPaymentsController : BaseController
    {
        #region Global Variables
        private readonly AccountHeadService _accountheadService;
        private readonly EmployeeService _employeeService;
        private readonly FederationGeneralPaymentsService _generalpaymentsService;
        private readonly GroupReceiptService _groupReceiptService;
        private readonly BankService _bankService;
        private readonly GroupService _groupService;
        private readonly ClusterService _clusterService;
        public GeneralPaymentsController()
        {
            _clusterService = new ClusterService();
            _groupService = new GroupService();
            _accountheadService = new AccountHeadService();
            _employeeService = new EmployeeService();
            _generalpaymentsService = new FederationGeneralPaymentsService();
            _groupReceiptService = new GroupReceiptService();
            _bankService = new BankService();
        }

        #endregion Global Variables
        [HttpGet]
        public ActionResult CreateGeneralPayments(string Id)
        {
            long AccountMasterId = string.IsNullOrEmpty(Id.DecryptString()) ? default(int) : Convert.ToInt64(Id.DecryptString());
            GeneralPaymentsDto objDto = new GeneralPaymentsDto();
            if (AccountMasterId > 0)
            {
                objDto = _generalpaymentsService.GetByID(AccountMasterId);
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
                            Decimal ClosingBalance = accountHeadDto.ClosingBalance - i.DrAmount;
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
            List<SelectListDto> lstselectDto = _accountheadService.GetGeneralReceiptLedgersDropDown(true);
            SelectList lstahcode = new SelectList(lstselectDto, "ID", "Text");
            ViewBag.ahcodes = lstahcode;

            BankMasterViewDto objBank = new BankMasterViewDto();
            List<BankMasterViewDto> lstFedBanks = _groupReceiptService.GetFederationBanks();
            SelectList lstFederationBanks = new SelectList(lstFedBanks, "BankEntryID", "AccountNumber", objBank.BankEntryID);
            ViewBag.federationbanks = lstFederationBanks;

            List<SelectListDto> lstClusters = _clusterService.GetClusterSelectList();
            SelectList slClusters = new SelectList(lstClusters, "ID", "Text");
            ViewBag.clusters = slClusters;

            List<GroupLookupDto> lstGroupDto = _groupService.Lookup();
            SelectList lstgroup = new SelectList(lstGroupDto, "GroupID", "GroupCode");
            ViewBag.GroupNames = lstgroup;
            int EmployeeID = UserInfo.UserID;


            EmployeeDto ObjEmployee = _employeeService.GetByID(EmployeeID);

            objDto.EmployeeCode = ObjEmployee.EmployeeCode;
            objDto.EmployeeName = ObjEmployee.EmployeeName;

            return View(objDto);
        }

        [HttpPost]
        public ActionResult CreateGeneralPayments(FormCollection form)
        {

            List<SelectListDto> lstClusters = _clusterService.GetClusterSelectList();
            SelectList slClusters = new SelectList(lstClusters, "ID", "Text");
            ViewBag.clusters = slClusters;

            List<GroupLookupDto> lstGroupDto = _groupService.Lookup();
            SelectList lstgroup = new SelectList(lstGroupDto, "GroupID", "GroupCode");
            ViewBag.GroupNames = lstgroup;

            List<SelectListDto> lstselectDto = _accountheadService.GetGeneralReceiptLedgersDropDown(true);
            SelectList lstahcode = new SelectList(lstselectDto, "ID", "Text");
            ViewBag.ahcodes = lstahcode;
            BankMasterViewDto objBank = new BankMasterViewDto();
            List<BankMasterViewDto> lstFedBanks = _groupReceiptService.GetFederationBanks();
            for (int i = 0; i < lstFedBanks.Count; i++)
            {
                var lst = lstFedBanks[i].AccountNumber + ':' + lstFedBanks[i].BankCode;
                lstFedBanks[i].AccountNumber = lst;
            }
            SelectList lstFederationBanks = new SelectList(lstFedBanks, "BankEntryID", "AccountNumber", objBank.BankEntryID);
            ViewBag.federationbanks = lstFederationBanks;
            long AccountMasterID = Convert.ToInt64(Request.Form["hdnObjectID"]);
            var generalpaymentsDto = ReadFormData(form);
            var resultDto = new ResultDto();
            if (generalpaymentsDto.AccountMasterID == 0)
                resultDto = _generalpaymentsService.Insert(generalpaymentsDto);
            else
                resultDto = _generalpaymentsService.Update(generalpaymentsDto);

            ViewBag.Result = resultDto;
            return View(generalpaymentsDto);
        }
        private GeneralPaymentsDto ReadFormData(FormCollection form)
        {
            GeneralPaymentsDto generalPaymentsDto = new GeneralPaymentsDto();
            generalPaymentsDto.AccountMasterID = Convert.ToInt64(Request.Form["hdnObjectID"]);
            if (generalPaymentsDto.AccountMasterID == 0)
            {
                int accountMasterID = default(int);
                int.TryParse(form["AccountMasterID"], out accountMasterID);
            }
            generalPaymentsDto.UserID = UserInfo.UserID;
            generalPaymentsDto.EmployeeID = UserInfo.UserID;
            if (form["BankAccount"] != null && form["BankAccount"] != string.Empty && form["BankAccount"] != "0")
            {
                generalPaymentsDto.BankAccount = Convert.ToInt32(form["BankAccount"]);
            }
            generalPaymentsDto.AHID = Convert.ToInt32(form["AccountHeadId"]);
            generalPaymentsDto.Amount = Convert.ToDecimal(form["AmountTotal"]);

            if (!string.IsNullOrEmpty(form["ddlgroup"]))
            generalPaymentsDto.GroupID = Convert.ToInt32(form["ddlgroup"]);
            generalPaymentsDto.VoucherRefNumber = Convert.ToString(form["VoucherRefNumber"]);
            generalPaymentsDto.PartyName = Convert.ToString(form["PartyName"]);
            generalPaymentsDto.TransactionMode = Convert.ToString(form["TransactionMode"]);
            generalPaymentsDto.ChequeNumber = Convert.ToString(form["ChequeNumber"]);
            generalPaymentsDto.Narration = Convert.ToString(form["Narration"]);
            generalPaymentsDto.TransactionType = Convert.ToInt32(form["TransactionType"]);
            DateTime dtTransactionDate = DateTime.ParseExact(form["TransactionDate"], "dd/MMM/yyyy", provider);
            DateTime dtChequeDate = DateTime.Now;
            if (form["ChequeDate"] != null && form["ChequeDate"] != string.Empty)
            {
                dtChequeDate = DateTime.ParseExact(form["ChequeDate"], "dd/MMM/yyyy", provider);
            }

            generalPaymentsDto.TransactionDate = dtTransactionDate;
            generalPaymentsDto.ChequeDate = dtChequeDate;

            int maxIndex = Convert.ToInt32(form["hdnMaxRateIndex"]);
            generalPaymentsDto.Addamount = new List<AddAmountDto>();
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
                Amount.IsMaster = true;

                generalPaymentsDto.Addamount.Add(Amount);

            }

            var MasterAmount = new AddAmountDto();

            MasterAmount.AHID = Convert.ToInt32(form["AccountHeadId"]);
            MasterAmount.CrAmount = Convert.ToDecimal(form["CRAmountTotal"]);
            MasterAmount.IsMaster = true;
            MasterAmount.DrAmount = 0;
            generalPaymentsDto.Addamount.Add(MasterAmount);
            return generalPaymentsDto;
        }
        public JsonResult GetAccountName(int id)
        {
            StringBuilder sbOptions = new StringBuilder();
            AccountHeadDto accountheaddto = _accountheadService.GetByID(id);
            List<SelectListDto> lstslaccounts = _accountheadService.GetSlAccountsGetByParentAhID(id, null);
            foreach (var item in lstslaccounts)
            {
                sbOptions.Append("<option value=" + item.ID + ">" + item.Text + "</option>");
            }

            return Json(new { AccountName = accountheaddto.AHName, SLAccounts = sbOptions.ToString() });
        }
        public JsonResult GetFAccountName(int id)
        {
            BankMasterDto objBank = _bankService.GetByID(id);
            return Json(new { FaccountName = objBank.BName, AHID = objBank.AHID, AccountName = objBank.AHName, AccountCode = objBank.AHCode });
        }
        public JsonResult GetGroupName(int id)
        {
            GroupMasterDto groupMasterDto = _groupService.GetByID(id);
            return Json(new { GroupName = groupMasterDto.GroupName });
        }
        public ActionResult BindDropDowns(string flag, int Id)
        {
            StringBuilder sbOptions = new StringBuilder();
            if (flag == "Cluster")
            {
                List<SelectListDto> lstvillageDto = _groupService.GetGroupCodeByClusterID(Id);
                foreach (var item in lstvillageDto)
                {
                    sbOptions.Append("<option value=" + item.ID + ">" + item.Text + "</option>");
                }
            }
            return Content(sbOptions.ToString());
        }
        public ActionResult ViewBalanceSummary(int ahId, bool isFederation)
        {
            AccountHeadDto accountHeadDto = _accountheadService.GetAccountHeadViewBalanceSummary(ahId, isFederation);
            return Json(new { ClosingBalance = accountHeadDto.ClosingBalance, BalanceType = accountHeadDto.BalanceType });
        }
        [HttpGet]
        public ActionResult GeneralPaymentsLookup()
        {
            var lstGeneralPaymentsDto = _generalpaymentsService.Lookup();
            return View(lstGeneralPaymentsDto);
        }
        [HttpGet]
        public ActionResult DeleteGeneralPayments(string Id)
        {
            int AccountmasterId = DecryptQueryString(Id);

            if (AccountmasterId < 1)
                return RedirectToAction("GeneralPaymentsLookup");

            ResultDto resultDto = _generalpaymentsService.Delete(AccountmasterId, UserInfo.UserID);

            TempData["Result"] = resultDto;

            return RedirectToAction("GeneralPaymentsLookup");
        }

        [HttpGet]
        public ActionResult ActiveInactiveGeneralPayments(string Id)
        {
            int AccountmasterId = DecryptQueryString(Id);

            if (AccountmasterId < 1)
                return RedirectToAction("GeneralPaymentsLookup");

            ResultDto resultDto = _generalpaymentsService.ChangeStatus(AccountmasterId, UserInfo.UserID);

            TempData["Result"] = resultDto;

            return RedirectToAction("GeneralPaymentsLookup");
        }

        public JsonResult GetAHID(string Id)
        {
            string isfed = Request.Form["isfederation"];
            bool isFederation = Convert.ToBoolean(isfed);

            AccountHeadDto objAccountHead = _accountheadService.GetCashInHandAccount(isFederation);
            return Json(new { AccountName = objAccountHead.AHName, AHID = objAccountHead.AHID, AHCode = objAccountHead.AHCode });
        }
        public ActionResult ViewGeneralPayments(string Id)
        {
            long AccountMasterId = string.IsNullOrEmpty(Id.DecryptString()) ? default(int) : Convert.ToInt64(Id.DecryptString());
            GeneralPaymentsDto objDto = new GeneralPaymentsDto();
            if (AccountMasterId > 0)
            {
                objDto = _generalpaymentsService.GetByViewID(AccountMasterId);
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
                            Decimal ClosingBalance = accountHeadDto.ClosingBalance - i.DrAmount;
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
            int EmployeeID = UserInfo.UserID;
            EmployeeDto ObjEmployee = _employeeService.GetByID(EmployeeID);
            objDto.EmployeeCode = ObjEmployee.EmployeeCode;
            objDto.EmployeeName = ObjEmployee.EmployeeName;
            return View(objDto);
            //return View();
        }
    }
}
