namespace MFIS.Web.Areas.Federation.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class BranchModel
    {
        public int BranchID { get; set; }
        
        public string BranchCode { get; set; }
        
        public string BranchName { get; set; }
        
        public string TEBranchName { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        
        public string Phone { get; set; }
        
        public string Email { get; set; }
        
        public string Address { get; set; }
        
        public int AccountantID { get; set; }

        [DataType(DataType.Date)]
        public DateTime AccountantFromDate { get; set; }
        
        public int ManagerID { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime ManagerFromDate { get; set; }
    }
}