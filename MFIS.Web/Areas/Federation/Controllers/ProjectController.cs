using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MFIS.Web.Areas.Federation.Models;
using BusinessEntities;
//using BusinessLogic.Interface;
using MFIS.Web.Controllers;
using BusinessLogic;
using Utilities;
using AutoMapper;
using BusinessLogic.AutoMapper;
using CoreComponents;
namespace MFIS.Web.Areas.Federation.Controllers
{
    public class ProjectController : BaseController
    {
        #region Global Variables
        private ProjectService _projectService;
        private FundSourceService _fundSourceService;
        public ProjectController()
        {
            _projectService = new ProjectService();
            _fundSourceService = new FundSourceService();
        }
        #endregion Global Variables

        [HttpGet]
        public ActionResult CreateProject(string id)
        {
            int projectId = string.IsNullOrEmpty(id.DecryptString()) ? default(int) : Convert.ToInt32(id.DecryptString());
            var projectMasterDto = new ProjectDto();
            var projectMasterModel = new ProjectModel();

            if (projectId > 0)
            {
                projectMasterDto = _projectService.GetByID(projectId);
                projectMasterDto.UserID = UserInfo.UserID;
                projectMasterModel = AutoMapperEntityConfiguration.Cast<ProjectModel>(projectMasterDto);
            }
            #region DropDowns
            List<SelectListDto> fundSourceSelectList = _fundSourceService.GetFundSourceSelectList();
            SelectList SlstfoundSource = new SelectList(fundSourceSelectList, "ID", "Text", projectMasterDto.FundSourceID);
            ViewBag.foundSource = SlstfoundSource;

            SelectList projectPurposes = GetDropDownListByMasterCode(Enums.RefMasterCodes.PRJ_PURPOSE);
            ViewBag.ProjectPurposes = projectPurposes;

            SelectList projectTypes= GetDropDownListByMasterCode(Enums.RefMasterCodes.PRJ_TYPES);
            ViewBag.ProjectTypes = projectTypes;
            #endregion DropDowns
            ViewBag.Result = new ResultDto();
            return View("CreateProject", projectMasterModel);
        }

        [HttpPost]
        public ActionResult CreateProject(ProjectModel objprojectModel)
        {
            ResultDto resultDto = new ResultDto();
            if (ModelState.IsValid)
            {
                var projectMasterDto = Mapper.Map<ProjectModel, ProjectDto>(objprojectModel);
                projectMasterDto.UserID = UserInfo.UserID;
                
                if (objprojectModel.ProjectID == 0)
                    resultDto = _projectService.Insert(projectMasterDto);
                else
                    resultDto = _projectService.Update(projectMasterDto);
                
                if (resultDto.ObjectId > 0)
                {
                    projectMasterDto = _projectService.GetByID(resultDto.ObjectId);
                    objprojectModel = AutoMapperEntityConfiguration.Cast<ProjectModel>(projectMasterDto);
                    resultDto.ObjectCode = projectMasterDto.ProjectCode;
                }
            }

            #region DropDowns
            var projectMasterdto = new ProjectDto();
            List<SelectListDto> fundSourceSelectList = _fundSourceService.GetFundSourceSelectList();
            SelectList SlstfoundSource = new SelectList(fundSourceSelectList, "ID", "Text", projectMasterdto.FundSourceID);
            ViewBag.foundSource = SlstfoundSource;

            SelectList projectPurposes = GetDropDownListByMasterCode(Enums.RefMasterCodes.PRJ_PURPOSE);
            ViewBag.ProjectPurposes = projectPurposes;

            SelectList projectTypes = GetDropDownListByMasterCode(Enums.RefMasterCodes.PRJ_TYPES);
            ViewBag.ProjectTypes = projectTypes;
            #endregion DropDowns
            
            ViewBag.Result = resultDto;
            return View("CreateProject", objprojectModel);
        }

        [HttpGet]
        public ActionResult ProjectLookup()
        {
            var projectMasterLookups = _projectService.GetLookup();
            return View("ProjectLookup", projectMasterLookups);
        }

        [HttpGet]
        public ActionResult DeleteProject(string Id)
        {
            int projectId = DecryptQueryString(Id);

            if (projectId < 1)
                return RedirectToAction("ProjectLookup");

            ResultDto resultDto = _projectService.Delete(projectId, UserInfo.UserID);

            TempData["Result"] = resultDto;

            return RedirectToAction("ProjectLookup");
        }
        public ActionResult ActiveInactiveProject(string Id)
        {
            int projectId = DecryptQueryString(Id);

            if (projectId < 1)
                return RedirectToAction("ProjectLookup");

            ResultDto resultDto = _projectService.ChangeStatus(projectId, UserInfo.UserID);

            TempData["Result"] = resultDto;

            return RedirectToAction("ProjectLookup");
        }
    }
}
