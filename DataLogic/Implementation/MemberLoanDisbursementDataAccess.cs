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
    public class MemberLoanDisbursementDataAccess
    {
        public InterestMasterDto GetInterestRatesByID(int GroupID, int PrincipleAHID)
        {

            InterestMasterDto objInterestRates = null;

            AdoHelper obj = new AdoHelper();
            SqlParameter[] parms = new SqlParameter[2];

            parms[0] = new SqlParameter("@GroupId", GroupID);
            parms[0].SqlDbType = System.Data.SqlDbType.Int;

            parms[1] = new SqlParameter("@PrincipleAHID", PrincipleAHID);
            parms[1].SqlDbType = System.Data.SqlDbType.Int;

            SqlDataReader Dr = obj.ExecDataReaderProc("uspGetInterestRates", parms);
            if (Dr.Read())
            {
                objInterestRates = new InterestMasterDto();
                if (!string.IsNullOrEmpty(Convert.ToString(Dr["PenalAHID"])))
                    objInterestRates.PenalAHID = Convert.ToInt32(Dr["PenalAHID"]);
                objInterestRates.InterestAHID = Convert.ToInt32(Dr["InterestAHID"]);
                objInterestRates.InterestRate = Convert.ToDecimal(Dr["ROI"]);
                objInterestRates.DueDay = Convert.ToInt32(Dr["DueDay"]);
                objInterestRates.InterestID = Convert.ToInt32(Dr["InterestID"]);
                objInterestRates.PenalROI = Convert.ToDecimal(Dr["PROI"]);
            }

            return objInterestRates;

        }
        public List<ScheduleDTO> GetSchedules(decimal LoanAmount, decimal interestrate, int loanperiod, DateTime StartPaymentDate)
        {
            List<ScheduleDTO> lstSchedules = new List<ScheduleDTO>();
            ScheduleDTO objSchedule = null;
            AdoHelper obj = new AdoHelper();
            SqlParameter[] parms = new SqlParameter[4];

            parms[0] = new SqlParameter("@LoanAmount", LoanAmount);
            parms[0].SqlDbType = System.Data.SqlDbType.Decimal;

            parms[1] = new SqlParameter("@InterestRate", interestrate);
            parms[1].SqlDbType = System.Data.SqlDbType.Decimal;

            parms[2] = new SqlParameter("@LoanPeriod", loanperiod);
            parms[2].SqlDbType = System.Data.SqlDbType.Int;

            parms[3] = new SqlParameter("@StartPaymentDate", StartPaymentDate);
            parms[3].SqlDbType = System.Data.SqlDbType.DateTime;

            SqlDataReader Dr = obj.ExecDataReaderProc("uspGetEMINew", parms);
            while (Dr.Read())
            {
                objSchedule = new ScheduleDTO();

                objSchedule.PERIOD = Convert.ToInt64(Dr["PERIOD"]);
                objSchedule.Opening_Balance = Convert.ToDecimal(Dr["OPENING_BALANCE"]);
                objSchedule.PAYDATE = Convert.ToDateTime(Dr["PAYDATE"]);
                objSchedule.PAYMENT = Convert.ToDecimal(Dr["PAYMENT"]);
                objSchedule.PRINCIPAL = Convert.ToDecimal(Dr["PRINCIPAL"]);
                objSchedule.DAYS = Convert.ToInt32(Dr["DAYS"]);
                objSchedule.INTEREST = Convert.ToDecimal(Dr["INTEREST"]);
                objSchedule.CURRENT_BALANCE = Convert.ToDecimal(Dr["CURRENT_BALANCE"]);
                objSchedule.INTERESTRate = interestrate;
                lstSchedules.Add(objSchedule);
            }
            return lstSchedules;
        }
        public GroupLoanDisbursementDto GetDisbursement(int loanMasterID)
        {
            GroupLoanDisbursementDto groupLoanDisbursementDto = new GroupLoanDisbursementDto();
            try
            {
                AdoHelper objAdo = new AdoHelper();

                SqlParameter[] parms = new SqlParameter[1];

                parms[0] = new SqlParameter("@LoanMasterID", loanMasterID);
                parms[0].SqlDbType = System.Data.SqlDbType.Int;


                SqlDataReader Dr = objAdo.ExecDataReaderProc("uspGetLoanDisbursement", parms);
                while (Dr.Read())
                {
                    if (DBNull.Value != Dr["DisbursedAmount"])
                    {
                        groupLoanDisbursementDto.DisbursedAmount = Convert.ToInt32(Dr["DisbursedAmount"]);
                        groupLoanDisbursementDto.NoOfInstallments = Convert.ToByte(Dr["NoofInstallmentsProposed"]);
                        groupLoanDisbursementDto.MonthlyPrincipalDemand = Convert.ToDecimal(Dr["MonthlyPrincipalDemand"]);
                        groupLoanDisbursementDto.OutStandingAmount = Convert.ToDecimal(Dr["OutStandingAmount"]);
                        if (!String.IsNullOrEmpty(Dr["PrincipalAHID"].ToString()))
                            groupLoanDisbursementDto.PrincipleAHId = Convert.ToInt32(Dr["PrincipalAHID"]);
                        if (!String.IsNullOrEmpty(Dr["InterestAHID"].ToString()))
                            groupLoanDisbursementDto.InterestAHID = Convert.ToInt32(Dr["InterestAHID"]);
                        groupLoanDisbursementDto.SLAccountNumber = Convert.ToInt32(Dr["SLAccountNumber"]);
                        groupLoanDisbursementDto.SLAccountName = Convert.ToString(Dr["SLAccountName"]);
                        groupLoanDisbursementDto.InterestRateID = Convert.ToInt32(Dr["InterestRateID"]);
                        groupLoanDisbursementDto.DisbursementDate = Convert.ToDateTime(Dr["DisbursementDate"]);
                        groupLoanDisbursementDto.InstallmentStartFrom = Convert.ToDateTime(Dr["InstallmentStartFrom"]);
                        groupLoanDisbursementDto.LoanClosingDate = Convert.ToDateTime(Dr["LoanClosingDate"]);
                        groupLoanDisbursementDto.FundSourceID = Dr["FundSourceID"] == DBNull.Value ? 0 : Convert.ToInt32(Dr["FundSourceID"]);
                        groupLoanDisbursementDto.ProjectID = Convert.ToInt32(Dr["ProjectID"]);
                        groupLoanDisbursementDto.LoanRefNumber = Convert.ToString(Dr["LoanRefNumber"]);
                        groupLoanDisbursementDto.ROI = Convert.ToDecimal(Dr["ROI"]);
                        groupLoanDisbursementDto.InterestMasterID = Convert.ToInt32(Dr["InterestMasterID"]);
                    }
                    groupLoanDisbursementDto.MeetingDay = Convert.ToInt32(Dr["MeetingDay"]);
                    groupLoanDisbursementDto.NoofInstallmentsProposed = Convert.ToByte(Dr["NoofInstallmentsProposed"]);
                    groupLoanDisbursementDto.LoanAmountApplied = Convert.ToDecimal(Dr["LoanAmountApplied"]);
                    groupLoanDisbursementDto.LoanMasterId = Convert.ToInt32(Dr["LoanMasterId"]);
                    groupLoanDisbursementDto.GroupID = DBNull.Value == Dr["GroupID"] ? 0 : Convert.ToInt32(Dr["GroupID"]);
                    groupLoanDisbursementDto.LoanCode = Convert.ToString(Dr["LoanCode"]);
                    groupLoanDisbursementDto.Groupcode = Convert.ToString(Dr["GroupCode"]);
                    groupLoanDisbursementDto.GroupName = Convert.ToString(Dr["GroupName"]);
                    groupLoanDisbursementDto.LoanApplicationDate = Convert.ToDateTime(Dr["LoanApplicationDate"]);
                    groupLoanDisbursementDto.LoanPurpose = Convert.ToString(Dr["PurposeName"]);
                    groupLoanDisbursementDto.Mode = Convert.ToString(Dr["ModeName"]);

                }
            }
            catch (Exception ex)
            {

            }
            return groupLoanDisbursementDto;
        }

        public ResultDto CreateSchedules(int LoanMasterId, decimal LoanAmount, decimal interestrate, int loanperiod, DateTime StartPaymentDate, int CurrentUserID, int PROI)
        {

            ResultDto objResult = new ResultDto();
            AdoHelper obj = new AdoHelper();
            SqlParameter[] parms = new SqlParameter[7];

            parms[0] = new SqlParameter("@LoanAmount", LoanAmount);
            parms[0].SqlDbType = System.Data.SqlDbType.Decimal;

            parms[1] = new SqlParameter("@InterestRate", interestrate);
            parms[1].SqlDbType = System.Data.SqlDbType.Decimal;

            parms[2] = new SqlParameter("@LoanPeriod", loanperiod);
            parms[2].SqlDbType = System.Data.SqlDbType.Int;

            parms[3] = new SqlParameter("@StartPaymentDate", StartPaymentDate);
            parms[3].SqlDbType = System.Data.SqlDbType.DateTime;

            parms[4] = new SqlParameter("@LoanMasterID", LoanMasterId);
            parms[4].SqlDbType = System.Data.SqlDbType.Int;

            parms[5] = new SqlParameter("@UserId", CurrentUserID);
            parms[5].SqlDbType = System.Data.SqlDbType.Int;

            parms[6] = new SqlParameter("@PenelInterestRate", PROI);
            parms[6].SqlDbType = System.Data.SqlDbType.Int;

            int i = obj.ExecNonQueryProc("uspCreateSchedule", parms);

            if (i > 0)
            {
                objResult.ObjectCode = "TRUE";
                objResult.Message = "Schedule Inserted Successfullly";
                objResult.ObjectId = LoanMasterId;

            }
            else
            {

                objResult.ObjectCode = "FALSE";
                objResult.Message = "Fail To Insert Schedles";
                objResult.ObjectId = LoanMasterId;

            }
            return objResult;


        }
        public bool CheckLoanDisbursed(int LoanMasterId)
        {

            bool Isdisbursed = false;
            AdoHelper obj = new AdoHelper();
            SqlParameter[] parms = new SqlParameter[2];

            parms[0] = new SqlParameter("@LoanMasterId", LoanMasterId);
            parms[0].SqlDbType = System.Data.SqlDbType.Int;

            parms[1] = new SqlParameter("@Flag", "CheckLoanDisbursed");
            parms[1].SqlDbType = System.Data.SqlDbType.VarChar;


            int i = Convert.ToInt16(obj.ExecScalarProc("uspGetInterestRates", parms));
            if (i > 0)
            {
                Isdisbursed = true;
            }
            return Isdisbursed;
        }
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
        public ResultDto SaveSecurityDetails(DataTable dt)
        {

            ResultDto objResult = new ResultDto();

            AdoHelper obj = new AdoHelper();
            SqlParameter[] parms = new SqlParameter[1];

            parms[0] = new SqlParameter("@DataTable", dt);
            parms[0].SqlDbType = System.Data.SqlDbType.Structured;

            int i = Convert.ToInt16(obj.ExecNonQueryProc("uspInsertAdditionalSecurityDetails", parms));
            if (i > 0)
            {
                objResult.ObjectCode = "TRUE";
                objResult.Message = "Additional Security Details Inserted Successfullly";
                objResult.ObjectId = 0;

            }
            else
            {

                objResult.ObjectCode = "FALSE";
                objResult.Message = "Fail To Insert Additional Security Details";
                objResult.ObjectId = 0;

            }
            return objResult;


        }
        public ResultDto GuarantorDetails(DataTable dt)
        {
            ResultDto objResult = new ResultDto();

            AdoHelper obj = new AdoHelper();
            SqlParameter[] parms = new SqlParameter[1];

            parms[0] = new SqlParameter("@DataTable", dt);
            parms[0].SqlDbType = System.Data.SqlDbType.Structured;

            int i = Convert.ToInt16(obj.ExecNonQueryProc("uspInsertGuarantorDetails", parms));
            if (i > 0)
            {
                objResult.ObjectCode = "TRUE";
                objResult.Message = "Additional Security Details Inserted Successfullly";
                objResult.ObjectId = 0;

            }
            else
            {

                objResult.ObjectCode = "FALSE";
                objResult.Message = "Fail To Insert Additional Security Details";
                objResult.ObjectId = 0;

            }
            return objResult;
        }
        public string GetSecurityName(int SecurityCode)
        {
            ResultDto objResult = new ResultDto();
            string SecurityNAme = "";
            AdoHelper obj = new AdoHelper();
            SqlParameter[] parms = new SqlParameter[1];

            parms[0] = new SqlParameter("@SecurityCode", SecurityCode);
            parms[0].SqlDbType = System.Data.SqlDbType.Int;

            SqlDataReader Dr = obj.ExecDataReaderProc("uspgetSecurityName", parms);
            if (Dr.Read())
            {
                SecurityNAme = Convert.ToString(Dr["LoanSecurityName"]);
            }
            return SecurityNAme;

        }
        public bool CheckLoanExisted(int memberId, int interestId)
        {

            AdoHelper obj = new AdoHelper();
            SqlParameter[] parms = new SqlParameter[2];

            parms[0] = new SqlParameter("@MemberID", memberId);
            parms[0].SqlDbType = System.Data.SqlDbType.Int;

            parms[1] = new SqlParameter("@InterestId", interestId);
            parms[1].SqlDbType = System.Data.SqlDbType.Int;

            object isLoanExists = (obj.ExecScalarProc("uspisexistedLoan", parms));
            bool loangivenflag = isLoanExists != null ? Convert.ToBoolean(isLoanExists) : false;

            return loangivenflag;

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

        public MemberLoanApplicationDto GetMemberLoanApplicationDetailsById(int loanMasterId)
        {
            MemberLoanApplicationDto objLoanApplication = new MemberLoanApplicationDto();
            AdoHelper objAdo = new AdoHelper();

            SqlParameter[] parms = new SqlParameter[1];

            parms[0] = new SqlParameter("@LoanMasterID", loanMasterId);
            parms[0].SqlDbType = System.Data.SqlDbType.Int;

            SqlDataReader dr = objAdo.ExecDataReaderProc("uspMemberLoanApplicationGetByLoanMasterID", parms);
            if (dr.Read())
            {

                objLoanApplication.LoanMasterId = Convert.ToInt32(dr["LoanMasterID"]);
                objLoanApplication.LoanCode = Convert.ToString(dr["LoanCode"]);
                objLoanApplication.LoanType = Convert.ToString(dr["LoanType"]);

                objLoanApplication.MemberID = Convert.ToInt32(dr["MemberID"]);
                objLoanApplication.MemberName = Convert.ToString(dr["MemberName"]);
                objLoanApplication.LoanApplicationDate = Convert.ToDateTime(dr["LoanApplicationDate"]);
                objLoanApplication.LoanPurpose = Convert.ToInt32(dr["LoanPurpose"]);
                objLoanApplication.LoanAmountApplied = Convert.ToInt32(dr["LoanAmountApplied"]);
                objLoanApplication.NoofInstallmentsProposed = Convert.ToByte(dr["NoofInstallmentsProposed"]);
                objLoanApplication.Mode = Convert.ToInt32(dr["Mode"]);

                objLoanApplication.ProjectID = Convert.ToInt32(dr["ProjectID"]);
                objLoanApplication.InterestMasterID = Convert.ToInt32(dr["InterestMasterID"]);
                objLoanApplication.InterestRateID = Convert.ToInt32(dr["GroupInterstRateID"]);
                objLoanApplication.PrincipleAHName = Convert.ToString(dr["PrincipalAHName"]);
                objLoanApplication.InterestAHName = Convert.ToString(dr["InterestAHName"]);
                objLoanApplication.ROI = Convert.ToInt32(dr["ROI"]);
                objLoanApplication.ReferenceNumber = Convert.ToString(dr["LoanRefNumber"]);
                objLoanApplication.SourceOfFund = Convert.ToString(dr["SourceOfFund"]);
                objLoanApplication.TransactionMode = Convert.ToString(dr["TransactionMode"]);

                if (dr["AppLevel"] != DBNull.Value)
                    objLoanApplication.ApprovalLevel = Convert.ToInt32(dr["AppLevel"]);
                objLoanApplication.Status = Convert.ToString(dr["Status"]);
                objLoanApplication.StatusCode = Convert.ToString(dr["StatusCode"]);
            }
            if (dr.NextResult())
            {
                objLoanApplication.lstApprovals = new List<MemberLoanApprovalDto>();
                while (dr.Read())
                {
                    objLoanApplication.lstApprovals.Add(new MemberLoanApprovalDto()
                    {
                        LoanSanctionAmount = Convert.ToInt32(dr["LoanSanctionAmount"]),
                        NoOfInstallments = Convert.ToByte(dr["NoOfInstallments"]),
                        ApprovalComments = Convert.ToString(dr["Comments"]),
                        Action = Convert.ToString(dr["ActionType"]),
                        ApprovalType = Convert.ToString(dr["ApproverType"]),
                        ActionDate = Convert.ToDateTime(dr["ActionDate"]),
                        ActionBy = Convert.ToString(dr["EmployeeName"])
                    });
                }
            }

            return objLoanApplication;
        }

        public MemberLoanClosure GetLoanClosureDemands(int loanMasterId, DateTime transacationDate)
        {
            var memberLoanClosure = new MemberLoanClosure();
            AdoHelper objAdo = new AdoHelper();

            SqlParameter[] parms = new SqlParameter[2];

            parms[0] = new SqlParameter("@LoanMasterID", loanMasterId);
            parms[0].SqlDbType = System.Data.SqlDbType.Int;

            parms[1] = new SqlParameter("@TransactionDate", transacationDate);
            parms[1].SqlDbType = System.Data.SqlDbType.Date;

            SqlDataReader dr = objAdo.ExecDataReaderProc("usp_LoanPreclosureDemand", parms);
            if (dr.Read())
            {
                memberLoanClosure.LoanMasterId = Convert.ToInt32(dr["LoanMasterID"]);
                memberLoanClosure.PrincipleDemand = Convert.ToDecimal(dr["PrincipleDemand"]);
                memberLoanClosure.InterestDemand = Convert.ToDecimal(dr["InterestDemand"]);
            }
            return memberLoanClosure;
         
        }

        public MemberLoanDisbursementDto GetMemberLoanDisbursementDetailsById(int loanMasterId, int userId)
        {
            var objLoanDisbursement = new MemberLoanDisbursementDto();
            AdoHelper objAdo = new AdoHelper();

            SqlParameter[] parms = new SqlParameter[2];

            parms[0] = new SqlParameter("@LoanMasterID", loanMasterId);
            parms[0].SqlDbType = System.Data.SqlDbType.Int;

            parms[1] = new SqlParameter("@UserId", userId);
            parms[1].SqlDbType = System.Data.SqlDbType.Int;

            SqlDataReader dr = objAdo.ExecDataReaderProc("uspMemberLoanDisbursementGetByLoanMasterID", parms);
            if (dr.Read())
            {
                objLoanDisbursement.LoanMasterId = Convert.ToInt32(dr["LoanMasterID"]);
                objLoanDisbursement.LoanCode = Convert.ToString(dr["LoanCode"]);

                objLoanDisbursement.MemberID = Convert.ToInt32(dr["MemberID"]);
                objLoanDisbursement.MemberName = Convert.ToString(dr["MemberName"]);
                objLoanDisbursement.LoanApplicationDate = Convert.ToDateTime(dr["LoanApplicationDate"]);
                objLoanDisbursement.LoanPurposeName = Convert.ToString(dr["Purpose"]);
                objLoanDisbursement.LoanAmountApplied = Convert.ToInt32(dr["LoanAmountApplied"]);
                objLoanDisbursement.NoofInstallmentsProposed = Convert.ToByte(dr["NoofInstallmentsProposed"]);
                objLoanDisbursement.FrequencyMode = Convert.ToString(dr["Mode"]);

                objLoanDisbursement.ProjectName = Convert.ToString(dr["ProjectName"]);
                objLoanDisbursement.InterestMasterID = Convert.ToInt32(dr["InterestMasterID"]);
                objLoanDisbursement.InterestRateID = Convert.ToInt32(dr["GroupInterstRateID"]);
                objLoanDisbursement.PrincipleAHName = Convert.ToString(dr["PrincipalAHName"]);
                objLoanDisbursement.InterestAHName = Convert.ToString(dr["InterestAHName"]);
                objLoanDisbursement.ROI = Convert.ToInt32(dr["ROI"]);
                objLoanDisbursement.ReferenceNumber = Convert.ToString(dr["LoanRefNumber"]);
                objLoanDisbursement.SourceOfFund = Convert.ToString(dr["SourceOfFund"]);
                objLoanDisbursement.TransactionMode = Convert.ToString(dr["TransactionMode"]);

                if (dr["AppLevel"] != DBNull.Value)
                    objLoanDisbursement.ApprovalLevel = Convert.ToInt32(dr["AppLevel"]);

                objLoanDisbursement.Status = Convert.ToString(dr["Status"]);
                objLoanDisbursement.StatusCode = Convert.ToString(dr["StatusCode"]);

                if (dr["MDSanctionAmount"] != DBNull.Value)
                    objLoanDisbursement.MDSanctionAmount = Convert.ToDecimal(dr["MDSanctionAmount"]);
                if (dr["MDInstallments"] != DBNull.Value)
                    objLoanDisbursement.MDInstallments = Convert.ToInt32(dr["MDInstallments"]);

                if (dr["DisbursedAmount"] != DBNull.Value)
                    objLoanDisbursement.DisbursedAmount = Convert.ToDecimal(dr["DisbursedAmount"]);
                if (dr["DisbursementDate"] != DBNull.Value)
                    objLoanDisbursement.DisbursementDate = Convert.ToDateTime(dr["DisbursementDate"]);

                if (dr["NoOfInstallments"] != DBNull.Value)
                    objLoanDisbursement.NoOfInstallments = Convert.ToInt32(dr["NoOfInstallments"]);

                objLoanDisbursement.ChequeNumber = Convert.ToString(dr["ChequeNumber"]);

                if (dr["ChequeDate"] != DBNull.Value)
                    objLoanDisbursement.ChequeDate = Convert.ToDateTime(dr["ChequeDate"]);

                if (dr["BankEntryId"] != DBNull.Value)
                    objLoanDisbursement.BankEntryId = Convert.ToInt32(dr["BankEntryId"]);

                if (dr["InstallmentStartFrom"] != DBNull.Value)
                    objLoanDisbursement.InstallmentStartFrom = Convert.ToDateTime(dr["InstallmentStartFrom"]);

                if (dr["LoanClosingDate"] != DBNull.Value)
                    objLoanDisbursement.FinalInstallmentDate = Convert.ToDateTime(dr["LoanClosingDate"]);

                if (dr["MonthlyPrincipalDemand"] != DBNull.Value)
                    objLoanDisbursement.MonthlyPrincipalDemand = Convert.ToInt32(dr["MonthlyPrincipalDemand"]);
                objLoanDisbursement.DisbursementComments = Convert.ToString(dr["DisbursementComments"]);

                if (objLoanDisbursement.TransactionMode.Length < 2 && objLoanDisbursement.TransactionMode == "B")
                {
                    objLoanDisbursement.TransactionMode = "BD";
                    if (objLoanDisbursement.ChequeDate != DateTime.MinValue)
                        objLoanDisbursement.TransactionMode = "BC";
                }

                if (dr["AccountMasterID"] != DBNull.Value)
                    objLoanDisbursement.AccountMasterId = Convert.ToInt32(dr["AccountMasterID"]);
                objLoanDisbursement.PaymentVoucherNumber = Convert.ToString(dr["VoucherNumber"]);
            }

            if (dr.NextResult())
            {
                objLoanDisbursement.lstApprovals = new List<MemberLoanApprovalDto>();
                while (dr.Read())
                {
                    objLoanDisbursement.lstApprovals.Add(new MemberLoanApprovalDto()
                    {
                        LoanSanctionAmount = Convert.ToInt32(dr["LoanSanctionAmount"]),
                        NoOfInstallments = Convert.ToByte(dr["NoOfInstallments"]),
                        ApprovalComments = Convert.ToString(dr["Comments"]),
                        Action = Convert.ToString(dr["ActionType"]),
                        ApprovalType = Convert.ToString(dr["ApproverType"]),
                        ActionDate = Convert.ToDateTime(dr["ActionDate"]),
                        ActionBy = Convert.ToString(dr["EmployeeName"])
                    });
                }
            }
            if (dr.NextResult())
            {
                objLoanDisbursement.Schedule = new List<ScheduleDTO>();
                ScheduleDTO objSchedule = null;
                while (dr.Read())
                {
                    objSchedule = new ScheduleDTO();

                    objSchedule.PERIOD = Convert.ToInt64(dr["PERIOD"]);
                    objSchedule.Opening_Balance = Convert.ToDecimal(dr["OPENING_BALANCE"]);
                    objSchedule.PAYDATE = Convert.ToDateTime(dr["PAYDATE"]);
                    objSchedule.PAYMENT = Convert.ToDecimal(dr["PAYMENT"]);
                    objSchedule.PRINCIPAL = Convert.ToDecimal(dr["PRINCIPAL"]);
                    objSchedule.DAYS = Convert.ToInt32(dr["DAYS"]);
                    objSchedule.INTEREST = Convert.ToDecimal(dr["INTEREST"]);
                    objSchedule.CURRENT_BALANCE = Convert.ToDecimal(dr["CURRENT_BALANCE"]);
                    objSchedule.INTERESTRate = Convert.ToInt32(dr["ROI"]);
                    objLoanDisbursement.Schedule.Add(objSchedule);
                }
            }
            return objLoanDisbursement;
        }

        public int ApproveMemberLoanApplication(int loanMasterId, Enums.ApprovalActions actionType, string approvalComments, int userId)
        {
            AdoHelper objAdo = new AdoHelper();
            SqlParameter[] parms = new SqlParameter[5];

            parms[0] = new SqlParameter("@LoanMasterID", loanMasterId);
            parms[0].SqlDbType = System.Data.SqlDbType.Int;

            parms[1] = new SqlParameter("@UserId", userId);
            parms[1].SqlDbType = System.Data.SqlDbType.Int;

            parms[2] = new SqlParameter("@ApprovalComments", approvalComments);
            parms[2].SqlDbType = System.Data.SqlDbType.VarChar;

            parms[3] = new SqlParameter("@ActionType", actionType.ToString());
            parms[3].SqlDbType = System.Data.SqlDbType.VarChar;

            parms[4] = new SqlParameter("@RetVal", 0);
            parms[4].SqlDbType = System.Data.SqlDbType.Int;
            parms[4].Direction = ParameterDirection.Output;

            objAdo.ExecNonQueryProc("uspApproveGroupLoan", parms);

            int retVal = Convert.ToInt32(parms[4].Value);

            return retVal;
        }

        public List<MemberLoanApplicationLookupDto> GetMemberLoanLookup(int GroupID, int userId)
        {
            List<MemberLoanApplicationLookupDto> lstLoans = new List<MemberLoanApplicationLookupDto>();
            AdoHelper objAdo = new AdoHelper();

            SqlParameter[] parms = new SqlParameter[2];

            parms[0] = new SqlParameter("@GroupId", GroupID);
            parms[0].SqlDbType = System.Data.SqlDbType.Int;

            parms[1] = new SqlParameter("@UserId", userId);
            parms[1].SqlDbType = System.Data.SqlDbType.Int;

            SqlDataReader dr = objAdo.ExecDataReaderProc("uspMemberLoanApplicationLookup", parms);
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
                if (dr["DisbursementDate"] != DBNull.Value)
                    objLoan.DisbursementDate = Convert.ToDateTime(dr["DisbursementDate"]);
                objLoan.Status = Convert.ToString(dr["Status"]);
                objLoan.StatusCode = Convert.ToString(dr["StatusCode"]);
                if (dr["AppLevel"] != DBNull.Value)
                    objLoan.ApprovalLevel = Convert.ToInt32(dr["AppLevel"]);
                objLoan.CanEdit = Convert.ToBoolean(dr["CanEdit"]);
                objLoan.CanView = Convert.ToBoolean(dr["CanView"]);
                objLoan.CanDelete = Convert.ToBoolean(dr["CanDelete"]);
                lstLoans.Add(objLoan);
            }
            return lstLoans;
        }

        public List<LoanAccountHeadDto> GetGroupLoanDepositAccountHeads(int GroupId, int groupInterestId, string type)
        {
            var lstLoanAh = new List<LoanAccountHeadDto>();
            var obj = new LoanAccountHeadDto();
            var objAdo = new AdoHelper();

            SqlParameter[] parms = new SqlParameter[3];

            parms[0] = new SqlParameter("@GroupId", GroupId);
            parms[1] = new SqlParameter("@Type", type);
            parms[2] = new SqlParameter("@GroupInterestID", groupInterestId);

            SqlDataReader dr = objAdo.ExecDataReaderProc("uspGetGroupLoanDepositAccountHeads", parms);
            while (dr.Read())
            {
                obj = new LoanAccountHeadDto();
                obj.InterestRateId = Convert.ToInt32(dr["GroupInterestRateID"]);
                obj.InterestMasterId = Convert.ToInt32(dr["GroupInterestID"]);
                obj.PrincipalAHId = Convert.ToInt32(dr["PrincipalAHID"]);
                obj.InterestAHId = Convert.ToInt32(dr["InterestAHID"]);
                obj.PrincipalAHName = Convert.ToString(dr["PrincipalAHName"]);
                obj.InterestAHName = Convert.ToString(dr["InterestAHName"]);
                obj.ROI = Convert.ToInt32(dr["ROI"]);

                lstLoanAh.Add(obj);
            }

            return lstLoanAh;
        }

        public ResultDto InsertUpdateMemberLoanApplication(MemberLoanApplicationDto obj)
        {
            ResultDto resultDto = new ResultDto();
            string obectName = "Loan Application";
            try
            {
                AdoHelper objAdo = new AdoHelper();
                SqlParameter[] parms = new SqlParameter[16];

                parms[0] = new SqlParameter("@LoanMasterId", obj.LoanMasterId);
                parms[0].SqlDbType = System.Data.SqlDbType.Int;
                parms[0].Direction = ParameterDirection.InputOutput;

                parms[1] = new SqlParameter("@LoanCode", SqlDbType.VarChar, 50);
                parms[1].Value = obj.LoanCode;
                parms[1].Direction = ParameterDirection.Output;

                parms[2] = new SqlParameter("@MemberID", obj.MemberID);
                parms[2].SqlDbType = System.Data.SqlDbType.Int;

                parms[3] = new SqlParameter("@GroupID", obj.GroupID);
                parms[3].SqlDbType = System.Data.SqlDbType.Int;

                parms[4] = new SqlParameter("@LoanApplicationDate", obj.LoanApplicationDate);
                parms[4].SqlDbType = System.Data.SqlDbType.Date;

                parms[5] = new SqlParameter("@LoanPurpose", obj.LoanPurpose);
                parms[5].SqlDbType = System.Data.SqlDbType.Int;

                parms[6] = new SqlParameter("@ProjectId", obj.ProjectID);
                parms[6].SqlDbType = System.Data.SqlDbType.Int;

                parms[7] = new SqlParameter("@LoanAmountApplied", obj.LoanAmountApplied);
                parms[7].SqlDbType = System.Data.SqlDbType.Money;

                parms[8] = new SqlParameter("@NoofInstallmentsProposed", obj.NoofInstallmentsProposed);
                parms[8].SqlDbType = System.Data.SqlDbType.TinyInt;

                parms[9] = new SqlParameter("@FrequencyMode", obj.Mode);
                parms[9].SqlDbType = System.Data.SqlDbType.Int;

                parms[10] = new SqlParameter("@UserId", obj.UserID);
                parms[10].SqlDbType = System.Data.SqlDbType.Int;

                parms[11] = new SqlParameter("@GroupInterstRateID", obj.InterestRateID);
                parms[11].SqlDbType = System.Data.SqlDbType.Int;

                parms[12] = new SqlParameter("@RefNumber", obj.ReferenceNumber);
                parms[12].SqlDbType = System.Data.SqlDbType.VarChar;

                parms[13] = new SqlParameter("@SourceOfFund", obj.SourceOfFund);
                parms[13].SqlDbType = System.Data.SqlDbType.VarChar;

                parms[14] = new SqlParameter("@TransactionMode", obj.TransactionMode);
                parms[14].SqlDbType = System.Data.SqlDbType.VarChar;

                parms[15] = new SqlParameter("@Status", SqlDbType.VarChar, 50);
                parms[15].Value = obj.Status;
                parms[15].Direction = ParameterDirection.Output;

                objAdo.ExecNonQueryProc("uspMemberLoanInsertUpdate", parms);

                resultDto.ObjectId = Convert.ToInt32(parms[0].Value);
                resultDto.ObjectCode = string.IsNullOrEmpty((string)parms[1].Value) ? obj.LoanCode : (string)parms[1].Value;
                obj.Status = (string)parms[15].Value;

                if (resultDto.ObjectId > 0)
                    resultDto.Message = string.Format("{0} details saved successfully with code : {1}", obectName, resultDto.ObjectCode);
                else if (resultDto.ObjectId == -1)
                    resultDto.Message = string.Format("Error occured while generating {0} code", obectName);
                else
                    resultDto.Message = string.Format("Error occured while saving {0} details", obectName);
            }
            catch (Exception)
            {
                resultDto.Message = string.Format("Service layer error occured while saving the {0} details", obectName);
                resultDto.ObjectId = -98;
            }
            return resultDto;
        }

        public int SubmitLoanApplicationApproval(MemberLoanApprovalDto objLoanApproval, int userId, int loanMasterId, bool isSave)
        {
            AdoHelper objAdo = new AdoHelper();
            SqlParameter[] parms = new SqlParameter[10];

            parms[0] = new SqlParameter("@LoanMasterId", loanMasterId);
            parms[0].SqlDbType = SqlDbType.Int;

            parms[1] = new SqlParameter("@UserId", userId);
            parms[1].SqlDbType = System.Data.SqlDbType.Int;

            parms[2] = new SqlParameter("@LoanSactionAmount", objLoanApproval.LoanSanctionAmount);
            parms[2].SqlDbType = System.Data.SqlDbType.Int;

            parms[3] = new SqlParameter("@NoOfInstallments", objLoanApproval.NoOfInstallments);
            parms[3].SqlDbType = System.Data.SqlDbType.Int;

            parms[4] = new SqlParameter("@Comments", objLoanApproval.ApprovalComments);
            parms[4].SqlDbType = System.Data.SqlDbType.VarChar;

            parms[5] = new SqlParameter("@Action", objLoanApproval.Action);
            parms[5].SqlDbType = System.Data.SqlDbType.VarChar;

            parms[6] = new SqlParameter("@ApproverType", objLoanApproval.ApprovalType);
            parms[6].SqlDbType = System.Data.SqlDbType.VarChar;

            parms[7] = new SqlParameter("@RetVal", SqlDbType.Int, 4);
            parms[7].Direction = ParameterDirection.Output;

            parms[8] = new SqlParameter("@isSave", isSave);
            parms[8].SqlDbType = System.Data.SqlDbType.Bit;

            parms[9] = new SqlParameter("@ActionDate", objLoanApproval.ActionDate);
            parms[9].SqlDbType = System.Data.SqlDbType.DateTime;

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

        public ResultDto SaveMemberLoanDisbursementDetails(MemberLoanDisbursementDto objMemberLoanDisbursement, int userId)
        {
            ResultDto objresult = new ResultDto();
            try
            {
                int parmsCount = objMemberLoanDisbursement.TransactionMode == "C" ? 12 : (objMemberLoanDisbursement.TransactionMode == "BC" ? 15 : 13);

                AdoHelper objAdo = new AdoHelper();
                SqlParameter[] parms = new SqlParameter[parmsCount];

                parms[0] = new SqlParameter("@LoanMasterId", objMemberLoanDisbursement.LoanMasterId);
                parms[0].SqlDbType = System.Data.SqlDbType.Int;

                parms[1] = new SqlParameter("@DisbursementComments", objMemberLoanDisbursement.DisbursementComments);
                parms[1].SqlDbType = System.Data.SqlDbType.VarChar;

                parms[2] = new SqlParameter("@DisbursedAmount", objMemberLoanDisbursement.DisbursedAmount);
                parms[2].SqlDbType = System.Data.SqlDbType.Decimal;

                parms[3] = new SqlParameter("@TransactionMode", objMemberLoanDisbursement.TransactionMode);
                parms[3].SqlDbType = System.Data.SqlDbType.VarChar;

                parms[4] = new SqlParameter("@DisbursedDate", objMemberLoanDisbursement.DisbursementDate);
                parms[4].SqlDbType = System.Data.SqlDbType.DateTime;

                parms[5] = new SqlParameter("@NoofInstallments", objMemberLoanDisbursement.NoOfInstallments);
                parms[5].SqlDbType = System.Data.SqlDbType.TinyInt;

                parms[6] = new SqlParameter("@InstallmentStartDate", objMemberLoanDisbursement.InstallmentStartFrom);
                parms[6].SqlDbType = System.Data.SqlDbType.DateTime;

                parms[7] = new SqlParameter("@InstallmentEndDate", objMemberLoanDisbursement.FinalInstallmentDate);
                parms[7].SqlDbType = System.Data.SqlDbType.DateTime;

                parms[8] = new SqlParameter("@MonthlyPrincipalDemand", objMemberLoanDisbursement.MonthlyPrincipalDemand);
                parms[8].SqlDbType = System.Data.SqlDbType.Int;

                parms[9] = new SqlParameter("@RefNo", objMemberLoanDisbursement.ReferenceNumber);
                parms[9].SqlDbType = System.Data.SqlDbType.VarChar;

                parms[10] = new SqlParameter("@UserId", userId);
                parms[10].SqlDbType = System.Data.SqlDbType.Int;

                parms[11] = new SqlParameter("@SourceOfFund", objMemberLoanDisbursement.SourceOfFund);
                parms[11].SqlDbType = System.Data.SqlDbType.VarChar;

                if (objMemberLoanDisbursement.TransactionMode != "C")
                {
                    parms[12] = new SqlParameter("@BankEntryId", objMemberLoanDisbursement.BankEntryId);
                    parms[12].SqlDbType = System.Data.SqlDbType.Int;

                    if (objMemberLoanDisbursement.TransactionMode == "BC")
                    {
                        parms[13] = new SqlParameter("@ChequeNumber", objMemberLoanDisbursement.ChequeNumber);
                        parms[13].SqlDbType = System.Data.SqlDbType.VarChar;

                        parms[14] = new SqlParameter("@ChequeDate", objMemberLoanDisbursement.ChequeDate);
                        parms[14].SqlDbType = System.Data.SqlDbType.DateTime;
                    }
                }

                int updated = objAdo.ExecNonQueryProc("uspUpdateDisbursementDetails", parms);

                if (updated > 0)
                {

                    objresult.Message = "Updated SuccessFully";
                    objresult.ObjectId = objMemberLoanDisbursement.LoanMasterId;
                    objresult.ObjectCode = "TRUE";
                }
                else
                {
                    objresult.Message = "Failed To UpDate";
                    objresult.ObjectId = objMemberLoanDisbursement.LoanMasterId;
                    objresult.ObjectCode = "FALSE";
                }
            }
            catch (Exception ex)
            {
                objresult.ObjectId = -99;
                objresult.Message = "Error Occured at Data Layer While Saving Disbursement Details";
            }
            return objresult;
        }

        public ResultDto ConfirmAndDisburseMemberLoan(int loanMasterId, int groupId, int userId)
        {
            ResultDto objresult = new ResultDto();
            try
            {
                AdoHelper objAdo = new AdoHelper();
                SqlParameter[] parms = new SqlParameter[3];

                parms[0] = new SqlParameter("@LoanMasterId", loanMasterId);
                parms[0].SqlDbType = System.Data.SqlDbType.Int;
                parms[0].Direction = ParameterDirection.InputOutput;

                parms[1] = new SqlParameter("@GroupId", groupId);
                parms[1].SqlDbType = System.Data.SqlDbType.Int;

                parms[2] = new SqlParameter("@UserId", userId);
                parms[2].SqlDbType = System.Data.SqlDbType.Int;

                int updated = objAdo.ExecNonQueryProc("uspMemberLoanConfirm", parms);

                updated = Convert.ToInt32(parms[0].Value);

                if (updated > 0)
                {

                    objresult.Message = "Loan Activated SuccessFully";
                    objresult.ObjectId = loanMasterId;
                    objresult.ObjectCode = "TRUE";
                }
                else if (updated == -97)
                {
                    objresult.Message = "Insufficient Funds to Disburse the Loan.";
                    objresult.ObjectId = updated;
                    objresult.ObjectCode = "FALSE";
                }
                else if (updated == -98)
                {
                    objresult.Message = "Cannot provide new loan Due to pending loans";
                    objresult.ObjectId = updated;
                    objresult.ObjectCode = "FALSE";
                }
                else if (updated == -98)
                {
                    objresult.Message = "Failed While Activating the Loan";
                    objresult.ObjectId = updated;
                    objresult.ObjectCode = "FALSE";
                }
            }
            catch (Exception ex)
            {
                objresult.ObjectId = -99;
                objresult.Message = "Error Occured at Data Layer While Activating the Loan";
            }
            return objresult;
        }
    }
}