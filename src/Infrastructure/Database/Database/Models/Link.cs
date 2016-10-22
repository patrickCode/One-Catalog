namespace Microsoft.Catalog.Database.Models
{
    public partial class Link: BaseModel
    {   
        public string Type { get; set; }
        public string Href { get; set; }
        public string Description { get; set; }
        public int ProjectId { get; set; }
    }
}