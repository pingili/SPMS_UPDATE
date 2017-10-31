using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BusinessEntities;
using BusinessLogic.AutoMapper;
//using BusinessLogic.Interface;
using MFIEntityFrameWork;
using System.Data.Entity.Core.Objects;
using Utilities;

namespace BusinessLogic
{
    public class FundSourceService 
    {
        private readonly MFISDBContext _dbContext;

        public FundSourceService()
        {
            _dbContext = new MFISDBContext();
            AutoMapperEntityConfiguration.Configure();
        }

        public List<FundSourceDto> GetAll()
        {
            List<uspFundSourceGetAll_Result> lstuspFundSourceGetAll_Result = _dbContext.uspFundSourceGetAll().ToList();
            List<FundSourceDto> LstFundSourceDto = new List<FundSourceDto>();
            foreach (var fundsource in lstuspFundSourceGetAll_Result)
            {
                LstFundSourceDto.Add(Mapper.Map<uspFundSourceGetAll_Result, FundSourceDto>(fundsource));
            }
            return LstFundSourceDto;
        }

        public List<FundSourceLookup> Lookup()
        {
            List<FundSourceLookup> lstFundsourceLookup = new List<FundSourceLookup>();
            List<uspFundSourceLookup_Result> lstuspFundSourceLookup_Result = _dbContext.uspFundSourceLookup().ToList();
            foreach (var fundsource in lstuspFundSourceLookup_Result)
            {
                lstFundsourceLookup.Add(Mapper.Map<uspFundSourceLookup_Result, FundSourceLookup>(fundsource));

            }
            return lstFundsourceLookup;
        }
        
        public FundSourceDto GetByID(int fundsourceID)
        {
            List<uspFundSourceGetByFundSourceId_Result> lstuspfundsourceGetByFundsourceId_result = _dbContext.uspFundSourceGetByFundSourceId(fundsourceID).ToList();
            FundSourceDto fundsourceDto = Mapper.Map<uspFundSourceGetByFundSourceId_Result, FundSourceDto>(lstuspfundsourceGetByFundsourceId_result.FirstOrDefault());
            return fundsourceDto;
        }

        public ResultDto Insert(FundSourceDto fundsource)
        {
            return InsertUpdate(fundsource);
        }

        public ResultDto Update(FundSourceDto fundsource)
        {
            return InsertUpdate(fundsource);
        }

        private ResultDto InsertUpdate(FundSourceDto fundsource)
        {
            ResultDto resultDto = new ResultDto();
            string obectName = "fundsource";

            try
            {
                ObjectParameter prmFundSourceID = new ObjectParameter("FundSourceID", fundsource.FundSourceID);
                ObjectParameter prmFundSourceCode = new ObjectParameter("FundSourceCode", string.Empty);
                int effectedCount = _dbContext.uspFundSourceInsertUpdate(prmFundSourceID, fundsource.FundSourceName,fundsource.TEFundSourseName, fundsource.UserId, prmFundSourceCode);

                resultDto.ObjectId = (int)prmFundSourceID.Value;
                resultDto.ObjectCode = string.IsNullOrEmpty((string)prmFundSourceCode.Value) ? fundsource.FundSourceCode : (string)prmFundSourceCode.Value;

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

        public List<SelectListDto> GetFundSourceSelectList()
        {
            List<SelectListDto> lstSelectListDto = new List<SelectListDto>();
            List<uspFundSourceGetAll_Result> lstuspFundSourceGetAll_Result = _dbContext.uspFundSourceGetAll().ToList();
            foreach (var fundSource in lstuspFundSourceGetAll_Result)
            {
                SelectListDto objSelectListDto = new SelectListDto()
                {
                    ID = fundSource.FundSourceID,
                    Text = string.Format("{0} ( {1} )", fundSource.FundSourceName, fundSource.FundSourceCode)
                };
                lstSelectListDto.Add(objSelectListDto);
            }
            return lstSelectListDto;
        }

        public ResultDto Delete(int fundSourceId, int userId)
        {
            ResultDto resultDto = new ResultDto();
            string obectName = "fundsource";

            try
            {
                ObjectParameter prmFundSourceID = new ObjectParameter("FundSourceID", fundSourceId);
                ObjectParameter prmFundSourceCode = new ObjectParameter("FundSourceCode", string.Empty);

                int effectedCount = _dbContext.uspFundSourceDelete(prmFundSourceID, prmFundSourceCode, userId);

                resultDto.ObjectId = (int)prmFundSourceID.Value;
                resultDto.ObjectCode = (string)prmFundSourceCode.Value;

                if (resultDto.ObjectId > 0)
                    resultDto.Message = string.Format("{0} : {1} details deleted successfully", obectName, resultDto.ObjectCode);
                //else if (resultDto.ObjectId == -1)
                //    resultDto.Message = string.Format("selected {0} : {1} aleready used in other transaction, in order to delete please remove the dependencies", obectName, resultDto.ObjectCode);
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

        public ResultDto ChangeStatus(int fundSourceId, int userId)
        {
            ResultDto resultDto = new ResultDto();
            string obectName = "fundsource";
            string statusCode = string.Empty;
            try
            {
                ObjectParameter prmFundSourceID = new ObjectParameter("FundSourceID", fundSourceId);
                ObjectParameter prmFundSourceCode = new ObjectParameter("FundSourceCode", string.Empty);
                ObjectParameter prmStatusCode = new ObjectParameter("StatusCode", string.Empty);

                int effectedCount = _dbContext.uspFundSourceChangeStatus(prmFundSourceID, prmFundSourceCode,prmStatusCode, userId);

                resultDto.ObjectId = (int)prmFundSourceID.Value;
                resultDto.ObjectCode = (string)prmFundSourceCode.Value;
                 statusCode = (string)prmStatusCode.Value;

                if (resultDto.ObjectId > 0)
                    resultDto.Message = string.Format("{0} : {1} details {2} successfully", obectName, resultDto.ObjectCode, statusCode == Constants.StatusCodes.Active ? "activated" :"inactivated");
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

