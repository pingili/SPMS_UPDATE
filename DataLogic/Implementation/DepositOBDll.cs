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
    public class DepositOBDll
    {
        public List<DepositOBLookup> DepositOBLookUpList(bool Type, int GroupId)
        {
            List<DepositOBLookup> lstloanOBLookup = new List<DepositOBLookup>();
            try
            {
                AdoHelper obj = new AdoHelper();
                SqlParameter[] parms = new SqlParameter[2];
                parms[0] = new SqlParameter("@Type", Type);
                parms[0].SqlDbType = System.Data.SqlDbType.Bit;

                parms[1] = new SqlParameter("@GroupId", GroupId);
                parms[1].SqlDbType = System.Data.SqlDbType.Int;
                SqlDataAdapter dadapter = new SqlDataAdapter();

                SqlDataReader sdr = obj.ExecDataReaderProc("uspDepositsLookupList", parms);
                while (sdr.Read())
                {
                    DepositOBLookup objLoanOBLookup = new DepositOBLookup();
                    objLoanOBLookup.GroupId = Convert.ToInt32(sdr["GroupId"]);
                    objLoanOBLookup.GroupName = Convert.ToString(sdr["GroupName"]);
                    objLoanOBLookup.SLAccountNumber = sdr["SLAccountNumber"].ToString();
                    objLoanOBLookup.DepositAmount = Convert.ToDecimal(sdr["DepositAmount"]);
                    //objLoanOBLookup.InterestDue = Convert.ToString(sdr["InterestDue"]);
                    //objLoanOBLookup.Interest = Convert.ToDecimal(sdr["Interest"]);
                    objLoanOBLookup.ROI = Convert.ToDecimal(sdr["ROI"].ToString());
                    objLoanOBLookup.MemberId = Convert.ToInt32(sdr["MemberID"]);
                    objLoanOBLookup.MemberName = Convert.ToString(sdr["MemberName"]);
                    //objLoanOBLookup.MonthlyDemand = Convert.ToDecimal(sdr["MonthlyDemand"]);
                    lstloanOBLookup.Add(objLoanOBLookup);
                }
            }
            catch (Exception ex)
            {

            }
            return lstloanOBLookup;
        }

        public DataSet DepositOBLookUpTable(bool Type, int GroupId)
        {
            DataSet ds = new DataSet();
            List<DepositOBLookup> lstloanOBLookup = new List<DepositOBLookup>();
            try
            {
                AdoHelper obj = new AdoHelper();
                SqlParameter[] parms = new SqlParameter[2];
                parms[0] = new SqlParameter("@Type", Type);
                parms[0].SqlDbType = System.Data.SqlDbType.Bit;

                parms[1] = new SqlParameter("@GroupId", GroupId);
                parms[1].SqlDbType = System.Data.SqlDbType.Int;
                SqlDataAdapter dadapter = new SqlDataAdapter();
                ds = obj.ExecDataSetProc("uspDepositsLookupTable", parms);
            }
            catch (Exception ex)
            {

            }
            return ds;
        }
        public DepositOBBySlAccountDto InsertDepositOB(DepositOBBySlAccountDto loanOBDto)
        {
            DepositOBBySlAccountDto result = loanOBDto;

            try
            {
                AdoHelper obj = new AdoHelper();
                SqlParameter[] parms = null;

                if (loanOBDto.MemberId > 0)
                    parms = new SqlParameter[]{
                new SqlParameter("@ID", loanOBDto.ID){SqlDbType = System.Data.SqlDbType.Int},
                new SqlParameter("@IsMemberDeposit", loanOBDto.IsMemberDeposit){SqlDbType = System.Data.SqlDbType.Bit},
                new SqlParameter("@MemberID", loanOBDto.MemberId){SqlDbType = System.Data.SqlDbType.Int},
                new SqlParameter("@GroupID", loanOBDto.GroupId){SqlDbType = System.Data.SqlDbType.Int},
                new SqlParameter("@SLAccountAHID", loanOBDto.SLAccountNumberAHID){                SqlDbType = System.Data.SqlDbType.Int},
                new SqlParameter("@DepositAcmount", loanOBDto.DepositAmount){SqlDbType = System.Data.SqlDbType.Decimal},
                new SqlParameter("@ROI", loanOBDto.ROI){SqlDbType = System.Data.SqlDbType.Int},
                new SqlParameter("@InterestMasterID", loanOBDto.InterestMasterID){SqlDbType = System.Data.SqlDbType.Int},
                new SqlParameter("@InterestRateID", loanOBDto.InterestRateID){ SqlDbType = System.Data.SqlDbType.Int},
                new SqlParameter("@UserID", loanOBDto.UserID){ SqlDbType = System.Data.SqlDbType.Int},
                new SqlParameter("@SlAccountNumber", loanOBDto.SLAccountNumber){ SqlDbType = System.Data.SqlDbType.VarChar}
                };

                else
                    parms = new SqlParameter[]{
                new SqlParameter("@ID", loanOBDto.ID){SqlDbType = System.Data.SqlDbType.Int},
                new SqlParameter("@IsMemberDeposit", loanOBDto.IsMemberDeposit){SqlDbType = System.Data.SqlDbType.Bit},
                new SqlParameter("@GroupID", loanOBDto.GroupId){SqlDbType = System.Data.SqlDbType.Int},
                new SqlParameter("@SLAccountAHID", loanOBDto.SLAccountNumberAHID){                SqlDbType = System.Data.SqlDbType.Int},
                new SqlParameter("@DepositAcmount", loanOBDto.DepositAmount){SqlDbType = System.Data.SqlDbType.Decimal},
                new SqlParameter("@ROI", loanOBDto.ROI){SqlDbType = System.Data.SqlDbType.Int},
                new SqlParameter("@InterestMasterID", loanOBDto.InterestMasterID){SqlDbType = System.Data.SqlDbType.Int},
                new SqlParameter("@InterestRateID", loanOBDto.InterestRateID){ SqlDbType = System.Data.SqlDbType.Int},
                new SqlParameter("@UserID", loanOBDto.UserID){ SqlDbType = System.Data.SqlDbType.Int},
                new SqlParameter("@SlAccountNumber", loanOBDto.SLAccountNumber){ SqlDbType = System.Data.SqlDbType.VarChar}
                };

                SqlDataReader sdr = obj.ExecDataReaderProc("uspDepositInsertUpdate", parms);
                while (sdr.Read())
                {
                    result.ID = Convert.ToInt32(sdr["ID"]);
                    if (sdr["SLAccountNumberAHID"] != DBNull.Value)
                        result.SLAccountNumberAHID = Convert.ToInt32(sdr["SLAccountNumberAHID"]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;

        }

        public DepositOBDto GetByID(int GroupID)
        {
            return GetByID(GroupID, 0);
        }

        public DepositOBDto GetByMemberID(int MemberID)
        {
            return GetByID(0, MemberID);
        }

        private DepositOBDto GetByID(int GroupID, int MemberID)
        {
            DepositOBDto objDepositOBDto = new DepositOBDto();
            try
            {
                AdoHelper obj = new AdoHelper();

                SqlParameter[] parms = new SqlParameter[1];

                if (GroupID > 0)
                {
                    parms[0] = new SqlParameter("@GroupID", GroupID);
                    parms[0].SqlDbType = System.Data.SqlDbType.Int;
                }

                if (MemberID > 0)
                {
                    parms[0] = new SqlParameter("@MemberID", MemberID);
                    parms[0].SqlDbType = System.Data.SqlDbType.Int;
                }
                SqlDataReader sdr = obj.ExecDataReaderProc("uspDepositgetbyID", parms);
                int count = 1;
                while (sdr.Read())
                {
                    if (count == 1)
                    {
                        objDepositOBDto.Id1 = Convert.ToInt32(sdr["ID"]);
                        objDepositOBDto.GroupId = Convert.ToInt32(sdr["GroupID"]);
                        objDepositOBDto.VillageID = Convert.ToInt32(sdr["VillageID"]);
                        objDepositOBDto.ClusterID = Convert.ToInt32(sdr["ClusterID"]);
                        objDepositOBDto.Interest1 = Convert.ToInt32(sdr["InterestMasterID"]);
                        objDepositOBDto.AHCode1 = Convert.ToString(sdr["AHCode"]);
                        objDepositOBDto.AHName1 = Convert.ToString(sdr["AHName"]);
                        objDepositOBDto.SLAccountNumber1 = Convert.ToString(sdr["SLAccountName"]);
                        objDepositOBDto.SLAccountNumberAHID1 = Convert.ToInt32(sdr["SLaHid"]);
                        objDepositOBDto.DepositBalance1 = Convert.ToInt32(sdr["DepositAcmount"]);
                        if (sdr["InterestAcmount"] != DBNull.Value)
                            objDepositOBDto.IneterestDue1 = Convert.ToDecimal(sdr["InterestAcmount"]);
                        if (sdr["DepositDate"] != DBNull.Value)
                            objDepositOBDto.DepositDate1 = Convert.ToDateTime(sdr["DepositDate"]);
                        if (sdr["Period"] != DBNull.Value)
                            objDepositOBDto.Period1 = Convert.ToInt32(sdr["Period"]);
                        if (sdr["MeturityDate"] != DBNull.Value)
                            objDepositOBDto.MeturityDate1 = Convert.ToDateTime(sdr["MeturityDate"]);
                        if (sdr["LastPaidDate"] != DBNull.Value)
                            objDepositOBDto.LastPaidDate1 = Convert.ToDateTime(sdr["LastPaidDate"]);
                        if (sdr["DemandAmountPerMonth"] != DBNull.Value)
                            objDepositOBDto.DemandAmountPerMonth1 = Convert.ToDecimal(sdr["DemandAmountPerMonth"]);
                        objDepositOBDto.MemberId = MemberID;

                    }
                    else if (count == 2)
                    {
                        objDepositOBDto.Id2 = Convert.ToInt32(sdr["ID"]);
                        objDepositOBDto.Interest2 = Convert.ToInt32(sdr["InterestMasterID"]);
                        objDepositOBDto.AHCode2 = Convert.ToString(sdr["AHCode"]);
                        objDepositOBDto.AHName2 = Convert.ToString(sdr["AHName"]);
                        objDepositOBDto.SLAccountNumber2 = Convert.ToString(sdr["SLAccountName"]);
                        objDepositOBDto.DepositBalance2 = Convert.ToInt32(sdr["DepositAcmount"]);
                        if (sdr["InterestAcmount"] != DBNull.Value)
                            objDepositOBDto.IneterestDue2 = Convert.ToDecimal(sdr["InterestAcmount"]);
                        if (sdr["DepositDate"] != DBNull.Value)
                            objDepositOBDto.DepositDate2 = Convert.ToDateTime(sdr["DepositDate"]);
                        if (sdr["Period"] != DBNull.Value)
                            objDepositOBDto.Period2 = Convert.ToInt32(sdr["Period"]);
                        if (sdr["SLaHid"] != DBNull.Value)
                            objDepositOBDto.SLAccountNumberAHID2 = Convert.ToInt32(sdr["SLaHid"]);
                        if (sdr["MeturityDate"] != DBNull.Value)
                            objDepositOBDto.MeturityDate2 = Convert.ToDateTime(sdr["MeturityDate"]);
                        if (sdr["LastPaidDate"] != DBNull.Value)
                            objDepositOBDto.LastPaidDate2 = Convert.ToDateTime(sdr["LastPaidDate"]);
                        if (sdr["DemandAmountPerMonth"] != DBNull.Value)
                            objDepositOBDto.DemandAmountPerMonth2 = Convert.ToDecimal(sdr["DemandAmountPerMonth"]);
                        objDepositOBDto.MemberId = MemberID;

                    }
                    else if (count == 3)
                    {
                        objDepositOBDto.Id3 = Convert.ToInt32(sdr["ID"]);
                        objDepositOBDto.Interest3 = Convert.ToInt32(sdr["InterestMasterID"]);
                        objDepositOBDto.AHCode3 = Convert.ToString(sdr["AHCode"]);
                        objDepositOBDto.AHName3 = Convert.ToString(sdr["AHName"]);
                        objDepositOBDto.SLAccountNumber3 = Convert.ToString(sdr["SLAccountName"]);
                        if (sdr["DepositAcmount"] != DBNull.Value)
                            objDepositOBDto.DepositBalance3 = Convert.ToInt32(sdr["DepositAcmount"]);
                        if (sdr["InterestAcmount"] != DBNull.Value)
                            objDepositOBDto.IneterestDue3 = Convert.ToDecimal(sdr["InterestAcmount"]);
                        if (sdr["DepositDate"] != DBNull.Value)
                            objDepositOBDto.DepositDate3 = Convert.ToDateTime(sdr["DepositDate"]);
                        if (sdr["SLaHid"] != DBNull.Value)
                            objDepositOBDto.SLAccountNumberAHID3 = Convert.ToInt32(sdr["SLaHid"]);
                        if (sdr["Period"] != DBNull.Value)
                            objDepositOBDto.Period3 = Convert.ToInt32(sdr["Period"]);
                        if (sdr["MeturityDate"] != DBNull.Value)
                            objDepositOBDto.MeturityDate3 = Convert.ToDateTime(sdr["MeturityDate"]);
                        if (sdr["LastPaidDate"] != DBNull.Value)
                            objDepositOBDto.LastPaidDate3 = Convert.ToDateTime(sdr["LastPaidDate"]);
                        if (sdr["DemandAmountPerMonth"] != DBNull.Value)
                            objDepositOBDto.DemandAmountPerMonth3 = Convert.ToDecimal(sdr["DemandAmountPerMonth"]);
                        objDepositOBDto.MemberId = MemberID;

                    }
                    else if (count == 4)
                    {
                        objDepositOBDto.Id4 = Convert.ToInt32(sdr["ID"]);
                        objDepositOBDto.Interest4 = Convert.ToInt32(sdr["InterestMasterID"]);
                        objDepositOBDto.AHCode4 = Convert.ToString(sdr["AHCode"]);
                        objDepositOBDto.AHName4 = Convert.ToString(sdr["AHName"]);
                        objDepositOBDto.SLAccountNumber4 = Convert.ToString(sdr["SLAccountName"]);
                        objDepositOBDto.DepositBalance4 = Convert.ToInt32(sdr["DepositAcmount"]);
                        objDepositOBDto.IneterestDue4 = Convert.ToDecimal(sdr["InterestAcmount"]);
                        objDepositOBDto.DepositDate4 = Convert.ToDateTime(sdr["DepositDate"]);
                        objDepositOBDto.SLAccountNumberAHID4 = Convert.ToInt32(sdr["SLaHid"]);
                        objDepositOBDto.Period4 = Convert.ToInt32(sdr["Period"]);
                        objDepositOBDto.MeturityDate4 = Convert.ToDateTime(sdr["MeturityDate"]);
                        objDepositOBDto.LastPaidDate4 = Convert.ToDateTime(sdr["LastPaidDate"]);
                        objDepositOBDto.DemandAmountPerMonth4 = Convert.ToDecimal(sdr["DemandAmountPerMonth"]);
                        objDepositOBDto.MemberId = MemberID;

                    }
                    count++;
                }
            }
            catch (Exception ex)
            {

            }
            return objDepositOBDto;
        }
    }
}

