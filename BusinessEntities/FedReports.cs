using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class DataEntryStatusDBDto
    {
        public int GroupId { get; set; }
        public string GroupCode { get; set; }
        public string GroupName { get; set; }

        public string ClusterCode { get; set; }
        public string ClusterName { get; set; }

        public string GroupOBStatus { get; set; }
        public int SavingsMemberCount { get; set; }

        public bool isConducted { get; set; }

        public int Apr { get; set; }
        public int May { get; set; }
        public int Jun { get; set; }
        public int Jul { get; set; }
        public int Aug { get; set; }
        public int Sep { get; set; }
        public int Oct { get; set; }
        public int Nov { get; set; }
        public int Dec { get; set; }
        public int Jan { get; set; }
        public int Feb { get; set; }
        public int Mar { get; set; }
    }

    public class DataEntryStatusReportDto
    {
        public int GroupId { get; set; }
        public string GroupCode { get; set; }
        public string GroupName { get; set; }

        public string ClusterCode { get; set; }
        public string ClusterName { get; set; }

        public string GroupOBStatus { get; set; }
        public int SavingsMemberCount { get; set; }
        
        public int AprC { get; set; }
        public int AprN { get; set; }
        
        public int MayC { get; set; }
        public int MayN { get; set; }
        
        public int JunC { get; set; }
        public int JunN { get; set; }
        
        public int JulC { get; set; }
        public int JulN { get; set; }
        
        public int AugC { get; set; }
        public int AugN { get; set; }
        
        public int SepC { get; set; }
        public int SepN { get; set; }
        
        public int OctC { get; set; }
        public int OctN { get; set; }
        
        public int NovC { get; set; }
        public int NovN { get; set; }
        
        public int DecC { get; set; }
        public int DecN { get; set; }
        
        public int JanC { get; set; }
        public int JanN { get; set; }
        
        public int FebC { get; set; }
        public int FebN { get; set; }
        
        public int MarC { get; set; }
        public int MarN { get; set; }
    }
}
