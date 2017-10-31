using BusinessLogic.AutoMapper;
//using BusinessLogic.Interface;
using MFIEntityFrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BusinessEntities;
using System.Data.Entity.Core.Objects;
using Utilities;
using CoreComponents;

namespace BusinessLogic
{
    public class FederationMeetingService 
    {
        private readonly MFISDBContext _dbContext;

        public FederationMeetingService()
        {
            _dbContext = new MFISDBContext();
            AutoMapperEntityConfiguration.Configure();
        }
        public List<MeetingMembersDTO> getLeders()
        {
            var leaders = _dbContext.uspmeetingLeaders().ToList();
            List<MeetingMembersDTO> lstmeeting = new List<MeetingMembersDTO>();
            foreach (var ldr in leaders)
            {
                lstmeeting.Add(new MeetingMembersDTO()
                {
                    MemberName = ldr.EmployeeName

                });
            }
            return lstmeeting;
        }
        public int InserFederationMeetig(FederationMeetingDTO obj, List<MeetingMembersDTO> lstmembers)
        {

            int suceess = 0;

            string bankxml = CommonMethods.SerializeListDto<List<MeetingMembersDTO>>(lstmembers);

            return suceess;


        }
        public ResultDto Insert(FederationMeetingDTO federation)
        {
            return InsertUpdateFederation(federation);
        }

        public ResultDto Update(FederationMeetingDTO federation)
        {
            return InsertUpdateFederation(federation);
        }
        private ResultDto InsertUpdateFederation(FederationMeetingDTO federation)
        {
            ResultDto resultDto = new ResultDto();
            string obectName = "Federation Meeting";
            try
            {
                string FederationMeetingmember = CommonMethods.SerializeListDto<List<MeetingMembersDTO>>(federation.lstFederationMemberDto);
                ObjectParameter paramfederationMeetingId = new ObjectParameter("FederationMeetingId", federation.FederationMeetingId);

                int count = _dbContext.uspFederationMeetingInsertUpdate(paramfederationMeetingId, federation.MeetingDate, federation.StartTime, federation.EndTime, federation.TransactionDate
                                            , federation.IsConducted, federation.OtherReason, federation.Reason, federation.IsSplMeeting
                                            , federation.MeetingObjective, federation.MeetingComments, federation.UserId, FederationMeetingmember);

                resultDto.ObjectId = (int)paramfederationMeetingId.Value;

                if (resultDto.ObjectId > 0)
                    resultDto.Message = string.Format("{0} details saved successfully", obectName);
                else if (resultDto.ObjectId == -1)
                    resultDto.Message = string.Format("Error occured while generating {0} code", obectName);
                else
                    resultDto.Message = string.Format("Error occured while saving {0} details", obectName);
            }
            catch (Exception)
            {
                resultDto.Message = string.Format("Service layer error occured while saving the {0} details", obectName);
                resultDto.ObjectId = -98;
            }
            return resultDto;
        }


        public ResultDto Delete(int FedMeetingId, int userId)
        {
            ResultDto resultDto = new ResultDto();
            string obectName = "FederationMeeting";

            try
            {
                ObjectParameter prmFederationmeetingId = new ObjectParameter("FederationMeetingId", FedMeetingId);

                int effectedCount = _dbContext.uspFederationMeetingDelete(prmFederationmeetingId, userId);

                resultDto.ObjectId = (int)prmFederationmeetingId.Value;

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

        public ResultDto ChangeStatus(int FedMeetingId, int userId)
        {
            ResultDto resultDto = new ResultDto();
            string obectName = "FederationMeeting";
            string statusCode = string.Empty;
            try
            {
                ObjectParameter prmFederationMeetingId = new ObjectParameter("FederationMeetingId", FedMeetingId);
                ObjectParameter prmStatusCode = new ObjectParameter("StatusCode", string.Empty);

                int effectedCount = _dbContext.uspFederationMeetingChangeStatus(prmFederationMeetingId, prmStatusCode, userId);

                resultDto.ObjectId = (int)prmFederationMeetingId.Value;
                statusCode = (string)prmStatusCode.Value;

                if (resultDto.ObjectId > 0)
                    resultDto.Message = string.Format("{0} : {1} details successfully", obectName, statusCode == Constants.StatusCodes.Active ? "activated" : "inactivated");
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

        public FederationMeetingDTO GetByID(int FedMeetingId)
        {
            List<uspFederationMeetingGetByFederationMeetingID_Result> lstuspFederationMeetingGetByFederationMeetingID_Result = _dbContext.uspFederationMeetingGetByFederationMeetingID(FedMeetingId).ToList();
            FederationMeetingDTO fedmeetingdto = new FederationMeetingDTO();
            fedmeetingdto = Mapper.Map<uspFederationMeetingGetByFederationMeetingID_Result, FederationMeetingDTO>(lstuspFederationMeetingGetByFederationMeetingID_Result.FirstOrDefault());

            List<MeetingMembersDTO> lstmemberdto = new List<MeetingMembersDTO>();
            List<uspFederationMeetingmembersGetById_Result> lstuspFederationMeetingmembersGetById_Result = _dbContext.uspFederationMeetingmembersGetById(FedMeetingId).ToList();
            // lstmemberdto = Mapper.Map<List<uspFederationMeetingmembersGetById_Result>, List<MeetingMembersDTO>>(lstuspFederationMeetingmembersGetById_Result.ToList());
            foreach (var item in lstuspFederationMeetingmembersGetById_Result)
            {
                MeetingMembersDTO objmemberdto = new MeetingMembersDTO();
                objmemberdto.MemberID = item.MemberID;
                objmemberdto.MemberName = item.MemberName;
                //  objmemberdto.FederationMeetingMemberID = item.FederationMeetingMemberID;
                objmemberdto.IsAttended = Convert.ToBoolean(item.IsAttended);
                lstmemberdto.Add(objmemberdto);

            }
            fedmeetingdto.lstFederationMemberDto = lstmemberdto;
            return fedmeetingdto;
        }


        public List<FederationMeetingLookupDto> Lookup()
        {
            List<FederationMeetingLookupDto> lstfedmeetingLookupDto = new List<FederationMeetingLookupDto>();
            List<uspFederationMeetingLookup_Result> lstuspfedMeetingLookup_Result = _dbContext.uspFederationMeetingLookup().ToList();
            foreach (var fedMeetings in lstuspfedMeetingLookup_Result)
            {
                FederationMeetingLookupDto fedMeetingDto = Mapper.Map<uspFederationMeetingLookup_Result, FederationMeetingLookupDto>(fedMeetings);
                fedMeetingDto.IsSpecialMeeting = fedMeetings.IsSplMeeting.HasValue ? fedMeetings.IsSplMeeting.Value : false;
                lstfedmeetingLookupDto.Add(fedMeetingDto);
            }
            return lstfedmeetingLookupDto;
        }

        public ResultDto Lock(int FedMeetingId)
        {
            ResultDto resultDto = new ResultDto();
            string objectName = "FederationMeeting";
            int userid = 1;
            try
            {
                ObjectParameter prmFederationmeetingId = new ObjectParameter("FederationMeetingId", FedMeetingId);
                ObjectParameter prmislocked = new ObjectParameter("IsLocked", false);

                int effectedCount = _dbContext.uspFederationMeetingLockedById(prmFederationmeetingId, prmislocked, userid);

                resultDto.ObjectId = (int)prmFederationmeetingId.Value;
                resultDto.Result = (bool)prmislocked.Value;

                if (resultDto.ObjectId > 0)
                    resultDto.Message = string.Format("{0}  details Lock successfully", objectName);
                else
                    resultDto.Message = string.Format("Error occured while Lock {0} details", objectName);
            }
            catch (Exception)
            {
                resultDto.Message = string.Format("Service layer error occured while Lock the {0} details", objectName);
                resultDto.ObjectId = -98;

            }
            return resultDto;
        }
    }
}
