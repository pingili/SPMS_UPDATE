using BusinessEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MFIS.Web.Models
{
    public class MemberLoanLedgerModel
    {
        public int MemberID { get; set; }
        public string OrganizationName { get; set; }
        public string MemberName { get; set; }
        public string GroupName { get; set; }
        public string GroupCode { get; set; }
        public string Panchayat { get; set; }
        public string Village { get; set; }
        public string Cluster { get; set; }
        public string ACNumber { get; set; }
        public int LoanDisbursed { get; set; }
        public decimal EMIPrincipal { get; set; }
        public int NoOfInstallments { get; set; }
        public decimal InstalmentFrequency { get; set; }
        public DateTime DueDate { get; set; }
        public decimal InterestRate { get; set; }
        public decimal PenalInterestRate { get; set; }
        public string FundSource { get; set; }
        public string Project { get; set; }
        public string LoanPurpose { get; set; }
        public DateTime Date { get; set; }
        public string VoucherType { get; set; }
        public string VoucherNumber { get; set; }
        public string ReceiptNumber { get; set; }
        public decimal Disbursement { get; set; }
        public decimal Repaid { get; set; }
        public decimal Due { get; set; }
        public decimal Paid { get; set; }
        public int TotalRepayment { get; set; }
        public string AHCode { get; set; }
        public string AHName { get; set; }
        public decimal Debit { get; set; }
        public decimal DemandPrincipal { get; set; }
        public decimal Balance { get; set; }
        public string BalanceType { get; set; }
        public int Noofdays { get; set; }
        public decimal PrepaymentPrincipal { get; set; }
        public decimal OverduePrincipal { get; set; }
        public decimal TotalOutstandingwithinterest { get; set; }
      
    }
}