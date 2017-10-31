using AutoMapper;
using BusinessEntities;
using BusinessLogic.AutoMapper;
//using BusinessLogic.Interface;
using CoreComponents;
using MFIEntityFrameWork;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace BusinessLogic
{
    public class GroupReceiptService 
    {
        #region Global Variables
        private readonly MFISDBContext _dbContext;

        public GroupReceiptService()
        {
            _dbContext = new MFISDBContext();
            AutoMapperEntityConfiguration.Configure();
        }
        #endregion
        public List<BankMasterViewDto> GetGroupBanks()
        {
            List<uspgroupGetAllBanksDetails_Result> lstuspGroupBankDetails_Result = _dbContext.uspgroupGetAllBanksDetails().ToList();


            List<BankMasterViewDto> lstBankMasterViewDto = new List<BankMasterViewDto>();
            foreach (var result in lstuspGroupBankDetails_Result)
                lstBankMasterViewDto.Add(Mapper.Map<uspgroupGetAllBanksDetails_Result, BankMasterViewDto>(result));

            return lstBankMasterViewDto;
        }
        public List<ReceiptTranscationDto> GetAccountdetails()
        {


            var lstRapDetails = _dbContext.uspGroupReceipt().ToList();

            List<ReceiptTranscationDto> lstGroupReceiptDto = new List<ReceiptTranscationDto>();
            foreach (var Ac in lstRapDetails)
            {
                lstGroupReceiptDto.Add(new ReceiptTranscationDto()
                {
                    AHID = Ac.AHID,
                    AHCode = Ac.AHCode,
                    AHName = Ac.AHName,
                    OpeningBalance = Convert.ToDecimal(Ac.OpeningBalance),  // CommonMethods.ToDisplayCurrency(Ac.OpeningBalance),
                    // CrAmount = Convert.ToDecimal(Ac.CrAmount)
                });
            }
            return lstGroupReceiptDto;
        }
        public List<BankMasterViewDto> GetFederationBanks()
        {
            List<uspOrganizationGetAllBanksDetails_Result> lstuspOrganizationGetAllBanksDetails_Result = _dbContext.uspOrganizationGetAllBanksDetails().ToList();


            List<BankMasterViewDto> lstBankMasterViewDto = new List<BankMasterViewDto>();
            foreach (var result in lstuspOrganizationGetAllBanksDetails_Result)
                lstBankMasterViewDto.Add(Mapper.Map<uspOrganizationGetAllBanksDetails_Result, BankMasterViewDto>(result));

            return lstBankMasterViewDto;
        }
        public ResultDto Insert(ReceiptMasterDto groupReceiptdto)
        {
            return InsertUpdateGroupReceipt(groupReceiptdto);
        }
        public ResultDto Update(ReceiptMasterDto groupReceiptdto)
        {
            return InsertUpdateGroupReceipt(groupReceiptdto);
        }
        private ResultDto InsertUpdateGroupReceipt(ReceiptMasterDto groupReceiptdto)
        {
            ResultDto resultDto = new ResultDto();
            string objectName = "GroupReceipt";
            try
            {
                string receiptxml = CommonMethods.SerializeListDto<List<ReceiptTranscationDto>>(groupReceiptdto.lstGroupReceiptTranscationDto);
                int id = 1;
                ObjectParameter paramaccounmasterID = new ObjectParameter("AccountMasterID", groupReceiptdto.AccountMasterID);
                ObjectParameter paramreceiptCode = new ObjectParameter("VoucherNumber", string.Empty);

                _dbContext.uspGroupReceiptInsertUpdate(paramaccounmasterID, groupReceiptdto.TransactionDate, paramreceiptCode, groupReceiptdto.VoucherRefNumber, groupReceiptdto.CodeSno, groupReceiptdto.PartyName, groupReceiptdto.EmployeeID,
                     groupReceiptdto.AHID, groupReceiptdto.SubHeadID, groupReceiptdto.TransactionType, groupReceiptdto.Amount, groupReceiptdto.TransactionMode,
                    groupReceiptdto.ChequeNumber, groupReceiptdto.ChequeDate, groupReceiptdto.BankAccount, groupReceiptdto.Narration, receiptxml, groupReceiptdto.IsGroup, groupReceiptdto.GroupID, groupReceiptdto.UserID);
                resultDto.ObjectId = Convert.ToInt32(paramaccounmasterID.Value);

                resultDto.ObjectCode = string.IsNullOrEmpty((string)paramreceiptCode.Value) ? groupReceiptdto.VoucherNumber : (string)paramreceiptCode.Value;

                if (resultDto.ObjectId > 0)
                    resultDto.Message = string.Format("{0} details saved successfully with code : {1}", objectName, resultDto.ObjectCode);
                else if (resultDto.ObjectId == -1)
                    resultDto.Message = string.Format("Error occured while generating {0} code", objectName);
                else
                    resultDto.Message = string.Format("Error occured while saving {0} details", objectName);
            }
            catch (Exception)
            {
                resultDto.Message = string.Format("Service layer error occured while saving the {0} details", objectName);
                resultDto.ObjectId = -98;

            }
            return resultDto;
        }
        public List<GropReceiptLookupDto> GroupReceiptLookup()
        {

            List<GropReceiptLookupDto> lstGropReceiptLookupDto = new List<GropReceiptLookupDto>();
            List<uspGroupReceiptLookup_Result> lstuspGroupReceiptLookup_Result = _dbContext.uspGroupReceiptLookup().ToList();
            foreach (var Groupreceipts in lstuspGroupReceiptLookup_Result)
            {
                GropReceiptLookupDto GropReceiptLookupDto = Mapper.Map<uspGroupReceiptLookup_Result, GropReceiptLookupDto>(Groupreceipts);
                lstGropReceiptLookupDto.Add(GropReceiptLookupDto);
            }
            return lstGropReceiptLookupDto;
        }
        public ResultDto Delete(int AccountmasterId, int CurrentUserID)
        {
            ResultDto resultDto = new ResultDto();
            string obectName = "Groupreceipts";

            try
            {
                ObjectParameter prmAccountMasterId = new ObjectParameter("AccountMasterId", AccountmasterId);
                ObjectParameter prmVoucherNumber = new ObjectParameter("VoucherNumber", string.Empty);

                int effectedCount = _dbContext.uspGroupReceiptDelete(prmAccountMasterId, prmVoucherNumber, CurrentUserID);

                resultDto.ObjectId = (int)prmAccountMasterId.Value;
                resultDto.ObjectCode = (string)prmVoucherNumber.Value;

                if (resultDto.ObjectId > 0)
                    resultDto.Message = string.Format("{0} : {1} details deleted successfully", obectName, resultDto.ObjectCode);
                else
                    resultDto.Message = string.Format("Error occured while deleting {0} details", obectName);
            }
            catch (Exception)
            {
                resultDto.Message = string.Format("Service layer error occured while deleting the {0} details", obectName);
                resultDto.ObjectId = -98;

            }
            return resultDto;
        }
        public ResultDto ChangeStatus(int AccountmasterId, int CurrentUserID)
        {
            ResultDto resultDto = new ResultDto();
            string obectName = "Groupreceipts";
            string statusCode = string.Empty;
            try
            {
                ObjectParameter prmAccountMasterId = new ObjectParameter("AccountMasterId", AccountmasterId);
                ObjectParameter prmVoucherNumber = new ObjectParameter("VoucherNumber", string.Empty);
                ObjectParameter prmStatusCode = new ObjectParameter("StatusCode", string.Empty);

                int effectedCount = _dbContext.uspGroupReceiptChangeStatus(prmAccountMasterId, prmVoucherNumber, prmStatusCode, AccountmasterId);

                resultDto.ObjectId = (int)AccountmasterId;
                resultDto.ObjectCode = (string)prmVoucherNumber.Value;
                statusCode = (string)prmStatusCode.Value;

                if (resultDto.ObjectId > 0)
                    resultDto.Message = string.Format("{0} : {1} details {2} successfully", obectName, resultDto.ObjectCode, statusCode == Constants.StatusCodes.Active ? "activated" : "inactivated");
                else
                    resultDto.Message = string.Format("Error occured while {0} {1} details", statusCode == Constants.StatusCodes.Active ? "activated" : "inactivated", obectName);
            }
            catch (Exception)
            {
                resultDto.Message = string.Format("Service layer error occured while {0} {1} details", statusCode == Constants.StatusCodes.Active ? "activated" : "inactivated", obectName);
                resultDto.ObjectId = -98;

            }
            return resultDto;
        }

        public ReceiptMasterDto GetByID(int AccountMasterID)
        {
            var prmAccountMasterID = new ObjectParameter("AccountMasterID", AccountMasterID);

            var results = new MFISDBContext()
                .MultipleResults(CustomProcNames.uspgroupReceiptByID, prmAccountMasterID)
                .With<ReceiptMasterDto>()
                .With<ReceiptTranscationDto>()
                .Execute();
            var receiptMasterDto = new ReceiptMasterDto();
            if ((results[0] as List<ReceiptMasterDto>).Count > 0)
            {
                receiptMasterDto = (results[0] as List<ReceiptMasterDto>)[0];
                var receiptTranscationDtoList = results[1] as List<ReceiptTranscationDto>;

                receiptMasterDto.lstGroupReceiptTranscationDto = receiptTranscationDtoList;
            }
            return receiptMasterDto;
        }

        public ReceiptTranscationDto GetAccountHeadClosingBalnces()
        {
            return Mapper.Map<uspGetAccountHeadClosingBalnces_Result, ReceiptTranscationDto>(_dbContext.uspGetAccountHeadClosingBalnces().ToList().FirstOrDefault());
        }
    }
}
