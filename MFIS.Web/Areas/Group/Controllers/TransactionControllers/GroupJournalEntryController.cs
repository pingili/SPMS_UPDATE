using AutoMapper;
using BusinessEntities;
using BusinessLogic.AutoMapper;
using BusinessLogic;
//using BusinessLogic.Interface;
using CoreComponents;
using MFIS.Web.Areas.Federation.Models;
using MFIS.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Utilities;

namespace MFIS.Web.Areas.Group.Controllers.TransactionControllers
{
    public class GroupJournalEntryController : BaseController
    {
        //
        // GET: /Group/GroupJournalEntry/

        //
        // GET: /Federation/JournalEntryVoucher/
       private readonly GroupJournalService1 _journalService;
        private readonly EmployeeService _employeeService;
        private readonly GroupReceiptService _groupReceiptService;
        private readonly AccountHeadService _accountHeadService;
        private readonly GroupService _groupService;
        private readonly ClusterService _clusterService;
        private readonly FederationGeneralPaymentsService _generalpaymentsService;



        public GroupJournalEntryController()
        {
            _journalService = new GroupJournalService1();
            _groupReceiptService = new GroupReceiptService();
            _accountHeadService = new AccountHeadService();
            _employeeService = new EmployeeService();
            _generalpaymentsService = new FederationGeneralPaymentsService();
            _groupService = new GroupService();
            _clusterService = new ClusterService();
        }
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult JournalEntryVoucherLookup()
        {

            var lastJournalEntryDto = _journalService.Lookup();


            return View(lastJournalEntryDto);
        }
        [HttpGet]
        public ActionResult CreateJournalEntryVoucher(string Id)
        {

            long AccountMasterId = string.IsNullOrEmpty(Id.DecryptString()) ? default(int) : Convert.ToInt64(Id.DecryptString());
            ReceiptMasterDto objDto = new ReceiptMasterDto();
            if (AccountMasterId > 0)
            {
                objDto = _journalService.GetByID(AccountMasterId);
            }
            LoadDropDown();
            int EmployeeID = UserInfo.UserID;
            EmployeeDto ObjEmployee = _employeeService.GetByID(EmployeeID);
            objDto.EmployeeCode = ObjEmployee.EmployeeCode;
            objDto.EmployeeName = ObjEmployee.EmployeeName;
            return View(objDto);
        }
        [HttpPost]
        public ActionResult CreateJournalEntryVoucher(FormCollection form)
        {

            var journalEntryDto = ReadFormData(form);
            var resultDto = new ResultDto();
            LoadDropDown();
            if (journalEntryDto.AccountMasterID == 0)

                resultDto = _journalService.Insert(journalEntryDto);
            else
                resultDto = _journalService.Update(journalEntryDto);
            ViewBag.Result = resultDto;
            return View(journalEntryDto);
        }

        private ReceiptMasterDto ReadFormData(FormCollection form)
        {
            ReceiptMasterDto groupReceiptDto = new ReceiptMasterDto();
            int AccountMasterID = default(int);
            DateTime dtTransactiondate = form["txtTransactionDate"].ConvertToDateTime();
            int.TryParse(form["AccountTranID"], out AccountMasterID);
            groupReceiptDto.UserID = UserInfo.UserID;
            groupReceiptDto.EmployeeID = UserInfo.UserID;
            groupReceiptDto.VoucherRefNumber = Convert.ToString(form["JournalEntryRefNo"]);
            groupReceiptDto.AccountMasterID = Convert.ToInt64(form["AccountMasterID"]);
            //groupReceiptDto.EmployeeID = Convert.ToInt32(form["EmployeeID"]);
            groupReceiptDto.Amount = Convert.ToDecimal(form["hdnTotal"]);
            groupReceiptDto.EmployeeName = Convert.ToString(form["EmployeeName"]);
            //groupReceiptDto.PartyName = Convert.ToString(form["PartyName"]);
            groupReceiptDto.TransactionDate = dtTransactiondate;
            groupReceiptDto.AHName = Convert.ToString(form["AHName"]);
            groupReceiptDto.AHCode = Convert.ToString(form["AHCode"]);
            groupReceiptDto.AHID = Convert.ToInt32(form["AHID"]);
            groupReceiptDto.Amount = Convert.ToDecimal(form["hdnTotal"]);
            //groupReceiptDto.AHID = Convert.ToInt32(form["SLAcNo"]);
            //groupReceiptDto.AccountHead = Convert.ToString(form["SLAName"]);
            groupReceiptDto.TransactionMode = Convert.ToString(form["TransactionMode"]);
            groupReceiptDto.Narration = Convert.ToString(form["Narration"]);
            groupReceiptDto.TransactionType = Convert.ToInt32(form["TransactionType"]);
            int maxIndex = Convert.ToInt32(form["hdnMaxJournalIndex"]);
            groupReceiptDto.lstGroupReceiptTranscationDto = new List<ReceiptTranscationDto>();
            ReceiptTranscationDto Amount = null;
            for (int i = 1; i <= maxIndex; i++)
            {
                if (form["hdnAHCode_" + i] == null) continue;

                Amount = new ReceiptTranscationDto();
                Amount.AHID = Convert.ToInt32(form["hdnAHID_" + i]);
                Amount.AHName = form["hdnAHCode_" + i];
                //Amount.AHCode = form["hdnSLACNo_" + i];
                //Amount.AHName = form["hdnSLAName_" + i];
                Amount.DrAmount = Convert.ToDecimal(form["hdnDrAmount_" + i]);
                Amount.CrAmount = Convert.ToDecimal(form["hdnCrAmount_" + i]);
                Amount.AccountTranID = Convert.ToInt64(form["hdnAccountTranID_" + i]);

                groupReceiptDto.lstGroupReceiptTranscationDto.Add(Amount);
            }
            return groupReceiptDto;
        }
        private void LoadDropDown()
        {
            var employeeSelectListDto = _employeeService.GetEmployeeCodeSelectList();
            var employeeSelectList = new SelectList(employeeSelectListDto, "ID", "Text");
            ViewBag.EmployeeNumber = employeeSelectList;

            var lstAccountHeads = _accountHeadService.GetJournalLedgers();

            //SelectList SlaAHSelectList = new SelectList(lstAccountHeads, "AHID", "AHName");
            //ViewBag.SLAAcHeads = SlaAHSelectList;

            List<SelectListDto> lstselectDto = _accountHeadService.GetGeneralReceiptLedgersDropDown(true);
            lstselectDto = lstselectDto.ToList().FindAll(l => l.Text != "Cash In Hand" && l.Text != "Bank Accounts" && l.ID != 365683 && l.ID != 365687).ToList();
            SelectList lstahcode = new SelectList(lstselectDto, "ID", "Text");
            ViewBag.SLAAcHeads = lstahcode;


            List<SelectListDto> lstClusters = _clusterService.GetClusterSelectList();
            SelectList slClusters = new SelectList(lstClusters, "ID", "Text");
            ViewBag.clusters = slClusters;

            List<GroupLookupDto> lstGroupDto = _groupService.Lookup();
            SelectList lstgroup = new SelectList(lstGroupDto, "GroupID", "GroupCode");
            ViewBag.GroupNames = lstgroup;
            BankMasterViewDto objBank = new BankMasterViewDto();
            List<BankMasterViewDto> lstFedBanks = _groupReceiptService.GetFederationBanks();
            SelectList lstFederationBanks = new SelectList(lstFedBanks, "BankEntryID", "AccountNumber", objBank.BankEntryID);
            ViewBag.federationbanks = lstFederationBanks;
        }
        public JsonResult GetEmployeename(int id)
        {
            EmployeeDto EmployeeDto = _employeeService.GetEmployeeNameByID(id);

            return Json(new
            {
                EmployeeName = EmployeeDto.EmployeeName
            });
        }
        public JsonResult GetGroupName(int id)
        {
            GroupMasterDto groupMasterDto = _groupService.GetByID(id);
            return Json(new { GroupName = groupMasterDto.GroupName });
        }
        public JsonResult GetClusterName(int id)
        {
            ClusterDto ClusterDto = _clusterService.GetByID(id);
            return Json(new { ClusterName = ClusterDto.ClusterName });
        }
        public JsonResult GetAccountName(int id)
        {
            StringBuilder sbOptions = new StringBuilder();
            AccountHeadDto accountheaddto = _accountHeadService.GetByID(id);
            List<SelectListDto> lstslaccounts = _accountHeadService.GetSlAccountsGetByParentAhID(id, GroupInfo.GroupID);
            foreach (var item in lstslaccounts)
            {
                sbOptions.Append("<option value=" + item.ID + ">" + item.Text + "</option>");
            }

            return Json(new { AccountName = accountheaddto.AHName, SLAccounts = sbOptions.ToString() });
        }
        public JsonResult GetSLAAccountHead(int id)
        {

            AccountHeadDto AccountDto = _accountHeadService.GetByID(id);
            return Json(new
            {
                AccountHead = AccountDto.AHCode,
                AHID = AccountDto.AHID,
                AccountName = AccountDto.AHName

            });
        }
        [HttpGet]
        public ActionResult DeleteJournalEntry(string Id)
        {
            int AccountmasterId = DecryptQueryString(Id);

            if (AccountmasterId < 1)
                return RedirectToAction("JournalEntryVoucherLookup");

            ResultDto resultDto = _journalService.Delete(AccountmasterId, UserInfo.UserID);

            TempData["Result"] = resultDto;

            return RedirectToAction("JournalEntryVoucherLookup");
        }
        [HttpGet]
        public ActionResult ActiveInactiveJournalEntryPayments(string Id)
        {
            int AccountmasterId = DecryptQueryString(Id);

            if (AccountmasterId < 1)
                return RedirectToAction("JournalEntryVoucherLookup");

            ResultDto resultDto = _journalService.ChangeStatus(AccountmasterId, UserInfo.UserID);

            TempData["Result"] = resultDto;

            return RedirectToAction("JournalEntryVoucherLookup");
        }
    }
}
