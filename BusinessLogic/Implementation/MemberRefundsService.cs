using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntities;
//using BusinessLogic.Interface;
using MFIEntityFrameWork;
using BusinessLogic.AutoMapper;
using AutoMapper;
using System.Data.Entity.Core.Objects;

namespace BusinessLogic
{
    public class MemberRefundsService 
    {
        private readonly MFISDBContext _dbContext;

        public MemberRefundsService()
        {
            _dbContext = new MFISDBContext();
            AutoMapperEntityConfiguration.Configure();
        }

        public List<SelectListDto> GetGroupsSelectList()
        {
            List<SelectListDto> lstSelectListDto = new List<SelectListDto>();
            List<uspGroupEmployeeGetALL_Result> lstuspGroupEmployeeGetAll = _dbContext.uspGroupEmployeeGetALL().ToList();
            foreach (var Groups in lstuspGroupEmployeeGetAll)
            {
                SelectListDto objSelectlistDto = new SelectListDto()
                {
                    ID = Groups.GroupID,
                    Text = Groups.GroupName
                };
                lstSelectListDto.Add(objSelectlistDto);
            }
            return lstSelectListDto;
        }
        public List<SelectListDto> GetEmployeeCodeByGroupId(int GroupId)
        {
            List<SelectListDto> lstSelectList = new List<SelectListDto>();
            List<uspGroupEmployeeGetByGroupID_Result> lstuspGroupEmployeeGetByGroupID_Result = _dbContext.uspGroupEmployeeGetByGroupID(GroupId).ToList();
            foreach (var employee in lstuspGroupEmployeeGetByGroupID_Result)
            {
                SelectListDto objSelectListDto = new SelectListDto()
                {
                    ID = employee.EmployeeID,
                    Text = employee.EmployeeCode
                };
                lstSelectList.Add(objSelectListDto);
            }
            return lstSelectList;
        }
        //public List<LoanMasterDto> GetAccountHeds(int MemberId)
        //{
        //    List<LoanMasterDto> lstLoanMasterDto = new List<LoanMasterDto>();
        //    LoanMasterDto ObjLoanMasterDto = new LoanMasterDto();
        //    ObjectParameter prmMemberID = new ObjectParameter("MemberId",ObjLoanMasterDto.MemberID);
        //    List<uspLoanRefundsGetAccountHeads_Result> lstuspLoanRefunds_Result = _dbContext.uspLoanRefundsGetAccountHeads(MemberId).ToList();
        //    foreach (var accountheads in lstuspLoanRefunds_Result)
        //    {
        //        var LoanMasterDto = Mapper.Map<uspLoanRefundsGetAccountHeads_Result, LoanMasterDto>(accountheads);
        //        lstLoanMasterDto.Add(LoanMasterDto);

        //    }
        //    return lstLoanMasterDto;
        //    }
        // 

        public List<LoanMasterDto> GetAccountHeds(int MemberId)
        {
            List<LoanMasterDto> lstLoanMasterDto = new List<LoanMasterDto>();
            return lstLoanMasterDto;
        }

    }
}

