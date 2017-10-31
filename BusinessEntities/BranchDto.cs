namespace BusinessEntities
{
    using System;

    public class BranchLookupDto
    {
        public int BranchID { get; set; }
        public string BranchName { get; set; }
        public string BranchCode { get; set; }
        public string ManagerName { get; set; }
        public DateTime StartDate { get; set; }
        public string StatusCode { get; set; }
        public string Status { get; set; }
    }

    public class BranchDto
    {
        public int BranchID { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string TEBranchName { get; set; }
        public DateTime StartDate { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int AccountantID { get; set; }
        public DateTime AccountantFromDate { get; set; }
        public int ManagerID { get; set; }
        public DateTime ManagerFromDate { get; set; }
        public int UserID { set; get; }
    }

    public class BranchViewDto
    {
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string TEBranchName { get; set; }
        public DateTime StartDate { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string AccountantName { get; set; }
        public DateTime AccountantFromDate { get; set; }
        public string ManagerName { get; set; }
        public DateTime ManagerFromDate { get; set; }
    }
}
