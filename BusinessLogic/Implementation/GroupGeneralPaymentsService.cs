using AutoMapper;
using BusinessEntities;
using BusinessLogic.AutoMapper;
using CoreComponents;
using DataLogic.Implementation;
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
    public class GroupGeneralPaymentsService
    {
        #region Global Variables
        private readonly MFISDBContext _dbContext;
        private readonly CommonService _commonService;

        public GroupGeneralPaymentsService()
        {
            _dbContext = new MFISDBContext();
            AutoMapperEntityConfiguration.Configure();
            _commonService = new CommonService();
        }
        #endregion Global Variables
        public ResultDto Insert(GeneralPaymentsDto generalpayments)
        {
            return insertUpdateGeneralPayments(generalpayments);
        }
        public ResultDto Update(GeneralPaymentsDto generalpayments)
        {
            return insertUpdateGeneralPayments(generalpayments);
        }
        private ResultDto insertUpdateGeneralPayments(GeneralPaymentsDto generalpayments)
        {
            ResultDto resultDto = new ResultDto();
            generalpayments.IsGroup = Convert.ToBoolean(1);

            string objectName = "Group General Payments";
            try
            {
                string amountxml = CommonMethods.SerializeListDto<List<AddAmountDto>>(generalpayments.Addamount);
                ObjectParameter paramAccountMasterID = new ObjectParameter("AccountMasterID", generalpayments.AccountMasterID);
                ObjectParameter paramVoucherNumber = new ObjectParameter("VoucherNumber", string.Empty);


                _dbContext.uspAccountMasterInsertUpdate(paramAccountMasterID, generalpayments.TransactionDate, paramVoucherNumber, generalpayments.VoucherRefNumber, generalpayments.PartyName, generalpayments.EmployeeID, generalpayments.AHID,
                   generalpayments.SubHeadID, generalpayments.TransactionType, generalpayments.Amount, generalpayments.TransactionMode, generalpayments.ChequeNumber, generalpayments.ChequeDate,
                   generalpayments.BankAccount, generalpayments.Narration, generalpayments.IsGroup, generalpayments.GroupID, generalpayments.UserID, generalpayments.Type, generalpayments.IsMaster, amountxml);
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


        public List<GeneralPaymentsLookupDto> Lookup(int groupID, int userId)
        {
            List<GeneralPaymentsLookupDto> lstGeneralPaymentsLookupDto = new List<GeneralPaymentsLookupDto>();
            GroupGeneralPaymentDal gropuGeneralPmtDal = new GroupGeneralPaymentDal();
            lstGeneralPaymentsLookupDto = gropuGeneralPmtDal.GroupGeneralPaymentLookUp(groupID, userId);
            return lstGeneralPaymentsLookupDto;
        }

        public ResultDto Delete(int AccountMasterId, int userId)
        {
            ResultDto resultDto = new ResultDto();
            string obectName = "GroupGeneralPayments";

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
            string obectName = "GroupGeneralPayments";
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


        public GeneralPaymentsDto GetByID(long AccountMasterId)
        {

            long accountmasterID = Convert.ToInt64(AccountMasterId);
            ObjectParameter prmAccountMasterId = new ObjectParameter("AccountMasterId", accountmasterID);

            var results = new MFISDBContext()
                .MultipleResults(CustomProcNames.uspGeneralPaymentsGetById, prmAccountMasterId)
                .With<GeneralPaymentsDto>()
                .With<AddAmountDto>()
                .Execute();
            GeneralPaymentsDto generalPaymentsDto = new GeneralPaymentsDto();
            if ((results[0] as List<GeneralPaymentsDto>).Count > 0)
            {
                generalPaymentsDto = (results[0] as List<GeneralPaymentsDto>)[0];
                var addAmountDtoList = results[1] as List<AddAmountDto>;
                generalPaymentsDto.Addamount = addAmountDtoList;
            }
            return generalPaymentsDto;
        }

        public GroupGeneralPaymentDto GetGroupGeneralPaymentById(long accountMasterId)
        {
            GroupGeneralPaymentDal objDal = new GroupGeneralPaymentDal();
            return objDal.GetGroupGeneralPaymentById(accountMasterId);
        }

        public ResultDto AddUpdateGeneralPayment(GroupGeneralPaymentDto _groupGeneralPaymentDto, int userId, int groupId)
        {
            GroupGeneralPaymentDal objDal = new GroupGeneralPaymentDal();
            string generalPaymentTranxml = CommonMethods.SerializeListDto<List<GroupGeneralPaymentTranDto>>(_groupGeneralPaymentDto.TransactionsList);

            return objDal.AddUpdateGeneralPayment(_groupGeneralPaymentDto, generalPaymentTranxml, userId, groupId);
        }
    }

    public class GroupMemberPaymentService
    {
        #region Global Variables
        private readonly CommonService _commonService;

        public GroupMemberPaymentService()
        {
            _commonService = new CommonService();
        }
        #endregion Global Variables

        public GroupMemberPaymentDto GetGroupMemberPaymentById(long accountMasterId)
        {
            return objMemberPaymentDal.GetGroupMemberPaymentById(accountMasterId);
        }

        public ResultDto AddUpdateMemberPayment(GroupMemberPaymentDto _groupMemberPaymentDto, int userId, int groupId)
        {
            string memberPaymentTranxml = CommonMethods.SerializeListDto<List<GroupMemberPaymentTranDto>>(_groupMemberPaymentDto.TransactionsList);

            return objMemberPaymentDal.AddUpdateGroupMemberPayment(_groupMemberPaymentDto, memberPaymentTranxml, userId, groupId);
        }

        public List<GroupMemberPaymentLookupDto> GetGroupMemberPaymentLookup(int groupId, int userId)
        {
            return objMemberPaymentDal.GetGroupMemberPaymentLookup(groupId, userId);
        }

        private GroupMemberPaymentDal __objMemberPaymentDal;
        private GroupMemberPaymentDal objMemberPaymentDal
        {
            get
            {
                if (__objMemberPaymentDal == null)
                    __objMemberPaymentDal = new GroupMemberPaymentDal();
                return __objMemberPaymentDal;
            }
        }
    }
}
