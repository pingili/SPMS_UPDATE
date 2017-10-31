using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using BusinessLogic.Interface;
using BusinessEntities;
using MFIEntityFrameWork;
using BusinessLogic.AutoMapper;
using AutoMapper;
using System.Data.Entity.Core.Objects;
using Utilities;

namespace BusinessLogic
{
    public class ProjectService 
    {
        #region Global Variables
        private readonly MFISDBContext _dbContext;
        private readonly CommonService _commonService;

        public ProjectService()
        {
            _dbContext = new MFISDBContext();
            _commonService = new CommonService();
            AutoMapperEntityConfiguration.Configure();
        }
        #endregion

        public List<ProjectDto> GetAll()
        {

            var lstProjectDto = new List<ProjectDto>();
            var lstuspProjectGetAll_Result = _dbContext.uspProjectGetAll().ToList();
            foreach (var project in lstuspProjectGetAll_Result)
            {
                ProjectDto objProjectDto = Mapper.Map<uspProjectGetAll_Result, ProjectDto>(project);
                lstProjectDto.Add(objProjectDto);
            }

            return lstProjectDto;
        }

        public List<ProjectLookupDto> GetLookup()
        {
            var lstProjectLookupDtos = new List<ProjectLookupDto>();
            var uspProjectLookupResults = _dbContext.uspProjectLookup().ToList();
            foreach (var project in uspProjectLookupResults)
            {
                ProjectLookupDto lookupDto = Mapper.Map<uspProjectLookup_Result, ProjectLookupDto>(project);
                lstProjectLookupDtos.Add(lookupDto);
            }
            return lstProjectLookupDtos;
        }

        public ProjectDto GetByID(int projectID)
        {
            uspProjectGetById_Result objuspProjectMasterGetByIdResult = _dbContext.uspProjectGetById(projectID).ToList().FirstOrDefault();

            ProjectDto objProjectMasterDto = AutoMapperEntityConfiguration.Cast<ProjectDto>(objuspProjectMasterGetByIdResult);

            return objProjectMasterDto;
        }

        public ResultDto Insert(ProjectDto projectDto)
        {
            return InsertUpdateBankMaster(projectDto);
        }

        public ResultDto Update(ProjectDto projectDto)
        {
            return InsertUpdateBankMaster(projectDto);
        }

        private ResultDto InsertUpdateBankMaster(ProjectDto projectDto)
        {
            string obectName = "project";
            ResultDto resultDto = new ResultDto();

            try
            {
                ObjectParameter paramProjectId = new ObjectParameter("ProjectId", projectDto.ProjectID);
                ObjectParameter paramProjectCode = new ObjectParameter("ProjectCode", string.Empty);

                _dbContext.uspProjectInsertUpdate(paramProjectId, paramProjectCode, projectDto.FundSourceID, projectDto.ProjectName, projectDto.Purpose, projectDto.Type, projectDto.UserID);

                resultDto.ObjectId = (int)paramProjectId.Value;
                resultDto.ObjectCode = string.IsNullOrEmpty((string)paramProjectCode.Value) ? projectDto.ProjectCode : (string)paramProjectCode.Value;

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

        public List<SelectListDto> GetProjectSelectList()
        {
            List<SelectListDto> lstSelectListDto = new List<SelectListDto>();
            List<uspProjectGetAll_Result> lstuspProjectGetAll_Result = _dbContext.uspProjectGetAll().ToList();
            foreach (var project in lstuspProjectGetAll_Result)
            {
                SelectListDto objSelectListDto = new SelectListDto()
                {
                    ID = project.ProjectID,
                    Text = project.ProjectName
                };
                lstSelectListDto.Add(objSelectListDto);
            }
            return lstSelectListDto;
        }

        public ResultDto Delete(int projectId, int userId)
        {

            ResultDto resultDto = new ResultDto();
            string obectName = "Project";

            try
            {
                ObjectParameter prmProjectId = new ObjectParameter("projectId", projectId);
                ObjectParameter prmProjectCode = new ObjectParameter("ProjectCode", string.Empty);

                int effectedCount = _dbContext.uspProjectDelete(prmProjectId, prmProjectCode, userId);

                resultDto.ObjectId = (int)prmProjectId.Value;
                resultDto.ObjectCode = (string)prmProjectCode.Value;

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

        public ResultDto ChangeStatus(int projectId, int userId)
        {
            ResultDto resultDto = new ResultDto();
            string obectName = "Project";
            string statusCode = string.Empty;
            try
            {
                ObjectParameter prmProjectId = new ObjectParameter("projectId", projectId);
                ObjectParameter prmProjectCode = new ObjectParameter("ProjectCode", string.Empty);
                ObjectParameter prmStatusCode = new ObjectParameter("StatusCode", string.Empty);

                int effectedCount = _dbContext.uspProjectChangeStatus(prmProjectId, prmProjectCode, prmStatusCode, userId);

                resultDto.ObjectId = (int)prmProjectId.Value;
                resultDto.ObjectCode = (string)prmProjectCode.Value;
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
