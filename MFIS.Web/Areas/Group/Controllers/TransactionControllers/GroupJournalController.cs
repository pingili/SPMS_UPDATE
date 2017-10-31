using BusinessEntities;
using BusinessLogic;
//using BusinessLogic.Interface;
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
using DataLogic.Implementation;
using BusinessLogic.Implementation;

namespace MFIS.Web.Areas.Group.Controllers.TransactionControllers
{
    public class GroupJournalController : BaseController
    {
        #region Global Variable
        private readonly MasterService _masterService;
        private readonly GroupJournalService _groupOtherJournalService;
        public GroupJournalController()
        {
            _masterService = new MasterService();
            _groupOtherJournalService = new GroupJournalService();
        }
        #endregion Global Variable

        [HttpGet]
        public ActionResult CreateGroupJournal(string type, string Id)
        {
            bool isMemberJournal = type != null && type.ToUpper() == "M";
            int accountMasterId = string.IsNullOrEmpty(Id.DecryptString()) ? default(int) : Convert.ToInt32(Id.DecryptString());
            GroupJournalDto groupJournalDto = new GroupJournalDto();
            ViewBag.EmpName = UserInfo.UserName;
            if (accountMasterId > 0)
            {
                groupJournalDto = _groupOtherJournalService.GetGroupJournalDetailsByID(accountMasterId);
                ViewBag.EmpName = groupJournalDto.EmployeeName;
            }
            LoadOtherJournalDropDowns(type);
            ViewBag.IsMemberJournal = isMemberJournal;
            return View(groupJournalDto);
        }

        [HttpPost]
        public ActionResult CreateGroupJournal(GroupJournalDto _groupOtherJournalDto)
        {
            try
            {
                //dto.EnmployeeName = dto.EnmployeeName;
                //dto.FAmount = dto.FAmount;
                //dto.FSLAccountId = dto.FSLAccountId;
                //dto.Narration = dto.Narration;
                //dto.OnBehalfOf = dto.OnBehalfOf;
                //dto.VocherRefNumber = dto.VocherRefNumber;
                bool isMemberJournal = false;
                int memberId = default(int);
                int.TryParse(Request.Form["MemberId"], out memberId);
                if (memberId > 0)
                {
                    _groupOtherJournalDto.MemberId = memberId;
                    isMemberJournal = true;
                }

                _groupOtherJournalDto.CrDr = Request.Form["FAmountMode"];
                DateTime dtTranDate = Request.Form["TransactionDate"].ConvertToDateTime();
                _groupOtherJournalDto.TransactionDate = dtTranDate;

                _groupOtherJournalDto.TransactionsList = new List<GroupJournalTranDto>();
                int maxCount = Convert.ToInt32(Request.Form["hdnMaxcount"]);
                GroupJournalTranDto objtrn = null;
                for (int i = 0; i <= maxCount; i++)
                {
                    if (Request.Form["hdnSLId_" + i] == null)
                        continue;
                    objtrn = new GroupJournalTranDto();
                    objtrn.GLAccount = Convert.ToString(Request.Form["hdnGLAccount_" + i]);
                    objtrn.SLAccount = Convert.ToString(Request.Form["hdnSLAccount_" + i]);
                    objtrn.SLAccountId = Convert.ToInt32(Request.Form["hdnSLId_" + i]);
                    objtrn.GLAccountId = Convert.ToInt32(Request.Form["hdnGLId_" + i]);
                    objtrn.Amount = Convert.ToDecimal(Request.Form["hdnAmount_" + i]);
                    _groupOtherJournalDto.TransactionsList.Add(objtrn);
                }

                ResultDto resultDto = new ResultDto();

                int GroupId = GroupInfo.GroupID;

                resultDto = _groupOtherJournalService.AddGroupOtherJournal(_groupOtherJournalDto, GroupId, UserInfo.UserID);

                _groupOtherJournalDto.VoucherNumber = resultDto.ObjectCode;
                _groupOtherJournalDto.AccountMasterID = resultDto.ObjectId;

                ViewBag.IsMemberJournal = isMemberJournal;
                LoadOtherJournalDropDowns(isMemberJournal ? "M" : "O");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(_groupOtherJournalDto);
        }

        [HttpGet]
        public ActionResult GroupJournalLookUp(string type)
        {
            bool isMemberJournal = type.ToUpper() == "M";
            var lstGroupJournalDto = _groupOtherJournalService.GetGroupJournalLookup(GroupInfo.GroupID,UserInfo.UserID, isMemberJournal);
            ViewBag.IsMemberJournal = isMemberJournal;
            return View(lstGroupJournalDto);
        }
        public void LoadOtherJournalDropDowns(string type)
        {
            TypeQueryResult lstFrom = _masterService.GetTypeQueryResult(type == "M" ? "GROUP_GMJ_FROM_GL_HEADS" : "GROUP_GOJ_FROM_GL_HEADS");
            ViewBag.lstFromGLAcHeads = new SelectList(lstFrom.OrderBy(a => a.Name), "Id", "Name");

            TypeQueryResult lstTo = _masterService.GetTypeQueryResult(type == "M" ? "GROUP_GMJ_TO_GL_HEADS" : "GROUP_GOJ_TO_GL_HEADS");
            ViewBag.lstToGLAcHeads = new SelectList(lstTo.OrderBy(a => a.Name), "Id", "Name");

            TypeQueryResult lstEmp = _masterService.GetTypeQueryResult("ACT_EMPLOYEES", GroupInfo.GroupID.ToString());
            ViewBag.slEmp = new SelectList(lstEmp, "Id", "Name", UserInfo.UserID);

            GroupMemberReceiptService objMrService = new GroupMemberReceiptService();
            List<MemberDto> lstMembers = objMrService.GetMembersByGroupId(GroupInfo.GroupID);
            ViewBag.grpMembers = new SelectList(lstMembers, "MemberId", "MemberName", "Select Member");
        }

        [HttpPost]
        public JsonResult GetGroupSubLedgerAccountHeadsByGLAHId(string glAHId)
        {
            try
            {
                var slAccountHeads = _groupOtherJournalService.GetSLAccountHeads(int.Parse(glAHId));
                return Json(new { slAccountHeads = slAccountHeads });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult DeleteJournal(string type, string Id)
        {
            int AccountmasterId = DecryptQueryString(Id);

            if (AccountmasterId < 1)
                return RedirectToAction("GroupJournalLookUp", new { type = type });

            GroupGeneralPaymentsService _groupGeneralPayemntService = new GroupGeneralPaymentsService();

            ResultDto resultDto = _groupGeneralPayemntService.Delete(AccountmasterId, UserInfo.UserID);

            TempData["Result"] = resultDto;

            return RedirectToAction("GroupJournalLookUp", new { type = type });
        }

        [HttpGet]
        public ActionResult ViewGroupJournal(string type, string Id)
        {
            bool isMemberJournal = type != null && type.ToUpper() == "M";
            int accountMasterId = string.IsNullOrEmpty(Id.DecryptString()) ? default(int) : Convert.ToInt32(Id.DecryptString());
            GroupJournalDto groupJournalDto = new GroupJournalDto();
            groupJournalDto = _groupOtherJournalService.GetGroupJournalDetailsByID(accountMasterId);
            ViewBag.IsMemberJournal = isMemberJournal;
            return View(groupJournalDto);
        }
    }

}
