using BusinessEntities;
using BusinessLogic.AutoMapper;
using MFIEntityFrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using System.Data.Entity.Core.Objects;
using Utilities;
namespace BusinessLogic
{
    public class DistrictService 
    {
        private readonly MFISDBContext _dbContext;

        public DistrictService()
        {
            _dbContext = new MFISDBContext();
            AutoMapperEntityConfiguration.Configure();
        }

        public List<DistrictDto> GetAll()
        {
            List<uspDistrictGetAll_Result> lstuspDistrictGetAll_Result = _dbContext.uspDistrictGetAll().ToList();
            List<DistrictDto> lstDistrictDto = new List<DistrictDto>();
            foreach (var result in lstuspDistrictGetAll_Result)
                lstDistrictDto.Add(Mapper.Map<uspDistrictGetAll_Result, DistrictDto>(result));

            return lstDistrictDto;
        }
        public List<DistrictLookupDto> Lookup()
        {
            List<DistrictLookupDto> lstdistrictlookUpDto = new List<DistrictLookupDto>();
            List<uspDistrictLookup_Result> lstuspDistrictLookup_Result = _dbContext.uspDistrictLookup().ToList();
            foreach (var district in lstuspDistrictLookup_Result)
            {
                lstdistrictlookUpDto.Add(Mapper.Map<uspDistrictLookup_Result, DistrictLookupDto>(district));
            }
            return lstdistrictlookUpDto;
        }

        public List<SelectListDto> GetDistrictSelectList()
        {
            List<SelectListDto> lstSelectListDto = new List<SelectListDto>();
            List<uspDistrictGetAll_Result> lstuspDistrictGetAll_Result = _dbContext.uspDistrictGetAll().ToList();
            foreach (var district in lstuspDistrictGetAll_Result)
            {
                SelectListDto objSelectListDto = new SelectListDto()
                {
                    ID = district.DistrictID,
                    Text = district.District
                };
                lstSelectListDto.Add(objSelectListDto);

            }
            return lstSelectListDto;
        }
        public DistrictDto GetByID(int districtId)
        {
            List<uspDistrictGetByDistrictId_Result> lstuspDistrictGetByDistrictId_Result = _dbContext.uspDistrictGetByDistrictId(districtId).ToList();
            DistrictDto districtDto = Mapper.Map<uspDistrictGetByDistrictId_Result, DistrictDto>(lstuspDistrictGetByDistrictId_Result.FirstOrDefault());
            return districtDto;
        }

        public ResultDto Insert(DistrictDto district)
        {
            return InsertUpdate(district);
        }

        public ResultDto Update(DistrictDto district)
        {
            return InsertUpdate(district);
        }

        private ResultDto InsertUpdate(DistrictDto district)
        {
            ResultDto resultDto = new ResultDto();
            string obectName = "district";

            try
            {
                ObjectParameter prmDistrictID = new ObjectParameter("DistrictID", district.DistrictID);
                ObjectParameter paramDistrictCode = new ObjectParameter("DistrictCode", string.Empty);
                int effectedCount = _dbContext.uspDistrictInsertUpdate(prmDistrictID, district.District, district.TEDistrictName, district.StateID, district.UserId, paramDistrictCode);

                resultDto.ObjectId = (int)prmDistrictID.Value;
                resultDto.ObjectCode = (string)paramDistrictCode.Value;
                resultDto.ObjectCode = string.IsNullOrEmpty((string)paramDistrictCode.Value) ? district.DistrictCode : (string)paramDistrictCode.Value;


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


        public List<SelectListDto> GetDistrictsByStateID(int Id)
        {
            var lstDistricts = GetAll().FindAll(l => l.StateID == Id);

            List<SelectListDto> lstSelectListDto = new List<SelectListDto>();

            lstDistricts.ForEach(districts =>
            {
                lstSelectListDto.Add(new SelectListDto()
                {
                    ID = districts.DistrictID,
                    Text = districts.District
                });
            });

            return lstSelectListDto;
        }
        public ResultDto Delete(int districtId, int userId)
        {
            ResultDto resultDto = new ResultDto();
            string objectName = "District";
            try
            {
                ObjectParameter prmResultID = new ObjectParameter("Result", resultDto.Result);
                ObjectParameter prmMessage = new ObjectParameter("Message", string.Empty);

                _dbContext.uspDistrictDelete(districtId,userId,prmResultID,prmMessage);

                resultDto.Result = (bool)prmResultID.Value;
                resultDto.Message = (string)prmMessage.Value;
                resultDto.ObjectId = (int)districtId;
            }
            catch (Exception)
            {
                resultDto.Message = string.Format("Service layer error occured while deleting the {0} details", objectName);
                resultDto.ObjectId = -98;
            }
            return resultDto;
        }
        public ResultDto ChangeStatus(int DistrictId, int userId)
        {
            ResultDto resultDto = new ResultDto();
            string obectName = "District";
            string statusCode = string.Empty;
            try
            {
                ObjectParameter prmDistrictID = new ObjectParameter("DistrictID", DistrictId);
                ObjectParameter prmDistrictCode = new ObjectParameter("DistrictCode", string.Empty);
                ObjectParameter prmStatusCode = new ObjectParameter("StatusCode", string.Empty);

               int effectedCount = _dbContext.uspDistrictChangeStatus(prmDistrictID, prmDistrictCode, prmStatusCode, userId);

                resultDto.ObjectId = (int)prmDistrictID.Value;
                resultDto.ObjectCode = (string)prmDistrictCode.Value;
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
