using BusinessLogic.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MFIS.Web.Controllers
{
    public class MemberConfirmationReportController : BaseController
    {
        [HttpGet]
        public ActionResult MemberConfirmationReport()
        {
            ViewBag.GroupName = GroupInfo.GroupName + "(" + GroupInfo.GroupCode + ")";

            List<DateTime> lstGroupMeetings = objBal.GetGroupMeetings(GroupInfo.GroupID);
            //List<SelectListDto> lstselectDto = _accountheadService.GetGeneralReceiptLedgersDropDown(true);
            SelectList lstGroupMeeting = new SelectList(lstGroupMeetings, "Date", "Date");
            ViewBag.GroupMeeting = lstGroupMeeting;
            var lstMemberConfirmations = objBal.GetMemberConfirmationReport(GroupInfo.GroupID, UserInfo.UserID, DateTime.MinValue,DateTime.Now);
            return View(lstMemberConfirmations);
        }
        [HttpPost]
        public ActionResult MemberConfirmationReport(DateTime groupmeetingdate)
        {
            ViewBag.GroupName = GroupInfo.GroupName + "(" + GroupInfo.GroupCode + ")";

            List<DateTime> lstGroupMeetings = objBal.GetGroupMeetings(GroupInfo.GroupID);
            //List<SelectListDto> lstselectDto = _accountheadService.GetGeneralReceiptLedgersDropDown(true);
            SelectList lstGroupMeeting = new SelectList(lstGroupMeetings, "Date", "Date");
            ViewBag.GroupMeeting = lstGroupMeeting;
            var lstMemberConfirmations = objBal.GetMemberConfirmationReport(GroupInfo.GroupID, UserInfo.UserID, DateTime.MinValue,groupmeetingdate);
            return View(lstMemberConfirmations);
        }

        [HttpGet]
        public ActionResult MemberDemandSheet()
        {
            ViewBag.GroupName = GroupInfo.GroupName + "(" + GroupInfo.GroupCode + ")";

            List<DateTime> lstGroupMeetings = objBal.GetGroupMeetings(GroupInfo.GroupID);
            //List<SelectListDto> lstselectDto = _accountheadService.GetGeneralReceiptLedgersDropDown(true);
            SelectList lstGroupMeeting = new SelectList(lstGroupMeetings, "Date", "Date");
            ViewBag.GroupMeeting = lstGroupMeeting;
            var lstMemberDemand = objBal.GetMemberDemandSheetReport(GroupInfo.GroupID, UserInfo.UserID, DateTime.MinValue);

            return View(lstMemberDemand);
        }

        public MemberConfirmationDemandService objBal
        {
            get
            {
                return new MemberConfirmationDemandService();
            }
        }

    }
}
