
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
    public class PanchayatService 
    {
        #region Global Variables
        private readonly MFISDBContext _dbContext;

        public PanchayatService()
        {
            _dbContext = new MFISDBContext();
            AutoMapperEntityConfiguration.Configure();
        }
        #endregion

        /*
        #region DropDownsBinding
        public List<SelectListDto> GetClustersList()
        {
            List<SelectListDto> lstSelectListDto = new List<SelectListDto>();
            List<uspClusterGetAll_Result> lstuspClusterGetAll_Result = _dbContext.uspClusterGetAll().ToList();
            foreach (var cluster in lstuspClusterGetAll_Result)
            {
                SelectListDto objSelectListDto = new SelectListDto()
                {
                    ID = cluster.ClusterID,
                    Text = cluster.ClusterName
                };
                lstSelectListDto.Add(objSelectListDto);
            }
            return lstSelectListDto;
        }

        public List<SelectListDto> GetVillageSelectList()
        {
            List<SelectListDto> lstSelectListDto = new List<SelectListDto>();
            List<uspVillageGetAll_Result> lstuspVillageGetAll_Result = _dbContext.uspVillageGetAll().ToList();
            foreach (var village in lstuspVillageGetAll_Result)
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

        #endregion
        */



        private ResultDto InsertUpdate(PanchayatDto Panchayatdto)
        {
            ResultDto resultDto = new ResultDto();
            string objectName = "panchayat";

            try
            {
                Panchayatdto.PanchayatCode = string.Empty;
                ObjectParameter paramPanchayatId = new ObjectParameter("PanchayatID", Panchayatdto.PanchayatID);
                ObjectParameter paramPanchayatCode = new ObjectParameter("PanchayatCode", Panchayatdto.PanchayatCode);
                int effectedCount = _dbContext.uspPanchayatInsertUpdate(paramPanchayatId, Panchayatdto.PanchayatName, Panchayatdto.TEPanchayatName, Panchayatdto.VillageID, Panchayatdto.UserId, paramPanchayatCode);

                resultDto.ObjectId = (int)paramPanchayatId.Value;
                resultDto.ObjectCode = (string)paramPanchayatCode.Value;

                if (resultDto.ObjectId > 0)
                    resultDto.Message = string.Format("{0} details saved successfully with code : {1}", objectName, resultDto.ObjectCode);
                else if (resultDto.ObjectId == -1)
                    resultDto.Message = string.Format("Error occured while generating {0} code", objectName);
                else

                    resultDto.Message = string.Format("Error occured while saving {0} details", objectName);

            }
            catch (Exception)
            {
                

                resultDto.Message = "Service layer error occured while saving the Panchayat details";

                resultDto.Message = "Service layer error occured while saving the Panchayat details";

                resultDto.Message = string.Format("Service layer error occured while saving the {0} details", objectName);

                resultDto.ObjectId = -98;

            }
            return resultDto;
        }

        public List<PanchayatDto> GetAll()
        {
            List<uspPanchayatGetAll_Result> lstuspPanchayatLookup_Result = _dbContext.uspPanchayatGetAll().ToList();


            List<PanchayatDto> lstPanchayatDto = new List<PanchayatDto>();
            foreach (var result in lstuspPanchayatLookup_Result)
                lstPanchayatDto.Add(Mapper.Map<uspPanchayatGetAll_Result, PanchayatDto>(result));

            return lstPanchayatDto;
        }

        public PanchayatDto GetByID(int panchayatId)
        {
            List<uspPanchayatGetByPanchayatId_Result> lstuspPanchayatGetByPanchayatId_Result = _dbContext.uspPanchayatGetByPanchayatId(panchayatId).ToList();
            PanchayatDto PanchayatDto = Mapper.Map<uspPanchayatGetByPanchayatId_Result, PanchayatDto>(lstuspPanchayatGetByPanchayatId_Result.FirstOrDefault());
            return PanchayatDto;

        }
        public PanchayatLookupDto GetByGroupID(int PanchayatId)
        {
            List<uspPanchayatGetByGroupID_Result> lstPanchayatGetByGroupId = _dbContext.uspPanchayatGetByGroupID(PanchayatId).ToList();
            PanchayatLookupDto objpanchayatDto = Mapper.Map<uspPanchayatGetByGroupID_Result, PanchayatLookupDto>(lstPanchayatGetByGroupId.FirstOrDefault());
            return objpanchayatDto;
        }

        public List<PanchayatLookupDto> Lookup()
        {
            List<PanchayatLookupDto> lstPanchayatLookupDto = new List<PanchayatLookupDto>();
            List<uspPanchayatLookup_Result> lstuspPanchayatLookup_Result = _dbContext.uspPanchayatLookup().ToList();
            foreach (var objspresult in lstuspPanchayatLookup_Result)
            {
                var objPanchayatLookupDto = Mapper.Map<uspPanchayatLookup_Result, PanchayatLookupDto>(objspresult);
                lstPanchayatLookupDto.Add(objPanchayatLookupDto);
            }
            //List<PanchayatLookupDto> lstPanchayatLookupDto = Mapper.Map<List<uspPanchayatLookup_Result>, List<PanchayatLookupDto>>(lstuspPanchayatLookup_Result);
            return lstPanchayatLookupDto;
        }

        public List<SelectListDto> GetPanchayatSelectList()
        {
            List<uspPanchayatGetAll_Result> lstuspPanchayatGetAll_Result = _dbContext.uspPanchayatGetAll().ToList();
            List<SelectListDto> lstSelectListDto = new List<SelectListDto>();


            foreach (var panchayat in lstuspPanchayatGetAll_Result)
            {
                SelectListDto objSelectListDto = new SelectListDto()
                {
                    ID = panchayat.PanchayatID,
                    Text = panchayat.PanchayatName
                };
                lstSelectListDto.Add(objSelectListDto);
            }
            return lstSelectListDto;
        }

        public ResultDto Update(PanchayatDto PanchayatDto)
        {
            return InsertUpdate(PanchayatDto);
        }

        public ResultDto Insert(PanchayatDto PanchayatDto)
        {
            return InsertUpdate(PanchayatDto);
        }


        public List<SelectListDto> GetPanchayatByVillageID(int Id)
        {
            var lstPanchayat = GetAll().FindAll(v => v.VillageID == Id);


            List<SelectListDto> lstSelectListDto = new List<SelectListDto>();

            lstPanchayat.ForEach(panchayat =>
            {
                lstSelectListDto.Add(new SelectListDto()
                {
                    ID = panchayat.PanchayatID,
                    Text = panchayat.PanchayatName
                });
            });

            return lstSelectListDto;
        }


        public ResultDto Delete(int panchayatId, int userId)
        {
            ResultDto resultDto = new ResultDto();
            string objectName = "Panchayat";

            try
            {
                ObjectParameter prmResultID = new ObjectParameter("Result", resultDto.Result);
                ObjectParameter prmMessage = new ObjectParameter("Message", string.Empty);

                _dbContext.uspPanchayatDelete(panchayatId, userId, prmResultID, prmMessage);


                resultDto.Result = (bool)prmResultID.Value;
                resultDto.Message = (string)prmMessage.Value;
                resultDto.ObjectId = (int)panchayatId;
            }
            catch (Exception)
            {
                resultDto.Message = string.Format("Service layer error occured while deleting the {0} details", objectName);
                resultDto.ObjectId = -98;

            }
            return resultDto;
        }

        public ResultDto ChangeStatus(int panchayatId, int userId)
        {
            ResultDto resultDto = new ResultDto();
            string obectName = "Panchayat";
            string statusCode = string.Empty;
            try
            {
                ObjectParameter prmPanchayatId = new ObjectParameter("PanchayatId", panchayatId);
                ObjectParameter prmPanchayatCode = new ObjectParameter("PanchayatCode", string.Empty);
                ObjectParameter prmStatusCode = new ObjectParameter("StatusCode", string.Empty);

                int effectedCount = _dbContext.uspPanchayatChangeStatus(prmPanchayatId, prmPanchayatCode, prmStatusCode, userId);

                resultDto.ObjectId = (int)prmPanchayatId.Value;
                resultDto.ObjectCode = (string)prmPanchayatCode.Value;
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

        public PanchayatViewDto GetViewByID(int panchayatId)
        {

            var prmPanchayatId = new ObjectParameter("PanchayatID", panchayatId);

            var results = new MFISDBContext()
                .MultipleResults(CustomProcNames.uspPanchayatGetViewByID, prmPanchayatId)
                .With<PanchayatViewDto>()
                .Execute();
            var panchayatViewDto = (results[0] as List<PanchayatViewDto>)[0];
            return panchayatViewDto;
        }
    }
}
