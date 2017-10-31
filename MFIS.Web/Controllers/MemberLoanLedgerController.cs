using BusinessEntities;
using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MFIS.Web.Controllers
{
    public class MemberLoanLedgerController : BaseController
    {
        SqlConnection con = new SqlConnection(DataLogic.DBConstants.MFIS_CS);
        [HttpGet]
        public ActionResult MemberLoanLedgerLookup()
        {
            DataSet ds = GetAllGeneralLedgers();
            ViewBag.MemberLoanLedgerdto = ds;
            return View();
        }
        public DataSet GetAllGeneralLedgers()
        {
            MemberLoanLedgerDto objmemberloanledgerdto = new MemberLoanLedgerDto();
            SqlDataAdapter da = new SqlDataAdapter("uspMemberloanLedger", con);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }

        public List<MemberLoanLedgerDto> GetMemberLoanLedger()
        {
            List<MemberLoanLedgerDto> lstmemberLedgerDto = new List<MemberLoanLedgerDto>();
            SqlCommand cmd = new SqlCommand("uspMemberloanLedger", con);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            MemberLoanLedgerDto memberLedgerDto = new MemberLoanLedgerDto();
            memberLedgerDto.lstmemberloanledger = new List<MemberLoanLedgerTransactiondto>();
            if (dr.Read())
            {
                memberLedgerDto.OrganizationName = dr["OrganizationName"].ToString();
                memberLedgerDto.MemberName = dr["MemberName"].ToString();
                memberLedgerDto.GroupCode = dr["GroupCode"].ToString();
                memberLedgerDto.GroupName = dr["GroupName"].ToString();
                memberLedgerDto.ACNumber = dr["AccountNumber"].ToString();
                memberLedgerDto.Cluster = dr["Cluster"].ToString();
                memberLedgerDto.DueDate = Convert.ToDateTime(dr["DueDate"]);
                memberLedgerDto.EMIPrincipal = Convert.ToDecimal(dr["EMIPrincipal"]);
                memberLedgerDto.FundSource = dr["FundSourse"].ToString();
                memberLedgerDto.InstalmentFrequency = Convert.ToDecimal(dr["InstalmentFrequency"]);
                memberLedgerDto.InterestRate = Convert.ToDecimal(dr["InterestRate"]);
                memberLedgerDto.LoanDisbursed = Convert.ToInt32(dr["LoanDisbursed"]);
                memberLedgerDto.NoOfInstallments = Convert.ToInt32(dr["NoOfInstallments"]);
                memberLedgerDto.Panchayat = dr["Neighbourhood"].ToString();
                memberLedgerDto.PenalInterestRate = Convert.ToDecimal(dr["PenalInterestrate"]);
                memberLedgerDto.Project = dr["Project"].ToString();
                memberLedgerDto.Village = dr["Village"].ToString();
            }
            if (dr.NextResult())
            {
                MemberLoanLedgerTransactiondto objTrans = null;
                while (dr.Read())
                {
                    objTrans = new MemberLoanLedgerTransactiondto();
                    objTrans.Date = Convert.ToDateTime(dr["Date"]);
                    objTrans.VoucherNumber = Convert.ToString(dr["VocherType"]);
                    objTrans.ReceiptNumber = dr["Repayment"].ToString();
                    objTrans.TotalRepayment = Convert.ToInt32(dr["TotalRepaymetns"]);
                    objTrans.AHCode = dr["TransactionAHCode"].ToString();
                    objTrans.Disbursement = Convert.ToDecimal(dr["Disbursement"]);
                    objTrans.DemandPrincipal = Convert.ToDecimal(dr["Dimandprincipal"]);
                    objTrans.Repaid = Convert.ToDecimal(dr["Repaid"]);
                    objTrans.Balance = Convert.ToDecimal(dr["Balance"]);
                    objTrans.BalanceType = Convert.ToString(dr["balancetype"]);
                    objTrans.Noofdays = Convert.ToInt32(dr["NoOfDays"]);
                    objTrans.PrepaymentPrincipal = Convert.ToDecimal(dr["Prepaymentprincipal"]);
                    objTrans.OverduePrincipal = Convert.ToDecimal(dr["overDueprincipal"]);
                    objTrans.TotalOutstandingwithinterest = Convert.ToDecimal(dr["TotalOutstandingprincipal"]);
                    memberLedgerDto.lstmemberloanledger.Add(objTrans);
                    lstmemberLedgerDto.Add(memberLedgerDto);
                }
            }
            return lstmemberLedgerDto;
        }

        public ActionResult ExportsReports()
        {
            List<MemberLoanLedgerDto> ds = GetMemberLoanLedger();
            ReportDocument rd = new ReportDocument();
            rd.Load(Server.MapPath("~/Reports/RegistersReports/MemberLoanLedger.rpt"));
            rd.SetDataSource(ds);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            try
            {
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", "MemberLoanLedger.pdf");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
