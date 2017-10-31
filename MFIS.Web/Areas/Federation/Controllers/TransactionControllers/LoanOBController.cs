using BusinessEntities;
using BusinessLogic;
using MFIS.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using System.Linq;

namespace MFIS.Web.Areas.Federation.Controllers.TransactionControllers
{
    public class LoanOBController : BaseController
    {
        private readonly VillageService _villageService;
        private readonly GroupService _groupService;
        private readonly ClusterService _clusterService;
        private readonly ProjectService _projectService;
        private readonly FundSourceService _fundSourceService;
        private readonly LoanPurposeService _loanpurposeService;
        private readonly InterestService _interestService;
        private readonly AccountHeadService _accountHeadService;
        private readonly GroupLoanDisbursementService _groupLoanDisbursementService;
        public LoanOBController()
        {
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

            int LoanMasterId = Convert.ToInt32(id);
            LoanOBByLoanDto lstdtos = new LoanOBByLoanDto();
            LoanOBBll objLoanOBBll = new LoanOBBll();
            LoanOBDto lstLoanOBDto = new LoanOBDto();
            char LoanType = 'G';
            if (LoanMasterId > 0)
            {
                lstLoanOBDto = objLoanOBBll.GetByID(LoanMasterId, LoanType);
                lstLoanOBDto.Total1 = lstLoanOBDto.PrincipalOutstanding1 + lstLoanOBDto.InterestDue1;
                lstLoanOBDto.Total2 = lstLoanOBDto.PrincipalOutstanding2 + lstLoanOBDto.InterestDue2;
                lstLoanOBDto.Total3 = lstLoanOBDto.PrincipalOutstanding3 + lstLoanOBDto.InterestDue3;
                lstLoanOBDto.Total4 = lstLoanOBDto.PrincipalOutstanding4 + lstLoanOBDto.InterestDue4;
            }

            List<SelectListDto> lstClusters = _clusterService.GetClusterSelectList();
            SelectList slClusters = new SelectList(lstClusters, "ID", "Text");
            ViewBag.clusters = slClusters;

            List<LoanPurposeLookupDto> lstLoanpurposeDto = _loanpurposeService.Lookup();
            SelectList lstloanpurpose = new SelectList(lstLoanpurposeDto, "LoanPurposeID", "Purpose");
            ViewBag.lonapurpose = lstloanpurpose;

            List<SelectListDto> lstProjects = _projectService.GetProjectSelectList();
            SelectList slProjects = new SelectList(lstProjects, "ID", "Text");
            ViewBag.projects = slProjects;

            List<SelectListDto> lstFundSources = _fundSourceService.GetFundSourceSelectList();
            SelectList slFundsource = new SelectList(lstFundSources, "ID", "Text");
            ViewBag.fundsource = slFundsource;


            List<SelectListDto> lstvilllages = _villageService.GetVillageSelectList();
            SelectList slvillages = new SelectList(lstvilllages, "ID", "Text");
            ViewBag.village = slvillages;


            List<SelectListDto> lstgroups = _groupService.GetGroupsSelectList();
            SelectList slgroups = new SelectList(lstgroups, "ID", "Text");

            if (lstLoanOBDto.GroupId == 0)
            {
                LoanOBBll loanOBBll = new LoanOBBll();
                List<LoanOBLookup> lstLoanOB = loanOBBll.LoanOBLookUpList('G', 0);
                foreach (var group in lstLoanOB)
                {
                    lstgroups.Remove(lstgroups.Find(l => l.ID == group.GroupID));
                }
                slgroups = new SelectList(lstgroups, "ID", "Text");
            }
            else
            {
                List<SelectListDto> lstGroupsTemp = new List<SelectListDto>();
                foreach (var group in lstgroups)
                {
                    if (group.ID == lstLoanOBDto.GroupId)
                    {
                        lstGroupsTemp.Add(lstgroups.Find(l => l.ID == group.ID));
                    }
                }
                slgroups = new SelectList(lstGroupsTemp, "ID", "Text");
            }


            ViewBag.group = slgroups;

            List<SelectListDto> lstInterests = _interestService.GetInterestsSelectList();
            SelectList slInterests = new SelectList(lstInterests, "ID", "Text");
            ViewBag.interests = slInterests;

            //List<SelectListDto> lstPrincipalAHIDs=_accountHeadService.loan

            ViewBag.LoanAccountHeads = slInterests;

            return View(lstLoanOBDto);

        }

        [HttpPost]
        public JsonResult SaveLoanOB(LoanOBDto loanOBDto)
        {
            LoanOBBll objLoanOBBll = new LoanOBBll();
            loanOBDto.UserID = UserInfo.UserID;
            loanOBDto = objLoanOBBll.InsertLoanOB(loanOBDto);
            string Message = "";
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
        [HttpPost]
        public JsonResult BindMeetingDay(int ID)
        {
            GroupMasterDto groupdto = _groupService.GetByID(ID);
            return Json(new { DueDay = groupdto.MeetingDay });
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
        public DateTime GetDisbursementDate(int LoanMasterId)
        {
            ScheduleDTO obj = new ScheduleDTO();
            obj = _groupLoanDisbursementService.GetDisbusementDate(LoanMasterId);
            return Convert.ToDateTime(obj.DisbursementDate);

        }
        [HttpPost]
        public ActionResult LoanObSchedule(int LoanMasterId, decimal LoanAmount, decimal InterestRate, int loanperiod, string LastpaidDate, decimal OverDue, string firstInstallmentate, string DisbursementDate)
        {
            List<ScheduleDTO> lstSchedule = _groupLoanDisbursementService.GetSchedulesOB(LoanMasterId, LoanAmount, InterestRate, loanperiod, Convert.ToDateTime(firstInstallmentate), Convert.ToDateTime(DisbursementDate));
            ViewBag.lstSchedules = lstSchedule;
            return View();

        }
        [HttpPost]

        public ActionResult LoanObCreateSchedule(int LoanMasterId, decimal LoanAmount, decimal InterestRate, int loanperiod, string LastpaidDate, string firstInstallmentate, string DisbursementDate)
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
            InterestMasterDto interestMasterDto = _interestService.GetByIDFedExt(id);
            return Json(new { result = interestMasterDto });
        }

        public ActionResult LoanOBLookup()
        {
            LoanOBBll loanOBBll = new LoanOBBll();
            List<LoanOBLookup> lstLoanOB = loanOBBll.LoanOBLookUp('G');
            List<LoanOBLookupPivot> lstLoanOBPivot = new List<LoanOBLookupPivot>();

            var lstLoanOBGroup = lstLoanOB.GroupBy(l => l.GroupID).Select(m => m.Key).ToList();
            foreach (var group in lstLoanOBGroup)
            {

                var groupLoans = lstLoanOB.FindAll(l => l.GroupID == group);
                LoanOBLookupPivot loanOBLookupPivot = new LoanOBLookupPivot();
                loanOBLookupPivot.GroupName = lstLoanOB.Find(l => l.GroupID == group).GroupName;
                loanOBLookupPivot.GroupID = group;
                foreach (var loan in groupLoans)
                {
                    if (groupLoans.IndexOf(loan) == 0)
                    {
                        loanOBLookupPivot.LoanAmountGiven1 = loan.LoanAmountGiven;
                        loanOBLookupPivot.PrincipalOutstanding1 = loan.PrincipalOutstanding;
                    }
                    else if (groupLoans.IndexOf(loan) == 1)
                    {
                        loanOBLookupPivot.LoanAmountGiven2 = loan.LoanAmountGiven;
                        loanOBLookupPivot.PrincipalOutstanding2 = loan.PrincipalOutstanding;
                    }
                    else if (groupLoans.IndexOf(loan) == 2)
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
           // return View(lstLoanOB);
        }

    }
}
