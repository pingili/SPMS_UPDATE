using AutoMapper;
using BusinessEntities;
using BusinessLogic;
//using BusinessLogic.Interface;
using MFIS.Web.Areas.Federation.Models;
using MFIS.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CoreComponents;
using System.Data;
using BusinessLogic.Implementation;


namespace MFIS.Web.Areas.Group.Controllers.TransactionControllers
{
    public class MemberLoanDisbursementControllerBACKUP : BaseController
    {
        #region Global Variables

        private readonly LoanPurposeService _loanpurposeService;
        private readonly MemberLoanApplicationService _memberloanapplicationService;
        private readonly MemberLoanDisbursementService _memberLoanDisbursementService;
        private readonly MemberService _memberService;
        private readonly FundSourceService _fundSourceService;
        private readonly GroupLoanDisbursementService _groupLoanDisbursementService;
        private readonly ProjectService _projectService;
        private readonly GroupReceiptService _groupReceiptService;
        private readonly BankService _bankService;
        private readonly LoanSecurityMasterService _loanSecurityService;
        private readonly AccountHeadService _accountHeadService;
        private readonly InterestService _interestService;
        private readonly MasterService _masterService;
        public MemberLoanDisbursementControllerBACKUP()
        {
            _loanpurposeService = new LoanPurposeService();
            _memberloanapplicationService = new MemberLoanApplicationService();
            _memberLoanDisbursementService = new MemberLoanDisbursementService();
            _groupLoanDisbursementService = new GroupLoanDisbursementService();
            _memberService = new MemberService();
            _accountHeadService = new AccountHeadService();
            _fundSourceService = new FundSourceService();
            _projectService = new ProjectService();
            _groupReceiptService = new GroupReceiptService();
            _bankService = new BankService();
            _loanSecurityService = new LoanSecurityMasterService();
            _interestService = new InterestService();
            _masterService = new MasterService();
        }
        #endregion Global Variables

        [HttpGet]
        public ActionResult CreateMemberLoanDisbursement(string Id)
        {
            int ID = string.IsNullOrEmpty(Id.DecryptString()) ? default(int) : Convert.ToInt32(Id.DecryptString());
            char LoanType = 'M';
            var groupLoanDisbursementDto = new GroupLoanDisbursementDto();

            if (ID > 0)
            {
                LoadLoanInterestDropDowns();
                
                groupLoanDisbursementDto = GetLoanDisbursementDto(ID, LoanType);
                if (groupLoanDisbursementDto.InterestMasterID > 0)
                {
                    InterestMasterDto objInterestMasterDto = GetInterestDetailsDto(groupLoanDisbursementDto.InterestMasterID);
                    groupLoanDisbursementDto.PrincipleAHName = objInterestMasterDto.PrincipalAHName;
                    groupLoanDisbursementDto.InterestAHName = objInterestMasterDto.InterestName;
                    groupLoanDisbursementDto.PrincipleAHId = Convert.ToInt32(objInterestMasterDto.PrincipalAHID);
                    groupLoanDisbursementDto.InterestRateID = objInterestMasterDto.InterestRateID;
                    groupLoanDisbursementDto.ROI = objInterestMasterDto.InterestRate;
                }

                List<SelectListDto> memberselectlist = _memberService.GetMemberCodeByGroupId(groupLoanDisbursementDto.GroupID);
                SelectList lstmembernames = new SelectList(memberselectlist, "id", "text");
                ViewBag.membercodes = lstmembernames;

            }
            List<GroupMeetingDto> lstGroupMeetings = new GroupOtherReceiptService().GetGroupOpenMeetingDates(GroupInfo.GroupID);
            ViewBag.MonthMeetings = new SelectList(lstGroupMeetings, "DisplayMeetingDate", "DisplayMeetingDate");
            
            TypeQueryResult lstBankAh = _masterService.GetTypeQueryResult("GROUP_OR_BANK_AH", GroupInfo.GroupID.ToString());
            ViewBag.slBankAh = new SelectList(lstBankAh.OrderBy(a => a.Name), "Id", "Name");
            return View(groupLoanDisbursementDto);
        }

        [HttpPost]
        public ActionResult CreateMemberLoanDisbursement(FormCollection form)
        {   
            GuarantorDetailsDto objdto = new GuarantorDetailsDto();
            List<GuarantorDetailsDto> lstobj = new List<GuarantorDetailsDto>();
            int count = Convert.ToInt32(Request.Form["hdnMaxSecurityIndex"]);
            int AmountId = Convert.ToInt32(Request.Form["AmountId"]);
            DataTable dt = new DataTable();
            dt.Columns.Add("LoanGuarantorMemberID", typeof(Int32));
            dt.Columns.Add("LoanMasterID", typeof(Int32));
            dt.Columns.Add("IsActive", typeof(bool));
            dt.Columns.Add("CreatedBy", typeof(Int32));
            dt.Columns.Add("CreatedOn", typeof(DateTime));


            for (int i = 1; i <= count; i++)
            {
                var AccountCode = Convert.ToInt32(form["hdnAccountCode_" + i]);
                var AccountName = Convert.ToString(form["hdnAccountName_" + i]);
                dt.Rows.Add(AmountId, AccountCode, AccountName, UserInfo.UserID, DateTime.Now);

            }

            ResultDto resultdto = _memberLoanDisbursementService.GuarantorDetails(dt);
            return Json(new { LoanMasterID = AmountId, Message = resultdto.Message, ObjectCode = resultdto.ObjectCode });
        }
        public JsonResult GetFAccountName(int id)
        {
            BankMasterDto objBank = _bankService.GetByID(id);
            return Json(new { FaccountName = objBank.BName });
        }

        public JsonResult GetMemberName(int id)
        {
            MemberDto memberDto = _memberService.GetById(id);
            return Json(new { MemberName = memberDto.MemberName });
        }
        //public JsonResult GetGroupName(int id)
        //{
        //    GroupLoanDisbursementDto groupLoanDisbursementDto = _groupLoanDisbursementService.GetByID(id);

        //    return Json(new
        //    {
        //        GroupName = groupLoanDisbursementDto.GroupName,LoanCode = groupLoanDisbursementDto.LoanCode,
        //        LoanApplicationDate = groupLoanDisbursementDto.LoanApplicationDate,LoanPurpose = groupLoanDisbursementDto.LoanPurpose,
        //        LoanAmountApplied = groupLoanDisbursementDto.LoanAmountApplied,
        //        NoOfInstallments = groupLoanDisbursementDto.NoOfInstallments,
        //        Mode = groupLoanDisbursementDto.Mode
        //    });
        //}

        public InterestMasterDto GetInterestDetailsDto(int id)
        {
            InterestMasterDto interestDto = _interestService.GetByIDExt(id);
            // InterestRatesDto interestRateDto = interestDto.InterestRates.Find(l => l.FromDate < DateTime.Now && DateTime.Now < l.ToDate);
            //InterestRatesDto interestRateDto = interestDto;
            //if (interestRateDto != null && interestRateDto.ROI > 0)
            //{
            //    interestDto.InterestRate = interestRateDto.ROI;

            //}
            //if (interestRateDto != null && interestRateDto.PenalROI > 0)
            //{
            //    interestDto.PenalROI = interestRateDto.PenalROI;
            //}
            //if (interestRateDto != null && interestRateDto.IntrestRateID > 0)
            //{
            //    interestDto.InterestRateID = interestRateDto.IntrestRateID;
            //}
            return interestDto;
        }

        //public JsonResult GetInterestDetails(int id)
        //{
        //    InterestMasterDto interestDto = GetInterestDetailsDto(id);
        //    return Json(new { result = interestDto });
        //}

        public ActionResult MemberLoanDisbursementLookup()
        {
            var lstGroupLoanDisbursementLookupDto = _memberLoanDisbursementService.MemberLoanDisbursementLookup(GroupInfo.GroupID);
            return View(lstGroupLoanDisbursementLookupDto);
        }
        
        public GroupLoanDisbursementDto GetLoanDisbursementDto(int id, char type)
        {
            var groupLoanDisbursementDto = new GroupLoanDisbursementDto();
            groupLoanDisbursementDto = _groupLoanDisbursementService.GetByIDCustom(id);
            if (groupLoanDisbursementDto.DisbursedAmount == 0)
            {
                groupLoanDisbursementDto.DisbursedAmount = groupLoanDisbursementDto.LoanAmountApplied;
                groupLoanDisbursementDto.NoOfInstallments = groupLoanDisbursementDto.NoofInstallmentsProposed;
                if (groupLoanDisbursementDto.NoOfInstallments > 0)
                    groupLoanDisbursementDto.MonthlyPrincipalDemand = groupLoanDisbursementDto.DisbursedAmount / groupLoanDisbursementDto.NoOfInstallments;
            }
            else
            {
                groupLoanDisbursementDto.FinalInstallmentDate = groupLoanDisbursementDto.InstallmentStartFrom.AddMonths(groupLoanDisbursementDto.NoOfInstallments);
            }
            return groupLoanDisbursementDto;
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
            SelectList principleAHSelectList = new SelectList(lstAccountHeadDto, "AHID", "AHName");
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
            SelectList interestAHSelectList = new SelectList(lstInterstAccountHeadDto, "AHID", "AHName");
            ViewBag.InterestAcHeads = interestAHSelectList;
            var accountHeadSelectListDto1 = _accountHeadService.GetAccountHeadSelectList();
            var penalAHDto = accountHeadSelectListDto1;//.FindAll(l => l.ID == 1);//TODO:Sample Example Need to Change
            SelectList penalAHSelectList = new SelectList(penalAHDto, "ID", "Text");
            ViewBag.PenalAcHeads = penalAHSelectList;

            List<SelectListDto> slistInterestDto = _interestService.GetInterestsSelectList(GroupInfo.GroupID);
            SelectList slistInterest = new SelectList(slistInterestDto, "ID", "Text");

            ViewBag.Interest = slistInterest;


        }


        public ActionResult selectIntrestRate(int GroupId, int PrAhID)
        {
            InterestMasterDto objInterestRates = _memberLoanDisbursementService.GetInterestRatesByID(GroupId, PrAhID);
            if (objInterestRates == null)
                objInterestRates = new InterestMasterDto();

            return Json(new { ROI = objInterestRates.InterestRate, penalAHId = objInterestRates.PenalAHID, interestRateId = objInterestRates.InterestAHID, DueDay = objInterestRates.DueDay, InterestRateId = objInterestRates.InterestID, PROI = objInterestRates.PenalROI }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Schedule(int LoanMasterId, decimal LoanAmount, decimal InterestRate, int loanperiod, DateTime StartPaymentDate)
        {
            List<ScheduleDTO> lstSchedule = _memberLoanDisbursementService.GetSchedules(LoanMasterId,LoanAmount, InterestRate, loanperiod, StartPaymentDate);
            ViewBag.lstSchedules = lstSchedule;
            var disbursementDate = GetDisbursementDate(LoanMasterId);
            ViewBag.DisbusementDate = disbursementDate;
            return View();
        }
        public DateTime GetDisbursementDate(int LoanMasterId)
        {
            ScheduleDTO obj = new ScheduleDTO();
            obj = _groupLoanDisbursementService.GetDisbusementDate(LoanMasterId);
            return Convert.ToDateTime(obj.DisbursementDate);

        }
        public ActionResult CheckForDisbursementStart(int LoanMasterId)
        {
            bool isDisbursed = false;
            isDisbursed = _memberLoanDisbursementService.CheckLoanDisbursed(LoanMasterId);
            return Json(new { isDisbursed = isDisbursed });
        }
        [HttpPost]
        public ActionResult CreateSchedule(int LoanMasterID, decimal LoanAmount, decimal InterestRate, int loanperiod, DateTime StartPaymentDate, int PROI)
        {
            ResultDto resultdto = _memberLoanDisbursementService.CreateSchedules(LoanMasterID, LoanAmount, InterestRate, loanperiod, StartPaymentDate, UserInfo.UserID, PROI);

            return Json(new { LoanMasterID = resultdto.ObjectId, Message = resultdto.Message, ObjectCode = resultdto.ObjectCode });
        }
        [HttpPost]
        public ActionResult CreateDisbursement(FormCollection form)
        {
            GroupLoanDisbursementDto obj = new GroupLoanDisbursementDto();
            obj.LoanMasterId = Convert.ToInt32(form["LoanMasterId"]);
            obj.DisbursedAmount = Convert.ToDecimal(form["txtDisBursedAmount"]);
            obj.TransactionMode = Convert.ToString(Request.Form["TransactionMode"]);
            
            string strDisbusementDate = obj.TransactionMode == "C" ? Request.Form["DisbursementDate"] : Request.Form["txtDisbursementDate"];
            DateTime dtDisbusementDate = strDisbusementDate.ConvertToDateTime();
            obj.DisbursementDate = dtDisbusementDate;

            if (obj.TransactionMode != "C")
                obj.BankEntryId = Convert.ToInt32(Request.Form["BankEntryId"]);

            if (obj.TransactionMode == "BC")
            {
                obj.ChequeNumber = Convert.ToString(Request.Form["ChequeNumber"]);
                obj.chequedate = Request.Form["ChequeDate"].ConvertToDateTime();
            }
            obj.NoOfInstallments = Convert.ToByte(form["NoOfInstallments"]);
            obj.LoanRefNumber = Convert.ToString(form["RefNo"]);
            obj.InstallmentStartFrom = Convert.ToDateTime(form["txtFirstInstallmentStartsFrom"]);
            obj.LoanClosingDate = Convert.ToDateTime(form["txtLastInstallmentDate"]);
            obj.MonthlyPrincipalDemand = Convert.ToDecimal(form["monthlyprincipaldemand"]);
            obj.GroupInterstRateID = Convert.ToInt32(form["hdninterestRateId"]);

            // obj.GroupInterstRateID = Convert.ToInt32(form["hdninterestRateId"]);
            //obj.FundSourceID = 0;// Convert.ToInt32(form["ddlfundSource"]);
            //obj.InterestRateID = Convert.ToInt32(form["hdninterestRateId"]);
            // obj.LoanAmountApplied = Convert.ToInt32(form[""]);
            //obj.GroupID = Convert.ToInt32(form["hdnMemberId"]);
            //obj.PrincipleAHId = Convert.ToInt32(form["hdnPrincipalAHID"]);
            //obj.SLAccountName = Convert.ToString(form["SLAccountName"]);
            //obj.OutStandingAmount = string.IsNullOrEmpty(form["OutStandingAmount"]) ? 0 : Convert.ToInt32(form["OutStandingAmount"]);
            //obj.InterestMasterID = string.IsNullOrEmpty(form["InterestMasterID"]) ? 0 : Convert.ToInt32(form["InterestMasterID"]);
            ResultDto resultdto = _memberLoanDisbursementService.InsertDisbursement(obj, UserInfo.UserID);

            return Json(new { LoanMasterID = resultdto.ObjectId, Message = resultdto.Message, ObjectCode = resultdto.ObjectCode });

        }
        public ActionResult GenerateVoucher(int LoanMasterId)
        {
            BankMasterViewDto objBank = new BankMasterViewDto();
            List<BankMasterViewDto> lstFedBanks = _groupReceiptService.GetGroupBanks();
            SelectList lstFederationBanks = new SelectList(lstFedBanks, "BankEntryID", "AccountNumber", objBank.BankEntryID);
            ViewBag.federationbanks = lstFederationBanks;
            GroupLoanDisbursementService objGLDS = new GroupLoanDisbursementService();
            DisbursementVoucherDto voucherDto = objGLDS.GetDisbursementVoucher(LoanMasterId);

            return View("GenerateVoucher", voucherDto);
        }
        public ActionResult GeneratePaymentVoucher(FormCollection Form)
        {

            int BankEntryI = 0;
            string chequenumer = "";
            DateTime ChequeDate = DateTime.Now;
            long BankAcount = 0;
            string Narration = "";
            if (!string.IsNullOrEmpty(Form["FederationBankAccountNumber"]))
            {
                BankEntryI = Convert.ToInt32(Form["FederationBankAccountNumber"]);
                chequenumer = Convert.ToString(Form["ChequeNumber"]);
                ChequeDate = Convert.ToDateTime(Form["ChequeDate1"]);
                BankAcount = Convert.ToInt64(Form["hdnAccountNumber"]);
                Narration = "";
            }
            decimal Amount = Convert.ToDecimal(Form["money"]);
            char Mode = Convert.ToChar(Form["TransactionMode"]);
            int LoanMasterID = Convert.ToInt32(Form["LoanMasterID"]);
            int GroupId = Convert.ToInt32(Form["GroupId"]);
            ResultDto resultdto = _groupLoanDisbursementService.GenerateVoucher(LoanMasterID, Amount, BankEntryI, UserInfo.UserID, Mode, GroupId, Narration, ChequeDate, chequenumer, BankAcount);
            return Json(new { LoanMasterID = resultdto.ObjectId, Message = resultdto.Message, ObjectCode = resultdto.ObjectCode, VoucherType = resultdto.Type, VoucherDate = resultdto.CreatedOn });


        }

        public ActionResult AdditionalSecuritiesDetails()
        {
            var slloansecurity = _loanSecurityService.GetLoanSecurityMasterSelectList();
            SelectList SlSecurity = new SelectList(slloansecurity, "ID", "TEXT");
            ViewBag.LoanSecurityDetails = SlSecurity;
            return View();

        }

        public ActionResult SaveAdditionalSecuritiesDetails(FormCollection form)
        {
            AdditionalSecurityDetailsDTO objdto = new AdditionalSecurityDetailsDTO();
            List<AdditionalSecurityDetailsDTO> lstobj = new List<AdditionalSecurityDetailsDTO>();
            int count = Convert.ToInt32(Request.Form["hdnMaxSecurityIndex"]);
            int LoanMasterId = Convert.ToInt32(Request.Form["LoanMasterId"]);
            DataTable dt = new DataTable();
            dt.Columns.Add("LoanMasterID", typeof(Int32));
            dt.Columns.Add("LoanSecurityID", typeof(Int32));
            dt.Columns.Add("Description", typeof(String));
            dt.Columns.Add("CreatedBy", typeof(Int32));
            dt.Columns.Add("CreatedOn", typeof(DateTime));


            for (int i = 1; i <= count; i++)
            {
                var LoanSecurityId = Convert.ToInt32(form["hdnLoanSecurityCode_" + i]);
                var Description = Convert.ToString(form["hdnLoanDescription_" + i]);
                dt.Rows.Add(LoanMasterId, LoanSecurityId, Description, UserInfo.UserID, DateTime.Now);

            }
            ResultDto objResult = _memberLoanDisbursementService.SaveSecurityDetails(dt);
            return Json(new { LoanMasterID = LoanMasterId, Message = objResult.Message, ObjectCode = objResult.ObjectCode });

        }
        public ActionResult GetSecurityname(int SecurityCode)
        {
            string SecurityNAme = _groupLoanDisbursementService.GetSecurityName(SecurityCode);
            return Json(new { SecurityName = SecurityNAme });
        }
    }
}
