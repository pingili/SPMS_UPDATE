using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class AccountHeadViewBalanceDto
    {
        public decimal OpeningBalance { get; set; }
        public decimal CloseingBalance { get; set; }
        public decimal CurrentYearBalance { get; set; }
        public string OpeningBalanceType { get; set; }
    }
}
