using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntities;
using BlackBeltCoder;
using System.Data.SqlClient;

namespace DataLogic.Implementation
{
    public class BankMasterDal
    {
        public List<BankMasterDto>  GetGroupBanks(int groupID)
        {
            List<BankMasterDto> lstBanks = new List<BankMasterDto>();
             AdoHelper obj = new AdoHelper();
            SqlParameter[] parms = new SqlParameter[1];

            parms[0] = new SqlParameter("@GroupID", groupID);
            parms[0].SqlDbType = System.Data.SqlDbType.Int;
            SqlDataReader Dr = obj.ExecDataReaderProc("uspGetGroupBanks", parms);
            while (Dr.Read())
            {
                BankMasterDto bankMasterDto = new BankMasterDto();
                bankMasterDto.AccountNumber = Dr["AccountNumber"].ToString();
                bankMasterDto.AHID= Convert.ToInt32( Dr["AHID"].ToString());
                bankMasterDto.AHName = Dr["AHName"].ToString();
                bankMasterDto.BankEntryID = Convert.ToInt32(Dr["BankEntryID"].ToString());
                lstBanks.Add(bankMasterDto);

            }
            return lstBanks;
        }
        public List<BankMasterLookupDto> GetLookup(int? groupId)
        {
            List<BankMasterLookupDto> lstBanks = new List<BankMasterLookupDto>();
            AdoHelper obj = new AdoHelper();
            SqlParameter parm = new SqlParameter();
            parm.ParameterName = "@GroupId";
            if (groupId != null)
                parm.Value = groupId.Value;

            SqlDataReader Dr = obj.ExecDataReaderProc("uspBankMasterLookup", parm);
            while (Dr.Read())
            {
                BankMasterLookupDto bankMasterDto = new BankMasterLookupDto();
                bankMasterDto.AccountNumber = Dr["AccountNumber"].ToString();
                bankMasterDto.BankEntryID = Convert.ToInt32(Dr["BankEntryID"].ToString());
                bankMasterDto.BankCode = Dr["BankCode"].ToString();
                bankMasterDto.BankName = Dr["BankName"].ToString();
                bankMasterDto.IFSC = Dr["IFSC"].ToString();
                bankMasterDto.isFederation = Dr["IS_FED"].ToString();
                bankMasterDto.ClusterName = Dr["CLUSTERNAME"].ToString();
                bankMasterDto.GroupName = Dr["GROUP_NAME"].ToString();
                bankMasterDto.StatusID = Convert.ToInt32(Dr["StatusID"]);
                bankMasterDto.StatusCode = Dr["StatusCode"].ToString();
                bankMasterDto.GroupId = Convert.ToInt32(Dr["GroupId"]);
                lstBanks.Add(bankMasterDto);
            }
            return lstBanks;
        }

    }
}
