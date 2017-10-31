using BusinessLogic.AutoMapper;
//using BusinessLogic.Interface;
using MFIEntityFrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BusinessEntities;
using System.Data.Entity.Core.Objects;
using Utilities;

namespace BusinessLogic
{
    public class MandalService 
    {
        #region Global Variables
        private readonly MFISDBContext _dbContext;
        private readonly CommonService _commonService;
        public MandalService()
        {
            _dbContext = new MFISDBContext();
            _commonService = new CommonService();
            AutoMapperEntityConfiguration.Configure();
        }
        #endregion

        public List<MandalDto> GetAll()
        {
            var lstMandalDto = new List<MandalDto>();
            var lstuspMandalGetAll_Result = _dbContext.uspMandalGetAll().ToList();
            foreach (var mandal in lstuspMandalGetAll_Result)
            {
                MandalDto objMandal = Mapper.Map<uspMandalGetAll_Result, MandalDto>(mandal);
                lstMandalDto.Add(objMandal);
            }

            return lstMandalDto;
        }

        public List<MandalLookupDto> Lookup()
        {
            var lstMandalLookupDto = new List<MandalLookupDto>();
            var lstuspMandalLookup_Result = _dbContext.uspMandalLookup().ToList();
            foreach (var mandal in lstuspMandalLookup_Result)
            {
                MandalLookupDto lookupDto = Mapper.Map<uspMandalLookup_Result, MandalLookupDto>(mandal);
                lstMandalLookupDto.Add(lookupDto);
            }
            return lstMandalLookupDto;
        }

        public MandalDto GetByID(int MandalID)
        {
            var objuspMandalGetByIdResult = _dbContext.uspMandalGetByID(MandalID).ToList().FirstOrDefault();

            MandalDto objMandalDto = AutoMapperEntityConfiguration.Cast<MandalDto>(objuspMandalGetByIdResult);

            return objMandalDto;
        }

        public ResultDto Insert(MandalDto mandalDto)
        {
            return InsertUpdateMandal(mandalDto);
        }

        public ResultDto Update(MandalDto mandalDto)
        {
            return InsertUpdateMandal(mandalDto);
        }

        private ResultDto InsertUpdateMandal(MandalDto mandalDto)
        {
            ResultDto resultDto = new ResultDto();
            string obectName = "mandal";
            try
            {
                ObjectParameter prmMandalID = new ObjectParameter("MandalID", mandalDto.MandalID);
                ObjectParameter prmMandalCode = new ObjectParameter("MandalCode", string.Empty);

                int count = _dbContext.uspMandalInsertUpdate(prmMandalID, mandalDto.Mandal, mandalDto.TEMandalName,mandalDto.MandalType, mandalDto.DistrictID, mandalDto.UserID, prmMandalCode);

                resultDto.ObjectId = (int)prmMandalID.Value;
                resultDto.ObjectCode = string.IsNullOrEmpty((string)prmMandalCode.Value) ? mandalDto.MandalCode : (string)prmMandalCode.Value;

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

        public List<SelectListDto> GetMandalSelectList()
        {
            List<SelectListDto> lstSelectListDto = new List<SelectListDto>();
            List<uspMandalGetAll_Result> lstuspMandalGetAll_Result = _dbContext.uspMandalGetAll().ToList();
            foreach (var mandal in lstuspMandalGetAll_Result)
            {
                SelectListDto objSelectListDto = new SelectListDto()
                {
                    ID = mandal.MandalID,
                    Text = mandal.Mandal
                };
                lstSelectListDto.Add(objSelectListDto);
            }
            return lstSelectListDto;
        }


        public List<SelectListDto> GetMandalByDistrictID(int Id)
        {
            var lstMandal = GetAll().FindAll(m => m.DistrictID == Id);

            List<SelectListDto> lstSelectListDto = new List<SelectListDto>();

            lstMandal.ForEach(Mandal =>
            {
                lstSelectListDto.Add(new SelectListDto()
                {
                    ID = Mandal.MandalID,
                    Text = Mandal.Mandal
                });
            });

            return lstSelectListDto;
        }


        public ResultDto Delete(int mandalId, int userId)
        {
            ResultDto resultDto = new ResultDto();
            string ObjectName = "Mandal";
            try
            {
                ObjectParameter prmResultID = new ObjectParameter("Result", resultDto.Result);
                ObjectParameter prmMessage = new ObjectParameter("Message", string.Empty);

                _dbContext.uspMandalDelete(mandalId,userId, prmResultID, prmMessage);
               
                resultDto.Result = (bool)prmResultID.Value;
                resultDto.Message = (string)prmMessage.Value;
                resultDto.ObjectId = (int)mandalId;

            }
            catch (Exception)
            {
                resultDto.Message = string.Format("Service layer error occured while deleting the {0} details", ObjectName);
                resultDto.ObjectId = -98;
            }
            return resultDto;
        }   


        public ResultDto ChangeStatus(int mandalId, int userId)
        {
            ResultDto resultDto = new ResultDto();
            string objectName = string.Empty;
            string statusCode = string.Empty;
            try
            {
                ObjectParameter prmMandalId = new ObjectParameter("MandalID", mandalId);
                ObjectParameter prmMandalCode = new ObjectParameter("MandalCode", mandalId);
                ObjectParameter prmStatusCode = new ObjectParameter("StatusCode", mandalId);
                int effectedCount = _dbContext.uspMandalChangeStatus(prmMandalId, prmMandalCode, prmStatusCode, userId);

                resultDto.ObjectId = (int)prmMandalId.Value;
                resultDto.ObjectCode = (string)prmMandalCode.Value;
                statusCode = (string)prmStatusCode.Value;

                if (resultDto.ObjectId > 0)
                    resultDto.Message = string.Format("{0} : {1} details {2} successfully", objectName, resultDto.ObjectCode, statusCode == Constants.StatusCodes.Active ? "activated" : "inactivated");
                else
                    resultDto.Message = string.Format("Error occured while {0} {1} details", statusCode == Constants.StatusCodes.Active ? "activated" : "inactivated",objectName);
            }
            catch (Exception)
            {

                resultDto.Message = string.Format("Service layer error occured while {0} {1} details", statusCode == Constants.StatusCodes.Active ? "activated" : "inactivated",objectName);
                resultDto.ObjectId = -98;

            }
            return resultDto;
        }
    }
}
