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

namespace BusinessLogic
{
    public class LoanPurposeService 
    {
        #region Gobal Variables

        private readonly MFISDBContext _dbContext;
        private readonly CommonService _commonService;

        public LoanPurposeService()
        {
            _dbContext = new MFISDBContext();

            _commonService = new CommonService();

            AutoMapperEntityConfiguration.Configure();
        }

        #endregion

        public List<LoanPurposeDto> GetAll()
        {
            throw new NotImplementedException();
        }
        public List<LoanPurposeLookupDto> Lookup()
        {
            List<LoanPurposeLookupDto> lstLoanPurposeLookupDto = new List<LoanPurposeLookupDto>();
            List<uspLoanPurposeLookup_Result> lstuspLoanPurposeGetAll_Result = _dbContext.uspLoanPurposeLookup().ToList();
            foreach (var Loanpurpose in lstuspLoanPurposeGetAll_Result)
            {
                LoanPurposeLookupDto loanpurposeDto = Mapper.Map<uspLoanPurposeLookup_Result, LoanPurposeLookupDto>(Loanpurpose);
                lstLoanPurposeLookupDto.Add(loanpurposeDto);
            }
            return lstLoanPurposeLookupDto;
        }

        public LoanPurposeDto GetByID(int loanPurposeId)
        {
            List<uspLoanPurposeGetByLoanPurposeId_Result> lstuspLoanPurposeGetByLoanPurposeId_Result = _dbContext.uspLoanPurposeGetByLoanPurposeId(loanPurposeId).ToList();
            var loanPurposeDto = Mapper.Map<uspLoanPurposeGetByLoanPurposeId_Result, LoanPurposeDto>(lstuspLoanPurposeGetByLoanPurposeId_Result.FirstOrDefault());
            return loanPurposeDto;
        }

        public ResultDto Insert(LoanPurposeDto loanPurpose)
        {
            return InsertUpdateLoanPurpose(loanPurpose);
        }

        public ResultDto Update(LoanPurposeDto loanPurpose)
        {
            return InsertUpdateLoanPurpose(loanPurpose);

        }
        private ResultDto InsertUpdateLoanPurpose(LoanPurposeDto loanPurpose)
        {
            ResultDto resultDto = new ResultDto();
            string obectName = "Loan Purpose";
            try
            {
                ObjectParameter paramloanPurposeId = new ObjectParameter("LoanPurposeId", loanPurpose.LoanPurposeID);
                ObjectParameter paramloanPurposeCode = new ObjectParameter("LoanPurposeCode", string.Empty);

                int count = _dbContext.uspLoanPurposeInsertUpdate(paramloanPurposeId, loanPurpose.Category, loanPurpose.Project,loanPurpose.Purpose, loanPurpose.TELoanPurpose, loanPurpose.UserID, paramloanPurposeCode);

                resultDto.ObjectId = (int)paramloanPurposeId.Value;
                resultDto.ObjectCode = string.IsNullOrEmpty((string)paramloanPurposeCode.Value) ? loanPurpose.LoanPurposeCode : (string)paramloanPurposeCode.Value;

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
        public List<SelectListDto> GetLoanPurposeSelectList()
        {
            List<SelectListDto> lstSelectListDto = new List<SelectListDto>();
            List<uspLoanPurposeGetAll_Result> lstuspLoanPurposeGetAll_Result = _dbContext.uspLoanPurposeGetAll().ToList();
            foreach (var Loan in lstuspLoanPurposeGetAll_Result)
            {
                SelectListDto objSelectListDto = new SelectListDto()
                {
                    ID = Loan.LoanPurposeID,
                    Text = Loan.Purpose
                };
                lstSelectListDto.Add(objSelectListDto);
            }
            return lstSelectListDto;
        }
        public ResultDto Delete(int loanpurposeId, int userId)
        {
            ResultDto resultDto = new ResultDto();
            string obectName = "LoanPurpose";

            try
            {
                ObjectParameter prmloanpurposeId = new ObjectParameter("LoanpurposeId", loanpurposeId);
                ObjectParameter prmLoanpurposeCode = new ObjectParameter("LoanPurposeCode", string.Empty);

                int effectedCount = _dbContext.uspLoanPurposeDelete(prmloanpurposeId, prmLoanpurposeCode, userId);

                resultDto.ObjectId = (int)prmloanpurposeId.Value;
                resultDto.ObjectCode = (string)prmLoanpurposeCode.Value;

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

        public ResultDto ChangeStatus(int loanpurposeId, int userId)
        {
            ResultDto resultDto = new ResultDto();
            string obectName = "LoanPurpose";
            string statusCode = string.Empty;
            try
            {
                ObjectParameter prmLoanPurposeId = new ObjectParameter("LoanPurposeID", loanpurposeId);
                ObjectParameter prmLoanPurposeCode = new ObjectParameter("LoanPurposeCode", string.Empty);
                ObjectParameter prmStatusCode = new ObjectParameter("StatusCode", string.Empty);

                int effectedCount = _dbContext.uspLoanPurposeChangeStatus(prmLoanPurposeId, prmLoanPurposeCode, prmStatusCode, userId);

                resultDto.ObjectId = (int)prmLoanPurposeId.Value;
                resultDto.ObjectCode = (string)prmLoanPurposeCode.Value;
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

        public List<LoanPurposeDto> GetLoanPurposeByProjectID(int ProjectID)
        {
            List<LoanPurposeDto> lstLoanPurposeDto = new List<LoanPurposeDto>();
            LoanPurposeDto loanPurposDto = null;
            List<uspLoanPurposeDetailsByProjectID_Result> lstuspLoanPurposeDetailsByProjectID_Result = _dbContext.uspLoanPurposeDetailsByProjectID(ProjectID).ToList();
            foreach (var loanPurpose in lstuspLoanPurposeDetailsByProjectID_Result)
            {
                loanPurposDto = new LoanPurposeDto();
                loanPurposDto.LoanPurposeID = loanPurpose.LoanPurposeID;
                loanPurposDto.LoanPurposeCode = loanPurpose.LoanPurposeCode;
                loanPurposDto.Purpose = loanPurpose.Purpose;
                lstLoanPurposeDto.Add(loanPurposDto);
            }
            return lstLoanPurposeDto;
        }
    }
}