using BusinessEntities;
using BusinessLogic;
using MFIS.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using CoreComponents;
using System.Linq;
using System.Data;
namespace MFIS.Web.Areas.Group.Controllers.TransactionControllers
{
    public class GroupLoanOBController : BaseController
    {
        private readonly VillageService _villageService;
        private readonly GroupService _groupService;
        private readonly ClusterService _clusterService;
        private readonly ProjectService _projectService;
        private readonly FundSourceService _fundSourceService;
        private readonly LoanPurposeService _loanpurposeService;
        private readonly InterestService _interestService;
        private readonly AccountHeadService _accountHeadService;
        private readonly MemberService _memberService;
        private readonly GroupLoanDisbursementService _groupLoanDisbursementService;
        public GroupLoanOBController()
        {
            _memberService = new MemberService();
            _villageService = new VillageService();
            _groupService = new GroupService();
            _clusterService = new ClusterService();
            _projectService = new ProjectService();
            _fundSourceService = new FundSourceService();
            _loanpurposeService = new LoanPurposeService();
            _interestService = new InterestService();
            _accountHeadService = new AccountHeadService();
            _groupLoanDisbursementService = new GroupLoanDisbursementService();
        }

        [HttpGet]
        public ActionResult GetLoanOB(int? id)
        {
            ViewBag.clusters = GroupInfo.Cluster;
            ViewBag.village = GroupInfo.Village;
            ViewBag.GroupName = GroupInfo.GroupName;
            ViewBag.MeetingDay = GroupInfo.MeetingDay;
            ViewBag.LockStatus = GroupInfo.LockStatus;
            int LoanMasterId = Convert.ToInt32(id);
            LoanOBByLoanDto lstdtos = new LoanOBByLoanDto();
            LoanOBBll objLoanOBBll = new LoanOBBll();
            LoanOBDto lstLoanOBDto = new LoanOBDto();
            if (LoanMasterId > 0)
            {
                lstLoanOBDto = objLoanOBBll.GetByID(LoanMasterId);
                lstLoanOBDto.Total1 = lstLoanOBDto.PrincipalOutstanding1 + lstLoanOBDto.InterestDue1;
                lstLoanOBDto.Total2 = lstLoanOBDto.PrincipalOutstanding2 + lstLoanOBDto.InterestDue2;
                lstLoanOBDto.Total3 = lstLoanOBDto.PrincipalOutstanding3 + lstLoanOBDto.InterestDue3;
                lstLoanOBDto.Total4 = lstLoanOBDto.PrincipalOutstanding4 + lstLoanOBDto.InterestDue4;
            }

            List<LoanPurposeLookupDto> lstLoanpurposeDto = _loanpurposeService.Lookup();
            SelectList lstloanpurpose = new SelectList(lstLoanpurposeDto, "LoanPurposeID", "Purpose");
            ViewBag.lonapurpose = lstloanpurpose;
            List<SelectListDto> lstProjects = _projectService.GetProjectSelectList();
            SelectList slProjects = new SelectList(lstProjects, "ID", "Text");
            ViewBag.projects = slProjects;
            List<MemberLookupDto> lstMembers = _memberService.GetByGroupId(GroupInfo.GroupID);
            SelectList slMembers = new SelectList(lstMembers, "MemberID", "MemberName");

            if (lstLoanOBDto.MemberID == 0)
            {
                LoanOBBll loanOBBll = new LoanOBBll();
                List<LoanOBLookup> lstLoanOB = loanOBBll.LoanOBLookUpList('M', GroupInfo.GroupID);
                foreach (var member in lstLoanOB)
                {
                    lstMembers.Remove(lstMembers.Find(l => l.MemberID == member.MemberID));
                }
                slMembers = new SelectList(lstMembers, "MemberID", "MemberName");
            }
            else
            {
                List<MemberLookupDto> lstMembersTemp = new List<MemberLookupDto>();
                foreach (var member in lstMembers)
                {
                    if (member.MemberID == lstLoanOBDto.MemberID)
                    {
                        lstMembersTemp.Add(lstMembers.Find(l => l.MemberID == member.MemberID));
                    }
                }
                slMembers = new SelectList(lstMembersTemp, "MemberID", "MemberName");
            }


            ViewBag.Members = slMembers;

            List<SelectListDto> lstFundSources = _fundSourceService.GetFundSourceSelectList();
            SelectList slFundsource = new SelectList(lstFundSources, "ID", "Text");
            ViewBag.fundsource = slFundsource;

            List<SelectListDto> lstInterests = _interestService.GetInterestsSelectList(GroupInfo.GroupID);
            SelectList slInterests = new SelectList(lstInterests, "ID", "Text");
            ViewBag.interests = slInterests;

            if (LoanMasterId == 0)
            {
                if (lstInterests != null && lstInterests.Count > 0 && lstInterests.Exists(l => l.Text.ToUpper().Contains("BIG LOANS TO")))
                {
                    SelectListDto s = lstInterests.Find(l => l.Text.ToUpper().Contains("BIG LOANS TO"));
                    InterestMasterDto interest = GetInterestDetailsDto(s.ID);
                    lstLoanOBDto.AHCode1 = interest.PrincipalAHCode;
                    lstLoanOBDto.AHName1 = interest.PrincipalAHName;
                    lstLoanOBDto.ROI1 = Convert.ToInt32(interest.InterestRate);
                    //lstLoanOBDto.InterestDue1 = 0;
                    lstLoanOBDto.Interest1 = s.ID;
                }

                if (lstInterests != null && lstInterests.Count > 0 && lstInterests.Exists(l => l.Text.ToUpper().Contains("SMALL LOANS TO")))
                {
                    SelectListDto s = lstInterests.Find(l => l.Text.ToUpper().Contains("SMALL LOANS TO"));
                    InterestMasterDto interest = GetInterestDetailsDto(s.ID);
                    lstLoanOBDto.AHCode2 = interest.PrincipalAHCode;
                    lstLoanOBDto.AHName2 = interest.PrincipalAHName;
                    lstLoanOBDto.ROI2= Convert.ToInt32(interest.InterestRate);
                    //lstLoanOBDto.InterestDue2 = 0;
                    lstLoanOBDto.Interest2 = s.ID;
                }
                if (lstInterests != null && lstInterests.Count > 0 && lstInterests.Exists(l => l.Text.ToUpper().Contains("HOUSING LOANS TO")))
                {
                    SelectListDto s = lstInterests.Find(l => l.Text.ToUpper().Contains("HOUSING LOANS TO"));
                    InterestMasterDto interest = GetInterestDetailsDto(s.ID);
                    lstLoanOBDto.AHCode3 = interest.PrincipalAHCode;
                    lstLoanOBDto.AHName3 = interest.PrincipalAHName;
                    lstLoanOBDto.ROI3 = Convert.ToInt32(interest.InterestRate);
                    //lstLoanOBDto.InterestDue3 = 0;
                    lstLoanOBDto.Interest3 = s.ID;
                }
            }
            else if (LoanMasterId > 0 && lstLoanOBDto.AHCode1 != null && lstLoanOBDto.AHCode2 != null && lstLoanOBDto.AHCode3 != null)
            {

            }
            else if (LoanMasterId > 0 && lstLoanOBDto.AHCode1 != null && lstLoanOBDto.AHCode2 != null)
            {

                if (lstInterests != null && lstInterests.Count > 0 && lstInterests.Exists(l => l.Text.ToUpper().Contains("BIG LOANS TO")))
                {
                    SelectListDto s = lstInterests.Find(l => l.Text.ToUpper().Contains("BIG LOANS TO"));
                    InterestMasterDto interest = GetInterestDetailsDto(s.ID);
                    lstLoanOBDto.AHCode3 = interest.PrincipalAHCode;
                    lstLoanOBDto.AHName3 = interest.PrincipalAHName;

                    lstLoanOBDto.Interest3 = s.ID;
                }

            }
            else
            {
                if (lstInterests != null && lstInterests.Count > 0 && lstInterests.Exists(l => l.Text.ToUpper().Contains("BIG LOANS TO")))
                {
                    SelectListDto s = lstInterests.Find(l => l.Text.ToUpper().Contains("BIG LOANS TO"));
                    InterestMasterDto interest = GetInterestDetailsDto(s.ID);
                    lstLoanOBDto.AHCode2 = interest.PrincipalAHCode;
                    lstLoanOBDto.AHName2 = interest.PrincipalAHName;

                    lstLoanOBDto.Interest2 = s.ID;
                }

                if (lstInterests != null && lstInterests.Count > 0 && lstInterests.Exists(l => l.Text.ToUpper().Contains("SMALL LOANS TO")))
                {
                    SelectListDto s = lstInterests.Find(l => l.Text.ToUpper().Contains("SMALL LOANS TO"));
                    InterestMasterDto interest = GetInterestDetailsDto(s.ID);
                    lstLoanOBDto.AHCode2 = interest.PrincipalAHCode;
                    lstLoanOBDto.AHName2 = interest.PrincipalAHName;

                    lstLoanOBDto.Interest2 = s.ID;
                }

                if (lstInterests != null && lstInterests.Count > 0 && lstInterests.Exists(l => l.Text.ToUpper().Contains("HOUSING LOANS TO")))
                {
                    SelectListDto s = lstInterests.Find(l => l.Text.ToUpper().Contains("HOUSING LOANS TO"));
                    InterestMasterDto interest = GetInterestDetailsDto(s.ID);
                    lstLoanOBDto.AHCode3 = interest.PrincipalAHCode;
                    lstLoanOBDto.AHName3 = interest.PrincipalAHName;

                    lstLoanOBDto.Interest3 = s.ID;
                }

            }
            ViewBag.lastpaidDate = string.Format("{0}/Mar/2016", GroupInfo.MeetingDay);
            lstLoanOBDto.LastPaidDate1 = ViewBag.lastpaidDate;
            lstLoanOBDto.LastPaidDate2 = ViewBag.lastpaidDate;
            lstLoanOBDto.LastPaidDate3 = ViewBag.lastpaidDate;
            lstLoanOBDto.LastPaidDate4 = ViewBag.lastpaidDate;
            ViewBag.meetingDay = GroupInfo.MeetingDay;
            //List<SelectListDto> lstPrincipalAHIDs=_accountHeadService.loan

            ViewBag.LoanAccountHeads = slInterests;
            return View(lstLoanOBDto);
        }


        [HttpPost]
        public JsonResult SaveLoanOB(LoanOBDto loanOBDto)
        {

            LoanOBBll objLoanOBBll = new LoanOBBll();
            loanOBDto.LoanType = 'M';
            loanOBDto.GroupId = GroupInfo.GroupID;
            loanOBDto.MemberID = Convert.ToInt32(Request.Form["MemberID"]);
            loanOBDto.UserID = UserInfo.UserID;
            loanOBDto = objLoanOBBll.InsertLoanOB(loanOBDto);
            string Message = null;
            string Fail = "";
            if (loanOBDto.LoanMasrterID1 != 0 && loanOBDto.IsDisbursed1 == false || loanOBDto.LoanMasrterID2 != 0 && loanOBDto.IsDisbursed2 == false || loanOBDto.LoanMasrterID3 != 0 && loanOBDto.IsDisbursed3 == false || loanOBDto.LoanMasrterID4 != 0 && loanOBDto.IsDisbursed4 == false)
            {
                Message = "Successfully Inserted";

            }
            else if (loanOBDto.IsDisbursed1 == true && loanOBDto.LoanMasrterID1 != 0)
            {
                Fail += loanOBDto.Total1 + "\n";
            }
            else if (loanOBDto.IsDisbursed2 == true && loanOBDto.LoanMasrterID2 != 0)
            {
                Fail += loanOBDto.Total2 + "\n";
            }
            else if (loanOBDto.IsDisbursed3 == true && loanOBDto.LoanMasrterID3 != 0)
            {
                Fail += loanOBDto.Total3 + "\n";
            }
            else if (loanOBDto.IsDisbursed4 == true && loanOBDto.LoanMasrterID4 != 0)
            {
                Fail += loanOBDto.Total4 + "\n";

            }
            else
            {
                Message = "Insert failed";
            }
            if (Fail.Length > 1)
            {
                Message = Message + "Failed to insert Principal Amount Outstandings are" + Fail;
            }
            else
            {
                Message = Message + "";
            }
            return Json(new { result = Message, LoanMasrterID1 = loanOBDto.LoanMasrterID1, LoanMasrterID2 = loanOBDto.LoanMasrterID2, LoanMasrterID3 = loanOBDto.LoanMasrterID3, LoanMasrterID4 = loanOBDto.LoanMasrterID4 });

        }

        [HttpGet]
        public ActionResult SelectGroup()
        {
            //IGroupService _groupService = new GroupService();
            //int EmployeeId = 1;
            //List<SelectListDto> lstVillages = _villageService.GetVillageSelectList();
            //SelectList slVillages = new SelectList(lstVillages, "ID", "Text");

            List<SelectListDto> lstClusters = _clusterService.GetClusterSelectList();
            SelectList slClusters = new SelectList(lstClusters, "ID", "Text");
            //List<GroupMasterDto> lstGroups = _groupService.GetByEmployeeId(EmployeeId);
            //SelectList lstgroup = new SelectList(lstGroups, "GroupID", "GroupName");

            //ViewBag.villages = slVillages;
            ViewBag.clusters = slClusters;
            //ViewBag.lstGroupNames = lstgroup;
            return View();
        }
        [HttpPost]
        public ActionResult LoanObSchedule(int LoanMasterId, decimal LoanAmount, decimal InterestRate, int loanperiod, string LastpaidDate, decimal OverDue, string firstInstallmentate, string DisbursementDate)
        {
            List<ScheduleDTO> lstSchedule = _groupLoanDisbursementService.GetSchedulesOB(LoanMasterId,LoanAmount, InterestRate, loanperiod, Convert.ToDateTime(firstInstallmentate), Convert.ToDateTime(DisbursementDate));
            ViewBag.lstSchedules = lstSchedule;
            return View();

        }
        [HttpPost]
        
        public ActionResult LoanObCreateSchedule(int LoanMasterId, decimal LoanAmount, decimal InterestRate, int loanperiod, string LastpaidDate,  string firstInstallmentate, string DisbursementDate)
        {
            //int LoanMasterId = Convert.ToInt32(Request.Form["LoanMasterId"]);
            DateTime StartPaymentDate = Convert.ToDateTime(firstInstallmentate);
            ResultDto resultdto = _groupLoanDisbursementService.CreateSchedulesOB(LoanMasterId, LoanAmount, InterestRate, loanperiod, StartPaymentDate, UserInfo.UserID, 0, DisbursementDate);

            return Json(new { Message = resultdto.Message });

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
                List<SelectListDto> lstGroups = _groupService.GetGroupByVillageID(Id);
                foreach (var item in lstGroups)
                {
                    sbOptions.Append("<option value=" + item.ID + ">" + item.Text + "</option>");
                }
            }
            return Content(sbOptions.ToString());

        }

        public JsonResult GetInterestDetails(int id)
        {
            InterestService _interestService = new InterestService();
            InterestMasterDto interestMasterDto = _interestService.GetByIDExt(id);
            return Json(new { result = interestMasterDto });
        }


        public InterestMasterDto GetInterestDetailsDto(int id)
        {
            InterestService _interestService = new InterestService();
            InterestMasterDto interestMasterDto = _interestService.GetByIDExt(id);
            return interestMasterDto;
        }

        public ActionResult LoanOBLookup()
        {
            LoanOBBll loanOBBll = new LoanOBBll();

            DataSet dsLoanOB = loanOBBll.LoanOBLookUpTable('M', GroupInfo.GroupID);
            ViewBag.DSLOANOB = dsLoanOB;

            return View();

            /*List<LoanOBLookup> lstLoanOB = loanOBBll.LoanOBLookUp('M', GroupInfo.GroupID);

            List<LoanOBLookupPivot> lstLoanOBPivot = new List<LoanOBLookupPivot>();

            var lstLoanOBGroup = lstLoanOB.GroupBy(l => l.MemberID).Select(m => m.Key).ToList();
            foreach (var member in lstLoanOBGroup)
            {

                var memberLoans = lstLoanOB.FindAll(l => l.MemberID == member);
                LoanOBLookupPivot loanOBLookupPivot = new LoanOBLookupPivot();
                loanOBLookupPivot.MemberName = lstLoanOB.Find(l => l.MemberID == member).MemberName;
                loanOBLookupPivot.MemberID = member;
                foreach (var loan in memberLoans)
                {
                    if (memberLoans.IndexOf(loan) == 0)
                    {
                        loanOBLookupPivot.LoanAmountGiven1 = loan.LoanAmountGiven;
                        loanOBLookupPivot.PrincipalOutstanding1 = loan.PrincipalOutstanding;
                    }
                    else if (memberLoans.IndexOf(loan) == 1)
                    {
                        loanOBLookupPivot.LoanAmountGiven2 = loan.LoanAmountGiven;
                        loanOBLookupPivot.PrincipalOutstanding2 = loan.PrincipalOutstanding;
                    }
                    else if (memberLoans.IndexOf(loan) == 2)
                    {
                        loanOBLookupPivot.LoanAmountGiven3 = loan.LoanAmountGiven;
                        loanOBLookupPivot.PrincipalOutstanding3 = loan.PrincipalOutstanding;
                    }
                    else
                        break;
                }

                lstLoanOBPivot.Add(loanOBLookupPivot);
            }
            var amount1 = lstLoanOBPivot.Sum(l => l.LoanAmountGiven1);
            var amount2 = lstLoanOBPivot.Sum(l => l.LoanAmountGiven2);
            var amount3 = lstLoanOBPivot.Sum(l => l.LoanAmountGiven3);
            var OSAmount1 = lstLoanOBPivot.Sum(l => l.PrincipalOutstanding1);
            var OSAmount2 = lstLoanOBPivot.Sum(l => l.PrincipalOutstanding2);
            var OSAmount3 = lstLoanOBPivot.Sum(l => l.PrincipalOutstanding3);
            ViewBag.LoanDisbursementTotal1 = amount1;
            ViewBag.LoanDisbursementTotal2 = amount2;
            ViewBag.LoanDisbursementTotal3 = amount3;
            ViewBag.LoanOutStandingAmountTotal1 = OSAmount1;
            ViewBag.LoanOutStandingAmountTotal2 = OSAmount2;
            ViewBag.LoanOutStandingAmountTotal3 = OSAmount3;

            return View(lstLoanOBPivot);
             * */
        }

        public ActionResult GetLoanPurposeByProjectID(int ProjectID)
        {
            StringBuilder sbOptions = new StringBuilder();
            List<LoanPurposeDto> lstLoanPurposeDto = new List<LoanPurposeDto>();
            lstLoanPurposeDto = _loanpurposeService.GetLoanPurposeByProjectID(ProjectID);
            foreach (var item in lstLoanPurposeDto)
            {
                sbOptions.Append("<option value=" + item.LoanPurposeID + ">" + item.Purpose + "</option>");
            }
            return Content(sbOptions.ToString());
        }

        public ActionResult DeleteLoanOBRecord(int id)
        {
            int MemberId = Convert.ToInt32(id);
            LoanOBBll objLoanOBBll = new LoanOBBll();
            ResultDto resultDto = objLoanOBBll.DeleteLoanOBByMemberID(MemberId);
            // TempData["Message"] = resultDto.ObjectCode;
            var message = resultDto.ObjectCode;
            return Content(message);
        }

    }
}
