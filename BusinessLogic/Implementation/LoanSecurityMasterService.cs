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
    public class LoanSecurityMasterService 
    {
        private readonly MFISDBContext _dbContext;
        private readonly CommonService _commonService;

        public LoanSecurityMasterService()
        {
            _dbContext = new MFISDBContext();
            _commonService = new CommonService();
            AutoMapperEntityConfiguration.Configure();
        }
        public List<LoanSecurityMasterDto> GetAll()
        {
            throw new NotImplementedException();
        }
        public List<LoanSecurityMasterLookupDto> Lookup()
        {
            List<LoanSecurityMasterLookupDto> lstLoanSecurityMasterDto = new List<LoanSecurityMasterLookupDto>();
            List<uspLoanSecurityMasterLookup_Result> lstuspLoanSecurityMasterLookup_Result = _dbContext.uspLoanSecurityMasterLookup().ToList();
            foreach (var Loansecurity in lstuspLoanSecurityMasterLookup_Result)
            {
                LoanSecurityMasterLookupDto loansecurityMasterDto = Mapper.Map<uspLoanSecurityMasterLookup_Result, LoanSecurityMasterLookupDto>(Loansecurity);
                lstLoanSecurityMasterDto.Add(loansecurityMasterDto);
            }
            return lstLoanSecurityMasterDto;
        }
        public LoanSecurityMasterDto GetByID(int LoanSecurityID)
        {
            List<uspLoanSecurityMasterGetByLoanSecurityID_Result> lstuspLoanSecurityGetByLoanSecurityID_Result = _dbContext.uspLoanSecurityMasterGetByLoanSecurityID(LoanSecurityID).ToList();
            var loanSecuriyMaseterDto = Mapper.Map<uspLoanSecurityMasterGetByLoanSecurityID_Result, LoanSecurityMasterDto>(lstuspLoanSecurityGetByLoanSecurityID_Result.FirstOrDefault());
            return loanSecuriyMaseterDto;
        }
        public ResultDto Insert(LoanSecurityMasterDto loansecurity)
        {
            return InsertUpdateLoanSecurity(loansecurity);
        }

        public ResultDto Update(LoanSecurityMasterDto loansecurity)
        {
            return InsertUpdateLoanSecurity(loansecurity);

        }
        private ResultDto InsertUpdateLoanSecurity(LoanSecurityMasterDto loansecurity)
        {
            ResultDto resultDto = new ResultDto();
            string obectName = "Loan Security";

            try
            {
                ObjectParameter paramLoanSecurityID = new ObjectParameter("LoanSecurityID", loansecurity.LoanSecurityID);
                ObjectParameter paramLoanSecurityCode = new ObjectParameter("LoanSecurityCode", string.Empty);
                int count = _dbContext.uspLoanSecurityMasterInsertUpdate(paramLoanSecurityID, loansecurity.Type, loansecurity.LoanSecurityName, loansecurity.UserID, paramLoanSecurityCode);
                resultDto.ObjectId = (int)paramLoanSecurityID.Value;
                resultDto.ObjectCode = string.IsNullOrEmpty((string)paramLoanSecurityCode.Value) ? loansecurity.LoanSecurityCode : (string)paramLoanSecurityCode.Value;

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
        public List<SelectListDto> GetLoanSecurityMasterSelectList()
        {
            List<SelectListDto> lstSelectListDto = new List<SelectListDto>();
            List<uspLoanSecurityMasterGetAll_Result> lstuspLoanSecurityMasterGetAll_Result = _dbContext.uspLoanSecurityMasterGetAll().ToList();
            foreach (var loansecurity in lstuspLoanSecurityMasterGetAll_Result)
            {
                SelectListDto objSelectListDto = new SelectListDto()
                {
                    ID = loansecurity.LoanSecurityID,
                    Text = loansecurity.LoanSecurityName
                };
                lstSelectListDto.Add(objSelectListDto);
            }
            return lstSelectListDto;
        }

        public ResultDto Delete(int loanSecurityId, int userId)
        {
            ResultDto resultDto = new ResultDto();
            string obectName = "LoanSecurityMaster";

            try
            {
                ObjectParameter prmLoanSecurityId = new ObjectParameter("LoanSecurityId", loanSecurityId);
                ObjectParameter prmLoanSecurityCode = new ObjectParameter("LoanSecurityCode", string.Empty);

                int effectedCount = _dbContext.uspLoanSecurityMasterDelete(prmLoanSecurityId, prmLoanSecurityCode, userId);

                resultDto.ObjectId = (int)prmLoanSecurityId.Value;
                resultDto.ObjectCode = (string)prmLoanSecurityCode.Value;

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

        public ResultDto ChangeStatus(int loanSecurityId, int userId)
        {
            ResultDto resultDto = new ResultDto();
            string obectName = "LoanSecurityMaster";
            string statusCode = string.Empty;
            try
            {
                ObjectParameter prmLoanSecurityId = new ObjectParameter("LoanSecurityID", loanSecurityId);
                ObjectParameter prmLoanSecurityCode = new ObjectParameter("LoanSecurityCode", string.Empty);
                ObjectParameter prmStatusCode = new ObjectParameter("StatusCode", string.Empty);

                int effectedCount = _dbContext.uspLoanSecurityMasterChangeStatus(prmLoanSecurityId, prmLoanSecurityCode, prmStatusCode, userId);

                resultDto.ObjectId = (int)prmLoanSecurityId.Value;
                resultDto.ObjectCode = (string)prmLoanSecurityCode.Value;
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
    }
}
