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
    public class VillageService 
    {
        private readonly MFISDBContext _dbContext;
        private readonly CommonService _commonService;

        public VillageService()
        {
            _dbContext = new MFISDBContext();
            _commonService = new CommonService();
            AutoMapperEntityConfiguration.Configure();
        }




       
        public List<VillageDto> GetAll()

        {
            List<VillageDto> lstVillageDto = new List<VillageDto>();
            List<uspVillageGetAll_Result> lstuspVillageGetAll_Result = _dbContext.uspVillageGetAll().ToList();
            foreach (var Village in lstuspVillageGetAll_Result)
            {
                VillageDto VillageDto = Mapper.Map<uspVillageGetAll_Result, VillageDto>(Village);
                lstVillageDto.Add(VillageDto);
            }
            return lstVillageDto;
        }

        public List<VillageLookupDto> Lookup()
        {
            List<VillageLookupDto> lstVillageDto = new List<VillageLookupDto>();
            List<uspVillageLookup_Result> lstuspVillageGetAll_Result = _dbContext.uspVillageLookup().ToList();
            foreach (var Village in lstuspVillageGetAll_Result)
            {
                VillageLookupDto VillageDto = Mapper.Map<uspVillageLookup_Result, VillageLookupDto>(Village);
                lstVillageDto.Add(VillageDto);
            }
            return lstVillageDto;
        }

        public VillageDto GetByID(int villageId)
        {
            VillageDto objVillageDto = new VillageDto();
            List<VillageDto> lstvillageDto = new List<VillageDto>();
            List<uspVillageGetById_Result> lstuspVillageGetById_Result = _dbContext.uspVillageGetById(villageId).ToList();
            objVillageDto = Mapper.Map<uspVillageGetById_Result, VillageDto>(lstuspVillageGetById_Result.FirstOrDefault());
            return objVillageDto;
        }

        public ResultDto Insert(VillageDto Village)
        {
            return InsertUpdateVillage(Village);
        }

        public ResultDto Update(VillageDto Village)
        {
            return InsertUpdateVillage(Village);
        }

        public List<SelectListDto> GetVillageSelectList()
        {
            List<SelectListDto> lstSelectListDto = new List<SelectListDto>();
            List<uspVillageGetAll_Result> uspVillageGetAll_Result = _dbContext.uspVillageGetAll().ToList();
            foreach (var village in uspVillageGetAll_Result)
            {
                SelectListDto objSelectListDto = new SelectListDto()
                {
                    ID = village.VillageID,
                    Text = village.Village
                };
                lstSelectListDto.Add(objSelectListDto);
            }
            return lstSelectListDto;
        }

        private ResultDto InsertUpdateVillage(VillageDto village)
        {
            ResultDto resultDto = new ResultDto();
            string obectName = "village";

            try
            {
                ObjectParameter paramVillageID = new ObjectParameter("VillageId", village.VillageID);
                ObjectParameter paramVillageCode = new ObjectParameter("VillageCode", string.Empty);
                int count = _dbContext.uspVillageInsertUpdate(paramVillageID, village.Village, village.TEVillageName, village.ClusterID, village.UserID, paramVillageCode);

                resultDto.ObjectId = (int)paramVillageID.Value;
                resultDto.ObjectCode = string.IsNullOrEmpty((string)paramVillageCode.Value) ? village.VillageCode : (string)paramVillageCode.Value;

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


        public List<SelectListDto> GetVillageByClusterID(int Id)
        {
            var lstVillage = GetAll().FindAll(v => v.ClusterID == Id);
            

            List<SelectListDto> lstSelectListDto = new List<SelectListDto>();

            lstVillage.ForEach(village =>
            {
                lstSelectListDto.Add(new SelectListDto()
                {
                    ID = village.VillageID,
                    Text = village.Village
                });
            });

            return lstSelectListDto;
        }


        public ResultDto Delete(int villageId, int userId)
        {
            ResultDto resultDto = new ResultDto();
            string obectName = "Village";

            try
            {
                ObjectParameter prmResultID = new ObjectParameter("Result", resultDto.Result);
                ObjectParameter prmMessage = new ObjectParameter("Message", string.Empty);

                _dbContext.uspVillageDelete(villageId, userId, prmResultID, prmMessage);

                resultDto.Result = (bool)prmResultID.Value;
                resultDto.Message = (string)prmMessage.Value;
                resultDto.ObjectId = (int)villageId;
            }
            catch (Exception)
            {
                resultDto.Message = string.Format("Service layer error occured while deleting the {0} details", obectName);
                resultDto.ObjectId = -98;

            }
            return resultDto;
        }

        public ResultDto ChangeStatus(int villageId, int userId)
        {
            ResultDto resultDto = new ResultDto();
            string obectName = "Village";
            string statusCode = string.Empty;
            try
            {
                ObjectParameter prmVillageId = new ObjectParameter("VillageId", villageId);
                ObjectParameter prmVillageCode = new ObjectParameter("VillageCode", string.Empty);
                ObjectParameter prmStatusCode = new ObjectParameter("StatusCode", string.Empty);

                int effectedCount = _dbContext.uspVillageChangeStatus(prmVillageId, prmVillageCode, prmStatusCode, userId);

                resultDto.ObjectId = (int)prmVillageId.Value;
                resultDto.ObjectCode = (string)prmVillageCode.Value;
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

        public VillageViewDto GetViewByID(int villageId)
        {
            var prmVillageId = new ObjectParameter("VillageID", villageId);

            var results = new MFISDBContext()
                .MultipleResults(CustomProcNames.uspVillageGetViewByID, prmVillageId)
                .With<VillageViewDto>()
                .Execute();
            var villageViewDto = (results[0] as List<VillageViewDto>)[0];
            return villageViewDto;
        }
    }
}
