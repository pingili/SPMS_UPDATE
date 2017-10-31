using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace MFIS.Web.Areas.Group.Models
{
    public class MemberReceiptModel
    {
        public class ReceiptMasterDto
        {
            public int AccountMasterID { get; set; }
            public int AHID { get; set; }
            public int CodeSno { get; set; }
            public DateTime TransactionDate { get; set; }
            public string VoucherNumber { get; set; }
            public string ReceiptNumber { get; set; }
            public string VoucherRefNumber { get; set; }
            public string PartyName { get; set; }
            public int EmployeeID { get; set; }
            public string ReferenceNumber { get; set; }

            public string EmployeeCode { get; set; }
            public string EmployeeName { get; set; }

            public int SubHeadID { get; set; }
            public int TransactionType { get; set; }
            public decimal Amount { get; set; }
            public string TransactionMode { get; set; }
            public string ChequeNumber { get; set; }
            public DateTime ChequeDate { get; set; }
            public int BankAccount { get; set; }
            public string Narration { get; set; }
            public int StatusID { get; set; }
            public bool IsGroup { get; set; }
            public int GroupID { get; set; }
            public string GroupCode { get; set; }
            public string GroupName { get; set; }
            public bool IsActive { get; set; }
            public string AccountHead { get; set; }
            // public decimal DrAmount { get; set; }


            //Added Non Existing Columns in the database
            public string GroupBankAccountNumber { get; set; }
            public string GroupBankAccountName { get; set; }
            public string FederationBankAccountNumber { get; set; }
            public string FederationBankAccountName { get; set; }
            public int VillageID { get; set; }
            public int ClusterID { get; set; }

            public List<ReceiptTranscationDto> lstGroupReceiptTranscationDto { get; set; }


            //Account Head Table Properties
            public string TE_AHName { get; set; }
            public int AHType { get; set; }
            public bool IsMemberTransaction { get; set; }
            public int AHLevel { get; set; }
            public bool IsFederation { get; set; }
            public int UserID { get; set; }
            public string AHIDS { get; set; }
            public decimal CurrentYearBalance { get; set; }

        }
        public class ReceiptTranscationDto
        {
            public int AHID { get; set; }
            public string AHCode { get; set; }
            public string AHName { get; set; }
            [XmlIgnore]
            public bool ISLAccount { get; set; }
            public string SLAcNo { get; set; }
            public decimal OpeningBalance { get; set; }
            public decimal ClosingBalance { get; set; }
            public int AccountTranID { get; set; }
            public int ParentAHID { get; set; }
            public decimal CrAmount { get; set; }
            public decimal DrAmount { get; set; }

        }
    }
}