using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
   public class StatusMasterDto
    {
        public int StatusID { get; set; }
        public string StatusCode { get; set; }
        public string Status { get; set; }
        public string StatusDescription { get; set; }
        public bool IsActive { get; set; }

        public int UserID { get; set; }
    }
}
