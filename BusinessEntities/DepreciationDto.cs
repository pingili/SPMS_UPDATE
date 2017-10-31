namespace BusinessEntities
{
    using System.Xml.Serialization;

    [XmlType("Depreciation")]
    public class DepreciationDto
    {
        public int DepreciationID { get; set; }

        [XmlIgnore]
        public string AHName { get; set; }

        public int AHID { get; set; }

        public byte Rate { get; set; }
    }
}