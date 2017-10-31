using BusinessEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MFIS.Web.Models
{
    public class BalanceSheetModel
    {
        public string OrgName { get; set; }
        public string OrgAddress { get; set; }
        public string Asset_AHCode { get; set; }
        public string Asset_AHName { get; set; }
        public double Asset_DrAmount { get; set; }
        public double Asset_CrAmount { get; set; }
        public double Asset_Sum { get; set; }

        public string Liability_AHCode { get; set; }
        public string Liability_AHName { get; set; }
        public double Liability_DrAmount { get; set; }
        public double Liability_CrAmount { get; set; }
        public double Liability_Sum { get; set; }

    }
}