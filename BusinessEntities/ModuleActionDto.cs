using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
   public class ModuleActionDto
    {
        public int ModuleActionId { get; set; }
        public string ModuleActionCode { get; set; }
        public string ModuleName { get; set; }
        public int ModuleId { get; set; }
        public string ActionName { get; set; }
        public string Url { get; set; }
        public int Status { get; set; }
    }
    public class ModuleActionLookup
    {
        public int ModuleActionId { get; set; }
        public string ModuleActionCode { get; set; }
        public string ModuleName { get; set; }
        public string ActionName { get; set; }
        public string Url { get; set; }
        public int Status { get; set; }
        public string StatusCode { get; set; }
    }
}
