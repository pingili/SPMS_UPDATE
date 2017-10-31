using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class RoleDto
    {
       public int RoleId { get; set; }
       public string RoleName { get; set; }
       public string RoleCode { get; set; }
       public int Status { get; set; }
       public String StatusCode{ get; set; }
    }
}
