using BlackBeltCoder;
using BusinessEntities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLogic.Implementation
{
    public class MemberConfirmationDemandDal
    {
        public List<MemberConfirmationDto> GetMemberConfirmationReport(int GroupId, int UserId, DateTime dtTranDate,DateTime meetingDate)
        {
            List<MemberConfirmationDto> lstMemberConfirmation = new List<MemberConfirmationDto>();
            
            AdoHelper objado = new AdoHelper();
            SqlParameter[] parms = new SqlParameter[dtTranDate != DateTime.MinValue ? 4 : 3];

            parms[0] = new SqlParameter("@GroupID", GroupId);
            parms[0].SqlDbType = System.Data.SqlDbType.Int;

            parms[1] = new SqlParameter("@UserId", UserId);
            parms[1].SqlDbType = System.Data.SqlDbType.Int;

            parms[2] = new SqlParameter("@MeetingDate", meetingDate);
            parms[2].SqlDbType = System.Data.SqlDbType.DateTime;
            if (dtTranDate != DateTime.MinValue)
            {
                parms[3] = new SqlParameter("@AsOnDate", dtTranDate);
                parms[3].SqlDbType = System.Data.SqlDbType.Int;
            }

            SqlDataReader dr = objado.ExecDataReaderProc("uspMemberConfirmationReport", parms);
            while (dr.Read())
            {
                MemberConfirmationDto obj = new MemberConfirmationDto();

                obj.MemberId = Convert.ToInt32(dr["MemberId"]);
                obj.MemberCode = Convert.ToString(dr["MemberCode"]);
                obj.MemberName = Convert.ToString(dr["MemberName"]);
                if (dr["PSPrincipal"] != DBNull.Value)
                    obj.PSPrincipal = Convert.ToInt32(dr["PSPrincipal"]);
                if (dr["PSInt"] != DBNull.Value)
                    obj.PSInt = Convert.ToInt32(dr["PSInt"]);

                if (dr["SSPrincipal"] != DBNull.Value)
                    obj.SSPrincipal = Convert.ToInt32(dr["SSPrincipal"]);
                if (dr["SSInt"] != DBNull.Value)
                    obj.SSInt = Convert.ToInt32(dr["SSInt"]);

                if (dr["SLoanDate"] != DBNull.Value)
                    obj.SLoanDate = Convert.ToDateTime(dr["SLoanDate"]);
                if (dr["SLoanPrincipal"] != DBNull.Value)
                    obj.SLoanPrincipal = Convert.ToInt32(dr["SLoanPrincipal"]);
                if (dr["SLoanInt"] != DBNull.Value)
                    obj.SLoanInt = Convert.ToInt32(dr["SLoanInt"]);

                if (dr["BLoanDate"] != DBNull.Value)
                    obj.BLoanDate = Convert.ToDateTime(dr["BLoanDate"]);
                if (dr["BLoanPrincipal"] != DBNull.Value)
                    obj.BLoanPrincipal = Convert.ToInt32(dr["BLoanPrincipal"]);
                if (dr["BLoanInt"] != DBNull.Value)
                    obj.BLoanInt = Convert.ToInt32(dr["BLoanInt"]);

                if (dr["HLoanDate"] != DBNull.Value)
                    obj.HLoanDate = Convert.ToDateTime(dr["HLoanDate"]);
                if (dr["HLoanPrincipal"] != DBNull.Value)
                    obj.HLoanPrincipal = Convert.ToInt32(dr["HLoanPrincipal"]);
                if (dr["HLoanInt"] != DBNull.Value)
                    obj.HLoanInt = Convert.ToInt32(dr["HLoanInt"]);


                lstMemberConfirmation.Add(obj);
            }

            return lstMemberConfirmation;
        }

        public List<DateTime> GetGroupMeetings(object groupId)
        {
            List<DateTime> lstGroupMeetings = new List<DateTime>();

            AdoHelper objado = new AdoHelper();
            SqlParameter[] parms = new SqlParameter[1];

            parms[0] = new SqlParameter("@GroupID", groupId);
            parms[0].SqlDbType = System.Data.SqlDbType.Int;

            SqlDataReader dr = objado.ExecDataReaderProc("uspGetGroupMeetings", parms);
            while (dr.Read())
            {
                DateTime meetingDate = new DateTime();
                if (dr["meetingDate"] != DBNull.Value)
                    meetingDate = Convert.ToDateTime(dr["meetingDate"]).Date;
                lstGroupMeetings.Add(meetingDate);
            }

            return lstGroupMeetings;
        }

        public List<MemberDemandSheetDto> GetMemberDemandSheetReport(int GroupId, int UserId, DateTime dtTranDate)
        {
            List<MemberDemandSheetDto> lstMemberDemandSheet = new List<MemberDemandSheetDto>();

            AdoHelper objado = new AdoHelper();
            SqlParameter[] parms = new SqlParameter[dtTranDate != DateTime.MinValue ? 3 : 2];

            parms[0] = new SqlParameter("@GroupID", GroupId);
            parms[0].SqlDbType = System.Data.SqlDbType.Int;

            parms[1] = new SqlParameter("@UserId", UserId);
            parms[1].SqlDbType = System.Data.SqlDbType.Int;

            if (dtTranDate != DateTime.MinValue)
            {
                parms[2] = new SqlParameter("@AsOnDate", dtTranDate);
                parms[2].SqlDbType = System.Data.SqlDbType.Int;
            }

            SqlDataReader dr = objado.ExecDataReaderProc("uspMemberDemandSheetReport", parms);
            while (dr.Read())
            {
                MemberDemandSheetDto obj = new MemberDemandSheetDto();

                obj.MemberId = Convert.ToInt32(dr["MemberId"]);
                obj.MemberCode = Convert.ToString(dr["MemberCode"]);
                obj.MemberName = Convert.ToString(dr["MemberName"]);
                if (dr["PSDemand"] != DBNull.Value)
                    obj.PSDemand = Convert.ToInt32(dr["PSDemand"]);
                if (dr["SSDemand"] != DBNull.Value)
                    obj.SSDemand = Convert.ToInt32(dr["SSDemand"]);

                if (dr["SLoanPrincipal"] != DBNull.Value)
                    obj.SLoanPrincipal = Convert.ToInt32(dr["SLoanPrincipal"]);
                if (dr["SLoanInt"] != DBNull.Value)
                    obj.SLoanInt = Convert.ToInt32(dr["SLoanInt"]);
                
                if (dr["BLoanPrincipal"] != DBNull.Value)
                    obj.BLoanPrincipal = Convert.ToInt32(dr["BLoanPrincipal"]);
                if (dr["BLoanInt"] != DBNull.Value)
                    obj.BLoanInt = Convert.ToInt32(dr["BLoanInt"]);
                
                if (dr["HLoanPrincipal"] != DBNull.Value)
                    obj.HLoanPrincipal = Convert.ToInt32(dr["HLoanPrincipal"]);
                if (dr["HLoanInt"] != DBNull.Value)
                    obj.HLoanInt = Convert.ToInt32(dr["HLoanInt"]);
                if (dr["TotalDemand"] != DBNull.Value)
                    obj.TotalDemand = Convert.ToInt32(dr["TotalDemand"]);

                lstMemberDemandSheet.Add(obj);
            }

            return lstMemberDemandSheet;
        }
    }
}
