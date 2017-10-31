using BusinessEntities;
using BusinessLogic.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CoreComponents;
using MFIS.Web.Controllers;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Xml;
using System.Configuration;
using System.Data.SqlClient;
using MFIS.Web.Areas.Group.Models;
using BusinessLogic;

namespace MFIS.Web.Areas.Group.Controllers.TransactionControllers
{
    public class GroupOtherPaymentsController : BaseController
    {
        #region Global Variables
        //private readonly GroupGeneralReceiptService _groupGeneralReceiptService;
        private readonly AccountHeadService _accountheadService;
        private readonly MasterService _masterService;
        private readonly GroupOtherReceiptService _groupOtherReceiptService;
        private readonly GroupOtherRecieptDto _groupOtherReceiptDto;
        public GroupOtherPaymentsController()
        {
            _masterService = new MasterService();
            _accountheadService = new AccountHeadService();
            //_groupGeneralReceiptService = new GroupGeneralReceiptService();
            _groupOtherReceiptService = new GroupOtherReceiptService();
            _groupOtherReceiptDto = new GroupOtherRecieptDto();
        }
        #endregion Global Variables 
        //
        // GET: /Group/GroupOtherPayments/
        [HttpGet]
        public ActionResult CreateGeneralPayments()
        {
            GeneralPaymentsDto objGeneralPaymentsModel = new GeneralPaymentsDto();
            List<GroupMeetingDto> lstGroupMeetings = _groupOtherReceiptService.MeetingDates(GroupInfo.GroupID);
            //If no meetings has been conducted user should not allow to make any transactions
            if (lstGroupMeetings == null || lstGroupMeetings.Count < 1)
            {
                return RedirectToAction("CreateGroupMeeting", "GroupMeeting");
            }
            lstGroupMeetings.ForEach(l => { l.DisplayMeetingDate = l.MeetingDate.ToDisplayDateFormat(); });
            ViewBag.MonthMeetings = new SelectList(lstGroupMeetings, "DisplayMeetingDate", "DisplayMeetingDate");
            LoadOtherReceiptDropDowns();
            return View(objGeneralPaymentsModel);
        }
        public void LoadOtherReceiptDropDowns()
        {

            TypeQueryResult lst = _masterService.GetTypeQueryResult("GROUP_GL_ACCOUNT_HEADS");
            ViewBag.lstGLAcHeads = new SelectList(lst.OrderBy(a => a.Name), "Id", "Name");

            TypeQueryResult lstBankAh = _masterService.GetTypeQueryResult("GROUP_OR_BANK_AH", GroupInfo.GroupID.ToString());
            ViewBag.slBankAh = new SelectList(lstBankAh.OrderBy(a => a.Name), "Id", "Name");

            TypeQueryResult lstEmp = _masterService.GetTypeQueryResult("ACT_EMPLOYEES", GroupInfo.GroupID.ToString());
            ViewBag.slEmp = new SelectList(lstEmp, "Id", "Name", UserInfo.UserID);
            List<AccountHeadDto> lstAccountHeadDtos = _accountheadService.GetGroupAccountTree(GroupInfo.GroupID);
            lstAccountHeadDtos = lstAccountHeadDtos.FindAll(l => l.AHLevel > 3).OrderBy(o => o.AHName).ToList();
            SelectList lstahcode = new SelectList(lstAccountHeadDtos.OrderBy(l => l.AHNameAndCode), "AHID", "AHNameAndCode");
            ViewBag.ahcodes = lstahcode;
        }
        [HttpPost]
        public ActionResult CreateGeneralPayments(FormCollection  form)
        {
            return View();
        }
    }
}
