namespace Microsoft.Catalog.Database.Models
{
    public partial class Project: BaseModel
    {   
        public string Name { get; set; }
        public string Description { get; set; }
        public string Abstract { get; set; }
        public string AdditionalDetail { get; set; }
    }
}