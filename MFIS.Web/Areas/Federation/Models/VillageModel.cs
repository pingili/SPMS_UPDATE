using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MFIS.Web.Areas.Federation.Models
{
    public class VillageModel
    {
        public int VillageID { get; set; }

        public string VillageCode { get; set; }

        [Required]
        [DisplayName("Village/Town Name")]
        public string Village { get; set; }

        [DisplayName("Village Name(Telugu)")]
        public string TEVillageName { get; set; }

        [DisplayName("Cluster")]
        public int ClusterID { get; set; }

        public int StatusID { get; set; }

        public string Status { get; set; }

        public string StatusCode { get; set; }

        [Required]
        [DisplayName("Mandal")]
        public int MandalID { get; set; }

        [Required]
        [DisplayName("State")]
        public int StateID { get; set; }

        [Required]
        [DisplayName("District")]
        public int DistrictID { get; set; }

        public string Cluster { get; set; }
        public string Mandal { get; set; }
        public string District { get; set; }
    }
}