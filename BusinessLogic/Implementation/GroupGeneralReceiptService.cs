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
  public  class GroupGeneralReceiptService 
    {
       #region Global Variables
        private readonly MFISDBContext _dbContext;
        private readonly CommonService _commonService;

        public GroupGeneralReceiptService()
        {
            _dbContext = new MFISDBContext();
            AutoMapperEntityConfiguration.Configure();
            _commonService = new CommonService();
        }
        #endregion Global Variables
        public ResultDto Insert(GeneralReceiptDto generalreceiptDto)
        {
            return insertupdateGeneralreceipt(generalreceiptDto);
        }

        public ResultDto Update(GeneralReceiptDto generalreceiptDto)
        {
            return insertupdateGeneralreceipt(generalreceiptDto);
        }
        private ResultDto insertupdateGeneralreceipt(GeneralReceiptDto generalreceiptDto)
        {
            ResultDto resultDto = new ResultDto();
            generalreceiptDto.IsGroup = Convert.ToBoolean(1);
            string objectName = "General Receipt";
            try
            {
                string amountxml = CommonMethods.SerializeListDto<List<AddAmountDto>>(generalreceiptDto.Addamount);
                ObjectParameter paramAccountMasterID = new ObjectParameter("AccountMasterID", generalreceiptDto.AccountMasterID);
                ObjectParameter paramVoucherNumber = new ObjectParameter("VoucherNumber", string.Empty);
                _dbContext.uspGeneralReceiptInsertUpdate(paramAccountMasterID, generalreceiptDto.TransactionDate, paramVoucherNumber, generalreceiptDto.VoucherRefNumber, generalreceiptDto.PartyName, generalreceiptDto.EmployeeID, generalreceiptDto.AHID,
                   generalreceiptDto.SubHeadID, generalreceiptDto.TransactionType, generalreceiptDto.Amount, generalreceiptDto.TransactionMode, generalreceiptDto.ChequeNumber, generalreceiptDto.ChequeDate,
                   generalreceiptDto.BankAccount, generalreceiptDto.Narration, generalreceiptDto.IsGroup, generalreceiptDto.GroupID, generalreceiptDto.UserID, generalreceiptDto.Type, amountxml);
                long masterObjectId = Convert.ToInt64(paramAccountMasterID.Value);
                resultDto.ObjectId = Convert.ToInt32(masterObjectId);
                resultDto.ObjectCode = string.IsNullOrEmpty((string)paramVoucherNumber.Value) ? generalreceiptDto.VoucherNumber : (string)paramVoucherNumber.Value;

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
        public List<GeneralReceiptLookupDto> Lookup()
        {
            List<GeneralReceiptLookupDto> lstGeneralReceiptLookupDto = new List<GeneralReceiptLookupDto>();
            List<uspGroupGeneralReceiptLookup_Result> lstuspGeneralPaymentsLookup_Result = _dbContext.uspGroupGeneralReceiptLookup().ToList();
            foreach (var GeneralReceipt in lstuspGeneralPaymentsLookup_Result)
            {
                GeneralReceiptLookupDto GeneralreceiptDto = Mapper.Map<uspGroupGeneralReceiptLookup_Result, GeneralReceiptLookupDto>(GeneralReceipt);
                lstGeneralReceiptLookupDto.Add(GeneralreceiptDto);
            }
            return lstGeneralReceiptLookupDto;
        }


        public ResultDto Delete(int AccountMasterId, int userId)
        {
            ResultDto resultDto = new ResultDto();
            string obectName = "GeneralPayments";

            try
            {
                ObjectParameter prmAccountMasterId = new ObjectParameter("AccountMasterId", AccountMasterId);
                ObjectParameter prmVoucherNumber = new ObjectParameter("VoucherNumber", string.Empty);

                int effectedCount = _dbContext.uspGeneralReceiptDelete(prmAccountMasterId, prmVoucherNumber, userId);

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

                int effectedCount = _dbContext.uspGeneralReceiptChangeStatus(prmAccountMasterId, prmVoucherNumber, prmStatusCode, userId);

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


        public GeneralReceiptDto GetByID(long AccountMasterId)
        {
            long accountmasterID = Convert.ToInt64(AccountMasterId);
            ObjectParameter prmAccountMasterId = new ObjectParameter("AccountMasterId", accountmasterID);

            var results = new MFISDBContext()
                .MultipleResults(CustomProcNames.uspGeneralReceiptGetById, prmAccountMasterId)
                .With<GeneralReceiptDto>()
                .With<AddAmountDto>()
                .Execute();
            GeneralReceiptDto generalReceiptDto = new GeneralReceiptDto();
            if ((results[0] as List<GeneralReceiptDto>).Count > 0)
            {
                generalReceiptDto = (results[0] as List<GeneralReceiptDto>)[0];
                var addAmountDtoList = results[1] as List<AddAmountDto>;
                generalReceiptDto.Addamount = addAmountDtoList;
            }
            return generalReceiptDto;
        }
    }
}
