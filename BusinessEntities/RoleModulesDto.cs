using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
   public class RoleModulesDto
    {
        public int RoleModuleId { get; set; }
        public string RoleModuleCode { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public int ModuleActionId { get; set; }
        public String ModuleActionName { get; set; }
        public int Status { get; set; }

        public string MainModule { get; set; }
        public string SubModules { get; set; }
        public string StatusCode { get; set; }
    }
      
}
