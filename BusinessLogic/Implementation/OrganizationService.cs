using BusinessEntities;
using BusinessLogic.AutoMapper;
//using BusinessLogic.Interface;
using MFIEntityFrameWork;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;

namespace BusinessLogic
{
    public class OrganizationService 
    {

        #region Global Variables
        private readonly MFISDBContext _dbContext;

        public OrganizationService()
        {
            _dbContext = new MFISDBContext();
            AutoMapperEntityConfiguration.Configure();
        }
        #endregion


        #region GetAll
        public OrganizationDto GetAll()
        {
            var objuspOrganizationGetAllResult = _dbContext.uspOrganizationGetAll(null).ToList().FirstOrDefault();

            OrganizationDto objOrganizationDto = new OrganizationDto();
            if (objuspOrganizationGetAllResult != null)
                objOrganizationDto = AutoMapperEntityConfiguration.Cast<OrganizationDto>(objuspOrganizationGetAllResult);

            return objOrganizationDto;
        }
        #endregion

        #region Insert
        public ResultDto Insert(OrganizationDto organizationDto)
        {
            return InsertUpdateOrganization(organizationDto);
        }
        #endregion

        #region Update
        public ResultDto Update(OrganizationDto organizationDto)
        {
            return InsertUpdateOrganization(organizationDto);
        }
        #endregion

        #region InsertUpdateOrganization
        private ResultDto InsertUpdateOrganization(OrganizationDto organizationDto)
        {
            ResultDto resultDto = new ResultDto();
            string obectName = "organization";

            try
            {
                ObjectParameter paramOrgID = new ObjectParameter("OrgID", organizationDto.OrgID);
                ObjectParameter paramOrgCode = new ObjectParameter("OrgCode", string.Empty);

                //   DateTime dtFromDate = ["organizationDto.RegistrationDate"].ConvertToDateTime();
                // DateTime dt =organizationDto.RegistrationDate;
                //  String.Format("{0:dd-MM-yyyy}", dt);
                //  DateTime dt = DateTime.ParseExact("organizationDto.RegistrationDate", "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);

                int effectcount = _dbContext.uspOrganizationInsertUpdate(paramOrgID, paramOrgCode, organizationDto.OrgName, organizationDto.TEOrgName, organizationDto.RegistrationNumber,
                   organizationDto.RegistrationDate, organizationDto.Address, organizationDto.PAN, organizationDto.TAN, organizationDto.VAT, organizationDto.TIN,
                   (byte)organizationDto.MemberRetirementAge, (byte)organizationDto.EmpRetirementAge, organizationDto.GroupsIndividuals,
                   organizationDto.MemCompGender.ToString(), organizationDto.UserID, organizationDto.FinancialYearStartDate, organizationDto.FinancialYearEndDate);

                resultDto.ObjectId = (int)paramOrgID.Value;
                resultDto.ObjectCode = string.IsNullOrEmpty((string)paramOrgCode.Value) ? organizationDto.OrgCode : (string)paramOrgCode.Value;

                if (resultDto.ObjectId > 0)
                    resultDto.Message = string.Format("{0} details saved successfully with code : {1}", obectName, resultDto.ObjectCode);
                else if (resultDto.ObjectId == -1)
                    resultDto.Message = string.Format("Error occured while generating {0} code", obectName);
                else
                    resultDto.Message = string.Format("Error occured while saving {0} details", obectName);
            }
            catch (Exception)
            {
                resultDto.Message = string.Format("Service layer error occured while saving the {0} details", obectName);
                resultDto.ObjectId = -98;
            }
            return resultDto;
        }
        #endregion

        #region GetOrganizationSelectList
        public List<SelectListDto> GetOrganizationSelectList()
        {
            List<SelectListDto> lstSelectListDto = new List<SelectListDto>();
            List<uspOrganizationGetAll_Result> lstuspOrganizationGetAll_Result = _dbContext.uspOrganizationGetAll(null).ToList();

            foreach (var organization in lstuspOrganizationGetAll_Result)
            {
                SelectListDto objSelectListDto = new SelectListDto()
                {
                    ID = organization.OrgID,
                    Text = organization.OrgName.ToString()
                };

                lstSelectListDto.Add(objSelectListDto);
            }
            return lstSelectListDto;
        }
        #endregion
        #region GetOrganizationSelectList
        public List<BankMasterDto> OrganizationGetAllBanksDetails()
        {
            List<BankMasterDto> lstBankList = new List<BankMasterDto>();
            List<uspOrganizationGetAllBanksDetails_Result> lstuspOrganizationGetAll_Result = _dbContext.uspOrganizationGetAllBanksDetails().ToList();

            foreach (var bank in lstuspOrganizationGetAll_Result)
            {
                BankMasterDto obj = new BankMasterDto()
                {
                   BankEntryID = bank.BankEntryID,

                   EntityId = bank.EntityID,
                   
                   
                   BName = bank.BankName,
                   BranchName = bank.BranchName,
                   IFSC = bank.IFSC,
                   AccountType = bank.AccountType,
                   AccountTypeText = bank.AccountTypeText,
                   AccountNumber = bank.AccountNumber,
                   ObjectID = bank.ObjectID
                };
                lstBankList.Add(obj);
                
            }
            return lstBankList;
        }
        #endregion
    }
}
