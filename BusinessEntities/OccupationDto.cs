using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class OccupationDto
    {
        public int OccupationID { get; set; }
        public string OccupationCode { get; set; }
        public int OccupationCategory { get; set; }
        public string Occupation { get; set; }
        public int UserId { get; set; }
    }
}
