using AutoMapper;
using BusinessEntities;
using BusinessLogic.AutoMapper;
//using BusinessLogic.Interface;
using CoreComponents;
using DataLogic;
using MFIEntityFrameWork;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using Utilities;

namespace BusinessLogic
{
    public class MemberReceiptService
    {
        #region Global Variables
        private readonly MFISDBContext _dbContext;

        public MemberReceiptService()
        {
            _dbContext = new MFISDBContext();
            AutoMapperEntityConfiguration.Configure();
        }
        #endregion

        public List<ReceiptTranscationDto> GetAccountdetails()
        {
            var lstRapDetails = _dbContext.uspMemberReceipt().ToList();

            List<ReceiptTranscationDto> lstGroupReceiptDto = new List<ReceiptTranscationDto>();
            foreach (var Ac in lstRapDetails)
            {
                lstGroupReceiptDto.Add(new ReceiptTranscationDto()
                {
                    AHID = Ac.AHID,
                    AHCode = Ac.AHCode,
                    AHName = Ac.AHName,
                    OpeningBalance = Convert.ToDecimal(Ac.OpeningBalance),
                    // CrAmount = Convert.ToDecimal(Ac.CrAmount)
                });
            }
            return lstGroupReceiptDto;
        }


        public ResultDto Insert(ReceiptMasterDto groupReceiptdto)
        {
            //return InsertUpdateMemberReceipt(groupReceiptdto);
            return new ReceiptDll().MemberReceiptInsertUpdate(groupReceiptdto);
        }

        public ResultDto Update(ReceiptMasterDto groupReceiptdto)
        {
            return InsertUpdateMemberReceipt(groupReceiptdto);
        }
        private ResultDto InsertUpdateMemberReceipt(ReceiptMasterDto groupReceiptdto)
        {
            ResultDto resultDto = new ResultDto();
            string objectName = "MemberReceipt";
            try
            {
                string receiptxml = CommonMethods.SerializeListDto<List<ReceiptTranscationDto>>(groupReceiptdto.lstGroupReceiptTranscationDto);

                ObjectParameter paramaccounmasterID = new ObjectParameter("AccountMasterID", groupReceiptdto.AccountMasterID);
                ObjectParameter paramreceiptCode = new ObjectParameter("VoucherNumber", string.Empty);

                _dbContext.uspMemberReceiptInsertUpdate(paramaccounmasterID, groupReceiptdto.TransactionDate, paramreceiptCode, groupReceiptdto.VoucherRefNumber, groupReceiptdto.CodeSno, groupReceiptdto.PartyName, groupReceiptdto.EmployeeID,
                    groupReceiptdto.AHID, groupReceiptdto.SubHeadID, groupReceiptdto.TransactionType, groupReceiptdto.Amount, groupReceiptdto.TransactionMode,
                    groupReceiptdto.ChequeNumber, groupReceiptdto.ChequeDate, groupReceiptdto.BankAccount, groupReceiptdto.Narration, receiptxml, groupReceiptdto.IsGroup, groupReceiptdto.GroupID, groupReceiptdto.MemberId, groupReceiptdto.UserID);

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


        public List<MemberReceiptLookupDto> MemberReceiptLookup(int GroupId)
        {
            List<MemberReceiptLookupDto> lstMemberReceiptLookupDto = new List<MemberReceiptLookupDto>();
             
            List<uspMemberReceiptLookup_Result> lstuspGroupReceiptLookup_Result = _dbContext.uspMemberReceiptLookup(GroupId).ToList();
            foreach (var MembeReceipts in lstuspGroupReceiptLookup_Result)
            {
                MemberReceiptLookupDto MemberReceiptLookupDto = Mapper.Map<uspMemberReceiptLookup_Result, MemberReceiptLookupDto>(MembeReceipts);
                lstMemberReceiptLookupDto.Add(MemberReceiptLookupDto);
            }
            return lstMemberReceiptLookupDto;
        }
        public ResultDto Delete(int AccountmasterId, int CurrentUserID)
        {
            ResultDto resultDto = new ResultDto();
            string obectName = "MemberReceipts";

            try
            {
                ObjectParameter prmAccountMasterId = new ObjectParameter("AccountMasterId", AccountmasterId);
                ObjectParameter prmVoucherNumber = new ObjectParameter("VoucherNumber", string.Empty);

                int effectedCount = _dbContext.uspMemerReceiptDelete(prmAccountMasterId, prmVoucherNumber, CurrentUserID);

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
            string obectName = "MemberReceipts";
            string statusCode = string.Empty;
            try
            {
                ObjectParameter prmAccountMasterId = new ObjectParameter("AccountMasterId", AccountmasterId);
                ObjectParameter prmVoucherNumber = new ObjectParameter("VoucherNumber", string.Empty);
                ObjectParameter prmStatusCode = new ObjectParameter("StatusCode", string.Empty);

                int effectedCount = _dbContext.uspMemberReceiptChangeStatus(prmAccountMasterId, prmVoucherNumber, prmStatusCode, AccountmasterId);

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
                .MultipleResults(MFIEntityFrameWork.CustomProcNames.uspMemberReceiptByID, prmAccountMasterID)
                .With<ReceiptMasterDto>()
                .With<ReceiptTranscationDto>()
                .Execute();

            var receiptMasterDto = (results[0] as List<ReceiptMasterDto>)[0];
            var receiptTranscationDtoList = results[1] as List<ReceiptTranscationDto>;

            receiptMasterDto.lstGroupReceiptTranscationDto = receiptTranscationDtoList;

            return receiptMasterDto;
        }
    }
}
