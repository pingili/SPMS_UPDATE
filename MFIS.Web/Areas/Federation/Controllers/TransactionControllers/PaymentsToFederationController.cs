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

namespace MFIS.Web.Areas.Federation.Controllers.TransactionControllers
{
    public class PaymentsToFederationController : BaseController
    {
        #region Global Variables
        private readonly AccountHeadService _accountheadService;
        private readonly EmployeeService _employeeService;
        private readonly FederationGeneralPaymentsService _generalpaymentsService;
        private readonly GroupReceiptService _groupReceiptService;
        private readonly BankService _bankService;
        private readonly GroupService _groupService;
        private readonly ClusterService _clusterService;
        private readonly MFISDBContext _dbContext;
        private readonly PaymentsToFederationService _paymentsToFederationService;


        public PaymentsToFederationController()
        {
            _clusterService = new ClusterService();
            _groupService = new GroupService();
            _accountheadService = new AccountHeadService();
            _employeeService = new EmployeeService();
            _generalpaymentsService = new FederationGeneralPaymentsService();
            _groupReceiptService = new GroupReceiptService();
            _bankService = new BankService();
            _dbContext = new MFISDBContext();
            _dbContext = new MFISDBContext();
            //_paymentsToFederationService = new PaymentsToFederationService();
            _bankService = new BankService();
            _groupReceiptService = new GroupReceiptService();
            _employeeService = new EmployeeService();

        }
        #endregion Global Variables

        [HttpGet]
        public ActionResult CreatePaymentsToFederation(string Id)
        {
            bool isFederation = true;
            long AccountMasterId = string.IsNullOrEmpty(Id.DecryptString()) ? default(int) : Convert.ToInt64(Id.DecryptString());
            GeneralReceiptDto objDto = new GeneralReceiptDto();
            if (AccountMasterId > 0)
            {
                objDto = _paymentsToFederationService.GetByID(AccountMasterId);
                if (objDto.Addamount.Count > 0)
                {
                    List<AddAmountDto> lstAccounts = new List<AddAmountDto>();

                    var list = objDto.Addamount;
                    foreach (var i in list)
                    {
                        AddAmountDto Addamountdto = new AddAmountDto();
                        int ahId = i.AHID;

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

            List<SelectListDto> lstselectDto = _accountheadService.GetGeneralReceiptLedgersDropDown(true);
            SelectList lstahcode = new SelectList(lstselectDto, "ID", "Text");
            ViewBag.ahcodes = lstahcode;
            isFederation = true;
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

            List<uspAccountHeadGetAll_Result> lstuspAccountHeadGetAll_Result = _dbContext.uspAccountHeadGetAll().ToList();

            var listOfSearchedIds = new List<string> { "2020", "2114", "1231", "1232", "1241", "1242", "1243" };
            lstuspAccountHeadGetAll_Result = (lstuspAccountHeadGetAll_Result.Where(x => listOfSearchedIds.Contains(x.AHCode))).ToList();
            SelectList FederationAHeads = new SelectList(lstuspAccountHeadGetAll_Result, "AHID", "AHName");
            ViewBag.FederationAH = FederationAHeads;

            return View(objDto);
        }
        [HttpPost]
        public ActionResult CreatePaymentsToFederation(FormCollection form)
        {
            bool isFederation = true;
            List<AccountHeadDto> lstslaccounts = _accountheadService.GetAll(isFederation).FindAll(f => f.AHLevel > 4 && f.IsSLAccount == false);
            SelectList slaccountno = new SelectList(lstslaccounts, "AHID", "AHCode");
            ViewBag.slaccounts = slaccountno;

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
            SelectList lstFederationBanks = new SelectList(lstFedBanks, "BankEntryID", "AccountNumber", objBank.BankEntryID);
            ViewBag.federationbanks = lstFederationBanks;
            long AccountMasterID = Convert.ToInt64(Request.Form["hdnObjectID"]);
            var generalpaymentsDto = ReadFormData(form);
            var resultDto = new ResultDto();
            if (generalpaymentsDto.AccountMasterID == 0)
                resultDto = _paymentsToFederationService.Insert(generalpaymentsDto);
            else
                resultDto = _paymentsToFederationService.Update(generalpaymentsDto);
            List<uspAccountHeadGetAll_Result> lstuspAccountHeadGetAll_Result = _dbContext.uspAccountHeadGetAll().ToList();

            var listOfSearchedIds = new List<string> { "2020", "2114", "1231", "1232", "1241", "1242", "1243" };
            lstuspAccountHeadGetAll_Result = (lstuspAccountHeadGetAll_Result.Where(x => listOfSearchedIds.Contains(x.AHCode))).ToList();
            SelectList FederationAHeads = new SelectList(lstuspAccountHeadGetAll_Result, "AHID", "AHName");
            ViewBag.FederationAH = FederationAHeads;
            ViewBag.Result = resultDto;
            return View(generalpaymentsDto);

        }
        private ReceiptTranscationDto GetAccountHeadClosingBalnces()
        {
            ReceiptTranscationDto objReceiptTranscationDto = new ReceiptTranscationDto();
            objReceiptTranscationDto = _groupReceiptService.GetAccountHeadClosingBalnces();
            return objReceiptTranscationDto;
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
            generalreceiptDto.AHID = Convert.ToInt32(form["hdnCashinHandAHID"]);
            generalreceiptDto.Amount = Convert.ToDecimal(form["DRAmountTotal"]);
            generalreceiptDto.GroupID = GroupInfo.GroupID;
            generalreceiptDto.VoucherRefNumber = Convert.ToString(form["VoucherRefNumber"]);
            generalreceiptDto.PartyName = Convert.ToString(form["PartyName"]);
            generalreceiptDto.TransactionMode = Convert.ToString(form["TransactionMode"]);
            generalreceiptDto.ChequeNumber = Convert.ToString(form["ChequeNumber"]);
            generalreceiptDto.Narration = Convert.ToString(form["Narration"]);
            generalreceiptDto.TransactionType = Convert.ToInt32(form["TransactionType"]);
            DateTime dtTransactionDate = DateTime.ParseExact(form["TransactionDate"], "dd/MMM/yyyy", provider);
            DateTime dtChequeDate = DateTime.Now;
            if (form["ChequeDate"] != null && form["ChequeDate"] != string.Empty)
            {
                dtChequeDate = DateTime.ParseExact(form["ChequeDate"], "dd/MMM/yyyy", provider);
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
                Amount.AHID = Convert.ToInt32(form["FedAHead_" + i]);
                Amount.AHCode = form["hdnAccountCode_" + i];
                Amount.Type = form["hdntypeBy_" + i];
                Amount.AHName = form["hdnAccountName_" + i];
                Amount.DrAmount = Convert.ToDecimal(form["hdnDrAmount_" + i]);
                Amount.CrAmount = Convert.ToDecimal(form["hdnCrAmount_" + i]);
                Amount.IsMaster = false;
                generalreceiptDto.Addamount.Add(Amount);
            }
            var MasterAmount = new AddAmountDto();
            MasterAmount.AHID = Convert.ToInt32(form["hdnCashinHandAHID"]);
            MasterAmount.CrAmount = 0;
            MasterAmount.DrAmount = Convert.ToDecimal(form["DRAmountTotal"]);
            MasterAmount.IsMaster = true;
            generalreceiptDto.Addamount.Add(MasterAmount);
            return generalreceiptDto;

        }

        public ActionResult PaymentsToFederationLookup()
        {
            int GroupId = GroupInfo.GroupID;
            var lstGeneralPaymentsDto = _paymentsToFederationService.PaymentsToFederationLookup(GroupId);
            return View(lstGeneralPaymentsDto);
        }
        [HttpGet]
        public ActionResult DeleteRefundsFromFederation(string Id)
        {
            int AccountmasterId = DecryptQueryString(Id);

            if (AccountmasterId < 1)
                return RedirectToAction("RefundsFromFederationLookup");

            ResultDto resultDto = _paymentsToFederationService.Delete(AccountmasterId, UserInfo.UserID);

            TempData["Result"] = resultDto;

            return RedirectToAction("RefundsFromFederationLookup");
        }

        [HttpGet]
        public ActionResult ActiveInactiveRefundsFromFederation(string Id)
        {
            int AccountmasterId = DecryptQueryString(Id);

            if (AccountmasterId < 1)
                return RedirectToAction("RefundsFromFederationLookup");

            ResultDto resultDto = _paymentsToFederationService.ChangeStatus(AccountmasterId, UserInfo.UserID);

            TempData["Result"] = resultDto;

            return RedirectToAction("RefundsFromFederationLookup");
        }

        private List<ReceiptTranscationDto> Accountheads()
        {
            int GroupId = GroupInfo.GroupID;
            List<ReceiptTranscationDto> lstGroupReceiptTranscationDto = _paymentsToFederationService.GetAccountdetails(GroupId);
            return lstGroupReceiptTranscationDto;
        }
        public JsonResult GetGroupName(int id)
        {
            GroupMasterDto groupMasterDto = _groupService.GetByID(id);
            return Json(new { GroupName = groupMasterDto.GroupName });
        }

    }
}
