using BusinessEntities;
using BusinessLogic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MFIS.Web.Controllers
{
    public class RecieptsAndPaymentsController : BaseController
    {
        // GET: /RecieptsAndPayments/
        SqlConnection con = new SqlConnection(DataLogic.DBConstants.MFIS_CS);
        public ActionResult ReceiptsAndPayments()
        {
            OrganizationService objIOrganizationService = new OrganizationService();
            OrganizationDto organizationDto = objIOrganizationService.GetAll();
            ViewBag.OrganizationDetails = organizationDto;
            ReceiptsAndPaymentsDto reptsAndpmts = new ReceiptsAndPaymentsDto();
            ReceiptsAndPaymentsListDto lstreptsAndpmts = GetAllReceiptsAndPayments();
            return View(lstreptsAndpmts);
        }

        public ReceiptsAndPaymentsListDto GetAllReceiptsAndPayments()
        {
            List<ReceiptsAndPaymentsDto> lst = new List<ReceiptsAndPaymentsDto>();
            int GroupId = GroupInfo.GroupID;
            SqlCommand cmd = new SqlCommand("uspGroupReceiptAndPayments", con);
            cmd.Parameters.Add(new SqlParameter("GroupID", GroupId));
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            ReceiptsAndPaymentsListDto objdto = new ReceiptsAndPaymentsListDto();
            
            while (dr.Read())
            {
                ReceiptsAndPaymentsDto obj = new ReceiptsAndPaymentsDto();
                obj.Ahid = dr["Ahid"] == DBNull.Value ? default(Int32) : Convert.ToInt32(dr["Ahid"]);
                obj.SLAccountHead = dr["SLAccountHead"] == DBNull.Value ? string.Empty : Convert.ToString(dr["SLAccountHead"]);
                obj.OpeningBalance = dr["OpeningBalance"] == DBNull.Value ? default(double) : Convert.ToDouble(dr["OpeningBalance"]);
                obj.April = dr["Apr"] == DBNull.Value ? default(double) : Convert.ToDouble(dr["Apr"]);
                obj.May = dr["May"] == DBNull.Value ? default(double) : Convert.ToDouble(dr["May"]);
                obj.June = dr["Jun"] == DBNull.Value ? default(double) : Convert.ToDouble(dr["Jun"]);
                obj.July = dr["Jul"] == DBNull.Value ? default(double) : Convert.ToDouble(dr["Jul"]);
                obj.August = dr["Aug"] == DBNull.Value ? default(double) : Convert.ToDouble(dr["Aug"]);
                obj.Sep = dr["Sep"] == DBNull.Value ? default(double) : Convert.ToDouble(dr["Sep"]);
                obj.Oct = dr["Oct"] == DBNull.Value ? default(double) : Convert.ToDouble(dr["Oct"]);
                obj.Nov = dr["Nov"] == DBNull.Value ? default(double) : Convert.ToDouble(dr["Nov"]);
                obj.Dec = dr["Dec"] == DBNull.Value ? default(double) : Convert.ToDouble(dr["Dec"]);
                obj.Jan = dr["Jan"] == DBNull.Value ? default(double) : Convert.ToDouble(dr["Jan"]);
                obj.Feb = dr["Feb"] == DBNull.Value ? default(double) : Convert.ToDouble(dr["Feb"]);
                obj.March = dr["Mar"] == DBNull.Value ? default(double) : Convert.ToDouble(dr["Mar"]);

                lst.Add(obj);
            }
            objdto.lstReceiptDto = lst;
            while (dr.NextResult())
            {
                while (dr.Read())
                {
                    ReceiptsAndPaymentsDto obj = new ReceiptsAndPaymentsDto();
                    obj.Ahid = dr["Ahid"] == DBNull.Value ? default(Int32) : Convert.ToInt32(dr["Ahid"]);
                    obj.SLAccountHead = dr["SLAccountHead"] == DBNull.Value ? string.Empty : Convert.ToString(dr["SLAccountHead"]);
                    obj.OpeningBalance = dr["OpeningBalance"] == DBNull.Value ? default(double) : Convert.ToDouble(dr["OpeningBalance"]);
                    obj.April = dr["Apr"] == DBNull.Value ? default(double) : Convert.ToDouble(dr["Apr"]);
                    obj.May = dr["May"] == DBNull.Value ? default(double) : Convert.ToDouble(dr["May"]);
                    obj.June = dr["Jun"] == DBNull.Value ? default(double) : Convert.ToDouble(dr["Jun"]);
                    obj.July = dr["Jul"] == DBNull.Value ? default(double) : Convert.ToDouble(dr["Jul"]);
                    obj.August = dr["Aug"] == DBNull.Value ? default(double) : Convert.ToDouble(dr["Aug"]);
                    obj.Sep = dr["Sep"] == DBNull.Value ? default(double) : Convert.ToDouble(dr["Sep"]);
                    obj.Oct = dr["Oct"] == DBNull.Value ? default(double) : Convert.ToDouble(dr["Oct"]);
                    obj.Nov = dr["Nov"] == DBNull.Value ? default(double) : Convert.ToDouble(dr["Nov"]);
                    obj.Dec = dr["Dec"] == DBNull.Value ? default(double) : Convert.ToDouble(dr["Dec"]);
                    obj.Jan = dr["Jan"] == DBNull.Value ? default(double) : Convert.ToDouble(dr["Jan"]);
                    obj.Feb = dr["Feb"] == DBNull.Value ? default(double) : Convert.ToDouble(dr["Feb"]);
                    obj.March = dr["Mar"] == DBNull.Value ? default(double) : Convert.ToDouble(dr["Mar"]);
                }
            }
            objdto.lstPaymentsDto = lst;
            return objdto;

        }


        public ActionResult IncomeAndExpenditure()
        {
            OrganizationService objIOrganizationService = new OrganizationService();
            OrganizationDto organizationDto = objIOrganizationService.GetAll();
            ViewBag.OrganizationDetails = organizationDto;
            ReceiptsAndPaymentsDto reptsAndpmts = new ReceiptsAndPaymentsDto();
            ReceiptsAndPaymentsListDto lstreptsAndpmts = GetAllIncomeAndExpenditure();
            return View(lstreptsAndpmts);
        }


        public ReceiptsAndPaymentsListDto GetAllIncomeAndExpenditure()
        {
            List<ReceiptsAndPaymentsDto> lst = new List<ReceiptsAndPaymentsDto>();
            int GroupId = GroupInfo.GroupID;
            SqlCommand cmd = new SqlCommand("uspGroupIncomeAndExpendature", con);
            cmd.Parameters.Add(new SqlParameter("GroupID", GroupId));
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            ReceiptsAndPaymentsListDto objdto = new ReceiptsAndPaymentsListDto();

            while (dr.Read())
            {
                ReceiptsAndPaymentsDto obj = new ReceiptsAndPaymentsDto();
                obj.Ahid = dr["Ahid"] == DBNull.Value ? default(Int32) : Convert.ToInt32(dr["Ahid"]);
                obj.SLAccountHead = dr["SLAccountHead"] == DBNull.Value ? string.Empty : Convert.ToString(dr["SLAccountHead"]);
                obj.OpeningBalance = dr["OpeningBalance"] == DBNull.Value ? default(double) : Convert.ToDouble(dr["OpeningBalance"]);
                obj.April = dr["Apr"] == DBNull.Value ? default(double) : Convert.ToDouble(dr["Apr"]);
                obj.May = dr["May"] == DBNull.Value ? default(double) : Convert.ToDouble(dr["May"]);
                obj.June = dr["Jun"] == DBNull.Value ? default(double) : Convert.ToDouble(dr["Jun"]);
                obj.July = dr["Jul"] == DBNull.Value ? default(double) : Convert.ToDouble(dr["Jul"]);
                obj.August = dr["Aug"] == DBNull.Value ? default(double) : Convert.ToDouble(dr["Aug"]);
                obj.Sep = dr["Sep"] == DBNull.Value ? default(double) : Convert.ToDouble(dr["Sep"]);
                obj.Oct = dr["Oct"] == DBNull.Value ? default(double) : Convert.ToDouble(dr["Oct"]);
                obj.Nov = dr["Nov"] == DBNull.Value ? default(double) : Convert.ToDouble(dr["Nov"]);
                obj.Dec = dr["Dec"] == DBNull.Value ? default(double) : Convert.ToDouble(dr["Dec"]);
                obj.Jan = dr["Jan"] == DBNull.Value ? default(double) : Convert.ToDouble(dr["Jan"]);
                obj.Feb = dr["Feb"] == DBNull.Value ? default(double) : Convert.ToDouble(dr["Feb"]);
                obj.March = dr["Mar"] == DBNull.Value ? default(double) : Convert.ToDouble(dr["Mar"]);

                lst.Add(obj);
            }
            objdto.lstReceiptDto = lst;
            while (dr.NextResult())
            {
                while (dr.Read())
                {
                    ReceiptsAndPaymentsDto obj = new ReceiptsAndPaymentsDto();
                    obj.Ahid = dr["Ahid"] == DBNull.Value ? default(Int32) : Convert.ToInt32(dr["Ahid"]);
                    obj.SLAccountHead = dr["SLAccountHead"] == DBNull.Value ? string.Empty : Convert.ToString(dr["SLAccountHead"]);
                    obj.OpeningBalance = dr["OpeningBalance"] == DBNull.Value ? default(double) : Convert.ToDouble(dr["OpeningBalance"]);
                    obj.April = dr["Apr"] == DBNull.Value ? default(double) : Convert.ToDouble(dr["Apr"]);
                    obj.May = dr["May"] == DBNull.Value ? default(double) : Convert.ToDouble(dr["May"]);
                    obj.June = dr["Jun"] == DBNull.Value ? default(double) : Convert.ToDouble(dr["Jun"]);
                    obj.July = dr["Jul"] == DBNull.Value ? default(double) : Convert.ToDouble(dr["Jul"]);
                    obj.August = dr["Aug"] == DBNull.Value ? default(double) : Convert.ToDouble(dr["Aug"]);
                    obj.Sep = dr["Sep"] == DBNull.Value ? default(double) : Convert.ToDouble(dr["Sep"]);
                    obj.Oct = dr["Oct"] == DBNull.Value ? default(double) : Convert.ToDouble(dr["Oct"]);
                    obj.Nov = dr["Nov"] == DBNull.Value ? default(double) : Convert.ToDouble(dr["Nov"]);
                    obj.Dec = dr["Dec"] == DBNull.Value ? default(double) : Convert.ToDouble(dr["Dec"]);
                    obj.Jan = dr["Jan"] == DBNull.Value ? default(double) : Convert.ToDouble(dr["Jan"]);
                    obj.Feb = dr["Feb"] == DBNull.Value ? default(double) : Convert.ToDouble(dr["Feb"]);
                    obj.March = dr["Mar"] == DBNull.Value ? default(double) : Convert.ToDouble(dr["Mar"]);
                }
            }
            objdto.lstPaymentsDto = lst;
            return objdto;

        }

    }
}
