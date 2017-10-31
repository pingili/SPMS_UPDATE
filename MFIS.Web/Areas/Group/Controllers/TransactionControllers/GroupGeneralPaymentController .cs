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
    public class GroupGeneralPaymentController : BaseController
    {
        #region Global Variables
        private readonly MasterService _masterService;
        private readonly  GroupGeneralPaymentsService _groupGeneralPayemntService;
        private readonly GroupOtherReceiptService _groupOtherReceiptService;
        public GroupGeneralPaymentController()
        {
            _masterService = new MasterService();
            _groupGeneralPayemntService = new GroupGeneralPaymentsService();
            _groupOtherReceiptService = new GroupOtherReceiptService();
        }
        #endregion Global Variables

        [HttpGet]
        public ActionResult AddGeneralPayment(string Id)
        {
            int accountMasterId = string.IsNullOrEmpty(Id.DecryptString()) ? default(int) : Convert.ToInt32(Id.DecryptString());
            GroupGeneralPaymentDto grpGeneralPaymentDto = new GroupGeneralPaymentDto();
            if (accountMasterId > 0)
            {
                grpGeneralPaymentDto = _groupGeneralPayemntService.GetGroupGeneralPaymentById(accountMasterId);
            }
            LoadOtherReceiptDropDowns();
            return View(grpGeneralPaymentDto);
        }

        public void LoadOtherReceiptDropDowns()
        {
            TypeQueryResult lst = _masterService.GetTypeQueryResult("GROUP_GOP_GL_HEADS");
            ViewBag.lstGLAcHeads = new SelectList(lst.OrderBy(a => a.Name), "Id", "Name");

            TypeQueryResult lstBankAh = _masterService.GetTypeQueryResult("GROUP_OR_BANK_AH", GroupInfo.GroupID.ToString());
            ViewBag.slBankAh = new SelectList(lstBankAh.OrderBy(a => a.Name), "Id", "Name");

            TypeQueryResult lstEmp = _masterService.GetTypeQueryResult("ACT_EMPLOYEES", GroupInfo.GroupID.ToString());
            ViewBag.slEmp = new SelectList(lstEmp, "Id", "Name", UserInfo.UserID);

            List<GroupMeetingDto> lstGroupMeetings = _groupOtherReceiptService.GetGroupOpenMeetingDates(GroupInfo.GroupID);
            ViewBag.MonthMeetings = new SelectList(lstGroupMeetings, "DisplayMeetingDate", "DisplayMeetingDate");
        }

        [HttpPost]
        public ActionResult AddGeneralPayment(FormCollection form)
        {
            GroupGeneralPaymentDto _groupGeneralPaymentDto = new GroupGeneralPaymentDto();
            try
            {
                _groupGeneralPaymentDto.AccountMasterID = Convert.ToInt32(Request.Form["AccountMasterID"]);
                _groupGeneralPaymentDto.TransactionMode = Convert.ToString(Request.Form["TransactionMode"]);

                string tranDate = _groupGeneralPaymentDto.TransactionMode == "C" ? Request.Form["TransactionDate"] : Request.Form["txtTransactionDate"];
                DateTime dtTranDate = tranDate.ConvertToDateTime();
                _groupGeneralPaymentDto.TransactionDate = dtTranDate;

                _groupGeneralPaymentDto.VoucherRefNumber = Convert.ToString(Request.Form["VoucherRefNumber"]);
                _groupGeneralPaymentDto.CollectionAgent = Convert.ToInt32(Request.Form["CollectionAgent"]);

                if (_groupGeneralPaymentDto.TransactionMode == "BC")
                {
                    _groupGeneralPaymentDto.ChequeNumber = Convert.ToString(Request.Form["ChequeNumber"]);
                    _groupGeneralPaymentDto.ChequeDate = Request.Form["ChequeDate"].ConvertToDateTime();
                }
                _groupGeneralPaymentDto.TotalAmount = Convert.ToDecimal(Request.Form["hdnTotalAmount"]);
                if (_groupGeneralPaymentDto.TransactionMode != "C")
                    _groupGeneralPaymentDto.BankEntryId = Convert.ToInt32(Request.Form["BankEntryId"]);

                _groupGeneralPaymentDto.Narration = Convert.ToString(Request.Form["Narration"]);
                ViewBag.LockStatus = GroupInfo.LockStatus;
                //Transactions Read
                _groupGeneralPaymentDto.TransactionsList = new List<GroupGeneralPaymentTranDto>();

                int maxCount = Convert.ToInt32(Request.Form["hdnMaxTranCount"]);

                GroupGeneralPaymentTranDto objTran = null;
                for (int index = 0; index <= maxCount; index++)
                {
                    if (Request.Form["hdnSLId_" + index] == null)
                        continue;

                    objTran = new GroupGeneralPaymentTranDto();
                    objTran.GLAccount = Convert.ToString(Request.Form["hdnGLAccount_" + index]);
                    objTran.SLAccount = Convert.ToString(Request.Form["hdnSLAccount_" + index]);
                    objTran.SLAccountId = Convert.ToInt32(Request.Form["hdnSLId_" + index]);
                    objTran.Amount = Convert.ToDecimal(Request.Form["hdnAmount_" + index]);

                    _groupGeneralPaymentDto.TransactionsList.Add(objTran);
                }

                //Save
                ResultDto resultDto = new ResultDto();
                int GroupId = GroupInfo.GroupID;
                resultDto = _groupGeneralPayemntService.AddUpdateGeneralPayment(_groupGeneralPaymentDto, UserInfo.UserID, GroupId);
                _groupGeneralPaymentDto.VoucherNumber = resultDto.ObjectCode;
                _groupGeneralPaymentDto.AccountMasterID = resultDto.ObjectId;

                LoadOtherReceiptDropDowns();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(_groupGeneralPaymentDto);
        }

        [HttpPost]
        public JsonResult GetGroupSubLedgerAccountHeadsByGLAHId(string glAHId)
        {
            var slAccountHeads = _groupOtherReceiptService.GetSLAccountHeads(int.Parse(glAHId));
            return Json(new { slAccountHeads = slAccountHeads });
        }
        
        [HttpGet]
        public ActionResult GeneralPaymentsLookup()
        {
            var lstGeneralPaymentsDto = _groupGeneralPayemntService.Lookup(GroupInfo.GroupID,UserInfo.UserID);

            return View(lstGeneralPaymentsDto);
        }

        public ActionResult DeleteGeneralPayments(string Id)
        {
            int AccountmasterId = DecryptQueryString(Id);

            if (AccountmasterId < 1)
                return RedirectToAction("GeneralPaymentsLookup");

            ResultDto resultDto = _groupGeneralPayemntService.Delete(AccountmasterId, 1);

            TempData["Result"] = resultDto;

            return RedirectToAction("GeneralPaymentsLookup");
        }

        public ActionResult ViewGeneralPayment(string id)
        {
            int accountMasterId = Convert.ToInt32(id.DecryptString());

            GroupGeneralPaymentDto grpGeneralPaymentDto = new GroupGeneralPaymentDto();
            if (accountMasterId > 0)
            {
                grpGeneralPaymentDto = _groupGeneralPayemntService.GetGroupGeneralPaymentById(accountMasterId);
            }

            if (accountMasterId < 1)
                return RedirectToAction("GeneralPaymentsLookup");

            return View(grpGeneralPaymentDto);
        }
    }
}