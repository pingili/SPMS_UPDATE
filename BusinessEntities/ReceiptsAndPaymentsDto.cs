using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class ReceiptsAndPaymentsDto
    {
        public int Ahid { get; set; }
        public string SLAccountHead { get; set; }
        public double OpeningBalance { get; set; }
        public double April { get; set; }
        public double May { get; set; }
        public double June { get; set; }
        public double July { get; set; }
        public double August { get; set; }
        public double Sep { get; set; }
        public double Oct { get; set; }
        public double Nov { get; set; }
        public double Dec { get; set; }
        public double Jan { get; set; }
        public double Feb { get; set; }
        public double March { get; set; }

    }

    public class ReceiptsAndPaymentsListDto
    {
        public List<ReceiptsAndPaymentsDto> lstReceiptDto { get; set; }
        public List<ReceiptsAndPaymentsDto> lstPaymentsDto { get; set; }
    }

}
