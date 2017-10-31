using System.ComponentModel.DataAnnotations;

namespace MFIS.Web.Areas.Federation.Models
{
    public class ProjectModel
    {
        public int ProjectID { get; set; }

        public int FundSourceID { get; set; }

        public string ProjectCode { get; set; }

        [Required(ErrorMessage = "Please Enter Project Name.")]
        public string ProjectName { get; set; }

        public int Purpose { get; set; }

        public int Type { get; set; }
    }
}