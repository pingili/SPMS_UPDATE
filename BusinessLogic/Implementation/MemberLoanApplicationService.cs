using AutoMapper;
using BusinessEntities;
using BusinessLogic.AutoMapper;
using DataLogic;
using MFIEntityFrameWork;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using Utilities;

namespace BusinessLogic
{
    public class MemberLoanApplicationService
    {
        #region Gobal Variables

        private readonly MFISDBContext _dbContext;
        private readonly CommonService _commonService;

        public MemberLoanApplicationService()
        {
            _dbContext = new MFISDBContext();

            _commonService = new CommonService();

            AutoMapperEntityConfiguration.Configure();
        }

        #endregion
        public ResultDto Insert(MemberLoanApplicationDto MemberLoanApplication)
        {
            return InsertUpdateMemberLoanApplicationOld(MemberLoanApplication);
        }

        public ResultDto Update(BusinessEntities.MemberLoanApplicationDto MemberLoanApplication)
        {
            return InsertUpdateMemberLoanApplicationOld(MemberLoanApplication);
        }
        private ResultDto InsertUpdateMemberLoanApplicationOld(MemberLoanApplicationDto MemberLoanApplication)
        {
            ResultDto resultDto = new ResultDto();
            MemberLoanApplication.LoanType = "M";
            string obectName = "Member Loan Application";
            try
            {
                ObjectParameter paramLoanMasterID = new ObjectParameter("LoanMasterID", MemberLoanApplication.LoanMasterId);
                ObjectParameter paramloanCode = new ObjectParameter("LoanCode", string.Empty);

                int count = _dbContext.uspLoanApplicationInsertUpdate(paramLoanMasterID, paramloanCode, MemberLoanApplication.LoanType, MemberLoanApplication.MemberID, MemberLoanApplication.GroupID,
                    MemberLoanApplication.LoanApplicationDate, MemberLoanApplication.LoanPurpose, MemberLoanApplication.FundSourceId, MemberLoanApplication.ProjectID, MemberLoanApplication.LoanAmountApplied, MemberLoanApplication.NoofInstallmentsProposed, MemberLoanApplication.Mode, MemberLoanApplication.UserID, MemberLoanApplication.InterestMasterID);

                resultDto.ObjectId = (int)paramLoanMasterID.Value;
                resultDto.ObjectCode = string.IsNullOrEmpty((string)paramloanCode.Value) ? MemberLoanApplication.LoanCode : (string)paramloanCode.Value;

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
        public List<MemberLoanApplicationLookupDto> Lookup(int GroupID, int userId)
        {
            MemberLoanDisbursementDataAccess objMemberLoandal = new MemberLoanDisbursementDataAccess();
            return objMemberLoandal.GetMemberLoanLookup(GroupID, userId);
        }

        public MemberLoanApplicationDto GetMemberLoanApplicationDetailsById(int LoanMasterId)
        {
            MemberLoanDisbursementDataAccess objMemberLoandal = new MemberLoanDisbursementDataAccess();
            return objMemberLoandal.GetMemberLoanApplicationDetailsById(LoanMasterId);
        }

        public MemberLoanDisbursementDto GetMemberLoanDisbursementDetailsById(int loanMasterId, int userId)
        {
            MemberLoanDisbursementDataAccess objMemberLoandal = new MemberLoanDisbursementDataAccess();
            return objMemberLoandal.GetMemberLoanDisbursementDetailsById(loanMasterId, userId);
        }
        public MemberLoanClosure GetLoanClosureDemands(int loanMasterId, DateTime transactionDate)
        {
            MemberLoanDisbursementDataAccess objMemberLoandal = new MemberLoanDisbursementDataAccess();
            return objMemberLoandal.GetLoanClosureDemands(loanMasterId, transactionDate);
        }
        

        public int SubmitLoanApplicationApproval(MemberLoanApprovalDto objLoanApproval, int userId, int loanMasterId, bool isSave = false)
        {
            MemberLoanDisbursementDataAccess objMemberLoandal = new MemberLoanDisbursementDataAccess();
            return objMemberLoandal.SubmitLoanApplicationApproval(objLoanApproval, userId, loanMasterId, isSave);
        }

        public ResultDto Delete(int LoanMasterId, int userId)
        {
            ResultDto resultDto = new ResultDto();
            string obectName = "MemberLoanApplication";

            try
            {
                ObjectParameter prmLoanMasterId = new ObjectParameter("LoanMasterId", LoanMasterId);
                ObjectParameter prmLoanCode = new ObjectParameter("LoanCode", string.Empty);

                int effectedCount = _dbContext.uspMemberLoanApplicationDelete(prmLoanMasterId, prmLoanCode, userId);

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
            string obectName = "MemberLoanApplication";
            string statusCode = string.Empty;
            try
            {
                ObjectParameter prmLoanMasterId = new ObjectParameter("LoanMasterId", LoanMasterId);
                ObjectParameter prmLoanCode = new ObjectParameter("LoanCode", string.Empty);
                ObjectParameter prmStatusCode = new ObjectParameter("StatusCode", string.Empty);

                int effectedCount = _dbContext.uspMemberLoanApplicationChangeStatus(prmLoanMasterId, prmLoanCode, prmStatusCode, userId);

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

        public MemberLoanApplicationViewDto GetMemberLoanApplicationViewDetails(int loanMasterId)
        {
            MemberLoanDisbursementDataAccess objMemberLoandal = new MemberLoanDisbursementDataAccess();
            return objMemberLoandal.GetMemberLoanApplicationViewDetails(loanMasterId);
        }

        public ResultDto ApproveMemberLoanApplication(int loanMasterId, string loanCode, Enums.ApprovalActions actionType, string approvalComments, int userId)
        {
            MemberLoanDisbursementDataAccess objMemberLoandal = new MemberLoanDisbursementDataAccess();
            int retVal = objMemberLoandal.ApproveMemberLoanApplication(loanMasterId, actionType, approvalComments, userId);
            ResultDto res = new ResultDto();

            res.ObjectId = retVal;

            if (retVal > 0)
            {
                res.Message = "Loan {" + loanCode + "} " + (actionType == Enums.ApprovalActions.APP ? "Approved" : actionType == Enums.ApprovalActions.CAN ? "Cancelled" : "Rejected") + " successfully.";
            }
            else
            {
                res.Message = "Unable to process your request";
            }

            return res;
        }

        public List<LoanAccountHeadDto> GetGroupLoanDepositAccountHeads(int GroupId, int groupInterestId = 0 , string type = "L")
        {
            MemberLoanDisbursementDataAccess objMemberLoandal = new MemberLoanDisbursementDataAccess();
            return objMemberLoandal.GetGroupLoanDepositAccountHeads(GroupId, groupInterestId, type);
        }

        public ResultDto InsertUpdateMemberLoanApplication(MemberLoanApplicationDto objMemberLoan)
        {
            MemberLoanDisbursementDataAccess objMemberLoandal = new MemberLoanDisbursementDataAccess();
            return objMemberLoandal.InsertUpdateMemberLoanApplication(objMemberLoan);
        }

        public ResultDto SaveMemberLoanDisbursementDetails(MemberLoanDisbursementDto objMemberLoanDisbursement, int userId)
        {
            MemberLoanDisbursementDataAccess objMemberLoandal = new MemberLoanDisbursementDataAccess();
            return objMemberLoandal.SaveMemberLoanDisbursementDetails(objMemberLoanDisbursement, userId);
        }

        public ResultDto ConfirmAndDisburseMemberLoan(int loanMasterId, int groupId, int userId)
        {
            MemberLoanDisbursementDataAccess objMemberLoandal = new MemberLoanDisbursementDataAccess();
            return objMemberLoandal.ConfirmAndDisburseMemberLoan(loanMasterId, groupId, userId);
            
        }
    }
}
