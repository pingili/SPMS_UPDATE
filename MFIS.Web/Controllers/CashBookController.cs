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
using CoreComponents;
namespace MFIS.Web.Controllers
{
    public class CashBookController : BaseController
    {
        SqlConnection con = new SqlConnection(DataLogic.DBConstants.MFIS_CS);

        #region Group CashBook BEGIN

        public ActionResult CreateCashBook_Group()
        {
            string FromDate = Request.Form["FromDate"];
            string ToDate = Request.Form["ToDate"];
            CashBookReportDto CashbookReportDto = new CashBookReportDto();
            if (FromDate != null && ToDate != null)
            {
                CashbookReportDto = GetAllCashBookReceipts(GroupInfo.GroupID, FromDate, ToDate);
                ViewBag.FromDate = (FromDate.ConvertToDateTime()).ToDisplayDateFormat();
                ViewBag.ToDate = (ToDate.ConvertToDateTime()).ToDisplayDateFormat();
            }
            //List<CashBookReportDto> lstCashbook = GetAllCashBookReceipts(GroupInfo.GroupID);
            bool isGroup = GroupInfo != null && GroupInfo.GroupID > 1;
            ViewBag.isGroup = isGroup;
            ViewBag.GroupCode = GroupInfo.GroupCode;
            ViewBag.GroupName = GroupInfo.GroupName;
            ViewBag.Cluster = GroupInfo.Cluster;
            ViewBag.Village = GroupInfo.Village;
            ViewBag.MeetingDay = GroupInfo.MeetingDay;
           
            return View("CreateCashBook", CashbookReportDto);
        }
        //public ActionResult CreateCashBook_Group()
        //{
        //    List<CashBookReportDto> lstCashbook = GetAllCashBookReceipts(GroupInfo.GroupID);
        //    ViewBag.GroupCode = GroupInfo.GroupCode;
        //    ViewBag.GroupName = GroupInfo.GroupName;
        //    ViewBag.Cluster = GroupInfo.Cluster;
        //    ViewBag.Village = GroupInfo.Village;



        //    return View("CreateCashBook", new List<CashBookReportDto>());
        //}


        public ActionResult GetCashBook(FormCollection form)
        {
            string FromDate = form["FromDate"];
            string ToDate = form["ToDate"];

            CashBookReportDto cashbookReportDto = GetAllCashBookReceipts(GroupInfo.GroupID, FromDate, ToDate);


            return View("CreateCashBook", cashbookReportDto);
        }

        //public ActionResult ExportsReports_Group()
        //{
        //    //List<CashBookReportDto> lstCashbook = GetAllCashBookReceipts(GroupInfo.GroupID);

        //    ReportDocument rd = new ReportDocument();
        //    rd.Load(Server.MapPath("~/RegistersReports/CashBook.rpt"));
        //    rd.SetDataSource(lstCashbook);
        //    Response.Buffer = false;
        //    Response.ClearContent();
        //    Response.ClearHeaders();
        //    try
        //    {
        //        Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
        //        stream.Seek(0, SeekOrigin.Begin);
        //        return File(stream, "application/pdf", "CashbookReceipt.pdf");
        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }

        //}

        #endregion Group CashBook END


        #region Federation CashBook BEGIN

        //[HttpPost]
        //public ActionResult CreateCashBook_Group()
        //{
        //    string FromDate = Request.Form["FromDate"];
        //    string ToDate = Request.Form["ToDate"];
        //    List<CashBookReportDto> lstCashbook = GetAllCashBookReceipts(GroupInfo.GroupID, FromDate, ToDate);
        //    return View("CreateCashBook", lstCashbook);
        //}

        //public ActionResult ExportsReports()
        //{
        //    List<CashBookReportDto> lstCashbook = GetAllCashBookReceipts(0);

        //    ReportDocument rd = new ReportDocument();
        //    rd.Load(Server.MapPath("~/RegistersReports/CashBook.rpt"));
        //    rd.SetDataSource(lstCashbook);
        //    Response.Buffer = false;
        //    Response.ClearContent();
        //    Response.ClearHeaders();
        //    try
        //    {
        //        Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
        //        stream.Seek(0, SeekOrigin.Begin);
        //        return File(stream, "application/pdf", "CashbookReceipt.pdf");
        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }

        //}

        #endregion Federation CashBook END

        #region Common
        private CashBookReportDto GetAllCashBookReceipts(int? groupID, string fromdate, string todate)
        {
            CashBookReportDto cashBookReportDto = new CashBookReportDto();
            List<MemberReceiptDto> memberReceipts = new List<MemberReceiptDto>();
            List<AccountVoucherDto> accountVouchers = new List<AccountVoucherDto>();
            OpeningBalanceDto objOpeningBalanceDto = new OpeningBalanceDto();


            SqlCommand cmd = new SqlCommand("uspCashbookReport", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@GroupID", groupID);
            cmd.Parameters.AddWithValue("@FromDate", fromdate);
            cmd.Parameters.AddWithValue("@ToDate", todate);
            cmd.CommandTimeout = 0;
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                MemberReceiptDto obj = new MemberReceiptDto();
                obj.MemberName = Convert.ToString(dr["MemberName"]);
                obj.ReceiptNo = Convert.ToString(dr["VoucherNumber"]);
                if (dr["Total"] != DBNull.Value)
                    obj.TotalReceipt = Convert.ToInt32(dr["Total"]);
                obj.Particulers = Convert.ToString(dr["Particulars"]);
                if (dr["Date"] != DBNull.Value)
                    obj.ReceiptDate = Convert.ToDateTime(dr["Date"]);
                if (dr["BIGLOAN_PRINCIPAL"] != DBNull.Value)
                    obj.BigLoanPrin = Convert.ToDouble(dr["BIGLOAN_PRINCIPAL"]);
                if (dr["BIGLOAN_INTEREST"] != DBNull.Value)
                    obj.BigLoanInt = Convert.ToDouble(dr["BIGLOAN_INTEREST"]);
                if (dr["SMALLLOAN_PRINCIPAL"] != DBNull.Value)
                    obj.SmallLoanPrin = Convert.ToDouble(dr["SMALLLOAN_PRINCIPAL"]);
                if (dr["SMALLLOAN_INTEREST"] != DBNull.Value)
                    obj.SmallLoanInt = Convert.ToDouble(dr["SMALLLOAN_INTEREST"]);
                if (dr["HOUSINGLOAN_INTEREST"] != DBNull.Value)
                    obj.HousigLoanPrin = Convert.ToDouble(dr["HOUSINGLOAN_INTEREST"]);
                if (dr["HOUSINGLOAN_PRINCIPAL"] != DBNull.Value)
                    obj.HousingLoanInt = Convert.ToDouble(dr["HOUSINGLOAN_PRINCIPAL"]);
                if (dr["PRIMARY_SAVINGS"] != DBNull.Value)
                    obj.RegularSavings = Convert.ToDouble(dr["PRIMARY_SAVINGS"]);
                if (dr["SPECIAL_SAVINGS"] != DBNull.Value)
                    obj.SpecialSavings = Convert.ToDouble(dr["SPECIAL_SAVINGS"]);
                if (dr["OTHERS"] != DBNull.Value)
                    obj.Others = Convert.ToDouble(dr["OTHERS"]);
                memberReceipts.Add(obj);
            }

            if (dr.NextResult())
            {
                while (dr.Read())
                {
                    AccountVoucherDto objparticulers = new AccountVoucherDto();
                    objparticulers.Particulars = Convert.ToString(dr["Particulars"]);
                    objparticulers.AccountMasterId = Convert.ToInt32(dr["AccountMasterID"]);
                    if (dr["Date"] != DBNull.Value)
                        objparticulers.ReceiptDate = Convert.ToDateTime(dr["Date"]);
                    if (dr["CrAmount"] != DBNull.Value)
                        objparticulers.CrAmount = Convert.ToDouble(dr["CrAmount"]);
                    if (dr["DrAmount"] != DBNull.Value)
                        objparticulers.DrAmount = Convert.ToDouble(dr["DrAmount"]);
                    if (dr["Amount"] != DBNull.Value)
                        objparticulers.Amount = Convert.ToDouble(dr["Amount"]);
                    objparticulers.ReceiptNo = Convert.ToString(dr["VoucherNumber"]);
                    accountVouchers.Add(objparticulers);
                }

                if (dr.NextResult())
                {
                    while (dr.Read())
                    {
                        if (dr["OpeningBalance"] != DBNull.Value)
                            objOpeningBalanceDto.OpeningBalance = Convert.ToDouble(dr["OpeningBalance"]);
                        if (dr["ClosingBalance"] != DBNull.Value)
                            objOpeningBalanceDto.ClosingBalance = Convert.ToDouble(dr["ClosingBalance"]);
                        if (dr["TrasactionAmount"] != DBNull.Value)
                            objOpeningBalanceDto.TransactionAmount = Convert.ToDouble(dr["TrasactionAmount"]);
                    }
                }
            }


            cashBookReportDto.accountVouchers = accountVouchers;
            cashBookReportDto.memberReceipts = memberReceipts;
            cashBookReportDto.openingBalaceDetails = objOpeningBalanceDto;

            return cashBookReportDto;
        }
        #endregion Common

    }
}
