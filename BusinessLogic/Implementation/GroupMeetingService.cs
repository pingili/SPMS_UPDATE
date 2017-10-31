using AutoMapper;
using BusinessEntities;
using BusinessLogic.AutoMapper;
//using BusinessLogic.Interface;
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
    public class GroupMeetingService
    {
        #region Gobal Variables

        private readonly MFISDBContext _dbContext;
        private readonly CommonService _commonService;

        public GroupMeetingService()
        {
            _dbContext = new MFISDBContext();

            _commonService = new CommonService();

            AutoMapperEntityConfiguration.Configure();
        }

        #endregion


        public ResultDto Insert(GroupMeetingDto Groupmeeting)
        {
            return InsertUpdateGroupMeeting(Groupmeeting);
        }

        public ResultDto Update(GroupMeetingDto Groupmeeting)
        {
            return InsertUpdateGroupMeeting(Groupmeeting);
        }
        private ResultDto InsertUpdateGroupMeeting(GroupMeetingDto Groupmeeting)
        {
            GroupMeetingDAL groupMeetingDAL = new GroupMeetingDAL();
            ResultDto resultdto = new ResultDto();
            string obectName = "Group Meeting";
            string GroupMeetingmember = CommonMethods.SerializeListDto<List<GroupMeetingMembersDto>>(Groupmeeting.lstgroupMembersDto);
            resultdto = groupMeetingDAL.InsertUpdateGroupMeeting(Groupmeeting, GroupMeetingmember);
            return resultdto;
        }

        public ResultDto Delete(int GroupMeetingId, int userId)
        {
            ResultDto resultDto = new ResultDto();
            string obectName = "GroupMeeting";

            try
            {
                ObjectParameter prmgroupmeetingId = new ObjectParameter("GroupMeetingId", GroupMeetingId);

                int effectedCount = _dbContext.uspGroupMeetingDelete(prmgroupmeetingId, userId);

                resultDto.ObjectId = (int)prmgroupmeetingId.Value;

                if (resultDto.ObjectId > 0)
                    resultDto.Message = string.Format("{0}  details deleted successfully", obectName);
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

        public ResultDto ChangeStatus(int GroupMeetingId, int userId)
        {
            ResultDto resultDto = new ResultDto();
            string obectName = "GroupMeeting";
            string statusCode = string.Empty;
            try
            {
                ObjectParameter prmGroupMeetingId = new ObjectParameter("GroupMeetingId", GroupMeetingId);
                ObjectParameter prmStatusCode = new ObjectParameter("StatusCode", string.Empty);

                int effectedCount = _dbContext.uspGroupMeetingChangeStatus(prmGroupMeetingId, prmStatusCode, userId);

                resultDto.ObjectId = (int)prmGroupMeetingId.Value;
                statusCode = (string)prmStatusCode.Value;

                if (resultDto.ObjectId > 0)  //"{0} : {1}details {2} successfully"
                    resultDto.Message = string.Format("{0} details {1} successfully", obectName, statusCode == Constants.StatusCodes.Active ? "activated" : "inactivated");
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

        public List<GroupMeetingLookupDto> Lookup(int Groupid)
        {
            GroupMeetingDAL dal = new GroupMeetingDAL();
            return dal.Lookup(Groupid);
        }

        public GroupMeetingDto GetByID(int groupMeetingID)
        {
            var objMeetingInfo = new GroupMeetingDAL().GetGroupMeetingDetailsByID(groupMeetingID);
            return objMeetingInfo;

            //List<uspGroupMeetingGetById_Result> lstuspMemberGetViewByID_Result = _dbContext.uspGroupMeetingGetById(GrouMeetingID).ToList();
            //GroupMeetingDto groupmeetingdto = new GroupMeetingDto();
            //var GroupMeetingDtoresult = Mapper.Map<uspGroupMeetingGetById_Result, GroupMeetingDto>(lstuspMemberGetViewByID_Result.FirstOrDefault());

            //List<GroupMeetingMembersDTO> lstmemberdto = new List<GroupMeetingMembersDTO>();
            //List<uspGroupMeetingmembersGetById_Result> lstuspGroupMeetingmembersGetById_Result = _dbContext.uspGroupMeetingmembersGetById(GrouMeetingID).ToList();
            //List<GroupMeetingMembersDTO> lstmemberKycDto = new List<GroupMeetingMembersDTO>();
            //foreach (var gmr in lstuspGroupMeetingmembersGetById_Result)
            //{
            //    GroupMeetingMembersDTO groupMeetingMembersDTO = Mapper.Map<uspGroupMeetingmembersGetById_Result, GroupMeetingMembersDTO>(gmr);
            //    lstmemberKycDto.Add(groupMeetingMembersDTO);
            //}

            //foreach (var item in lstmemberKycDto)
            //{
            //    GroupMeetingMembersDTO objmemberdto = new GroupMeetingMembersDTO();
            //    objmemberdto.MemberID = item.MemberID;
            //    objmemberdto.MemberName = item.MemberName;
            //    objmemberdto.GroupMeetingMemberID = item.GroupMeetingMemberID;
            //    objmemberdto.IsAttended = item.IsAttended;
            //    lstmemberdto.Add(objmemberdto);

            //}
            //GroupMeetingDtoresult.lstgroupMembersDto = lstmemberdto;
            //return GroupMeetingDtoresult;
        }

        public List<GroupMeetingViewDto> GetGroupMeetingsView(int groupMeetingId)
        {
            return new GroupMeetingDAL().GetGroupMeetingsView(groupMeetingId);
        }
    }
}
