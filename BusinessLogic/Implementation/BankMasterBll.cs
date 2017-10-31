using BusinessEntities;
using DataLogic.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Implementation
{
    public class BankMasterBll
    {
        public List<BankMasterDto> GetGroupBanks(int groupID)
        {
            BankMasterDal obj = new BankMasterDal();
            return obj.GetGroupBanks(groupID);
        }
    }
}
