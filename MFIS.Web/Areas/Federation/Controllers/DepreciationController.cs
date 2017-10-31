using AutoMapper;
using BusinessEntities;
using BusinessLogic;
using MFIS.Web.Areas.Federation.Models;
using MFIS.Web.Controllers;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MFIS.Web.Areas.Federation.Controllers
{
    public class DepreciationController : BaseController
    {
        #region Global Variables
        private DepreciationService _depreciationService;
        public DepreciationController()
        {
            _depreciationService = new DepreciationService();
        }

        #endregion Global Variables

        [HttpGet]
        public ActionResult CreateDepreciation()
        {
            var lstDepreciationModel = LoadDepreciationDetails();

            return View(lstDepreciationModel);
        }

        private List<DepreciationModel> LoadDepreciationDetails()
        {
            var lstDepreciationDto = _depreciationService.GetDepreciation();
            List<DepreciationModel> lstDepreciationModel = new List<DepreciationModel>();

            foreach (var data in lstDepreciationDto)
            {
                lstDepreciationModel.Add(new DepreciationModel
                {
                    AHID = data.AHID,
                    AHName = data.AHName,
                    Rate = data.Rate
                });
            }
            return lstDepreciationModel;
        }

        [HttpPost]
        public ActionResult CreateDepreciation(List<DepreciationModel> lstDepreciationModel)
        {
            if (ModelState.IsValid)
            {
                List<DepreciationDto> lstDepreciationDto = new List<DepreciationDto>();

                foreach (var depreciationModel in lstDepreciationModel)
                {
                    lstDepreciationDto.Add(Mapper.Map<DepreciationModel, DepreciationDto>(depreciationModel));
                }

                ResultDto result = _depreciationService.InsertUpdateDepreciationRecords(lstDepreciationDto, UserInfo.UserID);

                ViewBag.Result = result;
            }

           lstDepreciationModel = LoadDepreciationDetails();

            return View(lstDepreciationModel);
        }
    }
}
