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
    public class GroupMemberReceiptDal
    {
        public ResultDto AddUpdateMemberReceipt(GroupMemberReceiptDto objDto, string memberReceiptTranxml, int userId, int groupId)
        {
            ResultDto res = new ResultDto();
            try
            {
                SqlConnection con = new SqlConnection(DBConstants.MFIS_CS);
                SqlCommand cmd = new SqlCommand("uspGroupGroupMemberReceiptInsertUpdate", con);
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
                cmd.Parameters.AddWithValue("@TransactionXML", memberReceiptTranxml);
                cmd.Parameters.AddWithValue("@MemberId", objDto.MemberId);


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
        public List<GroupMeetingDto> GetMeetingDates(int GroupId)
        {
            List<GroupMeetingDto> lstgroupMeetingDto = new List<GroupMeetingDto>();
            SqlConnection con = new SqlConnection(DBConstants.MFIS_CS);
            SqlCommand cmd = new SqlCommand(ProcNames.uspGroupMeetingCurrentDates, con);
            cmd.Parameters.AddWithValue("@GroupId", GroupId);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            GroupMeetingDto groupMeetingDto;
            while (dr.Read())
            {
                groupMeetingDto = new GroupMeetingDto();
                groupMeetingDto.GroupMeetingID = Convert.ToInt32(dr["GroupMeetingID"]);
                groupMeetingDto.MeetingDate = Convert.ToDateTime(dr["MeetingDate"]);
                lstgroupMeetingDto.Add(groupMeetingDto);
            }
            con.Close();
            return lstgroupMeetingDto;
        }
        public List<EmployeeDto> GetEmployees()
        {
            List<EmployeeDto> lstEmployeesDto = new List<EmployeeDto>();
            SqlConnection con = new SqlConnection(DBConstants.MFIS_CS);
            SqlCommand cmd = new SqlCommand(ProcNames.uspEmployeeLookUp, con);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            EmployeeDto employeeDto;
            while (dr.Read())
            {
                employeeDto = new EmployeeDto();
                employeeDto.EmployeeID = Convert.ToInt32(dr["EmployeeID"]);
                employeeDto.EmployeeCode = Convert.ToString(dr["EmployeeCode"]);
                employeeDto.EmployeeName = Convert.ToString(dr["EmployeeName"]);
                lstEmployeesDto.Add(employeeDto);
            }
            con.Close();
            return lstEmployeesDto;
        }
        public List<MemberDto> GetMembersByGroupId(int GroupId)
        {
            List<MemberDto> lstMemberdto = new List<MemberDto>();
            SqlConnection con = new SqlConnection(DBConstants.MFIS_CS);
            SqlCommand cmd = new SqlCommand(ProcNames.uspMemberLookUp, con);
            cmd.Parameters.AddWithValue("@GroupId", GroupId);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            MemberDto memberDto;
            while (dr.Read())
            {
                memberDto = new MemberDto();
                memberDto.MemberID = Convert.ToInt32(dr["MemberID"]);
                memberDto.MemberName = Convert.ToString(dr["MemberName"]);
                memberDto.MemberCode = Convert.ToString(dr["MemberCode"]);
                lstMemberdto.Add(memberDto);
            }
            return lstMemberdto;
        }
        public List<GroupMemberReceiptTranDto> GetMemberReceiptTemplate()
        {
            List<GroupMemberReceiptTranDto> lstAccountHeadsTemplats = new List<GroupMemberReceiptTranDto>();
            SqlConnection con = new SqlConnection(DBConstants.MFIS_CS);
            SqlCommand cmd = new SqlCommand(ProcNames.uspGroupMemberAccountHeadTemplats, con);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            GroupMemberReceiptTranDto tempdto = null;
            while (dr.Read())
            {
                tempdto = new GroupMemberReceiptTranDto();
                tempdto.SLAccountId = Convert.ToInt32(dr["AHID"]);
                tempdto.GLAccount = Convert.ToString(dr["GLAccountHead"]);
                tempdto.SLAccount = Convert.ToString(dr["SLAccountHead"]);
                lstAccountHeadsTemplats.Add(tempdto);
            }

            return lstAccountHeadsTemplats;

        }
        public List<GroupMemberReceiptLookupDto> GroupMemberReceiptLookUp(int groupId, int userId)
        {
            List<GroupMemberReceiptLookupDto> lstMmbrRcpt = new List<GroupMemberReceiptLookupDto>();
            SqlConnection con = new SqlConnection(DBConstants.MFIS_CS);
            SqlCommand cmd = new SqlCommand(ProcNames.uspMemberReceiptLookup, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@GroupId", groupId);
            cmd.Parameters.AddWithValue("@UserId", userId);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            GroupMemberReceiptLookupDto recptDto = null;
            while (dr.Read())
            {
                recptDto = new GroupMemberReceiptLookupDto();
                recptDto.AccountMasterID = Convert.ToInt32(dr["AccountMasterID"]);
                recptDto.TransactionDate = Convert.ToDateTime(dr["TransactionDate"]);
                recptDto.VoucherNumber = Convert.ToString(dr["VoucherNumber"]);
                recptDto.Amount = Convert.ToDecimal(dr["Amount"]);
                recptDto.MemberName = Convert.ToString(dr["MemberName"]);
                recptDto.StatusCode = Convert.ToString(dr["StatusCode"]);
                recptDto.Narration = Convert.ToString(dr["Narration"]);
                recptDto.IsEdit = Convert.ToBoolean(dr["CanEdit"]);
                recptDto.IsDelete = Convert.ToBoolean(dr["CanDelete"]);
                recptDto.LockStatus = Convert.ToString(dr["LockStatus"]);
                recptDto.LockStatusCode = Convert.ToString(dr["LockStatusCode"]);
                lstMmbrRcpt.Add(recptDto);
            }
            return lstMmbrRcpt;
        }
        public List<GroupMemberDemandDto> GetMemberReceiptDemand(int MemberID, string TransactionDate)
        {
            List<GroupMemberDemandDto> lstMmbrRcpt = new List<GroupMemberDemandDto>();
            try
            {
                SqlConnection con = new SqlConnection(DBConstants.MFIS_CS);
                SqlCommand cmd = new SqlCommand(ProcNames.uspGetMemberReceiptDemands, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TransactionDate", TransactionDate);
                cmd.Parameters.AddWithValue("@MemberID", MemberID);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                GroupMemberDemandDto recptDto = null;
                while (dr.Read())
                {
                    recptDto = new GroupMemberDemandDto();

                    recptDto.Seq = Convert.ToInt32(dr["SEQ"]);
                    recptDto.GLAhId = Convert.ToInt32(dr["GLAhId"]);
                    recptDto.GLAhName = Convert.ToString(dr["GLAhName"]);
                    recptDto.SLAhId = Convert.ToInt32(dr["SLAhId"]);
                    recptDto.SLAhName = Convert.ToString(dr["SLAhName"]);
                    if (dr["SubAhId"] != DBNull.Value)
                    {
                        recptDto.SubAhId = Convert.ToInt32(dr["SubAhId"]);
                        recptDto.SubAhName = Convert.ToString(dr["SubAhName"]);
                        recptDto.LoanMasterId = Convert.ToInt32(dr["LoanMasterId"]);
                        recptDto.ReferenceNumber = Convert.ToString(dr["ReferenceNumber"]);
                    }
                    if (dr["Demand"] != DBNull.Value)
                        recptDto.Demand = Convert.ToInt32(dr["Demand"]);

                    lstMmbrRcpt.Add(recptDto);
                }
            }
            catch (Exception ex)
            {

            }
            return lstMmbrRcpt;
        }
        public GroupMemberReceiptDto EditGroupMemberReceipt(int AccountMasterId)
        {
            SqlConnection con = new SqlConnection(DBConstants.MFIS_CS);
            SqlCommand cmd = new SqlCommand(ProcNames.uspGetGroupMemberReceiptByAccountMasterId, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@AccountMasterId", AccountMasterId);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            GroupMemberReceiptDto recptDto = null;
            if (dr.Read())
            {
                recptDto = new GroupMemberReceiptDto();
                recptDto.AccountMasterID = AccountMasterId;
                recptDto.TransactionDate = Convert.ToDateTime(dr["TransactionDate"]);
                recptDto.TransactionMode = Convert.ToString(dr["TransactionMode"]);
                recptDto.VoucherNumber = Convert.ToString(dr["VoucherNumber"]);
                recptDto.VoucherRefNumber = Convert.ToString(dr["VoucherRefNumber"]);
                recptDto.TotalAmount = Convert.ToDecimal(dr["Amount"]);
                recptDto.CollectionAgent = Convert.ToInt32(dr["EmployeeID"]);
                recptDto.MemberId = Convert.ToInt32(dr["MemberId"]);
                if (!Convert.IsDBNull(dr["ChequeDate"]))
                    recptDto.ChequeDate = Convert.ToDateTime(dr["ChequeDate"]);
                recptDto.ChequeNumber = Convert.ToString(dr["ChequeNumber"]);
                if (!Convert.IsDBNull(dr["BankAccount"]))
                    recptDto.BankEntryId = Convert.ToInt32(dr["BankAccount"]);
                recptDto.Narration = Convert.ToString(dr["Narration"]);
            }
            if (dr.NextResult())
            {
                recptDto.Transactions = new List<GroupMemberReceiptTranDto>();
                GroupMemberReceiptTranDto objTran = null;
                while (dr.Read())
                {
                    objTran = new GroupMemberReceiptTranDto();
                    objTran.SLAccountId = Convert.ToInt32(dr["SLAHId"]);
                    if (dr["SubAhId"] != DBNull.Value)
                        objTran.SubAhId = Convert.ToInt32(dr["SubAhId"]);
                    objTran.ReferenceNumber = Convert.ToString(dr["ReferenceNumber"]);
                    objTran.Amount = Convert.ToDecimal(dr["CrAmount"]);
                    objTran.GLAccount = Convert.ToString(dr["GLAHName"]);
                    objTran.SLAccount = Convert.ToString(dr["SLAHName"]);

                    recptDto.Transactions.Add(objTran);
                }
            }
            return recptDto;
        }

        public GroupMemberReceiptViewDto GetGroupMemberReceiptViewDetails(int AccountMasterId)
        {
            SqlConnection con = new SqlConnection(DBConstants.MFIS_CS);
            SqlCommand cmd = new SqlCommand("uspGetGroupMemberReceiptView", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@AccountMasterId", AccountMasterId);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            GroupMemberReceiptViewDto recptDto = null;
            if (dr.Read())
            {
                recptDto = new GroupMemberReceiptViewDto();
                recptDto.MemberName = Convert.ToString(dr["MemberName"]);
                recptDto.TransactionMode = Convert.ToString(dr["TransactionMode"]);
                recptDto.TransactionDate = Convert.ToDateTime(dr["TransactionDate"]);
                recptDto.TotalAmount = Convert.ToDecimal(dr["Amount"]);
                recptDto.VoucherNumber = Convert.ToString(dr["VoucherNumber"]);
                recptDto.VoucherRefNumber = Convert.ToString(dr["VoucherRefNumber"]);
                recptDto.CollectionAgentName = Convert.ToString(dr["EmployeeName"]);
                recptDto.BankAccountHead = Convert.ToString(dr["BankAccount"]);
                if (!Convert.IsDBNull(dr["ChequeDate"]))
                    recptDto.ChequeDate = Convert.ToDateTime(dr["ChequeDate"]);
                recptDto.ChequeNumber = Convert.ToString(dr["ChequeNumber"]);
                recptDto.Narration = Convert.ToString(dr["Narration"]);
            }

            if (dr.NextResult())
            {
                recptDto.Transactions = new List<GroupMemberReceiptTranDto>();
                GroupMemberReceiptTranDto objTran = null;
                while (dr.Read())
                {
                    objTran = new GroupMemberReceiptTranDto();
                    objTran.Amount = Convert.ToDecimal(dr["Amount"]);
                    objTran.GLAccount = Convert.ToString(dr["GLAHName"]);
                    objTran.SLAccount = Convert.ToString(dr["SLAHName"]);

                    recptDto.Transactions.Add(objTran);
                }
            }
            return recptDto;
        }
    }
}
