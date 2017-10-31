using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using BusinessEntities;
using BusinessLogic.AutoMapper;
using BusinessLogic;
//using BusinessLogic.Interface;
using MFIS.Web.Areas.Federation.Models;
using MFIS.Web.Controllers;
using System.Data;
using DataLogic;

namespace MFIS.Web.Areas.Federation.Controllers
{
    public class AccountTreeController : BaseController
    {

        #region Global Variable
        private AccountHeadService _accountHeadService;
        public AccountTreeController()
        {
            _accountHeadService = new AccountHeadService();
        }
        public List<AccountHeadDto> AccountHeadDtos
        {
            get
            {
                return Session["AccountHeadDtos"] as List<AccountHeadDto>;
            }
            set
            {
                Session["AccountHeadDtos"] = value;
            }
        }
        #endregion Global Variable
        //
        // GET: /Federation/AccountTree/
        public ActionResult AccountTree()
        {
            List<AccountHeadDto> lstAccountHeadDtos = _accountHeadService.GetAll(true);
            lstAccountHeadDtos= lstAccountHeadDtos.FindAll(l => l.StatusID != 3);

            var lstBankMasterDto = new AccountHeadService().GetOrganizationBanks();

            var ahlevel1 = lstAccountHeadDtos.FindAll(l => l.ParentAHID==0);
            var assetparent= ahlevel1.Find(l => l.AHCode.ToUpper() == "ASSETS");
            var ahlevl2= lstAccountHeadDtos.Find(l=> l.ParentAHID==assetparent.AHID && l.AHCode.ToUpper()=="CLOSING BALANCES");
            var ahlevl3 = lstAccountHeadDtos.Find(l => l.ParentAHID == ahlevl2.AHID && l.AHCode.ToUpper() == "CASH AND BANK");
            var ahlevel4 = lstAccountHeadDtos.Find(l => l.ParentAHID == ahlevl3.AHID && l.AHCode.ToUpper() == "BANK ACCOUNTS");

            var lstBankAccountHeads = lstAccountHeadDtos.FindAll(l => l.ParentAHID == ahlevel4.AHID);
            foreach (var bank in lstBankAccountHeads)
            {
                if (!lstBankMasterDto.Exists(l => l == bank.AHID))
                    lstAccountHeadDtos.Remove(bank);
            }

            //var lstBankAHIDS = new AccountHeadService().GetBankAHIDs();
            
            //foreach (int ahid in lstBankAHIDS)
            //{
            //    lstAccountHeadDtos.Remove(lstAccountHeadDtos.Find(l => l.AHID == ahid));
            //}
            //ReferenceValueService _referenceValueService = new ReferenceValueService();
            //var lstAHTypes = _referenceValueService.GetByRefMasterCode("AHTYPE");
            //var assettype = lstAHTypes.Find(l => l.RefCode.ToUpper() == "ASSETS");
            //var bankParentAH = lstAccountHeadDtos.Find(l => l.AHLevel == 4 && l.AHCode.ToUpper() == "BANK ACCOUNTS" && l.AHType == assettype.RefID && l.IsFederation);
           // lstAccountHeadDtos.RemoveAll(l => l.ParentAHID == bankParentAH.AHID);
            //end filtering the bankaccounts

            //lstAccountHeadDtos.FindAll(l => l.AHLevel > 3).ForEach(l => l.AHName = l.AHName + " - " + l.AHCode);
            ViewBag.AccountHeads = lstAccountHeadDtos;
            AccountHeadDtos = lstAccountHeadDtos;
            AccountHeadModel accountHeadModel = new AccountHeadModel();
            ViewBag.IsFederation = true;
            return View(accountHeadModel);
        }

        public ActionResult GroupAccountTree()
        {
            List<AccountHeadDto> lstAccountHeadDtos = _accountHeadService.GetAll(false);
            var lstBankAHIDS = new AccountHeadService().GetBankAHIDs();

            foreach (int ahid in lstBankAHIDS)
            {
                lstAccountHeadDtos.Remove(lstAccountHeadDtos.Find(l => l.AHID == ahid));
            }

            List<AccountHeadDto> lstExtraHeads = new List<AccountHeadDto>();
            foreach (var obj in lstAccountHeadDtos)
            {
                if (obj.AHName.Trim() == "1" || obj.AHName.Trim() == "2")
                    lstExtraHeads.Add(obj);
            }

            foreach (var obj in lstExtraHeads)
            {
                lstAccountHeadDtos.Remove(obj);
            }


            //for(int i = 0; i < lstAccountHeadDtos.Count(); i++)
            //{
            //    if (lstAccountHeadDtos[i].AHName == "1" || lstAccountHeadDtos[i].AHName == "2")
            //        lstAccountHeadDtos.RemoveAt(i);
            //}
            
            //lstAccountHeadDtos.FindAll(l => l.AHLevel > 3).ForEach(l => l.AHName = l.AHName + " - " + l.AHCode);
            ViewBag.AccountHeads = lstAccountHeadDtos;
            AccountHeadDtos = lstAccountHeadDtos;
            AccountHeadModel accountHeadModel = new AccountHeadModel();
            ViewBag.IsFederation = false;
            return View("AccountTree", accountHeadModel);
        }

        public ActionResult AddEditSubGroupAccountHead(AccountHeadModel accountHeadModel, FormCollection form)
        {

            ResultDto result = new ResultDto();
            //if (ModelState.IsValid)
            //{
            bool isFederation = string.IsNullOrEmpty(form.Get("isFederation")) ? true : Convert.ToBoolean(form.Get("isFederation"));

            accountHeadModel.UserID = 1;
            accountHeadModel.StatusID = 1;
            accountHeadModel.IsFederation = isFederation;
            accountHeadModel.AHID = accountHeadModel.EditAHID > 0 ? accountHeadModel.EditAHID : accountHeadModel.AHID;
            accountHeadModel.AHCode = !string.IsNullOrEmpty(accountHeadModel.SubGroupAHCode)
                ? accountHeadModel.SubGroupAHCode
                : accountHeadModel.AHCode;
            accountHeadModel.AHName = !string.IsNullOrEmpty(accountHeadModel.SubGroupAHName)
                ? accountHeadModel.SubGroupAHName
                : accountHeadModel.AHName;
            accountHeadModel.TE_AHName = !string.IsNullOrEmpty(accountHeadModel.SubGroupTEAHName)
                ? accountHeadModel.SubGroupTEAHName
                : accountHeadModel.TE_AHName;
            AccountHeadDto accountHeadDto = AutoMapperEntityConfiguration.Cast<AccountHeadDto>(accountHeadModel);
            if (accountHeadModel.AHID > 0)
                result = _accountHeadService.Update(accountHeadDto);
            else
                result = _accountHeadService.Insert(accountHeadDto);
            //}
            ViewBag.IsFederation = isFederation;
            TempData["SuccessMsg"] = result;
            if (isFederation)
                return RedirectToAction("AccountTree");
            else
                return RedirectToAction("GroupAccountTree");
        }

        public ActionResult DeleteSubGroupAccountHead()
        {
            bool isFederation = string.IsNullOrEmpty(Request.Form.Get("isFederation")) ? true : Convert.ToBoolean(Request.Form.Get("isFederation"));
            int userId = 1;
            int Ahid = !string.IsNullOrEmpty(Request.Form["DeleteAHID"]) ? Convert.ToInt32(Request.Form["DeleteAHID"]) : 0;
            ResultDto result = new ResultDto();
            result = _accountHeadService.Delete(Ahid, userId);
            TempData["SuccessMsg"] = result;
            ViewBag.IsFederation = isFederation;
            if (isFederation)
                return RedirectToAction("AccountTree");
            else
                return RedirectToAction("GroupAccountTree");

        }

        public ActionResult BindDropDowns(int parentAhid, int accountHeadLevel, string Id, bool isFederation)
        {
            StringBuilder sbDropdownOptions = new StringBuilder();
            #region ddlEditSubGroup

            if (Id == "SubGroup")
            {
                StringBuilder sbAccountHeadOptions = new StringBuilder();
                StringBuilder sbSubGroupOptions = new StringBuilder();


                if (accountHeadLevel == 4)
                {
                    var dropDownAccountHeadOptions = (from item in AccountHeadDtos
                                                      where item.ParentAHID == parentAhid && item.AHLevel == 4 && (item.IsFederation == isFederation || item.AHLevel < 4)
                                                      orderby item.AHType, item.AHName
                                                      select new { AHCode = item.AHID + ":" + item.AHCode, AhName = item.AHName + " - " + item.AHCode, TE_AHName = item.TE_AHName }
                                        );
                    foreach (var dropDownOption in dropDownAccountHeadOptions)
                    {
                        sbSubGroupOptions.Append("<option value='" + dropDownOption.AHCode + "'>" + dropDownOption.AhName + " - " + dropDownOption.TE_AHName + "</option>");
                    }
                }
                else
                {
                    var dropDownSubGroupOptions = (from item in AccountHeadDtos
                                                   where item.ParentAHID == parentAhid && item.AHLevel == 4 && (item.IsFederation == isFederation || item.AHLevel < 4)
                                                   orderby item.AHType, item.AHName
                                                   select new { ParentAhid = item.AHID, AhName = item.AHName + " - " + item.AHCode, TE_AHName = item.TE_AHName }
                                                        );
                    foreach (var dropDownOption in dropDownSubGroupOptions)
                    {
                        sbSubGroupOptions.Append("<option value='" + dropDownOption.ParentAhid + "'>" + dropDownOption.AhName + "</option>");
                    }
                }


                if (accountHeadLevel == 5)
                {
                    var dropDownAccountHeadOptions = (from item in AccountHeadDtos
                                                      where item.ParentAHID == parentAhid && item.AHLevel == 5 && (item.IsFederation == isFederation || item.AHLevel < 4)
                                                      orderby item.AHType, item.AHName
                                                      select new { AHCode = item.AHID + "_" + item.AHCode, AhName = item.AHName + " - " + item.AHCode, TE_AHName = item.TE_AHName }
                                        );
                    foreach (var dropDownOption in dropDownAccountHeadOptions)
                    {
                        sbAccountHeadOptions.Append("<option value='" + dropDownOption.AHCode + "'>" + dropDownOption.AhName + '-' + dropDownOption.TE_AHName + "</option>");

                    }
                }

                return Json(new { AccountHeadOptions = sbAccountHeadOptions != null ? sbAccountHeadOptions.ToString() : "", SubGroupOptions = sbSubGroupOptions != null ? sbSubGroupOptions.ToString() : "", TE_Ahname = "select" });

            }
            #endregion ddlEditSubGroup
            else if (Id == "SearchAccountHead")
            {

                var dropDownAccountHeadOptions = (from item in AccountHeadDtos
                                                  where item.ParentAHID == parentAhid && item.AHLevel == 5 && (item.IsFederation == isFederation || item.AHLevel < 4)
                                                  orderby item.AHType, item.AHName
                                                  select new { AHCode = item.AHID + ":" + item.AHCode, AhName = item.AHName + " - " + item.AHCode, TE_AHName = item.TE_AHName }
                                           );
                foreach (var dropDownOption in dropDownAccountHeadOptions)
                {
                    sbDropdownOptions.Append("<option value='" + dropDownOption.AHCode + "'>" + dropDownOption.AhName + '-' + dropDownOption.TE_AHName + "</option>");
                }

                return Json(new { DropDownOptions = sbDropdownOptions.ToString() });

            }
            else if (Id == "MoveSubGroupAccountHead")
            {
                var subGroupFields = (from item in AccountHeadDtos
                                      where item.ParentAHID == parentAhid && item.AHLevel == 4 && (item.IsFederation == isFederation || item.AHLevel < 4)
                                      orderby item.AHType, item.AHName
                                      select new { ParentAhid = item.AHID, AhName = item.AHName + " - " + item.AHCode }
                                    );

                var accountHeadFields = (from item in AccountHeadDtos
                                         where item.ParentAHID == parentAhid && item.AHLevel == 5 && (item.IsFederation == isFederation || item.AHLevel < 4)
                                         orderby item.AHType, item.AHName
                                         select new { ParentAhid = item.AHID, AhName = item.AHName + " - " + item.AHCode }
                                    );

                return Json(new { SubGroupFields = subGroupFields, AccountHeadFields = accountHeadFields });

            }
            else if (Id == "ViewBalanceAccountHead")
            {
                var dropDownOptions = (from item in AccountHeadDtos
                                       where item.AHLevel == accountHeadLevel && (item.IsFederation == isFederation || item.AHLevel < 4)
                                       orderby item.AHType,item.AHName
                                       select new { ParentAhid = item.AHID, AhName = item.AHName + " - " + item.AHCode }
                                    );

                foreach (var dropDownOption in dropDownOptions)
                {
                    sbDropdownOptions.Append("<option value='" + dropDownOption.ParentAhid + "'>" + dropDownOption.AhName + "</option>");
                }

                return Json(new { DropDownOptions = sbDropdownOptions.ToString() });
            }
            else
            {
                var dropDownOptions = (from item in AccountHeadDtos
                                       where item.ParentAHID == parentAhid && (item.IsFederation == isFederation || item.AHLevel < 4)
                                       select new { ParentAhid = item.AHID, AhName = item.AHName, TE_AHName = item.TE_AHName }
                                    );
                foreach (var dropDownOption in dropDownOptions)
                {
                    sbDropdownOptions.Append("<option value='" + dropDownOption.ParentAhid + "'>" + dropDownOption.AhName + "</option>");
                }

                return Json(new { DropDownOptions = sbDropdownOptions.ToString() });

            }

        }

        public ActionResult MoveSubGroupOrAccountHead(AccountHeadModel accountHeadModel)
        {
            bool isFederation = string.IsNullOrEmpty(Request.Form.Get("isFederation")) ? true : Convert.ToBoolean(Request.Form.Get("isFederation"));
            ResultDto result = new ResultDto();
            accountHeadModel.UserID = 1;
            accountHeadModel.StatusID = 1;
            accountHeadModel.ParentAHID = accountHeadModel.ParentMoveAHID;
            AccountHeadDto accountHeadDto = AutoMapperEntityConfiguration.Cast<AccountHeadDto>(accountHeadModel);
            result = _accountHeadService.MoveAccountHead(accountHeadDto);
            ViewBag.IsFederation = isFederation;
            TempData["SuccessMsg"] = result;
            if (isFederation)
                return RedirectToAction("AccountTree");
            else
                return RedirectToAction("GroupAccountTree");

        }

        public ActionResult ViewBalanceSummary(int ahId, bool isFederation)
        {
            AccountHeadDto accountHeadDto = _accountHeadService.GetAccountHeadViewBalanceSummary(ahId, isFederation);
            return Json(new { OpenBalance = accountHeadDto.OpeningBalance, ClosingBalance = accountHeadDto.ClosingBalance, CurrentYearBalanceDr = accountHeadDto.CurrentYearBalanceDr, CurrentYearBalanceCr = accountHeadDto.CurrentYearBalanceCr, CurrentYearBalance = accountHeadDto.CurrentYearBalance, BalanceType = accountHeadDto.OpeningBalanceType });
        }

        public ActionResult GetOpenBalance(int Ahid)
        {
            AccountHeadDto accountHeadDto = _accountHeadService.GetByID(Ahid);
            return Json(new { OpenBalance = accountHeadDto.OpeningBalance, IsSla = accountHeadDto.IsSLAccount, IsMemberTransAction = accountHeadDto.IsMemberTransaction });
        }

        public JsonResult CheckAccountHeadCode(string code, bool isFedaration, int ahid)
        {
            AccountHeadService iservice = new AccountHeadService();
            bool isSuccess = iservice.CheckAccountHeadCode(code, isFedaration, ahid);
            return Json(new { issuccess = isSuccess });
        }
        public ActionResult AccountHeadTableView()
        {
            DataTable dt = new DataTable();
            AccountHeadDll adll = new AccountHeadDll();
            dt = adll.GetAccountHeadsfebTableView();
            ViewBag.AccountHeadDt = dt;
            return View();
        }
    }
}
