using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class StateDto
    {
        public int StateID { get; set; }
        public string StateCode { get; set; }
        public string State { get; set; }
        public string TEStateName { get; set; }
        public bool IsActive { get; set; }
        public int UserID { get; set; }
    }
}
