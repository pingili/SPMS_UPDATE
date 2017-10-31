using AutoMapper;
using BusinessEntities;
using BusinessLogic.AutoMapper;
using DataLogic;
//using BusinessLogic.Interface;
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
    public class MemberService 
    {

        #region Gobal Variables

        private readonly MFISDBContext _dbContext;
        private readonly CommonService _commonService;

        public MemberService()
        {
            _dbContext = new MFISDBContext();

            _commonService = new CommonService();

            AutoMapperEntityConfiguration.Configure();
        }

        #endregion

        public ResultDto Insert(MemberDto memberDto)
        {
            return Insertupdate(memberDto);
        }

        public ResultDto Update(MemberDto memberDto)
        {
            return Insertupdate(memberDto);
        }
        public ResultDto Insertupdate(MemberDto memberDto)
        {
            ResultDto resultDto = new ResultDto();
            string obectName = "Member";
            try
            {
                ObjectParameter parammemberId = new ObjectParameter("MemberId", memberDto.MemberID);
                ObjectParameter parammembercode = new ObjectParameter("MemberCode", string.Empty);
                int count = _dbContext.uspMemberInsertUpdate(parammemberId, parammembercode, memberDto.MemberRefCode, memberDto.SurName
                    , memberDto.TESurName, memberDto.MemberName, memberDto.TEMemberName, memberDto.Photo, memberDto.GroupID
                    , memberDto.Gender, memberDto.DateOfAdmission, memberDto.EducationQualification, memberDto.MobileNumber
                    , memberDto.Email, memberDto.DOB, memberDto.OccupationID, memberDto.Disability, memberDto.MaritalStatus
                    , memberDto.UserID, memberDto.DateOfRetirement, memberDto.Religion);
                resultDto.ObjectId = (int)parammemberId.Value;
                resultDto.ObjectCode = string.IsNullOrEmpty((string)parammembercode.Value) ? memberDto.MemberCode : (string)parammembercode.Value;

                if (resultDto.ObjectId > 0)
                    resultDto.Message = string.Format("{0} details saved successfully with code : {1}", obectName, resultDto.ObjectCode);
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




        public ResultDto UpdateFamily(MemberDto memberDto)
        {
            ResultDto resultDto = new ResultDto();
            string obectName = "MemberFamilyDetails";
            // ObjectParameter paramID = new ObjectParameter("MemberID",memberDto.MemberID);

            try
            {
                _dbContext.uspMemberFamilyDetailsUpdate(memberDto.MemberID, memberDto.NomineeName, memberDto.NomineeRelationship, memberDto.ParentGuardianName, memberDto.ParentGuardianRelationship, memberDto.SocialCategory
                    , memberDto.MonthlyIncome, memberDto.EarningMembersInFamily, memberDto.NonEarningMembersInFamily, memberDto.IncomeFrequency, memberDto.InvestmentSize, memberDto.AssetsInNameOfMember, memberDto.UserID);
                resultDto.ObjectId = (int)memberDto.MemberID;

                if (resultDto.ObjectId > 0)
                    resultDto.Message = string.Format("{0} details saved successfully with code : {1}", obectName, memberDto.MemberCode);
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
        public MemberDto GetById(int MemberId)
        {
            List<uspMemberGetByMemberID_Result> lstuspMemberGetByMemberID_Result = _dbContext.uspMemberGetByMemberID(MemberId).ToList();
            var MemberDto = Mapper.Map<uspMemberGetByMemberID_Result, MemberDto>(lstuspMemberGetByMemberID_Result.FirstOrDefault());
            return MemberDto;
        }
        public List<MemberLookupDto> LookUp(int GroupId)
        {
            List<MemberLookupDto> lstmemberlookupDto = new List<MemberLookupDto>();
            List<uspMemberLookup_Result> lstuspMemberLookup_Result = _dbContext.uspMemberLookup(GroupId).ToList();
            foreach (var Member in lstuspMemberLookup_Result)
            {
                var MemberlookupDto = Mapper.Map<uspMemberLookup_Result, MemberLookupDto>(Member);
                lstmemberlookupDto.Add(MemberlookupDto);
            }
            return lstmemberlookupDto;
        }



        public List<MemberLookupDto> GetByGroupId(int GroupId)
        {
            List<MemberLookupDto> lstmemberlookupDto = new List<MemberLookupDto>();
            List<uspMemberByGroupId_Result> lstuspMemberByGroupId_Result = _dbContext.uspMemberByGroupId(GroupId).ToList();
            foreach (var Member in lstuspMemberByGroupId_Result)
            {
                var MemberlookupDto = Mapper.Map<uspMemberByGroupId_Result, MemberLookupDto>(Member);
                lstmemberlookupDto.Add(MemberlookupDto);
            }
            return lstmemberlookupDto;
        }


        public MemberViewDto GetViewByID(int memberId)
        {
            List<uspMemberGetViewByID_Result> lstuspMemberGetViewByID_Result = _dbContext.uspMemberGetViewByID(memberId).ToList();
            MemberViewDto memberdto = new MemberViewDto();
            var MemberViewDto = Mapper.Map<uspMemberGetViewByID_Result, MemberViewDto>(lstuspMemberGetViewByID_Result.FirstOrDefault());

            List<MemberKYCDto> lstmemberkycdto = new List<MemberKYCDto>();
            List<uspMemberKYCGetViewByID_Result> lstuspMemberKYCGetViewByID_Result = _dbContext.uspMemberKYCGetViewByID(memberId).ToList();
            var lstmemberKycDto = Mapper.Map<List<uspMemberKYCGetViewByID_Result>, List<MemberKYCDto>>(lstuspMemberKYCGetViewByID_Result.ToList());
            foreach (var item in lstuspMemberKYCGetViewByID_Result)
            {
                MemberKYCDto objkycdto = new MemberKYCDto();
                objkycdto.KYCNumber = item.KYCNumber;
                objkycdto.FileName = item.FileName;
                objkycdto.MemberID = item.MemberID;
                lstmemberkycdto.Add(objkycdto);

            }

            MemberViewDto.MemberKYC = lstmemberkycdto;
            return MemberViewDto;
            //var prmMemberId = new ObjectParameter("MemberID", memberId);
            //var results = new MFISDBContext()
            //                .MultipleResults(ProcNames.uspMemberGetViewByID, prmMemberId)
            //                .With<MemberViewDto>()
            //                .With<MemberKYCDto>()
            //                .Execute();
            //var memberViewDto = (results[0] as List<MemberViewDto>)[0];
            //var memberKYC = results[1] as List<MemberKYCDto>;
            //memberViewDto.MemberKYC = memberKYC;
            //return memberViewDto;                    
        }


        public ResultDto UpdateAccountHead(MemberDto memberDto)
        {
            ResultDto resultDto = new ResultDto();
            string obectName = "MemberAccountHead";

            try
            {
                _dbContext.uspMemberAccountHeadUpdate(memberDto.MemberID, memberDto.AccountHeadID);
                resultDto.ObjectId = (int)memberDto.MemberID;
                if (resultDto.ObjectId > 0)
                    resultDto.Message = string.Format("{0} details saved successfully with code : {1}", obectName, memberDto.MemberCode);
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


        public ResultDto Delete(int memberId, int userId)
        {
            ResultDto resultDto = new ResultDto();
            string obectName = "Member";

            try
            {
                ObjectParameter prmmemberId = new ObjectParameter("MemberId", memberId);
                ObjectParameter prmmemberCode = new ObjectParameter("MemberCode", string.Empty);

                int effectedCount = _dbContext.uspMemberDelete(prmmemberId, prmmemberCode, userId);

                resultDto.ObjectId = (int)prmmemberId.Value;
                resultDto.ObjectCode = (string)prmmemberCode.Value;

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

        public ResultDto ChangeStatus(int memberId, int userId)
        {
            ResultDto resultDto = new ResultDto();
            string obectName = "Member";
            string statusCode = string.Empty;
            try
            {
                ObjectParameter prmMemberId = new ObjectParameter("MemberID", memberId);
                ObjectParameter prmMemberCode = new ObjectParameter("MemberCode", string.Empty);
                ObjectParameter prmStatusCode = new ObjectParameter("StatusCode", string.Empty);

                int effectedCount = _dbContext.uspMemberChangeStatus(prmMemberId, prmMemberCode, prmStatusCode, userId);

                resultDto.ObjectId = (int)prmMemberId.Value;
                resultDto.ObjectCode = (string)prmMemberCode.Value;
                statusCode = (string)prmStatusCode.Value;

                if (resultDto.ObjectId > 0)
                    resultDto.Message = string.Format("{0} : {1} details {2} successfully", obectName, resultDto.ObjectCode, statusCode == Constants.StatusCodes.Active ? "activated" : "inactivated");
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


        public List<SelectListDto> GetMemberByGroupId(int GroupId)
        {
            List<SelectListDto> lstSelectListDto = new List<SelectListDto>();
            List<uspMemberByGroupId_Result> uspMemberByGroupId_Result = _dbContext.uspMemberByGroupId(GroupId).ToList();
            foreach (var group in uspMemberByGroupId_Result)
            {
                SelectListDto objSelectListDto = new SelectListDto()
                {
                    ID = group.MemberID,
                    Text = group.MemberName

                };
                lstSelectListDto.Add(objSelectListDto);
            }
            return lstSelectListDto;
        }
        public List<SelectListDto> GetMemberCodeByGroupId(int GroupId)
        {
            List<SelectListDto> lstSelectListDto = new List<SelectListDto>();
            List<uspMemberByGroupId_Result> uspMemberByGroupId_Result = _dbContext.uspMemberByGroupId(GroupId).ToList();
            foreach (var group in uspMemberByGroupId_Result)
            {
                SelectListDto objSelectListDto = new SelectListDto()
                {
                    ID = group.MemberID,
                    Text = group.MemberCode

                };
                lstSelectListDto.Add(objSelectListDto);
            }
            return lstSelectListDto;
        }


        public List<MeetingMembersDTO> GetMemberSelectList()
        {
            List<MeetingMembersDTO> lstMemberDto = new List<MeetingMembersDTO>();
            List<uspMemberGetAll_Result> uspMemberGetAll_Result = _dbContext.uspMemberGetAll().ToList();
            foreach (var member in uspMemberGetAll_Result)
            {
                MeetingMembersDTO objmemberDto = new MeetingMembersDTO()
                {
                   MemberID = member.MemberID,
                   MemberName= member.MemberName
                };
                lstMemberDto.Add(objmemberDto);
            }
            return lstMemberDto;
        }

        public ResultDto UpdateMemberLeaderShip(int memberid, string leaderShipLevel, int designation, DateTime fromDate, int userid)
        {
            MemberDll memberDll=new MemberDll();
            return memberDll.UpdateMemberLeaderShip(memberid, leaderShipLevel, designation, fromDate, userid);
        }

        public List<MemberLookupDto> LeaderShipLookUp(int memberId)
        {
            List<MemberLookupDto> lstmemberlookupDto = new List<MemberLookupDto>();
            List<UspMemberLeadershiplookup_Result> lstUspMemberLeadershiplookup_Result = _dbContext.UspMemberLeadershiplookup(memberId).ToList();
            foreach (var Member in lstUspMemberLeadershiplookup_Result)
            {
                var MemberlookupDto = Mapper.Map<UspMemberLeadershiplookup_Result, MemberLookupDto>(Member);
                lstmemberlookupDto.Add(MemberlookupDto);
            }
            return lstmemberlookupDto;
        }


        //public List<MemberLookupDto> LookUp(int GroupId)
        //{
        //    List<MemberLookupDto> lstmemberlookupDto = new List<MemberLookupDto>();
        //    List<uspMemberLookup_Result> lstuspMemberLookup_Result = _dbContext.uspMemberLookup(GroupId).ToList();
        //    foreach (var Member in lstuspMemberLookup_Result)
        //    {
        //        var MemberlookupDto = Mapper.Map<uspMemberLookup_Result, MemberLookupDto>(Member);
        //        lstmemberlookupDto.Add(MemberlookupDto);
        //    }
        //    return lstmemberlookupDto;
        //}

    }
}

