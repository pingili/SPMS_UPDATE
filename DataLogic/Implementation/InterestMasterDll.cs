using BlackBeltCoder;
using BusinessEntities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLogic
{
    public class InterestMasterDll
    {
        public InterestMasterDto GetByID(int id)
        {
            InterestMasterDto interestMasterDto = new InterestMasterDto();
            AdoHelper obj = new AdoHelper();
            SqlParameter[] parms = new SqlParameter[1];

            parms[0] = new SqlParameter("@InterestID", id);
            parms[0].SqlDbType = System.Data.SqlDbType.Int;

            SqlDataReader Dr = obj.ExecDataReaderProc("uspGroupInterestByID2", parms);
            if (Dr.Read())
            {
                interestMasterDto.PrincipalAHCode = Dr["AHCode"].ToString();
                interestMasterDto.PrincipalAHID = Convert.ToInt32(Dr["PrincipalAHID"]);
                interestMasterDto.InterestAHID = Convert.ToInt32(Dr["InterestAHID"]);
                interestMasterDto.InterestID = Convert.ToInt32(Dr["InterestID"]);
                interestMasterDto.PrincipalAHName = Dr["AHName"].ToString();
                interestMasterDto.InterestRateID = DBNull.Value == Dr["InterestRateID"] ? 0 : Convert.ToInt32(Dr["InterestRateID"].ToString());
                interestMasterDto.InterestRate = Dr["ROI"] != DBNull.Value ? Convert.ToDecimal(Dr["ROI"]) : 0;
                interestMasterDto.InterestName = DBNull.Value == Dr["InterestName"] ? string.Empty : Dr["InterestName"].ToString();
            }
            return interestMasterDto;
        }
        public InterestMasterDto FedGetByID(int id)
        {
            InterestMasterDto interestMasterDto = new InterestMasterDto();
            AdoHelper obj = new AdoHelper();
            SqlParameter[] parms = new SqlParameter[1];

            parms[0] = new SqlParameter("@InterestID", id);
            parms[0].SqlDbType = System.Data.SqlDbType.Int;

            SqlDataReader Dr = obj.ExecDataReaderProc("uspInterestByID2", parms);
            if (Dr.Read())
            {
                interestMasterDto.PrincipalAHCode = Dr["AHCode"].ToString();
                interestMasterDto.PrincipalAHName = Dr["AHName"].ToString();
                if (Dr["IntrestRateID"] != DBNull.Value)
                    interestMasterDto.InterestRate = Convert.ToDecimal(Dr["IntrestRateID"]);
                if (Dr["PrincipalAHID"] != DBNull.Value)
                    interestMasterDto.PrincipalAHID = Convert.ToInt32(Dr["PrincipalAHID"]);

            }
            return interestMasterDto;
        }

        public InterestMasterDto GetGroupInterestByID(int id)
        {
            InterestMasterDto interestMasterDto = new InterestMasterDto();
            AdoHelper obj = new AdoHelper();
            SqlParameter[] parms = new SqlParameter[1];

            parms[0] = new SqlParameter("@InterestID", id);
            parms[0].SqlDbType = System.Data.SqlDbType.Int;

            SqlDataReader Dr = obj.ExecDataReaderProc("uspGroupInterestByID", parms);
            if (Dr.Read())
            {
                interestMasterDto.PrincipalAHCode = DBNull.Value == Dr["AHCode"] ? string.Empty : Dr["AHCode"].ToString();
                interestMasterDto.PrincipalAHName = DBNull.Value == Dr["AHName"] ? string.Empty : Dr["AHName"].ToString();
                interestMasterDto.PrincipalAHID = DBNull.Value == Dr["InterestAHID"] ? 0 : Convert.ToInt32(Dr["PrincipalAHID"]);
                interestMasterDto.InterestAHID = DBNull.Value == Dr["InterestAHID"] ? 0 : Convert.ToInt32(Dr["InterestAHID"]);
                interestMasterDto.InterestID = DBNull.Value == Dr["InterestID"] ? 0 : Convert.ToInt32(Dr["InterestID"]);
                interestMasterDto.Base = Dr["Base"] == DBNull.Value ? 0 : Convert.ToInt32(Dr["Base"]);
                interestMasterDto.CaluculationMethod = Dr["CaluculationMethod"] == DBNull.Value ? 0 : Convert.ToInt32(Dr["CaluculationMethod"]);
                interestMasterDto.InterestRates = new List<InterestRatesDto>();
            }
            Dr.NextResult();
            while (Dr.Read())
            {
                InterestRatesDto rate = new InterestRatesDto();
                rate.FromDate = Dr["FromDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(Dr["FromDate"]);
                rate.ToDate = Dr["ToDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(Dr["ToDate"]);
                rate.ROI = Dr["ROI"] == DBNull.Value ? 0 : Convert.ToInt32(Dr["ROI"]);
                rate.PenalROI = Dr["PenalROI"] == DBNull.Value ? 0 : Convert.ToDecimal(Dr["PenalROI"]);
                rate.IntrestRateID = Dr["InterestRateID"] == DBNull.Value ? 0 : Convert.ToInt32(Dr["InterestRateID"]);
                interestMasterDto.InterestRates.Add(rate);
            }
            return interestMasterDto;
        }
    }
}
