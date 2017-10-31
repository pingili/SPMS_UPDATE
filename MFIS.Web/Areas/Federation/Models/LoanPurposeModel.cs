using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MFIS.Web.Areas.Federation.Models
{
    public class LoanPurposeModel
    {
        public int LoanPurposeID { get; set; }
        
        [Required]
        [DisplayName("Project")]
        public int Project { get; set; }

        public int Category { get; set; }

        public string LoanPurposeCode { get; set; }

        [Required]
        [DisplayName("Purpose")]
        public string Purpose { get; set; }

        public string TELoanPurpose { get; set; }
    }
}