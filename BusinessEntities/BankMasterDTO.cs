using System.Collections.Generic;
namespace BusinessEntities
{
    public class BankMasterDto
    {
        public int BankEntryID { get; set; }
        public string BankCode { get; set; }
        public int BankName { get; set; }
        public string BName { get; set; }
        public string BranchName { get; set; }
        public string IFSC { get; set; }
        public string AccountNumber { get; set; }
        public int AccountType { get; set; }
        public string AccountTypeText { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public int AHID { get; set; }
        public string AHCode { get; set; }
        public string AHName { get; set; }
        public string Address { get; set; }
        public bool isMasterEntry { get; set; }
        public int UserID { get; set; }
        public int ObjectID { get; set; }
        public int EntityId { get; set; }
        public List<BankMasterViewDto> BankMasterView { get; set; }
    }
    public class BankMasterViewDto
    {
        public int BankEntryID { get; set; }
        public string BankCode { get; set; }
        public int AHID { get; set; }
        public string AccountNumber { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string IFSC { get; set; }
        public string ContactNumber { get; set; }
        public string AccountType { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }


    }
}
