using AutoMapper;
using BusinessEntities;
using BusinessLogic.AutoMapper;
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
    public class AccountHeadService 
    {
        #region Global Variables
        private readonly MFISDBContext _dbContext;
        private readonly CommonService _commonService;
        private readonly AccountHeadDll _accountHeadDll;
        public AccountHeadService()
        {
            _dbContext = new MFISDBContext();
            _commonService = new CommonService();
            _accountHeadDll = new AccountHeadDll();
            AutoMapperEntityConfiguration.Configure();
        }
        #endregion

        public List<AccountHeadDto> GetAll(bool isFederation)
        {
            var lstAccountHeadDto = new List<AccountHeadDto>();
            var lstuspAccountHeadGetAll_Result = _dbContext.uspAccountHeadGetAll().ToList().FindAll(f => f.IsFederation == isFederation || f.AHLevel < 4);
            foreach (var AH in lstuspAccountHeadGetAll_Result)
            {
                AccountHeadDto accountHeadDto = Mapper.Map<uspAccountHeadGetAll_Result, AccountHeadDto>(AH);
                lstAccountHeadDto.Add(accountHeadDto);
            }
            return lstAccountHeadDto;
        }

        public List<AccountHeadDto> GetJournalLedgers()
        {
            RefMaster rm = _dbContext.RefMasters.ToList().Find(r => r.RefMasterCode.ToUpper() == "AHTYPE");
            RefValueMaster rvm = _dbContext.RefValueMasters.ToList().Find(f => f.RefMasterID == rm.RefMasterID && f.RefCode.ToUpper() == "ASSETS");

            List<AccountHead> accountheads = _dbContext.AccountHeads.ToList();

            List<AccountHead> ahs = (List<AccountHead>)(from l3 in accountheads
                                                        from l4 in accountheads
                                                        from l5 in accountheads
                                                        where l3.AHID == l4.ParentAHID
                                                        && l4.AHID == l5.ParentAHID && l3.AHName.ToUpper() == "CLOSING BALANCES" && l3.AHType == rvm.RefID
                                                        select l5).ToList();

            var b = from ah in accountheads
                    where !(from o in ahs select o.AHID)
                             .Contains(ah.AHID)
                             && ah.AHLevel > 4
                    select ah;



            List<AccountHeadDto> lstResult = new List<AccountHeadDto>();
            foreach (var a in b)
            {
                lstResult.Add(new AccountHeadDto() { AHCode = a.AHCode, AHID = a.AHID, AHLevel = a.AHLevel, AHType = a.AHType, AHName = a.AHName, IsFederation = a.IsFederation, ParentAHID = a.ParentAHID.HasValue ? a.ParentAHID.Value : 0 });
            }
            return lstResult;
        }

        public AccountHeadDto GetByID(int ahid)
        {
            return Mapper.Map<uspAccountHeadGetByAHID_Result, AccountHeadDto>(_dbContext.uspAccountHeadGetByAHID(ahid).ToList().FirstOrDefault());
        }

        public AccountHeadDto GetByCashValue(string Cashvalue)
        {
            return new AccountHeadDto();
            // return Mapper.Map<uspAccountheadgetcashvalue_Result, AccountHeadDto>(_dbContext.uspAccountheadgetcashvalue(Cashvalue).ToList().FirstOrDefault());
        }
        public AccountHeadDto GetAccountHeadViewBalanceSummary(int ahId, bool isFederation)
        {
            AccountHeadDll iAccountHeadDll = new AccountHeadDll();

            return iAccountHeadDll.ViewBalance(ahId, isFederation);

        }

        public AccountHeadDto GetAccountHeadViewBalanceSummary(int ahId, bool isFederation, int groupId)
        {
            AccountHeadDll iAccountHeadDll = new AccountHeadDll();

            return iAccountHeadDll.ViewBalance(ahId, isFederation, groupId);

        }


        public ResultDto Insert(AccountHeadDto accountHead)
        {
            return AccountHeadInsertUpdate(accountHead);
        }

        public ResultDto Update(AccountHeadDto accountHead)
        {
            return AccountHeadInsertUpdate(accountHead);
        }

        public ResultDto AccountHeadInsertUpdate(AccountHeadDto accountHead)
        {
            ResultDto resultDto = new ResultDto();
            try
            {
                string ahCode = string.Empty;
                AccountHeadDll iAccountHeadDll = new AccountHeadDll();
                int accountHeadId = iAccountHeadDll.InsertUpdateAccountHead(accountHead, "ACCOUNT_TREE", accountHead.AHID, out ahCode);

                resultDto.ObjectId = accountHeadId;
                resultDto.ObjectCode = ahCode;

                /*ObjectParameter paramAccountHeadId = new ObjectParameter("AHID", accountHead.AHID);
                ObjectParameter paramaccountHeadCode = new ObjectParameter("AHCode", accountHead.AHCode);
                _dbContext.uspAccountHeadInsertUpdate(paramAccountHeadId, paramaccountHeadCode, accountHead.AHName,
                    accountHead.TE_AHName, accountHead.AHType, accountHead.ParentAHID, accountHead.IsMemberTransaction,
                    accountHead.IsSLAccount, accountHead.OpeningBalance, accountHead.OpeningBalanceType,
                    accountHead.AHLevel, accountHead.IsFederation, accountHead.UserID);

                resultDto.ObjectId = (int)paramAccountHeadId.Value;
                resultDto.ObjectCode = (string)paramaccountHeadCode.Value;

                int accountHeadId = (int)paramAccountHeadId.Value;*/

                if (accountHeadId > 0)
                    resultDto.Message = "AccountHead details saved success fully";
                else if (accountHeadId == -1)
                {
                    resultDto.Message = "Error occured while saving AccountHead details";
                    resultDto.ObjectId = -1;
                }
                else
                {
                    resultDto.Message = "Error occured while saving AccountHead details";
                    resultDto.ObjectId = -1;
                }
            }
            catch (Exception ex)
            {
                resultDto.Message = "Service layer error occured while saving the bank details";
                resultDto.ObjectId = -98;
            }
            return resultDto;
        }

        public ResultDto Delete(int ahid, int userId)
        {
            ResultDto resultDto = new ResultDto();
            string obectName = "AccountHead";

            try
            {
                ObjectParameter prmResultID = new ObjectParameter("Result", resultDto.Result);
                ObjectParameter prmMessage = new ObjectParameter("Message", string.Empty);

                _dbContext.uspAccountHeadDiscard(ahid, userId, prmResultID, prmMessage);

                resultDto.Result = (bool)prmResultID.Value;
                resultDto.Message = (string)prmMessage.Value;
                resultDto.ObjectId = (int)ahid;

            }
            catch (Exception)
            {
                resultDto.Message = string.Format("Service layer error occured while deleting the {0} details", obectName);
                resultDto.ObjectId = -98;

            }
            return resultDto;
        }


        public List<SelectListDto> GetAccountHeadSelectList()
        {
            List<SelectListDto> lstSelectListDto = new List<SelectListDto>();
            List<uspAccountHeadGetAll_Result> lstuspAccountHeadGetAll_Result = _dbContext.uspAccountHeadGetAll().ToList();
            foreach (var objuspAccountHeadGetAll_Result in lstuspAccountHeadGetAll_Result)
            {
                SelectListDto objSelectListDto = new SelectListDto()
                {
                    ID = objuspAccountHeadGetAll_Result.AHID,
                    Text = objuspAccountHeadGetAll_Result.AHName + " - (" + objuspAccountHeadGetAll_Result.AHCode + ")"
                };
                lstSelectListDto.Add(objSelectListDto);
            }
            return lstSelectListDto;
        }
        public List<AccountHeadDto> GetallAccountHeads()
        {
            List<AccountHeadDto> lstAccountHeadDto = new List<AccountHeadDto>();
            List<uspAccountHeadGetAll_Result> lstuspAccountHeadGetAll_Result = _dbContext.uspAccountHeadGetAll().ToList();
            foreach (var accountheads in lstuspAccountHeadGetAll_Result) 
            {

                AccountHeadDto objaccounthead = new AccountHeadDto();
                objaccounthead.AHID = accountheads.AHID;
                objaccounthead.AHName = accountheads.AHName;
                objaccounthead.AHLevel = accountheads.AHLevel;
                objaccounthead.IsFederation = accountheads.IsFederation;
                lstAccountHeadDto.Add(objaccounthead);
            }
            return lstAccountHeadDto;
        }
        public List<SelectListDto> GetFederaionAccountHeadSelectList()
        {
            List<SelectListDto> lstSelectListDto = new List<SelectListDto>();
            List<AccountHeadDto> lstAh = GetAll(true).ToList();
            foreach (var objuspAccountHeadGetAll_Result in lstAh)
            {
                SelectListDto objSelectListDto = new SelectListDto()
                {
                    ID = objuspAccountHeadGetAll_Result.AHID,
                    Text = objuspAccountHeadGetAll_Result.AHName + " - (" + objuspAccountHeadGetAll_Result.AHCode + ")"
                };
                lstSelectListDto.Add(objSelectListDto);
            }
            return lstSelectListDto;
        }

        //public List<SelectListDto> GetAccountHeadJVSelectList()
        //{
        //    List<SelectListDto> lstSelectJvDto = new List<SelectListDto>();
        //    List<uspGetAccountHeadsForJV_Result> lstuspAccountHeadGetAll_Result = _dbContext.uspGetAccountHeadsForJV().ToList();
        //    foreach (var objuspGetAccountHeadsForJV_Result in lstuspAccountHeadGetAll_Result)
        //    {
        //        SelectListDto objSelectListDto = new SelectListDto()
        //        {
        //            ID = uspGetAccountHeadsForJV_Result.A,
        //            Text = uspGetAccountHeadsForJV_Result.AHName
        //        };
        //        lstSelectJvDto.Add(objSelectListDto);
        //    }
        //    return lstSelectJvDto;
        //}
        public ResultDto MoveAccountHead(AccountHeadDto accountHeadDto)
        {
            ResultDto resultDto = new ResultDto();
            try
            {
                resultDto.ObjectId = accountHeadDto.ParentAHID;
                int effectedCount = _dbContext.uspAccountHeadMove(accountHeadDto.AHIDS, accountHeadDto.ParentAHID, accountHeadDto.AHLevel, accountHeadDto.AHType, accountHeadDto.UserID);

                if (effectedCount > 0)
                    resultDto.Message = "AccountHead details moved success fully";
                else if (effectedCount == -1)
                {
                    resultDto.Message = "Error occured while moved AccountHead";
                    resultDto.ObjectId = -1;
                }
                else
                {
                    resultDto.Message = "Error occured while moved AccountHead details";
                    resultDto.ObjectId = -1;

                }
            }
            catch (Exception ex)
            {
                resultDto.Message = "Service layer error occured while moved the AccountHead details";
                resultDto.ObjectId = -98;
            }
            return resultDto;
        }

        public List<SelectListDto> GetAHCodeByFederationVal()
        {
            List<SelectListDto> lstSelectListDto = new List<SelectListDto>();
            List<uspAccountGetAHCODEByFederation_Result> lstuspAccountGetAHCODEByFederation_Result = _dbContext.uspAccountGetAHCODEByFederation().ToList();
            foreach (var ahcode in lstuspAccountGetAHCODEByFederation_Result)
            {
                SelectListDto objSelectListDto = new SelectListDto()
                {
                    ID = ahcode.AHID,
                    Text = ahcode.AHCode
                };
                lstSelectListDto.Add(objSelectListDto);
            }
            return lstSelectListDto;
        }
        public List<SelectListDto> GetAHCodeByIsMemberTransaction()
        {
            List<SelectListDto> lstSelectListDto = new List<SelectListDto>();
            List<uspAccountGetAHCODEByIsmemberTransaction_Result> lstuspAccountGetAHCODEByIsmemberTransaction_Result = _dbContext.uspAccountGetAHCODEByIsmemberTransaction().ToList();
            foreach (var ahcode in lstuspAccountGetAHCODEByIsmemberTransaction_Result)
            {
                SelectListDto objSelectListDto = new SelectListDto()
                {
                    ID = ahcode.AHID,
                    Text = ahcode.AHCode
                };
                lstSelectListDto.Add(objSelectListDto);
            }
            return lstSelectListDto;
        }
        public List<SelectListDto> GetGeneralReceiptLedgersDropDown(bool isFederation)
        {

            List<AccountHead> accountHeads = _dbContext.AccountHeads.ToList().FindAll(l => l.AHLevel > 4 && l.IsFederation == isFederation).OrderBy(o => o.AHCode).ToList();
            List<SelectListDto> ddlAHs = new List<SelectListDto>();
            if (accountHeads != null)
            {

                foreach (var ah in accountHeads)
                {
                    ddlAHs.Add(new SelectListDto() { ID = ah.AHID, Text = ah.AHCode });
                }
            }
            return ddlAHs;
        }

        public List<SelectListDto> GetGroupPenelLedgersDropDown(bool isFederation)
        {
            List<AccountHead> accountHeads = _dbContext.AccountHeads.ToList().FindAll(l => l.AHLevel > 4 && l.IsFederation == isFederation).OrderBy(o => o.AHCode).ToList();
            List<SelectListDto> ddlAHs = new List<SelectListDto>();
            foreach (var ah in accountHeads)
            {
                ddlAHs.Add(new SelectListDto() { ID = ah.AHID, Text = ah.AHCode });
            }
            return ddlAHs;
        }
        public AccountHeadDto GetCashInHandAccount(bool isFederation)
        {
            RefMaster rm = _dbContext.RefMasters.ToList().Find(r => r.RefMasterCode.ToUpper() == "AHTYPE");
            RefValueMaster rvm = _dbContext.RefValueMasters.ToList().Find(f => f.RefMasterID == rm.RefMasterID && f.RefCode.ToUpper() == "ASSETS");

            List<AccountHead> lstAccountHeadDtos = _dbContext.AccountHeads.ToList();

            #region recent oldcode 
            /*
            var ahlevel1 = lstAccountHeadDtos.FindAll(l => l.ParentAHID == 0 || l.ParentAHID == null);
            var assetparent = ahlevel1.Find(l => l.AHCode.ToUpper() == "ASSETS");
            var ahlevl2 = lstAccountHeadDtos.Find(l => l.ParentAHID == assetparent.AHID && l.AHCode.ToUpper() == "CLOSING BALANCES");
            var ahlevl3 = lstAccountHeadDtos.Find(l => l.ParentAHID == ahlevl2.AHID && l.AHCode.ToUpper() == "CASH AND BANK");
            var ahlevel4 = lstAccountHeadDtos.Find(l => l.ParentAHID == ahlevl3.AHID && l.AHCode.ToUpper() == "CASH IN HAND" && isFederation);
            var a = lstAccountHeadDtos.Find(l => l.ParentAHID == ahlevel4.AHID && l.AHCode.ToUpper() == "CASH IN HAND" && l.IsFederation == isFederation);
            */
            #endregion recent oldcode

            var CashinhandValueFromSettings= _dbContext.SystemSettings.ToList().Find(l => l.SettingName == "GROUP_CASHINHAND");
            var CashinHandAccountHead = lstAccountHeadDtos.Find(l => l.AHID == Convert.ToInt32(CashinhandValueFromSettings.SettingValue));
              
            return new AccountHeadDto() { AHCode = CashinHandAccountHead.AHCode, AHID = CashinHandAccountHead.AHID, AHLevel = CashinHandAccountHead.AHLevel, AHType = CashinHandAccountHead.AHType, AHName = CashinHandAccountHead.AHName, IsFederation = CashinHandAccountHead.IsFederation, ParentAHID = CashinHandAccountHead.ParentAHID.HasValue ? CashinHandAccountHead.ParentAHID.Value : 0 };
        }

        public List<AccountHeadDto> GetReceiptSavingAccountHeads(bool isFederation)
        {
            List<AccountHeadDto> lstSavingHeads = new List<AccountHeadDto>();
            List<AccountHead> lstHeads = new List<AccountHead>();
            RefMaster rm = _dbContext.RefMasters.ToList().Find(r => r.RefMasterCode.ToUpper() == "AHTYPE");
            RefValueMaster rvm = _dbContext.RefValueMasters.ToList().Find(f => f.RefMasterID == rm.RefMasterID && f.RefCode.ToUpper() == "LIABILITIES");
            var liabilities = _dbContext.AccountHeads.ToList().FindAll(f => f.AHType == rvm.RefID);
            var savings = liabilities.Find(l => l.AHCode.ToUpper() == "DEPOSITS");
            var result = from l2 in liabilities
                         from l3 in liabilities
                         from l4 in liabilities
                         where l2.AHID == l3.ParentAHID && l3.AHID == l4.ParentAHID
                         && l4.AHLevel == 5
                         select l4;

            var result2 = from l2 in liabilities
                          from l3 in liabilities
                          from l4 in liabilities
                          from l5 in liabilities
                          where l2.AHID == l3.ParentAHID && l3.AHID == l4.ParentAHID && l4.AHID == l5.ParentAHID
                          && l5.AHLevel == 5
                          select l5;
            lstHeads.AddRange(result);
            lstHeads.AddRange(result2);

            foreach (var item in lstHeads)
            {
                AccountHeadDto dto = new AccountHeadDto() { AHID = item.AHID, AHCode = item.AHCode, AHName = item.AHName };
                lstSavingHeads.Add(dto);
            }

            return lstSavingHeads;

        }

        public bool CheckAccountHeadCode(string ahcode, bool isFedaration, int ahid)
        {
            var accountheads = _dbContext.AccountHeads.ToList().FindAll(f => f.AHCode.ToUpper() == ahcode.ToUpper() && f.IsFederation == isFedaration && f.StatusID == 1 && f.AHID != ahid);
            if (accountheads.Count > 0)
                return false;
            else
                return true;
        }

        public List<ReceiptTranscationDto> GetReceiptAccountHeads(bool isFederation)
        {
            RefMaster rm = _dbContext.RefMasters.ToList().Find(r => r.RefMasterCode.ToUpper() == "AHTYPE");
            RefValueMaster rvm = _dbContext.RefValueMasters.ToList().Find(f => f.RefMasterID == rm.RefMasterID && f.RefCode.ToUpper() == Enums.AccountTypes.EXPENDITURE.ToString());

            List<AccountHead> accountheads = _dbContext.AccountHeads.ToList();
            var accountHeads = _dbContext.AccountHeads.ToList().FindAll(f => f.AHLevel > 4 && f.IsMemberTransaction && f.IsFederation == isFederation && f.AHType != rvm.RefID);
            List<ReceiptTranscationDto> accountHeadsDto = new List<ReceiptTranscationDto>();

            var raps = _dbContext.ReceiptAppropriationPriorities.ToList().FindAll(f => f.StatusId == 1 && f.IsGroup != isFederation);
            var result = from ah in accountHeads
                         from rap in raps
                         where rap.AHID == ah.AHID
                         select new { ah.AHID, ah.AHCode, ah.AHName, ah.OpeningBalance, rap.Priority };
            var result2 = result.OrderBy(o => o.Priority);



            foreach (var Ac in result2)
            {
                accountHeadsDto.Add(new ReceiptTranscationDto()
                {
                    AHID = Ac.AHID,
                    AHCode = Ac.AHCode,
                    AHName = Ac.AHName,
                    OpeningBalance = Convert.ToDecimal(Ac.OpeningBalance)
                });
            }

            return accountHeadsDto;
        }

        public bool UpdateOB(int ahid, int groupid, decimal balance)
        {
            return _accountHeadDll.UpdateOB(ahid, groupid, balance);
        }

        public AccountHeadDto GetOB(int ahid, int groupid)
        {
            return _accountHeadDll.GetOB(ahid, groupid);
        }
        public List<SelectListDto> GetSlAccountsGetByParentAhID(int Parentahid, int? groupId)
        {
            List<SelectListDto> lstSelectListDto = new List<SelectListDto>();
            List<uspslaccountsgetbyparentahid_Result> lstuspslaccountsgetbyparentahid_Result = _dbContext.uspslaccountsgetbyparentahid(Parentahid, groupId).ToList();
            foreach (var ahcode in lstuspslaccountsgetbyparentahid_Result)
            {
                lstSelectListDto.Add(new SelectListDto()
                {
                    ID = (int)ahcode.AHID,
                    Text = ahcode.MemberName
                });
            }
            return lstSelectListDto;
        }
        public List<int> GetBankAHIDs()
        {
            AccountHeadDll ahdll = new AccountHeadDll();
            return ahdll.GetBankAHIDs();
        }

        public List<int> GetOrganizationBanks()
        {
            AccountHeadDll ahdll = new AccountHeadDll();
            return ahdll.GetOrganizationBanks();
        }
        public List<AccountHeadDto> GetGroupAccountTree(int groupId)
        {
            AccountHeadDll ahdll = new AccountHeadDll();
            return ahdll.GetGroupAccountTree(groupId);
        }

        public List<SelectListDto> GetGeneralLedgerAccountHeads(int? groupId)
        {
            AccountHeadDll ahdll = new AccountHeadDll();
            return ahdll.GetGeneralLedgerAccountHeads(groupId);
        }
    }
}
