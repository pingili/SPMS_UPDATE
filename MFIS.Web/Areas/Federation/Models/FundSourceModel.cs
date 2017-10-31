using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
namespace MFIS.Web.Areas.Federation.Models
{
    public class FundSourceModel
    {
        public int FundSourceID { get; set; }
        [DisplayName("FundSource Code")]
        public string FundSourceCode { get; set; }
        [DisplayName("FundSource Name")]
        [Required(ErrorMessage = "FundSource Name is required")]
        public string FundSourceName { get; set; }
        public string TEFundSourseName { get; set; }
        public int StatusID { get; set; }
        public string StatusCode { get; set; }
        public string Status { get; set; }
    }
}