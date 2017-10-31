using AutoMapper;
using BusinessEntities;
using BusinessLogic;
using DataLogic.Implementation;
//using BusinessLogic.Interface;
using MFIS.Web.Areas.Federation.Models;
using MFIS.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utilities;

namespace MFIS.Web.Areas.Federation.Controllers
{
    public class OrganizationController : BaseController
    {

        #region Global Variables
        private readonly OrganizationService _organizationService;
        private readonly BankService _bankService;
        private readonly VillageService _villageService;
        private readonly ClusterService _clusterService;
        private readonly InterestService _interestService;
        private readonly PanchayatService _panchayatService;
        private readonly CommonService _commonService;

        public OrganizationController()
        {
            _organizationService = new OrganizationService();
            _bankService = new BankService();
            _villageService = new VillageService();
            _clusterService = new ClusterService();
            _interestService = new InterestService();
            _panchayatService = new PanchayatService();
            _commonService = new CommonService();

        }
        #endregion Global Variables

        #region CreateOrganization
        [HttpGet]
        public ActionResult CreateOrganization()
        {
            var organizationDto = new OrganizationDto();
            var organizationModel = new OrganizationModel();

            organizationDto = _organizationService.GetAll();

            SelectList banks = GetDropDownListByMasterCode(Enums.RefMasterCodes.BANK_NAME);
            SelectList accountType = GetDropDownListByMasterCode(Enums.RefMasterCodes.BANK_ACCOUNT_TYPE);
            BankMasterDal bankService = new BankMasterDal();
            List<BankMasterLookupDto> lstBankMasterLookupDto = bankService.GetLookup(null);
            lstBankMasterLookupDto = lstBankMasterLookupDto.FindAll(l => !l.IsBankAlreadyConsumed);
            OrganizationService org = new OrganizationService();
            List<BankMasterDto> lstobj = org.OrganizationGetAllBanksDetails();
            ViewBag.Banks = lstobj;

            //if (Id > 0) 
            //{
            //    List<BankMasterDto> lstModel = _bankService.BanksGetByObjectId(Id);
            //    ViewBag.BankDetails = lstModel;
            //}
            ViewBag.ExistedBanks = lstBankMasterLookupDto;
            ViewBag.accountType = accountType;
            ViewBag.bankNames = banks;

            if (organizationDto != null && organizationDto.OrgID > 0)
            {
                organizationModel = Mapper.Map<OrganizationDto, OrganizationModel>(organizationDto);
            }

            ViewBag.Result = new ResultDto();
            return View(organizationModel);
        }
        #endregion

        #region CreateOrganization
        [HttpPost]
        public ActionResult CreateOrganization(OrganizationModel organizationModel, FormCollection form)
        {
            var resultDto = new ResultDto();

            SelectList banks = GetDropDownListByMasterCode(Enums.RefMasterCodes.BANK_NAME);
            SelectList accountType = GetDropDownListByMasterCode(Enums.RefMasterCodes.BANK_ACCOUNT_TYPE);
            BankMasterDal bankservice = new BankMasterDal();
            List<BankMasterLookupDto> lstBankMasterLookupDto = bankservice.GetLookup(null);
            ViewBag.ExistedBanks = lstBankMasterLookupDto;
            ViewBag.accountType = accountType;
            ViewBag.bankNames = banks;

            var organizationDto = Mapper.Map<OrganizationModel, OrganizationDto>(organizationModel);
            organizationDto.UserID = UserInfo.UserID;

            if (organizationDto.OrgID == 0)
                resultDto = _organizationService.Insert(organizationDto);
            else
                resultDto = _organizationService.Update(organizationDto);

            if (resultDto.ObjectId > 0)
            {
                organizationDto = _organizationService.GetAll();
                organizationModel = Mapper.Map<OrganizationDto, OrganizationModel>(organizationDto);
                resultDto.ObjectCode = organizationDto.OrgCode;

                List<BankMasterDto> lstBanks = GetBanksList(form);
                int OrgID = resultDto.ObjectId;
                resultDto = _commonService.InsertBankDetails(OrgID, Enums.EntityCodes.ORG, UserInfo.UserID, lstBanks);
                resultDto.Message = resultDto.ObjectId > 0 ? "Organization bank details saved successfully for orgnization code :" + organizationModel.OrgCode : resultDto.Message;

            }
            
            ViewBag.Result = resultDto;
            ModelState.Clear();
            return View(organizationModel);
        }
        #endregion

    }
}
