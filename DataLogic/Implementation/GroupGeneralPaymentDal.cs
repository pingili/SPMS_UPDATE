using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntities;
using BlackBeltCoder;
using System.Data.SqlClient;
using System.Data;

namespace DataLogic.Implementation
{
    public class GroupGeneralPaymentDal
    {
        public GroupGeneralPaymentDto GetGroupGeneralPaymentById(long accountMasterId)
        {
            GroupGeneralPaymentDto objGp = null;
            SqlConnection con = new SqlConnection(DBConstants.MFIS_CS);
            SqlCommand cmd = new SqlCommand("uspGeneralPaymentsGetById_NEW", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@AccountMasterId", accountMasterId);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                objGp = new GroupGeneralPaymentDto();
                objGp.AccountMasterID = accountMasterId;
                objGp.TransactionDate = Convert.ToDateTime(dr["TransactionDate"]);
                objGp.TransactionMode = Convert.ToString(dr["TransactionMode"]);
                objGp.VoucherNumber = Convert.ToString(dr["VoucherNumber"]);
                objGp.VoucherRefNumber = Convert.ToString(dr["VoucherRefNumber"]);
                objGp.CollectionAgent = Convert.ToInt32(dr["EmployeeID"]);
                objGp.CollectionAgentName = Convert.ToString(dr["EmployeeName"]);
                objGp.ChequeNumber = Convert.ToString(dr["ChequeNumber"]);
                if (!Convert.IsDBNull(dr["ChequeDate"]))
                    objGp.ChequeDate = Convert.ToDateTime(dr["ChequeDate"]);
                objGp.TotalAmount = Convert.ToDecimal(dr["Amount"]);
                if (!Convert.IsDBNull(dr["BankAccount"]))
                    objGp.BankEntryId = Convert.ToInt32(dr["BankAccount"]);
                objGp.ToAhNameForView = Convert.ToString(dr["ToAhName"]);
                objGp.Narration = Convert.ToString(dr["Narration"]);
            }
            if (dr.NextResult())
            {
                objGp.TransactionsList = new List<GroupGeneralPaymentTranDto>();
                GroupGeneralPaymentTranDto objTran = null;
                while (dr.Read())
                {
                    objTran = new GroupGeneralPaymentTranDto();
                    objTran.GLAccountId = Convert.ToInt32(dr["GLAhId"]);
                    objTran.SLAccountId = Convert.ToInt32(dr["SLAhId"]);
                    objTran.GLAccount = Convert.ToString(dr["GLAHNAME"]);
                    objTran.SLAccount = Convert.ToString(dr["SLAHNAME"]);
                    objTran.Amount = Convert.ToDecimal(dr["Amount"]);
                    objGp.TransactionsList.Add(objTran);
                }
            }
            return objGp;
        }

        public ResultDto AddUpdateGeneralPayment(GroupGeneralPaymentDto objDto, string generalPaymentTranxml, int userId, int groupId)
        {
            ResultDto res = new ResultDto();
            try
            {
                SqlConnection con = new SqlConnection(DBConstants.MFIS_CS);
                SqlCommand cmd = new SqlCommand("uspGroupGeneralPaymentInsertUpdate", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@TransactionMode", objDto.TransactionMode);
                cmd.Parameters.AddWithValue("@TransactionDate", objDto.TransactionDate);
                if (!string.IsNullOrWhiteSpace(objDto.VoucherRefNumber))
                    cmd.Parameters.AddWithValue("@VoucherRefNumber", objDto.VoucherRefNumber);
                cmd.Parameters.AddWithValue("@CollectionAgent", objDto.CollectionAgent);
                if (objDto.TransactionMode == "BC")
                {
                    cmd.Parameters.AddWithValue("@ChequeNumber", objDto.ChequeNumber);
                    cmd.Parameters.AddWithValue("@ChequeDate", objDto.ChequeDate);
                }
                cmd.Parameters.AddWithValue("@Amount", objDto.TotalAmount);
                if (objDto.TransactionMode != "C")
                    cmd.Parameters.AddWithValue("@BankEntryId", objDto.BankEntryId);

                cmd.Parameters.AddWithValue("@Narration", objDto.Narration);
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@GroupId", groupId);
                cmd.Parameters.AddWithValue("@TransactionXML", generalPaymentTranxml);


                SqlParameter prmAccountMasterId = new SqlParameter("@AccountMasterId", SqlDbType.Int);
                prmAccountMasterId.Value = objDto.AccountMasterID;
                prmAccountMasterId.Direction = ParameterDirection.InputOutput;
                cmd.Parameters.Add(prmAccountMasterId);

                SqlParameter prmVcoherNumber = new SqlParameter("@VoucherNumber", SqlDbType.VarChar, 32);
                prmVcoherNumber.Value = objDto.VoucherNumber;
                prmVcoherNumber.Direction = ParameterDirection.InputOutput;
                cmd.Parameters.Add(prmVcoherNumber);
                con.Open();
                cmd.ExecuteNonQuery();

                res.ObjectId = Convert.ToInt32(prmAccountMasterId.Value);
                res.ObjectCode = prmVcoherNumber.Value.ToString();
                con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return res;
        }

        public List<GeneralPaymentsLookupDto> GroupGeneralPaymentLookUp(int groupId,int userId)
        {
            List<GeneralPaymentsLookupDto> lstGroupGeneralPayments = new List<GeneralPaymentsLookupDto>();
            GeneralPaymentsLookupDto objGp = null;
            SqlConnection con = new SqlConnection(DBConstants.MFIS_CS);
            SqlCommand cmd = new SqlCommand("uspGroupGeneralPaymentsLookup", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@GroupID", groupId);
            cmd.Parameters.AddWithValue("@UserId", userId);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                objGp = new GeneralPaymentsLookupDto();
                objGp.AccountMasterID = Convert.ToInt64(dr["AccountMasterID"]);
                objGp.TransactionDate = Convert.ToDateTime(dr["TransactionDate"]);
                objGp.VoucherNumber = Convert.ToString(dr["VoucherNumber"]);
                objGp.AHName = Convert.ToString(dr["AHName"]);
                //objGp.MemberName = Convert.ToString(dr["MemberName"]);
                objGp.Amount = Convert.ToDecimal(dr["Amount"]);
                objGp.Narration = Convert.ToString(dr["Narration"]);
                objGp.StatusCode = Convert.ToString(dr["StatusCode"]);
                objGp.Status = Convert.ToString(dr["Status"]);
                objGp.IsEdit = Convert.ToBoolean(dr["CanEdit"]);
                objGp.IsDelete = Convert.ToBoolean(dr["CanDelete"]);
                objGp.LockStatus = Convert.ToString(dr["LockStatus"]);
                objGp.LockStatusCode = Convert.ToString(dr["LockStatusCode"]);

                lstGroupGeneralPayments.Add(objGp);
            }

            return lstGroupGeneralPayments;
        }
    }

    public class GroupMemberPaymentDal
    {
        public GroupMemberPaymentDto GetGroupMemberPaymentById(long accountMasterId)
        {
            GroupMemberPaymentDto objGp = null;
            SqlConnection con = new SqlConnection(DBConstants.MFIS_CS);
            SqlCommand cmd = new SqlCommand("uspMemberPaymentsGetById", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@AccountMasterId", accountMasterId);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                objGp = new GroupMemberPaymentDto();
                objGp.AccountMasterID = accountMasterId;
                objGp.TransactionDate = Convert.ToDateTime(dr["TransactionDate"]);
                objGp.TransactionMode = Convert.ToString(dr["TransactionMode"]);
                objGp.VoucherNumber = Convert.ToString(dr["VoucherNumber"]);
                objGp.VoucherRefNumber = Convert.ToString(dr["VoucherRefNumber"]);
                objGp.MemberId = Convert.ToInt32(dr["MemberID"]);
                objGp.MemberName = Convert.ToString(dr["MemberName"]);
                objGp.CollectionAgent = Convert.ToInt32(dr["EmployeeID"]);
                objGp.CollectionAgentName = Convert.ToString(dr["EmployeeName"]);
                objGp.ChequeNumber = Convert.ToString(dr["ChequeNumber"]);
                if (!Convert.IsDBNull(dr["ChequeDate"]))
                    objGp.ChequeDate = Convert.ToDateTime(dr["ChequeDate"]);
                objGp.TotalAmount = Convert.ToDecimal(dr["Amount"]);
                if (!Convert.IsDBNull(dr["BankAccount"]))
                    objGp.BankEntryId = Convert.ToInt32(dr["BankAccount"]);
                objGp.ToAhNameForView = Convert.ToString(dr["ToAhName"]);
                objGp.Narration = Convert.ToString(dr["Narration"]);
            }
            if (dr.NextResult())
            {
                objGp.TransactionsList = new List<GroupMemberPaymentTranDto>();
                GroupMemberPaymentTranDto objTran = null;
                while (dr.Read())
                {
                    objTran = new GroupMemberPaymentTranDto();
                    objTran.GLAccountId = Convert.ToInt32(dr["GLAhId"]);
                    objTran.SLAccountId = Convert.ToInt32(dr["SLAhId"]);
                    objTran.GLAccount = Convert.ToString(dr["GLAHNAME"]);
                    objTran.SLAccount = Convert.ToString(dr["SLAHNAME"]);
                    objTran.Amount = Convert.ToDecimal(dr["Amount"]);
                    objGp.TransactionsList.Add(objTran);
                }
            }
            return objGp;
        }

        public ResultDto AddUpdateGroupMemberPayment(GroupMemberPaymentDto objDto, string memberPaymentTranxml, int userId, int groupId)
        {
            ResultDto res = new ResultDto();
            try
            {
                SqlConnection con = new SqlConnection(DBConstants.MFIS_CS);
                SqlCommand cmd = new SqlCommand("uspGroupMemberPaymentInsertUpdate", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@TransactionMode", objDto.TransactionMode);
                cmd.Parameters.AddWithValue("@TransactionDate", objDto.TransactionDate);
                if (!string.IsNullOrWhiteSpace(objDto.VoucherRefNumber))
                    cmd.Parameters.AddWithValue("@VoucherRefNumber", objDto.VoucherRefNumber);
                cmd.Parameters.AddWithValue("@CollectionAgent", objDto.CollectionAgent);
                if (objDto.TransactionMode == "BC")
                {
                    cmd.Parameters.AddWithValue("@ChequeNumber", objDto.ChequeNumber);
                    cmd.Parameters.AddWithValue("@ChequeDate", objDto.ChequeDate);
                }
                cmd.Parameters.AddWithValue("@Amount", objDto.TotalAmount);
                if (objDto.TransactionMode != "C")
                    cmd.Parameters.AddWithValue("@BankEntryId", objDto.BankEntryId);

                cmd.Parameters.AddWithValue("@Narration", objDto.Narration);
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@GroupId", groupId);
                cmd.Parameters.AddWithValue("@MemberID", objDto.MemberId);
                cmd.Parameters.AddWithValue("@TransactionXML", memberPaymentTranxml);


                SqlParameter prmAccountMasterId = new SqlParameter("@AccountMasterId", SqlDbType.Int);
                prmAccountMasterId.Value = objDto.AccountMasterID;
                prmAccountMasterId.Direction = ParameterDirection.InputOutput;
                cmd.Parameters.Add(prmAccountMasterId);

                SqlParameter prmVcoherNumber = new SqlParameter("@VoucherNumber", SqlDbType.VarChar, 32);
                prmVcoherNumber.Value = objDto.VoucherNumber;
                prmVcoherNumber.Direction = ParameterDirection.InputOutput;
                cmd.Parameters.Add(prmVcoherNumber);
                con.Open();
                cmd.ExecuteNonQuery();

                res.ObjectId = Convert.ToInt32(prmAccountMasterId.Value);
                res.ObjectCode = prmVcoherNumber.Value.ToString();
                con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return res;
        }

        public List<GroupMemberPaymentLookupDto> GetGroupMemberPaymentLookup(int groupId,int userId)
        {
            List<GroupMemberPaymentLookupDto> lstMp = new List<GroupMemberPaymentLookupDto>();
            GroupMemberPaymentLookupDto objMp = null;
            SqlConnection con = new SqlConnection(DBConstants.MFIS_CS);
            SqlCommand cmd = new SqlCommand("uspGroupMemberPaymentsLookup", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@GroupId", groupId);
            cmd.Parameters.AddWithValue("@UserId", userId);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                objMp = new GroupMemberPaymentLookupDto();
                objMp.AccountMasterID = Convert.ToInt64(dr["AccountMasterID"]);
                objMp.TransactionDate = Convert.ToDateTime(dr["TransactionDate"]);
                objMp.VoucherNumber = Convert.ToString(dr["VoucherNumber"]);
                objMp.AHName = Convert.ToString(dr["AHName"]);
                objMp.MemberName = Convert.ToString(dr["MemberName"]);
                objMp.Amount = Convert.ToDecimal(dr["Amount"]);
                objMp.Narration = Convert.ToString(dr["Narration"]);
                objMp.StatusCode = Convert.ToString(dr["StatusCode"]);
                objMp.Status = Convert.ToString(dr["Status"]);
                objMp.IsEdit = Convert.ToBoolean(dr["CanEdit"]);
                objMp.IsDelete = Convert.ToBoolean(dr["CanDelete"]);
                objMp.LockStatus = Convert.ToString(dr["LockStatus"]);
                objMp.LockStatusCode = Convert.ToString(dr["LockStatusCode"]);

                lstMp.Add(objMp);
            }
            
            return lstMp;
        }
    }
}
