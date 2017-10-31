using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class CashBookReportDto
    {
        public List<AccountVoucherDto> accountVouchers = new List<AccountVoucherDto>();
        public List<MemberReceiptDto> memberReceipts = new List<MemberReceiptDto>();
        public OpeningBalanceDto openingBalaceDetails = new OpeningBalanceDto();
    }
    public class MemberReceiptDto
    {
        public int MemberID { get; set; }
        public string MemberName { get; set; }
        public string Particulers { get; set; }
        public string ReceiptNo { get; set; }
        public DateTime ReceiptDate { get; set; }

        public double TotalReceipt { get; set; }
        public double BigLoanPrin { get; set; }
        public double BigLoanInt { get; set; }
        public double SmallLoanPrin { get; set; }
        public double SmallLoanInt { get; set; }
        public double HousigLoanPrin { get; set; }
        public double HousingLoanInt { get; set; }
        public double RegularSavings { get; set; }
        public double SpecialSavings { get; set; }
        public double Others { get; set; }
    }
    public class AccountVoucherDto
    {
        public string ReceiptNo { get; set; }
        public DateTime ReceiptDate { get; set; }
        public int AccountMasterId { get; set; }
        public double CrAmount { get; set; }
        public double DrAmount { get; set; }
        public double Amount { get; set; }
        public string Particulars { get; set; }
    }
    public class OpeningBalanceDto
    {
        public double OpeningBalance { get; set; }
        public double ClosingBalance { get; set; }
        public double TransactionAmount { get; set; }
    }
}
