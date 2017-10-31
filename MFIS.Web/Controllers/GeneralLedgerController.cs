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
    public class GeneralLedgerController : BaseController
    {
        SqlConnection con = new SqlConnection(DataLogic.DBConstants.MFIS_CS);

        public ActionResult CreateGeneralLedger()
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
        public ActionResult CreateGeneralLedger(FormCollection form)
        {
            int ahId = Convert.ToInt32(form["AccountHead"]);
            DateTime dtFromDate = Convert.ToDateTime(form["FromDate"]);
            DateTime dtToDate = Convert.ToDateTime(form["ToDate"]);

            string orgAddress = string.Empty, ahName = string.Empty;

            List<GeneralLedgerDto> lstGeneralLedger = GetAllGeneralLedgers(ahId, GroupInfo.GroupID, dtFromDate, dtToDate, out orgAddress, out ahName);

            bool isGroup = GroupInfo != null && GroupInfo.GroupID > 1;
            ViewBag.isGroup = isGroup;

            ViewBag.AHID = ahId;
            ViewBag.FromDate = dtFromDate;
            ViewBag.ToDate = dtToDate;
            ViewBag.Address = orgAddress;

            LoadAccountHeads();
            return View(lstGeneralLedger);
        }

        public List<GeneralLedgerDto> GetAllGeneralLedgers(int AHID, int groupId, DateTime FromDate, DateTime ToDate, out string OrgAddress, out string AhName)
        {
            List<GeneralLedgerDto> lstLedger = new List<GeneralLedgerDto>();
            SqlCommand cmd = new SqlCommand("uspGeneralLedgerDReport", con);
            
            cmd.Parameters.AddWithValue("@AHID", AHID);
            cmd.Parameters.AddWithValue("@GroupId", groupId);
            cmd.Parameters.AddWithValue("@StartDate", FromDate);
            cmd.Parameters.AddWithValue("@EndDate", ToDate);

            cmd.Parameters.Add("@OrgAddress", SqlDbType.VarChar, 1000);
            cmd.Parameters["@OrgAddress"].Direction = ParameterDirection.Output;

            cmd.Parameters.Add("@AccountHeadName", SqlDbType.VarChar, 100);
            cmd.Parameters["@AccountHeadName"].Direction = ParameterDirection.Output;
            
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
                if (dr["DrAmount"] != DBNull.Value)
                    obj.DrAmount = Convert.ToDecimal(dr["DrAmount"]);
                if (dr["CrAmount"] != DBNull.Value)
                    obj.CrAmount = Convert.ToDecimal(dr["CrAmount"]);
                if (dr["Balance"] != DBNull.Value)
                    obj.Balance = Convert.ToDecimal(dr["Balance"]);
                lstLedger.Add(obj);
            }

            OrgAddress = Convert.ToString(cmd.Parameters["@OrgAddress"].Value);
            AhName = Convert.ToString(cmd.Parameters["@AccountHeadName"].Value);
            con.Close();

            return lstLedger;
        }
        public ActionResult ExportsReports(string ahID, string fromDate, string toDate)
        {
            int ahId = Convert.ToInt32(ahID);
            DateTime dtFromDate = fromDate.ConvertToDateTime();
            DateTime dtToDate = toDate.ConvertToDateTime();

            string orgAddress = string.Empty, ahName = string.Empty, ahCode = string.Empty;

            List<GeneralLedgerDto> lstLedgers = GetAllGeneralLedgers(ahId, GroupInfo.GroupID, dtFromDate, dtToDate, out orgAddress, out ahName);
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
