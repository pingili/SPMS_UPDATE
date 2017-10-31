using AutoMapper;
using BusinessEntities;
using BusinessLogic.AutoMapper;
using BusinessLogic;
//using BusinessLogic.Interface;
using CoreComponents;
using MFIS.Web.Areas.Federation.Models;
using MFIS.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utilities;

namespace MFIS.Web.Areas.Federation.Controllers
{
    public class InterestController : BaseController
    {
        #region Global Variables
        private readonly InterestService _interestService;
        private readonly AccountHeadService _accountHeadService;

        public InterestController()
        {
            _interestService = new InterestService();
            _accountHeadService = new AccountHeadService();
        }

        #endregion Global Variables

        #region Deposit Interest

        [HttpGet]
        public ActionResult DepositInterestLookup()
        {
            var lstDepositInterestLookupDto = _interestService.GetLookup(Enums.InterestTypes.D);
            return View("DepositInterestLookup", lstDepositInterestLookupDto);
        }

        [HttpGet]
        public ActionResult CreateDepositInterest(string id)
        {
            int interestId = string.IsNullOrEmpty(id.DecryptString()) ? default(int) : Convert.ToInt32(id.DecryptString());

            var interestDto = new InterestMasterDto();

            if (interestId > 0)
            {
                interestDto = _interestService.GetByID(interestId);
            }

            LoadDropDowns();

            ViewBag.Result = new ResultDto();

            return View(interestDto);
        }

        public ActionResult ViewDepositInterest(string id)
        {
            int interestId = DecryptQueryString(id);

            if (interestId <= 0)
                return RedirectToAction("DepositInterestLookup");

            var depositInterestViewDto = _interestService.GetViewByID(interestId);

            return View(depositInterestViewDto);
        }

        

        private void LoadDropDowns()
        {
            var lstAccountHeads = _accountHeadService.GetAll(true);
            List<AccountHeadDto> lstAccountHeadDto = new List<AccountHeadDto>();

            var depositAH = lstAccountHeads.Find(l => l.AHCode == "DEPOSITS" && l.AHLevel == 2);//TODO:Sample Example Need to Change
            var depositSubAH = lstAccountHeads.FindAll(l => l.ParentAHID == depositAH.AHID && l.AHLevel == 3);
            foreach (var majorGroupAH in depositSubAH)
            {
                var subGroupAh = lstAccountHeads.FindAll(l => l.ParentAHID == majorGroupAH.AHID && l.AHLevel == 4);
                foreach (var sgAH in subGroupAh)
                {
                    var lstAH = lstAccountHeads.FindAll(l => l.ParentAHID == sgAH.AHID && l.AHLevel == 5);
                    foreach (var AH in lstAH)
                    {
                        lstAccountHeadDto.Add(new AccountHeadDto()
                        {
                            AHID = AH.AHID,
                            AHName = AH.AHName
                        });
                    }
                }
                var ahMglevel = lstAccountHeads.FindAll(l => l.ParentAHID == majorGroupAH.AHID && l.AHLevel == 5);

                foreach (var mgAH in ahMglevel)
                {
                    lstAccountHeadDto.Add(new AccountHeadDto()
                    {
                        AHID = mgAH.AHID,
                        AHName = mgAH.AHName
                    });
                }
            }

         
            var lstinterestmaster = _interestService.GetAll(Enums.InterestTypes.D);
            foreach (var interestmaster in lstinterestmaster)
            {
                lstAccountHeadDto.Remove(lstAccountHeadDto.Find(l => l.AHID==interestmaster.PrincipalAHID));
            }
            SelectList principleAHSelectList = new SelectList(lstAccountHeadDto, "AHID", "AHName");
            ViewBag.PrincipleAcHeads = principleAHSelectList;

            //var interestAHDto = lstAccountHeads.FindAll(l => l.AHID == 4);//TODO:Sample Example Need to Change
            //foreach (var interest in interestAHDto)
            //{
            //    AccountHeadDto objaccount = new AccountHeadDto()
            //    {
            //        AHID=interest.AHID,
            //        AHName=interest.AHName
                
            //    };
            //    lstAccountHeadDto.Add(objaccount);
                
            //}
            List<AccountHeadDto> lstInterstAccountHeadDto = new List<AccountHeadDto>();
            var interestExpressAH = lstAccountHeads.Find(l => l.AHCode == "INTEREST EXPENSES" && l.AHLevel == 2);//TODO:Sample Example Need to Change
            var interestExpressSubAH = lstAccountHeads.FindAll(l => l.ParentAHID == interestExpressAH.AHID && l.AHLevel == 3);
            foreach (var majorGroupAH in interestExpressSubAH)
            {
                var subGroupAh = lstAccountHeads.FindAll(l => l.ParentAHID == majorGroupAH.AHID && l.AHLevel == 4);
                foreach (var sgAH in subGroupAh)
                {
                    var lstAH = lstAccountHeads.FindAll(l => l.ParentAHID == sgAH.AHID && l.AHLevel == 5);
                    foreach (var AH in lstAH)
                    {
                        lstInterstAccountHeadDto.Add(new AccountHeadDto()
                        {
                            AHID = AH.AHID,
                            AHName = AH.AHName
                        });
                    }
                }
                var ahMglevel = lstAccountHeads.FindAll(l => l.ParentAHID == majorGroupAH.AHID && l.AHLevel == 5);

                foreach (var mgAH in ahMglevel)
                {
                    lstInterstAccountHeadDto.Add(new AccountHeadDto()
                    {
                        AHID = mgAH.AHID,
                        AHName = mgAH.AHName
                    });
                }
            }
            var lstinterests = _interestService.GetAll(Enums.InterestTypes.L);
            foreach (var interestmaster in lstinterests)
            {
                lstInterstAccountHeadDto.Remove(lstInterstAccountHeadDto.Find(l => l.AHID == interestmaster.InterestAHID));
            }
            SelectList interestAHSelectList = new SelectList(lstInterstAccountHeadDto, "AHID", "AHName");
            ViewBag.InterestAcHeads = interestAHSelectList;

            //TODO : SECTION SHOULD REMOVE
            var accountHeadSelectListDto1 = _accountHeadService.GetAccountHeadSelectList();
            var penalAHDto = accountHeadSelectListDto1;//.FindAll(l => l.ID == 1);//TODO:Sample Example Need to Change
            SelectList penalAHSelectList = new SelectList(penalAHDto, "ID", "Text");
            ViewBag.PenalAcHeads = penalAHSelectList;
             
            SelectList baseTypeSelectList = GetDropDownListByMasterCode(Enums.RefMasterCodes.INTEREST_BASE_TYPE);
            ViewBag.BaseTypes = baseTypeSelectList;

            SelectList calMethodSelectList = GetDropDownListByMasterCode(Enums.RefMasterCodes.INTEREST_CALC_TYPE);
            ViewBag.CalcTypes = calMethodSelectList;
        }

        [HttpPost]
        public ActionResult CreateDepositInterest(FormCollection form)
        {
            var interestMasterDto = ReadFormData(form);
            var resultDto = new ResultDto();

            LoadDropDowns();
            if (interestMasterDto.InterestID == 0)
                resultDto = _interestService.Insert(interestMasterDto, Enums.InterestTypes.D);
            else
                resultDto = _interestService.Update(interestMasterDto, Enums.InterestTypes.D);

            if (resultDto.ObjectId > 0)
            {
                //LoadDropDowns();
                interestMasterDto = _interestService.GetByID(resultDto.ObjectId);

                resultDto.ObjectCode = interestMasterDto.InterestCode;
            }
            ViewBag.Result = resultDto;

            return View(interestMasterDto);
        }

        private InterestMasterDto ReadFormData(FormCollection form)
        {
            InterestMasterDto interestMasterDto = new InterestMasterDto();
            int interestId = default(int);
            int.TryParse(form["InterestID"], out interestId);

            int? penalAhId = null;

            interestMasterDto.InterestID = Convert.ToInt32(form["InterestID"]);
            interestMasterDto.InterestCode = Convert.ToString(form["InterestCode"]);
            interestMasterDto.InterestName = Convert.ToString(form["InterestName"]);
            interestMasterDto.Base = Convert.ToInt32(form["Base"]);
            interestMasterDto.CaluculationMethod = Convert.ToInt32(form["CaluculationMethod"]);
            interestMasterDto.PrincipalAHID = Convert.ToInt32(form["PrincipalAHID"]);
            interestMasterDto.InterestAHID = Convert.ToInt32(form["InterestAHID"]);
            interestMasterDto.PenalAHID = penalAhId; //Convert.ToInt32(form["PenalAHID"]);
            interestMasterDto.UserId = UserInfo.UserID;

            int maxIndex = Convert.ToInt32(form["hdnMaxRateIndex"]);

            interestMasterDto.InterestRates = new List<InterestRatesDto>();
            InterestRatesDto rate = null;
            for (int i = 1; i <= maxIndex; i++)
            {
                if (form["hdnROI_" + i] == null) continue;

                DateTime dtFromDate = form["hdnFDate_" + i].ConvertToDateTime();
                DateTime dtToDate = form["hdnToDate_" + i].ConvertToDateTime();

                rate = new InterestRatesDto();
                rate.ROI = Convert.ToDecimal(form["hdnROI_" + i]);
                rate.PenalROI = Convert.ToDecimal(form["hdnPROI_" + i]);
                rate.FromDate = dtFromDate;
                rate.ToDate = dtToDate;
                rate.IntrestRateID = Convert.ToInt32(form["hdnRateId_" + i]);

                interestMasterDto.InterestRates.Add(rate);
            }

            return interestMasterDto;
        }
   

        [HttpGet]
        public ActionResult ActiveInactiveDepositInterest(string Id)
        {
            int interestID = DecryptQueryString(Id);

            if (interestID < 1)
                return RedirectToAction("DepositInterestLookup");

            ResultDto resultDto = _interestService.ChangeStatus(interestID, UserInfo.UserID);

            TempData["Result"] = resultDto;

            return RedirectToAction("DepositInterestLookup");
        }

        #endregion

        #region Loan Interest
        [HttpGet]
        public ActionResult LoanInterestLookup()
        {
            var lstLoanInterestLookupDto = _interestService.GetLookup(Enums.InterestTypes.L);
            return View("LoanInterestLookup", lstLoanInterestLookupDto);
        }

        [HttpGet]
        public ActionResult CreateLoanInterest(string id)
        {
            int interestId = string.IsNullOrEmpty(id.DecryptString()) ? default(int) : Convert.ToInt32(id.DecryptString());

            var interestDto = new InterestMasterDto();

            if (interestId > 0)
            {
                interestDto = _interestService.GetByID(interestId);
            }

            LoadLoanInterestDropDowns();

            ViewBag.Result = new ResultDto();

            return View(interestDto);
        }
        [HttpGet]
        public ActionResult ViewLoanInterest(string id)
        {
            int interestId = DecryptQueryString(id);

            if (interestId <= 0)
                return RedirectToAction("LoanInterestLookup");

            var loanInterestViewDto = _interestService.GetLoanViewByID(interestId);

            return View(loanInterestViewDto);
        }

        private void LoadLoanInterestDropDowns()
        {



            var lstAccountHeads = _accountHeadService.GetAll(true);
            List<AccountHeadDto> lstAccountHeadDto = new List<AccountHeadDto>();

            var memberloanAH = lstAccountHeads.Find(l => l.AHCode == "MEMBER LOANS OUTSTANDING" && l.AHLevel == 2);//TODO:Sample Example Need to Change
            var memberloanSubAH = lstAccountHeads.FindAll(l => l.ParentAHID == memberloanAH.AHID && l.AHLevel == 3);
            foreach (var majorGroupAH in memberloanSubAH)
            {
                var subGroupAh = lstAccountHeads.FindAll(l => l.ParentAHID == majorGroupAH.AHID && l.AHLevel == 4);
                foreach (var sgAH in subGroupAh)
                {
                    var lstAH = lstAccountHeads.FindAll(l => l.ParentAHID == sgAH.AHID && l.AHLevel == 5);
                    foreach (var AH in lstAH)
                    {
                        lstAccountHeadDto.Add(new AccountHeadDto()
                        {
                            AHID = AH.AHID,
                            AHName = AH.AHName
                        });
                    }
                }
                var ahMglevel = lstAccountHeads.FindAll(l => l.ParentAHID == majorGroupAH.AHID && l.AHLevel == 5);

                foreach (var mgAH in ahMglevel)
                {
                    lstAccountHeadDto.Add(new AccountHeadDto()
                    {
                        AHID = mgAH.AHID,
                        AHName = mgAH.AHName
                    });
                }
            }
            var lstinterests = _interestService.GetAll(Enums.InterestTypes.L);
            foreach (var interestmaster in lstinterests)
            {
                lstAccountHeadDto.Remove(lstAccountHeadDto.Find(l => l.AHID == interestmaster.PrincipalAHID));
            }
            SelectList principleAHSelectList = new SelectList(lstAccountHeadDto, "AHID", "AHName");
            ViewBag.PrincipleAcHeads = principleAHSelectList;

            //var interestAHDto = lstAccountHeads.FindAll(l => l.AHID == 4);//TODO:Sample Example Need to Change
            //foreach (var interest in interestAHDto)
            //{
            //    AccountHeadDto objaccount = new AccountHeadDto()
            //    {
            //        AHID=interest.AHID,
            //        AHName=interest.AHName

            //    };
            //    lstAccountHeadDto.Add(objaccount);

            //}
            List<AccountHeadDto> lstInterstAccountHeadDto = new List<AccountHeadDto>();
            var incomeFromInterestAH = lstAccountHeads.Find(l => l.AHCode == "INCOME FROM INTEREST" && l.AHLevel == 2);//TODO:Sample Example Need to Change
            var incomeFromInterestSubAH = lstAccountHeads.FindAll(l => l.ParentAHID == incomeFromInterestAH.AHID && l.AHLevel == 3);
            foreach (var majorGroupAH in incomeFromInterestSubAH)
            {
                var subGroupAh = lstAccountHeads.FindAll(l => l.ParentAHID == majorGroupAH.AHID && l.AHLevel == 4);
                foreach (var sgAH in subGroupAh)
                {
                    var lstAH = lstAccountHeads.FindAll(l => l.ParentAHID == sgAH.AHID && l.AHLevel == 5);
                    foreach (var AH in lstAH)
                    {
                        lstInterstAccountHeadDto.Add(new AccountHeadDto()
                        {
                            AHID = AH.AHID,
                            AHName = AH.AHName
                        });
                    }
                }
                var ahMglevel = lstAccountHeads.FindAll(l => l.ParentAHID == majorGroupAH.AHID && l.AHLevel == 5);

                foreach (var mgAH in ahMglevel)
                {
                    lstInterstAccountHeadDto.Add(new AccountHeadDto()
                    {
                        AHID = mgAH.AHID,
                        AHName = mgAH.AHName
                    });
                }
            }
            var lstinterestmaster = _interestService.GetAll(Enums.InterestTypes.L);
            foreach (var interestmaster in lstinterestmaster)
            {
                lstInterstAccountHeadDto.Remove(lstInterstAccountHeadDto.Find(l => l.AHID == interestmaster.InterestAHID));
            }
            SelectList interestAHSelectList = new SelectList(lstInterstAccountHeadDto, "AHID", "AHName");
            ViewBag.InterestAcHeads = interestAHSelectList;
            var accountHeadSelectListDto1 = _accountHeadService.GetAccountHeadSelectList();
            //var accountHeadIncomeListDto = _accountHeadService.GetAll();
            //var principleAHDto = accountHeadSelectListDto;//.FindAll(l => l.ID == 1);//TODO:Sample Example Need to Change
            //SelectList principleAHSelectList = new SelectList(principleAHDto, "ID", "Text");
            //ViewBag.PrincipleAcHeads = principleAHSelectList;

            //var interestAHDto = accountHeadSelectListDto;//.FindAll(l => l.ID == 1);//TODO:Sample Example Need to Change
            //SelectList interestAHSelectList = new SelectList(interestAHDto, "ID", "Text");
            //ViewBag.InterestAcHeads = interestAHSelectList;

            var penalAHDto = accountHeadSelectListDto1;//.FindAll(l => l.ID == 1);//TODO:Sample Example Need to Change
            SelectList penalAHSelectList = new SelectList(penalAHDto, "ID", "Text");
            ViewBag.PenalAcHeads = penalAHSelectList;

            SelectList baseTypeSelectList = GetDropDownListByMasterCode(Enums.RefMasterCodes.INTEREST_BASE_TYPE);
            ViewBag.BaseTypes = baseTypeSelectList;

            SelectList calMethodSelectList = GetDropDownListByMasterCode(Enums.RefMasterCodes.INTEREST_CALC_TYPE);
            ViewBag.CalcTypes = calMethodSelectList;
        }

        [HttpPost]
        public ActionResult CreateLoanInterest(FormCollection form)
        {
            var interestMasterDto = ReadLoanInterestFormData(form);
            var resultDto = new ResultDto();

            if (interestMasterDto.InterestID == 0)
                resultDto = _interestService.Insert(interestMasterDto, Enums.InterestTypes.L);
            else
                resultDto = _interestService.Update(interestMasterDto, Enums.InterestTypes.L);

            if (resultDto.ObjectId > 0)
            {
                interestMasterDto = _interestService.GetByID(resultDto.ObjectId);

                resultDto.ObjectCode = interestMasterDto.InterestCode;
            }

            LoadLoanInterestDropDowns();

            ViewBag.Result = resultDto;

            return View(interestMasterDto);
        }

        private InterestMasterDto ReadLoanInterestFormData(FormCollection form)
        {
            InterestMasterDto interestMasterDto = new InterestMasterDto();
            int interestId = default(int);
            int.TryParse(form["InterestID"], out interestId);

            int? penalAhId = null;
            //int.TryParse(form["PenalAHID"], out penalAhId);


            interestMasterDto.InterestID = interestId;
            interestMasterDto.InterestCode = Convert.ToString(form["InterestCode"]);
            interestMasterDto.InterestName = Convert.ToString(form["InterestName"]);
            interestMasterDto.Base = Convert.ToInt32(form["Base"]);
            interestMasterDto.CaluculationMethod = Convert.ToInt32(form["CaluculationMethod"]);
            if (form["PrincipalAHID"] != "")
            {
                interestMasterDto.PrincipalAHID = Convert.ToInt32(form["PrincipalAHID"]);
            }
            interestMasterDto.InterestAHID = Convert.ToInt32(form["InterestAHID"]);
            
            interestMasterDto.PenalAHID =penalAhId;
          
            interestMasterDto.UserId = UserInfo.UserID;

            int maxIndex = Convert.ToInt32(form["hdnMaxRateIndex"]);

            interestMasterDto.InterestRates = new List<InterestRatesDto>();
            InterestRatesDto rate = null;
            for (int i = 1; i <= maxIndex; i++)
            {
                if (form["hdnROI_" + i] == null) continue;

                DateTime dtFromDate = form["hdnFDate_" + i].ConvertToDateTime();
                DateTime dtToDate = form["hdnToDate_" + i].ConvertToDateTime();

                rate = new InterestRatesDto();
                rate.ROI = Convert.ToDecimal(form["hdnROI_" + i]);
                rate.PenalROI = Convert.ToDecimal(form["hdnPROI_" + i]);
                rate.FromDate = dtFromDate;
                rate.ToDate = dtToDate;
                rate.IntrestRateID = Convert.ToInt32(form["hdnRateId_" + i]);

                interestMasterDto.InterestRates.Add(rate);
            }

            return interestMasterDto;
        }

        [HttpGet]
        public ActionResult ActiveInactiveLoanInterest(string Id)
        {
            int interestID = DecryptQueryString(Id);

            if (interestID < 1)
                return RedirectToAction("LoanInterestLookup");

            ResultDto resultDto = _interestService.ChangeStatus(interestID, UserInfo.UserID);

            TempData["Result"] = resultDto;

            return RedirectToAction("LoanInterestLookup");
        }
        
       
        #endregion
    }
}
