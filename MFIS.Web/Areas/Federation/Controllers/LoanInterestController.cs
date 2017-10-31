using MFIS.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MFIS.Web.Areas.Federation.Controllers
{
    public class LoanInterestController : BaseController
    {
        //
        // GET: /Federation/LoanInterest/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Federation/LoanInterest/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Federation/LoanInterest/Create

        public ActionResult AddLoanIterest()
        {
            return View();
        }

        //
        // POST: /Federation/LoanInterest/Create

        [HttpPost]
        public ActionResult AddLoanIterest(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Federation/LoanInterest/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Federation/LoanInterest/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Federation/LoanInterest/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Federation/LoanInterest/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
