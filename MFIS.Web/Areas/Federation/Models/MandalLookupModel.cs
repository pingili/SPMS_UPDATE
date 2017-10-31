using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MFIS.Web.Areas.Federation.Models
{
    public class MandalLookupModel
    {
        public int MandalID { get; set; }
        public string MandalCode { get; set; }
        public string MandalName { get; set; }
        public string DistrictName { get; set; }
        public string State { get; set; }
        public string StatusCode { get; set; }
        public string Status { get; set; }
    }
}