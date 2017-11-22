using BusinessEntities;
using CoreComponents;
using DataLogic.Implementation;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace BusinessLogic.Implementation
{
    public class MemberConfirmationDemandService
    {
        public List<MemberConfirmationDto> GetMemberConfirmationReport(int GroupId, int UserId, DateTime dtTranDate)
        {
            return objDal.GetMemberConfirmationReport(GroupId, UserId, dtTranDate);
        }

        public List<DateTime> GetGroupMeetings(int groupID)
        {
            return objDal.GetGroupMeetings(groupID);
        }

        public List<MemberDemandSheetDto> GetMemberDemandSheetReport(int GroupId, int UserId, DateTime dtTranDate)
        {
            return objDal.GetMemberDemandSheetReport(GroupId, UserId, dtTranDate);
        }


        public MemberConfirmationDemandDal objDal
        {
            get
            {
                return new MemberConfirmationDemandDal();
            }
        }
    }

 
}
