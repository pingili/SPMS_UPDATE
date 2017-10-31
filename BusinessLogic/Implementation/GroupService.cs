using AutoMapper;
using BusinessEntities;
using BusinessLogic.AutoMapper;
//using BusinessLogic.Interface;
using CoreComponents;
using DataLogic.Implementation;
using MFIEntityFrameWork;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace BusinessLogic
{
    public class GroupService 
    {
        #region Global Variables
        private readonly MFISDBContext _dbContext;
        public GroupService()
        {
            _dbContext = new MFISDBContext();
            AutoMapperEntityConfiguration.Configure();
        }
        #endregion
       
        public ResultDto InsertDepositLoanRates(List<GroupInterestRatesDto> lstRates, long GroupId, int userid, string type)
        {
            ResultDto resultDto = new ResultDto();
            string LoanDepositsxml = CommonMethods.SerializeListDto<List<GroupInterestRatesDto>>(lstRates);

            int count = _dbContext.uspGroupLoanDepositInterestRates(GroupId, userid, LoanDepositsxml, type);
            if (count > 0)
            {
                resultDto.ObjectCode = "TRUE";
                if (type == "L")
                    resultDto.Message = " Loan Interest Rates details saved success fully";
                else
                {
                    resultDto.Message = " Deposit Interest Rates details saved success fully";
                }
            }
            else if (count == -1)
            {
                resultDto.ObjectCode = "FALSE";
                if (type == "L")
                    resultDto.Message = "Error occured while inserting Loan Insert Details";
                else
                {
                    resultDto.Message = "Error occured while inserting Deposit Insert Details";
                }

            }
            else
            {
                resultDto.ObjectCode = "FALSE";
                if (type == "L")
                    resultDto.Message = "Error occured while saving Loan Insert details";
                else
                {
                    resultDto.Message = "Error occured while saving Deposit Insert details";
                }

            }
            return resultDto;

        }
       public List<GroupLookupDto> Lookup()
        {
            var lstuspGroupLookup_Result = _dbContext.uspGroupLookup().ToList();
            var lstGroupLookupDto = new List<GroupLookupDto>();

            foreach (var obj in lstuspGroupLookup_Result)
            {
                var dtoobj = Mapper.Map<uspGroupLookup_Result, GroupLookupDto>(obj);
                lstGroupLookupDto.Add(dtoobj);
            }
            return lstGroupLookupDto;
        }
        public List<GroupInterestRatesDto> GetInterestByID(int groupID, string type)
        {
            List<uspGroupDepositLoanInterestByID_Result> lstGroupDepositLoanIntrerest_Result = _dbContext.uspGroupDepositLoanInterestByID(groupID, type).ToList();
            List<GroupInterestRatesDto> objGroupInterestRatesDto = new List<GroupInterestRatesDto>();

            foreach (var obj in lstGroupDepositLoanIntrerest_Result)
            {
                GroupInterestRatesDto objGroupInterestRatesDto1 = new GroupInterestRatesDto()
                {
                    GroupInterestID = obj.GroupInterestID,
                    Base = obj.Base,
                    BaseText = obj.BaseText,
                    PrincipalAHID = obj.PrincipalAHID,
                    PenalAHID = Convert.ToInt32(obj.PenalAHID),
                    InterestAHID = obj.InterestAHID,
                    ROI = obj.ROI,
                    PenalROI = obj.PenalROI.HasValue ? obj.PenalROI.Value : default(decimal),
                    FromDate = obj.FromDate,
                    ToDate = obj.ToDate.HasValue ? obj.ToDate.Value : default(DateTime),
                    IsActive = obj.IsActive,
                    CaluculationMethodId = obj.CaluculationMethodID.HasValue ? obj.CaluculationMethodID.Value : default(int),
                    CaluculationMethod = obj.CaluculationMethod,
                    PenalAH = obj.PenalAH,
                    InterestAH = obj.InterestAH,
                    PrincipalAH = obj.PrincipalAH
                };

                objGroupInterestRatesDto.Add(objGroupInterestRatesDto1);
            }

            return objGroupInterestRatesDto;
        }


        public List<SelectListDto> GetGroupsSelectList()
        {

            List<SelectListDto> lstSelectListDto = new List<SelectListDto>();
            List<uspGroupGetAll_Result> lstuspGroupGetAll_Result = _dbContext.uspGroupGetAll().ToList();
            foreach (var group in lstuspGroupGetAll_Result)
            {
                SelectListDto objSelectListDto = new SelectListDto()
                {
                    ID = group.GroupID,
                    Text = group.GroupName

                };
                lstSelectListDto.Add(objSelectListDto);
            }
            return lstSelectListDto;

        }
       
        public ResultDto Delete(int groupid, int userId)
        {
            ResultDto resultDto = new ResultDto();
            string obectName = "Group";

            try
            {
                ObjectParameter prmResultID = new ObjectParameter("Result", resultDto.Result);
                ObjectParameter prmMessage = new ObjectParameter("Message", string.Empty);

                _dbContext.uspGroupDelete(groupid, userId, prmResultID, prmMessage);

                resultDto.Result = (bool)prmResultID.Value;
                resultDto.Message = (string)prmMessage.Value;
                resultDto.ObjectId = (int)groupid;

            }
            catch (Exception)
            {
                resultDto.Message = string.Format("Service layer error occured while deleting the {0} details", obectName);
                resultDto.ObjectId = -98;

            }
            return resultDto;

        }
        public GroupMasterDto GetByID(int groupID)
        {
            GroupMeetingDAL objDal = new GroupMeetingDAL();
            return objDal.GetGroupMasterDetailsByID(groupID);

            //List<uspGroupGetByGroupId_Result> lstuspGroupGetByGroupId_Result = _dbContext.uspGroupGetByGroupId(groupID).ToList();
            //GroupMasterDto objGroupMasterDto = Mapper.Map<uspGroupGetByGroupId_Result, GroupMasterDto>(lstuspGroupGetByGroupId_Result.FirstOrDefault());
            //return objGroupMasterDto;

        }
        public List<GroupMasterDto> GetByEmployeeId(int employeeId)
        {
            List<GroupMasterDto> objGroupMasterDto = new List<GroupMasterDto>();
            List<uspGroupDetailsGetByEmployeeId_Result> lstuspGroupGetByGroupId_Result = _dbContext.uspGroupDetailsGetByEmployeeId(employeeId).ToList();
            foreach (var Item in lstuspGroupGetByGroupId_Result)
            {
                GroupMasterDto objgroup = new GroupMasterDto();
                objgroup.GroupID = Item.GroupID;
                objgroup.GroupName = Item.GroupName;
                objgroup.GroupCode = Item.GroupCode;
                objgroup.TEGroupName = Item.TEGroupName;
                objgroup.Village = Item.Village;
                objgroup.ClusterName = Item.ClusterName;
                objGroupMasterDto.Add(objgroup);            
            }
            return objGroupMasterDto;

        }
        public ResultDto Insert(GroupMasterDto group)
        {
            return InsertUpdateGroup(group);
        }

        public ResultDto Update(GroupMasterDto group)
        {
            return InsertUpdateGroup(group);
        }

        private ResultDto InsertUpdateGroup(GroupMasterDto objgroup)
        {
            bool isSuccess = true;
            ResultDto resultDto = new ResultDto();
            string GroupCode = "";
            try
            {
                ObjectParameter paramGroupID = new ObjectParameter("GroupID", objgroup.GroupID);
                ObjectParameter paramGroupCode = new ObjectParameter("GroupCode", string.Empty);
                
                decimal? regularSavingAmount = objgroup.RegularSavingAmount;
                int? regularSavingAhId = objgroup.RegularSavingsAhId;

                int COUNT = _dbContext.uspGroupInsertUpdate(paramGroupID, objgroup.GroupRefNumber, objgroup.GroupName, objgroup.TEGroupName, objgroup.PanchayatID, objgroup.FormationDate, objgroup.Phone, objgroup.Email, objgroup.Address, objgroup.MeetingFrequency, objgroup.FederationTranStartDate, objgroup.DateOfClosure, objgroup.MeetingDay, objgroup.MeetingStartTime, objgroup.MeetingEndTime, regularSavingAmount, regularSavingAhId, objgroup.UserId, paramGroupCode);
                GroupCode = Convert.ToString(paramGroupCode.Value);
                resultDto.ObjectId = Convert.ToInt32(paramGroupID.Value);
                resultDto.ObjectCode = Convert.ToString(paramGroupCode.Value);

                int GroupId = (int)paramGroupID.Value;
                if (GroupId > 0)
                    resultDto.Message = "Group details saved success fully";
                else if (GroupId == -1)
                    resultDto.Message = "Group occured while generating Group code";
                else
                    resultDto.Message = "Group occured while saving Group details";
            }
            catch (Exception)
            {
                resultDto.Message = "Service layer error occured while saving the bank details";
                resultDto.ObjectId = -98;
            }
            return resultDto;


        }

        //public bool CreateGroup(GroupMasterDto objGroupMasterDto)
        //{
        //    bool isSuccess = true;
        //    try
        //    {
        //        GroupMaster groupMaster = Mapper.Map<GroupMasterDto, GroupMaster>(objGroupMasterDto);

        //        groupMaster.CreatedBy = 1;
        //        groupMaster.StatusID =
        //            _dbContext.StatusMasters.ToList().Find(l => l.StatusCode == Constants.StatusCodes.Active).StatusID;
        //        groupMaster.CreatedOn = DateTime.Now;

        //        _dbContext.GroupMasters.Add(groupMaster);
        //        _dbContext.SaveChanges();
        //    }
        //    catch (Exception)
        //    {
        //        isSuccess = false;
        //    }
        //    return isSuccess;
        //}


        public GroupMasterViewDto GetViewByID(int groupId)
        {
            var prmGroupId = new ObjectParameter("GroupID", groupId);

            var results = new MFISDBContext()
                .MultipleResults(CustomProcNames.uspGroupGetViewByID, prmGroupId)
                .With<GroupMasterViewDto>()
                .Execute();

            var groupViewDto = (results[0] as List<GroupMasterViewDto>)[0];
            return groupViewDto;
        }
        public List<SelectListDto> GetGroupByClusterID(int clusterid)
        {
            List<SelectListDto> lstSelectListDto = new List<SelectListDto>();
            List<uspGroupNameGetByClusterID_Result> lstuspGroupGetAll_Result = _dbContext.uspGroupNameGetByClusterID(clusterid).ToList();
            foreach (var group in lstuspGroupGetAll_Result)
            {
                SelectListDto objSelectListDto = new SelectListDto()
                {
                    ID = group.GroupID,
                    Text = group.GroupName

                };
                lstSelectListDto.Add(objSelectListDto);
            }
            return lstSelectListDto;
        }


        public List<SelectListDto> GetGroupByVillageID(int villageID)
        {
            List<SelectListDto> lstSelectListDto = new List<SelectListDto>();
            List<uspGroupNameGetByVillageID_Result> lsuspGroupNameGetByVillageID_Result = _dbContext.uspGroupNameGetByVillageID(villageID).ToList();
            foreach (var group in lsuspGroupNameGetByVillageID_Result)
            {
                SelectListDto objSelectListDto = new SelectListDto()
                {
                    ID = group.GroupID,
                    Text = group.GroupName

                };
                lstSelectListDto.Add(objSelectListDto);
            }
            return lstSelectListDto;
        }


        public List<SelectListDto> GetGroupCodeByClusterID(int Id)
        {
            List<SelectListDto> lstSelectListDto = new List<SelectListDto>();
            List<uspGroupNameGetByClusterID_Result> lstuspGroupGetAll_Result = _dbContext.uspGroupNameGetByClusterID(Id).ToList();
            foreach (var group in lstuspGroupGetAll_Result)
            {
                SelectListDto objSelectListDto = new SelectListDto()
                {
                    ID = group.GroupID,
                    Text = group.GroupCode+':'+group.GroupName

                };
                lstSelectListDto.Add(objSelectListDto);
            }
            return lstSelectListDto;
        }


        public ResultDto ChangeStatus(int GroupId, int CurrentUserID)
        {
            ResultDto resultDto = new ResultDto();
            string obectName = "Group";
            string statusCode = string.Empty;
            try
            {
                ObjectParameter prmGroupID = new ObjectParameter("GroupID", GroupId);
                ObjectParameter prmGroupCode = new ObjectParameter("GroupCode", string.Empty);
                ObjectParameter prmStatusCode = new ObjectParameter("StatusCode", string.Empty);

                _dbContext.uspGroupChangeStatus(prmGroupID, prmGroupCode, prmStatusCode, CurrentUserID);

                resultDto.ObjectId = (int)prmGroupID.Value;
                resultDto.ObjectCode = (string)prmGroupCode.Value;
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
    }
}
