using BlackBeltCoder;
using BusinessEntities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLogic.Implementation
{
    public class GroupMeetingDAL
    {
        public GroupMeetingDto GetDate(int groupId)
        {
            GroupMeetingDto grpMeetingDto = null;
            AdoHelper obj = new AdoHelper();
            SqlParameter[] parms = new SqlParameter[1];

            parms[0] = new SqlParameter("@GroupId", groupId);
            parms[0].SqlDbType = System.Data.SqlDbType.Int;

            SqlDataReader Dr = obj.ExecDataReaderProc("UspGroupCurrentMeeitngDate", parms);
            if (Dr.Read())
            {
                grpMeetingDto = new GroupMeetingDto();
                grpMeetingDto.Month = Convert.ToInt32(Dr["MeetingMonth"]);
                grpMeetingDto.Year = Convert.ToInt32(Dr["MeetingYear"]);
                grpMeetingDto.GroupMeetingDay = Convert.ToInt32(Dr["MeetingDay"]);
            }
            return grpMeetingDto;

        }
        public ResultDto InsertUpdateGroupMeeting(GroupMeetingDto Groupmeeting, string GroupMeetingMembers)
        {
            AdoHelper obj = new AdoHelper();
            ResultDto resultDto = new ResultDto();
            string obectName = "Group Meeting";
            try
            {
                SqlParameter[] parms = new SqlParameter[12];

                parms[0] = new SqlParameter("@GroupId", Groupmeeting.GroupID);
                parms[0].SqlDbType = System.Data.SqlDbType.Int;

                parms[1] = new SqlParameter("@MeetingObjective", Groupmeeting.MeetingObjective);
                parms[1].SqlDbType = System.Data.SqlDbType.VarChar;

                parms[2] = new SqlParameter("@MeetingComments", Groupmeeting.MeetingComments);
                parms[2].SqlDbType = System.Data.SqlDbType.VarChar;

                parms[3] = new SqlParameter("@MeetingDate", Groupmeeting.MeetingDate);
                parms[3].SqlDbType = System.Data.SqlDbType.Date;

                parms[4] = new SqlParameter("@TransactionDate", Groupmeeting.TransactionDate);
                parms[4].SqlDbType = System.Data.SqlDbType.Date;

                parms[5] = new SqlParameter("@IsConducted", Groupmeeting.IsConducted);
                parms[5].SqlDbType = System.Data.SqlDbType.Bit;

                parms[6] = new SqlParameter("@IsSplMeeting", Groupmeeting.IsSplMeeting);
                parms[6].SqlDbType = System.Data.SqlDbType.Bit;

                parms[7] = new SqlParameter("@Reason", Groupmeeting.Reason);
                parms[7].SqlDbType = System.Data.SqlDbType.VarChar;

                parms[8] = new SqlParameter("@OtherReason", Groupmeeting.OtherReason);
                parms[8].SqlDbType = System.Data.SqlDbType.VarChar;

                parms[9] = new SqlParameter("@UserId", Groupmeeting.UserId);
                parms[9].SqlDbType = System.Data.SqlDbType.Int;

                parms[10] = new SqlParameter("@GroupMeetingmember", GroupMeetingMembers);
                parms[10].SqlDbType = System.Data.SqlDbType.NVarChar;

                parms[11] = new SqlParameter("@GroupMeetingID", Groupmeeting.GroupMeetingID);
                parms[11].SqlDbType = System.Data.SqlDbType.Int;
                parms[11].Direction = ParameterDirection.InputOutput;

                obj.ExecNonQueryProc("UspGroupMeetingInsertUpdate", parms);
                resultDto.ObjectId = (int)parms[11].Value;

                if (resultDto.ObjectId > 0)
                    resultDto.Message = string.Format("{0}  details saved successfully", obectName);
                else if (resultDto.ObjectId == -99)
                    resultDto.Message = string.Format("Error occured while saving {0} details", obectName);
                else if (resultDto.ObjectId == -98)
                    resultDto.Message = string.Format("MEETING ALREADY CONDUCTED/NOT CONDUCTED BOTH TYPE OF ENTRIES NOT POSSIBLE", obectName);
                else if (resultDto.ObjectId == -97)
                    resultDto.Message = string.Format("MULTIPLE NOT CONDUCTED SHOULD NOT ALLOWED", obectName);
                else if (resultDto.ObjectId == -96)
                    resultDto.Message = string.Format("ALREADY MEETING EXISTIS WITH GIVEN DATE", obectName);
            }
            catch (Exception)
            {
                resultDto.Message = string.Format("Service layer error occured while saving the {0} details", obectName);
                resultDto.ObjectId = -98;
            }
            return resultDto;
        }
        public List<GroupMeetingDto> GetMeetingInfoByGroupID(int groupId)
        {
            GroupMeetingDto grpMeetingDto = null;
            List<GroupMeetingDto> lstGroup = new List<GroupMeetingDto>();
            AdoHelper obj = new AdoHelper();
            SqlParameter[] parms = new SqlParameter[1];

            parms[0] = new SqlParameter("@GroupId", groupId);
            parms[0].SqlDbType = System.Data.SqlDbType.Int;

            SqlDataReader Dr = obj.ExecDataReaderProc("UspGetGroupMeeitngInfo", parms);
            while (Dr.Read())
            {
                grpMeetingDto = new GroupMeetingDto();
                grpMeetingDto.GroupMeetingID = Convert.ToInt32(Dr["GroupMeetingID"]);
                grpMeetingDto.IsConducted = Convert.ToBoolean(Dr["IsConducted"]);
                grpMeetingDto.IsSplMeeting = Convert.ToBoolean(Dr["IsSplMeeting"]);
                grpMeetingDto.MeetingType = grpMeetingDto.IsSplMeeting == true ? "Special" : "Regular";
                grpMeetingDto.MeetingMemberCount = Convert.ToInt32(Dr["MemberCount"]);
                grpMeetingDto.MeetingDate = Convert.ToDateTime(Dr["MeetingDate"]);
                lstGroup.Add(grpMeetingDto);
            }
            return lstGroup;

        }
        public ResultDto Delete(int GroupMeetingId, int userId)
        {
            ResultDto resultDto = new ResultDto();
            string obectName = "GroupMeeting";

            try
            {
                AdoHelper obj = new AdoHelper();
                SqlParameter[] parms = new SqlParameter[2];

                parms[0] = new SqlParameter("@GroupMeetingId", GroupMeetingId);
                parms[0].SqlDbType = System.Data.SqlDbType.Int;
                parms[1] = new SqlParameter("@UserId", userId);
                parms[1].SqlDbType = System.Data.SqlDbType.Int;

                obj.ExecNonQueryProc("uspGroupMeetingDelete", parms);
                resultDto.ObjectId = (int)parms[1].Value;
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
        public List<GroupMeetingLookupDto> Lookup(int GroupId)
        {
            List<GroupMeetingLookupDto> lstLookupDto = new List<GroupMeetingLookupDto>();
            AdoHelper obj = new AdoHelper();
            SqlParameter[] parms = new SqlParameter[1];
            GroupMeetingLookupDto grpmeetingLookup = null;
            parms[0] = new SqlParameter("@GroupId", GroupId);
            parms[0].SqlDbType = System.Data.SqlDbType.Int;
            SqlDataReader Dr = obj.ExecDataReaderProc("uspGroupMeetingLookup", parms);
            while (Dr.Read())
            {
                grpmeetingLookup = new GroupMeetingLookupDto();
                grpmeetingLookup.MeetingDate = Convert.ToDateTime(Dr["MeetingDate"]);
                grpmeetingLookup.MeetingLockStatus = Convert.ToString(Dr["LockStatus"]);
                grpmeetingLookup.GroupMeetingID = Convert.ToInt32(Dr["GroupMeetingID"]);
                grpmeetingLookup.IsConducted = Convert.ToBoolean(Dr["IsConducted"]);
                grpmeetingLookup.IsSpecialMeeting = Convert.ToBoolean(Dr["IsSplMeeting"]);
                grpmeetingLookup.MembersCount = Convert.ToInt32(Dr["MemberCount"]);
                lstLookupDto.Add(grpmeetingLookup);
            }
            return lstLookupDto;
        }
        public void LockMeeting(int GroupId)
        {
            AdoHelper obj = new AdoHelper();
            SqlParameter[] parms = new SqlParameter[1];
            parms[0] = new SqlParameter("@GroupId", GroupId);
            parms[0].SqlDbType = System.Data.SqlDbType.Int;
            obj.ExecNonQueryProc("uspGroupMeetingLock", parms);

        }

        public GroupMasterDto GetGroupMasterDetailsByID(int groupID)
        {
            GroupMasterDto grp = new GroupMasterDto();
            AdoHelper obj = new AdoHelper();
            SqlParameter[] parms = new SqlParameter[1];

            parms[0] = new SqlParameter("@GroupId", groupID);
            parms[0].SqlDbType = System.Data.SqlDbType.Int;

            SqlDataReader dr = obj.ExecDataReaderProc("uspGroupGetByGroupId", parms);
            if (dr.Read())
            {
                grp.GroupID = Convert.ToInt32(dr["GroupID"]);
                grp.GroupCode = Convert.ToString(dr["GroupCode"]);
                grp.GroupRefNumber = Convert.ToString(dr["GroupRefNumber"]);
                grp.GroupName = Convert.ToString(dr["GroupName"]);
                grp.TEGroupName = Convert.ToString(dr["TEGroupName"]);
                grp.PanchayatID = Convert.ToInt32(dr["PanchayatID"]);
                grp.VillageID = Convert.ToInt32(dr["VillageID"]);
                grp.ClusterID = Convert.ToInt32(dr["ClusterID"]);

                grp.ClusterName = Convert.ToString(dr["ClusterName"]);
                grp.Village = Convert.ToString(dr["Village"]);
                if (dr["FormationDate"] != DBNull.Value)
                    grp.FormationDate = Convert.ToDateTime(dr["FormationDate"]);
                grp.Phone = Convert.ToString(dr["Phone"]);
                grp.Email = Convert.ToString(dr["Email"]);
                grp.Address = Convert.ToString(dr["Address"]);
                if (dr["MeetingFrequency"] != DBNull.Value)
                    grp.MeetingFrequency = Convert.ToInt32(dr["MeetingFrequency"]);
                if (dr["FederationTranStartDate"] != DBNull.Value)
                    grp.FederationTranStartDate = Convert.ToDateTime(dr["FederationTranStartDate"]);
                if (dr["DateOfClosure"] != DBNull.Value)
                    grp.DateOfClosure = Convert.ToDateTime(dr["DateOfClosure"]);
                if (dr["MeetingDay"] != DBNull.Value)
                    grp.MeetingDay = Convert.ToByte(dr["MeetingDay"]);
                if (dr["MeetingStartTime"] != DBNull.Value)
                    grp.MeetingStartTime = (TimeSpan)dr["MeetingStartTime"];
                if (dr["MeetingEndTime"] != DBNull.Value)
                    grp.MeetingEndTime = (TimeSpan)dr["MeetingEndTime"];
                if (dr["RegularSavingAmount"] != DBNull.Value)
                    grp.RegularSavingAmount = Convert.ToDecimal(dr["RegularSavingAmount"]);
                if (dr["RegularSavingsAhId"] != DBNull.Value)
                    grp.RegularSavingsAhId = Convert.ToInt32(dr["RegularSavingsAhId"]);
                grp.RegularSavingAccountHead = Convert.ToString(dr["RegularSavingAccountHead"]);
                if (dr["MeetingDate"] != DBNull.Value)
                    grp.MeetingDate = Convert.ToDateTime(dr["MeetingDate"]);
                grp.LockStatus = Convert.ToString(dr["LockStatus"]);
                grp.LockStatusCode = Convert.ToString(dr["LockStatusCode"]);
            }
            return grp;
        }

        public GroupMeetingDto GetGroupMeetingDetailsByID(int groupMeetingId)
        {
            GroupMeetingDto meetingInfo = new GroupMeetingDto();
            AdoHelper obj = new AdoHelper();
            SqlParameter[] parms = new SqlParameter[1];

            parms[0] = new SqlParameter("@GroupMeetingID", groupMeetingId);
            parms[0].SqlDbType = System.Data.SqlDbType.Int;

            SqlDataReader dr = obj.ExecDataReaderProc("uspGroupMeetingGetById", parms);
            if (dr.Read())
            {
                meetingInfo.GroupMeetingID = Convert.ToInt32(dr["GroupMeetingID"]);
                meetingInfo.GroupID = Convert.ToInt32(dr["GroupID"]);
                meetingInfo.MeetingComments = Convert.ToString(dr["MeetingComments"]);
                meetingInfo.MeetingObjective = Convert.ToString(dr["MeetingObjective"]);
                meetingInfo.MeetingDate = Convert.ToDateTime(dr["MeetingDate"]);
                if (dr["TransactionDate"] != DBNull.Value)
                    meetingInfo.TransactionDate = Convert.ToDateTime(dr["TransactionDate"]);
                meetingInfo.IsConducted = Convert.ToBoolean(dr["IsConducted"]);
                meetingInfo.IsSplMeeting = Convert.ToBoolean(dr["IsSplMeeting"]);
                if (dr["Reason"] != DBNull.Value)
                    meetingInfo.Reason = Convert.ToInt32(dr["Reason"]);
                meetingInfo.ReasonName = Convert.ToString(dr["ReasonName"]);
                meetingInfo.OtherReason = Convert.ToString(dr["OtherReason"]);
            }
            if (dr.NextResult())
            {
                meetingInfo.lstgroupMembersDto = new List<GroupMeetingMembersDto>();
                GroupMeetingMembersDto objMeetingMemberDto = null;
                while (dr.Read())
                {
                    objMeetingMemberDto = new GroupMeetingMembersDto();
                    objMeetingMemberDto.MemberID = Convert.ToInt32(dr["MemberID"]);
                    objMeetingMemberDto.IsAttended = Convert.ToBoolean(dr["IsAttended"]);
                    objMeetingMemberDto.MemberName = Convert.ToString(dr["MemberName"]);

                    meetingInfo.lstgroupMembersDto.Add(objMeetingMemberDto);
                }
            }
            return meetingInfo;
        }

        public List<GroupMeetingViewDto> GetGroupMeetingsView(int groupMeetingId)
        {
            var lstGroupMeetings = new List<GroupMeetingViewDto>();
            GroupMeetingViewDto objMeeting = null;
            AdoHelper obj = new AdoHelper();
            SqlParameter[] parms = new SqlParameter[1];

            parms[0] = new SqlParameter("@GroupMeetingID", groupMeetingId);
            parms[0].SqlDbType = System.Data.SqlDbType.Int;

            SqlDataReader dr = obj.ExecDataReaderProc("uspGetGroupMeetingsByMonth", parms);
            while (dr.Read())
            {
                objMeeting = new GroupMeetingViewDto();
                objMeeting.MeetingComments = Convert.ToString(dr["MeetingComments"]);
                objMeeting.MeetingObjective = Convert.ToString(dr["MeetingObjective"]);
                objMeeting.MeetingDate = Convert.ToDateTime(dr["MeetingDate"]);
                if (dr["TransactionDate"] != DBNull.Value)
                    objMeeting.TransactionDate = Convert.ToDateTime(dr["TransactionDate"]);
                objMeeting.IsConducted = Convert.ToBoolean(dr["IsConducted"]);
                objMeeting.IsSplMeeting = Convert.ToBoolean(dr["IsSplMeeting"]);
                objMeeting.ReasonName = Convert.ToString(dr["ReasonName"]);
                objMeeting.OtherReason = Convert.ToString(dr["OtherReason"]);
                objMeeting.Members = Convert.ToString(dr["Members"]);

                lstGroupMeetings.Add(objMeeting);
            }
            return lstGroupMeetings;
        }

        public void GroupMeeitngLock(int groupId, int userId,string type,int meetingId)
        {
            AdoHelper obj = new AdoHelper();
            SqlParameter[] parms = new SqlParameter[4];

            parms[0] = new SqlParameter("@GroupId", groupId);
            parms[0].SqlDbType = System.Data.SqlDbType.Int;

            parms[1] = new SqlParameter("@UserId", userId);
            parms[1].SqlDbType = System.Data.SqlDbType.Int;

            parms[2] = new SqlParameter("@type", type);
            parms[2].SqlDbType = System.Data.SqlDbType.VarChar;

            parms[3] = new SqlParameter("@meetingId", meetingId);
            parms[3].SqlDbType = System.Data.SqlDbType.Int;

            obj.ExecNonQueryProc("uspGroupMeetingLock", parms);

        }
    }
}
