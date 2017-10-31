using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessEntities;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using CrystalDecisions.CrystalReports.Engine;
//using BusinessLogic.Interface;
using BusinessLogic;
using MFIS.Web.Controllers;

namespace MFIS.Web.Views.TrialBalance
{
    public class TrialBalanceController : BaseController
    {

        SqlConnection con = new SqlConnection(DataLogic.DBConstants.MFIS_CS);

        #region Group Trail Balance Report
        public ActionResult CreateTrialBalance_Group()
        {
            OrganizationService objIOrganizationService = new OrganizationService();
            OrganizationDto organizationDto = objIOrganizationService.GetAll();
            ViewBag.OrganizationDetails = organizationDto;
            TrialBalanceTotalsDto objTotals = new TrialBalanceTotalsDto();
            List<TrialBalanceDto> lstTrialBalance = GetAllTrialBalanceReport_Group(objTotals);
            ViewBag.TrialBalanceTotals = objTotals;
            return View("CreateTrialBalance", lstTrialBalance);
        }
        public List<TrialBalanceDto> GetAllTrialBalanceReport_Group(TrialBalanceTotalsDto objTotals = null)
        {
            OrganizationService objIOrganizationService = new OrganizationService();
            OrganizationDto organizationDto = objIOrganizationService.GetAll();
            int GroupID = GroupInfo != null ? GroupInfo.GroupID : 0;
            if (objTotals == null) objTotals = new TrialBalanceTotalsDto();
            List<TrialBalanceDto> lstTrialBalance = new List<TrialBalanceDto>();
            SqlCommand cmd = new SqlCommand("uspTrialBalanceReport", con);
            cmd.Parameters.Add(new SqlParameter("GroupID", GroupID));
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                TrialBalanceDto obj = new TrialBalanceDto();
                obj.OrganizationName = organizationDto.OrgName;
                obj.OrganizationAddress = organizationDto.Address;
                obj.Ahcode = dr["AHCODE"] == DBNull.Value ? string.Empty : dr["AHCODE"].ToString();
                obj.AhType = dr["AHType"] == DBNull.Value ? default(Int32) : Convert.ToInt32(dr["AHType"]);
                obj.Accounthaedname = dr["ACCOUNTHEADNAME"] == DBNull.Value ? string.Empty : Convert.ToString(dr["ACCOUNTHEADNAME"]);
                obj.Debit1 = dr["OPENING_BALANCE_DEBIT"] == DBNull.Value ? default(double) : Convert.ToDouble(dr["OPENING_BALANCE_DEBIT"]);
                obj.Credit1 = dr["OPENING_BALANCE_CREDIT"] == DBNull.Value ? default(double) : Convert.ToDouble(dr["OPENING_BALANCE_CREDIT"]);
                obj.Debit2 = dr["TRAN_DEBIT"] == DBNull.Value ? default(double) : Convert.ToDouble(dr["TRAN_DEBIT"]);
                obj.Credit2 = dr["TRAN_CREDIT"] == DBNull.Value ? default(double) : Convert.ToDouble(dr["TRAN_CREDIT"]);
                obj.Debit3 = dr["CLOSING_DEBIT"] == DBNull.Value ? default(double) : Convert.ToDouble(dr["CLOSING_DEBIT"]);
                obj.Credit3 = dr["CLOSING_CREDIT"] == DBNull.Value ? default(double) : Convert.ToDouble(dr["CLOSING_CREDIT"]);

                lstTrialBalance.Add(obj);
            }
            if (dr.NextResult())
            {
                if (dr.Read())
                {
                    if (Convert.ToInt32(dr["TOTAL_RECORDS"]) != 0)
                        objTotals.TotalRecords = Convert.ToInt32(dr["TOTAL_RECORDS"]);
                    if (dr["OB_DR_Sum"] != DBNull.Value)
                        objTotals.OpeningBalDrSum = Convert.ToDouble(dr["OB_DR_Sum"]);
                    if (dr["OB_CR_SUM"] != DBNull.Value)
                        objTotals.OpeningBalCrSum = Convert.ToDouble(dr["OB_CR_SUM"]);
                    if (dr["TRAN_DR_SUM"] != DBNull.Value)
                        objTotals.TranBalDrSum = Convert.ToDouble(dr["TRAN_DR_SUM"]);
                    if (dr["TRAN_CR_SUM"] != DBNull.Value)
                        objTotals.TranBalCrSum = Convert.ToDouble(dr["TRAN_CR_SUM"]);
                    if (dr["CLOSING_DR_SUM"] != DBNull.Value)
                        objTotals.ClosingBalDrSum = Convert.ToDouble(dr["CLOSING_DR_SUM"]);
                    if (dr["CLOSING_CR_SUM"] != DBNull.Value)
                        objTotals.ClosingBalCrSum = Convert.ToDouble(dr["CLOSING_CR_SUM"]);
                }
            }
            return lstTrialBalance;
        }
        [HttpGet]
        public ActionResult ExportsReports_Group()
        {

            List<TrialBalanceDto> lstTrialBalance = GetAllTrialBalanceReport_Group();

            ReportDocument rd = new ReportDocument();
            rd.Load(Server.MapPath("~/Reports/FinancialRports/TrialBalance.rpt"));
            rd.SetDataSource(lstTrialBalance);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", "TrialBalance.pdf");

        }
        #endregion

        #region Federation Trail Balance Report
        public ActionResult CreateTrialBalance()
        {
            OrganizationService objIOrganizationService = new OrganizationService();
            OrganizationDto organizationDto = objIOrganizationService.GetAll();
            ViewBag.OrganizationDetails = organizationDto;
            TrialBalanceTotalsDto objTotals = new TrialBalanceTotalsDto();
            List<TrialBalanceDto> lstTrialBalance = GetAllTrialBalanceReport(objTotals);
            ViewBag.TrialBalanceTotals = objTotals;
            return View(lstTrialBalance);
        }
        public List<TrialBalanceDto> GetAllTrialBalanceReport(TrialBalanceTotalsDto objTotals = null)
        {
            if (objTotals == null) objTotals = new TrialBalanceTotalsDto();
            List<TrialBalanceDto> lstTrialBalance = new List<TrialBalanceDto>();
            SqlCommand cmd = new SqlCommand("uspTrialBalanceReport", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;
            
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                TrialBalanceDto obj = new TrialBalanceDto();
                obj.Ahcode = dr["AHCODE"] == DBNull.Value ? string.Empty : dr["AHCODE"].ToString();
                obj.Accounthaedname = dr["ACCOUNTHEADNAME"] == DBNull.Value ? string.Empty : Convert.ToString(dr["ACCOUNTHEADNAME"]);
                obj.Debit1 = dr["OPENING_BALANCE_DEBIT"] == DBNull.Value ? default(double) : Convert.ToDouble(dr["OPENING_BALANCE_DEBIT"]);
                obj.Credit1 = dr["OPENING_BALANCE_CREDIT"] == DBNull.Value ? default(double) : Convert.ToDouble(dr["OPENING_BALANCE_CREDIT"]);
                obj.Debit2 = dr["TRAN_DEBIT"] == DBNull.Value ? default(double) : Convert.ToDouble(dr["TRAN_DEBIT"]);
                obj.Credit2 = dr["TRAN_CREDIT"] == DBNull.Value ? default(double) : Convert.ToDouble(dr["TRAN_CREDIT"]);
                obj.Debit3 = dr["CLOSING_DEBIT"] == DBNull.Value ? default(double) : Convert.ToDouble(dr["CLOSING_DEBIT"]);
                obj.Credit3 = dr["CLOSING_CREDIT"] == DBNull.Value ? default(double) : Convert.ToDouble(dr["CLOSING_CREDIT"]);

                lstTrialBalance.Add(obj);
            }
            if (dr.NextResult())
            {
                if (dr.Read())
                {
                    objTotals.TotalRecords = dr["TOTAL_RECORDS"] == DBNull.Value ? default(int) : Convert.ToInt32(dr["TOTAL_RECORDS"]);
                    objTotals.OpeningBalDrSum = dr["OB_DR_Sum"] == DBNull.Value ? default(double) : Convert.ToDouble(dr["OB_DR_Sum"]);
                    objTotals.OpeningBalCrSum = dr["OB_CR_SUM"] == DBNull.Value ? default(double) : Convert.ToDouble(dr["OB_CR_SUM"]);
                    objTotals.TranBalDrSum = dr["TRAN_DR_SUM"] == DBNull.Value ? default(double) : Convert.ToDouble(dr["TRAN_DR_SUM"]);
                    objTotals.TranBalCrSum = dr["TRAN_CR_SUM"] == DBNull.Value ? default(double) : Convert.ToDouble(dr["TRAN_CR_SUM"]);
                    objTotals.ClosingBalDrSum = dr["CLOSING_DR_SUM"] == DBNull.Value ? default(double) : Convert.ToDouble(dr["CLOSING_DR_SUM"]);
                    objTotals.ClosingBalCrSum = dr["CLOSING_CR_SUM"] == DBNull.Value ? default(double) : Convert.ToDouble(dr["CLOSING_CR_SUM"]);
                }
            }
            return lstTrialBalance;
        }
        [HttpGet]
        public ActionResult ExportsReports()
        {
            if (GroupInfo.GroupID > 0)
                return RedirectToAction("ExportsReports_Group");
            List<TrialBalanceDto> lstTrialBalance = GetAllTrialBalanceReport();

            ReportDocument rd = new ReportDocument();
            rd.Load(Server.MapPath("~/Reports/FinancialRports/TrialBalance.rpt"));
            rd.SetDataSource(lstTrialBalance);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", "TrialBalance.pdf");

        }
        #endregion Federation Trail Balance Report

    }
}
