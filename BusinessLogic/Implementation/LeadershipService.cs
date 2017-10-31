//using BusinessLogic.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntities;
using MFIEntityFrameWork;
using BusinessLogic.AutoMapper;
using System.Data.Entity.Core.Objects;
using AutoMapper;
using Utilities;

namespace BusinessLogic
{
    
    public class LeadershipService 
    {
        #region Global Variables
        private readonly MFISDBContext _dbContext;
        private readonly CommonService _commonService;
        public LeadershipService()
        {
            _dbContext = new MFISDBContext();
            _commonService = new CommonService();
            AutoMapperEntityConfiguration.Configure();
        }
        #endregion
        public List<LeadershipDto> GetAll()
        {
            throw new NotImplementedException();
        }

        public LeadershipDto GetByID(int LeadershipID)
        {
            List<uspLeadershipGetById_Result> lstuspLeadershipGetById_Result = _dbContext.uspLeadershipGetById(LeadershipID).ToList();
            LeadershipDto leadershipDto = Mapper.Map<uspLeadershipGetById_Result, LeadershipDto>(lstuspLeadershipGetById_Result.FirstOrDefault());
            return leadershipDto;

        }

        public ResultDto Insert(LeadershipDto leadership)
        {
            return InsertUpdate(leadership);
        }
        public ResultDto Update(LeadershipDto leadership)
        {
            return InsertUpdate(leadership);
        }

      private ResultDto InsertUpdate(LeadershipDto leadership)
        {
            ResultDto resultDto = new ResultDto();
            string objectName = "Leadership";
            try
            {
                ObjectParameter prmLeaderID = new ObjectParameter("LeadershipID", leadership.LeadershipID);

                int count = _dbContext.uspLeadershipInsertUpdate(prmLeaderID,leadership.LeadershipLevel, leadership.GroupID, leadership.LeadershipTitle, leadership.FromDate, leadership.ToDate, leadership.MemberID, leadership.StatusID, leadership.UserID);
               
                resultDto.ObjectId = (int)prmLeaderID.Value;

                if (resultDto.ObjectId > 0)
                    resultDto.Message = string.Format("{0} details saved successfully ", objectName);
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

        public List<SelectListDto> GetLeadershiplevelSelectList()
        {
            List<SelectListDto> lstSelectListDto = new List<SelectListDto>();
            List<uspGetLeadershipLevel_Result> uspGetLeadershipLevel_Result = _dbContext.uspGetLeadershipLevel().ToList();
            foreach (var level in uspGetLeadershipLevel_Result)
            {
                SelectListDto objSelectListDto = new SelectListDto()
                {
                    ID = level.EntityID,
                    Text = level.EntityName
                };
                lstSelectListDto.Add(objSelectListDto);
            }
            return lstSelectListDto;
        }


        public List<LeadershipLookupDto> GetLookup()
        {
            var lstLeadershipLookupDto = new List<LeadershipLookupDto>();
            //var uspLeaderLookupResults = _dbContext.uspLeadershipLookUp().ToList();
            //foreach (var leader in uspLeaderLookupResults)
            //{
            //    LeadershipLookupDto lookupDto = Mapper.Map<uspLeadershipLookUp_Result, LeadershipLookupDto>(leader);
            //    lstLeadershipLookupDto.Add(lookupDto);
            //}

            return lstLeadershipLookupDto;
        }


        public ResultDto Delete(int LeadershipID, int userId)
        {
            ResultDto resultDto = new ResultDto();
            string obectName = "Leadership";

            try
            {
                ObjectParameter prmleaderId = new ObjectParameter("LeadershipID", LeadershipID);
                int effectedCount = _dbContext.uspLeadershipDelete(prmleaderId,userId);

                resultDto.ObjectId = (int)prmleaderId.Value;

                if (resultDto.ObjectId > 0)
                    resultDto.Message = string.Format("{0} : details deleted successfully", obectName);
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

        public ResultDto ChangeStatus(int LeadershipID, int userId)
        {
            ResultDto resultDto = new ResultDto();
            string obectName = "Leadership";
            string statusCode = string.Empty;
            try
            {
                ObjectParameter prmleadershipId = new ObjectParameter("LeadershipID", LeadershipID);
                ObjectParameter prmStatusCode = new ObjectParameter("StatusCode", string.Empty);

                int effectedCount = _dbContext.uspLeadershipChangeStatus(prmleadershipId,prmStatusCode,userId);

                resultDto.ObjectId = (int)prmleadershipId.Value;
                statusCode = (string)prmStatusCode.Value;

                if (resultDto.ObjectId > 0)
                    resultDto.Message = string.Format("{0} : details {1} successfully", obectName, statusCode == Constants.StatusCodes.Active ? "activated" : "inactivated");
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
