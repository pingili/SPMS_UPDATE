using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MFIS.Web.Areas.Federation.Models
{
    public class NPAModel
    {
        public int NPAID { get; set; }

        public string OverDuePeriod { get; set; }

        public int OverDuePeriodID { get; set; }

        public int RetVal { get; set; } 
        public byte Rate { get; set; }
    }
}