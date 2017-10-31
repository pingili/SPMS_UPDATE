using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class EmployeeKYCDto
    {
        public int EmployeeKYCID { get; set; }
        public int EmployeeID { get; set; }
        public int KYCType { get; set; }
        public string FileName { get; set; }
        public string ActualFileName { get; set; }
        public int StatusID { get; set; }
        public int UserID { get; set; }
        public string KYCNumber { get; set; }
    }
}
