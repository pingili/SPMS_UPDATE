using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class Enums
    {
        public enum RefMasterCodes
        {
            [Description("Bank Name")]
            BANK_NAME,

            [Description("Bank Account Type")]
            BANK_ACCOUNT_TYPE,

            [Description("Loan Purpose")]
            LOAN_PURPOSE,

            [Description("Loan Purpose Type")]
            LOAN_PURPOSE_TYPE,

            [Description("Occupation Type")]
            OCCUPATION_TYPE,

            [Description("Loan Security Type")]
            LOAN_SECURITY_TYPE,

            [Description("INTEREST BASE TYPE")]
            INTEREST_BASE_TYPE,

            [Description("INTEREST CALC TYPE")]
            INTEREST_CALC_TYPE,

            [Description("PRJ_TYPES")]
            PRJ_TYPES,

            [Description("PRJ_PURPOSE")]
            PRJ_PURPOSE,

            [Description("MEETING_FREQUENCY")]
            MEETING_FREQUENCY,


            [Description("EDUCATION_QUALIFICATION")]
            EDUCATION_QUALIFICATION,

            [Description("PARENT_GUARDIAN_RELATIONSHIP")]
            PARENT_GUARDIAN_RELATIONSHIP,

            [Description("SOCIAL_CATEGORY")]
            SOCIAL_CATEGORY,

            [Description("MONTHLY_INCOME")]
            MONTHLY_INCOME,

            [Description("INCOME_FREQUENCY")]
            INCOME_FREQUENCY,

            [Description("NOMINEE_RELATIONSHIP")]
            NOMINEE_RELATIONSHIP,

            [Description("RELIGION")]
            RELIGION,

            [Description("KYCTYPE")]
            KYCTYPE,

            [Description("DESIGNATION")]
            DESIGNATION,

            [Description("BLOODGROUP")]
            BLOODGROUP,

            [Description("LEADERSHIP_TITLE")]
            LEADERSHIP_TITLE,

            [Description("REASON")]
            REASON,


            [Description("Group Desigination")]
            GROUP_DESG,
            [Description("Cluster Desigination")]
            CLUSTER_DESG,
            [Description("Federation Desigination")]
            FED_DESG
            
        }

        public enum InterestTypes
        {
            [Description("Deposit Interest")]
            D,

            [Description("Loan Interest")]
            L,
        }

        public enum Groupreceipt
        {
            [Description("GroupReceipt")]
            GR,

        }

        public enum MemberTabs
        {
            PERSONALDETAILS,
            PROOFS,
            FAMILYDETAILS,
            GENERALACCOUNTHEAD,
            HISTORY,
            LEADERSHIP

        }

        public enum LeadershipDesigination
        {
            [Description("Group Desigination")]
            GROUP_DESG,
            [Description("Cluster Desigination")]
            CLUSTER_DESG,
            [Description("Federation Desigination")]
            FED_DESG
        }

        public enum KycType
        {
            AADHAR,
            VOTERID,
            PAN,
            RATIONCARD,
            JOBCARD,
            BANKACCOUNT
        }
        public enum Employeetabs
        {
            PERSONALDETAILS,
            PROOFS,
            FAMILYDETAILS,
            BANKAccountDetails,
            CreateLogin
        }

        public enum EntityCodes
        {
            [Description("Organization")]
            ORG,

            [Description("Branch")]
            BRANCH,

            [Description("Cluster")]
            CLUSTER,

            [Description("GroupMaster")]
            GROUP_MASTER,

            [Description("Member")]

            MEMBER,
            [Description("District")]
            DISTRICT,

            [Description("Mandal")]
            MANDAL,

            [Description("Village")]
            VILLAGE,

            [Description("Panchayat")]
            PANCHAYAT,

            [Description("BankMaster")]
            BANK_MASTER,

            [Description("Employee")]
            EMPLOYEE,

            [Description("FundSource")]
            FUND_SOURCE,

            [Description("Loan InterestMaster")]
            LOAN_INTEREST_MASTER,

            [Description("LoanMaster")]
            LOAN_MASTER,

            [Description("LoanPurpose")]
            LOAN_PURPOSE,

            [Description("LoanSecurity")]
            LOAN_SECURITY,

            [Description("Occupation")]
            OCCUPATION,

            [Description("Project")]
            PROJECT,

            [Description("Deposit Interest Master")]
            DEPOSIT_INTEREST_MASTER
        }

        public enum AccountTypes
        {
            [Description("ASSETS")]
            ASSETS,

            [Description("LIABILITIES")]
            LIABILITIES,

            [Description("INCOME")]
            INCOME,

            [Description("EXPENDITURE")]
            EXPENDITURE
        }

        public enum ApprovalActions
        {
            APP,
            CAN,
            REJ
        }

    }
}
