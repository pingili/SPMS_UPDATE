using MFIS.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MFIS.Web.Areas.Federation.Controllers.TransactionControllers
{
    public class DepositController : BaseController
    {   
        [HttpGet]
        public ActionResult CreateDeposit()
        {
            return View();
        }

    }
}
