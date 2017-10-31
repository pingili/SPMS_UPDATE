using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class MandalDto
    {
        public int MandalID { get; set; }
        public string MandalCode { get; set; }
        public string Mandal { get; set; }
        public string TEMandalName { get; set; }
        public string MandalType { get; set; }
        public int DistrictID { get; set; }
        public int StateID { get; set; }
        public int UserID { get; set; }
    }

    public class MandalLookupDto
    {
        public int MandalID { get; set; }
        public string MandalCode { get; set; }
        public string MandalType { get; set; }
        public string Mandal { get; set; }
        public string District { get; set; }
        public string State { get; set; }
        public string StatusCode { get; set; }
        public string Status { get; set; }
    }
}
