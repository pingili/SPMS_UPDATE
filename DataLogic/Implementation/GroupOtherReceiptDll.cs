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
    public class GroupOtherReceiptDll
    {
        public List<GroupMeetingDto> GetGroupOpenMeetingDates(int GroupId)
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

        public ResultDto InsertUpdateOtherReciept(GroupOtherRecieptDto objDto, int groupId, bool isContra)
        {
            ResultDto res = new ResultDto();
            try
            {
                SqlConnection con = new SqlConnection(DBConstants.MFIS_CS);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;
                cmd.CommandText = "uspGroupOtherRecieptInsertUpdate";
                cmd.Parameters.AddWithValue("@TransactionMode", objDto.TransactionMode);
                cmd.Parameters.AddWithValue("@TransactionDate", objDto.TransactionDate);
                if (!string.IsNullOrWhiteSpace(objDto.VoucherRefNumber))
                    cmd.Parameters.AddWithValue("@VoucherRefNumber", objDto.VoucherRefNumber);
                cmd.Parameters.AddWithValue("@CollectionAgent", objDto.CollectionAgent);
                cmd.Parameters.AddWithValue("@GLAccountId", objDto.GLAccountId);
                cmd.Parameters.AddWithValue("@SLAccountId", objDto.SLAccountId);
                if (objDto.TransactionMode == "BC")
                {
                    cmd.Parameters.AddWithValue("@ChequeNumber", objDto.ChequeNumber);
                    cmd.Parameters.AddWithValue("@ChequeDate", objDto.ChequeDate);
                }
                cmd.Parameters.AddWithValue("@Amount", objDto.Amount);
                if (objDto.TransactionMode != "C")
                    cmd.Parameters.AddWithValue("@BankEntryId", objDto.BankEntryId);
                if (objDto.TransactionMode == "C" && isContra == true)
                    cmd.Parameters.AddWithValue("@BankEntryId", objDto.BankEntryId);
                cmd.Parameters.AddWithValue("@Narration", objDto.Narration);
                cmd.Parameters.AddWithValue("@UserId", objDto.UserId);
                cmd.Parameters.AddWithValue("@GroupId", groupId);
                cmd.Parameters.AddWithValue("@IsContra", isContra);

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

        public List<GroupOtherReceiptLookUpDto> GroupOtherReceiptLookUp(int UserId, int GroupId)
        {
            List<GroupOtherReceiptLookUpDto> lstGroupOtherReceiptDto = new List<GroupOtherReceiptLookUpDto>();
            SqlConnection con = new SqlConnection(DBConstants.MFIS_CS);
            SqlCommand cmd = new SqlCommand(ProcNames.uspGroupGeneralReceiptLookup, con);
            cmd.Parameters.AddWithValue("@UserID", UserId);
            cmd.Parameters.AddWithValue("@GroupId", GroupId);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            GroupOtherReceiptLookUpDto OtherReceiptDto;
            while (dr.Read())
            {
                OtherReceiptDto = new GroupOtherReceiptLookUpDto();
                OtherReceiptDto.AccountMasterID = Convert.ToInt32(dr["AccountMasterID"]);
                OtherReceiptDto.AHName = Convert.ToString(dr["AHName"]);
                OtherReceiptDto.Amount = Convert.ToDecimal(dr["Amount"]);
                OtherReceiptDto.Narration = Convert.ToString(dr["Narration"]);
                OtherReceiptDto.TransactionDate = Convert.ToDateTime(dr["TransactionDate"]);
                OtherReceiptDto.TransactionMode = Convert.ToString(dr["TransactionMode"]);
                OtherReceiptDto.VoucherNumber = Convert.ToString(dr["VoucherNumber"]);
                OtherReceiptDto.VoucherRefNumber = Convert.ToString(dr["VoucherRefNumber"]);
                OtherReceiptDto.Status = Convert.ToString(dr["Status"]);
                OtherReceiptDto.StatusCode = Convert.ToString(dr["StatusCode"]);
                OtherReceiptDto.IsEdit = Convert.ToBoolean(dr["CanEdit"]);
                OtherReceiptDto.IsDelete = Convert.ToBoolean(dr["CanDelete"]);
                OtherReceiptDto.LockStatus = Convert.ToString(dr["LockStatus"]);
                OtherReceiptDto.LockStatusCode = Convert.ToString(dr["LockStatusCode"]);
                lstGroupOtherReceiptDto.Add(OtherReceiptDto);
            }
            con.Close();

            return lstGroupOtherReceiptDto;
        }

        public GroupOtherRecieptDto EditGroupOtherReceipt(int AccountMasterId)
        {
            SqlConnection con = new SqlConnection(DBConstants.MFIS_CS);
            SqlCommand cmd = new SqlCommand(ProcNames.GetGroupOtherReceiptById, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@AccountMasterId", AccountMasterId);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            GroupOtherRecieptDto OtherReceiptDto = null;
            if (dr.Read())
            {
                OtherReceiptDto = new GroupOtherRecieptDto();
                OtherReceiptDto.TransactionDate = Convert.ToDateTime(dr["TransactionDate"]);
                OtherReceiptDto.TransactionMode = Convert.ToString(dr["TransactionMode"]);
                OtherReceiptDto.VoucherNumber = Convert.ToString(dr["VoucherNumber"]);
                OtherReceiptDto.VoucherRefNumber = Convert.ToString(dr["VoucherRefNumber"]);
                OtherReceiptDto.CollectionAgent = Convert.ToInt32(dr["EmployeeID"]);
                OtherReceiptDto.ChequeNumber = Convert.ToString(dr["ChequeNumber"]);
                if (!Convert.IsDBNull(dr["ChequeDate"]))
                    OtherReceiptDto.ChequeDate = Convert.ToDateTime(dr["ChequeDate"]);
                OtherReceiptDto.Amount = Convert.ToDecimal(dr["Amount"]);
                if (!Convert.IsDBNull(dr["BankAccount"]))
                    OtherReceiptDto.BankEntryId = Convert.ToInt32(dr["BankAccount"]);
                OtherReceiptDto.SLAccountId = Convert.ToInt32(dr["AHID"]);
                OtherReceiptDto.GLAccountId = Convert.ToInt32(dr["ParentAHID"]);
                OtherReceiptDto.Narration = Convert.ToString(dr["Narration"]);
            }
            return OtherReceiptDto;
        }

        public ResultDto DeleteGroupOtherReceipt(int AccountMasterId, int UserId)
        {
            ResultDto dto = new ResultDto();
            SqlConnection con = new SqlConnection(DBConstants.MFIS_CS);
            SqlCommand cmd = new SqlCommand("uspDeleteGroupOtherReceipt", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@AccountMasterID", AccountMasterId);
            cmd.Parameters.AddWithValue("@UserId", UserId);
            SqlParameter prmVocherNumber = new SqlParameter("@VoucherNumber", SqlDbType.VarChar, 20);
            prmVocherNumber.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(prmVocherNumber);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            dto.ObjectId = i;
            return dto;
        }

        public GroupOtherReceiptViewDto GroupOtherReceiptView(int AccountMasterId)
        {
            SqlConnection con = new SqlConnection(DBConstants.MFIS_CS);
            GroupOtherReceiptViewDto viewdto = null;
            try
            {
                SqlCommand cmd = new SqlCommand(ProcNames.uspGroupOtherReceiptView, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AccountMasterId", AccountMasterId);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    viewdto = new GroupOtherReceiptViewDto();
                    viewdto.Amount = Convert.ToDecimal(dr["Amount"]);
                    if (dr["ChequeDate"] != DBNull.Value)
                        viewdto.ChequeDate = Convert.ToDateTime(dr["ChequeDate"]);
                    viewdto.ChequeNumber = Convert.ToString(dr["ChequeNumber"]);
                    viewdto.CollectionAgent = Convert.ToString(dr["EmployeeName"]);
                    viewdto.GLAccountName = Convert.ToString(dr["GlAccount"]);
                    viewdto.SLAccountName = Convert.ToString(dr["SlAccount"]);
                    viewdto.TransactionDate = Convert.ToDateTime(dr["TransactionDate"]);
                    viewdto.TransactionMode = Convert.ToString(dr["TransactionMode"]);
                    viewdto.VoucherNumber = Convert.ToString(dr["VoucherNumber"]);
                    viewdto.VoucherRefNumber = Convert.ToString(dr["VoucherRefNumber"]);
                    viewdto.BankName = Convert.ToString(dr["ToAccountHead"]);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return viewdto;

        }

        public GroupOtherRecieptUploadValidateInfo GetGroupOtherRecieptValidateInfo(int groupId)
        {
            SqlConnection con = new SqlConnection(DBConstants.MFIS_CS);
            GroupOtherRecieptUploadValidateInfo obj = new GroupOtherRecieptUploadValidateInfo();
            try
            {
                SqlCommand cmd = new SqlCommand("uspGroupOtherRecieptUploadValiateInfo", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@GroupId", groupId);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                obj.Employees = new List<UGOREmployeeDto>();
                while (dr.Read())
                {
                    obj.Employees.Add(new UGOREmployeeDto() { EmployeeID = Convert.ToInt32(dr["EmployeeID"]), EmployeeCode = Convert.ToString(dr["EmployeeCode"]) });
                }
                if (dr.NextResult())
                {
                    obj.GroupBanks = new List<UGORBankDto>();
                    while (dr.Read())
                    {
                        obj.GroupBanks.Add(new UGORBankDto() { BankEntryId = Convert.ToInt32(dr["BankEntryID"]), BankCode = Convert.ToString(dr["BankCode"]) });
                    }

                    if (dr.NextResult())
                    {
                        obj.GroupMeetings = new List<UGORMeetingDto>();
                        while (dr.Read())
                        {
                            obj.GroupMeetings.Add(new UGORMeetingDto() { MeetingDate = Convert.ToDateTime(dr["MeetingDate"]), isConducted = Convert.ToBoolean(dr["IsConducted"]) });
                        }

                        if (dr.NextResult())
                        {
                            obj.SlAccountHeads = new List<UGORAccountHeadDto>();

                            while (dr.Read())
                            {
                                obj.SlAccountHeads.Add(new UGORAccountHeadDto() { AHCode = Convert.ToString(dr["AHCode"]), AHId = Convert.ToInt32(dr["AHID"]) });
                            }

                            if (dr.NextResult())
                            {
                                if (dr.Read())
                                {
                                    obj.MeetingMonth = Convert.ToInt32(dr["MeetingMonth"]);
                                    obj.MeetingYear = Convert.ToInt32(dr["MeetingYear"]);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return obj;
        }
    }
}