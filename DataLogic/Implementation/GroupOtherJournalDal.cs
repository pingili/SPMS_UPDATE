using BusinessEntities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLogic.Implementation
{
    public class GroupJournalDal
    {
        public ResultDto AddGroupOtherJournal(string groupOtherJrnlTranxml, GroupJournalDto grpOthrJrnlDto, int GroupId, int UserId)
        {
            ResultDto res = new ResultDto();
            try
            {
                SqlConnection con = new SqlConnection(DBConstants.MFIS_CS);
                SqlCommand cmd = new SqlCommand("uspGroupOtherJournalInsertUpdate", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserID", UserId);
                cmd.Parameters.AddWithValue("@TransactionDate", grpOthrJrnlDto.TransactionDate);
                if (!string.IsNullOrWhiteSpace(grpOthrJrnlDto.VocherRefNumber))
                    cmd.Parameters.AddWithValue("@VoucherRefNumber", grpOthrJrnlDto.VocherRefNumber);
                cmd.Parameters.AddWithValue("@SLAccountId", grpOthrJrnlDto.FSLAccountId);
                cmd.Parameters.AddWithValue("@CollectionAgent", grpOthrJrnlDto.OnBehalfOfEmpId);
                cmd.Parameters.AddWithValue("@Amount", grpOthrJrnlDto.FAmount);
                cmd.Parameters.AddWithValue("@IsMasterCr", grpOthrJrnlDto.CrDr == "CR");
                cmd.Parameters.AddWithValue("@Narration", grpOthrJrnlDto.Narration);
                cmd.Parameters.AddWithValue("@GroupId", GroupId);
                cmd.Parameters.AddWithValue("@TransactionXML", groupOtherJrnlTranxml);
                if (grpOthrJrnlDto.MemberId > 0)
                    cmd.Parameters.AddWithValue("@MemberId", grpOthrJrnlDto.MemberId);

                SqlParameter prmAccountMasterId = new SqlParameter("@AccountMasterId", SqlDbType.Int);
                prmAccountMasterId.Value = grpOthrJrnlDto.AccountMasterID;
                prmAccountMasterId.Direction = ParameterDirection.InputOutput;
                cmd.Parameters.Add(prmAccountMasterId);

                SqlParameter prmVcoherNumber = new SqlParameter("@VoucherNumber", SqlDbType.VarChar, 32);
                prmVcoherNumber.Value = grpOthrJrnlDto.VoucherNumber;
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

        public List<SelectListDto> GetSLAccountHeads(int GLAccountId)
        {
            List<SelectListDto> lstdto = new List<SelectListDto>();
            try
            {
                SqlConnection con = new SqlConnection(DBConstants.MFIS_CS);
                SqlCommand cmd = new SqlCommand(ProcNames.uspGetSLAccountsByGLAccountId, con);
                cmd.Parameters.AddWithValue("@GLAccountId", GLAccountId);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                SelectListDto selectListDto;
                while (dr.Read())
                {
                    selectListDto = new SelectListDto();
                    selectListDto.ID = Convert.ToInt32(dr["AHID"]);
                    selectListDto.Text = Convert.ToString(dr["AHName"]);
                    lstdto.Add(selectListDto);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstdto;

        }

        public GroupJournalDto GetGroupJournalDetailsByID(int accountMasterId)
        {
            GroupJournalDto objJournal = null;
            SqlConnection con = new SqlConnection(DBConstants.MFIS_CS);
            SqlCommand cmd = new SqlCommand("uspGroupJournalDetailsByID", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@AccountMasterId", accountMasterId);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                objJournal = new GroupJournalDto();
                objJournal.AccountMasterID = accountMasterId;
                objJournal.TransactionDate = Convert.ToDateTime(dr["TransactionDate"]);
                objJournal.VocherRefNumber = Convert.ToString(dr["VoucherRefNumber"]);
                objJournal.OnBehalfOfEmpId = Convert.ToInt32(dr["OnBehalfOfEmpID"]);
                objJournal.OnBehalfOfEmpName = Convert.ToString(dr["OnBehalfOfEmpName"]);
                objJournal.VoucherNumber = Convert.ToString(dr["VoucherNumber"]);
                objJournal.EmployeeName = Convert.ToString(dr["EntryEmpName"]);
                objJournal.Narration = Convert.ToString(dr["Narration"]);
                objJournal.FAmount = Convert.ToDecimal(dr["Amount"]);
                objJournal.FGLAccountId = Convert.ToInt32(dr["GLAccountID"]);
                objJournal.FSLAccountId = Convert.ToInt32(dr["SLAccountID"]);

                objJournal.FGLAccountName = Convert.ToString(dr["GLAccount"]);
                objJournal.FSLAccountName = Convert.ToString(dr["SLAccount"]);
                objJournal.CrDr = Convert.ToString(dr["CrDr"]);
                if (dr["MemberID"] != DBNull.Value)
                {
                    objJournal.MemberId = Convert.ToInt32(dr["MemberID"]);
                    objJournal.MemberName = Convert.ToString(dr["MemberName"]);
                }
            }
            if (dr.NextResult())
            {
                objJournal.TransactionsList = new List<GroupJournalTranDto>();
                GroupJournalTranDto objTran = null;
                while (dr.Read())
                {
                    objTran = new GroupJournalTranDto();
                    objTran.GLAccountId = Convert.ToInt32(dr["GLAhId"]);
                    objTran.SLAccountId = Convert.ToInt32(dr["SLAhId"]);
                    objTran.GLAccount = Convert.ToString(dr["GLAHNAME"]);
                    objTran.SLAccount = Convert.ToString(dr["SLAHNAME"]);
                    objTran.Amount = Convert.ToDecimal(dr["Amount"]);
                    objJournal.TransactionsList.Add(objTran);
                }
            }
            return objJournal;
        }

        public List<GroupJournalLookUpDto> GetGroupJournalLookup(int groupId,int userId, bool isMemberJournal)
        {
            List<GroupJournalLookUpDto> lstJournal = new List<GroupJournalLookUpDto>();
            GroupJournalLookUpDto objJournal = null;

            SqlConnection con = new SqlConnection(DBConstants.MFIS_CS);
            SqlCommand cmd = new SqlCommand("uspGetGroupJournalLookup", con);
            cmd.Parameters.AddWithValue("@GroupId", groupId);
            cmd.Parameters.AddWithValue("@UserId", userId);
            cmd.Parameters.AddWithValue("@IsMemberJournal", isMemberJournal);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                objJournal = new GroupJournalLookUpDto();

                objJournal.AccountMasterID = Convert.ToInt32(dr["AccountMasterID"]);
                objJournal.VoucherNumber = Convert.ToString(dr["VoucherNumber"]);
                objJournal.MemberName = Convert.ToString(dr["MemberName"]);
                objJournal.AHName = Convert.ToString(dr["AHName"]);
                objJournal.TransactionDate = Convert.ToDateTime(dr["TransactionDate"]);
                objJournal.Amount = Convert.ToDecimal(dr["Amount"]);
                objJournal.Narration = Convert.ToString(dr["Narration"]);
                objJournal.StatusCode = Convert.ToString(dr["StatusCode"]);
                objJournal.Status = Convert.ToString(dr["Status"]);
                objJournal.AmountTranMode = Convert.ToString(dr["AmountTranType"]);
                objJournal.IsEdit = Convert.ToBoolean(dr["CanEdit"]);
                objJournal.IsDelete = Convert.ToBoolean(dr["CanDelete"]);
                objJournal.LockStatus = Convert.ToString(dr["LockStatus"]);
                objJournal.LockStatusCode = Convert.ToString(dr["LockStatusCode"]);

                lstJournal.Add(objJournal);
            }

            return lstJournal;
        }
    }
}