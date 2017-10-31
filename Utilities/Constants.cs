using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class Constants
    {
        #region General
        public static readonly string REF_ID = "RefID";
        public static readonly string REF_VALUE = "RefValue";
        #endregion

        public class StatusCodes
        {
            public static readonly string Active = "ACT";
            public static readonly string InActive = "IN_ACT";
            public static readonly string Pending = "PENDING";
        }

        public class InterestTypes
        {
            public static readonly string LoanInterest = "L";
            public static readonly string DepositInterest = "D";
        }

        public class SessionKeys
        {
            public static readonly string SK_USERID = "USER_ID";
            public static readonly string SK_USERINFO = "UserInfo";
            public static readonly string SK_GROUPINFO = "GroupInfo";
        }

        public class RoleCodes
        {
            public static readonly string SUPER_ADMIN = "SYS_ADMIN";
            public static readonly string ORG_ADMIN = "ORG_ADMIN";
            public static readonly string CLUSTER_ASSOCIATE = "CLUSTER_ASSOCIATE";
            public static readonly string CDA_ACCOUNTANT = "CDA_ACCOUNTANT";
            public static readonly string MIS_CO_ORDINATOR = "MIS_CO_ORDINATOR";
            public static readonly string USER = "USER";
        }
    }
}
