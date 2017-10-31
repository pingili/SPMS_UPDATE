using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using System.Threading.Tasks;
using System.Configuration;

namespace DataLogic
{
    public class DBConstants
    {
        public static string MFIS_CS = ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString;
    }

    public static class ProcNames
    {
        public static readonly string uspRolesLookUp = "uspRolesLookUp";
        public static readonly string uspRoleInsertUpdate = "uspRoleInsertUpdate";
        public static readonly string uspRoleModulesInsertUpdate="uspRoleModulesInsertUpdate";
        public static readonly string uspModuleGetAll = "uspModuleGetAll";
        public static readonly string uspGetModuleActionsByModuleID = "uspGetModuleActionsByModuleID";  
        public static readonly string uspModuleActionInsertUpdate = "uspModuleActionInsertUpdate";
        public static readonly string uspModuleActionLookup = "uspModuleActionLookup";

        public static readonly string uspModuleActionGetByID = "uspModuleActionGetByID";
        public static readonly string uspModuleActionChangeStatus = "uspModuleActionChangeStatus";
        public static readonly string uspModuleActionDelete = "uspModuleActionDelete";
             
        public static readonly string uspRolesGetByRoleID = "uspRolesGetByRoleID";
        public static readonly string uspRoleDelete = "uspRoleDelete";
        public static readonly string uspRoleChangeStatus = "uspRoleChangeStatus";
        public static readonly string uspGetRoleModulesLookup = "uspGetRoleModulesLookup";

        public static readonly string uspGroupMeetingCurrentDates = "uspGroupMeetingCurrentDates";
        public static readonly string uspEmployeeLookUp = "uspEmployeeLookUp";
        public static readonly string uspGetSLAccountsByGLAccountId = "uspGetSLAccountsByGLAccountId";
        public static readonly string uspGroupOtherRecieptInsertUpdate = "uspGroupOtherRecieptInsertUpdate";
        public static readonly string uspGroupGeneralReceiptLookup = "uspGroupGeneralReceiptLookup";
        public static readonly string GetGroupOtherReceiptById = "GetGroupOtherReceiptById";
        public static readonly string uspGroupOtherReceiptView = "uspGroupOtherReceiptView";

        public static readonly string uspMemberLookUp = "uspMemberLookUp";
        public static readonly string uspGroupMemberAccountHeadTemplats = "uspGroupMemberAccountHeadTemplats";
        public static readonly string uspMemberReceiptLookup = "uspMemberReceiptLookup";
        public static readonly string uspGetMemberReceiptDemands = "uspGetMemberReceiptDemands";
        public static readonly string uspGetGroupMemberReceiptByAccountMasterId= "uspGetGroupMemberReceiptByAccountMasterId";

        public static readonly string uspGroupOtherJournalInsertUpdate = "uspGroupOtherJournalInsertUpdate";
    }
}
