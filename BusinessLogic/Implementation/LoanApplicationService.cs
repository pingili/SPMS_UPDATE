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

namespace BusinessLogic
{
    public class LoanApplicationService 
    {
        #region Gobal Variables

        private readonly MFISDBContext _dbContext;
        private readonly CommonService _commonService;

        public LoanApplicationService()
        {
            _dbContext = new MFISDBContext();

            _commonService = new CommonService();

            AutoMapperEntityConfiguration.Configure();
        }

        #endregion
        public ResultDto Insert(LoanApplicationDto LoanApplication)
        {
            return InsertupdateLoanApplication(LoanApplication);
        }

        public ResultDto Update(LoanApplicationDto LoanApplication)
        {
            return InsertupdateLoanApplication(LoanApplication);
        }
        private ResultDto InsertupdateLoanApplication(LoanApplicationDto LoanApplication)
        {
            ResultDto resultDto = new ResultDto();
            LoanApplication.LoanType = "G";
            LoanApplication.Mode = 1045;
            LoanApplication.MemberID = 1;
            string obectName = "Loan Application";
            try
            {
                ObjectParameter paramLoanMasterID = new ObjectParameter("LoanMasterID", LoanApplication.LoanMasterId);
                ObjectParameter paramloanCode = new ObjectParameter("LoanCode", string.Empty);

                int count = _dbContext.uspLoanApplicationInsertUpdate(paramLoanMasterID, paramloanCode, LoanApplication.LoanType, LoanApplication.MemberID, LoanApplication.GroupID,
                    LoanApplication.LoanApplicationDate, LoanApplication.LoanPurpose, LoanApplication.FundSourceId, LoanApplication.ProjectID, LoanApplication.LoanAmountApplied, LoanApplication.NoofInstallmentsProposed, LoanApplication.Mode, LoanApplication.UserID, LoanApplication.InterestMasterID);

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
    }
}
