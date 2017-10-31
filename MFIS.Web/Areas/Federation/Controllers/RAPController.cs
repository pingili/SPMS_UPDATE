using BusinessEntities;
using BusinessLogic;
//using BusinessLogic.Interface;
using MFIS.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MFIS.Web.Areas.Federation.Controllers
{
    public class RAPController : BaseController
    {
        private RAPService _rapService;

        public RAPController()
        {
            _rapService= new RAPService();
        }

        #region FEDERATION RECEIPT APPROPRIATION PRIORITY

        [HttpGet]
        public ActionResult FederationRAP()
        {
            bool isGroup = false;

            var lstFederationRAPDto = LoadRAPDetailsByType(isGroup);

            return View("FederationRAP", lstFederationRAPDto);
        }

        [HttpPost]
        public ActionResult FederationRAP(List<RAPDto> lstFederationRAPDto)
        {
            bool isGroup = false;

            if (ModelState.IsValid)
            {
                ResultDto result = _rapService.ManageRAPByType(isGroup, lstFederationRAPDto, UserInfo.UserID);

                ViewBag.Result = result;
            }

            lstFederationRAPDto = LoadRAPDetailsByType(isGroup);

            return View("FederationRAP", lstFederationRAPDto);
        }

        #endregion

        private List<RAPDto> LoadRAPDetailsByType(bool isGroup)
        {
            List<RAPDto> lstRapDto = _rapService.GetRAPByType(isGroup);

            return lstRapDto;
        }

        #region GROUP RECEIPT APPROPRIATION PRIORITY

        [HttpGet]
        public ActionResult GroupRAP()
        {
            bool isGroup = true;

            var lstGroupRAPDto = LoadRAPDetailsByType(isGroup);

            return View("GroupRAP", lstGroupRAPDto);
        }

        [HttpPost]
        public ActionResult GroupRAP(List<RAPDto> lstGroupRAPDto)
        {
            bool isGroup = true;
            
            if (ModelState.IsValid)
            {
                ResultDto result = _rapService.ManageRAPByType(isGroup, lstGroupRAPDto, UserInfo.UserID);

                ViewBag.Result = result;
            }

            lstGroupRAPDto = LoadRAPDetailsByType(isGroup);

            return View("GroupRAP", lstGroupRAPDto);
        }

        #endregion

    }
}
