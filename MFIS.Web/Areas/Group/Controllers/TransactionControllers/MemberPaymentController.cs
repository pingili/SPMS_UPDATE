using BusinessEntities;
using BusinessLogic;
using BusinessLogic.Implementation;
using CoreComponents;
using MFIS.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace MFIS.Web.Areas.Group.Controllers.TransactionControllers
{
    public class MemberPaymentController : BaseController
    {
        #region Global Variables
        private readonly MasterService _masterService;
        private readonly GroupMemberPaymentService _groupMemberPayemntService;
        private readonly GroupOtherReceiptService _groupOtherReceiptService;
        public MemberPaymentController()
        {
            _masterService = new MasterService();
            _groupMemberPayemntService = new GroupMemberPaymentService();
            _groupOtherReceiptService = new GroupOtherReceiptService();
        }
        #endregion Global Variables

        [HttpGet]
        public ActionResult AddMemberPayment(string Id)
        {
            int accountMasterId = string.IsNullOrEmpty(Id.DecryptString()) ? default(int) : Convert.ToInt32(Id.DecryptString());
            GroupMemberPaymentDto grpMemberPaymentDto = new GroupMemberPaymentDto();
            if (accountMasterId > 0)
            {
                grpMemberPaymentDto = _groupMemberPayemntService.GetGroupMemberPaymentById(accountMasterId);
            }

            LoadMemberPaymentDropDowns();
            
            return View(grpMemberPaymentDto);
        }

        private void LoadMemberPaymentDropDowns()
        {
            TypeQueryResult lst = _masterService.GetTypeQueryResult("GROUP_GMP_GL_HEADS");
            ViewBag.lstGLAcHeads = new SelectList(lst.OrderBy(a => a.Name), "Id", "Name");

            TypeQueryResult lstBankAh = _masterService.GetTypeQueryResult("GROUP_OR_BANK_AH", GroupInfo.GroupID.ToString());
            ViewBag.slBankAh = new SelectList(lstBankAh.OrderBy(a => a.Name), "Id", "Name");

            TypeQueryResult lstEmp = _masterService.GetTypeQueryResult("ACT_EMPLOYEES", GroupInfo.GroupID.ToString());
            ViewBag.slEmp = new SelectList(lstEmp, "Id", "Name", UserInfo.UserID);

            GroupMemberReceiptService _groupMemberReceiptService = new GroupMemberReceiptService();
            List<MemberDto> lstMembers = _groupMemberReceiptService.GetMembersByGroupId(GroupInfo.GroupID);
            ViewBag.grpMembers = new SelectList(lstMembers, "MemberId", "MemberName", "Select Member");

            List<GroupMeetingDto> lstGroupMeetings = _groupOtherReceiptService.GetGroupOpenMeetingDates(GroupInfo.GroupID);
            ViewBag.MonthMeetings = new SelectList(lstGroupMeetings, "DisplayMeetingDate", "DisplayMeetingDate");
        }

        [HttpPost]
        public ActionResult AddMemberPayment(FormCollection form)
        {
            GroupMemberPaymentDto _groupMemberPaymentDto = new GroupMemberPaymentDto();
            try
            {
                _groupMemberPaymentDto.MemberId = Convert.ToInt32(Request.Form["MemberId"]);
                _groupMemberPaymentDto.AccountMasterID = Convert.ToInt32(Request.Form["AccountMasterID"]);
                _groupMemberPaymentDto.TransactionMode = Convert.ToString(Request.Form["TransactionMode"]);

                string tranDate = _groupMemberPaymentDto.TransactionMode == "C" ? Request.Form["TransactionDate"] : Request.Form["txtTransactionDate"];
                DateTime dtTranDate = tranDate.ConvertToDateTime();
                _groupMemberPaymentDto.TransactionDate = dtTranDate;

                _groupMemberPaymentDto.VoucherRefNumber = Convert.ToString(Request.Form["VoucherRefNumber"]);
                _groupMemberPaymentDto.CollectionAgent = Convert.ToInt32(Request.Form["CollectionAgent"]);

                if (_groupMemberPaymentDto.TransactionMode == "BC")
                {
                    _groupMemberPaymentDto.ChequeNumber = Convert.ToString(Request.Form["ChequeNumber"]);
                    _groupMemberPaymentDto.ChequeDate = Request.Form["ChequeDate"].ConvertToDateTime();
                }
                _groupMemberPaymentDto.TotalAmount = Convert.ToDecimal(Request.Form["hdnTotalAmount"]);
                if (_groupMemberPaymentDto.TransactionMode != "C")
                    _groupMemberPaymentDto.BankEntryId = Convert.ToInt32(Request.Form["BankEntryId"]);

                _groupMemberPaymentDto.Narration = Convert.ToString(Request.Form["Narration"]);

                //Transactions Read
                _groupMemberPaymentDto.TransactionsList = new List<GroupMemberPaymentTranDto>();

                int maxCount = Convert.ToInt32(Request.Form["hdnMaxTranCount"]);

                GroupMemberPaymentTranDto objTran = null;
                for (int index = 0; index <= maxCount; index++)
                {
                    if (Request.Form["hdnSLId_" + index] == null)
                        continue;

                    objTran = new GroupMemberPaymentTranDto();
                    objTran.GLAccount = Convert.ToString(Request.Form["hdnGLAccount_" + index]);
                    objTran.SLAccount = Convert.ToString(Request.Form["hdnSLAccount_" + index]);
                    objTran.SLAccountId = Convert.ToInt32(Request.Form["hdnSLId_" + index]);
                    objTran.Amount = Convert.ToDecimal(Request.Form["hdnAmount_" + index]);

                    _groupMemberPaymentDto.TransactionsList.Add(objTran);
                }

                //Save
                ResultDto resultDto = new ResultDto();
                int GroupId = GroupInfo.GroupID;
                resultDto = _groupMemberPayemntService.AddUpdateMemberPayment(_groupMemberPaymentDto, UserInfo.UserID, GroupId);
                _groupMemberPaymentDto.VoucherNumber = resultDto.ObjectCode;
                _groupMemberPaymentDto.AccountMasterID = resultDto.ObjectId;

                LoadMemberPaymentDropDowns();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(_groupMemberPaymentDto);
        }

        [HttpPost]
        public JsonResult GetGroupSubLedgerAccountHeadsByGLAHId(string glAHId)
        {
            var slAccountHeads = _groupOtherReceiptService.GetSLAccountHeads(int.Parse(glAHId));
            return Json(new { slAccountHeads = slAccountHeads });
        }

        [HttpGet]
        public ActionResult MemberPaymentsLookup()
        {
            var lstGeneralPaymentsDto = _groupMemberPayemntService.GetGroupMemberPaymentLookup(GroupInfo.GroupID,UserInfo.UserID);

            return View(lstGeneralPaymentsDto);
        }

        public ActionResult DeleteMemberPayments(string Id)
        {
            int AccountmasterId = DecryptQueryString(Id);

            if (AccountmasterId < 1)
                return RedirectToAction("MemberPaymentsLookup");

            ResultDto resultDto = new GroupGeneralPaymentsService().Delete(AccountmasterId, UserInfo.UserID);

            TempData["Result"] = resultDto;

            return RedirectToAction("MemberPaymentsLookup");
        }

        public ActionResult ViewMemberPayment(string id)
        {
            int accountMasterId = Convert.ToInt32(id.DecryptString());

            GroupMemberPaymentDto grpMemberPaymentDto = new GroupMemberPaymentDto();
            if (accountMasterId > 0)
            {
                grpMemberPaymentDto = _groupMemberPayemntService.GetGroupMemberPaymentById(accountMasterId);
            }

            if (accountMasterId < 1)
                return RedirectToAction("GeneralPaymentsLookup");

            return View(grpMemberPaymentDto);
        }
    }
}