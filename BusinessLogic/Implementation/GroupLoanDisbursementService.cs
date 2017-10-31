using AutoMapper;
using BusinessEntities;
using BusinessLogic.AutoMapper;
//using BusinessLogic.Interface;
using MFIEntityFrameWork;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using DataLogic;
using System.Data;


namespace BusinessLogic
{
    public class GroupLoanDisbursementService
    {
        private readonly MFISDBContext _dbContext;
        private readonly LoanDisbursementDataAccess _LoanDisbursementDataAccess;

        public GroupLoanDisbursementService()
        {
            _LoanDisbursementDataAccess = new LoanDisbursementDataAccess();
            _dbContext = new MFISDBContext();
            AutoMapperEntityConfiguration.Configure();
        }

        public GroupLoanDisbursementDto GetByID(int id)
        {
            List<uspFederationLoanDisbursement_Result> lstuspFederationLoanDisbursement_Result = _dbContext.uspFederationLoanDisbursement(id).ToList();
            GroupLoanDisbursementDto groupLoanDisbursementDto = Mapper.Map<uspFederationLoanDisbursement_Result, GroupLoanDisbursementDto>(lstuspFederationLoanDisbursement_Result.FirstOrDefault());
            return groupLoanDisbursementDto;
        }
        public GroupLoanDisbursementDto GetByIDCustom(int id)
        {
            return _LoanDisbursementDataAccess.GetDisbursement(id);
        }

        public List<GroupLoanDisbursementLookupDto> GroupLoanDisbursementLookup(char Type)
        {
            string LoanType = Convert.ToString(Type);
            List<GroupLoanDisbursementLookupDto> lstGroupLoanDisbursementLookupDto = new List<GroupLoanDisbursementLookupDto>();
            List<uspGroupLoanDisbursementLookup_Result> lstuspGroupLoanDisbursementLookup_Result = _dbContext.uspGroupLoanDisbursementLookup(LoanType).ToList();
            foreach (var GroupLoanDisbursement in lstuspGroupLoanDisbursementLookup_Result)
            {
                GroupLoanDisbursementLookupDto groupLoanDisbursementLookupDto = Mapper.Map<uspGroupLoanDisbursementLookup_Result, GroupLoanDisbursementLookupDto>(GroupLoanDisbursement);
                lstGroupLoanDisbursementLookupDto.Add(groupLoanDisbursementLookupDto);
            }
            return lstGroupLoanDisbursementLookupDto;
        }
        public InterestMasterDto GetInterestRatesByID(int GroupID, int PrincipleAHID)
        {

            InterestMasterDto objInterestMasterDto = new InterestMasterDto();
            objInterestMasterDto = _LoanDisbursementDataAccess.GetInterestRatesByID(GroupID, PrincipleAHID);
            return objInterestMasterDto;

        }
        public List<ScheduleDTO> GetSchedulesForDisbursemnet(int LoanMasterId, decimal LoanAmount, decimal interestrate, int loanperiod, DateTime StartPaymentDate)
        {
            List<ScheduleDTO> lstSchedule = new List<ScheduleDTO>();
            lstSchedule = _LoanDisbursementDataAccess.GetSchedulesForDisbursement(LoanMasterId,LoanAmount, interestrate, loanperiod, StartPaymentDate);

            //  lstSchedule = _LoanDisbursementDataAccess.GetSchedules(LoanAmount, interestrate, loanperiod, StartPaymentDate,OverDue, LastPaidDate);

            return lstSchedule;
        }
        public List<ScheduleDTO> GetSchedulesOB(int LoanMasterId, decimal LoanAmount, decimal interestrate, int loanperiod, DateTime StartPaymentDate, DateTime DisbursementDate)
        {
            List<ScheduleDTO> lstSchedule = new List<ScheduleDTO>();
            // lstSchedule = _LoanDisbursementDataAccess.GetSchedulesForDisbursement(LoanAmount, interestrate, loanperiod, StartPaymentDate);

            lstSchedule = _LoanDisbursementDataAccess.GetSchedulesOB(LoanMasterId, LoanAmount, interestrate, loanperiod, DisbursementDate, StartPaymentDate);

            return lstSchedule;
        }
        public ResultDto CreateSchedulesForDisbursement(int LoanMasterId, decimal LoanAmount, decimal interestrate, int loanperiod, DateTime StartPaymentDate, int CurrentUserID, int PROI, string LastPaidDateOrDisbDate = null)
        {
            ResultDto objResult = new ResultDto();
            //objResult = _LoanDisbursementDataAccess.CreateSchedules(LoanMasterId, LoanAmount, interestrate, loanperiod, StartPaymentDate, CurrentUserID, 0, LastPaidDateOrDisbDate);
            return objResult;

        }
        public ResultDto CreateSchedulesOB(int LoanMasterId, decimal LoanAmount, decimal interestrate, int loanperiod, DateTime StartPaymentDate, int CurrentUserID, int PROI, string LastPaidDateOrDisbDate = null)
        {
            ResultDto objResult = new ResultDto();
            objResult = _LoanDisbursementDataAccess.CreateSchedulesForOB(LoanMasterId, LoanAmount, interestrate, loanperiod, StartPaymentDate, CurrentUserID, 0, LastPaidDateOrDisbDate);
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
            objResult = _LoanDisbursementDataAccess.InsertDisbursemnet(obj, CurrentUserID);
            return objResult;
        }
        public ResultDto ConfirmAndDisburseGroupLoan(int loanMasterId,int userId)
        {
            return _LoanDisbursementDataAccess.ConfirmAndDisburseGroupLoan(loanMasterId, userId);

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
        public ResultDto GuarantorDetails(DataTable dt)
        {
            ResultDto objResult = _LoanDisbursementDataAccess.GuarantorDetails(dt);
            return objResult;
        }
        public string GetSecurityName(int SecurityCode)
        {

            string SecurityName = _LoanDisbursementDataAccess.GetSecurityName(SecurityCode);
            return SecurityName;
        }
        public ResultDto Delete(int LoanMasterId, int userId)
        {
            ResultDto resultDto = new ResultDto();
            string obectName = "GroupLoanDisbursement";

            try
            {
                ObjectParameter prmLoanMasterId = new ObjectParameter("LoanmasterId", LoanMasterId);
                ObjectParameter prmLoanCode = new ObjectParameter("LoanCode", string.Empty);

                int effectedCount = _dbContext.uspLoanDisbursementDelete(prmLoanMasterId, prmLoanCode, userId);

                resultDto.ObjectId = (int)prmLoanMasterId.Value;
                resultDto.ObjectCode = (string)prmLoanCode.Value;

                if (resultDto.ObjectId > 0)
                    resultDto.Message = string.Format("{0} : {1} details deleted successfully", obectName, resultDto.ObjectCode);
                else
                    resultDto.Message = string.Format("Error occured while deleting {0} details", obectName);
            }
            catch (Exception)
            {
                resultDto.Message = string.Format("Service layer error occured while deleting the {0} details", obectName);
                resultDto.ObjectId = -98;

            }
            return resultDto;
        }

        public ResultDto ChangeStatus(int LoanmasterId, int userId)
        {
            ResultDto resultDto = new ResultDto();
            string obectName = "GroupLoanDisbursement";
            string statusCode = string.Empty;
            try
            {
                ObjectParameter prmLoanmasterId = new ObjectParameter("LoanmasterId", LoanmasterId);
                ObjectParameter prmLoanCode = new ObjectParameter("LoanCode", string.Empty);
                ObjectParameter prmStatusCode = new ObjectParameter("StatusCode", string.Empty);

                int effectedCount = _dbContext.uspLoanDisbursementChangeStatus(prmLoanmasterId, prmLoanCode, prmStatusCode, userId);

                resultDto.ObjectId = (int)prmLoanmasterId.Value;
                resultDto.ObjectCode = (string)prmLoanCode.Value;
                statusCode = (string)prmStatusCode.Value;

                if (resultDto.ObjectId > 0)
                    resultDto.Message = string.Format("{0} : {1} details {2} successfully", obectName, resultDto.ObjectCode, statusCode == Constants.StatusCodes.Active ? "activated" : "inactivated");
                else
                    resultDto.Message = string.Format("Error occured while {0} {1} details", statusCode == Constants.StatusCodes.Active ? "activated" : "inactivated", obectName);
            }
            catch (Exception)
            {
                resultDto.Message = string.Format("Service layer error occured while {0} {1} details", statusCode == Constants.StatusCodes.Active ? "activated" : "inactivated", obectName);
                resultDto.ObjectId = -98;

            }
            return resultDto;
        }


        public ScheduleDTO GetDisbusementDate(int LoanMasterId)
        {
            ObjectParameter prmAccountMasterId = new ObjectParameter("LoanMasterId", LoanMasterId);
            var results = new MFISDBContext()
                .MultipleResults(MFIEntityFrameWork.CustomProcNames.GetDisbursementDatebyLoanMasterID, prmAccountMasterId)
                .With<ScheduleDTO>()
                .Execute();
            var ScheduleDto = (results[0] as List<ScheduleDTO>)[0];
            return ScheduleDto;
        }

        public ResultDto ValidateSchedule(int LoanMasterID)
        {
            LoanDisbursementDataAccess obj = new LoanDisbursementDataAccess();
            return obj.ValidateSchedule(LoanMasterID);
        }

        public DisbursementVoucherDto GetDisbursementVoucher(int LoanMasterID)
        {
            return new LoanDisbursementDataAccess().GetDisbursementVoucher(LoanMasterID);
        }
    }
}
