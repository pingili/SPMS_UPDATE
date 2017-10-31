using AutoMapper;
using BusinessEntities;
using BusinessLogic.AutoMapper;
using CoreComponents;
using MFIEntityFrameWork;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
//using BusinessLogic.Interface;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace BusinessLogic
{
    public class GroupJournalService1 
    {
        #region Global Variables
        private readonly MFISDBContext _dbContext;
        private readonly CommonService _commonService;

        public GroupJournalService1()
        {
            _dbContext = new MFISDBContext();
            _dbContext.Configuration.LazyLoadingEnabled = false;

            AutoMapperEntityConfiguration.Configure();
            _commonService = new CommonService();
        }
        #endregion Global Variables
        public ResultDto Insert(ReceiptMasterDto journalEntry)
        {
            return InsertUpdateJournalEntry(journalEntry);

        }
        public ResultDto Update(ReceiptMasterDto journalEntry)
        {
            return InsertUpdateJournalEntry(journalEntry);
        }
        public ResultDto InsertUpdateJournalEntry(ReceiptMasterDto journalEntry)
        {
            ResultDto resultDto = new ResultDto();
            string objectName = "Journal Entry Voucher";
            try
            {
                string amountxml = CommonMethods.SerializeListDto<List<ReceiptTranscationDto>>(journalEntry.lstGroupReceiptTranscationDto);
                ObjectParameter paramAccountMasterID = new ObjectParameter("AccountMasterID", journalEntry.AccountMasterID);
                ObjectParameter paramVoucherNumber = new ObjectParameter("VoucherNumber", string.Empty);//journalEntry.SLAcNo, journalEntry.SLAName,
                int effectedCount = _dbContext.uspGroupJournalEntryInsertUpdate(paramAccountMasterID,
                    journalEntry.TransactionDate,
                    paramVoucherNumber,
                    journalEntry.VoucherRefNumber,
                    journalEntry.EmployeeID,
                    journalEntry.AHID,
                    journalEntry.SubHeadID,
                    journalEntry.Amount,
                    //journalEntry.BankAccount,
                    journalEntry.Narration,
                    journalEntry.PartyName,
                    journalEntry.TransactionType,
                    journalEntry.TransactionMode,
                    journalEntry.IsGroup,
                    journalEntry.GroupID,
                    journalEntry.UserID,
                    amountxml);
                long masterObjectId = Convert.ToInt64(paramAccountMasterID.Value);
                resultDto.ObjectId = Convert.ToInt32(masterObjectId);
                resultDto.ObjectCode = string.IsNullOrEmpty((string)paramVoucherNumber.Value) ? journalEntry.VoucherNumber : (string)paramVoucherNumber.Value;
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
        public List<JournalLookupDto> Lookup()
        {

            List<JournalLookupDto> lstJournalEntryLookupDto = new List<JournalLookupDto>();
            List<uspGroupJournalEntryLookup_Result> lstuspGeneralPaymentsLookup_Result = _dbContext.uspGroupJournalEntryLookup().ToList();
            foreach (var JournalEntry in lstuspGeneralPaymentsLookup_Result)
            {
                JournalLookupDto JournalEntryDto = Mapper.Map<uspGroupJournalEntryLookup_Result, JournalLookupDto>(JournalEntry);
                lstJournalEntryLookupDto.Add(JournalEntryDto);
            }
            return lstJournalEntryLookupDto;
        }


        public ResultDto Delete(int AccountMasterId, int userId)
        {

            ResultDto resultDto = new ResultDto();
            string obectName = "GeneralPayments";

            try
            {
                ObjectParameter prmAccountMasterId = new ObjectParameter("AccountMasterId", AccountMasterId);
                ObjectParameter prmVoucherNumber = new ObjectParameter("VoucherNumber", string.Empty);

                int effectedCount = _dbContext.uspJournalEntryDelete(prmAccountMasterId, prmVoucherNumber, userId);

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

                int effectedCount = _dbContext.uspJournalEntryChangeStatus(prmAccountMasterId, prmVoucherNumber, prmStatusCode, userId);

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
        public ReceiptMasterDto GetByID(long AccountMasterId)
        {
            long accountmasterID = Convert.ToInt64(AccountMasterId);
            ObjectParameter prmAccountMasterId = new ObjectParameter("AccountMasterId", accountmasterID);

            var results = new MFISDBContext()
                .MultipleResults(CustomProcNames.uspJournalEntryVoucherGetById, prmAccountMasterId)
                .With<ReceiptMasterDto>()
                .With<ReceiptTranscationDto>()
                .Execute();

            var journalEntryDto = (results[0] as List<ReceiptMasterDto>)[0];
            var addAmountDtoList = results[1] as List<ReceiptTranscationDto>;

            journalEntryDto.lstGroupReceiptTranscationDto = addAmountDtoList;

            return journalEntryDto;
        }

    }
}
