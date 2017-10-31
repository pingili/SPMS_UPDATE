using AutoMapper;
using BusinessEntities;
using BusinessLogic.AutoMapper;
using DataLogic;
using MFIEntityFrameWork;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BusinessLogic
{
    public class MemberLoanDisbursementService 
    {

        private readonly MFISDBContext _dbContext;
        private readonly LoanDisbursementDataAccess _LoanDisbursementDataAccess;
        private readonly MemberLoanDisbursementDataAccess _MemberLoanDisbursementDataAccess;

        public MemberLoanDisbursementService()
        {
            _LoanDisbursementDataAccess = new LoanDisbursementDataAccess();
            _MemberLoanDisbursementDataAccess = new MemberLoanDisbursementDataAccess();
            _dbContext = new MFISDBContext();
            AutoMapperEntityConfiguration.Configure();
        }
        public GroupLoanDisbursementDto GetByID(int id)
        {
            List<uspFederationLoanDisbursement_Result> lstuspFederationLoanDisbursement_Result = _dbContext.uspFederationLoanDisbursement(id).ToList();
            GroupLoanDisbursementDto groupLoanDisbursementDto = Mapper.Map<uspFederationLoanDisbursement_Result, GroupLoanDisbursementDto>(lstuspFederationLoanDisbursement_Result.FirstOrDefault());
            return groupLoanDisbursementDto;
        }
        public List<MemberLoanApplicationLookupDto> MemberLoanDisbursementLookup(int groupID, int userId)
        {
            return _MemberLoanDisbursementDataAccess.MemberLoanDisbursementLookup(groupID, userId);
        }
        public List<GroupLoanDisbursementLookupDto> MemberLoanDisbursementLookup(int groupID)
        {
            List<GroupLoanDisbursementLookupDto> lstGroupLoanDisbursementLookupDto = new List<GroupLoanDisbursementLookupDto>();
            var objuspMemberLoanDisbursementLookupResult = _dbContext.uspMemberLoanDisbursementLookup(groupID).ToList();
            foreach (var obj in objuspMemberLoanDisbursementLookupResult)
            {
                lstGroupLoanDisbursementLookupDto.Add(new GroupLoanDisbursementLookupDto()
                {          
                    LoanMasterID=obj.LoanMasterID,
                    LoanCode=obj.LoanCode,
                    MemberId=obj.MemberID,
                    MemberName=obj.MemberName,
                    LoanAmountApplied=obj.LoanAmountApplied,
                    LoanPurpose=obj.LoanPurpose,
                    Purpose=obj.Purpose,
                    StatusCode=obj.StatusCode,
                    Status=obj.Status,
                    DisbursedAmount=obj.DisbursedAmount.HasValue?obj.DisbursedAmount.Value:0,
                    DisbursementDate=obj.DisbursementDate.HasValue?obj.DisbursementDate.Value:DateTime.MinValue
                   
                   
                });
            }
            return lstGroupLoanDisbursementLookupDto;

        }
        public InterestMasterDto GetInterestRatesByID(int GroupID, int PrincipleAHID)
        {

            InterestMasterDto objInterestMasterDto = new InterestMasterDto();
            objInterestMasterDto = _LoanDisbursementDataAccess.GetInterestRatesByID(GroupID, PrincipleAHID);
            return objInterestMasterDto;

        }
        public List<ScheduleDTO> GetSchedules(int LoanMasterId,decimal LoanAmount, decimal interestrate, int loanperiod, DateTime StartPaymentDate)
        {
            List<ScheduleDTO> lstSchedule = new List<ScheduleDTO>();
            lstSchedule = _LoanDisbursementDataAccess.GetSchedulesForDisbursement(LoanMasterId, LoanAmount, interestrate, loanperiod, StartPaymentDate);
            return lstSchedule;
        }
        public ResultDto CreateSchedules(int LoanMasterId, decimal LoanAmount, decimal interestrate, int loanperiod, DateTime StartPaymentDate, int CurrentUserID, int PROI)
        {
            ResultDto objResult = new ResultDto();
            objResult = _LoanDisbursementDataAccess.CreateSchedulesForDisbursement(LoanMasterId, LoanAmount, interestrate, loanperiod, StartPaymentDate, CurrentUserID, PROI);
            return objResult;

        }

        public bool CheckLoanDisbursed(int LoanMasterId)
        {
            bool isSuccess = false;

            isSuccess = _LoanDisbursementDataAccess.CheckLoanDisbursed(LoanMasterId);
            return isSuccess;

        }
        public ResultDto InsertDisbursement(GroupLoanDisbursementDto obj, int CurrentUserID)
        {
            ResultDto objResult = new ResultDto();
            //objResult = _MemberLoanDisbursementDataAccess.InsertDisbursemnet(obj, CurrentUserID);
            //ADDED NEW METHOD IN LOAN APPLICATION SERVICE SaveMemberLoanDisbursementDetails
            return objResult;
        }
        public ResultDto GenerateVoucher(int LoanMasterID, decimal LoanAmount, int BankEntryId, int UserID, char TransactionType, int GroupId, string Narration, DateTime ChequeDate, string chequenumer, long BankAcount)
        {
            ResultDto objResult = new ResultDto();

            objResult = _LoanDisbursementDataAccess.GenerateVoucher(LoanMasterID, LoanAmount, BankEntryId, UserID, TransactionType, GroupId, Narration, ChequeDate, chequenumer, BankAcount);
            return objResult;

        }
        public ResultDto SaveSecurityDetails(DataTable dt)
        {
            ResultDto objResult = _LoanDisbursementDataAccess.SaveSecurityDetails(dt);
            return objResult;

        }
        public string GetSecurityName(int SecurityCode)
        {

            string SecurityName = _LoanDisbursementDataAccess.GetSecurityName(SecurityCode);
            return SecurityName;
        }

        public ResultDto GuarantorDetails(DataTable dt)
        {
            ResultDto objResult = _MemberLoanDisbursementDataAccess.GuarantorDetails(dt);
            return objResult;
        }

    }
}
