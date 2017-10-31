using BlackBeltCoder;
using BusinessEntities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLogic
{
    public class AccountHeadDll 
    {

        public int InsertUpdateAccountHead(AccountHeadDto accountHead, string Origin, int OriginObjectId, out string ahCode)
        {
            AdoHelper obj = new AdoHelper();

            SqlParameter[] parms = new SqlParameter[15];

            parms[0] = new SqlParameter("@AHID", accountHead.AHID);
            parms[0].SqlDbType = System.Data.SqlDbType.Int;
            parms[0].Size = 4;
            parms[0].Direction = ParameterDirection.InputOutput;

            parms[1] = new SqlParameter("@AHCode", accountHead.AHCode);
            parms[1].SqlDbType = System.Data.SqlDbType.VarChar;
            parms[1].Size = 256;
            parms[1].Direction = ParameterDirection.InputOutput;

            parms[2] = new SqlParameter("@AHName", accountHead.AHName);
            parms[2].SqlDbType = System.Data.SqlDbType.VarChar;

            parms[3] = new SqlParameter("@TE_AHName", accountHead.TE_AHName);
            parms[3].SqlDbType = System.Data.SqlDbType.NVarChar;

            parms[4] = new SqlParameter("@AHType", accountHead.AHType);
            parms[4].SqlDbType = System.Data.SqlDbType.Int;

            parms[5] = new SqlParameter("@ParentAHID", accountHead.ParentAHID);
            parms[5].SqlDbType = System.Data.SqlDbType.Int;

            parms[6] = new SqlParameter("@IsMemberTransaction", accountHead.IsMemberTransaction);
            parms[6].SqlDbType = System.Data.SqlDbType.Bit;

            parms[7] = new SqlParameter("@IsSLAccount", accountHead.IsSLAccount);
            parms[7].SqlDbType = System.Data.SqlDbType.Bit;

            parms[8] = new SqlParameter("@OpeningBalance", accountHead.OpeningBalance);
            parms[8].SqlDbType = System.Data.SqlDbType.Money;

            parms[9] = new SqlParameter("@OpeningBalanceType", accountHead.OpeningBalanceType);
            parms[9].SqlDbType = System.Data.SqlDbType.VarChar;

            parms[10] = new SqlParameter("@AHLevel", accountHead.AHLevel);
            parms[10].SqlDbType = System.Data.SqlDbType.Int;

            parms[11] = new SqlParameter("@IsFederation", accountHead.IsFederation);
            parms[11].SqlDbType = System.Data.SqlDbType.Bit;

            parms[12] = new SqlParameter("@UserID", accountHead.UserID);
            parms[12].SqlDbType = System.Data.SqlDbType.Int;

            parms[13] = new SqlParameter("@Origin", Origin);
            parms[13].SqlDbType = System.Data.SqlDbType.VarChar;

            parms[14] = new SqlParameter("@OriginObjectId", OriginObjectId);
            parms[14].SqlDbType = System.Data.SqlDbType.Int;

            int rowCount = obj.ExecNonQueryProc("uspAccountHeadInsertUpdate", parms);

            int ahId = (int)parms[0].Value;
            ahCode = (string)parms[1].Value;

            return ahId;
        }

        public AccountHeadDto ViewBalance(int ahid, bool isFedaration)
        {

            AccountHeadDto viewBalance = new AccountHeadDto();
            AdoHelper obj = new AdoHelper();
            SqlParameter[] parms = new SqlParameter[2];

            parms[0] = new SqlParameter("@Ahid", ahid);
            parms[0].SqlDbType = System.Data.SqlDbType.Int;
            parms[1] = new SqlParameter("@IsFedaration", isFedaration);
            parms[1].SqlDbType = System.Data.SqlDbType.Bit;

            SqlDataReader Dr = obj.ExecDataReaderProc("uspAccountHeadViewBalance", parms);
            if (Dr.Read())
            {
                viewBalance.OpeningBalance = Convert.ToDecimal(Dr["OpeningBalance"].ToString());
                viewBalance.ClosingBalance = Convert.ToDecimal(Dr["ClosingBalance"].ToString());
                viewBalance.CurrentYearBalance = Convert.ToDecimal(Dr["CurrentYearBalance"].ToString());
                viewBalance.CurrentYearBalanceDr = Convert.ToDecimal(Dr["CurrentYearBalanceDr"].ToString());
                viewBalance.CurrentYearBalanceCr = Convert.ToDecimal(Dr["CurrentYearBalanceCr"].ToString());
                viewBalance.OpeningBalanceType = Dr["BalanceType"].ToString();
            }
            return viewBalance;
        }

        public AccountHeadDto ViewBalance(int ahid, bool isFedaration,int groupId)
        {

            AccountHeadDto viewBalance = new AccountHeadDto();
            AdoHelper obj = new AdoHelper();
            SqlParameter[] parms = new SqlParameter[3];

            parms[0] = new SqlParameter("@Ahid", ahid);
            parms[0].SqlDbType = System.Data.SqlDbType.Int;
            parms[1] = new SqlParameter("@IsFedaration", isFedaration);
            parms[1].SqlDbType = System.Data.SqlDbType.Bit;
            parms[2] = new SqlParameter("@GroupID", groupId);
            parms[2].SqlDbType = System.Data.SqlDbType.Int;

            SqlDataReader Dr = obj.ExecDataReaderProc("uspAccountHeadViewBalanceGroup", parms);
            if (Dr.Read())
            {
                viewBalance.OpeningBalance = Convert.ToDecimal(Dr["OpeningBalance"].ToString());
                viewBalance.ClosingBalance = Convert.ToDecimal(Dr["ClosingBalance"].ToString());
                viewBalance.CurrentYearBalance = Convert.ToDecimal(Dr["CurrentYearBalance"].ToString());
                viewBalance.CurrentYearBalanceDr = Convert.ToDecimal(Dr["CurrentYearBalanceDr"].ToString());
                viewBalance.CurrentYearBalanceCr = Convert.ToDecimal(Dr["CurrentYearBalanceCr"].ToString());
                viewBalance.OpeningBalanceType = Dr["BalanceType"].ToString();
            }
            return viewBalance;
        }

        public bool UpdateOB(int ahid, int groupid, decimal balance)
        {
            AccountHeadDto viewBalance = new AccountHeadDto();
            AdoHelper obj = new AdoHelper();
            SqlParameter[] parms = new SqlParameter[3];

            parms[0] = new SqlParameter("@Ahid", ahid);
            parms[0].SqlDbType = System.Data.SqlDbType.Int;
            parms[1] = new SqlParameter("@Balance", balance);
            parms[1].SqlDbType = System.Data.SqlDbType.Decimal;
            parms[2] = new SqlParameter("@GroupID", groupid);
            parms[2].SqlDbType = System.Data.SqlDbType.Int;

            int rowCount = obj.ExecNonQueryProc("uspGroupOBUpdate", parms);
            if (rowCount > 0)
                return true;
            else return false;
        }
        public AccountHeadDto GetOB(int ahid, int groupid) {
            AccountHeadDto viewBalance = new AccountHeadDto();
            AdoHelper obj = new AdoHelper();
            SqlParameter[] parms = new SqlParameter[2];

            parms[0] = new SqlParameter("@Ahid", ahid);
            parms[0].SqlDbType = System.Data.SqlDbType.Int;
            parms[1] = new SqlParameter("@GroupID", groupid);
            parms[1].SqlDbType = System.Data.SqlDbType.Int;

            SqlDataReader Dr = obj.ExecDataReaderProc("uspGroupOBGet", parms);
            if (Dr.Read())
            {
                viewBalance.OpeningBalance = Convert.ToDecimal(Dr["OpeningBalance"].ToString());
                viewBalance.OpeningBalanceType = Dr["OpeningBalanceType"].ToString();
            }
            return viewBalance;
        }
        public List<int> GetBankAHIDs()
        {
            List<int> ahids = new List<int>();
            AdoHelper obj = new AdoHelper();

            SqlDataReader Dr = obj.ExecDataReader("uspGetBankMasteAhids");
            while (Dr.Read())
            {
                ahids.Add(Convert.ToInt32(Dr["ahid"].ToString()));
            }
            return ahids;
        }

        public List<int> GetOrganizationBanks()
        {
            List<int> ahids = new List<int>();
            AdoHelper obj = new AdoHelper();

            SqlDataReader Dr = obj.ExecDataReader("uspGetOrganizationBanks");
            while (Dr.Read())
            {
                ahids.Add(Convert.ToInt32(Dr["ahid"].ToString()));
            }
            return ahids;
        }
        public DataTable GetAccountHeadsTbleView(int groupId)
        {
            DataTable dt = new DataTable();
            AdoHelper obj = new AdoHelper();
            DataSet ds = obj.ExecDataSet("EXEC uspGetGroupAccountHeadsTableView " + groupId);
            return ds.Tables[0];
        }
        public DataTable GetAccountHeadsfebTableView()
        {
            DataTable dt = new DataTable();
            AdoHelper obj = new AdoHelper();
            DataSet ds = obj.ExecDataSet("uspGetFederationAccountHeadsTableView");
            return ds.Tables[0];
        }
        public List<AccountHeadDto> GetGroupAccountTree(int groupId)
        {
            List<AccountHeadDto> accountHeadlist = new List<AccountHeadDto>();
            AdoHelper obj = new AdoHelper();
            SqlParameter[] parms = new SqlParameter[1];

            parms[0] = new SqlParameter("@GroupID", groupId);
            parms[0].SqlDbType = System.Data.SqlDbType.Int;

            SqlDataReader Dr = obj.ExecDataReaderProc("uspGetGroupAccountTree", parms);
            while (Dr.Read())
            {
                AccountHeadDto ahdto = new AccountHeadDto();
                ahdto.AHCode = Dr["AHCode"] == DBNull.Value ? string.Empty : Convert.ToString(Dr["AHCode"]);
                ahdto.AHID = Convert.ToInt32(Dr["AHID"]);
                ahdto.AHLevel= Convert.ToInt32(Dr["AHLevel"]);
                ahdto.AHName = Dr["AHName"] == DBNull.Value ? string.Empty : Convert.ToString(Dr["AHName"]);
                ahdto.AHType = Dr["AHType"] == DBNull.Value ? 0: Convert.ToInt32(Dr["AHType"]);
                ahdto.OpeningBalanceType = Dr["OpeningBalanceType"] == DBNull.Value ? string.Empty : Convert.ToString(Dr["OpeningBalanceType"]);
                ahdto.ParentAHID = Dr["ParentAHID"] == DBNull.Value ? 0 : Convert.ToInt32(Dr["ParentAHID"]);
                accountHeadlist.Add(ahdto);
            }
            return accountHeadlist;
        }


        public List<SelectListDto> GetGeneralLedgerAccountHeads(int? groupId)
        {
            List<SelectListDto> accountHeadlist = new List<SelectListDto>();
            AdoHelper obj = new AdoHelper();
            SqlParameter[] parms = new SqlParameter[1];

            parms[0] = new SqlParameter("@GroupID", groupId);
            parms[0].SqlDbType = System.Data.SqlDbType.Int;

            SqlDataReader Dr = obj.ExecDataReaderProc("uspGetGeneralLedgerAccountHeads", parms);
            while (Dr.Read())
            {
                SelectListDto objSelectListDto = new SelectListDto()
                {
                    ID = Convert.ToInt32(Dr["AHID"]),
                    Text = Convert.ToString(Dr["AHName"]) + " - (" + Convert.ToString(Dr["AHCode"]) + ")"
                };
                accountHeadlist.Add(objSelectListDto);
            }
            return accountHeadlist;
        }
    }
}
