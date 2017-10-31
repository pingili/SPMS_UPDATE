using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntities;
using System.Data;
using System.Data.SqlClient;
using BlackBeltCoder;

namespace DataLogic
{

    public class LoanDisbursementDataAccess
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

        public InterestMasterDto GetIntrestRateByIntrestId(int Id)
        {
            InterestMasterDto objIntrestMasterDto = null;
            AdoHelper obj = new AdoHelper();
            SqlParameter[] parms = new SqlParameter[1];

            parms[0] = new SqlParameter("@InterestID", Id);
            parms[0].SqlDbType = System.Data.SqlDbType.Int;

            SqlDataReader Dr = obj.ExecDataReaderProc("uspInterestByID", parms);
            if (Dr.Read())
            {
                objIntrestMasterDto = new InterestMasterDto();
                if (!string.IsNullOrEmpty(Convert.ToString(Dr["PenalAHID"])))
                    objIntrestMasterDto.PenalAHID = Convert.ToInt32(Dr["PenalAHID"]);
                objIntrestMasterDto.InterestAHID = Convert.ToInt32(Dr["InterestAHID"]);
                objIntrestMasterDto.InterestCode = Convert.ToString(Dr["InterestCode"]);
                objIntrestMasterDto.InterestName = Convert.ToString(Dr["InterestName"]);
                objIntrestMasterDto.InterestID = Convert.ToInt32(Dr["InterestID"]);
                objIntrestMasterDto.PrincipalAHID = Convert.ToInt32(Dr["PrincipalAHID"]);
                objIntrestMasterDto.AHName = Convert.ToString(Dr["AHName"]);
                objIntrestMasterDto.InterestAHName = Convert.ToString(Dr["INTERESTAHNAME"]);
                objIntrestMasterDto.CaluculationMethod = Convert.ToInt32(Dr["CaluculationMethod"]);
                objIntrestMasterDto.Base = Convert.ToInt32(Dr["Base"]);
            }
            List<InterestRatesDto> lstrates = new List<InterestRatesDto>();
            InterestRatesDto rateDto = null;
            if (Dr.NextResult())
            {
                while (Dr.Read())
                {
                    rateDto = new InterestRatesDto();
                    rateDto.IntrestRateID = Convert.ToInt32(Dr["IntrestRateID"]);
                    rateDto.ROI = Convert.ToInt32(Dr["ROI"]);
                    rateDto.PenalROI = Convert.ToInt32(Dr["PenalROI"]);
                    rateDto.FromDate = Convert.ToDateTime(Dr["FromDate"]);
                    if (Dr["ToDate"] != DBNull.Value)
                        rateDto.ToDate = Convert.ToDateTime(Dr["ToDate"]);
                    lstrates.Add(rateDto);
                }
                objIntrestMasterDto.InterestRates = lstrates;
            }
            return objIntrestMasterDto;
        }

        public List<ScheduleDTO> GetSchedulesForDisbursement(int LoanMasterId, decimal LoanAmount, decimal interestrate, int loanperiod, DateTime StartPaymentDate, string LastPaidDateOrDisbDate = null)
        {
            List<ScheduleDTO> lstSchedules = new List<ScheduleDTO>();
            ScheduleDTO objSchedule = null;
            AdoHelper obj = new AdoHelper();
            SqlParameter[] parms = new SqlParameter[6];

            parms[0] = new SqlParameter("@LoanAmount", LoanAmount);
            parms[0].SqlDbType = System.Data.SqlDbType.Decimal;

            parms[1] = new SqlParameter("@InterestRate", interestrate);
            parms[1].SqlDbType = System.Data.SqlDbType.Decimal;

            parms[2] = new SqlParameter("@LoanPeriod", loanperiod);
            parms[2].SqlDbType = System.Data.SqlDbType.Int;

            parms[3] = new SqlParameter("@StartPaymentDate", StartPaymentDate);
            parms[3].SqlDbType = System.Data.SqlDbType.DateTime;

            parms[4] = new SqlParameter("@CalculationStartDate", LastPaidDateOrDisbDate);
            parms[4].SqlDbType = System.Data.SqlDbType.DateTime;

            parms[5] = new SqlParameter("@LoanMasterId", LoanMasterId);
            parms[5].SqlDbType = System.Data.SqlDbType.Int;



            SqlDataReader Dr = obj.ExecDataReaderProc("uspGetEmiNew", parms);
            while (Dr.Read())
            {
                objSchedule = new ScheduleDTO();

                objSchedule.PERIOD = Convert.ToInt64(Dr["PERIOD"]);
                objSchedule.Opening_Balance = Convert.ToDecimal(Dr["OPENING_BALANCE"]);
                objSchedule.PAYDATE = Convert.ToDateTime(Dr["PAYDATE"]);
                if (Dr["PAYMENT"] != DBNull.Value)
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

        public List<ScheduleDTO> GetSchedulesOB(int LoanMasterID, decimal LoanAmount, decimal interestrate, int loanperiod, DateTime disbursementDate, DateTime firstInstallmentDate)
        {
            List<ScheduleDTO> lstSchedules = new List<ScheduleDTO>();
            ScheduleDTO objSchedule = null;
            AdoHelper obj = new AdoHelper();
            SqlParameter[] parms = new SqlParameter[6];
            parms[0] = new SqlParameter("@LoanMasterID", LoanMasterID);//
            parms[0].SqlDbType = System.Data.SqlDbType.Int;

            parms[1] = new SqlParameter("@LoanAmount", LoanAmount);
            parms[1].SqlDbType = System.Data.SqlDbType.Decimal;

            parms[2] = new SqlParameter("@InterestRate", interestrate);
            parms[2].SqlDbType = System.Data.SqlDbType.Decimal;

            parms[3] = new SqlParameter("@LoanPeriod", loanperiod);
            parms[3].SqlDbType = System.Data.SqlDbType.Int;

            parms[4] = new SqlParameter("@StartPaymentDate", firstInstallmentDate);
            parms[4].SqlDbType = System.Data.SqlDbType.DateTime;

            parms[5] = new SqlParameter("@CalculationStartDate", disbursementDate);
            parms[5].SqlDbType = System.Data.SqlDbType.DateTime;

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

        public ResultDto InsertDisbursemnet(GroupLoanDisbursementDto obj, int CurrentUserId)
        {
            ResultDto objresult = new ResultDto();
            try
            {
                AdoHelper objAdo = new AdoHelper();
                SqlParameter[] parms = new SqlParameter[23];

                parms[0] = new SqlParameter("@InterstRateId", obj.InterestRateID);
                parms[0].SqlDbType = System.Data.SqlDbType.Int;

                parms[1] = new SqlParameter("@NoofInstallments", obj.NoOfInstallments);
                parms[1].SqlDbType = System.Data.SqlDbType.TinyInt;

                parms[2] = new SqlParameter("@DisbursedAmount", obj.DisbursedAmount);
                parms[2].SqlDbType = System.Data.SqlDbType.Decimal;

                parms[3] = new SqlParameter("@DisbursedDate", obj.DisbursementDate);
                parms[3].SqlDbType = System.Data.SqlDbType.DateTime;

                parms[4] = new SqlParameter("@DueDays", obj.MeetingDay);
                parms[4].SqlDbType = System.Data.SqlDbType.Int;

                parms[5] = new SqlParameter("@InstallmentStartDate", obj.InstallmentStartFrom);
                parms[5].SqlDbType = System.Data.SqlDbType.DateTime;

                parms[6] = new SqlParameter("@InstallmentEndDate", obj.LoanClosingDate);
                parms[6].SqlDbType = System.Data.SqlDbType.DateTime;

                parms[7] = new SqlParameter("@MonthlyPrincipalDemand", obj.MonthlyPrincipalDemand);
                parms[7].SqlDbType = System.Data.SqlDbType.Decimal;

                parms[8] = new SqlParameter("@ProjectId", obj.ProjectID);
                parms[8].SqlDbType = System.Data.SqlDbType.Int;

                parms[9] = new SqlParameter("@RefNo", obj.LoanRefNumber);
                parms[9].SqlDbType = System.Data.SqlDbType.VarChar;

                parms[10] = new SqlParameter("@LoanMasterId", obj.LoanMasterId);
                parms[10].SqlDbType = System.Data.SqlDbType.Int;

                parms[11] = new SqlParameter("@GroupId", obj.GroupID);
                parms[11].SqlDbType = System.Data.SqlDbType.Int;

                parms[12] = new SqlParameter("@PrincipalAHId", obj.PrincipleAHId);
                parms[12].SqlDbType = System.Data.SqlDbType.Int;

                parms[13] = new SqlParameter("@UserId", CurrentUserId);
                parms[13].SqlDbType = System.Data.SqlDbType.Int;

                parms[14] = new SqlParameter("@IsFederation", 1);
                parms[14].SqlDbType = System.Data.SqlDbType.Bit;

                //parms[15] = new SqlParameter("@FundSourceId", obj.FundSourceID);
                //parms[15].SqlDbType = System.Data.SqlDbType.Int;

                parms[15] = new SqlParameter("@SLAccountName", obj.SLAccountName);
                parms[15].SqlDbType = System.Data.SqlDbType.VarChar;

                parms[16] = new SqlParameter("@OutStandingAmount", obj.OutStandingAmount);
                parms[16].SqlDbType = System.Data.SqlDbType.Decimal;

                parms[17] = new SqlParameter("@InterestMasterID", obj.InterestMasterID);
                parms[17].SqlDbType = System.Data.SqlDbType.Int;

                parms[18] = new SqlParameter("@BankID", obj.BankEntryId);
                parms[18].SqlDbType = System.Data.SqlDbType.Int;

                parms[19] = new SqlParameter("@ChequeNumber", obj.ChequeNumber);
                parms[19].SqlDbType = System.Data.SqlDbType.VarChar;

                if (obj.ChequeDate.Ticks > 0)
                    parms[20] = new SqlParameter("@ChequeDate", obj.ChequeDate);
                else
                    parms[20] = new SqlParameter("@ChequeDate", null);
                parms[20].SqlDbType = System.Data.SqlDbType.DateTime;

                parms[21] = new SqlParameter("@TransactionMode", obj.TransactionMode);
                parms[21].SqlDbType = System.Data.SqlDbType.VarChar;

                parms[22] = new SqlParameter("@GroupBankEntryId", obj.GroupBankEntryId);
                parms[22].SqlDbType = System.Data.SqlDbType.Int;

                int updated = objAdo.ExecNonQueryProc("uspLoandDisbursementInsertDetails", parms);

                if (updated > 0)
                {

                    objresult.Message = "Updated SuccessFully";
                    objresult.ObjectId = obj.LoanMasterId;
                    objresult.ObjectCode = "TRUE";
                }
                else
                {
                    objresult.Message = "Failed To UpDate";
                    objresult.ObjectId = obj.LoanMasterId;
                    objresult.ObjectCode = "FALSE";
                }
                return objresult;
            }
            catch (Exception ex) { throw ex; }

        }
        public ResultDto ConfirmAndDisburseGroupLoan(int loanMasterId, int userId)
        {
            ResultDto objresult = new ResultDto();
            try
            {
                AdoHelper objAdo = new AdoHelper();
                SqlParameter[] parms = new SqlParameter[2];

                parms[0] = new SqlParameter("@LoanMasterId", loanMasterId);
                parms[0].SqlDbType = System.Data.SqlDbType.Int;
                parms[0].Direction = ParameterDirection.InputOutput;

                parms[1] = new SqlParameter("@UserId", userId);
                parms[1].SqlDbType = System.Data.SqlDbType.Int;

                int updated = objAdo.ExecNonQueryProc("uspGroupLoanConfirm", parms);

                updated = Convert.ToInt32(parms[0].Value);

                if (updated > 0)
                {

                    objresult.Message = "Loan Activated SuccessFully";
                    objresult.ObjectId = loanMasterId;
                    objresult.ObjectCode = "TRUE";
                }
                else
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
                        if (Convert.ToByte(Dr["NoOfInstallments"]) != 0)
                            groupLoanDisbursementDto.NoOfInstallments = Convert.ToByte(Dr["NoOfInstallments"]);
                        else
                            groupLoanDisbursementDto.NoOfInstallments = Convert.ToByte(Dr["NoofInstallmentsProposed"]);
                        groupLoanDisbursementDto.MonthlyPrincipalDemand = Convert.ToDecimal(Dr["MonthlyPrincipalDemand"]);
                        groupLoanDisbursementDto.OutStandingAmount = Convert.ToDecimal(Dr["OutStandingAmount"]);
                        if (!String.IsNullOrEmpty(Dr["PrincipalAHID"].ToString()))
                            groupLoanDisbursementDto.PrincipleAHId = Convert.ToInt32(Dr["PrincipalAHID"]);
                        if (!String.IsNullOrEmpty(Dr["InterestAHID"].ToString()))
                            groupLoanDisbursementDto.InterestAHID = Convert.ToInt32(Dr["InterestAHID"]);
                        // groupLoanDisbursementDto.SLAccountNumber = Convert.ToInt32(Dr["SLAccountNumber"]);
                        groupLoanDisbursementDto.SLAccountName = Convert.ToString(Dr["SLAccountName"]);
                        groupLoanDisbursementDto.InterestRateID = Convert.ToInt32(Dr["InterestRateID"] == DBNull.Value ? 0 : Convert.ToInt32(Dr["InterestRateID"]));
                        groupLoanDisbursementDto.DisbursementDate = Convert.ToDateTime(Dr["DisbursementDate"]);
                        groupLoanDisbursementDto.InstallmentStartFrom = Convert.ToDateTime(Dr["InstallmentStartFrom"]);
                        groupLoanDisbursementDto.LoanClosingDate = Dr["LoanClosingDate"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(Dr["LoanClosingDate"]);
                        groupLoanDisbursementDto.LoanRefNumber = Convert.ToString(Dr["LoanRefNumber"]);
                        groupLoanDisbursementDto.ROI = DBNull.Value == Dr["ROI"] ? 0 : Convert.ToDecimal(Dr["ROI"]);
                        //  groupLoanDisbursementDto.InterestMasterID = Convert.ToInt32(Dr["InterestMasterID"]);
                        if (DBNull.Value != Dr["BankEntryId"])
                            groupLoanDisbursementDto.BankEntryId = Convert.ToInt32(Dr["BankEntryId"]);

                        groupLoanDisbursementDto.ChequeNumber = Convert.ToString(Dr["ChequeNumber"]);
                        if (DBNull.Value != Dr["ChequeDate"])
                            groupLoanDisbursementDto.chequedate = Convert.ToDateTime(Dr["ChequeDate"]);

                        groupLoanDisbursementDto.TransactionMode = (groupLoanDisbursementDto.BankEntryId == 0 ? "C" : groupLoanDisbursementDto.chequedate != DateTime.MinValue ? "BC" : "BD");
                        groupLoanDisbursementDto.InterestMasterID = Convert.ToInt32(Dr["MasterInterestId"]);

                    }
                    groupLoanDisbursementDto.MeetingDay = Convert.ToInt32(Dr["MeetingDay"]);
                    groupLoanDisbursementDto.NoofInstallmentsProposed = Convert.ToByte(Dr["NoofInstallmentsProposed"]);
                    groupLoanDisbursementDto.LoanAmountApplied = Convert.ToDecimal(Dr["LoanAmountApplied"]);
                    groupLoanDisbursementDto.LoanMasterId = Convert.ToInt32(Dr["LoanMasterId"]);
                    groupLoanDisbursementDto.GroupID = DBNull.Value == Dr["GroupID"] ? 0 : Convert.ToInt32(Dr["GroupID"]);
                    groupLoanDisbursementDto.LoanCode = Convert.ToString(Dr["LoanCode"]);
                    if (Convert.ToString(Dr["LoanType"]) == "G")
                    {
                        groupLoanDisbursementDto.Groupcode = Convert.ToString(Dr["GroupCode"]);
                        groupLoanDisbursementDto.GroupName = Convert.ToString(Dr["GroupName"]);
                    }
                    groupLoanDisbursementDto.LoanApplicationDate = Convert.ToDateTime(Dr["LoanApplicationDate"]);
                    groupLoanDisbursementDto.LoanPurpose = Convert.ToString(Dr["PurposeName"]);
                    groupLoanDisbursementDto.Mode = Convert.ToString(Dr["ModeName"]);
                    //groupLoanDisbursementDto.FundSourceID = Dr["FundSourceID"] == DBNull.Value ? 0 : Convert.ToInt32(Dr["FundSourceID"]);
                    groupLoanDisbursementDto.ProjectName = Convert.ToString(Dr["ProjectName"]);
                    groupLoanDisbursementDto.StatusCode = Convert.ToString(Dr["StatusCode"]);
                    groupLoanDisbursementDto.Status = Convert.ToString(Dr["Status"]);
                    if (Dr["GroupBankEntryId"] != DBNull.Value)
                        groupLoanDisbursementDto.GroupBankEntryId = Convert.ToInt32(Dr["GroupBankEntryId"]);
                    if (Dr["ProjectID"] != DBNull.Value)
                        groupLoanDisbursementDto.ProjectID = Convert.ToInt32(Dr["ProjectID"]);

                    if (Dr["GRAccountMasterId"] != DBNull.Value)
                    {
                        groupLoanDisbursementDto.GRAccountMasterId = Convert.ToInt32(Dr["GRAccountMasterId"]);
                        groupLoanDisbursementDto.GRVoucherNumber = Convert.ToString(Dr["GRVoucherNumber"]);
                    }
                    if (Dr["FPAccountMasterId"] != DBNull.Value)
                    {
                        groupLoanDisbursementDto.FPAccountMasterId = Convert.ToInt32(Dr["FPAccountMasterId"]);
                        groupLoanDisbursementDto.FPVoucherNumber = Convert.ToString(Dr["FPVoucherNumber"]);
                    }
                }
                if (Dr.NextResult())
                {
                    groupLoanDisbursementDto.Schedule = new List<ScheduleDTO>();
                    ScheduleDTO objSchedule = null;
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
                        objSchedule.INTERESTRate = Convert.ToInt32(Dr["ROI"]);
                        groupLoanDisbursementDto.Schedule.Add(objSchedule);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return groupLoanDisbursementDto;
        }
        public ResultDto CreateSchedulesForDisbursement(int LoanMasterId, decimal LoanAmount, decimal interestrate, int loanperiod, DateTime StartPaymentDate, int CurrentUserID, int PROI, string LastPaidDateOrDisbDate = null)
        {

            ResultDto objResult = new ResultDto();
            AdoHelper obj = new AdoHelper();
            SqlParameter[] parms = new SqlParameter[8];

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

            parms[7] = new SqlParameter("@CalculationStartDate", LastPaidDateOrDisbDate);
            parms[7].SqlDbType = System.Data.SqlDbType.VarChar;

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
        public ResultDto CreateSchedulesForOB(int LoanMasterId, decimal LoanAmount, decimal interestrate, int loanperiod, DateTime StartPaymentDate, int CurrentUserID, int PROI, string LastPaidDateOrDisbDate = null)
        {

            ResultDto objResult = new ResultDto();
            AdoHelper obj = new AdoHelper();
            SqlParameter[] parms = new SqlParameter[8];
            try
            {
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

                parms[7] = new SqlParameter("@CalculationStartDate", LastPaidDateOrDisbDate);
                parms[7].SqlDbType = System.Data.SqlDbType.VarChar;

                int i = obj.ExecNonQueryProc("uspCreateScheduleOB", parms);

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
            }
            catch (Exception ex)
            {

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
            SqlParameter[] parms = new SqlParameter[13];

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
            parms[4].Size = 256;
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

            parms[12] = new SqlParameter("@CreatedOn", DateTime.Now);
            parms[12].SqlDbType = System.Data.SqlDbType.DateTime;
            parms[12].Direction = ParameterDirection.Output;




            int count = obj.ExecNonQueryProc("uspdisbursementgenearatevoucher", parms);
            if (count > 0)
            {
                resultDto.ObjectCode = (string)parms[4].Value;
                resultDto.Type = TransactionType;
                resultDto.CreatedOn = Convert.ToString(DateTime.Now);
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


        public ResultDto ValidateSchedule(int LoanMasterID)
        {

            ResultDto objResult = new ResultDto();
            AdoHelper obj = new AdoHelper();
            SqlParameter[] parms = new SqlParameter[1];

            parms[0] = new SqlParameter("@LoanMasterID", LoanMasterID);
            parms[0].SqlDbType = System.Data.SqlDbType.Int;

            SqlDataReader Dr = obj.ExecDataReaderProc("uspValidateSchedule", parms);
            if (Dr.Read())
            {
                objResult.Message = Convert.ToString(Dr["ValidateMessage"]);
                objResult.Result = Convert.ToBoolean(Dr["IsValid"]);
            }
            return objResult;

        }

        public DisbursementVoucherDto GetDisbursementVoucher(int LoanMasterID)
        {
            DisbursementVoucherDto objDisbursementVoucherDto = new DisbursementVoucherDto();
            AdoHelper obj = new AdoHelper();
            SqlParameter[] parms = new SqlParameter[1];
            parms[0] = new SqlParameter("@LoanMasterID", LoanMasterID);
            parms[0].SqlDbType = System.Data.SqlDbType.Int;

            SqlDataReader Dr = obj.ExecDataReaderProc("uspGetDisbursementVoucher", parms);
            if (Dr.Read())
            {
                objDisbursementVoucherDto.Amount = Convert.ToDecimal(Dr["Amount"]);
                objDisbursementVoucherDto.VoucherNumber = Convert.ToString(Dr["VoucherNumber"]);
                objDisbursementVoucherDto.TransactionMode = Convert.ToString(Dr["TransactionMode"]);
                objDisbursementVoucherDto.TransactionDate = Convert.ToDateTime(Dr["TransactionDate"]);
                if (objDisbursementVoucherDto.TransactionMode == "B")
                {
                    objDisbursementVoucherDto.ChequeDate = Convert.ToDateTime(Dr["ChequeDate"]);
                    objDisbursementVoucherDto.ChequeNumber = Convert.ToString(Dr["ChequeNumber"]);
                    objDisbursementVoucherDto.BankID = Convert.ToInt32(Dr["AHID"]);
                    objDisbursementVoucherDto.BankName = Convert.ToString(Dr["BankName"]);
                }
            }
            return objDisbursementVoucherDto;

        }



        public GroupLoanApplicationDto GetGroupApplicationDetailsByID(int loanMasterId)
        {
            GroupLoanApplicationDto objGroupLoanApp = new GroupLoanApplicationDto();

            AdoHelper obj = new AdoHelper();
            SqlParameter[] parms = new SqlParameter[1];
            parms[0] = new SqlParameter("@LoanMasterID", loanMasterId);
            parms[0].SqlDbType = System.Data.SqlDbType.Int;

            SqlDataReader Dr = obj.ExecDataReaderProc("uspGroupLoanApplicationGetByLoanMasterID", parms);
            if (Dr.Read())
            {
                objGroupLoanApp.LoanMasterId = Convert.ToInt32(Dr["LoanMasterID"]);
                objGroupLoanApp.LoanCode = Convert.ToString(Dr["LoanCode"]);
                objGroupLoanApp.LoanType = Convert.ToString(Dr["LoanType"]);
                objGroupLoanApp.GroupID = Convert.ToInt32(Dr["GroupID"]);
                objGroupLoanApp.LoanApplicationDate = Convert.ToDateTime(Dr["LoanApplicationDate"]);
                objGroupLoanApp.LoanPurpose = Convert.ToInt32(Dr["LoanPurpose"]);
                objGroupLoanApp.FundSourse = Convert.ToInt32(Dr["FundSourceID"]);
                objGroupLoanApp.LoanAmountApplied = Convert.ToDecimal(Dr["LoanAmountApplied"]);
                objGroupLoanApp.NoofInstallmentsProposed = Convert.ToByte(Dr["NoofInstallmentsProposed"]);
                objGroupLoanApp.Mode = Convert.ToInt32(Dr["Mode"]);
                objGroupLoanApp.ProjectID = Convert.ToInt32(Dr["ProjectID"]);
                objGroupLoanApp.StatusCode = Convert.ToString(Dr["StatusCode"]);
                objGroupLoanApp.Status = Convert.ToString(Dr["Status"]);
            }
            return objGroupLoanApp;
        }
    }
}
