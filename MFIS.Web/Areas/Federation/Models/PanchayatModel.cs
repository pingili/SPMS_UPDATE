using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MFIS.Web.Areas.Federation.Models
{
    public class PanchayatModel
    {
        public int PanchayatID { get; set; }

        [DisplayName("Panchayat Name")]
        public string PanchayatCode { get; set; }

        [Required(ErrorMessage = "Panchayat  is Required")]
        [RegularExpression(@"(\S)+", ErrorMessage = "White space is not allowed")]
        [DisplayName("Panchayat Name")]
        public string PanchayatName { get; set; }

        [DisplayName("Panchayat Name(Telugu)")]
        //[Compare("PanchayatName")]
        public string TEPanchayatName { get; set; }

        [DisplayName("Village/Town")]
        public int VillageID { get; set; }
        public int StatusID { get; set; }

        [DisplayName("Cluster")]
        public int ClusterID { get; set; }

        [DisplayName("Mandal")]
        public int MandalID { get; set; }

        [DisplayName("District")]
        public int DistrictID { get; set; }

        [DisplayName("State")]
        public int StateID { get; set; }
        public int UserId { get; set; }
    }
}