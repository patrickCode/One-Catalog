namespace Microsoft.Catalog.Database.Models
{
    public class ProjectSummary: BaseModel
    {
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Abstract { get; set; }
        public string AdditionalDetail { get; set; }
        public string Technologies { get; set; }
        public string Contacts { get; set; }
        public string CodeLink { get; set; }
        public string PreviewLink { get; set; }
    }
}