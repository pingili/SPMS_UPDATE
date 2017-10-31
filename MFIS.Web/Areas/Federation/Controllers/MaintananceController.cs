using MFIS.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MFIS.Web.Areas.Federation.Controllers
{
    public class MaintananceController : BaseController
    {
        //
        // GET: /Federation/Maintanance/

        public ActionResult PageUnderConstruction()
        {
            return View("_FederationUnderConstruction");
        }
    }
}
