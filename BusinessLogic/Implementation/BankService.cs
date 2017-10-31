using AutoMapper;
using BusinessEntities;
using BusinessLogic.AutoMapper;
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
    public class BankService 
    {
        private readonly MFISDBContext _dbContext;

        public BankService()
        {
            _dbContext = new MFISDBContext();
            AutoMapperEntityConfiguration.Configure();
        }
        public List<BankMasterDto> BanksGetByObjectId(int groupId)
        {

            List<uspBankMasterGetAll_Result> lstuspBankMasterGetAll_Result = _dbContext.uspBankMasterGetAll().ToList();
            List<BankMasterDto> lstBankMasterDto = Mapper.Map<List<uspBankMasterGetAll_Result>, List<BankMasterDto>>(lstuspBankMasterGetAll_Result);
            BankMasterDto obj = null;
            foreach (var bank in lstuspBankMasterGetAll_Result)
            {
                obj = new BankMasterDto();
                obj.BankEntryID = bank.BankEntryID;
                obj.EntityId = bank.EntityID;
                obj.BankName = bank.BankName;
                obj.BName = bank.BName;
                obj.BranchName = bank.BranchName;
                obj.IFSC = bank.IFSC;
                obj.AccountType = bank.AccountType;
                obj.AccountTypeText = bank.AccountTypeText;
                obj.AccountNumber = bank.AccountNumber;
                obj.ObjectID = bank.ObjectID;
                lstBankMasterDto.Add(obj);
            }
          
            lstBankMasterDto = lstBankMasterDto.FindAll(g => g.ObjectID == groupId && g.EntityId == 9);
            return lstBankMasterDto;

        }
        public List<BankMasterDto> GetAll()
        {
            List<uspBankMasterGetAll_Result> lstuspBankMasterGetAll_Result = _dbContext.uspBankMasterGetAll().ToList();
            List<BankMasterDto> lstBankMasterDto = Mapper.Map<List<uspBankMasterGetAll_Result>, List<BankMasterDto>>(lstuspBankMasterGetAll_Result);
            return lstBankMasterDto;
        }

        //public List<BankMasterLookupDto> GetLookup()
        //{
        //    var lstBankMasterLookupDtos = new List<BankMasterLookupDto>();
        //    var uspBankMasterLookupResults = _dbContext.uspBankMasterLookup().ToList();
        //    foreach (var bank in uspBankMasterLookupResults)
        //    {
        //        BankMasterLookupDto lookupDto = Mapper.Map<uspBankMasterLookup_Result, BankMasterLookupDto>(bank);
        //        lstBankMasterLookupDtos.Add(lookupDto);
        //    }

        //    return lstBankMasterLookupDtos;
        //}


        public BankMasterDto GetByID(int bankEntryId)
        {
            uspBankMasterGetByBankId_Result objuspBankMasterGetByBankIdResult =
                _dbContext.uspBankMasterGetByBankId(bankEntryId).ToList().FirstOrDefault();
            BankMasterDto objBankMasterDto = AutoMapperEntityConfiguration.Cast<BankMasterDto>(objuspBankMasterGetByBankIdResult);

            return objBankMasterDto;
        }

        public ResultDto Insert(BankMasterDto bank)
        {
            return InsertUpdateBankMaster(bank);
        }

        public ResultDto Update(BankMasterDto bank)
        {
            return InsertUpdateBankMaster(bank);

        }

        private ResultDto InsertUpdateBankMaster(BankMasterDto bank)
        {
            ResultDto resultDto = new ResultDto();
            string obectName = "bank master";

            try
            {
                ObjectParameter paramBankEntryId = new ObjectParameter("BankEntryId", bank.BankEntryID);
                ObjectParameter paramBankCode = new ObjectParameter("BankCode", string.Empty);
                _dbContext.uspBankMasterInsertUpdate(paramBankEntryId, bank.BankName, bank.BranchName, bank.IFSC, bank.AccountNumber, bank.AccountType, bank.ContactNumber, bank.Email, bank.Address, bank.isMasterEntry, bank.UserID, paramBankCode);

                if (bank.BankEntryID > 0)
                    resultDto.ObjectCode = bank.BankCode;
                else
                    resultDto.ObjectCode = (string)paramBankCode.Value;
                resultDto.ObjectId = (int)paramBankEntryId.Value;

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

        public List<SelectListDto> GetBankSelectList()
        {
            List<SelectListDto> lstSelectListDto = new List<SelectListDto>();
            List<uspBankMasterGetAll_Result> lstuspBankMasterGetAll_Result = _dbContext.uspBankMasterGetAll().ToList();


            foreach (var bank in lstuspBankMasterGetAll_Result)
            {
                SelectListDto objSelectListDto = new SelectListDto()
                {
                    ID = bank.BankEntryID,
                    Text = bank.BankName.ToString()

                };
                lstSelectListDto.Add(objSelectListDto);
            }
            return lstSelectListDto;
        }

        public BankMasterViewDto GetViewByID(int bankEntryId)
        {
            var prmbankEntryId = new ObjectParameter("BankEntryID", bankEntryId);

            var results = new MFISDBContext()
                .MultipleResults(CustomProcNames.uspBankMasterGetViewByID, prmbankEntryId)
                .With<BankMasterViewDto>()
                .Execute();

            var bankMasterViewDto = (results[0] as List<BankMasterViewDto>)[0];

            return bankMasterViewDto;
        }
        //public bool AddBankMaster(BankMasterDto bankMasterDto)
        //{
        //    bool isSuccess = true;
        //    try
        //    {
        //        BankMaster bankMaster =new BankMaster();
        //        Utilities.Mapper1.Map(bankMasterDto, bankMaster);

        //        bankMaster.CreatedBy = 1;
        //        bankMaster.StatusID =
        //            _dbContext.StatusMasters.ToList().Find(l => l.StatusCode == Constants.StatusCodes.Active).StatusID;
        //        bankMaster.CreatedOn = DateTime.Now;
        //        bankMaster.isMasterEntry = true;

        //        _dbContext.BankMasters.Add(bankMaster);
        //        _dbContext.SaveChanges();
        //    }
        //    catch (Exception ex)
        //    {
        //        isSuccess = false;
        //    }
        //    return isSuccess;
        //}

        //public List<BankMasterDto> GetAllBankMasters()
        //{
        //    IList<BankMaster> bankMasters = _dbContext.BankMasters.ToList();

        //    List<BankMasterDto> lstBankMasterDto =new List<BankMasterDto>();
        //    Utilities.Mapper1.Map(bankMasters, lstBankMasterDto);
        //    return lstBankMasterDto;
        //}





        public ResultDto Delete(int bankEntryId, int userId)
        {
            ResultDto resultDto = new ResultDto();
            string obectName = "BankMaster";

            try
            {
                ObjectParameter prmbankEntryId = new ObjectParameter("BankEntryId", bankEntryId);
                ObjectParameter prmBankCode = new ObjectParameter("BankCode", string.Empty);

                int effectedCount = _dbContext.uspBankMasterDelete(prmbankEntryId, prmBankCode, userId);

                resultDto.ObjectId = (int)prmbankEntryId.Value;
                resultDto.ObjectCode = (string)prmBankCode.Value;

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

        public ResultDto ChangeStatus(int bankEntryId, int userId)
        {
            ResultDto resultDto = new ResultDto();
            string obectName = "BankMaster";
            string statusCode = string.Empty;
            try
            {
                ObjectParameter prmbankEntryId = new ObjectParameter("BankEntryID", bankEntryId);
                ObjectParameter prmBankCode = new ObjectParameter("BankCode", string.Empty);
                ObjectParameter prmStatusCode = new ObjectParameter("StatusCode", string.Empty);

                int effectedCount = _dbContext.uspBankMasterChangeStatus(prmbankEntryId, prmBankCode, prmStatusCode, userId);

                resultDto.ObjectId = (int)prmbankEntryId.Value;
                resultDto.ObjectCode = (string)prmBankCode.Value;
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
    }
}
