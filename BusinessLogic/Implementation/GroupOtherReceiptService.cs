using DataLogic.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntities;

namespace BusinessLogic.Implementation
{
    public class GroupOtherReceiptService
    {
        #region Global VAriables
        private readonly GroupOtherReceiptDll _groupOtherReceiptDal;
        public GroupOtherReceiptService()
        {
            _groupOtherReceiptDal = new GroupOtherReceiptDll();
        }
        #endregion Global Variables

        public List<SelectListDto> GetSLAccountHeads(int GLAHId)
        {
            List<SelectListDto> lstdto = _groupOtherReceiptDal.GetSLAccountHeads(GLAHId);

            return lstdto;
        }

        public ResultDto Insert(GroupOtherRecieptDto objDto, int GroupId, bool isContra)
        {
            ResultDto resultDto = new ResultDto();

            return _groupOtherReceiptDal.InsertUpdateOtherReciept(objDto, GroupId, isContra);
        }
        public List<GroupMeetingDto> GetGroupOpenMeetingDates(int GroupId)
        {
            List<GroupMeetingDto> lstgroupMeetings = _groupOtherReceiptDal.GetGroupOpenMeetingDates(GroupId);

            return lstgroupMeetings;
        }
        public List<EmployeeDto> GetEmployees()
        {
            List<EmployeeDto> lstempdto = new List<EmployeeDto>();
            GroupOtherReceiptDll obj = new GroupOtherReceiptDll();
            lstempdto = obj.GetEmployees();
            return lstempdto;

        }

        public List<GroupOtherReceiptLookUpDto> GroupOtherReceiptLookUp(int UserId, int GroupId)
        {
            List<GroupOtherReceiptLookUpDto> groupOtherReceiptDto = _groupOtherReceiptDal.GroupOtherReceiptLookUp(UserId, GroupId);

            return groupOtherReceiptDto;
        }
        public GroupOtherRecieptDto EditGroupOtherReceipt(int AccountMasterId)
        {
            GroupOtherRecieptDto dto = _groupOtherReceiptDal.EditGroupOtherReceipt(AccountMasterId);
            return dto;
        }

        public ResultDto DeleteGroupOtherReceipt(int AccountMasterId, int UserId)
        {
            ResultDto resultDto = _groupOtherReceiptDal.DeleteGroupOtherReceipt(AccountMasterId, UserId);
            return resultDto;
        }

        public GroupOtherReceiptViewDto GroupOtherReceiptView(int AccountMasterId)
        {
            GroupOtherReceiptViewDto viewDto = _groupOtherReceiptDal.GroupOtherReceiptView(AccountMasterId);
            return viewDto;
        }

        public GroupOtherRecieptUploadValidateInfo GetGroupOtherRecieptValidateInfo(int groupId)
        {
            return _groupOtherReceiptDal.GetGroupOtherRecieptValidateInfo(groupId);
        }
    }
}
