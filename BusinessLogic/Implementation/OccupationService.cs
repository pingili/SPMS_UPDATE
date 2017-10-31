using BusinessLogic.AutoMapper;
//using BusinessLogic.Interface;
using MFIEntityFrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntities;
using AutoMapper;
using System.Data.Entity.Validation;
using System.Data.Entity.Core.Objects;
using Utilities;


namespace BusinessLogic
{
    public class OccupationService 
    {
        private readonly MFISDBContext _dbContext;

        public OccupationService()
        {
            _dbContext = new MFISDBContext();
            AutoMapperEntityConfiguration.Configure();
        }
       public List<OccupationDto> GetAll()
        {
            List<uspOccupationGetAll_Result> lstuspOccupationGetAll_Result = _dbContext.uspOccupationGetAll().ToList();
            List<OccupationDto> lstDistrictDto = new List<OccupationDto>();
            foreach (var result in lstuspOccupationGetAll_Result)
                lstDistrictDto.Add(Mapper.Map<uspOccupationGetAll_Result, OccupationDto>(result));

            return lstDistrictDto;
        }
       public List<OccupationLookupDto> Lookup()
       {
           List<OccupationLookupDto> lstOccupationDto = new List<OccupationLookupDto>();
           List<uspOccupationLookup_Result> lstuspOccupationLookup_Result = _dbContext.uspOccupationLookup().ToList();
           foreach (var obj in lstuspOccupationLookup_Result)
               lstOccupationDto.Add(Mapper.Map<uspOccupationLookup_Result, OccupationLookupDto>(obj));
           return lstOccupationDto;
       }
        
       public OccupationDto GetByID(int OccupationID)
       {
           List<uspOccupationGetByOccupationID_Result> lstuspOccupationGetByOccupationID_Result = _dbContext.uspOccupationGetByOccupationID(OccupationID).ToList();
           OccupationDto objOccupationDto = Mapper.Map<uspOccupationGetByOccupationID_Result, OccupationDto>(lstuspOccupationGetByOccupationID_Result.FirstOrDefault());
           return objOccupationDto;
       }
       public ResultDto Insert(OccupationDto occupation)
       {
           return InsertUpdateOccupation(occupation);
       }

       public ResultDto Update(OccupationDto occupation)
       {
           return InsertUpdateOccupation(occupation);

       }
       private ResultDto InsertUpdateOccupation(OccupationDto occupation)
       {
           ResultDto resultDto = new ResultDto();
           string obectName = "occupation";
           try
           {
               ObjectParameter prmOccupationID = new ObjectParameter("OccupationID", occupation.OccupationID);
               ObjectParameter paramOccupationCode = new ObjectParameter("OccupationCode", string.Empty);
               int effectcount = _dbContext.uspOccupationInsertUpdate(prmOccupationID, paramOccupationCode, occupation.OccupationCategory, occupation.Occupation,  occupation.UserId);

               resultDto.ObjectId = (int)prmOccupationID.Value;
               resultDto.ObjectCode = string.IsNullOrEmpty((string)paramOccupationCode.Value) ? occupation.OccupationCode : (string)paramOccupationCode.Value;

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
         public List<SelectListDto> GetOccupationSelectList()
       {
           List<SelectListDto> lstSelectListDto = new List<SelectListDto>();
           List<uspOccupationGetAll_Result> lstuspOccupationGetAll_Result = _dbContext.uspOccupationGetAll().ToList();
             foreach(var occupation in lstuspOccupationGetAll_Result)
             {
                 SelectListDto objSelectListDto = new SelectListDto()
                 {
                     ID=occupation.OccupationID,
                     Text=occupation.Occupation
                 };
                 lstSelectListDto.Add(objSelectListDto);
             }
             return lstSelectListDto;
       }


         public ResultDto Delete(int occupationId, int userId)
         {
             ResultDto resultDto = new ResultDto();
             string obectName = "occupation";

             try
             {
                 ObjectParameter prmOccupationID = new ObjectParameter("OccupationID", occupationId);
                 ObjectParameter prmOccupationCode = new ObjectParameter("OccupationCode", string.Empty);

                 int effectedCount = _dbContext.uspOccupationDelete(prmOccupationID, prmOccupationCode, userId);

                 resultDto.ObjectId = (int)prmOccupationID.Value;
                 resultDto.ObjectCode = (string)prmOccupationCode.Value;

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

         public ResultDto ChangeStatus(int occupationId, int userId)
         {
             ResultDto resultDto = new ResultDto();
             string obectName = "occupation";
             string statusCode = string.Empty;
             try
             {
                 ObjectParameter prmOccupationID = new ObjectParameter("OccupationID", occupationId);
                 ObjectParameter prmOccupationCode = new ObjectParameter("OccupationCode", string.Empty);
                 ObjectParameter prmStatusCode = new ObjectParameter("StatusCode", string.Empty);

                 int effectedCount = _dbContext.uspOccupationChangeStatus(prmOccupationID, prmOccupationCode, prmStatusCode, userId);

                 resultDto.ObjectId = (int)prmOccupationID.Value;
                 resultDto.ObjectCode = (string)prmOccupationCode.Value;
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
