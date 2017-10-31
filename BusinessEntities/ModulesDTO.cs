using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
   public class ModulesDTO
    {
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public string ModuleType { get; set; }
        public string Url { get; set; }
        public int Status { get; set; }
    }
}
