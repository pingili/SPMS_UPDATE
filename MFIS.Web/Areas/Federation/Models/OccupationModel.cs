using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace MFIS.Web.Areas.Federation.Models
{
    public class OccupationModel
    {
        public int OccupationID { get; set; }
        [DisplayName("Code")]
        public string OccupationCode { get; set; }
        [DisplayName("Category")]
        [Required(ErrorMessage = "OccupationCategory is required")]
        public int OccupationCategory { get; set; }
        [DisplayName("Occupation Title")]
        [Required(ErrorMessage = "Occupation name is required")]
        public string Occupation { get; set; }
        public string StatusCode { get; set; }

    }
}