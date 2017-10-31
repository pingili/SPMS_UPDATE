using MFIS.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CoreComponents;
using BusinessEntities;
using System.Data;

namespace MFIS.Web.Controllers
{
    public class OpeningBalanceController : BaseController
    {
        //
        // GET: /OpeningBalance/
        SqlConnection con = new SqlConnection(DataLogic.DBConstants.MFIS_CS);
        public ActionResult GetOpeningBalance()
        {

            OpeningBalanceModel lstOpeningBalance = GetGroupOpeningBalance();
            ViewBag.GroupName = GroupInfo.GroupName;
            bool isGroup = GroupInfo != null && GroupInfo.GroupID > 1;
            ViewBag.isGroup = isGroup;
            return View(lstOpeningBalance);
        }
        public OpeningBalanceModel GetGroupOpeningBalance()
        {
            List<OpeningBalanceModelAssets> lstassets = new List<OpeningBalanceModelAssets>();
            List<OpeningBalanceModelLiabilities> lstliabilities = new List<OpeningBalanceModelLiabilities>();
            OpeningBalanceModel OpeningBalances = new OpeningBalanceModel();
            SqlCommand cmd = new SqlCommand("[usp_Group_OpeningBalance_Report]", con);
            cmd.Parameters.Add(new SqlParameter("@GroupId", GroupInfo.GroupID));
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;
            con.Open();
            OpeningBalances = new OpeningBalanceModel();
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                OpeningBalanceModelLiabilities liabilities = new OpeningBalanceModelLiabilities();
                liabilities.LiabilitiesAHID = Convert.ToInt32(dr["ahid"]);
                liabilities.LiabilitiesAHCode = Convert.ToString(dr["AHCode"]);
                liabilities.LiabilitiesAHName = Convert.ToString(dr["AHName"]);
                liabilities.LiabilitiesOpeningBalance = Convert.ToDouble(dr["OpeningBalance"]);
                OpeningBalances.LiabilitiesTotal += liabilities.LiabilitiesOpeningBalance;
                lstliabilities.Add(liabilities);
            }
            if (dr.NextResult())
            {
                while (dr.Read())
                {
                    OpeningBalanceModelAssets assets = new OpeningBalanceModelAssets();
                    assets.AssetAHID = Convert.ToInt32(dr["ahid"]);
                    assets.AssetAHCode = Convert.ToString(dr["AHCode"]);
                    assets.AssetAHName = Convert.ToString(dr["AHName"]);
                    assets.AssetOpeningBalance = Convert.ToDouble(dr["OpeningBalance"]);
                    OpeningBalances.AssetTotal += assets.AssetOpeningBalance;
                    lstassets.Add(assets);
                }

            }
            OpeningBalances.openingBalanceModelAssets = lstassets;
            OpeningBalances.openingBalanceModelLiabilities = lstliabilities;
            return OpeningBalances;

        }

    }
}
