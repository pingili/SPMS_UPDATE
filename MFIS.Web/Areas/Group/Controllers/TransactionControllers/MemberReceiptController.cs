using BusinessEntities;
using BusinessLogic;
using CoreComponents;
using DataLogic;
using MFIS.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MFIS.Web.Areas.Group.Controllers.TransactionControllers
{
    public class MemberReceiptController : BaseController
    {

        #region Global Variables
        private readonly GroupService _groupService;
        private readonly ReferenceValueService _referenceValueService;
        private readonly BankService _bankService;
        private readonly GroupReceiptService _groupReceiptService;
        private readonly EmployeeService _employeeService;
        private readonly MemberService _memberservice;
        private readonly MemberReceiptService _memberReceiptService;
        private readonly RepaymentService _repaymentService;
        private readonly AccountHeadService _accountheadService;
        private readonly ContraEntryService _ContraEntryService;
        public MemberReceiptController()
        {
            _groupService = new GroupService();
            _referenceValueService = new ReferenceValueService();
            _bankService = new BankService();
            _groupReceiptService = new GroupReceiptService();
            _employeeService = new EmployeeService();
            _memberservice = new MemberService();
            _memberReceiptService = new MemberReceiptService();
            _repaymentService = new RepaymentService();
            _accountheadService = new AccountHeadService();
            _ContraEntryService = new ContraEntryService();
        }
        #endregion Global VariablesMemberCodes
        [HttpGet]
        public ActionResult CreateMemberReceipt(string id)
        {

            int AccountMasterID = string.IsNullOrEmpty(id.DecryptString()) ? default(int) : Convert.ToInt32(id.DecryptString());

            ViewBag.MID = AccountMasterID;
            ReceiptMasterDto objreceipt = new ReceiptMasterDto();

            //objreceipt.lstGroupReceiptTranscationDto = Accountheads();

            //objreceipt.lstGroupReceiptTranscationDto = Accountheads();

            if (AccountMasterID > 0)
            {
                objreceipt = _memberReceiptService.GetByID(AccountMasterID);

                List<ReceiptTranscationDto> lstTransactionDto = new List<ReceiptTranscationDto>();
                lstTransactionDto= objreceipt.lstGroupReceiptTranscationDto;

                ReceiptDll objDll = new ReceiptDll();
                List<ReceiptTranscationDto> lstreceiptsHeads = objDll.GetMemberReceiptAccountdetails(objreceipt.MemberId.Value, DateTime.Now.ToDisplayDateFormat());
                objreceipt.lstGroupReceiptTranscationDto = lstreceiptsHeads;
                foreach (var tranHead in lstTransactionDto)
                {
                    if (objreceipt.lstGroupReceiptTranscationDto.Exists(l => l.AHID == tranHead.AHID))
                    {
                        objreceipt.lstGroupReceiptTranscationDto.Find(l => l.AHID == tranHead.AHID).CrAmount = tranHead.CrAmount;
                        objreceipt.lstGroupReceiptTranscationDto.Find(l => l.AHID == tranHead.AHID).DrAmount = tranHead.DrAmount;
                    }
                }
                objreceipt.CrTotal = objreceipt.lstGroupReceiptTranscationDto.FindAll(l => l.IsMaster == false).Sum(l => l.CrAmount + l.DrAmount);
                objreceipt.DrTotal = objreceipt.CrTotal;
            }
            LoadDropDowns();
            EmployeeDto obj = _employeeService.GetByID(UserInfo.UserID);

            //meeting date changes
            GroupMeetingService gms = new GroupMeetingService();
            List<GroupMeetingLookupDto> list = gms.Lookup(GroupInfo.GroupID);
            if (list.Count > 0)
            {
                DateTime meetingdate = list.Max(l => l.MeetingDate);
                ViewBag.MeetingDate = meetingdate;
                AccountHeadDto objAccountHead = _accountheadService.GetCashInHandAccount(false);
                ViewBag.CashInHandDetails = objAccountHead;

                //var receiptDto = GetAccountHeadClosingBalnces();
                //ViewBag.CashInHandDetails = receiptDto;

                objreceipt.EmployeeName = obj.EmployeeName;
                objreceipt.EmployeeCode = obj.EmployeeCode;
                objreceipt.EmployeeID = obj.EmployeeID;
                objreceipt.TransactionDate = meetingdate;
                //LoadDropDowns();
                return View(objreceipt);
            }
            else
            {
                throw new Exception("There is no meeting date for this group please create meeting");
               
            }

        }

        private ReceiptTranscationDto GetAccountHeadClosingBalnces()
        {
            ReceiptTranscationDto objReceiptTranscationDto = new ReceiptTranscationDto();
            objReceiptTranscationDto = _groupReceiptService.GetAccountHeadClosingBalnces();
            return objReceiptTranscationDto;
        }
        [HttpPost]
        public ActionResult CreateMemberReceipt(FormCollection form)
        {
            var groupReceiptdto = ReadFormData(form);
            var resultDto = new ResultDto();

            //var receiptDto = GetAccountHeadClosingBalnces();
            //ViewBag.CashInHandDetails = receiptDto;


            AccountHeadDto objAccountHead = _accountheadService.GetCashInHandAccount(false);
            int ahId = objAccountHead.AHID;
            //AccountHeadDto closeingBalance = _accountheadService.GetAccountHeadViewBalanceSummary(ahId, false, groupId);

            // objAccountHead.ClosingBalance = closeingBalance.ClosingBalance;
            ViewBag.CashInHandDetails = objAccountHead;

            LoadDropDowns();
            if (groupReceiptdto.AccountMasterID == 0)
            {
                //resultDto = _memberReceiptService.Insert(groupReceiptdto);
                resultDto = _memberReceiptService.Insert(groupReceiptdto);

            }
            else
            {
                resultDto = _memberReceiptService.Update(groupReceiptdto);
            }
            ViewBag.Result = resultDto;
            return View(groupReceiptdto);

        }
        private ReceiptMasterDto ReadFormData(FormCollection form)
        {
            ReceiptMasterDto groupReceiptDto = new ReceiptMasterDto();
            int accountMasterId = default(int);
            int.TryParse(form["AccountMasterID"], out accountMasterId);

            groupReceiptDto.IsGroup = true;
            groupReceiptDto.SubHeadID = 1;
            groupReceiptDto.AccountMasterID = accountMasterId;
            groupReceiptDto.MemberId = Convert.ToInt32(form["MemberId"]);
            groupReceiptDto.VoucherRefNumber = Convert.ToString(form["VoucherRefNumber"]);
            //groupReceiptDto.GroupID = Convert.ToInt32(form["GroupID"]);
            groupReceiptDto.EmployeeID = UserInfo.UserID;
            //groupReceiptDto.EmployeeCode = UserInfo.e ;
            //groupReceiptDto.EmployeeName = Convert.ToString(form["EmployeeName"]);
            groupReceiptDto.TransactionDate = Convert.ToDateTime(form["TransactionDate"]);
            groupReceiptDto.TransactionType = Convert.ToInt32(form["TransactionType"]);
            groupReceiptDto.TransactionMode = Convert.ToString(Request.Form["TransactionMode"]);
            groupReceiptDto.Amount = Convert.ToDecimal(form["Amount"]);
            if (form["GroupBankAccountNumber"] != null && form["GroupBankAccountNumber"] != string.Empty && form["GroupBankAccountNumber"] != "0")
            {
                groupReceiptDto.BankAccount = Convert.ToInt32(form["GroupBankAccountNumber"]);
            }
            if (groupReceiptDto.TransactionMode == "B")
            {
                groupReceiptDto.GroupBankAccountName = Convert.ToString(form["GroupBankAccountName"]);
                groupReceiptDto.ChequeNumber = Convert.ToString(form["ChequeNumber"]);
                groupReceiptDto.ChequeDate = Convert.ToDateTime(form["ChequeDate"]);
            }
            groupReceiptDto.AHID = Convert.ToInt32(form["hdnCashInHandAHID"]);
            groupReceiptDto.ReferenceNumber = Convert.ToString(form["ReferenceNumber"]);
            groupReceiptDto.FederationBankAccountNumber = Convert.ToString(form["FederationBankAccountNumber"]);
            groupReceiptDto.FederationBankAccountName = Convert.ToString(form["FederationBankAccountName"]);
            groupReceiptDto.Narration = Convert.ToString(form["Narration"]);
            groupReceiptDto.PartyName = Convert.ToString(form["PartyName"]);
            groupReceiptDto.UserID = UserInfo.UserID;
            groupReceiptDto.GroupID = GroupInfo.GroupID;

            int myIndex = Convert.ToInt32(form["index"]);

            groupReceiptDto.lstGroupReceiptTranscationDto = new List<ReceiptTranscationDto>();
            ReceiptTranscationDto receipt = null;

            //foreach (var rpt in groupReceiptDto.lstGroupReceiptTranscationDto)
            for (int i = 1; i <= myIndex; i++)
            {
                receipt = new ReceiptTranscationDto();

                receipt.CrAmount = Convert.ToDecimal(form["CrAmount_" + i]);
                if (receipt.CrAmount <= 0)
                    continue;

                receipt.AHID = Convert.ToInt32(form["AHID_" + i]);
                receipt.AHName = Convert.ToString(form["AHName_" + i]);
                //receipt.OpeningBalance = Convert.ToDecimal(form["OpeningBalance_" + i]);
                receipt.IsMaster = false;
                groupReceiptDto.lstGroupReceiptTranscationDto.Add(receipt);
            }
            receipt = new ReceiptTranscationDto();
            receipt.AHID = groupReceiptDto.AHID;
            receipt.AHName = "";
            receipt.IsMaster = true;
            //receipt.OpeningBalance = Convert.ToDecimal(form["OpeningBalance_" + i]);
            receipt.DrAmount = groupReceiptDto.lstGroupReceiptTranscationDto.Sum(s => s.CrAmount);
            groupReceiptDto.lstGroupReceiptTranscationDto.Add(receipt);

            return groupReceiptDto;
        }
        private void LoadDropDowns()
        {
            ReceiptMasterDto objreceipt = new ReceiptMasterDto();
            MemberDto memberdto = new MemberDto();
            int GroupId = GroupInfo.GroupID;
            List<MemberLookupDto> lstmemberlookupDto = _memberservice.LookUp(GroupId);

            //SelectList lstmemberCodes = new SelectList(lstmemberlookupDto, "MemberId", "MemberCode", memberdto.MemberID);
            SelectList lstmemberCodes = new SelectList(lstmemberlookupDto, "MemberId", "MemberName", memberdto.MemberID);
            ViewBag.MemberCodes = lstmemberCodes;

            int groupId = GroupInfo.GroupID;
            BankMasterViewDto objBank = new BankMasterViewDto();
            List<BankMasterViewDto> lstAllBanks = _ContraEntryService.GetAllGroupBanksByGroupId(groupId);
            SelectList lstgroupBanks = new SelectList(lstAllBanks, "BankEntryID", "AccountNumber", objBank.BankEntryID);
            ViewBag.GroupBanks = lstgroupBanks;

            //BankMasterViewDto objBank = new BankMasterViewDto();
            //List<BankMasterViewDto> lstAllBanks = _groupReceiptService.GetGroupBanks();
            //SelectList lstgroupBanks = new SelectList(lstAllBanks, "AHID", "AccountNumber", objBank.BankEntryID);
            //ViewBag.GroupBanks = lstgroupBanks;
        }
        private List<ReceiptTranscationDto> Accountheads()
        {
            List<ReceiptTranscationDto> lstGroupReceiptTranscationDto = _memberReceiptService.GetAccountdetails();

            // Start Update the call For ReceiptTranscation Details 07/17/2016

            //ReceiptDll MemberReceiptDll = new ReceiptDll();
            //List<ReceiptTranscationDto> lstGroupReceiptTranscationDto = MemberReceiptDll.GetMemberReceiptAccountdetails();    
            return lstGroupReceiptTranscationDto;

            //End Update the call for ReceiptTranscation Details 07/17/2016
        }

        public ActionResult MemberReceiptLookup()
        {
            int GroupId = GroupInfo.GroupID;
            var lstMemberReceiptDto = _memberReceiptService.MemberReceiptLookup( GroupId);
            return View(lstMemberReceiptDto);
        }

        [HttpGet]
        public ActionResult DeleteMemberReceipts(string Id)
        {
            int AccountmasterId = DecryptQueryString(Id);

            if (AccountmasterId < 1)
                return RedirectToAction("MemberReceiptLookUp");

            ResultDto resultDto = _memberReceiptService.Delete(AccountmasterId, UserInfo.UserID);

            TempData["Result"] = resultDto;

            return RedirectToAction("MemberReceiptLookUp");
        }

        [HttpGet]
        public ActionResult ActiveInactiveMembereceipts(string Id)
        {
            int AccountmasterId = DecryptQueryString(Id);

            if (AccountmasterId < 1)
                return RedirectToAction("MemberReceiptLookUp");

            ResultDto resultDto = _memberReceiptService.ChangeStatus(AccountmasterId, UserInfo.UserID);

            TempData["Result"] = resultDto;

            return RedirectToAction("MemberReceiptLookUp");
        }



        public JsonResult GetAccountName(int id)
        {
            MemberDto objmember = _memberservice.GetById(id);

            //return Json(new { membername = objmember.MemberName,memberId=objmember.MemberID });
            return Json(new { MemberCode = objmember.MemberCode, memberId = objmember.MemberID });

        }
        public JsonResult GetName(int id)
        {
            BankMasterDto objBank = _bankService.GetByID(id);
            return Json(new { AccountName = objBank.BName });
        }

        public ActionResult GetReceiptTemplate(int memberid, string transactiondate)
        {
            ReceiptDll objDll = new ReceiptDll();
            List<ReceiptTranscationDto> lstreceiptsHeads = objDll.GetMemberReceiptAccountdetails(memberid, transactiondate);

            ReceiptMasterDto objreceipt = new ReceiptMasterDto();
            objreceipt.lstGroupReceiptTranscationDto = lstreceiptsHeads;
            if (memberid > 0)
            {
                List<RepaymentDto> list = new List<RepaymentDto>();
                list = _repaymentService.GetRepayment(memberid, transactiondate);
                objreceipt.lstRepeyment = list;
            }
            List<ReceiptTranscationDto> lstDto = new List<ReceiptTranscationDto>();

            AccountHeadDto objAccountHead = _accountheadService.GetCashInHandAccount(false);
            ViewBag.CashInHandDetails = objAccountHead;

            return View("_ReceiptTemplate", objreceipt);
        }

        public ActionResult ViewBalanceSummary(int ahId, bool isFederation)
        {
            AccountHeadDto accountHeadDto = _accountheadService.GetAccountHeadViewBalanceSummary(ahId, isFederation);
            return Json(new { ClosingBalance = accountHeadDto.ClosingBalance, BalanceType = accountHeadDto.BalanceType });
        }
    }
}
