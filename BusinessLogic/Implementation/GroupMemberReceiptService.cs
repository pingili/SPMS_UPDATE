using BusinessEntities;
using CoreComponents;
using DataLogic.Implementation;
using MFIEntityFrameWork;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Implementation
{
    public class GroupMemberReceiptService
    {
        #region Global VAriables
        private readonly GroupMemberReceiptDal _groupMemberReceiptDal;
        private readonly GroupMemberReceiptDto _groupMemberReceiptDto;
        private readonly MFISDBContext _dbContext;
        public GroupMemberReceiptService()
        {
            _groupMemberReceiptDal = new GroupMemberReceiptDal();
            _groupMemberReceiptDto = new GroupMemberReceiptDto();
            _dbContext = new MFISDBContext();
        }
        #endregion Global Variables

        public ResultDto AddUpdateMemberReceipt(GroupMemberReceiptDto _groupMemberReceiptDto, int userId, int groupId)
        {
            GroupMemberReceiptDal objDal = new GroupMemberReceiptDal();
            string memberReceiptTranxml = CommonMethods.SerializeListDto<List<GroupMemberReceiptTranDto>>(_groupMemberReceiptDto.Transactions);

            return objDal.AddUpdateMemberReceipt(_groupMemberReceiptDto, memberReceiptTranxml, userId, groupId);
        }

        public List<GroupMeetingDto> MeetingDates(int GroupId)
        {
            List<GroupMeetingDto> lstgroupMeetings = _groupMemberReceiptDal.GetMeetingDates(GroupId);

            return lstgroupMeetings;
        }
        public List<EmployeeDto> GetEmployees()
        {
            List<EmployeeDto> lstempdto = new List<EmployeeDto>();
            lstempdto = _groupMemberReceiptDal.GetEmployees();
            return lstempdto;

        }
        public List<MemberDto> GetMembersByGroupId(int GroupId)
        {
            List<MemberDto> lstMembers = _groupMemberReceiptDal.GetMembersByGroupId(GroupId);
            return lstMembers;
        }
        public List<GroupMemberReceiptTranDto> GetMemberReceiptTemplate()
        {
            List<GroupMemberReceiptTranDto> lstAccountHeads = _groupMemberReceiptDal.GetMemberReceiptTemplate();
            return lstAccountHeads;

        }
        public List<GroupMemberReceiptLookupDto> GroupMemberReceiptLookUp(int groupId,int UserId)
        {
            return _groupMemberReceiptDal.GroupMemberReceiptLookUp(groupId,UserId);
        }
        public GroupMemberReceiptDto EditGroupMemberReceipt(int AccountMasterId)
        {
            GroupMemberReceiptDto groupMemberReceiptDto = _groupMemberReceiptDal.EditGroupMemberReceipt(AccountMasterId);
            return groupMemberReceiptDto;
        }
        public GroupMemberReceiptViewDto GetGroupMemberReceiptViewDetails(int AccountMasterId)
        {
            GroupMemberReceiptViewDto groupMemberReceiptDto = _groupMemberReceiptDal.GetGroupMemberReceiptViewDetails(AccountMasterId);
            return groupMemberReceiptDto;
        }
        public List<GroupMemberDemandDto> GetTransactionDemands(string TransactionDate, int MemberId)
        {
            List<GroupMemberDemandDto> lstDemands = _groupMemberReceiptDal.GetMemberReceiptDemand(MemberId, TransactionDate);
            return lstDemands;
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
    }
}
