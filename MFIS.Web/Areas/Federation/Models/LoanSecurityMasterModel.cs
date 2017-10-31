using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MFIS.Web.Areas.Federation.Models
{
    public class LoanSecurityMasterModel
    {
        public int LoanSecurityID { get; set; }

        [Required]
        [DisplayName("Type")]
        public int Type { get; set; }

        public string LoanSecurityCode { get; set; }

        [Required]
        [DisplayName("Name")]
        public string LoanSecurityName { get; set; }
    }
}