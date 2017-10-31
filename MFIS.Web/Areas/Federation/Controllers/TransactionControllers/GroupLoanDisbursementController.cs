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
using DataLogic;
using BusinessLogic.Implementation;

namespace MFIS.Web.Areas.Federation.Controllers.TransactionControllers
{
    public class GroupLoanDisbursementController : BaseController
    {
        #region Global Variables
        private readonly GroupService _groupService;
        private readonly LoanPurposeService _loanpurposeService;
        private readonly GroupLoanApplicationService _grouploanapplicationService;
        private readonly GroupLoanDisbursementService _groupLoanDisbursementService;
        private readonly MemberService _memberService;
        private readonly AccountHeadService _accountHeadService;
        private readonly FundSourceService _fundSourceService;
        private readonly ProjectService _projectService;
        private readonly GroupReceiptService _groupReceiptService;
        private readonly BankService _bankService;
        private readonly LoanSecurityMasterService _loanSecurityService;
        private readonly InterestService _interestService;
        private readonly LoanDisbursementDataAccess _loanDisbursementDataAccess;
        private readonly MasterService _masterService;

        public GroupLoanDisbursementController()
        {
            _groupService = new GroupService();
            _loanpurposeService = new LoanPurposeService();
            _grouploanapplicationService = new GroupLoanApplicationService();
            _groupLoanDisbursementService = new GroupLoanDisbursementService();
            _memberService = new MemberService();
            _accountHeadService = new AccountHeadService();
            _fundSourceService = new FundSourceService();
            _projectService = new ProjectService();
            _groupReceiptService = new GroupReceiptService();
            _bankService = new BankService();
            _loanSecurityService = new LoanSecurityMasterService();
            _interestService = new InterestService();
            _loanDisbursementDataAccess = new LoanDisbursementDataAccess();
            _masterService = new MasterService();
        }
        #endregion Global Variables

        [HttpGet]
        public ActionResult CreateGroupLoanDisbursement(string Id)
        {
            int ID = string.IsNullOrEmpty(Id.DecryptString()) ? default(int) : Convert.ToInt32(Id.DecryptString());
            char LoanType = 'G';
            var groupLoanDisbursementDto = new GroupLoanDisbursementDto();
            if (ID > 0)
            {
                Disbursement(ID);

                groupLoanDisbursementDto = GetLoanDisbursementDto(ID, LoanType);

                //Set interst ahnames while edit aftersaving, these are empty for 1st time
                if (groupLoanDisbursementDto.InterestMasterID > 0)
                {
                    InterestMasterDto objInterestMasterDto = GetInterestDetailsDto(groupLoanDisbursementDto.InterestMasterID);
                    groupLoanDisbursementDto.PrincipleAHName = objInterestMasterDto.AHName;
                    groupLoanDisbursementDto.InterestAHName = objInterestMasterDto.InterestAHName;
                    groupLoanDisbursementDto.PrincipleAHId = Convert.ToInt32(objInterestMasterDto.PrincipalAHID);
                    groupLoanDisbursementDto.InterestRateID = objInterestMasterDto.InterestRateID;
                }
                List<SelectListDto> memberselectlist = _memberService.GetMemberCodeByGroupId(groupLoanDisbursementDto.GroupID);
                SelectList lstmembernames = new SelectList(memberselectlist, "id", "text");
                ViewBag.membercodes = lstmembernames;

                TypeQueryResult lstBankAh = _masterService.GetTypeQueryResult("GROUP_OR_BANK_AH", groupLoanDisbursementDto.GroupID.ToString());
                ViewBag.slBankAh = new SelectList(lstBankAh.OrderBy(a => a.Name), "Id", "Name");
            }

            return View(groupLoanDisbursementDto);
        }
        [HttpPost]
        public ActionResult CreateGroupLoanDisbursement(FormCollection form)
        {
            BankMasterViewDto objBank = new BankMasterViewDto();
            List<BankMasterViewDto> lstFedBanks = _groupReceiptService.GetFederationBanks();
            SelectList lstFederationBanks = new SelectList(lstFedBanks, "BankEntryID", "AccountNumber", objBank.BankEntryID);
            ViewBag.federationbanks = lstFederationBanks;

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

            ResultDto resultdto = _groupLoanDisbursementService.GuarantorDetails(dt);
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

        public ActionResult GroupLoanDisbursementLookup()
        {
            char Type = 'G';
            var lstGroupLoanDisbursementLookupDto = _groupLoanDisbursementService.GroupLoanDisbursementLookup(Type);
            return View(lstGroupLoanDisbursementLookupDto);
        }
        public ActionResult Disbursement(int id)
        {
            char type = 'G';

            GroupLoanDisbursementDto groupLoanDisbursementDto = GetLoanDisbursementDto(id, type);
            LoadLoanInterestDropDowns();

            List<SelectListDto> fundSourceSelectList = _fundSourceService.GetFundSourceSelectList();
            SelectList SlstfoundSource = new SelectList(fundSourceSelectList, "ID", "Text");
            ViewBag.foundSource = SlstfoundSource;

            List<SelectListDto> projectSelectList = _projectService.GetProjectSelectList();
            SelectList slProjectList = new SelectList(projectSelectList, "ID", "Text");
            ViewBag.projects = slProjectList;

            BankMasterViewDto objBank = new BankMasterViewDto();
            List<BankMasterViewDto> lstFedBanks = _groupReceiptService.GetFederationBanks();
            SelectList lstFederationBanks = new SelectList(lstFedBanks, "BankEntryID", "AccountNumber", objBank.BankEntryID);
            ViewBag.federationbanks = lstFederationBanks;

            TypeQueryResult lstBankAh = _masterService.GetTypeQueryResult("GROUP_OR_BANK_AH", groupLoanDisbursementDto.GroupID.ToString());
            ViewBag.slBankAh = new SelectList(lstBankAh.OrderBy(a => a.Name), "Id", "Name");

            return View(groupLoanDisbursementDto);
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


        public InterestMasterDto GetInterestDetailsDto(int id)
        {
            //InterestMasterDto interestDto = _interestService.GetByID(id);
            InterestMasterDto interestDto = _loanDisbursementDataAccess.GetIntrestRateByIntrestId(id);
            InterestRatesDto interestRateDto = interestDto.InterestRates.Find(l => l.FromDate < DateTime.Now && (DateTime.Now < l.ToDate || l.ToDate == DateTime.MinValue));
            if (interestRateDto != null && interestRateDto.ROI > 0)
            {
                interestDto.InterestRate = interestRateDto.ROI;

            }
            if (interestRateDto != null && interestRateDto.PenalROI > 0)
            {
                interestDto.PenalROI = interestRateDto.PenalROI;
            }
            if (interestRateDto != null && interestRateDto.IntrestRateID > 0)
            {
                interestDto.InterestRateID = interestRateDto.IntrestRateID;
            }
            return interestDto;
        }

        public JsonResult GetInterestDetails(int id)
        {
            try
            {
                InterestMasterDto interestDto = GetInterestDetailsDto(id);
                return Json(new { result = interestDto });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void LoadLoanInterestDropDowns()
        {

            List<SelectListDto> slistInterestDto = _interestService.GetInterestsSelectList();
            SelectList slistInterest = new SelectList(slistInterestDto, "ID", "Text");

            ViewBag.Interest = slistInterest;

            var lstAccountHeads = _accountHeadService.GetAll(true);
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

        }


        public ActionResult selectIntrestRate(int GroupId, int PrAhID)
        {
            InterestMasterDto objInterestRates = _groupLoanDisbursementService.GetInterestRatesByID(GroupId, PrAhID);
            if (objInterestRates == null)
                objInterestRates = new InterestMasterDto();

            return Json(new { ROI = objInterestRates.InterestRate, penalAHId = objInterestRates.PenalAHID, interestRateId = objInterestRates.InterestAHID, DueDay = objInterestRates.DueDay, InterestRateId = objInterestRates.InterestID, PROI = objInterestRates.PenalROI }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Schedule(int LoanMasterId, decimal LoanAmount, decimal InterestRate, int loanperiod, DateTime StartPaymentDate)
        {
            //int LoanMasterId = Convert.ToInt32(Request.Form["LoanMasterId"]);

            List<ScheduleDTO> lstSchedule = _groupLoanDisbursementService.GetSchedulesForDisbursemnet(LoanMasterId, LoanAmount, InterestRate, loanperiod, StartPaymentDate);
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
            isDisbursed = _groupLoanDisbursementService.CheckLoanDisbursed(LoanMasterId);

            return Json(new { isDisbursed = isDisbursed });
        }
        [HttpPost]
        public ActionResult CreateSchedule(int LoanMasterID, decimal LoanAmount, decimal InterestRate, int loanperiod, DateTime StartPaymentDate, int PROI)
        {
            ResultDto resultdto = _groupLoanDisbursementService.CreateSchedulesForDisbursement(LoanMasterID, LoanAmount, InterestRate, loanperiod, StartPaymentDate, UserInfo.UserID, PROI);

            return Json(new { LoanMasterID = resultdto.ObjectId, Message = resultdto.Message, ObjectCode = resultdto.ObjectCode });
        }
        [HttpPost]
        public ActionResult CreateDisbursement(FormCollection form)
        {
            GroupLoanDisbursementDto obj = new GroupLoanDisbursementDto();
            obj.GroupInterstRateID = string.IsNullOrEmpty(form["hdninterestRateId"]) ? 0 : Convert.ToInt32(form["hdninterestRateId"]);
            obj.LoanMasterId = Convert.ToInt32(form["LoanMasterId"]);
            obj.DisbursedAmount = Convert.ToDecimal(form["txtDisBursedAmount"]);
            obj.NoOfInstallments = Convert.ToByte(form["noOfInstalments"]);
            obj.DisbursementDate = Convert.ToDateTime(form["txtDisbursementDate"]);
            obj.InstallmentStartFrom = Convert.ToDateTime(form["txtFirstInstallmentStartsFrom"]);
            obj.LoanClosingDate = Convert.ToDateTime(form["txtLastInstallmentDate"]);

            obj.InterestMasterID = string.IsNullOrEmpty(form["InterestMasterID"]) ? 0 : Convert.ToInt32(form["InterestMasterID"]);
            obj.InterestRateID = Convert.ToInt32(form["InterestRateID"]);

            obj.LoanRefNumber = Convert.ToString(form["RefNo"]);

            obj.MonthlyPrincipalDemand = Convert.ToDecimal(form["monthlyprincipaldemand"]);

            obj.ProjectID = Convert.ToInt32(form["ProjectID"]);
            obj.MeetingDay = Convert.ToInt32(form["txtDueDay"]);
            obj.GroupID = Convert.ToInt32(form["hdnGroupId"]);
            obj.PrincipleAHId = Convert.ToInt32(form["PrincipleAHId"]);
            obj.SLAccountName = Convert.ToString(form["SLAccountName"]);

            obj.TransactionMode = Convert.ToString(form["TransactionMode"]);
            obj.BankEntryId = Convert.ToInt32(form["BankEntryId"]);
            if (form["TransactionMode"] != "BD")
            {
                obj.ChequeNumber = Convert.ToString(form["ChequeNumber"]);
                obj.ChequeDate = Convert.ToDateTime(form["chequedate"]);
            }
            obj.GroupBankEntryId = Convert.ToInt32(form["GroupBankEntryId"]);
            ResultDto resultdto = _groupLoanDisbursementService.InsertDisbursement(obj, UserInfo.UserID);

            bool isConfirm = (Request.Form["hdnIsConfirm"] != null && Request.Form["hdnIsConfirm"] == "1");
            if (isConfirm)
            {
                resultdto = _groupLoanDisbursementService.ConfirmAndDisburseGroupLoan(obj.LoanMasterId, UserInfo.UserID);

                if (resultdto.ObjectId > 0)
                {
                    return RedirectToAction("GroupLoanDisbursementLookup");
                }
            }
            string encryptloanmasterId = obj.LoanMasterId.EncryptString();

            //return RedirectToAction("CreateGroupLoanDisbursement", new { encryptloanmasterId });

            return Json(new { LoanMasterID = resultdto.ObjectId, Message = resultdto.Message, ObjectCode = resultdto.ObjectCode });
        }

        public ActionResult GetVoucher(int LoanMasterId)
        {
            BankMasterViewDto objBank = new BankMasterViewDto();
            List<BankMasterViewDto> lstFedBanks = _groupReceiptService.GetFederationBanks();
            SelectList lstFederationBanks = new SelectList(lstFedBanks, "BankEntryID", "AccountNumber", objBank.BankEntryID);
            ViewBag.federationbanks = lstFederationBanks;

            GroupLoanDisbursementService objGLDS = new GroupLoanDisbursementService();
            DisbursementVoucherDto voucherDto = objGLDS.GetDisbursementVoucher(LoanMasterId);

            return View("GenerateVoucher", voucherDto);
        }
        [HttpPost]
        //public ActionResult GeneratePaymentVoucher(int LoanMasterID, decimal LoanAmount, string BankEntryId, string TransactionMode)
        //{
        //    int BankEntryI = 0;
        //    if (!string.IsNullOrEmpty(BankEntryId))
        //    {
        //        BankEntryI = Convert.ToInt32(BankEntryId);
        //    }
        //    decimal Amount = Convert.ToDecimal(LoanAmount);
        //    char Mode = Convert.ToChar(TransactionMode);


        //    ResultDto resultdto = _groupLoanDisbursementService.GenerateVoucher(LoanMasterID, Amount, BankEntryI, UserInfo.UserID, Mode);

        //    return Json(new { LoanMasterID = resultdto.ObjectId, Message = resultdto.Message, ObjectCode = resultdto.ObjectCode });


        //}
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
            return Json(new { LoanMasterID = resultdto.ObjectId, Message = resultdto.Message, ObjectCode = resultdto.ObjectCode, VoucherDate = resultdto.CreatedOn, VoucherType = resultdto.Type });


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
            ResultDto objResult = _groupLoanDisbursementService.SaveSecurityDetails(dt);
            return Json(new { LoanMasterID = LoanMasterId, Message = objResult.Message, ObjectCode = objResult.ObjectCode });

        }
        public ActionResult GetSecurityname(int SecurityCode)
        {
            string SecurityNAme = _groupLoanDisbursementService.GetSecurityName(SecurityCode);
            return Json(new { SecurityName = SecurityNAme });
        }
        [HttpGet]
        public ActionResult DeleteMemberLoanDisbursement(string Id)
        {
            int LoanmasterId = DecryptQueryString(Id);

            if (LoanmasterId < 1)
                return RedirectToAction("GroupLoanDisbursementLookup");

            ResultDto resultDto = _groupLoanDisbursementService.Delete(LoanmasterId, UserInfo.UserID);

            TempData["Result"] = resultDto;

            return RedirectToAction("GroupLoanDisbursementLookup");
        }

        [HttpGet]
        public ActionResult ActiveInactiveGroupreceipts(string Id)
        {
            int LoanmasterId = DecryptQueryString(Id);

            if (LoanmasterId < 1)
                return RedirectToAction("GroupLoanDisbursementLookup");

            ResultDto resultDto = _groupLoanDisbursementService.ChangeStatus(LoanmasterId, UserInfo.UserID);

            TempData["Result"] = resultDto;

            return RedirectToAction("GroupLoanDisbursementLookup");
        }

        public JsonResult ValidateSchedule(int LoanMasterID)
        {
            GroupLoanDisbursementService obj = new GroupLoanDisbursementService();
            ResultDto result = obj.ValidateSchedule(LoanMasterID);
            return Json(new { result = result });
        }
    }
}
