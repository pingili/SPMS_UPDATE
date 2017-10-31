using MFIS.Web.Areas.Federation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MFIS.Web.Controllers;
using Utilities;
using BusinessEntities;
//using BusinessLogic.Interface;
using BusinessLogic;
using System.Text;
using CoreComponents;

namespace MFIS.Web.Areas.Federation.Controllers.TransactionControllers
{
    public class FederationMeetingController : BaseController
    {
        private readonly MemberService _memberService;
        private readonly FederationMeetingService _federationMeetingService;
        private readonly VillageService _villageService;
        private readonly GroupService _groupService;

        public FederationMeetingController()
        {
            _memberService = new MemberService();
            _federationMeetingService = new FederationMeetingService();
            _villageService = new VillageService();
            _groupService = new GroupService();
        }
        [HttpGet]
        public ActionResult CreateFederationMeeting(string Id)
        {
            int FedMeetingId = string.IsNullOrEmpty(Id.DecryptString()) ? default(int) : Convert.ToInt32(Id.DecryptString());
            FederationMeetingDTO objfedmeetingDto = new FederationMeetingDTO();
            if (FedMeetingId > 0)
            {
                objfedmeetingDto = _federationMeetingService.GetByID(FedMeetingId);
            }

            if (objfedmeetingDto.lstFederationMemberDto == null || objfedmeetingDto.lstFederationMemberDto.Count() < 1)
            {
                List<MeetingMembersDTO> lstmemberDto = _memberService.GetMemberSelectList();
                objfedmeetingDto.lstFederationMemberDto = lstmemberDto;
            }

            List<SelectListDto> lstVillages = _villageService.GetVillageSelectList();
            SelectList slVillages = new SelectList(lstVillages, "ID", "Text");
            ViewBag.villages = slVillages;
            List<SelectListDto> lstGroupDto = _groupService.GetGroupsSelectList();
            SelectList lstgroup = new SelectList(lstGroupDto, "ID", "Text");
            ViewBag.GroupNames = lstgroup;

            SelectList Reason = GetDropDownListByMasterCode(Enums.RefMasterCodes.REASON);
            ViewBag.Reason = Reason;

            return View(objfedmeetingDto);
        }

        [HttpPost]
        public ActionResult CreateFederationMeeting(FederationMeetingDTO federation, FormCollection form)
        {
            List<SelectListDto> lstVillages = _villageService.GetVillageSelectList();
            SelectList slVillages = new SelectList(lstVillages, "ID", "Text");
            ViewBag.villages = slVillages;
            SelectList Reason = GetDropDownListByMasterCode(Enums.RefMasterCodes.REASON);
            ViewBag.Reason = Reason;

            List<MeetingMembersDTO> lstmemberDto = _memberService.GetMemberSelectList();
            ViewBag.members = lstmemberDto;
            bool IsAttand = false;

            string[] time1 = form["StartTime"].ToLower().Replace("am", "").Replace("pm", "").Split(':');
            string[] time2 = form["EndTime"].ToLower().Replace("am", "").Replace("pm", "").Split(':');

            int hour1 = int.Parse(time1[0]);
            if (form["StartTime"].ToLower().Contains("pm") && hour1 < 12)
                hour1 = hour1 + 12;

            int hour2 = int.Parse(time2[0]);
            if (form["EndTime"].ToLower().Contains("pm") && hour1 < 12)
                hour2 = hour2 + 12;

            federation.StartTime = new TimeSpan(hour1, int.Parse(time1[1]), 0);
            federation.EndTime = new TimeSpan(hour2, int.Parse(time2[1]), 0);
            federation.GroupId = Convert.ToInt32(form["ddlgroup"]);

            List<MeetingMembersDTO> obj = federation.lstFederationMemberDto;
            int maxIndex = Convert.ToInt32(form["hdnIndex"]);
            federation.lstFederationMemberDto = new List<MeetingMembersDTO>();
            MeetingMembersDTO members = null;
            for (int i = 1; federation.IsConducted && i <= maxIndex; i++)
            {
                if (form["hdnMemberID_" + i] == null) continue;

                members = new MeetingMembersDTO();
                members.MemberID = Convert.ToInt32(form["hdnMemberID_" + i]);
                members.MemberName = form["hdnMember_" + i];
                if (form["Checkmember_" + i] == "on")
                    members.IsAttended = true;
                federation.lstFederationMemberDto.Add(members);
            }

            var resultdto = new ResultDto();
            federation.IsConducted = !federation.IsConducted;
            federation.UserId = UserInfo.UserID;
            if (federation.FederationMeetingId == 0)
            {
                resultdto = _federationMeetingService.Insert(federation);
            }
            else
                resultdto = _federationMeetingService.Update(federation);
            ModelState.Clear();
            ViewBag.Result = resultdto;
            return RedirectToAction("FederationMeetingLookup");
        }
        public ActionResult FederationMeetingLookup()
        {
            var lstFedMeetingDto = _federationMeetingService.Lookup();
            return View(lstFedMeetingDto);
        }
        //public ActionResult BindDropDowns(string flag, int Id)
        //{
        //    StringBuilder sbOptions = new StringBuilder();
        //    if (flag == "Village")
        //    {
        //        List<SelectListDto> lstgroupsDto = _groupService.GetGroupByVillageID(Id);
        //        foreach (var item in lstgroupsDto)
        //        {
        //            sbOptions.Append("<option value=" + item.ID + ">" + item.Text + "</option>");
        //        }
        //    }
        //    return Content(sbOptions.ToString());

        //}
        //public JsonResult GetGroupName(int id)
        //{
        //    GroupMasterDto groupMasterDto = _groupService.GetByID(id);
        //    List<SelectListDto> memberlist = _memberService.GetMemberByGroupId(id);
        //    return Json(new { GroupName = groupMasterDto.GroupName, Members = memberlist });
        //}

        [HttpGet]
        public ActionResult DeleteFederationMeeting(string Id)
        {
            int fedmeetingId = DecryptQueryString(Id);

            if (fedmeetingId < 1)
                return RedirectToAction("FederationMeetingLookup");

            ResultDto resultDto = _federationMeetingService.Delete(fedmeetingId, UserInfo.UserID);

            TempData["Result"] = resultDto;

            return RedirectToAction("FederationMeetingLookup");
        }
        [HttpGet]
        public ActionResult LockFederationMeeting(string Id)
        {
            int fedmeetingId = DecryptQueryString(Id);

            if (fedmeetingId < 1)
                return RedirectToAction("FederationMeetingLookup");

            ResultDto resultDto = _federationMeetingService.Lock(fedmeetingId);

            TempData["Result"] = resultDto;
            return RedirectToAction("FederationMeetingLookup");
        }

        [HttpGet]
        public ActionResult ActiveInactiveFederationMeeting(string Id)
        {
            int fedmeetingId = DecryptQueryString(Id);

            if (fedmeetingId < 1)
                return RedirectToAction("FederationMeetingLookup");

            ResultDto resultDto = _federationMeetingService.ChangeStatus(fedmeetingId, UserInfo.UserID);

            TempData["Result"] = resultDto;

            return RedirectToAction("FederationMeetingLookup");
        }
    }
}
