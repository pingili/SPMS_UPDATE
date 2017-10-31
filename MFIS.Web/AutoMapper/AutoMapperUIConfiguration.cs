using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using BusinessEntities;
using MFIS.Web.Areas.Federation.Models;
using System.Reflection;
using MFIS.Web.Areas.Group.Models;

namespace MFIS.Web.AutoMapper
{
    public static class AutoMapperUIConfiguration
    {
        public static void Configure()
        {
            Mapper.CreateMap<AccountHeadModel, AccountHeadDto>();
            // Mapper.CreateMap<AccountHeadDto, AccountHead>();
            Mapper.CreateMap<BankMasterModel, BankMasterDto>();
            // Mapper.CreateMap<BankMasterDto, BankMaster>();
            Mapper.CreateMap<MandalModel, MandalDto>();
            Mapper.CreateMap<FundSourceModel, FundSourceDto>();
            Mapper.CreateMap<FundSourceDto, FundSourceModel>();
            Mapper.CreateMap<List<FundSourceDto>, List<FundSourceModel>>();
            Mapper.CreateMap<LoanPurposeModel, LoanPurposeDto>();
            Mapper.CreateMap<LoanPurposeDto, LoanPurposeModel>();
            Mapper.CreateMap<List<LoanPurposeDto>, List<LoanPurposeModel>>();
            Mapper.CreateMap<List<LoanPurposeLookupDto>, List<LoanPurposeModel>>();
            Mapper.CreateMap<OrganizationModel, OrganizationDto>();
            Mapper.CreateMap<OrganizationDto, OrganizationModel>();
            Mapper.CreateMap<OccupationModel, OccupationDto>();
            Mapper.CreateMap<NPAModel, NPADto>();
            Mapper.CreateMap<LoanSecurityMasterModel, LoanSecurityMasterDto>();
            Mapper.CreateMap<LoanSecurityMasterDto, LoanSecurityMasterModel>();
            Mapper.CreateMap<List<LoanSecurityMasterDto>, List<LoanSecurityMasterModel>>();
            Mapper.CreateMap<MandalDto, MandalModel>();
            Mapper.CreateMap<List<MandalDto>, List<MandalModel>>();
            Mapper.CreateMap<ClusterModel, ClusterDto>();
            Mapper.CreateMap<ClusterDto, ClusterModel>();
            Mapper.CreateMap<List<ClusterDto>, List<ClusterModel>>();
            Mapper.CreateMap<List<LoanSecurityMasterLookupDto>, List<LoanSecurityModel>>();
            Mapper.CreateMap<VillageModel, VillageDto>();
            Mapper.CreateMap<VillageDto, VillageModel>();
            Mapper.CreateMap<List<VillageDto>, List<VillageModel>>();
            Mapper.CreateMap<List<VillageLookupDto>, List<VillageModel>>();
            Mapper.CreateMap<BranchModel, BranchDto>();
            Mapper.CreateMap<BranchDto, BranchModel>();
            Mapper.CreateMap<DistrictModel, DistrictDto>();
            Mapper.CreateMap<DistrictDto, DistrictModel>();
            Mapper.CreateMap<List<LoanPurposeDto>, List<LoanPurposeModel>>();
            Mapper.CreateMap<OccupationDto, OccupationModel>();
            Mapper.CreateMap<OccupationModel, OccupationDto>();
            Mapper.CreateMap<ProjectModel, ProjectDto>();
            Mapper.CreateMap<PanchayatModel, PanchayatDto>();
            Mapper.CreateMap<PanchayatDto, PanchayatModel>();
            Mapper.CreateMap<DepreciationModel, DepreciationDto>();
            Mapper.CreateMap<DepreciationDto, DepreciationModel>();

            //Employee 
            Mapper.CreateMap<EmployeeModel, EmployeeDto>();
            Mapper.CreateMap<List<EmployeeModel>, List<EmployeeDto>>();
            Mapper.CreateMap<EmployeeDto, EmployeeModel>();
            Mapper.CreateMap<List<EmployeeDto>, List<EmployeeModel>>();

            //Group
            Mapper.CreateMap<List<GroupMasterDto>, List<GroupModel>>();
            Mapper.CreateMap<GroupMasterDto, GroupModel>();

            Mapper.CreateMap<List<VillageLookupDto>, List<VillageModel>>();

            //Member
            Mapper.CreateMap<MemberModel, MemberDto>();
            Mapper.CreateMap<List<MemberModel>,List<MemberDto>>();
            Mapper.CreateMap<List<MemberDto>, List<MemberModel>>();
            Mapper.CreateMap<MemberDto, MemberModel>();
            Mapper.CreateMap<List<MemberKYCDto>, List<MemberKycModel>>();
            Mapper.CreateMap<List<MemberKYCDto>, List<MemberKycModel>>();


            //LeaderShip
            Mapper.CreateMap<LeadershipModel, LeadershipDto>();
            Mapper.CreateMap<List<LeadershipModel>,List<LeadershipDto>>();
            Mapper.CreateMap<BranchDto, BranchModel>();
            Mapper.CreateMap<List<LeadershipDto>, List<LeadershipModel>>();
            Mapper.CreateMap<LeadershipDto, LeadershipModel>();

            //GroupLoanApplication
            Mapper.CreateMap<LoanApplicationModel, LoanApplicationDto>();
            Mapper.CreateMap<GroupLoanApplicationModel, GroupLoanApplicationDto>();
            Mapper.CreateMap<GroupLoanApplicationDto, GroupLoanApplicationModel>();

            Mapper.CreateMap<EmployeeDto, CreateMemberRefundsModel>();

           // Mapper.CreateMap<EmployeeDto, Member>();


        }

        public static T Cast<T>(this Object myobj)
        {
            Type objectType = myobj.GetType();
            Type target = typeof(T);
            var x = Activator.CreateInstance(target, false);
            var z = from source in objectType.GetMembers().ToList()
                    where source.MemberType == MemberTypes.Property
                    select source;
            var d = from source in target.GetMembers().ToList()
                    where source.MemberType == MemberTypes.Property
                    select source;
            List<MemberInfo> members = d.Where(memberInfo => d.Select(c => c.Name)
                .ToList().Contains(memberInfo.Name)).ToList();
            PropertyInfo propertyInfo;
            object value;
            foreach (var memberInfo in members)
            {
                propertyInfo = typeof(T).GetProperty(memberInfo.Name);
                if (myobj.GetType().GetProperty(memberInfo.Name) != null)
                    propertyInfo.SetValue(x, myobj.GetType().GetProperty(memberInfo.Name).GetValue(myobj, null), null);
            }
            return (T)x;
        }

    }
}