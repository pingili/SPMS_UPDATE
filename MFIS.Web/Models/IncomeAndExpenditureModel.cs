using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MFIS.Web.Models
{
    public class IncomeAndExpenditureModel
    {
        public string OrgName { get; set; }
        public string OrgAddress { get; set; }
        public string Income_AHCode { get; set; }
        public string Income_AHName { get; set; }
        public double Income_DrAmount { get; set; }
        public double Income_CrAmount { get; set; }
        public double Income_Sum { get; set; }

        public string Expenditure_AHCode { get; set; }
        public string Expenditure_AHName { get; set; }
        public double Expenditure_DrAmount { get; set; }
        public double Expenditure_CrAmount { get; set; }
        public double Expenditure_Sum { get; set; }
    }
}