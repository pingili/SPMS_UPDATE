using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class ModuleDto
    {
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public string ModuleType { get; set; }
        public string Url { get; set; }
        public string ModuleCode { get; set; }
        public bool IsFederation { get; set; }
        public int DisplayOrder { get; set; }
        public string ControlId { get; set; }
        public int ParentID { get; set; }
        public bool IsSeed { get; set; }
    }
}
