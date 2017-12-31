using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using BusinessEntities;
using BusinessLogic.AutoMapper;
using BusinessLogic;
//using BusinessLogic.Interface;
using MFIS.Web.Areas.Federation.Models;
using MFIS.Web.Controllers;
using System.Data;
using DataLogic;
using BusinessLogic.Implementation;

namespace MFIS.Web.Areas.Group.Controllers
{
    public class GroupAccountTreeController : BaseController
    {
        //
        // GET: /Group/GroupAccountTree/

        #region Global Variable
        private AccountHeadService _accountHeadService;
        private readonly MasterService _masterService;

        public GroupAccountTreeController()
        {
            _accountHeadService = new AccountHeadService();
            _masterService = new MasterService();
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

        public ActionResult GroupAccountTree()
        {
            //List<AccountHeadDto> lstAccountHeadDtos = _accountHeadService.GetAll(false);
            //var lstBankAHIDS = new AccountHeadService().GetBankAHIDs();

            //foreach (int ahid in lstBankAHIDS)
            //{
            //    lstAccountHeadDtos.Remove(lstAccountHeadDtos.Find(l => l.AHID == ahid));
            //}
            List<AccountHeadDto> lstAccountHeadDtos = _accountHeadService.GetGroupAccountTree(GroupInfo.GroupID);
            ViewBag.AccountHeads = lstAccountHeadDtos;
            AccountHeadDtos = lstAccountHeadDtos;
            ViewBag.IsFederation = false;
            AccountHeadModel accountHeadModel = new AccountHeadModel();
            return View(accountHeadModel);
        }

        public ActionResult AddEditSubGroupAccountHead(AccountHeadModel accountHeadModel, FormCollection form)
        {
            //if (ModelState.IsValid)
            //{
            accountHeadModel.UserID = 1;
            accountHeadModel.StatusID = 1;
            accountHeadModel.IsFederation = true;
            accountHeadModel.AHID = accountHeadModel.EditAHID > 0 ? accountHeadModel.EditAHID : accountHeadModel.AHID;
            accountHeadModel.AHCode = !string.IsNullOrEmpty(accountHeadModel.SubGroupAHCode)
                ? accountHeadModel.SubGroupAHCode
                : accountHeadModel.AHCode;
            accountHeadModel.AHName = !string.IsNullOrEmpty(accountHeadModel.SubGroupAHName)
                ? accountHeadModel.SubGroupAHName
                : accountHeadModel.AHName;
            AccountHeadDto accountHeadDto = AutoMapperEntityConfiguration.Cast<AccountHeadDto>(accountHeadModel);
            if (accountHeadModel.AHID > 0)
                _accountHeadService.Update(accountHeadDto);
            else
                _accountHeadService.Insert(accountHeadDto);
            //}

            return RedirectToAction("GroupAccountTree");
        }

        public ActionResult DeleteSubGroupAccountHead()
        {
            int userId = 1;
            int Ahid = !string.IsNullOrEmpty(Request.Form["DeleteAHID"]) ? Convert.ToInt32(Request.Form["DeleteAHID"]) : 0;
            _accountHeadService.Delete(Ahid, userId);
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
                                                      select new { AHCode = item.AHID + ":" + item.AHCode, AhName = item.AHName + " - " + item.AHCode }
                                        );
                    foreach (var dropDownOption in dropDownAccountHeadOptions)
                    {
                        sbSubGroupOptions.Append("<option value='" + dropDownOption.AHCode + "'>" + dropDownOption.AhName + "</option>");
                    }
                }
                else
                {
                    var dropDownSubGroupOptions = (from item in AccountHeadDtos
                                                   where item.ParentAHID == parentAhid && item.AHLevel == 4 && (item.IsFederation == isFederation || item.AHLevel < 4)
                                                   select new { ParentAhid = item.AHID, AhName = item.AHName + " - " + item.AHCode }
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
                                                      select new { AHCode = item.AHID + "_" + item.AHCode, AhName = item.AHName + " - " + item.AHCode }
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
                                                  where item.ParentAHID == parentAhid && item.AHLevel == 5 && (item.IsFederation == isFederation || item.AHLevel < 4)
                                                  select new { AHCode = item.AHID + ":" + item.AHCode, AhName = item.AHName + " - " + item.AHCode }
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
                                      where item.ParentAHID == parentAhid && item.AHLevel == 4 && (item.IsFederation == isFederation || item.AHLevel < 4)
                                      select new { ParentAhid = item.AHID, AhName = item.AHName + " - " + item.AHCode }
                                    );

                var accountHeadFields = (from item in AccountHeadDtos
                                         where item.ParentAHID == parentAhid && item.AHLevel == 5 && (item.IsFederation == isFederation || item.AHLevel < 4)
                                         select new { ParentAhid = item.AHID, AhName = item.AHName + " - " + item.AHCode }
                                    );

                return Json(new { SubGroupFields = subGroupFields, AccountHeadFields = accountHeadFields });

            }
            else if (Id == "ViewBalanceAccountHead")
            {
                TypeQueryResult lst = _masterService.GetTypeQueryResult("GROUP_UPD_OPENING_BAL_ASSETS_LIABILITIES", "G");
                ViewBag.lstGLAcHeads = new SelectList(lst.OrderBy(a => a.Name), "Id", "Name");

                //var dropDownOptions = (from item in AccountHeadDtos
                //                       where item.AHLevel == accountHeadLevel && (item.IsFederation == isFederation || item.AHLevel < 4)
                //                       select new { ParentAhid = item.AHID, AhName = item.AHName + " - " + item.AHCode }
                //                    );

                foreach (var dropDownOption in lst)
                {
                    sbDropdownOptions.Append("<option value='" + dropDownOption.Id + "'>" + dropDownOption.Name + "</option>");
                }

                return Json(new { DropDownOptions = sbDropdownOptions.ToString() });
            }
            else
            {
                var dropDownOptions = (from item in AccountHeadDtos
                                       where item.ParentAHID == parentAhid && (item.IsFederation == isFederation || item.AHLevel < 4)
                                       select new { ParentAhid = item.AHID, AhName = item.AHName }
                                    );
                foreach (var dropDownOption in dropDownOptions)
                {
                    sbDropdownOptions.Append("<option value='" + dropDownOption.ParentAhid + "'>" + dropDownOption.AhName + "</option>");
                }

                return Json(new { DropDownOptions = sbDropdownOptions.ToString() });

            }

        }

        public ActionResult GetOB(int ahid)
        {
            AccountHeadDto resultDto = _accountHeadService.GetOB(ahid, GroupInfo.GroupID);
            return Json(new { balance = resultDto.OpeningBalance, OpeningBalanceType = resultDto.OpeningBalanceType });
        }
        public ActionResult UpdateOB(int ahid, double balance)
        {
            bool result = _accountHeadService.UpdateOB(ahid, GroupInfo.GroupID, (decimal)balance);
            return Json(new { result = result });
        }

        //public ActionResult BindDropDowns(int parentAhid, int accountHeadLevel, string Id,bool isFederation)
        //{
        //    StringBuilder sbDropdownOptions = new StringBuilder();
        //    #region ddlEditSubGroup

        //    if (Id == "SubGroup")
        //    {
        //        StringBuilder sbAccountHeadOptions = new StringBuilder();
        //        StringBuilder sbSubGroupOptions = new StringBuilder();


        //        if (accountHeadLevel == 4)
        //        {
        //            var dropDownAccountHeadOptions = (from item in AccountHeadDtos
        //                                              where item.ParentAHID == parentAhid && item.IsFederation && item.AHLevel == 4
        //                                              select new { AHCode = item.AHID + "_" + item.AHCode, AhName = item.AHName }
        //                                );
        //            foreach (var dropDownOption in dropDownAccountHeadOptions)
        //            {
        //                sbSubGroupOptions.Append("<option value='" + dropDownOption.AHCode + "'>" + dropDownOption.AhName + "</option>");
        //            }
        //        }
        //        else
        //        {
        //            var dropDownSubGroupOptions = (from item in AccountHeadDtos
        //                                           where item.ParentAHID == parentAhid && item.IsFederation && item.AHLevel == 4
        //                                           select new { ParentAhid = item.AHID, AhName = item.AHName }
        //                                                );
        //            foreach (var dropDownOption in dropDownSubGroupOptions)
        //            {
        //                sbSubGroupOptions.Append("<option value='" + dropDownOption.ParentAhid + "'>" + dropDownOption.AhName + "</option>");
        //            }
        //        }


        //        if (accountHeadLevel == 5)
        //        {
        //            var dropDownAccountHeadOptions = (from item in AccountHeadDtos
        //                                              where item.ParentAHID == parentAhid && item.IsFederation && item.AHLevel == 5
        //                                              select new { AHCode = item.AHID + "_" + item.AHCode, AhName = item.AHName }
        //                                );
        //            foreach (var dropDownOption in dropDownAccountHeadOptions)
        //            {
        //                sbAccountHeadOptions.Append("<option value='" + dropDownOption.AHCode + "'>" + dropDownOption.AhName + "</option>");
        //            }
        //        }

        //        return Json(new { AccountHeadOptions = sbAccountHeadOptions != null ? sbAccountHeadOptions.ToString() : "", SubGroupOptions = sbSubGroupOptions != null ? sbSubGroupOptions.ToString() : "" });

        //    }
        //    #endregion ddlEditSubGroup
        //    else if (Id == "SearchAccountHead")
        //    {

        //        var dropDownAccountHeadOptions = (from item in AccountHeadDtos
        //                                          where item.ParentAHID == parentAhid && item.IsFederation && item.AHLevel == 5
        //                                          select new { AHCode = item.AHID + "_" + item.AHCode, AhName = item.AHName }
        //                                   );
        //        foreach (var dropDownOption in dropDownAccountHeadOptions)
        //        {
        //            sbDropdownOptions.Append("<option value='" + dropDownOption.AHCode + "'>" + dropDownOption.AhName + "</option>");
        //        }

        //        return Json(new { DropDownOptions = sbDropdownOptions.ToString() });

        //    }
        //    else if (Id == "MoveSubGroupAccountHead")
        //    {
        //        var subGroupFields = (from item in AccountHeadDtos
        //                              where item.ParentAHID == parentAhid && item.IsFederation && item.AHLevel == 4
        //                              select new { ParentAhid = item.AHID, AhName = item.AHName }
        //                            );

        //        var accountHeadFields = (from item in AccountHeadDtos
        //                                 where item.ParentAHID == parentAhid && item.IsFederation && item.AHLevel == 5
        //                                 select new { ParentAhid = item.AHID, AhName = item.AHName }
        //                            );

        //        return Json(new { SubGroupFields = subGroupFields, AccountHeadFields = accountHeadFields });

        //    }
        //    else if (Id == "ViewBalanceAccountHead")
        //    {
        //        var dropDownOptions = (from item in AccountHeadDtos
        //                               where item.IsFederation && item.AHLevel == accountHeadLevel
        //                               select new { ParentAhid = item.AHID, AhName = item.AHName }
        //                            );

        //        foreach (var dropDownOption in dropDownOptions)
        //        {
        //            sbDropdownOptions.Append("<option value='" + dropDownOption.ParentAhid + "'>" + dropDownOption.AhName + "</option>");
        //        }

        //        return Json(new { DropDownOptions = sbDropdownOptions.ToString() });
        //    }
        //    else
        //    {
        //        var dropDownOptions = (from item in AccountHeadDtos
        //                               where item.ParentAHID == parentAhid && item.IsFederation
        //                               select new { ParentAhid = item.AHID, AhName = item.AHName }
        //                            );
        //        foreach (var dropDownOption in dropDownOptions)
        //        {
        //            sbDropdownOptions.Append("<option value='" + dropDownOption.ParentAhid + "'>" + dropDownOption.AhName + "</option>");
        //        }

        //        return Json(new { DropDownOptions = sbDropdownOptions.ToString() });

        //    }

        //}

        public ActionResult MoveSubGroupOrAccountHead(AccountHeadModel accountHeadModel)
        {
            accountHeadModel.UserID = 1;
            accountHeadModel.StatusID = 1;
            accountHeadModel.ParentAHID = accountHeadModel.ParentMoveAHID;
            AccountHeadDto accountHeadDto = AutoMapperEntityConfiguration.Cast<AccountHeadDto>(accountHeadModel);
            _accountHeadService.MoveAccountHead(accountHeadDto);
            return RedirectToAction("GroupAccountTree");

        }

        public ActionResult ViewBalanceSummary(int ahId, bool isFederation)
        {
            AccountHeadDto accountHeadDto = _accountHeadService.GetAccountHeadViewBalanceSummary(ahId, isFederation, GroupInfo.GroupID);
            return Json(new { OpenBalance = accountHeadDto.OpeningBalance, ClosingBalance = accountHeadDto.ClosingBalance, CurrentYearBalanceDr = accountHeadDto.CurrentYearBalanceDr, CurrentYearBalanceCr = accountHeadDto.CurrentYearBalanceCr, CurrentYearBalance = accountHeadDto.CurrentYearBalance, BalanceType = accountHeadDto.OpeningBalanceType });
        }

        public ActionResult GetOpenBalance(int Ahid)
        {
            AccountHeadDto accountHeadDto = _accountHeadService.GetByID(Ahid);
            return Json(new { OpenBalance = accountHeadDto.OpeningBalance });
        }
        public ActionResult AccountHeadTableView()
        {
            DataTable dt = new DataTable();
            AccountHeadDll adll = new AccountHeadDll();
            dt = adll.GetAccountHeadsTbleView(GroupInfo.GroupID);
            ViewBag.AccountHeadDt = dt;
            return View();
        }

        public JsonResult GetSubLedgerBankAccountHeadsByGLAHId(string glAHId)
        {
            // var slAccountHeads = _groupOtherReceiptService.GetSLAccountHeads(int.Parse(glAHId));
            TypeQueryResult lstBankAh = _masterService.GetTypeQueryResult("GROUP_OR_BANK_AH", GroupInfo.GroupID.ToString(), glAHId);
           // ViewBag.slBankAh = new SelectList(lstBankAh.OrderBy(a => a.Name), "Id", "Name");

            return Json(new { slAccountHeads = lstBankAh });
        }
    }
}
