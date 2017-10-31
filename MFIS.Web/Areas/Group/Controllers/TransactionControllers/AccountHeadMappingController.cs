using BusinessEntities;
using BusinessLogic;
using BusinessLogic.Implementation;
using MFIS.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MFIS.Web.Areas.Group.Controllers.TransactionControllers
{
    public class AccountHeadMappingController : BaseController
    {
        private readonly AccountHeadService _accountheadservice;
        private readonly AccountHeadMappingService _accountheadmappingService;
        public AccountHeadMappingController()
        {
            _accountheadservice = new AccountHeadService();
            _accountheadmappingService = new AccountHeadMappingService();
        }
        [HttpGet]
        public ActionResult AccountHeadMapping()
        {
            List<AccountheadMappingDto> lstMappedAccountHeads =  _accountheadmappingService.GetMappingAccountHeads();
            List<AccountHeadDto> lstaccounthead = _accountheadservice.GetallAccountHeads();
            var FederationAccountHeads = lstaccounthead.FindAll(l => l.IsFederation == true && l.AHLevel > 3);//.OrderBy(a => new { a.AHType, a.AHName });
            var GroupAccountHeads = lstaccounthead.FindAll(l => l.IsFederation == false && l.AHLevel > 3);//.OrderBy(a => new { a.AHType, a.AHName });
            SelectList GroupAccountHeadsDDL = new SelectList(GroupAccountHeads, "AHID", "AHName");
            ViewBag.GroupAccountHeadsDDL = GroupAccountHeadsDDL;
            ViewBag.FederationAccountHeads = FederationAccountHeads;
            ViewBag.GroupAccountHeads = GroupAccountHeads;
            ViewBag.MappingAccountHeads = lstMappedAccountHeads;
            
            return View();
        }

        [HttpPost]
        public ActionResult AccountHeadMapping(FormCollection form)
        {
            int count = Convert.ToInt32(Request.Form["hdnIndex"]);
            DataTable dt = new DataTable();
            dt.Columns.Add("FedAHId", typeof(Int32));
            dt.Columns.Add("GroupAHId", typeof(Int32));
            dt.Columns.Add("Status", typeof(Int32));
            dt.Columns.Add("CreatedBy", typeof(Int32));
            dt.Columns.Add("CreatedOn", typeof(DateTime));


            for (int i = 1; i <= count; i++)
            {
                if (form["AHID_" + i] == "")
                    continue;

                var FedAhId = Convert.ToInt32(form["hdnFedAhId_" + i]);
                var GroupAHID = Convert.ToInt32(form["AHID_" + i]);
                dt.Rows.Add(FedAhId, GroupAHID, 1,UserInfo.UserID, DateTime.Now);
            }
            ResultDto resultdto = _accountheadmappingService.InsertAccountHeadMappnig(dt);
            List<AccountheadMappingDto> lstMappedAccountHeads = _accountheadmappingService.GetMappingAccountHeads();
            List<AccountHeadDto> lstaccounthead = _accountheadservice.GetallAccountHeads();
            var FederationAccountHeads = lstaccounthead.FindAll(l => l.IsFederation == true && l.AHLevel > 3);//.OrderBy(a => new { a.AHType, a.AHName });
            var GroupAccountHeads = lstaccounthead.FindAll(l => l.IsFederation == false && l.AHLevel > 3);//.OrderBy(a => new { a.AHType, a.AHName });
            SelectList GroupAccountHeadsDDL = new SelectList(GroupAccountHeads, "AHID", "AHName");
            ViewBag.GroupAccountHeadsDDL = GroupAccountHeadsDDL;
            ViewBag.FederationAccountHeads = FederationAccountHeads;
            ViewBag.GroupAccountHeads = GroupAccountHeads;
            ViewBag.MappingAccountHeads = lstMappedAccountHeads;
            ViewBag.Result = resultdto;
            return View();
        }

        public ActionResult GetMappedHeads()
        {
            List<AccountheadMappingDto> lstMappedAccountHeads = _accountheadmappingService.GetMappingAccountHeads();
            
            return Json(new {lstMappedAccountHeads=lstMappedAccountHeads});
        }

    }
}
