using AutoMapper;
using BusinessEntities;
using MFIEntityFrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


namespace BusinessLogic.AutoMapper
{
    public static class AutoMapperEntityConfiguration
    {
        public static void Configure()
        {
            //Account Head
            Mapper.CreateMap<List<uspAccountHeadGetAll_Result>, List<AccountHeadDto>>();
            Mapper.CreateMap<uspAccountHeadGetAll_Result, AccountHeadDto>();
            Mapper.CreateMap<List<uspAccountHeadGetByAHID_Result>, List<AccountHeadDto>>();
            Mapper.CreateMap<uspAccountHeadGetByAHID_Result, AccountHeadDto>();
            //Mapper.CreateMap<uspAccountheadgetcashvalue_Result, AccountHeadDto>();
            Mapper.CreateMap<uspAccountGetAHCODEByFederation_Result, AccountHeadDto>();



            //Branch
            Mapper.CreateMap<uspBranchLookup_Result, BranchLookupDto>();
            Mapper.CreateMap<List<uspBranchGetAll_Result>, List<BranchDto>>();
            Mapper.CreateMap<uspBranchGetAll_Result, BranchDto>();
            Mapper.CreateMap<List<uspBranchGetById_Result>, List<BranchDto>>();
            Mapper.CreateMap<uspBranchGetById_Result, BranchDto>();


            //Cluster
            Mapper.CreateMap<List<uspClusterGetAll_Result>, List<ClusterDto>>();
            Mapper.CreateMap<uspClusterGetAll_Result, ClusterDto>();
            Mapper.CreateMap<List<uspClusterGetByID_Result>, List<ClusterDto>>();
            Mapper.CreateMap<uspClusterGetByID_Result, ClusterDto>();

            Mapper.CreateMap<List<uspClusterLookup_Result>, List<ClusterDto>>();
            Mapper.CreateMap<uspClusterLookup_Result, ClusterDto>();
            Mapper.CreateMap<List<uspClusterLookup_Result>, List<ClusterLookupDto>>();
            Mapper.CreateMap<uspClusterLookup_Result, ClusterLookupDto>();




            //EMployee
            Mapper.CreateMap<List<uspEmployeeGetAll_Result>, List<EmployeeDto>>();
            Mapper.CreateMap<uspEmployeeGetAll_Result, EmployeeDto>();
            Mapper.CreateMap<List<uspEmployeeGetByID_Result>, List<EmployeeDto>>();
            Mapper.CreateMap<uspEmployeeGetByID_Result, EmployeeDto>();
            Mapper.CreateMap<uspEmployeeGetByID_Result, EmployeeDto>();
            Mapper.CreateMap<List<uspEmployeeLookUp_Result>, List<EmployeeLookupDto>>();
            Mapper.CreateMap<uspEmployeeLookUp_Result, EmployeeLookupDto>();
            Mapper.CreateMap<List<uspEmployeeKYCGetByEmployeeID_Result>, List<EmployeeKYCDto>>();
            Mapper.CreateMap<uspEmployeeKYCGetByEmployeeID_Result, EmployeeKYCDto>();

            Mapper.CreateMap<uspEmployeeGetViewByID_Result, EmployeeViewDto>();

            //Organization
            Mapper.CreateMap<List<uspOrganizationGetAll_Result>, List<OrganizationDto>>();
            Mapper.CreateMap<uspOrganizationGetAll_Result, OrganizationDto>();

            //District
            Mapper.CreateMap<List<uspDistrictGetAll_Result>, List<DistrictDto>>();
            Mapper.CreateMap<uspDistrictGetAll_Result, DistrictDto>();
            Mapper.CreateMap<List<uspDistrictGetByDistrictId_Result>, List<DistrictDto>>();
            Mapper.CreateMap<uspDistrictGetByDistrictId_Result, DistrictDto>();
            Mapper.CreateMap<uspDistrictLookup_Result, DistrictLookupDto>();
            Mapper.CreateMap<List<uspDistrictLookup_Result>, List<DistrictLookupDto>>();

            //Mandal
            Mapper.CreateMap<List<uspMandalGetAll_Result>, List<MandalDto>>();
            Mapper.CreateMap<uspMandalGetAll_Result, MandalDto>();
            Mapper.CreateMap<List<uspMandalGetByID_Result>, List<MandalDto>>();
            Mapper.CreateMap<uspMandalGetByID_Result, MandalDto>();
            Mapper.CreateMap<List<uspMandalLookup_Result>, List<MandalDto>>();
            Mapper.CreateMap<uspMandalLookup_Result, MandalDto>();
            Mapper.CreateMap<List<uspMandalLookup_Result>, List<MandalLookupDto>>();
            Mapper.CreateMap<uspMandalLookup_Result, MandalLookupDto>();



            //Occupation
            Mapper.CreateMap<List<uspOccupationGetAll_Result>, List<OccupationDto>>();
            Mapper.CreateMap<uspOccupationGetAll_Result, OccupationDto>();
            Mapper.CreateMap<List<uspOccupationGetByOccupationID_Result>, List<OccupationDto>>();
            Mapper.CreateMap<uspOccupationGetByOccupationID_Result, OccupationDto>();
            Mapper.CreateMap<uspOccupationLookup_Result, OccupationLookupDto>();
            Mapper.CreateMap<List<uspOccupationLookup_Result>, List<OccupationLookupDto>>();

            //Loan Purpose
            Mapper.CreateMap<List<uspLoanPurposeLookup_Result>, List<LoanPurposeLookupDto>>();
            Mapper.CreateMap<uspLoanPurposeLookup_Result, LoanPurposeLookupDto>();
            Mapper.CreateMap<List<uspLoanPurposeGetAll_Result>, List<LoanPurposeDto>>();
            Mapper.CreateMap<uspLoanPurposeGetAll_Result, LoanPurposeDto>();
            Mapper.CreateMap<List<uspLoanPurposeGetByLoanPurposeId_Result>, List<LoanPurposeDto>>();
            Mapper.CreateMap<uspLoanPurposeGetByLoanPurposeId_Result, LoanPurposeDto>();

            //Loan Security
            Mapper.CreateMap<List<uspLoanSecurityMasterLookup_Result>, List<LoanSecurityMasterLookupDto>>();
            Mapper.CreateMap<uspLoanSecurityMasterLookup_Result, LoanSecurityMasterLookupDto>();
            Mapper.CreateMap<List<uspLoanSecurityMasterGetAll_Result>, List<LoanSecurityMasterDto>>();
            Mapper.CreateMap<uspLoanSecurityMasterGetAll_Result, LoanSecurityMasterDto>();
            Mapper.CreateMap<List<uspLoanSecurityMasterLookup_Result>, List<LoanSecurityMasterDto>>();
            Mapper.CreateMap<uspLoanSecurityMasterLookup_Result, LoanSecurityMasterDto>();
            Mapper.CreateMap<List<uspLoanSecurityMasterGetByLoanSecurityID_Result>, List<LoanSecurityMasterDto>>();
            Mapper.CreateMap<uspLoanSecurityMasterGetByLoanSecurityID_Result, LoanSecurityMasterDto>();

            //Ref Values 
            Mapper.CreateMap<List<uspRefValuesByRefMasterCode_Result>, List<ReferenceValueDto>>();
            Mapper.CreateMap<uspRefValuesByRefMasterCode_Result, ReferenceValueDto>();

            //Status Master 
            Mapper.CreateMap<List<uspStatusMasterGetAll_Result>, List<StatusMasterDto>>();
            Mapper.CreateMap<uspStatusMasterGetAll_Result, StatusMasterDto>();
            Mapper.CreateMap<List<uspStatusMasterGetbByStatusCode_Result>, List<StatusMasterDto>>();
            Mapper.CreateMap<uspStatusMasterGetbByStatusCode_Result, StatusMasterDto>();

            //Bank 
            Mapper.CreateMap<List<uspBankDetailsByBankDetailID_Result>, List<BankMasterDto>>();
            Mapper.CreateMap<uspBankDetailsByBankDetailID_Result, BankMasterDto>();
            Mapper.CreateMap<List<uspBankMasterGetAll_Result>, List<BankMasterDto>>();
            Mapper.CreateMap<uspBankMasterGetAll_Result, BankMasterDto>();
            Mapper.CreateMap<uspBankMasterLookup_Result, BankMasterLookupDto>();
            Mapper.CreateMap<List<uspBankMasterLookup_Result>, List<BankMasterLookupDto>>();

            //Village
            Mapper.CreateMap<List<uspVillageLookup_Result>, List<VillageDto>>();
            Mapper.CreateMap<uspVillageLookup_Result, VillageDto>();
            Mapper.CreateMap<List<uspVillageGetAll_Result>, List<VillageDto>>();
            Mapper.CreateMap<uspVillageGetAll_Result, VillageDto>();
            Mapper.CreateMap<List<uspVillageLookup_Result>, List<VillageLookupDto>>();
            Mapper.CreateMap<uspVillageLookup_Result, VillageLookupDto>();
            Mapper.CreateMap<List<uspVillageGetById_Result>, List<VillageDto>>();
            Mapper.CreateMap<uspVillageGetById_Result, VillageDto>();



            //Project
            Mapper.CreateMap<uspProjectLookup_Result, ProjectLookupDto>();
            Mapper.CreateMap<uspProjectGetAll_Result, ProjectDto>();


            //FundSource
            Mapper.CreateMap<List<uspFundSourceGetByFundSourceId_Result>, List<FundSourceDto>>();
            Mapper.CreateMap<uspFundSourceGetByFundSourceId_Result, FundSourceDto>();
            Mapper.CreateMap<List<uspFundSourceGetAll_Result>, List<FundSourceDto>>();
            Mapper.CreateMap<uspFundSourceGetAll_Result, FundSourceDto>();
            Mapper.CreateMap<uspFundSourceLookup_Result, FundSourceLookup>();
            Mapper.CreateMap<List<uspFundSourceLookup_Result>, List<FundSourceLookup>>();

            //InterestMaster
            Mapper.CreateMap<List<uspInterestGetAll_Result>, List<InterestMasterDto>>();
            Mapper.CreateMap<uspInterestGetAll_Result, InterestMasterDto>();
            Mapper.CreateMap<uspInterestLookup_Result, InterestLookupDto>();
            Mapper.CreateMap<List<uspInterestLookup_Result>, List<InterestLookupDto>>();

            //Panchayat
            Mapper.CreateMap<List<uspPanchayatGetAll_Result>, List<PanchayatDto>>();
            Mapper.CreateMap<uspPanchayatGetAll_Result, PanchayatDto>();
            Mapper.CreateMap<uspPanchayatGetByPanchayatId_Result, PanchayatDto>();
            Mapper.CreateMap<List<uspPanchayatGetByPanchayatId_Result>, List<PanchayatDto>>();
            Mapper.CreateMap<uspPanchayatLookup_Result, PanchayatLookupDto>();
            Mapper.CreateMap<List<uspPanchayatLookup_Result>, List<PanchayatLookupDto>>();
            Mapper.CreateMap<uspPanchayatGetByGroupID_Result, PanchayatLookupDto>();


            //Group
            Mapper.CreateMap<List<uspGroupLookup_Result>, List<GroupLookupDto>>();
            Mapper.CreateMap<uspGroupLookup_Result, GroupLookupDto>();
            Mapper.CreateMap<List<uspGroupGetAll_Result>, List<GroupMasterDto>>();
            Mapper.CreateMap<uspGroupGetAll_Result, GroupMasterDto>();
            Mapper.CreateMap<uspGroupGetByGroupId_Result, GroupMasterDto>();
            Mapper.CreateMap<List<uspGroupGetByGroupId_Result>, List<GroupMasterDto>>();
            Mapper.CreateMap<uspMemberByGroupId_Result, MemberLookupDto>();
            Mapper.CreateMap<List<uspGroupDetailsGetByEmployeeId_Result>, List<GroupMasterDto>>();

            //Member
            Mapper.CreateMap<uspMemberGetByMemberID_Result, MemberDto>();
            Mapper.CreateMap<List<uspMemberLookup_Result>, List<MemberLookupDto>>();
            Mapper.CreateMap<uspMemberLookup_Result, MemberLookupDto>();

            Mapper.CreateMap<UspMemberLeadershiplookup_Result, MemberLookupDto>();

            Mapper.CreateMap<uspMemberGetViewByID_Result, MemberViewDto>();


            Mapper.CreateMap<uspMemberKYCGetByMemberID_Result, MemberKYCDto>();
            Mapper.CreateMap<List<uspMemberKYCGetViewByID_Result>, List<MemberKYCDto>>();

            Mapper.CreateMap<uspgroupGetAllBanksDetails_Result, BankMasterViewDto>();
            Mapper.CreateMap<List<uspgroupGetAllBanksDetails_Result>, List<BankMasterViewDto>>();


            //GroupLoanApplication
            Mapper.CreateMap<uspGroupLoanApplicationLookup_Result, GroupLoanApplicationLookupDto>();
            Mapper.CreateMap<uspGroupLoanApplicationGetByLoanMasterID_Result, GroupLoanApplicationDto>();
            Mapper.CreateMap<uspOrganizationGetAllBanksDetails_Result, BankMasterViewDto>();
            Mapper.CreateMap<List<uspOrganizationGetAllBanksDetails_Result>, List<BankMasterViewDto>>();
            Mapper.CreateMap<uspGroupGetAllBanksDetailsByGroupID_Result, BankMasterViewDto>();
            Mapper.CreateMap<uspGroupLoanApplicationGetViewByLoanMasterID_Result, GroupLoanApplicationDto>();

            //Leadership
            Mapper.CreateMap<uspLeadershipLookUp_Result, LeadershipLookupDto>();
            Mapper.CreateMap<uspLeadershipGetById_Result, LeadershipDto>();
            Mapper.CreateMap<List<uspLeadershipGetById_Result>, List<LeadershipDto>>();
            //JournalEntry
            Mapper.CreateMap<uspJournalEntryLookup_Result, JournalLookupDto>();
            Mapper.CreateMap<List<uspJournalEntryLookup_Result>, List<JournalLookupDto>>();
            Mapper.CreateMap<uspGroupJournalEntryLookup_Result, JournalLookupDto>();

            //GeneralPayments
            Mapper.CreateMap<uspGroupGeneralPaymentsLookup_Result, GeneralPaymentsLookupDto>();
            Mapper.CreateMap<uspFederationGeneralPaymentsLookup_Result, GeneralPaymentsLookupDto>();

            //GeneralReceipt
            Mapper.CreateMap<uspFederationGeneralReceiptLookup_Result, GeneralReceiptLookupDto>();
            Mapper.CreateMap<uspGroupGeneralReceiptLookup_Result, GeneralReceiptLookupDto>();


            //GroupReceipt
            Mapper.CreateMap<uspGroupReceiptLookup_Result, GropReceiptLookupDto>();
            Mapper.CreateMap<List<uspGroupReceiptLookup_Result>, List<GropReceiptLookupDto>>();
            Mapper.CreateMap<uspGetAccountHeadClosingBalnces_Result, ReceiptTranscationDto>();
            Mapper.CreateMap<List<uspGetAccountHeadClosingBalnces_Result>, List<ReceiptTranscationDto>>();

            Mapper.CreateMap<List<uspPaymentsToFederationLookup_Result>, List<PaymentsToFederationLookUpDto>>();
            Mapper.CreateMap<uspPaymentsToFederationLookup_Result, PaymentsToFederationLookUpDto>();

            //RefundsFromFederation
            Mapper.CreateMap<List<uspRefundsFromFederationLookup_Result>, List<RefundsFromFederationLookUpDto>>();
            Mapper.CreateMap<uspRefundsFromFederationLookup_Result, RefundsFromFederationLookUpDto>();



            //MemberReceipts
            Mapper.CreateMap<uspMemberReceiptLookup_Result, MemberReceiptLookupDto>();
            Mapper.CreateMap<List<uspMemberReceiptLookup_Result>, List<MemberReceiptLookupDto>>();

            // GroupLoanDisbursement
            Mapper.CreateMap<List<uspFederationLoanDisbursement_Result>, List<GroupLoanDisbursementDto>>();
            Mapper.CreateMap<uspFederationLoanDisbursement_Result, GroupLoanDisbursementDto>();
            Mapper.CreateMap<List<uspGroupLoanDisbursementLookup_Result>, List<GroupLoanDisbursementLookupDto>>();
            Mapper.CreateMap<uspGroupLoanDisbursementLookup_Result, GroupLoanDisbursementLookupDto>();

            Mapper.CreateMap<uspGetAccountHeadClosingBalnces_Result, ContraEntryWithDrawlTransactionsDto>();
            Mapper.CreateMap<uspFederationContraEntryWithDrawlLookup_Result, ContraEntryWithDrawlLookupDto>();


            //GroupMeetings
            Mapper.CreateMap<uspGroupMeetingLookup_Result, GroupMeetingLookupDto>();
            Mapper.CreateMap<uspGroupMeetingGetById_Result, GroupMeetingDto>();
            Mapper.CreateMap<List<uspGroupMeetingmembersGetById_Result>, List<MeetingMembersDTO>>();
            Mapper.CreateMap<List<uspGroupMeetingmembersGetById_Result>, List<GroupMeetingMembersDto>>();
            Mapper.CreateMap<uspGroupMeetingmembersGetById_Result, GroupMeetingMembersDto>();

            //Fedmeeting 
            Mapper.CreateMap<uspFederationMeetingLookup_Result, FederationMeetingLookupDto>();
            Mapper.CreateMap<List<uspFederationMeetingmembersGetById_Result>, List<MeetingMembersDTO>>();
            Mapper.CreateMap<uspFederationMeetingGetByFederationMeetingID_Result, FederationMeetingDTO>();


            //ContraEntry
            Mapper.CreateMap<uspMemberLoanApplicationGetByLoanMasterID_Result, MemberLoanApplicationDto>();
            Mapper.CreateMap<uspGroupContraEntryWithDrawlLookup_Result, ContraEntryWithDrawlLookupDto>();
            Mapper.CreateMap<uspFederationContraEntryDepositedLookup_Result, ContraEntryDepositedLookupDto>();
            Mapper.CreateMap<uspGroupContraEntryDepositedLookup_Result, ContraEntryDepositedLookupDto>();

        }

        public static T Cast<T>(this Object myobj)
        {
            try
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
