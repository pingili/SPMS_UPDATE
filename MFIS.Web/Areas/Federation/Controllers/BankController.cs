using AutoMapper;
using BusinessEntities;
using BusinessLogic;
using MFIS.Web.Areas.Federation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utilities;
//using BusinessLogic.Interface;
using MFIS.Web.Controllers;
using BusinessLogic.AutoMapper;
using CoreComponents;
using log4net;
using DataLogic.Implementation;

namespace MFIS.Web.Areas.Federation.Controllers
{

    public class BankController : BaseController
    {
        log4net.ILog _log = log4net.LogManager.GetLogger(typeof(BankController));  //Declaring Log4Net  

        #region Global Variables
        private BankService _bankService;
        public BankController()
        {
            _bankService = new BankService();
        }

        #endregion Global Variables


        [HttpGet]
        public ActionResult AddBank(string id)
        {
            _log.Info("AddBank() HTTP GET METHOD BEGIN");
            BankMasterModel bankMasterModel = new BankMasterModel();

            try
            {
                int bankId = string.IsNullOrEmpty(id.DecryptString()) ? default(int) : Convert.ToInt32(id.DecryptString());

                SelectList bankNames = GetDropDownListByMasterCode(Enums.RefMasterCodes.BANK_NAME);
                SelectList accountTypes = GetDropDownListByMasterCode(Enums.RefMasterCodes.BANK_ACCOUNT_TYPE);
                ViewBag.BankNames = bankNames;
                ViewBag.AccountTypes = accountTypes;
                BankMasterDto bankMasterDto = new BankMasterDto();
                if (bankId > 0)
                {
                    bankMasterDto = _bankService.GetByID(bankId);
                    bankMasterModel = AutoMapperEntityConfiguration.Cast<BankMasterModel>(bankMasterDto);
                }


                ViewBag.Result = new ResultDto();
            }
            catch (Exception ex)
            {
                _log.Error("Error : Error occured in AddBank() Http Get Method", ex);
            }
            _log.Info("AddBank() HTTP GET METHOD END");

            return View("AddBank", bankMasterModel);
        }

        [HttpPost]
        public ActionResult AddBank(BankMasterModel bankMasterModel)
        {
            ResultDto resultDto = new ResultDto();
            if (ModelState.IsValid)
            {
                var bankMasterDto = Mapper.Map<BankMasterModel, BankMasterDto>(bankMasterModel);
                bankMasterDto.UserID = UserInfo.UserID;
                bankMasterDto.isMasterEntry = true;
                if (bankMasterModel.BankEntryID == 0)
                    resultDto = _bankService.Insert(bankMasterDto);
                else
                    resultDto = _bankService.Update(bankMasterDto);
                if (resultDto.ObjectId > 0)
                {
                    bankMasterDto = _bankService.GetByID(resultDto.ObjectId);
                    bankMasterModel = AutoMapperEntityConfiguration.Cast<BankMasterModel>(bankMasterDto);
                    resultDto.ObjectCode = bankMasterDto.BankCode;
                }
            }

            SelectList bankNames = GetDropDownListByMasterCode(Enums.RefMasterCodes.BANK_NAME);
            SelectList accountTypes = GetDropDownListByMasterCode(Enums.RefMasterCodes.BANK_ACCOUNT_TYPE);
            ViewBag.BankNames = bankNames;
            ViewBag.AccountTypes = accountTypes;
            
            ViewBag.Result = resultDto;
            return View("AddBank", bankMasterModel);
        }

        [HttpGet]
        public ActionResult BankMasterLookup()
        {
            int? groupId = null;
            BankMasterDal _bankmasterdal = new BankMasterDal();
            bool isGroup = GroupInfo != null && GroupInfo.GroupID > 1;
            if (isGroup)
                groupId = GroupInfo.GroupID;
            List<BankMasterLookupDto> bankMasterLookups = _bankmasterdal.GetLookup(groupId);

            ViewBag.isGroup = isGroup;
            return View("BankLookup", bankMasterLookups);
        }

        [HttpGet]
        public ActionResult ViewBank(string id)
        {
            int bankId = DecryptQueryString(id);

            if (bankId <= 0)
                return RedirectToAction("BankLookup");

            bool isGroup = GroupInfo != null && GroupInfo.GroupID > 1;
            ViewBag.isGroup = isGroup;

            var bankMasterViewDto = _bankService.GetViewByID(bankId);

            return View(bankMasterViewDto);
        }

        [HttpGet]
        public ActionResult DeleteBankMaster(string Id)
        {
            int bankentryId = DecryptQueryString(Id);

            if (bankentryId < 1)
                return RedirectToAction("BankMasterLookup");

            ResultDto resultDto = _bankService.Delete(bankentryId, UserInfo.UserID);

            TempData["Result"] = resultDto;

            return RedirectToAction("BankMasterLookup");
        }

        [HttpGet]
        public ActionResult ActiveInactiveBankMaster(string Id)
        {
            int bankentryId = DecryptQueryString(Id);

            if (bankentryId < 1)
                return RedirectToAction("BankMasterLookup");

            ResultDto resultDto = _bankService.ChangeStatus(bankentryId, UserInfo.UserID);

            TempData["Result"] = resultDto;

            return RedirectToAction("BankMasterLookup");
        }

    }
}
