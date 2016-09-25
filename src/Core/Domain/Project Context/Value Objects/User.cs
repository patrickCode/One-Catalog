namespace Microsoft.Catalog.Domain.ProjectContext.ValueObjects
{
    public class User
    {
        public string Alias { get; set; }
        public string Name { get; set; }
        public User()
        {
            Alias = string.Empty;
            Name = string.Empty;
        }
        public User(string alias, string name)
        {
            Alias = alias;
            Name = name;
        }
    }
}