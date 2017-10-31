using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MFIS.Web.Areas.Federation.Models
{
    public class BankMasterModel
    {
        public int BankEntryID { get; set; }
        public string BankCode { get; set; }
        
        [Required(ErrorMessage = "Bank Name is required")]
        public int BankName { get; set; }
        
        [Required(ErrorMessage = "Bank Branch Name is required")]
        [StringLength(160)]
        public string BranchName { get; set; }

        [Required(ErrorMessage = "IFSC Code is required")]
        [StringLength(160)]
        public string IFSC { get; set; }

        [Required(ErrorMessage = "Account number is required")]
        [StringLength(160)]
        public string AccountNumber { get; set; }

        [Required(ErrorMessage = "Account Type is required")]
        public int AccountType { get; set; }

        [Required(ErrorMessage = "Contact Number is required")]
        public string ContactNumber { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        public bool isMasterEntry { get; set; }
        public int StatusID { get; set; }
        public List<BankMasterModel> lstBankMasterModel { get; set; }
    }
}