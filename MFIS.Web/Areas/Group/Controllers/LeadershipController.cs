using AutoMapper;
using BusinessEntities;
using BusinessLogic;
//using BusinessLogic.Interface;
using MFIS.Web.Areas.Group.Models;
using MFIS.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Utilities;
using CoreComponents;

namespace MFIS.Web.Areas.Group.Controllers
{
    public class LeadershipController : BaseController
    {
        #region Global Variables

        private readonly ClusterService _clusterService;
        private readonly BranchService _branchService;
        private readonly GroupService _groupService;
        private readonly MemberService _memberService;
        private readonly LeadershipService _leadershipService;

        public LeadershipController()
        {

            _clusterService = new ClusterService();
            _branchService = new BranchService();
            _groupService = new GroupService();
            _memberService = new MemberService();
            _leadershipService = new LeadershipService();
        }

        #endregion Global Variables
        [HttpGet]
        public ActionResult CreateLeadership(string id)
        {
            int LeadershipId = string.IsNullOrEmpty(id.DecryptString()) ? default(int) : Convert.ToInt32(id.DecryptString());
         
            LeadershipModel lmodel = new LeadershipModel();
            LeadershipDto dto = new LeadershipDto();
            if(LeadershipId>0)          
                dto = _leadershipService.GetByID(LeadershipId);
            lmodel = Mapper.Map<LeadershipDto, LeadershipModel>(dto);
       
            //lmodel.ClusterID = clusterId;
            
            LoadDropDowns(lmodel.BranchID, lmodel.ClusterID, lmodel.GroupID);

            return View(lmodel);
        }


        public void LoadDropDowns(int branchid, int clusterid, int groupid)
        {

            SelectList leadershiptitle = GetDropDownListByMasterCode(Enums.RefMasterCodes.LEADERSHIP_TITLE);
            ViewBag.leadershiptitle = leadershiptitle;

           

            List<SelectListDto> lstselectBranch = _branchService.GetBranchSelectList();
            SelectList lstbranchs = new SelectList(lstselectBranch, "ID", "Text");
            ViewBag.BranchNames = lstbranchs;


            List<SelectListDto> clusterSelectList = _clusterService.GetClusterSelectListByBranchID(branchid);
            SelectList lStClusters = new SelectList(clusterSelectList, "ID", "Text");
            ViewBag.ClusterNames = lStClusters;


            List<SelectListDto> groupSelectList = _groupService.GetGroupByClusterID(clusterid);
            SelectList lstGroupNames = new SelectList(groupSelectList, "ID", "Text");
            ViewBag.GroupNames = lstGroupNames;

            List<SelectListDto> memberSelectList = _memberService.GetMemberByGroupId(groupid);
            SelectList lstMemberNames = new SelectList(memberSelectList, "ID", "Text");
            ViewBag.MemberNames = lstMemberNames;

            List<SelectListDto> lstselectLeadershipLevel = _leadershipService.GetLeadershiplevelSelectList();
            SelectList lstLevels = new SelectList(lstselectLeadershipLevel, "ID", "Text");
            ViewBag.LeadershipLevels = lstLevels;


        }

        [HttpPost]
        public ActionResult CreateLeadership(LeadershipModel leaderModel)
        {
            LeadershipModel lmodel = new LeadershipModel();
            LoadDropDowns(lmodel.BranchID, lmodel.ClusterID, lmodel.GroupID);
             var resultDto = new ResultDto();
             if (ModelState.IsValid)
             {
                 var leadershipDto = Mapper.Map<LeadershipModel, LeadershipDto>(leaderModel);
                 leadershipDto.UserID = UserInfo.UserID;
                 if (leadershipDto.LeadershipID == 0)
                     resultDto = _leadershipService.Insert(leadershipDto);
                 else
                     resultDto = _leadershipService.Update(leadershipDto);

                  if (resultDto.ObjectId > 0)
                {
                    leadershipDto = _leadershipService.GetByID(resultDto.ObjectId);
                    leaderModel = Mapper.Map<LeadershipDto, LeadershipModel>(leadershipDto);
                    resultDto.ObjectId = leaderModel.LeadershipID;
                }
            }
             ViewBag.Result = resultDto;
           
             return View(leaderModel);
        }
        public ActionResult BindDropDowns(string flag, int Id)
        {

            StringBuilder sbOptions = new StringBuilder();
            if (flag == "Branch")
            {
                List<SelectListDto> lstClusterDto = _clusterService.GetClusterSelectListByBranchID(Id);
                foreach (var item in lstClusterDto)
                {
                    sbOptions.Append("<option value=" + item.ID + ">" + item.Text + "</option>");
                }
            }
            else if (flag == "Cluster")
            {

                List<SelectListDto> lstgrouplDto = _groupService.GetGroupByClusterID(Id);
                foreach (var item in lstgrouplDto)

                    sbOptions.Append("<option value=" + item.ID + ">" + item.Text + "</option>");
            }
            else if (flag == "Group")
            {

                List<SelectListDto> lstmemberDto = _memberService.GetMemberByGroupId(Id);
                foreach (var item in lstmemberDto)

                    sbOptions.Append("<option value=" + item.ID + ">" + item.Text + "</option>");
            }
            return Content(sbOptions.ToString());
        }

        [HttpGet]
        public ActionResult LeadershipLookup()
        {
            var lstLeadershipDto = _leadershipService.GetLookup();
            return View("LeadershipLookup", lstLeadershipDto);
        }
        public ActionResult DeleteLeadership(string Id)
        {
            int leadershipId = DecryptQueryString(Id);

            if (leadershipId < 1)
                return RedirectToAction("LeadershipLookup");

            ResultDto resultDto = _leadershipService.Delete(leadershipId, UserInfo.UserID);

            TempData["Result"] = resultDto;

            return RedirectToAction("LeadershipLookup");
        }
        public ActionResult ActiveInactiveLeadership(string Id)
        {
            int leadershipId = DecryptQueryString(Id);

            if (leadershipId < 1)
                return RedirectToAction("LeadershipLookup");

            ResultDto resultDto = _leadershipService.ChangeStatus(leadershipId, UserInfo.UserID);

            TempData["Result"] = resultDto;

            return RedirectToAction("LeadershipLookup");
        }


    }
}
