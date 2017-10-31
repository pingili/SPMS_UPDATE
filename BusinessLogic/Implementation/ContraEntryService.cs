using AutoMapper;
using BusinessEntities;
using BusinessLogic.AutoMapper;
using CoreComponents;
using DataLogic;
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
    public class ContraEntryService 
    {

        #region Global Variables
        private readonly MFISDBContext _dbContext;

        public ContraEntryService()
        {
            _dbContext = new MFISDBContext();
            AutoMapperEntityConfiguration.Configure();
        }
        #endregion


        //-------------- Federation Contra Entry With Drawl--------------------------//


        #region GetAllOrganizationBanks

        public List<BusinessEntities.BankMasterViewDto> GetAllOrganizationBanks()
        {
            List<uspOrganizationGetAllBanksDetails_Result> lstuspOrganizationGetAllBanksDetails_Result = _dbContext.uspOrganizationGetAllBanksDetails().ToList();


            List<BankMasterViewDto> lstBankMasterViewDto = new List<BankMasterViewDto>();
            foreach (var result in lstuspOrganizationGetAllBanksDetails_Result)
                lstBankMasterViewDto.Add(Mapper.Map<uspOrganizationGetAllBanksDetails_Result, BankMasterViewDto>(result));

            return lstBankMasterViewDto;

        }

        #endregion

        #region InsertContraEntryWithDrawl

        public ResultDto FederationInsertContraEntryWithDrawl(ContraEntryWithDrawlDto contraEntryWithDrawlDto)
        {
            return FederationInsertUpdateContraEntryWithDrawl(contraEntryWithDrawlDto);
        }

        #endregion

        #region UpdateContraEntryWithDrawl

        public ResultDto FederationUpdateContraEntryWithDrawl(ContraEntryWithDrawlDto contraEntryWithDrawlDto)
        {
            return FederationInsertUpdateContraEntryWithDrawl(contraEntryWithDrawlDto);
        }

        #endregion

        #region InsertUpdateContraEntryWithDrawl

        private ResultDto FederationInsertUpdateContraEntryWithDrawl(ContraEntryWithDrawlDto contraEntryWithDrawlDto)
        {
            ResultDto resultDto = new ResultDto();
            string objectName = "ContraEntryWithDrawl";

            try
            {
                string contraEntryTransactionsxml = CommonMethods.SerializeListDto<List<ContraEntryWithDrawlTransactionsDto>>(contraEntryWithDrawlDto.contraEntryWithDrawlTransactions);

                ObjectParameter paramaccounmasterID = new ObjectParameter("AccountMasterID", contraEntryWithDrawlDto.AccountMasterID);
                ObjectParameter paramreCEWCode;
                if (contraEntryWithDrawlDto.VoucherNumber == "")
                {
                     paramreCEWCode = new ObjectParameter("VoucherNumber", string.Empty);
                }
                else
                {
                    paramreCEWCode = new ObjectParameter("VoucherNumber", contraEntryWithDrawlDto.VoucherNumber);
                }

                _dbContext.uspFederationContraEntryWithDrawlInsertUpdate(paramaccounmasterID, contraEntryWithDrawlDto.TransactionDate, paramreCEWCode, contraEntryWithDrawlDto.VoucherRefNumber, contraEntryWithDrawlDto.CodeSno, contraEntryWithDrawlDto.PartyName, contraEntryWithDrawlDto.EmployeeID,
                    contraEntryWithDrawlDto.AHID, contraEntryWithDrawlDto.SubHeadID, contraEntryWithDrawlDto.TransactionType, contraEntryWithDrawlDto.Amount, contraEntryWithDrawlDto.TransactionMode,
                    contraEntryWithDrawlDto.ChequeNumber, contraEntryWithDrawlDto.ChequeDate, contraEntryWithDrawlDto.BankAccount, contraEntryWithDrawlDto.Narration, contraEntryTransactionsxml, contraEntryWithDrawlDto.IsGroup, contraEntryWithDrawlDto.UserID);

                resultDto.ObjectId =Convert.ToInt32(paramaccounmasterID.Value);        //(int) contraEntryWithDrawlDto.AccountMasterID;
                resultDto.ObjectCode = string.IsNullOrEmpty((string)paramreCEWCode.Value) ? contraEntryWithDrawlDto.VoucherNumber : (string)paramreCEWCode.Value;

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

        #endregion

        #region FederationContraEntryWithDrawlLookup

        public List<ContraEntryWithDrawlLookupDto> FederationContraEntryWithDrawlLookup()
        {
            List<ContraEntryWithDrawlLookupDto> lstFederationContraEntryWithDrawlLookupDto = new List<ContraEntryWithDrawlLookupDto>();
            List<uspFederationContraEntryWithDrawlLookup_Result> objFederationContraEntryWithDrawlLookup_Result = _dbContext.uspFederationContraEntryWithDrawlLookup().ToList();
            foreach (var contraEntryWithDrawlLokup in objFederationContraEntryWithDrawlLookup_Result)
            {
                ContraEntryWithDrawlLookupDto contraEntryWithDrawlLookupDto = Mapper.Map<uspFederationContraEntryWithDrawlLookup_Result, ContraEntryWithDrawlLookupDto>(contraEntryWithDrawlLokup);
                lstFederationContraEntryWithDrawlLookupDto.Add(contraEntryWithDrawlLookupDto);
            }
            return lstFederationContraEntryWithDrawlLookupDto;
        }

        #endregion

        #region FederationDeleteContraEntryWithDrawl

        public ResultDto FederationDeleteContraEntryWithDrawl(int AccountMasterId, int userId)
        {
            ResultDto resultDto = new ResultDto();
            string obectName = "ContraEntryWithDrawl";

            try
            {
                ObjectParameter prmAccountMasterId = new ObjectParameter("AccountMasterId", AccountMasterId);
                ObjectParameter prmVoucherNumber = new ObjectParameter("VoucherNumber", string.Empty);

                int effectedCount = _dbContext.uspFederationContraEntryWithDrawlDelete(prmAccountMasterId, prmVoucherNumber, userId);

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

        #endregion

        #region FederationChangeStatusContraEntryWithDrawl

        public ResultDto FederationChangeStatusContraEntryWithDrawl(long AccountMasterId, int userId)
        {
            ResultDto resultDto = new ResultDto();
            string obectName = "ContraEntryWithDrawl";
            string statusCode = string.Empty;
            try
            {
                ObjectParameter prmAccountMasterId = new ObjectParameter("AccountMasterId", AccountMasterId);
                ObjectParameter prmVoucherNumber = new ObjectParameter("VoucherNumber", string.Empty);
                ObjectParameter prmStatusCode = new ObjectParameter("StatusCode", string.Empty);

                int effectedCount = _dbContext.uspFederationContraEntryWithDrawlChangeStatus(prmAccountMasterId, prmVoucherNumber, prmStatusCode, userId);

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

        #endregion

        #region FederationContraEntryWithDrawlGetByAccountMasterId

        public ContraEntryWithDrawlDto FederationContraEntryWithDrawlGetByAccountMasterId(long AccountMasterId)
        {
            long accountmasterID = Convert.ToInt64(AccountMasterId);
            ObjectParameter prmAccountMasterId = new ObjectParameter("AccountMasterId", accountmasterID);

            var results = new MFISDBContext()
                .MultipleResults(CustomProcNames.uspFederationContraEntryWithDrawlByAccountMasterID, prmAccountMasterId)
                .With<ContraEntryWithDrawlDto>()
                .With<ContraEntryWithDrawlTransactionsDto>()
                .Execute();

            var contraEntryWithDrawlDto = (results[0] as List<ContraEntryWithDrawlDto>)[0];
            var contraEntryWithDrawlTransactionsList = results[1] as List<ContraEntryWithDrawlTransactionsDto>;
            List<ContraEntryWithDrawlTransactionsDto> finalcontraEntryWithDrawlTransactions = new List<ContraEntryWithDrawlTransactionsDto>();
            ContraEntryWithDrawlTransactionsDto singleContraEntryWithDrawlTransactionsDto = null;
            foreach (var contraEntryWithDrawlTransactions in contraEntryWithDrawlTransactionsList)
            {
                singleContraEntryWithDrawlTransactionsDto = new ContraEntryWithDrawlTransactionsDto();
                singleContraEntryWithDrawlTransactionsDto.BankAccount = contraEntryWithDrawlTransactions.BankAccount;
                singleContraEntryWithDrawlTransactionsDto.BankAccountName =contraEntryWithDrawlTransactions.BankAccountName;
                singleContraEntryWithDrawlTransactionsDto.AccountTranID = contraEntryWithDrawlTransactions.AccountTranID;
                singleContraEntryWithDrawlTransactionsDto.AHCode = contraEntryWithDrawlTransactions.AHCode;
                singleContraEntryWithDrawlTransactionsDto.AHID = contraEntryWithDrawlTransactions.AHID;
                singleContraEntryWithDrawlTransactionsDto.AHName = contraEntryWithDrawlTransactions.AHName;
                singleContraEntryWithDrawlTransactionsDto.AmountId = contraEntryWithDrawlTransactions.AmountId;
                singleContraEntryWithDrawlTransactionsDto.ClosingBalance = contraEntryWithDrawlTransactions.ClosingBalance;
                singleContraEntryWithDrawlTransactionsDto.CrAmount = contraEntryWithDrawlTransactions.CrAmount;
                singleContraEntryWithDrawlTransactionsDto.DrAmount = contraEntryWithDrawlTransactions.DrAmount;
                singleContraEntryWithDrawlTransactionsDto.Type = contraEntryWithDrawlTransactions.Type;
                singleContraEntryWithDrawlTransactionsDto.Narration = contraEntryWithDrawlTransactions.Narration;
                finalcontraEntryWithDrawlTransactions.Add(singleContraEntryWithDrawlTransactionsDto);
            }

            contraEntryWithDrawlDto.contraEntryWithDrawlTransactions = finalcontraEntryWithDrawlTransactions;

            return contraEntryWithDrawlDto;
        }

        #endregion

        //-------------Common For Both---------------------------------//

        #region GetAccountHeadClosingBalnces

        public ContraEntryWithDrawlTransactionsDto GetAccountHeadClosingBalnces()
        {
            return Mapper.Map<uspGetAccountHeadClosingBalnces_Result, ContraEntryWithDrawlTransactionsDto>(_dbContext.uspGetAccountHeadClosingBalnces().ToList().FirstOrDefault());
        }

        #endregion

        #region GetBankNameByAHID

        public BankMasterDto GetBankNameByAHID(int AHID)
        {
            var objBankName = _dbContext.uspGetBankNameByAccountNumber(AHID).ToList().FirstOrDefault();
            //BankMasterDto objBankMasterDto = AutoMapperEntityConfiguration.Cast<BankMasterDto>(objuspuspGetBankNameByAccountNumber);
            BankMasterDto objBankMasterDto = new BankMasterDto();
            objBankMasterDto.BName = objBankName;
            return objBankMasterDto;
        }

        #endregion

        //-------------------- Federation Contra Entry Deposited----------------------------//


        #region FederationInsertContraEntryDeposit

        public ResultDto FederationInsertContraEntryDeposit(ContraEntryDepositedDto contraEntryDepositedDto)
        {
            return FederationInsertUpdateContraEntryDeposited(contraEntryDepositedDto);
        }

        #endregion

        #region FederationUpdateContraEntryDeposited

        public ResultDto FederationUpdateContraEntryDeposited(ContraEntryDepositedDto contraEntryDepositedDto)
        {
            return FederationInsertUpdateContraEntryDeposited(contraEntryDepositedDto);
        }

        #endregion

        #region FederationInsertUpdateContraEntryDeposited

        private ResultDto FederationInsertUpdateContraEntryDeposited(ContraEntryDepositedDto contraEntryDepositedDto)
        {
            ResultDto resultDto = new ResultDto();
            string objectName = "ContraEntryDeposited";

            try
            {
                string contraEntryTransactionsxml = CommonMethods.SerializeListDto<List<ContraEntryDepositedTransactionsDto>>(contraEntryDepositedDto.contraEntryDepositedTransactions);

                ObjectParameter paramaccounmasterID = new ObjectParameter("AccountMasterID", contraEntryDepositedDto.AccountMasterID);
                ObjectParameter paramreCEWCode;
                if (contraEntryDepositedDto.VoucherNumber == "")
                {
                    paramreCEWCode = new ObjectParameter("VoucherNumber", string.Empty);
                }
                else
                {
                    paramreCEWCode = new ObjectParameter("VoucherNumber", contraEntryDepositedDto.VoucherNumber);
                }

                _dbContext.uspFederationContraEntryDepositedInsertUpdate(paramaccounmasterID, contraEntryDepositedDto.TransactionDate, paramreCEWCode, contraEntryDepositedDto.VoucherRefNumber, contraEntryDepositedDto.CodeSno, contraEntryDepositedDto.PartyName, contraEntryDepositedDto.EmployeeID,
                    contraEntryDepositedDto.AHID, contraEntryDepositedDto.SubHeadID, contraEntryDepositedDto.TransactionType, contraEntryDepositedDto.Amount, contraEntryDepositedDto.TransactionMode,
                      contraEntryDepositedDto.BankAccount, contraEntryDepositedDto.Narration, contraEntryTransactionsxml, contraEntryDepositedDto.IsGroup, contraEntryDepositedDto.UserID);

                resultDto.ObjectId = Convert.ToInt32(paramaccounmasterID.Value);
                resultDto.ObjectCode = string.IsNullOrEmpty((string)paramreCEWCode.Value) ? contraEntryDepositedDto.VoucherNumber : (string)paramreCEWCode.Value;

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

        #endregion

        #region FederationContraEntryDepositedLookup

        public List<ContraEntryDepositedLookupDto> FederationContraEntryDepositedLookup()
        {
            List<ContraEntryDepositedLookupDto> lstContraEntryDepositedLookupDto = new List<ContraEntryDepositedLookupDto>();
            List<uspFederationContraEntryDepositedLookup_Result> uspContraEntryDepositedLookup_Result = _dbContext.uspFederationContraEntryDepositedLookup().ToList();
            foreach (var contraEntryDepositedLokup in uspContraEntryDepositedLookup_Result)
            {
                ContraEntryDepositedLookupDto contraEntryDepositedLookupDto = Mapper.Map<uspFederationContraEntryDepositedLookup_Result, ContraEntryDepositedLookupDto>(contraEntryDepositedLokup);
                lstContraEntryDepositedLookupDto.Add(contraEntryDepositedLookupDto);
            }
            return lstContraEntryDepositedLookupDto;
        }

        #endregion

        #region FederationDeleteContraEntryDeposited

        public ResultDto FederationDeleteContraEntryDeposited(int AccountMasterId, int userId)
        {
            ResultDto resultDto = new ResultDto();
            string obectName = "ContraEntryDeposited";

            try
            {
                ObjectParameter prmAccountMasterId = new ObjectParameter("AccountMasterId", AccountMasterId);
                ObjectParameter prmVoucherNumber = new ObjectParameter("VoucherNumber", string.Empty);

                int effectedCount = _dbContext.uspFederationContraEntryWithDrawlDelete(prmAccountMasterId, prmVoucherNumber, userId);

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

        #endregion

        #region FederationChangeStatusContraEntryDeposited

        public ResultDto FederationChangeStatusContraEntryDeposited(int AccountMasterId, int userId)
        {
            ResultDto resultDto = new ResultDto();
            string obectName = "ContraEntryDeposited";
            string statusCode = string.Empty;
            try
            {
                ObjectParameter prmAccountMasterId = new ObjectParameter("AccountMasterId", AccountMasterId);
                ObjectParameter prmVoucherNumber = new ObjectParameter("VoucherNumber", string.Empty);
                ObjectParameter prmStatusCode = new ObjectParameter("StatusCode", string.Empty);

                int effectedCount = _dbContext.uspFederationContraEntryWithDrawlChangeStatus(prmAccountMasterId, prmVoucherNumber, prmStatusCode, userId);

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

        #endregion

        #region FederationContraEntryDepositedGetByAccountMasterId

        public ContraEntryDepositedDto FederationContraEntryDepositedGetByAccountMasterId(long AccountMasterId)
        {
            long accountmasterID = Convert.ToInt64(AccountMasterId);
            ObjectParameter prmAccountMasterId = new ObjectParameter("AccountMasterId", accountmasterID);

            var results = new MFISDBContext()
                .MultipleResults(CustomProcNames.uspFederationContraEntryWithDrawlByAccountMasterID, prmAccountMasterId)
                .With<ContraEntryDepositedDto>()
                .With<ContraEntryDepositedTransactionsDto>()
                .Execute();

            var contraEntryDepositedDto = (results[0] as List<ContraEntryDepositedDto>)[0];
            var contraEntryDepositedTransactionsList = results[1] as List<ContraEntryDepositedTransactionsDto>;
            List<ContraEntryDepositedTransactionsDto> finalcontraEntryDepositedTransactions = new List<ContraEntryDepositedTransactionsDto>();
            ContraEntryDepositedTransactionsDto singleContraEntryDepositedTransactionsDto = null;
            foreach (var contraEntryDepositedTransactions in contraEntryDepositedTransactionsList)
            {
                singleContraEntryDepositedTransactionsDto = new ContraEntryDepositedTransactionsDto();
                singleContraEntryDepositedTransactionsDto.BankAccount = contraEntryDepositedTransactions.BankAccount;
                singleContraEntryDepositedTransactionsDto.BankAccountName = contraEntryDepositedTransactions.BankAccountName;
                singleContraEntryDepositedTransactionsDto.AccountTranID = contraEntryDepositedTransactions.AccountTranID;
                singleContraEntryDepositedTransactionsDto.AHCode = contraEntryDepositedTransactions.AHCode;
                singleContraEntryDepositedTransactionsDto.AHID = contraEntryDepositedTransactions.AHID;
                singleContraEntryDepositedTransactionsDto.AHName = contraEntryDepositedTransactions.AHName;
                singleContraEntryDepositedTransactionsDto.AmountId = contraEntryDepositedTransactions.AmountId;
                singleContraEntryDepositedTransactionsDto.ClosingBalance = contraEntryDepositedTransactions.ClosingBalance;
                singleContraEntryDepositedTransactionsDto.CrAmount = contraEntryDepositedTransactions.CrAmount;
                singleContraEntryDepositedTransactionsDto.DrAmount = contraEntryDepositedTransactions.DrAmount;
                singleContraEntryDepositedTransactionsDto.Type = contraEntryDepositedTransactions.Type;
                singleContraEntryDepositedTransactionsDto.Narration = contraEntryDepositedTransactions.Narration;
                finalcontraEntryDepositedTransactions.Add(singleContraEntryDepositedTransactionsDto);
                singleContraEntryDepositedTransactionsDto.IsMaster = contraEntryDepositedTransactions.IsMaster;
            }

            contraEntryDepositedDto.contraEntryDepositedTransactions = finalcontraEntryDepositedTransactions;
            return contraEntryDepositedDto;
        }

        #endregion


        //--------- Group Contra Entry With Drawl-----------------------------------//

        #region GetAllGroupBanks

        public List<BusinessEntities.BankMasterViewDto> GetAllGroupBanksByGroupId(int GroupID)
        {
            List<uspGroupGetAllBanksDetailsByGroupID_Result> lstuspGroupGetAllBanksDetailsByGroupID_Result = _dbContext.uspGroupGetAllBanksDetailsByGroupID(GroupID).ToList();


            List<BankMasterViewDto> lstBankMasterViewDto = new List<BankMasterViewDto>();
            foreach (var result in lstuspGroupGetAllBanksDetailsByGroupID_Result)
                lstBankMasterViewDto.Add(Mapper.Map<uspGroupGetAllBanksDetailsByGroupID_Result, BankMasterViewDto>(result));

            return lstBankMasterViewDto;

        }

        #endregion

        #region GroupContraEntryWithDrawl

        public ResultDto GroupInsertContraEntryWithDrawl(ContraEntryWithDrawlDto contraEntryWithDrawlDto)
        {
            return GroupInsertUpdateContraEntryWithDrawl(contraEntryWithDrawlDto);
        }

        #endregion

        #region GroupUpdateContraEntryWithDrawl

        public ResultDto GroupUpdateContraEntryWithDrawl(ContraEntryWithDrawlDto contraEntryWithDrawlDto)
        {
            return GroupInsertUpdateContraEntryWithDrawl(contraEntryWithDrawlDto);
        }

        #endregion

        #region GroupInsertUpdateContraEntryWithDrawl

        private ResultDto GroupInsertUpdateContraEntryWithDrawl(ContraEntryWithDrawlDto contraEntryWithDrawlDto)
        {
            ResultDto resultDto = new ResultDto();
            string objectName = "ContraEntryWithDrawl";

            try
            {
                string contraEntryTransactionsxml = CommonMethods.SerializeListDto<List<ContraEntryWithDrawlTransactionsDto>>(contraEntryWithDrawlDto.contraEntryWithDrawlTransactions);

                ObjectParameter paramaccounmasterID = new ObjectParameter("AccountMasterID", contraEntryWithDrawlDto.AccountMasterID);
                ObjectParameter paramreCEWCode;
                if (contraEntryWithDrawlDto.VoucherNumber == "")
                {
                    paramreCEWCode = new ObjectParameter("VoucherNumber", string.Empty);
                }
                else
                {
                    paramreCEWCode = new ObjectParameter("VoucherNumber", contraEntryWithDrawlDto.VoucherNumber);
                }

                _dbContext.uspGroupContraEntryWithDrawlInsertUpdate(paramaccounmasterID, contraEntryWithDrawlDto.TransactionDate, paramreCEWCode, contraEntryWithDrawlDto.VoucherRefNumber, contraEntryWithDrawlDto.CodeSno, contraEntryWithDrawlDto.PartyName, contraEntryWithDrawlDto.EmployeeID,
                    contraEntryWithDrawlDto.AHID, contraEntryWithDrawlDto.SubHeadID, contraEntryWithDrawlDto.TransactionType, contraEntryWithDrawlDto.Amount, contraEntryWithDrawlDto.TransactionMode,
                    contraEntryWithDrawlDto.ChequeNumber,DateTime.Parse(contraEntryWithDrawlDto.ChequeDate.ToString()), contraEntryWithDrawlDto.BankAccount, contraEntryWithDrawlDto.Narration, contraEntryTransactionsxml, contraEntryWithDrawlDto.IsGroup, contraEntryWithDrawlDto.GroupID,contraEntryWithDrawlDto.UserID);

                resultDto.ObjectId = Convert.ToInt32(paramaccounmasterID.Value);        //(int) contraEntryWithDrawlDto.AccountMasterID;
                resultDto.ObjectCode = string.IsNullOrEmpty((string)paramreCEWCode.Value) ? contraEntryWithDrawlDto.VoucherNumber : (string)paramreCEWCode.Value;

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

        #endregion

        #region GroupContraEntryWithDrawlLookup

        public List<ContraEntryWithDrawlLookupDto> GroupContraEntryWithDrawlLookup(int groupId, int userId)
        {
            return new ContraEntryDal().GroupContraEntryWithDrawlLookup(groupId, userId);
            /*
            List<ContraEntryWithDrawlLookupDto> lstFederationContraEntryWithDrawlLookupDto = new List<ContraEntryWithDrawlLookupDto>();
            List<uspGroupContraEntryWithDrawlLookup_Result> objFederationContraEntryWithDrawlLookup_Result = _dbContext.uspGroupContraEntryWithDrawlLookup(groupId).ToList();
            foreach (var contraEntryWithDrawlLokup in objFederationContraEntryWithDrawlLookup_Result)
            {
                ContraEntryWithDrawlLookupDto contraEntryWithDrawlLookupDto = Mapper.Map<uspGroupContraEntryWithDrawlLookup_Result, ContraEntryWithDrawlLookupDto>(contraEntryWithDrawlLokup);
                lstFederationContraEntryWithDrawlLookupDto.Add(contraEntryWithDrawlLookupDto);
            }
            return lstFederationContraEntryWithDrawlLookupDto;*/
        }

        #endregion

        #region GroupDeleteContraEntryWithDrawl

        public ResultDto GroupDeleteContraEntryWithDrawl(int AccountMasterId, int userId)
        {
            ResultDto resultDto = new ResultDto();
            string obectName = "ContraEntryWithDrawl";

            try
            {
                ObjectParameter prmAccountMasterId = new ObjectParameter("AccountMasterId", AccountMasterId);
                ObjectParameter prmVoucherNumber = new ObjectParameter("VoucherNumber", string.Empty);

                int effectedCount = _dbContext.uspFederationContraEntryWithDrawlDelete(prmAccountMasterId, prmVoucherNumber, userId);

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

        #endregion

        #region GroupChangeStatusContraEntryWithDrawl

        public ResultDto GroupChangeStatusContraEntryWithDrawl(long AccountMasterId, int userId)
        {
            ResultDto resultDto = new ResultDto();
            string obectName = "ContraEntryWithDrawl";
            string statusCode = string.Empty;
            try
            {
                ObjectParameter prmAccountMasterId = new ObjectParameter("AccountMasterId", AccountMasterId);
                ObjectParameter prmVoucherNumber = new ObjectParameter("VoucherNumber", string.Empty);
                ObjectParameter prmStatusCode = new ObjectParameter("StatusCode", string.Empty);

                int effectedCount = _dbContext.uspFederationContraEntryWithDrawlChangeStatus(prmAccountMasterId, prmVoucherNumber, prmStatusCode, userId);

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

        #endregion

        #region GroupContraEntryWithDrawlGetByAccountMasterId
        public ContraEntryWithDrawlDto GroupContraEntryWithDrawlGetByAccountMasterId(long AccountMasterId)
        {
            long accountmasterID = Convert.ToInt64(AccountMasterId);
            ObjectParameter prmAccountMasterId = new ObjectParameter("AccountMasterId", accountmasterID);

            var results = new MFISDBContext()
                .MultipleResults(CustomProcNames.uspGroupContraEntryWithDrawlByAccountMasterID, prmAccountMasterId)
                .With<ContraEntryWithDrawlDto>()
                .With<ContraEntryWithDrawlTransactionsDto>()
                .Execute();

            var contraEntryWithDrawlDto = (results[0] as List<ContraEntryWithDrawlDto>)[0];
            var contraEntryWithDrawlTransactionsList = results[1] as List<ContraEntryWithDrawlTransactionsDto>;
            List<ContraEntryWithDrawlTransactionsDto> finalcontraEntryWithDrawlTransactions = new List<ContraEntryWithDrawlTransactionsDto>();
            ContraEntryWithDrawlTransactionsDto singleContraEntryWithDrawlTransactionsDto = null;
            foreach (var contraEntryWithDrawlTransactions in contraEntryWithDrawlTransactionsList)
            {
                singleContraEntryWithDrawlTransactionsDto = new ContraEntryWithDrawlTransactionsDto();
                singleContraEntryWithDrawlTransactionsDto.BankAccount = contraEntryWithDrawlTransactions.BankAccount;
                singleContraEntryWithDrawlTransactionsDto.BankAccountName = contraEntryWithDrawlTransactions.BankAccountName;
                singleContraEntryWithDrawlTransactionsDto.AccountTranID = contraEntryWithDrawlTransactions.AccountTranID;
                singleContraEntryWithDrawlTransactionsDto.AHCode = contraEntryWithDrawlTransactions.AHCode;
                singleContraEntryWithDrawlTransactionsDto.AHID = contraEntryWithDrawlTransactions.AHID;
                singleContraEntryWithDrawlTransactionsDto.AHName = contraEntryWithDrawlTransactions.AHName;
                singleContraEntryWithDrawlTransactionsDto.AmountId = contraEntryWithDrawlTransactions.AmountId;
                singleContraEntryWithDrawlTransactionsDto.ClosingBalance = contraEntryWithDrawlTransactions.ClosingBalance;
                singleContraEntryWithDrawlTransactionsDto.CrAmount = contraEntryWithDrawlTransactions.CrAmount;
                singleContraEntryWithDrawlTransactionsDto.DrAmount = contraEntryWithDrawlTransactions.DrAmount;
                singleContraEntryWithDrawlTransactionsDto.Type = contraEntryWithDrawlTransactions.Type;
                singleContraEntryWithDrawlTransactionsDto.Narration = contraEntryWithDrawlTransactions.Narration;
                finalcontraEntryWithDrawlTransactions.Add(singleContraEntryWithDrawlTransactionsDto);
            }

            contraEntryWithDrawlDto.contraEntryWithDrawlTransactions = finalcontraEntryWithDrawlTransactions;

            return contraEntryWithDrawlDto;
  
        }
        #endregion 

        //--------- Group Contra Entry Deposited -----------------------------------//

        #region GroupInsertContraEntryDeposit

        public ResultDto GroupInsertContraEntryDeposited(ContraEntryDepositedDto contraEntryDepositedDto)
        {
            return GroupInsertUpdateContraEntryDeposited(contraEntryDepositedDto);
        }

        #endregion

        #region GroupUpdateContraEntryDeposited

        public ResultDto GroupUpdateContraEntryDeposited(ContraEntryDepositedDto contraEntryDepositedDto)
        {
            return GroupInsertUpdateContraEntryDeposited(contraEntryDepositedDto);
        }

        #endregion

        #region GroupInsertUpdateContraEntryDeposited

        private ResultDto GroupInsertUpdateContraEntryDeposited(ContraEntryDepositedDto contraEntryDepositedDto)
        {
            ResultDto resultDto = new ResultDto();
            string objectName = "ContraEntryDeposited";

            try
            {
                string contraEntryTransactionsxml = CommonMethods.SerializeListDto<List<ContraEntryDepositedTransactionsDto>>(contraEntryDepositedDto.contraEntryDepositedTransactions);

                ObjectParameter paramaccounmasterID = new ObjectParameter("AccountMasterID", contraEntryDepositedDto.AccountMasterID);
                ObjectParameter paramreCEWCode;
                if (contraEntryDepositedDto.VoucherNumber == "")
                {
                    paramreCEWCode = new ObjectParameter("VoucherNumber", string.Empty);
                }
                else
                {
                    paramreCEWCode = new ObjectParameter("VoucherNumber", contraEntryDepositedDto.VoucherNumber);
                }

                _dbContext.uspGroupContraEntryDepositedInsertUpdate(paramaccounmasterID, contraEntryDepositedDto.TransactionDate, paramreCEWCode, contraEntryDepositedDto.VoucherRefNumber, contraEntryDepositedDto.CodeSno, contraEntryDepositedDto.PartyName, contraEntryDepositedDto.EmployeeID,
                    contraEntryDepositedDto.AHID, contraEntryDepositedDto.SubHeadID, contraEntryDepositedDto.TransactionType, contraEntryDepositedDto.Amount, contraEntryDepositedDto.TransactionMode,
                    contraEntryDepositedDto.BankAccount, contraEntryDepositedDto.Narration, contraEntryTransactionsxml, contraEntryDepositedDto.IsGroup, contraEntryDepositedDto.GroupID, contraEntryDepositedDto.UserID);

                resultDto.ObjectId = Convert.ToInt32(paramaccounmasterID.Value);
                resultDto.ObjectCode = string.IsNullOrEmpty((string)paramreCEWCode.Value) ? contraEntryDepositedDto.VoucherNumber : (string)paramreCEWCode.Value;

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

        #endregion

        #region GroupContraEntryDepositedLookup

        public List<ContraEntryDepositedLookupDto> GroupContraEntryDepositedLookup(int groupId, int userId)
        {
            return new ContraEntryDal().GroupContraEntryDepositedLookup(groupId, userId);

            /*List<ContraEntryDepositedLookupDto> lstContraEntryDepositedLookupDto = new List<ContraEntryDepositedLookupDto>();
            List<uspGroupContraEntryDepositedLookup_Result> uspContraEntryDepositedLookup_Result = _dbContext.uspGroupContraEntryDepositedLookup(groupId).ToList();
            foreach (var contraEntryDepositedLokup in uspContraEntryDepositedLookup_Result)
            {
                ContraEntryDepositedLookupDto contraEntryDepositedLookupDto = Mapper.Map<uspGroupContraEntryDepositedLookup_Result, ContraEntryDepositedLookupDto>(contraEntryDepositedLokup);
                lstContraEntryDepositedLookupDto.Add(contraEntryDepositedLookupDto);
            }
            return lstContraEntryDepositedLookupDto;*/
        }

        #endregion

        #region GroupDeleteContraEntryDeposited

        public ResultDto GroupDeleteContraEntryDeposited(int AccountMasterId, int userId)
        {
            ResultDto resultDto = new ResultDto();
            string obectName = "ContraEntryDiposited";

            try
            {
                ObjectParameter prmAccountMasterId = new ObjectParameter("AccountMasterId", AccountMasterId);
                ObjectParameter prmVoucherNumber = new ObjectParameter("VoucherNumber", string.Empty);

                int effectedCount = _dbContext.uspFederationContraEntryWithDrawlDelete(prmAccountMasterId, prmVoucherNumber, userId);

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

        #endregion

        #region GroupChangeStatusContraEntryDeposited

        public ResultDto GroupChangeStatusContraEntryDeposited(long AccountMasterId, int userId)
        {
            ResultDto resultDto = new ResultDto();
            string obectName = "ContraEntryDeposited";
            string statusCode = string.Empty;
            try
            {
                ObjectParameter prmAccountMasterId = new ObjectParameter("AccountMasterId", AccountMasterId);
                ObjectParameter prmVoucherNumber = new ObjectParameter("VoucherNumber", string.Empty);
                ObjectParameter prmStatusCode = new ObjectParameter("StatusCode", string.Empty);

                int effectedCount = _dbContext.uspFederationContraEntryWithDrawlChangeStatus(prmAccountMasterId, prmVoucherNumber, prmStatusCode, userId);

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

        #endregion

        #region GroupContraEntryDepositedGetByAccountMasterId
        public ContraEntryDepositedDto GroupContraEntryDepositedGetByAccountMasterId(long AccountMasterId)
        {
            long accountmasterID = Convert.ToInt64(AccountMasterId);
            ObjectParameter prmAccountMasterId = new ObjectParameter("AccountMasterId", accountmasterID);

            var results = new MFISDBContext()
                .MultipleResults(CustomProcNames.uspGroupContraEntryWithDrawlByAccountMasterID, prmAccountMasterId)
                .With<ContraEntryDepositedDto>()
                .With<ContraEntryDepositedTransactionsDto>()
                .Execute();

            var contraEntryDepositedDto = (results[0] as List<ContraEntryDepositedDto>)[0];
            var contraEntryDepositedTransactionsList = results[1] as List<ContraEntryDepositedTransactionsDto>;
            List<ContraEntryDepositedTransactionsDto> finalcontraEntryDepositedTransactions = new List<ContraEntryDepositedTransactionsDto>();
            ContraEntryDepositedTransactionsDto singleContraEntryDepositedTransactionsDto = null;
            foreach (var contraEntryDepositedTransactions in contraEntryDepositedTransactionsList)
            {
                singleContraEntryDepositedTransactionsDto = new ContraEntryDepositedTransactionsDto();
                singleContraEntryDepositedTransactionsDto.BankAccount = contraEntryDepositedTransactions.BankAccount;
                singleContraEntryDepositedTransactionsDto.BankAccountName = contraEntryDepositedTransactions.BankAccountName;
                singleContraEntryDepositedTransactionsDto.AccountTranID = contraEntryDepositedTransactions.AccountTranID;
                singleContraEntryDepositedTransactionsDto.AHCode = contraEntryDepositedTransactions.AHCode;
                singleContraEntryDepositedTransactionsDto.AHID = contraEntryDepositedTransactions.AHID;
                singleContraEntryDepositedTransactionsDto.AHName = contraEntryDepositedTransactions.AHName;
                singleContraEntryDepositedTransactionsDto.AmountId = contraEntryDepositedTransactions.AmountId;
                singleContraEntryDepositedTransactionsDto.ClosingBalance = contraEntryDepositedTransactions.ClosingBalance;
                singleContraEntryDepositedTransactionsDto.CrAmount = contraEntryDepositedTransactions.CrAmount;
                singleContraEntryDepositedTransactionsDto.DrAmount = contraEntryDepositedTransactions.DrAmount;
                singleContraEntryDepositedTransactionsDto.Type = contraEntryDepositedTransactions.Type;
                singleContraEntryDepositedTransactionsDto.Narration = contraEntryDepositedTransactions.Narration;
                finalcontraEntryDepositedTransactions.Add(singleContraEntryDepositedTransactionsDto);
                singleContraEntryDepositedTransactionsDto.IsMaster = contraEntryDepositedTransactions.IsMaster;
            }

            contraEntryDepositedDto.contraEntryDepositedTransactions = finalcontraEntryDepositedTransactions;
            return contraEntryDepositedDto;
        }
        #endregion

    }
}
