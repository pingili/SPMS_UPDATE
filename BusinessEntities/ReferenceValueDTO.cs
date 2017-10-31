namespace BusinessEntities
{
    public class ReferenceValueDto
    {
        public int RefID { get; set; }
        public int RefMasterID { get; set; }
        public string RefCode { get; set; }
        public string RefValue { get; set; }
        public string Description { get; set; }
        public byte DisplayOrder { get; set; }
        public bool IsSeedValue { get; set; }
        public bool IsActive { get; set; }
        public int UserID { get; set; }
    }
}
