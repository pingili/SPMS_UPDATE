using AutoMapper;
using BusinessEntities;
using BusinessLogic.AutoMapper;
using BusinessLogic;
//using BusinessLogic.Interface;
using MFIS.Web.Areas.Federation.Models;
using MFIS.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utilities;
using CoreComponents;
using System.Text;
using MFIEntityFrameWork;
using DataLogic.Implementation;

namespace MFIS.Web.Areas.Federation.Controllers
{
    public class GroupController : BaseController
    {
        #region Global Variables
        private readonly GroupService _groupService;
        private readonly BankService _bankService;
        private readonly VillageService _villageService;
        private readonly ClusterService _clusterService;
        private readonly InterestService _interestService;
        private readonly AccountHeadService _accountHeadService;
        private readonly PanchayatService _panchayatService;
        private readonly MemberService _memberService;
        private readonly CommonService _commonService;

        public GroupController()
        {
            _groupService = new GroupService();
            _bankService = new BankService();
            _villageService = new VillageService();
            _clusterService = new ClusterService();
            _interestService = new InterestService();
            _accountHeadService = new AccountHeadService();
            _panchayatService = new PanchayatService();
            _memberService = new MemberService();
            _commonService = new CommonService();
        }

        #endregion Global Variables

        [HttpGet]
        public ActionResult CreateGroup(int? id)
        {
            GroupModel groupModel = new GroupModel();
            List<SelectListDto> lstVillages = _villageService.GetVillageSelectList();
            SelectList slVillages = new SelectList(lstVillages, "ID", "Text");

            List<SelectListDto> lstClusters = _clusterService.GetClusterSelectList();
            SelectList slClusters = new SelectList(lstClusters, "ID", "Text");

            SelectList slMeetingFrrequency = GetDropDownListByMasterCode(Enums.RefMasterCodes.MEETING_FREQUENCY);

            List<SelectListDto> lstpanchayats = _panchayatService.GetPanchayatSelectList();
            SelectList slPanchayat = new SelectList(lstpanchayats, "ID", "Text");

            ViewBag.villages = slVillages;
            ViewBag.clusters = slClusters;
            ViewBag.panchayats = slPanchayat;

            ViewBag.MeetingFrrequency = slMeetingFrrequency;

            GroupMasterDto groupMasterDto = new GroupMasterDto();

            if (id.HasValue && id.Value > 0)
            {
                groupMasterDto = _groupService.GetByID(id.Value);

                groupModel = AutoMapperEntityConfiguration.Cast<GroupModel>(groupMasterDto);
            }
            LoadRegularSavingAccountHeadDropDown();
            ViewBag.Result = new ResultDto();
            return View("CreateGroup", groupModel);
        }

        [HttpPost]
        public ActionResult CreateGroup(GroupModel objGroupModel)
        {
            List<GroupMasterDto> lstObjGroup = new List<GroupMasterDto>();
            ResultDto resultDto = new ResultDto();

            int groupId = default(int);
            byte meetingDay = default(byte);

            int.TryParse(Request.Form["hdnObjectID"], out groupId);
            byte.TryParse(Request.Form["hdnMeetingDay"], out meetingDay);

            var groupMasterDTO = new GroupMasterDto()
            {
                GroupCode = objGroupModel.GroupCode,
                GroupID = groupId,
                GroupName = objGroupModel.GroupName,
                GroupRefNumber = objGroupModel.GroupRefNumber,
                TEGroupName = objGroupModel.TEGroupName,
                MeetingDay = meetingDay,
                MeetingEndTime = objGroupModel.MeetingEndTime,
                MeetingFrequency = objGroupModel.MeetingFrequency,
                MeetingStartTime = objGroupModel.MeetingStartTime,
                PanchayatID = objGroupModel.PanchayatID,
                ClusterID = objGroupModel.ClusterID,
                Phone = objGroupModel.Phone,
                FormationDate = objGroupModel.FormationDate,
                FederationTranStartDate = objGroupModel.FederationTranStartDate,
                DateOfClosure = objGroupModel.DateOfClosure,
                Email = objGroupModel.Email,
                Address = objGroupModel.Address,
                UserId = UserInfo.UserID,
                RegularSavingAmount = objGroupModel.RegularSavingAmount,
                RegularSavingsAhId = objGroupModel.RegularSavingsAhId
            };

            if (objGroupModel.GroupID == 0)
                resultDto = _groupService.Insert(groupMasterDTO);
            else
                resultDto = _groupService.Update(groupMasterDTO);

            return Json(new { groupid = resultDto.ObjectId, GroupCode = resultDto.ObjectCode, Message = resultDto.Message });
        }

        public ActionResult GroupLookUp()
        {
            var lstGroupLookupDto = _groupService.Lookup();
            return View(lstGroupLookupDto);
        }

        public void LoadRegularSavingAccountHeadDropDown()
        {
            var lstAccountHeads = _accountHeadService.GetAll(false);
            List<AccountHeadDto> lstAccountHeadDto = new List<AccountHeadDto>();

            var depositAH = lstAccountHeads.Find(l => l.AHCode == "DEPOSITS" && l.AHLevel == 2);//TODO:Sample Example Need to Change
            var depositSubAH = lstAccountHeads.FindAll(l => l.ParentAHID == depositAH.AHID && l.AHLevel == 3);
            foreach (var majorGroupAH in depositSubAH)
            {
                var subGroupAh = lstAccountHeads.FindAll(l => l.ParentAHID == majorGroupAH.AHID && l.AHLevel == 4);
                foreach (var sgAH in subGroupAh)
                {
                    var lstAH = lstAccountHeads.FindAll(l => l.ParentAHID == sgAH.AHID && l.AHLevel == 5);
                    foreach (var AH in lstAH)
                    {
                        lstAccountHeadDto.Add(new AccountHeadDto()
                        {
                            AHID = AH.AHID,
                            AHName = AH.AHName
                        });
                    }
                }
                var ahMglevel = lstAccountHeads.FindAll(l => l.ParentAHID == majorGroupAH.AHID && l.AHLevel == 5);

                foreach (var mgAH in ahMglevel)
                {
                    lstAccountHeadDto.Add(new AccountHeadDto()
                    {
                        AHID = mgAH.AHID,
                        AHName = mgAH.AHName
                    });
                }
            }

            SelectList RegSavingsAHSelectList = new SelectList(lstAccountHeadDto, "AHID", "AHName");
            ViewBag.RegularSavingAccountHeads = RegSavingsAHSelectList;
        }

        public ActionResult Member(string groupID)
        {
            int groupid = GroupInfo != null && GroupInfo.GroupID > 0 ? GroupInfo.GroupID : Convert.ToInt32(groupID);

            var memberDto = _memberService.GetByGroupId(groupid);

            ViewBag.Members = memberDto;

            return View(memberDto);
        }
        public ActionResult LeaderShip()
        {
            //long groupid = Convert.ToInt64(Session["GroupId"]);

            return View();
        }
        [HttpGet]
        public ActionResult CreateLoanInterestDetails(int? GroupID)
        {
            // int GroupId = Convert.ToInt32(Request.Form["hdnObjectID"]);
            var groupinterestDto = new InterestMasterDto();
            List<GroupInterestRatesDto> lstinterest = new List<GroupInterestRatesDto>();
            string type = "L";
            if (GroupID > 0)
            {

                lstinterest = _groupService.GetInterestByID(GroupID.Value, type);
                ViewBag.GroupInterestRates = lstinterest;
            }
            //InterestMasterModel interestModel = new InterestMasterModel();
            LoadLoanInterestDropDowns();
            InterestMasterDto interestDto = new InterestMasterDto();

            //List<SelectListDto> lstAccountHead = _accountHeadService.GetAccountHeadSelectList();
            //SelectList slstAccountHead = new SelectList(lstAccountHead, "ID", "Text");
            //ViewBag.AccountHead = slstAccountHead;
            //SelectList interestBaseType = GetDropDownListByMasterCode(Enums.RefMasterCodes.INTEREST_BASE_TYPE);
            //SelectList interestCalcType = GetDropDownListByMasterCode(Enums.RefMasterCodes.INTEREST_CALC_TYPE);
            //var interestCalType = new SelectList(interestCalcType, "", "", interestDto.InterestID);
            //ViewBag.InterestBaseType = interestBaseType;
            //ViewBag.InterestCalcType = interestCalcType;
            return View();
        }
        [HttpPost]
        public ActionResult CreateLoanInterestDetails(FormCollection Form)
        {
            int count = Convert.ToInt32(Request.Form["hdnMaxLoanIndex"]);
            int GroupId = Convert.ToInt32(Request.Form["hdnObjectID"]);
            string type = "L";

            List<GroupInterestRatesDto> lstobj = new List<GroupInterestRatesDto>();
            for (int i = 1; i <= count; i++)
            {
                GroupInterestRatesDto objGroupInterestdto = new GroupInterestRatesDto();
                if (!string.IsNullOrEmpty(Form["hdnloanentryId_" + i]))
                    objGroupInterestdto.GroupInterestID = Convert.ToInt32(Form["hdnloanentryId_" + i]);
                if (!string.IsNullOrEmpty(Form["hdnloanprincipleAChead_" + i]))
                {

                    objGroupInterestdto.InterestAHID = Convert.ToInt32(Form["hdnloanInterestACHead_" + i]);
                    objGroupInterestdto.PenalAHID = Convert.ToInt32(Form["hdnLoanpenalACHead_" + i]);
                    objGroupInterestdto.PrincipalAHID = Convert.ToInt32(Form["hdnloanprincipleAChead_" + i]);
                    objGroupInterestdto.Base = Convert.ToInt32(Form["hdnBase_" + i]);
                    objGroupInterestdto.CaluculationMethodId = Convert.ToInt32(Form["hdnCalMethod_" + i]);
                    objGroupInterestdto.ROI = Convert.ToDecimal(Form["hdnrateofint_" + i]);
                    objGroupInterestdto.PenalROI = Convert.ToDecimal(Form["hdnpenalrateofinterest_" + i]);
                    objGroupInterestdto.FromDate = Convert.ToDateTime(Form["hdnfromdate_" + i]);
                    objGroupInterestdto.ToDate = Convert.ToDateTime(Form["hdntodate_" + i]);
                    lstobj.Add(objGroupInterestdto);
                }


            }
            LoadLoanInterestDropDowns();
            ResultDto resultdto = _groupService.InsertDepositLoanRates(lstobj, GroupId, UserInfo.UserID, type);

            return Json(new { groupid = GroupId, ObjectCode = resultdto.ObjectCode, Message = resultdto.Message });
        }
        private void LoadLoanInterestDropDowns()
        {
            var lstAccountHeads = _accountHeadService.GetAll(false);
            List<AccountHeadDto> lstAccountHeadDto = new List<AccountHeadDto>();
            var memberloanAH = lstAccountHeads.Find(l => l.AHCode == "MEMBER LOANS OUTSTANDING" && l.AHLevel == 2);//TODO:Sample Example Need to Change
            var memberloanSubAH = lstAccountHeads.FindAll(l => l.ParentAHID == memberloanAH.AHID && l.AHLevel == 3);
            foreach (var majorGroupAH in memberloanSubAH)
            {
                var subGroupAh = lstAccountHeads.FindAll(l => l.ParentAHID == majorGroupAH.AHID && l.AHLevel == 4);
                foreach (var sgAH in subGroupAh)
                {
                    var lstAH = lstAccountHeads.FindAll(l => l.ParentAHID == sgAH.AHID && l.AHLevel == 5);
                    foreach (var AH in lstAH)
                    {
                        lstAccountHeadDto.Add(new AccountHeadDto()
                        {
                            AHID = AH.AHID,
                            AHName = AH.AHName
                        });
                    }
                }
                var ahMglevel = lstAccountHeads.FindAll(l => l.ParentAHID == majorGroupAH.AHID && l.AHLevel == 5);

                foreach (var mgAH in ahMglevel)
                {
                    lstAccountHeadDto.Add(new AccountHeadDto()
                    {
                        AHID = mgAH.AHID,
                        AHName = mgAH.AHName
                    });
                }
            }
            var lstAccountHeadsDto = lstAccountHeadDto.FindAll(l => l.IsFederation == false);
            SelectList principleAHSelectList = new SelectList(lstAccountHeadsDto, "AHID", "AHName");
            ViewBag.PrincipleAcHeads = principleAHSelectList;
            List<AccountHeadDto> lstInterstAccountHeadDto = new List<AccountHeadDto>();
            var incomeFromInterestAH = lstAccountHeads.Find(l => l.AHCode == "INCOME FROM INTEREST" && l.AHLevel == 2);//TODO:Sample Example Need to Change
            var incomeFromInterestSubAH = lstAccountHeads.FindAll(l => l.ParentAHID == incomeFromInterestAH.AHID && l.AHLevel == 3);
            foreach (var majorGroupAH in incomeFromInterestSubAH)
            {
                var subGroupAh = lstAccountHeads.FindAll(l => l.ParentAHID == majorGroupAH.AHID && l.AHLevel == 4);
                foreach (var sgAH in subGroupAh)
                {
                    var lstAH = lstAccountHeads.FindAll(l => l.ParentAHID == sgAH.AHID && l.AHLevel == 5);
                    foreach (var AH in lstAH)
                    {
                        lstInterstAccountHeadDto.Add(new AccountHeadDto()
                        {
                            AHID = AH.AHID,
                            AHName = AH.AHName
                        });
                    }
                }
                var ahMglevel = lstAccountHeads.FindAll(l => l.ParentAHID == majorGroupAH.AHID && l.AHLevel == 5);

                foreach (var mgAH in ahMglevel)
                {
                    lstInterstAccountHeadDto.Add(new AccountHeadDto()
                    {
                        AHID = mgAH.AHID,
                        AHName = mgAH.AHName
                    });
                }
            }
            var lstInterstAccountHead = lstInterstAccountHeadDto.FindAll(l => l.IsFederation == false && l.AHID != 372693 && l.AHID != 372694);
            SelectList interestAHSelectList = new SelectList(lstInterstAccountHead, "AHID", "AHName");
            ViewBag.InterestAcHeads = interestAHSelectList;
            List<SelectListDto> penalAHDto = _accountHeadService.GetGroupPenelLedgersDropDown(true);
            var accountHeadSelectListDto1 = _accountHeadService.GetAccountHeadSelectList();
            SelectList penalAHSelectList = new SelectList(penalAHDto, "ID", "Text");
            ViewBag.PenalAcHeads = penalAHSelectList;
            SelectList baseTypeSelectList = GetDropDownListByMasterCode(Enums.RefMasterCodes.INTEREST_BASE_TYPE);
            ViewBag.BaseTypes = baseTypeSelectList;
            SelectList calMethodSelectList = GetDropDownListByMasterCode(Enums.RefMasterCodes.INTEREST_CALC_TYPE);
            ViewBag.CalcTypes = calMethodSelectList;
        }
        [HttpGet]
        public ActionResult CreateDepositInterestDetails(int? GroupID)
        {
            // int GroupId = Convert.ToInt32(Request.Form["hdnObjectID"]);
            var groupinterestDto = new InterestMasterDto();
            List<GroupInterestRatesDto> lstinterest = new List<GroupInterestRatesDto>();
            string type = "D";
            if (GroupID > 0)
            {

                lstinterest = _groupService.GetInterestByID(GroupID.Value, type);
                ViewBag.GroupInterestRates = lstinterest;
            }
            //InterestMasterModel interestModel = new InterestMasterModel();
            InterestMasterDto interestDto = new InterestMasterDto();
            LoadDepositDropDowns();
            //List<SelectListDto> lstAccountHead = _accountHeadService.GetAccountHeadSelectList();
            //SelectList slstAccountHead = new SelectList(lstAccountHead, "ID", "Text");
            //ViewBag.AccountHead = slstAccountHead;
            //SelectList interestBaseType = GetDropDownListByMasterCode(Enums.RefMasterCodes.INTEREST_BASE_TYPE);
            //SelectList interestCalcType = GetDropDownListByMasterCode(Enums.RefMasterCodes.INTEREST_CALC_TYPE);
            //var interestCalType = new SelectList(interestCalcType, "", "", interestDto.InterestID);
            //ViewBag.InterestBaseType = interestBaseType;
            //ViewBag.InterestCalcType = interestCalcType;
            return View();

            ////InterestMasterModel interestModel = new InterestMasterModel();
            //InterestMasterDto interestDto = new InterestMasterDto();
            //List<SelectListDto> lstAccountHead = _accountHeadService.GetAccountHeadSelectList();
            //SelectList slstAccountHead = new SelectList(lstAccountHead, "ID", "Text");
            //ViewBag.AccountHead = slstAccountHead;
            //SelectList interestBaseType = GetDropDownListByMasterCode(Enums.RefMasterCodes.INTEREST_BASE_TYPE);
            //SelectList interestCalcType = GetDropDownListByMasterCode(Enums.RefMasterCodes.INTEREST_CALC_TYPE);
            //var interestCalType = new SelectList(interestCalcType, "", "", interestDto.InterestID);
            //ViewBag.InterestBaseType = interestBaseType;
            //ViewBag.InterestCalcType = interestCalcType;
            //return View();
        }
        [HttpPost]
        public ActionResult CreateDepositInterestDetails(FormCollection Form)
        {
            int count = Convert.ToInt32(Request.Form["hdnMaxDepIntIndex"]);
            int GroupId = Convert.ToInt32(Request.Form["hdnObjectID"]);
            string type = "D";

            List<GroupInterestRatesDto> lstobj = new List<GroupInterestRatesDto>();
            for (int i = 1; i <= count; i++)
            {
                GroupInterestRatesDto objGroupInterestdto = new GroupInterestRatesDto();

                if (!string.IsNullOrEmpty(Form["hdndepositentryId_" + i]))
                    objGroupInterestdto.GroupInterestID = Convert.ToInt32(Form["hdndepositentryId_" + i]);
                if (!string.IsNullOrEmpty(Form["hdndepositprincipleAChead_" + i]))
                {
                    objGroupInterestdto.InterestAHID = Convert.ToInt32(Form["hdndepositInterestACHead_" + i]);
                    objGroupInterestdto.PenalAHID = Convert.ToInt32(Form["hdndepositpenalACHead_" + i]);
                    objGroupInterestdto.PrincipalAHID = Convert.ToInt32(Form["hdndepositprincipleAChead_" + i]);
                    objGroupInterestdto.Base = Convert.ToInt32(Form["hdndepositBase_" + i]);
                    objGroupInterestdto.CaluculationMethodId = Convert.ToInt32(Form["hdndepositCalMethod_" + i]);
                    objGroupInterestdto.ROI = Convert.ToDecimal(Form["hdndepositrateofint_" + i]);
                    objGroupInterestdto.PenalROI = Convert.ToDecimal(Form["hdndepositpenalrateofinterest_" + i]);
                    objGroupInterestdto.FromDate = Convert.ToDateTime(Form["hdndepositfromdate_" + i]);
                    objGroupInterestdto.ToDate = Convert.ToDateTime(Form["hdndeposittodate_" + i]);
                    lstobj.Add(objGroupInterestdto);

                }
            }

            ResultDto resultdto = _groupService.InsertDepositLoanRates(lstobj, GroupId, UserInfo.UserID, type);
            LoadDepositDropDowns();

            return Json(new { groupid = GroupId, ObjectCode = resultdto.ObjectCode, Message = resultdto.Message });
        }
        private void LoadDepositDropDowns()
        {
            var lstAccountHeads = _accountHeadService.GetAll(false);
            List<AccountHeadDto> lstAccountHeadDto = new List<AccountHeadDto>();

            var depositAH = lstAccountHeads.Find(l => l.AHCode == "DEPOSITS" && l.AHLevel == 2);//TODO:Sample Example Need to Change
            var depositSubAH = lstAccountHeads.FindAll(l => l.ParentAHID == depositAH.AHID && l.AHLevel == 3);
            foreach (var majorGroupAH in depositSubAH)
            {
                var subGroupAh = lstAccountHeads.FindAll(l => l.ParentAHID == majorGroupAH.AHID && l.AHLevel == 4);
                foreach (var sgAH in subGroupAh)
                {
                    var lstAH = lstAccountHeads.FindAll(l => l.ParentAHID == sgAH.AHID && l.AHLevel == 5);
                    foreach (var AH in lstAH)
                    {
                        lstAccountHeadDto.Add(new AccountHeadDto()
                        {
                            AHID = AH.AHID,
                            AHName = AH.AHName
                        });
                    }
                }
                var ahMglevel = lstAccountHeads.FindAll(l => l.ParentAHID == majorGroupAH.AHID && l.AHLevel == 5);

                foreach (var mgAH in ahMglevel)
                {
                    lstAccountHeadDto.Add(new AccountHeadDto()
                    {
                        AHID = mgAH.AHID,
                        AHName = mgAH.AHName
                    });
                }
            }

            SelectList principleAHSelectList = new SelectList(lstAccountHeadDto, "AHID", "AHName");
            ViewBag.PrincipleAcHeads = principleAHSelectList;

            //var interestAHDto = lstAccountHeads.FindAll(l => l.AHID == 4);//TODO:Sample Example Need to Change
            //foreach (var interest in interestAHDto)
            //{
            //    AccountHeadDto objaccount = new AccountHeadDto()
            //    {
            //        AHID=interest.AHID,
            //        AHName=interest.AHName

            //    };
            //    lstAccountHeadDto.Add(objaccount);

            //}
            List<AccountHeadDto> lstInterstAccountHeadDto = new List<AccountHeadDto>();
            var interestExpressAH = lstAccountHeads.Find(l => l.AHCode == "INTEREST EXPENSES" && l.AHLevel == 2);//TODO:Sample Example Need to Change
            var interestExpressSubAH = lstAccountHeads.FindAll(l => l.ParentAHID == interestExpressAH.AHID && l.AHLevel == 3);
            foreach (var majorGroupAH in interestExpressSubAH)
            {
                var subGroupAh = lstAccountHeads.FindAll(l => l.ParentAHID == majorGroupAH.AHID && l.AHLevel == 4);
                foreach (var sgAH in subGroupAh)
                {
                    var lstAH = lstAccountHeads.FindAll(l => l.ParentAHID == sgAH.AHID && l.AHLevel == 5);
                    foreach (var AH in lstAH)
                    {
                        lstInterstAccountHeadDto.Add(new AccountHeadDto()
                        {
                            AHID = AH.AHID,
                            AHName = AH.AHName
                        });
                    }
                }
                var ahMglevel = lstAccountHeads.FindAll(l => l.ParentAHID == majorGroupAH.AHID && l.AHLevel == 5);

                foreach (var mgAH in ahMglevel)
                {
                    lstInterstAccountHeadDto.Add(new AccountHeadDto()
                    {
                        AHID = mgAH.AHID,
                        AHName = mgAH.AHName
                    });
                }
            }
            SelectList interestAHSelectList = new SelectList(lstInterstAccountHeadDto, "AHID", "AHName");
            ViewBag.InterestAcHeads = interestAHSelectList;

            //TODO : SECTION SHOULD REMOVE
            var accountHeadSelectListDto1 = _accountHeadService.GetAccountHeadSelectList();
            var penalAHDto = accountHeadSelectListDto1;//.FindAll(l => l.ID == 1);//TODO:Sample Example Need to Change
            SelectList penalAHSelectList = new SelectList(penalAHDto, "ID", "Text");
            ViewBag.PenalAcHeads = penalAHSelectList;

            SelectList baseTypeSelectList = GetDropDownListByMasterCode(Enums.RefMasterCodes.INTEREST_BASE_TYPE);
            ViewBag.BaseTypes = baseTypeSelectList;

            SelectList calMethodSelectList = GetDropDownListByMasterCode(Enums.RefMasterCodes.INTEREST_CALC_TYPE);
            ViewBag.CalcTypes = calMethodSelectList;
        }
        [HttpGet]
        public ActionResult ViewGroup(int? id)
        {
            // int groupId = DecryptQueryString(id);
            ViewBag.isGroupLogin = false;
            if (GroupInfo != null && GroupInfo.GroupID > 0)
            {
                id = GroupInfo.GroupID;
                ViewBag.isGroupLogin = true;
            }
            int gID = id.Value;
            ViewBag.GroupId = gID;
            if (gID <= 0)
                return RedirectToAction("GroupLookUp");
            SelectList MeetingFrrequency = GetDropDownListByMasterCode(Enums.RefMasterCodes.MEETING_FREQUENCY);
            ViewBag.MeetingFrrequency = MeetingFrrequency;

            var groupViewDto = _groupService.GetViewByID(gID);
            if (GroupInfo != null)
                ViewBag.Id = GroupInfo.GroupID;

            return View(groupViewDto);
        }

        [HttpGet]
        public ActionResult CreateGroupBankDetails(int Id)
        {

            SelectList banks = GetDropDownListByMasterCode(Enums.RefMasterCodes.BANK_NAME);
            SelectList branches = GetDropDownListByMasterCode(Enums.RefMasterCodes.BANK_NAME);
            SelectList accountType = GetDropDownListByMasterCode(Enums.RefMasterCodes.BANK_ACCOUNT_TYPE);
            BankMasterDal bankservice = new BankMasterDal();
            List<BankMasterLookupDto> lstBankMasterLookupDto = bankservice.GetLookup(null);
            lstBankMasterLookupDto = lstBankMasterLookupDto.FindAll(l => !l.IsBankAlreadyConsumed);

            // var villages = _dbContext.vi
            // ViewBag.Clusters = (SelectList) clusters;

            if (Id > 0)
            {
                List<BankMasterDto> lstModel = _bankService.BanksGetByObjectId(Id);
                ViewBag.BankDetails = lstModel;
            }
            ViewBag.ExistedBanks = lstBankMasterLookupDto;
            ViewBag.accountType = accountType;
            ViewBag.bankNames = banks;
            ViewBag.branches = branches;
            return View();
        }

        [HttpPost]
        public ActionResult CreateGroupBankDetails(FormCollection Form)
        {
            #region Dropdowns
            List<SelectListDto> lstVillages = _villageService.GetVillageSelectList();
            SelectList villages = new SelectList(lstVillages, "ID", "Text");

            List<SelectListDto> lstClusters = _clusterService.GetClusterSelectList();
            SelectList clusters = new SelectList(lstClusters, "ID", "Text");

            ViewBag.MeetingFrrequency = GetDropDownListByMasterCode(Enums.RefMasterCodes.MEETING_FREQUENCY);

            List<SelectListDto> lstpanchayats = _panchayatService.GetPanchayatSelectList();
            SelectList panchayats = new SelectList(lstpanchayats, "ID", "Text");

            ViewBag.villages = villages;
            ViewBag.clusters = clusters;
            ViewBag.panchayats = panchayats;
            #endregion

            int groupId = Convert.ToInt32(Request.Form["hdnObjectID"]);

            List<BankMasterDto> lstbank = GetBanksList(Form);

            ResultDto resultdto = _commonService.InsertBankDetails(groupId, Enums.EntityCodes.GROUP_MASTER, UserInfo.UserID, lstbank);

            return Json(new { groupid = groupId, ObjectCode = resultdto.ObjectId > 0 ? "TRUE" : "FALSE", Message = resultdto.Message });
        }

        [HttpGet]
        public ActionResult DeleteGroup(string Id)
        {
            int GroupId = DecryptQueryString(Id);

            if (GroupId < 1)
                return RedirectToAction("GroupLookUp");

            ResultDto resultDto = _groupService.Delete(GroupId, UserInfo.UserID);

            TempData["Result"] = resultDto;

            return RedirectToAction("GroupLookUp");
        }

        [HttpGet]
        public ActionResult ActiveInactiveGroup(string Id)
        {
            int GroupId = DecryptQueryString(Id);

            if (GroupId < 1)
                return RedirectToAction("GroupLookUp");

            ResultDto resultDto = _groupService.ChangeStatus(GroupId, UserInfo.UserID);

            TempData["Result"] = resultDto;

            return RedirectToAction("GroupLookUp");
        }

        public ActionResult BindDropDowns(string flag, int Id)
        {
             
            StringBuilder sbOptions = new StringBuilder();
            if (flag == "Cluster")
            {
                List<SelectListDto> lstvillageDto = _villageService.GetVillageByClusterID(Id);
                foreach (var item in lstvillageDto)
                {
                    sbOptions.Append("<option value=" + item.ID + ">" + item.Text + "</option>");
                }
            }
            else if (flag == "Village")
            {
                List<SelectListDto> lstpanchayat = _panchayatService.GetPanchayatByVillageID(Id);
                foreach (var item in lstpanchayat)
                {
                    sbOptions.Append("<option value=" + item.ID + ">" + item.Text + "</option>");
                }
            }
            return Content(sbOptions.ToString());

        }
        public ActionResult ViewGroupBankDetails(int id)
        {
            List<BankMasterDto> lstModel = new List<BankMasterDto>();
            if (id > 0)
            {
                lstModel = _bankService.BanksGetByObjectId(id);
                ViewBag.BankDetails = lstModel;
            }
            return View();
        }
        public ActionResult ViewMember(int id)
        {

            var memberDto = _memberService.GetByGroupId(id);

            ViewBag.Members = memberDto;

            return View(memberDto);
        }
        public ActionResult ViewLoanInterestDetails(int? GroupID)
        {
            List<GroupInterestRatesDto> lstinterest = new List<GroupInterestRatesDto>();
            string type = "L";
            if (GroupID > 0)
            {

                lstinterest = _groupService.GetInterestByID(GroupID.Value, type);
                ViewBag.GroupInterestRates = lstinterest;
            }
            return View();
        }
        public ActionResult ViewdepositinterestDetails(int? GroupID)
        {
            List<GroupInterestRatesDto> lstinterest = new List<GroupInterestRatesDto>();
            string type = "D";
            if (GroupID > 0)
            {
                lstinterest = _groupService.GetInterestByID(GroupID.Value, type);
                ViewBag.GroupInterestRates = lstinterest;
            }
            return View();
        }
        public ActionResult ViewLeaderShip()
        {
            return View();
        }
    }
}