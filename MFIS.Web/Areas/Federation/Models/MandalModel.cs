using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MFIS.Web.Areas.Federation.Models
{
    public class MandalModel
    {

        public int MandalID { get; set; }

        public string MandalCode { get; set; }

        [DisplayName("Mandal Name")]
        public string Mandal { get; set; }

        [DisplayName("Mandal Name(Telugu)")]
        public string TEMandalName { get; set; }

        [DisplayName("Mondal Type")]
        public char MandalType { get; set; }

        [DisplayName("District")]
        public int DistrictID { get; set; }

        public int StatusID { get; set; }

         [DisplayName("State")]
        public int StateID { get; set; }
    }
}