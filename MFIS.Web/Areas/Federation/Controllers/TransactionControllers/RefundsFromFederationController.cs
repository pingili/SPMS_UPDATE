using BusinessEntities;
using BusinessLogic;
//using BusinessLogic.Interface;
using MFIS.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MFIS.Web.Areas.Federation.Controllers.TransactionControllers
{
    public class RefundsFromFederationController : BaseController
    {
        #region Global Variables
        private readonly GroupService _groupService;

        public RefundsFromFederationController()
        {
            _groupService = new GroupService();
        }
        #endregion Global Variables
        [HttpGet]
        public ActionResult CreateRefundsFromFederation()
        {
            List<GroupLookupDto> lstGroupDto = _groupService.Lookup();
            SelectList lstgroup = new SelectList(lstGroupDto, "GroupID", "GroupCode");
            ViewBag.GroupNames = lstgroup;
            return View();
        }
        [HttpPost]
        public ActionResult CreateRefundsFromFederation(FormCollection form)
        {
            return View();
        }
        public ActionResult RefundsFromFederationLookup()
        {
            return View();
        }
    }
}
