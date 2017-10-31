using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class FederationLoanApplicationDto
    {
        public int ClusterId { get; set; }
        public int GroupId { get; set; }
        public int TotalMembers { get; set; }

        public int GLAccountId { get; set; }
        public int SlAccountId { get; set; }

        public int GRGLAccountId { get; set; }
        public string GRGLAccount { get; set; }
        public int GRSlAccountId { get; set; }
        public string GRSLAccount { get; set; }

        public double LoanSanctionAmount { get; set; }
        public int NoOfInstallments { get; set; }
        public int GroupBankAccount { get; set; }

    }
}
