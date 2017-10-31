using BusinessEntities;
using BusinessLogic;
using BusinessLogic.AutoMapper;
using MFIS.Web.Areas.Federation.Models;
using MFIS.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace MFIS.Web.Areas.Federation.Controllers
{
    public class GroupAccountTreeController : BaseController
    {
        #region Global Variable
        private AccountHeadService _accountHeadService;
        public GroupAccountTreeController()
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
           
            List<AccountHeadDto> lstAccountHeadDtos = _accountHeadService.GetAll(false);
            lstAccountHeadDtos = lstAccountHeadDtos.FindAll(l => l.StatusID != 3);

            var lstBankMasterDto = new AccountHeadService().GetOrganizationBanks();

            var ahlevel1 = lstAccountHeadDtos.FindAll(l => l.ParentAHID == 0);
            var assetparent = ahlevel1.Find(l => l.AHCode.ToUpper() == "ASSETS");
            var ahlevl2 = lstAccountHeadDtos.Find(l => l.ParentAHID == assetparent.AHID && l.AHCode.ToUpper() == "CLOSING BALANCES");
            var ahlevl3 = lstAccountHeadDtos.Find(l => l.ParentAHID == ahlevl2.AHID && l.AHCode.ToUpper() == "CASH AND BANK");
            var ahlevel4 = lstAccountHeadDtos.Find(l => l.ParentAHID == ahlevl3.AHID && l.AHCode.ToUpper() == "BANK ACCOUNTS");

            var lstBankAccountHeads = lstAccountHeadDtos.FindAll(l => l.ParentAHID == ahlevel4.AHID);
            foreach (var bank in lstBankAccountHeads)
            {
                if (!lstBankMasterDto.Exists(l => l == bank.AHID))
                    lstAccountHeadDtos.Remove(bank);
            }

            ViewBag.AccountHeads = lstAccountHeadDtos;
            AccountHeadDtos = lstAccountHeadDtos;
            AccountHeadModel accountHeadModel = new AccountHeadModel();
            return View(accountHeadModel);
        }

        public ActionResult AddEditSubGroupAccountHead(AccountHeadModel accountHeadModel, FormCollection form)
        {

            ResultDto result = new ResultDto();
            //if (ModelState.IsValid)
            //{
            accountHeadModel.UserID = 1;
            accountHeadModel.StatusID = 1;
            accountHeadModel.IsFederation = false;
            accountHeadModel.AHID = accountHeadModel.EditAHID > 0 ? accountHeadModel.EditAHID : accountHeadModel.AHID;
            accountHeadModel.AHCode = !string.IsNullOrEmpty(accountHeadModel.SubGroupAHCode)
                ? accountHeadModel.SubGroupAHCode
                : accountHeadModel.AHCode;
            accountHeadModel.AHName = !string.IsNullOrEmpty(accountHeadModel.SubGroupAHName)
                ? accountHeadModel.SubGroupAHName
                : accountHeadModel.AHName;
            AccountHeadDto accountHeadDto = AutoMapperEntityConfiguration.Cast<AccountHeadDto>(accountHeadModel);
            if (accountHeadModel.AHID > 0)
                result = _accountHeadService.Update(accountHeadDto);
            else
                result = _accountHeadService.Insert(accountHeadDto);
            //}

            TempData["SuccessMsg"] = result;
            return RedirectToAction("AccountTree");
        }

        public ActionResult DeleteSubGroupAccountHead()
        {
            int userId = 1;
            int Ahid = !string.IsNullOrEmpty(Request.Form["DeleteAHID"]) ? Convert.ToInt32(Request.Form["DeleteAHID"]) : 0;
            ResultDto result = new ResultDto();
            result = _accountHeadService.Delete(Ahid, userId);
            TempData["SuccessMsg"] = result;

            return RedirectToAction("AccountTree");
        }

        public ActionResult BindDropDowns(int parentAhid, int accountHeadLevel, string Id)
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
                                                      where item.ParentAHID == parentAhid && item.IsFederation && item.AHLevel == 4
                                                      select new { AHCode = item.AHID + ":" + item.AHCode, AhName = item.AHName }
                                        );
                    foreach (var dropDownOption in dropDownAccountHeadOptions)
                    {
                        sbSubGroupOptions.Append("<option value='" + dropDownOption.AHCode + "'>" + dropDownOption.AhName + "</option>");
                    }
                }
                else
                {
                    var dropDownSubGroupOptions = (from item in AccountHeadDtos
                                                   where item.ParentAHID == parentAhid && item.IsFederation && item.AHLevel == 4
                                                   select new { ParentAhid = item.AHID, AhName = item.AHName }
                                                        );
                    foreach (var dropDownOption in dropDownSubGroupOptions)
                    {
                        sbSubGroupOptions.Append("<option value='" + dropDownOption.ParentAhid + "'>" + dropDownOption.AhName + "</option>");
                    }
                }


                if (accountHeadLevel == 5)
                {
                    var dropDownAccountHeadOptions = (from item in AccountHeadDtos
                                                      where item.ParentAHID == parentAhid && item.IsFederation && item.AHLevel == 5
                                                      select new { AHCode = item.AHID + "_" + item.AHCode, AhName = item.AHName }
                                        );
                    foreach (var dropDownOption in dropDownAccountHeadOptions)
                    {
                        sbAccountHeadOptions.Append("<option value='" + dropDownOption.AHCode + "'>" + dropDownOption.AhName + "</option>");
                    }
                }

                return Json(new { AccountHeadOptions = sbAccountHeadOptions != null ? sbAccountHeadOptions.ToString() : "", SubGroupOptions = sbSubGroupOptions != null ? sbSubGroupOptions.ToString() : "" });

            }
            #endregion ddlEditSubGroup
            else if (Id == "SearchAccountHead")
            {

                var dropDownAccountHeadOptions = (from item in AccountHeadDtos
                                                  where item.ParentAHID == parentAhid && item.IsFederation && item.AHLevel == 5
                                                  select new { AHCode = item.AHID + ":" + item.AHCode, AhName = item.AHName }
                                           );
                foreach (var dropDownOption in dropDownAccountHeadOptions)
                {
                    sbDropdownOptions.Append("<option value='" + dropDownOption.AHCode + "'>" + dropDownOption.AhName + "</option>");
                }

                return Json(new { DropDownOptions = sbDropdownOptions.ToString() });

            }
            else if (Id == "MoveSubGroupAccountHead")
            {
                var subGroupFields = (from item in AccountHeadDtos
                                      where item.ParentAHID == parentAhid && item.IsFederation && item.AHLevel == 4
                                      select new { ParentAhid = item.AHID, AhName = item.AHName }
                                    );

                var accountHeadFields = (from item in AccountHeadDtos
                                         where item.ParentAHID == parentAhid && item.IsFederation && item.AHLevel == 5
                                         select new { ParentAhid = item.AHID, AhName = item.AHName }
                                    );

                return Json(new { SubGroupFields = subGroupFields, AccountHeadFields = accountHeadFields });

            }
            else if (Id == "ViewBalanceAccountHead")
            {
                var dropDownOptions = (from item in AccountHeadDtos
                                       where item.IsFederation && item.AHLevel == accountHeadLevel
                                       select new { ParentAhid = item.AHID, AhName = item.AHName }
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
                                       where item.ParentAHID == parentAhid && item.IsFederation
                                       select new { ParentAhid = item.AHID, AhName = item.AHName }
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
            ResultDto result = new ResultDto();
            accountHeadModel.UserID = 1;
            accountHeadModel.StatusID = 1;
            accountHeadModel.ParentAHID = accountHeadModel.ParentMoveAHID;
            AccountHeadDto accountHeadDto = AutoMapperEntityConfiguration.Cast<AccountHeadDto>(accountHeadModel);
            result = _accountHeadService.MoveAccountHead(accountHeadDto);
            TempData["SuccessMsg"] = result;
            return RedirectToAction("AccountTree");

        }

        public ActionResult ViewBalanceSummary(int ahId, bool isFederation)
        {
            AccountHeadDto accountHeadDto = _accountHeadService.GetAccountHeadViewBalanceSummary(ahId, isFederation);
            return Json(new { OpenBalance = accountHeadDto.OpeningBalance, ClosingBalance = accountHeadDto.ClosingBalance, CurrentYearBalance = accountHeadDto.CurrentYearBalance, BalanceType = accountHeadDto.OpeningBalanceType });
        }

        public ActionResult GetOpenBalance(int Ahid)
        {
            AccountHeadDto accountHeadDto = _accountHeadService.GetByID(Ahid);
            return Json(new { OpenBalance = accountHeadDto.OpeningBalance, IsSla = accountHeadDto.IsSLAccount, IsMemberTransAction = accountHeadDto.IsMemberTransaction });
        }

    }
}
