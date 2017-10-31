using BusinessEntities;
using CoreComponents;
using DataLogic.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Implementation
{  
    public class GroupJournalService
    {
        #region Global Variable
        private readonly GroupJournalDal _groupOtherJournalDal;
        public GroupJournalService()
        {
            _groupOtherJournalDal = new GroupJournalDal();
        }
        #endregion Global Variable

        public ResultDto AddGroupOtherJournal(GroupJournalDto grpotrjrnldto, int groupId, int UserId)
        {
            ResultDto resultDto = new ResultDto();
            string groupOtherTranxml = CommonMethods.SerializeListDto<List<GroupJournalTranDto>>(grpotrjrnldto.TransactionsList);
            resultDto = new GroupJournalDal().AddGroupOtherJournal(groupOtherTranxml, grpotrjrnldto, groupId, UserId);
            return resultDto;
        }

        public List<SelectListDto> GetSLAccountHeads(int GLAHId)
        {
            List<SelectListDto> lstdto = _groupOtherJournalDal.GetSLAccountHeads(GLAHId);
            return lstdto;
        }

        public GroupJournalDto GetGroupJournalDetailsByID(int accountMasterId)
        {
            GroupJournalDal objDal = new GroupJournalDal();
            return objDal.GetGroupJournalDetailsByID(accountMasterId);
        }

        public List<GroupJournalLookUpDto> GetGroupJournalLookup(int groupId,int userId, bool isMemberJournal)
        {
            GroupJournalDal objDal = new GroupJournalDal();
            return objDal.GetGroupJournalLookup(groupId,userId, isMemberJournal);
        }
    }
}
