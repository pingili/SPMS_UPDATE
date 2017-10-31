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
    public class ContraEntryDal
    {
        public List<ContraEntryDepositedLookupDto> GroupContraEntryDepositedLookup(int groupId, int userId)
        {
            List<ContraEntryDepositedLookupDto> lstContraDeposit = new List<ContraEntryDepositedLookupDto>();
            ContraEntryDepositedLookupDto objContraDeposit = null;

            SqlConnection con = new SqlConnection(DBConstants.MFIS_CS);
            SqlCommand cmd = new SqlCommand("uspGroupContraEntryDepositedLookup", con);
            cmd.Parameters.AddWithValue("@GroupId", groupId);
            cmd.Parameters.AddWithValue("@UserId", userId);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                objContraDeposit = new ContraEntryDepositedLookupDto();

                objContraDeposit.AccountMasterID = Convert.ToInt32(dr["AccountMasterID"]);
                objContraDeposit.VoucherNumber = Convert.ToString(dr["VoucherNumber"]);
                objContraDeposit.AHName = Convert.ToString(dr["AHName"]);
                objContraDeposit.TransactionDate = Convert.ToDateTime(dr["TransactionDate"]);
                objContraDeposit.Amount = Convert.ToDecimal(dr["Amount"]);
                objContraDeposit.Narration = Convert.ToString(dr["Narration"]);
                objContraDeposit.StatusCode = Convert.ToString(dr["StatusCode"]);
                objContraDeposit.Status = Convert.ToString(dr["Status"]);
                objContraDeposit.IsEdit = Convert.ToBoolean(dr["CanEdit"]);
                objContraDeposit.IsDelete = Convert.ToBoolean(dr["CanDelete"]);
                objContraDeposit.LockStatus = Convert.ToString(dr["LockStatus"]);
                objContraDeposit.LockStatusCode = Convert.ToString(dr["LockStatusCode"]);

                lstContraDeposit.Add(objContraDeposit);
            }

            return lstContraDeposit;
        }

        public List<ContraEntryWithDrawlLookupDto> GroupContraEntryWithDrawlLookup(int groupId, int userId)
        {
            List<ContraEntryWithDrawlLookupDto> lstContraWithDraw = new List<ContraEntryWithDrawlLookupDto>();
            ContraEntryWithDrawlLookupDto objContraWithDraw = null;

            SqlConnection con = new SqlConnection(DBConstants.MFIS_CS);
            SqlCommand cmd = new SqlCommand("uspGroupContraEntryWithDrawlLookup", con);
            cmd.Parameters.AddWithValue("@GroupId", groupId);
            cmd.Parameters.AddWithValue("@UserId", userId);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                objContraWithDraw = new ContraEntryWithDrawlLookupDto();

                objContraWithDraw.AccountMasterID = Convert.ToInt32(dr["AccountMasterID"]);
                objContraWithDraw.VoucherNumber = Convert.ToString(dr["VoucherNumber"]);
                objContraWithDraw.AHName = Convert.ToString(dr["AHName"]);
                objContraWithDraw.TransactionDate = Convert.ToDateTime(dr["TransactionDate"]);
                objContraWithDraw.Amount = Convert.ToDecimal(dr["Amount"]);
                objContraWithDraw.Narration = Convert.ToString(dr["Narration"]);
                objContraWithDraw.StatusCode = Convert.ToString(dr["StatusCode"]);
                objContraWithDraw.Status = Convert.ToString(dr["Status"]);
                objContraWithDraw.IsEdit = Convert.ToBoolean(dr["CanEdit"]);
                objContraWithDraw.IsDelete = Convert.ToBoolean(dr["CanDelete"]);
                objContraWithDraw.LockStatus = Convert.ToString(dr["LockStatus"]);
                objContraWithDraw.LockStatusCode = Convert.ToString(dr["LockStatusCode"]);

                lstContraWithDraw.Add(objContraWithDraw);
            }

            return lstContraWithDraw;
        }

    }
}
