using BusinessEntities;
using BusinessLogic;
//using BusinessLogic.Interface;
using MFIS.Web.Areas.Federation.Models;
using MFIS.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Utilities;
using CoreComponents;
using DataLogic;


namespace MFIS.Web.Areas.Federation.Controllers.TransactionControllers
{
    public class ReceiptController : BaseController
    {

        #region Global Variables
        private readonly GroupService _groupService;
        private readonly PanchayatService _panchayatService;
        private readonly ReferenceValueService _referenceValueService;
        private BankService _bankService;
        private readonly GroupReceiptService _groupReceiptService;
        private readonly ClusterService _ClusterService;
        private readonly VillageService _villageService;
        private readonly EmployeeService _employeeService;
        private readonly AccountHeadService _accountheadService;
        private readonly RepaymentService _repaymentService;

        public ReceiptController()
        {
            _groupService = new GroupService();
            _referenceValueService = new ReferenceValueService();
            _panchayatService = new PanchayatService();
            _bankService = new BankService();
            _groupReceiptService = new GroupReceiptService();
            _ClusterService = new ClusterService();
            _villageService = new VillageService();
            _employeeService = new EmployeeService();
            _accountheadService = new AccountHeadService();
            _repaymentService = new RepaymentService();
        }
        #endregion Global Variables
        [HttpGet]
        public ActionResult CreateGroupReceipt(string id)
        {
            int AccountMasterID = string.IsNullOrEmpty(id.DecryptString()) ? default(int) : Convert.ToInt32(id.DecryptString());
            ReceiptMasterDto objreceipt = new ReceiptMasterDto();


            //objreceipt.lstGroupReceiptTranscationDto = Accountheads();



            var ReceiptTranscationDto = GetAccountHeadClosingBalnces();
            ViewBag.CashInHandDetails = ReceiptTranscationDto;

            int EmployeeID = UserInfo.UserID;
            EmployeeDto obj = _employeeService.GetByID(EmployeeID);
            objreceipt.EmployeeName = obj.EmployeeName;
            objreceipt.EmployeeCode = obj.EmployeeCode;
            objreceipt.EmployeeID = obj.EmployeeID;
            LoadDropDowns();
            return View(objreceipt);
        }

        private void LoadDropDowns()
        {
            ReceiptMasterDto objreceipt = new ReceiptMasterDto();

            List<SelectListDto> lstClusters = _ClusterService.GetClusterSelectList();
            SelectList slClusters = new SelectList(lstClusters, "ID", "Text");
            ViewBag.Clusters = slClusters;
            #region DropDowns
            List<SelectListDto> groupSelectList = _groupService.GetGroupByClusterID(objreceipt.ClusterID);
            SelectList lstGroupNames = new SelectList(groupSelectList, "ID", "Text");
            ViewBag.GroupNames = lstGroupNames;

            //List<SelectListDto> villageSelectList = _villageService.GetVillageByClusterID(objreceipt.ClusterID);
            //SelectList lstvillage = new SelectList(villageSelectList, "ID", "Text");
            //ViewBag.Village = lstvillage;

            //List<SelectListDto> groupSelectList = _groupService.GetGroupByVillageID(objreceipt.VillageID);
            //SelectList lstGroupNames = new SelectList(groupSelectList, "ID", "Text");
            //ViewBag.GroupNames = lstGroupNames;

            //List<BankMasterViewDto> lstAllBanks = _groupReceiptService.GetGroupBanks();
            //SelectList lstgroupBanks = new SelectList(lstAllBanks, "BankEntryID", "AccountNumber", objBank.BankEntryID);
            //ViewBag.GroupBanks = lstgroupBanks;
            #endregion DropDowns
            BankMasterViewDto objBank = new BankMasterViewDto();
            List<BankMasterViewDto> lstFedBanks = _groupReceiptService.GetFederationBanks();
            SelectList lstFederationBanks = new SelectList(lstFedBanks, "BankEntryID", "AccountNumber", objBank.BankEntryID);
            ViewBag.federationbanks = lstFederationBanks;
        }

        private ReceiptTranscationDto GetAccountHeadClosingBalnces()
        {
            ReceiptTranscationDto objReceiptTranscationDto = new ReceiptTranscationDto();
            objReceiptTranscationDto = _groupReceiptService.GetAccountHeadClosingBalnces();
            return objReceiptTranscationDto;
        }

        [HttpPost]
        public ActionResult CreateGroupReceipt(FormCollection form)
        {
            var groupReceiptdto = ReadFormData(form);
            var resultDto = new ResultDto();
            LoadDropDowns();

            if (groupReceiptdto.AccountMasterID == 0)
            {
                resultDto = _groupReceiptService.Insert(groupReceiptdto);
            }
            else
            {
                resultDto = _groupReceiptService.Update(groupReceiptdto);
            }
            ViewBag.Result = resultDto;
            return View(groupReceiptdto);
        }
        private ReceiptMasterDto ReadFormData(FormCollection form)
        {
            var ReceiptTranscationDto = GetAccountHeadClosingBalnces();
            ViewBag.CashInHandDetails = ReceiptTranscationDto;

            ReceiptMasterDto groupReceiptDto = new ReceiptMasterDto();
            int accountMasterId = default(int);
            int.TryParse(form["AccountMasterID"], out accountMasterId);

            groupReceiptDto.IsGroup = false;
            groupReceiptDto.SubHeadID = 1;

            groupReceiptDto.AccountMasterID = Convert.ToInt32(form["AccountMasterID"]);
            groupReceiptDto.VoucherNumber = Convert.ToString(form["VoucherNumber"]);
            groupReceiptDto.GroupID = Convert.ToInt32(form["GroupID"]);
            groupReceiptDto.GroupName = Convert.ToString(form["GroupName"]);
            groupReceiptDto.EmployeeID = Convert.ToInt32(form["EmployeeID"]);
            groupReceiptDto.EmployeeCode = Convert.ToString(form["EmployeeCode"]);
            groupReceiptDto.EmployeeName = Convert.ToString(form["EmployeeName"]);
            DateTime dtTransactionDate = DateTime.ParseExact(form["TransactionDate"], "dd/MMM/yyyy", provider);
            DateTime dtChequeDate = DateTime.Now;
            if (form["ChequeDate"] != null && form["ChequeDate"] != string.Empty)
            {
                dtChequeDate = DateTime.ParseExact(form["ChequeDate"], "dd/MMM/yyyy", provider);
            }
            groupReceiptDto.TransactionDate = dtTransactionDate;
            groupReceiptDto.ChequeDate = dtChequeDate;
            groupReceiptDto.AHID = Convert.ToInt32(form["AccountHeadId"]);
            groupReceiptDto.TransactionType = Convert.ToInt32(form["TransactionType"]);
            groupReceiptDto.TransactionMode = Convert.ToString(Request.Form["TransactionMode"]);
            groupReceiptDto.Amount = Convert.ToDecimal(form["Amount"]);
            groupReceiptDto.GroupBankAccountName = Convert.ToString(form["GroupBankAccountName"]);
            groupReceiptDto.GroupBankAccountNumber = Convert.ToString(form["GroupBankAccountNumber"]);
            groupReceiptDto.ChequeNumber = Convert.ToString(form["ChequeNumber"]);

            groupReceiptDto.VoucherRefNumber = Convert.ToString(form["VoucherRefNumber"]);

            if (form["FederationBankAccountNumber"] != null && form["FederationBankAccountNumber"] != string.Empty && form["FederationBankAccountNumber"] != "0")
            {
                groupReceiptDto.BankAccount = Convert.ToInt32(form["FederationBankAccountNumber"]);
            }
            groupReceiptDto.FederationBankAccountName = Convert.ToString(form["FederationBankAccountName"]);
            groupReceiptDto.Narration = Convert.ToString(form["Narration"]);
            groupReceiptDto.PartyName = Convert.ToString(form["PartyName"]);
            groupReceiptDto.UserID = UserInfo.UserID;

            int myIndex = Convert.ToInt32(form["index"]);

            groupReceiptDto.lstGroupReceiptTranscationDto = new List<ReceiptTranscationDto>();
            ReceiptTranscationDto receipt = null;

            //foreach (var rpt in groupReceiptDto.lstGroupReceiptTranscationDto)
            for (int i = 1; i <= myIndex; i++)
            {

                receipt = new ReceiptTranscationDto();
                receipt.AHID = Convert.ToInt32(form["AHID_" + i]);
                receipt.AHName = Convert.ToString(form["AHName_" + i]);
                receipt.OpeningBalance = Convert.ToDecimal(form["OpeningBalance_" + i]);
                receipt.CrAmount = Convert.ToDecimal(form["CrAmount_" + i]);
                receipt.IsMaster = false;
                groupReceiptDto.lstGroupReceiptTranscationDto.Add(receipt);

            }
            receipt = new ReceiptTranscationDto();
            receipt.AHID = groupReceiptDto.AHID;
            receipt.AHName = "";
            receipt.IsMaster = true;
            //receipt.OpeningBalance = Convert.ToDecimal(form["OpeningBalance_" + i]);
            receipt.CrAmount = groupReceiptDto.lstGroupReceiptTranscationDto.Sum(s => s.CrAmount);


            groupReceiptDto.lstGroupReceiptTranscationDto.Add(receipt);
            return groupReceiptDto;
        }
        private List<ReceiptTranscationDto> Accountheads()
        {
            List<ReceiptTranscationDto> lstGroupReceiptTranscationDto = _accountheadService.GetReceiptAccountHeads(true);//_groupReceiptService.GetAccountdetails();

            // Start Update the call For ReceiptTranscation Details 07/17/2016

            //ReceiptDll MemberReceiptDll = new ReceiptDll();
            //List<ReceiptTranscationDto> lstGroupReceiptTranscationDto = MemberReceiptDll.();
            return lstGroupReceiptTranscationDto;

            //End Update the call for ReceiptTranscation Details 07/17/2016
        }

        public ActionResult GroupReceiptLookUp()
        {
            var lstGroupreceiptDto = _groupReceiptService.GroupReceiptLookup();
            return View(lstGroupreceiptDto);

        }
        public ActionResult BindDropDowns(string flag, int Id)
        {
            StringBuilder sbOptions = new StringBuilder();
            if (flag == "Cluster")
            {
                List<SelectListDto> lstgroupDto = _groupService.GetGroupCodeByClusterID(Id);
                foreach (var item in lstgroupDto)
                {
                    sbOptions.Append("<option value=" + item.ID + ">" + item.Text + "</option>");
                }
            }
            return Content(sbOptions.ToString());
        }
        public JsonResult GetSelectName(int id)
        {
            GroupMasterDto groupMasterDto = _groupService.GetByID(id);

            return Json(new { GroupName = groupMasterDto.GroupName });
        }

        public JsonResult GetFAccountName(int id)
        {
            BankMasterDto objBank = _bankService.GetByID(id);
            return Json(new { FaccountName = objBank.BName, AHID = objBank.AHID, AccountName = objBank.AHName, AccountCode = objBank.AHCode });
        }
        public JsonResult GetAHID(string Id)
        {
            string isfed = Request.Form["isfederation"];
            bool isFederation = Convert.ToBoolean(isfed);

            AccountHeadDto objAccountHead = _accountheadService.GetCashInHandAccount(isFederation);
            return Json(new { AccountName = objAccountHead.AHName, AHID = objAccountHead.AHID, AHCode = objAccountHead.AHCode });
        }
        public ActionResult ViewBalanceSummary(int ahId, bool isFederation)
        {
            AccountHeadDto accountHeadDto = _accountheadService.GetAccountHeadViewBalanceSummary(ahId, isFederation);
            return Json(new { ClosingBalance = accountHeadDto.ClosingBalance, BalanceType = accountHeadDto.BalanceType });
        }
        [HttpGet]
        public ActionResult DeleteGroupreceipts(string Id)
        {
            int AccountmasterId = DecryptQueryString(Id);

            if (AccountmasterId < 1)
                return RedirectToAction("GroupReceiptLookUp");

            ResultDto resultDto = _groupReceiptService.Delete(AccountmasterId, UserInfo.UserID);

            TempData["Result"] = resultDto;

            return RedirectToAction("GroupReceiptLookUp");
        }

        [HttpGet]
        public ActionResult ActiveInactiveGroupreceipts(string Id)
        {
            int AccountmasterId = DecryptQueryString(Id);

            if (AccountmasterId < 1)
                return RedirectToAction("GroupReceiptLookUp");

            ResultDto resultDto = _groupReceiptService.ChangeStatus(AccountmasterId, UserInfo.UserID);

            TempData["Result"] = resultDto;

            return RedirectToAction("GroupReceiptLookUp");
        }


        public ActionResult GetReceiptTemplate(int groupid, string transactiondate)
        {
            ReceiptDll objDll = new ReceiptDll();
            List<ReceiptTranscationDto> lstreceiptsHeads= objDll.GetGroupReceiptAccountdetails(groupid, transactiondate);
            ReceiptMasterDto objreceipt = new ReceiptMasterDto();
            objreceipt.lstGroupReceiptTranscationDto = lstreceiptsHeads;

            if (groupid> 0)
            {
               // objreceipt = _groupReceiptService.GetByID(groupid);
               // get repayments start
                List<RepaymentDto> list = new List<RepaymentDto>();
                list = _repaymentService.GetRepayment(groupid, transactiondate);
                objreceipt.lstRepeyment = list;
                //ViewBag.Repayments = list.Find(t => t.Amount);
                //ViewBag.Repayments = list;
                ///end
            }
            var receiptDto = GetAccountHeadClosingBalnces();
            ViewBag.CashInHandDetails = receiptDto;
            
            return View("_ReceiptTemplate",objreceipt);
        }
    }
}
