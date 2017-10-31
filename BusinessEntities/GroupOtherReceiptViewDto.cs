using System;
using CoreComponents;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class GroupOtherReceiptViewDto
    {
        public string VoucherNumber { get; set; }
        public string TransactionMode { get; set; }

        public DateTime TransactionDate { get; set; }
        private string _voucherRefNumber { get; set; }
        public string VoucherRefNumber
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_voucherRefNumber))
                    _voucherRefNumber = "N/A";
                return _voucherRefNumber;
            }
            set
            {
                _voucherRefNumber = value;
            }
        }
        public string CollectionAgent { get; set; }
        public string GLAccountName { get; set; }
        public string SLAccountName { get; set; }

        private string _chequeNumber { get; set; }
        public string ChequeNumber
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_chequeNumber))
                    _chequeNumber = "N/A";
                return _chequeNumber;
            }
            set
            {
                _chequeNumber = value;
            }
        }
        public DateTime ChequeDate { get; set; }
        public string StrChequeDate
        {
            get
            {
                string str = ChequeDate.ToDisplayDateFormat();
                if (string.IsNullOrWhiteSpace(str))
                    str = "N/A";
                return str;
            }
        }
        private string _narration { get; set; }
        public string Narration
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_narration))
                    _narration = "N/A";
                return _narration;
            }
            set
            {
                _narration = value;
            }
        }
        public string BankName { get; set; }
        public decimal Amount { get; set; }
    }
}
