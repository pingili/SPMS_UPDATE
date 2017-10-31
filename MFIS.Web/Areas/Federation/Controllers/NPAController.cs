using MFIS.Web.Areas.Federation.Models;
using MFIS.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLogic;
//using BusinessLogic.Interface;
using Utilities;
using BusinessEntities;
using AutoMapper;

namespace MFIS.Web.Areas.Federation.Controllers
{
    public class NPAController : BaseController
    {

        private NPAService _NpaService;
        public NPAController()
        {
            _NpaService = new NPAService();
        }

        [HttpGet]
        public ActionResult CreateNPA()
        {
            var lstNPA = LoadNPADetails();

            return View("CreateNPA", lstNPA);
        }

        private List<NPAModel> LoadNPADetails()
        {
            List<NPADto> lstNpaDto = _NpaService.GetNPADetails();
            List<NPAModel> lstNPA = new List<NPAModel>();
            foreach (var data in lstNpaDto)
            {
                lstNPA.Add(
                    new NPAModel
                    {
                        OverDuePeriodID = data.OverDuePeriodID,
                        OverDuePeriod = (data.OverDuePeriod),
                        Rate = data.Rate
                    });
            }
            return lstNPA;
        }

        [HttpPost]
        public ActionResult CreateNPA(List<NPAModel> objNPAModel)
        {
            if (ModelState.IsValid)
            {
                List<NPADto> lstNPADto = Mapper.Map<List<NPAModel>, List<NPADto>>(objNPAModel);

                ResultDto result = _NpaService.InsertUpdateNPARecords(lstNPADto, UserInfo.UserID);

                ViewBag.Result = result;
            }

            var lstNPA = LoadNPADetails();

            return View("CreateNPA", lstNPA);
        }
    }
}