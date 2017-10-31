using AutoMapper;
using BusinessEntities;
using BusinessLogic.AutoMapper;
using DataLogic;
//using BusinessLogic.Interface;
using MFIEntityFrameWork;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace BusinessLogic
{
   public class GroupLoanApplicationService 
    {
        #region Gobal Variables

        private readonly MFISDBContext _dbContext;
        private readonly CommonService _commonService;

        public GroupLoanApplicationService()
        {
            _dbContext = new MFISDBContext();

            _commonService = new CommonService();

            AutoMapperEntityConfiguration.Configure();
        }

        #endregion
        public ResultDto Insert(GroupLoanApplicationDto LoanApplication)
        {
            return InsertupdateLoanApplication(LoanApplication);
        }

        public ResultDto Update(GroupLoanApplicationDto LoanApplication)
        {
            return InsertupdateLoanApplication(LoanApplication);
        }
        private ResultDto InsertupdateLoanApplication(GroupLoanApplicationDto LoanApplication)
        {
            ResultDto resultDto = new ResultDto();
            LoanApplication.LoanType = "G";
            string obectName = "Loan Application";
            try
            {
                ObjectParameter paramLoanMasterID = new ObjectParameter("LoanMasterID", LoanApplication.LoanMasterId);
                ObjectParameter paramloanCode = new ObjectParameter("LoanCode", string.Empty);

                int count = _dbContext.uspLoanApplicationInsertUpdate(paramLoanMasterID, paramloanCode, LoanApplication.LoanType, LoanApplication.MemberID, LoanApplication.GroupID,
                    LoanApplication.LoanApplicationDate, LoanApplication.LoanPurpose, LoanApplication.FundSourse, LoanApplication.ProjectID, LoanApplication.LoanAmountApplied, LoanApplication.NoofInstallmentsProposed, LoanApplication.Mode, LoanApplication.UserID,LoanApplication.InterestMasterID);

                resultDto.ObjectId = (int)paramLoanMasterID.Value;
                resultDto.ObjectCode = string.IsNullOrEmpty((string)paramloanCode.Value) ? LoanApplication.LoanCode : (string)paramloanCode.Value;

                if (resultDto.ObjectId > 0)
                    resultDto.Message = string.Format("{0} details saved successfully with code : {1}", obectName, resultDto.ObjectCode);
                else if (resultDto.ObjectId == -1)
                    resultDto.Message = string.Format("Error occured while generating {0} code", obectName);
                else
                    resultDto.Message = string.Format("Error occured while saving {0} details", obectName);
            }
            catch (Exception)
            {
                resultDto.Message = string.Format("Service layer error occured while saving the {0} details", obectName);
                resultDto.ObjectId = -98;
            }
            return resultDto;
        }
        public List<GroupLoanApplicationLookupDto> Lookup()
        {
            List<GroupLoanApplicationLookupDto> lstgrouploanapplicationLookupDto = new List<GroupLoanApplicationLookupDto>();
            List<uspGroupLoanApplicationLookup_Result> lstuspgroupLoanApplicationLookup_Result = _dbContext.uspGroupLoanApplicationLookup().ToList();
            foreach (var grouploanapplication in lstuspgroupLoanApplicationLookup_Result)
            {
                GroupLoanApplicationLookupDto grouploanapplicationLookup = Mapper.Map<uspGroupLoanApplicationLookup_Result, GroupLoanApplicationLookupDto>(grouploanapplication);
                lstgrouploanapplicationLookupDto.Add(grouploanapplicationLookup);
            }
            return lstgrouploanapplicationLookupDto;
        }


        public GroupLoanApplicationDto GetByID(int LoanMasterId)
        {
            List<uspGroupLoanApplicationGetByLoanMasterID_Result> lstuspGroupLoanApplicationGetByLoanMasterID_Result = _dbContext.uspGroupLoanApplicationGetByLoanMasterID(LoanMasterId).ToList();
            var grouploanapplicationDto = Mapper.Map<uspGroupLoanApplicationGetByLoanMasterID_Result, GroupLoanApplicationDto>(lstuspGroupLoanApplicationGetByLoanMasterID_Result.FirstOrDefault());
            return grouploanapplicationDto;
        }


        public ResultDto Delete(int LoanMasterId, int userId)
        {
            ResultDto resultDto = new ResultDto();
            string obectName = "GroupLoanApplication";

            try
            {
                ObjectParameter prmLoanMasterId = new ObjectParameter("LoanMasterId", LoanMasterId);
                ObjectParameter prmLoanCode = new ObjectParameter("LoanCode", string.Empty);

                int effectedCount = _dbContext.uspGroupLoanApplicationDelete(prmLoanMasterId, prmLoanCode, userId);

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

        public ResultDto ChangeStatus(int LoanMasterId, int userId)
        {
            ResultDto resultDto = new ResultDto();
            string obectName = "GroupLoanApplication";
            string statusCode = string.Empty;
            try
            {
                ObjectParameter prmLoanMasterId = new ObjectParameter("LoanMasterId", LoanMasterId);
                ObjectParameter prmLoanCode = new ObjectParameter("LoanCode", string.Empty);
                ObjectParameter prmStatusCode = new ObjectParameter("StatusCode", string.Empty);

                int effectedCount = _dbContext.uspGroupLoanApplicationChangeStatus(prmLoanMasterId, prmLoanCode, prmStatusCode, userId);

                resultDto.ObjectId = (int)prmLoanMasterId.Value;
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


        public GroupLoanApplicationDto GetViewById(int loanmasterId)
        {
            List<uspGroupLoanApplicationGetViewByLoanMasterID_Result> lstuspGroupLoanApplicationGetByLoanMasterID_Result = _dbContext.uspGroupLoanApplicationGetViewByLoanMasterID(loanmasterId).ToList();
            var grouploanapplicationDto = Mapper.Map<uspGroupLoanApplicationGetViewByLoanMasterID_Result, GroupLoanApplicationDto>(lstuspGroupLoanApplicationGetByLoanMasterID_Result.FirstOrDefault());
            return grouploanapplicationDto;
        }

        public GroupLoanApplicationDto GetGroupApplicationDetailsByID(int loanMasterId)
        {
            return new LoanDisbursementDataAccess().GetGroupApplicationDetailsByID(loanMasterId);
        }
    }
}
