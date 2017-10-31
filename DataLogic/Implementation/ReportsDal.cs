using BlackBeltCoder;
using BusinessEntities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLogic.Implementation
{
    public class ReportsDal
    {
        public List<TrialBalanceDto> GetAllTrialBalanceReport_Group(int groupID,TrialBalanceTotalsDto objTotals = null)
        {
            if (objTotals == null) objTotals = new TrialBalanceTotalsDto();
            List<TrialBalanceDto> lstTrialBalance = new List<TrialBalanceDto>();
            AdoHelper objado = new AdoHelper();
            SqlParameter[] parms = new SqlParameter[1];

            parms[0] = new SqlParameter("@GroupID", groupID);
            parms[0].SqlDbType = System.Data.SqlDbType.Int;

            SqlDataReader dr = objado.ExecDataReaderProc("uspTrialBalanceReport", parms);
            while(dr.Read())
            {
                TrialBalanceDto obj = new TrialBalanceDto();
 
                obj.Ahcode = dr["AHCODE"] == DBNull.Value ? string.Empty : dr["AHCODE"].ToString();
                obj.AhType = dr["AHType"] == DBNull.Value ? default(Int32) : Convert.ToInt32(dr["AHType"]);
                obj.Accounthaedname = dr["ACCOUNTHEADNAME"] == DBNull.Value ? string.Empty : Convert.ToString(dr["ACCOUNTHEADNAME"]);
                obj.Debit1 = dr["OPENING_BALANCE_DEBIT"] == DBNull.Value ? default(double) : Convert.ToDouble(dr["OPENING_BALANCE_DEBIT"]);
                obj.Credit1 = dr["OPENING_BALANCE_CREDIT"] == DBNull.Value ? default(double) : Convert.ToDouble(dr["OPENING_BALANCE_CREDIT"]);
                obj.Debit2 = dr["TRAN_DEBIT"] == DBNull.Value ? default(double) : Convert.ToDouble(dr["TRAN_DEBIT"]);
                obj.Credit2 = dr["TRAN_CREDIT"] == DBNull.Value ? default(double) : Convert.ToDouble(dr["TRAN_CREDIT"]);
                obj.Debit3 = dr["CLOSING_DEBIT"] == DBNull.Value ? default(double) : Convert.ToDouble(dr["CLOSING_DEBIT"]);
                obj.Credit3 = dr["CLOSING_CREDIT"] == DBNull.Value ? default(double) : Convert.ToDouble(dr["CLOSING_CREDIT"]);

                lstTrialBalance.Add(obj);
            }
            if (dr.NextResult())
            {
                if (dr.Read())
                {
                    if (Convert.ToInt32(dr["TOTAL_RECORDS"]) != 0)
                        objTotals.TotalRecords = Convert.ToInt32(dr["TOTAL_RECORDS"]);
                    if (dr["OB_DR_Sum"] != DBNull.Value)
                        objTotals.OpeningBalDrSum = Convert.ToDouble(dr["OB_DR_Sum"]);
                    if (dr["OB_CR_SUM"] != DBNull.Value)
                        objTotals.OpeningBalCrSum = Convert.ToDouble(dr["OB_CR_SUM"]);
                    if (dr["TRAN_DR_SUM"] != DBNull.Value)
                        objTotals.TranBalDrSum = Convert.ToDouble(dr["TRAN_DR_SUM"]);
                    if (dr["TRAN_CR_SUM"] != DBNull.Value)
                        objTotals.TranBalCrSum = Convert.ToDouble(dr["TRAN_CR_SUM"]);
                    if (dr["CLOSING_DR_SUM"] != DBNull.Value)
                        objTotals.ClosingBalDrSum = Convert.ToDouble(dr["CLOSING_DR_SUM"]);
                    if (dr["CLOSING_CR_SUM"] != DBNull.Value)
                        objTotals.ClosingBalCrSum = Convert.ToDouble(dr["CLOSING_CR_SUM"]);
                }
            }
            return lstTrialBalance;
        }
    }
}
