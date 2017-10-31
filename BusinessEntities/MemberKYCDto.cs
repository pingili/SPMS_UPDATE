using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
   public class MemberKYCDto
    {
       public int MemberID { get; set; }
        public int MemberKYCID { get; set; }
        public int KYCType { get; set; }
        public string FileName { get; set; }
        public string ActualFileName { get; set; }
        public int StatusID { get; set; }
        public int UserID { get; set; }
        public string KYCNumber { get; set; }


      
    }
}
