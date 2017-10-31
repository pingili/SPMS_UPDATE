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
using System.Globalization;


namespace MFIS.Web.Areas.Group.Controllers.TransactionControllers
{
    public class GroupMeetingController : BaseController
    {
        GroupMeetingDAL dal = new GroupMeetingDAL();
        private readonly VillageService _villageService;
        private readonly GroupService _groupService;
        private readonly MemberService _memberService;
        private readonly GroupMeetingService _groupmeetingService;
        public GroupMeetingController()
        {
            _villageService = new VillageService();
            _groupService = new GroupService();
            _memberService = new MemberService();
            _groupmeetingService = new GroupMeetingService();
        }

        [HttpGet]
        public ActionResult GroupMeetingLookup()
        {
            int GroupId = GroupInfo.GroupID;
            var lstGroupMeetingDto = dal.Lookup(GroupId);

            var OpenMeetings = lstGroupMeetingDto.Find(l => l.MeetingLockStatus.ToUpper() == "OPEN");

            ViewBag.OpenMeetings = OpenMeetings;

            return View(lstGroupMeetingDto);
        }

        [HttpGet]
        public ActionResult CreateGroupMeeting(string Id)
        {

            int GroupMeetingId = string.IsNullOrEmpty(Id.DecryptString()) ? default(int) : Convert.ToInt32(Id.DecryptString());
            GroupMeetingDto groupmeetingdto = new GroupMeetingDto();

            if (GroupMeetingId > 0)
            {
                groupmeetingdto = _groupmeetingService.GetByID(GroupMeetingId);
                groupmeetingdto.IsConducted = !groupmeetingdto.IsConducted;
            }

            if (groupmeetingdto.lstgroupMembersDto == null || groupmeetingdto.lstgroupMembersDto.Count() < 1)
            {
                var newMembers = _memberService.GetByGroupId(GroupInfo.GroupID);
                var members = new List<GroupMeetingMembersDto>();
                foreach (var newmember in newMembers)
                {
                    members.Add(new GroupMeetingMembersDto() { IsAttended = false, MemberID = newmember.MemberID, MemberName = newmember.MemberName });
                }
                groupmeetingdto.lstgroupMembersDto = members;
            }
            GroupMeetingDAL dal = new GroupMeetingDAL();
            GroupMeetingDto MeetngDateGroupMeetingDto = dal.GetDate(GroupInfo.GroupID);

            if (MeetngDateGroupMeetingDto != null)
            {
                System.Globalization.DateTimeFormatInfo mfi = new System.Globalization.DateTimeFormatInfo();
                string strMonthName = mfi.GetMonthName(MeetngDateGroupMeetingDto.Month).ToString();
                groupmeetingdto.Month = MeetngDateGroupMeetingDto.Month;
                groupmeetingdto.Year = MeetngDateGroupMeetingDto.Year;
                groupmeetingdto.GroupMeetingDay = MeetngDateGroupMeetingDto.GroupMeetingDay;
                groupmeetingdto.MonthName = strMonthName;
                int NoOfDays = DateTime.DaysInMonth(groupmeetingdto.Year, groupmeetingdto.Month);
                List<SelectListDto> lstDates = new List<SelectListDto>();
                SelectListDto dateSelectListDto = null;
                for (int i = 1; i <= NoOfDays; i++)
                {
                    dateSelectListDto = new SelectListDto();
                    dateSelectListDto.ID = i;
                    dateSelectListDto.Text = i.ToString();
                    lstDates.Add(dateSelectListDto);
                }
                int GroupMeetingDay = GroupMeetingId > 0 ? groupmeetingdto.MeetingDate.Day : groupmeetingdto.GroupMeetingDay;
                if (TempData["Result"] != null)
                {
                    ViewBag.Result = TempData["Result"];
                }
                SelectList slDates = new SelectList(lstDates, "ID", "Text", GroupMeetingDay);
                ViewBag.Dates = slDates;
            }

            SelectList Reason = GetDropDownListByMasterCode(Enums.RefMasterCodes.REASON);
            ViewBag.Reason = Reason;
            List<GroupMeetingDto> lstGroupMeeting = dal.GetMeetingInfoByGroupID(GroupInfo.GroupID);
            ViewBag.lstGroupInfo = lstGroupMeeting;
            return View(groupmeetingdto);
        }

        [HttpPost]
        public ActionResult CreateGroupMeeting(GroupMeetingDto objmeeting, FormCollection form)
        {
            SelectList Reason = GetDropDownListByMasterCode(Enums.RefMasterCodes.REASON);
            ViewBag.Reason = Reason;
            if (form["TransactionDate"].Trim() != string.Empty)
                objmeeting.TransactionDate = Convert.ToDateTime(form["TransactionDate"]);
            string MeetingDate = form["MeetingYearMonth"] + "-" + form["MeetingDay"];
            objmeeting.MeetingDate = Convert.ToDateTime(MeetingDate);
            objmeeting.IsConducted = !objmeeting.IsConducted;
            objmeeting.GroupID = GroupInfo.GroupID;
            objmeeting.UserId = UserInfo.UserID;
            int maxIndex = Convert.ToInt32(form["hdnIndex"]);
            objmeeting.lstgroupMembersDto = new List<GroupMeetingMembersDto>();
            GroupMeetingMembersDto members = null;

            for (int i = 1; objmeeting.IsConducted && i <= maxIndex; i++)
            {
                if (form["hdnMemberID_" + i] == null) continue;

                members = new GroupMeetingMembersDto();
                members.MemberID = Convert.ToInt32(form["hdnMemberID_" + i]);
                members.MemberName = form["hdnMember_" + i];
                if (form["Checkmember_" + i] == "on")
                    members.IsAttended = true;
                objmeeting.lstgroupMembersDto.Add(members);
            }

            var resultDto = new ResultDto();
            objmeeting.GroupMeetingID = Convert.ToInt32(form["GroupMeetingID"]);

            if (objmeeting.GroupMeetingID == 0)
                resultDto = _groupmeetingService.Insert(objmeeting);
            else
                resultDto = _groupmeetingService.Update(objmeeting);

            UpdateGroupInfoSessionbyGroupId(GroupInfo.GroupID);

            TempData["Result"] = resultDto;
            GroupMeetingDto MeetngDateGroupMeetingDto = dal.GetDate(GroupInfo.GroupID);

            if (MeetngDateGroupMeetingDto != null)
            {
                System.Globalization.DateTimeFormatInfo mfi = new System.Globalization.DateTimeFormatInfo();
                string strMonthName = mfi.GetMonthName(MeetngDateGroupMeetingDto.Month).ToString();
                objmeeting.Month = MeetngDateGroupMeetingDto.Month;
                objmeeting.Year = MeetngDateGroupMeetingDto.Year;
                objmeeting.GroupMeetingDay = MeetngDateGroupMeetingDto.GroupMeetingDay;
                objmeeting.MonthName = strMonthName;
                int NoOfDays = DateTime.DaysInMonth(objmeeting.Year, objmeeting.Month);
                List<SelectListDto> lstDates = new List<SelectListDto>();
                SelectListDto dateSelectListDto = null;
                for (int i = 1; i <= NoOfDays; i++)
                {
                    dateSelectListDto = new SelectListDto();
                    dateSelectListDto.ID = i;
                    dateSelectListDto.Text = i.ToString();
                    lstDates.Add(dateSelectListDto);
                }
                SelectList slDates = new SelectList(lstDates, "ID", "Text", objmeeting.GroupMeetingDay);
                ViewBag.Dates = slDates;
                List<GroupMeetingDto> lstGroupMeeting = dal.GetMeetingInfoByGroupID(GroupInfo.GroupID);
                ViewBag.lstGroupInfo = lstGroupMeeting;

            }
            return RedirectToAction("GroupMeetingLookup");

        }

        [HttpGet]
        public ActionResult DeleteGroupMeeting(string Id)
        {
            int groupmeetingId = DecryptQueryString(Id);

            if (groupmeetingId < 1)
                return RedirectToAction("GroupMeetingLookup");

            ResultDto resultDto = dal.Delete(groupmeetingId, UserInfo.UserID);

            UpdateGroupInfoSessionbyGroupId(GroupInfo.GroupID);

            TempData["Result"] = resultDto;

            return RedirectToAction("CreateGroupMeeting");
        }

        [HttpGet]
        public ActionResult ActiveInactiveGroupMeeting(string Id)
        {
            int groupmeetingId = DecryptQueryString(Id);

            if (groupmeetingId < 1)
                return RedirectToAction("GroupMeetingLookup");

            ResultDto resultDto = _groupmeetingService.ChangeStatus(groupmeetingId, UserInfo.UserID);

            UpdateGroupInfoSessionbyGroupId(GroupInfo.GroupID);

            TempData["Result"] = resultDto;

            return RedirectToAction("GroupMeetingLookup");
        }

        public ActionResult GroupMeetingView(string Id, string type)
        {
            int groupMeetingId = string.IsNullOrEmpty(Id.DecryptString()) ? default(int) : Convert.ToInt32(Id.DecryptString());
            List<GroupMeetingViewDto> lstGroupMeetings = _groupmeetingService.GetGroupMeetingsView(groupMeetingId);

            DateTimeFormatInfo mfi = new DateTimeFormatInfo();
            ViewBag.MeetingHeader = string.Format("Group Meeting {0} {1}",
                    mfi.GetMonthName(lstGroupMeetings[0].MeetingDate.Month).ToString(),
                    lstGroupMeetings[0].MeetingDate.Year
                    );

            ViewBag.PageType = type;

            return View(lstGroupMeetings);

            /*GroupMeetingDto groupmeetingdto = new GroupMeetingDto();

            if (groupMeetingId > 0)
            {
                groupmeetingdto = _groupmeetingService.GetByID(groupMeetingId);
                groupmeetingdto.IsConducted = !groupmeetingdto.IsConducted;
            }

            if (groupmeetingdto.lstgroupMembersDto == null || groupmeetingdto.lstgroupMembersDto.Count() < 1)
            {
                var newMembers = _memberService.GetByGroupId(GroupInfo.GroupID);
                var members = new List<GroupMeetingMembersDto>();
                foreach (var newmember in newMembers)
                {
                    members.Add(new GroupMeetingMembersDto() { IsAttended = false, MemberID = newmember.MemberID, MemberName = newmember.MemberName });
                }
                groupmeetingdto.lstgroupMembersDto = members;
            }
            GroupMeetingDAL dal = new GroupMeetingDAL();
            GroupMeetingDto MeetngDateGroupMeetingDto = dal.GetDate(GroupInfo.GroupID);

            if (MeetngDateGroupMeetingDto != null)
            {
                System.Globalization.DateTimeFormatInfo mfi = new System.Globalization.DateTimeFormatInfo();
                string strMonthName = mfi.GetMonthName(MeetngDateGroupMeetingDto.Month).ToString();
                groupmeetingdto.Month = MeetngDateGroupMeetingDto.Month;
                groupmeetingdto.Year = MeetngDateGroupMeetingDto.Year;
                groupmeetingdto.GroupMeetingDay = MeetngDateGroupMeetingDto.GroupMeetingDay;
                groupmeetingdto.MonthName = strMonthName;
                int NoOfDays = DateTime.DaysInMonth(groupmeetingdto.Year, groupmeetingdto.Month);
                List<SelectListDto> lstDates = new List<SelectListDto>();
                SelectListDto dateSelectListDto = null;
                for (int i = 1; i <= NoOfDays; i++)
                {
                    dateSelectListDto = new SelectListDto();
                    dateSelectListDto.ID = i;
                    dateSelectListDto.Text = i.ToString();
                    lstDates.Add(dateSelectListDto);
                }
                int GroupMeetingDay = groupMeetingId > 0 ? groupmeetingdto.MeetingDate.Day : groupmeetingdto.GroupMeetingDay;
                if (TempData["Result"] != null)
                {
                    ViewBag.Result = TempData["Result"];
                }
                SelectList slDates = new SelectList(lstDates, "ID", "Text", GroupMeetingDay);
                ViewBag.Dates = slDates;
            }

            SelectList Reason = GetDropDownListByMasterCode(Enums.RefMasterCodes.REASON);
            ViewBag.Reason = Reason;
            List<GroupMeetingDto> lstGroupMeeting = dal.GetMeetingInfoByGroupID(GroupInfo.GroupID);
            ViewBag.lstGroupInfo = lstGroupMeeting;
            return View(groupmeetingdto);*/
        }
        public ActionResult LockGroupMeeting()
        {
            dal.LockMeeting(GroupInfo.GroupID);

            UpdateGroupInfoSessionbyGroupId(GroupInfo.GroupID);

            return RedirectToAction("GroupMeetingLookup");
        }
        public ActionResult GroupMeetingLock()
        {
            dal.GroupMeeitngLock(GroupInfo.GroupID, UserInfo.UserID);
            UpdateGroupInfoSessionbyGroupId(GroupInfo.GroupID);
            ResultDto resultDto = new ResultDto();
            resultDto.ObjectId = 1;
            resultDto.Message = "All open group meetings are locked successfully";
            TempData["Result"] = resultDto;
            return RedirectToAction("GroupMeetingLookup");
        }
    }

}
