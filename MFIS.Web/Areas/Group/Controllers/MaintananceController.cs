using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MFIS.Web.Areas.Group.Controllers
{
    public class MaintananceController : Controller
    {
        public ActionResult PageUnderConstruction()
        {
            return View("_GroupUnderConstruction");
        }
    }
}