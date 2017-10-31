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
    public class GroupCashBookController : BaseController
    {
        SqlConnection con = new SqlConnection(DataLogic.DBConstants.MFIS_CS);
        [HttpGet]
        public ActionResult GroupCashBookLookup()
        {
            DataSet Ds=GetAllGroupCashBook();
            ViewBag.Ds = Ds;
            return View();
        }
        public DataSet GetAllGroupCashBook()
        {
            MemberLoanLedgerDto objmemberloanledgerdto = new MemberLoanLedgerDto();
            SqlDataAdapter da = new SqlDataAdapter("uspGroupCashBook", con);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }
        public List<GroupCashBookDto> GetGroupCashBook()
        {
            List<GroupCashBookDto> lstGroupCashBookDto = new List<GroupCashBookDto>();
            SqlCommand cmd = new SqlCommand("uspGroupCashBook", con);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            GroupCashBookDto groupcashbookDto = new GroupCashBookDto();
            groupcashbookDto.lstcashreceiptsdto = new List<cashreceiptsdto>();
            groupcashbookDto.lstcashvoucherdto = new List<cashvoucherdto>();
            if (dr.Read())
            {
                groupcashbookDto.OrganizationName = dr["OrganizationName"].ToString();
                groupcashbookDto.MemberCode = dr["MemberCode"].ToString();
                groupcashbookDto.GroupCode = dr["GroupCode"].ToString();
                groupcashbookDto.GroupName = dr["GroupName"].ToString();
                groupcashbookDto.Cluster = dr["Cluster"].ToString();
                groupcashbookDto.Village = dr["Village"].ToString();
                groupcashbookDto.StaffName = dr["StaffName"].ToString();
                groupcashbookDto.Neighbourhood = dr["Neighbourhood"].ToString();
                groupcashbookDto.MemberName = dr["MemberName"].ToString();
                groupcashbookDto.GroupLeader2 = dr["GroupLeader2"].ToString();
                groupcashbookDto.GroupLeader1 = dr["GroupLeader1"].ToString();
            }
            if (dr.NextResult())
            {
                cashreceiptsdto objTransreceipt = null;
                while (dr.Read())
                {
                    objTransreceipt = new cashreceiptsdto();
                    objTransreceipt.OpeiningBalance = Convert.ToDecimal(dr["OpeiningBalance"]);
                    objTransreceipt.AHCode = Convert.ToString(dr["AHCode"]);
                    objTransreceipt.ReceiptNumber = dr["ReceiptNumber"].ToString();
                    objTransreceipt.LoanNumber = Convert.ToDecimal(dr["LoanNumber"]);
                    objTransreceipt.LoanPrincipal = Convert.ToDecimal(dr["LoanPrincipal"]);
                    objTransreceipt.LoanInterest = Convert.ToDecimal(dr["LoanInterest"]);
                    objTransreceipt.ReceiptTotal = dr["ReceiptTotal"].ToString();
                    objTransreceipt.TotalReceipt = dr["TotalReceipt"].ToString();
                    groupcashbookDto.lstcashreceiptsdto.Add(objTransreceipt);
                }
            }
            if (dr.NextResult())
            {
                cashvoucherdto objTransvoucher = null;
               
                while (dr.Read())
                {
                    objTransvoucher = new cashvoucherdto();
                    objTransvoucher.AHCode = dr["AHCode"].ToString();
                    objTransvoucher.VoucherNumber = dr["VoucherNumber"].ToString();
                    objTransvoucher.LoanNumber = dr["LoanNumber"].ToString();
                    objTransvoucher.LoanPrincipal = Convert.ToDecimal(dr["LoanPrincipal"]);
                    objTransvoucher.Paymentstotal = dr["Paymentstotal"].ToString();
                    objTransvoucher.TotalPayment = dr["TotalPayment"].ToString();
                    objTransvoucher.ClosingBalance = dr["ClosingBalance"].ToString();
                    groupcashbookDto.lstcashvoucherdto.Add(objTransvoucher);
                    lstGroupCashBookDto.Add(groupcashbookDto);
                }
            }
            return lstGroupCashBookDto;
        }
        public ActionResult ExportsReports()
        {
            List<GroupCashBookDto> lstLedgers = GetGroupCashBook();
            ReportDocument rd = new ReportDocument();
            rd.Load(Server.MapPath("~/Reports/RegistersReports/GroupCashBook.rpt"));
            rd.SetDataSource(lstLedgers);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            try
            {
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", "GroupCashBook.pdf");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
