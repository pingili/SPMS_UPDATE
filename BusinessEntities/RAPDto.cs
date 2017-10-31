using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BusinessEntities
{
    [XmlType("RAP")]
    public class RAPDto
    {
        public int AHID { get; set; }

        public string AHName { get; set; }

        public int Priority { get; set; }

        //public string AHCode { get; set; }

        //public string Category { get; set; }
        [XmlIgnore]
        public string AHCode { get; set; }

        [XmlIgnore]
        public string Category { get; set; }

    }
}
