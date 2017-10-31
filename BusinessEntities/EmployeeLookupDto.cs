using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class EmployeeLookupDto
    {

        public int EmployeeID { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeRefCode { get; set; }
        public string ParentGuardianName { get; set; }
        public string Gender { get; set; }
        public string Desigination { get; set; }
        public DateTime DOJ { get; set; }
        public DateTime DOB { get; set; }
        public string StatusCode { get; set; }
        public string Status { get; set; }
    }
}
