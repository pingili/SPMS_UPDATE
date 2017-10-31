using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MFIS.Web.Areas.Federation.Models
{
    public class DistrictModel
    {
        public int DistrictID { get; set; }

        [DisplayName("District Code")]
        public string DistrictCode { get; set; }

        [Required(ErrorMessage = "District name is required")]
        [DisplayName("District Name")]
        public string District { get; set; }

        [DisplayName("District Name(Telugu)")]
        public string TEDistrictName { get; set; }

        [DisplayName("State")]
        [Required(ErrorMessage = "State name is required")]
        public int StateID { get; set; }
    }
}