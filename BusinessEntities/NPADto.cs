namespace BusinessEntities
{
    using System.Xml.Serialization;

    [XmlType("NPA")]
    public class NPADto
    {
        public int NPAID { get; set; }
        
        [XmlIgnore]
        public string OverDuePeriod { get; set; }
        
        public int OverDuePeriodID { get; set; }

        public byte Rate { get; set; }
    }
}
