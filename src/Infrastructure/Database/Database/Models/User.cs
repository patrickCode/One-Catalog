namespace Microsoft.Catalog.Database.Models
{
    public class User: BaseModel
    {   
        public string Name { get; set; }
        public string Alias { get; set; }
        public string UserPrincipalName { get; set; }
    }
}