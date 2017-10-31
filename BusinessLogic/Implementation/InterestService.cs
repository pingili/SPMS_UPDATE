using BusinessLogic.AutoMapper;
using AutoMapper;
//using BusinessLogic.Interface;
using MFIEntityFrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntities;
using System.Data.Entity.Core.Objects;
using CoreComponents;
using Utilities;
using System.Data.SqlClient;
using DataLogic;

namespace BusinessLogic
{
    public class InterestService 
    {
        #region Global Variables
        private readonly MFISDBContext _dbContext;
        private readonly CommonService _commonService;

        public InterestService()
        {
            _dbContext = new MFISDBContext();
            AutoMapperEntityConfiguration.Configure();
            _commonService = new CommonService();
        }
        #endregion Global Variables

        public List<InterestLookupDto> GetLookup(Enums.InterestTypes type)
        {
            var lstInterestLookupDto = new List<InterestLookupDto>();
            var uspInterestmasterLookupResults = _dbContext.uspInterestLookup(type.ToString()).ToList();

            foreach (var interest in uspInterestmasterLookupResults)
            {
                InterestLookupDto lookupDto = Mapper.Map<uspInterestLookup_Result, InterestLookupDto>(interest);
                lstInterestLookupDto.Add(lookupDto);
            }

            return lstInterestLookupDto;
        }

        public List<InterestMasterDto> GetAll(Enums.InterestTypes type)
        {
            List<uspInterestGetAll_Result> lstuspInterestGetAll_Result = _dbContext.uspInterestGetAll().ToList();
            List<InterestMasterDto> lstInterestMasterDto = new List<InterestMasterDto>();
            foreach (var result in lstuspInterestGetAll_Result)
            {
                if (
                    (type == Enums.InterestTypes.D && result.InterestCode.ToUpper().StartsWith("DIR-")) ||
                    (type == Enums.InterestTypes.L && result.InterestCode.ToUpper().StartsWith("LIR-"))
                    )
                    lstInterestMasterDto.Add(Mapper.Map<uspInterestGetAll_Result, InterestMasterDto>(result));
            }
            return lstInterestMasterDto;
        }
        public InterestMasterDto GetByID(int interestID)
        {
            var prmInterestId = new ObjectParameter("InterestID", interestID);

            var results = new MFISDBContext()
                .MultipleResults(MFIEntityFrameWork.CustomProcNames.uspInterestByID, prmInterestId)
                .With<InterestMasterDto>()
                .With<InterestRatesDto>()
                .Execute();

            var interestMasterDto = (results[0] as List<InterestMasterDto>)[0];
            var interestRatesDtoList = results[1] as List<InterestRatesDto>;

            interestMasterDto.InterestRates = interestRatesDtoList;

            return interestMasterDto;
        }

        public ResultDto Insert(InterestMasterDto interest, Enums.InterestTypes type)
        {
            return InsertUpdateInterest(interest, type);
        }

        public ResultDto Update(InterestMasterDto interest, Enums.InterestTypes type)
        {
            return InsertUpdateInterest(interest, type);
        }

        private ResultDto InsertUpdateInterest(InterestMasterDto interest, Enums.InterestTypes type)
        {
            ResultDto resultDto = new ResultDto();
            string objectName = type.ToString() == "D" ? "deposit interest" : "loan interest";

            try
            {
                string interestratexml = CommonMethods.SerializeListDto<List<InterestRatesDto>>(interest.InterestRates);

                ObjectParameter paraminterestID = new ObjectParameter("InterestID", interest.InterestID);
                ObjectParameter paraminterestCode = new ObjectParameter("InterestCode", string.Empty);

                int effectedcount = _dbContext.uspInterestInsertUpdate(paraminterestID, interest.InterestName, interest.PrincipalAHID, interest.InterestAHID, interest.PenalAHID, interest.Base, interest.CaluculationMethod, type.ToString(), interest.UserId, interestratexml, paraminterestCode);

                resultDto.ObjectId = (int)paraminterestID.Value;
                resultDto.ObjectCode = string.IsNullOrEmpty((string)paraminterestCode.Value) ? interest.InterestCode : (string)paraminterestCode.Value;

                if (resultDto.ObjectId > 0)
                    resultDto.Message = string.Format("{0} details saved successfully with code : {1}", objectName, resultDto.ObjectCode);
                else if (resultDto.ObjectId == -1)
                    resultDto.Message = string.Format("Error occured while generating {0} code", objectName);
                else
                    resultDto.Message = string.Format("Error occured while saving {0} details", objectName);
            }
            catch (Exception)
            {
                resultDto.Message = string.Format("Service layer error occured while saving the {0} details", objectName);
                resultDto.ObjectId = -98;

            }
            return resultDto;
        }

        public List<SelectListDto> GetInterestsSelectList()
        {
            List<SelectListDto> lstSelectListDto = new List<SelectListDto>();
            List<uspInterestGetAll_Result> lstuspInterestGetAll_Result = _dbContext.uspInterestGetAll().ToList();
            foreach (var interest in lstuspInterestGetAll_Result)
            {
                if (!interest.InterestCode.ToUpper().StartsWith("DIR-"))
                {
                    SelectListDto objSelectListDto = new SelectListDto()
                    {
                        ID = interest.InterestID,
                        Text = interest.InterestName
                    };
                    lstSelectListDto.Add(objSelectListDto);
                }
            }
            return lstSelectListDto;
        }
        public List<SelectListDto> GetInterestsSelectList(int GroupId)
        {
            List<SelectListDto> lstSelectListDto = new List<SelectListDto>();
           
            List<uspGroupInterestGetAll_Result> lstuspInterestGetAll_Result = _dbContext.uspGroupInterestGetAll(GroupId).ToList();
            foreach (var interest in lstuspInterestGetAll_Result)
            {
                if (!interest.Type.ToUpper().StartsWith("D"))
                {
                    SelectListDto objSelectListDto = new SelectListDto()
                    {
                        ID = interest.GroupInterestID,
                        Text = interest.InterestName
                    };
                    lstSelectListDto.Add(objSelectListDto);
                }
            }
            return lstSelectListDto;
        }

        public List<SelectListDto> GetDepositInterestsSelectList(int GroupId)
        {
            List<SelectListDto> lstSelectListDto = new List<SelectListDto>();
            List<uspGroupInterestGetAll_Result> lstuspInterestGetAll_Result = _dbContext.uspGroupInterestGetAll(GroupId).ToList();
            foreach (var interest in lstuspInterestGetAll_Result)
            {
                if (interest.Type.ToUpper().StartsWith("D"))
                {
                    SelectListDto objSelectListDto = new SelectListDto()
                    {
                        ID = interest.GroupInterestID,
                        Text = interest.InterestName
                    };
                    lstSelectListDto.Add(objSelectListDto);
                }
            }
            return lstSelectListDto;
        }

        public List<SelectListDto> GetDepositInterestsSelectList()
        {
            List<SelectListDto> lstSelectListDto = new List<SelectListDto>();
            List<uspInterestGetAll_Result> lstuspInterestGetAll_Result = _dbContext.uspInterestGetAll().ToList();
            foreach (var interest in lstuspInterestGetAll_Result)
            {
                if (interest.InterestCode.ToUpper().StartsWith("DIR-"))
                {
                    SelectListDto objSelectListDto = new SelectListDto()
                    {
                        ID = interest.InterestID,
                        Text = interest.InterestName
                    };
                    lstSelectListDto.Add(objSelectListDto);
                }
            }
            return lstSelectListDto;
        }

        public ResultDto ChangeStatus(int interestID, int userId)
        {
            ResultDto resultDto = new ResultDto();
            string obectName = "Interest";
            string statusCode = string.Empty;
            try
            {
                ObjectParameter prmInterestID = new ObjectParameter("InterestID", interestID);
                ObjectParameter prmInterestCode = new ObjectParameter("InterestCode", string.Empty);
                ObjectParameter prmStatusCode = new ObjectParameter("StatusCode", string.Empty);

                int effectedCount = _dbContext.uspInterestChangeStatus(prmInterestID, prmInterestCode, prmStatusCode, userId);

                resultDto.ObjectId = (int)prmInterestID.Value;
                resultDto.ObjectCode = (string)prmInterestCode.Value;
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
        public InterestViewDto GetLoanViewByID(int interestId)
        {
            var prminterestId = new ObjectParameter("InterestID", interestId);

            var results = new MFISDBContext()
                .MultipleResults(MFIEntityFrameWork.CustomProcNames.uspLoanInterestGetViewByID, prminterestId)
                .With<InterestViewDto>()
                .With<InterestRatesDto>()
                .Execute();

            var interestViewDto = (results[0] as List<InterestViewDto>)[0];
            var interestRatesDtoList = results[1] as List<InterestRatesDto>;

            interestViewDto.InterestRates = interestRatesDtoList;

            return interestViewDto;
        }
        public InterestViewDto GetViewByID(int interestId)
        {
            var prminterestId = new ObjectParameter("InterestID", interestId);

            var results = new MFISDBContext()
                .MultipleResults(MFIEntityFrameWork.CustomProcNames.uspDepositInterestGetViewByID, prminterestId)
                .With<InterestViewDto>()
                .With<InterestRatesDto>()
                .Execute();

            var interestViewDto = (results[0] as List<InterestViewDto>)[0];
            var interestRatesDtoList = results[1] as List<InterestRatesDto>;

            interestViewDto.InterestRates = interestRatesDtoList;

            return interestViewDto;
        }


        public InterestMasterDto GetByIDExt(int id)
        {
            return new InterestMasterDll().GetByID(id);
        }
        public InterestMasterDto GetByIDFedExt(int id)
        {
            return new InterestMasterDll().FedGetByID(id);
        }

        public InterestMasterDto GetGroupInterestByID(int id)
        {
            return new InterestMasterDll().GetGroupInterestByID(id);
        }
    }
}
