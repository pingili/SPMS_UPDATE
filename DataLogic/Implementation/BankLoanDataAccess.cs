using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntities;
using System.Data;
using System.Data.SqlClient;
using BlackBeltCoder;
using Utilities;

namespace DataLogic
{
    public class BankLoanDataAccess
    {

        public ResultDto GenerateVoucher(int LoanMasterID, decimal LoanAmount, int BankEntryId, int UserID, char TransactionType, int GroupId, string Narration, DateTime ChequeDate, string chequenumer, long BankAcount)
        {
            ResultDto resultDto = new ResultDto();
            AdoHelper obj = new AdoHelper();
            SqlParameter[] parms = new SqlParameter[12];

            parms[0] = new SqlParameter("@LoanMasterId", LoanMasterID);
            parms[0].SqlDbType = System.Data.SqlDbType.Int;

            parms[1] = new SqlParameter("@Amount", LoanAmount);
            parms[1].SqlDbType = System.Data.SqlDbType.Decimal;

            parms[2] = new SqlParameter("@BankEntryId", BankEntryId);
            parms[2].SqlDbType = System.Data.SqlDbType.Int;

            parms[3] = new SqlParameter("@TransactionMode", TransactionType);
            parms[3].SqlDbType = System.Data.SqlDbType.Char;

            parms[4] = new SqlParameter("@VoucherNumber", "");
            parms[4].SqlDbType = System.Data.SqlDbType.VarChar;
            parms[4].Direction = ParameterDirection.Output;

            parms[5] = new SqlParameter("@ChequeNumber", chequenumer);
            parms[5].SqlDbType = System.Data.SqlDbType.VarChar;

            parms[6] = new SqlParameter("@GroupID", GroupId);
            parms[6].SqlDbType = System.Data.SqlDbType.Int;

            parms[7] = new SqlParameter("@ChequeDate", ChequeDate);
            parms[7].SqlDbType = System.Data.SqlDbType.Date;

            parms[8] = new SqlParameter("@BankAccount", BankAcount);
            parms[8].SqlDbType = System.Data.SqlDbType.BigInt;

            parms[9] = new SqlParameter("@Narration", Narration);
            parms[9].SqlDbType = System.Data.SqlDbType.VarChar;

            parms[10] = new SqlParameter("@IsGroup", false);
            parms[10].SqlDbType = System.Data.SqlDbType.Bit;

            parms[11] = new SqlParameter("@UserID", UserID);
            parms[11].SqlDbType = System.Data.SqlDbType.Bit;


            int count = obj.ExecNonQueryProc("uspdisbursementgenearatevoucher", parms);
            if (count > 0)
            {
                resultDto.ObjectCode = "TRUE";
                resultDto.Message = "Voucher Generated success fully";
            }
            else if (count == -1)
            {
                resultDto.ObjectCode = "FALSE";
                resultDto.Message = "Error occured while generating Voucher";
            }
            else
            {
                resultDto.ObjectCode = "FALSE";
                resultDto.Message = "Error occured while Savung Account Details";

            }
            resultDto.ObjectId = LoanMasterID;
            return resultDto;

        }

        public bool CheckLoanExisted(int grouID, int slAHID, string loanNumber)
        {
            bool IsLoanExisted = false;
            AdoHelper objAdo = new AdoHelper();

            SqlParameter[] parms = new SqlParameter[3];

            parms[0] = new SqlParameter("@GroupID", grouID);
            parms[0].SqlDbType = System.Data.SqlDbType.Int;

            parms[1] = new SqlParameter("@slAHID", slAHID);
            parms[1].SqlDbType = System.Data.SqlDbType.Int;

            parms[2] = new SqlParameter("@loanNumber", loanNumber);
            parms[2].SqlDbType = System.Data.SqlDbType.VarChar;
            parms[2].Direction = ParameterDirection.Output;

            int LoanExisted = Convert.ToInt32(objAdo.ExecScalarProc("uspCheckBankLoanExisted", parms));
            loanNumber = Convert.ToString(parms[2].Value);
            if (LoanExisted == 1)
            {
                IsLoanExisted = true;
            }
            return IsLoanExisted;

        }

        public MemberLoanApplicationViewDto GetMemberLoanApplicationViewDetails(int loanMasterId)
        {
            MemberLoanApplicationViewDto objLoanApplication = new MemberLoanApplicationViewDto();
            AdoHelper objAdo = new AdoHelper();

            SqlParameter[] parms = new SqlParameter[1];

            parms[0] = new SqlParameter("@LoanMasterID", loanMasterId);
            parms[0].SqlDbType = System.Data.SqlDbType.Int;

            SqlDataReader dr = objAdo.ExecDataReaderProc("uspGetMemberLoanApplicationView", parms);
            if (dr.Read())
            {
                objLoanApplication.LoanMasterId = Convert.ToInt32(dr["LoanMasterID"]);
                objLoanApplication.LoanCode = Convert.ToString(dr["LoanCode"]);
                objLoanApplication.MemberName = Convert.ToString(dr["MemberName"]);
                objLoanApplication.LoanApplicationDate = Convert.ToDateTime(dr["LoanApplicationDate"]);
                objLoanApplication.ProjectPurpose = Convert.ToString(dr["ProjectName"]);
                objLoanApplication.LoanPurpose = Convert.ToString(dr["LoanPurpose"]);
                objLoanApplication.LoanApplyAmount = Convert.ToInt32(dr["LoanAmountApplied"]);
                objLoanApplication.NoOfInstallmentsProposed = Convert.ToInt32(dr["NoofInstallmentsProposed"]);
                objLoanApplication.LoanPrincipalAccountHead = Convert.ToString(dr["PrincipalAccountHead"]);
                objLoanApplication.LoanRepaymentMode = Convert.ToString(dr["Mode"]);
                if (dr["AppLevel"] != DBNull.Value)
                    objLoanApplication.ApprovalLevel = Convert.ToInt32(dr["AppLevel"]);
                objLoanApplication.Status = Convert.ToString(dr["Status"]);
                objLoanApplication.StatusCode = Convert.ToString(dr["StatusCode"]);
                objLoanApplication.GroupApprovalComments = Convert.ToString(dr["GroupComments"]);
                objLoanApplication.ClusterApprovalComments = Convert.ToString(dr["ClusterComments"]);
                objLoanApplication.FederationApprovalComments = Convert.ToString(dr["FederationComments"]);
                if (dr["GroupActionDate"] != DBNull.Value)
                    objLoanApplication.GroupActionDate = Convert.ToDateTime(dr["GroupActionDate"]);
                if (dr["ClusterActionDate"] != DBNull.Value)
                    objLoanApplication.ClusterActionDate = Convert.ToDateTime(dr["ClusterActionDate"]);
                if (dr["FederationActionDate"] != DBNull.Value)
                    objLoanApplication.FederationActionDate = Convert.ToDateTime(dr["FederationActionDate"]);
                if (dr["DisbursementDate"] != DBNull.Value)
                {
                    objLoanApplication.LoanInterestAccountHead = Convert.ToString(dr["InterestAccountHead"]);
                    objLoanApplication.InterestRate = Convert.ToInt32(dr["ROI"]);
                    objLoanApplication.MonthlyDueDay = Convert.ToInt32(dr["DueDay"]);
                    objLoanApplication.DisbursementDate = Convert.ToDateTime(dr["DisbursementDate"]);
                    objLoanApplication.NoOfInstallment = Convert.ToInt32(dr["NoOfInstallments"]);
                    objLoanApplication.LoanReferenceNumber = Convert.ToString(dr["LoanRefNumber"]);
                    objLoanApplication.DisbursementAmount = Convert.ToDecimal(dr["DisbursedAmount"]);
                    objLoanApplication.InstallmentStartFrom = Convert.ToDateTime(dr["InstallmentStartFrom"]);
                    objLoanApplication.InstallmentClosingDate = Convert.ToDateTime(dr["LoanClosingDate"]);
                    objLoanApplication.MonthlyPrincipalDemand = Convert.ToInt32(dr["MonthlyPrincipalDemand"]);
                    objLoanApplication.ChequeNumber = Convert.ToString(dr["ChequeNumber"]);
                    if (dr["ChequeDate"] != DBNull.Value)
                        objLoanApplication.ChequeDate = Convert.ToDateTime(dr["ChequeDate"]);
                    objLoanApplication.BankAccountHead = Convert.ToString(dr["BankAccountHead"]);
                }
            }
            return objLoanApplication;
        }

        public BankLoanDto GetBankLoanApplicationDetailsById(int bankLoanId)
        {
            BankLoanDto objLoanApplication = new BankLoanDto();
            AdoHelper objAdo = new AdoHelper();

            SqlParameter[] parms = new SqlParameter[1];

            parms[0] = new SqlParameter("@BankLoanId", bankLoanId);
            parms[0].SqlDbType = System.Data.SqlDbType.Int;

            SqlDataReader dr = objAdo.ExecDataReaderProc("uspBankLoanApplicationGetByBankLoanID", parms);
            if (dr.Read())
            {
                objLoanApplication.BankLoanId = Convert.ToInt32(dr["BankLoanId"]);
                objLoanApplication.GroupId = Convert.ToInt32(dr["GroupId"]);
                objLoanApplication.LoanRequestDate = Convert.ToDateTime(dr["RequestDate"]);
                objLoanApplication.LoanAmountRequested = Convert.ToInt32(dr["RequestedAmount"]);
                objLoanApplication.NoofInstallments = Convert.ToByte(dr["NoOfInStallments"]);
                objLoanApplication.LoanAmountApproved = Convert.ToDecimal(dr["ApprovedAmount"]);
                objLoanApplication.LoanAmountApprovedDate = Convert.ToDateTime(dr["ApprovedDate"]);
                objLoanApplication.DisbursedAmount = Convert.ToDecimal(dr["DisbursementAmount"]);
                objLoanApplication.DisbursedDate = Convert.ToDateTime(dr["DisbursementDate"]);
                objLoanApplication.InterestRate = Convert.ToInt32(dr["InterestRate"]);
                objLoanApplication.BankEntryId = Convert.ToInt32(dr["BankEntryId"]);
                objLoanApplication.SLAHId = Convert.ToInt32(dr["SLAHID"]);
                objLoanApplication.ReferenceNumber = Convert.ToString(dr["ReferenceNumber"]);
                objLoanApplication.DueDate = Convert.ToDateTime(dr["DueDate"]);
                objLoanApplication.Status = Convert.ToString(dr["Status"]);
                objLoanApplication.LoanNumber = Convert.ToString(dr["LinkageNumber"]);
                objLoanApplication.Narration = Convert.ToString(dr["Narration"]);
                objLoanApplication.EMI = Convert.ToInt32(dr["EMI"]);
                
            }
            return objLoanApplication;
        }

        public List<BankLoanLookupDto> GetBankLoanLookup(int GroupID, int userId)
        {
            List<BankLoanLookupDto> lstLoans = new List<BankLoanLookupDto>();
            AdoHelper objAdo = new AdoHelper();

            SqlParameter[] parms = new SqlParameter[2];

            parms[0] = new SqlParameter("@GroupId", GroupID);
            parms[0].SqlDbType = System.Data.SqlDbType.Int;

            parms[1] = new SqlParameter("@UserId", userId);
            parms[1].SqlDbType = System.Data.SqlDbType.Int;

            SqlDataReader dr = objAdo.ExecDataReaderProc("uspBankLoanApplicationLookup", parms);
            BankLoanLookupDto objLoan = null;
            while (dr.Read())
            {
                objLoan = new BankLoanLookupDto();
                //objLoan.LoanMasterId = Convert.ToInt32(dr["LoanMasterID"]);
                //objLoan.LoanCode = Convert.ToString(dr["LoanCode"]);
                //objLoan.LoanType = Convert.ToString(dr["LoanType"]);
                //objLoan.MemberName = Convert.ToString(dr["MemberName"]);
                //objLoan.Purpose = Convert.ToString(dr["Purpose"]);
                //objLoan.LoanAmountApplied = Convert.ToInt32(dr["LoanAmountApplied"]);
                //if (dr["DisbursedAmount"] != DBNull.Value)
                //    objLoan.DisbursedAmount = Convert.ToDecimal(dr["DisbursedAmount"]);
                //if (dr["DisbursementDate"] != DBNull.Value)
                //    objLoan.DisbursementDate = Convert.ToDateTime(dr["DisbursementDate"]);
                //objLoan.Status = Convert.ToString(dr["Status"]);
                //objLoan.StatusCode = Convert.ToString(dr["StatusCode"]);
                //if (dr["AppLevel"] != DBNull.Value)
                //    objLoan.ApprovalLevel = Convert.ToInt32(dr["AppLevel"]);
                //objLoan.CanEdit = Convert.ToBoolean(dr["CanEdit"]);
                //objLoan.CanView = Convert.ToBoolean(dr["CanView"]);
                //objLoan.CanDelete = Convert.ToBoolean(dr["CanDelete"]);
                //lstLoans.Add(objLoan);
            }
            return lstLoans;
        }

        public ResultDto InsertUpdateBankLoanApplication(BankLoanDto obj)
        {
            ResultDto resultDto = new ResultDto();
            string obectName = "Loan Application";
            try
            {
                AdoHelper objAdo = new AdoHelper();
                SqlParameter[] parms = new SqlParameter[19];

                parms[0] = new SqlParameter("@BankLoanId", obj.BankLoanId);
                parms[0].SqlDbType = System.Data.SqlDbType.Int;
                parms[0].Direction = ParameterDirection.InputOutput;

                parms[1] = new SqlParameter("@GroupId", obj.GroupId);
                parms[1].SqlDbType = System.Data.SqlDbType.Int;

                parms[2] = new SqlParameter("@BankEntryId", obj.BankEntryId);
                parms[2].SqlDbType = System.Data.SqlDbType.Int;

                parms[3] = new SqlParameter("@LoanApplicationDate", obj.LoanRequestDate);
                parms[3].SqlDbType = System.Data.SqlDbType.Date;

                parms[4] = new SqlParameter("@Narration", obj.Narration);
                parms[4].SqlDbType = System.Data.SqlDbType.VarChar;

                parms[5] = new SqlParameter("@SLAHId", obj.SLAHId);
                parms[5].SqlDbType = System.Data.SqlDbType.Int;

                parms[6] = new SqlParameter("@LoanAmountApplied", obj.LoanAmountRequested);
                parms[6].SqlDbType = System.Data.SqlDbType.BigInt;

                parms[7] = new SqlParameter("@NoofInstallmentsProposed", obj.NoofInstallments);
                parms[7].SqlDbType = System.Data.SqlDbType.TinyInt;

                parms[8] = new SqlParameter("@LoanNumber", obj.LoanNumber);
                parms[8].SqlDbType = System.Data.SqlDbType.VarChar;

                parms[9] = new SqlParameter("@UserId", obj.UserID);
                parms[9].SqlDbType = System.Data.SqlDbType.Int;

                parms[10] = new SqlParameter("@InterstRate", obj.InterestRate);
                parms[10].SqlDbType = System.Data.SqlDbType.Int;

                parms[11] = new SqlParameter("@RefNumber", obj.ReferenceNumber);
                parms[11].SqlDbType = System.Data.SqlDbType.VarChar;

                parms[12] = new SqlParameter("@LoanAmountApprovedDate", obj.LoanAmountApprovedDate);
                parms[12].SqlDbType = System.Data.SqlDbType.DateTime;

                parms[13] = new SqlParameter("@LoanAmountApprovedAmount", obj.LoanAmountApproved);
                parms[13].SqlDbType = System.Data.SqlDbType.BigInt;

                parms[14] = new SqlParameter("@DisbursedDate", obj.DisbursedDate);
                parms[14].SqlDbType = System.Data.SqlDbType.DateTime;

                parms[15] = new SqlParameter("@DisbursedAmount", obj.DisbursedAmount);
                parms[15].SqlDbType = System.Data.SqlDbType.BigInt;

                parms[16] = new SqlParameter("@Status", SqlDbType.VarChar, 50);
                parms[16].Value = obj.Status;
                parms[16].Direction = ParameterDirection.Output;

                parms[17] = new SqlParameter("@EMI", SqlDbType.Int);
                parms[17].Value = obj.EMI;

                parms[18] = new SqlParameter("@DueDate", SqlDbType.Date);
                parms[18].Value = obj.DueDate;

                objAdo.ExecNonQueryProc("uspBankLoanInsertUpdate", parms);

                resultDto.ObjectId = Convert.ToInt32(parms[0].Value);
                //resultDto.ObjectCode = string.IsNullOrEmpty((string)parms[1].Value) ? obj.LoanCode : (string)parms[1].Value;
                obj.Status = (string)parms[16].Value;

                if (resultDto.ObjectId > 0)
                    resultDto.Message = string.Format("{0} details saved successfully", obectName);
                else if (resultDto.ObjectId == -1)
                    resultDto.Message = string.Format("Error occured while generating {0} code", obectName);
                else
                    resultDto.Message = string.Format("Error occured while saving {0} details", obectName);
            }
            catch (Exception ex)
            {
                resultDto.Message = string.Format("Service layer error occured while saving the {0} details", obectName);
                resultDto.ObjectId = -98;
            }
            return resultDto;
        }

        public int SubmitLoanApplicationApproval(BankLoanDto objbankloan, int userId, int loanMasterId, bool isSave)
        {
            AdoHelper objAdo = new AdoHelper();
            SqlParameter[] parms = new SqlParameter[9];

            parms[0] = new SqlParameter("@BankLoanId", objbankloan.BankLoanId);
            parms[0].SqlDbType = SqlDbType.Int;

            parms[1] = new SqlParameter("@UserId", userId);
            parms[1].SqlDbType = System.Data.SqlDbType.Int;


            parms[8] = new SqlParameter("@isSave", isSave);
            parms[8].SqlDbType = System.Data.SqlDbType.Bit;

            objAdo.ExecNonQueryProc("uspApproveMemberLoan", parms);

            int retVal = Convert.ToInt32(parms[7].Value);

            return retVal;
        }

        public List<MemberLoanApplicationLookupDto> MemberLoanDisbursementLookup(int groupID, int userId)
        {
            List<MemberLoanApplicationLookupDto> lstLoans = new List<MemberLoanApplicationLookupDto>();
            AdoHelper objAdo = new AdoHelper();

            SqlParameter[] parms = new SqlParameter[2];

            parms[0] = new SqlParameter("@GroupId", groupID);
            parms[0].SqlDbType = System.Data.SqlDbType.Int;

            parms[1] = new SqlParameter("@UserId", userId);
            parms[1].SqlDbType = System.Data.SqlDbType.Int;

            SqlDataReader dr = objAdo.ExecDataReaderProc("uspMemberLoanDisbursementLookup", parms);
            MemberLoanApplicationLookupDto objLoan = null;
            while (dr.Read())
            {
                objLoan = new MemberLoanApplicationLookupDto();
                objLoan.LoanMasterId = Convert.ToInt32(dr["LoanMasterID"]);
                objLoan.LoanCode = Convert.ToString(dr["LoanCode"]);
                objLoan.LoanType = Convert.ToString(dr["LoanType"]);
                objLoan.MemberName = Convert.ToString(dr["MemberName"]);
                objLoan.Purpose = Convert.ToString(dr["Purpose"]);
                objLoan.LoanAmountApplied = Convert.ToInt32(dr["LoanAmountApplied"]);
                if (dr["DisbursedAmount"] != DBNull.Value)
                    objLoan.DisbursedAmount = Convert.ToDecimal(dr["DisbursedAmount"]);
                if (dr["MDSanctionAmount"] != DBNull.Value)
                    objLoan.MDSanctionAmount = Convert.ToDecimal(dr["MDSanctionAmount"]);
                if (dr["DisbursementDate"] != DBNull.Value)
                    objLoan.DisbursementDate = Convert.ToDateTime(dr["DisbursementDate"]);
                objLoan.Status = Convert.ToString(dr["Status"]);
                objLoan.StatusCode = Convert.ToString(dr["StatusCode"]);
                if (dr["AppLevel"] != DBNull.Value)
                    objLoan.ApprovalLevel = Convert.ToInt32(dr["AppLevel"]);
                objLoan.CanEdit = Convert.ToBoolean(dr["CanEdit"]);
                objLoan.CanView = Convert.ToBoolean(dr["CanView"]);
                lstLoans.Add(objLoan);
            }
            return lstLoans;
        }


    }
}