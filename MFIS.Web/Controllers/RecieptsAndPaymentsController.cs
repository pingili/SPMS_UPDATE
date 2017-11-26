using BusinessEntities;
using BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MFIS.Web.Controllers
{
    public class RecieptsAndPaymentsController : BaseController
    {
        //
        // GET: /RecieptsAndPayments/

        public ActionResult ReceiptsAndPayments()
        {
            OrganizationService objIOrganizationService = new OrganizationService();
            OrganizationDto organizationDto = objIOrganizationService.GetAll();
            ViewBag.OrganizationDetails = organizationDto;
            return View();
        }

    }
}
