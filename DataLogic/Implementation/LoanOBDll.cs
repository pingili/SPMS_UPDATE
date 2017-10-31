using BlackBeltCoder;
using BusinessEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace DataLogic
{
    public class LoanOBDll
    {
        public List<LoanOBLookup> LoanOBLookUp(char loanType)
        {
            List<LoanOBLookup> lstloanOBLookup = new List<LoanOBLookup>();
            try
            {
                AdoHelper obj = new AdoHelper();

                SqlParameter[] parms = new SqlParameter[1];
                parms[0] = new SqlParameter("@LoanType", loanType);
                parms[0].SqlDbType = SqlDbType.Char;


                SqlDataReader sdr = obj.ExecDataReaderProc("uspLoanMasterGetOldLoansLookUp", parms);
                while (sdr.Read())
                {
                    LoanOBLookup objLoanOBLookup = new LoanOBLookup();
                    objLoanOBLookup.LoanMasrterID = Convert.ToInt32(sdr["LoanMasterID"]);
                    objLoanOBLookup.GroupID = Convert.ToInt32(sdr["GroupID"]);
                    objLoanOBLookup.GroupName = Convert.ToString(sdr["GroupName"]);
                    objLoanOBLookup.ROI = Convert.ToDecimal(sdr["ROI"].ToString());
                    objLoanOBLookup.InterestDue = Convert.ToDecimal(sdr["InterestDue"]);
                    objLoanOBLookup.LoanAmountGiven = Convert.ToDecimal(sdr["DisbursedAmount"]);
                    objLoanOBLookup.LoanDisbursedDate = sdr["DisbursementDate"].ToString();
                    objLoanOBLookup.MonthlyDemand = Convert.ToDecimal(sdr["EMI"]);
                    objLoanOBLookup.PrincipalDue = Convert.ToDecimal(sdr["PrincipalDue"]);
                    objLoanOBLookup.PrincipalOutstanding = Convert.ToDecimal(sdr["OutStandingAmount"]);
                    objLoanOBLookup.SLAccountNumber = sdr["SLAccountNumber"].ToString();
                    lstloanOBLookup.Add(objLoanOBLookup);
                }
            }
            catch (Exception ex)
            {

            }
            return lstloanOBLookup;
        }
        public List<LoanOBLookup> LoanOBLookUpList(char loanType, int GroupID)
        {
            List<LoanOBLookup> lstloanOBLookup = new List<LoanOBLookup>();
            try
            {
                AdoHelper obj = new AdoHelper();

                SqlParameter[] parms = new SqlParameter[2];
                parms[0] = new SqlParameter("@LoanType", loanType);
                parms[0].SqlDbType = SqlDbType.Char;

                parms[1] = new SqlParameter("@GroupId", GroupID);
                parms[1].SqlDbType = SqlDbType.Int;
                SqlDataReader sdr = obj.ExecDataReaderProc("uspLoanMasterGetGroupOldLoansLookUp", parms);
                while (sdr.Read())
                {
                    LoanOBLookup objLoanOBLookup = new LoanOBLookup();
                    objLoanOBLookup.LoanMasrterID = DBNull.Value == sdr["LoanMasterID"] ? 0 : Convert.ToInt32(sdr["LoanMasterID"]);
                    objLoanOBLookup.GroupID = DBNull.Value == sdr["GroupID"] ? 0 : Convert.ToInt32(sdr["GroupID"]);
                    objLoanOBLookup.ROI = DBNull.Value == sdr["ROI"] ? 0 : Convert.ToDecimal(sdr["ROI"].ToString());
                    objLoanOBLookup.InterestDue = DBNull.Value == sdr["InterestDue"] ? 0 : Convert.ToDecimal(sdr["InterestDue"]);
                    objLoanOBLookup.LoanAmountGiven = DBNull.Value == sdr["DisbursedAmount"] ? 0 : Convert.ToDecimal(sdr["DisbursedAmount"]);
                    objLoanOBLookup.LoanDisbursedDate = DBNull.Value == sdr["DisbursementDate"] ? "01/jan/0001" : Convert.ToDateTime(sdr["DisbursementDate"]).ToString("dd/MMM/yyyy");
                    objLoanOBLookup.MonthlyDemand = DBNull.Value == sdr["EMI"] ? 0 : Convert.ToDecimal(sdr["EMI"]);
                    objLoanOBLookup.PrincipalDue = DBNull.Value == sdr["PrincipalDue"] ? 0 : Convert.ToDecimal(sdr["PrincipalDue"]);
                    objLoanOBLookup.PrincipalOutstanding = DBNull.Value == sdr["OutStandingAmount"] ? 0 : Convert.ToDecimal(sdr["OutStandingAmount"]);
                    objLoanOBLookup.MemberID = DBNull.Value == sdr["MemberID"] ? 0 : Convert.ToInt32(sdr["MemberID"]);
                    objLoanOBLookup.SLAccountNumber = DBNull.Value == sdr["SLAccountNumber"] ? string.Empty : sdr["SLAccountNumber"].ToString();
                    objLoanOBLookup.MemberName = DBNull.Value == sdr["MemberName"] ? string.Empty : sdr["MemberName"].ToString();
                    lstloanOBLookup.Add(objLoanOBLookup);
                }
            }
            catch (Exception ex)
            {

            }
            return lstloanOBLookup;
        }
        public DataSet LoanOBLookUpTable(char loanType, int GroupID)
        {
            DataSet ds = new DataSet();
            List<DepositOBLookup> lstloanOBLookup = new List<DepositOBLookup>();
            try
            {
                AdoHelper obj = new AdoHelper();
                SqlParameter[] parms = new SqlParameter[2];
                parms[0] = new SqlParameter("@LoanType", loanType);
                parms[0].SqlDbType = SqlDbType.Char;

                parms[1] = new SqlParameter("@GroupId", GroupID);
                parms[1].SqlDbType = SqlDbType.Int;
                ds = obj.ExecDataSetProc("uspLoanMasterGetGroupOldLoansLookUpTable", parms);
            }
            catch (Exception ex)
            {

            }
            return ds;
        }
        public LoanOBDto GetByID(int loanMasterid)
        {
            return GetByID(loanMasterid, 'M');
        }
        public LoanOBDto GetByID(int loanMasterid, char loanType)
        {
            LoanOBDto objLoanOBDto = new LoanOBDto();
            LoanOBByLoanDto objLoanOB = new LoanOBByLoanDto();
            try
            {
                AdoHelper obj = new AdoHelper();

                SqlParameter[] parms = new SqlParameter[2];
                parms[0] = new SqlParameter("@LoanMasterID", loanMasterid);
                parms[0].SqlDbType = SqlDbType.Int;
                parms[1] = new SqlParameter("@Type", loanType);
                parms[1].SqlDbType = SqlDbType.Char;

                SqlDataReader sdr = obj.ExecDataReaderProc("uspLoanMasterGetByLoanmasterID", parms);
                int count = 1;
                while (sdr.Read())
                {
                    if (count == 1)
                    {
                        objLoanOBDto.LoanMasrterID1 = Convert.ToInt32(sdr["LoanMasterID"]);
                        objLoanOBDto.AHCode1 = Convert.ToString(sdr["AHCode"]);
                        objLoanOBDto.AHName1 = Convert.ToString(sdr["AHName"]);
                        objLoanOBDto.GroupId = Convert.ToInt32(sdr["GroupID"]);
                        objLoanOBDto.VillageID = Convert.ToInt32(sdr["VillageID"]);
                        objLoanOBDto.ClusterID = Convert.ToInt32(sdr["ClusterID"]);
                        objLoanOBDto.LoanDisbursedDate1 = Convert.ToString(sdr["LoanApplicationDate"]);
                        objLoanOBDto.Purpose1 = Convert.ToInt32(sdr["LoanPurposeID"]);
                        objLoanOBDto.LoanAmountGiven1 = Convert.ToDecimal(sdr["DisbursedAmount"]);
                        objLoanOBDto.Period1 = Convert.ToInt32(sdr["NoofInstallmentsProposed"]);
                        objLoanOBDto.Period1 = Convert.ToInt32(sdr["NoOfInstallments"]);
                        objLoanOBDto.SLAccountNumberAHID1 = Convert.ToInt32(sdr["SLAccountNumber"]);
                        objLoanOBDto.SLAccountNumber1 = Convert.ToString(sdr["SlAccountName"]);
                        objLoanOBDto.Interest1 = Convert.ToInt32(sdr["InterestRateID"]);
                        objLoanOBDto.FinalInstallmentDate1 = Convert.ToString(sdr["InstallmentStartFrom"]);
                        objLoanOBDto.MonthlyDemand1 = Convert.ToDecimal(sdr["MonthlyPrincipalDemand"]);
                        objLoanOBDto.Fundsource1 = Convert.ToInt32(sdr["FundSourceID"]);
                        objLoanOBDto.Project1 = Convert.ToInt32(sdr["ProjectID"]);
                        objLoanOBDto.PrincipalOutstanding1 = Convert.ToDecimal(sdr["OutStandingAmount"]);
                        objLoanOBDto.Interest1 = Convert.ToInt32(sdr["InterestMasterID"]);
                        objLoanOBDto.ROI1 = Convert.ToInt32(sdr["ROI"]);
                        objLoanOBDto.LastPaidDate1 = Convert.ToString(sdr["LastPaidDate"]);
                        objLoanOBDto.DueDay = Convert.ToInt32(sdr["MeetingDay"]);
                        if (sdr["InterestDue"] != DBNull.Value)
                            objLoanOBDto.InterestDue1 = Convert.ToDecimal(sdr["InterestDue"]);
                        if (sdr["MemberID"] != DBNull.Value)
                            objLoanOBDto.MemberID = Convert.ToInt32(sdr["MemberID"]);
                        if (sdr["PrincipalDue"] != DBNull.Value)
                            objLoanOBDto.PrincipalOD1 = Convert.ToDecimal(sdr["PrincipalDue"]);
                    }
                    else if (count == 2)
                    {
                        objLoanOBDto.LoanMasrterID2 = Convert.ToInt32(sdr["LoanMasterID"]);
                        objLoanOBDto.AHCode2 = Convert.ToString(sdr["AHCode"]);
                        objLoanOBDto.AHName2 = Convert.ToString(sdr["AHName"]);
                        objLoanOBDto.GroupId = Convert.ToInt32(sdr["GroupID"]);
                        objLoanOBDto.LoanDisbursedDate2 = Convert.ToString(sdr["LoanApplicationDate"]);
                        objLoanOBDto.Purpose2 = Convert.ToInt32(sdr["LoanPurposeID"]);
                        objLoanOBDto.LoanAmountGiven2 = Convert.ToDecimal(sdr["LoanAmountApplied"]);
                        objLoanOBDto.Period2 = Convert.ToInt32(sdr["NoofInstallmentsProposed"]);
                        objLoanOBDto.LoanAmountGiven2 = DBNull.Value == sdr["DisbursedAmount"] ? 0 : Convert.ToDecimal(sdr["DisbursedAmount"]);
                        objLoanOBDto.Period2 = sdr["NoOfInstallments"] == DBNull.Value ? 0 : Convert.ToInt32(sdr["NoOfInstallments"]);
                        objLoanOBDto.SLAccountNumberAHID2 = sdr["SLAccountNumber"] == DBNull.Value ? 0 : Convert.ToInt32(sdr["SLAccountNumber"]);
                        objLoanOBDto.SLAccountNumber2 = sdr["SlAccountName"] == DBNull.Value ? string.Empty : Convert.ToString(sdr["SlAccountName"]);
                        objLoanOBDto.Interest2 = Convert.ToInt32(sdr["InterestRateID"]);
                        objLoanOBDto.FinalInstallmentDate2 = Convert.ToString(sdr["InstallmentStartFrom"]);
                        objLoanOBDto.MonthlyDemand2 = Convert.ToDecimal(sdr["MonthlyPrincipalDemand"]);
                        objLoanOBDto.Fundsource2 = Convert.ToInt32(sdr["FundSourceID"]);
                        objLoanOBDto.Project2 = Convert.ToInt32(sdr["ProjectID"]);
                        objLoanOBDto.PrincipalOutstanding2 = Convert.ToDecimal(sdr["OutStandingAmount"]);
                        objLoanOBDto.Interest2 = Convert.ToInt32(sdr["InterestMasterID"]);
                        objLoanOBDto.DueDay = Convert.ToInt32(sdr["MeetingDay"]);
                        objLoanOBDto.LastPaidDate2 = Convert.ToString(sdr["LastPaidDate"]);
                        objLoanOBDto.ROI2 = Convert.ToInt32(sdr["ROI"]);
                        if (sdr["InterestDue"] != DBNull.Value)
                            objLoanOBDto.InterestDue2 = Convert.ToDecimal(sdr["InterestDue"]);
                        if (sdr["MemberID"] != DBNull.Value)
                            objLoanOBDto.MemberID = Convert.ToInt32(sdr["MemberID"]);
                        if (sdr["PrincipalDue"] != DBNull.Value)
                            objLoanOBDto.PrincipalOD2 = Convert.ToDecimal(sdr["PrincipalDue"]);
                    }
                    else if (count == 3)
                    {
                        objLoanOBDto.LoanMasrterID3 = Convert.ToInt32(sdr["LoanMasterID"]);
                        // objLoanOBDto.MemberId = Convert.ToInt32(sdr["MemberID"]);
                        objLoanOBDto.GroupId = Convert.ToInt32(sdr["GroupID"]);
                        objLoanOBDto.AHCode3 = Convert.ToString(sdr["AHCode"]);
                        objLoanOBDto.AHName3 = Convert.ToString(sdr["AHName"]);
                        objLoanOBDto.LoanDisbursedDate3 = Convert.ToString(sdr["LoanApplicationDate"]);
                        objLoanOBDto.Purpose3 = Convert.ToInt32(sdr["LoanPurposeID"]);
                        objLoanOBDto.LoanAmountGiven3 = Convert.ToDecimal(sdr["LoanAmountApplied"]);
                        objLoanOBDto.Period3 = Convert.ToInt32(sdr["NoofInstallmentsProposed"]);
                        objLoanOBDto.LoanAmountGiven3 = Convert.ToDecimal(sdr["DisbursedAmount"]);
                        objLoanOBDto.Period3 = Convert.ToInt32(sdr["NoOfInstallments"]);
                        objLoanOBDto.SLAccountNumberAHID3 = Convert.ToInt32(sdr["SLAccountNumber"]);
                        objLoanOBDto.SLAccountNumber3 = Convert.ToString(sdr["SlAccountName"]); objLoanOBDto.SLAccountNumber3 = Convert.ToString(sdr["SlAccountName"]);
                        objLoanOBDto.Interest3 = Convert.ToInt32(sdr["InterestRateID"]);
                        objLoanOBDto.FinalInstallmentDate3 = Convert.ToString(sdr["InstallmentStartFrom"]);
                        objLoanOBDto.MonthlyDemand3 = Convert.ToDecimal(sdr["MonthlyPrincipalDemand"]);
                        objLoanOBDto.Fundsource3 = Convert.ToInt32(sdr["FundSourceID"]);
                        objLoanOBDto.Project3 = Convert.ToInt32(sdr["ProjectID"]);
                        objLoanOBDto.PrincipalOutstanding3 = Convert.ToDecimal(sdr["OutStandingAmount"]);
                        objLoanOBDto.Interest3 = Convert.ToInt32(sdr["InterestMasterID"]);
                        objLoanOBDto.ROI3 = Convert.ToInt32(sdr["ROI"]);
                        objLoanOBDto.DueDay = Convert.ToInt32(sdr["MeetingDay"]);
                        objLoanOBDto.LastPaidDate3 = Convert.ToString(sdr["LastPaidDate"]);
                        if (sdr["MemberID"] != DBNull.Value)
                            objLoanOBDto.MemberID = Convert.ToInt32(sdr["MemberID"]);
                        if (sdr["InterestDue"] != DBNull.Value)
                            objLoanOBDto.InterestDue3 = Convert.ToDecimal(sdr["InterestDue"]);
                        if (sdr["PrincipalDue"] != DBNull.Value)
                            objLoanOBDto.PrincipalOD3 = Convert.ToDecimal(sdr["PrincipalDue"]);
                    }
                    else if (count == 4)
                    {
                        objLoanOBDto.LoanMasrterID4 = Convert.ToInt32(sdr["LoanMasterID"]);
                        //  objLoanOBDto.MemberId = Convert.ToInt32(sdr["MemberID"]);
                        objLoanOBDto.GroupId = Convert.ToInt32(sdr["GroupID"]);
                        objLoanOBDto.AHCode4 = Convert.ToString(sdr["AHCode"]);
                        objLoanOBDto.AHName4 = Convert.ToString(sdr["AHName"]);
                        objLoanOBDto.LoanDisbursedDate4 = Convert.ToString(sdr["LoanApplicationDate"]);
                        objLoanOBDto.Purpose4 = Convert.ToInt32(sdr["LoanPurposeID"]);
                        objLoanOBDto.LoanAmountGiven4 = Convert.ToDecimal(sdr["LoanAmountApplied"]);
                        objLoanOBDto.Period4 = Convert.ToInt32(sdr["NoofInstallmentsProposed"]);
                        objLoanOBDto.LoanAmountGiven4 = Convert.ToDecimal(sdr["DisbursedAmount"]);
                        objLoanOBDto.Period4 = Convert.ToInt32(sdr["NoOfInstallments"]);
                        objLoanOBDto.SLAccountNumberAHID4 = Convert.ToInt32(sdr["SLAccountNumber"]);
                        objLoanOBDto.SLAccountNumber4 = Convert.ToString(sdr["SlAccountName"]);
                        objLoanOBDto.Interest4 = Convert.ToInt32(sdr["InterestRateID"]);
                        objLoanOBDto.FinalInstallmentDate4 = Convert.ToString(sdr["InstallmentStartFrom"]);
                        objLoanOBDto.MonthlyDemand4 = Convert.ToDecimal(sdr["MonthlyPrincipalDemand"]);
                        objLoanOBDto.Fundsource4 = Convert.ToInt32(sdr["FundSourceID"]);
                        objLoanOBDto.Project4 = Convert.ToInt32(sdr["ProjectID"]);
                        objLoanOBDto.PrincipalOutstanding4 = Convert.ToDecimal(sdr["OutStandingAmount"]);
                        objLoanOBDto.Interest4 = Convert.ToInt32(sdr["InterestMasterID"]);
                        objLoanOBDto.DueDay = Convert.ToInt32(sdr["MeetingDay"]);
                        objLoanOBDto.ROI4 = Convert.ToInt32(sdr["ROI"]);
                        objLoanOBDto.LastPaidDate4 = Convert.ToString(sdr["LastPaidDate"]);
                        if (sdr["InterestDue"] != DBNull.Value)
                            objLoanOBDto.InterestDue4 = Convert.ToDecimal(sdr["InterestDue"]);
                        if (sdr["MemberID"] != DBNull.Value)
                            objLoanOBDto.MemberID = Convert.ToInt32(sdr["MemberID"]);
                        if (sdr["PrincipalDue"] != DBNull.Value)
                            objLoanOBDto.PrincipalOD4 = Convert.ToDecimal(sdr["PrincipalDue"]);
                    }
                    count++;
                }
            }
            catch (Exception ex)
            {

            }
            return objLoanOBDto;

        }
        public LoanOBByLoanDto InsertLoanOB(LoanOBByLoanDto loanOBDto)
        {
            LoanOBByLoanDto result = loanOBDto;

            try
            {
                AdoHelper obj = new AdoHelper();

                SqlParameter[] parms = new SqlParameter[21];
                if (loanOBDto.LoanMasrterID > 0)
                {
                    parms = new SqlParameter[23];
                    parms[21] = new SqlParameter("@LoanMasterID", loanOBDto.LoanMasrterID);
                    parms[21].SqlDbType = SqlDbType.Int;
                    parms[22] = new SqlParameter("@SLAccountNumberAHID", loanOBDto.SLAccountNumberAHID);
                    parms[22].SqlDbType = SqlDbType.Int;

                }
                parms[0] = new SqlParameter("@LoanType", loanOBDto.LoanType);
                parms[0].SqlDbType = SqlDbType.Char;

                parms[1] = new SqlParameter("@MemberID", loanOBDto.MemberId);
                parms[1].SqlDbType = SqlDbType.Int;

                parms[2] = new SqlParameter("@GroupID", loanOBDto.GroupId);
                parms[2].SqlDbType = SqlDbType.Int;

                parms[3] = new SqlParameter("@LoanApplicationDate", loanOBDto.LoanDisbursedDate);
                parms[3].SqlDbType = SqlDbType.Date;

                parms[4] = new SqlParameter("@LoanPurpose", loanOBDto.Purpose);
                parms[4].SqlDbType = SqlDbType.Int;

                parms[5] = new SqlParameter("@LoanAmountApplied", loanOBDto.LoanAmountGiven);
                parms[5].SqlDbType = SqlDbType.Decimal;


                parms[6] = new SqlParameter("@DisbursedAmount", loanOBDto.LoanAmountGiven);
                parms[6].SqlDbType = SqlDbType.Decimal;

                parms[7] = new SqlParameter("@NoOfInstallments", loanOBDto.Period);
                parms[7].SqlDbType = SqlDbType.Int;

                parms[8] = new SqlParameter("@DisbursementDate", loanOBDto.LoanDisbursedDate);
                parms[8].SqlDbType = SqlDbType.Date;

                parms[9] = new SqlParameter("@SLAccountNumber", loanOBDto.SLAccountNumber);
                parms[9].SqlDbType = SqlDbType.VarChar;

                parms[10] = new SqlParameter("@GroupInterstRateID", loanOBDto.Interest);
                parms[10].SqlDbType = SqlDbType.Int;

                parms[11] = new SqlParameter("@InterestRateID", loanOBDto.Interest);
                parms[11].SqlDbType = SqlDbType.Int;

                parms[12] = new SqlParameter("@InstallmentStartFrom", loanOBDto.FinalInstallmentDate);
                parms[12].SqlDbType = SqlDbType.Date;


                parms[13] = new SqlParameter("@MonthlyPrincipalDemand", loanOBDto.MonthlyDemand);
                parms[13].SqlDbType = SqlDbType.Decimal;

                parms[14] = new SqlParameter("@FundSourceID", loanOBDto.Fundsource);
                parms[14].SqlDbType = SqlDbType.Int;

                parms[15] = new SqlParameter("@ProjectID", loanOBDto.Project);
                parms[15].SqlDbType = SqlDbType.Int;

                parms[16] = new SqlParameter("@UserID", loanOBDto.UserID);
                parms[16].SqlDbType = SqlDbType.Int;

                parms[17] = new SqlParameter("@OutStandingAmount", loanOBDto.PrincipalOutstanding);
                parms[17].SqlDbType = SqlDbType.Decimal;

                parms[18] = new SqlParameter("@InterestMasterID", loanOBDto.Interest);
                parms[18].SqlDbType = SqlDbType.Int;

                parms[19] = new SqlParameter("@LastPaidDate", loanOBDto.LastPaidDate);
                parms[19].SqlDbType = SqlDbType.Date;

                parms[20] = new SqlParameter("@PrincipalOD", loanOBDto.PrincipalOD);
                parms[20].SqlDbType = SqlDbType.Int;

                SqlDataReader sdr = obj.ExecDataReaderProc("uspLoanDisbursementInsertOldLoans", parms);
                while (sdr.Read())
                {
                    result.LoanMasrterID = Convert.ToInt32(sdr["LoanMasterID"]);
                    result.SLAccountNumberAHID = Convert.ToInt32(sdr["SLAccountNumberAHID"]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;

        }

        public ResultDto DeleteLoanOBByMemberID(int memberid)
        {
            ResultDto resultDto = new ResultDto();
            try
            {
                AdoHelper obj = new AdoHelper();

                SqlParameter[] parms = new SqlParameter[1];
                parms[0] = new SqlParameter("@MemberID", memberid);
                parms[0].SqlDbType = SqlDbType.Int;

                int result = obj.ExecNonQueryProc("uspLoanMasterDeleteByMemberID", parms);
                if (result > 0) { resultDto.ObjectCode = "LoanOB Record Deleted Successfully"; }
                else { resultDto.ObjectCode = "Error Occuring While Deleting The LoanOB Record"; }

            }
            catch (Exception)
            {

                throw;
            }

            return resultDto;

        }

    }
}
