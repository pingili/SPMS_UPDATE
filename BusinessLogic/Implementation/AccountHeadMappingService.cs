using BusinessEntities;
using DataLogic.Implementation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Implementation
{
    public class AccountHeadMappingService
    {
        private readonly AccountHeadMappingDll _accountheadmappingDll;
        public AccountHeadMappingService()
        {
            _accountheadmappingDll = new AccountHeadMappingDll();
        }
        public ResultDto InsertAccountHeadMappnig(DataTable dt)
        {
            ResultDto objResult = _accountheadmappingDll.AccountHeadMappnig(dt);
            return objResult;
        }
        public List<AccountheadMappingDto> GetMappingAccountHeads()
        {
            return _accountheadmappingDll.GetAllAccountHead();
        }
    }
}
