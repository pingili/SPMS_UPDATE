using BusinessEntities;
using BusinessLogic.Implementation;
using CoreComponents;
using DataLogic.Implementation;
using MFIS.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MFIS.Web.Areas.Group.Controllers.TransactionControllers
{
    public class GroupMemberReceiptController : BaseController
    {
        #region Global Variables
        private readonly GroupMemberReceiptService _groupMemberReceiptService;
        private readonly MasterService _masterService;
        private readonly GroupMemberReceiptDto _grpMbrRecptDto;
        private readonly GroupMemberReceiptDal _grpMbrRcptDal;
        private readonly GroupOtherReceiptService _groupOtherReceiptService;
        public GroupMemberReceiptController()
        {
            _groupMemberReceiptService = new GroupMemberReceiptService();
            _masterService = new MasterService();
            _groupOtherReceiptService = new GroupOtherReceiptService();
            _grpMbrRecptDto = new GroupMemberReceiptDto();
            _grpMbrRcptDal = new GroupMemberReceiptDal();
        }
        #endregion Global Variables
        // GET: /Group/MemberReceipt1/
        [HttpGet]
        public ActionResult CreateGroupMemberReceipt(string Id)
        {
            GroupMemberReceiptDto grpMemberReceiptDto = new GroupMemberReceiptDto();
            int AccountMasterId = string.IsNullOrEmpty(Id.DecryptString()) ? default(int) : Convert.ToInt32(Id.DecryptString());
            if (AccountMasterId > 0)
            {
                //edit
                grpMemberReceiptDto = _groupMemberReceiptService.EditGroupMemberReceipt(AccountMasterId);
                ViewBag.AccountHeadsTeplate = grpMemberReceiptDto.Transactions;
            }
            else
            {
                //add
                List<GroupMemberReceiptTranDto> lstgrpmbrrptTemplate = new List<GroupMemberReceiptTranDto>();
                lstgrpmbrrptTemplate = _groupMemberReceiptService.GetMemberReceiptTemplate();
                ViewBag.AccountHeadsTeplate = lstgrpmbrrptTemplate;
            }
           
            LoadMemberReceiptDropDowns();

            return View(grpMemberReceiptDto);
        }
        [HttpPost]
        public ActionResult CreateGroupMemberReceipt(FormCollection form)
        {
            GroupMemberReceiptDto _grpMbrRecptDto = new GroupMemberReceiptDto();
            try
            {
                ViewBag.LockStatus = GroupInfo.LockStatus;
                _grpMbrRecptDto.AccountMasterID = Convert.ToInt32(Request.Form["AccountMasterID"]);
                _grpMbrRecptDto.TransactionMode = Convert.ToString(Request.Form["TransactionMode"]);

                string tranDate = _grpMbrRecptDto.TransactionMode == "C" ? Request.Form["TransactionDate"] : Request.Form["txtTransactionDate"];
                DateTime dtTranDate = tranDate.ConvertToDateTime();
                _grpMbrRecptDto.TransactionDate = dtTranDate;

                _grpMbrRecptDto.VoucherRefNumber = Convert.ToString(Request.Form["VoucherRefNumber"]);
                _grpMbrRecptDto.CollectionAgent = Convert.ToInt32(Request.Form["CollectionAgent"]);

                if (_grpMbrRecptDto.TransactionMode == "BC")
                {
                    _grpMbrRecptDto.ChequeNumber = Convert.ToString(Request.Form["ChequeNumber"]);
                    _grpMbrRecptDto.ChequeDate = Request.Form["ChequeDate"].ConvertToDateTime();
                }
                _grpMbrRecptDto.TotalAmount = Convert.ToDecimal(Request.Form["hdnTotalAmount"]);
                if (_grpMbrRecptDto.TransactionMode != "C")
                    _grpMbrRecptDto.BankEntryId = Convert.ToInt32(Request.Form["BankEntryId"]);
                _grpMbrRecptDto.MemberId = Convert.ToInt32(Request.Form["MemberId"]);
                _grpMbrRecptDto.Narration = Convert.ToString(Request.Form["Narration"]);

                //Transactions Read
                _grpMbrRecptDto.Transactions = new List<GroupMemberReceiptTranDto>();

                int maxCount = Convert.ToInt32(Request.Form["hdnMaxTranCount"]);
                GroupMemberReceiptTranDto objTran = null;
                for (int index = 1; index < maxCount; index++)
                {
                    objTran = new GroupMemberReceiptTranDto();
                    objTran.SLAccountId = Convert.ToInt32(Request.Form["hdnSlAccountID_" + index]);
                    objTran.GLAccount = Convert.ToString(Request.Form["hdnGlAccount_" + index]);
                    objTran.SLAccount = Convert.ToString(Request.Form["hdnSlAccount_" + index]);
                    objTran.ReferenceNumber = Convert.ToString(Request.Form["hdnRefNumber_" + index]);
                    decimal expectedDemand = default(decimal);
                    decimal.TryParse(Request.Form["hdnexpectedamount_" + index], out expectedDemand);
                    int subAhId = default(int);
                    int.TryParse(Request.Form["hdnSubAHID_" + index], out subAhId);
                    objTran.SubAhId = subAhId;

                    objTran.Amount = Convert.ToDecimal(Request.Form["CrAmount_" + index]);
                    if (objTran.SLAccount.ToUpper().Contains("SERVICE COST") && objTran.Amount < expectedDemand)
                    {
                        objTran.InterestDue = expectedDemand - objTran.Amount;
                    }
                    if (objTran.SLAccount.ToUpper().Contains("LOAN") && objTran.Amount < expectedDemand && !(objTran.SLAccount.ToUpper().Contains("SERVICE COST")))
                    {
                        objTran.PrincipleDue = expectedDemand - objTran.Amount;
                    }
                    _grpMbrRecptDto.Transactions.Add(objTran);
                }

                //Save
                int GroupId = GroupInfo.GroupID;
                ResultDto resultDto = _groupMemberReceiptService.AddUpdateMemberReceipt(_grpMbrRecptDto, UserInfo.UserID, GroupId);
                _grpMbrRecptDto.VoucherNumber = resultDto.ObjectCode;
                _grpMbrRecptDto.AccountMasterID = resultDto.ObjectId;

                LoadMemberReceiptDropDowns();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(_grpMbrRecptDto);
        }

        public void LoadMemberReceiptDropDowns()
        {
            TypeQueryResult lstBankAh = _masterService.GetTypeQueryResult("GROUP_OR_BANK_AH", GroupInfo.GroupID.ToString());
            ViewBag.slBankAh = new SelectList(lstBankAh.OrderBy(a => a.Name), "Id", "Name");

            TypeQueryResult lstEmp = _masterService.GetTypeQueryResult("ACT_EMPLOYEES", GroupInfo.GroupID.ToString());
            ViewBag.slEmp = new SelectList(lstEmp, "Id", "Name", UserInfo.UserID);

            List<MemberDto> lstMembers = _groupMemberReceiptService.GetMembersByGroupId(GroupInfo.GroupID);
            ViewBag.grpMembers = new SelectList(lstMembers, "MemberId", "MemberName", "Select Member");

            List<GroupMeetingDto> lstGroupMeetings = _groupOtherReceiptService.GetGroupOpenMeetingDates(GroupInfo.GroupID);
            ViewBag.MonthMeetings = new SelectList(lstGroupMeetings, "DisplayMeetingDate", "DisplayMeetingDate");
        }

        public ActionResult GroupMemberReceiptLookUp()
        {
            ViewBag.LockStatus = GroupInfo.LockStatus;
            List<GroupMemberReceiptLookupDto> lstdto = _groupMemberReceiptService.GroupMemberReceiptLookUp(GroupInfo.GroupID,UserInfo.UserID);
            return View(lstdto);
        }

        public JsonResult GetMemberReceiptDemands(int memberid, string transactiondate)
        {
            List<GroupMemberDemandDto> lstDemands = new List<GroupMemberDemandDto>();
            lstDemands = _groupMemberReceiptService.GetTransactionDemands(transactiondate, memberid);
            var Totaldemand = lstDemands.Sum(l => l.Demand);
            return Json(new { demands = lstDemands, totaldemands = Totaldemand });
        }

        [HttpGet]
        public ActionResult DeleteMemberReceipts(string Id)
        {
            int AccountmasterId = DecryptQueryString(Id);

            if (AccountmasterId < 1)
                return RedirectToAction("GroupMemberReceiptLookUp");

            ResultDto resultDto = _groupMemberReceiptService.Delete(AccountmasterId, UserInfo.UserID);

            TempData["Result"] = resultDto;

            return RedirectToAction("GroupMemberReceiptLookUp");
        }

        public ActionResult MemberReceiptView(string Id)
        {
            GroupMemberReceiptViewDto grpMemberReceiptDto = new GroupMemberReceiptViewDto();
            int AccountMasterId = string.IsNullOrEmpty(Id.DecryptString()) ? default(int) : Convert.ToInt32(Id.DecryptString());
            grpMemberReceiptDto = _groupMemberReceiptService.GetGroupMemberReceiptViewDetails(AccountMasterId);
            return View(grpMemberReceiptDto);
        }
    }
}
