using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
  public  class MemberLookupDto
    {
        public int MemberID { get; set; }
        public string MemberCode { get; set; }
        public string MemberRefCode { get; set; }
        public string MemberName { get; set; }
        public string GroupName { get; set; }
        public string Gender { get; set; }
        public string MandalType { get; set; }
        public System.DateTime DateOfAdmission { get; set; }
        public System.DateTime DOB { get; set; }
        public string Occupation { get; set; }
        public string ParentGuardianName { get; set; }
        public string Status { get; set; }
        public string StatusCode { get; set; }

        public string LeaderShipLevel { get; set; }
        public string Desigination { get; set; }
        public DateTime FromDate { get; set; }
        public string ToDate { get; set; }
    }
}
