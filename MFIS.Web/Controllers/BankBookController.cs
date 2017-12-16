using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessEntities;
using System.Data;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using BusinessLogic;
using CoreComponents;

namespace MFIS.Web.Controllers
{
    public class BankBookController : BaseController
    {
        SqlConnection con = new SqlConnection(DataLogic.DBConstants.MFIS_CS);

        [HttpGet]
        public ActionResult GroupBankBook()
        {
            LoadAccountHeads();
            bool isGroup = GroupInfo != null && GroupInfo.GroupID > 1;
            ViewBag.isGroup = isGroup;
            return View();
        }

        public void LoadAccountHeads()
        {
            AccountHeadService ahService = new AccountHeadService();
            List<SelectListDto> lstAccountHeads = null;
            if (GroupInfo != null && GroupInfo.GroupID > 0)
                lstAccountHeads = ahService.GetGeneralLedgerAccountHeads(GroupInfo.GroupID);
            else
                lstAccountHeads = ahService.GetGeneralLedgerAccountHeads(null);

            ViewBag.AccountHeads = new SelectList(lstAccountHeads, "ID", "Text");
        }

        [HttpPost]
        public ActionResult GroupBankBook(FormCollection form)
        {
            DateTime dtFromDate = Convert.ToDateTime(form["FromDate"]);
            DateTime dtToDate = Convert.ToDateTime(form["ToDate"]);

            string orgAddress = string.Empty, ahName = string.Empty;

            List<GeneralLedgerDto> lstBankBook = GetAllBankBook(GroupInfo.GroupID, dtFromDate, dtToDate, out orgAddress);
            bool isGroup = GroupInfo != null && GroupInfo.GroupID > 1;
            ViewBag.isGroup = isGroup;
            ViewBag.FromDate = dtFromDate;
            ViewBag.ToDate = dtToDate;
            ViewBag.Address = orgAddress;
            return View(lstBankBook);
        }

        public List<GeneralLedgerDto> GetAllBankBook(int groupId, DateTime FromDate, DateTime ToDate, out string OrgAddress)
        {
            List<GeneralLedgerDto> lstLedger = new List<GeneralLedgerDto>();
            SqlCommand cmd = new SqlCommand("uspGroupBankBook_v1", con);


            cmd.Parameters.AddWithValue("@GroupId", groupId);
            cmd.Parameters.AddWithValue("@StartDate", FromDate);
            cmd.Parameters.AddWithValue("@EndDate", ToDate);

            cmd.Parameters.Add("@OrgAddress", SqlDbType.VarChar, 1000);
            cmd.Parameters["@OrgAddress"].Direction = ParameterDirection.Output;

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;
            if (con.State == ConnectionState.Closed)
                con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                GeneralLedgerDto obj = new GeneralLedgerDto();

                obj.Date = Convert.ToDateTime(dr["TDate"]);
                obj.AHCode = Convert.ToString(dr["AHCode"]);
                obj.AHName = Convert.ToString(dr["AHName"]);
                obj.BankName = Convert.ToString(dr["BankName"]);
                obj.Branch = Convert.ToString(dr["BranchName"]);
                obj.AccountNumner = Convert.ToString(dr["AccountNumber"]);
                obj.VrNumber = Convert.ToString(dr["VoucherNumber"]);
                if (dr["DrAmount"] != DBNull.Value)
                    obj.DrAmount = Convert.ToDecimal(dr["DrAmount"]);
                if (dr["CrAmount"] != DBNull.Value)
                    obj.CrAmount = Convert.ToDecimal(dr["CrAmount"]);
                if (dr["Balance"] != DBNull.Value)
                    obj.Balance = Convert.ToDecimal(dr["Balance"]);
                lstLedger.Add(obj);
            }
            ViewBag.BankBalance = lstLedger != null && lstLedger.Count > 0 ? lstLedger[lstLedger.Count - 1].Balance.ToDisplayCurrencyRPT() : "";
            ViewBag.BankName = lstLedger != null && lstLedger.Count > 0 ? lstLedger.Where(x => x.AHName == "BANK").Select(x => x.BankName).First().ToString() : "";
            ViewBag.AccountNumber = lstLedger != null && lstLedger.Count > 0 ? lstLedger.Where(x => x.AHName == "BANK").Select(x => x.AccountNumner).First().ToString() : "";
            ViewBag.Branch = lstLedger != null && lstLedger.Count > 0  ? lstLedger.Where(x => x.AHName == "BANK").Select(x => x.Branch).First().ToString() : "";
            ViewBag.Group = GroupInfo.GroupName;
            ViewBag.GroupCode = GroupInfo.GroupCode;
            ViewBag.Cluster = GroupInfo.Cluster;
            OrgAddress = Convert.ToString(cmd.Parameters["@OrgAddress"].Value);
            con.Close();

            return lstLedger;
        }
        public ActionResult ExportsReports(string fromDate, string toDate)
        {
            DateTime dtFromDate = fromDate.ConvertToDateTime();
            DateTime dtToDate = toDate.ConvertToDateTime();

            string orgAddress = string.Empty, ahName = string.Empty, ahCode = string.Empty;

            List<GeneralLedgerDto> lstLedgers = GetAllBankBook(GroupInfo.GroupID, dtFromDate, dtToDate, out orgAddress);
            ReportDocument rd = new ReportDocument();
            rd.Load(Server.MapPath("~/Reports/RegistersReports/GeneralLedger.rpt"));
            rd.SetDataSource(lstLedgers);

            ahCode = lstLedgers.Count > 0 ? lstLedgers[0].AHCode : string.Empty;
            rd.SetParameterValue("@OrgAddress", orgAddress);
            rd.SetParameterValue("@AHName", ahName);
            rd.SetParameterValue("@AHCode", ahCode);
            rd.SetParameterValue("@FromDate", dtFromDate.ToDisplayDateFormat());
            rd.SetParameterValue("@ToDate", dtToDate.ToDisplayDateFormat());

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            try
            {
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", "GeneralLedger.pdf");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
