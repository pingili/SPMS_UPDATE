using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BusinessEntities;
using BusinessLogic.AutoMapper;
//using BusinessLogic.Interface;
using CoreComponents;
using MFIEntityFrameWork;
using Utilities;

namespace BusinessLogic
{
    public class PaymentsToFederationService 
    {
        #region Global Variables
        private readonly MFISDBContext _dbContext;
        private readonly CommonService _commonService;

        public PaymentsToFederationService()
        {
            _dbContext = new MFISDBContext();
            AutoMapperEntityConfiguration.Configure();
            _commonService = new CommonService();
        }
        #endregion Global Variables
        public ResultDto Insert(GeneralReceiptDto generalpayments)
        {
            return insertUpdaterefundsFromFederation(generalpayments);
        }
        public ResultDto Update(GeneralReceiptDto generalpayments)
        {
            return insertUpdaterefundsFromFederation(generalpayments);
        }
        private ResultDto insertUpdaterefundsFromFederation(GeneralReceiptDto generalpayments)
        {
            ResultDto resultDto = new ResultDto();
            generalpayments.IsGroup = true;
            generalpayments.IsPairedRecord = true;
            string objectName = "Refunds From Federation";
            try
            {
                string amountxml = CommonMethods.SerializeListDto<List<AddAmountDto>>(generalpayments.Addamount);
                ObjectParameter paramAccountMasterID = new ObjectParameter("AccountMasterID", generalpayments.AccountMasterID);
                ObjectParameter paramVoucherNumber = new ObjectParameter("VoucherNumber", string.Empty);


                _dbContext.uspAccountMasterRefundsFromFederationInsert(paramAccountMasterID, generalpayments.TransactionDate, paramVoucherNumber, generalpayments.VoucherRefNumber, generalpayments.PartyName, generalpayments.EmployeeID, generalpayments.AHID,
                   generalpayments.SubHeadID, generalpayments.TransactionType, generalpayments.Amount, generalpayments.TransactionMode, generalpayments.ChequeNumber, generalpayments.ChequeDate,
                   generalpayments.BankAccount, generalpayments.Narration, generalpayments.IsGroup, generalpayments.GroupID, generalpayments.UserID, generalpayments.Type, generalpayments.IsPairedRecord, amountxml);
                long masterObjectId = Convert.ToInt64(paramAccountMasterID.Value);
                resultDto.ObjectId = Convert.ToInt32(masterObjectId);
                resultDto.ObjectCode = string.IsNullOrEmpty((string)paramVoucherNumber.Value) ? generalpayments.VoucherNumber : (string)paramVoucherNumber.Value;

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


        public List<GeneralPaymentsLookupDto> Lookup()
        {
            List<GeneralPaymentsLookupDto> lstGeneralPaymentsLookupDto = new List<GeneralPaymentsLookupDto>();
            List<uspFederationGeneralPaymentsLookup_Result> lstuspGeneralPaymentsLookup_Result = _dbContext.uspFederationGeneralPaymentsLookup().ToList();
            foreach (var GeneralPayments in lstuspGeneralPaymentsLookup_Result)
            {
                GeneralPaymentsLookupDto GeneralpaymentsDto = Mapper.Map<uspFederationGeneralPaymentsLookup_Result, GeneralPaymentsLookupDto>(GeneralPayments);
                lstGeneralPaymentsLookupDto.Add(GeneralpaymentsDto);
            }
            return lstGeneralPaymentsLookupDto;
        }


        public ResultDto Delete(int AccountMasterId, int userId)
        {
            ResultDto resultDto = new ResultDto();
            string obectName = "GeneralPayments";

            try
            {
                ObjectParameter prmAccountMasterId = new ObjectParameter("AccountMasterId", AccountMasterId);
                ObjectParameter prmVoucherNumber = new ObjectParameter("VoucherNumber", string.Empty);

                int effectedCount = _dbContext.uspGeneralPaymentsDelete(prmAccountMasterId, prmVoucherNumber, userId);

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

        public ResultDto ChangeStatus(int AccountMasterId, int userId)
        {
            ResultDto resultDto = new ResultDto();
            string obectName = "GeneralPayments";
            string statusCode = string.Empty;
            try
            {
                ObjectParameter prmAccountMasterId = new ObjectParameter("AccountMasterId", AccountMasterId);
                ObjectParameter prmVoucherNumber = new ObjectParameter("VoucherNumber", string.Empty);
                ObjectParameter prmStatusCode = new ObjectParameter("StatusCode", string.Empty);

                int effectedCount = _dbContext.uspGeneralPaymentsChangeStatus(prmAccountMasterId, prmVoucherNumber, prmStatusCode, userId);

                resultDto.ObjectId = (int)prmAccountMasterId.Value;
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

        public List<PaymentsToFederationLookUpDto> PaymentsToFederationLookup(int GroupID)
        {
            List<PaymentsToFederationLookUpDto> lstRefundsFromFederationLookUpDto = new List<PaymentsToFederationLookUpDto>();
            List<uspRefundsFromFederationLookup_Result> lstuspRefundsFromFederationLookup_Result = _dbContext.uspRefundsFromFederationLookup(GroupID).ToList();
            foreach (var RefundsFromfederation in lstuspRefundsFromFederationLookup_Result)
            {
                PaymentsToFederationLookUpDto refundsFromFederationLookUpDto = Mapper.Map<uspRefundsFromFederationLookup_Result, PaymentsToFederationLookUpDto>(RefundsFromfederation);
                lstRefundsFromFederationLookUpDto.Add(refundsFromFederationLookUpDto);
            }
            return lstRefundsFromFederationLookUpDto;
        }
        public List<ReceiptTranscationDto> GetAccountdetails(int GroupId)
        {

            var lstRapDetails = _dbContext.uspGetAHNamesByGroupID(GroupId).ToList().FindAll(f => f.IsFederation == false);
            var listOfSearchedIds = new List<string> { "2020", "2114", "1231", "1232", "1241", "1242", "1243" };
            lstRapDetails = (lstRapDetails.Where(x => listOfSearchedIds.Contains(x.AHCODE))).ToList();
            // lstRapDetails = (lstRapDetails.where(x=>AHCode == "2020" && x.AHCode == "2114" && x.AHCode == "1231" && x.AHCode == "1232" && x.AHCode == "1241" && x.AHCode == "1242" && x.AHCode == "1243")).ToList();
            List<ReceiptTranscationDto> lstGroupReceiptDto = new List<ReceiptTranscationDto>();
            foreach (var Ac in lstRapDetails)
            {
                lstGroupReceiptDto.Add(new ReceiptTranscationDto()
                {
                    AHID = Ac.AHID,
                    AHCode = Ac.AHCODE,
                    AHName = Ac.AHName,
                    OpeningBalance = Convert.ToDecimal(Ac.OpeningBalance),
                    ClosingBalance = Ac.CloseingBalance
                    // CrAmount = Convert.ToDecimal(Ac.CrAmount)
                });
            }
            return lstGroupReceiptDto;
        }
        public GeneralReceiptDto GetByID(long AccountMasterId)
        {


            long accountmasterID = Convert.ToInt64(AccountMasterId);
            ObjectParameter prmAccountMasterId = new ObjectParameter("AccountMasterId", accountmasterID);

            var results = new MFISDBContext()
                .MultipleResults("uspRefundsFromfederationGetById", prmAccountMasterId)
                .With<GeneralReceiptDto>()
                .With<AddAmountDto>()
                .Execute();


            var generalPaymentsDto = (results[0] as List<GeneralReceiptDto>)[0];
            var addAmountDtoList = results[1] as List<AddAmountDto>;

            generalPaymentsDto.Addamount = addAmountDtoList;

            return generalPaymentsDto;
        }
    }
}
