using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class ResultDto
    {
        public int ObjectId { get; set; }
        public string ObjectCode { get; set; }
        public string Message { get; set; }
        public bool Result { get; set; }
        public char Type { get; set; }
        public string CreatedOn { get; set; }

    }
}
